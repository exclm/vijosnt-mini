Imports VijosNT.Executing
Imports VijosNT.Testing
Imports VijosNT.Utility

Friend NotInheritable Class ServiceEntry
    Public Shared Sub Main()
        SetWorkerThreads(40, 500)
        Dim TestSuite As New APlusBTestSuite()
        Dim wd As New WatchDog
        wd.Start()
        Dim pm As New ProcessMonitor
        pm.Start()
        With New Executor(1, New EnvironmentPool() {New TrustedEnvironmentPool()})
            For Each TestCase As TestCase In TestSuite.TryLoad("A+B")
                .Queue(New TestCaseExecutee(wd, pm, "D:\Works\C++\WinTest\x64\Release\WinTest.exe", TestCase, _
                    Sub(Result As TestCaseExecuteeResult)
                        Console.WriteLine("Test {0}: Score {1} Time {2}", Result.Index, Result.Score, Result.TimeQuotaUsage)
                    End Sub, Nothing))
            Next
        End With
    End Sub
End Class
