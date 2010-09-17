Namespace LocalDb
    Friend Class TestSuiteConfig
        Private m_Id As Int32
        Private m_Pattern As String
        Private m_NamespacePattern As String
        Private m_Class As TestSuiteClass
        Private m_Parameter As String

        Public Sub New(ByVal Reader As IDataReader)
            m_Id = Reader("Id")
            m_Pattern = Reader("Pattern")
            m_NamespacePattern = Reader("NamespacePattern")
            m_Class = [Enum].Parse(GetType(TestSuiteClass), Reader("ClassName"))
            m_Parameter = Reader("Parameter")
        End Sub

        Private Sub Commit()
            TestSuiteMapping.Update(m_Id, m_Pattern, m_NamespacePattern, m_Class.ToString(), Parameter)
        End Sub

        <DisplayName("文件名匹配"), CategoryAttribute("数据集设置"), DescriptionAttribute("用于匹配文件名的字符串, 支持 ?、* 通配符, 多个匹配串使用 ; 分隔")> _
        Public Property Pattern() As String
            Get
                Return m_Pattern
            End Get

            Set(ByVal Value As String)
                m_Pattern = Value
                Commit()
            End Set
        End Property

        <DisplayName("名字空间匹配"), CategoryAttribute("数据集设置"), DescriptionAttribute("用于匹配名字空间的字符串, 支持 ?、* 通配符, 多个匹配串使用 ; 分隔")> _
        Public Property NamespacePattern() As String
            Get
                Return m_NamespacePattern
            End Get

            Set(ByVal Value As String)
                m_NamespacePattern = Value
                Commit()
            End Set
        End Property

        <DisplayName("类型"), CategoryAttribute("数据集设置"), DescriptionAttribute("数据集类型")> _
        Public Property [Class]() As TestSuiteClass
            Get
                Return m_Class
            End Get

            Set(ByVal Value As TestSuiteClass)
                m_Class = Value
                Commit()
            End Set
        End Property

        <DisplayName("参数"), CategoryAttribute("数据集设置"), DescriptionAttribute("数据集类型参数, 对不同的数据集类型有不同的含义")> _
        Public Property Parameter() As String
            Get
                Return m_Parameter
            End Get

            Set(ByVal Value As String)
                m_Parameter = Value
                Commit()
            End Set
        End Property
    End Class
End Namespace
