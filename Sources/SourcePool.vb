Imports VijosNT.LocalDb
Imports VijosNT.Testing
Imports VijosNT.Utility

Namespace Sources
    Friend Class SourcePool
        Implements IDisposable

        Private Shared s_Instance As SourcePool

        Shared Sub New()
            s_Instance = New SourcePool()
        End Sub

        Public Shared Function Singleton() As SourcePool
            Return s_Instance
        End Function

        Private m_Namespaces As SortedDictionary(Of String, List(Of Source))

        Private Sub New()
            MiniHttpServer.Singleton()
            Reload()
        End Sub

        Public Sub Reload()
            With MiniHttpServer.Singleton()
                .Stop()

                Dim Namespaces = New SortedDictionary(Of String, List(Of Source))
                Dim Prefixes = New List(Of KeyValuePair(Of String, MiniHttpServer.Context))()

                Using Reader As IDataReader = SourceMapping.GetAll()
                    While Reader.Read()
                        Dim [Namespace] As String = Reader("Namespace")
                        Dim Source As Source
                        Select Case Reader("ClassName")
                            Case "APlusB"
                                Source = New APlusBSource([Namespace])
                            Case "Vijos"
                                Source = New VijosSource([Namespace], Reader("Parameter"))
                            Case Else
                                EventLog.WriteEntry(My.Resources.ServiceName, "数据源加载失败" & vbCrLf & "未找到类名为 " & Reader("ClassName") & " 的数据源提供程序。", EventLogEntryType.Warning)
                                Continue While
                        End Select
                        If Not Namespaces.ContainsKey([Namespace]) Then _
                            Namespaces.Add([Namespace], New List(Of Source))
                        Namespaces([Namespace]).Add(Source)
                        Dim HttpAnnouncement As String = Reader("HttpAnnouncement")
                        If HttpAnnouncement.Length <> 0 Then _
                            Prefixes.Add(New KeyValuePair(Of String, MiniHttpServer.Context)(HttpAnnouncement, New MiniHttpServer.Context(AddressOf OnContext, Source)))
                    End While
                End Using

                m_Namespaces = Namespaces
                .Start(Prefixes)
            End With
        End Sub

        Private Sub RemoteFeed(ByVal Source As Source, ByVal Queries As SortedDictionary(Of String, String))
            Dim Id = Int32.Parse(Queries("Id"))
            Dim Record = Source.Take(Id)
            If Not Runner.Singleton().Queue(Source.Namespace, Record.FileName, New MemoryStream(Encoding.Default.GetBytes(Record.SourceCode)), _
                Sub(Result As TestResult)
                    Try
                        Source.Untake(Id, Result)
                    Catch ex As Exception
                        ' Eat it
                    End Try
                End Sub) Then
                Source.Untake(Id)
            End If
        End Sub

        Private Function OnContext(ByVal State As Object, ByVal Session As MiniHttpServer.Session, ByVal Queries As SortedDictionary(Of String, String)) As Boolean
            Dim Succeeded = True
            Try
                RemoteFeed(DirectCast(State, Source), Queries)
            Catch ex As Exception
                Succeeded = False
            End Try

            Dim Builder As New StringBuilder()
            Builder.Append("VijosNT Mini ")
            Builder.AppendLine(Assembly.GetExecutingAssembly().GetName().Version.ToString())
            Builder.Append("Feed result: ")
            If Succeeded Then
                Builder.AppendLine("succeeded")
            Else
                Builder.AppendLine("failed")
            End If

            Try
                Return Session.Write("text/plain; charset=UTF-8", Encoding.UTF8.GetBytes(Builder.ToString()))
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function TryLoad(ByVal [Namespace] As String, ByVal Id As String) As IEnumerable(Of TestCase)
            Dim Sources As List(Of Source) = Nothing

            If Not m_Namespaces.TryGetValue([Namespace], Sources) Then _
                Return Nothing

            For Each Source In Sources
                If Source.Namespace = [Namespace] Then
                    Dim Cases = Source.TryLoad(Id)
                    If Cases IsNot Nothing Then Return Cases
                End If
            Next

            Return Nothing
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    MiniHttpServer.Singleton().Stop()
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