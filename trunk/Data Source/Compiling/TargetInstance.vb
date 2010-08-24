﻿Namespace Compiling
    Friend MustInherit Class TargetInstance
        Implements IDisposable

        Public MustOverride ReadOnly Property ApplicationName() As String

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
