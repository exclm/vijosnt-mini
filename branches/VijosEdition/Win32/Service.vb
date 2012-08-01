Namespace Win32
    Friend Class Service
        Inherits ServiceObject

        Public Sub New(ByVal OwnedHandle As IntPtr)
            MyBase.New(OwnedHandle)
        End Sub

        Public Sub Start()
            Win32True(StartService(GetHandleUnsafe(), 0, IntPtr.Zero))
        End Sub

        Public Function [Stop]() As Boolean
            Dim ServiceStatus As SERVICE_STATUS

            If Not ControlService(GetHandleUnsafe(), ServiceControl.SERVICE_CONTROL_STOP, ServiceStatus) Then
                Dim LastErr As Int32 = Marshal.GetLastWin32Error()
                If LastErr <> ERROR_SERVICE_NOT_ACTIVE Then
                    Throw New Win32Exception(LastErr)
                Else
                    Return False
                End If
            Else
                Return True
            End If
        End Function

        Public Function Delete() As Boolean
            If Not DeleteService(GetHandleUnsafe()) Then
                Dim LastErr As Int32 = Marshal.GetLastWin32Error()
                If LastErr <> ERROR_SERVICE_MARKED_FOR_DELETE Then
                    Throw New Win32Exception(LastErr)
                Else
                    Return False
                End If
            Else
                Return True
            End If
        End Function

        Public ReadOnly Property State() As ServiceState
            Get
                Dim Result As SERVICE_STATUS_PROCESS
                Dim BytesNeeded As Int32
                Win32True(QueryServiceStatusEx(GetHandleUnsafe(), SC_STATUS_TYPE.SC_STATUS_PROCESS_INFO, Result, Marshal.SizeOf(Result), BytesNeeded))
                Return Result.dwCurrentState
            End Get
        End Property
    End Class
End Namespace
