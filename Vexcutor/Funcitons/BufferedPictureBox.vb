Public Class BufferedPictureBox
    Inherits PictureBox
    Private context As BufferedGraphicsContext '双缓冲buffer管理
    Protected grafx As BufferedGraphics '双缓冲的buffer
    Private grfrec As TypeRecorder(Of Graphics)  '记录所有图片绘制过程用来在重绘时快速描绘
    Private myGraphics As Graphics '该空间的DC
    Public Sub New()
        MyBase.SetStyle(ControlStyles.UserPaint, True)
        MyBase.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        MyBase.SetStyle(ControlStyles.DoubleBuffer, True)
        MyBase.SetStyle(ControlStyles.ResizeRedraw, True)
        MyBase.SetStyle(ControlStyles.SupportsTransparentBackColor, True)

        context = BufferedGraphicsManager.Current
        context.MaximumBuffer = New Size(Width + 1, Height + 1)
        grafx = context.Allocate(CreateGraphics(), New Rectangle(0, 0, Width, Height))
        grafx.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        myGraphics = CreateGraphics()
        grafx.Graphics.Clear(Color.White)
    End Sub
    Protected Overrides Sub OnPaint(ByVal pe As System.Windows.Forms.PaintEventArgs)
        grafx.Render(pe.Graphics)
        MyBase.OnPaint(pe)
    End Sub
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        context = BufferedGraphicsManager.Current
        context.MaximumBuffer = New Size(Width + 1, Height + 1)
        If Width = 0 Or Height = 0 Then
            grafx = context.Allocate(CreateGraphics(), New Rectangle(0, 0, 1, 1))
        Else
            grafx = context.Allocate(CreateGraphics(), New Rectangle(0, 0, Width, Height))
        End If
        grafx.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        myGraphics = CreateGraphics()
        grafx.Graphics.Clear(Color.White)
        'VSSeq.Maximum = Math.Max(0, pbSeq.Height - pnlSeq.Height) + 1
        Draw()
    End Sub
    Public Sub Draw()
        grafx.Render(myGraphics)
    End Sub
    Public ReadOnly Property BufferedGraphics() As Graphics
        Get
            Return grafx.Graphics
        End Get
    End Property
End Class


Friend Class VectorMap
    Inherits BufferedPictureBox
    Private vGeneFile As Nuctions.GeneFile

    '用于酶切位点的分析
#Region "Enzyme Analysis"


    Private vRE As New List(Of String)
    Private eaDict As New List(Of RestrictionEnzymeCut)

    Public Property RestrictionSite() As List(Of String)
        Get
            Return vRE
        End Get
        Set(ByVal value As List(Of String))
            vRE = value
        End Set
    End Property

    Public Sub AnalyzeRE()
        Dim vList As New List(Of String)
        Dim ear As Nuctions.EnzymeAnalysis.EnzymeAnalysisResult
        Dim ct As RestrictionEnzymeCut
        eaDict.Clear()
        For Each res As String In vRE
            vList.Clear()
            vList.Add(res)
            ear = New Nuctions.EnzymeAnalysis.EnzymeAnalysisResult(vList, vGeneFile)


            For Each ea As Nuctions.EnzymeAnalysis In ear
                ct = New RestrictionEnzymeCut
                ct.SingleCut = (ear.Count = 1)
                ct.EnzymeAnalysis = ea
                ct.Degree = ea.StartRec / vGeneFile.Sequence.Length * 360
                eaDict.Add(ct)
            Next
        Next
        eaDict.Sort()
        For Each ct In eaDict
            vViews.Add(New FeatureView(ct.EnzymeAnalysis, ct.SingleCut, vAnnotationList.MaxLevel, Me, vGeneFile, R))
        Next
    End Sub


#End Region
    Private R As Single = 240
    Public Property GeneFile() As Nuctions.GeneFile
        Get
            Return vGeneFile
        End Get
        Set(ByVal value As Nuctions.GeneFile)
            vGeneFile = value
            If Not (vGeneFile Is Nothing) Then
                If Not (Parent Is Nothing) Then Me.Width = Me.Parent.Width \ 2
                If Me.Width < 300 Then Me.Width = 300
                R = (Me.Width - 120) / 2
#If Remote = 2 Then
                MsgBox("G1", MsgBoxStyle.OkOnly)
#End If
                AnalyzeMap()
#If Remote = 2 Then
                MsgBox("G2", MsgBoxStyle.OkOnly)
#End If
                AnalyzeRE()
#If Remote = 2 Then
                MsgBox("G3", MsgBoxStyle.OkOnly)
#End If
                AllocateText()
#If Remote = 2 Then
                MsgBox("G4", MsgBoxStyle.OkOnly)
#End If
                DrawMap()
#If Remote = 2 Then
                MsgBox("G5", MsgBoxStyle.OkOnly)
#End If
            End If
        End Set
    End Property

    '用于给绘图文件编制级别
#Region "编制级别"


    Private vAnnotationList As New AllocatList

    Public Class AllocatList
        Inherits List(Of List(Of LinearRegion))
        Private vDict As New Dictionary(Of Nuctions.GeneAnnotation, LinearRegion)
        Public Shadows Sub Add(ByVal ft As Nuctions.GeneAnnotation, ByVal vGeneFile As Nuctions.GeneFile)
            Dim Added As Boolean
            Dim Available As Boolean
            Dim vLR As LinearRegion
            Dim LG As List(Of LinearRegion)

            If vGeneFile.Iscircular Then
                vLR = New LinearRegion(ft.StartPosition, ft.EndPosition)
            Else
                vLR = New LinearRegion(ft.StartPosition, ft.EndPosition, vGeneFile.Sequence.Length)
            End If
            '添加到字典当中
            vDict.Add(ft, vLR)
            Added = False
            While Not Added
                For i As Integer = 0 To Me.Count - 1
                    LG = Me(i)
                    Available = True
                    For Each LR As LinearRegion In LG
                        Available = Available And Not (LR.OverLaps(vLR)) And Not (vLR.OverLaps(LR))
                    Next
                    If Available Then
                        LG.Add(vLR)
                        Added = True
                        vLR.Level = i
                    End If
                Next
                '如果所有的行都排满了 那么就再加一行
                If Not Added Then MyBase.Add(New List(Of LinearRegion))
            End While
        End Sub
        Public ReadOnly Property Level(ByVal ft As Nuctions.GeneAnnotation) As LinearRegion
            Get
                Return vDict(ft)
            End Get
        End Property
        Public ReadOnly Property MaxLevel() As Integer
            Get
                Return Me.Count
            End Get
        End Property
    End Class

    Public Sub AnalyzeMap()
        vAnnotationList = New AllocatList
        UpTA = New TextAllocator(True)
        DownTA = New TextAllocator(False)
        vViews.Clear()
        For Each ft As Nuctions.GeneAnnotation In vGeneFile.Features
            vAnnotationList.Add(ft, vGeneFile)
        Next

        vViews.Add(New FeatureView(Nothing, 0, vAnnotationList.MaxLevel, Me, vGeneFile, R))
        For Each ft As Nuctions.GeneAnnotation In vGeneFile.Features
            vViews.Add(New FeatureView(ft, vAnnotationList.Level(ft).Level, vAnnotationList.MaxLevel, Me, vGeneFile, R))
        Next
    End Sub

    Public Sub AllocateText()
