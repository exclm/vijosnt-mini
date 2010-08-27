Namespace Win32
    Friend Class ProcessEx
        Inherits KernelObject
        Implements IDisposable

#Region "Shared members"
        Protected Shared m_InheritHandleSyncRoot As Object
        Protected Shared m_ProcessorCount As Int64

        Shared Sub New()
            m_InheritHandleSyncRoot = New Object()

            ' We don't support CPU hotplugging
            m_ProcessorCount = Environment.ProcessorCount
        End Sub

        Public Shared Function Attach(ByVal ProcessID As Int32) As ProcessEx
            Dim ProcessHandle As IntPtr
            ProcessHandle = OpenProcess(ProcessAccess.PROCESS_ALL_ACCESS, False, ProcessID)
            Win32True(ProcessHandle <> 0)
            Return New ProcessEx(ProcessHandle)
        End Function

        Public Shared Function CreateSuspended(ByVal ApplicationName As String, ByVal CommandLine As String, _
            ByVal Environment As IEnumerable(Of String), ByVal CurrentDirectory As String, ByVal Desktop As String, _
            ByVal StdInputHandleNotOwned As IntPtr, ByVal StdOutputHandleNotOwned As IntPtr, ByVal StdErrorHandleNotOwned As IntPtr, _
            ByVal Token As Token) As Suspended

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
                    If StdInputHandleNotOwned <> IntPtr.Zero Then _
                        StartupInfo.hStdInput = KernelObject.Duplicate(StdInputHandleNotOwned, True)
                    If StdOutputHandleNotOwned <> IntPtr.Zero Then _
                        StartupInfo.hStdOutput = KernelObject.Duplicate(StdOutputHandleNotOwned, True)
                    If StdErrorHandleNotOwned <> IntPtr.Zero Then _
                        StartupInfo.hStdError = KernelObject.Duplicate(StdErrorHandleNotOwned, True)
                    Try
                        If Token Is Nothing Then
                            Win32True(CreateProcess(ApplicationName, CommandLine, Nothing, Nothing, True,
                                CreationFlags.CREATE_BREAKAWAY_FROM_JOB Or CreationFlags.CREATE_DEFAULT_ERROR_MODE Or CreationFlags.CREATE_NO_WINDOW Or CreationFlags.CREATE_SUSPENDED Or CreationFlags.CREATE_UNICODE_ENVIRONMENT,
                                EnvironmentPtr, CurrentDirectory, StartupInfo, ProcessInformation))
                        Else
                            Win32True(CreateProcessAsUser(Token.GetHandleUnsafe(), ApplicationName, CommandLine, Nothing, Nothing, True,
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

        Protected m_InitialProcessTime As Int64
        Protected m_InitialIdleProcessTime As Int64

        Public Sub New(ByVal OwnedHandle As IntPtr)
            MyBase.New(OwnedHandle)

            m_InitialProcessTime = GetProcessTime()
            m_InitialIdleProcessTime = GetIdleProcessTime()
        End Sub

        Public Sub Kill(ByVal ReturnCode As Int32)
            Win32True(TerminateProcess(MyBase.GetHandleUnsafe(), ReturnCode))
        End Sub

        Protected Function GetProcessTime() As Int64
            Dim CreationTime As Int64
            Dim ExitTime As Int64
            Dim KernelTime As Int64
            Dim UserTime As Int64

            Win32True(GetProcessTimes(MyBase.GetHandleUnsafe(), CreationTime, ExitTime, KernelTime, UserTime))

            Return KernelTime + UserTime
        End Function

        Public ReadOnly Property AliveTime() As Int64
            Get
                Dim ProcessTime As Int64 = GetProcessTime() - m_InitialProcessTime
                Dim IdleProcessTime As Int64 = (GetIdleProcessTime() - m_InitialIdleProcessTime) \ m_ProcessorCount

                Return Math.Max(ProcessTime, IdleProcessTime)
            End Get
        End Property

        Public ReadOnly Property ExitCode() As Int32
            Get
                Dim Result As Int32
                Win32True(GetExitCodeProcess(MyBase.GetHandleUnsafe(), Result))
                Return Result
            End Get
        End Property

        Public Class Suspended
            Inherits KernelObject
            Implements IDisposable

            Protected m_Resumed As Boolean
            Protected m_Thread As KernelObject

            Public Sub New(ByVal ProcessHandle As IntPtr, ByVal ThreadHandle As IntPtr)
                MyBase.New(ProcessHandle)
                m_Resumed = False
                m_Thread = New KernelObject(ThreadHandle)
            End Sub

            Public Function [Resume]() As ProcessEx
                If m_Resumed Then
                    Throw New Exception("The suspended process has already been resumed.")
                End If

                m_Resumed = True
                Win32True(ResumeThread(m_Thread.GetHandleUnsafe()) <> -1)
                Return New ProcessEx(MyBase.Duplicate())
            End Function

            Public Sub Kill(ByVal ExitCode As Int32)
                If m_Resumed Then
                    Throw New Exception("The suspended process has already been resumed.")
                End If

                m_Resumed = True
                Win32True(TerminateThread(m_Thread.GetHandleUnsafe(), 1))
            End Sub

#Region "IDisposable Support"
            Private disposedValue As Boolean ' To detect redundant calls

            ' IDisposable
            Protected Overrides Sub Dispose(ByVal disposing As Boolean)
                If Not Me.disposedValue Then
                    If Not m_Resumed Then
                        Me.Kill(1)
                    End If
                    m_Thread.Close()
                End If
                Me.disposedValue = True
                MyBase.Dispose(disposing)
            End Sub

            Protected Overrides Sub Finalize()
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(False)
                MyBase.Finalize()
            End Sub
#End Region
        End Class
    End Class
End Namespace