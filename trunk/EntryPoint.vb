Imports VijosNT.Background
Imports VijosNT.Utility

Public Class EntryPoint
    Public Shared Sub Main()
        ' TODO: Implement user interface
        'If Environment.UserInteractive Then
        '    Start UI
        'Else
        SetWorkerThreads(500, 500)
        Using Service As New Service()
            Service.Entry()
        End Using
        'End If
    End Sub
End Class
