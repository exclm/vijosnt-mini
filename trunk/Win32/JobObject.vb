Friend Class JobObject
    Implements IDisposable

    Public Class Limits
        Protected m_Data As JOBOBJECT_EXTENDED_LIMIT_INFORMATION

        Public Enum Limit
            BreakawayOk = &H800
            DieOnUnhandledException = &H400
            KillOnJobClose = &H2000
            SilentBreakawayOk = &H1000
        End Enum

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

        Public Function SetLimit(ByVal Limit As Limit, ByVal Value As Boolean) As Limits
            If Value Then
                m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags Or Limit
            Else
                m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags And Not Limit
            End If
            Return Me
        End Function

        Public Function GetLimit(ByVal Limit As Limit) As Boolean
            Return (m_Data.BasicLimitInformation.LimitFlags And Limit) = Limit
        End Function
    End Class

    Public Class UIRestrictions
        Protected m_Data As JOBOBJECT_BASIC_UI_RESTRICTIONS

        Public Enum Limit As Int32
            [Handles] = &H1
            ReadClipboard = &H2
            WriteClipboard = &H4
            SystemParameters = &H8
            DisplaySettings = &H10
            GlobalAtoms = &H20
            Desktop = &H40
            ExitWindows = &H80
            All = &HFF
        End Enum

        Public Sub New()
            ' Do nothing
        End Sub

        Public Sub New(ByVal JobObjectHandle As IntPtr)
            Win32True(QueryInformationJobObject(JobObjectHandle, JobObjectInfoClass.JobObjectBasicUIRestrictions, m_Data, Marshal.SizeOf(m_Data), Nothing))
        End Sub

        Public Sub SetInformation(ByVal JobObjectHandle As IntPtr)
            Win32True(SetInformationJobObject(JobObjectHandle, JobObjectInfoClass.JobObjectBasicUIRestrictions, m_Data, Marshal.SizeOf(m_Data)))
        End Sub

        Public Function SetLimit(ByVal Limit As Limit, ByVal Value As Boolean) As UIRestrictions
            If Value Then
                m_Data.UIRestrictionClass = m_Data.UIRestrictionClass Or Limit
            Else
                m_Data.UIRestrictionClass = m_Data.UIRestrictionClass And Not Limit
            End If
            Return Me
        End Function

        Public Function GetLimit(ByVal Limit As Limit) As Boolean
            Return (m_Data.UIRestrictionClass And Limit) = Limit
        End Function
    End Class

    Protected m_Handle As Handle

    Public Sub New()
        Dim JobHandle As IntPtr = CreateJobObject(Nothing, Nothing)
        Win32True(JobHandle <> 0)
        m_Handle = New Handle(JobHandle)
    End Sub

    Public Shared Function Create() As JobObject
        Return New JobObject()
    End Function

    Public Shared Function CreateLimits() As Limits
        Return New Limits()
    End Function

    Public Function GetLimits() As Limits
        Return New Limits(m_Handle.GetHandleUnsafe())
    End Function

    Public Function SetLimits(ByVal Limits As Limits) As JobObject
        Limits.SetInformation(m_Handle.GetHandleUnsafe())
        Return Me
    End Function

    Public Shared Function CreateUIRestrictions() As UIRestrictions
        Return New UIRestrictions()
    End Function

    Public Function GetUIRestrictions() As UIRestrictions
        Return New UIRestrictions(m_Handle.GetHandleUnsafe())
    End Function

    Public Function SetUIRestrictions(ByVal UIRestrictions As UIRestrictions) As JobObject
        UIRestrictions.SetInformation(m_Handle.GetHandleUnsafe())
        Return Me
    End Function

    Public Sub Assign(ByVal Process As Handle)
        Win32True(AssignProcessToJobObject(m_Handle.GetHandleUnsafe(), Process.GetHandleUnsafe()))
    End Sub

    Public Sub Terminate(ByVal ExitCode As Int32)
        Win32True(TerminateJobObject(m_Handle.GetHandleUnsafe(), ExitCode))
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            m_Handle.Dispose()
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
