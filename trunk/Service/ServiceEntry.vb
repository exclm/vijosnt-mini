Public NotInheritable Class ServiceEntry

    ' Ace0 and Ace1 are for WindowStation while Ace2 is for Desktop
    ' TODO: Readjust the access privilege
    Private Shared Ace0 As UserObject.AllowedAce = New UserObject.AllowedAce(UserObject.AceFlags.ContainerInheritAce Or UserObject.AceFlags.InheritOnlyAce Or UserObject.AceFlags.ObjectInheritAce, UserObject.AceMask.GenericRead Or UserObject.AceMask.GenericWrite Or UserObject.AceMask.GenericExecute Or UserObject.AceMask.GenericAll)
    Private Shared Ace1 As UserObject.AllowedAce = New UserObject.AllowedAce(UserObject.AceFlags.NoPropagateInheritAce, UserObject.AceMask.WinstaAll)
    Private Shared Ace2 As UserObject.AllowedAce = New UserObject.AllowedAce(0, UserObject.AceMask.DesktopAll)

    ' TODO: Figure out how to destroy a desktop

    Public Shared Sub Main()
        Using JobObject As New JobObject()
            JobObject.SetLimits(JobObject.GetLimits() _
                .SetProcessMemoryLimit(64 * 1024 * 1024) _
                .SetActiveProcessLimit(1) _
                .SetPriorityClassLimit(PriorityClass.BelowNormal))

            JobObject.GetLimits()
        End Using
    End Sub
End Class
