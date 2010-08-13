Namespace Testing
    Friend Class VijosTestSuite
        Inherits TestSuite

        Protected Const DefaultMemoryQuota As Int64 = 64 * 1024 * 1024

        Protected Structure Config
            Public Sub New(ByVal Index As Int32, ByVal InputFileName As String, ByVal AnswerFileName As String, ByVal TimeQuota As Int64, ByVal Weight As Int32)
                Me.Index = Index
                Me.InputFileName = InputFileName
                Me.AnswerFileName = AnswerFileName
                Me.TimeQuota = TimeQuota
                Me.Weight = Weight
            End Sub

            Dim Index As Int32
            Dim InputFileName As String
            Dim AnswerFileName As String
            Dim TimeQuota As Int64
            Dim Weight As Int32
        End Structure

        Protected m_Root As String

        Public Sub New(ByVal Root As String)
            m_Root = Root
        End Sub

        Public Overrides Function TryLoad(ByVal ID As String) As IEnumerable(Of TestCase)
            Try
                Dim ProblemRoot As String = Path.Combine(m_Root, ID)
                Dim Result As New List(Of TestCase)
                For Each Config As Config In LoadConfig(ProblemRoot)
                    Result.Add(LoadTestCase(ProblemRoot, Config))
                Next
                Return Result
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Protected Shared Function LoadConfig(ByVal ProblemRoot As String) As IEnumerable(Of Config)
            Using Reader As New StreamReader(Path.Combine(ProblemRoot, "Config.ini"))
                Dim Count As Int32 = Int32.Parse(Reader.ReadLine())
                Dim Result As New List(Of Config)
                For Index As Int32 = 0 To Count - 1
                    Dim Arguments As String() = Split(Reader.ReadLine(), "|", 5)
                    Result.Add(New Config(Index + 1, Path.Combine("Input", Arguments(0)), Path.Combine("Output", Arguments(1)), _
                        Math.BigMul(Int32.Parse(Arguments(2)), 10000 * 1000), Int32.Parse(Arguments(3))))
                Next
                Return Result
            End Using
        End Function

        Protected Shared Function LoadTestCase(ByVal ProblemRoot As String, ByVal Config As Config) As TestCase
            Return New LocalTestCase(Config.Index, Config.Weight, Path.Combine(ProblemRoot, Config.InputFileName), Path.Combine(ProblemRoot, Config.AnswerFileName), Config.TimeQuota, DefaultMemoryQuota)
        End Function
    End Class
End Namespace
