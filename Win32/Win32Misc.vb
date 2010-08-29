Namespace Win32
    Friend Module Win32Misc
        Public Function GetIdleProcessTime() As Int64
            Dim SystemInformation As IntPtr = Marshal.AllocHGlobal(328)

            Try
                NtSuccess(NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemPerformanceInformation, SystemInformation, 328, Nothing))
                Return Marshal.ReadInt64(SystemInformation, 0)
            Finally
                Marshal.FreeHGlobal(SystemInformation)
            End Try
        End Function

        Public Function PromptForCredentials(ByVal ParentWindow As IntPtr, ByVal Message As String, ByVal Title As String, ByRef UserName As String, ByRef Password As String) As DialogResult
            Const MaxLength As Int32 = 512

            Dim CredUIInfo As New CREDUI_INFO
            CredUIInfo.cbSize = Marshal.SizeOf(CredUIInfo)
            CredUIInfo.hwndParent = ParentWindow
            CredUIInfo.MessageText = Message
            CredUIInfo.CaptionText = Title

            Dim UserNamePtr As IntPtr = Marshal.AllocHGlobal(MaxLength * Marshal.SystemDefaultCharSize)
            Try
                Marshal.WriteInt16(UserNamePtr, 0)
                Dim PasswordPtr As IntPtr = Marshal.AllocHGlobal(MaxLength * Marshal.SystemDefaultCharSize)
                Try
                    Marshal.WriteInt16(PasswordPtr, 0)
                    Dim LastErr As Int32 = CredUIPromptForCredentials(CredUIInfo, String.Empty, 0, 0, UserNamePtr, MaxLength, PasswordPtr, MaxLength, False, _
                        CredUIFlags.CREDUI_FLAGS_GENERIC_CREDENTIALS Or CredUIFlags.CREDUI_FLAGS_ALWAYS_SHOW_UI Or CredUIFlags.CREDUI_FLAGS_DO_NOT_PERSIST)
                    Select Case LastErr
                        Case 0
                            UserName = Marshal.PtrToStringAuto(UserNamePtr)
                            Password = Marshal.PtrToStringAuto(PasswordPtr)
                            Return DialogResult.OK
                        Case ERROR_CANCELLED
                            Return DialogResult.Cancel
                        Case Else
                            Throw New Win32Exception(LastErr)
                    End Select
                Finally
                    Marshal.FreeHGlobal(PasswordPtr)
                End Try
            Finally
                Marshal.FreeHGlobal(UserNamePtr)
            End Try
        End Function
    End Module
End Namespace
