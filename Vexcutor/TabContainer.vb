Public Class TabContainer
    Inherits TabControl
    Public Sub New()
        MyBase.SetStyle(ControlStyles.UserPaint, True)
        MyBase.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        MyBase.SetStyle(ControlStyles.DoubleBuffer, True)
        MyBase.SetStyle(ControlStyles.ResizeRedraw, True)
        MyBase.SetStyle(ControlStyles.SupportsTransparentBackColor, True)
    End Sub
    Public TabStyle As Integer
    Public ClearColor As Color = Color.White

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        If TabPages.Count > 0 Then
            'Dim TabRect As Rectangle = MyBase.GetTabRect(0)
            'e.Graphics.FillRectangle(Brushes.White, New RectangleF(0, 0, Width, TabRect.Height))
            e.Graphics.Clear(ClearColor)
        End If
        For i As Integer = 0 To MyBase.TabPages.Count - 1
            Dim TabRect As Rectangle = MyBase.GetTabRect(i)
            Dim gp As New System.Drawing.Drawing2D.GraphicsPath
            Dim VO As New Vector2(TabRect.X, TabRect.Y)
            Dim VH As New Vector2(0, TabRect.Height)
            Dim VW As New Vector2(TabRect.Width, 0)


            Dim gb As System.Drawing.Drawing2D.LinearGradientBrush

            Select Case TabStyle
                Case 0
                    gp.AddLines(New PointF() {VO + VH, VO + (VW.GetBase * VH.GetLength + VH) / 2})
                    gp.AddBezier(CType(VO + (VW.GetBase * VH.GetLength + VH) / 2, PointF), VO + VW.GetBase * VH.GetLength, VO + VW.GetBase * VH.GetLength, VO + VW.GetBase * VH.GetLength * 2)
                    gp.AddLines(New PointF() {VO + VW.GetBase * VH.GetLength * 2, VO + VW, VO + VW + VH})
                    gp.CloseFigure()
                Case 1
                    gp.AddLines(New PointF() {VO + VH, VO + VW.GetBase * VH.GetLength / 2, VO + VW - VW.GetBase * VH.GetLength / 2, VO + VW + VH})
                    'gp.AddBezier(CType(VO + (VW.GetBase * VH.GetLength + VH) / 2, PointF), VO + VW.GetBase * VH.GetLength, VO + VW.GetBase * VH.GetLength, VO + VW.GetBase * VH.GetLength * 2)
                    'gp.AddLines(New PointF() {VO + VW.GetBase * VH.GetLength * 2, VO + VW, VO + VW + VH})
                    gp.CloseFigure()
            End Select
            If TypeOf TabPages(i) Is CustomTabPage Then
                Dim CTP As CustomTabPage = TabPages(i)

                If SelectedIndex = i Then
                    gb = New System.Drawing.Drawing2D.LinearGradientBrush(CType(VO, PointF), CType(VO + VH, PointF), CTP.SelectedGrad1Color, CTP.SelectedGrad2Color)
                    e.Graphics.FillPath(gb, gp)
                    e.Graphics.DrawPath(New Pen(CTP.SelectBorderColor), gp)
                    e.Graphics.DrawString(TabPages(i).Text, Font, New SolidBrush(CTP.SelectedFontColor), TabRect.X + VH.GetLength * 0.75, TabRect.Y + VH.GetLength / 4)

                Else
                    gb = New System.Drawing.Drawing2D.LinearGradientBrush(CType(VO, PointF), CType(VO + VH, PointF), CTP.Grad1Color, CTP.Grad2Color)
                    e.Graphics.FillPath(gb, gp)
                    e.Graphics.DrawPath(New Pen(CTP.BorderColor), gp)
                    e.Graphics.DrawString(TabPages(i).Text, Font, New SolidBrush(CTP.FontColor), TabRect.X + VH.GetLength * 0.75, TabRect.Y + VH.GetLength / 4)
                End If
            Else
                If SelectedIndex = i Then
                    gb = New System.Drawing.Drawing2D.LinearGradientBrush(CType(VO, PointF), CType(VO + VH, PointF), TabPages(i).BackColor, TabPages(i).ForeColor)
                    e.Graphics.FillPath(gb, gp)
                    e.Graphics.DrawPath(Pens.Red, gp)
                    e.Graphics.DrawString(TabPages(i).Text, Font, Brushes.OrangeRed, TabRect.X + VH.GetLength * 0.75, TabRect.Y + VH.GetLength / 4)
                Else
                    Dim c1 As Color = Color.FromArgb(64, TabPages(i).BackColor.R, TabPages(i).BackColor.G, TabPages(i).BackColor.B)
                    Dim c2 As Color = Color.FromArgb(160, TabPages(i).ForeColor.R, TabPages(i).ForeColor.G, TabPages(i).ForeColor.B)
                    gb = New System.Drawing.Drawing2D.LinearGradientBrush(CType(VO, PointF), CType(VO + VH, PointF), c1, c2)
                    e.Graphics.FillPath(gb, gp)
                    e.Graphics.DrawPath(Pens.Red, gp)
                    e.Graphics.DrawString(TabPages(i).Text, Font, Brushes.LawnGreen, TabRect.X + VH.GetLength * 0.75, TabRect.Y + VH.GetLength / 4)
                End If
            End If


        Next
        MyBase.OnPaint(e)
    End Sub
    Protected Overrides Sub OnSelecting(ByVal e As System.Windows.Forms.TabControlCancelEventArgs)
        MyBase.OnSelecting(e)
        MyBase.Update()
    End Sub
    Private Sub DrawControl()

    End Sub
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
    End Sub

    '页面管理工具
    'Private cDict As New Dictionary(Of Control, CustomTabPage)
    'Private pDict As New Dictionary(Of Control, List(Of CustomTabPage))

    'Public Sub AddPageFrom(ByVal HostControl As Control, ByVal NewPageControl As Control)

    'End Sub

