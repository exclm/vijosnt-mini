Namespace Feeding
    Friend Class DataSourceRecord
        Public Sub New(ByVal FileName As String, ByVal SourceCode As String)
            Me.FileName = FileName
            Me.SourceCode = SourceCode
        End Sub

        Public FileName As String
        Public SourceCode As String
    End Class
End Namespace
