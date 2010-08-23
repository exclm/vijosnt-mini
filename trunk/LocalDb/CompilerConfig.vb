Imports VijosNT.Utility

Namespace LocalDb
    Friend Class CompilerConfig
        Private m_Id As Int32
        Private m_Pattern As String
        Private m_ApplicationName As String
        Private m_CommandLine As String
        Private m_TimeQuota As Nullable(Of Int64)
        Private m_MemoryQuota As Nullable(Of Int64)
        Private m_ActiveProcessQuota As Nullable(Of Int32)
        Private m_SourceFileName As String
        Private m_TargetFileName As String
        Private m_TargetApplicationName As String
        Private m_TargetCommandLine As String

        Public Sub New(ByVal Reader As IDataReader)
            m_Id = Reader("Id")
            m_Pattern = Reader("Pattern")
            m_ApplicationName = Reader("ApplicationName")
            m_CommandLine = Reader("CommandLine")
            m_TimeQuota = DbToLocalInt64(Reader("TimeQuota"))
            m_MemoryQuota = DbToLocalInt64(Reader("MemoryQuota"))
            m_ActiveProcessQuota = DbToLocalInt32(Reader("ActiveProcessQuota"))
            m_SourceFileName = Reader("SourceFileName")
            m_TargetFileName = Reader("TargetFileName")
            m_TargetApplicationName = Reader("TargetApplicationName")
            m_TargetCommandLine = Reader("TargetCommandLine")
        End Sub

        Private Sub Commit()
            CompilerMapping.Update(m_Id, m_Pattern, m_ApplicationName, m_CommandLine, m_TimeQuota, m_MemoryQuota, m_ActiveProcessQuota, m_SourceFileName, m_TargetFileName, m_TargetApplicationName, m_TargetCommandLine)
        End Sub

        <DisplayName("扩展名匹配"), CategoryAttribute("编译器设置"), DescriptionAttribute("用于匹配文件扩展名的字符串, 支持 ?、* 通配符, 多个匹配串使用 ; 分隔")> _
        Public Property Pattern() As String
            Get
                Return m_Pattern
            End Get

            Set(ByVal Value As String)
                m_Pattern = Value
                Commit()
            End Set
        End Property

        <DisplayName("程序路径"), CategoryAttribute("编译器设置"), DescriptionAttribute("编译器程序的路径")> _
        Public Property ApplicationName() As String
            Get
                Return m_ApplicationName
            End Get

            Set(ByVal Value As String)
                m_ApplicationName = Value
                Commit()
            End Set
        End Property

        <DisplayName("命令行"), CategoryAttribute("编译器设置"), DescriptionAttribute("编译器程序的命令行")> _
        Public Property CommandLine() As String
            Get
                Return m_CommandLine
            End Get

            Set(ByVal Value As String)
                m_CommandLine = Value
                Commit()
            End Set
        End Property

        <DisplayName("时间配额"), CategoryAttribute("编译器设置"), DescriptionAttribute("编译器的时间配额, 以 ms 为单位, 留空表示不限制, 默认值为 15000")> _
        Public Property TimeQuota() As Nullable(Of Int32)
            Get
                If m_TimeQuota.HasValue Then
                    Return m_TimeQuota.Value \ 10000
                Else
                    Return Nothing
                End If
            End Get

            Set(ByVal Value As Nullable(Of Int32))
                If Value.HasValue Then
                    m_TimeQuota = Math.BigMul(Value.Value, 10000)
                Else
                    m_TimeQuota = Nothing
                End If
                Commit()
            End Set
        End Property

        <DisplayName("内存配额"), CategoryAttribute("编译器设置"), DescriptionAttribute("编译器的内存配额, 以 KB 为单位, 留空表示不限制, 默认值为空")> _
        Public Property MemoryQuota() As Nullable(Of Int32)
            Get
                If m_MemoryQuota.HasValue Then
                    Return m_MemoryQuota.Value \ 1024
                Else
                    Return Nothing
                End If
            End Get

            Set(ByVal Value As Nullable(Of Int32))
                If Value.HasValue Then
                    m_MemoryQuota = Math.BigMul(Value.Value, 1024)
                Else
                    m_MemoryQuota = Nothing
                End If
                Commit()
            End Set
        End Property

        <DisplayName("活动进程数配额"), CategoryAttribute("编译器设置"), DescriptionAttribute("编译器的活动进程数配额, 留空表示不限制, 默认值为空")> _
        Public Property ActiveProcessQuota() As Nullable(Of Int32)
            Get
                Return m_ActiveProcessQuota
            End Get

            Set(ByVal Value As Nullable(Of Int32))
                m_ActiveProcessQuota = Value
                Commit()
            End Set
        End Property

        <DisplayName("源文件名"), CategoryAttribute("编译器设置"), DescriptionAttribute("与命令行中的源文件名一致")> _
        Public Property SourceFileName() As String
            Get
                Return m_SourceFileName
            End Get

            Set(ByVal Value As String)
                m_SourceFileName = Value
                Commit()
            End Set
        End Property

        <DisplayName("目标文件名"), CategoryAttribute("编译器设置"), DescriptionAttribute("与命令行中的目标文件名一致")> _
        Public Property TargetFileName() As String
            Get
                Return m_TargetFileName
            End Get

            Set(ByVal Value As String)
                m_TargetFileName = Value
                Commit()
            End Set
        End Property

        <DisplayName("目标程序路径"), CategoryAttribute("执行设置"), DescriptionAttribute("当目标文件需要专用的解释器执行时, 设置为相应解释器的路径, 否则留空")> _
        Public Property TargetApplicationName() As String
            Get
                Return m_TargetApplicationName
            End Get

            Set(ByVal Value As String)
                m_TargetApplicationName = Value
                Commit()
            End Set
        End Property

        <DisplayName("目标命令行"), CategoryAttribute("执行设置"), DescriptionAttribute("目标文件或解释器的命令行参数")> _
        Public Property TargetCommandLine() As String
            Get
                Return m_TargetCommandLine
            End Get

            Set(ByVal Value As String)
                m_TargetCommandLine = Value
                Commit()
            End Set
        End Property
    End Class
End Namespace
