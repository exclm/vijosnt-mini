Imports VijosNT.Win32

Namespace Utility
    Friend Class WatchDog
        Implements IDisposable

        Public Delegate Sub Completion(ByVal Result As Result)

        Public Structure Result
            Public Sub New(ByVal State As Object, ByVal QuotaUsage As Int64)
                Me.State = State
                Me.QuotaUsage = QuotaUsage
            End Sub

            Dim State As Object
            Dim QuotaUsage As Int64
        End Structure

        Protected Structure Context
            Public Sub New(ByVal Process As ProcessEx, ByVal TimeQuota As Nullable(Of Int64), ByVal Completion As Completion, ByVal CompletionState As Object)
                Me.Process = Process
                Me.TimeQuota = TimeQuota
                Me.Completion = Completion
                Me.CompletionState = CompletionState
            End Sub

            Dim Process As ProcessEx
            Dim TimeQuota As Nullable(Of Int64)
            Dim Completion As Completion
            Dim CompletionState As Object
        End Structure

        Protected m_WaitPool As MiniWaitPool

        Public Sub New()
            m_WaitPool = New MiniWaitPool()
        End Sub

        Public Sub Start()
            m_WaitPool.Start()
        End Sub

        Public Sub [Stop]()
            m_WaitPool.Stop()
        End Sub

        Protected Sub SetWatchCompletion(ByVal Result As MiniWaitPool.Result)
            Dim Context As Context = DirectCast(Result.State, Context)

            If Not Result.Timeouted Then
                ' The waited process was ended, fire the completion
                Context.Completion.Invoke(New Result(Context.CompletionState, Context.Process.AliveTime))
                Context.Process.Close()
            Else
                ' The waited process is still running, set watch again
                SetWatchInternal(Context)
            End If
        End Sub

        Protected Sub SetWatchInternal(ByVal Context As Context)
            Dim AliveTime As Int64 = Context.Process.AliveTime

            If Context.TimeQuota Is Nothing Then
                ' The quota is not limited, set up the wait pool
                m_WaitPool.SetWait(Context.Process, Nothing, AddressOf SetWatchCompletion, Context)
            ElseIf AliveTime >= Context.TimeQuota Then
                ' The waited process exceeds the time quota, terminate it and fire the completion
                Try
                    Context.Process.Kill(ERROR_NOT_ENOUGH_QUOTA)
                Catch ex As Exception
                    ' eat it
                End Try
                Context.Completion.Invoke(New Result(Context.CompletionState, Context.Process.AliveTime))
                Context.Process.Close()
            Else
                ' There is still time quota remaining, set up the wait pool
                m_WaitPool.SetWait(Context.Process, Context.TimeQuota - AliveTime + 50000, AddressOf SetWatchCompletion, Context)
            End If
        End Sub

        Public Sub SetWatch(ByVal Process As ProcessEx, ByVal TimeQuota As Nullable(Of Int64), ByVal Completion As Completion, ByVal State As Object)
            SetWatchInternal(New Context(Process, TimeQuota, Completion, State))
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_WaitPool.Dispose()
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