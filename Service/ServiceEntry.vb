Imports VijosNT.Executing
Imports VijosNT.Utility

Friend NotInheritable Class ServiceEntry
    Public Shared Sub Main()
        Dim w As New WatchDog
        w.Start()
        Dim pm As New ProcessMonitor
        pm.Start()
        With New Executor(3, New EnvironmentPool() {New TrustedEnvironmentPool()})
            For i As Int32 = 0 To 0
                Using p As New Win32.Pipe()
                    .Queue(New ProcessExecutee(w, pm, "D:\Works\C++\WinTest\x64\Release\WinTest.exe", Nothing, Nothing, Nothing, Nothing, p.GetWriteHandle(), Nothing, Nothing, Nothing, Nothing, _
                        Sub(Result As ProcessExecuteeResult)
                            Console.WriteLine("Time: {0}ms, Memory: {1}kb, Status: {2:x}, Exception: {3}", _
                                Result.TimeQuotaUsage \ 10000, Result.MemoryQuotaUsage \ 1024, Result.ExitStatus, Result.Exception IsNot Nothing)
                        End Sub, Nothing))
                    StreamPipe.Connect(p.GetReadStream(), Console.OpenStandardOutput())
                End Using
            Next
        End With
    End Sub
End Class
