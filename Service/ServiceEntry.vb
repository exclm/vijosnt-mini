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
                For i As Int32 = 0 To 0
                    Dim Process As ProcessEx, Stream As Stream
                    Using StdoutPipe As New Pipe()
                        Using Suspended As ProcessEx.Suspended = ProcessEx.CreateSuspended("C:\Program Files\Java\jre6\bin\java.exe", "java hello_world.HelloWorld", Nothing, "D:\JavaCourse\Workspace\HelloWorld\bin", Desktop.GetName(), Nothing, StdoutPipe.GetWriteHandle(), StdoutPipe.GetWriteHandle(), Token)
                            Using JobObject As New JobObject()
                                With JobObject.Limits
                                    .ActiveProcess = 1
                                    .BreakawayOk = False
                                    .Commit()
                                End With
                                JobObject.Assign(Suspended)
                            End Using
                            Process = Suspended.Resume()
                        End Using
                        Stream = StdoutPipe.GetReadStream()
                    End Using

                    Using Reader As New StreamReader(Stream)
                        Console.WriteLine(Reader.ReadToEnd())
                    End Using
                    Process.Close()
                Next
            Finally
                Desktop.RemoveAceBySid(Token.GetSid())
                WindowStation.RemoveAceBySid(Token.GetSid())
            End Try
        End Using

        Console.Write((Date.Now - x).TotalMilliseconds)
    End Sub
End Class
