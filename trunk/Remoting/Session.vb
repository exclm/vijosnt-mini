Imports VijosNT.Feeding
Imports VijosNT.Notification
Imports VijosNT.Utility

Namespace Remoting
    Friend Class Session
        Implements IDisposable

        Private m_Stream As Stream
        Private m_Buffer As Byte()
        Private m_Runner As Runner

        Public Sub New(ByVal Stream As Stream, ByVal Runner As Runner)
            m_Stream = Stream
            m_Buffer = New Byte(0 To 4095) {}
            m_Runner = Runner
            m_Stream.BeginRead(m_Buffer, 0, m_Buffer.Length, AddressOf OnRead, Nothing)
            Notifier.Register("RunnerStatusChanged", AddressOf OnRunnerStatusChanged)
            Notifier.Register("LocalRecordChanged", AddressOf OnLocalRecordChanged)
        End Sub

        Private Sub OnRunnerStatusChanged(ByVal Param As Object)
            Try
                Write(ServerMessage.RunnerStatusChanged, DirectCast(Param, Boolean))
            Catch ex As Exception
                OnDisconnected()
                Me.Dispose()
            End Try
        End Sub

        Private Sub OnLocalRecordChanged(ByVal Param As Object)
            Try
                Write(ServerMessage.LocalRecordChanged)
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

        Public Sub Write(ByVal ParamArray Params As Object())
            Using Stream As New MemoryStream()
                Using Writer As New BinaryWriter(Stream)
                    For Each Param As Object In Params
                        Writer.Write(Param)
                    Next
                End Using
                Dim Buffer As Byte() = Stream.ToArray()
                m_Stream.BeginWrite(Buffer, 0, Buffer.Length, AddressOf OnWrite, Nothing)
            End Using
        End Sub

        Private Sub OnWrite(ByVal Result As IAsyncResult)
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
                    Case ClientMessage.FeedDataSource
                        OnFeedDataSource(Reader.ReadString())
                End Select
            End Using
        End Sub

        Private Sub OnReloadCompiler()
            Try
                m_Runner.ReloadCompiler()
            Catch ex As Exception
                ServiceUnhandledException(ex)
            End Try
        End Sub

        Private Sub OnReloadTestSuite()
            Try
                m_Runner.ReloadTestSuite()
            Catch ex As Exception
                ServiceUnhandledException(ex)
            End Try
        End Sub

        Private Sub OnReloadExecutor()
            Try
                m_Runner.ReloadExecutor()
            Catch ex As Exception
                ServiceUnhandledException(ex)
            End Try
        End Sub

        Private Sub OnReloadDataSource()
            Try
                m_Runner.ReloadDataSource()
            Catch ex As Exception
                ServiceUnhandledException(ex)
            End Try
        End Sub

        Private Sub OnFeedDataSource(ByVal DataSourceName As String)
            Try
                m_Runner.Feed(DataSourceName, Int32.MaxValue)
            Catch ex As Exception
                ServiceUnhandledException(ex)
            End Try
        End Sub

        Private Sub OnDisconnected()
            Notifier.Unregister("RunnerStatusChanged", AddressOf OnRunnerStatusChanged)
            Notifier.Unregister("LocalRecordChanged", AddressOf OnLocalRecordChanged)
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
