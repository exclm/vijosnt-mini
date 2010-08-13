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

        Public Sub SetWorkerThreads(ByVal Min As Int32, ByVal Max As Int32)
            Dim MinWorkerThreads As Int32, MinCompletionPortThreads As Int32
            Dim MaxWorkerThreads As Int32, MaxCompletionPortThreads As Int32
            ThreadPool.GetMinThreads(MinWorkerThreads, MinCompletionPortThreads)
            ThreadPool.GetMaxThreads(MaxWorkerThreads, MaxCompletionPortThreads)
            If Min <= MaxWorkerThreads Then
                ThreadPool.SetMinThreads(Min, MinCompletionPortThreads)
                ThreadPool.SetMaxThreads(Max, MaxCompletionPortThreads)
            Else
                ThreadPool.SetMaxThreads(Max, MaxCompletionPortThreads)
                ThreadPool.SetMinThreads(Min, MinCompletionPortThreads)
            End If
        End Sub
    End Module
End Namespace
