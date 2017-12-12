<Serializable()> Public Class PrintPage
    Implements IComparable(Of PrintPage)

    Public Title As String = ""
    Public Text As String = ""
    Public PageID As String = ""

    'standard as 100dpi

    'PageInfo is defined as mm.
    Public PageWidth As Single = 210
    Public PageHeight As Single = 297
    Public Property LeftSpace As Single = 10
    Public Property RightSpace As Single = 10
    Public Property TopSpace As Single = 14.14
    Public Property BottomSpace As Single = 14.14

    Public DPI As Integer = 300

    Public Property Left As Single

    Public Property Top As Single

    Public Property Width As Single

    Public Property Height As Single

    Private vTitleFont As New Font("Arial", 20, FontStyle.Bold)
    Private vTextFont As New Font("Arial", 12)
    Private vIDFont As New Font("Arial", 12, FontStyle.Bold)
    Public Function IsVisible(ci As ChartItem) As Boolean
        Dim PW As Single = PageWidth / Inch * DPI
        Dim sclf As Single = Width / PW
        Dim UW As Single = (PageWidth - LeftSpace - RightSpace) / Inch * DPI * sclf
        Dim UH As Single = (PageHeight - TopSpace - BottomSpace) / Inch * DPI * sclf

        Dim LS As Single = LeftSpace / Inch * DPI * sclf
        Dim TS As Single = TopSpace / Inch * DPI * sclf
        Return ci.Left > LS + Left AndAlso ci.Right < LS + UW + Left AndAlso ci.Top > TS + Top AndAlso ci.Bottom < TS + UH + Top
    End Function
    Public Sub Draw(ByVal g As Graphics, ByVal scale As Single, Optional Bounds As Boolean = True)
        Dim sfcenter As New System.Drawing.StringFormat()
        sfcenter.Alignment = StringAlignment.Center
        Dim tsf As System.Drawing.Drawing2D.Matrix = g.Transform
        g.TranslateTransform(Left, Top)
        If Bounds Then g.DrawRectangle(Pens.Black, New Rectangle(0, 0, Width, Height))

        Dim PW As Single = PageWidth / Inch * DPI
            Dim PH As Single = PageHeight / Inch * DPI
            Dim sclf As Single = Width / PW

            Dim UW As Single = (PageWidth - LeftSpace - RightSpace) / Inch * DPI * sclf
            Dim UH As Single = (PageHeight - TopSpace - BottomSpace) / Inch * DPI * sclf

            Dim LS As Single = LeftSpace / Inch * DPI * sclf
            Dim TS As Single = TopSpace / Inch * DPI * sclf
            Dim BS As Single = BottomSpace / Inch * DPI * sclf

            Dim KW As Single = (PageWidth - LeftSpace - RightSpace) / Inch * DPI

            If Bounds Then
                g.DrawRectangle(Pens.DarkGreen, New Rectangle(LS, TS, UW, UH))
                g.FillRectangle(Brushes.Red, New RectangleF(Width - 5 / scale, Height - 5 / scale, 5 / scale, 5 / scale))
            End If
            Dim ctsf As System.Drawing.Drawing2D.Matrix = g.Transform

            Dim szTitle As SizeF = g.MeasureString(Title, vTitleFont, KW, sfcenter)
            Dim szText As SizeF = g.MeasureString(Text, vTextFont, KW)
            Dim szID As SizeF = g.MeasureString(PageID, vIDFont, KW, sfcenter)

            g.TranslateTransform(LS, TS)
            g.ScaleTransform(sclf, sclf)
            g.DrawString(Title, vTitleFont, Brushes.Black, New RectangleF(0, 0, KW, szTitle.Height), sfcenter)
            g.Transform = ctsf
            g.TranslateTransform(LS, Height - BS - szText.Height * sclf - szID.Height * sclf)
            g.ScaleTransform(sclf, sclf)
            g.DrawString(Text, vTextFont, Brushes.Black, New RectangleF(0, 0, KW, szText.Height))
            g.Transform = ctsf
            g.TranslateTransform(LS, Height - BS - szID.Height * sclf)
            g.ScaleTransform(sclf, sclf)
            g.DrawString(PageID, vIDFont, Brushes.Black, New RectangleF(0, 0, KW, szID.Height), sfcenter)
            g.Transform = tsf
    End Sub
    Public ReadOnly Property PrintPixelWidth As Single
        Get
            Return (PageWidth - LeftSpace - RightSpace) / Inch * 300
        End Get
    End Property
    Public ReadOnly Property PrintPixelHeight As Single
        Get
            Return (PageHeight - TopSpace - BottomSpace) / Inch * 300
        End Get
    End Property
    Public Function Hittest(ByVal vP As PointF) As Boolean
        Return vP.X > Left And vP.X < Left + Width And vP.Y > Top And vP.Y < Top + Height
    End Function

    Public Function HitControlPoint(ByVal vP As PointF, ByVal scale As Single) As Boolean
        Return vP.X > _Left + _Width - 5 / scale And vP.X < _Left + _Width And vP.Y > _Top + _Height - 5 / scale And vP.Y < _Top + _Height
    End Function

    Dim sX As Single
    Dim sY As Single
    Dim sR As Single
    Dim sB As Single

    Public Sub StartMove()
        sX = Left
        sY = Top
    End Sub
    Dim rX As Single
    Dim rY As Single
    Public Sub StartResize()
        rX = _Width
        rY = _Height
    End Sub
    Public Sub MoveBy(ByVal dvX As Single, ByVal dvY As Single)
        Left = sX + dvX
        Top = sY + dvY
        RaiseEvent Moved(Me, New EventArgs)
    End Sub
    Public Sub ResizeBy(drX As Single, drY As Single)
        Dim vW As Single = rX + drX
        Dim vH As Single = rY + drY
        If vW < 1 Then vW = 1
        If vH < 1 Then vH = 1
        If vW / PageWidth < vH / PageHeight Then
            _Width = vW
            _Height = vW / PageWidth * PageHeight
        Else
            _Height = vH
            _Width = vH / PageHeight * PageWidth
        End If
        RaiseEvent Resized(Me, New EventArgs)
    End Sub
    Public Event Moved(ByVal sender As Object, ByVal e As EventArgs)
    Public Event Resized(sender As Object, e As EventArgs)

    Public Function CompareTo(other As PrintPage) As Integer Implements System.IComparable(Of PrintPage).CompareTo
        Return PageID.CompareTo(other.PageID)
    End Function
End Class