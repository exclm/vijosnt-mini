Namespace LocalDb
    Friend Class DataSourceMapping
        Private Shared m_SelectCommand As SQLiteCommand
        Private Shared m_SelectHeaderCommand As SQLiteCommand
        Private Shared m_SelectConfigCommand As SQLiteCommand
        Private Shared m_InsertCommand As SQLiteCommand
        Private Shared m_DeleteCommand As SQLiteCommand
        Private Shared m_UpdateCommand As SQLiteCommand

        Shared Sub New()
            Using Command As SQLiteCommand = Database.CreateCommand( _
                "CREATE TABLE IF NOT EXISTS DataSourceMapping2 (" & _
                "Id INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                "ClassName TEXT, " & _
                "Namespace TEXT, " & _
                "Parameter TEXT, " & _
                "PollingInterval INTEGER, " & _
                "IpcAnnouncement TEXT, " & _
                "HttpAnnouncement TEXT)")
                Command.ExecuteNonQuery()
            End Using

            m_SelectCommand = Database.CreateCommand( _
                "SELECT * FROM DataSourceMapping2")
            m_SelectHeaderCommand = Database.CreateCommand( _
                "SELECT Id, ClassName, Namespace, Parameter FROM DataSourceMapping2")
            m_SelectConfigCommand = Database.CreateCommand( _
                "SELECT * FROM DataSourceMapping2 WHERE Id = @Id")
            m_InsertCommand = Database.CreateCommand( _
                "INSERT INTO DataSourceMapping2 (Id, ClassName, Namespace, Parameter, PollingInterval, IpcAnnouncement, HttpAnnouncement) VALUES (NULL, @ClassName, @Namespace, @Parameter, @PollingInterval, @IpcAnnouncement, @HttpAnnouncement)")
            m_DeleteCommand = Database.CreateCommand( _
                "DELETE FROM DataSourceMapping2 WHERE Id = @Id")
            m_UpdateCommand = Database.CreateCommand( _
                "UPDATE DataSourceMapping2 SET ClassName = @ClassName, Namespace = @Namespace, Parameter = @Parameter, PollingInterval = @PollingInterval, IpcAnnouncement = @IpcAnnouncement, HttpAnnouncement = @HttpAnnouncement WHERE Id = @Id")
        End Sub

        Public Shared Function GetAll() As IDataReader
            Using Command As SQLiteCommand = m_SelectCommand.Clone()
                Return Command.ExecuteReader()
            End Using
        End Function

        Public Shared Function GetConfig(ByVal Id As Int32) As DataSourceConfig
            Using Command As SQLiteCommand = m_SelectConfigCommand.Clone()
                Command.Parameters.AddWithValue("@Id", Id)
                Using Reader As IDataReader = Command.ExecuteReader()
                    If Reader.Read() Then
                        Return New DataSourceConfig(Reader)
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

        Public Shared Sub Add(ByVal ClassName As String, ByVal [Namespace] As String, ByVal Parameter As String, ByVal PollingInterval As Nullable(Of Int32), ByVal IpcAnnouncement As String, ByVal HttpAnnouncement As String)
            Using Command As SQLiteCommand = m_InsertCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@ClassName", ClassName)
                    .AddWithValue("@Namespace", [Namespace])
                    .AddWithValue("@Parameter", Parameter)
                    .AddWithValue("@PollingInterval", PollingInterval)
                    .AddWithValue("@IpcAnnouncement", IpcAnnouncement)
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

        Public Shared Sub Update(ByVal Id As Int32, ByVal ClassName As String, ByVal [Namespace] As String, ByVal Parameter As String, ByVal PollingInterval As Nullable(Of Int32), ByVal IpcAnnouncement As String, ByVal HttpAnnouncement As String)
            Using Command As SQLiteCommand = m_UpdateCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@Id", Id)
                    .AddWithValue("@ClassName", ClassName)
                    .AddWithValue("@Namespace", [Namespace])
                    .AddWithValue("@Parameter", Parameter)
                    .AddWithValue("@PollingInterval", PollingInterval)
                    .AddWithValue("@IpcAnnouncement", IpcAnnouncement)
                    .AddWithValue("@HttpAnnouncement", HttpAnnouncement)
                End With
                Command.ExecuteNonQuery()
            End Using
        End Sub
    End Class
End Namespace