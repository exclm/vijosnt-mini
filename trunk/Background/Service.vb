Imports VijosNT.LocalDb

Namespace Background
    Friend Class Service
        Inherits ServiceBase

        Private m_Runner As Runner

        Public Sub New()
            AutoLog = False
            ServiceName = My.Resources.ServiceName
            m_Runner = New Runner()
        End Sub

        Protected Overrides Sub OnStart(ByVal args() As String)
            WriteStartupLog()
        End Sub

        Protected Overrides Sub OnStop()
            m_Runner.Dispose()
            EventLog.WriteEntry("服务已成功停止。")
        End Sub

        Private Sub WriteStartupLog()
            Dim Builder As New StringBuilder()
            Dim Assembly As Assembly = Assembly.GetExecutingAssembly()
            Builder.AppendLine("服务已成功启动。")
            Builder.AppendLine("VijosNT Mini 版本: " & Assembly.GetName().Version.ToString())
            Builder.AppendLine("公共语言运行库版本: " & Assembly.ImageRuntimeVersion)
            Builder.AppendLine("启动时间: " & Date.Now.ToString())
            Builder.AppendLine("数据库引擎: " & Database.EngineVersion)
            EventLog.WriteEntry(Builder.ToString())
        End Sub

        Private Sub InitializeComponent()

        End Sub
    End Class
End Namespace
