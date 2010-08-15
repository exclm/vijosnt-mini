Namespace LocalDb
    Friend Class Config
        Private Shared m_SelectCommand As SQLiteCommand
        Private Shared m_ReplaceCommand As SQLiteCommand

        Shared Sub New()
            Try
                Using Command As SQLiteCommand = Database.CreateCommand( _
                    "CREATE TABLE IF NOT EXISTS Config (" & _
                    "Key TEXT PRIMARY KEY, " & _
                    "Data TEXT)")
                    Command.ExecuteNonQuery()
                End Using

                m_SelectCommand = Database.CreateCommand( _
                    "SELECT Data FROM Config WHERE Key = @Key")
                m_ReplaceCommand = Database.CreateCommand( _
                    "REPLACE INTO Config (Key, Data) VALUES (@Key, @Data)")
            Catch ex As Exception
                Log.Add(LogLevel.Error, "初始化时发生异常", ex.ToString())
                Environment.Exit(1)
            End Try
        End Sub

        Private Shared Sub Put(ByVal Key As String, ByVal Data As String)
            Using Command As SQLiteCommand = m_ReplaceCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@Key", Key)
                    .AddWithValue("@Data", Data)
                End With
                Command.ExecuteNonQuery()
            End Using
        End Sub

        Private Shared Function [Get](ByVal Key As String) As String
            Using Command As SQLiteCommand = m_SelectCommand.Clone()
                With Command.Parameters
                    .AddWithValue("@Key", Key)
                End With
                Return Command.ExecuteScalar()
            End Using
        End Function

        Public Shared Property ExecutorSlots() As Int32
            Get
                Dim Value As String = [Get]("ExecutorSlots")
                If Value Is Nothing Then
                    Dim ProcessorCount As Int32 = Environment.ProcessorCount
                    Put("ExecutorSlots", ProcessorCount)
                    Return ProcessorCount
                Else
                    Return Int32.Parse(Value)
                End If
            End Get

            Set(ByVal Value As Int32)
                Put("ExecutorSlots", Value.ToString())
            End Set
        End Property

        Public Shared Property EnableSecurity() As Boolean
            Get
                Dim Value As String = [Get]("EnableSecurity")
                If Value Is Nothing Then
                    Put("EnableSecurity", False)
                    Return False
                Else
                    Return Boolean.Parse(Value)
                End If
            End Get

            Set(ByVal Value As Boolean)
                Put("EnableSecurity", Value.ToString())
            End Set
        End Property
    End Class
End Namespace
