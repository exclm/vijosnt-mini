Imports VijosNT.Feeding

Namespace Foreground
    Friend Class StressTest
        Private m_Daemon As Daemon
        Private m_Submitted As Int64
        Private m_Accepted As Int64
        Private m_Failed As Int64

        Public Sub New(ByVal Daemon As Daemon)

            ' 此调用是设计器所必需的。
            InitializeComponent()

            ' 在 InitializeComponent() 调用之后添加任何初始化。
            m_Daemon = Daemon
        End Sub

        Private Sub TimerUpdate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SpeedScroll.Scroll
            Dim Enabled = (SpeedScroll.Value <> 0)
            If StressTimer.Enabled <> Enabled Then _
                StressTimer.Enabled = Enabled
            If Enabled Then
                Dim Interval = 1000 \ SpeedScroll.Value
                If StressTimer.Interval <> Interval Then _
                    StressTimer.Interval = Interval
            End If
        End Sub

        Private Sub StressTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StressTimer.Tick
            Interlocked.Increment(m_Submitted)
            m_Daemon.DirectFeed(String.Empty, FileNameText.Text, CodeText.Text, _
                Sub(Result As TestResult)
                    If Result.Flag = TestResultFlag.Accepted Then
                        Interlocked.Increment(m_Accepted)
                    Else
                        Interlocked.Increment(m_Failed)
                    End If
                    Try
                        BeginInvoke(New MethodInvoker(AddressOf UpdateStatus))
                    Catch ex As InvalidOperationException
                    End Try
                End Sub)
            UpdateStatus()
        End Sub

        Private Sub UpdateStatus()
            Dim Submitted = m_Submitted, Accepted = m_Accepted, Failed = m_Failed
            StatusLabel.Text = "已提交: " & Submitted & ", 已通过: " & Accepted & ", 已失败: " & Failed & ", 堆积数: " & Submitted - (Accepted + Failed)
        End Sub
    End Class
End Namespace