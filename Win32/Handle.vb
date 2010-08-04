Friend Class Handle
    Implements IDisposable

    Private m_Handle As IntPtr

    Public Sub New(ByVal OwnedHandle As IntPtr)
        m_Handle = OwnedHandle
    End Sub

    Public Function GetHandleUnsafe() As IntPtr
        Return m_Handle
    End Function

    Public Function Duplicate() As IntPtr
        Return Duplicate(False)
    End Function

    Public Function Duplicate(ByVal InheritHandle As Boolean) As IntPtr
        Dim Result As IntPtr
        Win32True(DuplicateHandle(GetCurrentProcess(), m_Handle, GetCurrentProcess(), Result, 0, InheritHandle, DuplicateOption.DUPLICATE_SAME_ACCESS))
        Return Result
    End Function

    Public Sub Wait()
        Win32True(WaitForSingleObject(m_Handle, Timeout.Infinite) <> WaitResult.WAIT_FAILED)
    End Sub

    Public Function Wait(ByVal Timeout As Int32) As Boolean
        Dim Result As WaitResult = WaitForSingleObject(m_Handle, Timeout)
        Win32True(Result <> WaitResult.WAIT_FAILED)
        Return Result <> WaitResult.WAIT_TIMEOUT
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 检测冗余的调用

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            Win32True(CloseHandle(m_Handle))
        End If
        Me.disposedValue = True
    End Sub

    Protected Overrides Sub Finalize()
        ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ' Visual Basic 添加此代码是为了正确实现可处置模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
