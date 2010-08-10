﻿Namespace Win32
    Friend Class DebugObject
        Inherits KernelObject
        Implements IDisposable

        Public Enum DebugState As Int32
            CreateThread = 2
            CreateProcess = 3
            ExitThread = 4
            ExitProcess = 5
            Exception = 6
            Breakpoint = 7
            SingleStep = 8
            LoadDll = 9
            UnloadDll = 10
        End Enum

        Public Structure Header
            Dim NewState As DebugState
            Dim AppClientId As CLIENT_ID
        End Structure

        Public Structure NewThreadInfo
            Dim SubSystemKey As Int32
            Dim StartAddress As IntPtr
        End Structure

        Public Structure CreateThreadInfo
            Dim HandleToThread As IntPtr
            Dim NewThread As NewThreadInfo
        End Structure

        Public Structure NewProcessInfo
            Dim SubSystemKey As Int32
            Dim FileHandle As IntPtr
            Dim BaseOfImage As IntPtr
            Dim DebugInfoFileOffset As Int32
            Dim DebugInfoSize As Int32
            Dim InitialThread As NewThreadInfo
        End Structure

        Public Structure CreateProcessInfo
            Dim HandleToProcess As IntPtr
            Dim HandleToThread As IntPtr
            Dim NewProcess As NewProcessInfo
        End Structure

        Public Structure ExceptionInfo
            Dim ExceptionRecord As EXCEPTION_RECORD
            Dim FirstChance As Int32
        End Structure

        Public Structure ExitThreadInfo
            Dim ExitStatus As NTSTATUS
        End Structure

        Public Structure ExitProcessInfo
            Dim ExitStatus As NTSTATUS
        End Structure

        Public Structure LoadDllInfo
            Dim FileHandle As IntPtr
            Dim BaseOfDll As IntPtr
            Dim DebugInfoFileOffset As Int32
            Dim DebugInfoSize As Int32
            Dim NamePointer As IntPtr
        End Structure

        Public Structure UnloadDllInfo
            Dim BaseAddress As IntPtr
        End Structure

        Public Sub New()
            Dim DebugObjectHandle As IntPtr
            NtSuccess(NtCreateDebugObject(DebugObjectHandle, DebugObjectAccess.DEBUG_OBJECT_ALL_ACCESS, IntPtr.Zero, DebugObjectFlags.DEBUG_OBJECT_KILL_ON_CLOSE))
            InternalSetHandle(DebugObjectHandle)
        End Sub

        Public Sub Attach(ByVal Process As KernelObject)
            NtSuccess(NtDebugActiveProcess(Process.GetHandleUnsafe(), MyBase.GetHandleUnsafe()))
        End Sub

        Public Event OnCreateThread(ByVal Header As Header, ByVal Info As CreateThreadInfo)
        Public Event OnCreateProcess(ByVal Header As Header, ByVal Info As CreateProcessInfo)
        Public Event OnExitThread(ByVal Header As Header, ByVal Info As ExitThreadInfo)
        Public Event OnExitProcess(ByVal Header As Header, ByVal Info As ExitProcessInfo)
        Public Event OnException(ByVal Header As Header, ByVal Info As ExceptionInfo, ByRef Handled As Boolean)
        Public Event OnLoadDll(ByVal Header As Header, ByVal Info As LoadDllInfo)
        Public Event OnUnloadDll(ByVal Header As Header, ByVal Info As UnloadDllInfo)

        Public Function WaitForEvent() As Boolean
            Dim BufferPtr As IntPtr = Marshal.AllocHGlobal(IntPtr.Size * 22 + 8)
            Try
                Dim Status As NTSTATUS = NtWaitForDebugEvent(MyBase.GetHandleUnsafe(), False, IntPtr.Zero, BufferPtr)
                If Status = NTSTATUS.STATUS_DEBUGGER_INACTIVE Then Return False
                NtSuccess(Status)
                Dim Header As Header = Marshal.PtrToStructure(BufferPtr, GetType(Header))
                Dim InfoPtr As IntPtr = BufferPtr.ToInt64() + Marshal.SizeOf(Header)
                Select Case Header.NewState
                    Case DebugState.CreateThread
                        Dim Info As CreateThreadInfo = Marshal.PtrToStructure(InfoPtr, GetType(CreateThreadInfo))
                        RaiseEvent OnCreateThread(Header, Info)
                        NtSuccess(NtClose(Info.HandleToThread))
                    Case (DebugState.CreateProcess)
                        Dim Info As CreateProcessInfo = Marshal.PtrToStructure(InfoPtr, GetType(CreateProcessInfo))
                        RaiseEvent OnCreateProcess(Header, Info)
                        NtSuccess(NtClose(Info.HandleToProcess))
                        NtSuccess(NtClose(Info.HandleToThread))
                        NtSuccess(NtClose(Info.NewProcess.FileHandle))
                    Case DebugState.ExitThread
                        Dim Info As ExitThreadInfo = Marshal.PtrToStructure(InfoPtr, GetType(ExitThreadInfo))
                        RaiseEvent OnExitThread(Header, Info)
                    Case DebugState.ExitProcess
                        Dim Info As ExitProcessInfo = Marshal.PtrToStructure(InfoPtr, GetType(ExitProcessInfo))
                        RaiseEvent OnExitProcess(Header, Info)
                    Case DebugState.Exception, DebugState.Breakpoint, DebugState.SingleStep
                        Dim Info As ExceptionInfo = Marshal.PtrToStructure(InfoPtr, GetType(ExceptionInfo))
                        Dim Handled As Boolean = False
                        RaiseEvent OnException(Header, Info, Handled)
                        If Handled Then
                            NtSuccess(NtDebugContinue(MyBase.GetHandleUnsafe(), Header.AppClientId, NTSTATUS.DBG_EXCEPTION_HANDLED))
                        Else
                            NtSuccess(NtDebugContinue(MyBase.GetHandleUnsafe(), Header.AppClientId, NTSTATUS.DBG_EXCEPTION_NOT_HANDLED))
                        End If
                        Return True
                    Case DebugState.LoadDll
                        Dim Info As LoadDllInfo = Marshal.PtrToStructure(InfoPtr, GetType(LoadDllInfo))
                        RaiseEvent OnLoadDll(Header, Info)
                        NtSuccess(NtClose(Info.FileHandle))
                    Case DebugState.UnloadDll
                        Dim Info As UnloadDllInfo = Marshal.PtrToStructure(InfoPtr, GetType(UnloadDllInfo))
                        RaiseEvent OnUnloadDll(Header, Info)
                    Case Else
                        Throw New Win32Exception("Unknown debug state")
                End Select
                NtSuccess(NtDebugContinue(MyBase.GetHandleUnsafe(), Header.AppClientId, NTSTATUS.DBG_CONTINUE))
                Return True
            Finally
                Marshal.FreeHGlobal(BufferPtr)
            End Try
        End Function
    End Class
End Namespace