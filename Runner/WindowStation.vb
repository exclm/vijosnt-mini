Public NotInheritable Class WindowStation
    Inherits UserObject

    Private m_hWinSta As IntPtr
    Private m_Name As String

    Public Sub New()
        m_hWinSta = GetProcessWindowStation()

        If m_hWinSta = 0 Then
            Throw New Win32Exception()
        End If

        m_Name = MyBase.GetName()
    End Sub

    Protected Overrides Function GetHandle() As System.IntPtr
        Return m_hWinSta
    End Function

    Public Overrides Function GetName() As String
        Return m_Name
    End Function
End Class
