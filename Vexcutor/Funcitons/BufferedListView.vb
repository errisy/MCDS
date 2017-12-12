Public Class BufferedListView
    Inherits BufferedPictureBox
    Private ListSource As IList
    Public Property DataSource As IList(Of String)
        Set(ByVal value As IList(Of String))
            ListSource = value
            CurrentViewStart = 0
            SelectIndex = -1
            ReDraw()
            RaiseEvent LineSelectByClickOrDataSource(Me, New EventArgs)
        End Set
        Get
            Return ListSource
        End Get
    End Property
 
    Public Property CurrentViewStart As Integer
    Private SelectIndex As Integer = -1
    Private vDoubleSelectIndex As Integer = -1
    Public Property DoubleSelectIndex As Integer
        Set(ByVal value As Integer)
            vDoubleSelectIndex = value
            'Draw
            ReDraw()
        End Set
        Get
            Return vDoubleSelectIndex
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("The index of the selected line."), System.ComponentModel.DefaultValue(-1)> Public Property Index As Integer
        Set(ByVal value As Integer)
            If ListSource Is Nothing Then Exit Property
            SelectIndex = value
            ReDraw()
            RaiseEvent SelectIndexChanged(Me, New EventArgs)
        End Set
        Get
            Return SelectIndex
        End Get
    End Property

    Private LineHeight As Single
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        ReDraw()
    End Sub
    Public Sub ReDraw()
        If ListSource Is Nothing Then Exit Sub
        Dim g As Graphics = BufferedGraphics
        g.Clear(Color.White)

        Dim HalfLineGap As Integer = 2
        Dim LeftGap As Integer = 3
        Dim Ori As New Vector2(LeftGap, 0)
        Dim myFont As New Font("Arial", 12)
        Dim TextRect As SizeF
        TextRect = g.MeasureString("TEST", myFont)
        LineHeight = HalfLineGap * 2 + TextRect.Height

        Dim i As Integer = CurrentViewStart
        Dim vStr As String


        While i < ListSource.Count And Ori.Y < Me.Height
            Ori.Y += HalfLineGap
            Try
                vStr = ListSource(i)
                If vStr Is Nothing Then vStr = ""
            Catch ex As Exception
                vStr = "<Error>"
            End Try
            TextRect = g.MeasureString(vStr, myFont)
            Dim vRect As System.Drawing.Drawing2D.GraphicsPath = Ori.GetRoundRectPath(TextRect, 2)
            g.FillPath(IIf(i = SelectIndex, Brushes.LightCyan, Brushes.Wheat), vRect)
            g.DrawPath(IIf(i = SelectIndex, Pens.YellowGreen, Pens.LightYellow), vRect)
            g.DrawString(vStr, myFont, IIf(i = vDoubleSelectIndex, Brushes.Blue, Brushes.Black), Ori)
            Ori.Y += LineHeight
            Ori.Y += HalfLineGap
            i += 1
        End While

        'Draw Scroll Bar
        If ListSource.Count > Height / LineHeight Then
            Dim viewNumber As Integer = Math.Floor(Height / LineHeight)
            g.FillRectangle(Brushes.Gray, New Rectangle(Width - 12, 0, 12, Height))
            Dim barSize As New SizeF(12, viewNumber / ListSource.Count * Height)
            Dim barPosition As New Vector2(Width - 12, CurrentViewStart / (ListSource.Count - viewNumber))
            Dim varRect As System.Drawing.Drawing2D.GraphicsPath = barPosition.GetRoundRectPath(barSize, 2)
            Dim innerPosition As Vector2 = barPosition + New Vector2(2, 2)
            Dim innerSize As New SizeF(barSize.Width - 4, barSize.Height - 4)
            Dim innerRect As System.Drawing.Drawing2D.GraphicsPath = innerPosition.GetRoundRectPath(innerSize, 2)

            Dim vGradientBrush As New System.Drawing.Drawing2D.LinearGradientBrush(New PointF(Width - 12, 0), New PointF(Width, 0), Color.LightCyan, Color.AliceBlue)
            g.FillPath(Brushes.White, varRect)
            g.DrawPath(Pens.LightCoral, varRect)
            g.FillPath(vGradientBrush, innerRect)
        End If
        Draw()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        'Select or Scroll
        If ListSource Is Nothing Then Exit Sub
        If e.X >= Width - 12 Then
            'Scroll
            CurrentViewStart = e.Y / Height * (1 - Math.Floor(Height / LineHeight) / ListSource.Count)
            If CurrentViewStart > ListSource.Count - 1 Then CurrentViewStart = ListSource.Count - 1
            If CurrentViewStart < 0 Then CurrentViewStart = 0
            ReDraw()
        ElseIf LineHeight > 0.0F Then
            'Select

            Dim i As Integer = CurrentViewStart
            Dim y As Single = e.Y
            While y > LineHeight
                y -= LineHeight
                i += 1
            End While
            If i > ListSource.Count - 1 Then i = ListSource.Count - 1
            If i < -1 Then i = -1
            SelectIndex = i
            ReDraw()
            RaiseEvent LineSelectByClickOrDataSource(Me, New EventArgs)
            RaiseEvent SelectIndexChanged(Me, New EventArgs)
        End If
        MyBase.OnMouseDown(e)
    End Sub
    Protected Overrides Sub OnMouseDoubleClick(ByVal e As System.Windows.Forms.MouseEventArgs)
        If ListSource Is Nothing Then Exit Sub
        If e.X >= Width - 12 Then
            'Scroll
            CurrentViewStart = e.Y / Height * (1 - Math.Floor(Height / LineHeight) / ListSource.Count)
            If CurrentViewStart > ListSource.Count - 1 Then CurrentViewStart = ListSource.Count - 1
            If CurrentViewStart < 0 Then CurrentViewStart = 0
            ReDraw()
        ElseIf LineHeight > 0.0F Then
            'Select

            Dim i As Integer = CurrentViewStart
            Dim y As Single = e.Y
            While y > LineHeight
                y -= LineHeight
                i += 1
            End While
            If i > ListSource.Count - 1 Then i = ListSource.Count - 1
            If i < 0 Then i = 0
            vDoubleSelectIndex = i
            ReDraw()
            RaiseEvent LineDoubleSelect(Me, New EventArgs)
            RaiseEvent SelectIndexChanged(Me, New EventArgs)
        End If
        MyBase.OnMouseDoubleClick(e)
    End Sub
    Protected Overrides Sub OnMouseWheel(ByVal e As System.Windows.Forms.MouseEventArgs)
        If ListSource Is Nothing Then Exit Sub
        CurrentViewStart += e.Delta / 120
        If CurrentViewStart > ListSource.Count - 1 Then CurrentViewStart = ListSource.Count - 1
        If CurrentViewStart < 0 Then CurrentViewStart = 0
        MyBase.OnMouseWheel(e)
    End Sub
    Public Event SelectIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
    Public Event LineSelectByClickOrDataSource(ByVal sender As Object, ByVal e As EventArgs)
    Public Event LineDoubleSelect(ByVal sender As Object, ByVal e As EventArgs)
End Class
