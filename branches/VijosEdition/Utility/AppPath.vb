Namespace Utility
    Friend NotInheritable Class AppPath
        Inherits PathEx

        Public Overrides Function GetPath() As String
            Return My.Application.Info.DirectoryPath
        End Function
    End Class
End Namespace