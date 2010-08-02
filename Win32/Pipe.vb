Public Class Pipe
    Implements IDisposable

    Protected m_ReadHandleOwned As Boolean
    Private m_ReadHandle As IntPtr
    Protected m_WriteHandleOwned As Boolean
    Private m_WriteHandle As IntPtr

    Public Sub New()
        Win32True(CreatePipe(m_ReadHandle, m_WriteHandle, Nothing, 1))
        m_ReadHandleOwned = True
        m_WriteHandleOwned = True
    End Sub

    Public Function ConvertReadPipeToStream() As Stream
        If Not m_ReadHandleOwned Then
            Throw New Exception("The read pipe has already been converted to stream.")
        End If

        Dim SafeHandle As New SafeFileHandle(m_ReadHandle, True)
        m_ReadHandleOwned = False
        Return New FileStream(SafeHandle, FileAccess.Read)
    End Function

    Public Function ConvertWritePipeToStream() As Stream
        If Not m_WriteHandleOwned Then
            Throw New Exception("The write pipe has already been converted to stream.")
        End If

        Dim SafeHandle As New SafeFileHandle(m_WriteHandle, True)
        m_WriteHandleOwned = False
        Return New FileStream(SafeHandle, FileAccess.Write)
    End Function

    Public Function GetReadHandleUnsafe() As IntPtr
        If Not m_ReadHandleOwned Then
            Throw New Exception("The read pipe has already been converted to stream.")
        End If

        Return m_ReadHandle
    End Function

    Public Function GetWriteHandleUnsafe() As IntPtr
        If Not m_WriteHandleOwned Then
            Throw New Exception("The write pipe has already been converted to stream.")
        End If

        Return m_WriteHandle
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If m_ReadHandleOwned Then
                Win32True(CloseHandle(m_ReadHandle))
            End If
            If m_WriteHandleOwned Then
                Win32True(CloseHandle(m_WriteHandle))
            End If
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
