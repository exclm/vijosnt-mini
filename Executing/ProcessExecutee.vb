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
        Private m_TimeQuota As Nullable(Of Int64)
        Private m_MemoryQuota As Nullable(Of Int64)
        Private m_ActiveProcessQuota As Nullable(Of Int32)
        Private m_EnableUIRestrictions As Boolean
        Private m_Trigger As MiniTrigger
        Private m_Result As ProcessExecuteeResult
        Private m_JobObject As JobObject

        Protected Sub New()

        End Sub

        Public Sub New(ByVal WatchDog As WatchDog, ByVal ProcessMonitor As ProcessMonitor, _
            ByVal ApplicationName As String, ByVal CommandLine As String, _
            ByVal EnvironmentVariables As IEnumerable(Of String), ByVal CurrentDirectory As String, _
            ByVal StdInput As KernelObject, ByVal StdOutput As KernelObject, ByVal StdError As KernelObject, _
            ByVal TimeQuota As Nullable(Of Int64), ByVal MemoryQuota As Nullable(Of Int64), _
            ByVal ActiveProcessQuota As Nullable(Of Int32), ByVal EnableUIRestrictions As Boolean, _
            ByVal Completion As ProcessExecuteeCompletion, ByVal State As Object)
            FinalConstruct(WatchDog, ProcessMonitor, ApplicationName, CommandLine, EnvironmentVariables, CurrentDirectory, StdInput, StdOutput, StdError, TimeQuota, MemoryQuota, ActiveProcessQuota, EnableUIRestrictions, Completion, State)
        End Sub

        Protected Sub FinalConstruct(ByVal WatchDog As WatchDog, ByVal ProcessMonitor As ProcessMonitor, _
            ByVal ApplicationName As String, ByVal CommandLine As String, _
            ByVal EnvironmentVariables As IEnumerable(Of String), ByVal CurrentDirectory As String, _
            ByVal StdInput As KernelObject, ByVal StdOutput As KernelObject, ByVal StdError As KernelObject, _
            ByVal TimeQuota As Nullable(Of Int64), ByVal MemoryQuota As Nullable(Of Int64), _
            ByVal ActiveProcessQuota As Nullable(Of Int32), ByVal EnableUIRestrictions As Boolean, _
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
            m_Result = New ProcessExecuteeResult()
            m_Result.State = State
            m_Trigger = New MiniTrigger(2, _
                Sub()
                    m_JobObject.Kill(1)
                    m_JobObject.Close()
                    Completion.Invoke(m_Result)
                    MyBase.Execute()
                End Sub)
        End Sub

        Private Sub WatchDogCompletion(ByVal Result As WatchDog.Result)
            m_Result.TimeQuotaUsage = Result.QuotaUsage
            m_Result.MemoryQuotaUsage = m_JobObject.Limits.PeakProcessMemoryUsed
            m_Trigger.InvokeNonCritical()
        End Sub

        Private Sub ProcessMonitorCompletion(ByVal Result As ProcessMonitor.Result)
            m_Result.ExitStatus = Result.ExitStatus
            m_Result.Exception = Result.Exception
            m_Trigger.InvokeNonCritical()
        End Sub

        Public Overrides Sub Execute()
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
                        m_Result.ExitStatus = ERROR_NOT_ENOUGH_QUOTA
                        m_Result.MemoryQuotaUsage = VirtualSize
                        m_Trigger.InvokeNow()
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
                    m_Result.ExitStatus = Nothing
                    m_Trigger.InvokeNow()
                    Return
                End Try

                m_JobObject = New JobObject()
                m_JobObject.Assign(Suspended.GetHandleUnsafe())
                With m_JobObject.Limits
                    .ProcessMemory = m_MemoryQuota
                    .ActiveProcess = m_ActiveProcessQuota
                    .DieOnUnhandledException = True
                    .Commit()
                End With

                If m_EnableUIRestrictions Then
                    With m_JobObject.UIRestrictions
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

                m_ProcessMonitor.Attach(Suspended, AddressOf ProcessMonitorCompletion, Nothing)
                m_WatchDog.SetWatch(Suspended.Resume(), m_TimeQuota, AddressOf WatchDogCompletion, Nothing)
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
