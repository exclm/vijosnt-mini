Namespace Win32
    Friend Class NamedPipe
        Inherits KernelObject

        Public Sub New(ByVal Name As String, ByVal BufferSize As Int32)
            Dim Handle As IntPtr = CreateNamedPipe(Name, NamedPipeOpenMode.PIPE_ACCESS_DUPLEX Or NamedPipeOpenMode.FILE_FLAG_OVERLAPPED, NamedPipeMode.PIPE_TYPE_MESSAGE Or NamedPipeMode.PIPE_READMODE_MESSAGE Or NamedPipeMode.PIPE_WAIT, NamedPipeMaxInstances.PIPE_UNLIMITED_INSTANCES, BufferSize, BufferSize, 0, Nothing)
            Win32True(Handle <> INVALID_HANDLE_VALUE)
            InternalSetHandle(Handle)
        End Sub

        Public Sub Connect()
            Using ConnectedEvent As New ManualResetEvent(False)
                Dim Overlapped As NativeOverlapped
                Overlapped.EventHandle = ConnectedEvent.SafeWaitHandle.DangerousGetHandle()
                If Not ConnectNamedPipe(GetHandleUnsafe(), Overlapped) Then
                    Dim LastErr As Int32 = Marshal.GetLastWin32Error()
                    Select Case LastErr
                        Case ERROR_PIPE_CONNECTED
                            ' Do nothing
                        Case ERROR_IO_PENDING
                            ConnectedEvent.WaitOne()
                        Case Else
                            Throw New Win32Exception(LastErr)
                    End Select
                End If
            End Using
        End Sub
    End Class
End Namespace
