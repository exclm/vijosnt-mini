Namespace Feeding
    Friend Class DataSourceEntry
        Private m_DataSource As DataSourceBase
        Private m_HttpAnnouncement As String

        Public Sub New(ByVal DataSource As DataSourceBase, ByVal HttpAnnouncement As String)
            m_DataSource = DataSource
            m_HttpAnnouncement = HttpAnnouncement
        End Sub

        Public ReadOnly Property DataSource() As DataSourceBase
            Get
                Return m_DataSource
            End Get
        End Property

        Public ReadOnly Property HttpAnnouncement() As String
            Get
                Return m_HttpAnnouncement
            End Get
        End Property
    End Class
End Namespace
