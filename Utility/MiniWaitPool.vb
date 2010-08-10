Namespace Utility
    Friend Class MiniWaitPool
        Implements IDisposable

        Public Delegate Sub WaitPoolCallback(ByVal Result As Result)

        Public Structure Result
            Public Sub New(ByVal State As Object, ByVal Timeouted As Boolean)
                Me.State = State
                Me.Timeouted = Timeouted
            End Sub

            Dim State As Object
            Dim Timeouted As Boolean
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

        Protected m_SyncRoot As Object
        Protected m_Thread As Thread
        Protected m_Event As AutoResetEvent
        Protected m_List As List(Of Entry)
        Protected m_Stopped As Boolean

        Public Sub New()
            m_SyncRoot = New Object()
            m_Thread = New Thread(AddressOf ThreadEntry)
            m_Event = New AutoResetEvent(False)
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

        Public Sub SetWait(ByVal WaitHandle As WaitHandle, ByVal TimeoutValue As Nullable(Of Int64), ByVal Callback As WaitPoolCallback, ByVal State As Object)
            Dim TimeoutTick As Nullable(Of Int64)

            If TimeoutValue IsNot Nothing Then
                TimeoutTick = Date.Now.Ticks + TimeoutValue
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
            If Entry.Callback IsNot Nothing Then
                Try
                    Entry.Callback.Invoke(New Result(Entry.CallbackState, Timeouted))
                Catch ex As Exception
                    ' eat it
                End Try
            End If

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
End Namespace
