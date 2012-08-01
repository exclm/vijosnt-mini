Imports VijosNT.Utility

Namespace Compiling
    Friend Class Target
        Implements IDisposable

        Private m_TargetStream As Stream
        Private m_CompilerInstance As CompilerInstance

        Public Sub New(ByVal TargetStream As Stream, ByVal CompilerInstance As CompilerInstance)
            m_TargetStream = TargetStream
            m_CompilerInstance = CompilerInstance
        End Sub

        Public ReadOnly Property CompilerInstance() As CompilerInstance
            Get
                Return m_CompilerInstance
            End Get
        End Property

        Public Function CreateInstance() As TargetInstance
            Dim Compiler As Compiler = m_CompilerInstance.Compiler
            Dim TempPath As TempPath = TempPathServer.Singleton().CreateTempPath()
            Using DestinationStream As New FileStream(TempPath.Combine(Compiler.TargetFileName), FileMode.CreateNew, FileAccess.Write, FileShare.None)
                BufferedCopySeek0(m_TargetStream, DestinationStream)
            End Using
            Return New TargetInstance(TempPath, Me)
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_TargetStream.Close()
                    m_CompilerInstance.Dispose()
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
