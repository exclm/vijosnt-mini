Imports VijosNT.Utility

Namespace Compiling
    Friend MustInherit Class Compiler
        Public MustOverride Function CreateInstance(ByVal SourceCode As Stream) As CompilerInstance
        Public MustOverride ReadOnly Property ApplicationName() As String
        Public MustOverride ReadOnly Property CommandLine() As String
        Public MustOverride ReadOnly Property TimeQuota() As Nullable(Of Int64)
        Public MustOverride ReadOnly Property MemoryQuota() As Nullable(Of Int64)
        Public MustOverride ReadOnly Property ActiveProcessQuota() As Nullable(Of Int32)
    End Class
End Namespace
