Namespace LocalDb
    Friend Class Record
        Private Shared m_SelectHeaderCommand As SQLiteCommand
        Private Shared m_InsertCommand As SQLiteCommand

        Shared Sub New()
            Try
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

                m_SelectHeaderCommand = Database.CreateCommand( _
                    "SELECT Id, FileName, Date, Taken, Flag, Score, TimeUsage, MemoryUsage FROM Record ORDER BY Id DESC")
                m_InsertCommand = Database.CreateCommand( _
                    "INSERT INTO Record (Id, FileName, SourceCode, Date, Taken, Flag, Score, TimeUsage, MemoryUsage, Details) " & _
                    "VALUES (NULL, @FileName, @SourceCode, @Date, 0, 'None', NULL, NULL, NULL, NULL)")
            Catch ex As Exception
                EventLog.WriteEntry(My.Resources.ServiceName, ex.ToString(), EventLogEntryType.Error)
                Environment.Exit(1)
            End Try
        End Sub

        Public Shared Sub Add(ByVal FileName As String, ByVal SourceCode As String)
            Using Command As SQLiteCommand = m_InsertCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@FileName", FileName)
                    .AddWithValue("@SourceCode", SourceCode)
                    .AddWithValue("@Date", Date.Now.Ticks)
                End With
                Command.ExecuteNonQuery()
            End Using
        End Sub

        Public Shared Function GetHeaders() As IDataReader
            Using Command As SQLiteCommand = m_SelectHeaderCommand.Clone()
                Return Command.ExecuteReader()
            End Using
        End Function
    End Class
End Namespace
