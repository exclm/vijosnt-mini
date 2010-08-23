Namespace Foreground
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
            Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("VijosNT", New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2, TreeNode3})
            Me.MenuStrip = New System.Windows.Forms.MenuStrip()
            Me.FileMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.ExitMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.HelpMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.AboutMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.StatusStrip = New System.Windows.Forms.StatusStrip()
            Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
            Me.SplitContainer = New System.Windows.Forms.SplitContainer()
            Me.NavigationTree = New System.Windows.Forms.TreeView()
            Me.TabControl = New System.Windows.Forms.TabControl()
            Me.RootPage = New System.Windows.Forms.TabPage()
            Me.UninstallButton = New System.Windows.Forms.Button()
            Me.InstallButton = New System.Windows.Forms.Button()
            Me.CompilerPage = New System.Windows.Forms.TabPage()
            Me.CompilerSplit = New System.Windows.Forms.SplitContainer()
            Me.MoveDownCompilerButton = New System.Windows.Forms.Button()
            Me.MoveUpCompilerButton = New System.Windows.Forms.Button()
            Me.RemoveCompilerButton = New System.Windows.Forms.Button()
            Me.AddCompilerButton = New System.Windows.Forms.Button()
            Me.CompilerList = New System.Windows.Forms.ListView()
            Me.CompilerPatternHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.CompilerCommandLineHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.CompilerProperty = New System.Windows.Forms.PropertyGrid()
            Me.TestSuitePage = New System.Windows.Forms.TabPage()
            Me.TestSuiteSplit = New System.Windows.Forms.SplitContainer()
            Me.MoveDownTestSuiteButton = New System.Windows.Forms.Button()
            Me.MoveUpTestSuiteButton = New System.Windows.Forms.Button()
            Me.RemoveTestSuiteButton = New System.Windows.Forms.Button()
            Me.AddTestSuiteButton = New System.Windows.Forms.Button()
            Me.TestSuiteList = New System.Windows.Forms.ListView()
            Me.TestSuitePatternHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.TestSuiteClassHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.TestSuiteParameterHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.TestSuiteProperty = New System.Windows.Forms.PropertyGrid()
            Me.ExecutorPage = New System.Windows.Forms.TabPage()
            Me.SecurityGroup = New System.Windows.Forms.GroupBox()
            Me.ListView1 = New System.Windows.Forms.ListView()
            Me.SecurityDesktopNameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.SecurityUserNameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.SecurityPasswordHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.EnableSecurityCheck = New System.Windows.Forms.CheckBox()
            Me.ExecutorSlotsLabel = New System.Windows.Forms.Label()
            Me.ExecutorSlotsText = New System.Windows.Forms.TextBox()
            Me.ServiceTimer = New System.Windows.Forms.Timer(Me.components)
            Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
            Me.HelpStrip0 = New System.Windows.Forms.ToolStripSeparator()
            Me.HomePageMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.MenuStrip.SuspendLayout()
            Me.StatusStrip.SuspendLayout()
            Me.SplitContainer.Panel1.SuspendLayout()
            Me.SplitContainer.Panel2.SuspendLayout()
            Me.SplitContainer.SuspendLayout()
            Me.TabControl.SuspendLayout()
            Me.RootPage.SuspendLayout()
            Me.CompilerPage.SuspendLayout()
            Me.CompilerSplit.Panel1.SuspendLayout()
            Me.CompilerSplit.Panel2.SuspendLayout()
            Me.CompilerSplit.SuspendLayout()
            Me.TestSuitePage.SuspendLayout()
            Me.TestSuiteSplit.Panel1.SuspendLayout()
            Me.TestSuiteSplit.Panel2.SuspendLayout()
            Me.TestSuiteSplit.SuspendLayout()
            Me.ExecutorPage.SuspendLayout()
            Me.SecurityGroup.SuspendLayout()
            CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
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
            Me.HelpMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HomePageMenu, Me.HelpStrip0, Me.AboutMenu})
            Me.HelpMenu.Name = "HelpMenu"
            Me.HelpMenu.Size = New System.Drawing.Size(61, 21)
            Me.HelpMenu.Text = "帮助(&H)"
            '
            'AboutMenu
            '
            Me.AboutMenu.Name = "AboutMenu"
            Me.AboutMenu.Size = New System.Drawing.Size(152, 22)
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
            Me.SplitContainer.SplitterDistance = 194
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
            TreeNode4.Name = "Root"
            TreeNode4.Text = "VijosNT"
            Me.NavigationTree.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode4})
            Me.NavigationTree.Size = New System.Drawing.Size(194, 515)
            Me.NavigationTree.TabIndex = 0
            '
            'TabControl
            '
            Me.TabControl.Controls.Add(Me.RootPage)
            Me.TabControl.Controls.Add(Me.CompilerPage)
            Me.TabControl.Controls.Add(Me.TestSuitePage)
            Me.TabControl.Controls.Add(Me.ExecutorPage)
            Me.TabControl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabControl.Location = New System.Drawing.Point(0, 0)
            Me.TabControl.Margin = New System.Windows.Forms.Padding(2)
            Me.TabControl.Name = "TabControl"
            Me.TabControl.SelectedIndex = 0
            Me.TabControl.Size = New System.Drawing.Size(587, 515)
            Me.TabControl.TabIndex = 0
            '
            'RootPage
            '
            Me.RootPage.Controls.Add(Me.UninstallButton)
            Me.RootPage.Controls.Add(Me.InstallButton)
            Me.RootPage.Location = New System.Drawing.Point(4, 22)
            Me.RootPage.Margin = New System.Windows.Forms.Padding(2)
            Me.RootPage.Name = "RootPage"
            Me.RootPage.Padding = New System.Windows.Forms.Padding(2)
            Me.RootPage.Size = New System.Drawing.Size(579, 489)
            Me.RootPage.TabIndex = 0
            Me.RootPage.Text = "服务"
            Me.RootPage.UseVisualStyleBackColor = True
            '
            'UninstallButton
            '
            Me.UninstallButton.Location = New System.Drawing.Point(29, 61)
            Me.UninstallButton.Margin = New System.Windows.Forms.Padding(2)
            Me.UninstallButton.Name = "UninstallButton"
            Me.UninstallButton.Size = New System.Drawing.Size(90, 23)
            Me.UninstallButton.TabIndex = 1
            Me.UninstallButton.Text = "卸载服务(&U)"
            Me.UninstallButton.UseVisualStyleBackColor = True
            '
            'InstallButton
            '
            Me.InstallButton.Location = New System.Drawing.Point(29, 34)
            Me.InstallButton.Margin = New System.Windows.Forms.Padding(2)
            Me.InstallButton.Name = "InstallButton"
            Me.InstallButton.Size = New System.Drawing.Size(90, 23)
            Me.InstallButton.TabIndex = 0
            Me.InstallButton.Text = "安装服务(&I)"
            Me.InstallButton.UseVisualStyleBackColor = True
            '
            'CompilerPage
            '
            Me.CompilerPage.Controls.Add(Me.CompilerSplit)
            Me.CompilerPage.Location = New System.Drawing.Point(4, 22)
            Me.CompilerPage.Margin = New System.Windows.Forms.Padding(2)
            Me.CompilerPage.Name = "CompilerPage"
            Me.CompilerPage.Padding = New System.Windows.Forms.Padding(2)
            Me.CompilerPage.Size = New System.Drawing.Size(579, 489)
            Me.CompilerPage.TabIndex = 1
            Me.CompilerPage.Text = "编译器映射"
            Me.CompilerPage.UseVisualStyleBackColor = True
            '
            'CompilerSplit
            '
            Me.CompilerSplit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CompilerSplit.Location = New System.Drawing.Point(2, 2)
            Me.CompilerSplit.Margin = New System.Windows.Forms.Padding(2)
            Me.CompilerSplit.Name = "CompilerSplit"
            Me.CompilerSplit.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'CompilerSplit.Panel1
            '
            Me.CompilerSplit.Panel1.Controls.Add(Me.MoveDownCompilerButton)
            Me.CompilerSplit.Panel1.Controls.Add(Me.MoveUpCompilerButton)
            Me.CompilerSplit.Panel1.Controls.Add(Me.RemoveCompilerButton)
            Me.CompilerSplit.Panel1.Controls.Add(Me.AddCompilerButton)
            Me.CompilerSplit.Panel1.Controls.Add(Me.CompilerList)
            '
            'CompilerSplit.Panel2
            '
            Me.CompilerSplit.Panel2.Controls.Add(Me.CompilerProperty)
            Me.CompilerSplit.Size = New System.Drawing.Size(575, 485)
            Me.CompilerSplit.SplitterDistance = 145
            Me.CompilerSplit.SplitterWidth = 3
            Me.CompilerSplit.TabIndex = 6
            '
            'MoveDownCompilerButton
            '
            Me.MoveDownCompilerButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.MoveDownCompilerButton.Enabled = False
            Me.MoveDownCompilerButton.Location = New System.Drawing.Point(550, 29)
            Me.MoveDownCompilerButton.Margin = New System.Windows.Forms.Padding(2)
            Me.MoveDownCompilerButton.Name = "MoveDownCompilerButton"
            Me.MoveDownCompilerButton.Size = New System.Drawing.Size(23, 23)
            Me.MoveDownCompilerButton.TabIndex = 6
            Me.MoveDownCompilerButton.Text = "↓"
            Me.MoveDownCompilerButton.UseVisualStyleBackColor = True
            '
            'MoveUpCompilerButton
            '
            Me.MoveUpCompilerButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.MoveUpCompilerButton.Enabled = False
            Me.MoveUpCompilerButton.Location = New System.Drawing.Point(550, 2)
            Me.MoveUpCompilerButton.Margin = New System.Windows.Forms.Padding(2)
            Me.MoveUpCompilerButton.Name = "MoveUpCompilerButton"
            Me.MoveUpCompilerButton.Size = New System.Drawing.Size(23, 23)
            Me.MoveUpCompilerButton.TabIndex = 5
            Me.MoveUpCompilerButton.Text = "↑"
            Me.MoveUpCompilerButton.UseVisualStyleBackColor = True
            '
            'RemoveCompilerButton
            '
            Me.RemoveCompilerButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.RemoveCompilerButton.Enabled = False
            Me.RemoveCompilerButton.Location = New System.Drawing.Point(81, 120)
            Me.RemoveCompilerButton.Margin = New System.Windows.Forms.Padding(2)
            Me.RemoveCompilerButton.Name = "RemoveCompilerButton"
            Me.RemoveCompilerButton.Size = New System.Drawing.Size(75, 23)
            Me.RemoveCompilerButton.TabIndex = 4
            Me.RemoveCompilerButton.Text = "移除(&R)"
            Me.RemoveCompilerButton.UseVisualStyleBackColor = True
            '
            'AddCompilerButton
            '
            Me.AddCompilerButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.AddCompilerButton.Location = New System.Drawing.Point(2, 120)
            Me.AddCompilerButton.Margin = New System.Windows.Forms.Padding(2)
            Me.AddCompilerButton.Name = "AddCompilerButton"
            Me.AddCompilerButton.Size = New System.Drawing.Size(75, 23)
            Me.AddCompilerButton.TabIndex = 3
            Me.AddCompilerButton.Text = "添加(&A)"
            Me.AddCompilerButton.UseVisualStyleBackColor = True
            '
            'CompilerList
            '
            Me.CompilerList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.CompilerList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.CompilerPatternHeader, Me.CompilerCommandLineHeader})
            Me.CompilerList.FullRowSelect = True
            Me.CompilerList.HideSelection = False
            Me.CompilerList.Location = New System.Drawing.Point(2, 2)
            Me.CompilerList.Margin = New System.Windows.Forms.Padding(2)
            Me.CompilerList.MultiSelect = False
            Me.CompilerList.Name = "CompilerList"
            Me.CompilerList.Size = New System.Drawing.Size(544, 114)
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
            Me.CompilerProperty.Size = New System.Drawing.Size(575, 337)
            Me.CompilerProperty.TabIndex = 6
            Me.CompilerProperty.ToolbarVisible = False
            '
            'TestSuitePage
            '
            Me.TestSuitePage.Controls.Add(Me.TestSuiteSplit)
            Me.TestSuitePage.Location = New System.Drawing.Point(4, 22)
            Me.TestSuitePage.Margin = New System.Windows.Forms.Padding(2)
            Me.TestSuitePage.Name = "TestSuitePage"
            Me.TestSuitePage.Padding = New System.Windows.Forms.Padding(2)
            Me.TestSuitePage.Size = New System.Drawing.Size(579, 489)
            Me.TestSuitePage.TabIndex = 2
            Me.TestSuitePage.Text = "数据集映射"
            Me.TestSuitePage.UseVisualStyleBackColor = True
            '
            'TestSuiteSplit
            '
            Me.TestSuiteSplit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TestSuiteSplit.Location = New System.Drawing.Point(2, 2)
            Me.TestSuiteSplit.Margin = New System.Windows.Forms.Padding(2)
            Me.TestSuiteSplit.Name = "TestSuiteSplit"
            Me.TestSuiteSplit.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'TestSuiteSplit.Panel1
            '
            Me.TestSuiteSplit.Panel1.Controls.Add(Me.MoveDownTestSuiteButton)
            Me.TestSuiteSplit.Panel1.Controls.Add(Me.MoveUpTestSuiteButton)
            Me.TestSuiteSplit.Panel1.Controls.Add(Me.RemoveTestSuiteButton)
            Me.TestSuiteSplit.Panel1.Controls.Add(Me.AddTestSuiteButton)
            Me.TestSuiteSplit.Panel1.Controls.Add(Me.TestSuiteList)
            '
            'TestSuiteSplit.Panel2
            '
            Me.TestSuiteSplit.Panel2.Controls.Add(Me.TestSuiteProperty)
            Me.TestSuiteSplit.Size = New System.Drawing.Size(575, 485)
            Me.TestSuiteSplit.SplitterDistance = 215
            Me.TestSuiteSplit.SplitterWidth = 3
            Me.TestSuiteSplit.TabIndex = 7
            '
            'MoveDownTestSuiteButton
            '
            Me.MoveDownTestSuiteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.MoveDownTestSuiteButton.Enabled = False
            Me.MoveDownTestSuiteButton.Location = New System.Drawing.Point(550, 29)
            Me.MoveDownTestSuiteButton.Margin = New System.Windows.Forms.Padding(2)
            Me.MoveDownTestSuiteButton.Name = "MoveDownTestSuiteButton"
            Me.MoveDownTestSuiteButton.Size = New System.Drawing.Size(23, 23)
            Me.MoveDownTestSuiteButton.TabIndex = 6
            Me.MoveDownTestSuiteButton.Text = "↓"
            Me.MoveDownTestSuiteButton.UseVisualStyleBackColor = True
            '
            'MoveUpTestSuiteButton
            '
            Me.MoveUpTestSuiteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.MoveUpTestSuiteButton.Enabled = False
            Me.MoveUpTestSuiteButton.Location = New System.Drawing.Point(550, 2)
            Me.MoveUpTestSuiteButton.Margin = New System.Windows.Forms.Padding(2)
            Me.MoveUpTestSuiteButton.Name = "MoveUpTestSuiteButton"
            Me.MoveUpTestSuiteButton.Size = New System.Drawing.Size(23, 23)
            Me.MoveUpTestSuiteButton.TabIndex = 5
            Me.MoveUpTestSuiteButton.Text = "↑"
            Me.MoveUpTestSuiteButton.UseVisualStyleBackColor = True
            '
            'RemoveTestSuiteButton
            '
            Me.RemoveTestSuiteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.RemoveTestSuiteButton.Enabled = False
            Me.RemoveTestSuiteButton.Location = New System.Drawing.Point(81, 190)
            Me.RemoveTestSuiteButton.Margin = New System.Windows.Forms.Padding(2)
            Me.RemoveTestSuiteButton.Name = "RemoveTestSuiteButton"
            Me.RemoveTestSuiteButton.Size = New System.Drawing.Size(75, 23)
            Me.RemoveTestSuiteButton.TabIndex = 4
            Me.RemoveTestSuiteButton.Text = "移除(&R)"
            Me.RemoveTestSuiteButton.UseVisualStyleBackColor = True
            '
            'AddTestSuiteButton
            '
            Me.AddTestSuiteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.AddTestSuiteButton.Location = New System.Drawing.Point(2, 190)
            Me.AddTestSuiteButton.Margin = New System.Windows.Forms.Padding(2)
            Me.AddTestSuiteButton.Name = "AddTestSuiteButton"
            Me.AddTestSuiteButton.Size = New System.Drawing.Size(75, 23)
            Me.AddTestSuiteButton.TabIndex = 3
            Me.AddTestSuiteButton.Text = "添加(&A)"
            Me.AddTestSuiteButton.UseVisualStyleBackColor = True
            '
            'TestSuiteList
            '
            Me.TestSuiteList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.TestSuiteList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.TestSuitePatternHeader, Me.TestSuiteClassHeader, Me.TestSuiteParameterHeader})
            Me.TestSuiteList.FullRowSelect = True
            Me.TestSuiteList.HideSelection = False
            Me.TestSuiteList.Location = New System.Drawing.Point(2, 2)
            Me.TestSuiteList.Margin = New System.Windows.Forms.Padding(2)
            Me.TestSuiteList.MultiSelect = False
            Me.TestSuiteList.Name = "TestSuiteList"
            Me.TestSuiteList.Size = New System.Drawing.Size(544, 184)
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
            Me.TestSuiteProperty.Size = New System.Drawing.Size(575, 267)
            Me.TestSuiteProperty.TabIndex = 6
            Me.TestSuiteProperty.ToolbarVisible = False
            '
            'ExecutorPage
            '
            Me.ExecutorPage.Controls.Add(Me.SecurityGroup)
            Me.ExecutorPage.Controls.Add(Me.EnableSecurityCheck)
            Me.ExecutorPage.Controls.Add(Me.ExecutorSlotsLabel)
            Me.ExecutorPage.Controls.Add(Me.ExecutorSlotsText)
            Me.ExecutorPage.Location = New System.Drawing.Point(4, 22)
            Me.ExecutorPage.Margin = New System.Windows.Forms.Padding(2)
            Me.ExecutorPage.Name = "ExecutorPage"
            Me.ExecutorPage.Padding = New System.Windows.Forms.Padding(2)
            Me.ExecutorPage.Size = New System.Drawing.Size(579, 489)
            Me.ExecutorPage.TabIndex = 3
            Me.ExecutorPage.Text = "执行设置"
            Me.ExecutorPage.UseVisualStyleBackColor = True
            '
            'SecurityGroup
            '
            Me.SecurityGroup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.SecurityGroup.Controls.Add(Me.ListView1)
            Me.SecurityGroup.Enabled = False
            Me.SecurityGroup.Location = New System.Drawing.Point(31, 101)
            Me.SecurityGroup.Margin = New System.Windows.Forms.Padding(2)
            Me.SecurityGroup.Name = "SecurityGroup"
            Me.SecurityGroup.Padding = New System.Windows.Forms.Padding(2)
            Me.SecurityGroup.Size = New System.Drawing.Size(511, 364)
            Me.SecurityGroup.TabIndex = 3
            Me.SecurityGroup.TabStop = False
            Me.SecurityGroup.Text = "安全设置"
            '
            'ListView1
            '
            Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.SecurityDesktopNameHeader, Me.SecurityUserNameHeader, Me.SecurityPasswordHeader})
            Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ListView1.FullRowSelect = True
            Me.ListView1.HideSelection = False
            Me.ListView1.Location = New System.Drawing.Point(2, 16)
            Me.ListView1.Margin = New System.Windows.Forms.Padding(2)
            Me.ListView1.MultiSelect = False
            Me.ListView1.Name = "ListView1"
            Me.ListView1.Size = New System.Drawing.Size(507, 346)
            Me.ListView1.TabIndex = 2
            Me.ListView1.UseCompatibleStateImageBehavior = False
            Me.ListView1.View = System.Windows.Forms.View.Details
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
            'EnableSecurityCheck
            '
            Me.EnableSecurityCheck.AutoSize = True
            Me.EnableSecurityCheck.Location = New System.Drawing.Point(31, 66)
            Me.EnableSecurityCheck.Margin = New System.Windows.Forms.Padding(2)
            Me.EnableSecurityCheck.Name = "EnableSecurityCheck"
            Me.EnableSecurityCheck.Size = New System.Drawing.Size(72, 16)
            Me.EnableSecurityCheck.TabIndex = 2
            Me.EnableSecurityCheck.Text = "不要点我"
            Me.EnableSecurityCheck.UseVisualStyleBackColor = True
            '
            'ExecutorSlotsLabel
            '
            Me.ExecutorSlotsLabel.AutoSize = True
            Me.ExecutorSlotsLabel.Location = New System.Drawing.Point(28, 31)
            Me.ExecutorSlotsLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.ExecutorSlotsLabel.Name = "ExecutorSlotsLabel"
            Me.ExecutorSlotsLabel.Size = New System.Drawing.Size(119, 12)
            Me.ExecutorSlotsLabel.TabIndex = 1
            Me.ExecutorSlotsLabel.Text = "同时进行的任务个数:"
            '
            'ExecutorSlotsText
            '
            Me.ExecutorSlotsText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ExecutorSlotsText.Location = New System.Drawing.Point(151, 29)
            Me.ExecutorSlotsText.Margin = New System.Windows.Forms.Padding(2)
            Me.ExecutorSlotsText.Name = "ExecutorSlotsText"
            Me.ExecutorSlotsText.Size = New System.Drawing.Size(392, 21)
            Me.ExecutorSlotsText.TabIndex = 0
            '
            'ServiceTimer
            '
            Me.ServiceTimer.Interval = 500
            '
            'ErrorProvider
            '
            Me.ErrorProvider.ContainerControl = Me
            '
            'HelpStrip0
            '
            Me.HelpStrip0.Name = "HelpStrip0"
            Me.HelpStrip0.Size = New System.Drawing.Size(149, 6)
            '
            'HomePageMenu
            '
            Me.HomePageMenu.Name = "HomePageMenu"
            Me.HomePageMenu.Size = New System.Drawing.Size(152, 22)
            Me.HomePageMenu.Text = "主页(&H)"
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
            Me.CompilerPage.ResumeLayout(False)
            Me.CompilerSplit.Panel1.ResumeLayout(False)
            Me.CompilerSplit.Panel2.ResumeLayout(False)
            Me.CompilerSplit.ResumeLayout(False)
            Me.TestSuitePage.ResumeLayout(False)
            Me.TestSuiteSplit.Panel1.ResumeLayout(False)
            Me.TestSuiteSplit.Panel2.ResumeLayout(False)
            Me.TestSuiteSplit.ResumeLayout(False)
            Me.ExecutorPage.ResumeLayout(False)
            Me.ExecutorPage.PerformLayout()
            Me.SecurityGroup.ResumeLayout(False)
            CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
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
        Friend WithEvents InstallButton As System.Windows.Forms.Button
        Friend WithEvents UninstallButton As System.Windows.Forms.Button
        Friend WithEvents StatusLabel As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents ServiceTimer As System.Windows.Forms.Timer
        Friend WithEvents CompilerSplit As System.Windows.Forms.SplitContainer
        Friend WithEvents CompilerProperty As System.Windows.Forms.PropertyGrid
        Friend WithEvents MoveDownCompilerButton As System.Windows.Forms.Button
        Friend WithEvents MoveUpCompilerButton As System.Windows.Forms.Button
        Friend WithEvents RemoveCompilerButton As System.Windows.Forms.Button
        Friend WithEvents AddCompilerButton As System.Windows.Forms.Button
        Friend WithEvents CompilerList As System.Windows.Forms.ListView
        Friend WithEvents CompilerPatternHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents CompilerCommandLineHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents TestSuiteSplit As System.Windows.Forms.SplitContainer
        Friend WithEvents MoveDownTestSuiteButton As System.Windows.Forms.Button
        Friend WithEvents MoveUpTestSuiteButton As System.Windows.Forms.Button
        Friend WithEvents RemoveTestSuiteButton As System.Windows.Forms.Button
        Friend WithEvents AddTestSuiteButton As System.Windows.Forms.Button
        Friend WithEvents TestSuiteList As System.Windows.Forms.ListView
        Friend WithEvents TestSuitePatternHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents TestSuiteClassHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents TestSuiteProperty As System.Windows.Forms.PropertyGrid
        Friend WithEvents TestSuiteParameterHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents ExecutorSlotsText As System.Windows.Forms.TextBox
        Friend WithEvents ErrorProvider As System.Windows.Forms.ErrorProvider
        Friend WithEvents ExecutorSlotsLabel As System.Windows.Forms.Label
        Friend WithEvents EnableSecurityCheck As System.Windows.Forms.CheckBox
        Friend WithEvents SecurityGroup As System.Windows.Forms.GroupBox
        Friend WithEvents ListView1 As System.Windows.Forms.ListView
        Friend WithEvents SecurityDesktopNameHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents SecurityUserNameHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents SecurityPasswordHeader As System.Windows.Forms.ColumnHeader
        Friend WithEvents HomePageMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents HelpStrip0 As System.Windows.Forms.ToolStripSeparator
    End Class
End Namespace
