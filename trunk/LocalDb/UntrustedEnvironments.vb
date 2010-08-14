Namespace LocalDb
    Friend Class UntrustedEnvironments
        Private Shared m_SelectCommand As SQLiteCommand
        Private Shared m_InsertCommand As SQLiteCommand

        Shared Sub New()
            Try
                Using Command As SQLiteCommand = Database.CreateCommand( _
                    "CREATE TABLE IF NOT EXISTS UntrustedEnvironments (" & _
                    "DesktopName TEXT PRIMARY KEY UNIQUE, " & _
                    "UserName TEXT, " & _
                    "Password TEXT)")
                    Command.ExecuteNonQuery()
                End Using

                m_SelectCommand = Database.CreateCommand( _
                    "SELECT * FROM UntrustedEnvironments")
                m_InsertCommand = Database.CreateCommand( _
                    "INSERT INTO UntrustedEnvironments (DesktopName, UserName, Password) VALUES (@DesktopName, @UserName, @Password)")
            Catch ex As Exception
                Log.Add(LogLevel.Error, "初始化时发生异常", ex.ToString())
                Environment.Exit(1)
            End Try
        End Sub

        Public Shared Function GetAll() As IDataReader
            Using Command As SQLiteCommand = m_SelectCommand.Clone()
                Return Command.ExecuteReader()
            End Using
        End Function

        Public Shared Sub Add(ByVal DesktopName As String, ByVal UserName As String, ByVal Password As String)
            Using Command As SQLiteCommand = m_InsertCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@DesktopName", DesktopName)
                    .AddWithValue("@UserName", UserName)
                    .AddWithValue("@Password", Password)
                End With
                Command.ExecuteNonQuery()
            End Using
        End Sub
    End Class
End Namespace
