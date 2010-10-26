Imports VijosNT.Testing

Namespace Sources
    Friend Class APlusBSource
        Inherits Source

        Private Const DefaultTimeQuota As Int64 = 1000 * 10000
        Private Const DefaultMemoryQuota As Int64 = 128 * 1024 * 1024

        Public Sub New(ByVal [Namespace] As String)
            MyBase.New([Namespace])
        End Sub

        Public Overrides Function TryLoad(ByVal Id As String) As IEnumerable(Of TestCase)
            If Id.ToUpper() <> "A+B" Then _
                Return Nothing

            Dim Result As New List(Of TestCase)
            Dim Random As New Random()
            Dim a As Int32 = Random.Next(10000)
            Dim b As Int32 = Random.Next(10000)
            Result.Add(New StringTestCase(1, 100, a.ToString() & " " & b.ToString(), (a + b).ToString(), DefaultTimeQuota, DefaultMemoryQuota))
            Return Result
        End Function
    End Class
End Namespace
