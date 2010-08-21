Namespace Foreground
    Friend Class Daemon
        Implements IDisposable

        Private m_NotifyIcon As NotifyIcon
        Private m_Console As ConsoleForm

        Public Sub New()
            m_NotifyIcon = New NotifyIcon()
            m_NotifyIcon.Text = "VijosNT"
            AddHandler m_NotifyIcon.DoubleClick, AddressOf OnConsole
            m_NotifyIcon.ContextMenuStrip = CreateMenu()
            SetIconColor(Color.Green)
            m_NotifyIcon.Visible = True
        End Sub

        Public Sub SetIconColor(ByVal Color As Color)
            Select Case Color
                Case Color.Red
                    m_NotifyIcon.Icon = My.Resources.RedV
                Case Color.Green
                    m_NotifyIcon.Icon = My.Resources.GreenV
                Case Color.Blue
                    m_NotifyIcon.Icon = My.Resources.BlueV
            End Select
        End Sub

        Private Function CreateMenu() As ContextMenuStrip
            Dim Result As New ContextMenuStrip()
            With Result.Items
                With .Add("控制台(&C)", Nothing, AddressOf OnConsole)
                    .Font = New Font(.Font, FontStyle.Bold)
                End With
                .Add("-")
                .Add("退出(&X)", Nothing, AddressOf OnExit)
            End With
            Return Result
        End Function

        Private Sub OnConsole(ByVal sender As Object, ByVal e As EventArgs)
            If m_Console Is Nothing Then
                m_Console = New ConsoleForm(Me)
            End If
            m_Console.Show()
            m_Console.BringToFront()
        End Sub

        Public Sub ConsoleClosed()
            m_Console = Nothing
        End Sub

        Private Sub OnExit(ByVal sender As Object, ByVal e As EventArgs)
            Application.Exit()
        End Sub

        Public Sub Entry()
            Application.Run()
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_NotifyIcon.Dispose()
                    If m_Console IsNot Nothing Then
                        m_Console.Close()
                    End If
                End If
            End If
            Me.disposedValue = True
        End Sub

        ' Visual Basic 添加此代码是为了正确实现可处置模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace
