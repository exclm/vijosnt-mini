Public NotInheritable Class ProcessEx
    Inherits WaitHandle

#Region "Shared members"
    Public Shared Function Attach(ByVal ProcessID As Int32) As ProcessEx
        Dim hProcess As IntPtr
        hProcess = OpenProcess(ProcessAccess.PROCESS_ALL_ACCESS, False, ProcessID)
        Return New ProcessEx(hProcess)
    End Function
#End Region

    Private m_hProcess As IntPtr

    Public Sub New(ByVal hProcess As IntPtr)
        m_hProcess = hProcess
        MyBase.SafeWaitHandle = New SafeWaitHandle(hProcess, True)
    End Sub

    Public Sub Kill(ByVal ReturnCode As Int32)
        Win32True(TerminateProcess(m_hProcess, ReturnCode))
    End Sub
End Class
