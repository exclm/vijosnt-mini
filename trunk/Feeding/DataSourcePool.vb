Imports VijosNT.LocalDb

Namespace Feeding
    Friend Class DataSourcePool
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
            m_HttpListener.Prefixes.Clear()
            m_DataSources = ReadDataSource(m_HttpListener.Prefixes)
            m_HttpListener.Start()
            m_HttpListener.BeginGetContext(AddressOf OnContext, Nothing)
        End Sub

        Private Function ReadDataSource(ByVal Prefixes As HttpListenerPrefixCollection) As IEnumerable(Of DataSourceEntry)
            Dim Result As New List(Of DataSourceEntry)()
            Result.Add(New DataSourceEntry(New LocalDataSource(), String.Empty))
            Using Reader As IDataReader = DataSourceMapping.GetAll()
                While Reader.Read()
                    Dim Entry As DataSourceEntry
                    Entry.IpcAnnouncement = Reader("IpcAnnouncement")
                    If Entry.IpcAnnouncement.Length = 0 Then Entry.IpcAnnouncement = Nothing
                    Dim UriPrefix As String = Reader("HttpAnnouncement")
                    If UriPrefix.Length <> 0 Then
                        Entry.HttpAnnouncement = UriPrefix
                        Prefixes.Add(UriPrefix)
                    Else
                        Entry.HttpAnnouncement = Nothing
                    End If

                    ' TODO: Polling
                    Entry.Timer = Nothing

                    Select Case Reader("ClassName")
                        Case "Vijos"
                            Entry.DataSource = New VijosDataSource(Reader("Parameter"))
                        Case Else
                            EventLog.WriteEntry(My.Resources.ServiceName, "数据源加载失败" & vbCrLf & "未找到类名为 " & Reader("ClassName") & " 的数据源提供程序。", EventLogEntryType.Warning)
                            Continue While
                    End Select
                    Result.Add(Entry)
                End While
            End Using

            Return Result
        End Function

        Public Function Feed(ByVal Limit As Int32) As Int32
            Dim Count As Int32 = 0
            Dim DataSources As IEnumerable(Of DataSourceEntry) = m_DataSources
            For Each DataSource As DataSourceEntry In DataSources
                Dim Current As Int32 = m_Runner.Feed(DataSource.DataSource, Limit)
                Count += Current
                Limit -= Current
                If Limit = 0 Then Exit For
            Next
            Return Count
        End Function

        Public Function Feed(ByVal IpcAnnouncement As String, ByVal Limit As Int32) As Int32
            Dim Count As Int32 = 0
            Dim DataSources As IEnumerable(Of DataSourceEntry) = m_DataSources
            For Each DataSource As DataSourceEntry In DataSources
                If DataSource.IpcAnnouncement IsNot Nothing AndAlso DataSource.IpcAnnouncement = IpcAnnouncement Then
                    Dim Current As Int32 = m_Runner.Feed(DataSource.DataSource, Limit)
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
                Context.Response.ContentType = "text/html"
                Using Writer As New StreamWriter(Context.Response.OutputStream)
                    ' TODO: Parse url, then feed it
                    Writer.WriteLine(Context.Request.Url.ToString())
                End Using
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
