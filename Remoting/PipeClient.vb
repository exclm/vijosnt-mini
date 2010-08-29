Imports VijosNT.Win32

Namespace Remoting
    Friend Class PipeClient
        Private m_Stream As Stream
        Private m_Buffer As Byte()

        Public Event RunnerStatusChanged(ByVal Busy As Boolean)
        Public Event LocalRecordChanged()
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

        Public Sub Write(ByVal ParamArray Params As Object())
            Dim MyStream As Stream = m_Stream
            If MyStream Is Nothing Then _
                Throw New Exception("not connected")
            Using Stream As New MemoryStream()
                Using Writer As New BinaryWriter(Stream)
                    For Each Param As Object In Params
                        Writer.Write(Param)
                    Next
                End Using
                Dim Buffer As Byte() = Stream.ToArray()
                MyStream.BeginWrite(Buffer, 0, Buffer.Length, AddressOf OnWrite, MyStream)
            End Using
        End Sub

        Private Sub OnWrite(ByVal Result As IAsyncResult)
            Dim Stream As Stream = DirectCast(Result.AsyncState, Stream)
            Try
                Stream.EndWrite(Result)
            Catch ex As Exception
                ' Do nothing
            End Try
        End Sub

        Private Sub OnReceive(ByVal Length As Int32)
            Using Stream As New MemoryStream(m_Buffer, 0, Length), _
                Reader As New BinaryReader(Stream)
                Dim Message As ServerMessage = Reader.ReadInt32()
                Select Case Message
                    Case ServerMessage.RunnerStatusChanged
                        OnRunnerStatusChanged(Reader.ReadBoolean())
                    Case ServerMessage.LocalRecordChanged
                        OnLocalRecordChanged()
                End Select
            End Using
        End Sub

        Private Sub OnRunnerStatusChanged(ByVal Busy As Boolean)
            RaiseEvent RunnerStatusChanged(Busy)
        End Sub

        Private Sub OnLocalRecordChanged()
            RaiseEvent LocalRecordChanged()
        End Sub

        Private Sub OnDisconnected()
            RaiseEvent Disconnected()
        End Sub
    End Class
End Namespace