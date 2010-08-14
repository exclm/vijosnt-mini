﻿Imports VijosNT.Win32

Namespace Executing
    Friend MustInherit Class Environment
        Implements IDisposable

        Protected m_EnvironmentPool As EnvironmentPool

        Public Property Pool() As EnvironmentPool
            Get
                Return m_EnvironmentPool
            End Get

            Set(ByVal Value As EnvironmentPool)
                m_EnvironmentPool = Value
            End Set
        End Property

        Public Sub Untake()
            Me.Pool.Untake(Me)
        End Sub

        Public MustOverride ReadOnly Property Tag() As EnvironmentTag
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
