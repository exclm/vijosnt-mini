Namespace Executing
    Friend Class TrustedEnvironmentPool
        Inherits EnvironmentPoolBase

        Public Overrides Function Take() As EnvironmentBase
            Dim Result As New TrustedEnvironment()
            Result.Pool = Me
            Return Result
        End Function
    End Class
End Namespace
