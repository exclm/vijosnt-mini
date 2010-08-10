Imports VijosNT_Mini.Utility
Imports VijosNT_Mini.Win32

Namespace Executing
    Friend Class Executee
        Protected m_WaitPool As MiniWaitPool
        Protected m_RequiredEnvironment As EnvironmentTag
        Protected m_ApplicationName As String
        Protected m_Callback As ExecuteCompletionCallback
        Protected m_CallbackState As Object

        Public Sub New(ByVal WaitPool As MiniWaitPool, ByVal RequiredEnvironment As EnvironmentTag, ByVal ApplicationName As String, ByVal Callback As ExecuteCompletionCallback, ByVal State As Object)
            m_WaitPool = WaitPool
            m_RequiredEnvironment = RequiredEnvironment
            m_ApplicationName = ApplicationName
            m_Callback = Callback
            m_CallbackState = State
        End Sub

        Public ReadOnly Property RequiredEnvironment() As EnvironmentTag
            Get
                Return m_RequiredEnvironment
            End Get
        End Property

        Public Sub Execute(ByVal Environment As Environment)
            Using Suspended As ProcessEx.Suspended = ProcessEx.CreateSuspended(m_ApplicationName, Nothing, Nothing, Nothing, Environment.DesktopName, Nothing, Nothing, Nothing, Environment.Token)
                Dim Process As ProcessEx = Suspended.Resume()
                m_WaitPool.SetWait(Process, Nothing, _
                    Sub()
                        If m_Callback IsNot Nothing Then
                            m_Callback.Invoke(New ExecuteResult(m_CallbackState))
                        End If
                        Environment.Untake()
                    End Sub, Nothing)
            End Using
        End Sub
    End Class
End Namespace
