Public NotInheritable Class ServiceEntry
    Private Shared Ace0 As UserObject.AllowedAce = New UserObject.AllowedAce(UserObject.AceFlags.ContainerInheritAce Or UserObject.AceFlags.InheritOnlyAce Or UserObject.AceFlags.ObjectInheritAce, UserObject.AceMask.GenericRead Or UserObject.AceMask.GenericWrite Or UserObject.AceMask.GenericExecute Or UserObject.AceMask.GenericAll)
    Private Shared Ace1 As UserObject.AllowedAce = New UserObject.AllowedAce(UserObject.AceFlags.NoPropagateInheritAce, UserObject.AceMask.WinstaAll)
    Private Shared Ace2 As UserObject.AllowedAce = New UserObject.AllowedAce(0, UserObject.AceMask.DesktopAll)

    Public Shared Sub TestTime()
        Dim r As New RandomName
        Dim t As Int32 = GetTickCount()
        For i As Int32 = 0 To 999
            Using l As New Logon("vjmini", "password")
                Using w As New WindowStation()
                    w.AddAllowedAce(l.GetSid(), New UserObject.AllowedAce() {Ace0, Ace1})
                    Using d As New Desktop(r.Next(16))
                        d.AddAllowedAce(l.GetSid(), Ace2)

                        d.RemoveAceBySid(l.GetSid())
                    End Using
                    w.RemoveAceBySid(l.GetSid())
                End Using
            End Using
        Next

        Console.WriteLine("Time: {0}ms", GetTickCount() - t)
    End Sub

    Public Shared Sub Main()
        TestTime()
    End Sub
End Class
