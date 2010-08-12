Namespace Win32
    Friend Class FileEx
        Inherits KernelObject

        Public Sub New(ByVal FileName As String, ByVal Mode As FileMode, ByVal Access As FileAccess, ByVal Share As FileShare)
            Using FileStream As New FileStream(FileName, Mode, Access, Share)
                InternalSetHandle(Duplicate(FileStream.SafeFileHandle.DangerousGetHandle(), False))
            End Using
        End Sub
    End Class
End Namespace
