Imports VijosNT.LocalDb

Namespace Foreground
    Friend Class FloatingForm
        Private m_Daemon As Daemon
        Private m_Icon As Icon

        Private Sub FloatingForm_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
            Dim Files As String() = e.Data.GetData(DataFormats.FileDrop)
            For Each File In Files
                Using Reader As New StreamReader(File)
                    LocalDb.Record.Add(Path.GetFileName(File), Reader.ReadToEnd())
                End Using
            Next
            m_Daemon.FeedDataSource(String.Empty)
        End Sub

        Private Sub FloatingForm_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then _
                e.Effect = DragDropEffects.Copy
        End Sub

        Private Sub FloatingForm_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
            With e.Graphics
                .DrawIcon(m_Icon, 8, 8)
                .DrawString("将文件拖到这里进行评测", Font, Brushes.Black, 44, 18)
            End With
        End Sub

        Public Sub New(ByVal Daemon As Daemon)
            m_Daemon = Daemon
            m_Icon = My.Resources.BlueV

            ' 此调用是设计器所必需的。
            InitializeComponent()

            ' 在 InitializeComponent() 调用之后添加任何初始化。
            Left = Config.FloatingLeft
            Top = Config.FloatingTop
        End Sub
    End Class
End Namespace