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
                        ParseEnvironmentVariables(Reader("EnvironmentVariables")), _
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

        Private Function ParseEnvironmentVariables(ByVal Value As String) As IEnumerable(Of String)
            Dim Map As New Dictionary(Of String, String)(StringComparer.InvariantCultureIgnoreCase)
            For Each Variable As DictionaryEntry In Environment.GetEnvironmentVariables()
                Map.Add(Variable.Key, Variable.Value)
            Next
            Dim Delta As String() = Value.Split(New Char() {"|"c})
            For Index As Int32 = 0 To Delta.Length - 1
                With Delta(Index)
                    Dim Position As Int32 = .IndexOf("="c)
                    If Position = -1 Then Continue For
                    Dim Name As String = .Substring(0, Position)
                    Dim Parameter As String = .Substring(Position + 1)
                    If Map.ContainsKey(Name) Then _
                        Map.Remove(Name)
                    If Parameter.Length <> 0 Then _
                        Map.Add(Name, Environment.ExpandEnvironmentVariables(Parameter))
                End With
            Next
            Dim Result As New List(Of String)
            For Each KeyValue As KeyValuePair(Of String, String) In Map
                Result.Add(KeyValue.Key & "=" & KeyValue.Value)
            Next
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
