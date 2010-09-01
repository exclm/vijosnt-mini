Namespace Feeding
    Friend Class TestResult
        Public Sub New(ByVal State As Object, ByVal Flag As TestResultFlag, ByVal Warning As String, ByVal Score As Int32, ByVal TimeUsage As Int64, ByVal MemoryUsage As Int64, ByVal Entries As IEnumerable(Of TestResultEntry))
            Me.State = State
            Me.Flag = Flag
            Me.Warning = Warning
            Me.Score = Score
            Me.TimeUsage = TimeUsage
            Me.MemoryUsage = MemoryUsage
            Me.Entries = Entries
        End Sub

        Public State As Object
        Public Flag As TestResultFlag
        Public Warning As String
        Public Score As Int32
        Public TimeUsage As Int64
        Public MemoryUsage As Int64
        Public Entries As IEnumerable(Of TestResultEntry)
    End Class
End Namespace
