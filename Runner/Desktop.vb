Public NotInheritable Class Desktop
    Inherits UserObject
    Implements IDisposable

    Private m_hDesktop As IntPtr
    Private m_Name As String

    Public Sub New(ByVal Name As String)
        m_hDesktop = CreateDesktop(Name, Nothing, 0, 0,
            DESKTOP_READOBJECTS Or DESKTOP_CREATEWINDOW Or DESKTOP_WRITEOBJECTS Or READ_CONTROL Or WRITE_DAC, 0)

        If m_hDesktop = 0 Then
            Throw New Win32Exception()
        End If

        m_Name = Name
    End Sub

    Protected Overrides Function GetHandle() As System.IntPtr
        Return m_hDesktop
    End Function

    Public Overrides Function GetName() As String
        Return m_Name
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            Win32True(CloseDesktop(m_hDesktop))
        End If
        Me.disposedValue = True
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
