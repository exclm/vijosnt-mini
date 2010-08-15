Namespace LocalDb
    Friend Class Log
        Private Shared m_InsertCommand As SQLiteCommand

        Shared Sub New()
            Try
                Using Command As SQLiteCommand = Database.CreateCommand( _
                    "CREATE TABLE IF NOT EXISTS Log (" & _
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                    "Level INTEGER, " & _
                    "Title TEXT, " & _
                    "Date INTEGER, " & _
                    "Detail TEXT)")
                    Command.ExecuteNonQuery()
                End Using

                m_InsertCommand = Database.CreateCommand( _
                    "INSERT INTO Log (Id, Level, Title, Date, Detail) VALUES (NULL, @Level, @Title, @Date, @Detail)")
            Catch ex As Exception
                Trace.WriteLine(ex.ToString())
                Environment.Exit(1)
            End Try
        End Sub

        Public Shared Sub Add(ByVal Level As LogLevel, ByVal Title As String, ByVal Detail As String)
            Try
                Using Command As SQLiteCommand = m_InsertCommand.Clone()
                    With Command.Parameters
                        .AddWithValue("@Level", Level)
                        .AddWithValue("@Title", Title)
                        .AddWithValue("@Date", Date.Now.Ticks)
                        .AddWithValue("@Detail", Detail)
                    End With
                    Command.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                Trace.WriteLine("无法将日志写入数据库: " & Title & " (" & Level.ToString() & ")")
                Trace.Indent()
                Try
                    Trace.WriteLine(Detail)
                Finally
                    Trace.Unindent()
                End Try
                Throw
            End Try
        End Sub
    End Class
End Namespace
