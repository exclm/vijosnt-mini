Imports VijosNT.Utility
Imports VijosNT.Win32

Namespace Executing
    Friend Class ProcessExecutee
        Inherits Executee

        Private m_ApplicationName As String
        Private m_CommandLine As String
        Private m_EnvironmentVariables As IEnumerable(Of String)
        Private m_CurrentDirectory As String
        Private m_StdInput As KernelObject
        Private m_StdOutput As KernelObject
        Private m_StdError As KernelObject
        Private m_TimeQuota As Int64?
        Private m_MemoryQuota As Int64?
        Private m_ActiveProcessQuota As Int32?
        Private m_EnableUIRestrictions As Boolean
        Private m_Completion As ProcessExecuteeCompletion

        Protected Sub New(ByVal ApplicationName As String, ByVal CommandLine As String, _
            ByVal EnvironmentVariables As IEnumerable(Of String), ByVal CurrentDirectory As String, _
            ByVal TimeQuota As Int64?, ByVal MemoryQuota As Int64?, _
            ByVal ActiveProcessQuota As Int32?, ByVal EnableUIRestrictions As Boolean)

            m_ApplicationName = ApplicationName
            m_CommandLine = CommandLine
            m_EnvironmentVariables = EnvironmentVariables
            m_CurrentDirectory = CurrentDirectory
            m_TimeQuota = TimeQuota
            m_MemoryQuota = MemoryQuota
            m_ActiveProcessQuota = ActiveProcessQuota
            m_EnableUIRestrictions = EnableUIRestrictions
        End Sub

        Protected Property CurrentDirectory() As String
            Get
                Return m_CurrentDirectory
            End Get

            Set(ByVal Value As String)
                m_CurrentDirectory = Value
            End Set
        End Property

        Protected Property StdInput() As KernelObject
            Get
                Return m_StdInput
            End Get

            Set(ByVal Value As KernelObject)
                m_StdInput = Value
            End Set
        End Property

        Protected Property StdOutput() As KernelObject
            Get
                Return m_StdOutput
            End Get

            Set(ByVal Value As KernelObject)
                m_StdOutput = Value
            End Set
        End Property

        Protected Property StdError() As KernelObject
            Get
                Return m_StdError
            End Get

            Set(ByVal Value As KernelObject)
                m_StdError = Value
            End Set
        End Property

        Protected Property TimeQuota() As Int64?
            Get
                Return m_TimeQuota
            End Get

            Set(ByVal Value As Int64?)
                m_TimeQuota = Value
            End Set
        End Property

        Protected Property MemoryQuota() As Int64?
            Get
                Return m_MemoryQuota
            End Get

            Set(ByVal Value As Int64?)
                m_MemoryQuota = Value
            End Set
        End Property

        Protected Property Completion() As ProcessExecuteeCompletion
            Get
                Return m_Completion
            End Get

            Set(ByVal Value As ProcessExecuteeCompletion)
                m_Completion = Value
            End Set
        End Property

        Public Overrides Sub Execute()
            Dim JobObject = New JobObject()
            Dim Result = New ProcessExecuteeResult()
            Dim Trigger = New MiniTrigger(2, _
                Sub()
                    JobObject.Kill(1)
                    JobObject.Close()
                    m_Completion.Invoke(Result)
                    MyBase.Execute()
                End Sub)

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
                If m_MemoryQuota.HasValue Then
                    Dim VirtualSize As Int64
                    Using Executable As New PortableExecutable(m_ApplicationName)
                        VirtualSize = Executable.VirtualSize
                    End Using
                    If VirtualSize > m_MemoryQuota.Value Then
                        Result.ExitStatus = ERROR_NOT_ENOUGH_QUOTA
                        Result.MemoryQuotaUsage = VirtualSize
                        Trigger.InvokeNow()
                        Return
                    End If
                End If

                Environment.GiveAccess(m_CurrentDirectory)

                Dim Suspended As ProcessEx.Suspended
                Try
                    Suspended = ProcessEx.CreateSuspended( _
                        m_ApplicationName, m_CommandLine, m_EnvironmentVariables, m_CurrentDirectory, Environment.DesktopName, _
                        StdInputHandle, StdOutputHandle, StdErrorHandle, Environment.Token)
                Catch ex As Exception
                    Result.ExitStatus = Nothing
                    Trigger.InvokeNow()
                    Return
                End Try

                JobObject.Assign(Suspended.GetHandleUnsafe())
                With JobObject.Limits
                    .ProcessMemory = m_MemoryQuota
                    .ActiveProcess = m_ActiveProcessQuota
                    .DieOnUnhandledException = True
                    .Commit()
                End With

                If m_EnableUIRestrictions Then
                    With JobObject.UIRestrictions
                        .Handles = True
                        .ReadClipboard = True
                        .WriteClipboard = True
                        .SystemParameters = True
                        .DisplaySettings = True
                        .GlobalAtoms = True
                        .Desktop = True
                        .ExitWindows = True
                        .Commit()
                    End With
                End If

                ProcessMonitor.Singleton().Attach(Suspended, _
                    Sub(ExitResult As ProcessMonitor.Result)
                        Result.ExitStatus = ExitResult.ExitStatus
                        Result.Exception = ExitResult.Exception
                        Trigger.InvokeNonCritical()
                    End Sub)

                WatchDog.Singleton().SetWatch(Suspended.Resume(), m_TimeQuota, _
                    Sub(TimeQuotaUsage As Int64, MemoryQuotaUsage As Int64)
                        Result.TimeQuotaUsage = TimeQuotaUsage
                        Result.MemoryQuotaUsage = MemoryQuotaUsage
                        Trigger.InvokeNonCritical()
                    End Sub)
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
