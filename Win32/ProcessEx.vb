Friend Class ProcessEx
    Inherits WaitHandle

#Region "Shared members"
    Public Shared Function Attach(ByVal ProcessID As Int32) As ProcessEx
        Dim hProcess As IntPtr
        hProcess = OpenProcess(ProcessAccess.PROCESS_ALL_ACCESS, False, ProcessID)
        Return New ProcessEx(hProcess)
    End Function

    Public Shared Function Create(ByVal ApplicationName As String, ByVal CommandLine As String, _
        ByVal Environment As IEnumerable(Of String), ByVal CurrentDirectory As String, ByVal Desktop As String) As ProcessEx
        Using Suspended As Suspended = CreateSuspended(ApplicationName, CommandLine, Environment, CurrentDirectory, Desktop)
            Return Suspended.Resume()
        End Using
    End Function

    Public Shared Function CreateSuspended(ByVal ApplicationName As String, ByVal CommandLine As String, _
        ByVal Environment As IEnumerable(Of String), ByVal CurrentDirectory As String, ByVal Desktop As String) As Suspended

        Dim StartupInfo As STARTUPINFO

        StartupInfo.cb = Marshal.SizeOf(GetType(STARTUPINFO))
        StartupInfo.Desktop = Desktop
        StartupInfo.Title = Nothing
        StartupInfo.dwFlags = StartupFlags.STARTF_FORCEOFFFEEDBACK Or StartupFlags.STARTF_USESTDHANDLES

        Dim EnvironmentPtr As IntPtr = IntPtr.Zero

        If Environment IsNot Nothing _
            Then EnvironmentPtr = AllocEnvironment(Environment)

        Dim ProcessInformation As New PROCESS_INFORMATION

        Try
            CreateProcess(ApplicationName, CommandLine, Nothing, Nothing, False,
                CreationFlags.CREATE_BREAKAWAY_FROM_JOB Or CreationFlags.CREATE_DEFAULT_ERROR_MODE Or CreationFlags.CREATE_NO_WINDOW Or CreationFlags.CREATE_SUSPENDED Or CreationFlags.CREATE_UNICODE_ENVIRONMENT,
                EnvironmentPtr, CurrentDirectory, StartupInfo, ProcessInformation)
        Finally
            If EnvironmentPtr <> IntPtr.Zero Then _
                Marshal.FreeHGlobal(EnvironmentPtr)
        End Try

        Return New Suspended(ProcessInformation.hProcess, ProcessInformation.hThread)
    End Function

    Private Shared Function AllocEnvironment(ByVal Environment As IEnumerable(Of String)) As IntPtr
        ' Calculate size
        Dim Size As Int32 = 1
        For Each s As String In Environment
            Size += s.Length + 1
        Next
        Size <<= 1

        ' Empty environment block must also be terminated by two null chars
        If Size = 2 Then _
            Size = 4

        ' Allocate memory and fill data
        Dim Result As IntPtr = Marshal.AllocHGlobal(Size)

        Try
            Dim Pointer As IntPtr = Result
            For Each s As String In Environment
                Marshal.Copy(s, 0, Pointer, s.Length)
                Pointer = Pointer.ToInt64() + (s.Length << 1)
                Marshal.WriteInt16(Pointer, 0)
                Pointer = Pointer.ToInt64() + 2
            Next
            Marshal.WriteInt16(Pointer, 0)

            If Pointer = Result Then
                Pointer = Pointer.ToInt64() + 2
                Marshal.WriteInt16(Pointer, 0)
            End If

            ' Return the pointer
            Return Result
        Catch ex As Exception
            Marshal.FreeHGlobal(Result)
            Throw
        End Try
    End Function
