Friend Class Token
    Inherits KernelObject
    Implements IDisposable

    Public Sub New()
        Dim TokenHandle As IntPtr
        Win32True(OpenProcessToken(GetCurrentProcess(), TokenAccess.TOKEN_ALL_ACCESS, TokenHandle))
        InternalSetHandle(TokenHandle)
    End Sub

    Public Sub New(ByVal UserName As String, ByVal Password As String)
        Dim TokenHandle As IntPtr
        Win32True(LogonUser(UserName, ".", Password, LogonType.LOGON32_LOGON_INTERACTIVE, _
            LogonProvider.LOGON32_PROVIDER_DEFAULT, TokenHandle))
        InternalSetHandle(TokenHandle)
    End Sub

    Public Function GetSid() As Byte()
        Dim Length As Int32

        If Not GetTokenInformation(MyBase.GetHandleUnsafe(), TOKEN_INFORMATION_CLASS.TokenGroups, 0, 0, Length) Then
            Dim LastErr As Int32 = Marshal.GetLastWin32Error()
            If LastErr <> ERROR_INSUFFICIENT_BUFFER Then
                Throw New Win32Exception(LastErr)
            End If
        Else
            Throw New Win32Exception("GetTokenInformation succeeded with no information")
        End If

        Dim TokenGroupsPtr As IntPtr = Marshal.AllocHGlobal(Length)
        Try
            Win32True(GetTokenInformation(MyBase.GetHandleUnsafe(), TOKEN_INFORMATION_CLASS.TokenGroups, TokenGroupsPtr, Length, Length))

            Dim GroupCount As Int32 = Marshal.ReadInt32(TokenGroupsPtr, 0)
            For Index As Int32 = 0 To GroupCount - 1
                Dim Sid As IntPtr = Marshal.ReadIntPtr(TokenGroupsPtr, IntPtr.Size + Index * (IntPtr.Size * 2))
                Dim Attributes As Int32 = Marshal.ReadInt32(TokenGroupsPtr, IntPtr.Size + Index * (IntPtr.Size * 2) + IntPtr.Size)

                If (Attributes And SE_GROUP_LOGON_ID) = SE_GROUP_LOGON_ID Then
                    Dim SidLength As Int32 = GetLengthSid(Sid)
                    Dim NewSid As Byte() = New Byte(0 To SidLength - 1) {}
                    Dim NewSidHandle As GCHandle = GCHandle.Alloc(NewSid, GCHandleType.Pinned)
                    Try
                        Win32True(CopySid(SidLength, NewSidHandle.AddrOfPinnedObject(), Sid))
                    Finally
                        NewSidHandle.Free()
                    End Try
                    Return NewSid
                End If
            Next

            Throw New Win32Exception("Token group not found")
        Finally
            Marshal.FreeHGlobal(TokenGroupsPtr)
        End Try
    End Function

    Public Sub SetPrivilege(ByVal Privilege As String, ByVal EnablePrivilege As Boolean)
        Dim TokenPrivileges As TOKEN_PRIVILEGES

        Win32True(LookupPrivilegeValue(Nothing, Privilege, TokenPrivileges.Privilege.Luid))
        TokenPrivileges.PrivilegeCount = 1

        If EnablePrivilege Then
            TokenPrivileges.Privilege.Attributes = PrivilegeAttribute.SE_PRIVILEGE_ENABLED
        Else
            TokenPrivileges.Privilege.Attributes = 0
        End If

        Win32True(AdjustTokenPrivileges(MyBase.GetHandleUnsafe(), False, TokenPrivileges, Marshal.SizeOf(TokenPrivileges), Nothing, Nothing))
    End Sub
End Class
