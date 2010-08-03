Public NotInheritable Class ServiceEntry
    Public Shared Sub Main()
        Using Token As New Token()
            Token.SetPrivilege("SeAssignPrimaryTokenPrivilege", True)
        End Using

        Using WindowStation As New WindowStation(), Desktop As New Desktop("ForeverBell"), Token As New Token("vjmini", "vjmini123")
            Dim GenericAce As New UserObject.AllowedAce(0, UserObject.AceMask.GenericRead Or UserObject.AceMask.GenericWrite Or UserObject.AceMask.GenericExecute)
            WindowStation.AddAllowedAce(Token.GetSid(), GenericAce)
            Desktop.AddAllowedAce(Token.GetSid(), GenericAce)

            Using StdinPipe As New Pipe, StdoutPipe As New Pipe
                Using Suspended As ProcessEx.Suspended = ProcessEx.CreateSuspended("C:\MinGW64\bin\gcc.exe", "foo --version", Nothing, Nothing, Desktop.GetName(), StdinPipe.GetReadHandle(), StdoutPipe.GetWriteHandle(), StdoutPipe.GetWriteHandle())
                    Suspended.SetToken(Token)
                    StreamPipe.Create(Console.OpenStandardInput(), StdinPipe.GetWriteStream())
                    StreamPipe.Create(StdoutPipe.GetReadStream(), Console.OpenStandardOutput())

                    Using JobObject As JobObject = JobObject.Create().SetLimits(JobObject.CreateLimits().SetActiveProcessLimit(1))
                        JobObject.Assign(Suspended.GetHandle())
                    End Using

                    Using Process As ProcessEx = Suspended.Resume()
                        Process.GetHandle().Wait()
                    End Using
                End Using
            End Using
            Desktop.RemoveAceBySid(Token.GetSid())
            WindowStation.RemoveAceBySid(Token.GetSid())
        End Using
    End Sub
End Class
