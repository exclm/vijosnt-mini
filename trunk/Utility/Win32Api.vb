Friend Module Win32Api
    Public Declare Auto Function LogonUser Lib "advapi32.dll" ( _
        ByVal UserName As String, _
        ByVal Domain As String, _
        ByVal Password As String, _
        ByVal LogonType As LogonType, _
        ByVal LogonProvider As LogonProvider, _
        ByRef hToken As IntPtr) As Boolean

    Public Declare Auto Function GetTokenInformation Lib "advapi32.dll" ( _
        ByVal TokenHandle As IntPtr, _
        ByVal TokenInformationClass As TOKEN_INFORMATION_CLASS, _
        ByVal TokenInformation As IntPtr, _
        ByVal TokenInformationLength As Int32, _
        ByRef ReturnLength As Int32) As Boolean

    Public Declare Auto Function GetLengthSid Lib "advapi32.dll" ( _
        ByVal Sid As IntPtr) As Int32

    Public Declare Auto Function CopySid Lib "advapi32.dll" ( _
        ByVal nDestinationSidLength As Int32, _
        ByVal pDestinationSid As IntPtr, _
        ByVal pSourceSid As IntPtr) As Boolean

    Public Declare Auto Function EqualSid Lib "advapi32.dll" ( _
        ByVal pSid1 As IntPtr, _
        ByVal pSid2 As IntPtr) As Boolean

    Public Declare Auto Function InitializeSecurityDescriptor Lib "advapi32.dll" ( _
        ByVal pSecurityDescriptor As IntPtr, _
        ByVal dwRevision As Int32) As Boolean

    Public Declare Auto Function GetSecurityDescriptorDacl Lib "advapi32.dll" ( _
        ByVal pSecurityDescriptor As IntPtr, _
        ByRef bDaclPresent As Boolean, _
        ByRef pDacl As IntPtr, _
        ByRef bDaclDefaulted As Boolean) As Boolean

    Public Declare Auto Function SetSecurityDescriptorDacl Lib "advapi32.dll" ( _
        ByVal pSecurityDescriptor As IntPtr, _
        ByVal bDaclPresent As Boolean, _
        ByVal pDacl As IntPtr, _
        ByVal bDaclDefaulted As Boolean) As Boolean

    Public Declare Auto Function GetAclInformation Lib "advapi32.dll" ( _
        ByVal pAcl As IntPtr, _
        ByRef pAclInformation As ACL_SIZE_INFORMATION, _
        ByVal nAclInformationLength As Int32, _
        ByVal dwAclInformationClass As ACL_INFORMATION_CLASS) As Boolean

    Public Declare Auto Function InitializeAcl Lib "advapi32.dll" ( _
        ByVal pAcl As IntPtr, _
        ByVal nAclLength As Int32, _
        ByVal dwAclRevision As Int32) As Boolean

    Public Declare Auto Function GetAce Lib "advapi32.dll" ( _
        ByVal pAcl As IntPtr, _
        ByVal dwAceIndex As Int32, _
        ByRef pAce As IntPtr) As Boolean

    Public Declare Auto Function AddAce Lib "advapi32.dll" ( _
        ByVal pAcl As IntPtr, _
        ByVal dwAceRevision As Int32, _
        ByVal dwStartingAceIndex As Int32, _
        ByVal pAceList As IntPtr, _
        ByVal nAceListLength As Int32) As Boolean

    Public Declare Auto Function GetUserObjectSecurity Lib "user32.dll" ( _
        ByVal hObj As IntPtr, _
        ByRef pSIRequested As SECURITY_INFORMATION, _
        ByVal pSD As IntPtr, _
        ByVal nLength As Int32, _
        ByRef nLengthNeeded As Int32) As Boolean

    Public Declare Auto Function SetUserObjectSecurity Lib "user32.dll" ( _
        ByVal hObj As IntPtr, _
        ByRef pSIRequested As SECURITY_INFORMATION, _
        ByVal pSID As IntPtr) As Boolean

    Public Declare Auto Function GetUserObjectInformation Lib "user32.dll" ( _
        ByVal hObj As IntPtr, _
        ByVal nIndex As Int32, _
        ByVal pvInfo As IntPtr, _
        ByVal nLength As Int32, _
        ByRef nLengthNeeded As Int32) As Boolean

    Public Declare Auto Function GetProcessWindowStation Lib "user32.dll" () As IntPtr
    Public Declare Auto Function CreateDesktop Lib "user32.dll" ( _
        ByVal szDesktop As String, _
        ByVal szDevice As String, _
        ByVal pDevmode As IntPtr, _
        ByVal dwFlags As Int32, _
        ByVal dwDesiredAccess As Int32, _
        ByVal lpsa As IntPtr) As IntPtr

    Public Declare Auto Function CloseDesktop Lib "user32.dll" ( _
        ByVal hDesktop As IntPtr) As Boolean

    Public Declare Auto Function CloseHandle Lib "kernel32.dll" ( _
        ByVal hObject As IntPtr) As Boolean

    Public Structure ACL_SIZE_INFORMATION
        Dim AceCount As Int32
        Dim AclBytesInUse As Int32
        Dim AclBytesFree As Int32
    End Structure

    Public Structure ACE_HEADER
        Dim AceType As Byte
        Dim AceFlags As AceFlags
        Dim AceSize As Int16
    End Structure

    Public Enum LogonType As Int32
        LOGON32_LOGON_INTERACTIVE = 2
    End Enum

    Public Enum LogonProvider As Int32
        LOGON32_PROVIDER_DEFAULT = 0
    End Enum

    Public Enum TOKEN_INFORMATION_CLASS As Int32
        TokenGroups = 2
    End Enum

    Public Enum SECURITY_INFORMATION As Int32
        DACL_SECURITY_INFORMATION = 4
    End Enum

    Public Enum ACL_INFORMATION_CLASS As Int32
        AclSizeInformation = 2
    End Enum

    Public Enum AceFlags As Byte
        OBJECT_INHERIT_ACE = &H1
        CONTAINER_INHERIT_ACE = &H2
        NO_PROPAGATE_INHERIT_ACE = &H4
        INHERIT_ONLY_ACE = &H8
        INHERITED_ACE = &H10
        VALID_INHERIT_FLAGS = &H1F
    End Enum

    Public Const ERROR_INSUFFICIENT_BUFFER As Int32 = 122
    Public Const SE_GROUP_LOGON_ID As Int32 = &HC0000000
    Public Const SECURITY_DESCRIPTOR_REVISION As Int32 = 1
    Public Const ACL_REVISION As Int32 = 2
    Public Const ACCESS_ALLOWED_ACE_TYPE As Int32 = 0

    Public Const GENERIC_READ As Int32 = &H80000000
    Public Const GENERIC_WRITE As Int32 = &H40000000
    Public Const GENERIC_EXECUTE As Int32 = &H20000000
    Public Const GENERIC_ALL As Int32 = &H10000000

    Public Const WINSTA_ALL As Int32 = &HF037F
    Public Const DESKTOP_ALL As Int32 = &HF01FF

    Public Const DESKTOP_READOBJECTS As Int32 = 1
    Public Const DESKTOP_CREATEWINDOW As Int32 = 2
    Public Const DESKTOP_WRITEOBJECTS As Int32 = &H80
    Public Const READ_CONTROL As Int32 = &H20000
    Public Const WRITE_DAC As Int32 = &H40000

    Public Const UOI_NAME As Int32 = 2

    Public Sub Win32True(ByVal Value As Boolean)
        If Value = False Then
            Throw New Win32Exception()
        End If
    End Sub
End Module
