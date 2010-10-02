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
            Me.TickPerSecond = New System.Windows.Forms.TrackBar()
            Me.TockPerTick = New System.Windows.Forms.TrackBar()
            Me.StartCheck = New System.Windows.Forms.CheckBox()
            Me.StressTimer = New System.Windows.Forms.Timer(Me.components)
            CType(Me.TickPerSecond, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TockPerTick, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'FileNameText
            '
            Me.FileNameText.Location = New System.Drawing.Point(12, 12)
            Me.FileNameText.Name = "FileNameText"
            Me.FileNameText.Size = New System.Drawing.Size(270, 21)
            Me.FileNameText.TabIndex = 0
            Me.FileNameText.Text = "P1000.c"
            '
            'CodeText
            '
            Me.CodeText.Location = New System.Drawing.Point(12, 39)
            Me.CodeText.Multiline = True
            Me.CodeText.Name = "CodeText"
            Me.CodeText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.CodeText.Size = New System.Drawing.Size(270, 113)
            Me.CodeText.TabIndex = 1
            Me.CodeText.Text = "#include <stdio.h>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "int main(void)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "{" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    int a, b;" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    scanf(""%d%d"", &a, &b)" & _
                ";" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    printf(""%d\n"", a + b);" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "}" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
            '
            'TickPerSecond
            '
            Me.TickPerSecond.Location = New System.Drawing.Point(12, 158)
            Me.TickPerSecond.Minimum = 1
            Me.TickPerSecond.Name = "TickPerSecond"
            Me.TickPerSecond.Size = New System.Drawing.Size(270, 45)
            Me.TickPerSecond.TabIndex = 2
            Me.TickPerSecond.Value = 1
            '
            'TockPerTick
            '
            Me.TockPerTick.Location = New System.Drawing.Point(12, 209)
            Me.TockPerTick.Minimum = 1
            Me.TockPerTick.Name = "TockPerTick"
            Me.TockPerTick.Size = New System.Drawing.Size(270, 45)
            Me.TockPerTick.TabIndex = 3
            Me.TockPerTick.Value = 1
            '
            'StartCheck
            '
            Me.StartCheck.AutoSize = True
            Me.StartCheck.Location = New System.Drawing.Point(12, 260)
            Me.StartCheck.Name = "StartCheck"
            Me.StartCheck.Size = New System.Drawing.Size(48, 16)
            Me.StartCheck.TabIndex = 4
            Me.StartCheck.Text = "开始"
            Me.StartCheck.UseVisualStyleBackColor = True
            '
            'StressTimer
            '
            '
            'StressTest
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(442, 289)
            Me.Controls.Add(Me.StartCheck)
            Me.Controls.Add(Me.TockPerTick)
            Me.Controls.Add(Me.TickPerSecond)
            Me.Controls.Add(Me.CodeText)
            Me.Controls.Add(Me.FileNameText)
            Me.Name = "StressTest"
            Me.Text = "压力测试"
            CType(Me.TickPerSecond, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TockPerTick, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents FileNameText As System.Windows.Forms.TextBox
        Friend WithEvents CodeText As System.Windows.Forms.TextBox
        Friend WithEvents TickPerSecond As System.Windows.Forms.TrackBar
        Friend WithEvents TockPerTick As System.Windows.Forms.TrackBar
        Friend WithEvents StartCheck As System.Windows.Forms.CheckBox
        Friend WithEvents StressTimer As System.Windows.Forms.Timer
    End Class
End Namespace
