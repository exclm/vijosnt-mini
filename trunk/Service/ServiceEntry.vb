Imports VijosNT.Compiling
Imports VijosNT.Executing
Imports VijosNT.Testing
Imports VijosNT.Utility

Friend NotInheritable Class ServiceEntry
    Public Shared Sub Main()
        SetWorkerThreads(16, 64)

        Dim wd As New WatchDog
        Dim pm As New ProcessMonitor
        Dim ex As New Executor(4, New EnvironmentPool() {New TrustedEnvironmentPool(), _
            New UntrustedEnvironmentPool(New UntrustedEnvironment() { _
                New UntrustedEnvironment("Untrusted Desktop 1", "vjmini", "vjmini123"), _
                New UntrustedEnvironment("Untrusted Desktop 2", "vjmini", "vjmini123"), _
                New UntrustedEnvironment("Untrusted Desktop 3", "vjmini", "vjmini123"), _
                New UntrustedEnvironment("Untrusted Desktop 4", "vjmini", "vjmini123")
            }) _
        })
        Dim tps As New TempPathServer()

        wd.Start()
        pm.Start()

        Dim ss As New MemoryStream()
        Using sw As New StreamWriter(ss)
            sw.WriteLine("#include <stdio.h>")
            sw.WriteLine()
            sw.WriteLine("int main(void) {")
            sw.WriteLine("    int a, b;")
            sw.WriteLine("    scanf(""%d%d"", &a, &b);")
            sw.WriteLine("    printf(""%d\n"", a + b);")
            sw.WriteLine("}")
        End Using

        ex.Queue(New CompilerExecutee(wd, pm, tps, New LocalCompiler(tps, "C:\MinGW64\bin\gcc.exe", "gcc -O2 -std=c99 -o foo.exe foo.c -lm", Nothing, Nothing, Nothing, "foo.c", "foo.exe"), New MemoryStream(ss.ToArray()), _
            Sub(Result As CompilerExecuteeResult)
                If Result.Target IsNot Nothing Then
                    Console.WriteLine("Compile OK.")
                    Dim ts As New APlusBTestSuite()
                    For Each tc As TestCase In ts.TryLoad("A+B")
                        ex.Queue(New TestCaseExecutee(wd, pm, Result.Target.CreateInstance(), tc, _
                            Sub(rs As TestCaseExecuteeResult)
                                Console.WriteLine("Test {0}: Score {1} Time {2}ms Exception {3}", rs.Index, rs.Score, rs.TimeQuotaUsage \ 10000, rs.Exception IsNot Nothing)
                            End Sub, Nothing))
                    Next
                Else
                    Console.WriteLine("Compile error.")
                End If
            End Sub, Nothing))
        Console.WriteLine("Press any key to exit...")
        Console.ReadLine()
        Console.WriteLine("Exiting...")
        ex.Dispose()
        tps.Dispose()
        pm.Stop()
        wd.Stop()
    End Sub
End Class
