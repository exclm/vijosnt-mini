Friend NotInheritable Class AppPath
    Inherits Path

    Public Overrides Function GetPath() As String
        Return My.Application.Info.DirectoryPath
    End Function
End Class
