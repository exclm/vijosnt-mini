Imports VijosNT.Executing
Imports VijosNT.Utility

Namespace Testing
    Friend Class TestCaseExecutee
        Inherits ProcessExecutee

        Private m_Remaining As Int32
        Private m_Completion As TestCaseExecuteeCompletion
        Private m_Result As TestCaseExecuteeResult

        ' TODO: Use stuff from Compiling module to replace ApplicationName
        ' TODO: Stderr monitor
        Public Sub New(ByVal WatchDog As WatchDog, ByVal ProcessMonitor As ProcessMonitor, ByVal ApplicationName As String, ByVal TestCase As TestCase, ByVal Completion As TestCaseExecuteeCompletion, ByVal State As Object)
            m_Completion = Completion
            m_Result.State = State
            m_Result.Index = TestCase.Index

            m_Remaining = 2
            FinalConstruct(WatchDog, ProcessMonitor, ApplicationName, Nothing, Nothing, Nothing, _
                TestCase.OpenInput(), TestCase.OpenOutput(AddressOf TestCaseCompletion, Nothing), Nothing, _
                TestCase.TimeQuota, TestCase.MemoryQuota, 1, AddressOf ProcessExecuteeCompletion, Nothing)
        End Sub

        Public Overrides ReadOnly Property RequiredEnvironment() As EnvironmentTag
            Get
                Return EnvironmentTag.Untrusted
            End Get
        End Property

        Private Sub TestCaseCompletion(ByVal Result As TestCaseResult)
            m_Result.Score = Result.Score
            WorkCompleted()
        End Sub

        Private Sub ProcessExecuteeCompletion(ByVal Result As ProcessExecuteeResult)
            m_Result.ExitStatus = Result.ExitStatus
            m_Result.TimeQuotaUsage = Result.TimeQuotaUsage
            m_Result.MemoryQuotaUsage = Result.MemoryQuotaUsage
            m_Result.Exception = Result.Exception
            WorkCompleted()
        End Sub

        Private Sub WorkCompleted()
            If Interlocked.Decrement(m_Remaining) = 0 Then
                If m_Completion IsNot Nothing Then
                    Try
                        m_Completion.Invoke(m_Result)
                    Catch ex As Exception
                        ' eat it
                    End Try
                End If
            End If
        End Sub
    End Class
End Namespace
