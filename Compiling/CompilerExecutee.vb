Imports VijosNT.Executing
Imports VijosNT.Win32
Imports VijosNT.Utility

Namespace Compiling
    Friend Class CompilerExecutee
        Inherits ProcessExecutee

        Private m_TempPathServer As TempPathServer
        Private m_StreamRecorder As StreamRecorder
        Private m_Completion As CompilerExecuteeCompletion
        Private m_Result As CompilerExecuteeResult
        Private m_Remaining As Int32

        Public Sub New(ByVal WatchDog As WatchDog, ByVal ProcessMonitor As ProcessMonitor, ByVal TempPathServer As TempPathServer, ByVal Compiler As Compiler, ByVal SourceCode As Stream, ByVal Completion As CompilerExecuteeCompletion, ByVal State As Object)
            Dim CompilerInstance As CompilerInstance = Compiler.CreateInstance(SourceCode)
            m_TempPathServer = TempPathServer
            Dim StdOutputHandle As KernelObject
            Dim StdErrorHandle As KernelObject
            Using OutputPipe As New Pipe()
                m_StreamRecorder = New StreamRecorder(OutputPipe.GetReadStream(), 4096, AddressOf StdErrorCompletion, Nothing)
                StdOutputHandle = OutputPipe.GetWriteHandle()
                StdErrorHandle = OutputPipe.GetWriteHandle()
            End Using

            m_Completion = Completion
            m_Result.State = State

            m_Remaining = 2
            FinalConstruct(WatchDog, ProcessMonitor, _
                Compiler.ApplicationName, Compiler.CommandLine, CompilerInstance.EnvironmentVariables, CompilerInstance.WorkingDirectory, _
                Nothing, StdOutputHandle, StdErrorHandle, Compiler.TimeQuota, Compiler.MemoryQuota, Compiler.ActiveProcessQuota, _
                AddressOf ProcessExecuteeCompletion, CompilerInstance)
        End Sub

        Private Sub StdErrorCompletion(ByVal Result As StreamRecorder.Result)
            If Result.Buffer.Length <> 0 Then
                m_Result.StdErrorMessage = Encoding.Default.GetString(Result.Buffer)
            End If
            WorkCompleted()
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
            WorkCompleted()
        End Sub

        Public Overrides Sub Execute()
            m_StreamRecorder.Start()
            MyBase.Execute()
        End Sub

        Private Sub WorkCompleted()
            If Interlocked.Decrement(m_Remaining) = 0 Then
                Try
                    m_Completion.Invoke(m_Result)
                Finally
                    If m_Result.Target IsNot Nothing Then _
                        m_Result.Target.Dispose()
                End Try
            End If
        End Sub
    End Class
End Namespace
