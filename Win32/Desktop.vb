Public NotInheritable Class Desktop
    Inherits UserObject
    Implements IDisposable

    Private m_Handle As IntPtr
    Private m_Name As String

    Public Sub New(ByVal Name As String)
        m_Handle = CreateDesktop(Name, Nothing, 0, 0, DesktopAccess.DESKTOP_READOBJECTS Or DesktopAccess.DESKTOP_CREATEWINDOW Or _
            DesktopAccess.DESKTOP_WRITEOBJECTS Or DesktopAccess.READ_CONTROL Or DesktopAccess.WRITE_DAC, 0)

        If m_Handle = 0 Then
            Throw New Win32Exception()
        End If

        m_Name = Name
    End Sub

    Protected Overrides Function GetHandle() As System.IntPtr
        Return m_Handle
    End Function

    Public Overrides Function GetName() As String
        Return m_Name
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            Win32True(CloseDesktop(m_Handle))
        End If
        Me.disposedValue = True
        MyBase.Dispose(disposing)
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub
#End Region
End Class
