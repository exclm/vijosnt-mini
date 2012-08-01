Namespace LocalDb
    Friend Class Config
        Private Shared m_SelectCommand As SQLiteCommand
        Private Shared m_ReplaceCommand As SQLiteCommand

        Shared Sub New()
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

        Public Shared Property DisplayFloating() As Boolean
            Get
                Dim Value As String = [Get]("DisplayFloating")
                If Value Is Nothing Then
                    Put("DisplayFloating", True)
                    Return True
                Else
                    Return Boolean.Parse(Value)
                End If
            End Get

            Set(ByVal Value As Boolean)
                Put("DisplayFloating", Value.ToString())
            End Set
        End Property

        Public Shared Property FloatingTransparent() As Boolean
            Get
                Dim Value As String = [Get]("FloatingTransparent")
                If Value Is Nothing Then
                    Put("FloatingTransparent", True)
                    Return True
                Else
                    Return Boolean.Parse(Value)
                End If
            End Get

            Set(ByVal Value As Boolean)
                Put("FloatingTransparent", Value.ToString())
            End Set
        End Property

        Public Shared Property FloatingLeft() As Int32
            Get
                Dim Value As String = [Get]("FloatingLeft")
                If Value Is Nothing Then
                    Value = Screen.PrimaryScreen.WorkingArea.Width * 3 \ 4
                    Put("FloatingLeft", Value)
                End If
                Return Int32.Parse(Value)
            End Get

            Set(ByVal Value As Int32)
                Put("FloatingLeft", Value.ToString())
            End Set
        End Property

        Public Shared Property FloatingTop() As Int32
            Get
                Dim Value As String = [Get]("FloatingTop")
                If Value Is Nothing Then
                    Value = Screen.PrimaryScreen.WorkingArea.Height \ 4
                    Put("FloatingTop", Value)
                End If
                Return Int32.Parse(Value)
            End Get

            Set(ByVal Value As Int32)
                Put("FloatingTop", Value.ToString())
            End Set
        End Property
    End Class
End Namespace
