Namespace Utility
    Friend Class StreamRecorder
        Public Delegate Sub Completion(ByVal Result As Result)

        Public Structure Result
            Dim State As Object
            Dim Buffer As Byte()
            Dim Success As Boolean
        End Structure

        Private m_Stream As Stream
        Private m_Buffer As Byte()
        Private m_Length As Int32
        Private m_Completion As Completion
        Private m_CompletionState As Object

        Public Sub New(ByVal Stream As Stream, ByVal BufferSize As Int32, ByVal Completion As Completion, ByVal State As Object)
            m_Stream = Stream
            m_Buffer = New Byte(0 To BufferSize - 1) {}
            m_Length = 0
            m_Completion = Completion
            m_CompletionState = State
        End Sub

        Public Sub Start()
            m_Stream.BeginRead(m_Buffer, m_Length, m_Buffer.Length - m_Length, AddressOf OnRead, Nothing)
        End Sub

        Private Sub OnRead(ByVal Result As IAsyncResult)
            Try
                Dim Length As Int32 = m_Stream.EndRead(Result)
                If Length = 0 Then
                    InvokeCompletion(True)
                Else
                    m_Length += Length
                    If m_Length >= m_Buffer.Length Then
                        InvokeCompletion(False)
                    Else
                        Start()
                    End If
                End If
            Catch ex As Exception
                InvokeCompletion(False)
            End Try
        End Sub

        Private Sub InvokeCompletion(ByVal Success As Boolean)
            If m_Completion IsNot Nothing Then
                Dim Result As Result
                Result.State = m_CompletionState
                Result.Buffer = New Byte(0 To m_Length - 1) {}
                Buffer.BlockCopy(m_Buffer, 0, Result.Buffer, 0, m_Length)
                Result.Success = Success
                m_Completion.Invoke(Result)
            End If
        End Sub
    End Class
End Namespace
