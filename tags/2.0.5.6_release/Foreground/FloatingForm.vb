Imports VijosNT.Testing
Imports VijosNT.LocalDb
Imports VijosNT.Win32

Namespace Foreground
    Friend Class FloatingForm
        Private m_Daemon As Daemon
        Private m_Bitmap As Bitmap

        Private Sub FloatingForm_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
            Dim Files As String() = e.Data.GetData(DataFormats.FileDrop)
            For Each File As String In Files
                Using Stream As New FileStream(File, FileMode.Open, FileAccess.Read, FileShare.Read)
                    If Stream.Length < 1048576 OrElse _
                        MessageBox.Show("文件 " & File & " 的长度超过 1MB, 确定要继续吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        Dim FileName = Path.GetFileName(File)
                        Dim SourceCode As String
                        Using Reader As New StreamReader(Stream)
                            SourceCode = Reader.ReadToEnd()
                        End Using
                        If Not m_Daemon.DirectFeed2(String.Empty, FileName, File, _
                            Sub(Result As TestResult)
                                LocalDb.Record.Add(FileName, SourceCode, Result)
                                m_Daemon.ShowBalloon()
                            End Sub) Then

                            MessageBox.Show("VijosNT 服务未启动, 无法进行测评。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                        End If
                    End If
                End Using
            Next
        End Sub

        Private Sub FloatingForm_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then _
                e.Effect = DragDropEffects.Copy
        End Sub

        Private Sub FloatingForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            BackColor = Color.White
            Left = Config.FloatingLeft
            Top = Config.FloatingTop
            Width = 48
            Height = 48
            SetTransparent(Config.FloatingTransparent)
        End Sub

        Private Sub FloatingForm_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
            If e.Button = MouseButtons.Left Then
                If e.Clicks = 1 Then
                    ReleaseCapture()
                    SendMessage(Me.Handle, WM_SYSCOMMAND, SC_MOVE Or HT_CAPTION, 0)
                ElseIf e.Clicks = 2 Then
                    m_Daemon.ShowConsole()
                End If
            End If
        End Sub

        Private Sub FloatingForm_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
            With e.Graphics
                Dim Bitmap As Bitmap = m_Bitmap
                If Bitmap IsNot Nothing Then _
                    .DrawImage(Bitmap, 0, 0)
                .DrawRectangle(Pens.Black, New Rectangle(0, 0, 47, 47))
            End With
        End Sub

        Public Sub New(ByVal Daemon As Daemon)
            m_Daemon = Daemon

            ' 此调用是设计器所必需的。
            InitializeComponent()

            ' 在 InitializeComponent() 调用之后添加任何初始化。
        End Sub

        Public Sub SetBitmap(ByVal Bitmap As Bitmap)
            m_Bitmap = Bitmap
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

        Private Sub StartConsoleMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartConsoleMenu.Click
            m_Daemon.ShowConsole()
        End Sub

        Private Sub ExitProgramMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitProgramMenu.Click
            Application.Exit()
        End Sub
    End Class
End Namespace