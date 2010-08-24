Namespace Feeding
    Friend MustInherit Class DataSourceBase
        Public MustOverride Function Take() As Nullable(Of DataSourceRecord)
        Public MustOverride Sub Untake(ByVal Id As Int32)
        Public MustOverride Sub Untake(ByVal Id As Int32, ByVal Result As TestResult)
    End Class
End Namespace
