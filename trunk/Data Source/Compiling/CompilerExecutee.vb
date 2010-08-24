Imports VijosNT.Executing
Imports VijosNT.Utility

Namespace Compiling
    Friend Class CompilerExecutee
        Inherits ProcessExecutee

        Private m_TempPathServer As TempPathServer
        Private m_Completion As CompilerExecuteeCompletion
        Private m_Result As CompilerExecuteeResult

        ' TODO: Add stdout and stderr monitor
        Public Sub New(ByVal WatchDog As WatchDog, ByVal ProcessMonitor As ProcessMonitor, ByVal TempPathServer As TempPathServer, ByVal Compiler As Compiler, ByVal SourceCode As Stream, ByVal Completion As CompilerExecuteeCompletion, ByVal State As Object)
            Dim CompilerInstance As CompilerInstance = Compiler.CreateInstance(SourceCode)
            m_TempPathServer = TempPathServer
            m_Completion = Completion
            m_Result.State = State

            FinalConstruct(WatchDog, ProcessMonitor, _
                Compiler.ApplicationName, Compiler.CommandLine, CompilerInstance.EnvironmentVariables, CompilerInstance.WorkingDirectory, _
                Nothing, Nothing, Nothing, Compiler.TimeQuota, Compiler.MemoryQuota, Compiler.ActiveProcessQuota, _
                AddressOf ProcessExecuteeCompletion, CompilerInstance)
        End Sub

        Private Sub ProcessExecuteeCompletion(ByVal Result As ProcessExecuteeResult)
            Dim CompilerInstance As CompilerInstance = DirectCast(Result.State, CompilerInstance)
            m_Result.ExitStatus = Result.ExitStatus
            m_Result.TimeQuotaUsage = Result.TimeQuotaUsage
            m_Result.MemoryQuotaUsage = Result.MemoryQuotaUsage
            m_Result.Exception = Result.Exception
            If Result.ExitStatus IsNot Nothing And m_Result.Exception Is Nothing Then
                Try
                    m_Result.Target = CompilerInstance.OpenTarget()
                Catch ex As Exception
                    m_Result.Target = Nothing
                End Try
            End If
            Try
                m_Completion.Invoke(m_Result)
            Finally
                If m_Result.Target IsNot Nothing Then _
                    m_Result.Target.Dispose()
            End Try
        End Sub
    End Class
End Namespace
