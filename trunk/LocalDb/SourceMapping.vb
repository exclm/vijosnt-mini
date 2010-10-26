Namespace LocalDb
    Friend Class SourceMapping
        Private Shared m_SelectCommand As SQLiteCommand
        Private Shared m_SelectHeaderCommand As SQLiteCommand
        Private Shared m_SelectConfigCommand As SQLiteCommand
        Private Shared m_InsertCommand As SQLiteCommand
        Private Shared m_DeleteCommand As SQLiteCommand
        Private Shared m_UpdateCommand As SQLiteCommand
        Private Shared m_UpdateIdCommand As SQLiteCommand

        Shared Sub New()
            Using Command As SQLiteCommand = Database.CreateCommand( _
                "CREATE TABLE IF NOT EXISTS SourceMapping (" & _
                "Id INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                "ClassName TEXT, " & _
                "Namespace TEXT, " & _
                "Parameter TEXT, " & _
                "HttpAnnouncement TEXT)")
                Command.ExecuteNonQuery()
            End Using

            m_SelectCommand = Database.CreateCommand( _
                "SELECT * FROM SourceMapping")
            m_SelectHeaderCommand = Database.CreateCommand( _
                "SELECT Id, ClassName, Namespace, Parameter FROM SourceMapping")
            m_SelectConfigCommand = Database.CreateCommand( _
                "SELECT * FROM SourceMapping WHERE Id = @Id")
            m_InsertCommand = Database.CreateCommand( _
                "INSERT INTO SourceMapping (Id, ClassName, Namespace, Parameter, HttpAnnouncement) VALUES (NULL, @ClassName, @Namespace, @Parameter, @HttpAnnouncement)")
            m_DeleteCommand = Database.CreateCommand( _
                "DELETE FROM SourceMapping WHERE Id = @Id")
            m_UpdateCommand = Database.CreateCommand( _
                "UPDATE SourceMapping SET ClassName = @ClassName, Namespace = @Namespace, Parameter = @Parameter, HttpAnnouncement = @HttpAnnouncement WHERE Id = @Id")
            m_UpdateIdCommand = Database.CreateCommand( _
                "UPDATE CompilerMapping2 SET Id = @Id WHERE Id = @OriginalId")
        End Sub

        Public Shared Function GetAll() As IDataReader
            Using Command As SQLiteCommand = m_SelectCommand.Clone()
                Return Command.ExecuteReader()
            End Using
        End Function

        Public Shared Function GetConfig(ByVal Id As Int32) As SourceConfig
            Using Command As SQLiteCommand = m_SelectConfigCommand.Clone()
                Command.Parameters.AddWithValue("@Id", Id)
                Using Reader As IDataReader = Command.ExecuteReader()
                    If Reader.Read() Then
                        Return New SourceConfig(Reader)
                    Else
                        Return Nothing
                    End If
                End Using
            End Using
        End Function

        Public Shared Function GetHeaders() As IDataReader
            Using Command As SQLiteCommand = m_SelectHeaderCommand.Clone()
                Return Command.ExecuteReader()
            End Using
        End Function

        Public Shared Sub Add(ByVal ClassName As String, ByVal [Namespace] As String, ByVal Parameter As String, ByVal HttpAnnouncement As String)
            Using Command As SQLiteCommand = m_InsertCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@ClassName", ClassName)
                    .AddWithValue("@Namespace", [Namespace])
                    .AddWithValue("@Parameter", Parameter)
                    .AddWithValue("@HttpAnnouncement", HttpAnnouncement)
                End With
                Command.ExecuteNonQuery()
            End Using
        End Sub

        Public Shared Sub Remove(ByVal Id As Int32)
            Using Command As SQLiteCommand = m_DeleteCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@Id", Id)
                End With
                Command.ExecuteNonQuery()
            End Using
        End Sub

        Public Shared Sub Update(ByVal Id As Int32, ByVal ClassName As String, ByVal [Namespace] As String, ByVal Parameter As String, ByVal HttpAnnouncement As String)
            Using Command As SQLiteCommand = m_UpdateCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@Id", Id)
                    .AddWithValue("@ClassName", ClassName)
                    .AddWithValue("@Namespace", [Namespace])
                    .AddWithValue("@Parameter", Parameter)
                    .AddWithValue("@HttpAnnouncement", HttpAnnouncement)
                End With
                Command.ExecuteNonQuery()
            End Using
        End Sub

        Public Shared Sub Swap(ByVal Id0 As Int32, ByVal Id1 As Int32)
            Using Command0 As SQLiteCommand = m_UpdateIdCommand.Clone(), _
                Command1 As SQLiteCommand = m_UpdateIdCommand.Clone(), _
                Command2 As SQLiteCommand = m_UpdateIdCommand.Clone(), _
                Transaction As SQLiteTransaction = Database.CreateTransaction()

                Command0.Transaction = Transaction
                Command1.Transaction = Transaction
                Command2.Transaction = Transaction
                With Command0.Parameters
                    .AddWithValue("Id", 0)
                    .AddWithValue("OriginalId", Id0)
                End With
                With Command1.Parameters
                    .AddWithValue("Id", Id0)
                    .AddWithValue("OriginalId", Id1)
                End With
                With Command2.Parameters
                    .AddWithValue("Id", Id1)
                    .AddWithValue("OriginalId", 0)
                End With
                Command0.ExecuteNonQuery()
                Command1.ExecuteNonQuery()
                Command2.ExecuteNonQuery()
                Transaction.Commit()
            End Using
        End Sub
    End Class
End Namespace