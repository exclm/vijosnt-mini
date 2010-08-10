Namespace Executing
    Friend Class Executor
        Protected m_AvailableSlots As Int32
        Protected m_Pools As Dictionary(Of EnvironmentTag, EnvironmentPool)
        Protected m_PendingExecutees As Queue(Of Executee)

        Public Sub New(ByVal Slots As Int32, ByVal Pools As IEnumerable(Of EnvironmentPool))
            m_AvailableSlots = Slots
            m_Pools = New Dictionary(Of EnvironmentTag, EnvironmentPool)()
            m_PendingExecutees = New Queue(Of Executee)()

            For Each Pool In Pools
                Pool.Executor = Me
                m_Pools.Add(Pool.Tag, Pool)
            Next
        End Sub

        Public Function Take() As Boolean
            SyncLock Me
                Debug.Assert(m_AvailableSlots >= 0)

                If m_AvailableSlots <> 0 Then
                    m_AvailableSlots -= 1
                    Return True
                Else
                    Return False
                End If
            End SyncLock
        End Function

        Public Sub Untake()
            SyncLock Me
                If m_PendingExecutees.Count <> 0 Then
                    Dim Executee As Executee = m_PendingExecutees.Dequeue()
                    m_Pools(Executee.RequiredEnvironment).Queue(Executee)
                Else
                    m_AvailableSlots += 1
                End If
            End SyncLock
        End Sub

        Public Sub Queue(ByVal Executee As Executee)
            If Take() Then
                m_Pools(Executee.RequiredEnvironment).Queue(Executee)
            Else
                SyncLock Me
                    m_PendingExecutees.Enqueue(Executee)
                End SyncLock
            End If
        End Sub
    End Class
End Namespace
