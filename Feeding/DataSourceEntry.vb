Namespace Feeding
    Friend Class DataSourceEntry
        Implements IDisposable

        Private m_DataSourcePool As DataSourcePool
        Private m_DataSource As DataSourceBase
        Private m_IpcAnnouncement As String
        Private m_HttpAnnouncement As String
        Private m_Timer As System.Timers.Timer

        Public Sub New(ByVal DataSourcePool As DataSourcePool, ByVal DataSource As DataSourceBase, ByVal IpcAnnouncement As String, ByVal HttpAnnouncement As String, ByVal PollingInterval As Nullable(Of Int32))
            m_DataSourcePool = DataSourcePool
            m_DataSource = DataSource
            m_IpcAnnouncement = IpcAnnouncement
            m_HttpAnnouncement = HttpAnnouncement
            If PollingInterval.HasValue Then
                m_Timer = New System.Timers.Timer(Convert.ToDouble(PollingInterval.Value))
                AddHandler m_Timer.Elapsed, AddressOf OnTimer
                m_Timer.Start()
            End If
        End Sub

        Public Function Feed(ByVal Limit As Int32) As Int32
            Return m_DataSourcePool.Runner.Feed(m_DataSource, Limit)
        End Function

        Public Function MatchIpcAnnouncement(ByVal IpcAnnouncement As String) As Boolean
            If m_IpcAnnouncement Is Nothing Then
                Return False
            Else
                Return (m_IpcAnnouncement = IpcAnnouncement)
            End If
        End Function

        Public Function MatchHttpAnnouncement(ByVal UrlString As String) As Boolean
            If m_HttpAnnouncement Is Nothing Then
                Return False
            Else
                Return UrlString.StartsWith(m_HttpAnnouncement, StringComparison.CurrentCultureIgnoreCase)
            End If
        End Function

        Private Sub OnTimer(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Feed(Int32.MaxValue)
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    If m_Timer IsNot Nothing Then _
                        m_Timer.Dispose()
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
