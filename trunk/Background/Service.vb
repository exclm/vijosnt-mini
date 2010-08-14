Imports VijosNT.Compiling
Imports VijosNT.Executing
Imports VijosNT.LocalDb
Imports VijosNT.Testing
Imports VijosNT.Utility

Namespace Background
    Friend Class Service
        Implements IDisposable

        Private m_TempPathServer As TempPathServer
        Private m_WatchDog As WatchDog
        Private m_ProcessMonitor As ProcessMonitor
        Private m_Executor As Executor

        Public Sub New()
            SetWorkerThreads(Config.ExecutorSlots * 4, Config.ExecutorSlots * 8)
            m_WatchDog = New WatchDog()
            m_ProcessMonitor = New ProcessMonitor()
            m_TempPathServer = New TempPathServer()
            m_Executor = New Executor(Config.ExecutorSlots, New EnvironmentPoolBase() { _
                New TrustedEnvironmentPool(), New UntrustedEnvironmentPool(GetUntrustedEnvironments())})
            m_WatchDog.Start()
            m_ProcessMonitor.Start()
            ' TODO: Compiler list & Test suite mapping list
        End Sub

        Private Function GetUntrustedEnvironments() As IEnumerable(Of UntrustedEnvironment)
            Dim Result As New List(Of UntrustedEnvironment)
            Using Reader As IDataReader = UntrustedEnvironments.GetAll()
                While Reader.Read()
                    Result.Add(New UntrustedEnvironment(Reader("DesktopName"), Reader("UserName"), Reader("Password")))
                End While
            End Using
            Return Result
        End Function

        Public Sub Entry()
            WriteStartupLog()
            Console.WriteLine("Press any key to exit")
            Console.ReadKey()
        End Sub

        Private Sub WriteStartupLog()
            Dim Builder As New StringBuilder()
            Builder.AppendLine("数据库引擎: " & Database.EngineVersion)
            Log.Add(LogLevel.Information, "VijosNT 启动成功", Builder.ToString())
        End Sub

        Public Sub Queue(ByVal Compiler As Compiler, ByVal SourceCode As Stream, ByVal Completion As CompilerExecuteeCompletion, ByVal State As Object)
            m_Executor.Queue(New CompilerExecutee(m_WatchDog, m_ProcessMonitor, m_TempPathServer, Compiler, SourceCode, Completion, State))
        End Sub

        Public Sub Queue(ByVal TargetInstance As TargetInstance, ByVal TestCase As TestCase, ByVal Completion As TestCaseExecuteeCompletion, ByVal State As Object)
            m_Executor.Queue(New TestCaseExecutee(m_WatchDog, m_ProcessMonitor, TargetInstance, TestCase, Completion, State))
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_Executor.Dispose()
                    m_TempPathServer.Dispose()
                    m_ProcessMonitor.Stop()
                    m_WatchDog.Stop()
                End If
            End If
            Me.disposedValue = True
        End Sub

        ' Visual Basic 添加此代码是为了正确实现可处置模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace
