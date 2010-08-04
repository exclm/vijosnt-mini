Friend Class Pipe
    Implements IDisposable

    Protected m_ReadHandle As Handle
    Protected m_WriteHandle As Handle

    Public Sub New()
        Dim ReadHandle As IntPtr, WriteHandle As IntPtr
        Win32True(CreatePipe(ReadHandle, WriteHandle, Nothing, 0))
        m_ReadHandle = New Handle(ReadHandle)
        m_WriteHandle = New Handle(WriteHandle)
    End Sub

    Public Function GetReadStream() As Stream
        Return New FileStream(New SafeFileHandle(m_ReadHandle.Duplicate(), True), FileAccess.Read)
    End Function

    Public Function GetWriteStream() As Stream
        Return New FileStream(New SafeFileHandle(m_WriteHandle.Duplicate(), True), FileAccess.Write)
    End Function

    Public Function GetReadHandle() As Handle
        Return m_ReadHandle
    End Function

    Public Function GetWriteHandle() As Handle
        Return m_WriteHandle
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            m_ReadHandle.Dispose()
            m_WriteHandle.Dispose()
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
