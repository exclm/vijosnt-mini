Imports VijosNT.Feeding
Imports VijosNT.Utility

Namespace Remoting
    Friend Class Session
        Implements IDisposable

        Private m_Stream As Stream
        Private m_Buffer As Byte()

        Public Sub New(ByVal Stream As Stream)
            m_Stream = Stream
            m_Buffer = New Byte(0 To 4095) {}
            m_Stream.BeginRead(m_Buffer, 0, m_Buffer.Length, AddressOf OnRead, Nothing)
            AddHandler Runner.Singleton.RunnerStatusChanged, AddressOf OnRunnerStatusChanged
        End Sub

        Private Sub OnRunnerStatusChanged(ByVal Running As Boolean)
            Try
                Write(ServerMessage.RunnerStatusChanged, Running)
            Catch ex As Exception
                OnDisconnected()
                Me.Dispose()
            End Try
        End Sub

        Private Sub OnRead(ByVal Result As IAsyncResult)
            Try
                Dim Length As Int32 = m_Stream.EndRead(Result)
                If Length = 0 Then
                    OnDisconnected()
                    Me.Dispose()
                End If
                OnReceived(Length)
                m_Stream.BeginRead(m_Buffer, 0, m_Buffer.Length, AddressOf OnRead, Nothing)
            Catch ex As Exception
                OnDisconnected()
                Me.Dispose()
            End Try
        End Sub

        Public Function Write(ByVal ParamArray Params As Object()) As Boolean
            Using Stream As New MemoryStream()
                Using Writer As New BinaryWriter(Stream)
                    For Each Param As Object In Params
                        Writer.Write(Param)
                    Next
                End Using
                Return WriteBuffer(Stream.ToArray())
            End Using
        End Function

        Private Function WriteBuffer(ByVal Buffer As Byte()) As Boolean
            Try
                m_Stream.BeginWrite(Buffer, 0, Buffer.Length, AddressOf OnWriteBuffer, Nothing)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Private Sub OnWriteBuffer(ByVal Result As IAsyncResult)
            Try
                m_Stream.EndWrite(Result)
            Catch ex As Exception
                ' Do nothing
            End Try
        End Sub

        Private Sub OnReceived(ByVal Length As Int32)
            Using Stream As New MemoryStream(m_Buffer, 0, Length), _
                Reader As New BinaryReader(Stream)
                Dim Message As ClientMessage = Reader.ReadInt32()
                Select Case Message
                    Case ClientMessage.ReloadCompiler
                        OnReloadCompiler()
                    Case ClientMessage.ReloadTestSuite
                        OnReloadTestSuite()
                    Case ClientMessage.ReloadExecutor
                        OnReloadExecutor()
                    Case ClientMessage.ReloadDataSource
                        OnReloadDataSource()
                    Case ClientMessage.DirectFeed
                        OnDirectFeed(Reader.ReadInt32(), Reader.ReadString(), Reader.ReadString(), Reader.ReadString())
                    Case ClientMessage.DirectFeed2
                        OnDirectFeed2(Reader.ReadInt32(), Reader.ReadString(), Reader.ReadString(), Reader.ReadString())
                End Select
            End Using
        End Sub

        Private Sub OnReloadCompiler()
            Try
                Runner.Singleton.ReloadCompiler()
            Catch ex As Exception
                ServiceUnhandledException(ex)
            End Try
        End Sub

        Private Sub OnReloadTestSuite()
            Try
                Runner.Singleton.ReloadTestSuite()
            Catch ex As Exception
                ServiceUnhandledException(ex)
            End Try
        End Sub

        Private Sub OnReloadExecutor()
            Try
                Runner.Singleton.ReloadExecutor()
            Catch ex As Exception
                ServiceUnhandledException(ex)
            End Try
        End Sub

        Private Sub OnReloadDataSource()
            Try
                Runner.Singleton.ReloadDataSource()
            Catch ex As Exception
                ServiceUnhandledException(ex)
            End Try
        End Sub

        Private Sub DirectFeedSendReply(ByVal StateId As Int32, ByVal Result As TestResult)
            Using Stream As New MemoryStream()
                Using Writer As New BinaryWriter(Stream)
                    Writer.Write(ServerMessage.DirectFeedReply)
                    Writer.Write(StateId)
                    Writer.Write(Result.Flag)
                    If Result.Warning Is Nothing Then
                        Writer.Write(String.Empty)
                    Else
                        Writer.Write(Result.Warning)
                    End If
                    Writer.Write(Result.Score)
                    Writer.Write(Result.TimeUsage)
                    Writer.Write(Result.MemoryUsage)
                    If Result.Entries IsNot Nothing Then
                        For Each Entry As TestResultEntry In Result.Entries
                            Writer.Write(Entry.Index)
                            Writer.Write(Entry.Flag)
                            Writer.Write(Entry.Score)
                            Writer.Write(Entry.TimeUsage)
                            Writer.Write(Entry.MemoryUsage)
                            If Entry.Warning Is Nothing Then
                                Writer.Write(String.Empty)
                            Else
                                Writer.Write(Entry.Warning)
                            End If
                        Next
                    End If
                End Using
                WriteBuffer(Stream.ToArray())
            End Using
        End Sub

        Private Sub OnDirectFeedInternal(ByVal StateId As Int32, ByVal [Namespace] As String, ByVal FileName As String, ByVal SourceCodeStream As Stream)
            Try
                Runner.Singleton.Queue([Namespace], FileName, SourceCodeStream, _
                    Sub(Result As TestResult)
                        DirectFeedSendReply(StateId, Result)
                    End Sub)
            Catch ex As Exception
                ServiceUnhandledException(ex)
            End Try
        End Sub

        Private Sub OnDirectFeed(ByVal StateId As Int32, ByVal [Namespace] As String, ByVal FileName As String, ByVal SourceCode As String)
            OnDirectFeedInternal(StateId, [Namespace], FileName, New MemoryStream(Encoding.Default.GetBytes(SourceCode), False))
        End Sub

        Private Sub OnDirectFeed2(ByVal StateId As Int32, ByVal [Namespace] As String, ByVal FileName As String, ByVal SourceCodePath As String)
            Try
                OnDirectFeedInternal(StateId, [Namespace], FileName, New FileStream(SourceCodePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            Catch ex As IOException
                DirectFeedSendReply(StateId, New TestResult(TestResultFlag.None, "未找到提交的代码", 0, 0, 0, Nothing))
            End Try
        End Sub

        Private Sub OnDisconnected()
            RemoveHandler Runner.Singleton.RunnerStatusChanged, AddressOf OnRunnerStatusChanged
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                m_Stream.Dispose()
            End If
            Me.disposedValue = True
        End Sub

        Protected Overrides Sub Finalize()
            ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
            Dispose(False)
            MyBase.Finalize()
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
