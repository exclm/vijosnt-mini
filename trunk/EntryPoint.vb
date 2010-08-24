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
        If Environment.UserInteractive Then
            AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf OnUnhandledException
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
