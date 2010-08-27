Imports VijosNT.LocalDb
Imports VijosNT.Win32

Namespace Foreground
    Friend Class FloatingForm
        Private m_Daemon As Daemon
        Private m_Icon As Icon

        Private Sub FloatingForm_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
            Dim Files As String() = e.Data.GetData(DataFormats.FileDrop)
            For Each File As String In Files
                Using Stream As New FileStream(File, FileMode.Open, FileAccess.Read, FileShare.Read)
                    If Stream.Length < 1048576 OrElse _
                        MessageBox.Show("文件 " & File & " 的长度超过 1MB, 确定要继续吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        Using Reader As New StreamReader(Stream)
                            LocalDb.Record.Add(Path.GetFileName(File), Reader.ReadToEnd())
                        End Using
                    End If
                End Using
            Next
            m_Daemon.FeedDataSource(String.Empty)
        End Sub

        Private Sub FloatingForm_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then _
                e.Effect = DragDropEffects.Copy
        End Sub

        Private Sub FloatingForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            BackColor = Color.White
            Left = Config.FloatingLeft
            Top = Config.FloatingTop
            Width = 40
            Height = 40
            SetTransparent(Config.FloatingTransparent)
        End Sub

        Private Sub FloatingForm_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
            Select e.Button
                Case MouseButtons.Left
                    ReleaseCapture()
                    SendMessage(Me.Handle, WM_SYSCOMMAND, SC_MOVE Or HT_CAPTION, 0)
            End Select
        End Sub

        Private Sub FloatingForm_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
            With e.Graphics
                .DrawRectangle(Pens.Black, New Rectangle(0, 0, 39, 39))
                Dim Icon As Icon = m_Icon
                If Icon IsNot Nothing Then _
                    .DrawIcon(m_Icon, 4, 4)
            End With
        End Sub

        Public Sub New(ByVal Daemon As Daemon)
            m_Daemon = Daemon

            ' 此调用是设计器所必需的。
            InitializeComponent()

            ' 在 InitializeComponent() 调用之后添加任何初始化。
        End Sub

        Public Sub SetIcon(ByVal Icon As Icon)
            m_Icon = Icon
        End Sub

        Private Sub CloseMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CloseMenu.Click
            Close()
        End Sub

        Private Sub TransparentMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TransparentMenu.Click
            Dim Enable As Boolean = Not TransparentMenu.Checked
            Config.FloatingTransparent = Enable
            SetTransparent(Enable)
        End Sub

        Private Sub SetTransparent(ByVal Enable As Boolean)
            TransparentMenu.Checked = Enable
            If Enable Then
                Opacity = 0.5
            Else
                Opacity = 1.0
            End If
        End Sub
    End Class
End Namespace