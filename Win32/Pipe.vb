Namespace Win32
    Friend Class Pipe
        Implements IDisposable

        Public Class Stream
            Inherits FileStream

            Public Sub New(ByVal OwnedHandle As IntPtr, ByVal Access As FileAccess)
                MyBase.New(New SafeFileHandle(OwnedHandle, True), Access)
            End Sub
        End Class

        Protected m_ReadHandle As KernelObject
        Protected m_WriteHandle As KernelObject

        Public Sub New()
            Dim ReadHandle As IntPtr, WriteHandle As IntPtr
            Win32True(CreatePipe(ReadHandle, WriteHandle, Nothing, 0))
            m_ReadHandle = New KernelObject(ReadHandle)
            m_WriteHandle = New KernelObject(WriteHandle)
        End Sub

        Public Function GetReadStream() As Stream
            Return New Stream(m_ReadHandle.Duplicate(), FileAccess.Read)
        End Function

        Public Function GetWriteStream() As Stream
            Return New Stream(m_WriteHandle.Duplicate(), FileAccess.Write)
        End Function

        Public Function GetReadHandle() As KernelObject
            Return m_ReadHandle
        End Function

        Public Function GetWriteHandle() As KernelObject
            Return m_WriteHandle
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                m_ReadHandle.Close()
                m_WriteHandle.Close()
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
End Namespace