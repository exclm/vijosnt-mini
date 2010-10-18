Imports VijosNT.Win32

Namespace Executing
    Friend Class ProcessExecuteeResult
        Public State As Object
        Public ExitStatus As NTSTATUS?
        Public TimeQuotaUsage As Int64
        Public MemoryQuotaUsage As Int64
        Public Exception As EXCEPTION_RECORD?
    End Class
End Namespace
