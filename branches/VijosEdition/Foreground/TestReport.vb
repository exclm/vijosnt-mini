Imports VijosNT.LocalDb
Imports VijosNT.Testing
Imports VijosNT.Utility

Namespace Foreground
    Friend Module TestReport
        Public Function LoadTestResultXml(ByVal Id As Int32) As String
            Using TextWriter As New StringWriter(), _
                Writer As New XmlTextWriter(TextWriter)
                Writer.WriteStartDocument()
                Writer.WriteStartElement("TestResult")
                Writer.WriteElementString("Id", Id.ToString())
                Dim Details As Byte()
                Using Reader As IDataReader = Record.GetReport(Id)
                    Writer.WriteElementString("FileName", ReadData(Reader, "$FileName"))
                    Writer.WriteElementString("Extension", Path.GetExtension(ReadData(Reader, "$FileName")).Substring(1))
                    Writer.WriteElementString("SourceCode", ReadData(Reader, "$SourceCode"))
                    Writer.WriteElementString("Date", ReadData(Reader, "#Date"))
                    Writer.WriteElementString("Flag", ReadData(Reader, "$Flag"))
                    Writer.WriteElementString("Score", ReadData(Reader, "%Score"))
                    Writer.WriteElementString("TimeUsage", ReadData(Reader, "!TimeUsage"))
                    Writer.WriteElementString("MemoryUsage", ReadData(Reader, "@MemoryUsage"))
                    Details = ReadData(Reader, ".Details")
                End Using
                If Details IsNot Nothing Then
                    Using Stream As New MemoryStream(Details), _
                        Reader As New BinaryReader(Stream)
                        Writer.WriteElementString("Warning", Reader.ReadString())
                        While Reader.PeekChar() <> -1
                            Writer.WriteStartElement("TestEntry")
                            Writer.WriteElementString("Index", Reader.ReadInt32().ToString())
                            Writer.WriteElementString("Flag", DirectCast(Reader.ReadInt32(), TestResultFlag).ToString())
                            Writer.WriteElementString("Score", Reader.ReadInt32().ToString())
                            Writer.WriteElementString("TimeUsage", (Reader.ReadInt64() \ 10000).ToString() & "ms")
                            Writer.WriteElementString("MemoryUsage", (Reader.ReadInt64() \ 1024).ToString() & "KB")
                            Writer.WriteElementString("Warning", Reader.ReadString())
                            Writer.WriteEndElement()
                        End While
                    End Using
                End If
                Writer.WriteEndElement()
                Writer.WriteEndDocument()
                Return TextWriter.ToString()
            End Using
        End Function
    End Module
End Namespace
