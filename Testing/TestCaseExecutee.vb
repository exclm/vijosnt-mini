﻿Imports VijosNT.Compiling
Imports VijosNT.Executing
Imports VijosNT.Utility

Namespace Testing
    Friend Class TestCaseExecutee
        Inherits ProcessExecutee

        Private m_Remaining As Int32
        Private m_Completion As TestCaseExecuteeCompletion
        Private m_Result As TestCaseExecuteeResult
        Private m_TargetInstance As TargetInstance
        Private m_TestCase As TestCase

        ' TODO: Stderr monitor
        Public Sub New(ByVal WatchDog As WatchDog, ByVal ProcessMonitor As ProcessMonitor, ByVal TargetInstance As TargetInstance, ByVal TestCase As TestCase, ByVal Completion As TestCaseExecuteeCompletion, ByVal State As Object)
            m_Completion = Completion
            m_Result.State = State
            m_Result.Index = TestCase.Index
            m_TargetInstance = TargetInstance
            m_TestCase = TestCase

            m_Remaining = 2
            FinalConstruct(WatchDog, ProcessMonitor, TargetInstance.ApplicationName, TargetInstance.CommandLine, TargetInstance.EnvironmentVariables, TargetInstance.WorkingDirectory, _
                TestCase.OpenInput(), TestCase.OpenOutput(), Nothing, _
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
            m_TargetInstance.Dispose()
            m_Result.ExitStatus = Result.ExitStatus
            m_Result.TimeQuotaUsage = Result.TimeQuotaUsage
            m_Result.MemoryQuotaUsage = Result.MemoryQuotaUsage
            m_Result.Exception = Result.Exception
            WorkCompleted()
        End Sub

        Private Sub WorkCompleted()
            If Interlocked.Decrement(m_Remaining) = 0 Then
                If m_Completion IsNot Nothing Then
                    m_Completion.Invoke(m_Result)
                End If
            End If
        End Sub

        Public Overrides Sub Execute()
            m_TestCase.QueueJudgeWorker(AddressOf TestCaseCompletion, Nothing)
            MyBase.Execute()
        End Sub
    End Class
End Namespace
