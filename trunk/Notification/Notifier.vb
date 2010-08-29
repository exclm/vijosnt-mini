Namespace Notification
    Friend Class Notifier
        Private Shared m_Handlers As Dictionary(Of String, List(Of NotifyHandler))

        Shared Sub New()
            m_Handlers = New Dictionary(Of String, List(Of NotifyHandler))
        End Sub

        Public Shared Sub Register(ByVal Key As String, ByVal Handler As NotifyHandler)
            Dim List As List(Of NotifyHandler)
            SyncLock m_Handlers
                If m_Handlers.ContainsKey(Key) Then
                    List = m_Handlers(Key)
                Else
                    List = New List(Of NotifyHandler)
                    m_Handlers.Add(Key, List)
                End If
            End SyncLock
            SyncLock List
                List.Add(Handler)
            End SyncLock
        End Sub

        Public Shared Sub Unregister(ByVal Key As String, ByVal Handler As NotifyHandler)
            Dim List As List(Of NotifyHandler) = Nothing
            SyncLock m_Handlers
                If m_Handlers.ContainsKey(Key) Then
                    List = m_Handlers(Key)
                End If
            End SyncLock
            If List IsNot Nothing Then
                SyncLock List
                    List.Remove(Handler)
                End SyncLock
            End If
        End Sub

        Public Shared Sub Invoke(ByVal Key As String, ByVal Param As Object)
            Dim List As List(Of NotifyHandler) = Nothing
            SyncLock m_Handlers
                If m_Handlers.ContainsKey(Key) Then
                    List = m_Handlers(Key)
                End If
            End SyncLock
            If List IsNot Nothing Then
                SyncLock List
                    For Each Handler As NotifyHandler In List
                        Handler.Invoke(Param)
                    Next
                End SyncLock
            End If
        End Sub
    End Class
End Namespace
