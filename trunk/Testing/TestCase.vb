Imports VijosNT.Win32
Imports VijosNT.Utility

Namespace Testing
    Friend MustInherit Class TestCase
        Protected Structure Context
            Public Sub New(ByVal OutputStream As Stream, ByVal AnswerStream As Stream, ByVal Completion As TestCaseCompletion, ByVal CompletionState As Object)
                Me.OutputStream = OutputStream
                Me.AnswerStream = AnswerStream
                Me.Completion = Completion
                Me.CompletionState = CompletionState
            End Sub

            Dim OutputStream As Stream
            Dim AnswerStream As Stream
            Dim Completion As TestCaseCompletion
            Dim CompletionState As Object
        End Structure

        Public Overridable ReadOnly Property TimeQuota() As Nullable(Of Int64)
            Get
                Return Nothing
            End Get
        End Property

        Public Overridable ReadOnly Property MemoryQuota() As Nullable(Of Int64)
            Get
                Return Nothing
            End Get
        End Property

        Public Overridable Function OpenInputStream() As Stream
            Throw New NotImplementedException()
        End Function

        Public Overridable Function OpenInput() As KernelObject
            Using Pipe As New Pipe()
                StreamPipe.Connect(OpenInputStream(), Pipe.GetWriteStream())
                Return Pipe.GetReadHandle()
            End Using
        End Function

        Public Function OpenOutput(ByVal Completion As TestCaseCompletion, ByVal State As Object) As KernelObject
            Using Pipe As New Pipe()
                ThreadPool.UnsafeQueueUserWorkItem(AddressOf JudgeWorker, New Context(Pipe.GetReadStream(), OpenAnswerStream(), Completion, State))
                Return Pipe.GetWriteHandle()
            End Using
        End Function

        Protected Overridable Function OpenAnswerStream() As Stream
            Using Answer As KernelObject = OpenAnswer()
                Return (New FileStream(New SafeFileHandle(Answer.Duplicate(), True), FileAccess.Read))
            End Using
        End Function

        Protected Overridable Function OpenAnswer() As KernelObject
            Throw New NotImplementedException()
        End Function

        Protected Overridable Function LoadJudger() As Object
            Return New DefaultJudger()
        End Function

        Protected Overridable Sub JudgeWorker(ByVal State As Context)
            Dim Result As TestCaseResult

            Result.State = State.CompletionState
            Result.AnswerCorrect = False
            Result.Exception = Nothing

            Try
                Result.AnswerCorrect = LoadJudger().Compare(State.OutputStream, State.AnswerStream)
            Catch ex As Exception
                Result.Exception = ex
            End Try

            Try
                State.Completion.Invoke(Result)
            Catch ex As Exception
                ' eat it
            End Try
        End Sub
    End Class
End Namespace
