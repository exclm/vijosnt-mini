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
            Dim TreeNode9 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("编译器映射")
            Dim TreeNode10 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("数据集映射")
            Dim TreeNode11 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("执行设置")
            Dim TreeNode12 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("VijosNT", New System.Windows.Forms.TreeNode() {TreeNode9, TreeNode10, TreeNode11})
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
            Me.EnableSecurityCheck = New System.Windows.Forms.CheckBox()
            Me.ExecutorSlotsLabel = New System.Windows.Forms.Label()
            Me.ExecutorSlotsText = New System.Windows.Forms.TextBox()
            Me.ServiceTimer = New System.Windows.Forms.Timer(Me.components)
            Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
            Me.SecurityGroup = New System.Windows.Forms.GroupBox()
            Me.ListView1 = New System.Windows.Forms.ListView()
            Me.SecurityDesktopNameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.SecurityUserNameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.SecurityPasswordHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
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
            CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SecurityGroup.SuspendLayout()
            Me.SuspendLayout()
            '
            'MenuStrip
            '
            Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileMenu, Me.HelpMenu})
            Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
            Me.MenuStrip.Name = "MenuStrip"
            Me.MenuStrip.Size = New System.Drawing.Size(782, 28)
            Me.MenuStrip.TabIndex = 0
            '
            'FileMenu
            '
            Me.FileMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitMenu})
            Me.FileMenu.Name = "FileMenu"
            Me.FileMenu.Size = New System.Drawing.Size(69, 24)
            Me.FileMenu.Text = "文件(&F)"
            '
            'ExitMenu
            '
            Me.ExitMenu.Name = "ExitMenu"
            Me.ExitMenu.Size = New System.Drawing.Size(128, 24)
            Me.ExitMenu.Text = "退出(&X)"
            '
            'HelpMenu
            '
            Me.HelpMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutMenu})
            Me.HelpMenu.Name = "HelpMenu"
            Me.HelpMenu.Size = New System.Drawing.Size(73, 24)
            Me.HelpMenu.Text = "帮助(&H)"
            '
            'AboutMenu
            '
            Me.AboutMenu.Name = "AboutMenu"
            Me.AboutMenu.Size = New System.Drawing.Size(129, 24)
            Me.AboutMenu.Text = "关于(&A)"
            '
            'StatusStrip
            '
            Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel})
            Me.StatusStrip.Location = New System.Drawing.Point(0, 533)
            Me.StatusStrip.Name = "StatusStrip"
            Me.StatusStrip.Size = New System.Drawing.Size(782, 22)
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
            Me.SplitContainer.Location = New System.Drawing.Point(0, 28)
            Me.SplitContainer.Name = "SplitContainer"
            '
            'SplitContainer.Panel1
            '
            Me.SplitContainer.Panel1.Controls.Add(Me.NavigationTree)
            '
            'SplitContainer.Panel2
            '
            Me.SplitContainer.Panel2.Controls.Add(Me.TabControl)
            Me.SplitContainer.Size = New System.Drawing.Size(782, 505)
            Me.SplitContainer.SplitterDistance = 194
            Me.SplitContainer.TabIndex = 3
            '
            'NavigationTree
            '
            Me.NavigationTree.Dock = System.Windows.Forms.DockStyle.Fill
            Me.NavigationTree.Location = New System.Drawing.Point(0, 0)
            Me.NavigationTree.Name = "NavigationTree"
            TreeNode9.Name = "Compiler"
            TreeNode9.Text = "编译器映射"
            TreeNode10.Name = "TestSuite"
            TreeNode10.Text = "数据集映射"
            TreeNode11.Name = "Executor"
            TreeNode11.Text = "执行设置"
            TreeNode12.Name = "Root"
            TreeNode12.Text = "VijosNT"
            Me.NavigationTree.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode12})
            Me.NavigationTree.Size = New System.Drawing.Size(194, 505)
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
            Me.TabControl.Name = "TabControl"
            Me.TabControl.SelectedIndex = 0
            Me.TabControl.Size = New System.Drawing.Size(584, 505)
            Me.TabControl.TabIndex = 0
            '
            'RootPage
            '
            Me.RootPage.Controls.Add(Me.UninstallButton)
            Me.RootPage.Controls.Add(Me.InstallButton)
            Me.RootPage.Location = New System.Drawing.Point(4, 25)
            Me.RootPage.Name = "RootPage"
            Me.RootPage.Padding = New System.Windows.Forms.Padding(3)
            Me.RootPage.Size = New System.Drawing.Size(576, 476)
            Me.RootPage.TabIndex = 0
            Me.RootPage.Text = "服务"
            Me.RootPage.UseVisualStyleBackColor = True
            '
            'UninstallButton
            '
            Me.UninstallButton.Location = New System.Drawing.Point(39, 72)
            Me.UninstallButton.Name = "UninstallButton"
            Me.UninstallButton.Size = New System.Drawing.Size(121, 23)
            Me.UninstallButton.TabIndex = 1
            Me.UninstallButton.Text = "卸载服务(&U)"
            Me.UninstallButton.UseVisualStyleBackColor = True
            '
            'InstallButton
            '
            Me.InstallButton.Location = New System.Drawing.Point(39, 43)
            Me.InstallButton.Name = "InstallButton"
            Me.InstallButton.Size = New System.Drawing.Size(121, 23)
            Me.InstallButton.TabIndex = 0
            Me.InstallButton.Text = "安装服务(&I)"
            Me.InstallButton.UseVisualStyleBackColor = True
            '
            'CompilerPage
            '
            Me.CompilerPage.Controls.Add(Me.CompilerSplit)
            Me.CompilerPage.Location = New System.Drawing.Point(4, 25)
            Me.CompilerPage.Name = "CompilerPage"
            Me.CompilerPage.Padding = New System.Windows.Forms.Padding(3)
            Me.CompilerPage.Size = New System.Drawing.Size(576, 476)
            Me.CompilerPage.TabIndex = 1
            Me.CompilerPage.Text = "编译器映射"
            Me.CompilerPage.UseVisualStyleBackColor = True
            '
            'CompilerSplit
            '
            Me.CompilerSplit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CompilerSplit.Location = New System.Drawing.Point(3, 3)
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
            Me.CompilerSplit.Size = New System.Drawing.Size(570, 470)
            Me.CompilerSplit.SplitterDistance = 133
            Me.CompilerSplit.TabIndex = 6
            '
            'MoveDownCompilerButton
            '
            Me.MoveDownCompilerButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.MoveDownCompilerButton.Enabled = False
            Me.MoveDownCompilerButton.Location = New System.Drawing.Point(543, 33)
            Me.MoveDownCompilerButton.Name = "MoveDownCompilerButton"
            Me.MoveDownCompilerButton.Size = New System.Drawing.Size(24, 24)
            Me.MoveDownCompilerButton.TabIndex = 6
            Me.MoveDownCompilerButton.Text = "↓"
            Me.MoveDownCompilerButton.UseVisualStyleBackColor = True
            '
            'MoveUpCompilerButton
            '
            Me.MoveUpCompilerButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.MoveUpCompilerButton.Enabled = False
            Me.MoveUpCompilerButton.Location = New System.Drawing.Point(543, 3)
            Me.MoveUpCompilerButton.Name = "MoveUpCompilerButton"
            Me.MoveUpCompilerButton.Size = New System.Drawing.Size(24, 24)
            Me.MoveUpCompilerButton.TabIndex = 5
            Me.MoveUpCompilerButton.Text = "↑"
            Me.MoveUpCompilerButton.UseVisualStyleBackColor = True
            '
            'RemoveCompilerButton
            '
            Me.RemoveCompilerButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.RemoveCompilerButton.Enabled = False
            Me.RemoveCompilerButton.Location = New System.Drawing.Point(89, 106)
            Me.RemoveCompilerButton.Name = "RemoveCompilerButton"
            Me.RemoveCompilerButton.Size = New System.Drawing.Size(80, 24)
            Me.RemoveCompilerButton.TabIndex = 4
            Me.RemoveCompilerButton.Text = "移除(&R)"
            Me.RemoveCompilerButton.UseVisualStyleBackColor = True
            '
            'AddCompilerButton
            '
            Me.AddCompilerButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.AddCompilerButton.Location = New System.Drawing.Point(3, 106)
            Me.AddCompilerButton.Name = "AddCompilerButton"
            Me.AddCompilerButton.Size = New System.Drawing.Size(80, 24)
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
            Me.CompilerList.Location = New System.Drawing.Point(3, 3)
            Me.CompilerList.MultiSelect = False
            Me.CompilerList.Name = "CompilerList"
            Me.CompilerList.Size = New System.Drawing.Size(534, 97)
            Me.CompilerList.TabIndex = 1
            Me.CompilerList.UseCompatibleStateImageBehavior = False
            Me.CompilerList.View = System.Windows.Forms.View.Details
            '
            'CompilerPatternHeader
            '
            Me.CompilerPatternHeader.Text = "扩展名匹配"
            Me.CompilerPatternHeader.Width = 107
            '
            'CompilerCommandLineHeader
            '
            Me.CompilerCommandLineHeader.Text = "命令行"
            Me.CompilerCommandLineHeader.Width = 377
            '
            'CompilerProperty
            '
            Me.CompilerProperty.BackColor = System.Drawing.SystemColors.Window
            Me.CompilerProperty.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CompilerProperty.Location = New System.Drawing.Point(0, 0)
            Me.CompilerProperty.Name = "CompilerProperty"
            Me.CompilerProperty.PropertySort = System.Windows.Forms.PropertySort.Categorized
            Me.CompilerProperty.Size = New System.Drawing.Size(570, 333)
            Me.CompilerProperty.TabIndex = 6
            Me.CompilerProperty.ToolbarVisible = False
            '
            'TestSuitePage
            '
            Me.TestSuitePage.Controls.Add(Me.TestSuiteSplit)
            Me.TestSuitePage.Location = New System.Drawing.Point(4, 25)
            Me.TestSuitePage.Name = "TestSuitePage"
            Me.TestSuitePage.Padding = New System.Windows.Forms.Padding(3)
            Me.TestSuitePage.Size = New System.Drawing.Size(576, 476)
            Me.TestSuitePage.TabIndex = 2
            Me.TestSuitePage.Text = "数据集映射"
            Me.TestSuitePage.UseVisualStyleBackColor = True
            '
            'TestSuiteSplit
            '
            Me.TestSuiteSplit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TestSuiteSplit.Location = New System.Drawing.Point(3, 3)
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
            Me.TestSuiteSplit.Size = New System.Drawing.Size(570, 470)
            Me.TestSuiteSplit.SplitterDistance = 209
            Me.TestSuiteSplit.TabIndex = 7
            '
            'MoveDownTestSuiteButton
            '
            Me.MoveDownTestSuiteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.MoveDownTestSuiteButton.Enabled = False
            Me.MoveDownTestSuiteButton.Location = New System.Drawing.Point(543, 33)
            Me.MoveDownTestSuiteButton.Name = "MoveDownTestSuiteButton"
            Me.MoveDownTestSuiteButton.Size = New System.Drawing.Size(24, 24)
            Me.MoveDownTestSuiteButton.TabIndex = 6
            Me.MoveDownTestSuiteButton.Text = "↓"
            Me.MoveDownTestSuiteButton.UseVisualStyleBackColor = True
            '
            'MoveUpTestSuiteButton
            '
            Me.MoveUpTestSuiteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.MoveUpTestSuiteButton.Enabled = False
            Me.MoveUpTestSuiteButton.Location = New System.Drawing.Point(543, 3)
            Me.MoveUpTestSuiteButton.Name = "MoveUpTestSuiteButton"
            Me.MoveUpTestSuiteButton.Size = New System.Drawing.Size(24, 24)
            Me.MoveUpTestSuiteButton.TabIndex = 5
            Me.MoveUpTestSuiteButton.Text = "↑"
            Me.MoveUpTestSuiteButton.UseVisualStyleBackColor = True
            '
            'RemoveTestSuiteButton
            '
            Me.RemoveTestSuiteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.RemoveTestSuiteButton.Enabled = False
            Me.RemoveTestSuiteButton.Location = New System.Drawing.Point(89, 182)
            Me.RemoveTestSuiteButton.Name = "RemoveTestSuiteButton"
            Me.RemoveTestSuiteButton.Size = New System.Drawing.Size(80, 24)
            Me.RemoveTestSuiteButton.TabIndex = 4
            Me.RemoveTestSuiteButton.Text = "移除(&R)"
            Me.RemoveTestSuiteButton.UseVisualStyleBackColor = True
            '
            'AddTestSuiteButton
            '
            Me.AddTestSuiteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.AddTestSuiteButton.Location = New System.Drawing.Point(3, 182)
            Me.AddTestSuiteButton.Name = "AddTestSuiteButton"
            Me.AddTestSuiteButton.Size = New System.Drawing.Size(80, 24)
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
            Me.TestSuiteList.Location = New System.Drawing.Point(3, 3)
            Me.TestSuiteList.MultiSelect = False
            Me.TestSuiteList.Name = "TestSuiteList"
            Me.TestSuiteList.Size = New System.Drawing.Size(534, 173)
            Me.TestSuiteList.TabIndex = 1
            Me.TestSuiteList.UseCompatibleStateImageBehavior = False
            Me.TestSuiteList.View = System.Windows.Forms.View.Details
            '
            'TestSuitePatternHeader
            '
            Me.TestSuitePatternHeader.Text = "文件名匹配"
            Me.TestSuitePatternHeader.Width = 107
            '
            'TestSuiteClassHeader
            '
            Me.TestSuiteClassHeader.Text = "类型"
            Me.TestSuiteClassHeader.Width = 93
            '
            'TestSuiteParameterHeader
            '
            Me.TestSuiteParameterHeader.Text = "参数"
            Me.TestSuiteParameterHeader.Width = 285
            '
            'TestSuiteProperty
            '
            Me.TestSuiteProperty.BackColor = System.Drawing.SystemColors.Window
            Me.TestSuiteProperty.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TestSuiteProperty.Location = New System.Drawing.Point(0, 0)
            Me.TestSuiteProperty.Name = "TestSuiteProperty"
            Me.TestSuiteProperty.PropertySort = System.Windows.Forms.PropertySort.Categorized
            Me.TestSuiteProperty.Size = New System.Drawing.Size(570, 257)
            Me.TestSuiteProperty.TabIndex = 6
            Me.TestSuiteProperty.ToolbarVisible = False
            '
            'ExecutorPage
            '
            Me.ExecutorPage.Controls.Add(Me.SecurityGroup)
            Me.ExecutorPage.Controls.Add(Me.EnableSecurityCheck)
            Me.ExecutorPage.Controls.Add(Me.ExecutorSlotsLabel)
            Me.ExecutorPage.Controls.Add(Me.ExecutorSlotsText)
            Me.ExecutorPage.Location = New System.Drawing.Point(4, 25)
            Me.ExecutorPage.Name = "ExecutorPage"
            Me.ExecutorPage.Padding = New System.Windows.Forms.Padding(3)
            Me.ExecutorPage.Size = New System.Drawing.Size(576, 476)
            Me.ExecutorPage.TabIndex = 3
            Me.ExecutorPage.Text = "执行设置"
            Me.ExecutorPage.UseVisualStyleBackColor = True
            '
            'EnableSecurityCheck
            '
            Me.EnableSecurityCheck.AutoSize = True
            Me.EnableSecurityCheck.Location = New System.Drawing.Point(41, 83)
            Me.EnableSecurityCheck.Name = "EnableSecurityCheck"
            Me.EnableSecurityCheck.Size = New System.Drawing.Size(89, 19)
            Me.EnableSecurityCheck.TabIndex = 2
            Me.EnableSecurityCheck.Text = "不要点我"
            Me.EnableSecurityCheck.UseVisualStyleBackColor = True
            '
            'ExecutorSlotsLabel
            '
            Me.ExecutorSlotsLabel.AutoSize = True
            Me.ExecutorSlotsLabel.Location = New System.Drawing.Point(38, 39)
            Me.ExecutorSlotsLabel.Name = "ExecutorSlotsLabel"
            Me.ExecutorSlotsLabel.Size = New System.Drawing.Size(150, 15)
            Me.ExecutorSlotsLabel.TabIndex = 1
            Me.ExecutorSlotsLabel.Text = "同时进行的任务个数:"
            '
            'ExecutorSlotsText
            '
            Me.ExecutorSlotsText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ExecutorSlotsText.Location = New System.Drawing.Point(194, 36)
            Me.ExecutorSlotsText.Name = "ExecutorSlotsText"
            Me.ExecutorSlotsText.Size = New System.Drawing.Size(329, 25)
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
            'SecurityGroup
            '
            Me.SecurityGroup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.SecurityGroup.Controls.Add(Me.ListView1)
            Me.SecurityGroup.Enabled = False
            Me.SecurityGroup.Location = New System.Drawing.Point(41, 126)
            Me.SecurityGroup.Name = "SecurityGroup"
            Me.SecurityGroup.Size = New System.Drawing.Size(482, 316)
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
            Me.ListView1.Location = New System.Drawing.Point(3, 21)
            Me.ListView1.MultiSelect = False
            Me.ListView1.Name = "ListView1"
            Me.ListView1.Size = New System.Drawing.Size(476, 292)
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
            'ConsoleForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(782, 555)
            Me.Controls.Add(Me.SplitContainer)
            Me.Controls.Add(Me.StatusStrip)
            Me.Controls.Add(Me.MenuStrip)
            Me.MainMenuStrip = Me.MenuStrip
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
            CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
            Me.SecurityGroup.ResumeLayout(False)
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
    End Class
End Namespace
