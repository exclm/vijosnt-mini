Imports VijosNT.Compiling
Imports VijosNT.Executing
Imports VijosNT.Testing
Imports VijosNT.Utility

Namespace Background
    Friend Class Runner
        Implements IDisposable

        Private Class TestContext
            Public Completion As TestCompletion
            Public CompletionState As Object
            Public Compiler As Compiler
            Public TestCases As IEnumerable(Of TestCase)
            Public Flag As TestResultFlag
            Public Score As Int32
            Public TimeUsage As Int64
            Public MemoryUsage As Int64
            Public TestResults As SortedDictionary(Of Int32, TestResultEntry)
            Public Remaining As Int32
        End Class

        Private Class TestCaseContext
            Public TestContext As TestContext
            Public TestCase As TestCase
        End Class

        Private m_TempPathServer As TempPathServer
        Private m_WatchDog As WatchDog
        Private m_ProcessMonitor As ProcessMonitor
        Private m_Executor As Executor
        Private m_CompilerPool As LocalCompilerPool
        Private m_TestSuitePool As TestSuitePool
        Private m_Running As Int32
        Private m_CanExit As ManualResetEvent
        Private m_AllowQueuing As Boolean

        Public Sub New()
            m_WatchDog = New WatchDog()
            m_ProcessMonitor = New ProcessMonitor()
            m_TempPathServer = New TempPathServer()
            m_Executor = New Executor()
            m_WatchDog.Start()
            m_ProcessMonitor.Start()
            m_CompilerPool = New LocalCompilerPool(m_TempPathServer)
            m_TestSuitePool = New TestSuitePool()
            m_Running = 0
            m_CanExit = New ManualResetEvent(True)
            m_AllowQueuing = True
        End Sub

        Public Function Queue(ByVal CompilerText As String, ByVal SourceCode As Stream, ByVal TestSuiteId As String, ByVal Completion As TestCompletion, ByVal State As Object) As Boolean
            If Not m_AllowQueuing Then _
                Return False

            m_CanExit.Reset()
            Interlocked.Increment(m_Running)

            If Not m_AllowQueuing Then
                If Interlocked.Decrement(m_Running) = 0 Then _
                    m_CanExit.Set()
                Return False
            End If

            Dim Context As New TestContext()
            Context.Completion = Completion
            Context.CompletionState = State
            Context.Compiler = m_CompilerPool.TryGet(CompilerText)
            Context.TestCases = m_TestSuitePool.TryLoad(TestSuiteId)
            Context.Flag = TestResultFlag.Null
            Context.Score = 0
            Context.TimeUsage = 0
            Context.MemoryUsage = 0
            Context.TestResults = New SortedDictionary(Of Int32, TestResultEntry)()

            If Context.Compiler Is Nothing Then
                If Context.Completion IsNot Nothing Then
                    Context.Completion.Invoke(New TestResult(Context.CompletionState, TestResultFlag.CompilerNotFound, Nothing, 0, 0, 0, Nothing))
                End If
                If Interlocked.Decrement(m_Running) = 0 Then _
                    m_CanExit.Set()
                Return True
            End If

            If Context.TestCases Is Nothing Then
                If Context.Completion IsNot Nothing Then
                    Context.Completion.Invoke(New TestResult(Context.CompletionState, TestResultFlag.TestSuiteNotFound, Nothing, 0, 0, 0, Nothing))
                End If
                If Interlocked.Decrement(m_Running) = 0 Then _
                    m_CanExit.Set()
                Return True
            End If

            m_Executor.Queue(New CompilerExecutee(m_WatchDog, m_ProcessMonitor, m_TempPathServer, Context.Compiler, SourceCode, AddressOf TestCompileCompletion, Context))
            Return True
        End Function

        Private Sub TestCompileCompletion(ByVal Result As CompilerExecuteeResult)
            Dim Context As TestContext = DirectCast(Result.State, TestContext)
            Dim Warning As String = Nothing

            If Not Result.ExitStatus.HasValue Then
                Warning = "无法创建编译器进程"
            ElseIf Context.Compiler.MemoryQuota.HasValue AndAlso Result.MemoryQuotaUsage > Context.Compiler.MemoryQuota Then
                Warning = "编译器超出内存限制"
            ElseIf Context.Compiler.TimeQuota.HasValue AndAlso Result.TimeQuotaUsage > Context.Compiler.TimeQuota Then
                Warning = "编译器超出时间限制"
            ElseIf Result.Exception.HasValue Then
                ' TODO: Compiler raised an exception
                Warning = "编译器发生异常"
            ElseIf Result.ExitStatus <> Win32.NTSTATUS.STATUS_SUCCESS Then
                ' TODO: Compile error, get information from stderr
                Warning = "编译器返回非 0 值 (编译失败)"
            ElseIf Result.Target Is Nothing Then
                Warning = "编译器出现未知错误"
            Else
                Context.Remaining = 1
                For Each TestCase In Context.TestCases
                    Interlocked.Increment(Context.Remaining)
                    Dim TestCaseContext As New TestCaseContext()
                    TestCaseContext.TestContext = Context
                    TestCaseContext.TestCase = TestCase
                    m_Executor.Queue(New TestCaseExecutee(m_WatchDog, m_ProcessMonitor, Result.Target.CreateInstance(), TestCase, AddressOf TestExecuteCompletion, TestCaseContext))
                Next
                TestWorkCompleted(Context)
                Return
            End If

            If Context.Completion IsNot Nothing Then
                Context.Completion.Invoke(New TestResult(Context.CompletionState, TestResultFlag.CompileError, Warning, 0, 0, 0, Nothing))
            End If
            If Interlocked.Decrement(m_Running) = 0 Then _
                m_CanExit.Set()
        End Sub

        Private Sub TestExecuteCompletion(ByVal Result As TestCaseExecuteeResult)
            Dim Context As TestCaseContext = DirectCast(Result.State, TestCaseContext)
            Dim Entry As TestResultEntry

            Entry.Index = Result.Index
            Entry.Score = 0

            If Not Result.ExitStatus.HasValue Then
                Entry.Flag = TestResultFlag.RuntimeError
                Entry.Warning = "创建进程失败"
            Else
                If Result.Exception.HasValue Then
                    Entry.Flag = TestResultFlag.RuntimeError
                    ' TODO: Target raised an exception, try addr2line it
                    Entry.Warning = "发生异常"
                Else
                    If Result.ExitStatus <> Win32.NTSTATUS.STATUS_SUCCESS Then
                        Entry.Warning = "程序返回值非零 (" & DirectCast(Result.ExitStatus.Value, Int32).ToString() & ")"
                    Else
                        ' TODO: Collect data from stderr and make warning
                        Entry.Warning = Nothing
                    End If

                    If Not Result.Score.HasValue Then
                        Entry.Flag = TestResultFlag.JudgerError
                    ElseIf Context.TestCase.MemoryQuota.HasValue AndAlso Result.MemoryQuotaUsage > Context.TestCase.MemoryQuota Then
                        Entry.Flag = TestResultFlag.MemoryLimitExceeded
                    ElseIf Context.TestCase.TimeQuota.HasValue AndAlso Result.TimeQuotaUsage > Context.TestCase.TimeQuota Then
                        Entry.Flag = TestResultFlag.TimeLimitExceeded
                    Else
                        Entry.Score = Result.Score
                        If Result.Score = Context.TestCase.Weight Then
                            Entry.Flag = TestResultFlag.Accepted
                        Else
                            Entry.Flag = TestResultFlag.WrongAnswer
                        End If
                    End If
                    Entry.TimeUsage = Result.TimeQuotaUsage
                    Entry.MemoryUsage = Result.MemoryQuotaUsage
                End If
            End If

            SyncLock Context.TestContext
                Context.TestContext.TestResults.Add(Entry.Index, Entry)
                Context.TestContext.Score += Entry.Score
                Context.TestContext.TimeUsage += Entry.TimeUsage
                Context.TestContext.MemoryUsage += Entry.MemoryUsage
                If Entry.Flag > Context.TestContext.Flag Then
                    Context.TestContext.Flag = Entry.Flag
                End If
            End SyncLock

            TestWorkCompleted(Context.TestContext)
        End Sub

        Private Sub TestWorkCompleted(ByVal Context As TestContext)
            If Interlocked.Decrement(Context.Remaining) = 0 Then
                If Context.Completion IsNot Nothing Then
                    Context.Completion.Invoke(New TestResult(Context.CompletionState, Context.Flag, Nothing, Context.Score, Context.TimeUsage, Context.MemoryUsage, Context.TestResults.Values))
                End If
                If Interlocked.Decrement(m_Running) = 0 Then _
                    m_CanExit.Set()
            End If
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_AllowQueuing = False
                    m_CanExit.WaitOne()
                    m_CanExit.Close()
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
