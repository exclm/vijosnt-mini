Imports VijosNT.Executing
Imports VijosNT.Testing
Imports VijosNT.Utility

Friend NotInheritable Class ServiceEntry
    Public Shared Sub Main()
        SetWorkerThreads(16, 500)

        Dim ts As New VijosTestSuite("D:\Project\VijosNT\Tyvj_begin_data")
        Dim wd As New WatchDog
        Dim pm As New ProcessMonitor
        Dim ex As New Executor(4, New EnvironmentPool() {New UntrustedEnvironmentPool(New UntrustedEnvironment() { _
            New UntrustedEnvironment("Untrusted Desktop 1", "vjmini", "vjmini123"), _
            New UntrustedEnvironment("Untrusted Desktop 2", "vjmini", "vjmini123"), _
            New UntrustedEnvironment("Untrusted Desktop 3", "vjmini", "vjmini123"), _
            New UntrustedEnvironment("Untrusted Desktop 4", "vjmini", "vjmini123")
            })})

        wd.Start()
        pm.Start()

        For Each tc As TestCase In ts.TryLoad("P1000")
            ex.Queue(New TestCaseExecutee(wd, pm, "D:\Works\C++\WinTest\x64\Release\WinTest.exe", tc, _
                Sub(rs As TestCaseExecuteeResult)
                    Console.WriteLine("Test {0}: Score {1} Time {2}ms Exception {3}", rs.Index, rs.Score, rs.TimeQuotaUsage \ 10000, rs.Exception IsNot Nothing)
                End Sub, Nothing))
        Next
    End Sub
End Class