End Class

Public Class CustomTabPage
    Inherits TabPage
    Public SelectedGrad1Color As Color = Color.LightYellow
    Public SelectedGrad2Color As Color = Color.OrangeRed
    Public Grad1Color As Color = Color.Wheat
    Public Grad2Color As Color = Color.Gold
    Public SelectBorderColor As Color = Color.Red
    Public BorderColor As Color = Color.Purple
    Public SelectedFontColor As Color = Color.Black
    Public FontColor As Color = Color.Navy
    Public Sub SetCustomStyle(ByVal Style As CustomTabPageStyle)
        Select Case Style
            Case CustomTabPageStyle.Vector
                SelectedGrad1Color = Color.Blue
                SelectedGrad2Color = Color.Black
                Grad1Color = Color.DarkBlue
                Grad2Color = Color.Black
                SelectBorderColor = Color.Blue
                BorderColor = Color.Yellow
                SelectedFontColor = Color.Yellow
                FontColor = Color.Yellow
            Case CustomTabPageStyle.Project
                SelectedGrad1Color = Color.Red
                SelectedGrad2Color = Color.Black
                Grad1Color = Color.Brown
                Grad2Color = Color.Black
                SelectBorderColor = Color.Red
                BorderColor = Color.Yellow
                SelectedFontColor = Color.Cyan
                FontColor = Color.Cyan
            Case CustomTabPageStyle.Server
                SelectedGrad1Color = Color.Green
                SelectedGrad2Color = Color.Black
                Grad1Color = Color.DarkGreen
                Grad2Color = Color.Black
                SelectBorderColor = Color.Green
                BorderColor = Color.Yellow
                SelectedFontColor = Color.White
                FontColor = Color.White
            Case CustomTabPageStyle.TmViewer
                SelectedGrad1Color = Color.Chocolate
                SelectedGrad2Color = Color.Black
                Grad1Color = Color.SaddleBrown
                Grad2Color = Color.Black
                SelectBorderColor = Color.Chocolate
                BorderColor = Color.Yellow
                SelectedFontColor = Color.MintCream
                FontColor = Color.MintCream
            Case CustomTabPageStyle.CodonManager
                SelectedGrad1Color = Color.Fuchsia
                SelectedGrad2Color = Color.Black
                Grad1Color = Color.Purple
                Grad2Color = Color.Black
                SelectBorderColor = Color.Fuchsia
                BorderColor = Color.Yellow
                SelectedFontColor = Color.YellowGreen
                FontColor = Color.YellowGreen
            Case CustomTabPageStyle.BufferManager
                SelectedGrad1Color = Color.Coral
                SelectedGrad2Color = Color.Black
                Grad1Color = Color.Maroon
                Grad2Color = Color.Black
                SelectBorderColor = Color.Coral
                BorderColor = Color.Yellow
                SelectedFontColor = Color.Yellow
                FontColor = Color.Yellow
        End Select
    End Sub
End Class
Public Enum CustomTabPageStyle As Integer
    Vector
    Project
    Server
    TmViewer
    CodonManager
    BufferManager
End Enum
Public Class EditorTabpage
    Inherits CustomTabPage
    Protected Overrides Sub OnParentChanged(e As System.EventArgs)
        If Controls.Count > 0 Then
            If TypeOf Controls(0) Is ITabHostingControl Then
                DirectCast(Controls(0), ITabHostingControl).TabParentChanged(Parent)
            End If
        End If
    End Sub
End Class