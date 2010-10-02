Namespace Foreground
    Friend Class StressTest
        Private m_Daemon As Daemon

        Public Sub New(ByVal Daemon As Daemon)

            ' 此调用是设计器所必需的。
            InitializeComponent()

            ' 在 InitializeComponent() 调用之后添加任何初始化。
            m_Daemon = Daemon
        End Sub

        Private Sub TimerUpdate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TickPerSecond.Scroll, TockPerTick.Scroll, StartCheck.CheckedChanged
            Dim Enabled = StartCheck.Checked
            If StressTimer.Enabled <> Enabled Then _
                StressTimer.Enabled = Enabled
            Dim Interval = 1000 \ TickPerSecond.Value
            If StressTimer.Interval <> Interval Then _
                StressTimer.Interval = Interval
        End Sub

        Private Sub StressTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StressTimer.Tick
            For Index = 0 To TockPerTick.Value - 1
                LocalDb.Record.Add(FileNameText.Text, CodeText.Text)
            Next
            m_Daemon.FeedDataSource(String.Empty)
        End Sub
    End Class
End Namespace