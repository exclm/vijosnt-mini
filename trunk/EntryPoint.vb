Public Class EntryPoint
    Public Shared Sub Main()
        If Environment.UserInteractive Then
            Using Daemon As New Foreground.Daemon()
                Daemon.Entry()
            End Using
        Else
            ServiceBase.Run(New Background.Service())
        End If
    End Sub
End Class
