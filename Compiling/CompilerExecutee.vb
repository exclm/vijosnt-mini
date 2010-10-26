Imports VijosNT.Executing
Imports VijosNT.Win32
Imports VijosNT.Utility

Namespace Compiling
    Friend Class CompilerExecutee
        Inherits ProcessExecutee

        Private m_Compiler As Compiler
        Private m_SourceCode As Stream
        Private m_Completion As CompilerExecuteeCompletion
        Private m_CompletionState As Object

        Public Sub New(ByVal Compiler As Compiler, ByVal SourceCode As Stream, ByVal Completion As CompilerExecuteeCompletion, ByVal State As Object)
            MyBase.New(Compiler.ApplicationName, Compiler.CommandLine, _
                Compiler.EnvironmentVariables, Nothing, Compiler.TimeQuota, _
                Compiler.MemoryQuota, Compiler.ActiveProcessQuota, False)

            m_Compiler = Compiler
            m_SourceCode = SourceCode
            m_Completion = Completion
            m_CompletionState = State
        End Sub

        Public Overrides Sub Execute()
            Dim CompilerInstance As CompilerInstance = m_Compiler.CreateInstance(m_SourceCode)
            Dim Result = New CompilerExecuteeResult()
            Dim Trigger = New MiniTrigger(1, 1, _
                Sub()
                    Result.State = m_CompletionState
                    m_Completion.Invoke(Result)
                End Sub)

            MyBase.Completion = _
                Sub(ExecuteeResult As ProcessExecuteeResult)
                    Result.ExitStatus = ExecuteeResult.ExitStatus
                    Result.TimeQuotaUsage = ExecuteeResult.TimeQuotaUsage
                    Result.MemoryQuotaUsage = ExecuteeResult.MemoryQuotaUsage
                    Result.Exception = ExecuteeResult.Exception
                    If ExecuteeResult.ExitStatus IsNot Nothing And Result.Exception Is Nothing Then
                        Try
                            Result.Target = CompilerInstance.OpenTarget()
                        Catch ex As Exception
                            Result.Target = Nothing
                            CompilerInstance.Dispose()
                        End Try
                    End If
                    Trigger.InvokeCritical()
                End Sub

            MyBase.CurrentDirectory = CompilerInstance.WorkingDirectory
            Using OutputPipe As New Pipe()
                Dim StreamRecorder = New StreamRecorder(OutputPipe.GetReadStream(), 4096, _
                    Sub(StreamResult As StreamRecorder.Result)
                        If StreamResult.Buffer.Length <> 0 Then
                            Result.StdErrorMessage = Encoding.Default.GetString(StreamResult.Buffer)
                        End If
                        Trigger.InvokeNonCritical()
                    End Sub, Nothing)
                MyBase.StdOutput = OutputPipe.GetWriteHandle()
                MyBase.StdError = OutputPipe.GetWriteHandle()
                StreamRecorder.Start()
            End Using

            MyBase.Execute()
        End Sub
    End Class
End Namespace
