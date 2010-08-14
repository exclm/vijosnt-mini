Namespace Executing
    Friend Class UntrustedEnvironmentPool
        Inherits EnvironmentPoolBase
        Implements IDisposable

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

        Public Overrides Function Take() As EnvironmentBase
            SyncLock m_Stack
                If m_Stack.Count = 0 Then
                    Return Nothing
                Else
                    Return m_Stack.Pop()
                End If
            End SyncLock
        End Function

        Protected Overrides Sub UntakeInternal(ByVal Environment As EnvironmentBase)
            SyncLock m_Stack
                m_Stack.Push(Environment)
            End SyncLock
            MyBase.UntakeInternal(Environment)
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                While m_Stack.Count <> 0
                    m_Stack.Pop().Dispose()
                End While
            End If
            Me.disposedValue = True
            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides Sub Finalize()
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(False)
            MyBase.Finalize()
        End Sub
#End Region
    End Class
End Namespace
