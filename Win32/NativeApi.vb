Module NativeApi
    Public Declare Function NtQueryInformationProcess Lib "ntdll.dll" ( _
        ByVal ProcessHandle As IntPtr, _
        ByVal ProcessInformationClass As PROCESSINFOCLASS, _
        ByRef ProcessInformation As PROCESS_BASIC_INFORMATION, _
        ByVal ProcessInformationLength As Int32, _
        ByRef ReturnLength As Int32) As NTSTATUS

    Public Structure PROCESS_BASIC_INFORMATION
        Dim Reserved0 As IntPtr
        Dim PebBaseAddress As IntPtr
        Dim Reserved1 As IntPtr
        Dim Reserved2 As IntPtr
        Dim UniqueProcessId As IntPtr
        Dim Reserved3 As IntPtr
    End Structure

    Public Enum PROCESSINFOCLASS
        ProcessBasicInformation = 0
    End Enum

    Public Enum NTSTATUS As Int32
        STATUS_SUCCESS = 0
    End Enum
End Module
