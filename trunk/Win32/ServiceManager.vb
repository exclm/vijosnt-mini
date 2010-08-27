Namespace Win32
    Friend Class ServiceManager
        Inherits ServiceObject

        Public Sub New()
            Dim Handle As IntPtr = OpenSCManager(Nothing, Nothing, SCManagerAccess.SC_MANAGER_ALL_ACCESS)
            Win32True(Handle <> IntPtr.Zero)
            InternalSetHandle(Handle)
        End Sub

        Public Function Open(ByVal ServiceName As String) As Service
            Dim Handle As IntPtr = OpenService(GetHandleUnsafe(), ServiceName, ServiceAccess.SERVICE_ALL_ACCESS)

            If Handle = IntPtr.Zero Then
                Dim LastErr As Int32 = Marshal.GetLastWin32Error()
                If LastErr <> ERROR_SERVICE_DOES_NOT_EXIST Then
                    Throw New Win32Exception(LastErr)
                Else
                    Return Nothing
                End If
            Else
                Return New Service(Handle)
            End If
        End Function

        Public Function Create(ByVal ServiceName As String, ByVal DisplayName As String, ByVal BinaryPathName As String) As Service
            Dim Handle As IntPtr = CreateService(GetHandleUnsafe(), ServiceName, DisplayName, _
                ServiceAccess.SERVICE_ALL_ACCESS, ServiceType.SERVICE_WIN32_OWN_PROCESS, ServiceStartType.SERVICE_AUTO_START, _
                ServiceErrorControl.SERVICE_ERROR_NORMAL, BinaryPathName, Nothing, IntPtr.Zero, Nothing, Nothing, Nothing)

            Win32True(Handle <> IntPtr.Zero)
            Return New Service(Handle)
        End Function
    End Class
End Namespace
