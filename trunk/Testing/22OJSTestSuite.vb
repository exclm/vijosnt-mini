﻿Namespace Testing
    Friend Class _22OJSTestSuite
        Inherits TestSuite

        Private Structure Config
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

        Private m_Root As String
        Private m_MemoryQuota As Int64

        Public Sub New(ByVal Parameters As String)
            m_Root = Nothing
            m_MemoryQuota = 128 * 1024 * 1024

            For Each Parameter As String In Parameters.Split(New Char() {";"c})
                Dim Position As Int32 = Parameter.IndexOf("="c)
                If Position = -1 Then Continue For
                Dim Key As String = Parameter.Substring(0, Position)
                Dim Value As String = Parameter.Substring(Position + 1)
                Select Case Key.ToLower()
                    Case "root"
                        m_Root = Value
                    Case "memoryquota"
                        Int64.TryParse(Value, m_MemoryQuota)
                End Select
            Next
            If m_Root Is Nothing Then _
                Throw New ArgumentNullException("Root")
            If m_MemoryQuota <= 0 Then _
                Throw New ArgumentOutOfRangeException("MemoryQuota", "MemoryQuota 必须为一个正整数")
        End Sub

        Public Overrides Function TryLoad(ByVal Id As String) As IEnumerable(Of TestCase)
            Try
                Dim ProblemRoot As String = Path.Combine(m_Root, Id)
                Dim Result As New List(Of TestCase)
                Dim Configs As IEnumerable(Of Config) = LoadConfig(ProblemRoot)
                Dim Weight As Int32 = 0
                Dim Index As Int32 = 0
                Dim TotalWeight As Int32 = 0
                For Each Config As Config In Configs
                    Index += 1
                    TotalWeight += Config.Weight
                Next
                Weight = 100
                For Each Config As Config In Configs
                    Index -= 1
                    If Index = 0 Then
                        Config.Weight = Weight
                    Else
                        Config.Weight = Config.Weight * 100 \ TotalWeight
                        Weight -= Config.Weight
                    End If
                    Result.Add(LoadTestCase(ProblemRoot, Config))
                Next
                Return Result
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Private Function LoadConfig(ByVal ProblemRoot As String) As IEnumerable(Of Config)
            Dim TimeLimit, MemoryLimit As Int32
            Using Reader As New StreamReader(Path.Combine(ProblemRoot, "Config"))
                Dim Config = Reader.ReadLine().Split(";")
                For Each item As String In Config
                    Dim Configitem = item.Split("=")
                    Select Case Configitem(0).ToLower
                        Case "TimeLimit".ToLower
                            TimeLimit = Int32.Parse(Configitem(1))
                        Case "MemoryLimit".ToLower
                            MemoryLimit = Int32.Parse(Configitem(1))
                    End Select
                Next
            End Using
            Dim Count As Int32 = 0, i As Int32 = 0
            Dim inf, outf As String
            inf = "input" & i.ToString & ".txt"
            outf = "output" & i.ToString & ".txt"
            Do While File.Exists(Path.Combine(ProblemRoot, inf)) And File.Exists(Path.Combine(ProblemRoot, outf))
                i += 1
                inf = "input" & i.ToString & ".txt"
                outf = "output" & i.ToString & ".txt"
            Loop
            Count = i
            Dim Result As New List(Of Config)
            Do While File.Exists(Path.Combine(ProblemRoot, inf)) And File.Exists(Path.Combine(ProblemRoot, outf))
                i += 1
                Result.Add(New Config(i, inf, outf, _
                    Math.BigMul(Int32.Parse(TimeLimit), 10000), 100 / Count))
            Loop
            Return Result

        End Function

        Private Function LoadTestCase(ByVal ProblemRoot As String, ByVal Config As Config) As TestCase
            Return New LocalTestCase(Config.Index, Config.Weight, Path.Combine(ProblemRoot, Config.InputFileName), Path.Combine(ProblemRoot, Config.AnswerFileName), Config.TimeQuota, m_MemoryQuota)
        End Function
    End Class
End Namespace