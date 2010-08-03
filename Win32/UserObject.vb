Friend MustInherit Class UserObject
    Implements IDisposable

    Public Enum AceFlags As Byte
        ObjectInheritAce = &H1
        ContainerInheritAce = &H2
        NoPropagateInheritAce = &H4
        InheritOnlyAce = &H8
        InheritedAce = &H10
        ValidInheritFlags = &H1F
    End Enum

    Public Enum AceMask As Int32
        GenericRead = &H80000000
        GenericWrite = &H40000000
        GenericExecute = &H20000000
        GenericAll = &H10000000
        ReadControl = &H20000
        WinstaAll = &HF037F
        DesktopReadObjects = &H1
        DesktopWriteObjects = &H80
        DesktopAll = &HF01FF
        WinstaAccessGlobalAtoms = &H20
    End Enum

    Public Structure AllowedAce
        Public Sub New(ByVal Flags As AceFlags, ByVal Mask As AceMask)
            Me.Flags = Flags
            Me.Mask = Mask
        End Sub

        Public Flags As AceFlags
        Public Mask As AceMask
    End Structure

    Protected MustOverride Function GetHandle() As IntPtr

    Public Overridable Function GetName() As String
        Dim Handle As IntPtr = GetHandle()
        Dim Length As Int32

        If Not GetUserObjectInformation(Handle, UserObjectInformation.UOI_NAME, 0, 0, Length) Then
            Dim LastErr As Int32 = Marshal.GetLastWin32Error()
            If LastErr <> ERROR_INSUFFICIENT_BUFFER Then
                Throw New Win32Exception(LastErr)
            End If
        Else
            Throw New Win32Exception("GetUserObjectInformation succeeded with no information")
        End If

        Dim NamePtr As IntPtr = Marshal.AllocHGlobal(Length)
        Try
            Win32True(GetUserObjectInformation(Handle, UserObjectInformation.UOI_NAME, NamePtr, Length, Length))
            Return Marshal.PtrToStringAuto(NamePtr)
        Finally
            Marshal.FreeHGlobal(NamePtr)
        End Try
    End Function

    Public Sub AddAllowedAce(ByVal Sid As Byte(), ByVal Ace As AllowedAce)
        AddAllowedAce(Sid, New AllowedAce() {Ace})
    End Sub

    Public Sub AddAllowedAce(ByVal Sid As Byte(), ByVal AceList As IList(Of AllowedAce))
        Dim UserObject As IntPtr = GetHandle()
        Dim InfoRequired As SECURITY_INFORMATION = SECURITY_INFORMATION.DACL_SECURITY_INFORMATION
        Dim Length As Int32

        If Not GetUserObjectSecurity(UserObject, InfoRequired, 0, 0, Length) Then
            Dim LastErr As Int32 = Marshal.GetLastWin32Error()
            If LastErr <> ERROR_INSUFFICIENT_BUFFER Then
                Throw New Win32Exception(LastErr)
            End If
        Else
            Throw New Win32Exception("GetUserObjectSecurity succeeded with no information")
        End If

        Dim bDaclPresent As Boolean
        Dim AclOld As IntPtr
        Dim bDaclExist As Boolean
        Dim SecurityDescriptorOld As IntPtr = Marshal.AllocHGlobal(Length)

        Try
            Win32True(GetUserObjectSecurity(UserObject, InfoRequired, SecurityDescriptorOld, Length, Length))
            Win32True(GetSecurityDescriptorDacl(SecurityDescriptorOld, bDaclPresent, AclOld, bDaclExist))

            ' SecurityDescriptorOld should not be freed because AclOld is a reference

            Dim AclSizeNew As Int32 = AceList.Count * (Marshal.SizeOf(GetType(ACE_HEADER)) + 4 + Sid.Length)
            Dim AclSizeInfo As ACL_SIZE_INFORMATION

            If bDaclPresent Then
                Win32True(GetAclInformation(AclOld, AclSizeInfo, Marshal.SizeOf(AclSizeInfo), ACL_INFORMATION_CLASS.AclSizeInformation))
                AclSizeNew += AclSizeInfo.AclBytesInUse
            End If

            Dim AclNew As IntPtr = Marshal.AllocHGlobal(AclSizeNew)
            Try
                Win32True(InitializeAcl(AclNew, AclSizeNew, ACL_REVISION))
                If bDaclPresent Then
                    For Index As Int32 = 0 To AclSizeInfo.AceCount - 1
                        Dim TempAcePtr As IntPtr
                        Win32True(GetAce(AclOld, Index, TempAcePtr))
                        Win32True(AddAce(AclNew, ACL_REVISION, -1, TempAcePtr, Marshal.ReadInt16(TempAcePtr, 2)))
                    Next
                End If

                Dim SidHandle As GCHandle = GCHandle.Alloc(Sid, GCHandleType.Pinned)
                Try
                    For Each Ace As AllowedAce In AceList
                        Win32True(AddAccessAllowedAceEx(AclNew, ACL_REVISION, Ace.Flags, Ace.Mask, SidHandle.AddrOfPinnedObject()))
                    Next
                Finally
                    SidHandle.Free()
                End Try

                Dim SecurityDescriptorNew As IntPtr = Marshal.AllocHGlobal(Length)
                Try
                    Win32True(InitializeSecurityDescriptor(SecurityDescriptorNew, SECURITY_DESCRIPTOR_REVISION))
                    Win32True(SetSecurityDescriptorDacl(SecurityDescriptorNew, True, AclNew, False))
                    Win32True(SetUserObjectSecurity(UserObject, InfoRequired, SecurityDescriptorNew))
                Finally
                    Marshal.FreeHGlobal(SecurityDescriptorNew)
                End Try
            Finally
                Marshal.FreeHGlobal(AclNew)
            End Try
        Finally
            Marshal.FreeHGlobal(SecurityDescriptorOld)
        End Try
    End Sub

    Public Sub RemoveAceBySid(ByVal Sid As Byte())
        Dim UserObject As IntPtr = GetHandle()
        Dim InfoRequired As SECURITY_INFORMATION = SECURITY_INFORMATION.DACL_SECURITY_INFORMATION
        Dim Length As Int32

        If Not GetUserObjectSecurity(UserObject, InfoRequired, 0, 0, Length) Then
            Dim LastErr As Int32 = Marshal.GetLastWin32Error()
            If LastErr <> ERROR_INSUFFICIENT_BUFFER Then
                Throw New Win32Exception(LastErr)
            End If
        Else
            Throw New Win32Exception("GetUserObjectSecurity succeeded with no information")
        End If

        Dim SecurityDescriptorOld As IntPtr = Marshal.AllocHGlobal(Length)

        Try
            Win32True(GetUserObjectSecurity(UserObject, InfoRequired, SecurityDescriptorOld, Length, Length))

            Dim bDaclPresent As Boolean
            Dim AclOld As IntPtr
            Dim bDaclExist As Boolean

            Win32True(GetSecurityDescriptorDacl(SecurityDescriptorOld, bDaclPresent, AclOld, bDaclExist))

            ' SecurityDescriptorOld should not be freed because AclOld is a reference

            If bDaclPresent Then
                Dim AclSizeInfo As ACL_SIZE_INFORMATION
                Win32True(GetAclInformation(AclOld, AclSizeInfo, Marshal.SizeOf(AclSizeInfo), ACL_INFORMATION_CLASS.AclSizeInformation))
                Dim AclNew As IntPtr = Marshal.AllocHGlobal(AclSizeInfo.AclBytesInUse)
                Try
                    Win32True(InitializeAcl(AclNew, AclSizeInfo.AclBytesInUse, ACL_REVISION))
                    Dim SidHandle As GCHandle = GCHandle.Alloc(Sid, GCHandleType.Pinned)
                    Try
                        For Index As Int32 = 0 To AclSizeInfo.AceCount - 1
                            Dim TempAcePtr As IntPtr
                            Win32True(GetAce(AclOld, Index, TempAcePtr))

                            If EqualSid(TempAcePtr.ToInt64() + Marshal.SizeOf(GetType(ACE_HEADER)) + 4, _
                                SidHandle.AddrOfPinnedObject()) = True Then _
                                Continue For

                            Win32True(AddAce(AclNew, ACL_REVISION, -1, TempAcePtr, Marshal.ReadInt16(TempAcePtr, 2)))
                        Next
                    Finally
                        SidHandle.Free()
                    End Try

                    Dim SecurityDescriptorNew As IntPtr = Marshal.AllocHGlobal(Length)
                    Try
                        Win32True(InitializeSecurityDescriptor(SecurityDescriptorNew, SECURITY_DESCRIPTOR_REVISION))
                        Win32True(SetSecurityDescriptorDacl(SecurityDescriptorNew, bDaclPresent, AclNew, False))
                        Win32True(SetUserObjectSecurity(UserObject, InfoRequired, SecurityDescriptorNew))
                    Finally
                        Marshal.FreeHGlobal(SecurityDescriptorNew)
                    End Try
                Finally
                    Marshal.FreeHGlobal(AclNew)
                End Try
            End If
        Finally
            Marshal.FreeHGlobal(SecurityDescriptorOld)
        End Try
    End Sub

#Region "IDisposable Support"
    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        ' Do nothing
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
