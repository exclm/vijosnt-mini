Namespace Foreground
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FloatingForm
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
            Me.MyContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.StartConsoleMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.ContextBar0 = New System.Windows.Forms.ToolStripSeparator()
            Me.TransparentMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.CloseMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.MyContextMenu.SuspendLayout()
            Me.SuspendLayout()
            '
            'MyContextMenu
            '
            Me.MyContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartConsoleMenu, Me.TransparentMenu, Me.ContextBar0, Me.CloseMenu})
            Me.MyContextMenu.Name = "ContextMenu"
            Me.MyContextMenu.Size = New System.Drawing.Size(155, 98)
            '
            'StartConsoleMenu
            '
            Me.StartConsoleMenu.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Bold)
            Me.StartConsoleMenu.Name = "StartConsoleMenu"
            Me.StartConsoleMenu.Size = New System.Drawing.Size(154, 22)
            Me.StartConsoleMenu.Text = "启动控制台(&C)"
            '
            'ContextBar0
            '
            Me.ContextBar0.Name = "ContextBar0"
            Me.ContextBar0.Size = New System.Drawing.Size(151, 6)
            '
            'TransparentMenu
            '
            Me.TransparentMenu.Name = "TransparentMenu"
            Me.TransparentMenu.Size = New System.Drawing.Size(154, 22)
            Me.TransparentMenu.Text = "半透明(&T)"
            '
            'CloseMenu
            '
            Me.CloseMenu.Name = "CloseMenu"
            Me.CloseMenu.Size = New System.Drawing.Size(154, 22)
            Me.CloseMenu.Text = "关闭悬浮窗(&C)"
            '
            'FloatingForm
            '
            Me.AllowDrop = True
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.ClientSize = New System.Drawing.Size(221, 199)
            Me.ContextMenuStrip = Me.MyContextMenu
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Name = "FloatingForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
            Me.TopMost = True
            Me.MyContextMenu.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents MyContextMenu As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents TransparentMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ContextBar0 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents CloseMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents StartConsoleMenu As System.Windows.Forms.ToolStripMenuItem
    End Class
End Namespace
