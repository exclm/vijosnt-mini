Imports VijosNT.LocalDb

Namespace Feeding
    Friend Class LocalDataSource
        Inherits DataSourceBase

        Public Overrides Function Take() As Nullable(Of DataSourceRecord)
            Dim Id As Nullable(Of Int32) = Record.Acquire()
            If Not Id.HasValue Then _
                Return Nothing
            Dim Result As DataSourceRecord
            Result.Id = Id
            Using Reader As IDataReader = Record.GetSourceCode(Id)
                If Not Reader.Read() Then _
                    Return Nothing
                Result.FileName = Reader("FileName")
                Result.SourceCode = Reader("SourceCode")
                Result.Tag = Nothing
            End Using
            Return Result
        End Function

        Public Overrides Sub Untake(ByVal Id As Int32)
            Record.Release(Id)
        End Sub

        Public Overrides Sub Untake(ByVal Id As Int32, ByVal Result As TestResult)
            ' TODO: Serialize details into byte array
            Record.UpdateFinal(Id, Result.Flag.ToString(), Result.Score, Result.TimeUsage, Result.MemoryUsage, Nothing)
        End Sub
    End Class
End Namespace
