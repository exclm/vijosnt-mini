Namespace Feeding
    Friend Class VijosDataSource
        Inherits DataSourceBase

        Private m_TesterId As Int32
        Private m_Connection As SqlConnection
        Private m_SelectPendingCommand As SqlCommand
        Private m_SelectCodeCommand As SqlCommand
        Private m_SelectHeaderCommand As SqlCommand
        Private m_UpdateTakenCommand As SqlCommand
        Private m_UpdateUntakeCommand As SqlCommand
        Private m_UpdateFinalCommand As SqlCommand
        Private m_UpdateProblemCommand As SqlCommand
        Private m_UpdateUserCommand As SqlCommand

        Public Sub New(ByVal Parameters As String)
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

            m_Connection = New SqlConnection(ConnectionBuilder.ToString())
            m_Connection.Open()

            m_SelectPendingCommand = New SqlCommand( _
                "SELECT ID FROM Record WHERE CheckNF = 0 ORDER BY ID", m_Connection)
            m_SelectCodeCommand = New SqlCommand( _
                "SELECT ProblemID, Compiler, Code FROM Record WHERE ID = @Id", m_Connection)
            m_SelectHeaderCommand = New SqlCommand( _
                "SELECT ProblemID, UserID FROM Record WHERE ID = @Id", m_Connection)
            m_UpdateTakenCommand = New SqlCommand( _
                "UPDATE Record SET CheckNF = -@TesterId WHERE ID = @Id AND CheckNF = 0", m_Connection)
            m_UpdateTakenCommand.Parameters.AddWithValue("@TesterId", m_TesterId)
            m_UpdateUntakeCommand = New SqlCommand( _
                "UPDATE Record SET CheckNF = 0 WHERE ID = @Id", m_Connection)
            m_UpdateFinalCommand = New SqlCommand( _
                "UPDATE Record SET CheckNF = @TesterId, Score = @Score, RunTime = @TimeUsage, Body = @Details WHERE ID = @Id", m_Connection)
            m_UpdateFinalCommand.Parameters.AddWithValue("@TesterId", m_TesterId)
            m_UpdateProblemCommand = New SqlCommand( _
                "DECLARE @UserListOKPtr varbinary(16)" & vbCrLf & _
                "UPDATE Problem" & vbCrLf & _
                "SET OK = OK + 1, @UserListOKPtr = TEXTPTR(UserListOK)" & vbCrLf & _
                "WHERE ID = @ProblemId AND charindex('|' + @UserId + '|', UserListOK) = 0;" & vbCrLf & _
                "IF NOT(@UserListOKPtr IS NULL) BEGIN" & vbCrLf & _
                "UPDATETEXT Problem.UserListOK @UserListOKPtr NULL 0 @UserId" & vbCrLf & _
                "UPDATETEXT Problem.UserListOK @UserListOKPtr NULL 0 '|'" & vbCrLf & _
                "END;", m_Connection)
            m_UpdateUserCommand = New SqlCommand( _
                "DECLARE @ProblemListOKPtr varbinary(16)" & vbCrLf & _
                "UPDATE User" & vbCrLf & _
                "SET ProblemOK = ProblemOK + 1, @ProblemListOKPtr = TEXTPTR(ProblemListOK)" & vbCrLf & _
                "WHERE ID = @UserId AND charindex('|' + @ProblemId + '|', ProblemListOK) = 0;" & vbCrLf & _
                "IF NOT(@ProblemListOKPtr IS NULL) BEGIN" & vbCrLf & _
                "UPDATETEXT User.ProblemListOK @ProblemListOKPtr NULL 0 @ProblemId" & vbCrLf & _
                "UPDATETEXT User.ProblemListOK @ProblemListOKPtr NULL 0 '|'" & vbCrLf & _
                "END;", m_Connection)
        End Sub

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
            Using Command0 As SqlCommand = m_SelectPendingCommand.Clone(), _
                Reader As SqlDataReader = Command0.ExecuteReader()
                While Reader.Read()
                    Using Command1 As SqlCommand = m_UpdateTakenCommand.Clone()
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
            Using Command As SqlCommand = m_SelectCodeCommand.Clone()
                Command.Parameters.AddWithValue("@Id", TakenId.Value)
                Using Reader As SqlDataReader = Command.ExecuteReader()
                    If Not Reader.Read() Then _
                        Return Nothing
                    Result.FileName = Reader("ProblemID") & GetCompilerExtension(Reader("Compiler"))
                    Result.SourceCode = Reader("Code")
                End Using
            End Using
            Return Result
        End Function

        Public Overloads Overrides Sub Untake(ByVal Id As Int32)
            Using Command As SqlCommand = m_UpdateUntakeCommand.Clone()
                Command.Parameters.AddWithValue("@Id", Id)
                Command.ExecuteNonQuery()
            End Using
        End Sub

        Public Overloads Overrides Sub Untake(ByVal Id As Int32, ByVal Result As TestResult)
            Dim ProblemId As Int32
            Dim UserId As Int32

            Using Command As SqlCommand = m_SelectHeaderCommand.Clone()
                Command.Parameters.AddWithValue("@Id", Id)
                Using Reader As SqlDataReader = Command.ExecuteReader()
                    If Not Reader.Read() Then
                        Untake(Id)
                        Return
                    End If
                    ProblemId = Reader("ProblemID")
                    UserId = Reader("UserID")
                End Using
            End Using

            Dim Details As New StringBuilder()
            Details.AppendLine("VijosNT Mini " & Assembly.GetExecutingAssembly().GetName().Version.ToString())
            Details.AppendLine(Result.Flag.ToString() & " / " & Result.Score & " / " & (Result.TimeUsage \ 10000).ToString() & "ms / " & (Result.MemoryUsage \ 1024).ToString() & "KB")

            If Result.Warning IsNot Nothing AndAlso Result.Warning.Length <> 0 Then
                Details.AppendLine()
                Details.AppendLine(Result.Warning)
            End If

            If Result.Entries IsNot Nothing Then
                Details.AppendLine()
                For Each Entry As TestResultEntry In Result.Entries
                    Details.AppendLine("Test #" & Entry.Index.ToString() & ": " & Entry.Flag.ToString() & " / " & (Entry.TimeUsage \ 10000).ToString() & "ms / " & (Entry.MemoryUsage \ 1024).ToString() & "KB")
                Next
            End If

            Using Command0 As SqlCommand = m_UpdateFinalCommand.Clone(), _
                Command1 As SqlCommand = m_UpdateProblemCommand.Clone(), _
                Command2 As SqlCommand = m_UpdateUserCommand.Clone(), _
                Transaction As SqlTransaction = m_Connection.BeginTransaction()

                Command0.Transaction = Transaction
                Command1.Transaction = Transaction
                Command2.Transaction = Transaction

                With Command0.Parameters
                    .AddWithValue("@Id", Id)
                    .AddWithValue("@Score", Result.Score)
                    .AddWithValue("@TimeUsage", Result.TimeUsage \ 10000)
                    .AddWithValue("@Details", Details.ToString())
                End With
                With Command1.Parameters
                    .AddWithValue("@ProblemId", ProblemId)
                    .AddWithValue("@UserId", UserId)
                End With
                With Command2.Parameters
                    .AddWithValue("@ProblemId", ProblemId)
                    .AddWithValue("@UserId", UserId)
                End With

                Command0.ExecuteNonQuery()
                Command1.ExecuteNonQuery()
                Command2.ExecuteNonQuery()
                Transaction.Commit()
            End Using
        End Sub
    End Class
End Namespace
