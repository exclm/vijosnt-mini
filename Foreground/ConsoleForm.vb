﻿Imports VijosNT.LocalDb
Imports VijosNT.Utility
Imports VijosNT.Win32

Namespace Foreground
    Friend Class ConsoleForm
        Private m_Daemon As Daemon
        Private m_Pages As Dictionary(Of String, TabPage)
        Private m_RootNode As TreeNode
        Private m_Service As Service
        Private m_UnminimizedWindowState As FormWindowState

#Region "Members"
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
            FloatingFormButton.Checked = m_Daemon.ShowFloating

            SetDoubleBuffered(LocalSourceList)
        End Sub

        Public Sub Unminimize()
            WindowState = m_UnminimizedWindowState
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
                Case "DataSourcePage"
                    ApplyDataSourceButton.PerformClick()
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
                    RefreshLocalRecord()
                Case "CompilerPage"
                    Using Reader As IDataReader = CompilerMapping.GetHeaders()
                        RefreshListView(CompilerList, Reader, "Id", "Pattern", "CommandLine")
                    End Using
                    CompilerList_SelectedIndexChanged(Nothing, Nothing)
                Case "TestSuitePage"
                    Using Reader As IDataReader = TestSuiteMapping.GetAll()
                        RefreshListView(TestSuiteList, Reader, "Id", "Pattern", "NamespacePattern", "ClassName", "Parameter")
                    End Using
                    TestSuiteList_SelectedIndexChanged(Nothing, Nothing)
                Case "ExecutorPage"
                    RemoveHandler ExecutorSlotsText.TextChanged, AddressOf ExecutorSlotsText_TextChanged
                    ExecutorSlotsText.Text = Config.ExecutorSlots
                    AddHandler ExecutorSlotsText.TextChanged, AddressOf ExecutorSlotsText_TextChanged
                    RemoveHandler ExecutorSecurityCombo.SelectedIndexChanged, AddressOf ExecutorSecurityCombo_SelectedIndexChanged
                    If Config.EnableSecurity Then
                        ExecutorSecurityCombo.SelectedIndex = 0
                        SecurityList.Enabled = True
                    Else
                        ExecutorSecurityCombo.SelectedIndex = 1
                        SecurityList.Enabled = False
                    End If
                    AddHandler ExecutorSecurityCombo.SelectedIndexChanged, AddressOf ExecutorSecurityCombo_SelectedIndexChanged
                    RefreshSecurity()
                Case "DataSourcePage"
                    Using Reader As IDataReader = DataSourceMapping.GetHeaders()
                        RefreshListView(DataSourceList, Reader, "Id", "ClassName", "Namespace", "Parameter")
                    End Using
                    DataSourceList_SelectedIndexChanged(Nothing, Nothing)
            End Select
        End Sub
#End Region

