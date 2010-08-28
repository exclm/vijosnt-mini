Imports VijosNT.LocalDb
Imports VijosNT.Utility
Imports VijosNT.Win32

Namespace Foreground
    Friend Class ConsoleForm
        Private m_Daemon As Daemon
        Private m_Pages As Dictionary(Of String, TabPage)
        Private m_RootNode As TreeNode
        Private m_Service As Service

#Region "Constructor"
        Public Sub New(ByVal Daemon As Daemon)
            ' 此调用是设计器所必需的。
            InitializeComponent()

            ' 在 InitializeComponent() 调用之后添加任何初始化。
            m_Daemon = Daemon
            m_Pages = New Dictionary(Of String, TabPage)

            For Each Page As TabPage In TabControl.TabPages
                m_Pages.Add(Page.Name, Page)
            Next
            TabControl.TabPages.Clear()

            m_RootNode = NavigationTree.Nodes.Item("Root")
            NavigationTree.ExpandAll()
        End Sub
#End Region

#Region "Page management"
        Private ReadOnly Property CurrentPage() As String
            Get
                With TabControl.TabPages
                    If .Count <> 0 Then
                        Return .Item(0).Name
                    Else
                        Return Nothing
                    End If
                End With
            End Get
        End Property

        Private Sub DisplayPage(ByVal Name As String)
            With TabControl.TabPages
                LeavePage()
                .Clear()
                EnterPage(Name)
                .Add(m_Pages(Name))
                RefreshPage()
            End With
        End Sub

        Private Sub LeavePage()
            Select Case CurrentPage
                Case "RootPage"
                    ServiceTimer.Stop()
                    If m_Service IsNot Nothing Then
                        m_Service.Close()
                        m_Service = Nothing
                    End If
                Case "CompilerPage"
                    ApplyCompilerButton.PerformClick()
                Case "TestSuitePage"
                    ApplyTestSuiteButton.PerformClick()
                Case "ExecutorPage"
                    ApplyExecutorButton.PerformClick()
            End Select
            StatusLabel.Text = Nothing
        End Sub

        Private Sub EnterPage(ByVal Name As String)
            Select Case Name
                Case "RootPage"
                    m_Service = m_Daemon.OpenService()
                    ServiceTimer.Start()
            End Select
        End Sub

        Private Sub RefreshPage()
            Select Case CurrentPage
                Case "RootPage"
                    RefreshServiceStatus()
                    RefreshLocalDataSource()
                Case "CompilerPage"
                    Using Reader As IDataReader = CompilerMapping.GetHeaders()
                        RefreshListView(CompilerList, Reader, "Id", "Pattern", "CommandLine")
                    End Using
                    CompilerList_SelectedIndexChanged(Nothing, Nothing)
                Case "TestSuitePage"
                    Using Reader As IDataReader = TestSuiteMapping.GetAll()
                        RefreshListView(TestSuiteList, Reader, "Id", "Pattern", "ClassName", "Parameter")
                    End Using
                    TestSuiteList_SelectedIndexChanged(Nothing, Nothing)
                Case "ExecutorPage"
                    RemoveHandler ExecutorSlotsText.TextChanged, AddressOf ExecutorSlotsText_TextChanged
                    ExecutorSlotsText.Text = Config.ExecutorSlots
                    AddHandler ExecutorSlotsText.TextChanged, AddressOf ExecutorSlotsText_TextChanged
                    If Config.EnableSecurity Then
                        ExecutorSecurityCombo.SelectedIndex = 0
                    Else
                        ExecutorSecurityCombo.SelectedIndex = 1
                    End If
            End Select
        End Sub
#End Region

