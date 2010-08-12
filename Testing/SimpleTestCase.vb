Imports VijosNT.Win32

Namespace Testing
    Friend Class SimpleTestCase
        Inherits TestCase

        Protected m_InputFileName As String
        Protected m_AnswerFileName As String
        Protected m_TimeQuota As Nullable(Of Int64)
        Protected m_MemoryQuota As Nullable(Of Int64)

        Public Sub New(ByVal InputFileName As String, ByVal AnswerFileName As String, ByVal TimeQuota As Nullable(Of Int64), ByVal MemoryQuota As Nullable(Of Int64))
            m_InputFileName = InputFileName
            m_AnswerFileName = AnswerFileName
            m_TimeQuota = TimeQuota
            m_MemoryQuota = MemoryQuota
        End Sub

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

        Public Overrides Function OpenInput() As Win32.KernelObject
            Return New FileEx(m_InputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite Or FileShare.Delete)
        End Function

        Protected Overrides Function OpenAnswer() As Win32.KernelObject
            Return New FileEx(m_AnswerFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite Or FileShare.Delete)
        End Function
    End Class
End Namespace