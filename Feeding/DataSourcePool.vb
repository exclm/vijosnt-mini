Imports VijosNT.LocalDb
Imports VijosNT.Utility

Namespace Feeding
    Friend Class DataSourcePool
        Implements IDisposable

        Private m_DataSources As IEnumerable(Of DataSourceEntry)
        Private m_HttpServer As MiniHttpServer
        Private m_Runner As Runner

        Public Sub New(ByVal Runner As Runner)
            m_Runner = Runner
            m_HttpServer = New MiniHttpServer()
            Reload()
        End Sub

        Public Sub Reload()
            m_HttpServer.Stop()
            If m_DataSources IsNot Nothing Then _
                DisposeDataSource(m_DataSources)
            m_DataSources = ReadDataSource()
            m_HttpServer.Start(ReadPrefixes(m_DataSources))
        End Sub

        Public ReadOnly Property Runner() As Runner
            Get
                Return m_Runner
            End Get
        End Property

        Private Sub DisposeDataSource(ByVal Entries As IEnumerable(Of DataSourceEntry))
            For Each Entry As DataSourceEntry In Entries
                Entry.Dispose()
            Next
        End Sub

        Private Function ReadDataSource() As IEnumerable(Of DataSourceEntry)
            Dim Result As New List(Of DataSourceEntry)()
            Result.Add(New DataSourceEntry(Me, New LocalDataSource(String.Empty), String.Empty, Nothing, Nothing))
            Using Reader As IDataReader = DataSourceMapping.GetAll()
                While Reader.Read()
                    Dim DataSource As DataSourceBase
                    Select Case Reader("ClassName")
                        Case "Vijos"
                            DataSource = New VijosDataSource(Reader("Namespace"), Reader("Parameter"))
                        Case "_22OJS"
                            DataSource = New _22OJSDataSource(Reader("Namespace"), Reader("Parameter"))
                        Case Else
                            EventLog.WriteEntry(My.Resources.ServiceName, "数据源加载失败" & vbCrLf & "未找到类名为 " & Reader("ClassName") & " 的数据源提供程序。", EventLogEntryType.Warning)
                            Continue While
                    End Select
                    Dim IpcAnnouncement As String = Reader("IpcAnnouncement")
                    If IpcAnnouncement.Length = 0 Then IpcAnnouncement = Nothing
                    Dim HttpAnnouncement As String = Reader("HttpAnnouncement")
                    If HttpAnnouncement.Length = 0 Then HttpAnnouncement = Nothing
                    Result.Add(New DataSourceEntry(Me, DataSource, IpcAnnouncement, HttpAnnouncement, DbToLocalInt32(Reader("PollingInterval"))))
                End While
            End Using

            Return Result
        End Function

        Private Function ReadPrefixes(ByVal Entries As IEnumerable(Of DataSourceEntry)) As IEnumerable(Of KeyValuePair(Of String, MiniHttpServer.Context))
            Dim Result As New List(Of KeyValuePair(Of String, MiniHttpServer.Context))()

            For Each Entry In Entries
                Dim HttpAnnouncement = Entry.HttpAnnouncement
                If HttpAnnouncement IsNot Nothing Then
                    Result.Add(New KeyValuePair(Of String, MiniHttpServer.Context)(HttpAnnouncement, New MiniHttpServer.Context(AddressOf OnContext, Entry)))
                End If
            Next

            Return Result
        End Function

        Public Function Feed(ByVal Limit As Int32) As Int32
            Dim Count As Int32 = 0
            Dim DataSources As IEnumerable(Of DataSourceEntry) = m_DataSources
            For Each Entry As DataSourceEntry In DataSources
                Dim Current As Int32 = Entry.Feed(Limit)
                Count += Current
                Limit -= Current
                If Limit = 0 Then Exit For
            Next
            Return Count
        End Function

        Public Function Feed(ByVal IpcAnnouncement As String, ByVal Limit As Int32) As Int32
            Dim Count As Int32 = 0
            Dim DataSources As IEnumerable(Of DataSourceEntry) = m_DataSources
            For Each Entry As DataSourceEntry In DataSources
                If Entry.MatchIpcAnnouncement(IpcAnnouncement) Then
                    Dim Current As Int32 = Entry.Feed(Limit)
                    Count += Current
                    Limit -= Current
                    If Limit = 0 Then Exit For
                End If
            Next
            Return Count
        End Function

        Private Function OnContext(ByVal State As Object, ByVal Session As MiniHttpServer.Session, ByVal Queries As SortedDictionary(Of String, String)) As Boolean
            Dim Limit = 0

            If Not Queries.ContainsKey("Limit") OrElse _
                Not Int32.TryParse(Queries("Limit"), Limit) OrElse
                Limit <= 0 Then _
                Limit = Int32.MaxValue

            Dim FeedCount As Int32
            Try
                FeedCount = DirectCast(State, DataSourceEntry).Feed(Limit)
            Catch ex As Exception
                EventLog.WriteEntry(My.Resources.ServiceName, ex.ToString(), EventLogEntryType.Warning)
                Return False
            End Try

            Dim Builder = New StringBuilder()
            Builder.AppendLine("VijosNT Mini " & Assembly.GetExecutingAssembly().GetName().Version.ToString())
            Builder.AppendLine("Feeding " & FeedCount.ToString() & " record(s)")

            Try
                Return Session.Write("text/plain", Builder.ToString())
            Catch ex As Exception
                Return False
            End Try
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_HttpServer.Stop()
                    DisposeDataSource(m_DataSources)
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
