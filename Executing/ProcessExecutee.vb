Imports VijosNT.Utility
Imports VijosNT.Win32

Namespace Executing
    Friend Class ProcessExecutee
        Inherits Executee

        Protected m_WatchDog As WatchDog
        Protected m_ProcessMonitor As ProcessMonitor
        Protected m_JobObject As JobObject
        Protected m_ApplicationName As String
        Protected m_CommandLine As String
        Protected m_EnvironmentVariables As IEnumerable(Of String)
        Protected m_CurrentDirectory As String
        Protected m_StdInput As KernelObject
        Protected m_StdOutput As KernelObject
        Protected m_StdError As KernelObject
        Protected m_TimeQuota As Nullable(Of Int64)
        Protected m_MemoryQuota As Nullable(Of Int64)
        Protected m_ActiveProcessQuota As Nullable(Of Int32)
        Protected m_Remaining As Int32
        Protected m_Completion As ProcessExecuteeCompletion
        Protected m_Result As ProcessExecuteeResult
        Protected m_SyncRoot As Object

        Public Sub New(ByVal WatchDog As WatchDog, ByVal ProcessMonitor As ProcessMonitor, _
            ByVal ApplicationName As String, ByVal CommandLine As String, _
            ByVal EnvironmentVariables As IEnumerable(Of String), ByVal CurrentDirectory As String, _
            ByVal StdInput As KernelObject, ByVal StdOutput As KernelObject, ByVal StdError As KernelObject, _
            ByVal TimeQuota As Nullable(Of Int64), ByVal MemoryQuota As Nullable(Of Int64), ByVal ActiveProcessQuota As Nullable(Of Int32), _
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
            m_Completion = Completion
            m_Result.State = State
            m_SyncRoot = New Object()
        End Sub

        Protected Sub WorkCompleted()
            If Interlocked.Decrement(m_Remaining) = 0 Then
                If m_Completion IsNot Nothing Then
                    Try
                        m_Completion.Invoke(m_Result)
                    Catch ex As Exception
                        ' eat it
                    End Try
                End If
                MyBase.Execute()
            End If
        End Sub

        Protected Sub WatchDogCallback(ByVal Result As WatchDog.Result)
            SyncLock m_SyncRoot
                m_Result.TimeQuotaUsage = Result.QuotaUsage
                m_Result.MemoryQuotaUsage = m_JobObject.Limits.PeakProcessMemoryUsed
            End SyncLock
            WorkCompleted()
        End Sub

        Protected Sub ProcessMonitorCallback(ByVal Result As ProcessMonitor.Result)
            SyncLock m_SyncRoot
                m_Result.ExitStatus = Result.ExitStatus
                m_Result.Exception = Result.Exception
            End SyncLock
            WorkCompleted()
        End Sub

        Public Overrides Sub Execute()
            m_Remaining = 2

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
                Dim Suspended As ProcessEx.Suspended = ProcessEx.CreateSuspended( _
                    m_ApplicationName, m_CommandLine, m_EnvironmentVariables, m_CurrentDirectory, Environment.DesktopName, _
                    StdInputHandle, StdOutputHandle, StdErrorHandle, Environment.Token)

                m_JobObject = New JobObject()
                m_JobObject.Assign(Suspended.GetHandleUnsafe())
                With m_JobObject.Limits
                    .ProcessMemory = m_MemoryQuota
                    .ActiveProcess = m_ActiveProcessQuota
                    .DieOnUnhandledException = True
                    .Commit()
                End With

                m_ProcessMonitor.Attach(Suspended, AddressOf ProcessMonitorCallback, Nothing)
                m_WatchDog.SetWatch(Suspended.Resume(), m_TimeQuota, AddressOf WatchDogCallback, Nothing)
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
