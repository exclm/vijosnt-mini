Namespace Testing
    Friend Class FreeTestSuite
        Inherits TestSuite

        Private m_Root As String
        Private m_TimeQuota As Int64
        Private m_MemoryQuota As Int64

        Public Sub New(ByVal Parameters As String)
            m_Root = Nothing
            m_TimeQuota = 1000 * 10000
            m_MemoryQuota = 128 * 1024 * 1024

            For Each Parameter As String In Parameters.Split(New Char() {";"c})
                Dim Position As Int32 = Parameter.IndexOf("="c)
                If Position = -1 Then Continue For
                Dim Key As String = Parameter.Substring(0, Position)
                Dim Value As String = Parameter.Substring(Position + 1)
                Select Case Key.ToLower()
                    Case "root"
                        m_Root = Value
                    Case "timequota"
                        Int64.TryParse(Value, m_TimeQuota)
                    Case "memoryquota"
                        Int64.TryParse(Value, m_MemoryQuota)
                End Select
            Next
            If m_Root Is Nothing Then _
                Throw New ArgumentNullException("Root")
            If m_TimeQuota <= 0 Then _
                Throw New ArgumentOutOfRangeException("TimeQuota", "TimeQuota 必须为一个正整数")
            If m_MemoryQuota <= 0 Then _
                Throw New ArgumentOutOfRangeException("MemoryQuota", "MemoryQuota 必须为一个正整数")
        End Sub

        Private Class Couple
            Private m_Input As FileInfo
            Private m_Answer As FileInfo

            Public Sub New(ByVal Input As FileInfo, ByVal Answer As FileInfo)
                m_Input = Input
                m_Answer = Answer
            End Sub

            Public ReadOnly Property Input() As FileInfo
                Get
                    Return m_Input
                End Get
            End Property

            Public ReadOnly Property Answer() As FileInfo
                Get
                    Return m_Answer
                End Get
            End Property
        End Class

        Private Function PatchStringEvil(ByRef [String] As String, ByVal Keyword As String, ByVal Replacement As String) As Boolean
            Dim Replaced = False
            Do
                Dim Index = [String].IndexOf(Keyword, StringComparison.CurrentCultureIgnoreCase)
                If Index <> -1 Then
                    [String] = [String].Substring(0, Index) & Replacement & [String].Substring(Index + Keyword.Length)
                    Replaced = True
                Else
                    Return Replaced
                End If
            Loop
        End Function

        Private Function GetRelativePath(ByVal Parent As DirectoryInfo, ByVal File As FileInfo) As String
            Dim FileStack = New Stack(Of String)(New String() {File.Name})
            Dim Directory = File.Directory
            Do Until Directory.FullName.Equals(Parent.FullName, StringComparison.CurrentCultureIgnoreCase)
                FileStack.Push(Directory.Name)
                Directory = Directory.Parent
            Loop
            Dim Builder = New StringBuilder(FileStack.Pop())
            Do Until FileStack.Count = 0
                Builder.Append("\"c)
                Builder.Append(FileStack.Pop())
            Loop
            Return Builder.ToString()
        End Function

        Private Function GetCouples(ByVal Root As DirectoryInfo) As List(Of Couple)
            Dim Cache = New Dictionary(Of String, KeyValuePair(Of FileInfo, Boolean))
            Dim Result = New List(Of Couple)()

            For Each File In Root.GetFiles("*", SearchOption.AllDirectories)
                Dim IsInput = False, IsAnswer = False
                Dim Path = GetRelativePath(Root, File)

                IsInput = IsInput Or PatchStringEvil(Path, "input", "*2")
                IsInput = IsInput Or PatchStringEvil(Path, "inp", "*1")
                IsInput = IsInput Or PatchStringEvil(Path, "in", "*0")
                IsAnswer = IsAnswer Or PatchStringEvil(Path, "answer", "*2")
                IsAnswer = IsAnswer Or PatchStringEvil(Path, "ans", "*1")
                IsAnswer = IsAnswer Or PatchStringEvil(Path, "an", "*0")
                IsAnswer = IsAnswer Or PatchStringEvil(Path, "output", "*2")
                IsAnswer = IsAnswer Or PatchStringEvil(Path, "out", "*1")
                IsAnswer = IsAnswer Or PatchStringEvil(Path, "ou", "*0")

                If IsInput Xor IsAnswer Then
                    Console.WriteLine(Path)
                    Dim Value As KeyValuePair(Of FileInfo, Boolean) = Nothing
                    If Cache.TryGetValue(Path, Value) Then
                        If Value.Value <> IsAnswer Then
                            Cache.Remove(Path)
                            If Not IsAnswer Then
                                Result.Add(New Couple(File, Value.Key))
                            Else
                                Result.Add(New Couple(Value.Key, File))
                            End If
                        End If
                    Else
                        Cache.Add(Path, New KeyValuePair(Of FileInfo, Boolean)(File, IsAnswer))
                    End If
                End If
            Next

            Return Result
        End Function

        Private Function LoadTestCase(ByVal ProblemRoot As String, ByVal Index As Int32, ByVal Weight As Int32, ByVal Couple As Couple) As TestCase
            Return New LocalTestCase(Index, Weight, Path.Combine(ProblemRoot, Couple.Input.FullName), Path.Combine(ProblemRoot, Couple.Answer.FullName), m_TimeQuota, m_MemoryQuota)
        End Function

        Public Overrides Function TryLoad(ByVal Id As String) As IEnumerable(Of TestCase)
            Try
                Dim ProblemRoot = Path.Combine(m_Root, Id)
                Dim Result = New List(Of TestCase)
                Dim Couples = GetCouples(New DirectoryInfo(ProblemRoot))
                Dim RemainingWeight = 100
                Dim Index = 1
                For Each Couple As Couple In Couples
                    Dim Weight As Int32
                    If Index = Couples.Count Then
                        Weight = RemainingWeight
                    Else
                        Weight = 100 \ Couples.Count
                        RemainingWeight -= Weight
                    End If
                    Result.Add(LoadTestCase(ProblemRoot, Index, Weight, Couple))
                    Index += 1
                Next
                Return Result
            Catch ex As Exception
                Return Nothing
            End Try
        End Function
    End Class
End Namespace
