Imports VijosNT.Win32

Namespace Utility
    Friend Class ProcessMonitor
        Implements IDisposable

        Public Delegate Sub Completion(ByVal Result As Result)

        Private m_DebugObject As DebugObject
        Private m_Thread As Thread
        Private m_Contexts As Dictionary(Of Int32, Context)

        Private Class Context
            Implements IDisposable

            Public Sub New(ByVal Process As KernelObject, ByVal Completion As Completion)
                m_Process = Process
                m_Completion = Completion
                m_Exception = Nothing
            End Sub

            Public ReadOnly Property Process() As KernelObject
                Get
                    Return m_Process
                End Get
            End Property

            Public ReadOnly Property Completion() As Completion
                Get
                    Return m_Completion
                End Get
            End Property

            Public Property Exception As Nullable(Of EXCEPTION_RECORD)
                Get
                    Return m_Exception
                End Get

                Set(ByVal Value As Nullable(Of EXCEPTION_RECORD))
                    m_Exception = Value
                End Set
            End Property

            Private m_Process As KernelObject
            Private m_Completion As Completion
            Private m_Exception As Nullable(Of EXCEPTION_RECORD)

#Region "IDisposable Support"
            Private disposedValue As Boolean ' 检测冗余的调用

            ' IDisposable
            Protected Overridable Sub Dispose(ByVal disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        m_Process.Close()
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

        Public Class Result
            Public Sub New(ByVal ExitStatus As NTSTATUS, ByVal Exception As Nullable(Of EXCEPTION_RECORD))
                Me.ExitStatus = ExitStatus
                Me.Exception = Exception
            End Sub

            Public ExitStatus As NTSTATUS
            Public Exception As Nullable(Of EXCEPTION_RECORD)
        End Class

        Public Sub New()
            m_DebugObject = New DebugObject()
            AddHandler m_DebugObject.OnExitProcess, AddressOf ExitProcessHandler
            AddHandler m_DebugObject.OnException, AddressOf ExceptionHandler
            m_Thread = New Thread(AddressOf ThreadEntry)
            m_Contexts = New Dictionary(Of Int32, Context)
        End Sub

        Public Sub Start()
            m_Thread.Start()
        End Sub

        Public Sub [Stop]()
            m_DebugObject.Close()
        End Sub

        Public Sub Attach(ByVal Process As KernelObject, ByVal Completion As Completion)
            m_DebugObject.Attach(Process.GetHandleUnsafe())

            Dim ProcessId As Int32 = GetProcessId(Process.GetHandleUnsafe())
            Win32True(ProcessId <> 0)

            SyncLock m_Contexts
                m_Contexts.Add(ProcessId, New Context(Process, Completion))
            End SyncLock
        End Sub

        Private Sub ThreadEntry()
            While m_DebugObject.WaitForEvent()
                ' Do nothing
            End While
        End Sub

        Private Sub ExitProcessHandler(ByVal Header As DebugObject.Header, ByVal Info As DebugObject.ExitProcessInfo, ByRef ContinueStatus As NTSTATUS)
            Dim Context As Context
            SyncLock m_Contexts
                Context = m_Contexts(Header.AppClientId.UniqueProcess)
                m_Contexts.Remove(Header.AppClientId.UniqueProcess)
            End SyncLock
            Context.Completion.Invoke(New Result(Info.ExitStatus, Context.Exception))
            Context.Dispose()
        End Sub

        Private Sub ExceptionHandler(ByVal Header As DebugObject.Header, ByVal Info As DebugObject.ExceptionInfo, ByRef ContinueStatus As NTSTATUS)
            If Info.FirstChance = 0 Then
                Dim Context As Context
                SyncLock m_Contexts
                    Context = m_Contexts(Header.AppClientId.UniqueProcess)
                End SyncLock
                Context.Exception = Info.ExceptionRecord
                Win32True(TerminateProcess(Context.Process.GetHandleUnsafe(), Info.ExceptionRecord.ExceptionCode))
                ContinueStatus = NTSTATUS.DBG_EXCEPTION_HANDLED
            ElseIf Info.ExceptionRecord.ExceptionCode = ExceptionCode.EXCEPTION_BREAKPOINT Then
                ContinueStatus = NTSTATUS.DBG_EXCEPTION_HANDLED
            End If
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_DebugObject.Close()
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
