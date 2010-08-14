Namespace Executing
    Friend MustInherit Class EnvironmentPoolBase
        Implements IDisposable

        Public MustOverride ReadOnly Property Tag() As EnvironmentTag
        Public MustOverride Function Take() As EnvironmentBase

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

        Protected Overridable Sub UntakeInternal(ByVal Environment As EnvironmentBase)
            ' Do nothing
        End Sub

        Public Sub Untake(ByVal Environment As EnvironmentBase)
            SyncLock m_PendingExecutees
                If m_PendingExecutees.Count <> 0 Then
                    Dim Executee As Executee = m_PendingExecutees.Dequeue()
                    Executee.Environment = Environment
                    Executee.Execute()
                Else
                    UntakeInternal(Environment)
                End If

                ' N.B. The executor reference stands for queuing so we should untake anyway
                m_Executor.Untake()
            End SyncLock
        End Sub

        Public Sub Queue(ByVal Executee As Executee)
            Debug.Assert(Executee.RequiredEnvironment = Me.Tag)

            Dim Environment As EnvironmentBase = Me.Take()
            If Environment IsNot Nothing Then
                Executee.Environment = Environment
                Executee.Execute()
            Else
                SyncLock m_PendingExecutees
                    m_PendingExecutees.Enqueue(Executee)
                End SyncLock
            End If
        End Sub

#Region "IDisposable Support"
        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            ' Do nothing
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
