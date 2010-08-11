Namespace Win32
    Friend Class JobObject
        Inherits KernelObject
        Implements IDisposable

        Public Class JobLimits
            Protected m_Data As JOBOBJECT_EXTENDED_LIMIT_INFORMATION
            Protected m_JobObject As JobObject

            Public Sub New(ByVal JobObject As JobObject)
                m_JobObject = JobObject
                Update()
            End Sub

            Public Sub Commit()
                Win32True(SetInformationJobObject(m_JobObject.GetHandleUnsafe(), JobObjectInfoClass.JobObjectExtendedLimitInformation, m_Data, Marshal.SizeOf(m_Data)))
            End Sub

            Public Sub Update()
                Win32True(QueryInformationJobObject(m_JobObject.GetHandleUnsafe(), JobObjectInfoClass.JobObjectExtendedLimitInformation, m_Data, Marshal.SizeOf(m_Data), Nothing))
            End Sub

            Public Property ProcessTime() As Nullable(Of Int64)
                Get
                    If (m_Data.BasicLimitInformation.LimitFlags And JobObjectLimitFlags.JOB_OBJECT_LIMIT_PROCESS_TIME) <> 0 Then
                        Return m_Data.BasicLimitInformation.PerProcessUserTimeLimit
                    Else
                        Return Nothing
                    End If
                End Get

                Set(ByVal Value As Nullable(Of Int64))
                    If Value IsNot Nothing Then
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags Or JobObjectLimitFlags.JOB_OBJECT_LIMIT_PROCESS_TIME
                        m_Data.BasicLimitInformation.PerProcessUserTimeLimit = Value
                    Else
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags And Not JobObjectLimitFlags.JOB_OBJECT_LIMIT_PROCESS_TIME
                    End If
                End Set
            End Property

            Public Property PriorityClass() As Nullable(Of PriorityClass)
                Get
                    If (m_Data.BasicLimitInformation.LimitFlags And JobObjectLimitFlags.JOB_OBJECT_LIMIT_PRIORITY_CLASS) = JobObjectLimitFlags.JOB_OBJECT_LIMIT_PRIORITY_CLASS Then
                        Return m_Data.BasicLimitInformation.PriorityClass
                    Else
                        Return Nothing
                    End If
                End Get

                Set(ByVal Value As Nullable(Of PriorityClass))
                    If Value IsNot Nothing Then
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags Or JobObjectLimitFlags.JOB_OBJECT_LIMIT_PRIORITY_CLASS
                        m_Data.BasicLimitInformation.PriorityClass = Value
                    Else
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags And Not JobObjectLimitFlags.JOB_OBJECT_LIMIT_PRIORITY_CLASS
                    End If
                End Set
            End Property

            Public Property ActiveProcess() As Nullable(Of Int32)
                Get
                    If (m_Data.BasicLimitInformation.LimitFlags And JobObjectLimitFlags.JOB_OBJECT_LIMIT_ACTIVE_PROCESS) <> 0 Then
                        Return m_Data.BasicLimitInformation.ActiveProcessLimit
                    Else
                        Return Nothing
                    End If
                End Get

                Set(ByVal Value As Nullable(Of Int32))
                    If Value IsNot Nothing Then
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags Or JobObjectLimitFlags.JOB_OBJECT_LIMIT_ACTIVE_PROCESS
                        m_Data.BasicLimitInformation.ActiveProcessLimit = Value
                    Else
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags And Not JobObjectLimitFlags.JOB_OBJECT_LIMIT_ACTIVE_PROCESS
                    End If
                End Set
            End Property

            Public Property ProcessMemory() As Nullable(Of IntPtr)
                Get
                    If (m_Data.BasicLimitInformation.LimitFlags And JobObjectLimitFlags.JOB_OBJECT_LIMIT_PROCESS_MEMORY) <> 0 Then
                        Return m_Data.ProcessMemoryLimit
                    Else
                        Return Nothing
                    End If
                End Get

                Set(ByVal Value As Nullable(Of IntPtr))
                    If Value IsNot Nothing Then
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags Or JobObjectLimitFlags.JOB_OBJECT_LIMIT_PROCESS_MEMORY
                        m_Data.ProcessMemoryLimit = Value
                    Else
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags And Not JobObjectLimitFlags.JOB_OBJECT_LIMIT_PROCESS_MEMORY
                    End If
                End Set
            End Property

            Public Property BreakawayOk() As Boolean
                Get
                    Return (m_Data.BasicLimitInformation.LimitFlags And JobObjectLimitFlags.JOB_OBJECT_LIMIT_BREAKAWAY_OK) = JobObjectLimitFlags.JOB_OBJECT_LIMIT_BREAKAWAY_OK
                End Get

                Set(ByVal Value As Boolean)
                    If Value Then
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags Or JobObjectLimitFlags.JOB_OBJECT_LIMIT_BREAKAWAY_OK
                    Else
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags And Not JobObjectLimitFlags.JOB_OBJECT_LIMIT_BREAKAWAY_OK
                    End If
                End Set
            End Property

            Public Property DieOnUnhandledException() As Boolean
                Get
                    Return (m_Data.BasicLimitInformation.LimitFlags And JobObjectLimitFlags.JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION) = JobObjectLimitFlags.JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION
                End Get

                Set(ByVal Value As Boolean)
                    If Value Then
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags Or JobObjectLimitFlags.JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION
                    Else
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags And Not JobObjectLimitFlags.JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION
                    End If
                End Set
            End Property

            Public Property KillOnJobClose() As Boolean
                Get
                    Return (m_Data.BasicLimitInformation.LimitFlags And JobObjectLimitFlags.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE) = JobObjectLimitFlags.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
                End Get

                Set(ByVal Value As Boolean)
                    If Value Then
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags Or JobObjectLimitFlags.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
                    Else
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags And Not JobObjectLimitFlags.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
                    End If
                End Set
            End Property

            Public Property SilentBreakawayOk() As Boolean
                Get
                    Return (m_Data.BasicLimitInformation.LimitFlags And JobObjectLimitFlags.JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK) = JobObjectLimitFlags.JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK
                End Get

                Set(ByVal Value As Boolean)
                    If Value Then
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags Or JobObjectLimitFlags.JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK
                    Else
                        m_Data.BasicLimitInformation.LimitFlags = m_Data.BasicLimitInformation.LimitFlags And Not JobObjectLimitFlags.JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK
                    End If
                End Set
            End Property

            Public ReadOnly Property PeakProcessMemoryUsed() As Int64
                Get
                    Return m_Data.PeakProcessMemoryUsed.ToInt64()
                End Get
            End Property

            Public ReadOnly Property PeakJobMemoryUsed() As Int64
                Get
                    Return m_Data.PeakJobMemoryUsed.ToInt64()
                End Get
            End Property
        End Class

        Public Class JobUIRestrictions
            Protected m_Data As JOBOBJECT_BASIC_UI_RESTRICTIONS
            Protected m_JobObject As JobObject

            Public Sub New(ByVal JobObject As JobObject)
                m_JobObject = JobObject
                Update()
            End Sub

            Public Sub Commit()
                Win32True(SetInformationJobObject(m_JobObject.GetHandleUnsafe(), JobObjectInfoClass.JobObjectBasicUIRestrictions, m_Data, Marshal.SizeOf(m_Data)))
            End Sub

            Public Sub Update()
                Win32True(QueryInformationJobObject(m_JobObject.GetHandleUnsafe(), JobObjectInfoClass.JobObjectBasicUIRestrictions, m_Data, Marshal.SizeOf(m_Data), Nothing))
            End Sub

            Public Property [Handles]() As Boolean
                Get
                    Return (m_Data.UIRestrictionClass And UIRestrictionClass.JOB_OBJECT_UILIMIT_HANDLES) = UIRestrictionClass.JOB_OBJECT_UILIMIT_HANDLES
                End Get

                Set(ByVal Value As Boolean)
                    If Value Then
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass Or UIRestrictionClass.JOB_OBJECT_UILIMIT_HANDLES
                    Else
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass And Not UIRestrictionClass.JOB_OBJECT_UILIMIT_HANDLES
                    End If
                End Set
            End Property

            Public Property ReadClipboard() As Boolean
                Get
                    Return (m_Data.UIRestrictionClass And UIRestrictionClass.JOB_OBJECT_UILIMIT_READCLIPBOARD) = UIRestrictionClass.JOB_OBJECT_UILIMIT_READCLIPBOARD
                End Get

                Set(ByVal Value As Boolean)
                    If Value Then
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass Or UIRestrictionClass.JOB_OBJECT_UILIMIT_READCLIPBOARD
                    Else
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass And Not UIRestrictionClass.JOB_OBJECT_UILIMIT_READCLIPBOARD
                    End If
                End Set
            End Property

            Public Property WriteClipboard() As Boolean
                Get
                    Return (m_Data.UIRestrictionClass And UIRestrictionClass.JOB_OBJECT_UILIMIT_WRITECLIPBOARD) = UIRestrictionClass.JOB_OBJECT_UILIMIT_WRITECLIPBOARD
                End Get

                Set(ByVal Value As Boolean)
                    If Value Then
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass Or UIRestrictionClass.JOB_OBJECT_UILIMIT_WRITECLIPBOARD
                    Else
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass And Not UIRestrictionClass.JOB_OBJECT_UILIMIT_WRITECLIPBOARD
                    End If
                End Set
            End Property

            Public Property SystemParameters() As Boolean
                Get
                    Return (m_Data.UIRestrictionClass And UIRestrictionClass.JOB_OBJECT_UILIMIT_SYSTEMPARAMETERS) = UIRestrictionClass.JOB_OBJECT_UILIMIT_SYSTEMPARAMETERS
                End Get

                Set(ByVal Value As Boolean)
                    If Value Then
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass Or UIRestrictionClass.JOB_OBJECT_UILIMIT_SYSTEMPARAMETERS
                    Else
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass And Not UIRestrictionClass.JOB_OBJECT_UILIMIT_SYSTEMPARAMETERS
                    End If
                End Set
            End Property

            Public Property DisplaySettings() As Boolean
                Get
                    Return (m_Data.UIRestrictionClass And UIRestrictionClass.JOB_OBJECT_UILIMIT_DISPLAYSETTINGS) = UIRestrictionClass.JOB_OBJECT_UILIMIT_DISPLAYSETTINGS
                End Get

                Set(ByVal Value As Boolean)
                    If Value Then
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass Or UIRestrictionClass.JOB_OBJECT_UILIMIT_DISPLAYSETTINGS
                    Else
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass And Not UIRestrictionClass.JOB_OBJECT_UILIMIT_DISPLAYSETTINGS
                    End If
                End Set
            End Property

            Public Property GlobalAtoms() As Boolean
                Get
                    Return (m_Data.UIRestrictionClass And UIRestrictionClass.JOB_OBJECT_UILIMIT_GLOBALATOMS) = UIRestrictionClass.JOB_OBJECT_UILIMIT_GLOBALATOMS
                End Get

                Set(ByVal Value As Boolean)
                    If Value Then
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass Or UIRestrictionClass.JOB_OBJECT_UILIMIT_GLOBALATOMS
                    Else
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass And Not UIRestrictionClass.JOB_OBJECT_UILIMIT_GLOBALATOMS
                    End If
                End Set
            End Property

            Public Property Desktop() As Boolean
                Get
                    Return (m_Data.UIRestrictionClass And UIRestrictionClass.JOB_OBJECT_UILIMIT_DESKTOP) = UIRestrictionClass.JOB_OBJECT_UILIMIT_DESKTOP
                End Get

                Set(ByVal Value As Boolean)
                    If Value Then
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass Or UIRestrictionClass.JOB_OBJECT_UILIMIT_DESKTOP
                    Else
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass And Not UIRestrictionClass.JOB_OBJECT_UILIMIT_DESKTOP
                    End If
                End Set
            End Property

            Public Property ExitWindows() As Boolean
                Get
                    Return (m_Data.UIRestrictionClass And UIRestrictionClass.JOB_OBJECT_UILIMIT_EXITWINDOWS) = UIRestrictionClass.JOB_OBJECT_UILIMIT_EXITWINDOWS
                End Get

                Set(ByVal Value As Boolean)
                    If Value Then
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass Or UIRestrictionClass.JOB_OBJECT_UILIMIT_EXITWINDOWS
                    Else
                        m_Data.UIRestrictionClass = m_Data.UIRestrictionClass And Not UIRestrictionClass.JOB_OBJECT_UILIMIT_EXITWINDOWS
                    End If
                End Set
            End Property
        End Class

        Public Sub New()
            Dim JobHandle As IntPtr = CreateJobObject(Nothing, Nothing)
            Win32True(JobHandle <> 0)
            MyBase.InternalSetHandle(JobHandle)
        End Sub

        Public ReadOnly Property Limits() As JobLimits
            Get
                Return New JobLimits(Me)
            End Get
        End Property

        Public ReadOnly Property UIRestrictions() As JobUIRestrictions
            Get
                Return New JobUIRestrictions(Me)
            End Get
        End Property

        Public Sub Assign(ByVal ProcessHandleNotOwned As IntPtr)
            Win32True(AssignProcessToJobObject(MyBase.GetHandleUnsafe(), ProcessHandleNotOwned))
        End Sub

        Public Sub Kill(ByVal ExitCode As Int32)
            Win32True(TerminateJobObject(MyBase.GetHandleUnsafe(), ExitCode))
        End Sub
    End Class
End Namespace