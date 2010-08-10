Imports VijosNT.Utility
Imports VijosNT.Win32

Namespace Executing
    Friend MustInherit Class Executee
        Protected m_Environment As Environment

        Public Property Environment() As Environment
            Get
                Return m_Environment
            End Get

            Set(ByVal Value As Environment)
                m_Environment = Value
            End Set
        End Property

        Public MustOverride ReadOnly Property RequiredEnvironment() As EnvironmentTag

        Public Overridable Sub Execute()
            Environment.Untake()
        End Sub
    End Class
End Namespace
