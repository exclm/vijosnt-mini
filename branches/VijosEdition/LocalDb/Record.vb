Imports VijosNT.Testing

Namespace LocalDb
    Friend Class Record
        Private Shared m_SelectReportCommand As SQLiteCommand
        Private Shared m_SelectHeaderCommand As SQLiteCommand
        Private Shared m_SelectCodeCommand As SQLiteCommand
        Private Shared m_InsertCommand As SQLiteCommand
        Private Shared m_DeleteCommand As SQLiteCommand
        Private Shared m_DeleteOneCommand As SQLiteCommand

        Shared Sub New()
            Using Command As SQLiteCommand = Database.CreateCommand( _
                    "CREATE TABLE IF NOT EXISTS Record (" & _
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                    "FileName TEXT, " & _
                    "SourceCode TEXT, " & _
                    "Date INTEGER, " & _
                    "Taken INTEGER, " & _
                    "Flag TEXT, " & _
                    "Score INTEGER, " & _
                    "TimeUsage INTEGER, " & _
                    "MemoryUsage INTEGER, " & _
                    "Details BLOB)")
                Command.ExecuteNonQuery()
            End Using

            m_SelectReportCommand = Database.CreateCommand( _
                "SELECT Id, FileName, SourceCode, Date, Flag, Score, TimeUsage, MemoryUsage, Details FROM Record WHERE Id = @Id")
            m_SelectHeaderCommand = Database.CreateCommand( _
                "SELECT Id, FileName, Date, Taken, Flag, Score, TimeUsage, MemoryUsage FROM Record ORDER BY Id DESC")
            m_SelectCodeCommand = Database.CreateCommand( _
                "SELECT FileName, SourceCode FROM Record WHERE Id = @Id")
            m_InsertCommand = Database.CreateCommand( _
                "INSERT INTO Record (Id, FileName, SourceCode, Date, Taken, Flag, Score, TimeUsage, MemoryUsage, Details) " & _
                "VALUES (NULL, @FileName, @SourceCode, @Date, 0, @Flag, @Score, @TimeUsage, @MemoryUsage, @Details)")
            m_DeleteCommand = Database.CreateCommand( _
                "DELETE FROM Record")
            m_DeleteOneCommand = Database.CreateCommand( _
                "DELETE FROM Record WHERE Id = @Id")
        End Sub

        Public Shared Sub Add(ByVal FileName As String, ByVal SourceCode As String, ByVal Flag As String, ByVal Score As Int32, ByVal TimeUsage As Int64, ByVal MemoryUsage As Int64, ByVal Details As Byte())
            Using Command As SQLiteCommand = m_InsertCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@FileName", FileName)
                    .AddWithValue("@SourceCode", SourceCode)
                    .AddWithValue("@Date", Date.Now.Ticks)
                    .AddWithValue("@Flag", Flag)
                    .AddWithValue("@Score", Score)
                    .AddWithValue("@TimeUsage", TimeUsage)
                    .AddWithValue("@MemoryUsage", MemoryUsage)
                    .AddWithValue("@Details", Details)
                End With
                Command.ExecuteNonQuery()
            End Using
        End Sub

        Public Shared Sub Add(ByVal FileName As String, ByVal SourceCode As String, ByVal Result As TestResult)
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
                Record.Add(FileName, SourceCode, Result.Flag.ToString(), Result.Score, Result.TimeUsage, Result.MemoryUsage, Stream.ToArray())
            End Using
        End Sub

        Public Shared Function GetHeaders() As IDataReader
            Using Command As SQLiteCommand = m_SelectHeaderCommand.Clone()
                Return Command.ExecuteReader()
            End Using
        End Function

        Public Shared Function GetReport(ByVal Id As Int32) As IDataReader
            Using Command As SQLiteCommand = m_SelectReportCommand.Clone()
                Command.Parameters.AddWithValue("@Id", Id)
                Return Command.ExecuteReader()
            End Using
        End Function

        Public Shared Function GetSourceCode(ByVal Id As Int32) As IDataReader
            Using Command As SQLiteCommand = m_SelectCodeCommand.Clone()
                Command.Parameters.AddWithValue("@Id", Id)
                Return Command.ExecuteReader()
            End Using
        End Function

        Public Shared Sub Clear()
            Using Command As SQLiteCommand = m_DeleteCommand.Clone()
                Command.ExecuteNonQuery()
            End Using
        End Sub

        Public Shared Sub Remove(ByVal Id As Int32)
            Using Command As SQLiteCommand = m_DeleteOneCommand.Clone()
                Command.Parameters.AddWithValue("@Id", Id)
                Command.ExecuteNonQuery()
            End Using
        End Sub
    End Class
End Namespace
