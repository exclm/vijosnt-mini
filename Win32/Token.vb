Friend Class Token
    Implements IDisposable

    Protected m_Handle As IntPtr

    Public Sub New()
        Win32True(OpenProcessToken(GetCurrentProcess(), TokenAccess.TOKEN_ALL_ACCESS, m_Handle))
    End Sub

    Public Sub New(ByVal UserName As String, ByVal Password As String)
        Win32True(LogonUser(UserName, ".", Password, _
            LogonType.LOGON32_LOGON_INTERACTIVE, LogonProvider.LOGON32_PROVIDER_DEFAULT, _
            m_Handle))
    End Sub

    Public Function GetHandleUnsafe() As IntPtr
        Return m_Handle
    End Function

    Public Function GetSid() As Byte()
        Dim Length As Int32

        If Not GetTokenInformation(m_Handle, TOKEN_INFORMATION_CLASS.TokenGroups, 0, 0, Length) Then
            Dim LastErr As Int32 = Marshal.GetLastWin32Error()
            If LastErr <> ERROR_INSUFFICIENT_BUFFER Then
                Throw New Win32Exception(LastErr)
            End If
        Else
            Throw New Win32Exception("GetTokenInformation succeeded with no information")
        End If

        Dim TokenGroupsPtr As IntPtr = Marshal.AllocHGlobal(Length)
        Try
            Win32True(GetTokenInformation(m_Handle, TOKEN_INFORMATION_CLASS.TokenGroups, TokenGroupsPtr, Length, Length))

            Dim GroupCount As Int32 = Marshal.ReadInt32(TokenGroupsPtr, 0)
            For Index As Int32 = 0 To GroupCount - 1
                Dim Sid As IntPtr = Marshal.ReadInt32(TokenGroupsPtr, Index * 8 + 4)
                Dim Attributes As Int32 = Marshal.ReadInt32(TokenGroupsPtr, Index * 8 + 8)

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

        Win32True(AdjustTokenPrivileges(m_Handle, False, TokenPrivileges, Marshal.SizeOf(TokenPrivileges), Nothing, Nothing))
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            Win32True(CloseHandle(m_Handle))
        End If
        Me.disposedValue = True
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
