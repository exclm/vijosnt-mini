Namespace Executing
    Friend Class Executor
        Implements IDisposable

        Protected m_SyncRoot As Object
        Protected m_AvailableSlots As Int32
        Protected m_Pools As Dictionary(Of EnvironmentTag, EnvironmentPool)
        Protected m_PendingExecutees As Queue(Of Executee)

        Public Sub New(ByVal Slots As Int32, ByVal Pools As IEnumerable(Of EnvironmentPool))
            m_SyncRoot = New Object()
            m_AvailableSlots = Slots
            m_Pools = New Dictionary(Of EnvironmentTag, EnvironmentPool)()
            m_PendingExecutees = New Queue(Of Executee)()

            For Each Pool In Pools
                Pool.Executor = Me
                m_Pools.Add(Pool.Tag, Pool)
            Next
        End Sub

        Public Function Take() As Boolean
            SyncLock m_SyncRoot
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
            SyncLock m_SyncRoot
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
                SyncLock m_SyncRoot
                    m_PendingExecutees.Enqueue(Executee)
                End SyncLock
            End If
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    For Each Pool As EnvironmentPool In m_Pools.Values
                        Pool.Dispose()
                    Next

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
