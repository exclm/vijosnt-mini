Imports VijosNT.Utility

Namespace Compiling
    Friend Class LocalTargetInstance
        Inherits TargetInstance
        Implements IDisposable

        Protected m_TempPath As TempPath

        Public Sub New(ByVal TempPath As TempPath)
            m_TempPath = TempPath
        End Sub

        Public Overrides ReadOnly Property ApplicationName() As String
            Get
                Return m_TempPath.Combine("Target.exe")
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
