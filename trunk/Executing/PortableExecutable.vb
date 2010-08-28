Namespace Executing
    Friend Class PortableExecutable
        Implements IDisposable

        Private m_Stream As Stream
        Private m_Stack As Stack(Of Int64)

        Public Sub New(ByVal ApplicationName As String)
            m_Stream = New FileStream(ApplicationName, FileMode.Open, FileAccess.Read, FileShare.Read)
            m_Stack = New Stack(Of Int64)()
        End Sub

        Public ReadOnly Property DosHeader() As Int32
            Get
                Push()
                Try
                    SeekAbsolute(0)
                    If ReadInt16() <> &H5A4DS Then _
                        Throw New Exception("not a valid PE file")
                    Return 0
                Finally
                    Pop()
                End Try
            End Get
        End Property

        Public ReadOnly Property NtHeader() As Int32
            Get
                Push()
                Try
                    SeekAbsolute(DosHeader)
                    SeekRelative(&H3C)
                    Dim Result As Int32 = ReadInt32()
                    SeekAbsolute(Result)
                    If ReadInt32() <> &H4550 Then _
                        Throw New Exception("not a valid PE file")
                    Return Result
                Finally
                    Pop()
                End Try
            End Get
        End Property

        Public ReadOnly Property IsPE32Plus() As Boolean
            Get
                Push()
                Try
                    SeekAbsolute(NtHeader)
                    SeekRelative(&H4)
                    Dim Machine As Int16 = ReadInt16()
                    SeekRelative(&HE)
                    Dim SizeOfOptionalHeader As Int16 = ReadInt16()
                    SeekRelative(&H2)
                    Dim Magic As Int16 = ReadInt16()
                    If Machine = &H8664S AndAlso SizeOfOptionalHeader = &HF0S AndAlso Magic = &H20BS Then
                        Return True
                    ElseIf Machine = &H14CS AndAlso SizeOfOptionalHeader = &HE0S AndAlso Magic = &H10BS Then
                        Return False
                    Else
                        Throw New Exception("unknown PE type")
                    End If
                Finally
                    Pop()
                End Try
            End Get
        End Property

        Public ReadOnly Property NumberOfSections() As Int32
            Get
                Push()
                Try
                    SeekAbsolute(NtHeader)
                    SeekRelative(&H6)
                    Return ReadInt16()
                Finally
                    Pop()
                End Try
            End Get
        End Property

        Public ReadOnly Property SectionAlignment() As Int32
            Get
                Push()
                Try
                    ' PE32 and PE32+ share the same offset
                    SeekAbsolute(NtHeader + &H38)
                    Return ReadInt32()
                Finally
                    Pop()
                End Try
            End Get
        End Property

        Public ReadOnly Property SectionHeader(ByVal Index As Int32) As Int32
            Get
                Push()
                Try
                    Dim Result As Int32 = NtHeader
                    If IsPE32Plus Then
                        Result += &H108 + &H28 * Index
                    Else
                        Result += &HF8 + &H28 * Index
                    End If
                    Return Result
                Finally
                    Pop()
                End Try
            End Get
        End Property

        Public ReadOnly Property SectionName(ByVal Index As Int32) As Int64
            Get
                Push()
                Try
                    SeekAbsolute(SectionHeader(Index))
                    Return ReadInt64()
                Finally
                    Pop()
                End Try
            End Get
        End Property

        Public ReadOnly Property SectionVirtualSize(ByVal Index As Int32) As Int32
            Get
                Push()
                Try
                    SeekAbsolute(SectionHeader(Index) + &H8)
                    Return ReadInt32()
                Finally
                    Pop()
                End Try
            End Get
        End Property

        Public ReadOnly Property SectionVirtualSizeAligned(ByVal Index As Int32) As Int32
            Get
                Dim Alignment As Int32 = SectionAlignment
                Return (SectionVirtualSize(Index) + (Alignment - 1)) And Not (Alignment - 1)
            End Get
        End Property

        Public ReadOnly Property VirtualSize() As Int64
            Get
                Dim Sections As Int32 = NumberOfSections
                Dim Result As Int64 = 0
                For Index As Int32 = 0 To Sections - 1
                    Result += SectionVirtualSizeAligned(Index)
                Next
                Return Result
            End Get
        End Property

        Private Sub Push()
            m_Stack.Push(m_Stream.Position)
        End Sub

        Private Sub SeekAbsolute(ByVal Offset As Int32)
            m_Stream.Seek(Offset, SeekOrigin.Begin)
        End Sub

        Private Sub SeekRelative(ByVal Offset As Int32)
            m_Stream.Seek(Offset, SeekOrigin.Current)
        End Sub

        Private Function ReadInt16() As Int16
            Dim Buffer As Byte() = New Byte(0 To 1) {}
            If m_Stream.Read(Buffer, 0, 2) <> 2 Then _
                Throw New IOException()
            Return BitConverter.ToInt16(Buffer, 0)
        End Function

        Private Function ReadInt32() As Int32
            Dim Buffer As Byte() = New Byte(0 To 3) {}
            If m_Stream.Read(Buffer, 0, 4) <> 4 Then _
                Throw New IOException()
            Return BitConverter.ToInt32(Buffer, 0)
        End Function

        Private Function ReadInt64() As Int64
            Dim Buffer As Byte() = New Byte(0 To 7) {}
            If m_Stream.Read(Buffer, 0, 8) <> 8 Then _
                Throw New IOException()
            Return BitConverter.ToInt64(Buffer, 0)
        End Function

        Private Sub Pop()
            m_Stream.Position = m_Stack.Pop()
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_Stream.Close()
                End If
            End If
            Me.disposedValue = True
        End Sub

        ' Visual Basic 添加此代码是为了正确实现可处置模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace
