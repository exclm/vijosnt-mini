Namespace Background
    Friend Structure TestResult
        Public Sub New(ByVal State As Object, ByVal Flag As TestResultFlag, ByVal Warning As String, ByVal Score As Int32, ByVal TimeUsage As Int64, ByVal MemoryUsage As Int64, ByVal Entries As IEnumerable(Of TestResultEntry))
            Me.State = State
            Me.Flag = Flag
            Me.Warning = Warning
            Me.Score = Score
            Me.TimeUsage = TimeUsage
            Me.MemoryUsage = MemoryUsage
            Me.Entries = Entries
        End Sub

        Dim State As Object
        Dim Flag As TestResultFlag
        Dim Warning As String
        Dim Score As Int32
        Dim TimeUsage As Int64
        Dim MemoryUsage As Int64
        Dim Entries As IEnumerable(Of TestResultEntry)
    End Structure
End Namespace
