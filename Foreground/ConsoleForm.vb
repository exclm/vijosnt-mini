Namespace Foreground
    Friend Class ConsoleForm
        Private m_Daemon As Daemon

        Public Sub New(ByVal Daemon As Daemon)

            ' 此调用是设计器所必需的。
            InitializeComponent()

            ' 在 InitializeComponent() 调用之后添加任何初始化。
            m_Daemon = Daemon
        End Sub

        Private Sub ConsoleForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
            m_Daemon.ConsoleClosed()
        End Sub

        Private Sub ExitMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitMenu.Click
            Close()
        End Sub

        Private Sub AboutMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutMenu.Click
            MessageBox.Show("Hello world!", "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub
    End Class
End Namespace
