Namespace Feeding
    Friend Structure DataSourceEntry
        Public Sub New(ByVal DataSource As DataSourceBase, ByVal IpcAnnouncement As String)
            Me.DataSource = DataSource
            Me.IpcAnnouncement = IpcAnnouncement
        End Sub

        Dim DataSource As DataSourceBase
        Dim IpcAnnouncement As String
        Dim HttpAnnouncement As String
        Dim Timer As System.Timers.Timer
    End Structure
End Namespace
