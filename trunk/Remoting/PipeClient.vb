Imports VijosNT.Win32

Namespace Remoting
    Friend Class PipeClient
        Private m_Stream As Stream
        Private m_Buffer As Byte()

        Public Event Received(ByVal Buffer As Byte())
        Public Event Disconnected()

        Public Sub New()
            m_Stream = Nothing
            m_Buffer = New Byte(0 To 4095) {}
        End Sub

        Public ReadOnly Property Connected() As Boolean
            Get
                Return m_Stream IsNot Nothing
            End Get
        End Property

        Public Sub Connect(ByVal Name As String)
            If Connected Then
                Throw New Exception("already connected")
            End If

            Using File As New FileEx(Name, CreateFileAccess.GENERIC_READ Or CreateFileAccess.GENERIC_WRITE, CreateFileShare.FILE_SHARE_NONE, CreateFileDisposition.OPEN_EXISTING, CreateFileFlags.FILE_FLAG_OVERLAPPED)
                m_Stream = New FileStream(New SafeFileHandle(File.Duplicate(), True), FileAccess.ReadWrite, 4096, True)
            End Using

            m_Stream.BeginRead(m_Buffer, 0, m_Buffer.Length, AddressOf OnRead, m_Stream)
        End Sub

        Private Sub OnRead(ByVal Result As IAsyncResult)
            Dim Stream As Stream = DirectCast(Result.AsyncState, Stream)
            Try
                Dim Length As Int32 = Stream.EndRead(Result)
                If Length = 0 Then
                    m_Stream = Nothing
                    OnDisconnected()
                    Stream.Close()
                    Return
                End If
                OnReceive(Length)
                Stream.BeginRead(m_Buffer, 0, m_Buffer.Length, AddressOf OnRead, m_Stream)
            Catch ex As Exception
                m_Stream = Nothing
                OnDisconnected()
                Stream.Close()
            End Try
        End Sub

        Private Sub OnReceive(ByVal Length As Int32)
            Dim Block As Byte() = New Byte(0 To Length - 1) {}
            Buffer.BlockCopy(m_Buffer, 0, Block, 0, Length)
            RaiseEvent Received(Block)
        End Sub

        Private Sub OnDisconnected()
            RaiseEvent Disconnected()
        End Sub
    End Class
End Namespace