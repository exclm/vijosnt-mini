Imports VijosNT.Feeding
Imports VijosNT.Utility

Namespace Foreground
    Friend Class VijosContest
        Private Class ListViewRevertedNumericComparer
            Implements IComparer

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Int32 Implements System.Collections.IComparer.Compare
                Return Int32.Parse(DirectCast(y, ListViewItem).Text) - _
                    Int32.Parse(DirectCast(x, ListViewItem).Text)
            End Function
        End Class

        Private MustInherit Class AutoRef
            Private m_RefCount As Int32 = 0

            Public Sub AddRef()
                Interlocked.Increment(m_RefCount)
            End Sub

            Public Sub Release()
                If Interlocked.Decrement(m_RefCount) = 0 Then _
                    OnReleased()
            End Sub

            Public MustOverride Sub OnReleased()
        End Class

        Private Class Contest
            Inherits AutoRef

            Private m_Id As Int32
            Private m_ConnectionString As String

            Public Sub New(ByVal Id As Int32, ByVal ConnectionString As String)
                m_Id = Id
                m_ConnectionString = ConnectionString
                SetTesting()
            End Sub

            Private Function NewConnection() As SqlConnection
                Dim Result As New SqlConnection(m_ConnectionString)
                Result.Open()
                Return Result
            End Function

            Public Overrides Sub OnReleased()
                SetDone()
            End Sub

            Private Sub SetTesting()
                Using Connection As SqlConnection = NewConnection(), _
                    Command As New SqlCommand("UPDATE Test SET DoneNF = 0 WHERE ID = @ID", Connection)
                    Command.Parameters.Add(New SqlParameter("ID", m_Id))
                    Command.ExecuteNonQuery()
                End Using
            End Sub

            Private Sub SetDone()
                Using Connection As SqlConnection = NewConnection(), _
                    Command As New SqlCommand("UPDATE Test SET DoneNF = 1 WHERE ID = @ID", Connection)
                    Command.Parameters.Add(New SqlParameter("ID", m_Id))
                    Command.ExecuteNonQuery()
                End Using
            End Sub

            Public Sub SetTesting(ByVal Id As Int32)
                Using Connection As SqlConnection = NewConnection(), _
                    Command As New SqlCommand("UPDATE Testing SET DoneNF = 0 WHERE ID = @ID", Connection)
                    Command.Parameters.Add(New SqlParameter("ID", Id))
                    Command.ExecuteNonQuery()
                End Using
            End Sub

            Public Sub SetDone(ByVal Id As Int32, ByVal Score As Int32, ByVal RunTime As Int32, ByVal Body As String)
                Using Connection As SqlConnection = NewConnection(), _
                    Command As New SqlCommand("UPDATE Testing SET Score = @Score, RunTime = @RunTime, DoneNF = 1, Body = @Body WHERE ID = @ID", Connection)
                    Command.Parameters.Add(New SqlParameter("ID", Id))
                    Command.Parameters.Add(New SqlParameter("Score", Score))
                    Command.Parameters.Add(New SqlParameter("RunTime", RunTime))
                    Command.Parameters.Add(New SqlParameter("Body", Body))
                    Command.ExecuteNonQuery()
                End Using
            End Sub
        End Class

        Private Class TestUser
            Inherits AutoRef

            Private m_Form As Form
            Private m_Item As ListViewItem
            Private m_RecordList As New List(Of TestRecord)
            Private m_Contest As Contest
            Private m_Id As Int32

            Public Sub New(ByVal Form As Form, ByVal Item As ListViewItem, ByVal Contest As Contest, ByVal Id As Int32)
                m_Form = Form
                m_Item = Item
                m_Contest = Contest
                m_Id = Id
                Contest.AddRef()
                Contest.SetTesting(Id)
            End Sub

            Public Sub AddRecord(ByVal Record As TestRecord)
                SyncLock m_RecordList
                    m_RecordList.Add(Record)
                End SyncLock
            End Sub

            Public Overrides Sub OnReleased()
                Dim Builder As New StringBuilder()
                Dim TotalScore As Int32 = 0
                Dim TotalTime As Int32 = 0
                Dim Index As Int32 = 0

                Builder.Append("VijosNT <font color=""#ff8000""><strong>Mini</strong></font> ")
                Builder.Append(Assembly.GetExecutingAssembly().GetName().Version.ToString())
                Builder.Append("<br><br>")

                SyncLock m_RecordList
                    For Each Record As TestRecord In m_RecordList
                        Index += 1
                        Builder.Append("第 " & Index & " 题:<br>")
                        Record.Result.AppendToBuilder(Builder, Markup.Html)
                        TotalScore += Record.Result.Score
                        TotalTime += Record.Result.TimeUsage \ 10000
                        Builder.Append("<br>")
                    Next
                End SyncLock
                Finish(TotalScore, TotalTime)
                m_Contest.SetDone(m_Id, TotalScore, TotalTime, Builder.ToString())
                m_Contest.Release()
            End Sub

            Private Sub Finish(ByVal TotalScore As Int32, ByVal TotalTime As Int32)
                m_Form.BeginInvoke(New MethodInvoker( _
                    Sub()
                        m_Item.SubItems.Item(2).Text = TotalScore.ToString()
                        m_Item.SubItems.Item(3).Text = TotalTime.ToString() & "ms"
                        m_Item.SubItems.Item(5).Text = "True"
                        m_Item.EnsureVisible()
                    End Sub))
            End Sub
        End Class

        Private Class TestRecord
            Private m_Form As Form
            Private m_Item As ListViewItem
            Private m_User As TestUser
            Private m_Result As TestResult

            Public Sub New(ByVal Form As Form, ByVal Item As ListViewItem, ByVal User As TestUser)
                m_Form = Form
                m_Item = Item
                m_User = User
                User.AddRef()
                User.AddRecord(Me)
            End Sub

            Public Sub Finish(ByVal Result As TestResult)
                Try
                    m_Form.BeginInvoke(New MethodInvoker( _
                        Sub()
                            m_Result = Result
                            m_User.Release()

                            With m_Item
                                Dim Builder As New StringBuilder()
                                Result.AppendToBuilder(Builder, Markup.None)
                                .Tag = Builder.ToString()
                                .SubItems.Item(4).Text = Result.Flag.ToString()
                                .SubItems.Item(5).Text = Result.Score.ToString()
                                .SubItems.Item(6).Text = (Result.TimeUsage \ 10000).ToString() & "ms"
                                .EnsureVisible()
                            End With
                        End Sub))
                Catch ex As InvalidOperationException
                End Try
            End Sub

            Public ReadOnly Property Result() As TestResult
                Get
                    Return m_Result
                End Get
            End Property
        End Class

        Private m_Daemon As Daemon
        Private m_ConnectionString As String
        Private m_VijosPath As String
        Private m_CompilerTestId As Int32
        Private m_TestId As String
        Private m_AutoRefresh As Boolean

        Public Sub New(ByVal Daemon As Daemon, ByVal ConfigFileName As String)

            ' 此调用是设计器所必需的。
            InitializeComponent()

            ' 在 InitializeComponent() 调用之后添加任何初始化。
            m_Daemon = Daemon
            m_CompilerTestId = 0
            m_TestId = Nothing
            m_AutoRefresh = True

            ' 读取 Config.xml
            Dim Builder = New SqlConnectionStringBuilder()
            Using Reader = XmlReader.Create(ConfigFileName)
                If Not Reader.ReadToFollowing("Config") Then _
                    Throw New Exception("未找到名为 Config 的元素")
                If Not Reader.MoveToAttribute("DataName") Then _
                    Throw New Exception("未找到名为 DataName 的属性")
                Builder.InitialCatalog = Reader.ReadContentAsString()
                If Not Reader.MoveToAttribute("DataSource") Then _
                    Throw New Exception("未找到名为 DataSource 的属性")
                Builder.DataSource = Reader.ReadContentAsString()
                If Not Reader.MoveToAttribute("Password") Then _
                    Throw New Exception("未找到名为 Password 的属性")
                Builder.Password = Reader.ReadContentAsString()
                If Not Reader.MoveToAttribute("Path") Then _
                    Throw New Exception("未找到名为 Path 的属性")
                m_VijosPath = Reader.ReadContentAsString()
                If Not m_VijosPath.EndsWith("\") Then _
                    m_VijosPath = m_VijosPath & "\"
                If Not Reader.MoveToAttribute("User") Then _
                    Throw New Exception("未找到名为 User 的属性")
                Builder.UserID = Reader.ReadContentAsString()
            End Using

            m_ConnectionString = Builder.ToString()

            SetDoubleBuffered(lvCompiler)
            SetDoubleBuffered(lvContest)
            SetDoubleBuffered(lvRecord)
            SetDoubleBuffered(lvTesting)
        End Sub

        Private Sub TestCompiler(ByVal Name As String, ByVal FileName As String, ByVal Code As String)
            m_CompilerTestId += 1

            Dim Item As ListViewItem = lvCompiler.Items.Add(m_CompilerTestId.ToString())
            Item.SubItems.AddRange(New String() {Name, String.Empty, String.Empty, String.Empty})
            If Not m_Daemon.DirectFeed(txtNamespace0.Text, FileName, Code, _
                Sub(Result As TestResult)
                    Try
                        BeginInvoke(New MethodInvoker( _
                             Sub()
                                 Dim Builder As New StringBuilder()
                                 Result.AppendToBuilder(Builder, Markup.None)
                                 Item.Tag = Builder.ToString()
                                 Item.SubItems.Item(2).Text = FormatEnumString(Result.Flag.ToString())
                                 Item.SubItems.Item(3).Text = Result.Score.ToString()
                                 Item.SubItems.Item(4).Text = (Result.TimeUsage \ 10000).ToString() & "ms"
                                 Item.EnsureVisible()
                             End Sub))
                    Catch ex As InvalidOperationException
                    End Try
                End Sub) Then
                Item.Tag = "评测时发生内部错误"
                Item.SubItems.Item(2).Text = "Internal Error"
                Item.SubItems.Item(3).Text = "0"
                Item.SubItems.Item(4).Text = "0ms"
                Item.EnsureVisible()
            End If
        End Sub

        Private Sub btnTestCompiler_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTestCompiler.Click
            Dim ProblemId = txtProblemId.Text

            lvCompiler.BeginUpdate()
            Try
                TestCompiler("Free Pascal", ProblemId & ".pas", "var a,b:longint;begin;read(a,b);write(a+b);end.")
                TestCompiler("GCC", ProblemId & ".c", "main(a,b){scanf(""%d%d"",&a,&b);printf(""%d\n"",a+b);}")
                TestCompiler("G++", ProblemId & ".cpp", "#include <iostream>" & vbCrLf & "int main(){int a,b;std::cin>>a>>b;std::cout<<a+b;}")
            Catch ex As Exception
                MsgBox(ex.ToString(), MsgBoxStyle.Critical)
            Finally
                lvCompiler.EndUpdate()
            End Try
        End Sub

        Private Sub tmrAutoRefresh_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrAutoRefresh.Tick
            RefreshContest(False)
        End Sub

        Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
            RefreshContest(True)
        End Sub

        Private Function NewConnection() As SqlConnection
            Dim Result As New SqlConnection(m_ConnectionString)
            Result.Open()
            Return Result
        End Function

        Private Sub RefreshContest(ByVal ShowError As Boolean)
            Try
                Dim sb As New StringBuilder
                sb.AppendLine("SELECT t.ID, t.Name, t.DoneNF")
                sb.AppendLine("FROM Test t")
                sb.AppendLine("WHERE 0 = 0")
                If Not cbDisplayLive.Checked Then _
                    sb.AppendLine("AND t.EndTime <= GETDATE()")
                If Not cbDisplayTested.Checked Then _
                    sb.AppendLine("AND t.DoneNF = 0")
                Using Connection As SqlConnection = NewConnection(), _
                    Command As New SqlCommand(sb.ToString(), Connection), _
                    Reader As SqlDataReader = Command.ExecuteReader()
                    lvContest.BeginUpdate()
                    Try
                        For Each Item As ListViewItem In lvContest.Items
                            Item.Tag = New Object()
                        Next
                        While Reader.Read()
                            Dim Id As String = "T" & Reader.Item("ID")
                            Dim Title As String = Reader.Item("Name")
                            Dim DoneNF As Boolean = Reader.Item("DoneNF")
                            Dim Item As ListViewItem
                            If lvContest.Items.ContainsKey(Id) Then
                                Item = lvContest.Items(Id)
                                Item.Tag = Nothing
                            Else
                                Item = lvContest.Items.Add(Id, Id, 0)
                                Item.SubItems.AddRange(New String() {"", ""})
                            End If
                            Item.SubItems(1).Text = Title
                            Item.SubItems(2).Text = DoneNF.ToString()
                        End While
                        For Each Item As ListViewItem In lvContest.Items
                            If Item.Tag IsNot Nothing Then
                                Item.Remove()
                            End If
                        Next
                    Finally
                        lvContest.EndUpdate()
                    End Try
                End Using
            Catch ex As Exception
                If ShowError Then _
                    MsgBox(ex.ToString(), MsgBoxStyle.Critical)
            End Try
        End Sub

        Private Sub cbAutoRefresh_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAutoRefresh.CheckedChanged
            tmrAutoRefresh.Enabled = cbAutoRefresh.Checked
        End Sub

        Private Sub cbDisplayLive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbDisplayLive.CheckedChanged
            RefreshContest(False)
        End Sub

        Private Sub cbDisplayTested_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbDisplayTested.CheckedChanged
            RefreshContest(False)
        End Sub

        Private Sub btnMoveTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveTo.Click
            If lvContest.SelectedItems.Count = 0 Then
                MsgBox("您必须选中要测评的比赛。", MsgBoxStyle.Information)
                Return
            End If
            txtTestId.Text = lvContest.SelectedItems(0).Text
            tabMain.SelectedTab = tpTest
            btnDisplay_Click(sender, e)
        End Sub

        Private Function GetTestId() As Boolean
            Dim TestId As String = txtTestId.Text
            If TestId Is Nothing OrElse TestId.Length <> 5 OrElse TestId(0) <> "T"c Then _
                Return False
            For Index As Int32 = 1 To TestId.Length - 1
                If Not "0123456789".Contains(TestId(Index)) Then _
                    Return False
            Next
            m_TestId = TestId.Substring(1)
            Return True
        End Function

        Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
            If Not GetTestId() Then
                MsgBox("测试号必须为大写字母 T 开头, 并紧随四位数字。", MsgBoxStyle.Information)
                Return
            End If

            Try
                Dim sb As New StringBuilder
                sb.AppendLine("SELECT t.ID, u.Username, t.Score, t.RunTime, t.Compiler, t.DoneNF")
                sb.AppendLine("FROM Testing t, [User] u")
                sb.AppendLine("WHERE t.TestID = " & m_TestId)
                sb.AppendLine("AND t.UserID = u.ID")
                Using Connection As SqlConnection = NewConnection(), _
                    Command As New SqlCommand(sb.ToString(), Connection), _
                    Reader As SqlDataReader = Command.ExecuteReader()
                    lvTesting.BeginUpdate()
                    Try
                        lvTesting.Items.Clear()
                        While Reader.Read()
                            Dim Id As Int32 = Reader.Item("ID")
                            Dim Username As String = Reader.Item("Username")
                            Dim Score As Int32 = Reader.Item("Score")
                            Dim RunTime As Double = Reader.Item("RunTime")
                            Dim Compiler As String = Reader.Item("Compiler")
                            Dim DoneNF As Boolean = Reader.Item("DoneNF")
                            With lvTesting.Items.Add(Id.ToString(), Id.ToString(), 0)
                                .SubItems.Add(Username)
                                .SubItems.Add(Score.ToString())
                                .SubItems.Add(RunTime.ToString() & "ms")
                                .SubItems.Add(Compiler)
                                .SubItems.Add(DoneNF)
                                .Checked = Not DoneNF
                            End With
                        End While
                    Finally
                        lvTesting.EndUpdate()
                    End Try
                End Using
            Catch ex As Exception
                MsgBox(ex.ToString(), MsgBoxStyle.Critical)
            End Try
        End Sub

        Private Sub btnSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click
            lvTesting.BeginUpdate()
            Try
                For Each Item As ListViewItem In lvTesting.Items
                    Item.Checked = True
                Next
            Catch ex As Exception
                MsgBox(ex.ToString(), MsgBoxStyle.Critical)
            Finally
                lvTesting.EndUpdate()
            End Try
        End Sub

        Private Sub btnSelectNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectNone.Click
            lvTesting.BeginUpdate()
            Try
                For Each Item As ListViewItem In lvTesting.Items
                    Item.Checked = False
                Next
            Catch ex As Exception
                MsgBox(ex.ToString(), MsgBoxStyle.Critical)
            Finally
                lvTesting.EndUpdate()
            End Try
        End Sub

        Private Sub btnSelectUntested_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectUntested.Click
            lvTesting.BeginUpdate()
            Try
                For Each Item As ListViewItem In lvTesting.Items
                    Item.Checked = (Item.SubItems(5).Text = "False")
                Next
            Catch ex As Exception
                MsgBox(ex.ToString(), MsgBoxStyle.Critical)
            Finally
                lvTesting.EndUpdate()
            End Try
        End Sub

        Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
            Static RecordId As Int32 = 0
            btnStart.Enabled = False
            Dim CheckedItems = lvTesting.CheckedItems
            MiniThreadPool.Queue( _
                Sub()
                    Try
                        Dim ct As New Contest(Int32.Parse(m_TestId), m_ConnectionString)
                        ct.AddRef()
                        Try
                            For Each Item As ListViewItem In CheckedItems
                                Dim Item0 = Item
                                Using Connection As SqlConnection = NewConnection(), _
                                    Command As New SqlCommand( _
                                        "SELECT tt.UserID, u.Username, tt.Compiler, t.ProblemList" & vbCrLf & _
                                        "FROM Testing tt, Test t, [User] u" & vbCrLf & _
                                        "WHERE tt.ID = " & Item0.Text & vbCrLf & _
                                        "AND tt.TestID = t.ID" & vbCrLf & _
                                        "AND tt.UserID = u.ID", Connection), _
                                    Reader As SqlDataReader = Command.ExecuteReader()
                                    If Reader.Read() Then
                                        Item0.Checked = False
                                        Dim UserId As Int32 = Reader.Item("UserID")
                                        Dim Username As String = Reader.Item("Username")
                                        Dim Compiler As String = Reader.Item("Compiler")
                                        Dim ProblemList As String() = Split(Reader.Item("ProblemList"), "|")
                                        Select Case Compiler
                                            Case "", "FPC", "TPC"
                                                Compiler = "FPC"
                                            Case "GCC"
                                                Compiler = "GCC"
                                            Case "CPP"
                                                Compiler = "G++"
                                        End Select
                                        Dim ItemText As String = Nothing
                                        Invoke(New MethodInvoker( _
                                            Sub()
                                                ItemText = Item0.Text
                                            End Sub))
                                        Dim tu As New TestUser(Me, Item0, ct, Int32.Parse(ItemText))
                                        tu.AddRef()
                                        Try
                                            For Each Problem In ProblemList
                                                Dim TrimmedProblem = Problem.Trim()
                                                If TrimmedProblem.Length <> 0 Then
                                                    RecordId += 1
                                                    Dim RecordItem As ListViewItem = Nothing
                                                    Invoke(New MethodInvoker( _
                                                        Sub()
                                                            RecordItem = lvRecord.Items.Add(RecordId.ToString())
                                                            RecordItem.SubItems.AddRange(New String() {Username, "P" & TrimmedProblem, Compiler, String.Empty, String.Empty, String.Empty})
                                                        End Sub))
                                                    Dim tr As New TestRecord(Me, RecordItem, tu)
                                                    Using FinishedEvent As New ManualResetEvent(False)
                                                        If m_Daemon.DirectFeed2(txtNamespace1.Text, "P" & TrimmedProblem & VijosDataSource.GetCompilerExtension(Compiler), m_VijosPath & "Upload\U" & UserId.ToString() & "\P" & TrimmedProblem & ".pas", _
                                                            Sub(Result As TestResult)
                                                                tr.Finish(Result)
                                                                FinishedEvent.Set()
                                                            End Sub) Then
                                                            FinishedEvent.WaitOne()
                                                        Else
                                                            tr.Finish(New TestResult(Nothing, TestResultFlag.InternalError, "评测时发生内部错误", 0, 0, 0, Nothing))
                                                        End If
                                                    End Using
                                                End If
                                            Next
                                        Finally
                                            tu.Release()
                                        End Try
                                    End If
                                End Using
                            Next
                        Finally
                            ct.Release()
                        End Try
                    Catch ex As Exception
                        BeginInvoke(New MethodInvoker( _
                            Sub()
                                MsgBox(ex.ToString(), MsgBoxStyle.Critical)
                            End Sub))
                    Finally
                        BeginInvoke(New MethodInvoker( _
                            Sub()
                                btnStart.Enabled = True
                            End Sub))
                    End Try
                End Sub, Nothing)
        End Sub

        Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            lvCompiler.ListViewItemSorter = New ListViewRevertedNumericComparer()
            lvRecord.ListViewItemSorter = New ListViewRevertedNumericComparer()
        End Sub

        Private Sub lvCompiler_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvCompiler.MouseDoubleClick
            btnDisplayCompiler_Click(sender, e)
        End Sub

        Private Sub lvContest_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvContest.MouseDoubleClick
            btnMoveTo_Click(sender, e)
        End Sub

        Private Sub btnDisplayCompiler_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplayCompiler.Click
            If lvCompiler.SelectedItems.Count = 0 Then
                MsgBox("您必须选中要查看的记录。", MsgBoxStyle.Critical)
                Return
            End If
            MsgBox(lvCompiler.SelectedItems(0).Tag, MsgBoxStyle.Information)
        End Sub

        Private Sub btnClearCompiler_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearCompiler.Click
            lvCompiler.Items.Clear()
        End Sub

        Private Sub btnClearRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearRecord.Click
            lvRecord.Items.Clear()
        End Sub

        Private Sub btnDisplayRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplayRecord.Click
            If lvRecord.SelectedItems.Count = 0 Then
                MsgBox("您必须选中要查看的记录。", MsgBoxStyle.Critical)
                Return
            End If
            MsgBox(lvRecord.SelectedItems(0).Tag, MsgBoxStyle.Information)
        End Sub

        Private Sub lvRecord_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvRecord.MouseDoubleClick
            btnDisplayRecord_Click(sender, e)
        End Sub

        Private Sub tpContest_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tpContest.Enter
            RefreshContest(False)
            cbAutoRefresh.Checked = m_AutoRefresh
        End Sub

        Private Sub tpContest_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tpContest.Leave
            m_AutoRefresh = cbAutoRefresh.Checked
            cbAutoRefresh.Checked = False
        End Sub

        Private Sub txtNamespace0_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNamespace0.TextChanged
            If txtNamespace1.Text <> txtNamespace0.Text Then _
                txtNamespace1.Text = txtNamespace0.Text
        End Sub

        Private Sub txtNamespace1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNamespace1.TextChanged
            If txtNamespace0.Text <> txtNamespace1.Text Then _
                txtNamespace0.Text = txtNamespace1.Text
        End Sub
    End Class
End Namespace