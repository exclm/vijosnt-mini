Namespace LocalDb
    Friend Class TestSuiteMapping
        Private Shared m_SelectCommand As SQLiteCommand
        Private Shared m_InsertCommand As SQLiteCommand
        ' TODO: Move up, move down

        Shared Sub New()
            Try
                Using Command As SQLiteCommand = Database.CreateCommand( _
                    "CREATE TABLE IF NOT EXISTS TestSuiteMapping (" & _
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                    "Pattern TEXT, " & _
                    "ClassName TEXT, " & _
                    "Parameter TEXT)")
                    Command.ExecuteNonQuery()
                End Using

                m_SelectCommand = Database.CreateCommand( _
                    "SELECT * FROM TestSuiteMapping ORDER BY Id")
                m_InsertCommand = Database.CreateCommand( _
                    "INSERT INTO TestSuiteMapping (Id, Pattern, ClassName, Parameter) VALUES (NULL, @Pattern, @ClassName, @Parameter)")
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

        Public Shared Sub Add(ByVal Pattern As String, ByVal ClassName As String, ByVal Parameter As String)
            Using Command As SQLiteCommand = m_InsertCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@Pattern", Pattern)
                    .AddWithValue("@ClassName", ClassName)
                    .AddWithValue("@Parameter", Parameter)
                End With
                Command.ExecuteNonQuery()
            End Using
        End Sub
    End Class
End Namespace
