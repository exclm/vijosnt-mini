Imports VijosNT.Win32

Namespace Executing
    Friend MustInherit Class EnvironmentBase
        Implements IDisposable

        Protected m_EnvironmentPool As EnvironmentPoolBase

        Public Property Pool() As EnvironmentPoolBase
            Get
                Return m_EnvironmentPool
            End Get

            Set(ByVal Value As EnvironmentPoolBase)
                m_EnvironmentPool = Value
            End Set
        End Property

        Public Sub Untake()
            Me.Pool.Untake(Me)
        End Sub

        Public Overridable Sub GiveAccess(ByVal DirectoryName As String)
            ' Do nothing
        End Sub

        Public MustOverride ReadOnly Property DesktopName() As String
        Public MustOverride ReadOnly Property Token() As Token

#Region "IDisposable Support"
        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            ' Do nothing
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
