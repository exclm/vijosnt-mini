Imports VijosNT.Feeding
Imports VijosNT.LocalDb
Imports VijosNT.Remoting
Imports VijosNT.Win32

Namespace Foreground
    Friend Class Daemon
        Implements IDisposable

        Public Delegate Sub DirectFeedCompletion(ByVal Result As TestResult)

        Private m_ServiceManager As ServiceManager
        Private m_LastColor As Color
        Private m_Bitmap As Bitmap
        Private m_NotifyIcon As NotifyIcon
        Private m_Console As ConsoleForm
        Private m_ServiceTimer As System.Timers.Timer
        Private m_ServiceInstalled As Boolean
        Private m_PipeClient As PipeClient
        Private m_FloatingMenu As ToolStripMenuItem
        Private m_Floating As FloatingForm
        Private m_PendingFeeds As Dictionary(Of Int32, DirectFeedCompletion)

        Public Sub New()
            CreateServiceManager()
            CreateNotifyIcon()
            CreateTimer()
            CreatePipeClient()
            CreateFloating()
            OnServiceTimer()
            m_PendingFeeds = New Dictionary(Of Int32, DirectFeedCompletion)
        End Sub

        Public Sub Entry()
            Application.Run()
        End Sub

        Private Sub CreateServiceManager()
            m_ServiceManager = New ServiceManager()
        End Sub

        Private Sub CreateNotifyIcon()
            m_NotifyIcon = New NotifyIcon()
            m_NotifyIcon.Text = "VijosNT Daemon"
            m_NotifyIcon.ContextMenuStrip = CreateMenu()
            m_NotifyIcon.Visible = True
            AddHandler m_NotifyIcon.DoubleClick, AddressOf OnConsole
            AddHandler m_NotifyIcon.BalloonTipClicked, AddressOf OnConsole
        End Sub

        Private Function CreateMenu() As ContextMenuStrip
            Dim Result As New ContextMenuStrip()
            With Result.Items
                With .Add("启动控制台(&C)", Nothing, AddressOf OnConsole)
                    .Font = New Font(.Font, FontStyle.Bold)
                End With
                With DirectCast(.Add("工具"), ToolStripMenuItem).DropDownItems
                    .Add("压力测试(&S)", Nothing, _
                         Sub()
                             Dim StressTest As New StressTest(Me)
                             StressTest.Show()
                         End Sub)

                    .Add("Vijos 比赛评测(&T)", Nothing, _
                         Sub()
                             Using Dialog As New OpenFileDialog
                                 Dialog.Title = "请选择 Vijos 设置文件 (Config.xml)"
                                 Dialog.Filter = "Config.xml|Config.xml"
                                 If Dialog.ShowDialog() = DialogResult.OK Then
                                     Try
                                         Dim VijosContest As New VijosContest(Me, Dialog.FileName)
                                         VijosContest.Show()
                                     Catch ex As Exception
                                         MessageBox.Show(ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                     End Try
                                 End If
                             End Using
                         End Sub)
                End With
                m_FloatingMenu = DirectCast(.Add("悬浮窗(&F)", Nothing, _
                    Sub()
                        ShowFloating = Not ShowFloating()
                    End Sub), ToolStripMenuItem)
                .Add("-")
                .Add("退出(&X)", Nothing, _
                     Sub()
                         Application.Exit()
                     End Sub)
            End With
            Return Result
        End Function

        Private Sub CreateTimer()
            m_ServiceTimer = New System.Timers.Timer(500)
            AddHandler m_ServiceTimer.Elapsed, AddressOf OnServiceTimer
        End Sub

        Private Sub CreatePipeClient()
            m_PipeClient = New PipeClient()

            AddHandler m_PipeClient.RunnerStatusChanged, _
                Sub(Busy As Boolean)
                    If Busy Then
                        SetIconColor(Color.Blue)
                    Else
                        SetIconColor(Color.Green)
                    End If
                End Sub

            AddHandler m_PipeClient.DirectFeedReply, _
                Sub(StateId As Int32, Result As TestResult)
                    Dim Completion As DirectFeedCompletion = Nothing
                    SyncLock m_PendingFeeds
                        If m_PendingFeeds.TryGetValue(StateId, Completion) Then
                            m_PendingFeeds.Remove(StateId)
                        End If
                    End SyncLock
                    If Completion IsNot Nothing Then
                        Completion.Invoke(Result)
                    End If
                End Sub

            AddHandler m_PipeClient.Disconnected, _
                Sub()
                    TestService()
                End Sub
        End Sub

        Private Sub CreateFloating()
            If Config.DisplayFloating Then _
                ShowFloating = True
        End Sub

        Public Sub SetIconColor(ByVal Color As Color)
            If Color <> m_LastColor Then
                m_LastColor = Color
                Select Case Color
                    Case Color.Red
                        m_NotifyIcon.Icon = My.Resources.RedV
                        m_Bitmap = My.Resources.RedVBmp
                    Case Color.Green
                        m_NotifyIcon.Icon = My.Resources.GreenV
                        m_Bitmap = My.Resources.GreenVBmp
                    Case Color.Blue
                        m_NotifyIcon.Icon = My.Resources.BlueV
                        m_Bitmap = My.Resources.BlueVBmp
                End Select
                If m_Floating IsNot Nothing Then
                    m_Floating.SetBitmap(m_Bitmap)
                    m_Floating.Invalidate()
                End If
            End If
        End Sub

        Public Sub ShowConsole()
            If m_Console Is Nothing Then
                m_Console = New ConsoleForm(Me)
                AddHandler m_Console.FormClosed, AddressOf OnConsoleClosed
            ElseIf m_Console.WindowState = FormWindowState.Minimized Then
                m_Console.Unminimize()
            End If
            m_Console.Show()
            m_Console.Activate()
        End Sub

        Private Sub OnConsole(ByVal sender As Object, ByVal e As EventArgs)
            ShowConsole()
        End Sub

        Private Sub OnConsoleClosed(ByVal sender As Object, ByVal e As EventArgs)
            m_Console = Nothing
        End Sub

        Public Property ShowFloating() As Boolean
            Get
                Return m_Floating IsNot Nothing
            End Get

            Set(ByVal Value As Boolean)
                If Value = ShowFloating Then Return
                If Value Then
                    m_Floating = New FloatingForm(Me)
                    m_Floating.SetBitmap(m_Bitmap)

                    AddHandler m_Floating.FormClosing, _
                        Sub(sender As Object, e As FormClosingEventArgs)
                            If e.CloseReason = CloseReason.UserClosing Then
                                Config.DisplayFloating = False
                            End If
                        End Sub

                    AddHandler m_Floating.FormClosed, _
                        Sub()
                            Config.FloatingTop = m_Floating.Top
                            Config.FloatingLeft = m_Floating.Left
                            m_Floating = Nothing
                            m_FloatingMenu.Checked = False
                            If m_Console IsNot Nothing Then
                                m_Console.FloatingFormButton.Checked = False
                            End If
                        End Sub

                    m_Floating.Show()
                Else
                    m_Floating.Close()
                    m_Floating = Nothing
                End If
                m_FloatingMenu.Checked = Value
                Config.DisplayFloating = Value
                If m_Console IsNot Nothing Then _
                    m_Console.FloatingFormButton.Checked = Value
            End Set
        End Property

        Private Sub TestService()
            Dim Service As Service = m_ServiceManager.Open(My.Resources.ServiceName)
            If Service IsNot Nothing Then
                ServiceInstalled = True
                Service.Dispose()
            Else
                ServiceInstalled = False
                Return
            End If
        End Sub

        Private Function TryConnect() As Boolean
            Try
                m_PipeClient.Connect(My.Resources.PipeName)
            Catch ex As Exception
                Return False
            End Try
            RefreshLocalRecord()
            Return True
        End Function

        Private Sub OnServiceTimer()
            TestService()
            If TryConnect() Then
                SetIconColor(Color.Green)
                m_ServiceTimer.Stop()
            Else
                SetIconColor(Color.Red)
            End If
        End Sub

        Public Sub RefreshLocalRecord()
            If m_Console IsNot Nothing Then
                m_Console.Invoke(New MethodInvoker( _
                    Sub()
                        m_Console.RefreshLocalRecord()
                    End Sub))
            End If
        End Sub

        Public Sub RefreshLocalRecordUnsafe()
            If m_Console IsNot Nothing Then
                m_Console.RefreshLocalRecord()
            End If
        End Sub

        Public Sub ShowBalloon()
            If m_Console IsNot Nothing Then
                m_Console.Invoke(New MethodInvoker( _
                    Sub()
                        m_Console.RefreshLocalRecord()
                    End Sub))
            Else
                m_NotifyIcon.ShowBalloonTip(3000, "VijosNT", "测试完毕, 点击此气泡查看测试结果", ToolTipIcon.Info)
            End If
        End Sub

        Public Function CreateService() As Service
            Return m_ServiceManager.Create(My.Resources.ServiceName, My.Resources.DisplayName, Application.ExecutablePath)
        End Function

        Public Function OpenService() As Service
            Return m_ServiceManager.Open(My.Resources.ServiceName)
        End Function

        Public Property ServiceInstalled() As Boolean
            Get
                Return m_ServiceInstalled
            End Get

            Set(ByVal Value As Boolean)
                m_ServiceInstalled = Value
                If Value Then
                    If Not m_PipeClient.Connected Then
                        m_ServiceTimer.Start()
                    End If
                Else
                    SetIconColor(Color.Red)
                    m_ServiceTimer.Stop()
                End If
            End Set
        End Property

        Public Function ReloadCompiler() As Boolean
            Try
                m_PipeClient.Write(ClientMessage.ReloadCompiler)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function ReloadTestSuite() As Boolean
            Try
                m_PipeClient.Write(ClientMessage.ReloadTestSuite)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function ReloadExecutor() As Boolean
            Try
                m_PipeClient.Write(ClientMessage.ReloadExecutor)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function ReloadDataSource() As Boolean
            Try
                m_PipeClient.Write(ClientMessage.ReloadDataSource)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function DirectFeed(ByVal [Namespace] As String, ByVal FileName As String, ByVal SourceCode As String, ByVal Completion As DirectFeedCompletion) As Boolean
            Dim Random = New Random()
            Dim StateId = Random.Next()

            SyncLock m_PendingFeeds
                While m_PendingFeeds.ContainsKey(StateId)
                    StateId = Random.Next()
                End While
                m_PendingFeeds.Add(StateId, Completion)
            End SyncLock

            Try
                m_PipeClient.Write(ClientMessage.DirectFeed, StateId, [Namespace], FileName, SourceCode)
                Return True
            Catch ex As Exception
                m_PendingFeeds.Remove(StateId)
                Return False
            End Try
        End Function

        Public Function DirectFeed2(ByVal [Namespace] As String, ByVal FileName As String, ByVal SourceCodePath As String, ByVal Completion As DirectFeedCompletion) As Boolean
            Dim Random = New Random()
            Dim StateId = Random.Next()

            SyncLock m_PendingFeeds
                While m_PendingFeeds.ContainsKey(StateId)
                    StateId = Random.Next()
                End While
                m_PendingFeeds.Add(StateId, Completion)
            End SyncLock

            Try
                m_PipeClient.Write(ClientMessage.DirectFeed2, StateId, [Namespace], FileName, SourceCodePath)
                Return True
            Catch ex As Exception
                m_PendingFeeds.Remove(StateId)
                Return False
            End Try
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_ServiceTimer.Dispose()
                    m_NotifyIcon.Dispose()
                    If m_Console IsNot Nothing Then
                        m_Console.Close()
                    End If
                    m_ServiceManager.Dispose()
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
