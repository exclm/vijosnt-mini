Public NotInheritable Class ServiceEntry

    ' Ace0 and Ace1 are for WindowStation while Ace2 is for Desktop
    ' TODO: Readjust the access privilege
    Private Shared Ace0 As UserObject.AllowedAce = New UserObject.AllowedAce(UserObject.AceFlags.ContainerInheritAce Or UserObject.AceFlags.InheritOnlyAce Or UserObject.AceFlags.ObjectInheritAce, UserObject.AceMask.GenericRead Or UserObject.AceMask.GenericWrite Or UserObject.AceMask.GenericExecute Or UserObject.AceMask.GenericAll)
    Private Shared Ace1 As UserObject.AllowedAce = New UserObject.AllowedAce(UserObject.AceFlags.NoPropagateInheritAce, UserObject.AceMask.WinstaAll)
    Private Shared Ace2 As UserObject.AllowedAce = New UserObject.AllowedAce(0, UserObject.AceMask.DesktopAll)

    ' TODO: Figure out how to destroy a desktop

    Public Shared Sub Main()
        Using Logon As New Token("vjmini", "vjmini123")
            Using Suspended As ProcessEx.Suspended = ProcessEx.CreateSuspended("c:\windows\system32\cmd.exe", Nothing, Nothing, Nothing, Nothing)
                Suspended.SetToken(Logon.GetHandleUnsafe())

                Using StdinPipe As New Pipe, StdoutPipe As New Pipe
                    Suspended.SetStdHandles(StdinPipe.GetReadHandleUnsafe(), StdoutPipe.GetWriteHandleUnsafe(), StdoutPipe.GetWriteHandleUnsafe())
                    StreamPipe.Create(Console.OpenStandardInput(), StdinPipe.ConvertWritePipeToStream())
                    StreamPipe.Create(StdoutPipe.ConvertReadPipeToStream(), Console.OpenStandardOutput())
                End Using

                JobObject.Create().SetLimits(JobObject.CreateLimits().SetActiveProcessLimit(1)).Assign(Suspended.GetHandleUnsafe()).Dispose()
                Using x As ProcessEx = Suspended.Resume()
                    x.WaitOne()
                End Using
            End Using
        End Using
    End Sub
End Class
