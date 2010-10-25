Imports VijosNT.Compiling
Imports VijosNT.Executing
Imports VijosNT.Testing
Imports VijosNT.Utility
Imports VijosNT.Win32

Namespace Feeding
    Friend Class Runner
        Implements IDisposable

        Private Shared s_Instance As Runner

        Shared Sub New()
            s_Instance = New Runner()
        End Sub

        Public Shared Function Singleton() As Runner
            Return s_Instance
        End Function

        Private Class TestContext
            Public Completion As TestCompletion
            Public Compiler As Compiler
            Public TestCases As IEnumerable(Of TestCase)
            Public Flag As TestResultFlag
            Public Score As Int32
            Public TimeUsage As Int64
            Public MemoryUsage As Int64
            Public TestResults As SortedDictionary(Of Int32, TestResultEntry)
            Public Remaining As Int32
            Public Warning As String
        End Class

        Private Class TestCaseContext
            Public TestContext As TestContext
            Public TestCase As TestCase
        End Class

        Private m_WatchDog As WatchDog
        Private m_ProcessMonitor As ProcessMonitor
        Private m_Executor As Executor
        Private m_TestSuitePool As TestSuitePool
        Private m_DataSourcePool As DataSourcePool
        Private m_Running As Int32
        Private m_CanExit As ManualResetEvent
        Private m_AllowQueuing As Boolean

        Private Sub New()
            m_WatchDog = New WatchDog()
            m_ProcessMonitor = New ProcessMonitor()
            TempPathServer.Singleton()
            m_Executor = New Executor()
            m_WatchDog.Start()
            m_ProcessMonitor.Start()
            CompilerPool.Singleton()
            m_TestSuitePool = New TestSuitePool()
            m_DataSourcePool = New DataSourcePool(Me)
            m_Running = 0
            m_CanExit = New ManualResetEvent(True)
            m_AllowQueuing = True
        End Sub

        Public Sub ReloadCompiler()
            CompilerPool.Singleton.Reload()
        End Sub

        Public Sub ReloadTestSuite()
            m_TestSuitePool.Reload()
        End Sub

        Public Sub ReloadExecutor()
            m_AllowQueuing = False
            m_CanExit.WaitOne()
            m_Executor.Dispose()
            m_Executor = New Executor()
            m_AllowQueuing = True
        End Sub

        Public Sub ReloadDataSource()
            m_DataSourcePool.Reload()
        End Sub

        Private Structure FeedContext
            Dim Source As DataSourceBase
            Dim Id As Int32
        End Structure

        Public Function Queue(ByVal [Namespace] As String, ByVal FileName As String, ByVal SourceCode As Stream, ByVal Completion As TestCompletion) As Boolean
            If Not m_AllowQueuing Then _
                Return False

            m_CanExit.Reset()
            If Interlocked.Increment(m_Running) = 1 Then
                RaiseEvent RunnerStatusChanged(True)
            End If

            If Not m_AllowQueuing Then
                If Interlocked.Decrement(m_Running) = 0 Then
                    RaiseEvent RunnerStatusChanged(False)
                    m_CanExit.Set()
                End If
                Return False
            End If

            Dim Context As New TestContext()
            Context.Completion = Completion
            Context.Compiler = CompilerPool.Singleton.TryGet(Path.GetExtension(FileName))
            Context.TestCases = m_TestSuitePool.TryLoad([Namespace], Path.GetFileNameWithoutExtension(FileName))
            Context.Flag = TestResultFlag.None
            Context.Score = 0
            Context.TimeUsage = 0
            Context.MemoryUsage = 0
            Context.TestResults = New SortedDictionary(Of Int32, TestResultEntry)()

            If Context.Compiler Is Nothing Then
                Context.Completion.Invoke(New TestResult(TestResultFlag.CompilerNotFound, Nothing, 0, 0, 0, Nothing))
                If Interlocked.Decrement(m_Running) = 0 Then
                    RaiseEvent RunnerStatusChanged(False)
                    m_CanExit.Set()
                End If
                Return True
            End If

            If Context.TestCases Is Nothing Then
                Context.Completion.Invoke(New TestResult(TestResultFlag.TestSuiteNotFound, Nothing, 0, 0, 0, Nothing))
                If Interlocked.Decrement(m_Running) = 0 Then
                    RaiseEvent RunnerStatusChanged(False)
                    m_CanExit.Set()
                End If
                Return True
            End If

            With Context.Compiler
                If .ApplicationName Is Nothing OrElse .ApplicationName.Length = 0 Then
                    Dim Result As New CompilerExecuteeResult
                    Result.State = Context
                    Result.ExitStatus = NTSTATUS.STATUS_SUCCESS
                    Result.Target = .CreateInstance(SourceCode).OpenTarget()
                    TestCompileCompletion(Result)
                Else
                    Try
                        m_Executor.Queue(New CompilerExecutee(m_WatchDog, m_ProcessMonitor, Context.Compiler, SourceCode, AddressOf TestCompileCompletion, Context))
                    Catch ex As Exception
                        Context.Completion.Invoke(New TestResult(TestResultFlag.InternalError, ex.ToString(), 0, 0, 0, Nothing))
                        If Interlocked.Decrement(m_Running) = 0 Then
                            RaiseEvent RunnerStatusChanged(False)
                            m_CanExit.Set()
                        End If
                        Return True
                    End Try
                End If
            End With

            Return True
        End Function

        Private Function FormatException(ByVal Exception As EXCEPTION_RECORD) As String
            Dim Builder As New StringBuilder()
            Select Case Exception.ExceptionCode
                Case ExceptionCode.EXCEPTION_ACCESS_VIOLATION
                    Select Case Exception.ExceptionInformation(0)
                        Case 0
                            Builder.Append("读取")
                        Case 1
                            Builder.Append("写入")
                        Case 8
                            Builder.Append("执行")
                    End Select
                    Builder.Append("访问违规, 地址: 0x")
                    Builder.Append(Exception.ExceptionInformation(1).ToString("x8"))
                Case ExceptionCode.EXCEPTION_INT_DIVIDE_BY_ZERO
                    Builder.Append("整形运算除数为零")
                Case ExceptionCode.EXCEPTION_FLT_DIVIDE_BY_ZERO
                    Builder.Append("浮点运算除数为零")
                Case ExceptionCode.EXCEPTION_STACK_OVERFLOW
                    Builder.Append("堆栈溢出")
                Case ExceptionCode.EXCEPTION_FLT_OVERFLOW
                    Builder.Append("浮点运算溢出")
                Case Else
                    Builder.Append("异常代码: ")
                    Builder.Append(Exception.ExceptionCode.ToString())
            End Select
            Return Builder.ToString()
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
                Warning = FormatException(Result.Exception.Value)
            ElseIf Result.ExitStatus <> Win32.NTSTATUS.STATUS_SUCCESS Then
                Warning = "编译失败, 返回值为 " & DirectCast(Result.ExitStatus.Value, Int32).ToString()
                If Result.StdErrorMessage Is Nothing Then
                    Warning += ", 编译器无输出"
                Else
                    Warning += ", 编译器输出如下" & vbCrLf & Result.StdErrorMessage
                End If
            ElseIf Result.Target Is Nothing Then
                Warning = "编译器出现未知错误"
            Else
                Context.Warning = Result.StdErrorMessage
                Context.Remaining = 1
                For Each TestCase In Context.TestCases
                    Interlocked.Increment(Context.Remaining)
                    Dim TestCaseContext As New TestCaseContext()
                    TestCaseContext.TestContext = Context
                    TestCaseContext.TestCase = TestCase
                    m_Executor.Queue(New TestCaseExecutee(m_WatchDog, m_ProcessMonitor, Result.Target.CreateInstance(), TestCase, AddressOf TestExecuteCompletion, TestCaseContext))
                Next
                Result.Target.Dispose()
                TestWorkCompleted(Context)
                Return
            End If

            ' Compile failed
            Context.Completion.Invoke(New TestResult(TestResultFlag.CompileError, Warning, 0, 0, 0, Nothing))
            If Interlocked.Decrement(m_Running) = 0 Then
                RaiseEvent RunnerStatusChanged(False)
                m_CanExit.Set()
            End If
        End Sub

        Private Sub TestExecuteCompletion(ByVal Result As TestCaseExecuteeResult)
            Dim Context As TestCaseContext = DirectCast(Result.State, TestCaseContext)
            Dim Entry As New TestResultEntry()

            Entry.Index = Result.Index
            Entry.Score = 0

            If Not Result.ExitStatus.HasValue Then
                Entry.Flag = TestResultFlag.RuntimeError
                Entry.Warning = "创建进程失败"
            Else
                If Result.Exception.HasValue Then
                    Entry.Flag = TestResultFlag.RuntimeError
                    Entry.Warning = FormatException(Result.Exception.Value)
                Else
                    If Result.StdErrorMessage IsNot Nothing Then
                        Entry.Warning = "标准错误输出如下" & vbCrLf & Result.StdErrorMessage
                    Else
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
                End If
            End If
            Entry.TimeUsage = Result.TimeQuotaUsage
            Entry.MemoryUsage = Result.MemoryQuotaUsage

            SyncLock Context.TestContext
                With Context.TestContext
                    .TestResults.Add(Entry.Index, Entry)
                    .Score += Entry.Score
                    If Entry.Flag = TestResultFlag.Accepted Then
                        .TimeUsage += Entry.TimeUsage
                        .MemoryUsage = Math.Max(.MemoryUsage, Entry.MemoryUsage)
                    End If
                    If Entry.Flag > .Flag Then
                        .Flag = Entry.Flag
                    End If
                End With
            End SyncLock

            TestWorkCompleted(Context.TestContext)
        End Sub

        Private Sub TestWorkCompleted(ByVal Context As TestContext)
            If Interlocked.Decrement(Context.Remaining) = 0 Then
                Context.Completion.Invoke(New TestResult(Context.Flag, Context.Warning, Context.Score, Context.TimeUsage, Context.MemoryUsage, Context.TestResults.Values))
                If Interlocked.Decrement(m_Running) = 0 Then
                    RaiseEvent RunnerStatusChanged(False)
                    m_CanExit.Set()
                End If
            End If
        End Sub

        Public Event RunnerStatusChanged(ByVal Running As Boolean)

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_AllowQueuing = False
                    m_CanExit.WaitOne()
                    m_CanExit.Close()
                    m_DataSourcePool.Dispose()
                    m_Executor.Dispose()
                    m_ProcessMonitor.Stop()
                    m_WatchDog.Stop()
                    TempPathServer.Singleton.Dispose()
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
