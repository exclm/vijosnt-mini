Imports VijosNT.Win32

Namespace Executing
    Friend Class UntrustedEnvironment
        Inherits Environment

        Protected m_Desktop As Desktop
        Protected m_Token As Token

        Public Sub New(ByVal DesktopName As String, ByVal UserName As String, ByVal Password As String)
            m_Desktop = New Desktop(DesktopName)
            m_Token = New Token(UserName, Password)
        End Sub

        Public Overrides ReadOnly Property Tag() As EnvironmentTag
            Get
                Return EnvironmentTag.Untrusted
            End Get
        End Property

        Public Overrides ReadOnly Property DesktopName() As String
            Get
                Return m_Desktop.GetName()
            End Get
        End Property

        Public Overrides ReadOnly Property Token() As Token
            Get
                Return m_Token
            End Get
        End Property
    End Class
End Namespace
