Namespace Executing
    Friend Class PortableExecutable
        Implements IDisposable

        Private m_Stream As Stream
        Private m_Stack As Stack(Of Int64)
        Private m_DosHeader As Nullable(Of Int32)
        Private m_NtHeader As Nullable(Of Int32)
        Private m_IsPE32Plus As Nullable(Of Boolean)
        Private m_NumberOfSections As Nullable(Of Int32)
        Private m_SectionAlignment As Nullable(Of Int32)
        Private m_VirtualSize As Nullable(Of Int64)

        Public Sub New(ByVal ApplicationName As String)
            m_Stream = New FileStream(ApplicationName, FileMode.Open, FileAccess.Read, FileShare.Read)
            m_Stack = New Stack(Of Int64)()
            m_DosHeader = Nothing
            m_NtHeader = Nothing
            m_IsPE32Plus = Nothing
            m_NumberOfSections = Nothing
            m_SectionAlignment = Nothing
            m_VirtualSize = Nothing
        End Sub

        Public ReadOnly Property DosHeader() As Int32
            Get
                If m_DosHeader.HasValue Then Return m_DosHeader.Value
                Push()
                Try
                    SeekAbsolute(0)
                    If ReadInt16() <> &H5A4DS Then _
                        Throw New Exception("not a valid PE file")
                    m_DosHeader = 0
                    Return 0
                Finally
                    Pop()
                End Try
            End Get
        End Property

        Public ReadOnly Property NtHeader() As Int32
            Get
                If m_NtHeader.HasValue Then Return m_NtHeader.Value
                Push()
                Try
                    SeekAbsolute(DosHeader)
                    SeekRelative(&H3C)
                    Dim Result As Int32 = ReadInt32()
                    SeekAbsolute(Result)
                    If ReadInt32() <> &H4550 Then _
                        Throw New Exception("not a valid PE file")
                    m_NtHeader = Result
                    Return Result
                Finally
                    Pop()
                End Try
            End Get
        End Property

        Public ReadOnly Property IsPE32Plus() As Boolean
            Get
                If m_IsPE32Plus.HasValue Then Return m_IsPE32Plus.Value
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
                        m_IsPE32Plus = True
                        Return True
                    ElseIf Machine = &H14CS AndAlso SizeOfOptionalHeader = &HE0S AndAlso Magic = &H10BS Then
                        m_IsPE32Plus = False
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
                If m_NumberOfSections.HasValue Then Return m_NumberOfSections.Value
                Push()
                Try
                    SeekAbsolute(NtHeader)
                    SeekRelative(&H6)
                    Dim Result As Int32 = ReadInt16()
                    m_NumberOfSections = Result
                    Return Result
                Finally
                    Pop()
                End Try
            End Get
        End Property

        Public ReadOnly Property SectionAlignment() As Int32
            Get
                If m_SectionAlignment.HasValue Then Return m_SectionAlignment.Value
                Push()
                Try
                    ' PE32 and PE32+ share the same offset
                    SeekAbsolute(NtHeader + &H38)
                    Dim Result As Int32 = ReadInt32()
                    m_SectionAlignment = Result
                    Return Result
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
                If m_VirtualSize.HasValue Then Return m_VirtualSize.Value
                Dim Result As Int64 = 0
                For Index As Int32 = 0 To NumberOfSections - 1
                    Result += SectionVirtualSizeAligned(Index)
                Next
                m_VirtualSize = Result
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
