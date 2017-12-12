Public Class Line

    Public Sub New()

    End Sub
    Public Y As Integer = 0
    Public Sstart As Integer
    Public Send As Integer
    Public GF As Nuctions.GeneFile
    Public l As Integer
    Public Edgewidth As Integer
    Public REList As New List(Of RE)

    '酶切位点占的行数
    Public ENZLevel As Integer

    '用来测试选种与否的工具
    Public Rect As Rectangle
    '
    Public FRect As Rectangle
    Public RRect As Rectangle
    Public SRect As Rectangle

    Public Sub Deploy(ByRef vY As Integer)
        Y = vY
        For Each f As VecLineFeature In Features
            f.Deploy(vY)
        Next

        '绘制酶切位点

        vY += CharWidth * 1.4 * ENZLevel
        '空一行给引物设计

        FRect = New Rectangle(Edgewidth + 1, vY, GetX(Send - Sstart + 1, Edgewidth) - Edgewidth - 1, CharWidth * 1.5)
        vY += CharWidth * 1.5

        SRect = New Rectangle(Edgewidth + 1, vY, GetX(LineCharCount + 1, Edgewidth) - Edgewidth - 1, CharWidth * 3)
        vY += CharWidth * 2.4

        '空一行给引物设计
        RRect = New Rectangle(Edgewidth + 1, vY, GetX(Send - Sstart + 1, Edgewidth) - Edgewidth - 1, CharWidth * 1.5)
        vY += CharWidth * 1.5

        vY += 2

        vY += 5

        Rect = New Rectangle(0, Y, GetX(Send - Sstart + 1, Edgewidth), vY - Y)
    End Sub

    Public Sub Draw(ByVal g As Graphics, ByVal Selection As SequenceRegion)
        Dim vY As Integer = Y
        For Each f As VecLineFeature In Features
            f.Draw(g, vY)
        Next

        '绘制酶切位点

        Dim LB As System.Drawing.Drawing2D.LinearGradientBrush


        For Each rr As RE In REList
            If rr.Separate Then
                LB = New System.Drawing.Drawing2D.LinearGradientBrush(New PointF(GetX((rr.P1.X - Sstart), Edgewidth), 0), New PointF(GetX((rr.P1.Y - Sstart), Edgewidth), 0), Color.Orange, Color.Yellow)

                g.FillRectangle(LB, New RectangleF(GetX((rr.P1.X - Sstart), Edgewidth), vY + (rr.L1 - 1) * CharWidth * 1.4, GetX((rr.P1.Y - Sstart), Edgewidth) - GetX((rr.P1.X - Sstart), Edgewidth), CharWidth * 1.4))
                g.DrawString(rr.REName, vFont, Brushes.Black, GetX((rr.P1.X Mod LineCharCount), Edgewidth), vY + (rr.L1 - 1) * CharWidth * 1.4)

                LB = New System.Drawing.Drawing2D.LinearGradientBrush(New PointF(GetX((rr.P2.X - Sstart), Edgewidth), 0), New PointF(GetX((rr.P2.X - Sstart), Edgewidth), 0), Color.Orange, Color.Yellow)

                g.FillRectangle(LB, New RectangleF(GetX((rr.P2.X - Sstart), Edgewidth), vY + (rr.L2 - 1) * CharWidth * 1.4, GetX((rr.P2.Y - Sstart), Edgewidth) - GetX((rr.P2.X - Sstart), Edgewidth), CharWidth * 1.4))
                g.DrawString(rr.REName, vFont, Brushes.Black, GetX((rr.P2.X - Sstart), Edgewidth), vY + (rr.L2 - 1) * CharWidth * 1.4)

            Else
                LB = New System.Drawing.Drawing2D.LinearGradientBrush(New PointF(GetX((rr.P1.X - Sstart), Edgewidth), 0), New PointF(GetX((rr.P1.Y - Sstart), Edgewidth), 0), Color.Orange, Color.Yellow)

                g.FillRectangle(LB, New RectangleF(GetX((rr.P1.X - Sstart), Edgewidth), vY + (rr.L1 - 1) * CharWidth * 1.4, GetX((rr.P1.Y - Sstart), Edgewidth) - GetX((rr.P1.X - Sstart), Edgewidth), CharWidth * 1.4))
                g.DrawString(rr.REName, vFont, Brushes.Black, GetX((rr.P1.X - Sstart), Edgewidth), vY + (rr.L1 - 1) * CharWidth * 1.4)

            End If
        Next


        vY += CharWidth * 1.4 * ENZLevel
        '空一行给引物设计
        'If vY > Top Or vY + CharWidth * 1.5 < Bottom Then
        '    FRect = New Rectangle(Edgewidth + 1, vY, GetX(Send - Sstart + 1, Edgewidth) - Edgewidth - 1, CharWidth * 1.5)
        'End If
        vY += CharWidth * 1.5

        Dim vSel As Boolean = False

        'SRect = New Rectangle(Edgewidth + 1, vY, GetX(LineCharCount + 1, Edgewidth) - Edgewidth - 1, CharWidth * 3)
        g.DrawString((Sstart - 1).ToString, vFont, Brushes.DarkBlue, 2, vY)
        For i As Integer = Sstart - 1 To Send - 1
            vSel = i And Selection
            g.DrawString(GF.Sequence.Chars(i), vFont, IIf(vSel, Brushes.Blue, Brushes.Black), (i Mod LineCharCount) * CharWidth * 0.8 + ((i Mod LineCharCount) \ CharGroup) * (CharWidth / 3) + Edgewidth, vY)
            g.DrawString(GF.RCSequence.Chars(l - 1 - i), vFont, IIf(vSel, Brushes.Blue, Brushes.Black), (i Mod LineCharCount) * CharWidth * 0.8 + ((i Mod LineCharCount) \ CharGroup) * (CharWidth / 3) + Edgewidth, vY + CharWidth * 1.2)
        Next

        vY += CharWidth * 2.4

        '空一行给引物设计
        'If vY > Top Or vY + CharWidth * 1.5 < Bottom Then
        '    RRect = New Rectangle(Edgewidth + 1, vY, GetX(Send - Sstart + 1, Edgewidth) - Edgewidth - 1, CharWidth * 1.5)
        'End If
        vY += CharWidth * 1.5

        vY += 2

        g.DrawLine(GrayPen, 0, vY, GetX(LineCharCount + 1, Edgewidth), vY)

        vY += 5

        'Rect = New Rectangle(0, Y, GetX(Send - Sstart + 1, Edgewidth), vY - Y)
    End Sub

    Public Sub DrawSelect(ByVal g As Graphics, ByVal vStart As Integer, ByVal vEnd As Integer)

    End Sub

    Public Sub DrawFPrimer(ByVal g As Graphics, ByVal index As Integer, ByVal Seq As String)
        For i As Integer = 0 To Seq.Length - 1
            g.DrawString(Seq.Chars(i), vFont, Brushes.Red, (i + index Mod LineCharCount) * CharWidth * 0.8 + ((i + index Mod LineCharCount) \ CharGroup) * (CharWidth / 3) + Edgewidth, FRect.Top)
        Next
    End Sub
    Public Sub DrawRPrimer(ByVal g As Graphics, ByVal index As Integer, ByVal Seq As String)
        For i As Integer = 0 To Seq.Length - 1
            g.DrawString(Seq.Chars(i), vFont, Brushes.Red, (index - i Mod LineCharCount) * CharWidth * 0.8 + ((index - i Mod LineCharCount) \ CharGroup) * (CharWidth / 3) + Edgewidth, RRect.Top)
        Next
    End Sub

    Public Sub ClearFPrimer(ByVal g As Graphics)
        g.FillRectangle(Brushes.White, FRect)

    End Sub
    Public Sub ClearRPrimer(ByVal g As Graphics)
        g.FillRectangle(Brushes.White, RRect)
    End Sub

    Public Features As New List(Of VecLineFeature)


    ''用于设计引物的工具
    'Public Sub DrawFPrimer()

    'End Sub

    'Public Sub DrawRPrimer()

    'End Sub


