Imports VijosNT.LocalDb
Imports VijosNT.Utility

Namespace Compiling
    Friend Class CompilerPool
        Private m_TempPathServer As TempPathServer
        Private m_Entries As IEnumerable(Of CompilerEntry)

        Public Sub New(ByVal TempPathServer As TempPathServer)
            m_TempPathServer = TempPathServer
            Reload()
        End Sub

        Public Sub Reload()
            m_Entries = ReadEntries()
        End Sub

        Private Function ReadEntries() As ICollection(Of CompilerEntry)
            Dim Result As New List(Of CompilerEntry)

            Using Reader As IDataReader = CompilerMapping.GetAll()
                While Reader.Read()
                    Dim Entry As CompilerEntry
                    Entry.Regex = New Regex(RegexSimpleEscape(Reader("Pattern")), RegexOptions.IgnoreCase)
                    Entry.Compiler = New Compiler(m_TempPathServer, _
                        Reader("ApplicationName"), _
                        Reader("CommandLine"), _
                        ParseEnvironmentVariables(Reader("EnvironmentVariables")), _
                        DbToLocalInt64(Reader("TimeQuota")), _
                        DbToLocalInt64(Reader("MemoryQuota")), _
                        DbToLocalInt32(Reader("ActiveProcessQuota")), _
                        Reader("SourceFileName"), _
                        Reader("TargetFileName"), _
                        Reader("TargetApplicationName"), _
                        Reader("TargetCommandLine"), _
                        DbToLocalInt64(Reader("TimeOffset")), _
                        DbToLocalDouble(Reader("TimeFactor")), _
                        DbToLocalInt64(Reader("MemoryOffset")), _
                        DbToLocalDouble(Reader("MemoryFactor")))
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
            For Each Variable As String In Value.Split(New Char() {"|"c})
                Dim Position As Int32 = Variable.IndexOf("="c)
                If Position = -1 Then Continue For
                Dim Name As String = Variable.Substring(0, Position)
                Dim Parameter As String = Variable.Substring(Position + 1)
                If Map.ContainsKey(Name) Then _
                    Map.Remove(Name)
                If Parameter.Length <> 0 Then _
                    Map.Add(Name, Environment.ExpandEnvironmentVariables(Parameter))
            Next
            Dim Result As New List(Of String)
            For Each KeyValue As KeyValuePair(Of String, String) In Map
                Result.Add(KeyValue.Key & "=" & KeyValue.Value)
            Next
            Return Result
        End Function

        Public Function TryGet(ByVal Text As String) As Compiler
            Dim Entries As IEnumerable(Of CompilerEntry) = m_Entries

            For Each Entry As CompilerEntry In Entries
                If Entry.Regex.IsMatch(Text) Then
                    Return Entry.Compiler
                End If
            Next
            Return Nothing
        End Function
    End Class
End Namespace
