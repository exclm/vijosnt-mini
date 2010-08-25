﻿Imports VijosNT.LocalDb
Imports VijosNT.Remoting
Imports VijosNT.Win32

Namespace Foreground
    Friend Class Daemon
        Implements IDisposable

        Private m_ServiceManager As ServiceManager
        Private m_LastColor As Color
        Private m_Icon As Icon
        Private m_NotifyIcon As NotifyIcon
        Private m_Console As ConsoleForm
        Private m_ServiceTimer As System.Timers.Timer
        Private m_ServiceInstalled As Boolean
        Private m_PipeClient As PipeClient
        Private m_FloatingMenu As ToolStripMenuItem
        Private m_Floating As FloatingForm

        Public Sub New()
            CreateServiceManager()
            CreateNotifyIcon()
            CreateTimer()
            CreatePipeClient()
            CreateFloating()
            OnServiceTimer(Nothing, Nothing)
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
        End Sub

        Private Function CreateMenu() As ContextMenuStrip
            Dim Result As New ContextMenuStrip()
            With Result.Items
                With .Add("控制台(&C)", Nothing, AddressOf OnConsole)
                    .Font = New Font(.Font, FontStyle.Bold)
                End With
                m_FloatingMenu = DirectCast(.Add("悬浮窗(&F)", Nothing, AddressOf OnFloating), ToolStripMenuItem)
                .Add("-")
                .Add("退出(&X)", Nothing, AddressOf OnExit)
            End With
            Return Result
        End Function

        Private Sub CreateTimer()
            m_ServiceTimer = New System.Timers.Timer(500)
            AddHandler m_ServiceTimer.Elapsed, AddressOf OnServiceTimer
        End Sub

        Private Sub CreatePipeClient()
            m_PipeClient = New PipeClient()
            AddHandler m_PipeClient.Disconnected, AddressOf OnPipeDisconnect
        End Sub

        Private Sub CreateFloating()
            If Config.DisplayFloating Then _
                OnFloating(Nothing, Nothing)
        End Sub

        Public Sub SetIconColor(ByVal Color As Color)
            If Color <> m_LastColor Then
                m_LastColor = Color
                Select Case Color
                    Case Color.Red
                        m_Icon = My.Resources.RedV
                    Case Color.Green
                        m_Icon = My.Resources.GreenV
                    Case Color.Blue
                        m_Icon = My.Resources.BlueV
                End Select
                m_NotifyIcon.Icon = m_Icon
                If m_Floating IsNot Nothing Then
                    m_Floating.SetIcon(m_Icon)
                    m_Floating.Invalidate()
                End If
            End If
        End Sub

        Private Sub OnConsole(ByVal sender As Object, ByVal e As EventArgs)
            If m_Console Is Nothing Then
                m_Console = New ConsoleForm(Me)
                AddHandler m_Console.FormClosed, AddressOf OnConsoleClosed
            End If
            m_Console.Show()
            m_Console.Activate()
        End Sub

        Private Sub OnConsoleClosed(ByVal sender As Object, ByVal e As EventArgs)
            m_Console = Nothing
        End Sub

        Public Sub OnFloating(ByVal sender As Object, ByVal e As EventArgs)
            If m_Floating Is Nothing Then
                m_Floating = New FloatingForm(Me)
                m_Floating.SetIcon(m_Icon)
                AddHandler m_Floating.FormClosing, AddressOf OnFloatingClosing
                AddHandler m_Floating.FormClosed, AddressOf OnFloatingClosed
                m_Floating.Show()
                m_FloatingMenu.Checked = True
                Config.DisplayFloating = True
            Else
                m_Floating.Close()
                m_Floating = Nothing
                m_FloatingMenu.Checked = False
                Config.DisplayFloating = False
            End If
        End Sub

        Private Sub OnFloatingClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs)
            If e.CloseReason = CloseReason.UserClosing Then _
                Config.DisplayFloating = False
        End Sub

        Private Sub OnFloatingClosed(ByVal sender As Object, ByVal e As EventArgs)
            Config.FloatingTop = m_Floating.Top
            Config.FloatingLeft = m_Floating.Left
            m_Floating = Nothing
            m_FloatingMenu.Checked = False
        End Sub

        Private Sub OnExit(ByVal sender As Object, ByVal e As EventArgs)
            Application.Exit()
        End Sub

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
            Return True
        End Function

        Private Sub OnServiceTimer(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            TestService()
            If TryConnect() Then
                SetIconColor(Color.Green)
                m_ServiceTimer.Stop()
            Else
                SetIconColor(Color.Red)
            End If
        End Sub

        Private Sub OnPipeDisconnect()
            TestService()
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

        Public Function FeedDataSource(ByVal DataSourceName As String) As Boolean
            Try
                m_PipeClient.Write(ClientMessage.FeedDataSource, DataSourceName)
                Return True
            Catch ex As Exception
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
