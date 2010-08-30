Imports VijosNT.Utility

Namespace LocalDb
    Friend Class DataSourceConfig
        Private m_Id As Int32
        Private m_Class As DataSourceClass
        Private m_Parameter As String
        Private m_PollingInterval As Nullable(Of Int32)
        Private m_IpcAnnouncement As String
        Private m_HttpAnnouncement As String

        Public Sub New(ByVal Reader As IDataReader)
            m_Id = Reader("Id")
            m_Class = [Enum].Parse(GetType(DataSourceClass), Reader("ClassName"))
            m_Parameter = Reader("Parameter")
            m_PollingInterval = DbToLocalInt32(Reader("PollingInterval"))
            m_IpcAnnouncement = Reader("IpcAnnouncement")
            m_HttpAnnouncement = Reader("HttpAnnouncement")
        End Sub

        Private Sub Commit()
            DataSourceMapping.Update(m_Id, m_Class.ToString(), m_Parameter, m_PollingInterval, m_IpcAnnouncement, m_HttpAnnouncement)
        End Sub

        <DisplayName("类型"), CategoryAttribute("数据源设置"), DescriptionAttribute("数据源的类型")> _
        Public Property [Class]() As DataSourceClass
            Get
                Return m_Class
            End Get

            Set(ByVal Value As DataSourceClass)
                m_Class = Value
                Commit()
            End Set
        End Property

        <DisplayName("参数"), CategoryAttribute("数据源设置"), DescriptionAttribute("数据源类型参数, 对不同的数据源类型有不同的含义")> _
        Public Property Parameter() As String
            Get
                Return m_Parameter
            End Get

            Set(ByVal Value As String)
                m_Parameter = Value
                Commit()
            End Set
        End Property

        <DisplayName("Ipc 通告关键字"), CategoryAttribute("数据源设置"), DescriptionAttribute("采用 Ipc 方式进行通告的关键字, 通过向 Ipc 端口发送请求进行通告, 请求中包含此字符串")> _
        Public Property IpcAnnouncement() As String
            Get
                Return m_IpcAnnouncement
            End Get

            Set(ByVal Value As String)
                m_IpcAnnouncement = Value
                Commit()
            End Set
        End Property

        <DisplayName("Http 通告地址"), CategoryAttribute("数据源设置"), DescriptionAttribute("采用 Http 方式进行通告的地址, 通过访问设定的 Uri 进行通告, 以 http:// 开头, 不包含末尾的 /")> _
        Public Property HttpAnnouncement() As String
            Get
                Return m_HttpAnnouncement
            End Get

            Set(ByVal Value As String)
                If Value.Length <> 0 AndAlso Not Value.StartsWith("http://") Then _
                    Throw New ArgumentException("Http 通告地址必须以 http:// 开头。", "Http 通告地址")
                While Value.EndsWith("/")
                    Value = Value.Substring(0, Value.Length - 1)
                End While
                m_HttpAnnouncement = Value
                Commit()
            End Set
        End Property

        <DisplayName("轮询间隔"), CategoryAttribute("数据源设置"), DescriptionAttribute("采用主动方式对数据源进行轮询 (不推荐) 的间隔, 以 ms 为单位, 为空表示禁用轮询 (默认, 推荐). 轮询会降低系统性能, 并且会使笔记本电脑无法进入省电状态, 不到万不得已请勿使用")> _
        Public Property PollingInterval() As Nullable(Of Int32)
            Get
                Return m_PollingInterval
            End Get

            Set(ByVal Value As Nullable(Of Int32))
                If Value.HasValue AndAlso Value.Value < 500 Then _
                    Throw New ArgumentOutOfRangeException("轮询间隔", "轮询间隔必须大于或等于 500ms。")
                m_PollingInterval = Value
                Commit()
            End Set
        End Property
    End Class
End Namespace
