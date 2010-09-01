Imports VijosNT.Win32

Namespace Executing
    Friend Class ProcessExecuteeResult
        Public State As Object
        Public ExitStatus As Nullable(Of NTSTATUS)
        Public TimeQuotaUsage As Int64
        Public MemoryQuotaUsage As Int64
        Public Exception As Nullable(Of EXCEPTION_RECORD)
    End Class
End Namespace
