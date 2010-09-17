Namespace Feeding
    Friend MustInherit Class DataSourceBase
        Private m_Namespace As String

        Public Sub New(ByVal [Namespace] As String)
            m_Namespace = [Namespace]
        End Sub

        Public ReadOnly Property [Namespace]() As String
            Get
                Return m_Namespace
            End Get
        End Property

        Public MustOverride Function Take() As Nullable(Of DataSourceRecord)
        Public MustOverride Sub Untake(ByVal Id As Int32)
        Public MustOverride Sub Untake(ByVal Id As Int32, ByVal Result As TestResult)
    End Class
End Namespace
