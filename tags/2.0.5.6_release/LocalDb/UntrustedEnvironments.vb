Namespace LocalDb
    Friend Class UntrustedEnvironments
        Private Shared m_SelectCommand As SQLiteCommand
        Private Shared m_InsertCommand As SQLiteCommand
        Private Shared m_DeleteCommand As SQLiteCommand

        Shared Sub New()
            Using Command As SQLiteCommand = Database.CreateCommand( _
                    "CREATE TABLE IF NOT EXISTS UntrustedEnvironments2 (" & _
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                    "UserName TEXT, " & _
                    "Password TEXT)")
                Command.ExecuteNonQuery()
            End Using

            m_SelectCommand = Database.CreateCommand( _
                "SELECT * FROM UntrustedEnvironments2")
            m_InsertCommand = Database.CreateCommand( _
                "INSERT INTO UntrustedEnvironments2 (Id, UserName, Password) VALUES (NULL, @UserName, @Password)")
            m_DeleteCommand = Database.CreateCommand( _
                "DELETE FROM UntrustedEnvironments2 WHERE Id = @Id")
        End Sub

        Public Shared Function GetAll() As IDataReader
            Using Command As SQLiteCommand = m_SelectCommand.Clone()
                Return Command.ExecuteReader()
            End Using
        End Function

        Public Shared Sub Add(ByVal UserName As String, ByVal Password As String)
            Using Command As SQLiteCommand = m_InsertCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@UserName", UserName)
                    .AddWithValue("@Password", Password)
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
    End Class
End Namespace
