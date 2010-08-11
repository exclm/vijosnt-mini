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
#End Region

#Region "kernel32.dll"
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

        Public Enum PROCESS_INFORMATION_CLASS
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

        Public Const ERROR_INSUFFICIENT_BUFFER As Int32 = 122
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
