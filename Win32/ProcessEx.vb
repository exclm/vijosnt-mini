Friend Class ProcessEx
    Implements IDisposable

#Region "Shared members"
    Protected Shared m_InheritHandleSyncRoot As Object

    Shared Sub New()
        m_InheritHandleSyncRoot = New Object()
    End Sub

    Public Shared Function Attach(ByVal ProcessID As Int32) As ProcessEx
        Dim hProcess As IntPtr
        hProcess = OpenProcess(ProcessAccess.PROCESS_ALL_ACCESS, False, ProcessID)
        Return New ProcessEx(New Handle(hProcess))
    End Function

    Public Shared Function CreateSuspended(ByVal ApplicationName As String, ByVal CommandLine As String, _
        ByVal Environment As IEnumerable(Of String), ByVal CurrentDirectory As String, ByVal Desktop As String, _
        ByVal StdInput As Handle, ByVal StdOutput As Handle, ByVal StdError As Handle, ByVal Token As Token) As Suspended

        Dim StartupInfo As STARTUPINFO

        StartupInfo.cb = Marshal.SizeOf(GetType(STARTUPINFO))
        StartupInfo.Desktop = Desktop
        StartupInfo.Title = Nothing
        StartupInfo.dwFlags = StartupFlags.STARTF_FORCEOFFFEEDBACK Or StartupFlags.STARTF_USESTDHANDLES

        Dim ProcessInformation As New PROCESS_INFORMATION

        Dim EnvironmentPtr As IntPtr = IntPtr.Zero
        If Environment IsNot Nothing Then EnvironmentPtr = AllocEnvironment(Environment)
        Try
            SyncLock m_InheritHandleSyncRoot
                If StdInput IsNot Nothing Then _
                    StartupInfo.hStdInput = StdInput.Duplicate(True)
                If StdOutput IsNot Nothing Then _
                    StartupInfo.hStdOutput = StdOutput.Duplicate(True)
                If StdError IsNot Nothing Then _
                    StartupInfo.hStdError = StdError.Duplicate(True)
                Try
                    If Token Is Nothing Then
                        Win32True(CreateProcess(ApplicationName, CommandLine, Nothing, Nothing, True,
                            CreationFlags.CREATE_BREAKAWAY_FROM_JOB Or CreationFlags.CREATE_DEFAULT_ERROR_MODE Or CreationFlags.CREATE_NO_WINDOW Or CreationFlags.CREATE_SUSPENDED Or CreationFlags.CREATE_UNICODE_ENVIRONMENT,
                            EnvironmentPtr, CurrentDirectory, StartupInfo, ProcessInformation))
                    Else
                        Win32True(CreateProcessAsUser(Token.GetHandle().GetHandleUnsafe(), ApplicationName, CommandLine, Nothing, Nothing, True,
                            CreationFlags.CREATE_BREAKAWAY_FROM_JOB Or CreationFlags.CREATE_DEFAULT_ERROR_MODE Or CreationFlags.CREATE_NO_WINDOW Or CreationFlags.CREATE_SUSPENDED Or CreationFlags.CREATE_UNICODE_ENVIRONMENT,
                            EnvironmentPtr, CurrentDirectory, StartupInfo, ProcessInformation))
                    End If
                Finally
                    If StartupInfo.hStdInput <> IntPtr.Zero Then _
                        Win32True(CloseHandle(StartupInfo.hStdInput))
                    If StartupInfo.hStdOutput <> IntPtr.Zero Then _
                        Win32True(CloseHandle(StartupInfo.hStdOutput))
                    If StartupInfo.hStdError <> IntPtr.Zero Then _
                        Win32True(CloseHandle(StartupInfo.hStdError))
                End Try
            End SyncLock
        Finally
            If EnvironmentPtr <> IntPtr.Zero Then Marshal.FreeHGlobal(EnvironmentPtr)
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

    Protected m_Handle As Handle

    Public Sub New(ByVal Handle As Handle)
        m_Handle = Handle
    End Sub

    Public Function GetHandle() As Handle
        Return m_Handle
    End Function

    Public Sub Kill(ByVal ReturnCode As Int32)
        Win32True(TerminateProcess(m_Handle.GetHandleUnsafe(), ReturnCode))
    End Sub

    ' TODO: attach debugger
    Public Class Suspended
        Implements IDisposable

        Protected m_Resumed As Boolean
        Protected m_Process As Handle
        Protected m_Thread As Handle

        Public Sub New(ByVal ProcessHandle As IntPtr, ByVal ThreadHandle As IntPtr)
            m_Resumed = False
            m_Process = New Handle(ProcessHandle)
            m_Thread = New Handle(ThreadHandle)
        End Sub

        Public Function GetHandle() As Handle
            If m_Resumed Then
                Throw New Exception("The suspended process has already been resumed.")
            End If

            Return m_Process
        End Function

        Public Function [Resume]() As ProcessEx
            If m_Resumed Then
                Throw New Exception("The suspended process has already been resumed.")
            End If

            m_Resumed = True
            Win32True(ResumeThread(m_Thread.GetHandleUnsafe()) <> -1)
            Return New ProcessEx(New Handle(m_Process.Duplicate()))
        End Function

        Public Sub Terminate(ByVal ExitCode As Int32)
            If m_Resumed Then
                Throw New Exception("The suspended process has already been resumed.")
            End If

            m_Resumed = True
            Win32True(TerminateThread(m_Thread.GetHandleUnsafe(), ExitCode))
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If Not m_Resumed Then
                    Me.Terminate(1)
                End If
                m_Process.Dispose()
                m_Thread.Dispose()
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

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 检测冗余的调用

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            m_Handle.Dispose()
        End If
        Me.disposedValue = True
    End Sub

    Protected Overrides Sub Finalize()
        ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ' Visual Basic 添加此代码是为了正确实现可处置模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
