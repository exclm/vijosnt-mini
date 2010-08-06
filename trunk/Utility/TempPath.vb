Friend Class TempPath
    Inherits Path
    Implements IDisposable

    Protected m_Root As DirectoryInfo
    Protected m_Server As TempPathServer

    Public Sub New(ByVal Root As DirectoryInfo, ByVal Server As TempPathServer)
        m_Root = Root
        m_Server = Server
    End Sub

    Public Overrides Function GetPath() As String
        Return m_Root.FullName
    End Function

    Public Overrides Function GetDirectoryInfo() As System.IO.DirectoryInfo
        Return m_Root
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            m_Server.Free(m_Root)
        End If
        Me.disposedValue = True
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