#If Remote = 2 Then
        MsgBox("AT0", MsgBoxStyle.OkOnly)
#End If
        vViews.Sort()
#If Remote = 2 Then
        MsgBox("AT1", MsgBoxStyle.OkOnly)
#End If
        For Each ftv As FeatureView In vViews
#If Remote = 2 Then
            MsgBox("AT2", MsgBoxStyle.OkOnly)
#End If
            ftv.AllocateText(UpTA, DownTA)
#If Remote = 2 Then
            MsgBox("AT3", MsgBoxStyle.OkOnly)
#End If
        Next
#If Remote = 2 Then
        MsgBox("ATX", MsgBoxStyle.OkOnly)
#End If
    End Sub
#End Region

#Region "视图事件管理"
    Public Event SelectChanged(ByVal sender As Object, ByVal e As SelectChangedEventArgs)
    Private SelectStart As Integer
    Private SelectEnd As Integer

    Public Sub SelectSequence(ByVal start As Integer, ByVal [end] As Integer)
        ReDrawMap()
        SelectStart = start
        SelectEnd = [end]
        ReDrawSelectSequence()
        ReDrawPCR()
        Draw()
    End Sub
    Public Sub ReDrawSelectSequence()
        Dim st As Single = SelectStart / vGeneFile.Sequence.Length * 360
        Dim sw As Single

        If vGeneFile.Iscircular Then
            If SelectEnd < SelectStart Then
                sw = (SelectEnd - SelectStart) / vGeneFile.Sequence.Length * 360 + 360
            Else
                sw = (SelectEnd - SelectStart) / vGeneFile.Sequence.Length * 360
            End If
            BufferedGraphics.DrawPie(Pens.Blue, New RectangleF(-R, -R, 2 * R, 2 * R), st, sw)
        Else
            sw = (SelectEnd - SelectStart) / vGeneFile.Sequence.Length * 360
            BufferedGraphics.DrawRectangle(Pens.Blue, -R + 2 * R * st / 360, 20, 2 * R * sw / 360, 40)
        End If
    End Sub
    Private PCRStart As Integer = -1
    Private PCREnd As Integer = -1
    Private PCRValid As Boolean = False
    Public Sub DrawPCR(ByVal start As Integer, ByVal [end] As Integer, Optional ByVal Valid As Boolean = False)
        ReDrawMap()
        ReDrawSelectSequence()
        PCRStart = start
        PCREnd = [end]
        If PCRStart >= 0 And PCREnd >= 0 Then PCRValid = True
        ReDrawPCR()
        Draw()
    End Sub
    Public Sub ReDrawPCR()
        If PCRValid Then
            Dim st As Single = PCRStart / vGeneFile.Sequence.Length * 360
            Dim sw As Single

            If vGeneFile.Iscircular Then
                If PCREnd < PCRStart Then
                    sw = (PCREnd - PCRStart) / vGeneFile.Sequence.Length * 360 + 360
                Else
                    sw = (PCREnd - PCRStart) / vGeneFile.Sequence.Length * 360
                End If
                BufferedGraphics.DrawPie(Pens.Red, New RectangleF(-R * 0.9, -R * 0.9, 2 * R * 0.9, 2 * R * 0.9), st, sw)
            Else
                sw = (PCREnd - PCRStart) / vGeneFile.Sequence.Length * 360
                BufferedGraphics.DrawRectangle(Pens.Red, -R + 2 * R * st / 360, 20, 2 * R * sw / 360, 30)
            End If
        End If
    End Sub
    Public Class SelectChangedEventArgs
        Public Start As Integer
        Public [End] As Integer
        Public Feature As Nuctions.GeneAnnotation
        Public Sub New(ByVal vStart As Integer, ByVal vEnd As Integer)
            Start = vStart
            [End] = vEnd
        End Sub
        Public Sub New(ByVal vStart As Integer, ByVal vEnd As Integer, ByVal vFeature As Nuctions.GeneAnnotation)
            Start = vStart
            [End] = vEnd
            Feature = vFeature
        End Sub
    End Class
    Private dragging As Boolean = False
    Private selStart As Integer
    Private selEnd As Integer
    Private _SelectedFeature As Nuctions.GeneAnnotation
    Public Property SelectedFeature As Nuctions.GeneAnnotation
        Get
            Return _SelectedFeature
        End Get
        Set(value As Nuctions.GeneAnnotation)
            'If _SelectedFeature IsNot Nothing Then
            '    For Each VF In vViews
            '        If VF.Feature Is _SelectedFeature Then
            '            VF.DrawShape()
            '        End If
            '    Next
            'End If

            _SelectedFeature = value
            'If _SelectedFeature IsNot Nothing Then
            '    For Each VF In vViews
            '        If VF.Feature Is _SelectedFeature Then
            '            VF.DrawSelectedShape()
            '        End If
            '    Next
            'End If
        End Set
    End Property
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left Then

            Dim VF As FeatureView

            For i As Integer = vViews.Count - 1 To 0 Step -1
                VF = vViews(i)
                If VF.Region.IsVisible(e.X - OffsetX, e.Y - OffsetY) Then
                    SelectedFeature = VF.Feature
                    RaiseEvent SelectChanged(Me, New SelectChangedEventArgs(VF.StartFeature, VF.EndFeature - 1, VF.Feature))
                    SelectSequence(VF.StartFeature, VF.EndFeature)
                    Exit Sub
                End If
            Next
            If vGeneFile.Iscircular Then
                selStart = Math.Atan2(e.Y - OffsetY, e.X - OffsetX) / Math.PI / 2 * vGeneFile.Sequence.Length - 1
                selEnd = Math.Atan2(e.Y - OffsetY, e.X - OffsetX) / Math.PI / 2 * vGeneFile.Sequence.Length
                If selStart < 0 Then selStart += vGeneFile.Sequence.Length
                If selEnd < 0 Then selEnd += vGeneFile.Sequence.Length
                SelectSequence(selStart, selEnd)
                'RaiseEvent SelectChanged(Me, New SelectChangedEventArgs(selStart, selEnd))
            Else
                If e.X - OffsetX >= -R And e.X - OffsetX <= R Then
                    selStart = (e.X - OffsetX + R) / 2 / R * vGeneFile.Sequence.Length
                    selEnd = (e.X - OffsetX + R) / 2 / R * vGeneFile.Sequence.Length
                    SelectSequence(selStart, selEnd)
                    RaiseEvent SelectChanged(Me, New SelectChangedEventArgs(selStart, selEnd))
                End If
            End If
            dragging = True
        End If

    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If dragging Then
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If vGeneFile.Iscircular Then
                    selEnd = Math.Atan2(e.Y - OffsetY, e.X - OffsetX) / Math.PI / 2 * vGeneFile.Sequence.Length
                    If selEnd < 0 Then selEnd += vGeneFile.Sequence.Length
                    SelectSequence(selStart, selEnd)
                    RaiseEvent SelectChanged(Me, New SelectChangedEventArgs(selStart, selEnd))
                Else
                    If e.X - OffsetX >= -R And e.X - OffsetX <= R Then
                        selEnd = (e.X - OffsetX + R) / 2 / R * vGeneFile.Sequence.Length
                        SelectSequence(selStart, selEnd)
                        RaiseEvent SelectChanged(Me, New SelectChangedEventArgs(selStart, selEnd))
                    End If
                End If
            End If
        End If

    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If dragging Then
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If vGeneFile.Iscircular Then
                    selEnd = Math.Atan2(e.Y - OffsetY, e.X - OffsetX) / Math.PI / 2 * vGeneFile.Sequence.Length
                    If selEnd < 0 Then selEnd += vGeneFile.Sequence.Length
                    SelectSequence(selStart, selEnd)
                    RaiseEvent SelectChanged(Me, New SelectChangedEventArgs(selStart, selEnd))
                Else
                    If e.X - OffsetX >= -R And e.X - OffsetX <= R Then
                        selEnd = (e.X - OffsetX + R) / 2 / R * vGeneFile.Sequence.Length
                        SelectSequence(selStart, selEnd)
                        RaiseEvent SelectChanged(Me, New SelectChangedEventArgs(selStart, selEnd))
                    End If
                End If
                dragging = False
            End If
        End If
    End Sub