#End Region

    Public Sub New(ByVal OwnedHandle As IntPtr)
        MyBase.SafeWaitHandle = New SafeWaitHandle(OwnedHandle, True)
    End Sub

    Public Function GetHandleUnsafe() As IntPtr
        Return MyBase.SafeWaitHandle.DangerousGetHandle()
    End Function

    Public Sub Kill(ByVal ReturnCode As Int32)
        Win32True(TerminateProcess(GetHandleUnsafe(), ReturnCode))
    End Sub

    ' TODO: attach debugger
    Public Class Suspended
        Implements IDisposable

        Protected m_Resumed As Boolean
        Protected m_ProcessHandle As IntPtr
        Protected m_ThreadHandle As IntPtr

        Public Sub New(ByVal ProcessHandle As IntPtr, ByVal ThreadHandle As IntPtr)
            m_Resumed = False
            m_ProcessHandle = ProcessHandle
            m_ThreadHandle = ThreadHandle
        End Sub

        Public Function SetToken(ByVal TokenHandle As IntPtr) As Suspended
            If m_Resumed Then
                Throw New Exception("The suspended process has already been resumed.")
            End If

            Dim AccessToken As PROCESS_ACCESS_TOKEN

            AccessToken.Token = TokenHandle
            AccessToken.Thread = IntPtr.Zero

            NtSuccess(NtSetInformationProcess(m_ProcessHandle, PROCESSINFOCLASS.ProcessAccessToken, AccessToken, Marshal.SizeOf(AccessToken)))
            Return Me
        End Function

        Public Function SetStdHandles(ByVal StdInputHandle As IntPtr, ByVal StdOutputHandle As IntPtr, ByVal StdErrorHandle As IntPtr) As Suspended
            If m_Resumed Then
                Throw New Exception("The suspended process has already been resumed.")
            End If

            Dim TargetStdInputHandle As IntPtr
            Dim TargetStdOutputHandle As IntPtr
            Dim TargetStdErrorHandle As IntPtr

            Win32True(DuplicateHandle(GetCurrentProcess, StdInputHandle, m_ProcessHandle, TargetStdInputHandle, 0, True, DuplicateOption.DUPLICATE_SAME_ACCESS))
            Win32True(DuplicateHandle(GetCurrentProcess, StdOutputHandle, m_ProcessHandle, TargetStdOutputHandle, 0, True, DuplicateOption.DUPLICATE_SAME_ACCESS))
            Win32True(DuplicateHandle(GetCurrentProcess, StdErrorHandle, m_ProcessHandle, TargetStdErrorHandle, 0, True, DuplicateOption.DUPLICATE_SAME_ACCESS))

            ' TODO: Identify platform more strictly
            If IntPtr.Size = 4 Then
                SetPebStdHandles32(m_ProcessHandle, TargetStdInputHandle, TargetStdOutputHandle, TargetStdErrorHandle)
            Else
                SetPebStdHandles64(m_ProcessHandle, TargetStdInputHandle, TargetStdOutputHandle, TargetStdErrorHandle)
            End If

            Return Me
        End Function

        Private Shared Sub SetPebStdHandles32(ByVal ProcessHandle As IntPtr, ByVal StdInputHandle As IntPtr, ByVal StdOutputHandle As IntPtr, ByVal StdErrorHandle As IntPtr)
            Dim ProcessInformation As PROCESS_BASIC_INFORMATION
            Dim ReturnLength As Int32

            NtSuccess(NtQueryInformationProcess(ProcessHandle, PROCESSINFOCLASS.ProcessBasicInformation, ProcessInformation, Marshal.SizeOf(ProcessInformation), ReturnLength))

            ' PEB +0x10: ProcessParameters
            Dim ProcessParameters As IntPtr = ProcessInformation.PebBaseAddress
            ProcessParameters = ProcessParameters.ToInt32() + &H10
            Dim Buffer As IntPtr = Marshal.AllocHGlobal(&HC)
            Try
                Dim NumberOfBytesRead As Int32
                Win32True(ReadProcessMemory(ProcessHandle, ProcessParameters, Buffer, &H4, NumberOfBytesRead))

                ' ProcessParameters +0x18
                Dim StdHandlesPtr As IntPtr = Marshal.ReadIntPtr(Buffer)
                StdHandlesPtr = StdHandlesPtr.ToInt32() + &H18

                Marshal.WriteIntPtr(Buffer, &H0, StdInputHandle)
                Marshal.WriteIntPtr(Buffer, &H4, StdOutputHandle)
                Marshal.WriteIntPtr(Buffer, &H8, StdErrorHandle)

                Dim NumberOfBytesWritten As Int32
                Win32True(WriteProcessMemory(ProcessHandle, StdHandlesPtr, Buffer, &HC, NumberOfBytesWritten))
            Finally
                Marshal.FreeHGlobal(Buffer)
            End Try
        End Sub

        Private Shared Sub SetPebStdHandles64(ByVal ProcessHandle As IntPtr, ByVal StdInputHandle As IntPtr, ByVal StdOutputHandle As IntPtr, ByVal StdErrorHandle As IntPtr)
            Dim ProcessInformation As PROCESS_BASIC_INFORMATION
            Dim ReturnLength As Int32

            NtSuccess(NtQueryInformationProcess(ProcessHandle, PROCESSINFOCLASS.ProcessBasicInformation, ProcessInformation, Marshal.SizeOf(ProcessInformation), ReturnLength))

            ' PEB +0x20: ProcessParameters
            Dim ProcessParameters As IntPtr = ProcessInformation.PebBaseAddress
            ProcessParameters = ProcessParameters.ToInt64() + &H20
            Dim Buffer As IntPtr = Marshal.AllocHGlobal(&H18)
            Try
                Dim NumberOfBytesRead As Int32
                Win32True(ReadProcessMemory(ProcessHandle, ProcessParameters, Buffer, &H8, NumberOfBytesRead))

                ' ProcessParameters +0x20
                Dim StdHandlesPtr As IntPtr = Marshal.ReadIntPtr(Buffer)
                StdHandlesPtr = StdHandlesPtr.ToInt64() + &H20

                Marshal.WriteIntPtr(Buffer, &H0, StdInputHandle)
                Marshal.WriteIntPtr(Buffer, &H8, StdOutputHandle)
                Marshal.WriteIntPtr(Buffer, &H10, StdErrorHandle)

                Dim NumberOfBytesWritten As Int32
                Win32True(WriteProcessMemory(ProcessHandle, StdHandlesPtr, Buffer, &H18, NumberOfBytesWritten))
            Finally
                Marshal.FreeHGlobal(Buffer)
            End Try
        End Sub

        Public Function GetHandleUnsafe() As IntPtr
            If m_Resumed Then
                Throw New Exception("The suspended process has already been resumed.")
            End If

            Return m_ProcessHandle
        End Function

        Public Function [Resume]() As ProcessEx
            If m_Resumed Then
                Throw New Exception("The suspended process has already been resumed.")
            End If

            Try
                If ResumeThread(m_ThreadHandle) = -1 Then _
                    Throw New Win32Exception()

                Win32True(CloseHandle(m_ThreadHandle))
                Return New ProcessEx(m_ProcessHandle)
            Finally
                m_Resumed = True
            End Try
        End Function

        Public Function Terminate(ByVal ExitCode As Int32) As Suspended
            If m_Resumed Then
                Throw New Exception("The suspended process has already been resumed.")
            End If

            Try
                Win32True(TerminateThread(m_ThreadHandle, ExitCode))
                Win32True(CloseHandle(m_ThreadHandle))
                Win32True(CloseHandle(m_ProcessHandle))
            Finally
                m_Resumed = True
            End Try

            Return Me
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If Not m_Resumed Then
                    Me.Terminate(1)
                End If
            End If
            Me.disposedValue = True
        End Sub

        Protected Overrides Sub Finalize()
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(False)
            MyBase.Finalize()
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Class
