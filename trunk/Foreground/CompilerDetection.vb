Namespace Foreground
    Friend Module CompilerDetection
        Private Function FindFile(ByVal SearchList As IEnumerable(Of String), ByVal ExecutableName As String) As String
            For Each SearchPath As String In SearchList
                Try
                    Dim Directory As New DirectoryInfo(SearchPath)
                    If Not Directory.Exists Then Continue For
                    Dim Files As FileInfo() = Directory.GetFiles(ExecutableName, SearchOption.TopDirectoryOnly)
                    If Files.Length <> 0 Then _
                        Return Files(0).FullName
                Catch ex As Exception
                    ' Do nothing
                End Try
            Next
            Return Nothing
        End Function

        Private Function FindDirectory(ByVal SearchList As IEnumerable(Of String)) As String
            For Each SearchPath As String In SearchList
                Try
                    Dim Directory As New DirectoryInfo(SearchPath)
                    If Directory.Exists Then
                        Return Directory.FullName
                    End If
                Catch ex As Exception
                    ' Do nothing
                End Try
            Next
            Return Nothing
        End Function

        Private Function ShowOpen(ByVal Title As String, ByVal Filter As String) As String
            Using Dialog As New OpenFileDialog()
                Dialog.Title = Title
                Dialog.Filter = Filter
                If Dialog.ShowDialog() = DialogResult.OK Then
                    Return Dialog.FileName
                Else
                    Return Nothing
                End If
            End Using
        End Function

        Private Function DetectCompiler(ByVal SearchList As IEnumerable(Of String), ByVal ExecutableName As String) As String
            Dim Result As String = FindFile(SearchList, ExecutableName)
            If Result IsNot Nothing AndAlso _
                MessageBox.Show("编译器路径: " & Result & vbCrLf & vbCrLf & "此信息是否正确?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then _
                Result = Nothing
            If Result Is Nothing Then _
                Result = ShowOpen("查找 " & ExecutableName, ExecutableName & "|" & ExecutableName & "|所有文件 (*.*)|*.*")
            Return Result
        End Function

        Public Function DetectMingw(ByVal ExecutableName As String) As String
            Dim SearchList As New List(Of String)
            SearchList.AddRange(Environment.GetEnvironmentVariable("path").Split(New Char() {";"c}))
            SearchList.Add("C:\MinGW")
            SearchList.Add("C:\MinGW64")
            Return DetectCompiler(SearchList, ExecutableName)
        End Function

        Public Function DetectMssdk() As String
            Dim ProgramFilesPath As String = Environment.GetEnvironmentVariable("ProgramFiles(x86)")
            If ProgramFilesPath Is Nothing Then _
                ProgramFilesPath = Environment.GetEnvironmentVariable("ProgramFiles")
            Dim SearchList As New List(Of String)
            SearchList.Add(Path.Combine(ProgramFilesPath, "Microsoft SDKs\Windows\v7.0A"))
            SearchList.Add(Path.Combine(ProgramFilesPath, "Microsoft SDKs\Windows\v7.0"))
            SearchList.Add(Path.Combine(ProgramFilesPath, "Microsoft SDKs\Windows\v6.0A"))
            SearchList.Add(Path.Combine(ProgramFilesPath, "Microsoft SDKs\Windows\v6.0"))
            SearchList.Add(Path.Combine(ProgramFilesPath, "Microsoft SDKs\Windows\v5.0A"))
            SearchList.Add(Path.Combine(ProgramFilesPath, "Microsoft SDKs\Windows\v5.0"))
            Dim Result As String = FindDirectory(SearchList)
            If Result IsNot Nothing AndAlso _
                MessageBox.Show("SDK 路径: " & Result & vbCrLf & vbCrLf & "此信息是否正确?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then _
                Result = Nothing

            If Result Is Nothing Then
                Using Dialog As New FolderBrowserDialog()
                    Dialog.Description = "请选择 SDK 路径"
                    If Dialog.ShowDialog() = DialogResult.OK Then
                        Result = Dialog.SelectedPath
                    End If
                End Using
            End If
            Return Result
        End Function

        Public Function DetectMscl(ByRef Ide As String, ByRef Include As String, ByRef [Lib] As String) As String
            Dim ProgramFilesPath As String = Environment.GetEnvironmentVariable("ProgramFiles(x86)")
            If ProgramFilesPath Is Nothing Then _
                ProgramFilesPath = Environment.GetEnvironmentVariable("ProgramFiles")
            Dim SearchList As New List(Of String)
            SearchList.Add(Path.Combine(ProgramFilesPath, "Microsoft Visual Studio 10.0\VC\bin"))
            SearchList.Add(Path.Combine(ProgramFilesPath, "Microsoft Visual Studio 9.0\VC\bin"))
            SearchList.Add(Path.Combine(ProgramFilesPath, "Microsoft Visual Studio 8\VC\bin"))
            Dim Result As String = DetectCompiler(SearchList, "cl.exe")
            If Result Is Nothing Then Return Nothing
            Dim Binary As New FileInfo(Result)
            Dim Directory As DirectoryInfo = Binary.Directory
            While (Ide Is Nothing OrElse Include Is Nothing OrElse [Lib] Is Nothing) AndAlso Directory IsNot Nothing
                If Ide Is Nothing Then
                    Try
                        Dim Dirs As DirectoryInfo() = Directory.GetDirectories("Common7\IDE")
                        If Dirs.Length <> 0 Then Ide = Dirs(0).FullName
                    Catch ex As Exception
                    End Try
                End If
                If Include Is Nothing Then
                    Try
                        Dim Dirs As DirectoryInfo() = Directory.GetDirectories("include")
                        If Dirs.Length <> 0 Then Include = Dirs(0).FullName
                    Catch ex As Exception
                    End Try
                End If
                If [Lib] Is Nothing Then
                    Try
                        Dim Dirs As DirectoryInfo() = Directory.GetDirectories("lib")
                        If Dirs.Length <> 0 Then [Lib] = Dirs(0).FullName
                    Catch ex As Exception
                    End Try
                End If
                Directory = Directory.Parent
            End While
            Return Result
        End Function

        Public Function DetectNetfx(ByVal ExecutableName As String) As String
            Dim RuntimeDirectory As New DirectoryInfo(RuntimeEnvironment.GetRuntimeDirectory())
            Dim NetfxDirectory As DirectoryInfo = RuntimeDirectory.Parent
            Dim ExecutableDirectory As DirectoryInfo = Nothing
            Dim ExecutableFile As FileInfo = Nothing
            For Each Directory As DirectoryInfo In NetfxDirectory.GetDirectories("v*.*")
                If ExecutableDirectory Is Nothing OrElse Directory.Name > ExecutableDirectory.Name Then
                    Dim Files As FileInfo() = Directory.GetFiles(ExecutableName)
                    If Files.Length <> 0 Then
                        ExecutableDirectory = Directory
                        ExecutableFile = Files(0)
                    End If
                End If
            Next
            Dim Result As String
            If ExecutableFile IsNot Nothing Then
                Result = ExecutableFile.FullName
            Else
                Result = Nothing
            End If
            If Result IsNot Nothing AndAlso _
                MessageBox.Show("编译器路径: " & Result & vbCrLf & vbCrLf & "此信息是否正确?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then _
                Result = Nothing
            If Result Is Nothing Then _
                Result = ShowOpen("查找 " & ExecutableName, ExecutableName & "|" & ExecutableName & "|所有文件 (*.*)|*.*")
            Return Result
        End Function
    End Module
End Namespace
