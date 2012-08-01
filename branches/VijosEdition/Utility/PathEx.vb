Namespace Utility
    Friend MustInherit Class PathEx
        Public MustOverride Function GetPath() As String

        Public Overridable Function GetDirectoryInfo() As DirectoryInfo
            Return New DirectoryInfo(GetPath())
        End Function

        Public Function Combine(ByVal RelativePath As String) As String
            Return Path.Combine(GetPath(), RelativePath)
        End Function
    End Class
End Namespace
