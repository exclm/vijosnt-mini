Imports VijosNT.Executing
Imports VijosNT.Utility

Friend NotInheritable Class ServiceEntry
    Public Shared Sub Main()
        Dim w As New WatchDog
        w.Start()
        With New Executor(2, New EnvironmentPool() {New TrustedEnvironmentPool()})
            For i As Int32 = 0 To 3 - 1
                .Queue(New ProcessExecutee(w, "c:\windows\system32\notepad.exe", Nothing, Nothing, _
                    Sub(Result As ProcessExecuteeResult)
                        Console.WriteLine("Time quota usage: {0}ms", Result.TimeQuotaUsage \ 10000)
                    End Sub, Nothing))
            Next
        End With
    End Sub
End Class
