Public NotInheritable Class WindowStation
    Inherits UserObject

    Private m_Handle As IntPtr
    Private m_Name As String

    Public Sub New()
        m_Handle = GetProcessWindowStation()

        If m_Handle = 0 Then
            Throw New Win32Exception()
        End If
    End Sub

    Protected Overrides Function GetHandle() As System.IntPtr
        Return m_Handle
    End Function

    Public Overrides Function GetName() As String
        If m_Name Is Nothing Then
            m_Name = MyBase.GetName()
        End If

        Return m_Name
    End Function
End Class
