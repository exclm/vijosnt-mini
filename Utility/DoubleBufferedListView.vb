Namespace Utility
    Friend Class DoubleBufferedListView
        Inherits ListView

        Public Sub New()
            MyBase.New()
            DoubleBuffered = True
        End Sub
    End Class
End Namespace
