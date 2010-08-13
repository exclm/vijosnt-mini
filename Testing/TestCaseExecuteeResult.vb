Imports VijosNT.Win32

Namespace Testing
    Friend Structure TestCaseExecuteeResult
        Dim State As Object
        Dim Index As Int32
        Dim ExitStatus As Nullable(Of NTSTATUS)
        Dim Score As Nullable(Of Int32)
        Dim TimeQuotaUsage As Int64
        Dim MemoryQuotaUsage As Int64
        Dim Exception As Nullable(Of EXCEPTION_RECORD)
    End Structure
End Namespace
