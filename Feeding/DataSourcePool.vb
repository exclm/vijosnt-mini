Namespace Feeding
    Friend Class DataSourcePool
        Private m_DataSources As IDictionary(Of String, DataSourceBase)

        Public Sub New()
            Reload()
        End Sub

        Public Sub Reload()
            m_DataSources = ReadDataSource()
        End Sub

        Private Function ReadDataSource() As IDictionary(Of String, DataSourceBase)
            Dim Result As New Dictionary(Of String, DataSourceBase)
            Result.Add(String.Empty, New LocalDataSource())
            ' TODO: Read external data sources from database
            Return Result
        End Function

        Public Function [Get](ByVal Key As String) As DataSourceBase
            Return m_DataSources(Key)
        End Function

        Public ReadOnly Property Sources() As ICollection(Of DataSourceBase)
            Get
                Return m_DataSources.Values
            End Get
        End Property
    End Class
End Namespace
