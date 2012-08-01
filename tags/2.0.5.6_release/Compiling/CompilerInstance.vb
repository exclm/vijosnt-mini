Imports VijosNT.Utility

Namespace Compiling
    Friend Class CompilerInstance
        Implements IDisposable

        Private m_WorkingPath As TempPath
        Private m_Compiler As Compiler

        Public Sub New(ByVal WorkingPath As TempPath, ByVal Compiler As Compiler)
            m_WorkingPath = WorkingPath
            m_Compiler = Compiler
        End Sub

        Public ReadOnly Property Compiler() As Compiler
            Get
                Return m_Compiler
            End Get
        End Property

        Public ReadOnly Property WorkingDirectory() As String
            Get
                Return m_WorkingPath.GetPath()
            End Get
        End Property

        Public Function OpenTarget() As Target
            Return New Target(New FileStream(m_WorkingPath.Combine(m_Compiler.TargetFileName), FileMode.Open, FileAccess.Read, FileShare.Read), Me)
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_WorkingPath.Dispose()
                End If
            End If
            Me.disposedValue = True
        End Sub

        ' Visual Basic 添加此代码是为了正确实现可处置模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace
