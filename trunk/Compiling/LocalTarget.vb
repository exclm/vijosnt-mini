Imports VijosNT.Utility

Namespace Compiling
    Friend Class LocalTarget
        Inherits Target
        Implements IDisposable

        Private m_TargetStream As Stream
        Private m_CompilerInstance As LocalCompilerInstance

        Public Sub New(ByVal TargetStream As Stream, ByVal CompilerInstance As LocalCompilerInstance)
            m_TargetStream = TargetStream
            m_CompilerInstance = CompilerInstance
        End Sub

        Public Overrides ReadOnly Property CompilerInstance() As CompilerInstance
            Get
                Return m_CompilerInstance
            End Get
        End Property

        Public Overrides Function CreateInstance() As TargetInstance
            Dim Compiler As LocalCompiler = DirectCast(m_CompilerInstance.Compiler, LocalCompiler)
            Dim TempPath As TempPath = Compiler.TempPathServer.CreateTempPath()
            Using DestinationStream As New FileStream(TempPath.Combine(Compiler.TargetFileName), FileMode.CreateNew, FileAccess.Write, FileShare.None)
                BufferedCopySeek0(m_TargetStream, DestinationStream)
            End Using
            Return New LocalTargetInstance(TempPath, Me)
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_TargetStream.Close()
                    m_CompilerInstance.Dispose()
                End If
            End If
            Me.disposedValue = True
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace
