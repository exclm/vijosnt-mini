Imports VijosNT.Background
Imports VijosNT.LocalDb
Imports VijosNT.Utility

Public Class EntryPoint
    Public Shared Sub Main()
        ' TODO: Implement user interface
        'If Environment.UserInteractive Then
        '    Start UI
        'Else
        Try
            Using Service As New Service()
                Service.Entry()
            End Using
        Catch ex As Exception
            Log.Add(LogLevel.Error, "Unexpected exception caught", ex.ToString())
        End Try
        'End If
    End Sub
End Class
