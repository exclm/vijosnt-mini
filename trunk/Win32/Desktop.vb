Namespace Win32
    Friend Class Desktop
        Inherits UserObject
        Implements IDisposable

        Protected m_Handle As IntPtr
        Protected m_Name As String

        Public Sub New(ByVal Name As String)
            m_Handle = CreateDesktop(Name, Nothing, 0, 0, DesktopAccess.DESKTOP_READOBJECTS Or DesktopAccess.DESKTOP_CREATEWINDOW Or _
                DesktopAccess.DESKTOP_WRITEOBJECTS Or DesktopAccess.READ_CONTROL Or DesktopAccess.WRITE_DAC Or DesktopAccess.DESKTOP_SWITCHDESKTOP, 0)
            Win32True(m_Handle <> 0)
            m_Name = Name
        End Sub

        Protected Overrides Function GetHandle() As System.IntPtr
            Return m_Handle
        End Function

        Public Overrides Function GetName() As String
            Return m_Name
        End Function

        Public Sub SwitchTo()
            Win32True(SwitchDesktop(m_Handle))
        End Sub

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
End Namespace