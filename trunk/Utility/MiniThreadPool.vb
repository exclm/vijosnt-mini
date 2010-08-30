Namespace Utility
    Friend Class MiniThreadPool
        Public Delegate Sub Completion(ByVal State As Object)

        Private Class TaskContext
            Implements IDisposable

            Public Sub New(ByVal Completion As Completion, ByVal State As Object)
                m_Completion = Completion
                m_State = State
                m_Event = New ManualResetEvent(False)
            End Sub

            Public Function Invoke() As Boolean
                If m_Completion IsNot Nothing Then
                    m_Event.Set()
                    m_Completion.Invoke(m_State)
                    Return True
                Else
                    Return False
                End If
            End Function

            Public Sub WaitForStart()
                m_Event.WaitOne()
            End Sub

            Private m_Completion As Completion
            Private m_State As Object
            Private m_Event As ManualResetEvent

#Region "IDisposable Support"
            Private disposedValue As Boolean ' 检测冗余的调用

            ' IDisposable
            Protected Overridable Sub Dispose(ByVal disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        m_Event.Close()
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

        Private Class ThreadContext
            Public Sub New()
                m_Event = New AutoResetEvent(False)
            End Sub

            Public Function WaitForTask(ByVal Timeout As Int32) As TaskContext
                If m_Event.WaitOne(Timeout, False) Then
                    Return m_Task
                Else
                    Return Nothing
                End If
            End Function

            Public Sub SetTask(ByVal Task As TaskContext)
                m_Task = Task
                m_Event.Set()
            End Sub

            Dim m_Event As AutoResetEvent
            Dim m_Task As TaskContext
        End Class

        Private Shared m_ThreadStack As Stack(Of ThreadContext)
        Private Const m_ThreadTimeout As Int32 = 60000

        Shared Sub New()
            m_ThreadStack = New Stack(Of ThreadContext)()
        End Sub

        Public Shared Sub Queue(ByVal Completion As Completion, ByVal State As Object)
            Using TaskContext As New TaskContext(Completion, State)
                Dim ThreadContext As ThreadContext
                SyncLock m_ThreadStack
                    If m_ThreadStack.Count = 0 Then
                        ThreadContext = Nothing
                    Else
                        ThreadContext = m_ThreadStack.Pop()
                    End If
                End SyncLock
                If ThreadContext IsNot Nothing Then
                    ThreadContext.SetTask(TaskContext)
                Else
                    Dim Thread As New Thread(AddressOf ThreadEntry)
                    Thread.IsBackground = True
                    Thread.Start(TaskContext)
                End If
                If Completion IsNot Nothing Then _
                    TaskContext.WaitForStart()
            End Using
        End Sub

        Private Shared Sub ThreadEntry(ByVal Task As TaskContext)
            Dim ThreadContext As New ThreadContext()
            Do While Task.Invoke()
                SyncLock m_ThreadStack
                    m_ThreadStack.Push(ThreadContext)
                End SyncLock
                While True
                    Task = ThreadContext.WaitForTask(m_ThreadTimeout)
                    If Task Is Nothing Then
                        Queue(Nothing, Nothing)
                    Else
                        Exit While
                    End If
                End While
            Loop
        End Sub
    End Class
End Namespace
