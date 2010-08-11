Imports VijosNT.Utility
Imports VijosNT.Win32

Namespace Executing
    Friend Class ProcessExecutee
        Inherits Executee

        Protected m_WatchDog As WatchDog
        Protected m_ApplicationName As String
        Protected m_CommandLine As String
        Protected m_TimeQuota As Nullable(Of Int64)
        Protected m_Remaining As Int32
        Protected m_Completion As ProcessExecuteeCompletion
        Protected m_Result As ProcessExecuteeResult

        Public Sub New(ByVal WatchDog As WatchDog, ByVal ApplicationName As String, ByVal CommandLine As String, ByVal TimeQuota As Nullable(Of Int64), ByVal Completion As ProcessExecuteeCompletion, ByVal State As Object)
            m_WatchDog = WatchDog
            m_ApplicationName = ApplicationName
            m_CommandLine = CommandLine
            m_TimeQuota = TimeQuota
            m_Completion = Completion
            m_Result.State = State
        End Sub

        Protected Sub WorkCompleted()
            If Interlocked.Decrement(m_Remaining) = 0 Then
                If m_Completion IsNot Nothing Then
                    Try
                        m_Completion.Invoke(m_Result)
                    Catch ex As Exception
                        ' eat it
                    End Try
                End If
                MyBase.Execute()
            End If
        End Sub

        Protected Sub WatchDogCallback(ByVal Result As WatchDog.Result)
            m_Result.TimeQuotaUsage = Result.QuotaUsage
            WorkCompleted()
        End Sub

        Protected Sub ExecuteWorker()
            m_Remaining = 1
            Using Suspended As ProcessEx.Suspended = ProcessEx.CreateSuspended( _
                m_ApplicationName, m_CommandLine, Nothing, Nothing, Environment.DesktopName, _
                Nothing, Nothing, Nothing, Environment.Token)
                ' TODO: Stdio
                ' TODO: Job object
                ' TODO: Debugger
                Dim Process As ProcessEx = Suspended.Resume()
                m_WatchDog.SetWatch(Process, m_TimeQuota, AddressOf WatchDogCallback, Nothing)
            End Using
        End Sub

        Public Overrides Sub Execute()
            ThreadPool.UnsafeQueueUserWorkItem(AddressOf ExecuteWorker, Nothing)
        End Sub
    End Class
End Namespace
