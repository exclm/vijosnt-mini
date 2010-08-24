Imports VijosNT.Utility

Namespace Compiling
    Friend Class LocalCompiler
        Inherits Compiler

        Private m_TempPathServer As TempPathServer
        Private m_ApplicationName As String
        Private m_CommandLine As String
        Private m_TimeQuota As Nullable(Of Int64)
        Private m_MemoryQuota As Nullable(Of Int64)
        Private m_ActiveProcessQuota As Nullable(Of Int32)
        Private m_SourceFileName As String
        Private m_TargetFileName As String

        Public Sub New(ByVal TempPathServer As TempPathServer, ByVal ApplicationName As String, ByVal CommandLine As String, _
            ByVal TimeQuota As Nullable(Of Int64), ByVal MemoryQuota As Nullable(Of Int64), ByVal ActiveProcessQuota As Nullable(Of Int64), _
            ByVal SourceFileName As String, ByVal TargetFileName As String)

            m_TempPathServer = TempPathServer
            m_ApplicationName = ApplicationName
            m_CommandLine = CommandLine
            m_TimeQuota = TimeQuota
            m_MemoryQuota = MemoryQuota
            m_ActiveProcessQuota = ActiveProcessQuota
            m_SourceFileName = SourceFileName
            m_TargetFileName = TargetFileName
        End Sub

        Public Overrides Function CreateInstance(ByVal SourceCode As Stream) As CompilerInstance
            Dim TempPath As TempPath = m_TempPathServer.CreateTempPath()
            Using SourceFile As New FileStream(TempPath.Combine(m_SourceFileName), FileMode.CreateNew, FileAccess.Write, FileShare.None)
                BufferedCopy(SourceCode, SourceFile)
            End Using
            Return New LocalCompilerInstance(TempPath, m_TargetFileName, m_TempPathServer)
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

        Public Overrides ReadOnly Property TimeQuota As Nullable(Of Int64)
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
    End Class
End Namespace
