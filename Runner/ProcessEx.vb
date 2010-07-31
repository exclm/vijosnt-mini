Public NotInheritable Class ProcessEx
    Inherits WaitHandle

#Region "Shared members"
    Public Shared Function Attach(ByVal ProcessID As Int32) As ProcessEx
        Dim hProcess As IntPtr
        hProcess = OpenProcess(ProcessAccess.PROCESS_ALL_ACCESS, False, ProcessID)
        Return New ProcessEx(hProcess)
    End Function

    ' TODO: login token, attach debugger\job object (in the Suspended class)
    Public Shared Function Create(
        ByVal ApplicationName As String,
        ByVal CommandLine As String,
        ByVal Environment As IEnumerable(Of String),
        ByVal CurrentDirectory As String,
        ByVal StdInputHandle As IntPtr,
        ByVal StdOutputHandle As IntPtr,
        ByVal StdErrorHandle As IntPtr,
        ByVal Desktop As String) As Suspended

        Dim StartupInfo As STARTUPINFO

        StartupInfo.cb = Marshal.SizeOf(GetType(STARTUPINFO))
        StartupInfo.Desktop = Desktop
        StartupInfo.Title = Nothing
        StartupInfo.dwFlags = StartupFlags.STARTF_FORCEOFFFEEDBACK Or StartupFlags.STARTF_USESTDHANDLES
        StartupInfo.hStdInput = StdInputHandle
        StartupInfo.hStdOutput = StdOutputHandle
        StartupInfo.hStdError = StdErrorHandle

        Dim EnvironmentPtr As IntPtr = IntPtr.Zero

        If Environment IsNot Nothing Then
            ' TODO: Allocate EnvironmentPtr and initialize with specified data
        End If

        Dim ProcessInformation As New PROCESS_INFORMATION

        Try
            CreateProcess(ApplicationName, CommandLine, Nothing, Nothing, False,
                CreationFlags.CREATE_BREAKAWAY_FROM_JOB Or CreationFlags.CREATE_DEFAULT_ERROR_MODE Or CreationFlags.CREATE_NO_WINDOW Or CreationFlags.CREATE_SUSPENDED Or CreationFlags.CREATE_UNICODE_ENVIRONMENT,
                EnvironmentPtr, CurrentDirectory, StartupInfo, ProcessInformation)
        Finally
            If EnvironmentPtr <> IntPtr.Zero Then
                Marshal.FreeHGlobal(EnvironmentPtr)
            End If
        End Try

        Return New Suspended(ProcessInformation.hProcess, ProcessInformation.hThread)
    End Function
#End Region

    Private m_Handle As IntPtr

    Public Sub New(ByVal OwnedHandle As IntPtr)
        m_Handle = OwnedHandle
        MyBase.SafeWaitHandle = New SafeWaitHandle(m_Handle, True)
    End Sub

    Public Sub Kill(ByVal ReturnCode As Int32)
        Win32True(TerminateProcess(m_Handle, ReturnCode))
    End Sub

    Public Class Suspended
        Implements IDisposable

        Protected m_ProcessHandle As IntPtr
        Protected m_ThreadHandle As IntPtr
        Protected m_Resumed As Boolean

        Public Sub New(ByVal ProcessHandle As IntPtr, ByVal ThreadHandle As IntPtr)
            m_Resumed = False
            m_ProcessHandle = ProcessHandle
            m_ThreadHandle = ThreadHandle
        End Sub

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
                m_ProcessHandle = 0
                m_ThreadHandle = 0
            End Try
        End Function

        Public Sub Terminate(ByVal ExitCode As Int32)
            If m_Resumed Then
                Throw New Exception("The suspended process has already been resumed.")
            End If

            Try
                Win32True(TerminateThread(m_ThreadHandle, ExitCode))
            Finally
                m_Resumed = True
                m_ProcessHandle = 0
                m_ThreadHandle = 0
            End Try
        End Sub

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
