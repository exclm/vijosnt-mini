Imports VijosNT.LocalDb
Imports VijosNT.Utility

Namespace Feeding
    Friend Class DataSourcePool
        Implements IDisposable

        Private m_DataSources As IEnumerable(Of DataSourceEntry)
        Private m_HttpListener As HttpListener
        Private m_Runner As Runner

        Public Sub New(ByVal Runner As Runner)
            m_Runner = Runner
            m_HttpListener = New HttpListener()
            Reload()
        End Sub

        Public Sub Reload()
            m_HttpListener.Stop()
            DisposeDataSource(m_DataSources, m_HttpListener.Prefixes)
            m_DataSources = ReadDataSource(m_HttpListener.Prefixes)
            m_HttpListener.Start()
            m_HttpListener.BeginGetContext(AddressOf OnContext, Nothing)
        End Sub

        Public ReadOnly Property Runner() As Runner
            Get
                Return m_Runner
            End Get
        End Property

        Private Sub DisposeDataSource(ByVal Entries As IEnumerable(Of DataSourceEntry), ByVal Prefixes As HttpListenerPrefixCollection)
            If Entries IsNot Nothing Then
                For Each Entry As DataSourceEntry In Entries
                    Entry.Dispose()
                Next
            End If
            Prefixes.Clear()
        End Sub

        Private Function ReadDataSource(ByVal Prefixes As HttpListenerPrefixCollection) As IEnumerable(Of DataSourceEntry)
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
                    If HttpAnnouncement.Length <> 0 Then
                        Prefixes.Add(HttpAnnouncement & "/")
                    Else
                        HttpAnnouncement = Nothing
                    End If
                    Result.Add(New DataSourceEntry(Me, DataSource, IpcAnnouncement, HttpAnnouncement, DbToLocalInt32(Reader("PollingInterval"))))
                End While
            End Using

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

        Private Sub OnContext(ByVal Result As IAsyncResult)
            Try
                Dim Context As HttpListenerContext = m_HttpListener.EndGetContext(Result)
                m_HttpListener.BeginGetContext(AddressOf OnContext, Nothing)
                Context.Response.ContentType = "text/plain"
                Dim UrlString As String = Context.Request.Url.ToString()
                Dim DataSources As IEnumerable(Of DataSourceEntry) = m_DataSources
                Dim Limit As Int32
                If Not Int32.TryParse(Context.Request.QueryString("Limit"), Limit) OrElse
                    Limit <= 0 Then _
                    Limit = Int32.MaxValue
                Dim Count As Int32 = 0
                For Each Entry As DataSourceEntry In DataSources
                    If Entry.MatchHttpAnnouncement(UrlString) Then
                        Dim Current As Int32 = Entry.Feed(Limit)
                        Count += Current
                        Limit -= Current
                        If Limit = 0 Then Exit For
                    End If
                Next
                Using Writer As New StreamWriter(Context.Response.OutputStream)
                    Writer.WriteLine("VijosNT Mini " & Assembly.GetExecutingAssembly().GetName().Version.ToString())
                    Writer.WriteLine("Feeding " & Count & " record(s)")
                End Using
            Catch ex As Exception
            End Try
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_HttpListener.Stop()
                    DisposeDataSource(m_DataSources, m_HttpListener.Prefixes)
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
