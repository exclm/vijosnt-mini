Friend Class StreamPipe
    Inherits WaitHandle

    Protected m_Source As Stream
    Protected m_Target As Stream
    Protected m_Buffer As Byte()
    Protected m_Event As ManualResetEvent

    Public Sub New(ByVal Source As Stream, ByVal Target As Stream)
        Me.New(Source, Target, 4096)
    End Sub

    Public Sub New(ByVal Source As Stream, ByVal Target As Stream, ByVal BufferSize As Int32)
        m_Source = Source
        m_Target = Target
        m_Buffer = New Byte(0 To BufferSize - 1) {}
        m_Source.BeginRead(m_Buffer, 0, BufferSize, AddressOf OnRead, Nothing)
        m_Event = New ManualResetEvent(False)

        MyBase.SafeWaitHandle = m_Event.SafeWaitHandle
    End Sub

    Private Sub OnRead(ByVal ar As IAsyncResult)
        Try
            Dim Length As Int32 = m_Source.EndRead(ar)
            If Length = 0 Then Throw New EndOfStreamException()
            m_Target.BeginWrite(m_Buffer, 0, Length, AddressOf OnWrite, Nothing)
        Catch ex As Exception
            m_Event.Set()
        End Try
    End Sub

    Private Sub OnWrite(ByVal ar As IAsyncResult)
        Try
            m_Target.EndWrite(ar)
            m_Source.BeginRead(m_Buffer, 0, m_Buffer.Length, AddressOf OnRead, Nothing)
        Catch ex As Exception
            m_Event.Set()
        End Try
    End Sub
End Class
