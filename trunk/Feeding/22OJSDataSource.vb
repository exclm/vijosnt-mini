Imports VijosNT.Utility

Namespace Feeding
    Friend Class _22OJSDataSource
        Inherits DataSourceBase

        Private m_TesterId As Int32
        Private m_ConnectionString As String
        Private m_SelectPendingCommand As SqlCommand
        Private m_SelectCodeCommand As SqlCommand
        Private m_SelectHeaderCommand As SqlCommand
        Private m_UpdateTakenCommand As SqlCommand
        Private m_UpdateUntakeCommand As SqlCommand
        Private m_UpdateFinalCommand As SqlCommand
        Private m_UpdateProblemCommand As SqlCommand
        Private m_UpdateUserCommand As SqlCommand

        Public Sub New(ByVal [Namespace] As String, ByVal Parameters As String)
            MyBase.New([Namespace])

            Dim Server As String = Nothing
            Dim Database As String = Nothing
            Dim UserName As String = Nothing
            Dim Password As String = Nothing
            m_TesterId = 1

            For Each Parameter As String In Parameters.Split(New Char() {";"c})
                Dim Position As Int32 = Parameter.IndexOf("="c)
                If Position = -1 Then Continue For
                Dim Key As String = Parameter.Substring(0, Position)
                Dim Value As String = Parameter.Substring(Position + 1)
                Select Case Key.ToLower()
                    Case "server"
                        Server = Value
                    Case "database"
                        Database = Value
                    Case "username"
                        UserName = Value
                    Case "password"
                        Password = Value
                    Case "testerid"
                        m_TesterId = Int32.Parse(Value)
                End Select
            Next
            If Server Is Nothing Then _
                Throw New ArgumentNullException("Server")
            If Database Is Nothing Then _
                Throw New ArgumentNullException("Database")
            If UserName Is Nothing Then _
                Throw New ArgumentNullException("UserName")
            If Password Is Nothing Then _
                Throw New ArgumentNullException("Password")
            If m_TesterId <= 0 Then _
                Throw New ArgumentOutOfRangeException("TesterId", "TesterId 必须为一个正整数")

            Dim ConnectionBuilder As New SqlConnectionStringBuilder
            ConnectionBuilder.DataSource = Server
            ConnectionBuilder.InitialCatalog = Database
            ConnectionBuilder.UserID = UserName
            ConnectionBuilder.Password = Password
            ConnectionBuilder.Pooling = True
            m_ConnectionString = ConnectionBuilder.ToString()

            m_SelectPendingCommand = New SqlCommand( _
                "SELECT ID FROM rec WHERE zt = 'Waiting' ORDER BY ID")
            m_SelectCodeCommand = New SqlCommand( _
                "SELECT qid, codem, code FROM Record WHERE ID = @Id")
            m_SelectHeaderCommand = New SqlCommand( _
                "SELECT qid, user FROM Record WHERE ID = @Id")
            m_UpdateTakenCommand = New SqlCommand( _
                "UPDATE rec SET zt = 'Running' WHERE ID = @Id AND zt = 'Waiting'")
            m_UpdateUntakeCommand = New SqlCommand( _
                "UPDATE rec SET zt = 'Waiting' WHERE ID = @Id")
            m_UpdateFinalCommand = New SqlCommand( _
                "UPDATE rec SET zt = @Status, data = @Details WHERE ID = @Id")
            m_UpdateUserCommand = New SqlCommand( _
                "DECLARE @ProblemListOKPtr varbinary(16)" & vbCrLf & _
                "UPDATE udata" & vbCrLf & _
                "SET tg = tg + 1, @ProblemListOKPtr = TEXTPTR(tgq)" & vbCrLf & _
                "WHERE [user] = @UserId AND charindex(@ProblemId + ' ', tgq) = 0;" & vbCrLf & _
                "IF NOT(@ProblemListOKPtr IS NULL) BEGIN" & vbCrLf & _
                "UPDATETEXT udata.tgq @ProblemListOKPtr NULL 0 @ProblemId" & vbCrLf & _
                "UPDATETEXT udata.tgq @ProblemListOKPtr NULL 0 ' '" & vbCrLf & _
                "END;")
        End Sub

        Private Function CloneConnection() As SqlConnection
            Dim Result As New SqlConnection(m_ConnectionString)
            Result.Open()
            Return Result
        End Function

        Private Function CloneCommand(ByVal Command As SqlCommand, ByVal Connection As SqlConnection) As SqlCommand
            Dim Result As SqlCommand = Command.Clone()
            Result.Connection = Connection
            Return Result
        End Function

        Private Function GetCompilerExtension(ByVal CompilerName As String) As String
            Select Case CompilerName.ToLower()
                Case "c", "gcc"
                    Return ".c"
                Case "cpp", "gpp", "g++"
                    Return ".cpp"
                Case Else
                    Return ".pas"
            End Select
        End Function

        Public Overrides Function Take() As Nullable(Of DataSourceRecord)
            Dim TakenId As Nullable(Of Int32)
            Using Connection0 As SqlConnection = CloneConnection(), _
                Command0 As SqlCommand = CloneCommand(m_SelectPendingCommand, Connection0), _
                Reader As SqlDataReader = Command0.ExecuteReader()
                While Reader.Read()
                    Using Connection1 As SqlConnection = CloneConnection(), _
                        Command1 As SqlCommand = CloneCommand(m_UpdateTakenCommand, Connection1)
                        Dim Id As Int32 = Reader("Id")
                        Command1.Parameters.AddWithValue("@Id", Id)
                        If Command1.ExecuteNonQuery() <> 0 Then
                            TakenId = Id
                            Exit While
                        End If
                    End Using
                End While
            End Using
            If Not TakenId.HasValue Then _
                Return Nothing
            Dim Result As DataSourceRecord
            Result.Id = TakenId.Value
            Using Connection As SqlConnection = CloneConnection(), _
                Command As SqlCommand = CloneCommand(m_SelectCodeCommand.Clone, Connection)
                Command.Parameters.AddWithValue("@Id", TakenId.Value)
                Using Reader As SqlDataReader = Command.ExecuteReader()
                    If Not Reader.Read() Then _
                        Return Nothing
                    Result.FileName = "Q" & Reader("qid") & GetCompilerExtension(Reader("codem"))
                    Result.SourceCode = Reader("code")
                End Using
            End Using
            Return Result
        End Function

        Public Overloads Overrides Sub Untake(ByVal Id As Int32)
            Using Connection As SqlConnection = CloneConnection(), _
                Command As SqlCommand = CloneCommand(m_UpdateUntakeCommand, Connection)
                Command.Parameters.AddWithValue("@Id", Id)
                Command.ExecuteNonQuery()
            End Using
        End Sub

        Public Overloads Overrides Sub Untake(ByVal Id As Int32, ByVal Result As TestResult)
            Dim ProblemId As String
            Dim UserId As String

            Using Connection As SqlConnection = CloneConnection(), _
                Command As SqlCommand = CloneCommand(m_SelectHeaderCommand, Connection)
                Command.Parameters.AddWithValue("@Id", Id)
                Using Reader As SqlDataReader = Command.ExecuteReader()
                    If Not Reader.Read() Then
                        Untake(Id)
                        Return
                    End If
                    ProblemId = Reader("qid")
                    UserId = Reader("user")
                End Using
            End Using

            Dim Details As New StringBuilder()
            Details.AppendLine("VijosNT <font color=#ff8000><b>Mini</b></font> " & Assembly.GetExecutingAssembly().GetName().Version.ToString())

            If Result.Warning IsNot Nothing AndAlso Result.Warning.Length <> 0 Then
                Details.AppendLine()
                Details.AppendLine(Result.Warning)
            End If

            If Result.Entries IsNot Nothing Then
                Details.AppendLine()
                For Each Entry As TestResultEntry In Result.Entries
                    Details.Append("#" & Entry.Index.ToString("00") & ": ")
                    If Entry.Flag = TestResultFlag.Accepted Then
                        Details.Append("<font color=#ff0000>Accepted</font>")
                    Else
                        Details.Append("<font color=#0000ff>" & FormatEnumString(Entry.Flag.ToString()) & "</font>")
                    End If
                    Details.AppendLine(" (" & (Entry.TimeUsage \ 10000).ToString() & "ms, " & (Entry.MemoryUsage \ 1024).ToString() & "KB)")
                    If Entry.Warning IsNot Nothing AndAlso Entry.Warning.Length <> 0 Then
                        Details.AppendLine(Entry.Warning)
                    End If
                Next
            End If

            Details.AppendLine()
            If Result.Flag = TestResultFlag.Accepted Then
                Details.Append("<font color=#ff0000>Accepted</font>")
            Else
                Details.Append("<font color=#0000ff>" & FormatEnumString(Result.Flag.ToString()) & "</font>")
            End If
            Details.AppendLine(" / " & Result.Score & " / " & (Result.TimeUsage \ 10000).ToString() & "ms / " & (Result.MemoryUsage \ 1024).ToString() & "KB")

            Using Connection As SqlConnection = CloneConnection(), _
                Command0 As SqlCommand = CloneCommand(m_UpdateFinalCommand, Connection), _
                Command2 As SqlCommand = CloneCommand(m_UpdateUserCommand, Connection), _
                Transaction As SqlTransaction = Connection.BeginTransaction()

                Command0.Transaction = Transaction

                With Command0.Parameters
                    .AddWithValue("@Id", Id)
                    .AddWithValue("@Status", Result.Flag.ToString)
                    .AddWithValue("@Details", Details.ToString())
                End With
                Command0.ExecuteNonQuery()

                If Result.Score = 100 Then
                    Command2.Transaction = Transaction
                    With Command2.Parameters
                        .AddWithValue("@ProblemId", ProblemId)
                        .AddWithValue("@UserId", UserId)
                    End With
                    Command2.ExecuteNonQuery()
                End If

                Transaction.Commit()
            End Using
        End Sub
    End Class
End Namespace
