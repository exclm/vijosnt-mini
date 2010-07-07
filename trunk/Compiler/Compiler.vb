Public NotInheritable Class Compiler
    Implements ICompiler

    Private m_Name As String
    Private m_CommandLine As String
    Private m_WorkingPath As String
    Private m_Source As String
    Private m_Target As String
    Private m_AllowParallel As Boolean

    Public Sub New(ByVal Name As String, ByVal CommandLine As String, ByVal WorkingPath As String, _
                   ByVal Source As String, ByVal Target As String, ByVal AllowParallel As Boolean)
        m_Name = Name
        m_CommandLine = CommandLine
        m_WorkingPath = WorkingPath
        m_Source = Source
        m_Target = Target
        m_AllowParallel = AllowParallel
    End Sub

    Public Function Compile(ByVal SourceCode As Stream) As ICompilerInstance Implements ICompiler.Compile

    End Function

    Public Function GetName() As String Implements ICompiler.GetName
        Return m_Name
    End Function
End Class
