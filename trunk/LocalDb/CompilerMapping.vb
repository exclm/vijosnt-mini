Namespace LocalDb
    Friend Class CompilerMapping
        Private Shared m_SelectCommand As SQLiteCommand
        Private Shared m_InsertCommand As SQLiteCommand
        ' TODO: Move up, move down

        Shared Sub New()
            Try
                Using Command As SQLiteCommand = Database.CreateCommand( _
                    "CREATE TABLE IF NOT EXISTS CompilerMapping (" & _
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                    "Pattern TEXT, " & _
                    "ApplicationName TEXT, " & _
                    "CommandLine TEXT, " & _
                    "TimeQuota INTEGER, " & _
                    "MemoryQuota INTEGER, " & _
                    "ActiveProcessQuota INTEGER, " & _
                    "SourceFileName TEXT, " & _
                    "TargetFileName TEXT, " & _
                    "TargetApplicationName TEXT, " & _
                    "TargetCommandLine TEXT)")
                    Command.ExecuteNonQuery()
                End Using

                m_SelectCommand = Database.CreateCommand( _
                    "SELECT * FROM CompilerMapping ORDER BY Id")
                m_InsertCommand = Database.CreateCommand( _
                    "INSERT INTO CompilerMapping (Id, Pattern, ApplicationName, CommandLine, TimeQuota, MemoryQuota, ActiveProcessQuota, SourceFileName, TargetFileName, TargetApplicationName, TargetCommandLine) VALUES (NULL, @Pattern, @ApplicationName, @CommandLine, @TimeQuota, @MemoryQuota, @ActiveProcessQuota, @SourceFileName, @TargetFileName, @TargetApplicationName, @TargetCommandLine)")
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

        Public Shared Sub Add(ByVal Pattern As String, ByVal ApplicationName As String, ByVal CommandLine As String, ByVal TimeQuota As Nullable(Of Int64), ByVal MemoryQuota As Nullable(Of Int64), ByVal ActiveProcessQuota As Nullable(Of Int32), ByVal SourceFileName As String, ByVal TargetFileName As String, ByVal TargetApplicationName As String, ByVal TargetCommandLine As String)
            Using Command As SQLiteCommand = m_InsertCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@Pattern", Pattern)
                    .AddWithValue("@ApplicationName", ApplicationName)
                    .AddWithValue("@CommandLine", CommandLine)
                    .AddWithValue("@TimeQuota", TimeQuota)
                    .AddWithValue("@MemoryQuota", MemoryQuota)
                    .AddWithValue("@ActiveProcessQuota", ActiveProcessQuota)
                    .AddWithValue("@SourceFileName", SourceFileName)
                    .AddWithValue("@TargetFileName", TargetFileName)
                    .AddWithValue("@TargetApplicationName", TargetApplicationName)
                    .AddWithValue("@TargetCommandLine", TargetCommandLine)
                End With
                Command.ExecuteNonQuery()
            End Using
        End Sub
    End Class
End Namespace
