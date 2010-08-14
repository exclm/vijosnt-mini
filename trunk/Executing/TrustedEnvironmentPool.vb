Namespace Executing
    Friend Class TrustedEnvironmentPool
        Inherits EnvironmentPoolBase

        Public Overrides ReadOnly Property Tag() As EnvironmentTag
            Get
                Return EnvironmentTag.Trusted
            End Get
        End Property

        Public Overrides Function Take() As EnvironmentBase
            Dim Result As New TrustedEnvironment()
            Result.Pool = Me
            Return Result
        End Function
    End Class
End Namespace
