Imports VijosNT.Feeding
Imports VijosNT.Win32

Namespace Remoting
    Friend Class PipeServer
        Private m_Thread As Thread

        Public Sub New()
            m_Thread = New Thread(AddressOf ThreadEntry)
            m_Thread.IsBackground = True
            m_Thread.Start()
        End Sub

        Private Sub ThreadEntry()
            While True
                Using Pipe As New NamedPipe(My.Resources.PipeName, 512)
                    Pipe.Connect()
                    Dim Session As New Session(New FileStream(New SafeFileHandle(Pipe.Duplicate(), True), FileAccess.ReadWrite, 4096, True))
                End Using
            End While
        End Sub
    End Class
End Namespace
