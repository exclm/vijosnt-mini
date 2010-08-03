Public Class MiniWaitPool
    ' TODO: Use GetTickCount64() instead of Date

    Public Delegate Sub WaitPoolCallback(ByVal State As Object, ByVal Timeouted As Boolean)

    Protected Structure WaitPoolEntry
        Public Sub New(ByVal WaitHandle As WaitHandle, ByVal Timeout As Nullable(Of Date), ByVal Callback As WaitPoolCallback, ByVal CallbackState As Object)
            Me.WaitHandle = WaitHandle
            Me.Timeout = Timeout
            Me.Callback = Callback
            Me.CallbackState = CallbackState
        End Sub

        Dim WaitHandle As WaitHandle
        Dim Timeout As Nullable(Of Date)
        Dim Callback As WaitPoolCallback
        Dim CallbackState As Object
    End Structure

    Protected m_Thread As Thread
    Protected m_Event As AutoResetEvent
    Protected m_SyncRoot As Object
    Protected m_List As List(Of WaitPoolEntry)
    Protected m_Stopped As Boolean

    Public Sub New()
        m_Thread = New Thread(AddressOf ThreadEntry)
        m_Event = New AutoResetEvent(False)
        m_SyncRoot = New Object()
        m_List = New List(Of WaitPoolEntry)
        m_Stopped = False

        m_List.Add(New WaitPoolEntry(m_Event, Nothing, AddressOf InternalCallback, Nothing))
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
        Dim TimeoutDate As Nullable(Of Date)

        If TimeoutValue <> Timeout.Infinite Then
            TimeoutDate = Date.Now.Add(New TimeSpan(CType(TimeoutValue, Int64) * 10000))
        Else
            TimeoutDate = Nothing
        End If

        SyncLock m_SyncRoot
            m_List.Add(New WaitPoolEntry(WaitHandle, TimeoutDate, Callback, State))
        End SyncLock
        m_Event.Set()
    End Sub

    Protected Sub InternalCallback()
        SyncLock m_SyncRoot
            If Not m_Stopped Then
                m_List.Add(New WaitPoolEntry(m_Event, Nothing, AddressOf InternalCallback, Nothing))
            End If
        End SyncLock
    End Sub

    Protected Sub DispatchEntry(ByVal Entry As WaitPoolEntry, ByVal Timeouted As Boolean)
        Try
            Entry.Callback.Invoke(Entry.CallbackState, Timeouted)
        Catch ex As Exception
            ' eat it
        End Try

        SyncLock m_SyncRoot
            m_List.Remove(Entry)
        End SyncLock
    End Sub

    Protected Sub ThreadEntry()
        While True
            Dim Entries As WaitPoolEntry()

            SyncLock m_SyncRoot
                Entries = m_List.ToArray()
            End SyncLock

            If Entries.Length = 0 Then Exit While

            Dim TimeoutDate As Nullable(Of Date) = Nothing
            Dim TimeoutSource As Int32 = -1
            Dim WaitHandles As WaitHandle() = New WaitHandle(0 To Entries.Length - 1) {}

            For Index As Int32 = 0 To Entries.Length - 1
                WaitHandles(Index) = Entries(Index).WaitHandle
                Dim Value As Nullable(Of Date) = Entries(Index).Timeout
                If Value IsNot Nothing AndAlso (TimeoutDate Is Nothing OrElse Value < TimeoutDate) Then
                    TimeoutDate = Value
                    TimeoutSource = Index
                End If
            Next

            Dim TimeoutValue As Int32 = Timeout.Infinite

            If TimeoutDate IsNot Nothing Then
                TimeoutValue = CType((TimeoutDate.Value - Now).TotalMilliseconds, Int32)
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
