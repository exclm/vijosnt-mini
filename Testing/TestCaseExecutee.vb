Imports VijosNT.Compiling
Imports VijosNT.Executing
Imports VijosNT.Utility
Imports VijosNT.Win32

Namespace Testing
    Friend Class TestCaseExecutee
        Inherits ProcessExecutee

        Private m_TargetInstance As TargetInstance
        Private m_TestCase As TestCase
        Private m_Completion As TestCaseExecuteeCompletion
        Private m_CompletionState As Object

        Public Sub New(ByVal TargetInstance As TargetInstance, ByVal TestCase As TestCase, ByVal Completion As TestCaseExecuteeCompletion, ByVal State As Object)
            MyBase.New(TargetInstance.ApplicationName, TargetInstance.CommandLine, _
                TargetInstance.EnvironmentVariables, TargetInstance.WorkingDirectory, _
                Nothing, Nothing, 1, True)

            m_TargetInstance = TargetInstance
            m_TestCase = TestCase
            m_Completion = Completion
            m_CompletionState = State
        End Sub

        Public Overrides ReadOnly Property RequiredEnvironment() As EnvironmentTag
            Get
                Return EnvironmentTag.Untrusted
            End Get
        End Property

        Public Overrides Sub Execute()
            Dim TimeQuota As Int64? = m_TestCase.TimeQuota
            Dim MemoryQuota As Int64? = m_TestCase.MemoryQuota

            With m_TargetInstance.Target.CompilerInstance.Compiler
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

            MyBase.TimeQuota = TimeQuota
            MyBase.MemoryQuota = MemoryQuota

            Dim Result = New TestCaseExecuteeResult()
            Result.Index = m_TestCase.Index

            Dim Trigger = New MiniTrigger(1, 2, _
                Sub()
                    Result.State = m_CompletionState
                    m_Completion.Invoke(Result)
                End Sub)

            MyBase.Completion = _
                Sub(ExecuteeResult As ProcessExecuteeResult)
                    Dim TimeQuotaUsage As Int64 = ExecuteeResult.TimeQuotaUsage
                    Dim MemoryQuotaUsage As Int64 = ExecuteeResult.MemoryQuotaUsage

                    With m_TargetInstance.Target.CompilerInstance.Compiler
                        If .TimeOffset.HasValue Then
                            TimeQuotaUsage = Math.Max(TimeQuotaUsage + .TimeOffset.Value, 0)
                        End If
                        If .TimeFactor.HasValue Then
                            Try
                                TimeQuotaUsage /= .TimeFactor.Value
                            Catch ex As OverflowException
                                TimeQuotaUsage = m_TestCase.TimeQuota + 1
                            End Try
                        End If
                        If .MemoryOffset.HasValue Then
                            MemoryQuotaUsage = Math.Max(TimeQuotaUsage + .MemoryOffset.Value, 0)
                        End If
                        If .MemoryFactor.HasValue Then
                            Try
                                MemoryQuotaUsage /= .MemoryFactor.Value
                            Catch ex As OverflowException
                                MemoryQuotaUsage = m_TestCase.MemoryQuota + 1
                            End Try
                        End If
                    End With

                    m_TargetInstance.Dispose()

                    Result.ExitStatus = ExecuteeResult.ExitStatus
                    Result.TimeQuotaUsage = TimeQuotaUsage
                    Result.MemoryQuotaUsage = MemoryQuotaUsage
                    Result.Exception = ExecuteeResult.Exception
                    Trigger.InvokeCritical()
                End Sub

            ' Open test case I/O
            MyBase.StdInput = m_TestCase.OpenInput()
            MyBase.StdOutput = m_TestCase.OpenOutput()

            ' Start recording stderr
            Using OutputPipe As New Pipe()
                Dim StreamRecorder = New StreamRecorder(OutputPipe.GetReadStream(), 4096, _
                    Sub(StreamResult As StreamRecorder.Result)
                        If StreamResult.Buffer.Length <> 0 Then
                            Result.StdErrorMessage = Encoding.Default.GetString(StreamResult.Buffer)
                        End If
                        Trigger.InvokeNonCritical()
                    End Sub, Nothing)
                MyBase.StdError = OutputPipe.GetWriteHandle()
                StreamRecorder.Start()
            End Using

            ' Start judging
            m_TestCase.QueueJudgeWorker( _
                Sub(TestCaseResult As TestCaseResult)
                    Result.Score = TestCaseResult.Score
                    Trigger.InvokeNonCritical()
                End Sub, Nothing)
            MyBase.Execute()
        End Sub
    End Class
End Namespace
