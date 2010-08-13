Imports VijosNT.Executing
Imports VijosNT.Testing
Imports VijosNT.Utility

Friend NotInheritable Class ServiceEntry
    Public Shared Sub Main()
        SetWorkerThreads(40, 500)

        Dim ts As New APlusBTestSuite()
        Dim wd As New WatchDog
        Dim pm As New ProcessMonitor
        Dim ex As New Executor(4, New EnvironmentPool() {New TrustedEnvironmentPool()})

        wd.Start()
        pm.Start()

        For Each tc As TestCase In ts.TryLoad("A+B")
            ex.Queue(New TestCaseExecutee(wd, pm, "D:\Works\C++\WinTest\x64\Release\WinTest.exe", tc, _
                Sub(rs As TestCaseExecuteeResult)
                    Console.WriteLine("Test {0}: Score {1} Time {2}ms", rs.Index, rs.Score, rs.TimeQuotaUsage \ 10000)
                End Sub, Nothing))
        Next
    End Sub
End Class
