﻿Imports VijosNT.LocalDb
Imports VijosNT.Win32

Namespace Foreground
    Friend Class ConsoleForm
        Private m_Daemon As Daemon
        Private m_Pages As Dictionary(Of String, TabPage)
        Private m_RootNode As TreeNode
        Private m_ServiceManager As ServiceManager
        Private m_Service As Service

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

        Private Sub DisplayPage(ByVal Name As String)
            With TabControl.TabPages
                If .Count <> 0 Then
                    LeavePage(.Item(0).Name)
                End If
                .Clear()
                EnterPage(Name)
                .Add(m_Pages(Name))
            End With
        End Sub

        Private Sub ExitMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitMenu.Click
            Close()
        End Sub

        Private Sub AboutMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutMenu.Click
            MessageBox.Show("Hello world!", "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub NavigationTree_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles NavigationTree.AfterSelect
            DisplayPage(e.Node.Name & "Page")
        End Sub

        Private Sub LeavePage(ByVal Name As String)
            Select Case Name
                Case "RootPage"
                    ServiceTimer.Enabled = False
                    If m_Service IsNot Nothing Then
                        m_Service.Close()
                        m_Service = Nothing
                    End If
            End Select
            StatusLabel.Text = Nothing
        End Sub

        Private Sub EnterPage(ByVal Name As String)
            Select Case Name
                Case "RootPage"
                    m_ServiceManager = New ServiceManager()
                    m_Service = m_ServiceManager.Open(My.Resources.ServiceName)
                    ServiceTimer.Enabled = True
            End Select
            RefreshPage(Name)
        End Sub

        Private Sub RefreshPage(ByVal Name As String)
            Select Case Name
                Case "RootPage"
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
                Case "CompilerPage"
                    With CompilerList
                        Dim SelectedId As Int32 = -1
                        With .SelectedItems
                            If .Count <> 0 Then
                                SelectedId = .Item(0).Tag
                            End If
                        End With
                        With .Items
                            .Clear()
                            Using Reader As IDataReader = CompilerMapping.GetHeaders()
                                While Reader.Read()
                                    Dim Id As Int32 = Reader("Id")
                                    With .Add(Reader("Pattern"))
                                        .SubItems.Add(Reader("CommandLine"))
                                        .Tag = Id
                                        If Id = SelectedId Then
                                            .Selected = True
                                        End If
                                    End With
                                End While
                            End Using
                        End With
                        If .Items.Count <> 0 AndAlso .SelectedItems.Count = 0 Then
                            .Items(0).Selected = True
                        End If
                        CompilerList_SelectedIndexChanged(Nothing, Nothing)
                    End With
                Case "TestSuitePage"
                    With TestSuiteList
                        Dim SelectedId As Int32 = -1
                        With .SelectedItems
                            If .Count <> 0 Then
                                SelectedId = .Item(0).Tag
                            End If
                        End With
                        With .Items
                            .Clear()
                            Using Reader As IDataReader = TestSuiteMapping.GetAll()
                                While Reader.Read()
                                    Dim Id As Int32 = Reader("Id")
                                    With .Add(Reader("Pattern"))
                                        With .SubItems
                                            .Add(Reader("ClassName"))
                                            .Add(Reader("Parameter"))
                                        End With
                                        .Tag = Id
                                        If Id = SelectedId Then
                                            .Selected = True
                                        End If
                                    End With
                                End While
                            End Using
                        End With
                        If .Items.Count <> 0 AndAlso .SelectedItems.Count = 0 Then
                            .Items(0).Selected = True
                        End If
                        TestSuiteList_SelectedIndexChanged(Nothing, Nothing)
                    End With
                Case "ExecutorPage"
                    ExecutorSlotsText.Text = Config.ExecutorSlots
            End Select
        End Sub

        Private Sub InstallButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InstallButton.Click
            m_Service = m_ServiceManager.Create(My.Resources.ServiceName, My.Resources.DisplayName, Application.ExecutablePath)
            m_Service.Start()
            m_Daemon.ServiceInstalled = True
            RefreshPage("RootPage")
        End Sub

        Private Sub UninstallButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UninstallButton.Click
            m_Service.Stop()
            m_Service.Delete()
            m_Service.Close()
            m_Service = Nothing
            RefreshPage("RootPage")
        End Sub

        Private Sub ServiceTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ServiceTimer.Tick
            RefreshPage("RootPage")
        End Sub

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

        Private Sub AddCompilerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddCompilerButton.Click
            CompilerMapping.Add("*", String.Empty, String.Empty, 15000 * 10000, Nothing, Nothing, String.Empty, String.Empty, String.Empty, String.Empty)
            RefreshPage("CompilerPage")
        End Sub

        Private Sub CompilerProperty_PropertyValueChanged(ByVal s As Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles CompilerProperty.PropertyValueChanged
            RefreshPage("CompilerPage")
        End Sub

        Private Sub RemoveCompilerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveCompilerButton.Click
            CompilerMapping.Remove(CompilerList.SelectedItems.Item(0).Tag)
            RefreshPage("CompilerPage")
        End Sub

        Private Sub MoveUpCompilerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveUpCompilerButton.Click
            With CompilerList.SelectedItems.Item(0)
                Dim Id As Int32 = .Tag
                With CompilerList.Items(.Index - 1)
                    CompilerMapping.Swap(Id, .Tag)
                    .Selected = True
                End With
            End With
            RefreshPage("CompilerPage")
        End Sub

        Private Sub MoveDownCompilerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveDownCompilerButton.Click
            With CompilerList.SelectedItems.Item(0)
                Dim Id As Int32 = .Tag
                With CompilerList.Items(.Index + 1)
                    CompilerMapping.Swap(Id, .Tag)
                    .Selected = True
                End With
            End With
            RefreshPage("CompilerPage")
        End Sub

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

        Private Sub AddTestSuiteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddTestSuiteButton.Click
            TestSuiteMapping.Add("*", "APlusB", String.Empty)
            RefreshPage("TestSuitePage")
        End Sub

        Private Sub RemoveTestSuiteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveTestSuiteButton.Click
            TestSuiteMapping.Remove(TestSuiteList.SelectedItems.Item(0).Tag)
            RefreshPage("TestSuitePage")
        End Sub

        Private Sub MoveUpTestSuiteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveUpTestSuiteButton.Click
            With TestSuiteList.SelectedItems.Item(0)
                Dim Id As Int32 = .Tag
                With TestSuiteList.Items(.Index - 1)
                    TestSuiteMapping.Swap(Id, .Tag)
                    .Selected = True
                End With
            End With
            RefreshPage("TestSuitePage")
        End Sub

        Private Sub MoveDownTestSuiteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveDownTestSuiteButton.Click
            With TestSuiteList.SelectedItems.Item(0)
                Dim Id As Int32 = .Tag
                With TestSuiteList.Items(.Index + 1)
                    TestSuiteMapping.Swap(Id, .Tag)
                    .Selected = True
                End With
            End With
            RefreshPage("TestSuitePage")
        End Sub

        Private Sub TestSuiteProperty_PropertyValueChanged(ByVal s As Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles TestSuiteProperty.PropertyValueChanged
            RefreshPage("TestSuitePage")
        End Sub

        Private Sub ExecutorSlotsText_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExecutorSlotsText.Validated
            Config.ExecutorSlots = Int32.Parse(ExecutorSlotsText.Text)
        End Sub

        Private Sub ExecutorSlotsText_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ExecutorSlotsText.Validating
            Try
                Dim ExecutorSlots As Int32 = Int32.Parse(ExecutorSlotsText.Text)
                If ExecutorSlots < 1 OrElse ExecutorSlots > 16 Then
                    Throw New ArgumentOutOfRangeException("ExecutorSlots")
                End If
                ErrorProvider.Clear()
            Catch ex As Exception
                ErrorProvider.SetError(ExecutorSlotsText, "必须为 1-16 之间的整数")
                e.Cancel = True
            End Try
        End Sub

        Private Sub EnableSecurityCheck_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles EnableSecurityCheck.CheckedChanged
            If EnableSecurityCheck.Checked Then
                EnableSecurityCheck.Checked = False
                MessageBox.Show("点了也没用, 哈哈~~", "zzz..", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub

        Private Sub HomePageMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HomePageMenu.Click
            Process.Start("http://code.google.com/p/vijosnt-mini/")
        End Sub
    End Class
End Namespace
