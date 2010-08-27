Imports VijosNT.Utility

Namespace Compiling
    Friend Class LocalCompilerInstance
        Inherits CompilerInstance
        Implements IDisposable

        Private m_WorkingPath As TempPath
        Private m_Compiler As LocalCompiler

        Public Sub New(ByVal WorkingPath As TempPath, ByVal Compiler As LocalCompiler)
            m_WorkingPath = WorkingPath
            m_Compiler = Compiler
        End Sub

        Public Overrides ReadOnly Property Compiler() As Compiler
            Get
                Return m_Compiler
            End Get
        End Property

        Public Overrides ReadOnly Property WorkingDirectory() As String
            Get
                Return m_WorkingPath.GetPath()
            End Get
        End Property

        Public Overrides Function OpenTarget() As Target
            Return New LocalTarget(New FileStream(m_WorkingPath.Combine(m_Compiler.TargetFileName), FileMode.Open, FileAccess.Read, FileShare.Read), Me)
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_WorkingPath.Dispose()
                End If
            End If
            Me.disposedValue = True
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace
