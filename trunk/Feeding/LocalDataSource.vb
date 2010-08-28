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
            Using Stream As New MemoryStream()
                Using Writer As New BinaryWriter(Stream)
                    If Result.Warning Is Nothing Then
                        Writer.Write(String.Empty)
                    Else
                        Writer.Write(Result.Warning)
                    End If
                    If Result.Entries IsNot Nothing Then
                        For Each Entry As TestResultEntry In Result.Entries
                            Writer.Write(Entry.Index)
                            Writer.Write(Entry.Flag)
                            Writer.Write(Entry.Score)
                            Writer.Write(Entry.TimeUsage)
                            Writer.Write(Entry.MemoryUsage)
                            If Entry.Warning Is Nothing Then
                                Writer.Write(String.Empty)
                            Else
                                Writer.Write(Entry.Warning)
                            End If
                        Next
                    End If
                End Using
                Record.UpdateFinal(Id, Result.Flag.ToString(), Result.Score, Result.TimeUsage, Result.MemoryUsage, Stream.ToArray())
            End Using
        End Sub
    End Class
End Namespace
