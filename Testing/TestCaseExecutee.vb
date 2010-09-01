Imports VijosNT.Compiling
Imports VijosNT.Executing
Imports VijosNT.Utility
Imports VijosNT.Win32

Namespace Testing
    Friend Class TestCaseExecutee
        Inherits ProcessExecutee

        Private m_StreamRecorder As StreamRecorder
        Private m_Result As TestCaseExecuteeResult
        Private m_TargetInstance As TargetInstance
        Private m_TestCase As TestCase
        Private m_Trigger As MiniTrigger

        Public Sub New(ByVal WatchDog As WatchDog, ByVal ProcessMonitor As ProcessMonitor, ByVal TargetInstance As TargetInstance, ByVal TestCase As TestCase, ByVal Completion As TestCaseExecuteeCompletion, ByVal State As Object)
            Dim StdErrorHandle As KernelObject
            Using OutputPipe As New Pipe()
                m_StreamRecorder = New StreamRecorder(OutputPipe.GetReadStream(), 4096, AddressOf StdErrorCompletion, Nothing)
                StdErrorHandle = OutputPipe.GetWriteHandle()
            End Using

            m_Result = New TestCaseExecuteeResult()
            m_Result.State = State
            m_Result.Index = TestCase.Index
            m_TargetInstance = TargetInstance
            m_TestCase = TestCase
            m_Trigger = New MiniTrigger(1, 2, _
                Sub()
                    Completion.Invoke(m_Result)
                End Sub)

            FinalConstruct(WatchDog, ProcessMonitor, TargetInstance.ApplicationName, TargetInstance.CommandLine, TargetInstance.EnvironmentVariables, TargetInstance.WorkingDirectory, _
                TestCase.OpenInput(), TestCase.OpenOutput(), StdErrorHandle, _
                TestCase.TimeQuota, TestCase.MemoryQuota, 1, True, AddressOf ProcessExecuteeCompletion, Nothing)
        End Sub

        Public Overrides ReadOnly Property RequiredEnvironment() As EnvironmentTag
            Get
                Return EnvironmentTag.Untrusted
            End Get
        End Property

        Private Sub StdErrorCompletion(ByVal Result As StreamRecorder.Result)
            If Result.Buffer.Length <> 0 Then
                m_Result.StdErrorMessage = Encoding.Default.GetString(Result.Buffer)
            End If
            m_Trigger.InvokeNonCritical()
        End Sub

        Private Sub TestCaseCompletion(ByVal Result As TestCaseResult)
            m_Result.Score = Result.Score
            m_Trigger.InvokeNonCritical()
        End Sub

        Private Sub ProcessExecuteeCompletion(ByVal Result As ProcessExecuteeResult)
            m_TargetInstance.Dispose()
            m_Result.ExitStatus = Result.ExitStatus
            m_Result.TimeQuotaUsage = Result.TimeQuotaUsage
            m_Result.MemoryQuotaUsage = Result.MemoryQuotaUsage
            m_Result.Exception = Result.Exception
            m_Trigger.InvokeCritical()
        End Sub

        Public Overrides Sub Execute()
            m_TestCase.QueueJudgeWorker(AddressOf TestCaseCompletion, Nothing)
            m_StreamRecorder.Start()
            MyBase.Execute()
        End Sub
    End Class
End Namespace
