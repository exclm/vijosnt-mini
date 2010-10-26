Namespace LocalDb
    Friend Class CompilerMapping
        Private Shared m_SelectCommand As SQLiteCommand
        Private Shared m_SelectHeaderCommand As SQLiteCommand
        Private Shared m_SelectConfigCommand As SQLiteCommand
        Private Shared m_InsertCommand As SQLiteCommand
        Private Shared m_DeleteCommand As SQLiteCommand
        Private Shared m_UpdateCommand As SQLiteCommand
        Private Shared m_UpdateIdCommand As SQLiteCommand

        Shared Sub New()
            Using Command As SQLiteCommand = Database.CreateCommand( _
                    "CREATE TABLE IF NOT EXISTS CompilerMapping2 (" & _
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                    "Pattern TEXT, " & _
                    "ApplicationName TEXT, " & _
                    "CommandLine TEXT, " & _
                    "EnvironmentVariables TEXT, " & _
                    "TimeQuota INTEGER, " & _
                    "MemoryQuota INTEGER, " & _
                    "ActiveProcessQuota INTEGER, " & _
                    "SourceFileName TEXT, " & _
                    "TargetFileName TEXT, " & _
                    "TargetApplicationName TEXT, " & _
                    "TargetCommandLine TEXT, " & _
                    "TimeOffset INTEGER, " & _
                    "TimeFactor REAL, " & _
                    "MemoryOffset INTEGER, " & _
                    "MemoryFactor REAL)")
                Command.ExecuteNonQuery()
            End Using

            m_SelectCommand = Database.CreateCommand( _
                "SELECT * FROM CompilerMapping2 ORDER BY Id")
            m_SelectHeaderCommand = Database.CreateCommand( _
                "SELECT Id, Pattern, CommandLine FROM CompilerMapping2 ORDER BY Id")
            m_SelectConfigCommand = Database.CreateCommand( _
                "SELECT * FROM CompilerMapping2 WHERE Id = @Id")
            m_InsertCommand = Database.CreateCommand( _
                "INSERT INTO CompilerMapping2 (Id, Pattern, ApplicationName, CommandLine, EnvironmentVariables, TimeQuota, MemoryQuota, ActiveProcessQuota, SourceFileName, TargetFileName, TargetApplicationName, TargetCommandLine, TimeOffset, TimeFactor, MemoryOffset, MemoryFactor) VALUES (NULL, @Pattern, @ApplicationName, @CommandLine, @EnvironmentVariables, @TimeQuota, @MemoryQuota, @ActiveProcessQuota, @SourceFileName, @TargetFileName, @TargetApplicationName, @TargetCommandLine, @TimeOffset, @TimeFactor, @MemoryOffset, @MemoryFactor)")
            m_DeleteCommand = Database.CreateCommand( _
                "DELETE FROM CompilerMapping2 WHERE Id = @Id")
            m_UpdateCommand = Database.CreateCommand( _
                "UPDATE CompilerMapping2 SET Pattern = @Pattern, ApplicationName = @ApplicationName, CommandLine = @CommandLine, EnvironmentVariables = @EnvironmentVariables, TimeQuota = @TimeQuota, MemoryQuota = @MemoryQuota, ActiveProcessQuota = @ActiveProcessQuota, SourceFileName = @SourceFileName, TargetFileName = @TargetFileName, TargetApplicationName = @TargetApplicationName, TargetCommandLine = @TargetCommandLine, TimeOffset = @TimeOffset, TimeFactor = @TimeFactor, MemoryOffset = @MemoryOffset, MemoryFactor = @MemoryFactor WHERE Id = @Id")
            m_UpdateIdCommand = Database.CreateCommand( _
                "UPDATE CompilerMapping2 SET Id = @Id WHERE Id = @OriginalId")
        End Sub

        Public Shared Function GetAll() As IDataReader
            Using Command As SQLiteCommand = m_SelectCommand.Clone()
                Return Command.ExecuteReader()
            End Using
        End Function

        Public Shared Function GetHeaders() As IDataReader
            Using Command As SQLiteCommand = m_SelectHeaderCommand.Clone()
                Return Command.ExecuteReader()
            End Using
        End Function

        Public Shared Function GetConfig(ByVal Id As Int32) As CompilerConfig
            Using Command As SQLiteCommand = m_SelectConfigCommand.Clone()
                Command.Parameters.AddWithValue("@Id", Id)
                Using Reader As IDataReader = Command.ExecuteReader()
                    If Reader.Read() Then
                        Return New CompilerConfig(Reader)
                    Else
                        Return Nothing
                    End If
                End Using
            End Using
        End Function

        Public Shared Sub Add(ByVal Pattern As String, ByVal ApplicationName As String, ByVal CommandLine As String, ByVal EnvironmentVariables As String, ByVal TimeQuota As Int64?, ByVal MemoryQuota As Int64?, ByVal ActiveProcessQuota As Int32?, ByVal SourceFileName As String, ByVal TargetFileName As String, ByVal TargetApplicationName As String, ByVal TargetCommandLine As String, ByVal TimeOffset As Int64?, ByVal TimeFactor As Double?, ByVal MemoryOffset As Int64?, ByVal MemoryFactor As Double?)
            Using Command As SQLiteCommand = m_InsertCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@Pattern", Pattern)
                    .AddWithValue("@ApplicationName", ApplicationName)
                    .AddWithValue("@CommandLine", CommandLine)
                    .AddWithValue("@EnvironmentVariables", EnvironmentVariables)
                    .AddWithValue("@TimeQuota", TimeQuota)
                    .AddWithValue("@MemoryQuota", MemoryQuota)
                    .AddWithValue("@ActiveProcessQuota", ActiveProcessQuota)
                    .AddWithValue("@SourceFileName", SourceFileName)
                    .AddWithValue("@TargetFileName", TargetFileName)
                    .AddWithValue("@TargetApplicationName", TargetApplicationName)
                    .AddWithValue("@TargetCommandLine", TargetCommandLine)
                    .AddWithValue("@TimeOffset", TimeOffset)
                    .AddWithValue("@TimeFactor", TimeFactor)
                    .AddWithValue("@MemoryOffset", MemoryOffset)
                    .AddWithValue("@MemoryFactor", MemoryFactor)
                End With
                Command.ExecuteNonQuery()
            End Using
        End Sub

        Public Shared Sub Remove(ByVal Id As Int32)
            Using Command As SQLiteCommand = m_DeleteCommand.Clone()
                Command.Parameters.AddWithValue("@Id", Id)
                Command.ExecuteNonQuery()
            End Using
        End Sub

        Public Shared Sub Update(ByVal Id As Int32, ByVal Pattern As String, ByVal ApplicationName As String, ByVal CommandLine As String, ByVal EnvironmentVariables As String, ByVal TimeQuota As Int64?, ByVal MemoryQuota As Int64?, ByVal ActiveProcessQuota As Int32?, ByVal SourceFileName As String, ByVal TargetFileName As String, ByVal TargetApplicationName As String, ByVal TargetCommandLine As String, ByVal TimeOffset As Int64?, ByVal TimeFactor As Double?, ByVal MemoryOffset As Int64?, ByVal MemoryFactor As Double?)
            Using Command As SQLiteCommand = m_UpdateCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@Id", Id)
                    .AddWithValue("@Pattern", Pattern)
                    .AddWithValue("@ApplicationName", ApplicationName)
                    .AddWithValue("@CommandLine", CommandLine)
                    .AddWithValue("@EnvironmentVariables", EnvironmentVariables)
                    .AddWithValue("@TimeQuota", TimeQuota)
                    .AddWithValue("@MemoryQuota", MemoryQuota)
                    .AddWithValue("@ActiveProcessQuota", ActiveProcessQuota)
                    .AddWithValue("@SourceFileName", SourceFileName)
                    .AddWithValue("@TargetFileName", TargetFileName)
                    .AddWithValue("@TargetApplicationName", TargetApplicationName)
                    .AddWithValue("@TargetCommandLine", TargetCommandLine)
                    .AddWithValue("@TimeOffset", TimeOffset)
                    .AddWithValue("@TimeFactor", TimeFactor)
                    .AddWithValue("@MemoryOffset", MemoryOffset)
                    .AddWithValue("@MemoryFactor", MemoryFactor)
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
