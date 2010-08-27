﻿Namespace Foreground
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ConsoleForm
        Inherits System.Windows.Forms.Form

        'Form 重写 Dispose，以清理组件列表。
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Windows 窗体设计器所必需的
        Private components As System.ComponentModel.IContainer

        '注意: 以下过程是 Windows 窗体设计器所必需的
        '可以使用 Windows 窗体设计器修改它。
        '不要使用代码编辑器修改它。
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("编译器映射")
            Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("数据集映射")
            Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("执行设置")
            Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("本地数据源")
            Dim TreeNode5 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("外部数据源")
            Dim TreeNode6 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("VijosNT", New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2, TreeNode3, TreeNode4, TreeNode5})
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConsoleForm))
            Me.MenuStrip = New System.Windows.Forms.MenuStrip()
            Me.FileMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.ExitMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.HelpMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.WikiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.HelpStrip0 = New System.Windows.Forms.ToolStripSeparator()
            Me.HomePageMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.CheckUpdateMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.ReportIssue = New System.Windows.Forms.ToolStripMenuItem()
            Me.HelpStrip1 = New System.Windows.Forms.ToolStripSeparator()
            Me.AboutMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.StatusStrip = New System.Windows.Forms.StatusStrip()
            Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
            Me.SplitContainer = New System.Windows.Forms.SplitContainer()
            Me.NavigationTree = New System.Windows.Forms.TreeView()
            Me.TabControl = New System.Windows.Forms.TabControl()
            Me.RootPage = New System.Windows.Forms.TabPage()
            Me.ServiceToolStrip = New System.Windows.Forms.ToolStrip()
            Me.InstallButton = New System.Windows.Forms.ToolStripButton()
            Me.UninstallButton = New System.Windows.Forms.ToolStripButton()
            Me.CompilerPage = New System.Windows.Forms.TabPage()
            Me.CompilerSplit = New System.Windows.Forms.SplitContainer()
            Me.CompilerList = New System.Windows.Forms.ListView()
            Me.CompilerPatternHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.CompilerCommandLineHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.CompilerProperty = New System.Windows.Forms.PropertyGrid()
            Me.CompilerToolStrip = New System.Windows.Forms.ToolStrip()
            Me.AddCompilerButton = New System.Windows.Forms.ToolStripSplitButton()
            Me.NewCompilerMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddCompilerBar0 = New System.Windows.Forms.ToolStripSeparator()
            Me.MingwMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.GccMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.GppMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.MsvsMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.MscMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.MscppMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.MscsMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.MsvbMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.JavaMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.RemoveCompilerButton = New System.Windows.Forms.ToolStripButton()
            Me.CompilerSeperator0 = New System.Windows.Forms.ToolStripSeparator()
            Me.MoveUpCompilerButton = New System.Windows.Forms.ToolStripButton()
            Me.MoveDownCompilerButton = New System.Windows.Forms.ToolStripButton()
            Me.CompilerSeperator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.ApplyCompilerButton = New System.Windows.Forms.ToolStripButton()
            Me.TestSuitePage = New System.Windows.Forms.TabPage()
            Me.TestSuiteSplit = New System.Windows.Forms.SplitContainer()
            Me.TestSuiteList = New System.Windows.Forms.ListView()
            Me.TestSuitePatternHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.TestSuiteClassHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.TestSuiteParameterHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.TestSuiteProperty = New System.Windows.Forms.PropertyGrid()
            Me.TestSuiteToolStrip = New System.Windows.Forms.ToolStrip()
            Me.AddTestSuiteButton = New System.Windows.Forms.ToolStripSplitButton()
            Me.NewTestSuiteMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddTestSuiteBar0 = New System.Windows.Forms.ToolStripSeparator()
            Me.AddAPlusBMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.RemoveTestSuiteButton = New System.Windows.Forms.ToolStripButton()
            Me.TestSuiteSeperator0 = New System.Windows.Forms.ToolStripSeparator()
            Me.MoveUpTestSuiteButton = New System.Windows.Forms.ToolStripButton()
            Me.MoveDownTestSuiteButton = New System.Windows.Forms.ToolStripButton()
            Me.TestSuiteSeperator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.ApplyTestSuiteButton = New System.Windows.Forms.ToolStripButton()
            Me.ExecutorPage = New System.Windows.Forms.TabPage()
            Me.SecurityList = New System.Windows.Forms.ListView()
            Me.SecurityDesktopNameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.SecurityUserNameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.SecurityPasswordHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.ExecutorToolStrip = New System.Windows.Forms.ToolStrip()
            Me.ExecutorSlotsLabel = New System.Windows.Forms.ToolStripLabel()
            Me.ExecutorSlotsText = New System.Windows.Forms.ToolStripTextBox()
            Me.ExecutorSeperator0 = New System.Windows.Forms.ToolStripSeparator()
            Me.ExecutorSecurityLabel = New System.Windows.Forms.ToolStripLabel()
            Me.ExecutorSecurityCombo = New System.Windows.Forms.ToolStripComboBox()
            Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.AddSecurityButton = New System.Windows.Forms.ToolStripButton()
            Me.RemoveSecurityButton = New System.Windows.Forms.ToolStripButton()
            Me.CheckSecurityButton = New System.Windows.Forms.ToolStripButton()
            Me.ExecutorSeperator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.ApplyExecutorButton = New System.Windows.Forms.ToolStripButton()
            Me.LocalDataSourcePage = New System.Windows.Forms.TabPage()
            Me.LocalSourceList = New System.Windows.Forms.ListView()
            Me.LocalSourceIdHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.LocalSourceFlagHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.LocalSourceFileNameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.LocalSourceScoreHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.LocalSourceTimeUsageHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.LocalSourceMemoryUsageHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.LocalSourceDateHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.LocalSourceToolStrip = New System.Windows.Forms.ToolStrip()
            Me.RefershLocalButton = New System.Windows.Forms.ToolStripButton()
            Me.RemoteDataSourcePage = New System.Windows.Forms.TabPage()
            Me.ServiceTimer = New System.Windows.Forms.Timer(Me.components)
            Me.PythonMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.MenuStrip.SuspendLayout()
            Me.StatusStrip.SuspendLayout()
            Me.SplitContainer.Panel1.SuspendLayout()
            Me.SplitContainer.Panel2.SuspendLayout()
            Me.SplitContainer.SuspendLayout()
            Me.TabControl.SuspendLayout()
            Me.RootPage.SuspendLayout()
            Me.ServiceToolStrip.SuspendLayout()
            Me.CompilerPage.SuspendLayout()
            Me.CompilerSplit.Panel1.SuspendLayout()
            Me.CompilerSplit.Panel2.SuspendLayout()
            Me.CompilerSplit.SuspendLayout()
            Me.CompilerToolStrip.SuspendLayout()
            Me.TestSuitePage.SuspendLayout()
            Me.TestSuiteSplit.Panel1.SuspendLayout()
            Me.TestSuiteSplit.Panel2.SuspendLayout()
            Me.TestSuiteSplit.SuspendLayout()
            Me.TestSuiteToolStrip.SuspendLayout()
            Me.ExecutorPage.SuspendLayout()
            Me.ExecutorToolStrip.SuspendLayout()
            Me.LocalDataSourcePage.SuspendLayout()
            Me.LocalSourceToolStrip.SuspendLayout()
            Me.SuspendLayout()
            '
            'MenuStrip
            '
            Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileMenu, Me.HelpMenu})
            Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
            Me.MenuStrip.Name = "MenuStrip"
            Me.MenuStrip.Padding = New System.Windows.Forms.Padding(4, 2, 0, 2)
            Me.MenuStrip.Size = New System.Drawing.Size(784, 25)
            Me.MenuStrip.TabIndex = 0
            '
            'FileMenu
            '
            Me.FileMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitMenu})
            Me.FileMenu.Name = "FileMenu"
            Me.FileMenu.Size = New System.Drawing.Size(58, 21)
            Me.FileMenu.Text = "文件(&F)"
            '
            'ExitMenu
            '
            Me.ExitMenu.Name = "ExitMenu"
            Me.ExitMenu.Size = New System.Drawing.Size(116, 22)
            Me.ExitMenu.Text = "退出(&X)"
            '
            'HelpMenu
            '
            Me.HelpMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WikiToolStripMenuItem, Me.HelpStrip0, Me.HomePageMenu, Me.CheckUpdateMenu, Me.ReportIssue, Me.HelpStrip1, Me.AboutMenu})
            Me.HelpMenu.Name = "HelpMenu"
            Me.HelpMenu.Size = New System.Drawing.Size(61, 21)
            Me.HelpMenu.Text = "帮助(&H)"
            '
            'WikiToolStripMenuItem
            '
            Me.WikiToolStripMenuItem.Name = "WikiToolStripMenuItem"
            Me.WikiToolStripMenuItem.Size = New System.Drawing.Size(141, 22)
            Me.WikiToolStripMenuItem.Text = "&Wiki"
            '
            'HelpStrip0
            '
            Me.HelpStrip0.Name = "HelpStrip0"
            Me.HelpStrip0.Size = New System.Drawing.Size(138, 6)
            '
            'HomePageMenu
            '
            Me.HomePageMenu.Name = "HomePageMenu"
            Me.HomePageMenu.Size = New System.Drawing.Size(141, 22)
            Me.HomePageMenu.Text = "主页(&H)"
            '
            'CheckUpdateMenu
            '
            Me.CheckUpdateMenu.Name = "CheckUpdateMenu"
            Me.CheckUpdateMenu.Size = New System.Drawing.Size(141, 22)
            Me.CheckUpdateMenu.Text = "检查更新(&U)"
            '
            'ReportIssue
            '
            Me.ReportIssue.Name = "ReportIssue"
            Me.ReportIssue.Size = New System.Drawing.Size(141, 22)
            Me.ReportIssue.Text = "报告问题(&R)"
            '
            'HelpStrip1
            '
            Me.HelpStrip1.Name = "HelpStrip1"
            Me.HelpStrip1.Size = New System.Drawing.Size(138, 6)
            '
            'AboutMenu
            '
            Me.AboutMenu.Name = "AboutMenu"
            Me.AboutMenu.Size = New System.Drawing.Size(141, 22)
            Me.AboutMenu.Text = "关于(&A)"
            '
            'StatusStrip
            '
            Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel})
            Me.StatusStrip.Location = New System.Drawing.Point(0, 540)
            Me.StatusStrip.Name = "StatusStrip"
            Me.StatusStrip.Padding = New System.Windows.Forms.Padding(1, 0, 10, 0)
            Me.StatusStrip.Size = New System.Drawing.Size(784, 22)
            Me.StatusStrip.TabIndex = 2
            '
            'StatusLabel
            '
            Me.StatusLabel.Name = "StatusLabel"
            Me.StatusLabel.Size = New System.Drawing.Size(0, 17)
            '
            'SplitContainer
            '
            Me.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
            Me.SplitContainer.Location = New System.Drawing.Point(0, 25)
            Me.SplitContainer.Margin = New System.Windows.Forms.Padding(2)
            Me.SplitContainer.Name = "SplitContainer"
            '
            'SplitContainer.Panel1
            '
            Me.SplitContainer.Panel1.Controls.Add(Me.NavigationTree)
            '
            'SplitContainer.Panel2
            '
            Me.SplitContainer.Panel2.Controls.Add(Me.TabControl)
            Me.SplitContainer.Size = New System.Drawing.Size(784, 515)
            Me.SplitContainer.SplitterDistance = 189
            Me.SplitContainer.SplitterWidth = 3
            Me.SplitContainer.TabIndex = 3
            '
            'NavigationTree
            '
            Me.NavigationTree.Dock = System.Windows.Forms.DockStyle.Fill
            Me.NavigationTree.Location = New System.Drawing.Point(0, 0)
            Me.NavigationTree.Margin = New System.Windows.Forms.Padding(2)
            Me.NavigationTree.Name = "NavigationTree"
            TreeNode1.Name = "Compiler"
            TreeNode1.Text = "编译器映射"
            TreeNode2.Name = "TestSuite"
            TreeNode2.Text = "数据集映射"
            TreeNode3.Name = "Executor"
            TreeNode3.Text = "执行设置"
            TreeNode4.Name = "LocalDataSource"
            TreeNode4.Text = "本地数据源"
            TreeNode5.Name = "RemoteDataSource"
            TreeNode5.Text = "外部数据源"
            TreeNode6.Name = "Root"
            TreeNode6.Text = "VijosNT"
            Me.NavigationTree.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode6})
            Me.NavigationTree.Size = New System.Drawing.Size(189, 515)
            Me.NavigationTree.TabIndex = 0
            '
            'TabControl
            '
            Me.TabControl.Controls.Add(Me.RootPage)
            Me.TabControl.Controls.Add(Me.CompilerPage)
            Me.TabControl.Controls.Add(Me.TestSuitePage)
            Me.TabControl.Controls.Add(Me.ExecutorPage)
            Me.TabControl.Controls.Add(Me.LocalDataSourcePage)
            Me.TabControl.Controls.Add(Me.RemoteDataSourcePage)
            Me.TabControl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabControl.Location = New System.Drawing.Point(0, 0)
            Me.TabControl.Margin = New System.Windows.Forms.Padding(2)
            Me.TabControl.Name = "TabControl"
            Me.TabControl.SelectedIndex = 0
            Me.TabControl.Size = New System.Drawing.Size(592, 515)
            Me.TabControl.TabIndex = 0
            '
            'RootPage
            '
            Me.RootPage.Controls.Add(Me.ServiceToolStrip)
            Me.RootPage.Location = New System.Drawing.Point(4, 22)
            Me.RootPage.Margin = New System.Windows.Forms.Padding(2)
            Me.RootPage.Name = "RootPage"
            Me.RootPage.Padding = New System.Windows.Forms.Padding(2)
            Me.RootPage.Size = New System.Drawing.Size(584, 489)
            Me.RootPage.TabIndex = 0
            Me.RootPage.Text = "服务"
            Me.RootPage.UseVisualStyleBackColor = True
            '
            'ServiceToolStrip
            '
            Me.ServiceToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InstallButton, Me.UninstallButton})
            Me.ServiceToolStrip.Location = New System.Drawing.Point(2, 2)
            Me.ServiceToolStrip.Name = "ServiceToolStrip"
            Me.ServiceToolStrip.Size = New System.Drawing.Size(580, 25)
            Me.ServiceToolStrip.TabIndex = 2
            '
            'InstallButton
            '
            Me.InstallButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.InstallButton.Image = CType(resources.GetObject("InstallButton.Image"), System.Drawing.Image)
            Me.InstallButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.InstallButton.Name = "InstallButton"
            Me.InstallButton.Size = New System.Drawing.Size(72, 22)
            Me.InstallButton.Text = "安装服务(&I)"
            '
            'UninstallButton
            '
            Me.UninstallButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.UninstallButton.Image = CType(resources.GetObject("UninstallButton.Image"), System.Drawing.Image)
            Me.UninstallButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.UninstallButton.Name = "UninstallButton"
            Me.UninstallButton.Size = New System.Drawing.Size(77, 22)
            Me.UninstallButton.Text = "卸载服务(&U)"
            '
            'CompilerPage
            '
            Me.CompilerPage.Controls.Add(Me.CompilerSplit)
            Me.CompilerPage.Controls.Add(Me.CompilerToolStrip)
            Me.CompilerPage.Location = New System.Drawing.Point(4, 22)
            Me.CompilerPage.Margin = New System.Windows.Forms.Padding(2)
            Me.CompilerPage.Name = "CompilerPage"
            Me.CompilerPage.Padding = New System.Windows.Forms.Padding(2)
            Me.CompilerPage.Size = New System.Drawing.Size(584, 489)
            Me.CompilerPage.TabIndex = 1
            Me.CompilerPage.Text = "编译器映射"
            Me.CompilerPage.UseVisualStyleBackColor = True
            '
            'CompilerSplit
            '
            Me.CompilerSplit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CompilerSplit.Location = New System.Drawing.Point(2, 27)
            Me.CompilerSplit.Margin = New System.Windows.Forms.Padding(2)
            Me.CompilerSplit.Name = "CompilerSplit"
            Me.CompilerSplit.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'CompilerSplit.Panel1
            '
            Me.CompilerSplit.Panel1.Controls.Add(Me.CompilerList)
            '
            'CompilerSplit.Panel2
            '
            Me.CompilerSplit.Panel2.Controls.Add(Me.CompilerProperty)
            Me.CompilerSplit.Size = New System.Drawing.Size(580, 460)
            Me.CompilerSplit.SplitterDistance = 121
            Me.CompilerSplit.SplitterWidth = 3
            Me.CompilerSplit.TabIndex = 8
            '
            'CompilerList
            '
            Me.CompilerList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.CompilerPatternHeader, Me.CompilerCommandLineHeader})
            Me.CompilerList.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CompilerList.FullRowSelect = True
            Me.CompilerList.HideSelection = False
            Me.CompilerList.Location = New System.Drawing.Point(0, 0)
            Me.CompilerList.Margin = New System.Windows.Forms.Padding(2)
            Me.CompilerList.MultiSelect = False
            Me.CompilerList.Name = "CompilerList"
            Me.CompilerList.Size = New System.Drawing.Size(580, 121)
            Me.CompilerList.TabIndex = 1
            Me.CompilerList.UseCompatibleStateImageBehavior = False
            Me.CompilerList.View = System.Windows.Forms.View.Details
            '
            'CompilerPatternHeader
            '
            Me.CompilerPatternHeader.Text = "扩展名匹配"
            Me.CompilerPatternHeader.Width = 119
            '
            'CompilerCommandLineHeader
            '
            Me.CompilerCommandLineHeader.Text = "命令行"
            Me.CompilerCommandLineHeader.Width = 389
            '
            'CompilerProperty
            '
            Me.CompilerProperty.BackColor = System.Drawing.SystemColors.Window
            Me.CompilerProperty.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CompilerProperty.Location = New System.Drawing.Point(0, 0)
            Me.CompilerProperty.Margin = New System.Windows.Forms.Padding(2)
            Me.CompilerProperty.Name = "CompilerProperty"
            Me.CompilerProperty.PropertySort = System.Windows.Forms.PropertySort.Categorized
            Me.CompilerProperty.Size = New System.Drawing.Size(580, 336)
            Me.CompilerProperty.TabIndex = 6
            Me.CompilerProperty.ToolbarVisible = False
            '
            'CompilerToolStrip
            '
            Me.CompilerToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddCompilerButton, Me.RemoveCompilerButton, Me.CompilerSeperator0, Me.MoveUpCompilerButton, Me.MoveDownCompilerButton, Me.CompilerSeperator1, Me.ApplyCompilerButton})
            Me.CompilerToolStrip.Location = New System.Drawing.Point(2, 2)
            Me.CompilerToolStrip.Name = "CompilerToolStrip"
            Me.CompilerToolStrip.Size = New System.Drawing.Size(580, 25)
            Me.CompilerToolStrip.TabIndex = 7
            '
            'AddCompilerButton
            '
            Me.AddCompilerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.AddCompilerButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewCompilerMenu, Me.AddCompilerBar0, Me.MingwMenu, Me.MsvsMenu, Me.JavaMenu, Me.PythonMenu})
            Me.AddCompilerButton.Image = CType(resources.GetObject("AddCompilerButton.Image"), System.Drawing.Image)
            Me.AddCompilerButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AddCompilerButton.Name = "AddCompilerButton"
            Me.AddCompilerButton.Size = New System.Drawing.Size(64, 22)
            Me.AddCompilerButton.Text = "添加(&A)"
            '
            'NewCompilerMenu
            '
            Me.NewCompilerMenu.Name = "NewCompilerMenu"
            Me.NewCompilerMenu.Size = New System.Drawing.Size(211, 22)
            Me.NewCompilerMenu.Text = "新编译器映射"
            '
            'AddCompilerBar0
            '
            Me.AddCompilerBar0.Name = "AddCompilerBar0"
            Me.AddCompilerBar0.Size = New System.Drawing.Size(208, 6)
            '
            'MingwMenu
            '
            Me.MingwMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GccMenu, Me.GppMenu})
            Me.MingwMenu.Name = "MingwMenu"
            Me.MingwMenu.Size = New System.Drawing.Size(211, 22)
            Me.MingwMenu.Text = "MinGW"
            '
            'GccMenu
            '
            Me.GccMenu.Name = "GccMenu"
            Me.GccMenu.Size = New System.Drawing.Size(152, 22)
            Me.GccMenu.Text = "C 编译器"
            '
            'GppMenu
            '
            Me.GppMenu.Name = "GppMenu"
            Me.GppMenu.Size = New System.Drawing.Size(152, 22)
            Me.GppMenu.Text = "C++ 编译器"
            '
            'MsvsMenu
            '
            Me.MsvsMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MscMenu, Me.MscppMenu, Me.MscsMenu, Me.MsvbMenu})
            Me.MsvsMenu.Name = "MsvsMenu"
            Me.MsvsMenu.Size = New System.Drawing.Size(211, 22)
            Me.MsvsMenu.Text = "Microsoft Visual Studio"
            '
            'MscMenu
            '
            Me.MscMenu.Name = "MscMenu"
            Me.MscMenu.Size = New System.Drawing.Size(159, 22)
            Me.MscMenu.Text = "C 编译器"
            '
            'MscppMenu
            '
            Me.MscppMenu.Name = "MscppMenu"
            Me.MscppMenu.Size = New System.Drawing.Size(159, 22)
            Me.MscppMenu.Text = "C++ 编译器"
            '
            'MscsMenu
            '
            Me.MscsMenu.Name = "MscsMenu"
            Me.MscsMenu.Size = New System.Drawing.Size(159, 22)
            Me.MscsMenu.Text = "C# 编译器"
            '
            'MsvbMenu
            '
            Me.MsvbMenu.Name = "MsvbMenu"
            Me.MsvbMenu.Size = New System.Drawing.Size(159, 22)
            Me.MsvbMenu.Text = "VB.NET 编译器"
            '
            'JavaMenu
            '
            Me.JavaMenu.Name = "JavaMenu"
            Me.JavaMenu.Size = New System.Drawing.Size(211, 22)
            Me.JavaMenu.Text = "Java"
            '
            'RemoveCompilerButton
            '
            Me.RemoveCompilerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.RemoveCompilerButton.Enabled = False
            Me.RemoveCompilerButton.Image = CType(resources.GetObject("RemoveCompilerButton.Image"), System.Drawing.Image)
            Me.RemoveCompilerButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.RemoveCompilerButton.Name = "RemoveCompilerButton"
            Me.RemoveCompilerButton.Size = New System.Drawing.Size(52, 22)
            Me.RemoveCompilerButton.Text = "移除(&R)"
            '
            'CompilerSeperator0
            '
            Me.CompilerSeperator0.Name = "CompilerSeperator0"
            Me.CompilerSeperator0.Size = New System.Drawing.Size(6, 25)
            '
            'MoveUpCompilerButton
            '
            Me.MoveUpCompilerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.MoveUpCompilerButton.Enabled = False
            Me.MoveUpCompilerButton.Image = CType(resources.GetObject("MoveUpCompilerButton.Image"), System.Drawing.Image)
            Me.MoveUpCompilerButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.MoveUpCompilerButton.Name = "MoveUpCompilerButton"
            Me.MoveUpCompilerButton.Size = New System.Drawing.Size(53, 22)
            Me.MoveUpCompilerButton.Text = "上移(&U)"
            '
            'MoveDownCompilerButton
            '
            Me.MoveDownCompilerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.MoveDownCompilerButton.Enabled = False
            Me.MoveDownCompilerButton.Image = CType(resources.GetObject("MoveDownCompilerButton.Image"), System.Drawing.Image)
            Me.MoveDownCompilerButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.MoveDownCompilerButton.Name = "MoveDownCompilerButton"
            Me.MoveDownCompilerButton.Size = New System.Drawing.Size(53, 22)
            Me.MoveDownCompilerButton.Text = "下移(&D)"
            '
            'CompilerSeperator1
            '
            Me.CompilerSeperator1.Name = "CompilerSeperator1"
            Me.CompilerSeperator1.Size = New System.Drawing.Size(6, 25)
            '
            'ApplyCompilerButton
            '
            Me.ApplyCompilerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.ApplyCompilerButton.Enabled = False
            Me.ApplyCompilerButton.Image = CType(resources.GetObject("ApplyCompilerButton.Image"), System.Drawing.Image)
            Me.ApplyCompilerButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ApplyCompilerButton.Name = "ApplyCompilerButton"
            Me.ApplyCompilerButton.Size = New System.Drawing.Size(52, 22)
            Me.ApplyCompilerButton.Text = "应用(&A)"
            '
            'TestSuitePage
            '
            Me.TestSuitePage.Controls.Add(Me.TestSuiteSplit)
            Me.TestSuitePage.Controls.Add(Me.TestSuiteToolStrip)
            Me.TestSuitePage.Location = New System.Drawing.Point(4, 22)
            Me.TestSuitePage.Margin = New System.Windows.Forms.Padding(2)
            Me.TestSuitePage.Name = "TestSuitePage"
            Me.TestSuitePage.Padding = New System.Windows.Forms.Padding(2)
            Me.TestSuitePage.Size = New System.Drawing.Size(584, 489)
            Me.TestSuitePage.TabIndex = 2
            Me.TestSuitePage.Text = "数据集映射"
            Me.TestSuitePage.UseVisualStyleBackColor = True
            '
            'TestSuiteSplit
            '
            Me.TestSuiteSplit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TestSuiteSplit.Location = New System.Drawing.Point(2, 27)
            Me.TestSuiteSplit.Margin = New System.Windows.Forms.Padding(2)
            Me.TestSuiteSplit.Name = "TestSuiteSplit"
            Me.TestSuiteSplit.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'TestSuiteSplit.Panel1
            '
            Me.TestSuiteSplit.Panel1.Controls.Add(Me.TestSuiteList)
            '
            'TestSuiteSplit.Panel2
            '
            Me.TestSuiteSplit.Panel2.Controls.Add(Me.TestSuiteProperty)
            Me.TestSuiteSplit.Size = New System.Drawing.Size(580, 460)
            Me.TestSuiteSplit.SplitterDistance = 196
            Me.TestSuiteSplit.SplitterWidth = 3
            Me.TestSuiteSplit.TabIndex = 11
            '
            'TestSuiteList
            '
            Me.TestSuiteList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.TestSuitePatternHeader, Me.TestSuiteClassHeader, Me.TestSuiteParameterHeader})
            Me.TestSuiteList.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TestSuiteList.FullRowSelect = True
            Me.TestSuiteList.HideSelection = False
            Me.TestSuiteList.Location = New System.Drawing.Point(0, 0)
            Me.TestSuiteList.Margin = New System.Windows.Forms.Padding(2)
            Me.TestSuiteList.MultiSelect = False
            Me.TestSuiteList.Name = "TestSuiteList"
            Me.TestSuiteList.Size = New System.Drawing.Size(580, 196)
            Me.TestSuiteList.TabIndex = 1
            Me.TestSuiteList.UseCompatibleStateImageBehavior = False
            Me.TestSuiteList.View = System.Windows.Forms.View.Details
            '
            'TestSuitePatternHeader
            '
            Me.TestSuitePatternHeader.Text = "文件名匹配"
            Me.TestSuitePatternHeader.Width = 119
            '
            'TestSuiteClassHeader
            '
            Me.TestSuiteClassHeader.Text = "类型"
            Me.TestSuiteClassHeader.Width = 93
            '
            'TestSuiteParameterHeader
            '
            Me.TestSuiteParameterHeader.Text = "参数"
            Me.TestSuiteParameterHeader.Width = 297
            '
            'TestSuiteProperty
            '
            Me.TestSuiteProperty.BackColor = System.Drawing.SystemColors.Window
            Me.TestSuiteProperty.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TestSuiteProperty.Location = New System.Drawing.Point(0, 0)
            Me.TestSuiteProperty.Margin = New System.Windows.Forms.Padding(2)
            Me.TestSuiteProperty.Name = "TestSuiteProperty"
            Me.TestSuiteProperty.PropertySort = System.Windows.Forms.PropertySort.Categorized
            Me.TestSuiteProperty.Size = New System.Drawing.Size(580, 261)
            Me.TestSuiteProperty.TabIndex = 6
            Me.TestSuiteProperty.ToolbarVisible = False
            '
            'TestSuiteToolStrip
            '
            Me.TestSuiteToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddTestSuiteButton, Me.RemoveTestSuiteButton, Me.TestSuiteSeperator0, Me.MoveUpTestSuiteButton, Me.MoveDownTestSuiteButton, Me.TestSuiteSeperator1, Me.ApplyTestSuiteButton})
            Me.TestSuiteToolStrip.Location = New System.Drawing.Point(2, 2)
            Me.TestSuiteToolStrip.Name = "TestSuiteToolStrip"
            Me.TestSuiteToolStrip.Size = New System.Drawing.Size(580, 25)
            Me.TestSuiteToolStrip.TabIndex = 10
            '
            'AddTestSuiteButton
            '
            Me.AddTestSuiteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.AddTestSuiteButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewTestSuiteMenu, Me.AddTestSuiteBar0, Me.AddAPlusBMenu})
            Me.AddTestSuiteButton.Image = CType(resources.GetObject("AddTestSuiteButton.Image"), System.Drawing.Image)
            Me.AddTestSuiteButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AddTestSuiteButton.Name = "AddTestSuiteButton"
            Me.AddTestSuiteButton.Size = New System.Drawing.Size(64, 22)
            Me.AddTestSuiteButton.Text = "添加(&A)"
            '
            'NewTestSuiteMenu
            '
            Me.NewTestSuiteMenu.Name = "NewTestSuiteMenu"
            Me.NewTestSuiteMenu.Size = New System.Drawing.Size(148, 22)
            Me.NewTestSuiteMenu.Text = "新数据集映射"
            '
            'AddTestSuiteBar0
            '
            Me.AddTestSuiteBar0.Name = "AddTestSuiteBar0"
            Me.AddTestSuiteBar0.Size = New System.Drawing.Size(145, 6)
            '
            'AddAPlusBMenu
            '
            Me.AddAPlusBMenu.Name = "AddAPlusBMenu"
            Me.AddAPlusBMenu.Size = New System.Drawing.Size(148, 22)
            Me.AddAPlusBMenu.Text = "A+B"
            '
            'RemoveTestSuiteButton
            '
            Me.RemoveTestSuiteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.RemoveTestSuiteButton.Enabled = False
            Me.RemoveTestSuiteButton.Image = CType(resources.GetObject("RemoveTestSuiteButton.Image"), System.Drawing.Image)
            Me.RemoveTestSuiteButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.RemoveTestSuiteButton.Name = "RemoveTestSuiteButton"
            Me.RemoveTestSuiteButton.Size = New System.Drawing.Size(52, 22)
            Me.RemoveTestSuiteButton.Text = "移除(&R)"
            '
            'TestSuiteSeperator0
            '
            Me.TestSuiteSeperator0.Name = "TestSuiteSeperator0"
            Me.TestSuiteSeperator0.Size = New System.Drawing.Size(6, 25)
            '
            'MoveUpTestSuiteButton
            '
            Me.MoveUpTestSuiteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.MoveUpTestSuiteButton.Enabled = False
            Me.MoveUpTestSuiteButton.Image = CType(resources.GetObject("MoveUpTestSuiteButton.Image"), System.Drawing.Image)
            Me.MoveUpTestSuiteButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.MoveUpTestSuiteButton.Name = "MoveUpTestSuiteButton"
            Me.MoveUpTestSuiteButton.Size = New System.Drawing.Size(53, 22)
            Me.MoveUpTestSuiteButton.Text = "上移(&U)"
            '
            'MoveDownTestSuiteButton
            '
            Me.MoveDownTestSuiteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.MoveDownTestSuiteButton.Enabled = False
            Me.MoveDownTestSuiteButton.Image = CType(resources.GetObject("MoveDownTestSuiteButton.Image"), System.Drawing.Image)
            Me.MoveDownTestSuiteButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.MoveDownTestSuiteButton.Name = "MoveDownTestSuiteButton"
            Me.MoveDownTestSuiteButton.Size = New System.Drawing.Size(53, 22)
            Me.MoveDownTestSuiteButton.Text = "下移(&D)"
            '
            'TestSuiteSeperator1
            '
            Me.TestSuiteSeperator1.Name = "TestSuiteSeperator1"
            Me.TestSuiteSeperator1.Size = New System.Drawing.Size(6, 25)
            '
            'ApplyTestSuiteButton
            '
            Me.ApplyTestSuiteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.ApplyTestSuiteButton.Enabled = False
            Me.ApplyTestSuiteButton.Image = CType(resources.GetObject("ApplyTestSuiteButton.Image"), System.Drawing.Image)
            Me.ApplyTestSuiteButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ApplyTestSuiteButton.Name = "ApplyTestSuiteButton"
            Me.ApplyTestSuiteButton.Size = New System.Drawing.Size(52, 22)
            Me.ApplyTestSuiteButton.Text = "应用(&A)"
            '
            'ExecutorPage
            '
            Me.ExecutorPage.Controls.Add(Me.SecurityList)
            Me.ExecutorPage.Controls.Add(Me.ExecutorToolStrip)
            Me.ExecutorPage.Location = New System.Drawing.Point(4, 22)
            Me.ExecutorPage.Margin = New System.Windows.Forms.Padding(2)
            Me.ExecutorPage.Name = "ExecutorPage"
            Me.ExecutorPage.Padding = New System.Windows.Forms.Padding(2)
            Me.ExecutorPage.Size = New System.Drawing.Size(584, 489)
            Me.ExecutorPage.TabIndex = 3
            Me.ExecutorPage.Text = "执行设置"
            Me.ExecutorPage.UseVisualStyleBackColor = True
            '
            'SecurityList
            '
            Me.SecurityList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.SecurityDesktopNameHeader, Me.SecurityUserNameHeader, Me.SecurityPasswordHeader})
            Me.SecurityList.Dock = System.Windows.Forms.DockStyle.Fill
            Me.SecurityList.FullRowSelect = True
            Me.SecurityList.HideSelection = False
            Me.SecurityList.Location = New System.Drawing.Point(2, 27)
            Me.SecurityList.Margin = New System.Windows.Forms.Padding(2)
            Me.SecurityList.MultiSelect = False
            Me.SecurityList.Name = "SecurityList"
            Me.SecurityList.Size = New System.Drawing.Size(580, 460)
            Me.SecurityList.TabIndex = 11
            Me.SecurityList.UseCompatibleStateImageBehavior = False
            Me.SecurityList.View = System.Windows.Forms.View.Details
            '
            'SecurityDesktopNameHeader
            '
            Me.SecurityDesktopNameHeader.Text = "桌面名称"
            Me.SecurityDesktopNameHeader.Width = 129
            '
            'SecurityUserNameHeader
            '
            Me.SecurityUserNameHeader.Text = "用户名"
            Me.SecurityUserNameHeader.Width = 122
            '
            'SecurityPasswordHeader
            '
            Me.SecurityPasswordHeader.Text = "密码"
            Me.SecurityPasswordHeader.Width = 154
            '
            'ExecutorToolStrip
            '
            Me.ExecutorToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExecutorSlotsLabel, Me.ExecutorSlotsText, Me.ExecutorSeperator0, Me.ExecutorSecurityLabel, Me.ExecutorSecurityCombo, Me.ToolStripSeparator1, Me.AddSecurityButton, Me.RemoveSecurityButton, Me.CheckSecurityButton, Me.ExecutorSeperator1, Me.ApplyExecutorButton})
            Me.ExecutorToolStrip.Location = New System.Drawing.Point(2, 2)
            Me.ExecutorToolStrip.Name = "ExecutorToolStrip"
            Me.ExecutorToolStrip.Size = New System.Drawing.Size(580, 25)
            Me.ExecutorToolStrip.TabIndex = 10
            '
            'ExecutorSlotsLabel
            '
            Me.ExecutorSlotsLabel.Name = "ExecutorSlotsLabel"
            Me.ExecutorSlotsLabel.Size = New System.Drawing.Size(47, 22)
            Me.ExecutorSlotsLabel.Text = "并发数:"
            '
            'ExecutorSlotsText
            '
            Me.ExecutorSlotsText.AutoSize = False
            Me.ExecutorSlotsText.Name = "ExecutorSlotsText"
            Me.ExecutorSlotsText.Size = New System.Drawing.Size(30, 25)
            '
            'ExecutorSeperator0
            '
            Me.ExecutorSeperator0.Name = "ExecutorSeperator0"
            Me.ExecutorSeperator0.Size = New System.Drawing.Size(6, 25)
            '
            'ExecutorSecurityLabel
            '
            Me.ExecutorSecurityLabel.Name = "ExecutorSecurityLabel"
            Me.ExecutorSecurityLabel.Size = New System.Drawing.Size(35, 22)
            Me.ExecutorSecurityLabel.Text = "安全:"
            '
            'ExecutorSecurityCombo
            '
            Me.ExecutorSecurityCombo.AutoSize = False
            Me.ExecutorSecurityCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.ExecutorSecurityCombo.Items.AddRange(New Object() {"启用", "禁用"})
            Me.ExecutorSecurityCombo.Name = "ExecutorSecurityCombo"
            Me.ExecutorSecurityCombo.Size = New System.Drawing.Size(48, 25)
            '
            'ToolStripSeparator1
            '
            Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
            Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
            '
            'AddSecurityButton
            '
            Me.AddSecurityButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.AddSecurityButton.Image = CType(resources.GetObject("AddSecurityButton.Image"), System.Drawing.Image)
            Me.AddSecurityButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AddSecurityButton.Name = "AddSecurityButton"
            Me.AddSecurityButton.Size = New System.Drawing.Size(52, 22)
            Me.AddSecurityButton.Text = "添加(&A)"
            '
            'RemoveSecurityButton
            '
            Me.RemoveSecurityButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.RemoveSecurityButton.Image = CType(resources.GetObject("RemoveSecurityButton.Image"), System.Drawing.Image)
            Me.RemoveSecurityButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.RemoveSecurityButton.Name = "RemoveSecurityButton"
            Me.RemoveSecurityButton.Size = New System.Drawing.Size(52, 22)
            Me.RemoveSecurityButton.Text = "移除(&R)"
            '
            'CheckSecurityButton
            '
            Me.CheckSecurityButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.CheckSecurityButton.Image = CType(resources.GetObject("CheckSecurityButton.Image"), System.Drawing.Image)
            Me.CheckSecurityButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CheckSecurityButton.Name = "CheckSecurityButton"
            Me.CheckSecurityButton.Size = New System.Drawing.Size(76, 22)
            Me.CheckSecurityButton.Text = "安全检查(&C)"
            '
            'ExecutorSeperator1
            '
            Me.ExecutorSeperator1.Name = "ExecutorSeperator1"
            Me.ExecutorSeperator1.Size = New System.Drawing.Size(6, 25)
            '
            'ApplyExecutorButton
            '
            Me.ApplyExecutorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.ApplyExecutorButton.Enabled = False
            Me.ApplyExecutorButton.Image = CType(resources.GetObject("ApplyExecutorButton.Image"), System.Drawing.Image)
            Me.ApplyExecutorButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ApplyExecutorButton.Name = "ApplyExecutorButton"
            Me.ApplyExecutorButton.Size = New System.Drawing.Size(52, 22)
            Me.ApplyExecutorButton.Text = "应用(&A)"
            '
            'LocalDataSourcePage
            '
            Me.LocalDataSourcePage.Controls.Add(Me.LocalSourceList)
            Me.LocalDataSourcePage.Controls.Add(Me.LocalSourceToolStrip)
            Me.LocalDataSourcePage.Location = New System.Drawing.Point(4, 22)
            Me.LocalDataSourcePage.Name = "LocalDataSourcePage"
            Me.LocalDataSourcePage.Padding = New System.Windows.Forms.Padding(3)
            Me.LocalDataSourcePage.Size = New System.Drawing.Size(584, 489)
            Me.LocalDataSourcePage.TabIndex = 4
            Me.LocalDataSourcePage.Text = "本地数据源"
            Me.LocalDataSourcePage.UseVisualStyleBackColor = True
            '
            'LocalSourceList
            '
            Me.LocalSourceList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.LocalSourceIdHeader, Me.LocalSourceFlagHeader, Me.LocalSourceFileNameHeader, Me.LocalSourceScoreHeader, Me.LocalSourceTimeUsageHeader, Me.LocalSourceMemoryUsageHeader, Me.LocalSourceDateHeader})
            Me.LocalSourceList.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LocalSourceList.FullRowSelect = True
            Me.LocalSourceList.HideSelection = False
            Me.LocalSourceList.Location = New System.Drawing.Point(3, 28)
            Me.LocalSourceList.MultiSelect = False
            Me.LocalSourceList.Name = "LocalSourceList"
            Me.LocalSourceList.Size = New System.Drawing.Size(578, 458)
            Me.LocalSourceList.TabIndex = 3
            Me.LocalSourceList.UseCompatibleStateImageBehavior = False
            Me.LocalSourceList.View = System.Windows.Forms.View.Details
            '
            'LocalSourceIdHeader
            '
            Me.LocalSourceIdHeader.Text = "Id"
            Me.LocalSourceIdHeader.Width = 59
            '
            'LocalSourceFlagHeader
            '
            Me.LocalSourceFlagHeader.Text = "Flag"
            Me.LocalSourceFlagHeader.Width = 78
            '
            'LocalSourceFileNameHeader
            '
            Me.LocalSourceFileNameHeader.Text = "文件名"
            '
            'LocalSourceScoreHeader
            '
            Me.LocalSourceScoreHeader.Text = "得分"
            '
            'LocalSourceTimeUsageHeader
            '
            Me.LocalSourceTimeUsageHeader.Text = "时间"
            '
            'LocalSourceMemoryUsageHeader
            '
            Me.LocalSourceMemoryUsageHeader.Text = "内存"
            '
            'LocalSourceDateHeader
            '
            Me.LocalSourceDateHeader.Text = "日期"
            Me.LocalSourceDateHeader.Width = 124
            '
            'LocalSourceToolStrip
            '
            Me.LocalSourceToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RefershLocalButton})
            Me.LocalSourceToolStrip.Location = New System.Drawing.Point(3, 3)
            Me.LocalSourceToolStrip.Name = "LocalSourceToolStrip"
            Me.LocalSourceToolStrip.Size = New System.Drawing.Size(578, 25)
            Me.LocalSourceToolStrip.TabIndex = 2
            '
            'RefershLocalButton
            '
            Me.RefershLocalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.RefershLocalButton.Image = CType(resources.GetObject("RefershLocalButton.Image"), System.Drawing.Image)
            Me.RefershLocalButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.RefershLocalButton.Name = "RefershLocalButton"
            Me.RefershLocalButton.Size = New System.Drawing.Size(52, 22)
            Me.RefershLocalButton.Text = "刷新(&R)"
            '
            'RemoteDataSourcePage
            '
            Me.RemoteDataSourcePage.Location = New System.Drawing.Point(4, 22)
            Me.RemoteDataSourcePage.Name = "RemoteDataSourcePage"
            Me.RemoteDataSourcePage.Padding = New System.Windows.Forms.Padding(3)
            Me.RemoteDataSourcePage.Size = New System.Drawing.Size(584, 489)
            Me.RemoteDataSourcePage.TabIndex = 5
            Me.RemoteDataSourcePage.Text = "外部数据源"
            Me.RemoteDataSourcePage.UseVisualStyleBackColor = True
            '
            'ServiceTimer
            '
            Me.ServiceTimer.Interval = 500
            '
            'PythonMenu
            '
            Me.PythonMenu.Name = "PythonMenu"
            Me.PythonMenu.Size = New System.Drawing.Size(211, 22)
            Me.PythonMenu.Text = "Python"
            '
            'ConsoleForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(784, 562)
            Me.Controls.Add(Me.SplitContainer)
            Me.Controls.Add(Me.StatusStrip)
            Me.Controls.Add(Me.MenuStrip)
            Me.MainMenuStrip = Me.MenuStrip
            Me.Margin = New System.Windows.Forms.Padding(2)
            Me.Name = "ConsoleForm"
            Me.Text = "VijosNT 控制台"
            Me.MenuStrip.ResumeLayout(False)
            Me.MenuStrip.PerformLayout()
            Me.StatusStrip.ResumeLayout(False)
            Me.StatusStrip.PerformLayout()
            Me.SplitContainer.Panel1.ResumeLayout(False)
            Me.SplitContainer.Panel2.ResumeLayout(False)
            Me.SplitContainer.ResumeLayout(False)
            Me.TabControl.ResumeLayout(False)
            Me.RootPage.ResumeLayout(False)
            Me.RootPage.PerformLayout()
            Me.ServiceToolStrip.ResumeLayout(False)
            Me.ServiceToolStrip.PerformLayout()
            Me.CompilerPage.ResumeLayout(False)
            Me.CompilerPage.PerformLayout()
            Me.CompilerSplit.Panel1.ResumeLayout(False)
            Me.CompilerSplit.Panel2.ResumeLayout(False)
            Me.CompilerSplit.ResumeLayout(False)
            Me.CompilerToolStrip.ResumeLayout(False)
            Me.CompilerToolStrip.PerformLayout()
            Me.TestSuitePage.ResumeLayout(False)
            Me.TestSuitePage.PerformLayout()
            Me.TestSuiteSplit.Panel1.ResumeLayout(False)
            Me.TestSuiteSplit.Panel2.ResumeLayout(False)
            Me.TestSuiteSplit.ResumeLayout(False)
            Me.TestSuiteToolStrip.ResumeLayout(False)
            Me.TestSuiteToolStrip.PerformLayout()
            Me.ExecutorPage.ResumeLayout(False)
            Me.ExecutorPage.PerformLayout()
            Me.ExecutorToolStrip.ResumeLayout(False)
            Me.ExecutorToolStrip.PerformLayout()
            Me.LocalDataSourcePage.ResumeLayout(False)
            Me.LocalDataSourcePage.PerformLayout()
            Me.LocalSourceToolStrip.ResumeLayout(False)
            Me.LocalSourceToolStrip.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
        Friend WithEvents FileMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ExitMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
        Friend WithEvents SplitContainer As System.Windows.Forms.SplitContainer
        Friend WithEvents NavigationTree As System.Windows.Forms.TreeView
        Friend WithEvents HelpMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AboutMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents TabControl As System.Windows.Forms.TabControl
        Friend WithEvents RootPage As System.Windows.Forms.TabPage
        Friend WithEvents CompilerPage As System.Windows.Forms.TabPage
        Friend WithEvents TestSuitePage As System.Windows.Forms.TabPage
        Friend WithEvents ExecutorPage As System.Windows.Forms.TabPage
        Friend WithEvents StatusLabel As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents ServiceTimer As System.Windows.Forms.Timer
        Friend WithEvents HomePageMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents HelpStrip0 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents WikiToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents CheckUpdateMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents HelpStrip1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents LocalDataSourcePage As System.Windows.Forms.TabPage
        Friend WithEvents RemoteDataSourcePage As System.Windows.Forms.TabPage
        Friend WithEvents ReportIssue As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents CompilerToolStrip As System.Windows.Forms.ToolStrip
        Friend WithEvents RemoveCompilerButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents MoveUpCompilerButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents MoveDownCompilerButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents ApplyCompilerButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents AddCompilerButton As System.Windows.Forms.ToolStripSplitButton
        Friend WithEvents CompilerSplit As System.Windows.Forms.SplitContainer
        Friend WithEvents CompilerList As System.Windows.Forms.ListView
        Friend WithEvents CompilerPatternHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents CompilerCommandLineHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents CompilerProperty As System.Windows.Forms.PropertyGrid
        Friend WithEvents CompilerSeperator0 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents CompilerSeperator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents ServiceToolStrip As System.Windows.Forms.ToolStrip
        Friend WithEvents InstallButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents UninstallButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents TestSuiteToolStrip As System.Windows.Forms.ToolStrip
        Friend WithEvents TestSuiteSplit As System.Windows.Forms.SplitContainer
        Friend WithEvents TestSuiteList As System.Windows.Forms.ListView
        Friend WithEvents TestSuitePatternHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents TestSuiteClassHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents TestSuiteParameterHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents TestSuiteProperty As System.Windows.Forms.PropertyGrid
        Friend WithEvents AddTestSuiteButton As System.Windows.Forms.ToolStripSplitButton
        Friend WithEvents RemoveTestSuiteButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents MoveUpTestSuiteButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents TestSuiteSeperator0 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents MoveDownTestSuiteButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents TestSuiteSeperator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents ApplyTestSuiteButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents ExecutorToolStrip As System.Windows.Forms.ToolStrip
        Friend WithEvents ExecutorSlotsLabel As System.Windows.Forms.ToolStripLabel
        Friend WithEvents ExecutorSlotsText As System.Windows.Forms.ToolStripTextBox
        Friend WithEvents ExecutorSecurityLabel As System.Windows.Forms.ToolStripLabel
        Friend WithEvents ExecutorSecurityCombo As System.Windows.Forms.ToolStripComboBox
        Friend WithEvents ApplyExecutorButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents ExecutorSeperator0 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents ExecutorSeperator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents AddSecurityButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents RemoveSecurityButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents CheckSecurityButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents SecurityList As System.Windows.Forms.ListView
        Friend WithEvents SecurityDesktopNameHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents SecurityUserNameHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents SecurityPasswordHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents LocalSourceToolStrip As System.Windows.Forms.ToolStrip
        Friend WithEvents RefershLocalButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents LocalSourceList As System.Windows.Forms.ListView
        Friend WithEvents LocalSourceIdHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents LocalSourceFlagHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents LocalSourceFileNameHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents LocalSourceScoreHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents LocalSourceTimeUsageHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents LocalSourceMemoryUsageHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents LocalSourceDateHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents NewCompilerMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AddCompilerBar0 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents MingwMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents GccMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents GppMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents MsvsMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents MscMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents MscppMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents MscsMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents MsvbMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents NewTestSuiteMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AddTestSuiteBar0 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents AddAPlusBMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents JavaMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents PythonMenu As System.Windows.Forms.ToolStripMenuItem
    End Class
End Namespace