Imports VijosNT.LocalDb
Imports VijosNT.Utility

Namespace Compiling
    Friend Class LocalCompilerPool
        Private m_Entries As IList(Of LocalCompilerEntry)

        Public Sub New(ByVal TempPathServer As TempPathServer)
            m_Entries = New List(Of LocalCompilerEntry)

            Using Reader As IDataReader = CompilerMapping.GetAll()
                While Reader.Read()
                    Dim Entry As LocalCompilerEntry
                    Entry.Regex = New Regex(RegexSimpleEscape(Reader("Pattern")), RegexOptions.IgnoreCase)
                    Entry.Compiler = New LocalCompiler(TempPathServer, _
                        Reader("ApplicationName"), _
                        Reader("CommandLine"), _
                        DbToLocalInt64(Reader("TimeQuota")), _
                        DbToLocalInt64(Reader("MemoryQuota")), _
                        DbToLocalInt32(Reader("ActiveProcessQuota")), _
                        Reader("SourceFileName"), Reader("TargetFileName"))
                    ' TODO: TargetApplicationName and TargetCommandLine
                    m_Entries.Add(Entry)
                End While
            End Using
        End Sub

        Public Function TryGet(ByVal Text As String) As LocalCompiler
            For Each Entry As LocalCompilerEntry In m_Entries
                If Entry.Regex.IsMatch(Text) Then
                    Return Entry.Compiler
                End If
            Next
            Return Nothing
        End Function
    End Class
End Namespace
