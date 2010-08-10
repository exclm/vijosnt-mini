Imports VijosNT_Mini.Win32

Namespace Executing
    Friend MustInherit Class Environment
        Protected m_EnvironmentPool As EnvironmentPool

        Public Property Pool() As EnvironmentPool
            Get
                Return m_EnvironmentPool
            End Get

            Set(ByVal Value As EnvironmentPool)
                m_EnvironmentPool = Value
            End Set
        End Property

        Public Sub Untake()
            Me.Pool.Untake(Me)
        End Sub

        Public MustOverride ReadOnly Property Tag() As EnvironmentTag
        Public MustOverride ReadOnly Property DesktopName() As String
        Public MustOverride ReadOnly Property Token() As Token
    End Class
End Namespace
