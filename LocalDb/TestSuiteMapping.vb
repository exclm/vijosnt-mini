Namespace LocalDb
    Friend Class TestSuiteMapping
        Private Shared m_SelectCommand As SQLiteCommand
        Private Shared m_SelectConfigCommand As SQLiteCommand
        Private Shared m_InsertCommand As SQLiteCommand
        Private Shared m_UpdateCommand As SQLiteCommand
        Private Shared m_DeleteCommand As SQLiteCommand
        Private Shared m_UpdateIdCommand As SQLiteCommand

        Shared Sub New()
            Using Command As SQLiteCommand = Database.CreateCommand( _
                    "CREATE TABLE IF NOT EXISTS TestSuiteMapping2 (" & _
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                    "Pattern TEXT, " & _
                    "NamespacePattern TEXT, " & _
                    "ClassName TEXT, " & _
                    "Parameter TEXT)")
                Command.ExecuteNonQuery()
            End Using

            m_SelectCommand = Database.CreateCommand( _
                "SELECT * FROM TestSuiteMapping2 ORDER BY Id")
            m_SelectConfigCommand = Database.CreateCommand( _
                "SELECT * FROM TestSuiteMapping2 WHERE Id = @Id")
            m_InsertCommand = Database.CreateCommand( _
                "INSERT INTO TestSuiteMapping2 (Id, Pattern, NamespacePattern, ClassName, Parameter) VALUES (NULL, @Pattern, @NamespacePattern, @ClassName, @Parameter)")
            m_UpdateCommand = Database.CreateCommand( _
                "UPDATE TestSuiteMapping2 SET Pattern = @Pattern, NamespacePattern = @NamespacePattern, ClassName = @ClassName, Parameter = @Parameter WHERE Id = @Id")
            m_DeleteCommand = Database.CreateCommand( _
                "DELETE FROM TestSuiteMapping2 WHERE Id = @Id")
            m_UpdateIdCommand = Database.CreateCommand( _
                "UPDATE TestSuiteMapping2 SET Id = @Id WHERE Id = @OriginalId")
        End Sub

        Public Shared Function GetAll() As IDataReader
            Using Command As SQLiteCommand = m_SelectCommand.Clone()
                Return Command.ExecuteReader()
            End Using
        End Function

        Public Shared Function GetConfig(ByVal Id As Int32) As TestSuiteConfig
            Using Command As SQLiteCommand = m_SelectConfigCommand.Clone()
                Command.Parameters.AddWithValue("@Id", Id)
                Using Reader As IDataReader = Command.ExecuteReader()
                    If Reader.Read() Then
                        Return New TestSuiteConfig(Reader)
                    Else
                        Return Nothing
                    End If
                End Using
            End Using
        End Function

        Public Shared Sub Add(ByVal Pattern As String, ByVal NamespacePattern As String, ByVal ClassName As String, ByVal Parameter As String)
            Using Command As SQLiteCommand = m_InsertCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@Pattern", Pattern)
                    .AddWithValue("@NamespacePattern", NamespacePattern)
                    .AddWithValue("@ClassName", ClassName)
                    .AddWithValue("@Parameter", Parameter)
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

        Public Shared Sub Update(ByVal Id As Int32, ByVal Pattern As String, ByVal NamespacePattern As String, ByVal ClassName As String, ByVal Parameter As String)
            Using Command As SQLiteCommand = m_UpdateCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@Id", Id)
                    .AddWithValue("@Pattern", Pattern)
                    .AddWithValue("@NamespacePattern", NamespacePattern)
                    .AddWithValue("@ClassName", ClassName)
                    .AddWithValue("@Parameter", Parameter)
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
