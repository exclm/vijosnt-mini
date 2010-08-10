Namespace Utility
    Friend Class StreamPipe
        Protected m_Source As Stream
        Protected m_Target As Stream
        Protected m_Buffer As Byte()

        Public Shared Sub Connect(ByVal Source As Stream, ByVal Target As Stream)
            Connect(Source, Target, 4096)
        End Sub

        Public Shared Sub Connect(ByVal Source As Stream, ByVal Target As Stream, ByVal BufferSize As Int32)
            Dim StreamPipe As New StreamPipe(Source, Target, BufferSize)
        End Sub

        Protected Sub New(ByVal Source As Stream, ByVal Target As Stream, ByVal BufferSize As Int32)
            m_Source = Source
            m_Target = Target
            m_Buffer = New Byte(0 To BufferSize - 1) {}
            m_Source.BeginRead(m_Buffer, 0, BufferSize, AddressOf OnRead, Nothing)
        End Sub

        Protected Sub OnRead(ByVal ar As IAsyncResult)
            Try
                Dim Length As Int32 = m_Source.EndRead(ar)
                If Length = 0 Then Throw New EndOfStreamException()
                m_Target.BeginWrite(m_Buffer, 0, Length, AddressOf OnWrite, Nothing)
            Catch ex As Exception
                m_Target.Close()
                m_Source.Close()
            End Try
        End Sub

        Protected Sub OnWrite(ByVal ar As IAsyncResult)
            Try
                m_Target.EndWrite(ar)
                m_Source.BeginRead(m_Buffer, 0, m_Buffer.Length, AddressOf OnRead, Nothing)
            Catch ex As Exception
                m_Target.Close()
                m_Source.Close()
            End Try
        End Sub
    End Class
End Namespace