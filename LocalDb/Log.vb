Namespace LocalDb
    Friend Class Log
        Private Shared m_InsertCommand As SQLiteCommand

        Shared Sub New()
            Try
                Database.CreateCommand( _
                    "CREATE TABLE IF NOT EXISTS Log (" & _
                    "ID INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                    "Level INTEGER, " & _
                    "Title TEXT, " & _
                    "Date INTEGER, " & _
                    "Detail TEXT)").ExecuteNonQuery()

                m_InsertCommand = Database.CreateCommand( _
                    "INSERT INTO Log (ID, Level, Title, Date, Detail) VALUES (NULL, @Level, @Title, @Date, @Detail)")
            Catch ex As Exception
                Trace.WriteLine(ex.ToString())
                Throw
            End Try
        End Sub

        Public Shared Sub Add(ByVal Level As LogLevel, ByVal Title As String, ByVal Detail As String)
            Dim Command As SQLiteCommand = m_InsertCommand.Clone()
            With Command.Parameters
                .AddWithValue("@Level", Level)
                .AddWithValue("@Title", Title)
                .AddWithValue("@Date", Date.Now)
                .AddWithValue("@Detail", Detail)
            End With
            Command.ExecuteNonQuery()
        End Sub
    End Class
End Namespace
