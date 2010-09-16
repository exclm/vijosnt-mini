Imports VijosNT.Win32

Namespace LocalDb
    Friend Class Database
        Private Shared m_Connection As SQLiteConnection
        Private Shared m_VacuumCommand As SQLiteCommand

        Shared Sub New()
            Dim Builder As New SQLiteConnectionStringBuilder()
            Builder.DataSource = Path.Combine(My.Application.Info.DirectoryPath, "VijosNT.db3")
            m_Connection = New SQLiteConnection(Builder.ToString())
            m_Connection.Open()
            m_VacuumCommand = CreateCommand("VACUUM")
        End Sub

        Public Shared Function CreateCommand(ByVal CommandText As String) As SQLiteCommand
            Return New SQLiteCommand(CommandText, m_Connection)
        End Function

        Public Shared Function CreateTransaction() As SQLiteTransaction
            Return m_Connection.BeginTransaction()
        End Function

        Public Shared ReadOnly Property EngineVersion() As String
            Get
                Return "SQLite " & SQLiteConnection.SQLiteVersion
            End Get
        End Property

        Public Shared Sub Vacuum()
            Using Command As SQLiteCommand = m_VacuumCommand.Clone()
                Command.ExecuteNonQuery()
            End Using
        End Sub
    End Class
End Namespace
