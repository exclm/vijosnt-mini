Namespace LocalDb
    Friend Class Record
        Private Shared m_SelectHeaderCommand As SQLiteCommand
        Private Shared m_SelectPendingCommand As SQLiteCommand
        Private Shared m_SelectCodeCommand As SQLiteCommand
        Private Shared m_InsertCommand As SQLiteCommand
        Private Shared m_UpdateTakenCommand As SQLiteCommand
        Private Shared m_UpdateUntakeCommand As SQLiteCommand
        Private Shared m_UpdateFinalCommand As SQLiteCommand

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
                m_SelectPendingCommand = Database.CreateCommand( _
                    "SELECT Id FROM Record WHERE Taken = 0 ORDER BY Id")
                m_SelectCodeCommand = Database.CreateCommand( _
                    "SELECT FileName, SourceCode FROM Record WHERE Id = @Id")
                m_InsertCommand = Database.CreateCommand( _
                    "INSERT INTO Record (Id, FileName, SourceCode, Date, Taken, Flag, Score, TimeUsage, MemoryUsage, Details) " & _
                    "VALUES (NULL, @FileName, @SourceCode, @Date, 0, 'None', NULL, NULL, NULL, NULL)")
                m_UpdateTakenCommand = Database.CreateCommand( _
                    "UPDATE Record SET Taken = -1 WHERE Id = @Id AND Taken = 0")
                m_UpdateUntakeCommand = Database.CreateCommand( _
                    "UPDATE Record SET Taken = 0 WHERE Id = @Id")
                m_UpdateFinalCommand = Database.CreateCommand( _
                    "UPDATE Record SET Taken = 1, Flag = @Flag, Score = @Score, TimeUsage = @TimeUsage, MemoryUsage = @MemoryUsage, Details = @Details WHERE Id = @Id")
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

        Public Shared Function Acquire() As Nullable(Of Int32)
            Using Command0 As SQLiteCommand = m_SelectPendingCommand.Clone(), _
                Reader As SQLiteDataReader = Command0.ExecuteReader()
                While Reader.Read()
                    Using Command1 As SQLiteCommand = m_UpdateTakenCommand.Clone()
                        Dim Id As Int32 = Reader("Id")
                        Command1.Parameters.AddWithValue("@Id", Id)
                        If Command1.ExecuteNonQuery() <> 0 Then
                            Return Id
                        End If
                    End Using
                End While
            End Using
            Return Nothing
        End Function

        Public Shared Sub Release(ByVal Id As Int32)
            Using Command As SQLiteCommand = m_UpdateUntakeCommand.Clone()
                Command.Parameters.AddWithValue("@Id", Id)
                Command.ExecuteNonQuery()
            End Using
        End Sub

        Public Shared Function GetSourceCode(ByVal Id As Int32) As IDataReader
            Using Command As SQLiteCommand = m_SelectCodeCommand.Clone()
                Command.Parameters.AddWithValue("@Id", Id)
                Return Command.ExecuteReader()
            End Using
        End Function

        Public Shared Sub UpdateFinal(ByVal Id As Int32, ByVal Flag As String, ByVal Score As Int32, ByVal TimeUsage As Int64, ByVal MemoryUsage As Int64, ByVal Details As Byte())
            Using Command As SQLiteCommand = m_UpdateFinalCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@Id", Id)
                    .AddWithValue("@Flag", Flag)
                    .AddWithValue("@Score", Score)
                    .AddWithValue("@TimeUsage", TimeUsage)
                    .AddWithValue("@MemoryUsage", MemoryUsage)
                    .AddWithValue("@Details", Details)
                End With
                Command.ExecuteNonQuery()
            End Using
        End Sub
    End Class
End Namespace
