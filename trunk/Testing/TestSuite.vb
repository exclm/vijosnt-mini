Namespace Testing
    Friend MustInherit Class TestSuite
        Public MustOverride Function TryLoad(ByVal ID As String) As IEnumerable(Of TestCase)
    End Class
End Namespace
