Friend NotInheritable Class ServiceEntry
    Protected Shared Sub WatchDogCallback(ByVal Result As WatchDog.Result)
        Dim Process As ProcessEx = DirectCast(Result.State, ProcessEx)
        Console.WriteLine("Quota usage: {0} ms", Result.QuotaUsage \ 10000)
        Console.WriteLine("Exit code: {0}", Process.ExitCode)
        Process.Close()
    End Sub

    Public Shared Sub Main()
        Dim Process As ProcessEx, WatchDog As New WatchDog()
        WatchDog.Start()

        Using Suspended As ProcessEx.Suspended = ProcessEx.CreateSuspended("D:\Works\C++\WinTest\x64\Release\WinTest.exe", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            Process = Suspended.Resume()
            WatchDog.SetWatch(Process, 980 * 10000, AddressOf WatchDogCallback, Process)
        End Using
    End Sub
End Class
