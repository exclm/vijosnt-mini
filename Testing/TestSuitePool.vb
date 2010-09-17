Imports VijosNT.LocalDb
Imports VijosNT.Utility

Namespace Testing
    Friend Class TestSuitePool
        Private m_Entries As IEnumerable(Of TestSuiteEntry)

        Public Sub New()
            Reload()
        End Sub

        Public Sub Reload()
            m_Entries = ReadEntries()
        End Sub

        Private Function ReadEntries() As IEnumerable(Of TestSuiteEntry)
            Dim Result As New List(Of TestSuiteEntry)

            Using Reader As IDataReader = TestSuiteMapping.GetAll()
                While Reader.Read()
                    Dim Entry As TestSuiteEntry
                    Entry.NamespaceRegex = New Regex(RegexSimpleEscape(Reader("NamespacePattern")), RegexOptions.IgnoreCase)
                    Entry.IdRegex = New Regex(RegexSimpleEscape(Reader("Pattern")), RegexOptions.IgnoreCase)
                    Select Case Reader("ClassName")
                        Case "APlusB"
                            Entry.TestSuite = New APlusBTestSuite()
                        Case "Vijos"
                            Entry.TestSuite = New VijosTestSuite(Reader("Parameter"))
                        Case Else
                            EventLog.WriteEntry(My.Resources.ServiceName, "测试数据集加载失败" & vbCrLf & "未找到类名为 " & Reader("ClassName") & " 的测试数据集容器。", EventLogEntryType.Warning)
                            Continue While
                    End Select
                    Result.Add(Entry)
                End While
            End Using

            Return Result
        End Function

        Public Function TryLoad(ByVal [Namespace] As String, ByVal Id As String) As IEnumerable(Of TestCase)
            Dim Entries As IEnumerable(Of TestSuiteEntry) = m_Entries

            For Each Entry As TestSuiteEntry In Entries
                If Entry.NamespaceRegex.IsMatch([Namespace]) AndAlso Entry.IdRegex.IsMatch(Id) Then
                    Dim Result As IEnumerable(Of TestCase) = Entry.TestSuite.TryLoad(Id)
                    If Result IsNot Nothing Then _
                        Return Result
                End If
            Next
            Return Nothing
        End Function
    End Class
End Namespace
