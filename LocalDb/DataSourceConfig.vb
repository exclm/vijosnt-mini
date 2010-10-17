Imports VijosNT.Utility

Namespace LocalDb
    Friend Class DataSourceConfig
        Private m_Id As Int32
        Private m_Class As DataSourceClass
        Private m_Namespace As String
        Private m_Parameter As String
        Private m_HttpAnnouncement As String

        Public Sub New(ByVal Reader As IDataReader)
            m_Id = Reader("Id")
            m_Class = [Enum].Parse(GetType(DataSourceClass), Reader("ClassName"))
            m_Namespace = Reader("Namespace")
            m_Parameter = Reader("Parameter")
            m_HttpAnnouncement = Reader("HttpAnnouncement")
        End Sub

        Private Sub Commit()
            DataSourceMapping.Update(m_Id, m_Class.ToString(), m_Namespace, m_Parameter, m_HttpAnnouncement)
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

        <DisplayName("名字空间"), CategoryAttribute("数据源设置"), DescriptionAttribute("数据源所使用数据集的名字空间")> _
        Public Property [Namespace]() As String
            Get
                Return m_Namespace
            End Get

            Set(ByVal Value As String)
                m_Namespace = Value
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
    End Class
End Namespace
