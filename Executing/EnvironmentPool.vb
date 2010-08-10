Namespace Executing
    Friend MustInherit Class EnvironmentPool
        Public MustOverride ReadOnly Property Tag() As EnvironmentTag
        Public MustOverride Function Take() As Environment

        Protected m_PendingExecutees As Queue(Of Executee)
        Protected m_Executor As Executor

        Public Sub New()
            m_PendingExecutees = New Queue(Of Executee)
        End Sub

        Public Property Executor() As Executor
            Get
                Return m_Executor
            End Get

            Set(ByVal Value As Executor)
                m_Executor = Value
            End Set
        End Property

        Protected Overridable Sub UntakeInternal(ByVal Environment As Environment)
            m_Executor.Untake()
        End Sub

        Public Sub Untake(ByVal Environment As Environment)
            SyncLock m_PendingExecutees
                If m_PendingExecutees.Count <> 0 Then
                    Dim Executee As Executee = m_PendingExecutees.Dequeue()
                    Executee.Environment = Environment
                    Executee.Execute()
                Else
                    UntakeInternal(Environment)
                End If
            End SyncLock
        End Sub

        Public Sub Queue(ByVal Executee As Executee)
            Debug.Assert(Executee.RequiredEnvironment = Me.Tag)

            Dim Environment As Environment = Me.Take()
            If Environment IsNot Nothing Then
                Executee.Environment = Environment
                Executee.Execute()
            Else
                SyncLock m_PendingExecutees
                    m_PendingExecutees.Enqueue(Executee)
                End SyncLock
            End If
        End Sub
    End Class
End Namespace
