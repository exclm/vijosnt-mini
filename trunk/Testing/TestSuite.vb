Namespace Testing
    Friend MustInherit Class TestSuite
        Public MustOverride Function TryLoad(ByVal Id As String) As IEnumerable(Of TestCase)
    End Class
End Namespace
