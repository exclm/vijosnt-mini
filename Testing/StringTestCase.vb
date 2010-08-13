Imports VijosNT.Win32

Namespace Testing
    Friend Class StringTestCase
        Inherits TestCase

        Protected m_Index As Int32
        Protected m_Weight As Int32
        Protected m_InputString As String
        Protected m_AnswerString As String
        Protected m_TimeQuota As Nullable(Of Int64)
        Protected m_MemoryQuota As Nullable(Of Int64)

        Public Sub New(ByVal Index As Int32, ByVal Weight As Int32, ByVal InputString As String, ByVal AnswerString As String, ByVal TimeQuota As Nullable(Of Int64), ByVal MemoryQuota As Nullable(Of Int64))
            m_Index = Index
            m_Weight = Weight
            m_InputString = InputString
            m_AnswerString = AnswerString
            m_TimeQuota = TimeQuota
            m_MemoryQuota = MemoryQuota
        End Sub

        Public Overrides ReadOnly Property Index() As Int32
            Get
                Return m_Index
            End Get
        End Property

        Public Overrides ReadOnly Property Weight() As Int32
            Get
                Return m_Weight
            End Get
        End Property

        Public Overrides ReadOnly Property TimeQuota() As Nullable(Of Int64)
            Get
                Return m_TimeQuota
            End Get
        End Property

        Public Overrides ReadOnly Property MemoryQuota() As Nullable(Of Int64)
            Get
                Return m_MemoryQuota
            End Get
        End Property

        Public Overrides Function OpenInputStream() As Stream
            Using Stream As New MemoryStream()
                Using Writer As New StreamWriter(Stream)
                    Writer.Write(m_InputString)
                End Using
                Return New MemoryStream(Stream.ToArray())
            End Using
        End Function

        Protected Overrides Function OpenAnswerStream() As Stream
            Using Stream As New MemoryStream()
                Using Writer As New StreamWriter(Stream)
                    Writer.Write(m_AnswerString)
                End Using
                Return New MemoryStream(Stream.ToArray())
            End Using
        End Function
    End Class
End Namespace
