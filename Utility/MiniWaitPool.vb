Friend Class MiniWaitPool
    Public Delegate Sub WaitPoolCallback(ByVal Result As Result)

    Public Structure Result
        Public Sub New(ByVal Timeouted As Boolean, ByVal State As Object)
            Me.Timeouted = Timeouted
            Me.State = State
        End Sub

        Dim Timeouted As Boolean
        Dim State As Object
    End Structure

    Protected Structure Entry
        Public Sub New(ByVal WaitHandle As WaitHandle, ByVal TimeoutTick As Nullable(Of Int64), ByVal Callback As WaitPoolCallback, ByVal CallbackState As Object)
            Me.WaitHandle = WaitHandle
            Me.TimeoutTick = TimeoutTick
            Me.Callback = Callback
            Me.CallbackState = CallbackState
        End Sub

        Dim WaitHandle As WaitHandle
        Dim TimeoutTick As Nullable(Of Int64)
        Dim Callback As WaitPoolCallback
        Dim CallbackState As Object
    End Structure

    Protected m_Thread As Thread
    Protected m_Event As AutoResetEvent
    Protected m_SyncRoot As Object
    Protected m_List As List(Of Entry)
    Protected m_Stopped As Boolean

    Public Sub New()
        m_Thread = New Thread(AddressOf ThreadEntry)
        m_Event = New AutoResetEvent(False)
        m_SyncRoot = New Object()
        m_List = New List(Of Entry)
        m_Stopped = False

        m_List.Add(New Entry(m_Event, Nothing, AddressOf InternalCallback, Nothing))
    End Sub

    Public Sub Start()
        m_Thread.Start()
    End Sub

    Public Sub [Stop]()
        SyncLock m_SyncRoot
            m_Stopped = True
        End SyncLock
        m_Event.Set()
    End Sub

    Public Sub Add(ByVal WaitHandle As WaitHandle, ByVal TimeoutValue As Int32, ByVal Callback As WaitPoolCallback, ByVal State As Object)
        Dim TimeoutTick As Nullable(Of Int64)

        If TimeoutValue <> Timeout.Infinite Then
            TimeoutTick = Date.Now.Ticks + Math.BigMul(TimeoutValue, 10000)
        Else
            TimeoutTick = Nothing
        End If

        SyncLock m_SyncRoot
            m_List.Add(New Entry(WaitHandle, TimeoutTick, Callback, State))
        End SyncLock
        m_Event.Set()
    End Sub

    Protected Sub InternalCallback(ByVal Result As Result)
        SyncLock m_SyncRoot
            If Not m_Stopped Then
                m_List.Add(New Entry(m_Event, Nothing, AddressOf InternalCallback, Nothing))
            End If
        End SyncLock
    End Sub

    Protected Sub DispatchEntry(ByVal Entry As Entry, ByVal Timeouted As Boolean)
        Try
            Entry.Callback.Invoke(New Result(Timeouted, Entry.CallbackState))
        Catch ex As Exception
            ' eat it
        End Try

        SyncLock m_SyncRoot
            m_List.Remove(Entry)
        End SyncLock
    End Sub

    Protected Sub ThreadEntry()
        While True
            Dim Entries As Entry()

            SyncLock m_SyncRoot
                Entries = m_List.ToArray()
            End SyncLock

            If Entries.Length = 0 Then Exit While

            Dim TimeoutTick As Nullable(Of Int64) = Nothing
            Dim TimeoutSource As Int32 = -1
            Dim WaitHandles As WaitHandle() = New WaitHandle(0 To Entries.Length - 1) {}

            For Index As Int32 = 0 To Entries.Length - 1
                WaitHandles(Index) = Entries(Index).WaitHandle
                Dim Tick As Nullable(Of Int64) = Entries(Index).TimeoutTick
                If Tick IsNot Nothing AndAlso (TimeoutTick Is Nothing OrElse Tick < TimeoutTick) Then
                    TimeoutTick = Tick
                    TimeoutSource = Index
                End If
            Next

            Dim TimeoutValue As Int32 = Timeout.Infinite

            If TimeoutTick IsNot Nothing Then
                TimeoutValue = CType((TimeoutTick - Date.Now.Ticks) \ 10000, Int32)
                If TimeoutValue < 0 Then
                    DispatchEntry(Entries(TimeoutSource), True)
                    Continue While
                End If
            End If

            Dim Source As Int32 = WaitHandle.WaitAny(WaitHandles, TimeoutValue)

            If Source <> WaitHandle.WaitTimeout Then
                DispatchEntry(Entries(Source), False)
            Else
                DispatchEntry(Entries(TimeoutSource), True)
            End If
        End While
    End Sub
End Class
