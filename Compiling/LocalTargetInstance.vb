Imports VijosNT.Utility

Namespace Compiling
    Friend Class LocalTargetInstance
        Inherits TargetInstance
        Implements IDisposable

        Private m_TempPath As TempPath
        Private m_Target As LocalTarget
        Private m_ApplicationName As String
        Private m_CommandLine As String
        Private m_EnvironmentVariables As IEnumerable(Of String)

        Public Sub New(ByVal TempPath As TempPath, ByVal Target As LocalTarget)
            m_TempPath = TempPath
            m_Target = Target
            With m_Target.CompilerInstance.Compiler
                If .TargetApplicationName Is Nothing OrElse .TargetApplicationName.Length = 0 Then
                    m_ApplicationName = m_TempPath.Combine(.TargetFileName)
                Else
                    m_ApplicationName = .TargetApplicationName
                End If
                If .TargetCommandLine Is Nothing OrElse .TargetCommandLine.Length = 0 Then
                    m_CommandLine = Nothing
                Else
                    m_CommandLine = .TargetCommandLine
                End If
                m_EnvironmentVariables = .EnvironmentVariables
            End With
        End Sub

        Public Overrides ReadOnly Property Target() As Target
            Get
                Return m_Target
            End Get
        End Property

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

        Public Overrides ReadOnly Property EnvironmentVariables() As IEnumerable(Of String)
            Get
                Return m_EnvironmentVariables
            End Get
        End Property

        Public Overrides ReadOnly Property WorkingDirectory() As String
            Get
                Return m_TempPath.GetPath()
            End Get
        End Property

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_TempPath.Dispose()
                End If
            End If
            Me.disposedValue = True
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace
