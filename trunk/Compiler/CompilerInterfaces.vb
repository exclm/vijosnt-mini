Public Interface ICompiler
    Function GetName() As String
    Function Compile(ByVal SourceCode As Stream) As ICompilerInstance
End Interface

Public Interface ICompilerInstance
    Inherits IRunnableObject
End Interface

Public Interface IExecutable

End Interface

Public Interface IExecutableInstance
    Inherits IRunnableObject
End Interface
