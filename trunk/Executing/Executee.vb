Imports VijosNT.Utility
Imports VijosNT.Win32

Namespace Executing
    Friend MustInherit Class Executee
        Protected m_Environment As EnvironmentBase

        Public Property Environment() As EnvironmentBase
            Get
                Return m_Environment
            End Get

            Set(ByVal Value As EnvironmentBase)
                m_Environment = Value
            End Set
        End Property

        Public Overridable ReadOnly Property RequiredEnvironment() As EnvironmentTag
            Get
                Return EnvironmentTag.Trusted
            End Get
        End Property

        Public Overridable Sub Execute()
            Environment.Pool.Untake(Environment)
        End Sub
    End Class
End Namespace
