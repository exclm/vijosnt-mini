Imports VijosNT.Utility

Namespace Compiling
    Friend Class LocalTarget
        Inherits Target
        Implements IDisposable

        Private m_TempPathServer As TempPathServer
        Private m_TargetStream As Stream
        Private m_CompilerInstance As LocalCompilerInstance

        Public Sub New(ByVal TempPathServer As TempPathServer, ByVal TargetStream As Stream, ByVal CompilerInstance As LocalCompilerInstance)
            m_TempPathServer = TempPathServer
            m_TargetStream = TargetStream
            m_CompilerInstance = CompilerInstance
        End Sub

        Public Overrides Function CreateInstance() As TargetInstance
            Dim TempPath As TempPath = m_TempPathServer.CreateTempPath()
            Using DestinationStream As New FileStream(TempPath.Combine("Target.exe"), FileMode.CreateNew, FileAccess.Write, FileShare.None)
                BufferedCopySeek0(m_TargetStream, DestinationStream)
            End Using
            Return New LocalTargetInstance(TempPath)
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