#Region "Global events"
        Private Sub VacuumMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VacuumMenu.Click
            Database.Vacuum()
            MessageBox.Show("数据库压缩完毕。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub ExitMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitMenu.Click
            Close()
        End Sub

        Private Sub AboutMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutMenu.Click
            MessageBox.Show("VijosNT Mini " & Assembly.GetExecutingAssembly().GetName().Version.ToString() & vbCrLf & "written by iceboy, twd2, bml" & vbCrLf & "http://vijosnt-mini.googlecode.com", "关于", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

        Private Sub ConsoleForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
            Dim CurrentWindowState As FormWindowState = WindowState
            If CurrentWindowState <> FormWindowState.Minimized Then _
                m_UnminimizedWindowState = CurrentWindowState
        End Sub
#End Region

#Region "Root page"
        Private Sub RefreshServiceStatus()
            If m_Service Is Nothing Then
                StartButton.Enabled = True
                StopButton.Enabled = False
                StatusLabel.Text = "VijosNT 服务未安装"
            Else
                StartButton.Enabled = False
                StopButton.Enabled = True
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

        Public Sub RefreshLocalRecord()
            Using Reader As IDataReader = Record.GetHeaders()
                RefreshListView(LocalSourceList, Reader, "Id", "Id", "$Flag", "FileName", "%Score", "!TimeUsage", "@MemoryUsage", "#Date")
            End Using
        End Sub

        Private Sub StartButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartButton.Click
            m_Service = m_Daemon.CreateService()
            Try
                m_Service.Start()
            Catch ex As Win32Exception
                MessageBox.Show("服务启动失败 (通常是因为设置错误), 详情请参见事件查看器。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                m_Service.Delete()
                m_Service.Close()
                m_Service = Nothing
                Return
            End Try
            m_Daemon.ServiceInstalled = True
            RefreshServiceStatus()
        End Sub

        Private Sub StopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StopButton.Click
            Try
                m_Service.Stop()
                m_Service.Delete()
            Catch ex As Win32Exception
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
            m_Service.Close()
            m_Service = Nothing
            RefreshServiceStatus()
        End Sub

        Private Sub ServiceTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ServiceTimer.Tick
            RefreshServiceStatus()
        End Sub

        Private Sub FloatingFormButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FloatingFormButton.Click
            Dim Value As Boolean = Not m_Daemon.ShowFloating
            m_Daemon.ShowFloating = Value
            FloatingFormButton.Checked = Value
        End Sub

        Private Sub RefreshLocalButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefershLocalButton.Click
            RefreshLocalRecord()
        End Sub

        Private Sub ClearLocalButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearLocalButton.Click
            If MessageBox.Show("确定要清除所有记录吗?", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Record.Clear()
                RefreshLocalRecord()
            End If
        End Sub

        Private Sub GenerateReport(ByVal sender As Object, ByVal e As System.EventArgs) Handles LocalSourceList.DoubleClick, GenerateReportMenu.Click
            With LocalSourceList.SelectedItems
                If .Count = 0 Then _
                    Return

                Using Reader As New StringReader(LoadTestResultXml(.Item(0).Tag())), _
                    XmlReader As New XmlTextReader(Reader)
                    Dim Dialog As New TestResultForm(XmlReader)
                    Dialog.Show()
                End Using
            End With
        End Sub

        Private Sub DeleteRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteRecord.Click
            With LocalSourceList.SelectedItems
                If .Count = 0 Then _
                    Return
                Record.Remove(.Item(0).Tag())
                RefreshLocalRecord()
            End With
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
            CompilerMapping.Add(".*", String.Empty, String.Empty, String.Empty, 15000 * 10000, Nothing, Nothing, String.Empty, String.Empty, String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing)
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

        Private Sub ApplyCompilerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyCompilerButton.Click
            m_Daemon.ReloadCompiler()
            ApplyCompilerButton.Enabled = False
        End Sub

        Private Sub GccMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GccMenu.Click
            Dim Gcc As String = DetectMingw("gcc.exe")
            If Gcc IsNot Nothing Then
                CompilerMapping.Add(".c", Gcc, "gcc -O2 -s -Wall -ofoo.exe foo.c -lm", "PATH=" & Path.GetDirectoryName(Gcc) & ";%PATH%", 15000 * 10000, Nothing, Nothing, "foo.c", "foo.exe", String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing)
                ApplyCompilerButton.Enabled = True
                RefreshPage()
            End If
        End Sub

        Private Sub GppMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GppMenu.Click
            Dim Gpp As String = DetectMingw("g++.exe")
            If Gpp IsNot Nothing Then
                CompilerMapping.Add(".cpp;.cxx;.cc", Gpp, "g++ -O2 -s -Wall -ofoo.exe foo.cpp -lm", "PATH=" & Path.GetDirectoryName(Gpp) & ";%PATH%", 15000 * 10000, Nothing, Nothing, "foo.cpp", "foo.exe", String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing)
                ApplyCompilerButton.Enabled = True
                RefreshPage()
            End If
        End Sub

        Private Sub FpcMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FpcMenu.Click
            Dim Fpc As String = DetectFpc()
            If Fpc IsNot Nothing Then
                CompilerMapping.Add(".pas", Fpc, "fpc -O2 -Xs -Sgic -vw -ofoo.exe foo.pas", "PATH=" & Path.GetDirectoryName(Fpc) & ";%PATH%", 15000 * 10000, Nothing, Nothing, "foo.pas", "foo.exe", String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing)
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
                    CompilerMapping.Add(".c", ClPath, "cl /nologo /O2 /TC /Fefoo.exe foo.c", Join(EnvironmentVariables.ToArray(), "|"), 15000 * 10000, Nothing, Nothing, "foo.c", "foo.exe", String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing)
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
                    CompilerMapping.Add(".cpp;.cxx;.cc", ClPath, "cl /nologo /O2 /TP /EHsc /Fefoo.exe foo.cpp", Join(EnvironmentVariables.ToArray(), "|"), 15000 * 10000, Nothing, Nothing, "foo.cpp", "foo.exe", String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing)
                    ApplyCompilerButton.Enabled = True
                    RefreshPage()
                End If
            End If
        End Sub

        Private Sub MscsMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MscsMenu.Click
            Dim Csc As String = DetectNetfx("csc.exe")
            If Csc IsNot Nothing Then
                CompilerMapping.Add(".cs", Csc, "csc /nologo /debug- /optimize+ /checked- /unsafe+ /out:foo.exe foo.cs", "PATH=" & Path.GetDirectoryName(Csc) & ";%PATH%", 15000 * 10000, Nothing, Nothing, "foo.cs", "foo.exe", String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing)
                ApplyCompilerButton.Enabled = True
                RefreshPage()
            End If
        End Sub

        Private Sub MsvbMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MsvbMenu.Click
            Dim Vbc As String = DetectNetfx("vbc.exe")
            If Vbc IsNot Nothing Then
                CompilerMapping.Add(".vb", Vbc, "vbc /nologo /debug- /optimize+ /removeintchecks+ /out:foo.exe foo.vb", "PATH=" & Path.GetDirectoryName(Vbc) & ";%PATH%", 15000 * 10000, Nothing, Nothing, "foo.vb", "foo.exe", String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing)
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
                    CompilerMapping.Add(".java", Javac, "javac -g:none Main.java", "PATH=" & JavacPath & ";%PATH%", 15000 * 10000, Nothing, Nothing, "Main.java", "Main.class", Java, "java Main", Nothing, Nothing, Nothing, Nothing)
                    ApplyCompilerButton.Enabled = True
                    RefreshPage()
                End If
            End If
        End Sub

        Private Sub PythonMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PythonMenu.Click
            Dim Python As String = DetectPython()
            If Python IsNot Nothing Then
                CompilerMapping.Add(".py", String.Empty, String.Empty, "PATH=" & Path.GetDirectoryName(Python) & ";%PATH%", 15000 * 10000, Nothing, Nothing, "foo.py", "foo.py", Python, "python -O foo.py", Nothing, Nothing, Nothing, Nothing)
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
            TestSuiteMapping.Add("*", String.Empty, "APlusB", String.Empty)
            ApplyTestSuiteButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub AddAPlusBMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAPlusBMenu.Click
            TestSuiteMapping.Add("A+B", String.Empty, "APlusB", String.Empty)
            ApplyTestSuiteButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub AddVijosSuiteMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddVijosSuiteMenu.Click
            Using Dialog As New FolderBrowserDialog
                Dialog.Description = "请选择包含 Vijos 格式数据集的目录"
                If Dialog.ShowDialog() = DialogResult.OK Then
                    TestSuiteMapping.Add("*", String.Empty, "Vijos", "Root=" & Dialog.SelectedPath & ";MemoryQuota=134217728")
                    ApplyTestSuiteButton.Enabled = True
                    RefreshPage()
                End If
            End Using
        End Sub

        Private Sub AddFreeSuiteMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddFreeSuiteMenu.Click
            Using Dialog As New FolderBrowserDialog
                Dialog.Description = "请选择包含 Free 格式数据集的目录"
                If Dialog.ShowDialog() = DialogResult.OK Then
                    TestSuiteMapping.Add("*", String.Empty, "Free", "Root=" & Dialog.SelectedPath & ";TimeQuota=10000000;MemoryQuota=134217728")
                    ApplyTestSuiteButton.Enabled = True
                    RefreshPage()
                End If
            End Using
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

        Private Sub ApplyTestSuiteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyTestSuiteButton.Click
            m_Daemon.ReloadTestSuite()
            ApplyTestSuiteButton.Enabled = False
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

        Private Sub RefreshSecurity()
            Using Reader As IDataReader = UntrustedEnvironments.GetAll()
                RefreshListView(SecurityList, Reader, "Id", "UserName", "Password")
            End Using
            SecurityList_SelectedIndexChanged(Nothing, Nothing)
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

            If ExecutorSecurityCombo.SelectedIndex = 0 Then
                If SecurityList.Items.Count <> 0 Then
                    Config.EnableSecurity = True
                Else
                    MessageBox.Show("安全已被禁用, 因为没有提供任何供不可信程序执行的环境。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Config.EnableSecurity = False
                End If
            Else
                Config.EnableSecurity = False
            End If
            Config.ExecutorSlots = ExecutorSlots

            m_Daemon.ReloadExecutor()
            ApplyExecutorButton.Enabled = False
            RefreshPage()
        End Sub

        Private Sub ExecutorSecurityCombo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExecutorSecurityCombo.SelectedIndexChanged
            Dim EnableSecurity As Boolean = (ExecutorSecurityCombo.SelectedIndex = 0)
            SecurityList.Enabled = EnableSecurity
            AddSecurityButton.Enabled = EnableSecurity
            ApplyExecutorButton.Enabled = True
        End Sub

        Private Sub AddSecurityButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddSecurityButton.Click
            Dim UserName As String = Nothing
            Dim Password As String = Nothing

            If PromptForCredentials(Handle, "请输入一组 Windows 用户名和密码用于不可信程序的执行", "VijosNT", UserName, Password) <> DialogResult.OK Then _
                Return

            Dim LoginSucceeded As Boolean
            Try
                Using Token As New Token(UserName, Password)
                    ' Do nothing
                End Using
                LoginSucceeded = True
            Catch ex As Win32Exception
                LoginSucceeded = False
            End Try

            If Not LoginSucceeded AndAlso _
                MessageBox.Show("登录失败, 使用错误的用户名和密码会使服务无法正常运行, 确定要继续吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then _
                Return

            UntrustedEnvironments.Add(UserName, Password)
            RefreshSecurity()
            ApplyExecutorButton.Enabled = True
        End Sub

        Private Sub RemoveSecurityButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RemoveSecurityButton.Click
            With SecurityList.SelectedItems
                If .Count <> 0 Then
                    UntrustedEnvironments.Remove(.Item(0).Tag)
                    RefreshSecurity()
                    ApplyExecutorButton.Enabled = True
                End If
            End With
        End Sub

        Private Sub SecurityList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SecurityList.SelectedIndexChanged
            RemoveSecurityButton.Enabled = (SecurityList.SelectedItems.Count <> 0)
        End Sub
#End Region

#Region "Data source page"
        Private Sub DataSourceList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataSourceList.SelectedIndexChanged
            With DataSourceList.SelectedItems
                If .Count <> 0 Then
                    DataSourceProperty.SelectedObject = DataSourceMapping.GetConfig(.Item(0).Tag)
                    RemoveDataSourceButton.Enabled = True
                Else
                    DataSourceProperty.SelectedObject = Nothing
                    RemoveDataSourceButton.Enabled = False
                End If
            End With
        End Sub

        Private Sub AddDataSourceButton_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddDataSourceButton.ButtonClick, NewDataSourceMenu.Click
            DataSourceMapping.Add("Vijos", String.Empty, String.Empty, String.Empty)
            ApplyDataSourceButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub RemoveDataSourceButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveDataSourceButton.Click
            DataSourceMapping.Remove(DataSourceList.SelectedItems.Item(0).Tag)
            ApplyDataSourceButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub ApplyDataSourceButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyDataSourceButton.Click
            m_Daemon.ReloadDataSource()
            ApplyDataSourceButton.Enabled = False
        End Sub

        Private Sub DataSourceProperty_PropertyValueChanged(ByVal s As Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles DataSourceProperty.PropertyValueChanged
            ApplyDataSourceButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub VijosDataSourceMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VijosDataSourceMenu.Click
            DataSourceMapping.Add("Vijos", String.Empty, "Server=(local);Database=vijos;UserName=sa;Password=admin", String.Empty)
            ApplyDataSourceButton.Enabled = True
            RefreshPage()
        End Sub

        Private Sub VijosLocalDataSourceMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VijosLocalDataSourceMenu.Click
            Using Dialog As New OpenFileDialog
                Dialog.Title = "请选择 Vijos 设置文件 (Config.xml)"
                Dialog.Filter = "Config.xml|Config.xml"
                If Dialog.ShowDialog() = DialogResult.OK Then
                    DataSourceMapping.Add("Vijos", String.Empty, "Config=" & Dialog.FileName, String.Empty)
                    ApplyDataSourceButton.Enabled = True
                    RefreshPage()
                End If
            End Using
        End Sub

        Private Sub _22OJSDataSourceMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _22OJSDataSourceMenu.Click
            DataSourceMapping.Add("_22OJS", String.Empty, "Server=(local);Database=22OJS;UserName=sa;Password=admin", String.Empty)
            ApplyDataSourceButton.Enabled = True
            RefreshPage()
        End Sub
#End Region
    End Class
End Namespace
