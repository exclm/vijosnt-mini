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
            Me.ContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.TransparentMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.ContextBar0 = New System.Windows.Forms.ToolStripSeparator()
            Me.CloseMenu = New System.Windows.Forms.ToolStripMenuItem()
            Me.ContextMenu.SuspendLayout()
            Me.SuspendLayout()
            '
            'ContextMenu
            '
            Me.ContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TransparentMenu, Me.ContextBar0, Me.CloseMenu})
            Me.ContextMenu.Name = "ContextMenu"
            Me.ContextMenu.Size = New System.Drawing.Size(153, 76)
            '
            'TransparentMenu
            '
            Me.TransparentMenu.Name = "TransparentMenu"
            Me.TransparentMenu.Size = New System.Drawing.Size(152, 22)
            Me.TransparentMenu.Text = "半透明(&T)"
            '
            'ContextBar0
            '
            Me.ContextBar0.Name = "ContextBar0"
            Me.ContextBar0.Size = New System.Drawing.Size(149, 6)
            '
            'CloseMenu
            '
            Me.CloseMenu.Name = "CloseMenu"
            Me.CloseMenu.Size = New System.Drawing.Size(152, 22)
            Me.CloseMenu.Text = "关闭(&C)"
            '
            'FloatingForm
            '
            Me.AllowDrop = True
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.ClientSize = New System.Drawing.Size(221, 199)
            Me.ContextMenuStrip = Me.ContextMenu
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Name = "FloatingForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
            Me.TopMost = True
            Me.ContextMenu.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents ContextMenu As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents TransparentMenu As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ContextBar0 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents CloseMenu As System.Windows.Forms.ToolStripMenuItem
    End Class
End Namespace
