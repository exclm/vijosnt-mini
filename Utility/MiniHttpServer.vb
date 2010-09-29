Namespace Utility
    Friend Class MiniHttpServer
        Public Delegate Function RequestCallback(ByVal State As Object, ByVal Session As Session, ByVal Queries As SortedDictionary(Of String, String)) As Boolean

        Public Class Context
            Private m_Callback As RequestCallback
            Private m_State As Object

            Public Sub New(ByVal Callback As RequestCallback, ByVal State As Object)
                m_Callback = Callback
                m_State = State
            End Sub

            Public Function Invoke(ByVal Session As Session, ByVal Queries As SortedDictionary(Of String, String)) As Boolean
                Return m_Callback.Invoke(m_State, Session, Queries)
            End Function
        End Class

        Public Class Listener
            Private m_Server As MiniHttpServer
            Private m_Listener As TcpListener
            Private m_ListenerV6 As TcpListener
            Private m_Addresses As Dictionary(Of IPAddress, SortedDictionary(Of String, SortedDictionary(Of String, Context)))

            Public Sub New(ByVal Server As MiniHttpServer, ByVal Address As IPAddress, ByVal Port As Int32, _
                ByVal Addresses As Dictionary(Of IPAddress, SortedDictionary(Of String, SortedDictionary(Of String, Context))))

                m_Server = Server
                m_Listener = New TcpListener(Address, Port)
                If Address.Equals(IPAddress.Any) Then
                    m_ListenerV6 = New TcpListener(IPAddress.IPv6Any, Port)
                End If
                m_Addresses = Addresses
            End Sub

            Public Sub New(ByVal Server As MiniHttpServer, ByVal Address As IPAddress, ByVal Port As Int32, _
                ByVal Hosts As SortedDictionary(Of String, SortedDictionary(Of String, Context)))

                Me.New(Server, Address, Port, New Dictionary(Of IPAddress, SortedDictionary(Of String, SortedDictionary(Of String, Context))))
                m_Addresses.Add(Address, Hosts)
            End Sub

            Public Sub Start()
                m_Listener.Start()
                m_Listener.BeginAcceptTcpClient(AddressOf OnAccept, m_Listener)
                If m_ListenerV6 IsNot Nothing Then
                    m_ListenerV6.Start()
                    m_ListenerV6.BeginAcceptTcpClient(AddressOf OnAccept, m_ListenerV6)
                End If
            End Sub

            Public Sub [Stop]()
                m_Listener.Stop()
                If m_ListenerV6 IsNot Nothing Then _
                    m_ListenerV6.Stop()
            End Sub

            Private Sub OnAccept(ByVal Result As IAsyncResult)
                Dim Listener = DirectCast(Result.AsyncState, TcpListener)
                Dim Client = Listener.EndAcceptTcpClient(Result)
                Dim Session = New Session(Me, Client)
                Listener.BeginAcceptTcpClient(AddressOf OnAccept, Listener)
            End Sub

            Public ReadOnly Property Server() As MiniHttpServer
                Get
                    Return m_Server
                End Get
            End Property

            Public ReadOnly Property Addresses() As Dictionary(Of IPAddress, SortedDictionary(Of String, SortedDictionary(Of String, Context)))
                Get
                    Return m_Addresses
                End Get
            End Property
        End Class

        Public Class Session
            Private m_Listener As Listener
            Private m_Client As TcpClient
            Private m_Buffer As Byte()
            Private m_MemoryStream As MemoryStream
            Private m_Automaton As Int32
            Private m_KeepAlive As Boolean

            Public Sub New(ByVal Listener As Listener, ByVal Client As TcpClient)
                m_Listener = Listener
                m_Client = Client
                m_Buffer = New Byte(0 To 4095) {}
                m_MemoryStream = New MemoryStream()
                m_Automaton = 1

                Dim NetworkStream = m_Client.GetStream()
                Try
                    NetworkStream.BeginRead(m_Buffer, 0, m_Buffer.Length, AddressOf OnRead, Nothing)
                Catch ex As Exception
                    m_Client.Close()
                End Try
            End Sub

            Private Sub OnRead(ByVal Result As IAsyncResult)
                Try
                    Dim NetworkStream = m_Client.GetStream(), Length = NetworkStream.EndRead(Result)

                    If Length = 0 Then
                        NetworkStream.Close()
                        m_Client.Close()
                        Return
                    End If

                    For Index = 0 To Length - 1
                        Dim [Byte] = m_Buffer(Index)
                        m_MemoryStream.WriteByte([Byte])
                        Select Case [Byte]
                            Case AscW(vbCr)
                                If (m_Automaton And 5) <> 0 Then
                                    m_Automaton <<= 1
                                Else
                                    m_Automaton = 1
                                End If
                            Case AscW(vbLf)
                                If (m_Automaton And 10) <> 0 Then
                                    m_Automaton <<= 1
                                Else
                                    m_Automaton = 1
                                End If
                            Case Else
                                m_Automaton = 1
                        End Select
                        If (m_Automaton And 16) <> 0 Then
                            If Not OnRequest() Then Return
                            m_MemoryStream = New MemoryStream()
                            m_Automaton = 1
                        End If
                    Next

                    NetworkStream.BeginRead(m_Buffer, 0, m_Buffer.Length, AddressOf OnRead, m_Buffer)
                Catch ex As Exception
                    m_Client.Close()
                End Try
            End Sub

            Private Function OnRequest() As Boolean
                Dim Host = String.Empty, UrlUnsafe = "/"

                m_KeepAlive = False
                m_MemoryStream.Seek(0, SeekOrigin.Begin)

                Using Reader = New StreamReader(m_MemoryStream)
                    Dim Line = Reader.ReadLine()
                    If Line Is Nothing Then Return Write(400)

                    Dim LineSplit = Line.Split(New Char() {" "c}, 3, StringSplitOptions.RemoveEmptyEntries)
                    If LineSplit.Length <> 3 Then Return Write(400)

                    Select Case LineSplit(2)
                        Case "HTTP/1.1"
                            m_KeepAlive = True
                        Case "HTTP/1.0", "HTTP/0.9"
                            ' Do nothing
                        Case Else
                            Return Write(400)
                    End Select

                    If LineSplit(0) <> "GET" Then Return Write(400)

                    UrlUnsafe = LineSplit(1)
                    Line = Reader.ReadLine()
                    While Line.Length <> 0
                        LineSplit = Line.Split(New Char() {":"c}, 2)
                        If LineSplit.Length <> 2 Then Return Write(400)

                        Select Case LineSplit(0).Trim().ToLower()
                            Case "host"
                                Host = LineSplit(1).Trim()
                            Case "content-length"
                                Return Write(400, False)
                            Case "connection"
                                For Each Param In LineSplit(1).Split(New Char() {","c})
                                    Select Case Param.Trim().ToLower()
                                        Case "keep-alive"
                                            m_KeepAlive = True
                                        Case "close"
                                            m_KeepAlive = False
                                    End Select
                                Next
                        End Select

                        Line = Reader.ReadLine()
                    End While
                End Using

                Dim Addresses = m_Listener.Addresses
                Dim Hosts As SortedDictionary(Of String, SortedDictionary(Of String, Context)) = Nothing

                If Not Addresses.TryGetValue(DirectCast(m_Client.Client.LocalEndPoint, IPEndPoint).Address, Hosts) AndAlso _
                    Not Addresses.TryGetValue(IPAddress.Any, Hosts) Then _
                    Return Write(403)

                Dim PortSeperator = Host.IndexOf(":"c)
                If PortSeperator <> -1 Then Host = Host.Substring(0, PortSeperator)
                Dim Urls As SortedDictionary(Of String, Context) = Nothing

                If Not Hosts.TryGetValue(Host, Urls) AndAlso _
                    Not Hosts.TryGetValue(String.Empty, Urls) Then _
                    Return Write(403)

                Dim Url = HttpUtility.UrlDecode(UrlUnsafe)
                Dim QuerySeperator = Url.IndexOf("?"c)
                Dim Queries = New SortedDictionary(Of String, String)(StringComparer.CurrentCultureIgnoreCase)
                If QuerySeperator <> -1 Then
                    For Each KeyValue In Url.Substring(QuerySeperator + 1).Split(New Char() {"&"c})
                        Dim KvSeperator = KeyValue.IndexOf("="c)
                        Dim Key, Value As String
                        If KvSeperator = -1 Then
                            Key = KeyValue
                            Value = Nothing
                        Else
                            Key = KeyValue.Substring(0, KvSeperator)
                            Value = KeyValue.Substring(KvSeperator + 1)
                        End If
                        If Not Queries.ContainsKey(Key) Then Queries.Add(Key, Value)
                    Next
                    Url = Url.Substring(0, QuerySeperator)
                End If

                Dim Context As Context = Nothing
                If Not Urls.TryGetValue(Url, Context) Then _
                    Return Write(404)

                Return Context.Invoke(Me, Queries)
            End Function

            Public Function Write(ByVal Status As Int32) As Boolean
                Dim Title As String
                Dim Text As String
                Select Case Status
                    Case 200
                        Title = "200 OK"
                        Text = "<h1>It works!</h1>"
                    Case 400
                        Title = "400 Bad Request"
                        Text = "<h1>Bad Request</h1>"
                    Case 403
                        Title = "403 Forbidden"
                        Text = "<h1>Forbidden</h1>"
                    Case 404
                        Title = "404 Not Found"
                        Text = "<h1>Not Found</h1>"
                    Case Else
                        Throw New ArgumentException()
                End Select
                Return Write(Title, "text/html; charset=UTF-8", Text)
            End Function

            Public Function Write(ByVal ContentType As String, ByVal Text As String) As Boolean
                Return Write("200 OK", ContentType, Text)
            End Function

            Public Function Write(ByVal Title As String, ByVal ContentType As String, ByVal Text As String) As Boolean
                Return Write(Title, ContentType, Encoding.UTF8.GetBytes(Text))
            End Function

            Public Function Write(ByVal Title As String, ByVal ContentType As String, ByVal Buffer As Byte()) As Boolean
                Dim Response = New StringBuilder()
                Response.Append("HTTP/1.1 ")
                Response.AppendLine(Title)
                If ContentType IsNot Nothing Then
                    Response.Append("Content-Type: ")
                    Response.AppendLine(ContentType)
                End If
                Response.Append("Server: VijosNT/")
                Response.AppendLine(Assembly.GetExecutingAssembly().GetName().Version.ToString())
                Response.Append("Content-Length: ")
                If Buffer IsNot Nothing Then
                    Response.AppendLine(Buffer.Length.ToString())
                Else
                    Response.AppendLine("0")
                End If
                If Not m_KeepAlive Then _
                    Response.AppendLine("Connection: close")
                Response.AppendLine()
                Dim Header = Encoding.Default.GetBytes(Response.ToString())
                Dim NetworkStream = m_Client.GetStream()
                If Buffer IsNot Nothing Then
                    NetworkStream.BeginWrite(Header, 0, Header.Length, AddressOf OnWrite0, Buffer)
                Else
                    NetworkStream.BeginWrite(Header, 0, Header.Length, AddressOf OnWrite1, Nothing)
                End If
                Return m_KeepAlive
            End Function

            Private Sub OnWrite0(ByVal Result As IAsyncResult)
                Try
                    Dim NetworkStream = m_Client.GetStream()
                    NetworkStream.EndWrite(Result)
                    Dim Buffer = DirectCast(Result.AsyncState, Byte())
                    NetworkStream.BeginWrite(Buffer, 0, Buffer.Length, AddressOf OnWrite1, Nothing)
                Catch ex As Exception
                    m_Client.Close()
                End Try
            End Sub

            Private Sub OnWrite1(ByVal Result As IAsyncResult)
                Try
                    Dim NetworkStream = m_Client.GetStream()
                    NetworkStream.EndWrite(Result)
                    If Not m_KeepAlive Then NetworkStream.Close()
                Catch ex As Exception
                    m_Client.Close()
                End Try
            End Sub
        End Class

        Private m_SyncRoot As Object
        Private m_Contexts As IEnumerable(Of Listener)

        Public Sub New()
            m_SyncRoot = New Object()
        End Sub

        Public Sub [Stop]()
            If m_Contexts IsNot Nothing Then
                For Each Context In m_Contexts
                    Context.Stop()
                Next
                m_Contexts = Nothing
            End If
        End Sub

        Public Sub Start(ByVal Prefixes As IEnumerable(Of KeyValuePair(Of String, Context)))
            m_Contexts = GetContexts(Prefixes)
            For Each Context In m_Contexts
                Context.Start()
            Next
        End Sub

        Private Function GetContexts(ByVal Prefixes As IEnumerable(Of KeyValuePair(Of String, Context))) As IEnumerable(Of Listener)
            Dim Ports As New SortedDictionary(Of Int32,  _
                Dictionary(Of IPAddress,  _
                SortedDictionary(Of String,  _
                SortedDictionary(Of String, Context))))

            For Each Prefix In Prefixes
                Dim PrefixText = Prefix.Key
                If Not PrefixText.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) Then
                    Throw New Exception("prefix must starts with http://")
                End If
                Dim UrlSeperator = PrefixText.IndexOf("/"c, 7)
                Dim Host As String
                Dim Url As String
                If UrlSeperator = -1 Then
                    Host = PrefixText.Substring(7)
                    Url = "/"
                Else
                    Host = PrefixText.Substring(7, UrlSeperator - 7)
                    Url = PrefixText.Substring(UrlSeperator)
                End If
                Host = Host.ToLower()
                Dim PortSeperator = Host.LastIndexOf(":"c)
                Dim Port As Int32
                If PortSeperator = -1 Then
                    Port = 80
                Else
                    Port = Int32.Parse(Host.Substring(PortSeperator + 1))
                    Host = Host.Substring(0, PortSeperator)
                End If

                Dim Addresses As Dictionary(Of IPAddress,  _
                    SortedDictionary(Of String,  _
                    SortedDictionary(Of String, Context))) = Nothing

                If Not Ports.TryGetValue(Port, Addresses) Then
                    Addresses = New Dictionary(Of IPAddress,  _
                        SortedDictionary(Of String,  _
                        SortedDictionary(Of String, Context)))()
                    Ports.Add(Port, Addresses)
                End If

                Dim IPAddress As IPAddress = Nothing
                If IPAddress.TryParse(Host, IPAddress) Then
                    Host = String.Empty
                Else
                    IPAddress = IPAddress.Any
                End If

                Dim Hosts As SortedDictionary(Of String,  _
                    SortedDictionary(Of String, Context)) = Nothing

                If Not Addresses.TryGetValue(IPAddress, Hosts) Then
                    Hosts = New SortedDictionary(Of String,  _
                        SortedDictionary(Of String, Context))(StringComparer.CurrentCultureIgnoreCase)
                    Addresses.Add(IPAddress, Hosts)
                End If

                Dim Urls As SortedDictionary(Of String, Context) = Nothing

                If Not Hosts.TryGetValue(Host, Urls) Then
                    Urls = New SortedDictionary(Of String, Context)()
                    Hosts.Add(Host, Urls)
                End If

                Urls.Add(Url, Prefix.Value)
            Next

            Dim Result As New List(Of Listener)

            For Each Port In Ports
                Dim Addresses = Port.Value

                If Addresses.ContainsKey(IPAddress.Any) Then
                    Result.Add(New Listener(Me, IPAddress.Any, Port.Key, Addresses))
                Else
                    For Each Address In Addresses
                        Result.Add(New Listener(Me, Address.Key, Port.Key, Address.Value))
                    Next
                End If
            Next

            Return Result
        End Function
    End Class
End Namespace
