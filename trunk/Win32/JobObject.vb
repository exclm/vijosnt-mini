Friend Class JobObject
    Implements IDisposable

    Protected m_Handle As IntPtr

    Public Sub New()
        m_Handle = CreateJobObject(Nothing, Nothing)

        If m_Handle = 0 Then
            Throw New Win32Exception()
        End If
    End Sub

    Public Sub Assign(ByVal ProcessHandle As IntPtr)
        Win32True(AssignProcessToJobObject(m_Handle, ProcessHandle))
    End Sub

    ' TODO: implement various limits provided by win32

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            Win32True(CloseHandle(m_Handle))
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
