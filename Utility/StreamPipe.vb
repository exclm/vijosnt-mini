Namespace Utility
    Friend Class StreamPipe
        Private m_Source As Stream
        Private m_Target As Stream
        Private m_Buffer As Byte()

        Public Shared Sub Connect(ByVal Source As Stream, ByVal Target As Stream)
            Connect(Source, Target, 4096)
        End Sub

        Public Shared Sub Connect(ByVal Source As Stream, ByVal Target As Stream, ByVal BufferSize As Int32)
            Dim StreamPipe As New StreamPipe(Source, Target, BufferSize)
        End Sub

        Private Sub New(ByVal Source As Stream, ByVal Target As Stream, ByVal BufferSize As Int32)
            m_Source = Source
            m_Target = Target
            m_Buffer = New Byte(0 To BufferSize - 1) {}
            MiniThreadPool.Queue(AddressOf OnTransfer, Nothing)
        End Sub

        Private Sub OnTransfer(ByVal State As Object)
            Try
                While True
                    Dim Length As Int32 = m_Source.Read(m_Buffer, 0, m_Buffer.Length)
                    If Length = 0 Then
                        m_Target.Close()
                        m_Source.Close()
                        Return
                    End If
                    m_Target.Write(m_Buffer, 0, Length)
                End While
            Catch ex As Exception
                m_Target.Close()
                m_Source.Close()
            End Try
        End Sub
    End Class
End Namespace
