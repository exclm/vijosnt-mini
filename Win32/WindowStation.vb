Namespace Win32
    Friend Class WindowStation
        Inherits UserObject

        Protected m_Handle As IntPtr
        Protected m_Name As String

        Public Sub New()
            m_Handle = GetProcessWindowStation()
            Win32True(m_Handle <> 0)
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
End Namespace