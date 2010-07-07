Public NotInheritable Class CompilerManager
    Private m_Map As Dictionary(Of String, ICompiler)

    Public Sub New()
        m_Map = New Dictionary(Of String, ICompiler)
    End Sub

    Public Sub Add(ByVal Compiler As ICompiler)
        m_Map.Add(Compiler.GetName(), Compiler)
    End Sub
End Class
