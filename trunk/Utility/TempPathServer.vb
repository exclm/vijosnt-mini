Namespace Utility
    Friend Class TempPathServer
        Implements IDisposable

        Protected Const m_SleepTime As Int32 = 30 * 1000
        Protected Const m_TempPathLength As Int32 = 16

        Protected m_Root As DirectoryInfo
        Protected m_Pendings As List(Of DirectoryInfo)
        Protected m_CleanupThread As Thread
        Protected m_RandomString As RandomString

        Public Sub New()
            Dim AppPath As New AppPath()
            m_Root = AppPath.GetDirectoryInfo().CreateSubdirectory("Temp")
            m_Pendings = New List(Of DirectoryInfo)(m_Root.GetDirectories())
            m_CleanupThread = New Thread(AddressOf CleanupThreadEntry)
            m_RandomString = New RandomString()
            m_CleanupThread.Priority = ThreadPriority.Lowest
            m_CleanupThread.IsBackground = True
            m_CleanupThread.Start(m_Pendings)
        End Sub

        Public Function CreateTempPath() As TempPath
            Dim Dir As DirectoryInfo
            SyncLock m_Root
                Dim Path As String = m_RandomString.Next(m_TempPathLength)
                Dir = m_Root.CreateSubdirectory(Path)
            End SyncLock
            Return New TempPath(Dir, Me)
        End Function

        Public Sub Free(ByVal Dir As DirectoryInfo)
            Try
                Dir.Delete(True)
            Catch ex As Exception
                Dir.Refresh()
                If Dir.Exists() Then
                    SyncLock m_Pendings
                        m_Pendings.Add(Dir)
                    End SyncLock
                End If
            End Try
        End Sub

        ' Should not use member function so that the class object will not be referenced.
        Protected Shared Sub CleanupThreadEntry(ByVal Pendings As IList(Of DirectoryInfo))
            While True
                Work(Pendings)
                Thread.Sleep(m_SleepTime)
            End While
        End Sub

        Protected Shared Sub Work(ByVal Pendings As IList(Of DirectoryInfo))
            SyncLock Pendings
                Dim Index As Int32 = 0
                While Index < Pendings.Count
                    Dim Dir As DirectoryInfo = Pendings(Index)
                    Try
                        Dir.Delete(True)
                    Catch ex As Exception
                        ' Do nothing
                    End Try
                    Dir.Refresh()
                    If Dir.Exists() Then
                        Index += 1
                    Else
                        Pendings(Index) = Pendings(Pendings.Count - 1)
                        Pendings.RemoveAt(Pendings.Count - 1)
                    End If
                End While
            End SyncLock
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean

        Protected Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
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
End Namespace