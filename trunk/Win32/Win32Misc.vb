Namespace Win32
    Friend Module Win32Misc
        Public Function GetIdleProcessTime() As Int64
            Dim SystemInformation As IntPtr = Marshal.AllocHGlobal(328)

            Try
                NtSuccess(NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemPerformanceInformation, SystemInformation, 328, Nothing))
                Return Marshal.ReadInt64(SystemInformation, 0)
            Finally
                Marshal.FreeHGlobal(SystemInformation)
            End Try
        End Function
    End Module
End Namespace