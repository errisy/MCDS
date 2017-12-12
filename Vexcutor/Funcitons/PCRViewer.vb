Public Class SequenceViewer

    Private context As BufferedGraphicsContext '双缓冲buffer管理
    Private grafx As BufferedGraphics '双缓冲的buffer
    Private grfrec As TypeRecorder(Of Graphics)  '记录所有图片绘制过程用来在重绘时快速描绘
    Private myGraphics As Graphics '该空间的DC

    '°C

    Public Sub New()
        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        context = BufferedGraphicsManager.Current
        context.MaximumBuffer = New Size(pbSeq.Width + 1, pbSeq.Height + 1)
        grafx = context.Allocate(CreateGraphics(), New Rectangle(0, 0, pbSeq.Width, pbSeq.Height))
        grafx.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        myGraphics = pbSeq.CreateGraphics
        grafx.Graphics.Clear(Color.White)
    End Sub

    '鼠标事件 用来处理拖拽和选中等等
    Private Selecting As Boolean = False

    Private Sub SequenceViewer_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbSeq.MouseDown
        Dim EP As New Point(e.X / ScaleFactor, e.Y / ScaleFactor)
        For Each ln As Line In lList
            If ln.Rect.Contains(EP) Then
                If ln.FRect.Contains(EP) Then
                    DrawFPrimer(GetPos(EP.X, Edgewidth) + ln.Sstart)
                ElseIf ln.RRect.Contains(EP) Then
                    DrawRPrimer(GetPos(EP.X, Edgewidth) + ln.Sstart)
                ElseIf ln.SRect.Contains(EP) Then
                    '选中序列
                    'TATA[XhoI][BamHI]
                    If e.Button = Windows.Forms.MouseButtons.Left Then
                        DrawSelect(GetPos(EP.X, Edgewidth) + ln.Sstart - 1, GetPos(EP.X, Edgewidth) + ln.Sstart)
                        pbMap.SelectSequence(GetPos(EP.X, Edgewidth) + ln.Sstart - 1, GetPos(EP.X, Edgewidth) + ln.Sstart)
                        Selecting = True
                    End If
                Else
                    '查找所有的工具
                    '搜索Features
                    For Each f As VecLineFeature In ln.Features
                        For Each rct As Rectangle In f.RectSection
                            If rct.Contains(EP) Then
                                '选中整个Feature
                                DrawSelect(f.Feature.StartPosition - 1, f.Feature.EndPosition)
                                pbMap.SelectSequence(f.Feature.StartPosition - 1, f.Feature.EndPosition)
                            End If
                        Next
                    Next
                End If

            End If
        Next
        'delete this
        Me.ParentForm.Text = pbSeq.Size.ToString

        pnlSeq.Focus()
    End Sub

    Private Sub SequenceViewer_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbSeq.MouseMove
        Dim EP As New Point(e.X / ScaleFactor, e.Y / ScaleFactor)
        For Each ln As Line In lList
            If ln.Rect.Contains(EP) Then
                If ln.FRect.Contains(EP) Then

                ElseIf ln.RRect.Contains(EP) Then

                ElseIf ln.SRect.Contains(EP) Then
                    '选中序列
                    'TATA[XhoI][BamHI]
                    If e.Button = Windows.Forms.MouseButtons.Left Then
                        If Selecting Then DrawSelect(SelectStart, GetPos(EP.X, Edgewidth) + ln.Sstart) : pbMap.SelectSequence(SelectStart, GetPos(EP.X, Edgewidth) + ln.Sstart)
                    End If
                Else
                    '查找所有的工具

                End If
            End If
        Next
    End Sub

    Private Sub SequenceViewer_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbSeq.MouseUp
        Dim EP As New Point(e.X / ScaleFactor, e.Y / ScaleFactor)
        For Each ln As Line In lList
            If ln.Rect.Contains(EP) Then
                If ln.FRect.Contains(EP) Then

                ElseIf ln.RRect.Contains(EP) Then

                ElseIf ln.SRect.Contains(EP) Then
                    '选中序列
                    'TATA[XhoI][BamHI]
                    If e.Button = Windows.Forms.MouseButtons.Left Then
                        DrawSelect(SelectStart, GetPos(EP.X, Edgewidth) + ln.Sstart)
                        pbMap.SelectSequence(SelectStart, GetPos(EP.X, Edgewidth) + ln.Sstart)
                        Selecting = False
                    End If

                Else
                    '查找所有的工具

                End If
            End If
        Next
    End Sub

    Private SelectStart As Integer
    Private SelectEnd As Integer
    Private Sub DrawSelect(ByVal SelStart As Integer, ByVal SelEnd As Integer)
        Dim l As Integer = GeneFile.Sequence.Length
        lbSelection.Text = "Select: " + SelStart.ToString + " - " + SelEnd.ToString

        If vGeneFile.Iscircular Then
            If SelStart >= SelEnd Then
                For i As Integer = 0 To SelEnd - 1
                    DrawChar(i, Brushes.Blue, l)
                Next
                For i As Integer = SelEnd To SelStart - 1
                    DrawChar(i, Brushes.Black, l)
                Next
                For i As Integer = SelStart To vGeneFile.Sequence.Length - 1
                    DrawChar(i, Brushes.Blue, l)
                Next
            Else
                For i As Integer = 0 To SelStart - 1
                    DrawChar(i, Brushes.Black, l)
                Next
                For i As Integer = SelStart To SelEnd - 1
                    DrawChar(i, Brushes.Blue, l)
                Next
                For i As Integer = SelEnd To vGeneFile.Sequence.Length - 1
                    DrawChar(i, Brushes.Black, l)
                Next
            End If
        Else
            For i As Integer = 0 To SelStart - 1
                DrawChar(i, Brushes.Black, l)
            Next
            For i As Integer = SelStart To SelEnd - 1
                DrawChar(i, Brushes.Blue, l)
            Next
            For i As Integer = SelEnd To vGeneFile.Sequence.Length - 1
                DrawChar(i, Brushes.Black, l)
            Next
        End If
        SelectStart = SelStart
        SelectEnd = SelEnd
        grafx.Render(myGraphics)
    End Sub
    Private Sub DrawChar(ByVal i As Integer, ByVal brs As Brush, ByVal l As Integer)
        grafx.Graphics.DrawString(vGeneFile.Sequence.Chars(i), vFont, brs, (i Mod LineCharCount) * CharWidth * 0.8 + ((i Mod LineCharCount) \ CharGroup) * (CharWidth / 3) + Edgewidth, lList(i \ LineCharCount).SRect.Y)
        grafx.Graphics.DrawString(vGeneFile.RCSequence.Chars(l - 1 - i), vFont, brs, (i Mod LineCharCount) * CharWidth * 0.8 + ((i Mod LineCharCount) \ CharGroup) * (CharWidth / 3) + Edgewidth, lList(i \ LineCharCount).SRect.Y + CharWidth * 1.2)

    End Sub
    Private Sub SequenceViewer_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pbSeq.Paint
        grafx.Render(e.Graphics)
    End Sub

    Private FPList As New List(Of Integer)
    Private RPList As New List(Of Integer)
    Private pmrFPos As Integer = -1
    Private pmrRPos As Integer = -1

    Public Sub DrawFPrimer(ByVal Pos As Integer)
        '计算出一个符合退火温度的引物
        Dim Seq As String = ""
        pmrFPos = Pos
        pbMap.DrawPCR(pmrFPos, pmrRPos)
        For Each i As Integer In FPList
            lList(i).ClearFPrimer(grafx.Graphics)
        Next
        FPList.Clear()

        For i As Integer = 10 To 30
            If Pos - 1 + i > vGeneFile.Sequence.Length Then
                Seq = vGeneFile.Sequence.Substring(Pos - 1) + vGeneFile.Sequence.Substring(0, Pos - 1 + i - vGeneFile.Sequence.Length)
            Else
                Seq = vGeneFile.Sequence.Substring(Pos - 1, i)
            End If
            If Nuctions.CalculateTm(Seq, nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm > nudTMFrom.Value Then Exit For
        Next

        Dim eos As Integer = Pos + Seq.Length - 1
        Dim l As Integer = Pos \ LineCharCount

        Dim z As Integer = Pos
        Dim zl As Integer
        tbF.Text = ParseAttachPrimer(tbF.Text) + Seq
        While z < eos
            If l > (lList.Count - 1) Then l = 0 : z -= vGeneFile.Sequence.Length : eos -= vGeneFile.Sequence.Length
            zl = Math.Min(lList(l).Send - z + 1, eos - z + 1)
            lList(l).DrawFPrimer(grafx.Graphics, z - lList(l).Sstart, Seq.Substring(0, zl))
            Seq = Seq.Substring(zl)
            z += zl
            FPList.Add(l)
            l += 1
        End While
        grafx.Render(myGraphics)

    End Sub

    Public Sub DrawRPrimer(ByVal Pos As Integer)
        '计算出一个符合退火温度的引物
        Dim Seq As String = ""
        pmrRPos = Pos
        pbMap.DrawPCR(pmrFPos, pmrRPos)
        Dim RPos As Integer = vGeneFile.Sequence.Length - Pos
        For Each i As Integer In RPList
            lList(i).ClearRPrimer(grafx.Graphics)
        Next
        RPList.Clear()

        For i As Integer = 10 To 30
            If RPos + i > vGeneFile.Sequence.Length Then
                Seq = vGeneFile.RCSequence.Substring(RPos) + vGeneFile.RCSequence.Substring(0, RPos + i - vGeneFile.Sequence.Length)
            Else
                Seq = vGeneFile.RCSequence.Substring(RPos, i)
            End If
            If Nuctions.CalculateTm(Seq, nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm > nudTMFrom.Value Then Exit For
        Next

        Dim eos As Integer = Pos - Seq.Length + 1
        Dim l As Integer = Pos \ LineCharCount

        Dim z As Integer = Pos
        Dim zl As Integer
        tbR.Text = ParseAttachPrimer(tbR.Text) + Seq
        While z > eos
            If l < 0 Then l = lList.Count - 1 : z += vGeneFile.Sequence.Length : eos += vGeneFile.Sequence.Length
            zl = Math.Min(z - lList(l).Sstart + 1, z - eos + 1)
            lList(l).DrawRPrimer(grafx.Graphics, z - lList(l).Sstart, Seq.Substring(0, zl))
            Seq = Seq.Substring(zl)
            z -= zl
            RPList.Add(l)
            l -= 1
        End While
        grafx.Render(myGraphics)

    End Sub

    Private Shared CharWidth As Integer = 16
    Private vGeneFile As Nuctions.GeneFile
    Private Shared vFont As New Font("Arial", CharWidth, FontStyle.Bold, GraphicsUnit.Pixel)
    Private Shared nFont As New Font("Arial", CharWidth, FontStyle.Bold, GraphicsUnit.Pixel)

    Private vRE As List(Of String)

    Public Property RestrictionSites() As List(Of String)
        Get
            Return vRE
        End Get
        Set(ByVal value As List(Of String))
            vRE = value
            pbMap.RestrictionSite = vRE
        End Set
    End Property

    '通过设置GeneFile属性激活视图的生成
    Public Property GeneFile() As Nuctions.GeneFile
        Get
            Return vGeneFile
        End Get
        Set(ByVal value As Nuctions.GeneFile)
            vGeneFile = value
            If Not (vGeneFile Is Nothing) Then
                Dim l As Integer = vGeneFile.Sequence.Length
                linenumber = l \ LineCharCount + 1
                pbSeq.Width = pnlSeq.Width - 16
                pbSeq.Height = ParseLines(vGeneFile)
                pbMap.GeneFile = vGeneFile
                Draw()
            End If
        End Set
    End Property
    Private linenumber As Integer
    Private Shared LineCharCount As Integer = 50
    Private Shared CharGroup As Integer = 10

    Private FeatureDict As New Dictionary(Of Nuctions.GeneAnnotation, List(Of VecLineFeature))
    Private lList As New List(Of Line)

    Private REList As New List(Of ReadOnlyCollectionBase)
    Private Edgewidth As Integer

    Public Class RE
        Public REStart As Integer
        Public REEnd As Integer
        Public REName As String

        Public Separate As Boolean = False
        Public P1 As Point
        Public L1 As Integer
        Public P2 As Point
        Public L2 As Integer
    End Class


    Public Function ParseLines(ByVal GenFile As Nuctions.GeneFile) As Integer

        Dim l As Integer = GenFile.Sequence.Length

        Dim Y As Integer = 0


        Dim nLine As Line


        '以下代码用于酶切位点的分析

        Dim ear As New Nuctions.EnzymeAnalysis.EnzymeAnalysisResult(vRE, GenFile)

        Dim eaList As List(Of Nuctions.EnzymeAnalysis) = ear.CutList

        eaList.Sort(New Nuctions.EnzymeAnalysis.EnzymeAnalysisResult.EnzymeAnalysisSorter)

        Dim maxLevel As Integer

        Edgewidth = vGeneFile.Sequence.Length.ToString.Length * CharWidth * 0.8 + 10

        For i As Integer = 0 To l \ LineCharCount
            nLine = New Line
            nLine.Sstart = 1 + i * LineCharCount
            nLine.Send = Math.Min(LineCharCount + i * LineCharCount, l)
            nLine.l = l
            nLine.Edgewidth = Edgewidth
            nLine.GF = GenFile
            If nLine.Send > l Then nLine.Send = l
            Dim f As Nuctions.GeneAnnotation
            For Each k As Integer In GenFile.Features.Keys
                f = GenFile.Features(k)

                Dim vf As New VecLineFeature
                vf.Feature = f
                vf.FPen = New Pen(GetColor(k, GenFile.Features.Count))
                Dim ps As Point() = Intersect(f.StartPosition, f.EndPosition + 1, nLine.Sstart, nLine.Send + 1, l, GenFile.Iscircular)
                For Each p As Point In ps
                    vf.RectList.Add(New PointF(GetX(p.X - nLine.Sstart, Edgewidth), GetX(p.Y - nLine.Sstart, Edgewidth)))
                Next
                If vf.RectList.Count > 0 Then
                    nLine.Features.Add(vf)
                    '把这个Feature放入字典当中
                    If FeatureDict.ContainsKey(f) Then
                        FeatureDict(f).Add(vf)
                    Else
                        FeatureDict.Add(f, New List(Of VecLineFeature))
                        FeatureDict(f).Add(vf)
                    End If
                End If
            Next
            '分析酶切位点

            Dim REalloc As Boolean(,) = New Boolean(vRE.Count - 1, 49) {}
            maxLevel = 0
            For Each rea As Nuctions.EnzymeAnalysis In eaList
                Dim RE As New RE
                RE.REName = rea.Enzyme.Name
                RE.REStart = rea.StartRec
                RE.REEnd = rea.EndRec
                Dim ps As Point() = Intersect(RE.REStart, RE.REEnd, nLine.Sstart, nLine.Send + 1, l, GenFile.Iscircular)

                Select Case ps.Length
                    Case 0

                    Case 1
                        RE.P1 = ps(0)
                        RE.L1 = FindEmpty(REalloc, 49, vRE.Count - 1, RE.P1.X Mod LineCharCount, RE.P1.Y Mod LineCharCount)
                        maxLevel = Math.Max(maxLevel, RE.L1)
                        nLine.REList.Add(RE)
                    Case 2
                        RE.Separate = True
                        RE.P1 = ps(0)
                        RE.L1 = FindEmpty(REalloc, 49, vRE.Count - 1, RE.P1.X Mod LineCharCount, RE.P1.Y Mod LineCharCount)
                        maxLevel = Math.Max(maxLevel, RE.L1)
                        RE.P2 = ps(1)
                        RE.L2 = FindEmpty(REalloc, 49, vRE.Count - 1, RE.P2.X Mod LineCharCount, RE.P2.Y Mod LineCharCount)
                        maxLevel = Math.Max(maxLevel, RE.L2)
                        nLine.REList.Add(RE)
                End Select

            Next
            nLine.ENZLevel = maxLevel
            lList.Add(nLine)
            Y += CharWidth * (5.4 + 1.4 * (nLine.Features.Count + nLine.ENZLevel)) + 7
        Next
        Return Y
    End Function

    Public Shared Function Intersect(ByVal S1 As Integer, ByVal E1 As Integer, ByVal S2 As Integer, ByVal E2 As Integer, ByVal L As Integer, ByVal circular As Boolean) As Point()
        If E1 >= S1 Then
            Dim s As Integer
            Dim e As Integer

            s = Math.Max(S1, S2)
            e = Math.Min(E1, E2)
            If e > s Then
                Return New Point() {New Point(s, e)}
            End If
        ElseIf circular Then
            Dim s As Integer
            Dim e As Integer
            Dim pList As New List(Of Point)
            s = Math.Max(0, S2)
            e = Math.Min(E1, E2)
            If e > s Then
                pList.Add(New Point(s, e))
            End If
            s = Math.Max(S1, S2)
            e = Math.Min(L, E2)
            If e > s Then
                pList.Add(New Point(s, e))
            End If
            Return pList.ToArray
        End If
        Return New Point() {}
    End Function

    Public Shared Function FindEmpty(ByVal map As Boolean(,), ByVal w As Integer, ByVal h As Integer, ByVal StartI As Integer, ByVal EndI As Integer) As Integer
        Dim ept As Boolean = True
        Dim level As Integer = 0
        While ept = True
            level += 1
            ept = False
            For i As Integer = StartI To EndI - 1
                ept = ept Or map(level - 1, i)
            Next
        End While
        For i As Integer = StartI To EndI - 1
            map(level - 1, i) = True
        Next
        Return level
    End Function

    Public Shared Function GetX(ByVal n As Integer, ByVal Edgewidth As Integer) As Single
        Return n * CharWidth * 0.8 + (n \ CharGroup) * (CharWidth / 3) + Edgewidth + 1
    End Function

    Public Shared Function GetPos(ByVal vX As Single, ByVal Edgewidth As Integer) As Integer
        Dim z As Single = vX - 1 - Edgewidth - CharWidth * 1 / 3
        Dim g As Integer = Math.Ceiling(z / (CharGroup * CharWidth * 0.8 + CharWidth / 3))
        Return Math.Ceiling((z - g * (CharWidth / 3)) / (CharWidth * 0.8))
    End Function

    Public Shared Function GetColor(ByVal i As Integer, ByVal Count As Integer) As Color
        Dim v As Integer = i / Count * 256 * 3

        Select Case v
            Case 0 To 255
                Return Color.FromArgb(v Mod 256, 255 - (v Mod 256), 40)

            Case 256 To 511
                Return Color.FromArgb(40, v Mod 256, 255 - (v Mod 256))

            Case 511 To 767
                Return Color.FromArgb(255 - (v Mod 256), 40, v Mod 256)

        End Select

    End Function

    Private Shared GrayPen As New Pen(Color.Gray, 2)

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

        Public Sub Draw(ByVal g As Graphics, ByRef vY As Integer)
            Y = vY
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
            FRect = New Rectangle(Edgewidth + 1, vY, GetX(Send - Sstart + 1, Edgewidth) - Edgewidth - 1, CharWidth * 1.5)
            vY += CharWidth * 1.5


            SRect = New Rectangle(Edgewidth + 1, vY, GetX(LineCharCount + 1, Edgewidth) - Edgewidth - 1, CharWidth * 3)
            g.DrawString((Sstart - 1).ToString, vFont, Brushes.DarkBlue, 2, vY)
            For i As Integer = Sstart - 1 To Send - 1
                g.DrawString(GF.Sequence.Chars(i), vFont, Brushes.Black, (i Mod LineCharCount) * CharWidth * 0.8 + ((i Mod LineCharCount) \ CharGroup) * (CharWidth / 3) + Edgewidth, vY)
                g.DrawString(GF.RCSequence.Chars(l - 1 - i), vFont, Brushes.Black, (i Mod LineCharCount) * CharWidth * 0.8 + ((i Mod LineCharCount) \ CharGroup) * (CharWidth / 3) + Edgewidth, vY + CharWidth * 1.2)
            Next
            vY += CharWidth * 2.4

            '空一行给引物设计
            RRect = New Rectangle(Edgewidth + 1, vY, GetX(Send - Sstart + 1, Edgewidth) - Edgewidth - 1, CharWidth * 1.5)
            vY += CharWidth * 1.5

            vY += 2
            g.DrawLine(GrayPen, 0, vY, GetX(LineCharCount + 1, Edgewidth), vY)

            vY += 5

            Rect = New Rectangle(0, Y, GetX(Send - Sstart + 1, Edgewidth), vY - Y)
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


        '用于设计引物的工具
        Public Sub DrawFPrimer()

        End Sub

        Public Sub DrawRPrimer()

        End Sub


    End Class

    Public Class VecLineFeature


        '最重要的是上端的流位置
        Public Y As Integer = 0
        Public Feature As Nuctions.GeneAnnotation
        Public FPen As Pen

        '用来测试选种与否的工具
        Public RectList As New List(Of PointF)
        Public RectSection As New List(Of Rectangle)

        Public Sub Draw(ByVal g As Graphics, ByRef vY As Integer)
            Y = vY
            Dim Rect As Rectangle
            For Each rf As PointF In RectList
                Rect = New Rectangle(rf.X, Y, rf.Y - rf.X, CharWidth * 1.2)
                g.DrawRectangle(FPen, Rect)
                RectSection.Add(Rect)
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

    Public Shared Function GetStartPosition() As Integer

    End Function

    Public ScaleFactor As Single = 1
    Public Sub Draw()
        '计算控件宽度以决定现实多少内容

        '我们默认是现实100bp/行
        '根据行数决定控件的高度
        grafx.Graphics.Clear(Color.White)
        grafx.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        grafx.Graphics.ScaleTransform(ScaleFactor, ScaleFactor)
        Dim Y As Integer = 0
        If Not (vGeneFile Is Nothing) Then
            For Each vl As Line In lList
                vl.Draw(grafx.Graphics, Y)
            Next
        End If
        grafx.Render(myGraphics)
    End Sub

    Private Sub SequenceViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pbSeq.Width = pnlMap.Width
    End Sub

    Private Sub pbSeq_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbSeq.MouseWheel, pnlSeq.MouseWheel, Me.MouseWheel

    End Sub

    Private Sub pbSeq_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbSeq.Resize
        context = BufferedGraphicsManager.Current
        context.MaximumBuffer = New Size(pbSeq.Width + 1, pbSeq.Height + 1)
        grafx = context.Allocate(CreateGraphics(), New Rectangle(0, 0, pbSeq.Width, pbSeq.Height))
        grafx.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        myGraphics = pbSeq.CreateGraphics
        'VSSeq.Maximum = Math.Max(0, pbSeq.Height - pnlSeq.Height) + 1
        Draw()
    End Sub

    Private Sub pnlSeq_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnlSeq.Resize
        'VSSeq.Maximum = Math.Max(0, pbSeq.Height - pnlSeq.Height) + 1
    End Sub

    Private Sub VSSeq_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'pbSeq.Top = -VSSeq.Value
    End Sub

    Private Function ParseInnerPrimer(ByVal pmr As String) As String
        Dim ip As Integer = pmr.LastIndexOf(">")
        ip = IIf(ip < 0, 0, ip)
        Return pmr.Substring(ip)
    End Function
    Private Function ParseAttachPrimer(ByVal pmr As String) As String
        Dim ip As Integer = pmr.LastIndexOf(">")
        ip = IIf(ip < 0, pmr.Length, ip)
        Return pmr.Substring(0, ip) + ">"
    End Function

    Private Sub tbF_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbF.TextChanged

        infoF.Text = String.Format("A:{0} {1}nt B:{2} {3}nt", Nuctions.CalculateTm(tbF.Text, nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(tbF.Text).Length.ToString, _
                                   Nuctions.CalculateTm(ParseInnerPrimer(tbF.Text), nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(ParseInnerPrimer(tbF.Text)).Length.ToString)

        AnaPrimers()
    End Sub

    Private Sub tbR_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbR.TextChanged
        infoR.Text = String.Format("A:{0} {1}nt B:{2} {3}nt", Nuctions.CalculateTm(tbR.Text, nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(tbR.Text).Length.ToString, _
                                   Nuctions.CalculateTm(ParseInnerPrimer(tbR.Text), nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(ParseInnerPrimer(tbR.Text)).Length.ToString)
        AnaPrimers()
    End Sub


    Private Sub nudTMPara_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudNa.ValueChanged, nudC.ValueChanged
        If tbF.Text.Length > 0 Then
            infoF.Text = String.Format("A:{0} {1}nt B:{2} {3}nt", Nuctions.CalculateTm(tbF.Text, nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(tbF.Text).Length.ToString, _
                                       Nuctions.CalculateTm(ParseInnerPrimer(tbF.Text), nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(ParseInnerPrimer(tbF.Text).Length.ToString))
            AnaPrimers()
        End If
        If tbR.Text.Length > 0 Then
            infoR.Text = String.Format("A:{0} {1}nt B:{2} {3}nt", Nuctions.CalculateTm(tbR.Text, nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(tbR.Text).Length.ToString, _
                                       Nuctions.CalculateTm(ParseInnerPrimer(tbR.Text), nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(ParseInnerPrimer(tbR.Text).Length.ToString))
            AnaPrimers()
        End If
    End Sub

    '利用引物分析工具分析引物
    Public Sub AnaPrimers()
        Dim pf As String = Nuctions.TAGCFilter(tbF.Text)
        Dim pr As String = Nuctions.TAGCFilter(tbR.Text)
        Dim gList As New List(Of Nuctions.GeneFile)
        Dim pmrlist As New Dictionary(Of String, String)
        pmrlist.Add("F", pf)
        pmrlist.Add("R", pr)
        gList.Add(vGeneFile)
        pafPrimer.AnalyzePrimers(pmrlist, gList, nudNa.Value * 0.001, nudC.Value * 0.000000001)
    End Sub


    Private Sub pbMap_SelectChanged(ByVal sender As Object, ByVal e As VectorMap.SelectChangedEventArgs) Handles pbMap.SelectChanged
        DrawSelect(e.Start, e.End)
        ScrollTo(e.Start)
    End Sub
    Private Sub ScrollTo(ByVal Value As Integer)
        If Value < 0 Then Value = 0
        If Value > vGeneFile.Sequence.Length Then Value = vGeneFile.Sequence.Length
        Dim i As Integer = (Value + 1) \ LineCharCount
        pbScroll.Bounds = New Rectangle(0, lList(i).Y + pbSeq.Bounds.Top, 1, lList(i).Rect.Height)
        pnlSeq.ScrollControlIntoView(pbScroll)
        'ParentForm.Text = pbScroll.Location.ToString + pbScroll.Bounds.ToString + lList(i).Y.ToString
    End Sub

#Region "处理外部事件"
    Public Shared Function DesignPrimers(ByVal pmrs As Dictionary(Of String, String)) As CustomTabPage

    End Function
    Public Shared Function ViewSequence(ByVal vGeneFile As Nuctions.GeneFile, ByVal vRE As List(Of String)) As CustomTabPage

    End Function

#End Region
End Class
