Namespace Testing
    Friend Class APlusBTestSuite
        Inherits TestSuite

        Public Overrides Function TryLoad(ByVal Id As String) As IEnumerable(Of TestCase)
            Dim Result As New List(Of TestCase)
            Dim Random As New Random()
            For Index As Int32 = 0 To 9
                Dim a As Int32 = Random.Next(10000)
                Dim b As Int32 = Random.Next(10000)
                Result.Add(New StringTestCase(Index + 1, 10, a.ToString() & " " & b.ToString(), (a + b).ToString(), 1000 * 10000, 64 * 1024 * 1024))
            Next
            Return Result
        End Function
    End Class
End Namespace
