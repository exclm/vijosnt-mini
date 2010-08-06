Friend MustInherit Class Path
    Public MustOverride Function GetPath() As String

    Public Overridable Function GetDirectoryInfo() As DirectoryInfo
        Return New DirectoryInfo(GetPath())
    End Function

    Public Function ResolvePath(ByVal RelativePath As String) As String
        Return GetPath() & "\" & RelativePath
    End Function
End Class
