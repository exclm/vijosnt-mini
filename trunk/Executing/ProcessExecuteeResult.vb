﻿Imports VijosNT.Win32

Namespace Executing
    Friend Structure ProcessExecuteeResult
        Dim State As Object
        Dim ExitStatus As Nullable(Of NTSTATUS)
        Dim TimeQuotaUsage As Int64
        Dim MemoryQuotaUsage As Int64
        Dim Exception As Nullable(Of EXCEPTION_RECORD)
    End Structure
End Namespace
