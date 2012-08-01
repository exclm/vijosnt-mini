Imports VijosNT.Utility

Namespace Compiling
    Friend Class TargetInstance
        Implements IDisposable

        Private m_TempPath As TempPath
        Private m_Target As Target
        Private m_ApplicationName As String
        Private m_CommandLine As String
        Private m_EnvironmentVariables As IEnumerable(Of String)

        Public Sub New(ByVal TempPath As TempPath, ByVal Target As Target)
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

        Public ReadOnly Property Target() As Target
            Get
                Return m_Target
            End Get
        End Property

        Public ReadOnly Property ApplicationName() As String
            Get
                Return m_ApplicationName
            End Get
        End Property

        Public ReadOnly Property CommandLine() As String
            Get
                Return m_CommandLine
            End Get
        End Property

        Public ReadOnly Property EnvironmentVariables() As IEnumerable(Of String)
            Get
                Return m_EnvironmentVariables
            End Get
        End Property

        Public ReadOnly Property WorkingDirectory() As String
            Get
                Return m_TempPath.GetPath()
            End Get
        End Property

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_TempPath.Dispose()
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
