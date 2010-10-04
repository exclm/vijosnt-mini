Namespace Foreground
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class VijosContest
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
            Me.scTest = New System.Windows.Forms.SplitContainer()
            Me.btnSelectUntested = New System.Windows.Forms.Button()
            Me.lblTestID = New System.Windows.Forms.Label()
            Me.btnSelectNone = New System.Windows.Forms.Button()
            Me.txtTestID = New System.Windows.Forms.TextBox()
            Me.btnSelectAll = New System.Windows.Forms.Button()
            Me.btnDisplay = New System.Windows.Forms.Button()
            Me.btnStart = New System.Windows.Forms.Button()
            Me.lvTesting = New System.Windows.Forms.ListView()
            Me.chTsID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chTsUser = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chTsScore = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chTsTime = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chTsCompiler = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chTsDone = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.btnClearRecord = New System.Windows.Forms.Button()
            Me.btnDisplayRecord = New System.Windows.Forms.Button()
            Me.lvRecord = New System.Windows.Forms.ListView()
            Me.chRecID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chRecUser = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chRecProblem = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chRecCompiler = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chRecStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chRecScore = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chRecTime = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.tabMain = New System.Windows.Forms.TabControl()
            Me.tpCompiler = New System.Windows.Forms.TabPage()
            Me.pnlCompiler = New System.Windows.Forms.Panel()
            Me.btnClearCompiler = New System.Windows.Forms.Button()
            Me.btnDisplayCompiler = New System.Windows.Forms.Button()
            Me.lvCompiler = New System.Windows.Forms.ListView()
            Me.chClID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chClCompiler = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chClStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chClScore = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chClTime = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.btnTestCompiler = New System.Windows.Forms.Button()
            Me.txtProblemID = New System.Windows.Forms.TextBox()
            Me.lblProblemID = New System.Windows.Forms.Label()
            Me.tpContest = New System.Windows.Forms.TabPage()
            Me.pnlContest = New System.Windows.Forms.Panel()
            Me.btnMoveTo = New System.Windows.Forms.Button()
            Me.lvContest = New System.Windows.Forms.ListView()
            Me.chCtID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chCtName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chCtDone = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.btnRefresh = New System.Windows.Forms.Button()
            Me.cbDisplayTested = New System.Windows.Forms.CheckBox()
            Me.cbDisplayLive = New System.Windows.Forms.CheckBox()
            Me.cbAutoRefresh = New System.Windows.Forms.CheckBox()
            Me.tpTest = New System.Windows.Forms.TabPage()
            Me.tmrAutoRefresh = New System.Windows.Forms.Timer(Me.components)
            Me.scTest.Panel1.SuspendLayout()
            Me.scTest.Panel2.SuspendLayout()
            Me.scTest.SuspendLayout()
            Me.tabMain.SuspendLayout()
            Me.tpCompiler.SuspendLayout()
            Me.pnlCompiler.SuspendLayout()
            Me.tpContest.SuspendLayout()
            Me.pnlContest.SuspendLayout()
            Me.tpTest.SuspendLayout()
            Me.SuspendLayout()
            '
            'scTest
            '
            Me.scTest.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.scTest.Location = New System.Drawing.Point(6, 6)
            Me.scTest.Name = "scTest"
            Me.scTest.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'scTest.Panel1
            '
            Me.scTest.Panel1.Controls.Add(Me.btnSelectUntested)
            Me.scTest.Panel1.Controls.Add(Me.lblTestID)
            Me.scTest.Panel1.Controls.Add(Me.btnSelectNone)
            Me.scTest.Panel1.Controls.Add(Me.txtTestID)
            Me.scTest.Panel1.Controls.Add(Me.btnSelectAll)
            Me.scTest.Panel1.Controls.Add(Me.btnDisplay)
            Me.scTest.Panel1.Controls.Add(Me.btnStart)
            Me.scTest.Panel1.Controls.Add(Me.lvTesting)
            '
            'scTest.Panel2
            '
            Me.scTest.Panel2.Controls.Add(Me.btnClearRecord)
            Me.scTest.Panel2.Controls.Add(Me.btnDisplayRecord)
            Me.scTest.Panel2.Controls.Add(Me.lvRecord)
            Me.scTest.Size = New System.Drawing.Size(513, 430)
            Me.scTest.SplitterDistance = 235
            Me.scTest.TabIndex = 1
            '
            'btnSelectUntested
            '
            Me.btnSelectUntested.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnSelectUntested.Location = New System.Drawing.Point(178, 209)
            Me.btnSelectUntested.Name = "btnSelectUntested"
            Me.btnSelectUntested.Size = New System.Drawing.Size(121, 23)
            Me.btnSelectUntested.TabIndex = 25
            Me.btnSelectUntested.Text = "选中未评测用户(&U)"
            Me.btnSelectUntested.UseVisualStyleBackColor = True
            '
            'lblTestID
            '
            Me.lblTestID.AutoSize = True
            Me.lblTestID.Location = New System.Drawing.Point(14, 18)
            Me.lblTestID.Name = "lblTestID"
            Me.lblTestID.Size = New System.Drawing.Size(47, 12)
            Me.lblTestID.TabIndex = 17
            Me.lblTestID.Text = "测试号:"
            '
            'btnSelectNone
            '
            Me.btnSelectNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnSelectNone.Location = New System.Drawing.Point(97, 209)
            Me.btnSelectNone.Name = "btnSelectNone"
            Me.btnSelectNone.Size = New System.Drawing.Size(75, 23)
            Me.btnSelectNone.TabIndex = 24
            Me.btnSelectNone.Text = "全不选(&N)"
            Me.btnSelectNone.UseVisualStyleBackColor = True
            '
            'txtTestID
            '
            Me.txtTestID.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtTestID.Location = New System.Drawing.Point(67, 15)
            Me.txtTestID.Name = "txtTestID"
            Me.txtTestID.Size = New System.Drawing.Size(348, 21)
            Me.txtTestID.TabIndex = 18
            Me.txtTestID.Text = "T1000"
            '
            'btnSelectAll
            '
            Me.btnSelectAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnSelectAll.Location = New System.Drawing.Point(16, 209)
            Me.btnSelectAll.Name = "btnSelectAll"
            Me.btnSelectAll.Size = New System.Drawing.Size(75, 23)
            Me.btnSelectAll.TabIndex = 23
            Me.btnSelectAll.Text = "全选(&A)"
            Me.btnSelectAll.UseVisualStyleBackColor = True
            '
            'btnDisplay
            '
            Me.btnDisplay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnDisplay.Location = New System.Drawing.Point(421, 13)
            Me.btnDisplay.Name = "btnDisplay"
            Me.btnDisplay.Size = New System.Drawing.Size(75, 23)
            Me.btnDisplay.TabIndex = 19
            Me.btnDisplay.Text = "显示(&D)"
            Me.btnDisplay.UseVisualStyleBackColor = True
            '
            'btnStart
            '
            Me.btnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnStart.Location = New System.Drawing.Point(421, 209)
            Me.btnStart.Name = "btnStart"
            Me.btnStart.Size = New System.Drawing.Size(75, 23)
            Me.btnStart.TabIndex = 22
            Me.btnStart.Text = "开始(&S)"
            Me.btnStart.UseVisualStyleBackColor = True
            '
            'lvTesting
            '
            Me.lvTesting.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lvTesting.CheckBoxes = True
            Me.lvTesting.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chTsID, Me.chTsUser, Me.chTsScore, Me.chTsTime, Me.chTsCompiler, Me.chTsDone})
            Me.lvTesting.FullRowSelect = True
            Me.lvTesting.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lvTesting.HideSelection = False
            Me.lvTesting.Location = New System.Drawing.Point(16, 42)
            Me.lvTesting.MultiSelect = False
            Me.lvTesting.Name = "lvTesting"
            Me.lvTesting.Size = New System.Drawing.Size(480, 161)
            Me.lvTesting.TabIndex = 20
            Me.lvTesting.UseCompatibleStateImageBehavior = False
            Me.lvTesting.View = System.Windows.Forms.View.Details
            '
            'chTsID
            '
            Me.chTsID.Text = "ID"
            Me.chTsID.Width = 55
            '
            'chTsUser
            '
            Me.chTsUser.Text = "用户"
            Me.chTsUser.Width = 83
            '
            'chTsScore
            '
            Me.chTsScore.Text = "分数"
            Me.chTsScore.Width = 55
            '
            'chTsTime
            '
            Me.chTsTime.Text = "有效耗时"
            Me.chTsTime.Width = 69
            '
            'chTsCompiler
            '
            Me.chTsCompiler.Text = "编译器"
            Me.chTsCompiler.Width = 58
            '
            'chTsDone
            '
            Me.chTsDone.Text = "已评测"
            Me.chTsDone.Width = 55
            '
            'btnClearRecord
            '
            Me.btnClearRecord.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnClearRecord.Location = New System.Drawing.Point(340, 156)
            Me.btnClearRecord.Name = "btnClearRecord"
            Me.btnClearRecord.Size = New System.Drawing.Size(75, 23)
            Me.btnClearRecord.TabIndex = 27
            Me.btnClearRecord.Text = "清除(&C)"
            Me.btnClearRecord.UseVisualStyleBackColor = True
            '
            'btnDisplayRecord
            '
            Me.btnDisplayRecord.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnDisplayRecord.Location = New System.Drawing.Point(421, 156)
            Me.btnDisplayRecord.Name = "btnDisplayRecord"
            Me.btnDisplayRecord.Size = New System.Drawing.Size(75, 23)
            Me.btnDisplayRecord.TabIndex = 26
            Me.btnDisplayRecord.Text = "显示(&D)"
            Me.btnDisplayRecord.UseVisualStyleBackColor = True
            '
            'lvRecord
            '
            Me.lvRecord.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lvRecord.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chRecID, Me.chRecUser, Me.chRecProblem, Me.chRecCompiler, Me.chRecStatus, Me.chRecScore, Me.chRecTime})
            Me.lvRecord.FullRowSelect = True
            Me.lvRecord.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lvRecord.HideSelection = False
            Me.lvRecord.Location = New System.Drawing.Point(16, 3)
            Me.lvRecord.MultiSelect = False
            Me.lvRecord.Name = "lvRecord"
            Me.lvRecord.Size = New System.Drawing.Size(480, 147)
            Me.lvRecord.TabIndex = 21
            Me.lvRecord.UseCompatibleStateImageBehavior = False
            Me.lvRecord.View = System.Windows.Forms.View.Details
            '
            'chRecID
            '
            Me.chRecID.Text = "ID"
            Me.chRecID.Width = 49
            '
            'chRecUser
            '
            Me.chRecUser.Text = "用户"
            Me.chRecUser.Width = 81
            '
            'chRecProblem
            '
            Me.chRecProblem.Text = "题目"
            Me.chRecProblem.Width = 57
            '
            'chRecCompiler
            '
            Me.chRecCompiler.Text = "编译器"
            Me.chRecCompiler.Width = 61
            '
            'chRecStatus
            '
            Me.chRecStatus.Text = "状态"
            Me.chRecStatus.Width = 106
            '
            'chRecScore
            '
            Me.chRecScore.Text = "分数"
            Me.chRecScore.Width = 51
            '
            'chRecTime
            '
            Me.chRecTime.Text = "有效耗时"
            Me.chRecTime.Width = 69
            '
            'tabMain
            '
            Me.tabMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tabMain.Controls.Add(Me.tpCompiler)
            Me.tabMain.Controls.Add(Me.tpContest)
            Me.tabMain.Controls.Add(Me.tpTest)
            Me.tabMain.Location = New System.Drawing.Point(12, 12)
            Me.tabMain.Name = "tabMain"
            Me.tabMain.SelectedIndex = 0
            Me.tabMain.Size = New System.Drawing.Size(533, 468)
            Me.tabMain.TabIndex = 1
            '
            'tpCompiler
            '
            Me.tpCompiler.Controls.Add(Me.pnlCompiler)
            Me.tpCompiler.Location = New System.Drawing.Point(4, 22)
            Me.tpCompiler.Name = "tpCompiler"
            Me.tpCompiler.Padding = New System.Windows.Forms.Padding(3)
            Me.tpCompiler.Size = New System.Drawing.Size(525, 442)
            Me.tpCompiler.TabIndex = 1
            Me.tpCompiler.Text = "编译器"
            Me.tpCompiler.UseVisualStyleBackColor = True
            '
            'pnlCompiler
            '
            Me.pnlCompiler.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pnlCompiler.Controls.Add(Me.btnClearCompiler)
            Me.pnlCompiler.Controls.Add(Me.btnDisplayCompiler)
            Me.pnlCompiler.Controls.Add(Me.lvCompiler)
            Me.pnlCompiler.Controls.Add(Me.btnTestCompiler)
            Me.pnlCompiler.Controls.Add(Me.txtProblemID)
            Me.pnlCompiler.Controls.Add(Me.lblProblemID)
            Me.pnlCompiler.Location = New System.Drawing.Point(6, 6)
            Me.pnlCompiler.Name = "pnlCompiler"
            Me.pnlCompiler.Size = New System.Drawing.Size(513, 430)
            Me.pnlCompiler.TabIndex = 0
            '
            'btnClearCompiler
            '
            Me.btnClearCompiler.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnClearCompiler.Location = New System.Drawing.Point(339, 395)
            Me.btnClearCompiler.Name = "btnClearCompiler"
            Me.btnClearCompiler.Size = New System.Drawing.Size(75, 23)
            Me.btnClearCompiler.TabIndex = 20
            Me.btnClearCompiler.Text = "清除(&C)"
            Me.btnClearCompiler.UseVisualStyleBackColor = True
            '
            'btnDisplayCompiler
            '
            Me.btnDisplayCompiler.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnDisplayCompiler.Location = New System.Drawing.Point(420, 395)
            Me.btnDisplayCompiler.Name = "btnDisplayCompiler"
            Me.btnDisplayCompiler.Size = New System.Drawing.Size(75, 23)
            Me.btnDisplayCompiler.TabIndex = 19
            Me.btnDisplayCompiler.Text = "显示(&D)"
            Me.btnDisplayCompiler.UseVisualStyleBackColor = True
            '
            'lvCompiler
            '
            Me.lvCompiler.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lvCompiler.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chClID, Me.chClCompiler, Me.chClStatus, Me.chClScore, Me.chClTime})
            Me.lvCompiler.FullRowSelect = True
            Me.lvCompiler.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lvCompiler.HideSelection = False
            Me.lvCompiler.Location = New System.Drawing.Point(23, 45)
            Me.lvCompiler.MultiSelect = False
            Me.lvCompiler.Name = "lvCompiler"
            Me.lvCompiler.Size = New System.Drawing.Size(472, 344)
            Me.lvCompiler.TabIndex = 18
            Me.lvCompiler.UseCompatibleStateImageBehavior = False
            Me.lvCompiler.View = System.Windows.Forms.View.Details
            '
            'chClID
            '
            Me.chClID.Text = "ID"
            Me.chClID.Width = 53
            '
            'chClCompiler
            '
            Me.chClCompiler.Text = "编译器"
            Me.chClCompiler.Width = 94
            '
            'chClStatus
            '
            Me.chClStatus.Text = "状态"
            Me.chClStatus.Width = 124
            '
            'chClScore
            '
            Me.chClScore.Text = "分数"
            Me.chClScore.Width = 54
            '
            'chClTime
            '
            Me.chClTime.Text = "有效耗时"
            Me.chClTime.Width = 71
            '
            'btnTestCompiler
            '
            Me.btnTestCompiler.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnTestCompiler.Location = New System.Drawing.Point(420, 16)
            Me.btnTestCompiler.Name = "btnTestCompiler"
            Me.btnTestCompiler.Size = New System.Drawing.Size(75, 23)
            Me.btnTestCompiler.TabIndex = 17
            Me.btnTestCompiler.Text = "测试(&T)"
            Me.btnTestCompiler.UseVisualStyleBackColor = True
            '
            'txtProblemID
            '
            Me.txtProblemID.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtProblemID.Location = New System.Drawing.Point(86, 18)
            Me.txtProblemID.Name = "txtProblemID"
            Me.txtProblemID.Size = New System.Drawing.Size(318, 21)
            Me.txtProblemID.TabIndex = 16
            Me.txtProblemID.Text = "P1000"
            '
            'lblProblemID
            '
            Me.lblProblemID.AutoSize = True
            Me.lblProblemID.Location = New System.Drawing.Point(21, 21)
            Me.lblProblemID.Name = "lblProblemID"
            Me.lblProblemID.Size = New System.Drawing.Size(59, 12)
            Me.lblProblemID.TabIndex = 15
            Me.lblProblemID.Text = "A+B 题号:"
            '
            'tpContest
            '
            Me.tpContest.Controls.Add(Me.pnlContest)
            Me.tpContest.Location = New System.Drawing.Point(4, 22)
            Me.tpContest.Name = "tpContest"
            Me.tpContest.Padding = New System.Windows.Forms.Padding(3)
            Me.tpContest.Size = New System.Drawing.Size(459, 414)
            Me.tpContest.TabIndex = 2
            Me.tpContest.Text = "比赛"
            Me.tpContest.UseVisualStyleBackColor = True
            '
            'pnlContest
            '
            Me.pnlContest.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pnlContest.Controls.Add(Me.btnMoveTo)
            Me.pnlContest.Controls.Add(Me.lvContest)
            Me.pnlContest.Controls.Add(Me.btnRefresh)
            Me.pnlContest.Controls.Add(Me.cbDisplayTested)
            Me.pnlContest.Controls.Add(Me.cbDisplayLive)
            Me.pnlContest.Controls.Add(Me.cbAutoRefresh)
            Me.pnlContest.Location = New System.Drawing.Point(6, 6)
            Me.pnlContest.Name = "pnlContest"
            Me.pnlContest.Size = New System.Drawing.Size(447, 402)
            Me.pnlContest.TabIndex = 0
            '
            'btnMoveTo
            '
            Me.btnMoveTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnMoveTo.Location = New System.Drawing.Point(355, 47)
            Me.btnMoveTo.Name = "btnMoveTo"
            Me.btnMoveTo.Size = New System.Drawing.Size(75, 23)
            Me.btnMoveTo.TabIndex = 5
            Me.btnMoveTo.Text = "转至(&M)"
            Me.btnMoveTo.UseVisualStyleBackColor = True
            '
            'lvContest
            '
            Me.lvContest.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lvContest.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chCtID, Me.chCtName, Me.chCtDone})
            Me.lvContest.FullRowSelect = True
            Me.lvContest.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lvContest.HideSelection = False
            Me.lvContest.Location = New System.Drawing.Point(16, 93)
            Me.lvContest.MultiSelect = False
            Me.lvContest.Name = "lvContest"
            Me.lvContest.Size = New System.Drawing.Size(414, 293)
            Me.lvContest.Sorting = System.Windows.Forms.SortOrder.Descending
            Me.lvContest.TabIndex = 4
            Me.lvContest.UseCompatibleStateImageBehavior = False
            Me.lvContest.View = System.Windows.Forms.View.Details
            '
            'chCtID
            '
            Me.chCtID.Text = "测试号"
            Me.chCtID.Width = 67
            '
            'chCtName
            '
            Me.chCtName.Text = "标题"
            Me.chCtName.Width = 177
            '
            'chCtDone
            '
            Me.chCtDone.Text = "已评测"
            Me.chCtDone.Width = 63
            '
            'btnRefresh
            '
            Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnRefresh.Location = New System.Drawing.Point(355, 18)
            Me.btnRefresh.Name = "btnRefresh"
            Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
            Me.btnRefresh.TabIndex = 3
            Me.btnRefresh.Text = "刷新(&R)"
            Me.btnRefresh.UseVisualStyleBackColor = True
            '
            'cbDisplayTested
            '
            Me.cbDisplayTested.AutoSize = True
            Me.cbDisplayTested.Location = New System.Drawing.Point(16, 62)
            Me.cbDisplayTested.Name = "cbDisplayTested"
            Me.cbDisplayTested.Size = New System.Drawing.Size(138, 16)
            Me.cbDisplayTested.TabIndex = 2
            Me.cbDisplayTested.Text = "显示评测过的比赛(&T)"
            Me.cbDisplayTested.UseVisualStyleBackColor = True
            '
            'cbDisplayLive
            '
            Me.cbDisplayLive.AutoSize = True
            Me.cbDisplayLive.Location = New System.Drawing.Point(16, 40)
            Me.cbDisplayLive.Name = "cbDisplayLive"
            Me.cbDisplayLive.Size = New System.Drawing.Size(138, 16)
            Me.cbDisplayLive.TabIndex = 1
            Me.cbDisplayLive.Text = "显示未完成的比赛(&L)"
            Me.cbDisplayLive.UseVisualStyleBackColor = True
            '
            'cbAutoRefresh
            '
            Me.cbAutoRefresh.AutoSize = True
            Me.cbAutoRefresh.Location = New System.Drawing.Point(16, 18)
            Me.cbAutoRefresh.Name = "cbAutoRefresh"
            Me.cbAutoRefresh.Size = New System.Drawing.Size(90, 16)
            Me.cbAutoRefresh.TabIndex = 0
            Me.cbAutoRefresh.Text = "自动刷新(&A)"
            Me.cbAutoRefresh.UseVisualStyleBackColor = True
            '
            'tpTest
            '
            Me.tpTest.Controls.Add(Me.scTest)
            Me.tpTest.Location = New System.Drawing.Point(4, 22)
            Me.tpTest.Name = "tpTest"
            Me.tpTest.Padding = New System.Windows.Forms.Padding(3)
            Me.tpTest.Size = New System.Drawing.Size(525, 442)
            Me.tpTest.TabIndex = 3
            Me.tpTest.Text = "评测"
            Me.tpTest.UseVisualStyleBackColor = True
            '
            'tmrAutoRefresh
            '
            Me.tmrAutoRefresh.Interval = 500
            '
            'VijosContest
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(557, 492)
            Me.Controls.Add(Me.tabMain)
            Me.Name = "VijosContest"
            Me.Text = "Vijos 比赛测试"
            Me.scTest.Panel1.ResumeLayout(False)
            Me.scTest.Panel1.PerformLayout()
            Me.scTest.Panel2.ResumeLayout(False)
            Me.scTest.ResumeLayout(False)
            Me.tabMain.ResumeLayout(False)
            Me.tpCompiler.ResumeLayout(False)
            Me.pnlCompiler.ResumeLayout(False)
            Me.pnlCompiler.PerformLayout()
            Me.tpContest.ResumeLayout(False)
            Me.pnlContest.ResumeLayout(False)
            Me.pnlContest.PerformLayout()
            Me.tpTest.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents scTest As System.Windows.Forms.SplitContainer
        Friend WithEvents btnSelectUntested As System.Windows.Forms.Button
        Friend WithEvents lblTestID As System.Windows.Forms.Label
        Friend WithEvents btnSelectNone As System.Windows.Forms.Button
        Friend WithEvents txtTestID As System.Windows.Forms.TextBox
        Friend WithEvents btnSelectAll As System.Windows.Forms.Button
        Friend WithEvents btnDisplay As System.Windows.Forms.Button
        Friend WithEvents btnStart As System.Windows.Forms.Button
        Friend WithEvents lvTesting As System.Windows.Forms.ListView
        Friend WithEvents chTsID As System.Windows.Forms.ColumnHeader
        Friend WithEvents chTsUser As System.Windows.Forms.ColumnHeader
        Friend WithEvents chTsScore As System.Windows.Forms.ColumnHeader
        Friend WithEvents chTsTime As System.Windows.Forms.ColumnHeader
        Friend WithEvents chTsCompiler As System.Windows.Forms.ColumnHeader
        Friend WithEvents chTsDone As System.Windows.Forms.ColumnHeader
        Friend WithEvents btnClearRecord As System.Windows.Forms.Button
        Friend WithEvents btnDisplayRecord As System.Windows.Forms.Button
        Friend WithEvents lvRecord As System.Windows.Forms.ListView
        Friend WithEvents chRecID As System.Windows.Forms.ColumnHeader
        Friend WithEvents chRecUser As System.Windows.Forms.ColumnHeader
        Friend WithEvents chRecProblem As System.Windows.Forms.ColumnHeader
        Friend WithEvents chRecCompiler As System.Windows.Forms.ColumnHeader
        Friend WithEvents chRecStatus As System.Windows.Forms.ColumnHeader
        Friend WithEvents chRecScore As System.Windows.Forms.ColumnHeader
        Friend WithEvents chRecTime As System.Windows.Forms.ColumnHeader
        Friend WithEvents tabMain As System.Windows.Forms.TabControl
        Friend WithEvents tpCompiler As System.Windows.Forms.TabPage
        Friend WithEvents pnlCompiler As System.Windows.Forms.Panel
        Friend WithEvents btnClearCompiler As System.Windows.Forms.Button
        Friend WithEvents btnDisplayCompiler As System.Windows.Forms.Button
        Friend WithEvents lvCompiler As System.Windows.Forms.ListView
        Friend WithEvents chClID As System.Windows.Forms.ColumnHeader
        Friend WithEvents chClCompiler As System.Windows.Forms.ColumnHeader
        Friend WithEvents chClStatus As System.Windows.Forms.ColumnHeader
        Friend WithEvents chClScore As System.Windows.Forms.ColumnHeader
        Friend WithEvents chClTime As System.Windows.Forms.ColumnHeader
        Friend WithEvents btnTestCompiler As System.Windows.Forms.Button
        Friend WithEvents txtProblemID As System.Windows.Forms.TextBox
        Friend WithEvents lblProblemID As System.Windows.Forms.Label
        Friend WithEvents tpContest As System.Windows.Forms.TabPage
        Friend WithEvents pnlContest As System.Windows.Forms.Panel
        Friend WithEvents btnMoveTo As System.Windows.Forms.Button
        Friend WithEvents lvContest As System.Windows.Forms.ListView
        Friend WithEvents chCtID As System.Windows.Forms.ColumnHeader
        Friend WithEvents chCtName As System.Windows.Forms.ColumnHeader
        Friend WithEvents chCtDone As System.Windows.Forms.ColumnHeader
        Friend WithEvents btnRefresh As System.Windows.Forms.Button
        Friend WithEvents cbDisplayTested As System.Windows.Forms.CheckBox
        Friend WithEvents cbDisplayLive As System.Windows.Forms.CheckBox
        Friend WithEvents cbAutoRefresh As System.Windows.Forms.CheckBox
        Friend WithEvents tpTest As System.Windows.Forms.TabPage
        Friend WithEvents tmrAutoRefresh As System.Windows.Forms.Timer
    End Class
End Namespace
