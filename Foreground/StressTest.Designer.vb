Namespace Foreground
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class StressTest
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
            Me.FileNameText = New System.Windows.Forms.TextBox()
            Me.CodeText = New System.Windows.Forms.TextBox()
            Me.SpeedScroll = New System.Windows.Forms.TrackBar()
            Me.StressTimer = New System.Windows.Forms.Timer(Me.components)
            Me.StatusStrip = New System.Windows.Forms.StatusStrip()
            Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
            Me.FileNameLabel = New System.Windows.Forms.Label()
            Me.SpeedLabel = New System.Windows.Forms.Label()
            CType(Me.SpeedScroll, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.StatusStrip.SuspendLayout()
            Me.SuspendLayout()
            '
            'FileNameText
            '
            Me.FileNameText.Location = New System.Drawing.Point(65, 12)
            Me.FileNameText.Name = "FileNameText"
            Me.FileNameText.Size = New System.Drawing.Size(272, 21)
            Me.FileNameText.TabIndex = 0
            Me.FileNameText.Text = "P1000.c"
            '
            'CodeText
            '
            Me.CodeText.Location = New System.Drawing.Point(12, 39)
            Me.CodeText.Multiline = True
            Me.CodeText.Name = "CodeText"
            Me.CodeText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.CodeText.Size = New System.Drawing.Size(325, 113)
            Me.CodeText.TabIndex = 1
            Me.CodeText.Text = "#include <stdio.h>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "int main(void)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "{" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    int a, b;" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    scanf(""%d%d"", &a, &b)" & _
                ";" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    printf(""%d\n"", a + b);" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "}" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
            '
            'SpeedScroll
            '
            Me.SpeedScroll.Location = New System.Drawing.Point(53, 158)
            Me.SpeedScroll.Name = "SpeedScroll"
            Me.SpeedScroll.Size = New System.Drawing.Size(284, 45)
            Me.SpeedScroll.TabIndex = 2
            '
            'StressTimer
            '
            '
            'StatusStrip
            '
            Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel})
            Me.StatusStrip.Location = New System.Drawing.Point(0, 210)
            Me.StatusStrip.Name = "StatusStrip"
            Me.StatusStrip.Size = New System.Drawing.Size(349, 22)
            Me.StatusStrip.TabIndex = 5
            Me.StatusStrip.Text = "StatusStrip1"
            '
            'StatusLabel
            '
            Me.StatusLabel.Name = "StatusLabel"
            Me.StatusLabel.Size = New System.Drawing.Size(0, 17)
            '
            'FileNameLabel
            '
            Me.FileNameLabel.AutoSize = True
            Me.FileNameLabel.Location = New System.Drawing.Point(12, 15)
            Me.FileNameLabel.Name = "FileNameLabel"
            Me.FileNameLabel.Size = New System.Drawing.Size(47, 12)
            Me.FileNameLabel.TabIndex = 6
            Me.FileNameLabel.Text = "文件名:"
            '
            'SpeedLabel
            '
            Me.SpeedLabel.AutoSize = True
            Me.SpeedLabel.Location = New System.Drawing.Point(12, 159)
            Me.SpeedLabel.Name = "SpeedLabel"
            Me.SpeedLabel.Size = New System.Drawing.Size(35, 12)
            Me.SpeedLabel.TabIndex = 7
            Me.SpeedLabel.Text = "速度:"
            '
            'StressTest
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(349, 232)
            Me.Controls.Add(Me.SpeedLabel)
            Me.Controls.Add(Me.FileNameLabel)
            Me.Controls.Add(Me.StatusStrip)
            Me.Controls.Add(Me.SpeedScroll)
            Me.Controls.Add(Me.CodeText)
            Me.Controls.Add(Me.FileNameText)
            Me.Name = "StressTest"
            Me.Text = "压力测试"
            CType(Me.SpeedScroll, System.ComponentModel.ISupportInitialize).EndInit()
            Me.StatusStrip.ResumeLayout(False)
            Me.StatusStrip.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents FileNameText As System.Windows.Forms.TextBox
        Friend WithEvents CodeText As System.Windows.Forms.TextBox
        Friend WithEvents SpeedScroll As System.Windows.Forms.TrackBar
        Friend WithEvents StressTimer As System.Windows.Forms.Timer
        Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
        Friend WithEvents StatusLabel As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents FileNameLabel As System.Windows.Forms.Label
        Friend WithEvents SpeedLabel As System.Windows.Forms.Label
    End Class
End Namespace
