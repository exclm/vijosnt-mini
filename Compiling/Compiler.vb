Imports VijosNT.Utility

Namespace Compiling
    Friend MustInherit Class Compiler
        Public MustOverride Function CreateInstance(ByVal SourceCode As Stream) As CompilerInstance
        Public MustOverride ReadOnly Property ApplicationName() As String
        Public MustOverride ReadOnly Property CommandLine() As String
        Public MustOverride ReadOnly Property EnvironmentVariables() As IEnumerable(Of String)
        Public MustOverride ReadOnly Property TimeQuota() As Nullable(Of Int64)
        Public MustOverride ReadOnly Property MemoryQuota() As Nullable(Of Int64)
        Public MustOverride ReadOnly Property ActiveProcessQuota() As Nullable(Of Int32)
        Public MustOverride ReadOnly Property SourceFileName() As String
        Public MustOverride ReadOnly Property TargetFileName() As String
        Public MustOverride ReadOnly Property TargetApplicationName() As String
        Public MustOverride ReadOnly Property TargetCommandLine() As String
    End Class
End Namespace