#End Region
    Private OffsetX As Single
    Private OffsetY As Single

    Public Sub DrawMap()
        Dim rf As RectangleF
        Dim t, b, l, r As Single

        For Each VF As FeatureView In vViews
            rf = VF.Region.GetBounds(BufferedGraphics)
            If rf.Left < l Then l = rf.Left
            If rf.Right > r Then r = rf.Right
            If rf.Top < t Then t = rf.Top
            If rf.Bottom > b Then b = rf.Bottom
        Next
        Me.Width = r - l + 60
        Me.Height = b - t + 60
        BufferedGraphics.ResetTransform()
        BufferedGraphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        OffsetX = -l + 30
        OffsetY = -t + 30
        BufferedGraphics.TranslateTransform(OffsetX, OffsetY)
        For Each VF As FeatureView In vViews
            VF.DrawShape()
        Next
        For Each VF As FeatureView In vViews
            VF.DrawLine()
        Next
        For Each VF As FeatureView In vViews
            VF.DrawText()
        Next
        Draw()
    End Sub
    Public Function GetVectorMap() As Bitmap
        Dim bitmap As New Bitmap(Me.Width, Me.Height, Imaging.PixelFormat.Format32bppArgb)
        Dim g As Graphics = Graphics.FromImage(bitmap)
        grafx.Render(g)
        g.Dispose()
        Return bitmap
    End Function
    Public Sub DrawImage(ByVal g As Graphics, ByVal dX As Single, ByVal dY As Single)
        g.TranslateTransform(dX + OffsetX, dY + OffsetY)

        For Each VF As FeatureView In vViews
            VF.DrawShapeTo(g)
        Next
        For Each VF As FeatureView In vViews
            VF.DrawLineTo(g)
        Next
        For Each VF As FeatureView In vViews
            VF.DrawTextTo(g)
        Next
        g.TranslateTransform(-dX - OffsetX, -dY - OffsetY)
    End Sub
    Public Sub ReDrawMap() 'How to draw the map and shape?
        BufferedGraphics.Clear(Color.White)
        For Each VF As FeatureView In vViews
            'If VF.Feature Is selected Then
            If VF.Feature Is _SelectedFeature Then
                VF.DrawSelectedShape()
            Else
                VF.DrawShape()
            End If
        Next
        For Each VF As FeatureView In vViews
            'VF.DrawLine()
            If VF.Feature Is _SelectedFeature Then
                VF.DrawSelectedLine()
            Else
                VF.DrawLine()
            End If
        Next
        For Each VF As FeatureView In vViews
            If VF.Feature Is _SelectedFeature Then
                VF.DrawSelectedText()
            Else
                VF.DrawText()
            End If
        Next
    End Sub

    '管理所有视图的工具
    Private vViews As New List(Of FeatureView)
    Private UpTA As New TextAllocator(True)
    Private DownTA As New TextAllocator(False)

End Class

