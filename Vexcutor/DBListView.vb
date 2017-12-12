Public Class DBListView
    Inherits ListView
    Public Sub New()
        'MyBase.SetStyle(ControlStyles.UserPaint, True)
        MyBase.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        MyBase.SetStyle(ControlStyles.DoubleBuffer, True)
        MyBase.SetStyle(ControlStyles.ResizeRedraw, True)
    End Sub

    'Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
    '    RaiseEvent Paint(Me, e)
    '    MyBase.OnPaint(e)
    'End Sub
    'Public Shadows Event Paint(ByVal sender As Object, ByVal e As PaintEventArgs)

End Class
