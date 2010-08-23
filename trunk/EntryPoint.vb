Public Class EntryPoint
    Public Shared Sub Main()
        If Environment.UserInteractive Then
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
            ServiceBase.Run(New Background.Service())
        End If
    End Sub
End Class
