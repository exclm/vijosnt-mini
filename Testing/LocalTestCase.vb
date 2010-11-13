Imports VijosNT.Win32

Namespace Testing
    Friend Class LocalTestCase
        Inherits TestCase

        Protected m_Index As Int32
        Protected m_Weight As Int32
        Protected m_InputFileName As String
        Protected m_AnswerFileName As String
        Protected m_TimeQuota As Int64?
        Protected m_MemoryQuota As Int64?

        Public Sub New(ByVal Index As Int32, ByVal Weight As Int32, ByVal InputFileName As String, ByVal AnswerFileName As String, ByVal TimeQuota As Int64?, ByVal MemoryQuota As Int64?)
            m_Index = Index
            m_Weight = Weight
            m_InputFileName = InputFileName
            m_AnswerFileName = AnswerFileName
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

        Public Overrides ReadOnly Property TimeQuota() As Int64?
            Get
                Return m_TimeQuota
            End Get
        End Property

        Public Overrides ReadOnly Property MemoryQuota() As Int64?
            Get
                Return m_MemoryQuota
            End Get
        End Property

        Public Overrides Function OpenInput() As Win32.KernelObject
            Try
                Return New FileEx(m_InputFileName, CreateFileAccess.GENERIC_READ, CreateFileShare.FILE_SHARE_READ, CreateFileDisposition.OPEN_EXISTING, CreateFileFlags.FILE_FLAG_SEQUENTIAL_SCAN)
            Catch ex As Exception
                Return New FileEx("nul", CreateFileAccess.GENERIC_READ, CreateFileShare.FILE_SHARE_READ, CreateFileDisposition.OPEN_EXISTING, CreateFileFlags.FILE_FLAG_SEQUENTIAL_SCAN)
            End Try
        End Function

        Protected Overrides Function OpenAnswer() As Win32.KernelObject
            Try
                Return New FileEx(m_AnswerFileName, CreateFileAccess.GENERIC_READ, CreateFileShare.FILE_SHARE_READ, CreateFileDisposition.OPEN_EXISTING, CreateFileFlags.FILE_FLAG_SEQUENTIAL_SCAN)
            Catch ex As Exception
                Return New FileEx("nul", CreateFileAccess.GENERIC_READ, CreateFileShare.FILE_SHARE_READ, CreateFileDisposition.OPEN_EXISTING, CreateFileFlags.FILE_FLAG_SEQUENTIAL_SCAN)
            End Try
        End Function
    End Class
End Namespace