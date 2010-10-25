Imports VijosNT.Utility
Imports VijosNT.Win32

Namespace Executing
    Friend Class ProcessExecutee
        Inherits Executee

        Private m_WatchDog As WatchDog
        Private m_ProcessMonitor As ProcessMonitor
        Private m_ApplicationName As String
        Private m_CommandLine As String
        Private m_EnvironmentVariables As IEnumerable(Of String)
        Private m_CurrentDirectory As String
        Private m_StdInput As KernelObject
        Private m_StdOutput As KernelObject
        Private m_StdError As KernelObject
        Private m_TimeQuota As Int64?
        Private m_MemoryQuota As Int64?
        Private m_ActiveProcessQuota As Int32?
        Private m_EnableUIRestrictions As Boolean
        Private m_Completion As ProcessExecuteeCompletion
        Private m_CompletionState As Object

        Protected Sub New()

        End Sub

        Public Sub New(ByVal WatchDog As WatchDog, ByVal ProcessMonitor As ProcessMonitor, _
            ByVal ApplicationName As String, ByVal CommandLine As String, _
            ByVal EnvironmentVariables As IEnumerable(Of String), ByVal CurrentDirectory As String, _
            ByVal StdInput As KernelObject, ByVal StdOutput As KernelObject, ByVal StdError As KernelObject, _
            ByVal TimeQuota As Int64?, ByVal MemoryQuota As Int64?, _
            ByVal ActiveProcessQuota As Int32?, ByVal EnableUIRestrictions As Boolean, _
            ByVal Completion As ProcessExecuteeCompletion, ByVal State As Object)
            FinalConstruct(WatchDog, ProcessMonitor, ApplicationName, CommandLine, EnvironmentVariables, CurrentDirectory, StdInput, StdOutput, StdError, TimeQuota, MemoryQuota, ActiveProcessQuota, EnableUIRestrictions, Completion, State)
        End Sub

        Protected Sub FinalConstruct(ByVal WatchDog As WatchDog, ByVal ProcessMonitor As ProcessMonitor, _
            ByVal ApplicationName As String, ByVal CommandLine As String, _
            ByVal EnvironmentVariables As IEnumerable(Of String), ByVal CurrentDirectory As String, _
            ByVal StdInput As KernelObject, ByVal StdOutput As KernelObject, ByVal StdError As KernelObject, _
            ByVal TimeQuota As Int64?, ByVal MemoryQuota As Int64?, _
            ByVal ActiveProcessQuota As Int32?, ByVal EnableUIRestrictions As Boolean, _
            ByVal Completion As ProcessExecuteeCompletion, ByVal State As Object)

            m_WatchDog = WatchDog
            m_ProcessMonitor = ProcessMonitor
            m_ApplicationName = ApplicationName
            m_CommandLine = CommandLine
            m_EnvironmentVariables = EnvironmentVariables
            m_CurrentDirectory = CurrentDirectory
            m_StdInput = StdInput
            m_StdOutput = StdOutput
            m_StdError = StdError
            m_TimeQuota = TimeQuota
            m_MemoryQuota = MemoryQuota
            m_ActiveProcessQuota = ActiveProcessQuota
            m_EnableUIRestrictions = EnableUIRestrictions
            m_Completion = Completion
            m_CompletionState = State
        End Sub

        Public Overrides Sub Execute()
            Dim JobObject = New JobObject()
            Dim Result = New ProcessExecuteeResult()
            Dim Trigger = New MiniTrigger(2, _
                Sub()
                    JobObject.Kill(1)
                    JobObject.Close()
                    Result.State = m_CompletionState
                    m_Completion.Invoke(Result)
                    MyBase.Execute()
                End Sub)

            Dim StdInputHandle As IntPtr = IntPtr.Zero
            Dim StdOutputHandle As IntPtr = IntPtr.Zero
            Dim StdErrorHandle As IntPtr = IntPtr.Zero

            If m_StdInput IsNot Nothing Then _
                StdInputHandle = m_StdInput.GetHandleUnsafe()
            If m_StdOutput IsNot Nothing Then _
                StdOutputHandle = m_StdOutput.GetHandleUnsafe()
            If m_StdError IsNot Nothing Then _
                StdErrorHandle = m_StdError.GetHandleUnsafe()

            Try
                If m_MemoryQuota.HasValue Then
                    Dim VirtualSize As Int64
                    Using Executable As New PortableExecutable(m_ApplicationName)
                        VirtualSize = Executable.VirtualSize
                    End Using
                    If VirtualSize > m_MemoryQuota.Value Then
                        Result.ExitStatus = ERROR_NOT_ENOUGH_QUOTA
                        Result.MemoryQuotaUsage = VirtualSize
                        Trigger.InvokeNow()
                        Return
                    End If
                End If

                Environment.GiveAccess(m_CurrentDirectory)

                Dim Suspended As ProcessEx.Suspended
                Try
                    Suspended = ProcessEx.CreateSuspended( _
                        m_ApplicationName, m_CommandLine, m_EnvironmentVariables, m_CurrentDirectory, Environment.DesktopName, _
                        StdInputHandle, StdOutputHandle, StdErrorHandle, Environment.Token)
                Catch ex As Exception
                    Result.ExitStatus = Nothing
                    Trigger.InvokeNow()
                    Return
                End Try

                JobObject.Assign(Suspended.GetHandleUnsafe())
                With JobObject.Limits
                    .ProcessMemory = m_MemoryQuota
                    .ActiveProcess = m_ActiveProcessQuota
                    .DieOnUnhandledException = True
                    .Commit()
                End With

                If m_EnableUIRestrictions Then
                    With JobObject.UIRestrictions
                        .Handles = True
                        .ReadClipboard = True
                        .WriteClipboard = True
                        .SystemParameters = True
                        .DisplaySettings = True
                        .GlobalAtoms = True
                        .Desktop = True
                        .ExitWindows = True
                        .Commit()
                    End With
                End If

                m_ProcessMonitor.Attach(Suspended, _
                    Sub(ExitResult As ProcessMonitor.Result)
                        Result.ExitStatus = ExitResult.ExitStatus
                        Result.Exception = ExitResult.Exception
                        Trigger.InvokeNonCritical()
                    End Sub)

                m_WatchDog.SetWatch(Suspended.Resume(), m_TimeQuota, _
                    Sub(TimeQuotaUsage As Int64, MemoryQuotaUsage As Int64)
                        Result.TimeQuotaUsage = TimeQuotaUsage
                        Result.MemoryQuotaUsage = MemoryQuotaUsage
                        Trigger.InvokeNonCritical()
                    End Sub)
            Finally
                If m_StdInput IsNot Nothing Then _
                    m_StdInput.Close()
                If m_StdOutput IsNot Nothing Then _
                    m_StdOutput.Close()
                If m_StdError IsNot Nothing Then _
                    m_StdError.Close()
            End Try
        End Sub
    End Class
End Namespace
