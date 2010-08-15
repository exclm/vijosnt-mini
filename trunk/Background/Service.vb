Imports VijosNT.LocalDb

Namespace Background
    Friend Class Service
        Implements IDisposable

        Private m_Runner As Runner

        Public Sub New()
            m_Runner = New Runner()
        End Sub

        Public Sub Entry()
            WriteStartupLog()
            Using ms As New MemoryStream()
                Using sr As New StreamWriter(ms)
                    sr.WriteLine("#include <stdio.h>")
                    sr.WriteLine()
                    sr.WriteLine("int main(void)")
                    sr.WriteLine("{")
                    sr.WriteLine("    int a, b;")
                    sr.WriteLine("    scanf(""%d%d"", &a, &b);")
                    sr.WriteLine("    printf(""%d\n"", a + b);")
                    sr.WriteLine("    return 0;")
                    sr.WriteLine("}")
                End Using

                Console.WriteLine("Enter a number to perform test")
                Console.WriteLine("Type other things to exit")

                Do
                    Console.Write("> ")
                    Dim Number As Int32
                    Try
                        Number = Int32.Parse(Console.ReadLine())
                    Catch ex As Exception
                        Exit Do
                    End Try

                    For Index As Int32 = 0 To Number - 1
                        m_Runner.Queue("gcc", New MemoryStream(ms.ToArray()), "A+B", _
                            Sub(Result As TestResult)
                                Console.WriteLine("{0}, {1}, {2}ms, {3}kb", Result.Flag, Result.Score, Result.TimeUsage \ 10000, Result.MemoryUsage \ 1024)
                            End Sub, Nothing)
                    Next
                Loop
            End Using
        End Sub

        Private Sub WriteStartupLog()
            Dim Builder As New StringBuilder()
            Builder.AppendLine("数据库引擎: " & Database.EngineVersion)
            Log.Add(LogLevel.Information, "VijosNT 启动成功", Builder.ToString())
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    m_Runner.Dispose()
                End If
            End If
            Me.disposedValue = True
        End Sub

        ' Visual Basic 添加此代码是为了正确实现可处置模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace
