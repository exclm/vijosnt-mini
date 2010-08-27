Imports VijosNT.Win32

Namespace Utility
    Friend Class ProcessMonitor
        Public Delegate Sub Callback(ByVal Result As Result)

        Private m_DebugObject As DebugObject
        Private m_Thread As Thread
        Private m_Contexts As Dictionary(Of Int32, Context)

        Private Class Context
            Implements IDisposable

            Public Sub New(ByVal Process As KernelObject, ByVal Callback As Callback, ByVal State As Object)
                m_Process = Process
                m_Callback = Callback
                m_CallbackState = State
                m_Exception = Nothing
            End Sub

            Public ReadOnly Property Process() As KernelObject
                Get
                    Return m_Process
                End Get
            End Property

            Public ReadOnly Property Callback() As Callback
                Get
                    Return m_Callback
                End Get
            End Property

            Public ReadOnly Property CallbackState() As Object
                Get
                    Return m_CallbackState
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
            Private m_Callback As Callback
            Private m_CallbackState As Object
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

        Public Structure Result
            Public Sub New(ByVal State As Object, ByVal ExitStatus As NTSTATUS, ByVal Exception As Nullable(Of EXCEPTION_RECORD))
                Me.State = State
                Me.ExitStatus = ExitStatus
                Me.Exception = Exception
            End Sub

            Dim State As Object
            Dim ExitStatus As NTSTATUS
            Dim Exception As Nullable(Of EXCEPTION_RECORD)
        End Structure

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

        Public Sub Attach(ByVal Process As KernelObject, ByVal Callback As Callback, ByVal State As Object)
            m_DebugObject.Attach(Process.GetHandleUnsafe())

            Dim ProcessId As Int32 = GetProcessId(Process.GetHandleUnsafe())
            Win32True(ProcessId <> 0)

            SyncLock m_Contexts
                m_Contexts.Add(ProcessId, New Context(Process, Callback, State))
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
            Context.Callback.Invoke(New Result(Context.CallbackState, Info.ExitStatus, Context.Exception))
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
    End Class
End Namespace
