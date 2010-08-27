Namespace Win32
    Friend Module NativeMethods

#Region "advapi32.dll"
        Public Declare Auto Function OpenProcessToken Lib "advapi32.dll" ( _
            ByVal ProcessHandle As IntPtr, _
            ByVal DesiredAccess As TokenAccess, _
            ByRef TokenHandle As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function LookupPrivilegeValue Lib "advapi32.dll" ( _
            ByVal SystemName As String, _
            ByVal Name As String, _
            ByRef Luid As LUID) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function AdjustTokenPrivileges Lib "advapi32.dll" ( _
            ByVal TokenHandle As IntPtr, _
            <MarshalAs(UnmanagedType.Bool)> ByVal DisableAllPrivileges As Boolean, _
            ByRef NewState As TOKEN_PRIVILEGES, _
            ByVal BufferLength As Int32, _
            ByRef PreviousState As TOKEN_PRIVILEGES, _
            ByRef ReturnLength As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function CreateProcessAsUser Lib "advapi32.dll" ( _
            ByVal TokenHandle As IntPtr, _
            ByVal ApplicationName As String, _
            ByVal CommandLine As String, _
            ByRef ProcessAttributes As SECURITY_ATTRIBUTES, _
            ByRef ThreadAttributes As SECURITY_ATTRIBUTES, _
            <MarshalAs(UnmanagedType.Bool)> ByVal InheritHandles As Boolean, _
            ByVal CreationFlags As CreationFlags, _
            ByVal Environment As IntPtr, _
            ByVal CurrentDirectory As String, _
            ByRef StartupInfo As STARTUPINFO, _
            ByRef ProcessInformation As PROCESS_INFORMATION _
            ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function ImpersonateLoggedOnUser Lib "advapi32.dll" ( _
            ByVal hToken As IntPtr _
            ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function RevertToSelf Lib "advapi32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function LogonUser Lib "advapi32.dll" ( _
            ByVal UserName As String, _
            ByVal Domain As String, _
            ByVal Password As String, _
            ByVal LogonType As LogonType, _
            ByVal LogonProvider As LogonProvider, _
            ByRef hToken As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function GetTokenInformation Lib "advapi32.dll" ( _
            ByVal TokenHandle As IntPtr, _
            ByVal TokenInformationClass As TOKEN_INFORMATION_CLASS, _
            ByVal TokenInformation As IntPtr, _
            ByVal TokenInformationLength As Int32, _
            ByRef ReturnLength As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function GetLengthSid Lib "advapi32.dll" ( _
            ByVal Sid As IntPtr) As Int32

        Public Declare Auto Function CopySid Lib "advapi32.dll" ( _
            ByVal nDestinationSidLength As Int32, _
            ByVal pDestinationSid As IntPtr, _
            ByVal pSourceSid As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function EqualSid Lib "advapi32.dll" ( _
            ByVal pSid1 As IntPtr, _
            ByVal pSid2 As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function InitializeSecurityDescriptor Lib "advapi32.dll" ( _
            ByVal pSecurityDescriptor As IntPtr, _
            ByVal dwRevision As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function GetSecurityDescriptorDacl Lib "advapi32.dll" ( _
            ByVal pSecurityDescriptor As IntPtr, _
            <MarshalAs(UnmanagedType.Bool)> ByRef bDaclPresent As Boolean, _
            ByRef pDacl As IntPtr, _
            <MarshalAs(UnmanagedType.Bool)> ByRef bDaclDefaulted As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function SetSecurityDescriptorDacl Lib "advapi32.dll" ( _
            ByVal pSecurityDescriptor As IntPtr, _
            <MarshalAs(UnmanagedType.Bool)> ByVal bDaclPresent As Boolean, _
            ByVal pDacl As IntPtr, _
            <MarshalAs(UnmanagedType.Bool)> ByVal bDaclDefaulted As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function GetAclInformation Lib "advapi32.dll" ( _
            ByVal pAcl As IntPtr, _
            ByRef pAclInformation As ACL_SIZE_INFORMATION, _
            ByVal nAclInformationLength As Int32, _
            ByVal dwAclInformationClass As ACL_INFORMATION_CLASS) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function InitializeAcl Lib "advapi32.dll" ( _
            ByVal pAcl As IntPtr, _
            ByVal nAclLength As Int32, _
            ByVal dwAclRevision As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function GetAce Lib "advapi32.dll" ( _
            ByVal pAcl As IntPtr, _
            ByVal dwAceIndex As Int32, _
            ByRef pAce As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function AddAce Lib "advapi32.dll" ( _
            ByVal pAcl As IntPtr, _
            ByVal dwAceRevision As Int32, _
            ByVal dwStartingAceIndex As Int32, _
            ByVal pAceList As IntPtr, _
            ByVal nAceListLength As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function AddAccessAllowedAceEx Lib "advapi32.dll" (
            ByVal pAcl As IntPtr, _
            ByVal dwAceRevision As Int32, _
            ByVal AceFlags As Int32, _
            ByVal AccessMask As Int32, _
            ByVal pSid As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function OpenSCManager Lib "advapi32.dll" ( _
            ByVal MachineName As String, _
            ByVal DatabaseName As String, _
            ByVal dwDesiredAccess As SCManagerAccess) As IntPtr

        Public Declare Auto Function CloseServiceHandle Lib "advapi32.dll" ( _
            ByVal hSCObject As IntPtr) As Boolean

        Public Declare Auto Function OpenService Lib "advapi32.dll" ( _
            ByVal hSCManager As IntPtr, _
            ByVal ServiceName As String, _
            ByVal dwDesiredAccess As ServiceAccess) As IntPtr

        Public Declare Auto Function DeleteService Lib "advapi32.dll" ( _
            ByVal hService As IntPtr) As Boolean

        Public Declare Auto Function CreateService Lib "advapi32.dll" ( _
            ByVal hSCManager As IntPtr, _
            ByVal ServiceName As String, _
            ByVal DisplayName As String, _
            ByVal dwDesiredAccess As ServiceAccess, _
            ByVal dwServiceType As ServiceType, _
            ByVal dwStartType As ServiceStartType, _
            ByVal dwErrorControl As ServiceErrorControl, _
            ByVal BinaryPathName As String, _
            ByVal LoadOrderGroup As String, _
            ByVal lpdwTagId As Int32, _
            ByVal Dependencies As String, _
            ByVal ServiceStartName As String, _
            ByVal Password As String) As IntPtr

        Public Declare Auto Function StartService Lib "advapi32.dll" ( _
            ByVal hService As IntPtr, _
            ByVal dwNumServiceArgs As Int32, _
            ByVal lpServiceArgVectors As IntPtr) As Boolean

        Public Declare Auto Function ControlService Lib "advapi32.dll" ( _
            ByVal hService As IntPtr, _
            ByVal dwControl As ServiceControl, _
            ByRef ServiceStatus As SERVICE_STATUS) As Boolean

        Public Declare Auto Function QueryServiceStatusEx Lib "advapi32.dll" ( _
            ByVal hService As IntPtr, _
            ByVal InfoLevel As SC_STATUS_TYPE, _
            ByRef Buffer As SERVICE_STATUS_PROCESS, _
            ByVal cbBufSize As Int32, _
            ByRef cbBytesNeeded As Int32) As Boolean

        Public Enum TokenAccess As Int32
            TOKEN_ADJUST_PRIVILEGES = &H20
            TOKEN_ALL_ACCESS = &HF01FF
        End Enum

        Public Enum PrivilegeAttribute As Int32
            SE_PRIVILEGE_ENABLED_BY_DEFAULT = &H1
            SE_PRIVILEGE_ENABLED = &H2
            SE_PRIVILEGE_USED_FOR_ACCESS = &H80000000
        End Enum

        Public Structure LUID
            Dim LowPart As Int32
            Dim HighPart As Int32
        End Structure

        Public Structure LUID_AND_ATTRIBUTES
            Dim Luid As LUID
            Dim Attributes As PrivilegeAttribute
        End Structure

        Public Structure TOKEN_PRIVILEGES
            Dim PrivilegeCount As Int32
            Dim Privilege As LUID_AND_ATTRIBUTES
        End Structure

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

        Public Enum DesktopAccess As Int32
            DESKTOP_READOBJECTS = 1
            DESKTOP_CREATEWINDOW = 2
            DESKTOP_WRITEOBJECTS = &H80
            DESKTOP_SWITCHDESKTOP = &H100
            READ_CONTROL = &H20000
            WRITE_DAC = &H40000
        End Enum

        Public Enum UserObjectInformation As Int32
            UOI_NAME = 2
        End Enum

        Public Enum SCManagerAccess As Int32
            SC_MANAGER_ALL_ACCESS = &HF003F
        End Enum

        Public Enum ServiceAccess As Int32
            SERVICE_ALL_ACCESS = &HF01FF
        End Enum

        Public Structure SERVICE_STATUS
            Dim dwServiceType As Int32
            Dim dwCurrentState As ServiceState
            Dim dwControlsAccepted As Int32
            Dim dwWin32ExitCode As Int32
            Dim dwServiceSpecificExitCode As Int32
            Dim dwCheckPoint As Int32
            Dim dwWaitHint As Int32
        End Structure

        Public Structure SERVICE_STATUS_PROCESS
            Dim dwServiceType As Int32
            Dim dwCurrentState As ServiceState
            Dim dwControlsAccepted As Int32
            Dim dwWin32ExitCode As Int32
            Dim dwServiceSpecificExitCode As Int32
            Dim dwCheckPoint As Int32
            Dim dwWaitHint As Int32
            Dim dwProcessId As Int32
            Dim dwServiceFlags As Int32
        End Structure

        Public Enum ServiceState As Int32
            SERVICE_CONTINUE_PENDING = 5
            SERVICE_PAUSE_PENDING = 6
            SERVICE_PAUSED = 7
            SERVICE_RUNNING = 4
            SERVICE_START_PENDING = 2
            SERVICE_STOP_PENDING = 3
            SERVICE_STOPPED = 1
        End Enum

        Public Enum SC_STATUS_TYPE As Int32
            SC_STATUS_PROCESS_INFO = 0
        End Enum

        Public Enum ServiceType As Int32
            SERVICE_ADAPTER = &H4
            SERVICE_FILE_SYSTEM_DRIVER = &H2
            SERVICE_KERNEL_DRIVER = &H1
            SERVICE_RECOGNIZER_DRIVER = &H8
            SERVICE_WIN32_OWN_PROCESS = &H10
            SERVICE_WIN32_SHARE_PROCESS = &H20
            SERVICE_INTERACTIVE_PROCESS = &H100
        End Enum

        Public Enum ServiceStartType As Int32
            SERVICE_AUTO_START = &H2
            SERVICE_BOOT_START = &H0
            SERVICE_DEMAND_START = &H3
            SERVICE_DISABLED = &H4
            SERVICE_SYSTEM_START = &H1
        End Enum

        Public Enum ServiceErrorControl As Int32
            SERVICE_ERROR_CRITICAL = &H3
            SERVICE_ERROR_IGNORE = &H0
            SERVICE_ERROR_NORMAL = &H1
            SERVICE_ERROR_SEVERE = &H2
        End Enum

        Public Enum ServiceControl As Int32
            SERVICE_CONTROL_CONTINUE = &H3
            SERVICE_CONTROL_INTERROGATE = &H4
            SERVICE_CONTROL_NETBINDADD = &H7
            SERVICE_CONTROL_NETBINDDISABLE = &HA
            SERVICE_CONTROL_NETBINDENABLE = &H9
            SERVICE_CONTROL_NETBINDREMOVE = &H8
            SERVICE_CONTROL_PARAMCHANGE = &H6
            SERVICE_CONTROL_PAUSE = &H2
            SERVICE_CONTROL_STOP = &H1
        End Enum
#End Region

#Region "user32.dll"
        Public Declare Auto Function GetUserObjectSecurity Lib "user32.dll" ( _
            ByVal hObj As IntPtr, _
            ByRef pSIRequested As SECURITY_INFORMATION, _
            ByVal pSD As IntPtr, _
            ByVal nLength As Int32, _
            ByRef nLengthNeeded As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function SetUserObjectSecurity Lib "user32.dll" ( _
            ByVal hObj As IntPtr, _
            ByRef pSIRequested As SECURITY_INFORMATION, _
            ByVal pSID As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function GetUserObjectInformation Lib "user32.dll" ( _
            ByVal hObj As IntPtr, _
            ByVal nIndex As UserObjectInformation, _
            ByVal pvInfo As IntPtr, _
            ByVal nLength As Int32, _
            ByRef nLengthNeeded As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function GetProcessWindowStation Lib "user32.dll" () As IntPtr

        Public Declare Auto Function CreateDesktop Lib "user32.dll" ( _
            ByVal szDesktop As String, _
            ByVal szDevice As String, _
            ByVal pDevmode As IntPtr, _
            ByVal dwFlags As Int32, _
            ByVal dwDesiredAccess As DesktopAccess, _
            ByVal lpsa As IntPtr) As IntPtr

        Public Declare Auto Function CloseDesktop Lib "user32.dll" ( _
            ByVal hDesktop As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function SwitchDesktop Lib "user32.dll" ( _
            ByVal hDesktop As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function SendMessage Lib "user32.dll" ( _
            ByVal hWnd As IntPtr, ByVal Msg As Int32, ByVal wParam As Int32, ByVal lParam As Int32) As Int32

        Public Declare Auto Function ReleaseCapture Lib "user32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Const WM_SYSCOMMAND As Int32 = &H112
        Public Const SC_MOVE As Int32 = &HF010
        Public Const HT_CAPTION As Int32 = &H2
#End Region

#Region "kernel32.dll"
        Public Declare Auto Function CreateFile Lib "kernel32.dll" ( _
            ByVal FileName As String, _
            ByVal DesiredAccess As CreateFileAccess, _
            ByVal ShareMode As CreateFileShare, _
            ByRef SecurityAttributes As SECURITY_ATTRIBUTES, _
            ByVal CreateDisposition As CreateFileDisposition, _
            ByVal FlagsAndAttributes As CreateFileFlags, _
            ByVal TemplateHandle As IntPtr) As IntPtr

        Public Declare Auto Function GetProcessId Lib "kernel32.dll" ( _
            ByVal Process As IntPtr) As Int32

        Public Declare Auto Function CreateProcess Lib "kernel32.dll" ( _
            ByVal ApplicationName As String, _
            ByVal CommandLine As String, _
            ByRef ProcessAttributes As SECURITY_ATTRIBUTES, _
            ByRef ThreadAttributes As SECURITY_ATTRIBUTES, _
            <MarshalAs(UnmanagedType.Bool)> ByVal InheritHandles As Boolean, _
            ByVal CreationFlags As CreationFlags, _
            ByVal Environment As IntPtr, _
            ByVal CurrentDirectory As String, _
            ByRef StartupInfo As STARTUPINFO, _
            ByRef ProcessInformation As PROCESS_INFORMATION _
            ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function CloseHandle Lib "kernel32.dll" ( _
            ByVal hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function WaitForSingleObject Lib "kernel32.dll" ( _
            ByVal hHandle As IntPtr, _
            ByVal Timeout As Int32) As WaitResult

        Public Declare Auto Function TerminateProcess Lib "kernel32.dll" ( _
            ByVal hProcess As IntPtr, _
            ByVal ExitCode As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function TerminateThread Lib "kernel32.dll" ( _
            ByVal hThread As IntPtr, _
            ByVal ExitCode As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function GetExitCodeProcess Lib "kernel32.dll" ( _
            ByVal hProcess As IntPtr, _
            ByRef ExitCode As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function CreateJobObject Lib "kernel32.dll" ( _
            ByRef JobAttributes As SECURITY_ATTRIBUTES, _
            ByVal Name As String) As IntPtr

        Public Declare Auto Function AssignProcessToJobObject Lib "kernel32.dll" ( _
            ByVal hJob As IntPtr, _
            ByVal hProcess As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function QueryInformationJobObject Lib "kernel32.dll" ( _
            ByVal hJob As IntPtr, _
            ByVal JobObjectInfoClass As JobObjectInfoClass, _
            ByRef lpJobObjectInfo As JOBOBJECT_EXTENDED_LIMIT_INFORMATION, _
            ByVal cbJobObjectInfoLength As Int32, _
            ByRef lpReturnLength As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function QueryInformationJobObject Lib "kernel32.dll" ( _
            ByVal hJob As IntPtr, _
            ByVal JobObjectInfoClass As JobObjectInfoClass, _
            ByRef lpJobObjectInfo As JOBOBJECT_BASIC_UI_RESTRICTIONS, _
            ByVal cbJobObjectInfoLength As Int32, _
            ByRef lpReturnLength As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function SetInformationJobObject Lib "kernel32.dll" ( _
            ByVal hJob As IntPtr, _
            ByVal JobObjectInfoClass As JobObjectInfoClass, _
            ByRef lpJobObjectInfo As JOBOBJECT_EXTENDED_LIMIT_INFORMATION, _
            ByVal cbJobObjectInfoLength As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function SetInformationJobObject Lib "kernel32.dll" ( _
            ByVal hJob As IntPtr, _
            ByVal JobObjectInfoClass As JobObjectInfoClass, _
            ByRef lpJobObjectInfo As JOBOBJECT_BASIC_UI_RESTRICTIONS, _
            ByVal cbJobObjectInfoLength As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function ResumeThread Lib "kernel32.dll" ( _
            ByVal hThread As IntPtr) As Int32

        Public Declare Auto Function GetProcessTimes Lib "kernel32.dll" ( _
            ByVal hProcess As IntPtr, _
            ByRef CreationTime As Int64, _
            ByRef ExitTime As Int64, _
            ByRef KernelTime As Int64, _
            ByRef UserTime As Int64) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function ReadProcessMemory Lib "kernel32.dll" ( _
            ByVal hProcess As IntPtr, _
            ByVal lpBaseAddress As IntPtr, _
            ByVal Buffer As IntPtr, _
            ByVal nSize As IntPtr, _
            ByRef NumberOfBytesRead As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function WriteProcessMemory Lib "kernel32.dll" ( _
            ByVal hProcess As IntPtr, _
            ByVal lpBaseAddress As IntPtr, _
            ByVal Buffer As IntPtr, _
            ByVal nSize As IntPtr, _
            ByRef NumberOfBytesWritten As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function TerminateJobObject Lib "kernel32.dll" ( _
            ByVal hJob As IntPtr, _
            ByVal uExitCode As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function OpenProcess Lib "kernel32.dll" ( _
            ByVal dwDesiredAccess As ProcessAccess, _
            <MarshalAs(UnmanagedType.Bool)> ByVal bInheritHandle As Boolean, _
            ByVal dwProcessId As Int32) As IntPtr

        Public Declare Auto Function CreatePipe Lib "kernel32.dll" ( _
            ByRef hReadPipe As IntPtr, _
            ByRef hWritePipe As IntPtr, _
            ByRef PipeAttributes As SECURITY_ATTRIBUTES, _
            ByVal nSize As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function ReadFile Lib "kernel32.dll" ( _
            ByVal hFile As IntPtr, _
            ByRef lpBuffer As Byte, _
            ByVal NumberOfBytesToRead As Int32, _
            ByRef NumberOfBytesRead As Int32, _
            ByRef lpOverlapped As NativeOverlapped) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function WriteFile Lib "kernel32.dll" ( _
            ByVal hFile As IntPtr, _
            ByRef lpBuffer As Byte, _
            ByVal NumberOfBytesToWrite As Int32, _
            ByRef NumberOfBytesWritten As Int32, _
            ByRef lpOverlapped As NativeOverlapped) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function DuplicateHandle Lib "kernel32.dll" ( _
            ByVal hSourceProcessHandle As IntPtr, _
            ByVal hSourceHandle As IntPtr, _
            ByVal hTargetProcessHandle As IntPtr, _
            ByRef hTargetHandle As IntPtr, _
            ByVal dwDesiredAccess As Int32, _
            <MarshalAs(UnmanagedType.Bool)> ByVal bInheritHandle As Boolean, _
            ByVal dwOptions As DuplicateOption) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function GetCurrentProcess Lib "kernel32.dll" () As IntPtr

        Public Declare Auto Function CreateNamedPipe Lib "kernel32.dll" ( _
            ByVal Name As String, _
            ByVal OpenMode As NamedPipeOpenMode, _
            ByVal PipeMode As NamedPipeMode, _
            ByVal nMaxInstances As NamedPipeMaxInstances, _
            ByVal nOutBufferSize As Int32,
            ByVal nInBufferSize As Int32,
            ByVal nDefaultTimeOut As Int32, _
            ByRef SecurityAttributes As SECURITY_ATTRIBUTES) As IntPtr

        Public Declare Auto Function ConnectNamedPipe Lib "kernel32.dll" ( _
            ByVal NamedPipeHandle As IntPtr, _
            ByRef lpOverlapped As NativeOverlapped) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Auto Function IsWow64Process Lib "kernel32.dll" ( _
            ByVal ProcessHandle As IntPtr, _
            <MarshalAs(UnmanagedType.Bool)> ByRef Wow64Process As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Enum CreateFileAccess As Int32
            GENERIC_ALL = &H10000000
            GENERIC_READ = &H80000000
            GENERIC_WRITE = &H40000000
            GENERIC_EXECUTE = &H20000000
        End Enum

        Public Enum CreateFileShare As Int32
            FILE_SHARE_NONE = &H0
            FILE_SHARE_DELETE = &H4
            FILE_SHARE_READ = &H1
            FILE_SHARE_WRITE = &H2
        End Enum

        Public Enum CreateFileDisposition As Int32
            CREATE_ALWAYS = 2
            CREATE_NEW = 1
            OPEN_ALWAYS = 4
            OPEN_EXISTING = 3
            TRUNCATE_EXISTING = 5
        End Enum

        Public Enum CreateFileFlags As Int32
            FILE_FLAG_BACKUP_SEMANTICS = &H2000000
            FILE_FLAG_DELETE_ON_CLOSE = &H4000000
            FILE_FLAG_NO_BUFFERING = &H20000000
            FILE_FLAG_OPEN_NO_RECALL = &H100000
            FILE_FLAG_OPEN_REPARSE_POINT = &H200000
            FILE_FLAG_OVERLAPPED = &H40000000
            FILE_FLAG_POSIX_SEMANTICS = &H1000000
            FILE_FLAG_RANDOM_ACCESS = &H10000000
            FILE_FLAG_SEQUENTIAL_SCAN = &H8000000
            FILE_FLAG_WRITE_THROUGH = &H80000000
        End Enum

        Public Enum DuplicateOption As Int32
            DUPLICATE_CLOSE_SOURCE = 1
            DUPLICATE_SAME_ACCESS = 2
        End Enum

        Public Structure STARTUPINFO
            Dim cb As Int32
            Dim Reserved As IntPtr
            <MarshalAs(UnmanagedType.LPTStr)> _
            Dim Desktop As String
            <MarshalAs(UnmanagedType.LPTStr)> _
            Dim Title As String
            Dim dwX As Int32
            Dim dwY As Int32
            Dim dwXSize As Int32
            Dim dwYSize As Int32
            Dim dwXCountChars As Int32
            Dim dwYCountChars As Int32
            Dim dwFillAttribute As Int32
            Dim dwFlags As StartupFlags
            Dim wShowWindow As Int16
            Dim cbReserved2 As Int16
            Dim lpReserved2 As IntPtr
            Dim hStdInput As IntPtr
            Dim hStdOutput As IntPtr
            Dim hStdError As IntPtr
        End Structure

        Public Structure PROCESS_INFORMATION
            Dim hProcess As IntPtr
            Dim hThread As IntPtr
            Dim dwProcessId As Int32
            Dim dwThreadId As Int32
        End Structure

        Public Structure SECURITY_ATTRIBUTES
            Dim nLength As Int32
            Dim lpSecurityDescriptor As IntPtr
            <MarshalAs(UnmanagedType.Bool)> Dim bInheritHandle As Boolean
        End Structure

        Public Structure JOBOBJECT_BASIC_LIMIT_INFORMATION
            Dim PerProcessUserTimeLimit As Int64
            Dim PerJobUserTimeLimit As Int64
            Dim LimitFlags As JobObjectLimitFlags
            Dim MinimumWorkingSetSize As IntPtr
            Dim MaximumWorkingSetSize As IntPtr
            Dim ActiveProcessLimit As Int32
            Dim Affinity As IntPtr
            Dim PriorityClass As PriorityClass
            Dim SchedulingClass As Int32
        End Structure

        Public Structure IO_COUNTERS
            Dim ReadOperationCount As Int64
            Dim WriteOperationCount As Int64
            Dim OtherOperationCount As Int64
            Dim ReadTransferCount As Int64
            Dim WriteTransferCount As Int64
            Dim OtherTransferCount As Int64
        End Structure

        Public Structure JOBOBJECT_EXTENDED_LIMIT_INFORMATION
            Dim BasicLimitInformation As JOBOBJECT_BASIC_LIMIT_INFORMATION
            Dim IoInfo As IO_COUNTERS
            Dim ProcessMemoryLimit As IntPtr
            Dim JobMemoryLimit As IntPtr
            Dim PeakProcessMemoryUsed As IntPtr
            Dim PeakJobMemoryUsed As IntPtr
        End Structure

        Public Enum PriorityClass As Int32
            None = 0
            Normal = &H20
            Idle = &H40
            High = &H80
            RealTime = &H100
            BelowNormal = &H4000
            AboveNormal = &H8000
        End Enum

        Public Enum WaitResult As Int32
            WAIT_OBJECT_0 = 0
            WAIT_TIMEOUT = &H102
            WAIT_FAILED = -1
        End Enum

        ' http://msdn.microsoft.com/en-us/library/ms684863(v=VS.85).aspx
        Public Enum CreationFlags As Int32
            CREATE_BREAKAWAY_FROM_JOB = &H1000000
            CREATE_DEFAULT_ERROR_MODE = &H4000000
            CREATE_NEW_CONSOLE = &H10
            CREATE_NEW_PROCESS_GROUP = &H200
            CREATE_NO_WINDOW = &H8000000
            CREATE_PROTECTED_PROCESS = &H40000
            CREATE_PRESERVE_CODE_AUTHZ_LEVEL = &H2000000
            CREATE_SEPARATE_WOW_VDM = &H800
            CREATE_SHARED_WOW_VDM = &H1000
            CREATE_SUSPENDED = &H4
            CREATE_UNICODE_ENVIRONMENT = &H400
            DEBUG_ONLY_THIS_PROCESS = &H2
            DEBUG_PROCESS = &H1
            DETACHED_PROCESS = &H8
            EXTENDED_STARTUPINFO_PRESENT = &H80000
            INHERIT_PARENT_AFFINITY = &H10000
        End Enum

        ' http://msdn.microsoft.com/en-us/library/ms686331(v=VS.85).aspx
        Public Enum StartupFlags As Int32
            STARTF_FORCEONFEEDBACK = &H40
            STARTF_FORCEOFFFEEDBACK = &H80
            STARTF_PREVENTPINNING = &H2000
            STARTF_RUNFULLSCREEN = &H20
            STARTF_TITLEISAPPID = &H1000
            STARTF_TITLEISLINKNAME = &H800
            STARTF_USECOUNTCHARS = &H8
            STARTF_USEFILLATTRIBUTE = &H10
            STARTF_USEHOTKEY = &H200
            STARTF_USEPOSITION = &H4
            STARTF_USESHOWWINDOW = &H1
            STARTF_USESIZE = &H2
            STARTF_USESTDHANDLES = &H100
        End Enum

        Public Enum JobObjectLimitFlags As Int32
            JOB_OBJECT_LIMIT_ACTIVE_PROCESS = &H8
            JOB_OBJECT_LIMIT_AFFINITY = &H10
            JOB_OBJECT_LIMIT_BREAKAWAY_OK = &H800
            JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION = &H400
            JOB_OBJECT_LIMIT_JOB_MEMORY = &H200
            JOB_OBJECT_LIMIT_JOB_TIME = &H4
            JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = &H2000
            JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME = &H40
            JOB_OBJECT_LIMIT_PRIORITY_CLASS = &H20
            JOB_OBJECT_LIMIT_PROCESS_MEMORY = &H100
            JOB_OBJECT_LIMIT_PROCESS_TIME = &H2
            JOB_OBJECT_LIMIT_SCHEDULING_CLASS = &H80
            JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK = &H1000
            JOB_OBJECT_LIMIT_WORKINGSET = &H1
        End Enum

        Public Enum JobObjectInfoClass As Int32
            JobObjectAssociateCompletionPortInformation = 7
            JobObjectBasicLimitInformation = 2
            JobObjectBasicUIRestrictions = 4
            JobObjectEndOfJobTimeInformation = 6
            JobObjectExtendedLimitInformation = 9
        End Enum

        Public Structure JOBOBJECT_BASIC_UI_RESTRICTIONS
            Dim UIRestrictionClass As UIRestrictionClass
        End Structure

        Public Enum UIRestrictionClass As Int32
            JOB_OBJECT_UILIMIT_DESKTOP = &H40
            JOB_OBJECT_UILIMIT_DISPLAYSETTINGS = &H10
            JOB_OBJECT_UILIMIT_EXITWINDOWS = &H80
            JOB_OBJECT_UILIMIT_GLOBALATOMS = &H20
            JOB_OBJECT_UILIMIT_HANDLES = &H1
            JOB_OBJECT_UILIMIT_READCLIPBOARD = &H2
            JOB_OBJECT_UILIMIT_SYSTEMPARAMETERS = &H8
            JOB_OBJECT_UILIMIT_WRITECLIPBOARD = &H4
        End Enum

        Public Enum ProcessAccess As Int32
            PROCESS_ALL_ACCESS = &H1F0FFF
        End Enum

        Public Enum NamedPipeOpenMode As Int32
            PIPE_ACCESS_DUPLEX = &H3
            PIPE_ACCESS_INBOUND = &H1
            PIPE_ACCESS_OUTBOUND = &H2
            FILE_FLAG_FIRST_PIPE_INSTANCE = &H80000
            FILE_FLAG_WRITE_THROUGH = &H80000000
            FILE_FLAG_OVERLAPPED = &H40000000
            WRITE_DAC = &H40000
            WRITE_OWNER = &H80000
            ACCESS_SYSTEM_SECURITY = &H1000000
        End Enum

        Public Enum NamedPipeMode As Int32
            PIPE_TYPE_BYTE = &H0
            PIPE_TYPE_MESSAGE = &H4
            PIPE_READMODE_BYTE = &H0
            PIPE_READMODE_MESSAGE = &H2
            PIPE_WAIT = &H0
            PIPE_NOWAIT = &H1
        End Enum

        Public Enum NamedPipeMaxInstances As Int32
            PIPE_UNLIMITED_INSTANCES = 255
        End Enum
#End Region

#Region "ntdll.dll"
        Public Declare Auto Function NtQuerySystemInformation Lib "ntdll.dll" ( _
            ByVal SystemInformationClass As SYSTEM_INFORMATION_CLASS, _
            ByVal SystemInformation As IntPtr, _
            ByVal SystemInformationLength As Int32, _
            ByRef ReturnLength As Int32) As NTSTATUS

        Public Declare Auto Function NtQueryInformationProcess Lib "ntdll.dll" ( _
            ByVal ProcessHandle As IntPtr, _
            ByVal ProcessInformationClass As PROCESS_INFORMATION_CLASS, _
            ByRef ProcessInformation As PROCESS_BASIC_INFORMATION, _
            ByVal ProcessInformationLength As Int32, _
            ByRef ReturnLength As Int32) As NTSTATUS

        Public Declare Auto Function NtSetInformationProcess Lib "ntdll.dll" ( _
            ByVal ProcessHandle As IntPtr, _
            ByVal ProcessInformationClass As PROCESS_INFORMATION_CLASS, _
            ByRef ProcessInformation As PROCESS_ACCESS_TOKEN, _
            ByVal ProcessInformationLength As Int32) As NTSTATUS

        Public Declare Function NtCreateDebugObject Lib "ntdll.dll" ( _
            ByRef DebugObjectHandle As IntPtr, _
            ByVal DesiredAccess As DebugObjectAccess, _
            ByVal ObjectAttributes As IntPtr, _
            ByVal Flags As DebugObjectFlags) As NTSTATUS

        Public Declare Function NtClose Lib "ntdll.dll" ( _
            ByVal Handle As IntPtr) As NTSTATUS

        ' Need PROCESS_SET_PORT for the process object
        ' and DEBUG_PROCESS_ASSIGN for the debug object
        Public Declare Function NtDebugActiveProcess Lib "ntdll.dll" ( _
            ByVal ProcessHandle As IntPtr, _
            ByVal DebugObjectHandle As IntPtr) As NTSTATUS

        Public Declare Function NtRemoveProcessDebug Lib "ntdll.dll" ( _
            ByVal ProcessHandle As IntPtr, _
            ByVal DebugObjectHandle As IntPtr) As NTSTATUS

        Public Declare Function NtWaitForDebugEvent Lib "ntdll.dll" ( _
            ByVal DebugObjectHandle As IntPtr, _
            ByVal Alertable As Boolean, _
            ByVal Timeout As IntPtr, _
            ByVal WaitStateChange As IntPtr) As NTSTATUS

        Public Declare Function NtDebugContinue Lib "ntdll.dll" ( _
            ByVal DebugObjectHandle As IntPtr, _
            ByRef ClientId As CLIENT_ID, _
            ByVal ContinueStatus As NTSTATUS) As NTSTATUS

        Public Declare Auto Function RtlNtStatusToDosError Lib "ntdll.dll" ( _
            ByVal Status As NTSTATUS) As Int32

        Public Structure CLIENT_ID
            Dim UniqueProcess As IntPtr
            Dim UniqueThread As IntPtr
        End Structure

        Public Enum DebugObjectAccess As Int32
            DEBUG_OBJECT_WAIT_STATE_CHANGE = &H1
            DEBUG_OBJECT_ADD_REMOVE_PROCESS = &H2
            DEBUG_OBJECT_SET_INFORMATION = &H4
            DEBUG_OBJECT_ALL_ACCESS = &H1F000F
        End Enum

        Public Enum DebugObjectFlags As Int32
            DEBUG_OBJECT_KILL_ON_CLOSE = &H1
        End Enum

        Public Structure PROCESS_BASIC_INFORMATION
            Dim Reserved0 As IntPtr
            Dim PebBaseAddress As IntPtr
            Dim Reserved1 As IntPtr
            Dim Reserved2 As IntPtr
            Dim UniqueProcessId As IntPtr
            Dim Reserved3 As IntPtr
        End Structure

        Public Structure PROCESS_ACCESS_TOKEN
            Dim Token As IntPtr
            Dim Thread As IntPtr
        End Structure

        Public Enum SYSTEM_INFORMATION_CLASS As Int32
            SystemPerformanceInformation = 2
        End Enum

        Public Enum PROCESS_INFORMATION_CLASS As Int32
            ProcessBasicInformation = 0
        End Enum

        Public Enum ExceptionCode As Int32
            EXCEPTION_ACCESS_VIOLATION = &HC0000005
            EXCEPTION_DATATYPE_MISALIGNMENT = &H80000002
            EXCEPTION_BREAKPOINT = &H80000003
            EXCEPTION_SINGLE_STEP = &H80000004
            EXCEPTION_ARRAY_BOUNDS_EXCEEDED = &HC000008C
            EXCEPTION_FLT_DENORMAL_OPERAND = &HC000008D
            EXCEPTION_FLT_DIVIDE_BY_ZERO = &HC000008E
            EXCEPTION_FLT_INEXACT_RESULT = &HC000008F
            EXCEPTION_FLT_INVALID_OPERATION = &HC0000090
            EXCEPTION_FLT_OVERFLOW = &HC0000091
            EXCEPTION_FLT_STACK_CHECK = &HC0000092
            EXCEPTION_FLT_UNDERFLOW = &HC0000093
            EXCEPTION_INT_DIVIDE_BY_ZERO = &HC0000094
            EXCEPTION_INT_OVERFLOW = &HC0000095
            EXCEPTION_PRIVILEGED_INSTRUCTION = &HC0000096
            EXCEPTION_IN_PAGE_ERROR = &HC0000006
            EXCEPTION_ILLEGAL_INSTRUCTION = &HC000001D
            EXCEPTION_NONCONTINUABLE_EXCEPTION = &HC0000025
            EXCEPTION_STACK_OVERFLOW = &HC00000FD
            EXCEPTION_INVALID_DISPOSITION = &HC0000026
            EXCEPTION_GUARD_PAGE_VIOLATION = &H80000001
            EXCEPTION_INVALID_HANDLE = &HC0000008
            EXCEPTION_CONTROL_C_EXIT = &HC000013A
        End Enum

        Public Structure EXCEPTION_RECORD
            Dim ExceptionCode As ExceptionCode
            Dim ExceptionFlags As Int32
            Dim ExceptionRecord As IntPtr
            Dim ExceptionAddress As IntPtr
            Dim NumberParameters As Int32

            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=15)> _
            Dim ExceptionInformation As IntPtr()
        End Structure

        Public Enum NTSTATUS As Int32
            STATUS_SUCCESS = &H0
            STATUS_DEBUGGER_INACTIVE = &HC0000354
            DBG_EXCEPTION_HANDLED = &H10001
            DBG_EXCEPTION_NOT_HANDLED = &H80010001
            DBG_TERMINATE_THREAD = &H40010003
            DBG_TERMINATE_PROCESS = &H40010004
            DBG_CONTINUE = &H10002
        End Enum

        Public Function NT_SUCCESS(ByVal Status As NTSTATUS) As Boolean
            Return Status >= 0
        End Function
#End Region

        Public Const INVALID_HANDLE_VALUE As Int32 = -1
        Public Const ERROR_INSUFFICIENT_BUFFER As Int32 = 122
        Public Const ERROR_PIPE_CONNECTED As Int32 = 535
        Public Const ERROR_IO_PENDING As Int32 = 997
        Public Const ERROR_SERVICE_DOES_NOT_EXIST As Int32 = 1060
        Public Const ERROR_SERVICE_NOT_ACTIVE As Int32 = 1062
        Public Const ERROR_SERVICE_MARKED_FOR_DELETE As Int32 = 1072
        Public Const ERROR_NOT_ENOUGH_QUOTA As Int32 = 1816
        Public Const SE_GROUP_LOGON_ID As Int32 = &HC0000000
        Public Const SECURITY_DESCRIPTOR_REVISION As Int32 = 1
        Public Const ACL_REVISION As Int32 = 2

        Public Sub Win32True(ByVal Value As Boolean)
            If Not Value Then
                Throw New Win32Exception()
            End If
        End Sub

        Public Sub NtSuccess(ByVal Status As NTSTATUS)
            If Not NT_SUCCESS(Status) Then
                Throw New Win32Exception(RtlNtStatusToDosError(Status))
            End If
        End Sub
    End Module
End Namespace
