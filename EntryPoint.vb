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
            Try
                Using Service As New Service()
                    Service.Entry()
                End Using
            Catch ex As Exception
                Log.Add(LogLevel.Error, "Unexpected exception caught", ex.ToString())
            End Try
        End If
    End Sub
End Class
