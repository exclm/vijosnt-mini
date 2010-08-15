﻿Imports VijosNT.Utility
Imports VijosNT.Win32

Namespace Executing
    Friend Class ProcessExecutee
        Inherits Executee

        Private m_WatchDog As WatchDog
        Private m_ProcessMonitor As ProcessMonitor
        Private m_JobObject As JobObject
        Private m_ApplicationName As String
        Private m_CommandLine As String
        Private m_EnvironmentVariables As IEnumerable(Of String)
        Private m_CurrentDirectory As String
        Private m_StdInput As KernelObject
        Private m_StdOutput As KernelObject
        Private m_StdError As KernelObject
        Private m_TimeQuota As Nullable(Of Int64)
        Private m_MemoryQuota As Nullable(Of Int64)
        Private m_ActiveProcessQuota As Nullable(Of Int32)
        Private m_Remaining As Int32
        Private m_Completion As ProcessExecuteeCompletion
        Private m_Result As ProcessExecuteeResult

        Protected Sub New()

        End Sub

        Public Sub New(ByVal WatchDog As WatchDog, ByVal ProcessMonitor As ProcessMonitor, _
            ByVal ApplicationName As String, ByVal CommandLine As String, _
            ByVal EnvironmentVariables As IEnumerable(Of String), ByVal CurrentDirectory As String, _
            ByVal StdInput As KernelObject, ByVal StdOutput As KernelObject, ByVal StdError As KernelObject, _
            ByVal TimeQuota As Nullable(Of Int64), ByVal MemoryQuota As Nullable(Of Int64), ByVal ActiveProcessQuota As Nullable(Of Int32), _
            ByVal Completion As ProcessExecuteeCompletion, ByVal State As Object)
            FinalConstruct(WatchDog, ProcessMonitor, ApplicationName, CommandLine, EnvironmentVariables, CurrentDirectory, StdInput, StdOutput, StdError, TimeQuota, MemoryQuota, ActiveProcessQuota, Completion, State)
        End Sub

        Protected Sub FinalConstruct(ByVal WatchDog As WatchDog, ByVal ProcessMonitor As ProcessMonitor, _
            ByVal ApplicationName As String, ByVal CommandLine As String, _
            ByVal EnvironmentVariables As IEnumerable(Of String), ByVal CurrentDirectory As String, _
            ByVal StdInput As KernelObject, ByVal StdOutput As KernelObject, ByVal StdError As KernelObject, _
            ByVal TimeQuota As Nullable(Of Int64), ByVal MemoryQuota As Nullable(Of Int64), ByVal ActiveProcessQuota As Nullable(Of Int32), _
            ByVal Completion As ProcessExecuteeCompletion, ByVal State As Object)

            m_WatchDog = WatchDog
            m_ProcessMonitor = ProcessMonitor
            m_ApplicationName = ApplicationName
            m_CommandLine = CommandLine
            m_EnvironmentVariables = EnvironmentVariables
            m_CurrentDirectory = CurrentDirectory
            m_StdInput = StdInput
            m_StdOutput = StdOutput
            m_StdError = StdError
            m_TimeQuota = TimeQuota
            m_MemoryQuota = MemoryQuota
            m_ActiveProcessQuota = ActiveProcessQuota
            m_Completion = Completion
            m_Result.State = State
        End Sub

        Private Sub WorkCompleted()
            If Interlocked.Decrement(m_Remaining) = 0 Then
                If m_Completion IsNot Nothing Then
                    m_Completion.Invoke(m_Result)
                End If
                MyBase.Execute()
            End If
        End Sub

        Private Sub WatchDogCallback(ByVal Result As WatchDog.Result)
            m_Result.TimeQuotaUsage = Result.QuotaUsage
            m_Result.MemoryQuotaUsage = m_JobObject.Limits.PeakProcessMemoryUsed
            WorkCompleted()
        End Sub

        Private Sub ProcessMonitorCallback(ByVal Result As ProcessMonitor.Result)
            m_Result.ExitStatus = Result.ExitStatus
            m_Result.Exception = Result.Exception
            WorkCompleted()
        End Sub

        Public Overrides Sub Execute()
            Dim StdInputHandle As IntPtr = IntPtr.Zero
            Dim StdOutputHandle As IntPtr = IntPtr.Zero
            Dim StdErrorHandle As IntPtr = IntPtr.Zero

            If m_StdInput IsNot Nothing Then _
                StdInputHandle = m_StdInput.GetHandleUnsafe()
            If m_StdOutput IsNot Nothing Then _
                StdOutputHandle = m_StdOutput.GetHandleUnsafe()
            If m_StdError IsNot Nothing Then _
                StdErrorHandle = m_StdError.GetHandleUnsafe()

            Try
                ' TODO: Check PE (Static memory limit)
                ' TODO: Add access to working directory

                Dim Suspended As ProcessEx.Suspended
                Try
                    Suspended = ProcessEx.CreateSuspended( _
                        m_ApplicationName, m_CommandLine, m_EnvironmentVariables, m_CurrentDirectory, Environment.DesktopName, _
                        StdInputHandle, StdOutputHandle, StdErrorHandle, Environment.Token)
                Catch ex As Exception
                    m_Result.ExitStatus = Nothing
                    m_Remaining = 1
                    WorkCompleted()
                    Return
                End Try

                m_JobObject = New JobObject()
                m_JobObject.Assign(Suspended.GetHandleUnsafe())
                With m_JobObject.Limits
                    .ProcessMemory = m_MemoryQuota
                    .ActiveProcess = m_ActiveProcessQuota
                    .DieOnUnhandledException = True
                    .Commit()
                End With

                ' TODO: UI Limit

                m_Remaining = 2
                m_ProcessMonitor.Attach(Suspended, AddressOf ProcessMonitorCallback, Nothing)
                m_WatchDog.SetWatch(Suspended.Resume(), m_TimeQuota, AddressOf WatchDogCallback, Nothing)
            Finally
                If m_StdInput IsNot Nothing Then _
                    m_StdInput.Close()
                If m_StdOutput IsNot Nothing Then _
                    m_StdOutput.Close()
                If m_StdError IsNot Nothing Then _
                    m_StdError.Close()
            End Try
        End Sub
    End Class
End Namespace
