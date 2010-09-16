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

            Dim TimeQuota As Nullable(Of Int64) = TestCase.TimeQuota
            Dim MemoryQuota As Nullable(Of Int64) = TestCase.MemoryQuota

            With TargetInstance.Target.CompilerInstance.Compiler
                If TimeQuota.HasValue Then
                    If .TimeFactor.HasValue Then _
                        TimeQuota *= .TimeFactor.Value
                    If .TimeOffset.HasValue Then _
                        TimeQuota -= .TimeOffset.Value
                End If
                If MemoryQuota.HasValue Then
                    If .MemoryFactor.HasValue Then _
                        MemoryQuota *= .MemoryFactor.Value
                    If .MemoryOffset.HasValue Then _
                        MemoryQuota -= .MemoryOffset.Value
                End If
            End With

            FinalConstruct(WatchDog, ProcessMonitor, TargetInstance.ApplicationName, TargetInstance.CommandLine, TargetInstance.EnvironmentVariables, TargetInstance.WorkingDirectory, _
                TestCase.OpenInput(), TestCase.OpenOutput(), StdErrorHandle, _
                TimeQuota, MemoryQuota, 1, True, AddressOf ProcessExecuteeCompletion, Nothing)
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
            Dim TimeQuotaUsage As Int64 = Result.TimeQuotaUsage
            Dim MemoryQuotaUsage As Int64 = Result.MemoryQuotaUsage

            With m_TargetInstance.Target.CompilerInstance.Compiler
                If .TimeOffset.HasValue Then _
                    TimeQuotaUsage = Math.Max(TimeQuotaUsage + .TimeOffset.Value, 0)
                If .TimeFactor.HasValue Then
                    Try
                        TimeQuotaUsage /= .TimeFactor.Value
                    Catch ex As OverflowException
                        TimeQuotaUsage = m_TestCase.TimeQuota + 1
                    End Try
                End If
                If .MemoryOffset.HasValue Then _
                    MemoryQuotaUsage = Math.Max(TimeQuotaUsage + .MemoryOffset.Value, 0)
                If .MemoryFactor.HasValue Then
                    Try
                        MemoryQuotaUsage /= .MemoryFactor.Value
                    Catch ex As OverflowException
                        MemoryQuotaUsage = m_TestCase.MemoryQuota + 1
                    End Try
                End If
            End With

            m_TargetInstance.Dispose()

            m_Result.ExitStatus = Result.ExitStatus
            m_Result.TimeQuotaUsage = TimeQuotaUsage
            m_Result.MemoryQuotaUsage = MemoryQuotaUsage
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
