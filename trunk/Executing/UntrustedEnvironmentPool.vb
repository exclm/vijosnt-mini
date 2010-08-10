Namespace Executing
    Friend Class UntrustedEnvironmentPool
        Inherits EnvironmentPool

        Protected m_Stack As Stack(Of UntrustedEnvironment)

        Public Sub New(ByVal Environments As IEnumerable(Of UntrustedEnvironment))
            m_Stack = New Stack(Of UntrustedEnvironment)

            For Each Environment In Environments
                Environment.Pool = Me
                m_Stack.Push(Environment)
            Next
        End Sub

        Public Overrides ReadOnly Property Tag() As EnvironmentTag
            Get
                Return EnvironmentTag.Untrusted
            End Get
        End Property

        Public Overrides Function Take() As Environment
            SyncLock Me
                If m_Stack.Count = 0 Then
                    Return Nothing
                Else
                    Return m_Stack.Pop()
                End If
            End SyncLock
        End Function

        Protected Overrides Sub UntakeInternal(ByVal Environment As Environment)
            SyncLock Me
                m_Stack.Push(Environment)
            End SyncLock
            MyBase.UntakeInternal(Environment)
        End Sub
    End Class
End Namespace
