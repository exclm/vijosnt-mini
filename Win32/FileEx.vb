Namespace Win32
    Friend Class FileEx
        Inherits KernelObject

        Public Sub New(ByVal FileName As String, ByVal DesiredAccess As CreateFileAccess, ByVal ShareMode As CreateFileShare, ByVal CreateDisposition As CreateFileDisposition, ByVal Flags As CreateFileFlags)
            Dim Handle As IntPtr = CreateFile(FileName, DesiredAccess, ShareMode, Nothing, CreateDisposition, Flags, IntPtr.Zero)
            Win32True(Handle <> INVALID_HANDLE_VALUE)
            InternalSetHandle(Handle)
        End Sub
    End Class
End Namespace
