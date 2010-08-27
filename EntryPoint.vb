Imports VijosNT.Win32

Public Class EntryPoint
    Private Shared Sub OnUnhandledException(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        MessageBox.Show(e.ExceptionObject.ToString(), "未捕获的异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Environment.Exit(1)
    End Sub

    Private Shared Sub OnUnhandledExceptionService(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        EventLog.WriteEntry(My.Resources.ServiceName, e.ExceptionObject.ToString(), EventLogEntryType.Error)
        Environment.Exit(1)
    End Sub

    Public Shared Sub Main()
        Dim Wow64Process As Boolean
        Win32True(IsWow64Process(GetCurrentProcess(), Wow64Process))
        If Wow64Process Then
            MessageBox.Show("不支持在 x64 操作系统上使用 x86 版本。", "VijosNT", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        If Environment.UserInteractive Then
            AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf OnUnhandledException
            Application.EnableVisualStyles()
            Dim CreatedNew As Boolean
            Using Mutex As New Mutex(True, "VijosNT Daemon Mutex", CreatedNew)
                If Not CreatedNew Then
                    MessageBox.Show("VijosNT Daemon 一个或一个以上的实例已经在运行中。", "VijosNT Daemon", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
                Using Daemon As New Foreground.Daemon()
                    Daemon.Entry()
                End Using
            End Using
        Else
            AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf OnUnhandledExceptionService
            ServiceBase.Run(New Background.Service())
        End If
    End Sub
End Class
