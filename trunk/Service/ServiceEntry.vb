Public NotInheritable Class ServiceEntry
    Public Shared Sub Main()
        Using Token As New Token()
            Token.SetPrivilege("SeAssignPrimaryTokenPrivilege", True)
        End Using

        Dim x As Date = Date.Now

        Using WindowStation As New WindowStation(), Desktop As New Desktop("ForeverBell"), Token As New Token("vjmini", "vjmini123")
            Dim GenericAce As New UserObject.AllowedAce(0, UserObject.AceMask.GenericRead Or UserObject.AceMask.GenericWrite Or UserObject.AceMask.GenericExecute)
            WindowStation.AddAllowedAce(Token.GetSid(), GenericAce)
            Desktop.AddAllowedAce(Token.GetSid(), GenericAce)
            Try
                For i As Int32 = 0 To 99
                    Dim Process As ProcessEx, Stream As Stream
                    Using StdoutPipe As New Pipe()
                        Using Suspended As ProcessEx.Suspended = ProcessEx.CreateSuspended("c:\MinGW64\bin\gcc.exe", Nothing, Nothing, Nothing, Desktop.GetName(), Nothing, Nothing, StdoutPipe.GetWriteHandle(), Token)
                            Using JobObject As JobObject = JobObject.Create().SetLimits(JobObject.CreateLimits().SetActiveProcessLimit(1))
                                JobObject.Assign(Suspended.GetHandle())
                            End Using
                            Process = Suspended.Resume()
                        End Using
                        Stream = StdoutPipe.GetReadStream()
                    End Using

                    Using Reader As New StreamReader(Stream)
                        Console.WriteLine(Reader.ReadToEnd())
                    End Using
                    Process.GetHandle().WaitOne()
                Next
            Finally
                Desktop.RemoveAceBySid(Token.GetSid())
                WindowStation.RemoveAceBySid(Token.GetSid())
            End Try
        End Using

        Console.Write((Date.Now - x).TotalMilliseconds)
    End Sub
End Class
