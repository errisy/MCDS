Public Class PrimerAnalysisFrame
    Inherits PictureBox

    Private context As BufferedGraphicsContext '双缓冲buffer管理
    Private grafx As BufferedGraphics '双缓冲的buffer
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
        Try
            grafx.Render(pe.Graphics)

        Catch ex As Exception

        End Try
        MyBase.OnPaint(pe)
    End Sub
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        context = BufferedGraphicsManager.Current
        context.MaximumBuffer = New Size(Width + 1, Height + 1)
        grafx = context.Allocate(CreateGraphics(), New Rectangle(0, 0, Width + 1, Height + 1))
        grafx.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        myGraphics = CreateGraphics()
        grafx.Graphics.Clear(Color.White)
        'VSSeq.Maximum = Math.Max(0, pbSeq.Height - pnlSeq.Height) + 1
        If TempPAR.OK Then OnResultReturn(TempPAR)
        Draw()
    End Sub
    Private Sub Draw()
        Try
            grafx.Render(myGraphics)
        Catch ex As Exception

        End Try
    End Sub

    'Private MR As New TypeRecorder(Of Graphics)
    Private RegularFont As New Font("Calibri", 10)
    Private BoldFont As New Font("Calibri", 10, FontStyle.Bold)

    Private StreamLocation As PointF
    Private TopBorder As Single = 3
    Private BottomBorder As Single = 3
    Private LeftBorder As Single = 2
    Private LineSpacerRatio As Single = 0.8 'depends on the height of font
    Private PassageSpacerRatio As Single = 0.2
    Private TabSpacerRatio As Single = 1.2
    Private LeftLimit As Single
    Private PassageWidth As Single
    Private EqualWidthRatio As Single = 0.5
    Private SplitWidth As Single = 15

    Private Sub StartDraw()
        StreamLocation = New PointF(LeftBorder, TopBorder)
        LeftLimit = LeftBorder
        PassageWidth = LeftLimit
    End Sub
    Private Sub NextLine(ByVal vFont As Font)
        If (StreamLocation.Y + LineSpacerRatio * vFont.Height) > (Height - BottomBorder - LineSpacerRatio * vFont.Height) Then
            LeftLimit = PassageWidth + SplitWidth
            PassageWidth = LeftLimit
            StreamLocation.X = LeftLimit
            StreamLocation.Y = TopBorder
        Else
            StreamLocation.X = LeftLimit
            StreamLocation.Y += LineSpacerRatio * vFont.Height
        End If
    End Sub
    Private Sub NextPassage(ByVal vFont As Font)
        If (StreamLocation.Y + LineSpacerRatio * vFont.Height) > (Height - BottomBorder - LineSpacerRatio * vFont.Height) Then
            LeftLimit = PassageWidth + SplitWidth
            PassageWidth = LeftLimit
            StreamLocation.X = LeftLimit
            StreamLocation.Y = TopBorder
        Else
            StreamLocation.X = LeftLimit
            StreamLocation.Y += (LineSpacerRatio + PassageSpacerRatio) * vFont.Height
        End If
    End Sub
    Private Sub TabSpace(ByVal vFont As Font)
        StreamLocation.X += TabSpacerRatio * vFont.Height
        PassageWidth = Math.Max(StreamLocation.X, PassageWidth)
    End Sub
    Private Sub DrawString(ByVal Value As String, ByVal vFont As Font, ByVal vBrush As Brush)
        grafx.Graphics.DrawString(Value, vFont, vBrush, StreamLocation)
        StreamLocation.X += grafx.Graphics.MeasureString(Value, vFont).Width
        PassageWidth = Math.Max(StreamLocation.X, PassageWidth)
    End Sub
    Private Sub DrawEqualWidthChar(ByVal Value As String, ByVal vFont As Font, ByVal vBrush As Brush)
        grafx.Graphics.DrawString(Value, vFont, vBrush, StreamLocation)
        StreamLocation.X += vFont.Height * EqualWidthRatio
        PassageWidth = Math.Max(StreamLocation.X, PassageWidth)
    End Sub
    Private Sub TabTo(ByVal Value As Single)
        StreamLocation.X = LeftLimit + Value
        PassageWidth = Math.Max(StreamLocation.X, PassageWidth)
    End Sub

    Friend Sub OnResultReturn(ByVal PAR As Nuctions.PrimerAnalysisResult) ' Handles PA.ReturnResult
        Try

            StartDraw()

            grafx.Graphics.Clear(Color.White)
            Dim tab1 As Single
            Dim tab2 As Single
            Dim i As Integer

            For Each key As String In PAR.Primers.Keys
                tab1 = Math.Max(grafx.Graphics.MeasureString(key, BoldFont).Width, tab1)
            Next
            tab1 += TabSpacerRatio * RegularFont.Height
            tab2 = tab1 + TabSpacerRatio * RegularFont.Height + grafx.Graphics.MeasureString("(000)", BoldFont).Width + grafx.Graphics.MeasureString("000.00", BoldFont).Width

            DrawString("Hairpins", BoldFont, Brushes.Red)
            For Each key As String In PAR.Hairpins.Keys
                NextLine(RegularFont)
                DrawString(key, BoldFont, Brushes.Black)
                TabTo(tab1)
                DrawString("(" + PAR.Hairpins(key).Count.ToString + ")", BoldFont, Brushes.Green)
                TabSpace(RegularFont)

                If PAR.Hairpins(key).Count > 0 Then
                    Dim pmr As String = PAR.Primers(key)
                    Dim info As Nuctions.StructInfo = PAR.Hairpins(key)(0)
                    DrawString(PAR.Hairpins(key)(0).AG.ToString("0.00"), RegularFont, IIf(info.AG > 5, Brushes.Red, Brushes.Blue))
                    TabTo(tab2)
                    If vShowSequencing Then
                        For i = 0 To pmr.Length - 1
                            Select Case i
                                Case info.S1 To info.E1 - 1, info.S2 To info.E2 - 1
                                    DrawEqualWidthChar(pmr.Substring(i, 1), BoldFont, Brushes.Red)
                                Case Else
                                    DrawEqualWidthChar(pmr.Substring(i, 1), RegularFont, Brushes.Navy)
                            End Select

                        Next
                    End If
                Else
                    DrawString(0.ToString("0.00"), RegularFont, Brushes.Blue)
                End If
            Next



            NextPassage(RegularFont)
            DrawString("Dimers", BoldFont, Brushes.Red)
            For Each key As String In PAR.Dimers.Keys
                NextLine(RegularFont)
                DrawString(key, BoldFont, Brushes.Black)
                TabTo(tab1)
                DrawString("(" + PAR.Dimers(key).Count.ToString + ")", BoldFont, Brushes.Green)
                TabSpace(RegularFont)
                If PAR.Dimers(key).Count > 0 Then
                    Dim pmr As String = PAR.Primers(key)
                    Dim info As Nuctions.StructInfo = PAR.Dimers(key)(0)
                    DrawString(PAR.Dimers(key)(0).AG.ToString("0.00"), RegularFont, IIf(info.AG > 5, Brushes.Red, Brushes.Blue))
                    TabTo(tab2)
                    If vShowSequencing Then
                        For i = 0 To pmr.Length - 1
                            Select Case i
                                Case info.S1 To info.E1 - 1, info.S2 To info.E2 - 1
                                    DrawEqualWidthChar(pmr.Substring(i, 1), BoldFont, Brushes.Red)
                                Case Else
                                    DrawEqualWidthChar(pmr.Substring(i, 1), RegularFont, Brushes.Navy)
                            End Select

                        Next
                    End If
                End If
            Next


            NextPassage(RegularFont)
            DrawString("CrossDimers", BoldFont, Brushes.Red)
            DrawString("(" + PAR.CrossDimers.Count.ToString + ")", RegularFont, Brushes.Green)
            If PAR.CrossDimers.Count > 0 Then
                Dim info As Nuctions.StructInfo = PAR.CrossDimers(0)
                NextLine(RegularFont)
                DrawString(info.K1, BoldFont, Brushes.Black)
                TabTo(tab1)
                DrawString(info.AG.ToString("0.00"), RegularFont, IIf(info.AG > 5, Brushes.Red, Brushes.Blue))
                TabTo(tab2)
                If vShowSequencing Then
                    For i = 0 To PAR.Primers(info.K1).Length - 1
                        Select Case i
                            Case info.S1 To info.E1 - 1
                                DrawEqualWidthChar(PAR.Primers(info.K1).Substring(i, 1), BoldFont, Brushes.Red)
                            Case Else
                                DrawEqualWidthChar(PAR.Primers(info.K1).Substring(i, 1), RegularFont, Brushes.Navy)
                        End Select
                    Next
                End If
                NextLine(RegularFont)
                DrawString(info.K2, BoldFont, Brushes.Black)
                TabTo(tab1)
                DrawString(info.AG.ToString("0.00"), RegularFont, IIf(info.AG > 5, Brushes.Red, Brushes.Blue))
                TabTo(tab2)
                If vShowSequencing Then
                    For i = 0 To PAR.Primers(info.K2).Length - 1
                        Select Case i
                            Case info.S2 To info.E2 - 1
                                DrawEqualWidthChar(PAR.Primers(info.K2).Substring(i, 1), BoldFont, Brushes.Red)
                            Case Else
                                DrawEqualWidthChar(PAR.Primers(info.K2).Substring(i, 1), RegularFont, Brushes.Navy)
                        End Select
                    Next
                End If
            End If




            NextPassage(RegularFont)
            DrawString("Products", BoldFont, Brushes.Red)
            DrawString("(" + PAR.Products.Count.ToString + ")", BoldFont, Brushes.Green)
            If PAR.Products.Count > 0 Then
                For i = 0 To PAR.Products.Count - 1
                    If i = 2 Then Exit For
                    NextLine(RegularFont)
                    DrawString("<" + i.ToString + ">", BoldFont, Brushes.Navy)
                    TabSpace(RegularFont)
                    DrawString(PAR.Products(i).Sequence.Length.ToString + "bp", BoldFont, Brushes.Red)
                Next
            End If
            NextPassage(RegularFont)


            DrawString("Priming", BoldFont, Brushes.Red)
            For Each key As String In PAR.Primings.Keys()
                NextLine(RegularFont)
                DrawString(key, BoldFont, Brushes.Black)
                TabTo(tab1)

                If PAR.Primings(key).Count > 0 Then
                    Dim pmr As String = PAR.Primers(key)
                    Dim info As Nuctions.PrimeInfo
                    For fi As Integer = 0 To PAR.Primings(key).Count - 1
                        If fi = 2 Then Exit For
                        DrawString("(" + (PAR.Primings(key).Count).ToString + ")", BoldFont, Brushes.Green)
                        TabSpace(RegularFont)
                        info = PAR.Primings(key)(fi)
                        DrawString(PAR.Primings(key)(fi).AG.ToString("0.00"), RegularFont, IIf(info.AG > 5, Brushes.Red, Brushes.Blue))
                        TabTo(tab2)
                        If vShowSequencing Then
                            For i = 0 To pmr.Length - 1
                                Select Case i
                                    Case info.S1 To info.E1 - 1, info.S2 To info.E2 - 1
                                        DrawEqualWidthChar(pmr.Substring(i, 1), BoldFont, Brushes.Red)
                                    Case Else
                                        DrawEqualWidthChar(pmr.Substring(i, 1), RegularFont, Brushes.Navy)
                                End Select
                            Next
                        End If
                        NextLine(RegularFont)
                        TabTo(tab1)
                    Next
                End If
            Next

        Catch ex As Exception

        End Try
        Draw()
    End Sub
    Private WithEvents PA As Nuctions.PrimerAnalyzer

    Private RunningTask As System.Threading.Tasks.Task(Of Nuctions.PrimerAnalysisResult)
    Private WaitingTask As System.Threading.Tasks.Task(Of Nuctions.PrimerAnalysisResult)

    Private TempPAR As Nuctions.PrimerAnalysisResult
    Public Async Sub AnalyzePrimers(ByVal Primers As Dictionary(Of String, String), ByVal Template As List(Of Nuctions.GeneFile), ByVal Na As Single, ByVal C As Single)

        PA = New Nuctions.PrimerAnalyzer

        'PA.Analyze(Primers, Template, Na, C)

        'WaitingTask = PA.AsyncAnalyze(Primers, Template, Na, C)
        Dim par As Nuctions.PrimerAnalysisResult
        'Do
        'If RunningTask Is Nothing OrElse RunningTask.Status = Threading.Tasks.TaskStatus.RanToCompletion Then
        '    RunningTask = WaitingTask
        '    WaitingTask = Nothing
        '    RunningTask.Start()
        '    par = Await RunningTask
        'Else
        '    Exit Sub
        'End If
        'Loop While WaitingTask IsNot Nothing
        Dim t = PA.AsyncAnalyze(Primers, Template, Na, C)
        t.Start()
        par = Await t
        TempPAR = par
        OnResultReturn(par)
        'SVmutex.WaitOne()
        'Dim bi As New System.Windows.Media.Imaging.BitmapImage()
        'bi.BeginInit()
        'Dim ms As New System.IO.MemoryStream
        'SV.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
        'bi.StreamSource = ms
        'bi.EndInit()
        'SVImage.Source = bi
    End Sub



    Private vShowSequencing As Boolean = True

    <System.ComponentModel.Category("选项"), System.ComponentModel.Browsable(True)> Public Property ShowSequencing() As Boolean
        Get
            Return vShowSequencing
        End Get
        Set(ByVal value As Boolean)
            vShowSequencing = value
        End Set
    End Property
 
End Class
