Namespace Utility
    Friend Class MiniTrigger
        Public Delegate Sub Completion()

        Private m_NumberOfCriticals As Int32
        Private m_NumberOfNonCriticals As Int32
        Private m_CriticalEvent As ManualResetEvent
        Private m_Completion As Completion

        Public Sub New(ByVal NumberOfNonCriticals As Int32, ByVal Completion As Completion)
            m_NumberOfNonCriticals = NumberOfNonCriticals
            m_CriticalEvent = New ManualResetEvent(True)
            m_Completion = Completion
        End Sub

        Public Sub New(ByVal NumberOfCriticals As Int32, ByVal NumberOfNonCriticals As Int32, ByVal Completion As Completion)
            m_NumberOfCriticals = NumberOfCriticals
            m_NumberOfNonCriticals = NumberOfNonCriticals
            m_CriticalEvent = New ManualResetEvent(False)
            m_Completion = Completion
        End Sub

        Public Sub InvokeCritical()
            If Interlocked.Decrement(m_NumberOfCriticals) = 0 Then
                m_CriticalEvent.Set()
            End If
        End Sub

        Public Sub InvokeNonCritical()
            If Interlocked.Decrement(m_NumberOfNonCriticals) = 0 Then
                m_CriticalEvent.WaitOne()
                InvokeNow()
            End If
        End Sub

        Public Sub InvokeNow()
            m_CriticalEvent.Close()
            m_Completion.Invoke()
        End Sub
    End Class
End Namespace
