Imports VijosNT.Feeding
Imports VijosNT.LocalDb
Imports VijosNT.Remoting

Namespace Background
    Friend Class Service
        Inherits ServiceBase

        Private m_Server As PipeServer

        Public Sub New()
            AutoLog = False
            ServiceName = My.Resources.ServiceName
            Runner.Singleton()
            m_Server = New PipeServer()
        End Sub

        Protected Overrides Sub OnStart(ByVal args() As String)
            Dim Builder As New StringBuilder()
            Dim Assembly As Assembly = Assembly.GetExecutingAssembly()
            Builder.AppendLine("服务已成功启动。")
            Builder.AppendLine("VijosNT Mini 版本: " & Assembly.GetName().Version.ToString())
            Builder.AppendLine("公共语言运行库版本: " & Assembly.ImageRuntimeVersion)
            Builder.AppendLine("启动时间: " & Date.Now.ToString())
            Builder.AppendLine("数据库引擎: " & Database.EngineVersion)
            EventLog.WriteEntry(Builder.ToString())
        End Sub

        Protected Overrides Sub OnStop()
            Runner.Singleton.Dispose()
            EventLog.WriteEntry("服务已成功停止。")
        End Sub
    End Class
End Namespace
