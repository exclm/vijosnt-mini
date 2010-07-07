Public Interface IRunnableObject
    Sub PreRunning()
    Function GetStdin() As Stream
    Function GetStdout() As Stream
    Function GetStderr() As Stream
    Sub PostRunning()
    Sub SetReturnCode(ByVal ReturnCode As Int32)
End Interface
