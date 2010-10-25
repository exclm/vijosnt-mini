﻿Imports VijosNT.Win32

Namespace Utility
    Friend Class WatchDog
        Implements IDisposable

        Public Delegate Sub Completion(ByVal TimeQuotaUsage As Int64, ByVal MemoryQuotaUsage As Int64)

        Protected Structure Context
            Public Sub New(ByVal Process As ProcessEx, ByVal TimeQuota As Int64?, ByVal Completion As Completion)
                Me.Process = Process
                Me.TimeQuota = TimeQuota
                Me.Completion = Completion
            End Sub

            Dim Process As ProcessEx
            Dim TimeQuota As Int64?
            Dim Completion As Completion
        End Structure

        Protected m_WaitPool As MiniWaitPool

        Public Sub New()
            m_WaitPool = New MiniWaitPool()
        End Sub

        Public Sub Start()
            m_WaitPool.Start()
        End Sub

        Public Sub [Stop]()
            m_WaitPool.Stop()
        End Sub

        Protected Sub SetWatchCompletion(ByVal Result As MiniWaitPool.Result)
            Dim Context As Context = DirectCast(Result.State, Context)

            If Not Result.Timeouted Then
                ' The waited process was ended, fire the completion
                With Context.Process
                    Context.Completion.Invoke(.AliveTime, .PeakMemoryUsage)
                    .Close()
                End With
            Else
                ' The waited process is still running, set watch again
                SetWatchInternal(Context)
            End If
        End Sub

        Protected Sub SetWatchInternal(ByVal Context As Context)
            Dim AliveTime As Int64 = Context.Process.AliveTime

            If Context.TimeQuota Is Nothing Then
                ' The quota is not limited, set up the wait pool
                m_WaitPool.SetWait(Context.Process, Nothing, AddressOf SetWatchCompletion, Context)
            ElseIf AliveTime >= Context.TimeQuota Then
                ' The waited process exceeds the time quota, terminate it and fire the completion
                Try
                    Context.Process.Kill(ERROR_NOT_ENOUGH_QUOTA)
                Catch ex As Exception
                    ' eat it
                End Try
                With Context.Process
                    Context.Completion.Invoke(.AliveTime, .PeakMemoryUsage)
                    .Close()
                End With
            Else
                ' There is still time quota remaining, set up the wait pool
                m_WaitPool.SetWait(Context.Process, Context.TimeQuota - AliveTime + 50000, AddressOf SetWatchCompletion, Context)
            End If
        End Sub

        Public Sub SetWatch(ByVal Process As ProcessEx, ByVal TimeQuota As Int64?, ByVal Completion As Completion)
            SetWatchInternal(New Context(Process, TimeQuota, Completion))
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_WaitPool.Dispose()
                End If
            End If
            Me.disposedValue = True
        End Sub

        ' Visual Basic 添加此代码是为了正确实现可处置模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace