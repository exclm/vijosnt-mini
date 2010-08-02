Friend Class JobObject
    Implements IDisposable

    Public Class Limits
        Protected m_Data As JOBOBJECT_EXTENDED_LIMIT_INFORMATION

        Public Sub New()
            m_Data.BasicLimitInformation.PriorityClass = PriorityClass.Normal
            m_Data.BasicLimitInformation.SchedulingClass = 5
        End Sub

        Public Sub New(ByVal JobObjectHandle As IntPtr)
            Win32True(QueryInformationJobObject(JobObjectHandle, JobObjectInfoClass.JobObjectExtendedLimitInformation, m_Data, Marshal.SizeOf(m_Data), Nothing))
        End Sub

        Public Sub SetInformation(ByVal JobObjectHandle As IntPtr)
            Win32True(SetInformationJobObject(JobObjectHandle, JobObjectInfoClass.JobObjectExtendedLimitInformation, m_Data, Marshal.SizeOf(m_Data)))
        End Sub

        Public Function GetProcessTimeLimit() As Nullable(Of Int64)
            If (m_Data.BasicLimitInformation.LimitFlags And JobObjectLimitFlags.JOB_OBJECT_LIMIT_PROCESS_TIME) <> 0 Then
                Return m_Data.BasicLimitInformation.PerProcessUserTimeLimit
            Else
                Return Nothing
            End If
        End Function

        Public Function SetProcessTimeLimit(ByVal Value As Nullable(Of Int64)) As Limits
            If Value IsNot Nothing Then
                m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags Or JobObjectLimitFlags.JOB_OBJECT_LIMIT_PROCESS_TIME
                m_Data.BasicLimitInformation.PerProcessUserTimeLimit = Value
            Else
                m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags And Not JobObjectLimitFlags.JOB_OBJECT_LIMIT_PROCESS_TIME
            End If
            Return Me
        End Function

        Public Function GetPriorityClassLimit() As Nullable(Of PriorityClass)
            If (m_Data.BasicLimitInformation.LimitFlags And JobObjectLimitFlags.JOB_OBJECT_LIMIT_PRIORITY_CLASS) = JobObjectLimitFlags.JOB_OBJECT_LIMIT_PRIORITY_CLASS Then
                Return m_Data.BasicLimitInformation.PriorityClass
            Else
                Return Nothing
            End If
        End Function

        Public Function SetPriorityClassLimit(ByVal Value As Nullable(Of PriorityClass)) As Limits
            If Value IsNot Nothing Then
                m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags Or JobObjectLimitFlags.JOB_OBJECT_LIMIT_PRIORITY_CLASS
                m_Data.BasicLimitInformation.PriorityClass = Value
            Else
                m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags And Not JobObjectLimitFlags.JOB_OBJECT_LIMIT_PRIORITY_CLASS
            End If
            Return Me
        End Function

        Public Function GetActiveProcessLimit() As Nullable(Of Int32)
            If (m_Data.BasicLimitInformation.LimitFlags And JobObjectLimitFlags.JOB_OBJECT_LIMIT_ACTIVE_PROCESS) <> 0 Then
                Return m_Data.BasicLimitInformation.ActiveProcessLimit
            Else
                Return Nothing
            End If
        End Function

        Public Function SetActiveProcessLimit(ByVal Value As Nullable(Of Int32)) As Limits
            If Value IsNot Nothing Then
                m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags Or JobObjectLimitFlags.JOB_OBJECT_LIMIT_ACTIVE_PROCESS
                m_Data.BasicLimitInformation.ActiveProcessLimit = Value
            Else
                m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags And Not JobObjectLimitFlags.JOB_OBJECT_LIMIT_ACTIVE_PROCESS
            End If
            Return Me
        End Function

        Public Function GetProcessMemoryLimit() As Nullable(Of IntPtr)
            If (m_Data.BasicLimitInformation.LimitFlags And JobObjectLimitFlags.JOB_OBJECT_LIMIT_PROCESS_MEMORY) <> 0 Then
                Return m_Data.ProcessMemoryLimit
            Else
                Return Nothing
            End If
        End Function

        Public Function SetProcessMemoryLimit(ByVal Value As Nullable(Of IntPtr)) As Limits
            If Value IsNot Nothing Then
                m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags Or JobObjectLimitFlags.JOB_OBJECT_LIMIT_PROCESS_MEMORY
                m_Data.ProcessMemoryLimit = Value
            Else
                m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags And Not JobObjectLimitFlags.JOB_OBJECT_LIMIT_PROCESS_MEMORY
            End If
            Return Me
        End Function
    End Class

    Public Class UIRestrictions
        Protected m_Data As JOBOBJECT_BASIC_UI_RESTRICTIONS

        ' TODO: implement this class
    End Class

    Protected m_Handle As IntPtr

    Public Sub New()
        m_Handle = CreateJobObject(Nothing, Nothing)

        If m_Handle = 0 Then
            Throw New Win32Exception()
        End If
    End Sub

    Public Function CreateLimits() As Limits
        Return New Limits()
    End Function

    Public Function GetLimits() As Limits
        Return New Limits(m_Handle)
    End Function

    Public Sub SetLimits(ByVal Limits As Limits)
        Limits.SetInformation(m_Handle)
    End Sub

    ' TODO: implement UI restrictions

    Public Sub Assign(ByVal ProcessHandle As IntPtr)
        Win32True(AssignProcessToJobObject(m_Handle, ProcessHandle))
    End Sub

    Public Sub Terminate(ByVal ExitCode As Int32)
        Win32True(TerminateJobObject(m_Handle, ExitCode))
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            Win32True(CloseHandle(m_Handle))
        End If
        Me.disposedValue = True
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
