Public NotInheritable Class TempPathServer
    Implements IDisposable

    Private Const m_SleepTime As Int32 = 30 * 1000
    Private Const m_TempPathLength As Int32 = 16

    Private m_Root As DirectoryInfo
    Private m_Pendings As List(Of DirectoryInfo)
    Private m_CleanupThread As Thread
    Private m_RandomName As RandomName

    Public Sub New()
        Dim AppPath As New AppPath()
        m_Root = AppPath.GetDirectoryInfo().CreateSubdirectory("Temp")
        m_Pendings = New List(Of DirectoryInfo)
        m_CleanupThread = New Thread(AddressOf CleanupThreadEntry)
        m_RandomName = New RandomName()
        m_CleanupThread.Priority = ThreadPriority.Lowest
        m_CleanupThread.Start(m_Pendings)
    End Sub

    Public Function CreateTempPath() As TempPath
        SyncLock m_Root
            Dim Path As String = m_RandomName.Next(m_TempPathLength)
            Dim Dir As DirectoryInfo = m_Root.CreateSubdirectory(Path)
            Return New TempPath(Dir, Me)
        End SyncLock
    End Function

    Public Sub Free(ByVal Dir As DirectoryInfo)
        Try
            Dir.Delete(True)
        Catch ex As IOException
            Dir.Refresh()
            If Dir.Exists() Then
                SyncLock m_Pendings
                    m_Pendings.Add(Dir)
                End SyncLock
            End If
        End Try
    End Sub

    ' Should not use member function so that the class object will not be referenced.
    Private Shared Sub CleanupThreadEntry(ByVal Pendings As IList(Of DirectoryInfo))
        Try
            While True
                Thread.Sleep(m_SleepTime)
                Work(Pendings)
            End While
        Catch ex As ThreadAbortException
        End Try
    End Sub

    Private Shared Sub Work(ByVal Pendings As IList(Of DirectoryInfo))
        SyncLock Pendings
            For Each Dir As DirectoryInfo In Pendings
                Try
                    Dir.Delete(True)
                Catch ex As IOException
                End Try
                Dir.Refresh()
                If Not Dir.Exists() Then
                    Pendings.Remove(Dir)
                End If
            Next
        End SyncLock
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean

    Protected Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            m_CleanupThread.Abort()
            Work(m_Pendings)
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
