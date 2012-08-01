Imports VijosNT.Win32

Namespace Executing
    Friend Class TrustedEnvironment
        Inherits EnvironmentBase

        Public Overrides ReadOnly Property DesktopName() As String
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property Token() As Token
            Get
                Return Nothing
            End Get
        End Property
    End Class
End Namespace
