Imports VijosNT.Background
Imports VijosNT.Foreground
Imports VijosNT.LocalDb

Public Class EntryPoint
    Public Shared Sub Main()
        If Environment.UserInteractive Then
            Using Daemon As New Daemon()
                Daemon.Entry()
            End Using
        Else
            ServiceBase.Run(New Service())
        End If
    End Sub
End Class