#Region "Global events"
        Private Sub ExitMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitMenu.Click
            Close()
        End Sub

        Private Sub AboutMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutMenu.Click
            MessageBox.Show("Hello world!", "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub WikiToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WikiToolStripMenuItem.Click
            Process.Start("http://code.google.com/p/vijosnt-mini/w/list")
        End Sub

        Private Sub HomePageMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HomePageMenu.Click
            Process.Start("http://code.google.com/p/vijosnt-mini/")
        End Sub

        Private Sub CheckUpdateMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckUpdateMenu.Click
            Process.Start("http://code.google.com/p/vijosnt-mini/downloads/list")
        End Sub

        Private Sub ReportIssue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReportIssue.Click
            Process.Start("http://code.google.com/p/vijosnt-mini/issues/list")
        End Sub

        Private Sub NavigationTree_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles NavigationTree.AfterSelect
            DisplayPage(e.Node.Name & "Page")
        End Sub

        Private Sub ConsoleForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            LeavePage()
        End Sub

        Private Sub ConsoleForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
            If e.KeyData = Keys.F5 Then
                RefreshPage()
            End If
        End Sub
#End Region

#Region "Root page"
        Private Sub RefreshServiceStatus()
            If m_Service Is Nothing Then
                InstallButton.Enabled = True
                UninstallButton.Enabled = False
                StatusLabel.Text = "VijosNT 服务未安装"
            Else
                InstallButton.Enabled = False
                UninstallButton.Enabled = True
                Dim State As ServiceState = m_Service.State
                Select Case State
                    Case ServiceState.SERVICE_START_PENDING
                        StatusLabel.Text = "VijosNT 服务正在启动"
                    Case ServiceState.SERVICE_RUNNING
                        StatusLabel.Text = "VijosNT 服务已启动"
                    Case ServiceState.SERVICE_STOP_PENDING
                        StatusLabel.Text = "VijosNT 服务正在停止"
                    Case ServiceState.SERVICE_STOPPED
                        StatusLabel.Text = "VijosNT 服务已停止"
                    Case Else
                        StatusLabel.Text = "VijosNT 服务处于未知状态 (" & State.ToString() & ")"
                End Select
            End If
        End Sub

        Private Sub RefreshLocalDataSource()
            Using Reader As IDataReader = Record.GetHeaders()
                RefreshListView(LocalSourceList, Reader, "Id", "Id", "$Flag", "FileName", "%Score", "!TimeUsage", "@MemoryUsage", "#Date")
            End Using
        End Sub

        Private Sub InstallButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InstallButton.Click
            m_Service = m_Daemon.CreateService()
            m_Service.Start()
            m_Daemon.ServiceInstalled = True
            RefreshServiceStatus()
        End Sub

        Private Sub UninstallButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UninstallButton.Click
            m_Service.Stop()
            m_Service.Delete()
            m_Service.Close()
            m_Service = Nothing
            RefreshServiceStatus()
        End Sub

        Private Sub ServiceTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ServiceTimer.Tick
            RefreshServiceStatus()
        End Sub

        Private Sub RefreshLocalButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefershLocalButton.Click
            RefreshLocalDataSource()
        End Sub

        Private Sub ClearLocalButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearLocalButton.Click
            If MessageBox.Show("确定要清除所有记录吗?", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Record.Clear()
                RefreshLocalDataSource()
            End If
        End Sub
#End Region

#Region "Compiler page"
        Private Sub CompilerList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompilerList.SelectedIndexChanged
            With CompilerList.SelectedItems
                If .Count <> 0 Then
                    CompilerProperty.SelectedObject = CompilerMapping.GetConfig(.Item(0).Tag)
                    RemoveCompilerButton.Enabled = True
                    With .Item(0)
                        MoveUpCompilerButton.Enabled = .Index <> 0
                        MoveDownCompilerButton.Enabled = .Index <> CompilerList.Items.Count - 1
                    End With
                Else
                    CompilerProperty.SelectedObject = Nothing
                    RemoveCompilerButton.Enabled = False
                    MoveUpCompilerButton.Enabled = False
                    MoveDownCompilerButton.Enabled = False
                End If
            End With
        End Sub

        Private Sub AddCompilerButton_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddCompilerButton.ButtonClick, NewCompilerMenu.Click
            CompilerMapping.Add(".*", String.Empty, String.Empty, String.Empty, 15000 * 10000, Nothing, Nothing, String.Empty, String.Empty, String.Empty, String.Empty)
            ApplyCompilerButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub CompilerProperty_PropertyValueChanged(ByVal s As Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles CompilerProperty.PropertyValueChanged
            ApplyCompilerButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub RemoveCompilerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveCompilerButton.Click
            CompilerMapping.Remove(CompilerList.SelectedItems.Item(0).Tag)
            ApplyCompilerButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub MoveUpCompilerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveUpCompilerButton.Click
            With CompilerList.SelectedItems.Item(0)
                Dim Id As Int32 = .Tag
                With CompilerList.Items(.Index - 1)
                    CompilerMapping.Swap(Id, .Tag)
                    .Selected = True
                End With
            End With
            ApplyCompilerButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub MoveDownCompilerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveDownCompilerButton.Click
            With CompilerList.SelectedItems.Item(0)
                Dim Id As Int32 = .Tag
                With CompilerList.Items(.Index + 1)
                    CompilerMapping.Swap(Id, .Tag)
                    .Selected = True
                End With
            End With
            ApplyCompilerButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub GccMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GccMenu.Click
            Dim Gcc As String = DetectMingw("gcc.exe")
            If Gcc IsNot Nothing Then
                CompilerMapping.Add(".c", Gcc, "gcc -O2 -s -o foo.exe foo.c -lm", "PATH=" & Path.GetDirectoryName(Gcc) & ";%PATH%", 15000 * 10000, Nothing, Nothing, "foo.c", "foo.exe", String.Empty, String.Empty)
                ApplyCompilerButton.Enabled = True
                RefreshPage()
            End If
        End Sub

        Private Sub GppMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GppMenu.Click
            Dim Gpp As String = DetectMingw("g++.exe")
            If Gpp IsNot Nothing Then
                CompilerMapping.Add(".cpp;.cxx;.cc", Gpp, "g++ -O2 -s -o foo.exe foo.cpp -lm", "PATH=" & Path.GetDirectoryName(Gpp) & ";%PATH%", 15000 * 10000, Nothing, Nothing, "foo.cpp", "foo.exe", String.Empty, String.Empty)
                ApplyCompilerButton.Enabled = True
                RefreshPage()
            End If
        End Sub

        Private Sub MscMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MscMenu.Click
            Dim Ide As String = Nothing
            Dim Include As String = Nothing
            Dim [Lib] As String = Nothing
            Dim ClPath As String = DetectMscl(Ide, Include, [Lib])
            If ClPath IsNot Nothing Then
                Dim SdkPath As String = DetectMssdk()
                If SdkPath IsNot Nothing Then
                    Dim EnvironmentVariables As New List(Of String)
                    If Ide IsNot Nothing Then _
                        EnvironmentVariables.Add("PATH=" & Path.GetDirectoryName(ClPath) & ";" & Ide & ";%PATH%")
                    If Include IsNot Nothing Then _
                        EnvironmentVariables.Add("INCLUDE=" & Include & ";" & Path.Combine(SdkPath, "Include") & ";%INCLUDE%")
                    If [Lib] IsNot Nothing Then _
                        EnvironmentVariables.Add("LIB=" & [Lib] & ";" & Path.Combine(SdkPath, "Lib") & ";%LIB%")
                    CompilerMapping.Add(".c", ClPath, "cl /O2 /TC /Fefoo.exe foo.c", Join(EnvironmentVariables.ToArray(), "|"), 15000 * 10000, Nothing, Nothing, "foo.c", "foo.exe", String.Empty, String.Empty)
                    ApplyCompilerButton.Enabled = True
                    RefreshPage()
                End If
            End If
        End Sub

        Private Sub MscppMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MscppMenu.Click
            Dim Ide As String = Nothing
            Dim Include As String = Nothing
            Dim [Lib] As String = Nothing
            Dim ClPath As String = DetectMscl(Ide, Include, [Lib])
            If ClPath IsNot Nothing Then
                Dim SdkPath As String = DetectMssdk()
                If SdkPath IsNot Nothing Then
                    Dim EnvironmentVariables As New List(Of String)
                    If Ide IsNot Nothing Then _
                        EnvironmentVariables.Add("PATH=" & Path.GetDirectoryName(ClPath) & ";" & Ide & ";%PATH%")
                    If Include IsNot Nothing Then _
                        EnvironmentVariables.Add("INCLUDE=" & Include & ";" & Path.Combine(SdkPath, "Include") & ";%INCLUDE%")
                    If [Lib] IsNot Nothing Then _
                        EnvironmentVariables.Add("LIB=" & [Lib] & ";" & Path.Combine(SdkPath, "Lib") & ";%LIB%")
                    CompilerMapping.Add(".cpp;.cxx;.cc", ClPath, "cl /O2 /TP /Fefoo.exe foo.cpp", Join(EnvironmentVariables.ToArray(), "|"), 15000 * 10000, Nothing, Nothing, "foo.cpp", "foo.exe", String.Empty, String.Empty)
                    ApplyCompilerButton.Enabled = True
                    RefreshPage()
                End If
            End If
        End Sub

        Private Sub MscsMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MscsMenu.Click
            Dim Csc As String = DetectNetfx("csc.exe")
            If Csc IsNot Nothing Then
                CompilerMapping.Add(".cs", Csc, "csc /debug- /optimize+ /checked- /unsafe+ /out:foo.exe foo.cs", "PATH=" & Path.GetDirectoryName(Csc) & ";%PATH%", 15000 * 10000, Nothing, Nothing, "foo.cs", "foo.exe", String.Empty, String.Empty)
                ApplyCompilerButton.Enabled = True
                RefreshPage()
            End If
        End Sub

        Private Sub MsvbMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MsvbMenu.Click
            Dim Vbc As String = DetectNetfx("vbc.exe")
            If Vbc IsNot Nothing Then
                CompilerMapping.Add(".vb", Vbc, "vbc /debug- /optimize+ /removeintchecks+ /out:foo.exe foo.vb", "PATH=" & Path.GetDirectoryName(Vbc) & ";%PATH%", 15000 * 10000, Nothing, Nothing, "foo.vb", "foo.exe", String.Empty, String.Empty)
                ApplyCompilerButton.Enabled = True
                RefreshPage()
            End If
        End Sub

        Private Sub JavaMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles JavaMenu.Click
            Dim Javac As String = DetectJdk("javac.exe")
            If Javac IsNot Nothing Then
                Dim Java As String = DetectJdk("java.exe")
                If Java IsNot Nothing Then
                    Dim JavacPath As String = Path.GetDirectoryName(Javac)
                    Dim JavaPath = Path.GetDirectoryName(Java)
                    If JavacPath <> JavaPath Then _
                        JavacPath &= ";" & JavaPath
                    CompilerMapping.Add(".java", Javac, "javac -g:none Main.java", "PATH=" & JavacPath & ";%PATH%", 15000 * 10000, Nothing, Nothing, "Main.java", "Main.class", Java, "java Main")
                    ApplyCompilerButton.Enabled = True
                    RefreshPage()
                End If
            End If
        End Sub

        Private Sub PythonMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PythonMenu.Click
            Dim Python As String = DetectPython()
            If Python IsNot Nothing Then
                CompilerMapping.Add(".py", String.Empty, String.Empty, "PATH=" & Path.GetDirectoryName(Python) & ";%PATH%", 15000 * 10000, Nothing, Nothing, "foo.py", "foo.py", Python, "python -O foo.py")
                ApplyCompilerButton.Enabled = True
                RefreshPage()
            End If
        End Sub
#End Region

#Region "Test suite page"
        Private Sub TestSuiteList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TestSuiteList.SelectedIndexChanged
            With TestSuiteList.SelectedItems
                If .Count <> 0 Then
                    TestSuiteProperty.SelectedObject = TestSuiteMapping.GetConfig(.Item(0).Tag)
                    RemoveTestSuiteButton.Enabled = True
                    With .Item(0)
                        MoveUpTestSuiteButton.Enabled = .Index <> 0
                        MoveDownTestSuiteButton.Enabled = .Index <> TestSuiteList.Items.Count - 1
                    End With
                Else
                    TestSuiteProperty.SelectedObject = Nothing
                    RemoveTestSuiteButton.Enabled = False
                    MoveUpTestSuiteButton.Enabled = False
                    MoveDownTestSuiteButton.Enabled = False
                End If
            End With
        End Sub

        Private Sub AddTestSuiteButton_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddTestSuiteButton.ButtonClick, NewTestSuiteMenu.Click
            TestSuiteMapping.Add("*", "APlusB", String.Empty)
            ApplyTestSuiteButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub AddAPlusBMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAPlusBMenu.Click
            TestSuiteMapping.Add("A+B", "APlusB", String.Empty)
            ApplyTestSuiteButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub RemoveTestSuiteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveTestSuiteButton.Click
            TestSuiteMapping.Remove(TestSuiteList.SelectedItems.Item(0).Tag)
            ApplyTestSuiteButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub MoveUpTestSuiteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveUpTestSuiteButton.Click
            With TestSuiteList.SelectedItems.Item(0)
                Dim Id As Int32 = .Tag
                With TestSuiteList.Items(.Index - 1)
                    TestSuiteMapping.Swap(Id, .Tag)
                    .Selected = True
                End With
            End With
            ApplyTestSuiteButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub MoveDownTestSuiteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveDownTestSuiteButton.Click
            With TestSuiteList.SelectedItems.Item(0)
                Dim Id As Int32 = .Tag
                With TestSuiteList.Items(.Index + 1)
                    TestSuiteMapping.Swap(Id, .Tag)
                    .Selected = True
                End With
            End With
            ApplyTestSuiteButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub TestSuiteProperty_PropertyValueChanged(ByVal s As Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles TestSuiteProperty.PropertyValueChanged
            ApplyTestSuiteButton.Enabled = True
            RefreshPage()
        End Sub
#End Region

#Region "Executor page"
        Private Sub ExecutorSlotsText_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExecutorSlotsText.TextChanged
            ApplyExecutorButton.Enabled = True
        End Sub

        Private Sub ApplyCompilerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyCompilerButton.Click
            m_Daemon.ReloadCompiler()
            ApplyCompilerButton.Enabled = False
        End Sub

        Private Sub ApplyTestSuiteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyTestSuiteButton.Click
            m_Daemon.ReloadTestSuite()
            ApplyTestSuiteButton.Enabled = False
        End Sub

        Private Sub ApplyExecutorButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyExecutorButton.Click
            Dim ExecutorSlots As Int32
            Try
                ExecutorSlots = Int32.Parse(ExecutorSlotsText.Text)
                If ExecutorSlots < 1 OrElse ExecutorSlots > 16 Then
                    Throw New ArgumentOutOfRangeException("ExecutorSlots")
                End If
            Catch ex As Exception
                MessageBox.Show("并发数必须为 1-16 之间的整数。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                RefreshPage()
                ApplyExecutorButton.Enabled = False
                Return
            End Try

            Config.ExecutorSlots = ExecutorSlots
            m_Daemon.ReloadExecutor()
            ApplyExecutorButton.Enabled = False
        End Sub

        Private Sub ExecutorSecurityCombo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExecutorSecurityCombo.SelectedIndexChanged
            Dim EnableSecurity As Boolean = ExecutorSecurityCombo.SelectedIndex = 0
            Config.EnableSecurity = EnableSecurity
            SecurityList.Enabled = EnableSecurity
        End Sub
#End Region

        Private Sub NotImplementedHandler() Handles AddSecurityButton.Click, RemoveSecurityButton.Click, CheckSecurityButton.Click
            MessageBox.Show("还没写代码 O(∩_∩)O", "友情提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub
    End Class
End Namespace
