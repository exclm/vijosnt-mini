﻿Imports VijosNT.Win32

Namespace Executing
    Friend Class UntrustedEnvironment
        Inherits Environment
        Implements IDisposable

        Protected m_WindowStation As WindowStation
        Protected m_Desktop As Desktop
        Protected m_Token As Token

        Public Sub New(ByVal DesktopName As String, ByVal UserName As String, ByVal Password As String)
            m_WindowStation = New WindowStation()
            m_Desktop = New Desktop(DesktopName)
            m_Token = New Token(UserName, Password)

            Dim Sid As Byte() = m_Token.GetSid()
            m_WindowStation.AddAllowedAce(Sid, New UserObject.AllowedAce(0, UserObject.AceMask.GenericRead Or UserObject.AceMask.GenericWrite Or UserObject.AceMask.GenericExecute))
            m_Desktop.AddAllowedAce(Sid, New UserObject.AllowedAce(0, UserObject.AceMask.GenericRead Or UserObject.AceMask.GenericWrite Or UserObject.AceMask.GenericExecute))
        End Sub

        Public Overrides ReadOnly Property Tag() As EnvironmentTag
            Get
                Return EnvironmentTag.Untrusted
            End Get
        End Property

        Public Overrides ReadOnly Property DesktopName() As String
            Get
                Return m_WindowStation.GetName() & "\" & m_Desktop.GetName()
            End Get
        End Property

        Public Overrides ReadOnly Property Token() As Token
            Get
                Return m_Token
            End Get
        End Property

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                Dim Sid As Byte() = m_Token.GetSid()
                m_WindowStation.RemoveAceBySid(Sid)
                m_Desktop.RemoveAceBySid(Sid)
                If disposing Then
                    m_WindowStation.Dispose()
                    m_Desktop.Dispose()
                    m_Token.Close()
                End If
            End If
            Me.disposedValue = True
        End Sub

        Protected Overrides Sub Finalize()
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(False)
            MyBase.Finalize()
        End Sub
#End Region

    End Class
End Namespace
