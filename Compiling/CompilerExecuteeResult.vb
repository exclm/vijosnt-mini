﻿Imports VijosNT.Win32

Namespace Compiling
    Friend Class CompilerExecuteeResult
        Public State As Object
        Public Target As Target
        Public ExitStatus As NTSTATUS?
        Public StdErrorMessage As String
        Public TimeQuotaUsage As Int64
        Public MemoryQuotaUsage As Int64
        Public Exception As EXCEPTION_RECORD?
    End Class
End Namespace
