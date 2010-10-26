Imports VijosNT.Testing

Namespace Sources
    Friend Class Source
        Private m_Namespace As String

        Public Property [Namespace] As String
            Get
                Return m_Namespace
            End Get

            Set(ByVal Value As String)
                m_Namespace = [Namespace]
            End Set
        End Property

        Public Overridable Function Take(ByVal Id As Int32) As SourceRecord
            Return Nothing
        End Function

        Public Overridable Sub Untake(ByVal Id As Int32)
            Throw New NotImplementedException()
        End Sub

        Public Overridable Sub Untake(ByVal Id As Int32, ByVal Result As TestResult)
            Throw New NotImplementedException()
        End Sub

        Public Overridable Function TryLoad(ByVal Id As String) As IEnumerable(Of TestCase)
            Return Nothing
        End Function
    End Class
End Namespace
