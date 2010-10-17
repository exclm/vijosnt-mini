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
            m_DataSources = ReadDataSource()
            m_HttpServer.Start(ReadPrefixes(m_DataSources))
        End Sub

        Public ReadOnly Property Runner() As Runner
            Get
                Return m_Runner
            End Get
        End Property

        Private Function ReadDataSource() As IEnumerable(Of DataSourceEntry)
            Dim Result As New List(Of DataSourceEntry)()
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
                    Dim HttpAnnouncement As String = Reader("HttpAnnouncement")
                    If HttpAnnouncement.Length = 0 Then HttpAnnouncement = Nothing
                    Result.Add(New DataSourceEntry(DataSource, HttpAnnouncement))
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

        Private Sub RemoteFeed(ByVal DataSource As DataSourceBase, ByVal Queries As SortedDictionary(Of String, String))
            Dim Id = Int32.Parse(Queries("Id"))
            Dim Record = DataSource.Take(Id)
            If Not m_Runner.Queue(DataSource.Namespace, Record.FileName, New MemoryStream(Encoding.Default.GetBytes(Record.SourceCode)), _
                Sub(Result As TestResult)
                    Try
                        DataSource.Untake(Id, Result)
                    Catch ex As Exception
                        ' Eat it
                    End Try
                End Sub) Then
                DataSource.Untake(Id)
            End If
        End Sub

        Private Function OnContext(ByVal State As Object, ByVal Session As MiniHttpServer.Session, ByVal Queries As SortedDictionary(Of String, String)) As Boolean
            Dim Succeeded = True
            Try
                RemoteFeed(DirectCast(State, DataSourceEntry).DataSource, Queries)
            Catch ex As Exception
                Succeeded = False
            End Try

            Using Stream As New MemoryStream()
                Using Writer As New XmlTextWriter(Stream, Encoding.UTF8)
                    Writer.WriteStartDocument()
                    Writer.WriteStartElement("FeedResult")
                    Writer.WriteElementString("Version", Assembly.GetExecutingAssembly().GetName().Version.ToString())
                    Writer.WriteElementString("Succeeded", Succeeded.ToString())
                    Writer.WriteEndElement()
                    Writer.WriteEndDocument()
                End Using

                Try
                    Return Session.Write("text/xml", Stream.ToArray())
                Catch ex As Exception
                    Return False
                End Try
            End Using
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_HttpServer.Stop()
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