Public Class LinearRegion
    Public Start As Integer
    Public [End] As Integer
    Public Length As Integer
    Public Level As Integer
    Public Sub New()
    End Sub
    Public Sub New(ByVal vStart As Integer, ByVal vEnd As Integer, Optional ByVal vLength As Integer = -1)
        Start = vStart
        [End] = vEnd
        Length = vLength
        If Length > 0 Then
            '如果有固定长度的话 就是一个封闭区间了
            If Start < 0 Then Start = 0
            If [End] < 0 Then [End] = 0
            If Start > Length Then Start = Length
            If [End] > Length Then [End] = Length
        End If
    End Sub
    Public ReadOnly Property IsCircular() As Boolean
        Get
            Return Length > 0
        End Get
    End Property
    Public Function Contains(ByVal Index As Integer) As Boolean
        If Length > 0 Then
            If Start > [End] Then
                Return (0 <= Index And Index < [End]) Or (Start <= Index And Index < Length)
            Else
                Return (Start <= Index And Index < [End])
            End If
        Else
            Return (Start <= Index And Index < [End])
        End If
    End Function
    Public Function OverLaps(ByVal LR As LinearRegion) As Boolean
        Return Contains(LR.Start) Or Contains(LR.End)
    End Function
    Public Shared Operator =(ByVal LR1 As LinearRegion, ByVal LR2 As LinearRegion) As Boolean
        Return LR1.Start = LR2.Start And LR1.End = LR2.End
    End Operator
    Public Shared Operator <>(ByVal LR1 As LinearRegion, ByVal LR2 As LinearRegion) As Boolean
        Return Not (LR1.Start = LR2.Start And LR1.End = LR2.End)
    End Operator
End Class

Public Class TextAllocator
    Inherits List(Of RectangleF)
    '默认是向上寻找
    Public Up As Boolean = True
    Public Sub New(ByVal vUp As Boolean)
        Up = vUp
    End Sub
    Public Shadows Sub Add(ByVal rect As ExtRectF)
        Dim allocated As Boolean = False
        Dim hit As Boolean

        While Not allocated
            hit = False
            For Each r As RectangleF In Me
                If r.IntersectsWith(rect) Then
                    hit = True
                    If up Then
                        rect.Y = r.Top - rect.Height - 1
                    Else
                        rect.Y = r.Bottom + 1
                    End If
                    Exit For
                End If
            Next
            If Not hit Then
                MyBase.Add(rect)
                allocated = True
            End If
        End While
    End Sub
