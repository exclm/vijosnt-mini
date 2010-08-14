Imports VijosNT.Background

Public Class EntryPoint
    Public Shared Sub Main()
        ' TODO: Implement user interface
        'If Environment.UserInteractive Then
        '    Start UI
        'Else
        Using Service As New Service()
            Service.Entry()
        End Using
        'End If
    End Sub
End Class
