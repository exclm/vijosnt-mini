Imports VijosNT.Executing
Imports VijosNT.Win32
Imports VijosNT.Utility

Namespace Compiling
    Friend Class CompilerExecutee
        Inherits ProcessExecutee

        Private m_StreamRecorder As StreamRecorder
        Private m_Result As CompilerExecuteeResult
        Private m_Trigger As MiniTrigger

        Public Sub New(ByVal WatchDog As WatchDog, ByVal ProcessMonitor As ProcessMonitor, ByVal Compiler As Compiler, ByVal SourceCode As Stream, ByVal Completion As CompilerExecuteeCompletion, ByVal State As Object)
            Dim CompilerInstance As CompilerInstance = Compiler.CreateInstance(SourceCode)
            Dim StdOutputHandle As KernelObject
            Dim StdErrorHandle As KernelObject
            Using OutputPipe As New Pipe()
                m_StreamRecorder = New StreamRecorder(OutputPipe.GetReadStream(), 4096, AddressOf StdErrorCompletion, Nothing)
                StdOutputHandle = OutputPipe.GetWriteHandle()
                StdErrorHandle = OutputPipe.GetWriteHandle()
            End Using

            m_Result = New CompilerExecuteeResult()
            m_Result.State = State
            m_Trigger = New MiniTrigger(1, 1, _
                Sub()
                    Completion.Invoke(m_Result)
                End Sub)

            FinalConstruct(WatchDog, ProcessMonitor, _
                Compiler.ApplicationName, Compiler.CommandLine, Compiler.EnvironmentVariables, CompilerInstance.WorkingDirectory, _
                Nothing, StdOutputHandle, StdErrorHandle, Compiler.TimeQuota, Compiler.MemoryQuota, Compiler.ActiveProcessQuota, False, _
                AddressOf ProcessExecuteeCompletion, CompilerInstance)
        End Sub

        Private Sub StdErrorCompletion(ByVal Result As StreamRecorder.Result)
            If Result.Buffer.Length <> 0 Then
                m_Result.StdErrorMessage = Encoding.Default.GetString(Result.Buffer)
            End If
            m_Trigger.InvokeNonCritical()
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
                    CompilerInstance.Dispose()
                End Try
            End If
            m_Trigger.InvokeCritical()
        End Sub

        Public Overrides Sub Execute()
            m_StreamRecorder.Start()
            MyBase.Execute()
        End Sub
    End Class
End Namespace
