Imports VijosNT.Utility

Namespace Testing
    Public Class DefaultJudger
        Public Function Compare(ByVal Output As Stream, ByVal Answer As Stream) As Boolean
            Using OutputReader As New StreamReader(Output), _
                AnswerReader As New StreamReader(Answer)

                Dim OutputCharacter As Int32
                Dim OutputIsBlank As Boolean
                Dim AnswerCharacter As Int32
                Dim AnswerIsBlank As Boolean

                ReadNextChar(OutputReader, OutputCharacter, OutputIsBlank)
                ReadNextChar(AnswerReader, AnswerCharacter, AnswerIsBlank)

                If OutputCharacter <> AnswerCharacter Then Return False
                If OutputCharacter = -1 Then Return True

                Do
                    ReadNextChar(OutputReader, OutputCharacter, OutputIsBlank)
                    ReadNextChar(AnswerReader, AnswerCharacter, AnswerIsBlank)

                    If OutputCharacter <> AnswerCharacter Then Return False
                    If OutputCharacter = -1 AndAlso AnswerCharacter = -1 Then Return True
                    If OutputIsBlank <> AnswerIsBlank Then Return False
                Loop
            End Using
        End Function
    End Class
End Namespace