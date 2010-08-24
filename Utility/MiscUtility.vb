Namespace Utility
    Module MiscUtility
        Public Function IsBlankChar(ByVal Character As Int32) As Boolean
            Select Case Character
                Case 0, AscW(vbCr(0)), AscW(vbLf(0)), AscW(" "c)
                    Return True
                Case Else
                    Return False
            End Select
        End Function

        Public Sub ReadNextChar(ByVal Reader As TextReader, ByRef Character As Int32, ByRef IsBlank As Boolean)
            Dim NextChar As Int32 = Reader.Read()
            While IsBlankChar(NextChar)
                NextChar = Reader.Read()
                IsBlank = True
            End While
            Character = NextChar
        End Sub

        Public Sub BufferedCopy(ByVal Source As Stream, ByVal Target As Stream)
            BufferedCopy(Source, Target, 4096)
        End Sub

        Public Sub BufferedCopy(ByVal Source As Stream, ByVal Target As Stream, ByVal BufferSize As Int32)
            Dim Buffer As Byte() = New Byte(0 To BufferSize - 1) {}
            Try
                Do
                    Dim Length As Int32 = Source.Read(Buffer, 0, Buffer.Length)
                    If Length = 0 Then _
                        Return
                    Target.Write(Buffer, 0, Length)
                Loop
            Finally
                Target.Close()
                Source.Close()
            End Try
        End Sub

        Public Sub BufferedCopySeek0(ByVal Source As Stream, ByVal Target As Stream)
            BufferedCopySeek0(Source, Target, 4096)
        End Sub

        Public Sub BufferedCopySeek0(ByVal Source As Stream, ByVal Target As Stream, ByVal BufferSize As Int32)
            Dim Buffer As Byte() = New Byte(0 To BufferSize - 1) {}
            Try
                Do
                    Dim Length As Int32 = Source.Read(Buffer, 0, Buffer.Length)
                    If Length = 0 Then _
                        Return
                    Target.Write(Buffer, 0, Length)
                Loop
            Finally
                Target.Close()
                Source.Seek(0, SeekOrigin.Begin)
            End Try
        End Sub

        Public Function RegexSimpleEscape(ByVal Pattern As String) As String
            Dim Builder As New StringBuilder()
            Builder.Append("^(")
            Using Reader As New StringReader(Pattern)
                Dim NextInt As Int32 = Reader.Read()
                While NextInt <> -1
                    Dim NextChar As Char = ChrW(NextInt)
                    If NextChar = "*"c Then
                        Builder.Append(".*")
                    ElseIf NextChar = "?"c Then
                        Builder.Append(".{1}")
                    ElseIf NextChar = ";"c Then
                        Builder.Append("|")
                    Else
                        Builder.Append(Regex.Escape(NextChar))
                    End If
                    NextInt = Reader.Read()
                End While
            End Using
            Builder.Append("$)")
            Return Builder.ToString()
        End Function

        Public Function DbToLocalInt64(ByVal Value As Object) As Nullable(Of Int64)
            If IsDBNull(Value) Then
                Return Nothing
            Else
                Return Convert.ToInt64(Value)
            End If
        End Function

        Public Function DbToLocalInt32(ByVal Value As Object) As Nullable(Of Int32)
            If IsDBNull(Value) Then
                Return Nothing
            Else
                Return Convert.ToInt32(Value)
            End If
        End Function

        Public Function DbToLocalString(ByVal Value As Object) As String
            If IsDBNull(Value) Then
                Return Nothing
            Else
                Return Convert.ToString(Value)
            End If
        End Function

        Public Function DbToLocalDate(ByVal Value As Object) As Nullable(Of Date)
            If IsDBNull(Value) Then
                Return Nothing
            Else
                Return New Date(Convert.ToInt64(Value))
            End If
        End Function

        Public Function ReadData(ByVal DataReader As IDataReader, ByVal Name As String) As Object
            If Name.StartsWith("$") Then
                Return DbToLocalString(DataReader(Name.Substring(1)))
            ElseIf Name.StartsWith("%") Then
                Return DbToLocalInt32(DataReader(Name.Substring(1)))
            ElseIf Name.StartsWith("&") Then
                Return DbToLocalInt64(DataReader(Name.Substring(1)))
            ElseIf Name.StartsWith("#") Then
                Return DbToLocalDate(DataReader(Name.Substring(1)))
            Else
                Return DataReader(Name)
            End If
        End Function
    End Module
End Namespace
