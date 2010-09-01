Imports VijosNT.Win32

Namespace Testing
    Friend Class TestCaseExecuteeResult
        Public State As Object
        Public Index As Int32
        Public ExitStatus As Nullable(Of NTSTATUS)
        Public StdErrorMessage As String
        Public Score As Nullable(Of Int32)
        Public TimeQuotaUsage As Int64
        Public MemoryQuotaUsage As Int64
        Public Exception As Nullable(Of EXCEPTION_RECORD)
    End Class
End Namespace
