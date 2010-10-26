Namespace Foreground
    Friend Class TestResultForm
        Private Shared m_Xslt As XslCompiledTransform

        Shared Sub New()
            m_Xslt = New XslCompiledTransform()
            m_Xslt.Load(New XmlTextReader(New StringReader(My.Resources.TestReport)))
        End Sub

        Public Sub New(ByVal Input As XmlReader)

            ' 此调用是设计器所必需的。
            InitializeComponent()

            ' 在 InitializeComponent() 调用之后添加任何初始化。
            Using Writer As New StringWriter()
                m_Xslt.Transform(Input, New XmlTextWriter(Writer))
                WebBrowser.DocumentText = Writer.ToString()
            End Using
        End Sub
    End Class
End Namespace