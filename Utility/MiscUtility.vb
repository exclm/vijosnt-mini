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

        Public Function DbToLocalInt64(ByVal Value As Object) As Int64?
            If IsDBNull(Value) Then
                Return Nothing
            Else
                Return Convert.ToInt64(Value)
            End If
        End Function

        Public Function DbToLocalInt32(ByVal Value As Object) As Int32?
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

        Public Function DbToLocalDate(ByVal Value As Object) As Date?
            If IsDBNull(Value) Then
                Return Nothing
            Else
                Return New Date(Convert.ToInt64(Value))
            End If
        End Function

        Public Function DbToLocalByteArray(ByVal Value As Object) As Byte()
            If IsDBNull(Value) Then
                Return Nothing
            Else
                Return DirectCast(Value, Byte())
            End If
        End Function

        Public Function DbToLocalDouble(ByVal Value As Object) As Double?
            If IsDBNull(Value) Then
                Return Nothing
            Else
                Return Convert.ToDouble(Value)
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
            ElseIf Name.StartsWith("!") Then
                Dim Value As Int64? = DbToLocalInt64(DataReader(Name.Substring(1)))
                If Value.HasValue Then
                    Return (Value.Value \ 10000).ToString() & "ms"
                Else
                    Return Nothing
                End If
            ElseIf Name.StartsWith("@") Then
                Dim Value As Int64? = DbToLocalInt64(DataReader(Name.Substring(1)))
                If Value.HasValue Then
                    Return (Value.Value \ 1024).ToString() & "KB"
                Else
                    Return Nothing
                End If
            ElseIf Name.StartsWith(".") Then
                Return DbToLocalByteArray(DataReader(Name.Substring(1)))
            Else
                Return DataReader(Name)
            End If
        End Function

        Public Sub RefreshListView(ByVal ListView As ListView, ByVal DataReader As IDataReader, ByVal IdColumnName As String, ByVal FirstColumnName As String, ByVal ParamArray RestColumnNames As String())
            ListView.BeginUpdate()
            Try
                Dim SelectedId As Int32 = -1
                With ListView.SelectedItems
                    If .Count <> 0 Then
                        SelectedId = .Item(0).Tag
                    End If
                End With
                With ListView.Items
                    .Clear()
                    While DataReader.Read()
                        Dim Id As Int32 = DataReader(IdColumnName)
                        With .Add(CType(ReadData(DataReader, FirstColumnName), String))
                            .Tag = Id
                            With .SubItems
                                For Each ColumnName As String In RestColumnNames
                                    .Add(CType(ReadData(DataReader, ColumnName), String))
                                Next
                            End With
                            If Id = SelectedId Then
                                .Selected = True
                            End If
                        End With
                    End While
                End With
            Finally
                ListView.EndUpdate()
            End Try
        End Sub

        Public Sub ServiceUnhandledException(ByVal ex As Exception)
            EventLog.WriteEntry(My.Resources.ServiceName, ex.ToString(), EventLogEntryType.Error)
            Environment.Exit(1)
        End Sub

        Public Sub SetDoubleBuffered(ByVal Control As Control)
            If SystemInformation.TerminalServerSession Then
                Return
            End If

            Dim [Property] As PropertyInfo = GetType(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic Or BindingFlags.Instance)
            [Property].SetValue(Control, True, Nothing)
        End Sub

        Public Function FormatEnumString(ByVal Value As String) As String
            Dim Builder As New StringBuilder()
            Dim FirstCharacter As Boolean = True
            For Each Character As Char In Value
                If Not FirstCharacter AndAlso Char.IsUpper(Character) Then _
                    Builder.Append(" "c)
                Builder.Append(Character)
                FirstCharacter = False
            Next
            Return Builder.ToString()
        End Function
    End Module
End Namespace
