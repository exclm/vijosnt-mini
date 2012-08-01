Namespace Sources
    Friend Class YajoSource
        Inherits Source

        Private m_ConnectionString As String
        Private m_Tester As String

        Public Sub New(ByVal Parameters As String)
            Dim Server As String = Nothing
            Dim Database As String = Nothing
            Dim UserName As String = Nothing
            Dim Password As String = Nothing

            For Each Parameter In Parameters.Split(New Char() {";"c})
                Dim Position = Parameter.IndexOf("="c)
                If Position = -1 Then Continue For
                Dim Key = Parameter.Substring(0, Position)
                Dim Value = Parameter.Substring(Position + 1)
                Select Case Key.ToLower()
                    Case "server"
                        Server = Value
                    Case "database"
                        Database = Value
                    Case "username"
                        UserName = Value
                    Case "password"
                        Password = Value
                    Case "tester"
                        m_Tester = Value
                End Select
            Next
            If Server Is Nothing Then _
                Throw New ArgumentNullException("Server")
            If Database Is Nothing Then _
                Throw New ArgumentNullException("Database")
            If UserName Is Nothing Then _
                Throw New ArgumentNullException("UserName")
            If Password Is Nothing Then _
                Throw New ArgumentNullException("Password")
            If m_Tester Is Nothing Then _
                Throw New ArgumentOutOfRangeException("Tester", "Tester 不能为空")

            Dim ConnectionBuilder = New MySqlConnectionStringBuilder()
            ConnectionBuilder.Server = Server
            ConnectionBuilder.Database = Database
            ConnectionBuilder.UserID = UserName
            ConnectionBuilder.Password = Password
            ConnectionBuilder.Pooling = True
            m_ConnectionString = ConnectionBuilder.ToString()
        End Sub
    End Class
End Namespace