End Class
Public Class FeatureView
    Implements IComparable(Of FeatureView)
    Public Shape As System.Drawing.Drawing2D.GraphicsPath
    Public Region As Region
    Public LineStart As PointF
    Public LineEnd As PointF
    '对于环形的质粒图有方向一说
    Public Direction As Single
    Public TextRectangle As ExtRectF
    Public Text As String
    Public Font As Font = New Font("Arial", 10)
    Public FtFont As Font = New Font("Arial", 10, FontStyle.Bold)
    Public TextColor As Brush
    Public FillColor As Brush
    Public MaxLevel As Integer
    Public G As BufferedPictureBox
    Public IsCircular As Boolean
    Public StartFeature As Integer
    Public EndFeature As Integer
    Public IsEnzyme As Boolean = False
    Public Feature As Nuctions.GeneAnnotation
    Public IsSource As Boolean = False

    'Public TextSerial As Single

    Public Sub New(ByVal ft As Nuctions.GeneAnnotation, ByVal Level As Integer, ByVal vMaxLevel As Integer, ByVal vGrafx As BufferedPictureBox, ByVal vG As Nuctions.GeneFile, ByVal R As Single) ', ByVal UpAllocator As TextAllocator, ByVal DownAllocator As TextAllocator)
        If ft Is Nothing Then
            '基因本身 一个矩形或者一个圆环
            'MaxLevel Allocate Text
            'Mr. Gene
            IsSource = True
            IsCircular = vG.Iscircular

            Text = vG.Name + " <" + vG.Sequence.Length.ToString + "bp>"
            G = vGrafx
            TextColor = Brushes.Black
            MaxLevel = vMaxLevel
            Shape = GetSmallRoundRectangle(R, 0, 0, 360)
            TextRectangle = New ExtRectF
            Font = New Font("Arial", 14, FontStyle.Bold)
            TextRectangle.Size = G.BufferedGraphics.MeasureString(Text, FtFont)
            If IsCircular Then
                TextRectangle.Center = New PointF(0, 0)
            Else
                TextRectangle.Center = New PointF(0, 50)
            End If
            LineEnd = LineStart
            FillColor = Brushes.Gray
            Region = New Region(TextRectangle)
            Region.Union(Shape)
            StartFeature = 0
            EndFeature = vG.Sequence.Length
            'TextSerial = 0
        Else
            Feature = ft
            IsCircular = vG.Iscircular
            Text = ft.Label + " - " + ft.Type
            G = vGrafx
            TextColor = Brushes.Black
            MaxLevel = vMaxLevel
            Dim start As Single
            Dim sweep As Single
            If ft.Complement Then
                start = ft.EndPosition / vG.Sequence.Length * 360
                If ft.StartPosition > ft.EndPosition Then
                    sweep = -(ft.EndPosition + vG.Sequence.Length - ft.StartPosition + 1) / vG.Sequence.Length * 360
                Else
                    sweep = -(ft.EndPosition - ft.StartPosition + 1) / vG.Sequence.Length * 360
                End If
            Else
                start = (ft.StartPosition - 1) / vG.Sequence.Length * 360
                If ft.StartPosition > ft.EndPosition Then
                    sweep = (ft.EndPosition + vG.Sequence.Length - ft.StartPosition + 1) / vG.Sequence.Length * 360
                Else
                    sweep = (ft.EndPosition - ft.StartPosition + 1) / vG.Sequence.Length * 360
                End If
            End If
            Select Case ft.Type.ToLower
                Case "match", "miss"
                    Shape = GetLargeRoundArrow(R, Level, start, sweep)
                Case Else
                    Shape = GetRoundArrow(R, Level, start, sweep)
            End Select
            TextRectangle = New ExtRectF
            TextRectangle.Size = G.BufferedGraphics.MeasureString(Text, FtFont)
            'If Direction >= 180 Then
            '    TextRectangle.South = LineEnd
            '    UpAllocator.Add(TextRectangle)
            '    LineEnd = TextRectangle.South
            'Else
            '    TextRectangle.North = LineEnd
            '    DownAllocator.Add(TextRectangle)
            '    LineEnd = TextRectangle.North
            'End If
            'Region.Union(TextRectangle)
            Select Case ft.Type.ToLower
                Case "source", "match"
                    FillColor = Brushes.Yellow
                Case "gene"
                    FillColor = Brushes.LawnGreen
                Case "cds"
                    FillColor = Brushes.SeaGreen
                Case "promoter"
                    FillColor = Brushes.Purple
                Case "misc_feature"
                    FillColor = Brushes.Brown
                Case "exon"
                    FillColor = Brushes.Gold
                Case "loci"
                    FillColor = Brushes.Peru
                Case "mutation"
                    FillColor = Brushes.PaleVioletRed
                Case "operon"
                    FillColor = Brushes.Tomato
                Case "orit"
                    FillColor = Brushes.Chartreuse
                Case "primer_bind"
                    FillColor = Brushes.SpringGreen
                Case "rep_origin", "miss"
                    FillColor = Brushes.Red
                Case "terminator"
                    FillColor = Brushes.DarkSlateGray
                Case "recombination_site"
                    FillColor = Brushes.Pink
                Case "rbs"
                    FillColor = Brushes.Cyan
                Case Else
                    FillColor = Brushes.Orange
                    'CDS()
                    'exon()
                    'enhancer()
                    'gene()
                    'loci()
                    'misc_feature()
                    'misc_signal()
                    'mutation()
                    'operon()
                    'oriT()
                    'primer_bind()
                    'promoter()
                    'RBS()
                    'recombination_site()
                    'rep_origin()
                    'terminator()
            End Select
            Region = New Region(Shape)
            StartFeature = ft.StartPosition - 1
            EndFeature = ft.EndPosition
        End If
    End Sub
    Friend Sub New(ByVal ea As Nuctions.EnzymeAnalysis, ByVal SingleCut As Boolean, ByVal vMaxLevel As Integer, ByVal vGrafx As BufferedPictureBox, ByVal vG As Nuctions.GeneFile, ByVal R As Single) ', ByVal UpAllocator As TextAllocator, ByVal DownAllocator As TextAllocator)
        IsCircular = vG.Iscircular
        Dim SC As Integer = ea.SCut Mod vG.Sequence.Length
        Dim AC As Integer = ea.ACut Mod vG.Sequence.Length
        Text = ea.Enzyme.Name + " (" + SC.ToString + "/" + AC.ToString + ")"
        G = vGrafx
        TextColor = Brushes.Black
        MaxLevel = vMaxLevel
        Dim start As Single
        Dim sweep As Single
        start = ea.StartRec / vG.Sequence.Length * 360
        sweep = (ea.EndRec - ea.StartRec) / vG.Sequence.Length * 360
        Shape = GetLargeRoundRectangle(R, 0, start, sweep)
        TextRectangle = New ExtRectF
        IsEnzyme = True
        TextRectangle.Size = G.BufferedGraphics.MeasureString(Text, Font)
        'If Direction >= 180 Then
        '    TextRectangle.South = LineEnd
        '    UpAllocator.Add(TextRectangle)
        '    LineEnd = TextRectangle.South
        'Else
        '    TextRectangle.North = LineEnd
        '    DownAllocator.Add(TextRectangle)
        '    LineEnd = TextRectangle.North
        'End If
        'Region.Union(TextRectangle)

        If SingleCut Then
            TextColor = Brushes.OrangeRed
        Else
            TextColor = Brushes.DarkViolet
        End If
        FillColor = Brushes.Navy
        Region = New Region(Shape)
        StartFeature = ea.StartRec
        EndFeature = ea.EndRec
    End Sub
    Public Sub AllocateText(ByVal UpAllocator As TextAllocator, ByVal DownAllocator As TextAllocator)
        If Not IsSource Then
            If Direction >= 180 Then
                TextRectangle.South = LineEnd
                UpAllocator.Add(TextRectangle)
                LineEnd = TextRectangle.South
            Else
                TextRectangle.North = LineEnd
                DownAllocator.Add(TextRectangle)
                LineEnd = TextRectangle.North
            End If
            Region.Union(TextRectangle)
        End If
    End Sub
    Public Sub DrawShape()
        G.BufferedGraphics.FillPath(Me.FillColor, Shape)
        G.BufferedGraphics.DrawPath(Pens.Black, Shape)
    End Sub
    Public Sub DrawSelectedShape()
        G.BufferedGraphics.FillPath(Me.FillColor, Shape)
        G.BufferedGraphics.DrawPath(New Pen(Brushes.Blue, 1.5F), Shape)
    End Sub
    Public Sub DrawShapeTo(ByVal vG As Graphics)
        vG.FillPath(Me.FillColor, Shape)
        vG.DrawPath(Pens.Black, Shape)
    End Sub
    Public Sub DrawLine()
        G.BufferedGraphics.DrawLine(Pens.Black, LineStart, LineEnd)
    End Sub
    Public Sub DrawSelectedLine()
        G.BufferedGraphics.DrawLine(Pens.Blue, LineStart, LineEnd)
    End Sub
    Public Sub DrawLineTo(ByVal vG As Graphics)
        vG.DrawLine(Pens.Black, LineStart, LineEnd)
    End Sub
    Public Sub DrawText()
        G.BufferedGraphics.DrawString(Text, IIf(IsEnzyme, Font, FtFont), TextColor, TextRectangle.Location)
    End Sub
    Public Sub DrawSelectedText()
        G.BufferedGraphics.DrawString(Text, IIf(IsEnzyme, Font, FtFont), Brushes.Blue, TextRectangle.Location)
    End Sub
    Public Sub DrawTextTo(ByVal vG As Graphics)
        vG.DrawString(Text, IIf(IsEnzyme, Font, FtFont), TextColor, TextRectangle.Location)
    End Sub
    Private Function GetRect(ByVal R As Single) As RectangleF
        Return New RectangleF(-R, -R, 2 * R, 2 * R)
    End Function
    Private ChannelWidth As Single = 20
    Private Function GetRoundArrow(ByVal R As Single, ByVal Channel As Integer, ByVal start As Single, ByVal sweep As Single) As System.Drawing.Drawing2D.GraphicsPath
        If IsCircular Then
            Dim gp As New System.Drawing.Drawing2D.GraphicsPath
            Dim rc As Single = R + Channel * ChannelWidth
            Dim ri As Single = rc - ChannelWidth / 4
            Dim ro As Single = rc + ChannelWidth / 4
            Dim re As Single = rc + ChannelWidth / 2
            Dim rv As Single = rc - ChannelWidth / 2
            If Math.Abs(sweep / 180 * Math.PI * rc) < ChannelWidth Then
                gp.AddArc(-ro, -ro, 2 * ro, 2 * ro, start, sweep / 2)
                gp.AddLine(CSng(ro * Cos(start + sweep / 2)), CSng(ro * Sin(start + sweep / 2)), CSng(re * Cos(start + sweep / 2)), CSng(re * Sin(start + sweep / 2)))
                gp.AddLine(CSng(re * Cos(start + sweep / 2)), CSng(re * Sin(start + sweep / 2)), CSng(rc * Cos(start + sweep)), CSng(rc * Sin(start + sweep)))
                gp.AddLine(CSng(rc * Cos(start + sweep)), CSng(rc * Sin(start + sweep)), CSng(rv * Cos(start + sweep / 2)), CSng(rv * Sin(start + sweep / 2)))
                gp.AddLine(CSng(rv * Cos(start + sweep / 2)), CSng(rv * Sin(start + sweep / 2)), CSng(ri * Cos(start + sweep / 2)), CSng(ri * Sin(start + sweep / 2)))
                gp.AddArc(-ri, -ri, 2 * ri, 2 * ri, start + sweep / 2, -sweep / 2)
                gp.CloseFigure()
            Else
                Dim av As Single = Math.Sign(sweep) * (Math.Abs(sweep) - (ChannelWidth / 2 / rc / Math.PI * 180))
                gp.AddArc(-ro, -ro, 2 * ro, 2 * ro, start, av)
                gp.AddLine(CSng(ro * Cos(start + av)), CSng(ro * Sin(start + av)), CSng(re * Cos(start + av)), CSng(re * Sin(start + av)))
                gp.AddLine(CSng(re * Cos(start + av)), CSng(re * Sin(start + av)), CSng(rc * Cos(start + sweep)), CSng(rc * Sin(start + sweep)))
                gp.AddLine(CSng(rc * Cos(start + sweep)), CSng(rc * Sin(start + sweep)), CSng(rv * Cos(start + av)), CSng(rv * Sin(start + av)))
                gp.AddLine(CSng(rv * Cos(start + av)), CSng(rv * Sin(start + av)), CSng(ri * Cos(start + av)), CSng(ri * Sin(start + av)))
                gp.AddArc(-ri, -ri, 2 * ri, 2 * ri, start + av, -av)
                gp.CloseFigure()
            End If
            LineStart = New PointF(rc * Cos(start + sweep / 2), rc * Sin(start + sweep / 2))
            Dim rm As Single = R + ChannelWidth * (MaxLevel + 3)
            LineEnd = New PointF(rm * Cos(start + sweep / 2), rm * Sin(start + sweep / 2))
            Direction = start + sweep / 2
            Return gp
        Else
            Dim gp As New System.Drawing.Drawing2D.GraphicsPath
            Dim rc As Single = 0 - Channel * ChannelWidth
            Dim ri As Single = rc - ChannelWidth / 4
            Dim ro As Single = rc + ChannelWidth / 4
            Dim re As Single = rc + ChannelWidth / 2
            Dim rv As Single = rc - ChannelWidth / 2
            If Math.Abs(sweep / 360 * R) < ChannelWidth / 2 Then
                gp.AddLines(New PointF() {New PointF(-R + 2 * R * start / 360, ro), _
                                           New PointF(-R + 2 * R * (start + sweep / 2) / 360, ro), _
                                            New PointF(-R + 2 * R * (start + sweep / 2) / 360, re), _
                                            New PointF(-R + 2 * R * (start + sweep) / 360, rc), _
                                             New PointF(-R + 2 * R * (start + sweep / 2) / 360, rv), _
                                              New PointF(-R + 2 * R * (start + sweep / 2) / 360, ri), _
                                              New PointF(-R + 2 * R * (start) / 360, ri)})
                gp.CloseFigure()
            Else
                Dim av As Single = Math.Sign(sweep) * ChannelWidth / 2
                gp.AddLines(New PointF() {New PointF(-R + 2 * R * start / 360, ro), _
                           New PointF(-R + 2 * R * (start + sweep) / 360 - av, ro), _
                 New PointF(-R + 2 * R * (start + sweep) / 360 - av, re), _
                New PointF(-R + 2 * R * (start + sweep) / 360, rc), _
                 New PointF(-R + 2 * R * (start + sweep) / 360 - av, rv), _
                 New PointF(-R + 2 * R * (start + sweep) / 360 - av, ri), _
                  New PointF(-R + 2 * R * (start) / 360, ri)})
                gp.CloseFigure()
            End If
            LineStart = New PointF(-R + 2 * R * (start + sweep / 2) / 360, rc)
            Dim rm As Single = -ChannelWidth * (MaxLevel + 3)
            LineEnd = New PointF(-R + 2 * R * (start + sweep / 2) / 360, rm)
            Direction = 270
            Return gp
        End If
    End Function
    Public Function GetSmallRoundRectangle(ByVal R As Single, ByVal Channel As Integer, ByVal start As Single, ByVal sweep As Single) As System.Drawing.Drawing2D.GraphicsPath
        If IsCircular Then


            Dim gp As New System.Drawing.Drawing2D.GraphicsPath
            Dim rc As Single = R + Channel * ChannelWidth
            Dim ri As Single = rc - ChannelWidth / 6
            Dim ro As Single = rc + ChannelWidth / 6
            gp.AddArc(-ro, -ro, 2 * ro, 2 * ro, start, sweep)
            gp.AddLine(CSng(ro * Cos(start + sweep)), CSng(ro * Sin(start + sweep)), CSng(ri * Cos(start + sweep)), CSng(ri * Sin(start + sweep)))
            gp.AddArc(-ri, -ri, 2 * ri, 2 * ri, start + sweep, -sweep)
            gp.CloseFigure()
            LineStart = New PointF(rc * Cos(start + sweep / 2), rc * Sin(start + sweep / 2))
            Dim rm As Single = R + ChannelWidth * (MaxLevel + 3)
            LineEnd = New PointF(rm * Cos(start + sweep / 2), rm * Sin(start + sweep / 2))
            Direction = start + sweep / 2
            Return gp
        Else
            Dim gp As New System.Drawing.Drawing2D.GraphicsPath
            Dim rc As Single = 0 - Channel * ChannelWidth
            Dim ri As Single = rc - ChannelWidth / 6
            Dim ro As Single = rc + ChannelWidth / 6
            gp.AddLines(New PointF() {New PointF(-R + 2 * R * start / 360, ro), _
                        New PointF(-R + 2 * R * (start + sweep) / 360, ro), _
             New PointF(-R + 2 * R * (start + sweep) / 360, ri), _
              New PointF(-R + 2 * R * (start) / 360, ri)})
            gp.CloseFigure()
            LineStart = New PointF(-R + 2 * R * (start + sweep / 2) / 360, rc)
            Dim rm As Single = -ChannelWidth * (MaxLevel + 3)
            LineEnd = New PointF(-R + 2 * R * (start + sweep / 2) / 360, rm)
            Direction = 270
            Return gp
        End If
    End Function
    Public Function GetLargeRoundArrow(ByVal R As Single, ByVal Channel As Integer, ByVal start As Single, ByVal sweep As Single) As System.Drawing.Drawing2D.GraphicsPath
        If IsCircular Then
            Dim gp As New System.Drawing.Drawing2D.GraphicsPath
            Dim rc As Single = R + Channel * ChannelWidth
            Dim ri As Single = rc - ChannelWidth / 4
            Dim ro As Single = rc + ChannelWidth / 4
            Dim re As Single = rc + ChannelWidth / 2
            Dim rv As Single = rc - ChannelWidth / 2
            If Math.Abs(sweep / 180 * Math.PI * rc) < ChannelWidth Then
                gp.AddArc(-ro, -ro, 2 * ro, 2 * ro, start, sweep * 2 / 3)
                'gp.AddLine(CSng(ro * Cos(start + sweep * 2 / 3)), CSng(ro * Sin(start + sweep / 2)), CSng(re * Cos(start + sweep / 2)), CSng(re * Sin(start + sweep / 2)))
                gp.AddLine(CSng(re * Cos(start + sweep * 2 / 3)), CSng(re * Sin(start + sweep / 2)), CSng(rc * Cos(start + sweep)), CSng(rc * Sin(start + sweep)))
                gp.AddLine(CSng(rc * Cos(start + sweep)), CSng(rc * Sin(start + sweep)), CSng(rv * Cos(start + sweep * 2 / 3)), CSng(rv * Sin(start + sweep * 2 / 3)))
                'gp.AddLine(CSng(rv * Cos(start + sweep * 2 / 3)), CSng(rv * Sin(start + sweep / 2)), CSng(ri * Cos(start + sweep / 2)), CSng(ri * Sin(start + sweep / 2)))
                gp.AddArc(-ri, -ri, 2 * ri, 2 * ri, start + sweep * 2 / 3, -sweep * 2 / 3)
                gp.CloseFigure()
            Else
                Dim av As Single = Math.Sign(sweep) * (Math.Abs(sweep) - (ChannelWidth / 3 / rc / Math.PI * 180))
                gp.AddArc(-ro, -ro, 2 * ro, 2 * ro, start, av)
                'gp.AddLine(CSng(ro * Cos(start + av)), CSng(ro * Sin(start + av)), CSng(re * Cos(start + av)), CSng(re * Sin(start + av)))
                gp.AddLine(CSng(re * Cos(start + av)), CSng(re * Sin(start + av)), CSng(rc * Cos(start + sweep)), CSng(rc * Sin(start + sweep)))
                gp.AddLine(CSng(rc * Cos(start + sweep)), CSng(rc * Sin(start + sweep)), CSng(rv * Cos(start + av)), CSng(rv * Sin(start + av)))
                'gp.AddLine(CSng(rv * Cos(start + av)), CSng(rv * Sin(start + av)), CSng(ri * Cos(start + av)), CSng(ri * Sin(start + av)))
                gp.AddArc(-ri, -ri, 2 * ri, 2 * ri, start + av, -av)
                gp.CloseFigure()
            End If
            LineStart = New PointF(rc * Cos(start + sweep / 2), rc * Sin(start + sweep / 2))
            Dim rm As Single = R + ChannelWidth * (MaxLevel + 3)
            LineEnd = New PointF(rm * Cos(start + sweep / 2), rm * Sin(start + sweep / 2))
            Direction = start + sweep / 2
            Return gp
        Else
            Dim gp As New System.Drawing.Drawing2D.GraphicsPath
            Dim rc As Single = 0 - Channel * ChannelWidth
            Dim ri As Single = rc - ChannelWidth / 4
            Dim ro As Single = rc + ChannelWidth / 4
            Dim re As Single = rc + ChannelWidth / 2
            Dim rv As Single = rc - ChannelWidth / 2
            If Math.Abs(sweep / 360 * R) < ChannelWidth / 2 Then
                gp.AddLines(New PointF() {New PointF(-R + 2 * R * start / 360, ro), _
                                           New PointF(-R + 2 * R * (start + sweep * 2 / 3) / 360, ro), _
                                            New PointF(-R + 2 * R * (start + sweep) / 360, rc), _
                                              New PointF(-R + 2 * R * (start + sweep * 2 / 3) / 360, ri), _
                                              New PointF(-R + 2 * R * (start) / 360, ri)})
                gp.CloseFigure()
            Else
                Dim av As Single = Math.Sign(sweep) * ChannelWidth / 3
                gp.AddLines(New PointF() {New PointF(-R + 2 * R * start / 360, ro), _
                           New PointF(-R + 2 * R * (start + sweep) / 360 - av, ro), _
                New PointF(-R + 2 * R * (start + sweep) / 360, rc), _
                 New PointF(-R + 2 * R * (start + sweep) / 360 - av, ri), _
                  New PointF(-R + 2 * R * (start) / 360, ri)})
                gp.CloseFigure()
            End If
            LineStart = New PointF(-R + 2 * R * (start + sweep / 2) / 360, rc)
            Dim rm As Single = -ChannelWidth * (MaxLevel + 3)
            LineEnd = New PointF(-R + 2 * R * (start + sweep / 2) / 360, rm)
            Direction = 270
            Return gp
        End If
    End Function
    Public Function GetLargeRoundRectangle(ByVal R As Single, ByVal Channel As Integer, ByVal start As Single, ByVal sweep As Single) As System.Drawing.Drawing2D.GraphicsPath
        If IsCircular Then
            Dim gp As New System.Drawing.Drawing2D.GraphicsPath
            Dim rc As Single = R + Channel * ChannelWidth
            Dim ri As Single = rc - ChannelWidth / 2
            Dim ro As Single = rc + ChannelWidth / 2
            gp.AddArc(-ro, -ro, 2 * ro, 2 * ro, start, sweep)
            gp.AddLine(CSng(ro * Cos(start + sweep)), CSng(ro * Sin(start + sweep)), CSng(ri * Cos(start + sweep)), CSng(ri * Sin(start + sweep)))
            gp.AddArc(-ri, -ri, 2 * ri, 2 * ri, start + sweep, -sweep)
            gp.CloseFigure()
            LineStart = New PointF(rc * Cos(start + sweep / 2), rc * Sin(start + sweep / 2))
            Dim rm As Single = R + ChannelWidth * (MaxLevel + 3)
            LineEnd = New PointF(rm * Cos(start + sweep / 2), rm * Sin(start + sweep / 2))
            Direction = start + sweep / 2
            Return gp
        Else
            Dim gp As New System.Drawing.Drawing2D.GraphicsPath
            Dim rc As Single = 0 - Channel * ChannelWidth
            Dim ri As Single = rc - ChannelWidth / 2
            Dim ro As Single = rc + ChannelWidth / 2
            gp.AddLines(New PointF() {New PointF(-R + 2 * R * start / 360, ro), _
                       New PointF(-R + 2 * R * (start + sweep) / 360, ro), _
            New PointF(-R + 2 * R * (start + sweep) / 360, ri), _
             New PointF(-R + 2 * R * (start) / 360, ri)})
            gp.CloseFigure()
            LineStart = New PointF(-R + 2 * R * (start + sweep / 2) / 360, rc)
            Dim rm As Single = -ChannelWidth * (MaxLevel + 3)
            LineEnd = New PointF(-R + 2 * R * (start + sweep / 2) / 360, rm)
            Direction = 270
            Return gp
        End If
    End Function


    Private Function Sin(ByVal d As Single) As Single
        Return Math.Sin(d / 180 * Math.PI)
    End Function
    Private Function Cos(ByVal d As Single) As Single
        Return Math.Cos(d / 180 * Math.PI)
    End Function

    Public Function CompareTo(ByVal other As FeatureView) As Integer Implements System.IComparable(Of FeatureView).CompareTo
        Dim z As Integer
        If IsSource Or other.IsSource Then
            z = IIf(IsSource, -1, 0) + IIf(other.IsSource, 1, 0)
        Else
            If IsCircular Then
                'Dim delta = Math.Atan(Math.Abs((LineStart.Y / LineStart.X))) - Math.Atan(Math.Abs((other.LineStart.Y / other.LineStart.X)))
                Dim delta = Math.Abs(LineStart.Y) / (Math.Abs(LineStart.X) + 1) - Math.Abs(other.LineStart.Y) / (Math.Abs(other.LineStart.X) + 1)
                If delta = 0 Then
                    z = Math.Sign(Math.Abs(other.LineStart.X) - Math.Abs(LineStart.X))
                Else
                    z = Math.Sign(delta)
                End If
            Else
                z = Math.Sign(Math.Abs(LineStart.Y) - Math.Abs(other.LineStart.Y))
            End If
        End If
        Return z
    End Function
