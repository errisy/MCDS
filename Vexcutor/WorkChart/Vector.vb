Public Structure Vector2
    Public X As Single
    Public Y As Single
    Public Sub New(ByVal vX As Single, ByVal vY As Single)
        X = vX
        Y = vY
    End Sub
    Public Sub New(ByVal vP As PointF)
        X = vP.X
        Y = vP.Y
    End Sub
    Public Sub New(ByVal vP As Point)
        X = vP.X
        Y = vP.Y
    End Sub
    Public Function GetBase() As Vector2
        If X = 0 And Y = 0 Then
            Return New Vector2(0, 0)
        Else
            Return Me / Math.Sqrt(X * X + Y * Y)
        End If
    End Function
    Public Function Turn(ByVal Rad As Single) As Vector2
        Dim vX As Single = X * Math.Cos(Rad) - Y * Math.Sin(Rad)
        Dim vY As Single = X * Math.Sin(Rad) + Y * Math.Cos(Rad)
        Return New Vector2(vX, vY)
    End Function
    Public Function GetLength() As Single
        Return Math.Sqrt(X * X + Y * Y)
    End Function
    Public Function GetRectEdge(ByVal vSizeF As SizeF) As PointF()
        Return New PointF() {New PointF(X, Y), New PointF(X + vSizeF.Width, Y), New PointF(X + vSizeF.Width, Y + vSizeF.Height), New PointF(X, Y + vSizeF.Height)}
    End Function
    Public Function GetRoundRectPath(ByVal vSizeF As SizeF, Optional ByVal RoundSize As Single = 4) As System.Drawing.Drawing2D.GraphicsPath
        Dim gp As New System.Drawing.Drawing2D.GraphicsPath
        Dim wRound As Single = IIf(vSizeF.Width / 4 > RoundSize, RoundSize, vSizeF.Width / 4)
        Dim hRound As Single = IIf(vSizeF.Height / 4 > RoundSize, RoundSize, vSizeF.Height / 4)
        'gp.AddLine(X + wRound, Y, X + vSizeF.Width - wRound, Y)
        gp.AddBezier(New PointF(X + vSizeF.Width - wRound, Y), New PointF(X + vSizeF.Width - wRound / 2, Y), New PointF(X + vSizeF.Width, Y + hRound / 2), New PointF(X + vSizeF.Width, Y + hRound))
        gp.AddLine(X + vSizeF.Width, Y + hRound, X + vSizeF.Width, Y + vSizeF.Height - hRound)
        gp.AddBezier(New PointF(X + vSizeF.Width, Y + vSizeF.Height - hRound), New PointF(X + vSizeF.Width, Y + vSizeF.Height - hRound / 2), New PointF(X + vSizeF.Width - wRound / 2, Y + vSizeF.Height), New PointF(X + vSizeF.Width - wRound, Y + vSizeF.Height))
        gp.AddLine(X + vSizeF.Width - wRound, Y + vSizeF.Height, X + wRound, Y + vSizeF.Height)
        gp.AddBezier(New PointF(X + wRound, Y + vSizeF.Height), New PointF(X + wRound / 2, Y + vSizeF.Height), New PointF(X, Y + vSizeF.Height - hRound / 2), New PointF(X, Y + vSizeF.Height - hRound))
        gp.AddLine(X, Y + vSizeF.Height - hRound, X, Y + hRound)
        gp.AddBezier(New PointF(X, Y + hRound), New PointF(X, Y + hRound / 2), New PointF(X + wRound / 2, Y), New PointF(X + wRound, Y))
        gp.CloseFigure()
        Return gp
    End Function
    Public Shared Widening Operator CType(ByVal vPoint As PointF) As Vector2
        Return New Vector2(vPoint.X, vPoint.Y)
    End Operator
    Public Shared Widening Operator CType(ByVal vPoint As Point) As Vector2
        Return New Vector2(vPoint.X, vPoint.Y)
    End Operator
    Public Shared Narrowing Operator CType(ByVal vVector As Vector2) As PointF
        Return New PointF(vVector.X, vVector.Y)
    End Operator
    Public Shared Narrowing Operator CType(ByVal vVector As Vector2) As Point
        Return New Point(vVector.X, vVector.Y)
    End Operator
    Public Shared Operator -(ByVal vVector1 As Vector2, ByVal vVector2 As Vector2) As Vector2
        Return New Vector2(vVector1.X - vVector2.X, vVector1.Y - vVector2.Y)
    End Operator
    Public Shared Operator -(ByVal vVector2 As Vector2) As Vector2
        Return New Vector2(-vVector2.X, -vVector2.Y)
    End Operator
    Public Shared Operator +(ByVal vVector1 As Vector2, ByVal vVector2 As Vector2) As Vector2
        Return New Vector2(vVector1.X + vVector2.X, vVector1.Y + vVector2.Y)
    End Operator
    Public Shared Operator *(ByVal vVector1 As Vector2, ByVal vValue As Single) As Vector2
        Return New Vector2(vVector1.X * vValue, vVector1.Y * vValue)
    End Operator
    Public Shared Operator /(ByVal vVector1 As Vector2, ByVal vValue As Single) As Vector2
        Return New Vector2(vVector1.X / vValue, vVector1.Y / vValue)
    End Operator
End Structure

'Public Interface TabContentControl
'    Event OnCloseTab(ByVal sender As Object, ByVal e As EventArgs)
'End Interface