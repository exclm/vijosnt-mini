Friend Class WatchDog
    Public Delegate Sub WatchDogCallback(ByVal Result As Result)

    Public Structure Result
        Public Sub New(ByVal State As Object, ByVal QuotaUsage As Int64)
            Me.State = State
            Me.QuotaUsage = QuotaUsage
        End Sub

        Dim State As Object
        Dim QuotaUsage As Int64
    End Structure

    Protected Structure Context
        Public Sub New(ByVal Process As ProcessEx, ByVal TimeQuota As Int64, ByVal Callback As WatchDogCallback, ByVal CallbackState As Object)
            Me.Process = Process
            Me.TimeQuota = TimeQuota
            Me.Callback = Callback
            Me.CallbackState = CallbackState
        End Sub

        Dim Process As ProcessEx
        Dim TimeQuota As Int64
        Dim Callback As WatchDogCallback
        Dim CallbackState As Object
    End Structure

    Protected m_WaitPool As MiniWaitPool

    Public Sub WatchDog()
        m_WaitPool = New MiniWaitPool()
    End Sub

    Public Sub Start()
        m_WaitPool.Start()
    End Sub

    Public Sub [Stop]()
        m_WaitPool.Stop()
    End Sub

    Protected Sub SetWatchCallback(ByVal Result As MiniWaitPool.Result)
        Dim Context As Context = DirectCast(Result.State, Context)

        If Not Result.Timeouted Then
            ' The waited process was ended, fire the callback
            Context.Callback.Invoke(New Result(Context.CallbackState, Context.Process.AliveTime))
        Else
            ' The waited process is still running, set watch again
            SetWatchInternal(Context)
        End If
    End Sub

    Protected Sub SetWatchInternal(ByVal Context As Context)
        Dim AliveTime As Int64 = Context.Process.AliveTime

        If AliveTime > Context.TimeQuota Then
            ' The waited process exceeds the time quota, terminate it and fire the callback
            Context.Process.Kill(ERROR_NOT_ENOUGH_QUOTA)
            Context.Callback.Invoke(New Result(Context.CallbackState, Context.Process.AliveTime))
        Else
            ' There is still time quota remaining, set up the wait pool
            m_WaitPool.SetWait(Context.Process, Context.TimeQuota - AliveTime, AddressOf SetWatchCallback, Context)
        End If
    End Sub

    Public Sub SetWatch(ByVal Process As ProcessEx, ByVal TimeQuota As Int64, ByVal Callback As WatchDogCallback, ByVal State As Object)
        SetWatchInternal(New Context(Process, TimeQuota, Callback, State))
    End Sub
End Class
