Public NotInheritable Class ServiceEntry

    ' Ace0 and Ace1 are for WindowStation while Ace2 is for Desktop
    ' TODO: Readjust the access privilege
    Private Shared Ace0 As UserObject.AllowedAce = New UserObject.AllowedAce(UserObject.AceFlags.ContainerInheritAce Or UserObject.AceFlags.InheritOnlyAce Or UserObject.AceFlags.ObjectInheritAce, UserObject.AceMask.GenericRead Or UserObject.AceMask.GenericWrite Or UserObject.AceMask.GenericExecute Or UserObject.AceMask.GenericAll)
    Private Shared Ace1 As UserObject.AllowedAce = New UserObject.AllowedAce(UserObject.AceFlags.NoPropagateInheritAce, UserObject.AceMask.WinstaAll)
    Private Shared Ace2 As UserObject.AllowedAce = New UserObject.AllowedAce(0, UserObject.AceMask.DesktopAll)

    ' TODO: Figure out how to destroy a desktop

    Public Shared Sub Main()
        Using s As ProcessEx.Suspended = ProcessEx.CreateSuspended("c:\windows\system32\cmd.exe", Nothing, Nothing, Nothing, Nothing)
            Dim s0 As Stream, s1 As Stream, s2 As Stream
            Using p0 As New Pipe, p1 As New Pipe, p2 As New Pipe
                s.SetStdHandles(p0.GetReadHandleUnsafe(), p1.GetWriteHandleUnsafe(), p2.GetWriteHandleUnsafe())
                s0 = p0.ConvertWritePipeToStream()
                s1 = p1.ConvertReadPipeToStream()
                s2 = p2.ConvertReadPipeToStream()
            End Using

            Using sp0 As New StreamPipe(Console.OpenStandardInput(), s0), _
                sp1 As New StreamPipe(s1, Console.OpenStandardOutput()), _
                sp2 As New StreamPipe(s2, Console.OpenStandardError()), _
                p As ProcessEx = s.Resume()

                p.WaitOne()
            End Using
        End Using
    End Sub
End Class
