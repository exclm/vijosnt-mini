Imports VijosNT.LocalDb
Imports VijosNT.Utility

Namespace Compiling
    Friend Class LocalCompilerPool
        Private m_TempPathServer As TempPathServer
        Private m_Entries As IEnumerable(Of LocalCompilerEntry)

        Public Sub New(ByVal TempPathServer As TempPathServer)
            m_TempPathServer = TempPathServer
            Reload()
        End Sub

        Public Sub Reload()
            m_Entries = ReadEntries()
        End Sub

        Private Function ReadEntries() As ICollection(Of LocalCompilerEntry)
            Dim Result As New List(Of LocalCompilerEntry)

            Using Reader As IDataReader = CompilerMapping.GetAll()
                While Reader.Read()
                    Dim Entry As LocalCompilerEntry
                    Entry.Regex = New Regex(RegexSimpleEscape(Reader("Pattern")), RegexOptions.IgnoreCase)
                    Entry.Compiler = New LocalCompiler(m_TempPathServer, _
                        Reader("ApplicationName"), _
                        Reader("CommandLine"), _
                        DbToLocalInt64(Reader("TimeQuota")), _
                        DbToLocalInt64(Reader("MemoryQuota")), _
                        DbToLocalInt32(Reader("ActiveProcessQuota")), _
                        Reader("SourceFileName"), Reader("TargetFileName"))
                    ' TODO: TargetApplicationName and TargetCommandLine
                    Result.Add(Entry)
                End While
            End Using

            Return Result
        End Function

        Public Function TryGet(ByVal Text As String) As LocalCompiler
            Dim Entries As IEnumerable(Of LocalCompilerEntry) = m_Entries

            For Each Entry As LocalCompilerEntry In Entries
                If Entry.Regex.IsMatch(Text) Then
                    Return Entry.Compiler
                End If
            Next
            Return Nothing
        End Function
    End Class
End Namespace
