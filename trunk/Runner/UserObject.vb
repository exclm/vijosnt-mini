Public MustInherit Class UserObject
    Protected MustOverride Function GetHandle() As IntPtr

    Public Overridable Function GetName() As String
        Dim Handle As IntPtr = GetHandle()
        Dim Length As Int32
        Dim NamePtr As IntPtr
        Dim LastErr As Int32
        Dim Name As String

        If GetUserObjectInformation(Handle, UOI_NAME, 0, 0, Length) = True Then
            Throw New Win32Exception("GetUserObjectInformation succeeded with no information")
        End If

        LastErr = Marshal.GetLastWin32Error()
        If LastErr <> ERROR_INSUFFICIENT_BUFFER Then
            Throw New Win32Exception(LastErr)
        End If

        NamePtr = Marshal.AllocHGlobal(Length)
        Try
            Win32True(GetUserObjectInformation(Handle, UOI_NAME, NamePtr, Length, Length))
            Name = Marshal.PtrToStringAuto(NamePtr)
        Finally
            Marshal.FreeHGlobal(NamePtr)
        End Try

        Return Name
    End Function
End Class
