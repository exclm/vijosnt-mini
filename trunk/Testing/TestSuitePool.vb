﻿Imports VijosNT.LocalDb
Imports VijosNT.Utility

Namespace Testing
    Friend Class TestSuitePool
        Private m_Entries As IList(Of TestSuiteEntry)

        Public Sub New()
            m_Entries = New List(Of TestSuiteEntry)

            Using Reader As IDataReader = TestSuiteMapping.GetAll()
                While Reader.Read()
                    Dim Entry As TestSuiteEntry
                    Entry.Regex = New Regex(RegexSimpleEscape(Reader("Pattern")), RegexOptions.IgnoreCase)
                    Select Case Reader("ClassName")
                        Case "APlusBTestSuite"
                            Entry.TestSuite = New APlusBTestSuite()
                        Case "VijosTestSuite"
                            Entry.TestSuite = New VijosTestSuite(Reader("Parameter"))
                        Case Else
                            Log.Add(LogLevel.Warning, "测试数据集加载失败", "未找到类名为 " & Reader("ClassName") & " 的测试数据集容器。")
                            Continue While
                    End Select
                    m_Entries.Add(Entry)
                End While
            End Using
        End Sub

        Public Function TryLoad(ByVal Text As String) As IEnumerable(Of TestCase)
            For Each Entry As TestSuiteEntry In m_Entries
                If Entry.Regex.IsMatch(Text) Then
                    Dim Result As IEnumerable(Of TestCase) = Entry.TestSuite.TryLoad(Text)
                    If Result IsNot Nothing Then _
                        Return Result
                End If
            Next
            Return Nothing
        End Function
    End Class
End Namespace