End Class

Public Class ExtRectF
    Public X As Single
    Public Y As Single
    Public Width As Single
    Public Height As Single
    Public Sub New()
    End Sub
    Public Sub New(ByVal vX As Single, ByVal vY As Single, ByVal vWidth As Single, ByVal vHeight As Single)
        X = vX
        Y = vY
        Width = vWidth
        Height = vHeight
    End Sub
    Public Property Location() As PointF
        Get
            Return New PointF(X, Y)
        End Get
        Set(ByVal value As PointF)
            X = value.X
            Y = value.Y
        End Set
    End Property
    Public Property North() As PointF
        Get
            Return New PointF(X + Width / 2, Y)
        End Get
        Set(ByVal value As PointF)
            X = value.X - Width / 2
            Y = value.Y
        End Set
    End Property
    Public Property South() As PointF
        Get
            Return New PointF(X + Width / 2, Y + Height)
        End Get
        Set(ByVal value As PointF)
            X = value.X - Width / 2
            Y = value.Y - Height
        End Set
    End Property
    Public Property Center() As PointF
        Get
            Return New PointF(X + Width / 2, Y + Height / 2)
        End Get
        Set(ByVal value As PointF)
            X = value.X - Width / 2
            Y = value.Y - Height / 2
        End Set
    End Property
    Public Property Size() As SizeF
        Get
            Return New SizeF(Width, Height)
        End Get
        Set(ByVal value As SizeF)
            Width = value.Width
            Height = value.Height
        End Set
    End Property
    Public Property Top() As Single
        Get
            Return Y
        End Get
        Set(ByVal value As Single)
            Y = value
        End Set
    End Property
    Public Property Bottom() As Single
        Get
            Return Y + Height
        End Get
        Set(ByVal value As Single)
            Y = value - Height
        End Set
    End Property
    Public Property Left() As Single
        Get
            Return X
        End Get
        Set(ByVal value As Single)
            X = value
        End Set
    End Property
    Public Property Right() As Single
        Get
            Return X + Width
        End Get
        Set(ByVal value As Single)
            X = value - Width
        End Set
    End Property



    Public Shared Narrowing Operator CType(ByVal RectF As ExtRectF) As RectangleF
        Return New RectangleF(RectF.X, RectF.Y, RectF.Width, RectF.Height)
    End Operator
    Public Shared Widening Operator CType(ByVal RectF As RectangleF) As ExtRectF
        Return New ExtRectF(RectF.X, RectF.Y, RectF.Width, RectF.Height)
    End Operator
End Class