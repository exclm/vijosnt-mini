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
            Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("安全设置")
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
            Me.CompilerList = New System.Windows.Forms.ListView()
            Me.PatternHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.CompilerProperty = New System.Windows.Forms.PropertyGrid()
            Me.MoveDownCompilerButton = New System.Windows.Forms.Button()
            Me.MoveUpCompilerButton = New System.Windows.Forms.Button()
            Me.RemoveCompilerButton = New System.Windows.Forms.Button()
            Me.AddCompilerButton = New System.Windows.Forms.Button()
            Me.TestSuitePage = New System.Windows.Forms.TabPage()
            Me.SecurityPage = New System.Windows.Forms.TabPage()
            Me.ServiceTimer = New System.Windows.Forms.Timer(Me.components)
            Me.MenuStrip.SuspendLayout()
            Me.StatusStrip.SuspendLayout()
            Me.SplitContainer.Panel1.SuspendLayout()
            Me.SplitContainer.Panel2.SuspendLayout()
            Me.SplitContainer.SuspendLayout()
            Me.TabControl.SuspendLayout()
            Me.RootPage.SuspendLayout()
            Me.CompilerPage.SuspendLayout()
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
            TreeNode1.Name = "Compiler"
            TreeNode1.Text = "编译器映射"
            TreeNode2.Name = "TestSuite"
            TreeNode2.Text = "数据集映射"
            TreeNode3.Name = "Security"
            TreeNode3.Text = "安全设置"
            TreeNode4.Name = "Root"
            TreeNode4.Text = "VijosNT"
            Me.NavigationTree.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode4})
            Me.NavigationTree.Size = New System.Drawing.Size(194, 505)
            Me.NavigationTree.TabIndex = 0
            '
            'TabControl
            '
            Me.TabControl.Controls.Add(Me.RootPage)
            Me.TabControl.Controls.Add(Me.CompilerPage)
            Me.TabControl.Controls.Add(Me.TestSuitePage)
            Me.TabControl.Controls.Add(Me.SecurityPage)
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
            Me.CompilerPage.Controls.Add(Me.CompilerList)
            Me.CompilerPage.Controls.Add(Me.CompilerProperty)
            Me.CompilerPage.Controls.Add(Me.MoveDownCompilerButton)
            Me.CompilerPage.Controls.Add(Me.MoveUpCompilerButton)
            Me.CompilerPage.Controls.Add(Me.RemoveCompilerButton)
            Me.CompilerPage.Controls.Add(Me.AddCompilerButton)
            Me.CompilerPage.Location = New System.Drawing.Point(4, 25)
            Me.CompilerPage.Name = "CompilerPage"
            Me.CompilerPage.Padding = New System.Windows.Forms.Padding(3)
            Me.CompilerPage.Size = New System.Drawing.Size(576, 476)
            Me.CompilerPage.TabIndex = 1
            Me.CompilerPage.Text = "编译器映射"
            Me.CompilerPage.UseVisualStyleBackColor = True
            '
            'CompilerList
            '
            Me.CompilerList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.CompilerList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.PatternHeader})
            Me.CompilerList.FullRowSelect = True
            Me.CompilerList.HideSelection = False
            Me.CompilerList.Location = New System.Drawing.Point(6, 6)
            Me.CompilerList.MultiSelect = False
            Me.CompilerList.Name = "CompilerList"
            Me.CompilerList.Size = New System.Drawing.Size(167, 434)
            Me.CompilerList.TabIndex = 0
            Me.CompilerList.UseCompatibleStateImageBehavior = False
            Me.CompilerList.View = System.Windows.Forms.View.Details
            '
            'PatternHeader
            '
            Me.PatternHeader.Text = "扩展名"
            Me.PatternHeader.Width = 79
            '
            'CompilerProperty
            '
            Me.CompilerProperty.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.CompilerProperty.BackColor = System.Drawing.SystemColors.Window
            Me.CompilerProperty.Location = New System.Drawing.Point(209, 6)
            Me.CompilerProperty.Name = "CompilerProperty"
            Me.CompilerProperty.Size = New System.Drawing.Size(359, 434)
            Me.CompilerProperty.TabIndex = 5
            '
            'MoveDownCompilerButton
            '
            Me.MoveDownCompilerButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.MoveDownCompilerButton.Enabled = False
            Me.MoveDownCompilerButton.Location = New System.Drawing.Point(179, 36)
            Me.MoveDownCompilerButton.Name = "MoveDownCompilerButton"
            Me.MoveDownCompilerButton.Size = New System.Drawing.Size(24, 24)
            Me.MoveDownCompilerButton.TabIndex = 4
            Me.MoveDownCompilerButton.Text = "↓"
            Me.MoveDownCompilerButton.UseVisualStyleBackColor = True
            '
            'MoveUpCompilerButton
            '
            Me.MoveUpCompilerButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.MoveUpCompilerButton.Enabled = False
            Me.MoveUpCompilerButton.Location = New System.Drawing.Point(179, 6)
            Me.MoveUpCompilerButton.Name = "MoveUpCompilerButton"
            Me.MoveUpCompilerButton.Size = New System.Drawing.Size(24, 24)
            Me.MoveUpCompilerButton.TabIndex = 3
            Me.MoveUpCompilerButton.Text = "↑"
            Me.MoveUpCompilerButton.UseVisualStyleBackColor = True
            '
            'RemoveCompilerButton
            '
            Me.RemoveCompilerButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.RemoveCompilerButton.Enabled = False
            Me.RemoveCompilerButton.Location = New System.Drawing.Point(93, 446)
            Me.RemoveCompilerButton.Name = "RemoveCompilerButton"
            Me.RemoveCompilerButton.Size = New System.Drawing.Size(80, 24)
            Me.RemoveCompilerButton.TabIndex = 2
            Me.RemoveCompilerButton.Text = "移除(&R)"
            Me.RemoveCompilerButton.UseVisualStyleBackColor = True
            '
            'AddCompilerButton
            '
            Me.AddCompilerButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.AddCompilerButton.Location = New System.Drawing.Point(6, 446)
            Me.AddCompilerButton.Name = "AddCompilerButton"
            Me.AddCompilerButton.Size = New System.Drawing.Size(80, 24)
            Me.AddCompilerButton.TabIndex = 1
            Me.AddCompilerButton.Text = "添加(&A)"
            Me.AddCompilerButton.UseVisualStyleBackColor = True
            '
            'TestSuitePage
            '
            Me.TestSuitePage.Location = New System.Drawing.Point(4, 25)
            Me.TestSuitePage.Name = "TestSuitePage"
            Me.TestSuitePage.Padding = New System.Windows.Forms.Padding(3)
            Me.TestSuitePage.Size = New System.Drawing.Size(576, 476)
            Me.TestSuitePage.TabIndex = 2
            Me.TestSuitePage.Text = "数据集映射"
            Me.TestSuitePage.UseVisualStyleBackColor = True
            '
            'SecurityPage
            '
            Me.SecurityPage.Location = New System.Drawing.Point(4, 25)
            Me.SecurityPage.Name = "SecurityPage"
            Me.SecurityPage.Padding = New System.Windows.Forms.Padding(3)
            Me.SecurityPage.Size = New System.Drawing.Size(576, 476)
            Me.SecurityPage.TabIndex = 3
            Me.SecurityPage.Text = "安全设置"
            Me.SecurityPage.UseVisualStyleBackColor = True
            '
            'ServiceTimer
            '
            Me.ServiceTimer.Interval = 500
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
        Friend WithEvents SecurityPage As System.Windows.Forms.TabPage
        Friend WithEvents InstallButton As System.Windows.Forms.Button
        Friend WithEvents UninstallButton As System.Windows.Forms.Button
        Friend WithEvents StatusLabel As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents ServiceTimer As System.Windows.Forms.Timer
        Friend WithEvents CompilerProperty As System.Windows.Forms.PropertyGrid
        Friend WithEvents MoveDownCompilerButton As System.Windows.Forms.Button
        Friend WithEvents MoveUpCompilerButton As System.Windows.Forms.Button
        Friend WithEvents RemoveCompilerButton As System.Windows.Forms.Button
        Friend WithEvents AddCompilerButton As System.Windows.Forms.Button
        Friend WithEvents CompilerList As System.Windows.Forms.ListView
        Friend WithEvents PatternHeader As System.Windows.Forms.ColumnHeader
    End Class
End Namespace
