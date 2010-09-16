Imports VijosNT.Utility

Namespace Compiling
    Friend Class LocalCompiler
        Inherits Compiler

        Private m_TempPathServer As TempPathServer
        Private m_ApplicationName As String
        Private m_CommandLine As String
        Private m_EnvironmentVariables As IEnumerable(Of String)
        Private m_TimeQuota As Nullable(Of Int64)
        Private m_MemoryQuota As Nullable(Of Int64)
        Private m_ActiveProcessQuota As Nullable(Of Int32)
        Private m_SourceFileName As String
        Private m_TargetFileName As String
        Private m_TargetApplicationName As String
        Private m_TargetCommandLine As String
        Private m_TimeOffset As Nullable(Of Int64)
        Private m_TimeFactor As Nullable(Of Double)
        Private m_MemoryOffset As Nullable(Of Int64)
        Private m_MemoryFactor As Nullable(Of Double)

        Public Sub New(ByVal TempPathServer As TempPathServer, ByVal ApplicationName As String, ByVal CommandLine As String, ByVal EnvironmentVariables As IEnumerable(Of String), _
            ByVal TimeQuota As Nullable(Of Int64), ByVal MemoryQuota As Nullable(Of Int64), ByVal ActiveProcessQuota As Nullable(Of Int64), _
            ByVal SourceFileName As String, ByVal TargetFileName As String, _
            ByVal TargetApplicationName As String, ByVal TargetCommandLine As String, _
            ByVal TimeOffset As Nullable(Of Int64), ByVal TimeFactor As Nullable(Of Double), ByVal MemoryOffset As Nullable(Of Int64), ByVal MemoryFactor As Nullable(Of Double))

            m_TempPathServer = TempPathServer
            m_ApplicationName = ApplicationName
            m_CommandLine = CommandLine
            m_EnvironmentVariables = EnvironmentVariables
            m_TimeQuota = TimeQuota
            m_MemoryQuota = MemoryQuota
            m_ActiveProcessQuota = ActiveProcessQuota
            m_SourceFileName = SourceFileName
            m_TargetFileName = TargetFileName
            m_TargetApplicationName = TargetApplicationName
            m_TargetCommandLine = TargetCommandLine
            m_TimeOffset = TimeOffset
            m_TimeFactor = TimeFactor
            m_MemoryOffset = MemoryOffset
            m_MemoryFactor = MemoryFactor
        End Sub

        Public ReadOnly Property TempPathServer() As TempPathServer
            Get
                Return m_TempPathServer
            End Get
        End Property

        Public Overrides Function CreateInstance(ByVal SourceCode As Stream) As CompilerInstance
            Dim TempPath As TempPath = m_TempPathServer.CreateTempPath()
            Using SourceFile As New FileStream(TempPath.Combine(m_SourceFileName), FileMode.CreateNew, FileAccess.Write, FileShare.None)
                BufferedCopy(SourceCode, SourceFile)
            End Using
            Return New LocalCompilerInstance(TempPath, Me)
        End Function

        Public Overrides ReadOnly Property ApplicationName() As String
            Get
                Return m_ApplicationName
            End Get
        End Property

        Public Overrides ReadOnly Property CommandLine() As String
            Get
                Return m_CommandLine
            End Get
        End Property

        Public Overrides ReadOnly Property EnvironmentVariables As IEnumerable(Of String)
            Get
                Return m_EnvironmentVariables
            End Get
        End Property

        Public Overrides ReadOnly Property TimeQuota() As Nullable(Of Int64)
            Get
                Return m_TimeQuota
            End Get
        End Property

        Public Overrides ReadOnly Property MemoryQuota() As Nullable(Of Int64)
            Get
                Return m_MemoryQuota
            End Get
        End Property

        Public Overrides ReadOnly Property ActiveProcessQuota() As Nullable(Of Int32)
            Get
                Return m_ActiveProcessQuota
            End Get
        End Property

        Public Overrides ReadOnly Property SourceFileName() As String
            Get
                Return m_SourceFileName
            End Get
        End Property

        Public Overrides ReadOnly Property TargetFileName() As String
            Get
                Return m_TargetFileName
            End Get
        End Property

        Public Overrides ReadOnly Property TargetApplicationName() As String
            Get
                Return m_TargetApplicationName
            End Get
        End Property

        Public Overrides ReadOnly Property TargetCommandLine() As String
            Get
                Return m_TargetCommandLine
            End Get
        End Property

        Public Overrides ReadOnly Property TimeOffset() As Nullable(Of Int64)
            Get
                Return m_TimeOffset
            End Get
        End Property

        Public Overrides ReadOnly Property TimeFactor() As Nullable(Of Double)
            Get
                Return m_TimeFactor
            End Get
        End Property

        Public Overrides ReadOnly Property MemoryOffset() As Nullable(Of Int64)
            Get
                Return m_MemoryOffset
            End Get
        End Property

        Public Overrides ReadOnly Property MemoryFactor() As Nullable(Of Double)
            Get
                Return m_MemoryFactor
            End Get
        End Property
    End Class
End Namespace
