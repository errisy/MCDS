Friend Class SequenceViewer

    Private context As BufferedGraphicsContext '双缓冲buffer管理
    Private grafx As BufferedGraphics '双缓冲的buffer
    Private grfrec As TypeRecorder(Of Graphics)  '记录所有图片绘制过程用来在重绘时快速描绘
    Private myGraphics As Graphics '该空间的DC
    Protected _PrimerPairList As New PrimerPairList
    Protected _PrimerPairListModel As New PrimerPairListModel With {.SearchPrimers = AddressOf llCloneSelection_LinkClicked}
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
        iHostFind.Child = _PrimerPairList
        _PrimerPairList.DataContext = _PrimerPairListModel
    End Sub

    Public Sub Save()
        If FileAddress Is Nothing OrElse FileAddress = "" Then
            SaveAs()
        ElseIf FileAddress.StartsWith("tcp:\\") Then
            'tcp://user#
            SaveToServer()
        ElseIf FileAddress.ToLower.EndsWith(".gb") Then
            SaveAs()
        Else
            SaveTo(FileAddress)
        End If
    End Sub
    Public Sub SaveAs()
        If GeneFile.Name IsNot Nothing AndAlso GeneFile.Name.Length > 0 Then
            SettingEntry.SaveGeneDialog.FileName = GeneFile.Name
        ElseIf Not (FileAddress Is Nothing) And (FileAddress.ToLower.EndsWith(".gb") OrElse FileAddress.ToLower.EndsWith(".vct")) Then
            Dim fi As New System.IO.FileInfo(FileAddress)
            Try
                SettingEntry.SaveGeneDialog.InitialDirectory = fi.Directory.FullName
                SettingEntry.SaveGeneDialog.FileName = fi.Name.Substring(0, fi.Name.LastIndexOf("."))
            Catch ex As Exception

            End Try
        End If
        If SettingEntry.SaveGeneDialog.ShowDialog Then
            SaveTo(SettingEntry.SaveGeneDialog.FileName)
        End If
    End Sub
    Public Sub SaveToServer()
        If RemoteUserName Is Nothing OrElse RemoteUserName.Length = 0 OrElse RemotePassword Is Nothing OrElse RemotePassword.Length = 0 Then
            'frmMain.SaveSequenceViewerAsToServer(Me, ParentTab.Text)
        Else
            'frmMain.SaveSequenceViewerToServer(Me)
        End If
    End Sub
    Public Function SaveToBytes() As Byte()
        Dim DC As New Dictionary(Of String, Object) From {{"DNA", GeneFile}, {"Enzyme", RestrictionSites}}
        Return SettingEntry.SaveToZXMLBytes(DC)
    End Function
    Public Sub SaveTo(ByVal filename As String)
        Dim fl As String = filename.ToLower
        If fl.EndsWith(".vct") Then
            SettingEntry.SaveToZXML(New Dictionary(Of String, Object) From {{"DNA", GeneFile}, {"Enzyme", RestrictionSites}}, filename)
            FileAddress = filename
        ElseIf fl.EndsWith(".gb") Then
            GeneFile.WriteToFile(filename)
            'Dim fi As New IO.FileInfo(filename)
            'GeneFile.WriteIndexFile(fi.FullName.Substring(0, fi.FullName.LastIndexOf(".")) + ".idx")
            'If fi.FullName.IndexOf(SettingEntry.DatabasePath) > -1 Then
            '    'frmMain.RemoveDatabaseMenus()
            '    'frmMain.LoadDatabase()
            '    SettingEntry.UpdateDatabasePath()
            'End If
            FileAddress = filename
        End If
    End Sub
    Public Property ParentTab As TabPage

    '鼠标事件 用来处理拖拽和选中等等
    Private Selecting As Boolean = False

    '这两个是用来保存文件时使用的
    Public VectorName As String = ""

    Private vFileAddress As String = ""

    Friend Property FileAddress As String
        Get
            Return vFileAddress
        End Get
        Set(ByVal value As String)
            lbFileAddress.Text = String.Format("[File Location] - {0}", value)
            vFileAddress = value
        End Set
    End Property

    '在远程保存的过程中将最先尝试这个密码组合
    Friend RemoteUserName As String = ""
    Friend RemotePassword As String = ""
    '标记当前的保存中这个账户和密码是否有效
    Friend AccountWorking As Boolean = False

    Public ContentChanged As Boolean = False

    Private Sub SequenceViewer_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbSeq.MouseDown
        Dim EP As New Point(e.X / ScaleFactor, (e.Y + vsbSeq.Value) / ScaleFactor)
        Dim ln As Line
        Dim lStart As Integer = (LineLocator >> (EP.Y - 1)) - 2
        Dim lEnd As Integer = (LineLocator >> (EP.Y + 1)) + 2
        For i As Integer = lStart To lEnd
            If i < 0 Then Continue For
            If i >= LineLocator.Count Then Exit For
            ln = LineLocator.Values(i)

            If ln.Rect.Contains(EP) Then
                If ln.FRect.Contains(EP) And PrimerDesign Then
                    DrawFPrimer(GetPos(EP.X, Edgewidth) + ln.Sstart)
                ElseIf ln.RRect.Contains(EP) And PrimerDesign Then
                    DrawRPrimer(GetPos(EP.X, Edgewidth) + ln.Sstart)
                ElseIf ln.SRect.Contains(EP) Then
                    '选中序列
                    'TATA[XhoI][BamHI]
                    If e.Button = Windows.Forms.MouseButtons.Left Then
                        If ModifierKeys = Keys.Shift Then
                            'SelectStart
                            DrawSelect(SelectStart, GetPos(EP.X, Edgewidth) + ln.Sstart - 1)
                        Else
                            DrawSelect(GetPos(EP.X, Edgewidth) + ln.Sstart - 1, GetPos(EP.X, Edgewidth) + ln.Sstart - 1)
                            pbMap.SelectSequence(GetPos(EP.X, Edgewidth) + ln.Sstart - 1, GetPos(EP.X, Edgewidth) + ln.Sstart)
                            Selecting = True
                        End If
                    End If
                Else
                    '查找所有的工具
                    '搜索Features
                    For Each f As VecLineFeature In ln.Features
                        For Each rct As Rectangle In f.RectSection
                            If rct.Contains(EP) Then
                                '选中整个Feature
                                SelectedFeature = f.Feature
                                DrawSelect(f.Feature.StartPosition - 1, f.Feature.EndPosition - 1)
                                pbMap.SelectedFeature = SelectedFeature
                                pbMap.SelectSequence(f.Feature.StartPosition - 1, f.Feature.EndPosition)
                                Exit For
                            End If
                        Next
                    Next
                    For Each rez As RE In ln.REList
                        For Each rct As Rectangle In rez.RectSection
                            If rct.Contains(EP) Then
                                '选中整个Feature
                                DrawSelect(rez.REStart - 1, rez.REEnd - 2)
                                pbMap.SelectSequence(rez.REStart - 1, rez.REEnd)
                                Exit For
                            End If
                        Next
                    Next
                End If

            End If
        Next
        pnlSeq.Focus()
    End Sub

    Private Sub SequenceViewer_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbSeq.MouseMove
        Dim EP As New Point(e.X / ScaleFactor, (e.Y + vsbSeq.Value) / ScaleFactor)
        Dim ln As Line
        Dim lStart As Integer = (LineLocator >> (EP.Y - 1)) - 2
        Dim lEnd As Integer = (LineLocator >> (EP.Y + 1)) + 2
        For i As Integer = lStart To lEnd
            If i < 0 Then Continue For
            If i >= LineLocator.Count Then Exit For
            ln = LineLocator.Values(i)
            If ln.Rect.Contains(EP) Then
                If ln.FRect.Contains(EP) Then

                ElseIf ln.RRect.Contains(EP) Then

                ElseIf ln.SRect.Contains(EP) Then
                    '选中序列
                    'TATA[XhoI][BamHI]
                    If e.Button = Windows.Forms.MouseButtons.Left Then
                        If Selecting Then DrawSelect(SelectStart, GetPos(EP.X, Edgewidth) + ln.Sstart - 1) : pbMap.SelectSequence(SelectStart, GetPos(EP.X, Edgewidth) + ln.Sstart)
                    End If
                Else
                    '查找所有的工具

                End If
            End If
        Next
    End Sub

    Private Sub SequenceViewer_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbSeq.MouseUp
        Dim EP As New Point(e.X / ScaleFactor, (e.Y + vsbSeq.Value) / ScaleFactor)
        Dim ln As Line
        Dim lStart As Integer = (LineLocator >> (EP.Y - 1)) - 2
        Dim lEnd As Integer = (LineLocator >> (EP.Y + 1)) + 2
        For i As Integer = lStart To lEnd
            If i < 0 Then Continue For
            If i >= LineLocator.Count Then Exit For
            ln = LineLocator.Values(i)
            If ln.Rect.Contains(EP) Then
                If ln.FRect.Contains(EP) Then

                ElseIf ln.RRect.Contains(EP) Then

                ElseIf ln.SRect.Contains(EP) Then
                    '选中序列
                    'TATA[XhoI][BamHI]
                    If e.Button = Windows.Forms.MouseButtons.Left Then
                        DrawSelect(SelectStart, GetPos(EP.X, Edgewidth) + ln.Sstart - 1)
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
        Dim sl As Integer
        If SelEnd > SelStart Then
            sl = SelEnd - SelStart + 1
        ElseIf SelEnd = SelStart
            sl = 1
        Else
            sl = SelEnd + l - SelStart + 1
        End If
        lbSelection.Text = String.Format(": {0}-{1} [{2}bp]", (SelStart + 1).ToString, (SelEnd + 1).ToString, sl.ToString)

        SelectStart = SelStart
        SelectEnd = SelEnd
        Draw()
        'grafx.Render(myGraphics)
    End Sub
    Private Sub DrawChar(ByVal i As Integer, ByVal brs As Brush, ByVal l As Integer)
        If i > l - 1 Then Exit Sub
        Dim lf As Single = (i Mod LineCharCount) * CharWidth * 0.8 + ((i Mod LineCharCount) \ CharGroup) * (CharWidth / 3) + Edgewidth
        Static lr As Single = CharWidth * 0.8
        grafx.Graphics.FillRectangle(Brushes.White, New RectangleF(lf, lList(i \ LineCharCount).SRect.Y, lr, CharWidth * 2.4))
        grafx.Graphics.DrawString(vGeneFile.Sequence.Chars(i), vFont, brs, lf, lList(i \ LineCharCount).SRect.Y)
        grafx.Graphics.DrawString(vGeneFile.RCSequence.Chars(l - 1 - i), vFont, brs, lf, lList(i \ LineCharCount).SRect.Y + CharWidth * 1.2)
    End Sub
    Private Sub SequenceViewer_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pbSeq.Paint
        grafx.Render(e.Graphics)
    End Sub

    Private FPList As New List(Of Integer)
    Private RPList As New List(Of Integer)
    Private pmrFPos As Integer = -1
    Private pmrRPos As Integer = -1

    Public Sub DrawFPrimer(ByVal Pos As Integer, Optional ByVal length As Integer = -1)
        '计算出一个符合退火温度的引物
        Dim Seq As String = ""
        pmrFPos = Pos
        If PrimerDesign Then pbMap.DrawPCR(pmrFPos, pmrRPos)
        For Each i As Integer In FPList
            lList(i).ClearFPrimer(grafx.Graphics)
        Next
        FPList.Clear()

        If length < 0 Then
            For i As Integer = 10 To 30
                If Pos - 1 + i > vGeneFile.Sequence.Length Then
                    Seq = vGeneFile.Sequence.Substring(Pos - 1) + vGeneFile.Sequence.Substring(0, Pos - 1 + i - vGeneFile.Sequence.Length)
                Else
                    Seq = vGeneFile.Sequence.Substring(Pos - 1, i)
                End If
                If Nuctions.CalculateTm(Seq, nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm > nudTMFrom.Value Then Exit For
            Next
        Else
            Dim i As Integer = length
            If Pos - 1 + i > vGeneFile.Sequence.Length Then
                Seq = vGeneFile.Sequence.Substring(Pos - 1) + vGeneFile.Sequence.Substring(0, Pos - 1 + i - vGeneFile.Sequence.Length)
            Else
                Seq = vGeneFile.Sequence.Substring(Pos - 1, i)
            End If
        End If

        Dim eos As Integer = Pos + Seq.Length - 1
        Dim l As Integer = Pos \ LineCharCount - IIf((Pos Mod LineCharCount) = 0, 1, 0)

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

    Private Sub RedrawFPrimer(ByVal Pos As Integer, ByVal Seq As String)
        Dim eos As Integer = Pos + Seq.Length - 1
        Dim l As Integer = Pos \ LineCharCount - IIf((Pos Mod LineCharCount) = 0, 1, 0)
        If l < 0 Then l = lList.Count - 1
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

    Public Sub DrawRPrimer(ByVal Pos As Integer, Optional ByVal length As Integer = -1)
        '计算出一个符合退火温度的引物
        Dim Seq As String = ""
        pmrRPos = Pos
        If PrimerDesign Then pbMap.DrawPCR(pmrFPos, pmrRPos)
        Dim RPos As Integer = vGeneFile.Sequence.Length - Pos
        For Each i As Integer In RPList
            lList(i).ClearRPrimer(grafx.Graphics)
        Next
        RPList.Clear()

        If length < 0 Then
            For i As Integer = 10 To 30
                If RPos + i > vGeneFile.Sequence.Length Then
                    Seq = vGeneFile.RCSequence.Substring(RPos) + vGeneFile.RCSequence.Substring(0, RPos + i - vGeneFile.Sequence.Length)
                Else
                    Seq = vGeneFile.RCSequence.Substring(RPos, i)
                End If
                If Nuctions.CalculateTm(Seq, nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm > nudTMFrom.Value Then Exit For
            Next
        Else
            Dim i As Integer = length
            If RPos + i > vGeneFile.Sequence.Length Then
                Seq = vGeneFile.RCSequence.Substring(RPos) + vGeneFile.RCSequence.Substring(0, RPos + i - vGeneFile.Sequence.Length)
            Else
                Seq = vGeneFile.RCSequence.Substring(RPos, i)
            End If
        End If
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
    Private Sub RedrawRPrimer(ByVal Pos As Integer, ByVal Seq As String)
        Dim eos As Integer = Pos - Seq.Length + 1
        Dim l As Integer = Pos \ LineCharCount

        Dim z As Integer = Pos
        Dim zl As Integer
        tbR.Text = ParseAttachPrimer(tbR.Text) + Seq
        While z > eos
            If l < 0 Then l = lList.Count - 1 : z += vGeneFile.Sequence.Length : eos += vGeneFile.Sequence.Length
            zl = Math.Min(z - lList(l).Sstart + 1, z - eos + 1)
            If zl < 0 Then Exit While
            lList(l).DrawRPrimer(grafx.Graphics, z - lList(l).Sstart, Seq.Substring(0, zl))
            Seq = Seq.Substring(zl)
            z -= zl
            RPList.Add(l)
            l -= 1
        End While
        grafx.Render(myGraphics)
    End Sub

    Private Shared CharWidth As Integer = 12 ' 16
    Private vGeneFile As Nuctions.GeneFile
    Private Shared vFont As New Font("Arial", CharWidth, FontStyle.Bold, GraphicsUnit.Pixel)
    Private Shared nFont As New Font("Arial", CharWidth, FontStyle.Bold, GraphicsUnit.Pixel)

    Private vRE As List(Of String)

    Private enzSetting As Boolean = False

    Public Property RestrictionSites() As List(Of String)
        Get
            Return vRE
        End Get
        Set(ByVal value As List(Of String))
            vRE = value
            enzSetting = True
            Dim stb As New System.Text.StringBuilder
            For Each ez As String In vRE
                stb.Append(ez)
                stb.Append(" ")
            Next
            lbEnzymes.Text = stb.ToString
            tbEnzymes.Text = lbEnzymes.Text
            enzSetting = False
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
            tbDNAName.Text = vGeneFile.Name
            If Not (vGeneFile Is Nothing) Then
                'Dim l As Integer = vGeneFile.Sequence.Length
                'linenumber = l \ LineCharCount + 1
                'pbSeq.Width = pnlSeq.Width - 16
                'pbSeq.Height = pnlSeq.Height
                'Dim vHgt As Integer = ParseLines(vGeneFile)
                'vsbSeq.Maximum = If(vHgt - pbSeq.Height > 0, vHgt - pbSeq.Height, 0)
                'pbMap.GeneFile = vGeneFile
                Deploy()
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
        Public RectSection As New List(Of Rectangle)
        Public Separate As Boolean = False
        Public P1 As Point
        Public L1 As Integer
        Public P2 As Point
        Public L2 As Integer
    End Class


    Public Function ParseLines(ByVal GenFile As Nuctions.GeneFile) As Integer

        Dim l As Integer = GenFile.Sequence.Length

        Dim Y As Integer = 0

        lList.Clear()
        Dim nLine As Line


        '以下代码用于酶切位点的分析

        Dim ear As New Nuctions.EnzymeAnalysis.EnzymeAnalysisResult(vRE, GenFile)

        Dim eaList As List(Of Nuctions.EnzymeAnalysis) = ear.CutList

        eaList.Sort(New Nuctions.EnzymeAnalysis.EnzymeAnalysisResult.EnzymeAnalysisSorter)

        Dim maxLevel As Integer
        'Dim pLine As FeatureSection

        Edgewidth = vGeneFile.Sequence.Length.ToString.Length * CharWidth * 0.8 + 10

        For i As Integer = 0 To l \ LineCharCount
            nLine = New Line
            nLine.Sstart = 1 + i * LineCharCount
            nLine.Send = Math.Min(LineCharCount + i * LineCharCount, l)
            nLine.l = l
            nLine.Edgewidth = Edgewidth
            nLine.GF = GenFile
            If nLine.Send > l Then nLine.Send = l

            '添加Features
            Dim f As Nuctions.GeneAnnotation
            For k As Integer = 0 To GenFile.Features.Count - 1
                f = GenFile.Features(k)

                Dim vf As New VecLineFeature
                vf.Feature = f
                vf.FPen = New Pen(GetColor(k, GenFile.Features.Count))
                vf.GetX = AddressOf GetX
                vf.Edge = Edgewidth
                vf.RectList = IntersectFeature(f, nLine, l, Edgewidth, GenFile.Iscircular, GeneFile)
                If TypeOf f Is Nuctions.CompareAnnotation Then
                    Dim ca As Nuctions.CompareAnnotation = f
                    vf.Offset = IIf(f.Complement, -1, 1) * Math.Max(ca.MatchStart, ca.MatchEnd)
                End If

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
            '可以在此时进行排序
            nLine.Features.Sort(VecLineFeatureComparer.SharedInstance)
            '分析酶切位点

            Dim REalloc As Boolean(,) = New Boolean(vRE.Count - 1, 49) {}
            maxLevel = 0
            For Each rea As Nuctions.EnzymeAnalysis In eaList
                Dim RE As New RE
                RE.REName = rea.Enzyme.Name
                RE.REStart = rea.StartRec + 1
                RE.REEnd = rea.EndRec + 1
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

                '分析RE如何绘制的

            Next
            nLine.ENZLevel = maxLevel
            lList.Add(nLine)
            Y += CharWidth * (5.4 + 1.4 * (nLine.Features.Count + nLine.ENZLevel)) + 7
        Next
        Return Y
    End Function

    Public Shared Function Intersect(ByVal S1 As Integer, ByVal E1 As Integer, ByVal S2 As Integer, ByVal E2 As Integer, ByVal L As Integer, ByVal circular As Boolean) As Point()
        Dim fl As New List(Of FeatureSection)
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
    Public Class VecLineFeatureComparer
        Implements IComparer(Of VecLineFeature)
        Public Function Compare(ByVal x As VecLineFeature, ByVal y As VecLineFeature) As Integer Implements System.Collections.Generic.IComparer(Of VecLineFeature).Compare
            Return Math.Sign(y.Offset - x.Offset)
        End Function
        Public Shared SharedInstance As New VecLineFeatureComparer
    End Class
    Public Class FeatureSection
        ''' <summary>
        ''' 本行中的起点
        ''' </summary>
        ''' <remarks></remarks>
        Public X1 As Single
        ''' <summary>
        ''' 本行中的终点
        ''' </summary>
        ''' <remarks></remarks>
        Public X2 As Single
        ''' <summary>
        ''' 在基因文件当中的起点
        ''' </summary>
        ''' <remarks></remarks>
        Public N1 As Integer
        ''' <summary>
        ''' 在基因文件当中的终点
        ''' </summary>
        ''' <remarks></remarks>
        Public N2 As Integer
        Public F1 As Integer
        Public F2 As Integer
        ''' <summary>
        ''' 本行在基因文件中的起点
        ''' </summary>
        ''' <remarks></remarks>
        Public LineStart As Integer

        ''绘制氨基酸序列的工具
        Public AAs As New List(Of AminooAcidCode)
        'Public AAStart As Integer

    End Class
    Public Structure AminooAcidCode
        Public Index As Integer
        Public SequenceOffset As Integer
        Public Code As String
    End Structure

    ''' <summary>
    ''' 探测Feature和行的交集
    ''' </summary>
    ''' <param name="vFeature"></param>
    ''' <param name="vLine"></param>
    ''' <param name="L"></param>
    ''' <param name="vEdge"></param>
    ''' <param name="circular"></param>
    ''' <param name="gf"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IntersectFeature(ByVal vFeature As Nuctions.GeneAnnotation, ByVal vLine As Line, ByVal L As Integer, ByVal vEdge As Single, ByVal circular As Boolean, gf As Nuctions.GeneFile) As List(Of FeatureSection)

        'f.StartPosition, f.EndPosition + 1, nLine.Sstart, nLine.Send + 1
        Dim S1, E1, S2, E2 As Integer
        S1 = vFeature.StartPosition
        E1 = vFeature.EndPosition + 1
        S2 = vLine.Sstart
        E2 = vLine.Send + 1

        Dim fl As New List(Of FeatureSection)
        Dim offset As Integer = Integer.MinValue
        If TypeOf vFeature Is Nuctions.CompareAnnotation Then
            Dim CA As Nuctions.CompareAnnotation = vFeature
            offset = IIf(CA.Complement, -1, 1) * CA.MatchStart
        End If

        If E1 >= S1 Then
            Dim s As Integer
            Dim e As Integer
            s = Math.Max(S1, S2)
            e = Math.Min(E1, E2)
            If e > s Then
                Dim fs As New FeatureSection() With {.N1 = s, .N2 = e, .X1 = GetX(s - S2, vEdge), .X2 = GetX(e - S2, vEdge), .F1 = s - S1, .F2 = e - S1, .LineStart = S2}
                fl.Add(fs)
                'vFeature.GetSuqence
                If vFeature.Type.ToLower = "cds" Or vFeature.Type.ToLower = "gene" Then
                    If vFeature.Complement Then
                        'Start is the larger side
                        Dim delta As Integer = e - vFeature.StartPosition
                        Dim u As Integer = (delta \ 3) * 3 + vFeature.StartPosition
                        For aai As Integer = u To s + 1 Step -3
                            Dim CodeTriplet As String = Nuctions.ReverseComplement(gf.SubSequence(aai - 4, aai - 2))
                            If CodeTriplet IsNot Nothing AndAlso CodeTriplet.Length = 3 Then _
                            fs.AAs.Add(New AminooAcidCode With {.Code = SettingEntry.CodonCol.CodeTable(CodeTriplet).ShortName, .Index = aai - 1, .SequenceOffset = (vFeature.EndPosition + 1 - aai) \ 3 + 1})
                        Next
                    Else
                        Dim delta As Integer = s - vFeature.StartPosition
                        Dim u As Integer = (delta \ 3) * 3 + vFeature.StartPosition
                        For aai As Integer = u To e Step 3
                            Dim CodeTriplet As String = gf.SubSequence(aai - 1, aai + 1)
                            If CodeTriplet IsNot Nothing AndAlso CodeTriplet.Length = 3 Then _
                            fs.AAs.Add(New AminooAcidCode With {.Code = SettingEntry.CodonCol.CodeTable(CodeTriplet).ShortName, .Index = aai, .SequenceOffset = (aai - vFeature.StartPosition) \ 3 + 1})
                        Next
                    End If
                End If
            End If
        ElseIf circular Then
            Dim s As Integer
            Dim e As Integer
            s = Math.Max(0, S2)
            e = Math.Min(E1, E2)
            If e > s Then
                Dim fs As New FeatureSection() With {.N1 = s, .N2 = e, .X1 = GetX(s - S2, vEdge), .X2 = GetX(e - S2, vEdge), .F1 = s - S1, .F2 = e - S1, .LineStart = S2}
                fl.Add(fs)
                If vFeature.Type.ToLower = "cds" Or vFeature.Type.ToLower = "gene" Then
                    Dim sp As Integer = vFeature.StartPosition - gf.Length
                    If vFeature.Complement Then
                        'Start is the larger side
                        Dim delta As Integer = e - sp
                        Dim u As Integer = (delta \ 3) * 3 + sp
                        For aai As Integer = u To s Step -3
                            Dim CodeTriplet As String = Nuctions.ReverseComplement(gf.SubSequence(aai - 4, aai - 2))
                            If CodeTriplet IsNot Nothing AndAlso CodeTriplet.Length = 3 Then _
                            fs.AAs.Add(New AminooAcidCode With {.Code = SettingEntry.CodonCol.CodeTable(CodeTriplet).ShortName, .Index = aai - 1, .SequenceOffset = (vFeature.EndPosition + 1 - aai) \ 3 + 1})
                        Next
                    Else
                        Dim delta As Integer = s - sp
                        Dim u As Integer = (delta \ 3) * 3 + sp
                        For aai As Integer = u To e Step 3
                            Dim CodeTriplet As String = gf.SubSequence(aai - 1, aai + 1)
                            If CodeTriplet IsNot Nothing AndAlso CodeTriplet.Length = 3 Then _
                            fs.AAs.Add(New AminooAcidCode With {.Code = SettingEntry.CodonCol.CodeTable(CodeTriplet).ShortName, .Index = aai, .SequenceOffset = (aai - sp) \ 3 + 1})
                        Next
                    End If
                End If
            End If
            s = Math.Max(S1, S2)
            e = Math.Min(L, E2)
            If e > s Then
                Dim fs As New FeatureSection() With {.N1 = s, .N2 = e, .X1 = GetX(s - S2, vEdge), .X2 = GetX(e - S2, vEdge), .F1 = s - S1, .F2 = e - S1, .LineStart = S2}
                fl.Add(fs)
                If vFeature.Type.ToLower = "cds" Or vFeature.Type.ToLower = "gene" Then
                    If vFeature.Complement Then
                        'Start is the larger side
                        Dim delta As Integer = e - vFeature.StartPosition
                        Dim u As Integer = (delta \ 3) * 3 + vFeature.StartPosition
                        For aai As Integer = u To s Step -3
                            Dim CodeTriplet As String = Nuctions.ReverseComplement(gf.SubSequence(aai - 4, aai - 2))
                            If CodeTriplet IsNot Nothing AndAlso CodeTriplet.Length = 3 Then _
                            fs.AAs.Add(New AminooAcidCode With {.Code = SettingEntry.CodonCol.CodeTable(CodeTriplet).ShortName, .Index = aai - 1, .SequenceOffset = (vFeature.EndPosition + 1 - aai) \ 3 + 1})
                        Next
                    Else
                        Dim delta As Integer = s - vFeature.StartPosition
                        Dim u As Integer = (delta \ 3) * 3 + vFeature.StartPosition
                        For aai As Integer = u To e Step 3
                            Dim CodeTriplet As String = gf.SubSequence(aai - 1, aai + 1)
                            If CodeTriplet IsNot Nothing AndAlso CodeTriplet.Length = 3 Then _
                            fs.AAs.Add(New AminooAcidCode With {.Code = SettingEntry.CodonCol.CodeTable(CodeTriplet).ShortName, .Index = aai, .SequenceOffset = (aai - vFeature.StartPosition) \ 3 + 1})
                        Next
                    End If
                End If
            End If
        End If
        'fl.Sort(New FeatureSectionComparer)
        Return fl
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

    Public Delegate Function CalX(ByVal n As Integer, ByVal Edgewidth As Integer) As Single


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


            For Each rr As RE In REList
                If rr.Separate Then

                    rr.RectSection.Add(New Rectangle(GetX((rr.P1.X - Sstart), Edgewidth), vY + (rr.L1 - 1) * CharWidth * 1.4, GetX((rr.P1.Y - Sstart), Edgewidth) - GetX((rr.P1.X - Sstart), Edgewidth), CharWidth * 1.4))
                    rr.RectSection.Add(New Rectangle(GetX((rr.P2.X - Sstart), Edgewidth), vY + (rr.L2 - 1) * CharWidth * 1.4, GetX((rr.P2.Y - Sstart), Edgewidth) - GetX((rr.P2.X - Sstart), Edgewidth), CharWidth * 1.4))

                Else
                    rr.RectSection.Add(New Rectangle(GetX((rr.P1.X - Sstart), Edgewidth), vY + (rr.L1 - 1) * CharWidth * 1.4, GetX((rr.P1.Y - Sstart), Edgewidth) - GetX((rr.P1.X - Sstart), Edgewidth), CharWidth * 1.4))
                End If

            Next


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
            'Exit Sub
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
            g.DrawString(Sstart.ToString, vFont, Brushes.DarkBlue, 2, vY)
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
        'Public RectList As New List(Of PointF)
        Public RectList As New List(Of FeatureSection)
        Public RectSection As New List(Of Rectangle)
        Public GetX As CalX
        Public Edge As Single
        Public Offset As Integer

        '用来显示氨基酸序列的工具
        'Public AminoAcidMode As Boolean = False

        Public Sub Deploy(ByRef vY As Integer)
            Y = vY
            Dim Rect As Rectangle
            For Each rf As FeatureSection In RectList
                Rect = New Rectangle(rf.X1, Y, rf.X2 - rf.X1, CharWidth * 1.2)
                RectSection.Add(Rect)
            Next
            vY += CharWidth * 1.4
        End Sub
        Public Sub Draw(ByVal g As Graphics, ByRef vY As Integer)
            'Draw only when the veiw is between top and bottom.
            Y = vY
            Dim BackBrush As SolidBrush = Brushes.Transparent
            Dim FontFrowardBrush As SolidBrush = Brushes.Black
            Dim FontReverseBrush As SolidBrush = Brushes.Black
            Dim CA As Nuctions.CompareAnnotation
            Dim ds As Single
            Dim endline As Integer
            Dim SP As Integer
            Dim EP As Integer

            Select Case Feature.Type
                Case "match"
                    BackBrush = Brushes.Yellow
                    FontFrowardBrush = Brushes.Red
                    FontReverseBrush = Brushes.Green
                Case "miss"
                    BackBrush = Brushes.Silver
                    FontFrowardBrush = Brushes.DarkRed
                    FontReverseBrush = Brushes.DarkGreen
                Case Else
                    BackBrush = Brushes.White
            End Select

            Dim Rect As Rectangle
            For Each rf As FeatureSection In RectList
                Rect = New Rectangle(rf.X1, Y, rf.X2 - rf.X1, CharWidth * 1.2)

                g.FillRectangle(BackBrush, New Rectangle(rf.X1, Y, rf.X2 - rf.X1, CharWidth * 1.2))

                g.DrawRectangle(FPen, Rect)
                'RectSection.Add(Rect)
                If TypeOf Feature Is Nuctions.CompareAnnotation Then
                    '(i Mod LineCharCount) * CharWidth * 0.8 + ((i Mod LineCharCount) \ CharGroup) * (CharWidth / 3) + Edgewidth, vY
                    CA = Feature
                    'de = g.MeasureString(CA.EndPosition.ToString, vFont).Width
                    endline = Math.Min(Feature.Note.Length - 1, rf.F2 - 1)

                    If Feature.Complement Then
                        SP = CA.MatchEnd
                        EP = CA.MatchStart

                        For i As Integer = rf.F1 To endline
                            g.DrawString(Nuctions.ReverseComplementF(Feature.Note.Chars(Feature.Note.Length - 1 - i)), vFont, FontReverseBrush, GetX.Invoke(i - rf.F1 + rf.N1 - rf.LineStart, Edge), Y)
                        Next
                    Else
                        SP = CA.MatchStart
                        EP = CA.MatchEnd
                        For i As Integer = rf.F1 To endline
                            g.DrawString(Feature.Note.Chars(i), vFont, FontFrowardBrush, GetX.Invoke(i - rf.F1 + rf.N1 - rf.LineStart, Edge), Y)
                        Next
                    End If
                    ds = g.MeasureString(SP.ToString, vFont).Width
                    g.DrawString(SP.ToString, vFont, Brushes.Black, GetX.Invoke(rf.N1 - rf.LineStart, Edge) - ds, Y)
                    g.DrawString(EP.ToString, vFont, Brushes.Black, GetX.Invoke(endline - rf.F1 + rf.N1 - rf.LineStart + 1, Edge), Y)
                ElseIf Feature.Type.ToLower = "cds" Or Feature.Type.ToLower = "gene" Then
                    'g.DrawString(Feature.Label + " (" + Feature.Note + ") ", vFont, Brushes.DarkRed, rf.X1, Y)
                    'Feature.StartPosition()
                    Dim cBrush As Brush
                    cBrush = IIf(Feature.Complement, ReverseAminoBrush, ForwardAminoBrush)
                    For Each fs In RectList
                        For Each aa As AminooAcidCode In fs.AAs
                            g.DrawString(aa.Code, vFont, cBrush, GetX.Invoke(aa.Index - rf.LineStart, Edge), Y)
                            g.DrawString(aa.SequenceOffset.ToString, AminoFont, IndexBrush, GetX.Invoke(aa.Index - rf.LineStart, Edge) + 10, Y)
                        Next
                    Next
                    'g.DrawString(Feature.Label + " (" + Feature.Note + ") ", vFont, Brushes.YellowGreen, rf.X1, Y)
                Else
                    g.DrawString(Feature.Label + " (" + Feature.Note + ") ", vFont, Brushes.DarkRed, rf.X1, Y)
                End If
            Next
            vY += CharWidth * 1.4
        End Sub
        Private Shared ForwardAminoBrush As New SolidBrush(Color.FromArgb(162, 0, 128, 0))
        Private Shared ReverseAminoBrush As New SolidBrush(Color.FromArgb(162, 128, 0, 0))
        Private Shared IndexBrush As New SolidBrush(Color.FromArgb(162, 0, 0, 64))
        Private Shared AminoFont As New Font("Arial", 8, FontStyle.Regular, GraphicsUnit.Pixel)
        Public Sub DrawSelect(ByVal g As Graphics)
            For Each rf As FeatureSection In RectList
                g.FillRectangle(Brushes.Cyan, New Rectangle(rf.X1, Y, rf.X2 - rf.X1, CharWidth * 1.2))
                g.DrawRectangle(FPen, New Rectangle(rf.X1, Y, rf.X2 - rf.X1, CharWidth * 1.2))
                If TypeOf Feature Is Nuctions.CompareAnnotation Then
                    '(i Mod LineCharCount) * CharWidth * 0.8 + ((i Mod LineCharCount) \ CharGroup) * (CharWidth / 3) + Edgewidth, vY
                    For i As Integer = rf.F1 To rf.F2 - 1
                        g.DrawString(Feature.Note.Substring(i, 1), vFont, Brushes.DarkRed, GetX.Invoke(i - rf.LineStart, Edge), Y)
                    Next
                Else
                    g.DrawString(Feature.Label + " (" + Feature.Note + ") ", vFont, Brushes.Red, rf.X1, Y)
                End If
            Next
        End Sub
        Public Sub DrawDeSelect(ByVal g As Graphics)
            For Each rf As FeatureSection In RectList
                If Feature.Type = "match" Then
                    g.FillRectangle(Brushes.Yellow, New Rectangle(rf.X1, Y, rf.X2 - rf.X1, CharWidth * 1.2))
                ElseIf Feature.Type = "miss" Then
                    g.FillRectangle(Brushes.OrangeRed, New Rectangle(rf.X1, Y, rf.X2 - rf.X1, CharWidth * 1.2))
                Else
                    g.FillRectangle(Brushes.White, New Rectangle(rf.X1, Y, rf.X2 - rf.X1, CharWidth * 1.2))
                End If
                g.DrawRectangle(FPen, New Rectangle(rf.X1, Y, rf.X2 - rf.X1, CharWidth * 1.2))
                If TypeOf Feature Is Nuctions.CompareAnnotation Then
                    '(i Mod LineCharCount) * CharWidth * 0.8 + ((i Mod LineCharCount) \ CharGroup) * (CharWidth / 3) + Edgewidth, vY
                    For i As Integer = rf.F1 To rf.F2 - 2
                        g.DrawString(Feature.Note.Substring(i, 1), vFont, Brushes.DarkRed, GetX.Invoke(i - rf.LineStart, Edge), Y)
                    Next
                Else
                    g.DrawString(Feature.Label + " (" + Feature.Note + ") ", vFont, Brushes.Red, rf.X1, Y)
                End If
            Next
        End Sub
    End Class

    Public Shared Function GetStartPosition() As Integer

    End Function

    Public ScaleFactor As Single = 1
    Private LineLocator As New RegionLocator
    Public Sub Deploy()
        '计算控件宽度以决定显示多少内容
        Dim l As Integer = vGeneFile.Sequence.Length
        linenumber = l \ LineCharCount + 1
        pbSeq.Width = pnlSeq.Width - 16
        pbSeq.Height = pnlSeq.Height
        Dim vHgt As Integer = ParseLines(vGeneFile)
        vsbSeq.Maximum = If(vHgt - pbSeq.Height > 0, vHgt - pbSeq.Height, 0)
        pbMap.GeneFile = vGeneFile
        '我们默认是现实100bp/行
        '根据行数决定控件的高度
        LineLocator.Clear()
        Dim Y As Integer = 0
        If Not (vGeneFile Is Nothing) Then
            For Each vl As Line In lList
                vl.Deploy(Y)
                LineLocator.Add(vl)
            Next
        End If
        grafx.Render(myGraphics)
    End Sub
    Public Sub Draw()
        '计算控件宽度以决定显示多少内容
        'Exit Sub
        '我们默认是现实100bp/行
        '根据行数决定控件的高度
        pbSeq.Top = pnlSeq.VerticalScroll.Value
        grafx.Graphics.Clear(Color.White)
        grafx.Graphics.ResetTransform()
        grafx.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        grafx.Graphics.ScaleTransform(ScaleFactor, ScaleFactor)
        Dim Y As Integer = 0
        Dim vTop As Integer = vsbSeq.Value
        Dim vBottom As Integer = vsbSeq.Value + pbSeq.Height
        grafx.Graphics.TranslateTransform(0, -vTop)
        Dim sr As SequenceRegion
        If Not (vGeneFile Is Nothing) Then
            sr = New SequenceRegion(vGeneFile.Iscircular, vGeneFile.Length) With {.Start = SelectStart, .End = SelectEnd}
            Dim lStart As Integer = (LineLocator >> vTop) - 2
            Dim lEnd As Integer = (LineLocator >> vBottom) + 2
            For i As Integer = lStart To lEnd
                If i < 0 Then Continue For
                If i >= LineLocator.Count Then Exit For
                Dim vl As Line = LineLocator.Values(i)
                vl.Draw(grafx.Graphics, sr)
            Next
        End If
        If PrimerDesign Then
            If tbF.Text.Length > 0 Then
                Dim iPF As Integer = tbF.Text.IndexOf(">")
                If iPF > -1 Then
                    RedrawFPrimer(pmrFPos, tbF.Text.Substring(iPF + 1))
                Else
                    RedrawFPrimer(pmrFPos, tbF.Text)
                End If
            End If
            If tbR.Text.Length > 0 Then
                Dim iPR As Integer = tbR.Text.IndexOf(">")
                If iPR > -1 Then
                    RedrawRPrimer(pmrRPos, tbR.Text.Substring(iPR + 1))
                Else
                    RedrawRPrimer(pmrRPos, tbR.Text)
                End If
            End If

        End If
        grafx.Render(myGraphics)
    End Sub

    'True for forward
    Private SearchDirection As Boolean = True
    Private SearchIndex As Integer = -1
    Private SelectedFeature As Nuctions.GeneAnnotation
    Public Sub SearchSequence(ByVal seq As String)
        Dim Pattern As String = Nuctions.TAGCNFilter(seq)
        Dim DNAPattern As String = Nuctions.WildCardNucleotides2RegexPattern(Pattern)
        Dim pLength As String = Pattern.Length
        Dim gLength As Integer = vGeneFile.Length
        Dim reg As Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(DNAPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        'DrawFPrimer(1, 0)
        'DrawRPrimer(0, 0)

        If SearchDirection Then
            Dim targetN As String = vGeneFile.Sequence + IIf(vGeneFile.Iscircular, vGeneFile.Sequence.Substring(0, pLength - 1), "")
            If reg.IsMatch(targetN, SearchIndex + 1) Then
                SearchIndex = reg.Match(targetN, SearchIndex + 1).Index
            Else
                SearchIndex = -1
            End If
            'SearchIndex = vGeneFile.Sequence.IndexOf(Nuctions.TAGCFilter(seq), SearchIndex + 1)
            If SearchIndex > -1 Then
                DrawSelect(SearchIndex, (SearchIndex + pLength - 1) Mod gLength)
                ScrollTo(SearchIndex)
                'DrawFPrimer(SearchIndex + 1, seq.Length)
                pbMap.SelectSequence(SearchIndex, SearchIndex + pLength - 1)
            Else
                Dim targetR As String = vGeneFile.RCSequence + IIf(vGeneFile.Iscircular, vGeneFile.RCSequence.Substring(0, pLength - 1), "")
                If reg.IsMatch(targetR, SearchIndex + 1) Then
                    SearchIndex = reg.Match(targetR, SearchIndex + 1).Index
                Else
                    SearchIndex = -1
                End If
                SearchDirection = False
                If SearchIndex > -1 Then
                    DrawSelect(vGeneFile.Length - ((SearchIndex + pLength) Mod gLength), vGeneFile.Length - SearchIndex - 1)
                    ScrollTo(vGeneFile.Length - ((SearchIndex + pLength) Mod gLength))
                    'DrawRPrimer(vGeneFile.Length - SearchIndex, seq.Length)
                    pbMap.SelectSequence(vGeneFile.Length - ((SearchIndex + pLength) Mod gLength), vGeneFile.Length - SearchIndex - 1)
                End If
            End If
        Else
            Dim targetR As String = vGeneFile.RCSequence + IIf(vGeneFile.Iscircular, vGeneFile.RCSequence.Substring(0, pLength - 1), "")
            If reg.IsMatch(targetR, SearchIndex + 1) Then
                SearchIndex = reg.Match(targetR, SearchIndex + 1).Index
            Else
                SearchIndex = -1
            End If
            'SearchIndex = vGeneFile.RCSequence.IndexOf(Nuctions.TAGCFilter(seq), SearchIndex + 1)
            If SearchIndex > -1 Then
                DrawSelect(vGeneFile.Length - ((SearchIndex + pLength) Mod gLength), vGeneFile.Length - SearchIndex - 1)
                ScrollTo(vGeneFile.Length - ((SearchIndex + pLength) Mod gLength))
                'DrawRPrimer(vGeneFile.Length - SearchIndex, seq.Length)
                pbMap.SelectSequence(vGeneFile.Length - ((SearchIndex + pLength) Mod gLength), vGeneFile.Length - SearchIndex - 1)
            Else
                Dim targetN As String = vGeneFile.Sequence + IIf(vGeneFile.Iscircular, vGeneFile.Sequence.Substring(0, pLength - 1), "")
                If reg.IsMatch(targetN, SearchIndex + 1) Then
                    SearchIndex = reg.Match(targetN, SearchIndex + 1).Index
                Else
                    SearchIndex = -1
                End If
                SearchDirection = True
                If SearchIndex > -1 Then
                    DrawSelect(SearchIndex, (SearchIndex + pLength - 1) Mod gLength)
                    ScrollTo(SearchIndex)
                    'DrawFPrimer(SearchIndex + 1, seq.Length)
                    pbMap.SelectSequence(SearchIndex, (SearchIndex + pLength - 1) Mod gLength)
                End If
            End If
        End If

    End Sub

    Public Function AddFeature() As Nuctions.Feature
        Dim ga As New Nuctions.GeneAnnotation
        ga.StartPosition = SelectStart + 1
        ga.EndPosition = SelectEnd + 1
        ga.Label = "Feature"
        ga.Type = "misc_feature"
        Dim ff As New frmAddFeature
        SelectedFeature = ff.ShowDialog(ga, vGeneFile, Me.ParentForm)
        SelectedFeature.Vector = vGeneFile
        SelectedFeature.Feature = New Nuctions.Feature(SelectedFeature.Label, SelectedFeature.GetSuqence, SelectedFeature.Label, SelectedFeature.Type, SelectedFeature.Note)
        If Not (SelectedFeature Is Nothing) Then
            vGeneFile.Features.Add(SelectedFeature)
            Me.GeneFile = vGeneFile
            ContentChanged = True
        End If
        Return SelectedFeature.Feature
    End Function
    Public Function RemoveFeature() As Nuctions.Feature
        If SelectedFeature Is Nothing Then Return Nothing
        Dim ga = SelectedFeature
        vGeneFile.Features.Remove(SelectedFeature)
        Me.GeneFile = vGeneFile
        ContentChanged = True
        Return ga.Feature
    End Function
    Public Function ManageFeature() As Nuctions.GeneAnnotation
        'search a feature
        If Not (SelectedFeature Is Nothing) Then
            Dim _Annotation As Nuctions.GeneAnnotation = SelectedFeature
            Dim ff As New frmAddFeature
            SelectedFeature = ff.ShowDialog(_Annotation, vGeneFile, Me.ParentForm)
            Me.GeneFile = vGeneFile
            ContentChanged = True
            Return _Annotation
        End If
    End Function

    Private Sub SequenceViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pnlMap.HorizontalScroll.Enabled = True
        pnlMap.HorizontalScroll.Visible = True
        pnlMap.VerticalScroll.Enabled = True
        pnlMap.VerticalScroll.Visible = True
        pbSeq.Width = pnlMap.Width
    End Sub

    Private Sub pbSeq_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbSeq.MouseWheel, pnlSeq.MouseWheel, Me.MouseWheel
        Dim d As Integer = e.Delta / -10

        Dim oldV As Integer = vsbSeq.Value

        If d = 0 Then Exit Sub
        For delta As Integer = Math.Sign(d) * 6 To d Step Math.Sign(d) * 6
            If oldV + delta < 0 Then
                vsbSeq.Value = 0
                Draw()
                pbSeq.Refresh()
                Exit For
            ElseIf oldV + delta > vsbSeq.Maximum Then
                vsbSeq.Value = vsbSeq.Maximum
                Draw()
                pbSeq.Refresh()
                Exit For
            Else
                vsbSeq.Value = oldV + delta
                Draw()
                pbSeq.Refresh()
            End If
        Next

    End Sub
    Private Sub vsbSeq_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles vsbSeq.Scroll
        Draw()
        pbSeq.Refresh()
    End Sub

    Private Sub pbSeq_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbSeq.Resize

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

        infoF.Text = String.Format("A:{0} {1}nt B:{2} {3}nt", Nuctions.CalculateTm(tbF.Text, nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(tbF.Text).Length.ToString,
                                   Nuctions.CalculateTm(ParseInnerPrimer(tbF.Text), nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(ParseInnerPrimer(tbF.Text)).Length.ToString)

        AnaPrimers()
    End Sub

    Private Sub tbR_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbR.TextChanged
        infoR.Text = String.Format("A:{0} {1}nt B:{2} {3}nt", Nuctions.CalculateTm(tbR.Text, nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(tbR.Text).Length.ToString,
                                   Nuctions.CalculateTm(ParseInnerPrimer(tbR.Text), nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(ParseInnerPrimer(tbR.Text)).Length.ToString)
        AnaPrimers()
    End Sub


    Private Sub nudTMPara_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudNa.ValueChanged, nudC.ValueChanged
        If tbF.Text.Length > 0 Then
            infoF.Text = String.Format("A:{0} {1}nt B:{2} {3}nt", Nuctions.CalculateTm(tbF.Text, nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(tbF.Text).Length.ToString,
                                       Nuctions.CalculateTm(ParseInnerPrimer(tbF.Text), nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(ParseInnerPrimer(tbF.Text).Length.ToString))
            AnaPrimers()
        End If
        If tbR.Text.Length > 0 Then
            infoR.Text = String.Format("A:{0} {1}nt B:{2} {3}nt", Nuctions.CalculateTm(tbR.Text, nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(tbR.Text).Length.ToString,
                                       Nuctions.CalculateTm(ParseInnerPrimer(tbR.Text), nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm.ToString("0.0"), Nuctions.TAGCFilter(ParseInnerPrimer(tbR.Text).Length.ToString))
            AnaPrimers()
        End If
    End Sub

    '利用引物分析工具分析引物
    Dim oldAnalysis As System.Threading.Thread

    Public Sub AnaPrimers()
        If Not (oldAnalysis Is Nothing) AndAlso oldAnalysis.ThreadState = Threading.ThreadState.Running Then oldAnalysis.Abort()
        Dim thr As New System.Threading.Thread(AddressOf AsynAnalyze)
        thr.Start()
        oldAnalysis = thr
    End Sub
    Public Sub AsynAnalyze()
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
        SelectedFeature = e.Feature
        DrawSelect(e.Start, e.End)
        ScrollTo(e.Start)
    End Sub
    Private Sub ScrollTo(ByVal Value As Integer)
        If Value < 0 Then Value = 0
        If Value > vGeneFile.Sequence.Length Then Value = vGeneFile.Sequence.Length
        Dim i As Integer = (Value + 1) \ LineCharCount
        Dim v As Integer = lList(i).Y
        If v < 0 Then
            vsbSeq.Value = 0
        ElseIf v > vsbSeq.Maximum Then
            vsbSeq.Value = vsbSeq.Maximum
        Else
            vsbSeq.Value = v
        End If
        Draw()
        'ParentForm.Text = pbScroll.Location.ToString + pbScroll.Bounds.ToString + lList(i).Y.ToString
    End Sub

#Region "处理外部事件"
    Private PrimerDesign As Boolean = True
    Private Sub HidePrimer()
        pnlVec.Top = pafPrimer.Top + 3
        pnlVec.Height += pafPrimer.Height
        PrimerDesign = False
    End Sub
    Public Sub ViewMode()
        pnlPrimer.Visible = False
        llApplyDNAName.Visible = False
        HidePrimer()
        pafPrimer.Visible = False
    End Sub
    Public Sub ClearMode()
        pafPrimer.Visible = False
        HidePrimer()
        pnlPrimer.Visible = False
    End Sub
    Public Sub PCRMode(ByVal Primers As Dictionary(Of String, String))
        llFF.Visible = True
        llFR.Visible = True
        llSendF.Visible = True
        llSendR.Visible = True
        llPair.Visible = True
        llRF.Visible = True
        llRR.Visible = True
        llApplyDNAName.Visible = False
        llEnzymeSwitch.Visible = True
        pnlEnzymes.Visible = False
        iHostFind.Visible = True

        '搜索所有匹配项目
        Dim pmr As String

        Dim bnd As String
        Dim att As String
        Dim idx As Integer
        Dim found As Boolean


        For Each key As String In Primers.Keys
            pmr = Primers(key)
            found = False
            idx = pmr.LastIndexOf(">")
            If idx > -1 Then
                bnd = Nuctions.TAGCFilter(pmr.Substring(idx, pmr.Length - idx))
                att = Nuctions.TAGCFilter(pmr.Substring(0, idx))
                idx = vGeneFile.Sequence.IndexOf(bnd)
                If idx > -1 Then
                    found = True
                    DrawFPrimer(idx + 1, bnd.Length)
                    tbF.Text = att + ">" + bnd
                    tbFKey.Text = key
                End If
                idx = vGeneFile.RCSequence.IndexOf(bnd)
                If idx > -1 Then
                    found = True
                    DrawRPrimer(vGeneFile.Sequence.Length - idx, bnd.Length)
                    tbR.Text = att + ">" + bnd
                    tbRKey.Text = key
                End If
            End If
            If Not found Then
                For i As Integer = pmr.Length To 12 Step -1
                    bnd = Nuctions.TAGCFilter(pmr.Substring(pmr.Length - i, i))
                    att = Nuctions.TAGCFilter(pmr.Substring(0, i))
                    idx = vGeneFile.Sequence.IndexOf(bnd)
                    If idx > 0 Then
                        found = True
                        DrawFPrimer(idx, bnd.Length)
                        tbF.Text = att + ">" + bnd
                        tbFKey.Text = key
                    End If
                    idx = vGeneFile.RCSequence.IndexOf(bnd)
                    If idx > 0 Then
                        found = True
                        DrawRPrimer(idx, bnd.Length)
                        tbR.Text = att + ">" + bnd
                        tbRKey.Text = key
                    End If
                    If found Then Exit For
                Next
            End If
        Next
    End Sub

    Public Sub TryDrawFPrimer()
        Dim pmr = tbF.Text
        Dim bdi = tbF.Text.LastIndexOf(">"c)
        If bdi > -1 Then
            Dim bnd = tbF.Text.Substring(bdi + 1).ToUpper
            Dim Fseq As String = vGeneFile.Sequence + IIf(vGeneFile.Iscircular, vGeneFile.Sequence.Substring(0, bnd.Length - 1), "")
            Dim idx = Fseq.IndexOf(bnd)
            If idx > -1 Then DrawFPrimer(idx + 1, bnd.Length)
        End If
    End Sub
    Public Sub TryDrawRPrimer()
        Dim pmr = tbR.Text
        Dim bdi = tbR.Text.LastIndexOf(">"c)
        If bdi > -1 Then
            Dim bnd = tbR.Text.Substring(bdi + 1).ToUpper
            Dim Rseq As String = vGeneFile.RCSequence + IIf(vGeneFile.Iscircular, vGeneFile.RCSequence.Substring(0, bnd.Length - 1), "")
            Dim idx = Rseq.IndexOf(bnd)
            If idx > -1 Then DrawRPrimer(vGeneFile.Sequence.Length - idx, bnd.Length)
        End If
    End Sub
    Public Sub SelectMode()
        pnlPrimer.Visible = False
        llSelectWhole.Visible = True
        llSelectPart.Enabled = True
        pafPrimer.Visible = False
        llApplyDNAName.Visible = False
        HidePrimer()
    End Sub
#End Region


    Public SelectEvent As SelectEvent
    Public PCREvent As PCREvent

    Private Sub llSelectWhole_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llSelectWhole.LinkClicked
        SelectEvent(New SelectEventArgs(vGeneFile, "Whole"))
    End Sub

    Private Sub llSelectPart_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llSelectPart.LinkClicked
        Dim vG As Nuctions.GeneFile = vGeneFile.GetSubGeneFile(SelectStart, SelectEnd)
        vG.Name = vGeneFile.Name
        SelectEvent(New SelectEventArgs(vG, SelectStart.ToString + " - " + SelectEnd.ToString))
    End Sub



    Private Sub llFF_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llFF.LinkClicked
        PCREvent(New PCREventArgs(tbFKey.Text, tbF.Text, "F"))
    End Sub

    Private Sub llFR_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llFR.LinkClicked
        PCREvent(New PCREventArgs(tbFKey.Text, tbF.Text, "R"))
    End Sub

    Private Sub llRF_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llRF.LinkClicked
        PCREvent(New PCREventArgs(tbRKey.Text, tbR.Text, "F"))
    End Sub

    Private Sub llRR_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llRR.LinkClicked
        PCREvent(New PCREventArgs(tbRKey.Text, tbR.Text, "R"))
    End Sub

    Private Sub llPair_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llPair.LinkClicked
        PCREvent(New PCREventArgs(tbFKey.Text, tbF.Text, "F"))
        PCREvent(New PCREventArgs(tbRKey.Text, tbR.Text, "R"))

    End Sub
    Private SendingTarget As String = ""
    Private Sub llSendF_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llSendF.LinkClicked
        SendingTarget = "F"
        ShowSendMenu()
    End Sub
    Private Sub llSendR_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llSendR.LinkClicked
        SendingTarget = "R"
        ShowSendMenu()
    End Sub
    Public GetSequenceTargets As System.Func(Of List(Of DNASequence))
    Private Sub ShowSendMenu()
        If GetSequenceTargets.OK Then
            Dim dList As List(Of DNASequence) = GetSequenceTargets.Invoke
            cmsSendTo.Items.Clear()
            Dim mi As ToolStripMenuItem
            For Each ds As DNASequence In dList
                mi = New ToolStripMenuItem(ds.SequenceName)
                mi.Tag = ds
                mi.ToolTipText = "F"
                AddHandler mi.Click, AddressOf SendMenuClick
                cmsSendTo.Items.Add(mi)
            Next
            cmsSendTo.Show(Cursor.Position)
        End If
    End Sub
    Private Sub SendMenuClick(sender As Object, e As EventArgs)
        Dim mi As ToolStripMenuItem = sender
        Dim ds As DNASequence = mi.Tag
        Select Case SendingTarget
            Case "F"
                If tbF.Text.Contains(">") Then
                    Dim il As Integer = tbF.Text.LastIndexOf(">")
                    ds.Sequence = tbF.Text.Substring(il)
                Else
                    ds.Sequence = tbF.Text
                End If
            Case "R"
                If tbR.Text.Contains(">") Then
                    Dim il As Integer = tbR.Text.LastIndexOf(">")
                    ds.Sequence = tbR.Text.Substring(il)
                Else
                    ds.Sequence = tbR.Text
                End If
        End Select
    End Sub

    '外部功能指令 拷贝序列
    Public Function CopySelectedSequence() As String
        Return vGeneFile.SubSequence(SelectStart, SelectEnd)
    End Function

    Private Sub llClose_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llClose.LinkClicked
        If TypeOf Parent Is TabPage Then
            Dim tc As TabContainer = Parent.Parent
            ' Try Close TabPage
            If tc Is SettingEntry.MainUIWindow.tcMainHost Then
                RaiseEvent CloseTab(Me, New EventArgs)
            Else
                tc.TabPages.Remove(Parent)
            End If
        End If
    End Sub

    Public Event CloseTab(ByVal sender As Object, ByVal e As EventArgs)

    Private Sub pbSeq_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbSeq.Click

    End Sub

    Private Sub pnlSeq_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnlSeq.Resize
        'reset the buffer size to pnlSeq
        context = BufferedGraphicsManager.Current
        context.MaximumBuffer = New Size(pbSeq.Width + 1, pbSeq.Height + 1)
        grafx = context.Allocate(CreateGraphics(), New Rectangle(0, 0, pbSeq.Width + 1, pbSeq.Height + 1))
        grafx.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        myGraphics = pbSeq.CreateGraphics
        'VSSeq.Maximum = Math.Max(0, pbSeq.Height - pnlSeq.Height) + 1
        Draw()
    End Sub

    Private vPCRMode As Boolean = True
    Private Sub llEnzymeSwitch_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llEnzymeSwitch.LinkClicked
        vPCRMode = Not vPCRMode
        pnlPrimer.Visible = vPCRMode
        pnlEnzymes.Visible = Not vPCRMode
    End Sub

    Private Sub tbEnzymes_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbEnzymes.KeyDown
        If e.KeyCode = Keys.Enter Then
            enzSetting = True
            Dim str As String() = tbEnzymes.Text.ToLower.Split(" ")
            Dim stList As New List(Of String)
            Dim rList As New List(Of String)

            stList.AddRange(str)

            Dim stb As New System.Text.StringBuilder

            For Each key As Nuctions.RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
                If stList.IndexOf(key.Name.ToLower) > -1 Then
                    stb.Append(key.Name)
                    stb.Append(" ")
                    rList.Add(key.Name)
                End If
            Next
            lbEnzymes.Text = stb.ToString
            tbEnzymes.Text = lbEnzymes.Text
            vRE = rList
            pbMap.RestrictionSite = vRE
            Deploy()
            pbMap.GeneFile = vGeneFile
            Draw()
            enzSetting = False
            ContentChanged = True
        End If
    End Sub

    Private Sub tbEnzymes_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbEnzymes.TextChanged
        If enzSetting Then Exit Sub
        Dim str As String() = tbEnzymes.Text.ToLower.Split(" ")
        Dim stList As New List(Of String)
        stList.AddRange(str)

        Dim stb As New System.Text.StringBuilder

        For Each key As Nuctions.RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
            If stList.IndexOf(key.Name.ToLower) > -1 Then
                stb.Append(key.Name)
                stb.Append(" ")
            End If
        Next
        lbEnzymes.Text = stb.ToString
        If lbEnzymes.Text.Length = 0 Then
            lbEnzymes.Text = "[None]"
        End If
    End Sub

    Private Sub llApplyDNAName_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llApplyDNAName.LinkClicked
        GeneFile.Name = tbDNAName.Text
        If Not (ParentTab Is Nothing) Then ParentTab.Text = GeneFile.Name
        ContentChanged = True
        Deploy()
        Draw()
    End Sub

    Private Sub llCloneSelection_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        SearchNearestPrimer()
    End Sub




    Private Sub SearchNearestPrimer()
        Dim Pos As Integer
        Dim Na As Single = nudNa.Value * 0.001
        Dim C As Single = nudC.Value * 0.000000001

        Dim Fs As New List(Of Primer)
        Dim Rs As New List(Of Primer)

        Dim lt As Integer = vGeneFile.Length

        Dim pmr As Primer
        Dim IsLinear As Boolean = Not vGeneFile.Iscircular
        Dim Tm As Single = nudTMFrom.Value
        Dim WithIn As Integer = 50
        WithIn = nudWithin.Value
        Dim shortl As Integer = 14
        Dim longl As Integer = 35
        Dim li As Integer = 20
        Dim searchmode As Integer = 0

        Dim Seq As String
        Dim lastSeq As String
        Dim MaxHairpinF As String
        Dim MaxHairpinR As String
        Dim MaxDimerF As String
        Dim MaxDimerR As String

        Seq = ""
        lastSeq = ""
        MaxHairpinF = ""
        MaxHairpinR = ""
        MaxDimerF = ""
        MaxDimerR = ""
        'pmrFPos = Pos

        Dim distance As Integer
        distance = 0
        For Pos = SelectStart To SelectStart - WithIn Step -1
            If IsLinear And Pos < 0 Then Exit For
            Dim InitialSequence As String = vGeneFile.SubSequence(Pos, Pos + li)
            If InitialSequence.Length = 0 Then Continue For
            searchmode = 0
            While True
                Select Case searchmode
                    Case 0
                        lastSeq = vGeneFile.SubSequence(Pos, Pos + li)
                        searchmode = IIf(Nuctions.CalculateTm(lastSeq, Na, C).Tm >= Tm, 1, -1)
                    Case -1
                        li += 1
                        Seq = vGeneFile.SubSequence(Pos, Pos + li)
                        searchmode = IIf(Nuctions.CalculateTm(Seq, Na, C).Tm >= Tm, 1, -1)
                        If searchmode > 0 Then
                            Exit While
                        ElseIf li > longl Then
                            Exit While
                        Else
                            lastSeq = Seq
                        End If
                    Case 1
                        li -= 1
                        Seq = vGeneFile.SubSequence(Pos, Pos + li)
                        searchmode = IIf(Nuctions.CalculateTm(Seq, Na, C).Tm >= Tm, 1, -1)
                        If searchmode < 0 Then
                            Seq = lastSeq
                            Exit While
                        ElseIf li < shortl Then
                            Exit While
                        Else
                            lastSeq = Seq
                        End If
                End Select
            End While
            'For i As Integer = 10 To 30
            '    Seq = vGeneFile.SubSequence(Pos, Pos + i)
            '    If Nuctions.CalculateTm(Seq, Na, C).Tm > Tm Then Exit For
            'Next
            If MaxHairpinF.Length > 0 AndAlso MaxHairpinR.Length > 0 AndAlso MaxDimerF.Length > 0 AndAlso MaxDimerR.Length > 0 AndAlso
                 (Seq.Contains(MaxHairpinF) And Seq.Contains(MaxHairpinR)) AndAlso (Seq.Contains(MaxDimerF) And Seq.Contains(MaxDimerR)) Then Continue For
            pmr = New Primer With {.Pos = Pos, .Seq = Seq, .Score = Nuctions.PrimerAnalyzer.AnalyzeSinglePrimer(Seq, Na, C, MaxHairpinF, MaxHairpinR, MaxDimerF, MaxDimerR)}
            pmr.MaxHairpinF = MaxHairpinF
            pmr.MaxHairpinR = MaxHairpinR
            pmr.MaxDimerF = MaxDimerF
            pmr.MaxDimerR = MaxDimerR
            pmr.Distance = distance
            Fs.Add(pmr)
            distance += 1
        Next

        Fs.Sort()

        Seq = ""
        lastSeq = ""
        MaxHairpinF = ""
        MaxHairpinR = ""
        MaxDimerF = ""
        MaxDimerR = ""
        'pmrRPos = Pos

        Dim RPos As Integer = lt - Pos
        distance = 0
        For Pos = lt - SelectEnd To lt - SelectEnd - WithIn Step -1
            If IsLinear And Pos < 0 Then Exit For
            Dim InitialSequence As String = vGeneFile.SubRCSequence(Pos, Pos + li)
            If InitialSequence.Length = 0 Then Continue For
            searchmode = 0
            While True
                Select Case searchmode
                    Case 0
                        lastSeq = vGeneFile.SubRCSequence(Pos, Pos + li)
                        searchmode = IIf(Nuctions.CalculateTm(lastSeq, Na, C).Tm >= Tm, 1, -1)
                    Case -1
                        li += 1
                        Seq = vGeneFile.SubRCSequence(Pos, Pos + li)
                        searchmode = IIf(Nuctions.CalculateTm(Seq, Na, C).Tm >= Tm, 1, -1)
                        If searchmode > 0 Then
                            Exit While
                        ElseIf li > longl Then
                            Exit While
                        Else
                            lastSeq = Seq
                        End If
                    Case 1
                        li -= 1
                        Seq = vGeneFile.SubRCSequence(Pos, Pos + li)
                        searchmode = IIf(Nuctions.CalculateTm(Seq, Na, C).Tm >= Tm, 1, -1)
                        If searchmode < 0 Then
                            Seq = lastSeq
                            Exit While
                        ElseIf li < shortl Then
                            Exit While
                        Else
                            lastSeq = Seq
                        End If
                End Select
            End While
            'For i As Integer = 10 To 30
            '    Seq = vGeneFile.SubRCSequence(Pos, Pos + i)
            '    If Nuctions.CalculateTm(Seq, nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm > Tm Then Exit For
            'Next
            If MaxHairpinF.Length > 0 AndAlso MaxHairpinR.Length > 0 AndAlso MaxDimerF.Length > 0 AndAlso MaxDimerR.Length > 0 AndAlso
                 (Seq.Contains(MaxHairpinF) And Seq.Contains(MaxHairpinR)) AndAlso (Seq.Contains(MaxDimerF) And Seq.Contains(MaxDimerR)) Then Continue For
            pmr = New Primer With {.Pos = Pos, .Seq = Seq, .Score = Nuctions.PrimerAnalyzer.AnalyzeSinglePrimer(Seq, Na, C, MaxHairpinF, MaxHairpinR, MaxDimerF, MaxDimerR)}
            pmr.MaxHairpinF = MaxHairpinF
            pmr.MaxHairpinR = MaxHairpinR
            pmr.MaxDimerF = MaxDimerF
            pmr.MaxDimerR = MaxDimerR
            pmr.Distance = distance
            Rs.Add(pmr)
            distance += 1
        Next

        Rs.Sort()

        Dim cnt As Integer
        cnt = 20
        For Each pm As Primer In Fs
            cnt += IIf(pm.Score = 30.0F, 1, 0)
        Next
        Dim MF As Integer = Math.Min(Fs.Count - 1, cnt)
        cnt = 20
        For Each pm As Primer In Rs
            cnt += IIf(pm.Score = 30.0F, 1, 0)
        Next
        Dim MR As Integer = Math.Min(Rs.Count - 1, cnt)

        Dim PPs As New List(Of PrimerPair)
        For fi As Integer = 0 To MF
            For ri As Integer = 0 To MR
                PPs.Add(New PrimerPair(Fs(fi), Rs(ri), Na, C))
            Next
        Next

        PPs.Sort()
        _PrimerPairListModel.PrimerPairs.Clear()
        If PPs.Count > 0 Then
            For Each pair In PPs
                Dim ppm As New PrimerPairModel With {.UseF = AddressOf UseF, .UseR = AddressOf UseR, .UsePair = AddressOf UsePair}
                ppm.FSequence = pair.F.Seq
                ppm.FLength = pair.F.Seq.Length
                ppm.FTm = Nuctions.CalculateTm(pair.F.Seq, Na, C).Tm
                ppm.FDimer = Nuctions.CalculateTm(pair.F.MaxDimerF, Na, C).dG
                ppm.FHairpin = Nuctions.CalculateTm(pair.F.MaxHairpinF, Na, C).dG
                ppm.FDistance = pair.F.Distance
                ppm.RSequence = pair.R.Seq
                ppm.RLength = pair.R.Seq.Length
                ppm.RTm = Nuctions.CalculateTm(pair.R.Seq, Na, C).Tm
                ppm.RDimer = Nuctions.CalculateTm(pair.R.MaxDimerR, Na, C).dG
                ppm.RHairpin = Nuctions.CalculateTm(pair.R.MaxHairpinR, Na, C).dG
                ppm.RDistance = pair.R.Distance
                ppm.CrossDimer = pair.CrossDimer
                _PrimerPairListModel.PrimerPairs.Add(ppm)
            Next

            Dim pp As PrimerPair = PPs(0)
            pmrFPos = pp.F.Pos Mod lt
            If pmrFPos < 0 Then pmrFPos += lt
            pmrRPos = pp.R.Pos Mod lt
            If pmrRPos < 0 Then pmrRPos += lt
            '绘制两条引物
            If PrimerDesign Then pbMap.DrawPCR(pmrFPos + 1, lt - pmrRPos)

            For Each i As Integer In FPList
                lList(i).ClearFPrimer(grafx.Graphics)
            Next
            FPList.Clear()
            For Each i As Integer In RPList
                lList(i).ClearRPrimer(grafx.Graphics)
            Next
            RPList.Clear()
            Dim eos As Integer
            Dim l As Integer
            Dim z As Integer
            Dim zl As Integer

            Seq = pp.F.Seq
            Pos = pmrFPos + 1
            eos = Pos + Seq.Length - 1
            l = Pos \ LineCharCount - IIf((Pos Mod LineCharCount) = 0, 1, 0)
            If l < 0 Then l = lList.Count - 1
            z = Pos
            zl = 0
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
            pmrFPos = Pos

            Seq = pp.R.Seq
            Pos = lt - pmrRPos
            eos = Pos - Seq.Length + 1
            l = Pos \ LineCharCount
            z = Pos
            zl = 0
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
            pmrRPos = Pos
            grafx.Render(myGraphics)
        End If
    End Sub

    Private Sub UseF(value As PrimerPairModel)
        Dim Findex As Integer = tbF.Text.LastIndexOf(">"c)
        tbF.Text = tbF.Text.Substring(0, Findex + 1) + value.FSequence
        TryDrawFPrimer()
        grafx.Render(myGraphics)
    End Sub
    Private Sub UseR(value As PrimerPairModel)
        Dim Rindex As Integer = tbR.Text.LastIndexOf(">"c)
        tbR.Text = tbR.Text.Substring(0, Rindex + 1) + value.RSequence
        TryDrawRPrimer()
        grafx.Render(myGraphics)
    End Sub
    Private Sub UsePair(value As PrimerPairModel)
        Dim Findex As Integer = tbF.Text.LastIndexOf(">"c)
        tbF.Text = tbF.Text.Substring(0, Findex + 1) + value.FSequence
        TryDrawFPrimer()
        Dim Rindex As Integer = tbR.Text.LastIndexOf(">"c)
        tbR.Text = tbR.Text.Substring(0, Rindex + 1) + value.RSequence
        TryDrawRPrimer()
        grafx.Render(myGraphics)
    End Sub
    Private Sub snbPixelPerKBP_ValueChanged(sender As Object, e As EventArgs) Handles snbPixelPerKBP.ValueChanged
        If Me.vGeneFile Is Nothing Then Return
        Dim gf As Nuctions.GeneFile = Me.vGeneFile
        pnlMap.Controls.Remove(pbMap)
        If snbPixelPerKBP.Value > 0 Then
            'Dim gf As Nuctions.GeneFile = di.DNAs(1)
            If gf.Iscircular Then
                pbMap.Width = gf.Length / Math.PI / 1000 * snbPixelPerKBP.Value
            Else
                pbMap.Width = gf.Length / 1000 * snbPixelPerKBP.Value
            End If
        Else
            pbMap.Width = 300
        End If
        pbMap.GeneFile = vGeneFile
        pnlMap.Controls.Add(pbMap)
        pbMap.Top = 0
        pbMap.Left = 0
    End Sub

End Class
