Imports VijosNT.Utility

Namespace Feeding
    Friend Class TestResult
        Public Sub New(ByVal State As Object, ByVal Flag As TestResultFlag, ByVal Warning As String, ByVal Score As Int32, ByVal TimeUsage As Int64, ByVal MemoryUsage As Int64, ByVal Entries As IEnumerable(Of TestResultEntry))
            Me.State = State
            Me.Flag = Flag
            Me.Warning = Warning
            Me.Score = Score
            Me.TimeUsage = TimeUsage
            Me.MemoryUsage = MemoryUsage
            Me.Entries = Entries
        End Sub

        Public State As Object
        Public Flag As TestResultFlag
        Public Warning As String
        Public Score As Int32
        Public TimeUsage As Int64
        Public MemoryUsage As Int64
        Public Entries As IEnumerable(Of TestResultEntry)

        Public Sub AppendToBuilder(ByVal Builder As StringBuilder, ByVal Markup As Markup)
            Dim Markups As String()

            Select Case Markup
                Case Markup.Ubb
                    Markups = New String() {"[color=#ff0000]", "[color=#0000ff]", "[/color]", vbCrLf}
                Case Markup.Html
                    Markups = New String() {"<font color=""#ff0000"">", "<font color=""#0000ff"">", "</font>", "<br>"}
                Case Else
                    Markups = New String() {String.Empty, String.Empty, String.Empty, vbCrLf}
            End Select

            If Warning IsNot Nothing AndAlso Warning.Length <> 0 Then
                Builder.Append(Warning)
                Builder.Append(Markups(3))
            End If

            Dim TimeLimitExceeded As Boolean = False
            Dim MemoryLimitExceeded As Boolean = False

            If Entries IsNot Nothing Then
                For Each Entry As TestResultEntry In Entries
                    Builder.Append("#" & Entry.Index.ToString("00") & ": ")
                    If Entry.Flag = TestResultFlag.Accepted Then
                        Builder.Append(Markups(0))
                        Builder.Append("Accepted")
                        Builder.Append(Markups(2))
                    Else
                        Builder.Append(Markups(1))
                        Builder.Append(FormatEnumString(Entry.Flag.ToString()))
                        Builder.Append(Markups(2))
                    End If
                    If Entry.Flag = TestResultFlag.TimeLimitExceeded Then
                        Builder.Append(" (?, " & (Entry.MemoryUsage \ 1024).ToString() & "KB)")
                        Builder.Append(Markups(3))
                        TimeLimitExceeded = True
                    ElseIf Entry.Flag = TestResultFlag.MemoryLimitExceeded Then
                        Builder.Append(" (" & (Entry.TimeUsage \ 10000).ToString() & "ms, ?)")
                        Builder.Append(Markups(3))
                        MemoryLimitExceeded = True
                    Else
                        Builder.Append(" (" & (Entry.TimeUsage \ 10000).ToString() & "ms, " & (Entry.MemoryUsage \ 1024).ToString() & "KB)")
                        Builder.Append(Markups(3))
                    End If
                    If Entry.Warning IsNot Nothing AndAlso Entry.Warning.Length <> 0 Then
                        Builder.Append(Entry.Warning)
                        Builder.Append(Markups(3))
                    End If
                Next
                Builder.Append(Markups(3))
            End If

            If Flag = TestResultFlag.Accepted Then
                Builder.Append(Markups(0))
                Builder.Append("Accepted")
                Builder.Append(Markups(2))
            Else
                Builder.Append(Markups(1))
                Builder.Append(FormatEnumString(Flag.ToString()))
                Builder.Append(Markups(2))
            End If
            Builder.Append(" / " & Score & " / ")

            If TimeLimitExceeded Then
                Builder.Append("? / ")
            Else
                Builder.Append((TimeUsage \ 10000).ToString() & "ms / ")
            End If

            If MemoryLimitExceeded Then
                Builder.Append("?")
            Else
                Builder.Append((MemoryUsage \ 1024).ToString() & "KB")
            End If

            Builder.Append(Markups(3))
        End Sub
    End Class
End Namespace
