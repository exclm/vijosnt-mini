Imports VijosNT.Testing

Namespace Sources
    Friend Class VijosSource
        Inherits Source

        Private Class Config
            Public Sub New(ByVal Index As Int32, ByVal InputFileName As String, ByVal AnswerFileName As String, ByVal TimeQuota As Int64, ByVal Weight As Int32)
                Me.Index = Index
                Me.InputFileName = InputFileName
                Me.AnswerFileName = AnswerFileName
                Me.TimeQuota = TimeQuota
                Me.Weight = Weight
            End Sub

            Public Index As Int32
            Public InputFileName As String
            Public AnswerFileName As String
            Public TimeQuota As Int64
            Public Weight As Int32
        End Class

        Private m_ProblemRoot As String
        Private m_MemoryQuota As Int64
        Private m_ConnectionString As String
        Private m_TesterId As Int32
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

            m_ProblemRoot = Nothing
            m_MemoryQuota = 128 * 1024 * 1024
            m_TesterId = 1

            For Each Parameter In Parameters.Split(New Char() {";"c})
                Dim Position = Parameter.IndexOf("="c)
                If Position = -1 Then Continue For
                Dim Key = Parameter.Substring(0, Position)
                Dim Value = Parameter.Substring(Position + 1)
                Select Case Key.ToLower()
                    Case "root"
                        Using Reader = XmlReader.Create(Path.Combine(Value, "Config.xml"))
                            If Reader.ReadToFollowing("Config") Then
                                If Reader.MoveToAttribute("DataName") Then _
                                    Database = Reader.ReadContentAsString()
                                If Reader.MoveToAttribute("DataSource") Then _
                                    Server = Reader.ReadContentAsString()
                                If Reader.MoveToAttribute("Password") Then _
                                    Password = Reader.ReadContentAsString()
                                If Reader.MoveToAttribute("User") Then _
                                    UserName = Reader.ReadContentAsString()
                                If Reader.MoveToAttribute("TesterID") Then _
                                    m_TesterId = Int32.Parse(Reader.ReadContentAsString())
                            End If
                        End Using
                        m_ProblemRoot = Path.Combine(Value, "Problem")
                    Case "memoryquota"
                        Int64.TryParse(Value, m_MemoryQuota)
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
            If m_ProblemRoot Is Nothing Then _
                Throw New ArgumentNullException("Root")
            If m_MemoryQuota <= 0 Then _
                Throw New ArgumentOutOfRangeException("MemoryQuota", "MemoryQuota 必须为一个正整数")

            Dim ConnectionBuilder = New SqlConnectionStringBuilder
            ConnectionBuilder.DataSource = Server
            ConnectionBuilder.InitialCatalog = Database
            ConnectionBuilder.UserID = UserName
            ConnectionBuilder.Password = Password
            ConnectionBuilder.Pooling = True
            m_ConnectionString = ConnectionBuilder.ToString()

            m_SelectCodeCommand = New SqlCommand( _
                "SELECT ProblemID, Compiler, Code FROM Record WHERE ID = @Id")
            m_SelectHeaderCommand = New SqlCommand( _
                "SELECT ProblemID, UserID FROM Record WHERE ID = @Id")
            m_UpdateTakenCommand = New SqlCommand( _
                "UPDATE Record SET CheckNF = -@TesterId WHERE ID = @Id AND CheckNF = 0")
            m_UpdateTakenCommand.Parameters.AddWithValue("@TesterId", m_TesterId)
            m_UpdateUntakeCommand = New SqlCommand( _
                "UPDATE Record SET CheckNF = 0 WHERE ID = @Id")
            m_UpdateFinalCommand = New SqlCommand( _
                "UPDATE Record SET CheckNF = @TesterId, Score = @Score, RunTime = @TimeUsage, Body = @Details WHERE ID = @Id")
            m_UpdateFinalCommand.Parameters.AddWithValue("@TesterId", m_TesterId)
            m_UpdateProblemCommand = New SqlCommand( _
                "DECLARE @UserListOKPtr varbinary(16)" & vbCrLf & _
                "UPDATE Problem" & vbCrLf & _
                "SET OK = OK + 1, @UserListOKPtr = TEXTPTR(UserListOK)" & vbCrLf & _
                "WHERE ID = @ProblemId AND charindex('|' + @UserId + '|', UserListOK) = 0;" & vbCrLf & _
                "IF NOT(@UserListOKPtr IS NULL) BEGIN" & vbCrLf & _
                "UPDATETEXT Problem.UserListOK @UserListOKPtr NULL 0 @UserId" & vbCrLf & _
                "UPDATETEXT Problem.UserListOK @UserListOKPtr NULL 0 '|'" & vbCrLf & _
                "END;")
            m_UpdateUserCommand = New SqlCommand( _
                "DECLARE @ProblemListOKPtr varbinary(16)" & vbCrLf & _
                "UPDATE [User]" & vbCrLf & _
                "SET ProblemOK = ProblemOK + 1, @ProblemListOKPtr = TEXTPTR(ProblemListOK)" & vbCrLf & _
                "WHERE ID = @UserId AND charindex('|' + @ProblemId + '|', ProblemListOK) = 0;" & vbCrLf & _
                "IF NOT(@ProblemListOKPtr IS NULL) BEGIN" & vbCrLf & _
                "UPDATETEXT [User].ProblemListOK @ProblemListOKPtr NULL 0 @ProblemId" & vbCrLf & _
                "UPDATETEXT [User].ProblemListOK @ProblemListOKPtr NULL 0 '|'" & vbCrLf & _
                "END;")
        End Sub

        Public Overrides Function TryLoad(ByVal Id As String) As IEnumerable(Of TestCase)
            Try
                Dim ProblemRoot As String = Path.Combine(m_ProblemRoot, Id)
                Dim Result As New List(Of TestCase)
                Dim Configs As IEnumerable(Of Config) = LoadConfig(ProblemRoot)
                Dim Weight As Int32 = 0
                Dim Index As Int32 = 0
                Dim TotalWeight As Int32 = 0
                For Each Config As Config In Configs
                    Index += 1
                    TotalWeight += Config.Weight
                Next
                Weight = 100
                For Each Config As Config In Configs
                    Index -= 1
                    If Index = 0 Then
                        Config.Weight = Weight
                    Else
                        Config.Weight = Config.Weight * 100 \ TotalWeight
                        Weight -= Config.Weight
                    End If
                    Result.Add(LoadTestCase(ProblemRoot, Config))
                Next
                Return Result
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Private Function LoadConfig(ByVal ProblemRoot As String) As IEnumerable(Of Config)
            Using Reader As New StreamReader(Path.Combine(ProblemRoot, "Config.ini"))
                Dim Count As Int32 = Int32.Parse(Reader.ReadLine())
                Dim Result As New List(Of Config)
                For Index As Int32 = 0 To Count - 1
                    Dim Arguments As String() = Split(Reader.ReadLine(), "|", 5)
                    Result.Add(New Config(Index + 1, Path.Combine("Input", Arguments(0)), Path.Combine("Output", Arguments(1)), _
                        Convert.ToInt64(Double.Parse(Arguments(2)) * 10000 * 1000), Int32.Parse(Arguments(3))))
                Next
                Return Result
            End Using
        End Function

        Private Function LoadTestCase(ByVal ProblemRoot As String, ByVal Config As Config) As TestCase
            Return New LocalTestCase(Config.Index, Config.Weight, Path.Combine(ProblemRoot, Config.InputFileName), Path.Combine(ProblemRoot, Config.AnswerFileName), Config.TimeQuota, m_MemoryQuota)
        End Function

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

        Public Shared Function GetCompilerExtension(ByVal CompilerName As String) As String
            If CompilerName.StartsWith(".") Then
                Return CompilerName
            Else
                Select Case CompilerName.ToLower()
                    Case "c", "gcc"
                        Return ".c"
                    Case "cpp", "gpp", "g++"
                        Return ".cpp"
                    Case Else
                        Return ".pas"
                End Select
            End If
        End Function

        Public Overrides Function Take(ByVal Id As Int32) As SourceRecord
            Using Connection As SqlConnection = CloneConnection()
                Using Command As SqlCommand = CloneCommand(m_UpdateTakenCommand, Connection)
                    Command.Parameters.AddWithValue("@Id", Id)
                    If Command.ExecuteNonQuery() = 0 Then Return Nothing
                End Using
                Using Command As SqlCommand = CloneCommand(m_SelectCodeCommand.Clone, Connection)
                    Command.Parameters.AddWithValue("@Id", Id)
                    Using Reader As SqlDataReader = Command.ExecuteReader()
                        If Not Reader.Read() Then _
                            Return Nothing
                        Return New SourceRecord("P" & Reader("ProblemID") & GetCompilerExtension(Reader("Compiler")), Reader("Code"))
                    End Using
                End Using
            End Using
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
                    ProblemId = Reader("ProblemID")
                    UserId = Reader("UserID")
                End Using
            End Using

            Dim Details As New StringBuilder()
            Details.AppendLine("VijosNT [color=#ff8000][b]Mini[/b][/color] " & Assembly.GetExecutingAssembly().GetName().Version.ToString())
            Details.AppendLine()
            Result.AppendToBuilder(Details, Markup.Ubb)

            Using Connection As SqlConnection = CloneConnection(), _
                Command0 As SqlCommand = CloneCommand(m_UpdateFinalCommand, Connection), _
                Command1 As SqlCommand = CloneCommand(m_UpdateProblemCommand, Connection), _
                Command2 As SqlCommand = CloneCommand(m_UpdateUserCommand, Connection), _
                Transaction As SqlTransaction = Connection.BeginTransaction()

                Command0.Transaction = Transaction

                With Command0.Parameters
                    .AddWithValue("@Id", Id)
                    .AddWithValue("@Score", Result.Score)
                    .AddWithValue("@TimeUsage", Result.TimeUsage \ 10000)
                    .AddWithValue("@Details", Details.ToString())
                End With
                Command0.ExecuteNonQuery()

                If Result.Score = 100 Then
                    Command1.Transaction = Transaction
                    Command2.Transaction = Transaction
                    With Command1.Parameters
                        .AddWithValue("@ProblemId", ProblemId)
                        .AddWithValue("@UserId", UserId)
                    End With
                    With Command2.Parameters
                        .AddWithValue("@ProblemId", ProblemId)
                        .AddWithValue("@UserId", UserId)
                    End With
                    Command1.ExecuteNonQuery()
                    Command2.ExecuteNonQuery()
                End If

                Transaction.Commit()
            End Using
        End Sub
    End Class
End Namespace
