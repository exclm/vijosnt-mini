Friend Class RandomString
    Protected Const m_ValidChars As String = "abcdefghijklmnopqrstuvwxyz0123456789"
    Protected m_Random As Random

    Public Sub New()
        m_Random = New Random()
    End Sub

    Public Function [Next](ByVal Length As Int32) As String
        Dim Builder As New StringBuilder(Length, Length)

        For Index As Int32 = 0 To Length - 1
            Builder.Append(m_ValidChars(m_Random.Next(m_ValidChars.Length)))
        Next

        Return Builder.ToString()
    End Function
End Class