End Class

Public Class VecLineFeature


    '最重要的是上端的流位置
    Public Y As Integer = 0
    Public Feature As Nuctions.GeneAnnotation
    Public FPen As Pen

    '用来测试选种与否的工具
    Public RectList As New List(Of PointF)
    Public RectSection As New List(Of Rectangle)

    Public Sub Deploy(ByRef vY As Integer)
        Y = vY
        Dim Rect As Rectangle
        For Each rf As PointF In RectList
            Rect = New Rectangle(rf.X, Y, rf.Y - rf.X, CharWidth * 1.2)
            RectSection.Add(Rect)
        Next
        vY += CharWidth * 1.4
    End Sub
    Public Sub Draw(ByVal g As Graphics, ByRef vY As Integer)
        'Draw only when the veiw is between top and bottom.
        Y = vY


        Dim Rect As Rectangle
        For Each rf As PointF In RectList
            Rect = New Rectangle(rf.X, Y, rf.Y - rf.X, CharWidth * 1.2)
            g.DrawRectangle(FPen, Rect)
            'RectSection.Add(Rect)
            g.DrawString(Feature.Label + " (" + Feature.Note + ") ", vFont, Brushes.DarkRed, rf.X, Y)
        Next
        vY += CharWidth * 1.4
    End Sub

    Public Sub DrawSelect(ByVal g As Graphics)
        For Each rf As PointF In RectList
            g.FillRectangle(Brushes.Yellow, New Rectangle(rf.X, Y, rf.Y - rf.X, CharWidth * 1.2))
            g.DrawRectangle(FPen, New Rectangle(rf.X, Y, rf.Y - rf.X, CharWidth * 1.2))
            g.DrawString(Feature.Label + " (" + Feature.Note + ") ", vFont, Brushes.Red, rf.X, Y)
        Next
    End Sub
    Public Sub DrawDeSelect(ByVal g As Graphics)
        For Each rf As PointF In RectList
            g.FillRectangle(Brushes.White, New Rectangle(rf.X, Y, rf.Y - rf.X, CharWidth * 1.2))
            g.DrawRectangle(FPen, New Rectangle(rf.X, Y, rf.Y - rf.X, CharWidth * 1.2))
            g.DrawString(Feature.Label + " (" + Feature.Note + ") ", vFont, Brushes.Red, rf.X, Y)
        Next
    End Sub
End Class

