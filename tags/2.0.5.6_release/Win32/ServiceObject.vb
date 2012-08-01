Namespace Win32
    Friend Class ServiceObject
        Implements IDisposable

        Private m_Handle As IntPtr

        Public Sub New(ByVal OwnedHandle As IntPtr)
            InternalSetHandle(OwnedHandle)
        End Sub

        Protected Sub New()
            ' Do nothing
        End Sub

        Protected Sub InternalSetHandle(ByVal OwnedHandle As IntPtr)
            m_Handle = OwnedHandle
        End Sub

        Public Function GetHandleUnsafe() As IntPtr
            Return m_Handle
        End Function

        Public Sub Close()
            Dispose(True)
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                Win32True(CloseServiceHandle(m_Handle))
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
End Namespace
