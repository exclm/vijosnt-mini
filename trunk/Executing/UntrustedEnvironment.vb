Imports VijosNT.Win32

Namespace Executing
    Friend Class UntrustedEnvironment
        Inherits Environment
        Implements IDisposable

        Protected m_Desktop As Desktop
        Protected m_Token As Token

        Public Sub New(ByVal DesktopName As String, ByVal UserName As String, ByVal Password As String)
            m_Desktop = New Desktop(DesktopName)
            m_Token = New Token(UserName, Password)
        End Sub

        Public Overrides ReadOnly Property Tag() As EnvironmentTag
            Get
                Return EnvironmentTag.Untrusted
            End Get
        End Property

        Public Overrides ReadOnly Property DesktopName() As String
            Get
                Return m_Desktop.GetName()
            End Get
        End Property

        Public Overrides ReadOnly Property Token() As Token
            Get
                Return m_Token
            End Get
        End Property

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_Desktop.Dispose()
                    m_Token.Close()
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
