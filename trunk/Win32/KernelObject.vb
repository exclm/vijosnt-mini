Namespace Win32
    Friend Class KernelObject
        Inherits WaitHandle
        Implements IDisposable

        Public Sub New(ByVal OwnedHandle As IntPtr)
            InternalSetHandle(OwnedHandle)
        End Sub

        Public Sub New(ByVal Handle As IntPtr, ByVal Owned As Boolean)
            If Owned Then
                InternalSetHandle(Handle)
            Else
                InternalSetHandle(Duplicate(Handle, False))
            End If
        End Sub

        Public Sub New(ByVal ObjectNotOwned As KernelObject)
            InternalSetHandle(ObjectNotOwned.Duplicate())
        End Sub

        Protected Sub New()
            ' Do nothing
        End Sub

        Protected Sub InternalSetHandle(ByVal OwnedHandle As IntPtr)
            MyBase.SafeWaitHandle = New SafeWaitHandle(OwnedHandle, True)
        End Sub

        Public Function GetHandleUnsafe() As IntPtr
            Return SafeWaitHandle.DangerousGetHandle()
        End Function

        Public Function Duplicate() As IntPtr
            Return Duplicate(False)
        End Function

        Public Function Duplicate(ByVal InheritHandle As Boolean) As IntPtr
            Return Duplicate(GetHandleUnsafe(), InheritHandle)
        End Function

        Public Shared Function Duplicate(ByVal SourceHandle As IntPtr, ByVal InheritHandle As Boolean) As IntPtr
            Dim Result As IntPtr
            Win32True(DuplicateHandle(GetCurrentProcess(), SourceHandle, GetCurrentProcess(), Result, 0, InheritHandle, DuplicateOption.DUPLICATE_SAME_ACCESS))
            Return Result
        End Function
    End Class
End Namespace