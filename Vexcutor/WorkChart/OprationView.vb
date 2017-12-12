Public Class OperationView
    Implements System.ComponentModel.INotifyPropertyChanged

    Private smList As New List(Of ChartItem)
    Private EnzymeSites As New List(Of String)
    Private vFeatures As New List(Of Nuctions.Feature)
    Private vHosts As New List(Of Nuctions.Host)

    Public Menu As ContextMenuStrip
    Public Sub Reload(DNA As DNAInfo)
        For Each ci In smList
            If ci.MolecularInfo Is DNA Then ci.Reload(DNA, EnzymeSites)
        Next
    End Sub
    Public Property Hosts As List(Of Nuctions.Host)
        Get
            Return vHosts
        End Get
        Set(value As List(Of Nuctions.Host))
            vHosts = value
        End Set
    End Property

    Public Property ScaleValue() As Single
        Get
            Return vScale
        End Get
        Set(ByVal value As Single)
            vScale = value
        End Set
    End Property

    Public Property Offset() As PointF
        Get
            Return New PointF(offsetX, offsetY)
        End Get
        Set(ByVal value As PointF)
            offsetX = value.X
            offsetY = value.Y
        End Set
    End Property

    Public ReadOnly Property SelectedItems() As List(Of ChartItem)
        Get
            Return vSelectedItems
        End Get
    End Property

    Public ReadOnly Property Items() As List(Of ChartItem)
        Get
            Return smList
        End Get
    End Property
    Public Sub RemoveItems(dItems As IList(Of ChartItem))
        Dim SelectionChanged As Boolean = False
        For Each ci In dItems.ToArray
            Remove(ci)
            If vSelectedItems.Contains(ci) Then vSelectedItems.Remove(ci) : SelectionChanged = True
        Next
        If SelectionChanged Then OnSelectedIndexChanged()
    End Sub
    Public ReadOnly Property EnzymeCol() As List(Of String)
        Get
            Return EnzymeSites
        End Get
    End Property

    Public ReadOnly Property Features As List(Of Nuctions.Feature)
        Get
            Return vFeatures
        End Get
    End Property

    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property Primers As List(Of PrimerInfo) = New List(Of PrimerInfo)
    Public Sub PrintPage(PrintPageList As List(Of PrintPage), Optional Direct As Boolean = False)
        If PrintPageList.Count = 0 Then Exit Sub
        If Direct Then
            pDoc.PrinterSettings = prtDiag.PrinterSettings
            Dim vList As New List(Of PrintPage)
            vList.AddRange(PrintPageList)
            vList.Sort()
            OnPrintingQueue.Clear()
            For Each pp As PrintPage In vList
                OnPrintingQueue.Add(pp)
            Next
            pDoc.Print()
        ElseIf Direct OrElse prtDiag.ShowDialog() = DialogResult.OK Then
            pDoc.PrinterSettings = prtDiag.PrinterSettings
            Dim vList As New List(Of PrintPage)
            vList.AddRange(PrintPageList)
            vList.Sort()
            OnPrintingQueue.Clear()
            For Each pp As PrintPage In vList
                OnPrintingQueue.Add(pp)
            Next
            pDoc.Print()
        End If
    End Sub
    Public Sub DeletePage(PrintPageList As List(Of PrintPage))
        If PrintPageList.Count = 0 Then Exit Sub
        Dim SelectionChanged As Boolean = False
        For Each pPage In PrintPageList.ToArray
            PrintPages.Remove(pPage)
            If vSelectedPages.Contains(pPage) Then
                vSelectedPages.Remove(pPage)
                SelectionChanged = True
            End If
        Next
        If SelectionChanged Then OnSelectedPrintPageChanged()
        Draw()
    End Sub
    Private prtDiag As New System.Windows.Forms.PrintDialog
    Private WithEvents pDoc As New System.Drawing.Printing.PrintDocument
    Private OnPrintingQueue As New List(Of PrintPage)
    Private PageNumber As Integer = 0
    Private Sub pDoc_BeginPrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles pDoc.BeginPrint
        PageNumber = 0
    End Sub

    Private Sub Doc_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles pDoc.PrintPage
        Dim pp As PrintPage = OnPrintingQueue(PageNumber)
        Dim dnaList As New List(Of DNAInfo)
        Dim ciList As New List(Of ChartItem)

        Dim sl As Single = e.PageSettings.PaperSize.Width / pp.Width

        e.PageSettings.Margins = New System.Drawing.Printing.Margins(pp.LeftSpace / Inch * 100, pp.RightSpace / Inch * 100, pp.TopSpace / Inch * 100, pp.BottomSpace / Inch * 100)

        Dim PW As Single = pp.PageWidth / Inch * pp.DPI
        Dim PH As Single = pp.PageHeight / Inch * pp.DPI
        Dim sclf As Single = pp.Width / PW

        Dim UW As Single = (pp.PageWidth - pp.LeftSpace - pp.RightSpace) / Inch * pp.DPI * sclf
        Dim UH As Single = (pp.PageHeight - pp.TopSpace - pp.BottomSpace) / Inch * pp.DPI * sclf

        Dim LS As Single = pp.LeftSpace / Inch * pp.DPI * sclf
        Dim TS As Single = pp.TopSpace / Inch * pp.DPI * sclf

        Dim g As Graphics = e.Graphics
        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBilinear
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.ScaleTransform(sl, sl)
        g.TranslateTransform(-pp.Left, -pp.Top)

        For Each ci As ChartItem In Items
            If pp.IsVisible(ci) Then
                ciList.Add(ci)
                dnaList.Add(ci.MolecularInfo)
            End If
        Next
        For Each ci As ChartItem In ciList
            ci.Draw(g, True, False)
        Next
        For Each ci As ChartItem In ciList
            ci.DrawArrow(g, dnaList)
        Next
        pp.Draw(g, vScale, False)
        PageNumber += 1
        e.HasMorePages = (PageNumber < OnPrintingQueue.Count)
    End Sub
    Private Sub Doc_EndPrint(sender As Object, e As Printing.PrintEventArgs) Handles pDoc.EndPrint

    End Sub

    Public Sub SummarziePirmers(cItem As ChartItem, Dict As Dictionary(Of String, String))
        Dim nPrimers As New List(Of PrimerInfo)
        For Each pi As PrimerInfo In Primers
            If pi.UserCreated Then nPrimers.Add(pi)
        Next
        Primers.Clear()
        Dim PrimerDict As New Dictionary(Of String, String)

        Dim ErrorMessage As New System.Text.StringBuilder

        For Each ci As ChartItem In Items
            Select Case ci.MolecularInfo.MolecularOperation
                Case Nuctions.MolecularOperationEnum.PCR
                    Dim pf As New PrimerInfo With {.Name = ci.MolecularInfo.PCR_FPrimerName, .Sequence = ci.MolecularInfo.PCR_ForwardPrimer, .NeedSynthesis = True}
                    Dim pr As New PrimerInfo With {.Name = ci.MolecularInfo.PCR_RPrimerName, .Sequence = ci.MolecularInfo.PCR_ReversePrimer, .NeedSynthesis = True}
                    If ci.MolecularInfo.PCR_ReversePrimer IsNot Nothing AndAlso ci.MolecularInfo.PCR_ForwardPrimer IsNot Nothing Then
                        If (pf.Name IsNot Nothing AndAlso pf.Name.Length > 0) AndAlso (pf.Sequence IsNot Nothing And pf.Sequence.Length > 0) AndAlso Not ExistInUserPrimers(pf, nPrimers) Then
                            If Not PrimerDict.ContainsValue(Nuctions.TAGCFilter(pf.Sequence)) And Not PrimerDict.ContainsKey(pf.Name.ToLower) Then
                                pf.Calculate()
                                Primers.Add(pf)
                                PrimerDict.Add(pf.Name.ToLower, Nuctions.TAGCFilter(pf.Sequence))
                            End If
                        End If
                        If (pr.Name IsNot Nothing AndAlso pr.Name.Length > 0) AndAlso (pr.Sequence IsNot Nothing And pr.Sequence.Length > 0) AndAlso Not ExistInUserPrimers(pr, nPrimers) Then
                            If Not PrimerDict.ContainsValue(Nuctions.TAGCFilter(pr.Sequence)) And Not PrimerDict.ContainsKey(pr.Name.ToLower) Then
                                pr.Calculate()
                                Primers.Add(pr)
                                PrimerDict.Add(pr.Name.ToLower, Nuctions.TAGCFilter(pr.Sequence))
                            End If
                        End If
                        If cItem IsNot ci Then
                            For Each kp In Dict
                                If kp.Key.ToLower = pf.Name.ToLower AndAlso Nuctions.TAGCFilter(kp.Value) <> Nuctions.TAGCFilter(pf.Sequence) Then ErrorMessage.AppendFormat("Primer name {0} has been used by Node {1}. Please change the primer name.{2}", kp.Key, ci.Index, vbCrLf)
                                If kp.Key.ToLower = pr.Name.ToLower AndAlso Nuctions.TAGCFilter(kp.Value) <> Nuctions.TAGCFilter(pr.Sequence) Then ErrorMessage.AppendFormat("Primer name {0} has been used by Node {1}. Please change the primer name.{2}", kp.Key, ci.Index, vbCrLf)
                                If Nuctions.TAGCFilter(kp.Value) = Nuctions.TAGCFilter(pf.Sequence) AndAlso kp.Key.ToLower <> pf.Name.ToLower Then ErrorMessage.AppendFormat("Primer sequence {0} has been used by Node {1} under the name {2}. Please try keep using the same name.{3}", kp.Key, ci.Index, pf.Name, vbCrLf)
                                If Nuctions.TAGCFilter(kp.Value) = Nuctions.TAGCFilter(pr.Sequence) AndAlso kp.Key.ToLower <> pr.Name.ToLower Then ErrorMessage.AppendFormat("Primer sequence {0} has been used by Node {1} under the name {2}. Please try keep using the same name.{3}", kp.Key, ci.Index, pr.Name, vbCrLf)
                            Next
                        End If
                    End If
                Case Nuctions.MolecularOperationEnum.Screen
                    Dim pf As New PrimerInfo With {.Name = ci.MolecularInfo.Screen_FName, .Sequence = ci.MolecularInfo.Screen_FPrimer, .NeedSynthesis = True}
                    Dim pr As New PrimerInfo With {.Name = ci.MolecularInfo.Screen_RName, .Sequence = ci.MolecularInfo.Screen_RPrimer, .NeedSynthesis = True}
                    If ci.MolecularInfo.Screen_FPrimer IsNot Nothing AndAlso ci.MolecularInfo.Screen_RPrimer IsNot Nothing Then
                        If (pf.Name IsNot Nothing AndAlso pf.Name.Length > 0) AndAlso (pf.Sequence IsNot Nothing And pf.Sequence.Length > 0) AndAlso Not ExistInUserPrimers(pf, nPrimers) Then
                            If Not PrimerDict.ContainsValue(Nuctions.TAGCFilter(pf.Sequence)) And Not PrimerDict.ContainsKey(pf.Name.ToLower) Then
                                pf.Calculate()
                                Primers.Add(pf)
                                PrimerDict.Add(pf.Name.ToLower, Nuctions.TAGCFilter(pf.Sequence))
                            End If
                        End If
                        If (pr.Name IsNot Nothing AndAlso pr.Name.Length > 0) AndAlso (pr.Sequence IsNot Nothing And pr.Sequence.Length > 0) AndAlso Not ExistInUserPrimers(pr, nPrimers) Then
                            If Not PrimerDict.ContainsValue(Nuctions.TAGCFilter(pr.Sequence)) And Not PrimerDict.ContainsKey(pr.Name.ToLower) Then
                                pr.Calculate()
                                Primers.Add(pr)
                                PrimerDict.Add(pr.Name.ToLower, Nuctions.TAGCFilter(pr.Sequence))
                            End If
                        End If
                        If cItem IsNot ci Then
                            For Each kp In Dict
                                If kp.Key.ToLower = pf.Name.ToLower AndAlso Nuctions.TAGCFilter(kp.Value) <> Nuctions.TAGCFilter(pf.Sequence) Then ErrorMessage.AppendFormat("Primer name {0} has been used by Node {1}. Please change the primer name.{2}", kp.Key, ci.Index, vbCrLf)
                                If kp.Key.ToLower = pr.Name.ToLower AndAlso Nuctions.TAGCFilter(kp.Value) <> Nuctions.TAGCFilter(pr.Sequence) Then ErrorMessage.AppendFormat("Primer name {0} has been used by Node {1}. Please change the primer name.{2}", kp.Key, ci.Index, vbCrLf)
                                If Nuctions.TAGCFilter(kp.Value) = Nuctions.TAGCFilter(pf.Sequence) AndAlso kp.Key.ToLower <> pf.Name.ToLower Then ErrorMessage.AppendFormat("Primer sequence {0} has been used by Node {1} under the name {2}. Please try keep using the same name.{3}", kp.Key, ci.Index, pf.Name, vbCrLf)
                                If Nuctions.TAGCFilter(kp.Value) = Nuctions.TAGCFilter(pr.Sequence) AndAlso kp.Key.ToLower <> pr.Name.ToLower Then ErrorMessage.AppendFormat("Primer sequence {0} has been used by Node {1} under the name {2}. Please try keep using the same name.{3}", kp.Key, ci.Index, pr.Name, vbCrLf)
                            Next
                        End If
                    End If
                Case Nuctions.MolecularOperationEnum.SequencingResult
                    Dim ps As New PrimerInfo With {.Name = ci.MolecularInfo.SequencingPrimerName, .Sequence = ci.MolecularInfo.SequencingPrimer, .NeedSynthesis = True}
                    If (ps.Name IsNot Nothing AndAlso ps.Name.Length > 0) AndAlso (ps.Sequence IsNot Nothing And ps.Sequence.Length > 0) AndAlso Not ExistInUserPrimers(ps, nPrimers) Then
                        If Not PrimerDict.ContainsValue(Nuctions.TAGCFilter(ps.Sequence)) And Not PrimerDict.ContainsKey(Nuctions.TAGCFilter(ps.Name.ToLower)) Then
                            ps.Calculate()
                            Primers.Add(ps)
                            PrimerDict.Add(ps.Name.ToLower, Nuctions.TAGCFilter(ps.Sequence))
                        End If
                        If cItem IsNot ci Then
                            For Each kp In Dict
                                If kp.Key.ToLower = ps.Name.ToLower AndAlso Nuctions.TAGCFilter(kp.Value) <> Nuctions.TAGCFilter(ps.Sequence) Then ErrorMessage.AppendFormat("Primer name {0} has been used by Node {1}. Please change the primer name.{2}", kp.Key, ci.Index, vbCrLf)

                                If Nuctions.TAGCFilter(kp.Value) = Nuctions.TAGCFilter(ps.Sequence) AndAlso kp.Key.ToLower <> ps.Name.ToLower Then ErrorMessage.AppendFormat("Primer sequence {0} has been used by Node {1} under the name {2}. Please try keep using the same name.{3}", kp.Key, ci.Index, ps.Name, vbCrLf)

                            Next
                        End If
                    End If
            End Select
        Next
        If ErrorMessage.Length > 0 Then
            MsgBox(ErrorMessage.ToString, MsgBoxStyle.OkOnly, "Vexcutor - Duplicate Primers or Primer Names")
        End If
        Primers.AddRange(nPrimers)
    End Sub

    Private Function ExistInUserPrimers(pi As PrimerInfo, uPrimer As List(Of PrimerInfo)) As Boolean
        For Each p As PrimerInfo In uPrimer
            If pi.Name.ToLower = p.Name.ToLower And Nuctions.TAGCFilter(pi.Sequence) = Nuctions.TAGCFilter(p.Sequence) Then Return True
        Next
        Return False
    End Function

    Public Function Add(ByVal vDNA As DNAInfo, ByVal Enzymes As IEnumerable(Of String)) As ChartItem
        Dim ci As ChartItem
        ci = New ChartItem(vDNA, Enzymes, smList.Count)
        AddHandler ci.RequireParent, AddressOf OnRequireParent
        smList.Add(ci)
        EnzymeSites = Enzymes
        Return ci
    End Function


    Public Sub Remove(ByVal vItem As ChartItem)
        RemoveHandler vItem.RequireParent, AddressOf OnRequireParent
        smList.Remove(vItem)
        For Each ci As ChartItem In smList
            If ci.MolecularInfo.Source.Contains(vItem.MolecularInfo) Then ci.MolecularInfo.Source.Remove(vItem.MolecularInfo)
        Next
        ResetIndex()
        Draw()
    End Sub

    Public Sub ResetIndex()
        Dim i As Integer = 0
        For Each ci As ChartItem In smList
            ci.SetIndex(i)
            i += 1
        Next
    End Sub

    Public Sub OnRequireParent(ByVal sender As Object, ByVal e As RequireParentEventArgs)
        e.Parent = Me
    End Sub

    Public Sub LoadSummary(ByVal Source As List(Of DNAInfo), ByVal Enzymes As List(Of String), ByVal cFeature As List(Of Nuctions.Feature))
        smList.Clear()
        Dim ci As ChartItem
        For Each di As DNAInfo In Source
#If Remote = 1 Then
            MsgBox("Before Create ChartItem", MsgBoxStyle.OkOnly)
#End If
            ci = New ChartItem(di, Enzymes, smList.Count)
#If Remote = 1 Then
            MsgBox("ChartItem Created", MsgBoxStyle.OkOnly)
#End If
            AddHandler ci.RequireParent, AddressOf OnRequireParent
#If Remote = 1 Then
            MsgBox("Handler Attached", MsgBoxStyle.OkOnly)
#End If
            smList.Add(ci)
        Next
        EnzymeSites = Enzymes
        vFeatures = cFeature
    End Sub


    Private _PrintView As Boolean = False
    Public Property PrintView As Boolean
        Get
            Return _PrintView
        End Get
        Set(value As Boolean)
            _PrintView = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("PrintView"))
        End Set
    End Property


    Private Shared synthenomeBrush As New System.Drawing.TextureBrush(My.Resources.Synthenome2)

    Public Sub Draw()
        Dim g As Graphics = bpbMain.BufferedGraphics
        g.ResetTransform()
        If vSourceMode Then
            g.Clear(Color.LightCyan)
            g.FillRectangle(synthenomeBrush, 0, 0, Width, Height)
        Else
            g.Clear(Color.White)
            g.FillRectangle(synthenomeBrush, 0, 0, Width, Height)
        End If

        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.InterpolationMode = Drawing2D.InterpolationMode.Low
        g.ScaleTransform(vScale, vScale)
        g.TranslateTransform(offsetX, offsetY)
        For Each si As ChartItem In smList
            si.Draw(g, True)
        Next
        For Each si As ChartItem In smList
            si.DrawArrow(g)
        Next
        If PrintView Then DrawPrintPages(g)
        bpbMain.Draw()
    End Sub
    Public PrintPages As New List(Of PrintPage)

    Public Sub DrawPrintPages(ByVal g As Graphics)
        For Each pp As PrintPage In PrintPages
            pp.Draw(g, ScaleValue)
        Next
    End Sub

    Dim RectSelChanged As Boolean

    Public Sub DrawRect()
        Dim g As Graphics = bpbMain.BufferedGraphics
        g.ResetTransform()
        If vSourceMode Then
            g.Clear(Color.LightCyan)
            g.FillRectangle(synthenomeBrush, 0, 0, Width, Height)
        Else
            g.Clear(Color.White)
            g.FillRectangle(synthenomeBrush, 0, 0, Width, Height)
        End If
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.InterpolationMode = Drawing2D.InterpolationMode.Low
        g.ScaleTransform(vScale, vScale)
        g.TranslateTransform(offsetX, offsetY)
        Dim vL As Single = Math.Min(startX, rectX)
        Dim vR As Single = Math.Max(startX, rectX)
        Dim vT As Single = Math.Min(startY, rectY)
        Dim vB As Single = Math.Max(startY, rectY)

        For Each si As ChartItem In smList
            If si.Left < vR And si.Right > vL And si.Top < vB And si.Bottom > vT And (Not vSelectedItems.Contains(si)) Then
                vSelectedItems.Add(si)
                RectSelChanged = True
            End If
            If Not (si.Left < vR And si.Right > vL And si.Top < vB And si.Bottom > vT) And vSelectedItems.Contains(si) Then
                vSelectedItems.Remove(si)
                RectSelChanged = True
            End If
            si.Draw(g, True)
        Next

        For Each si As ChartItem In smList
            si.DrawArrow(g)
        Next
        If PrintView Then DrawPrintPages(g)
        If recting Then g.DrawRectangle(Pens.OrangeRed, vL, vT, vR - vL, vB - vT)
        bpbMain.Draw()
    End Sub

    Dim offsetX As Single
    Dim offsetY As Single
    Dim oldX As Single
    Dim oldY As Single
    Dim startX As Single
    Dim startY As Single
    Dim vScale As Single = 1
    Dim dragging As Boolean = False
    Dim moving As Boolean = False
    Dim draggingItem As ChartItem
    'x2 = x1*scale + offset
    Dim relatedItems As New List(Of ChartItem)
    Dim vSelectedItems As New List(Of ChartItem)
    Public Event SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

    Public Sub OnSelectedIndexChanged()
        RaiseEvent SelectedIndexChanged(Me, New EventArgs)
        Me.Focus()
    End Sub
    Public Event SelectedPrintPageChanged(ByVal sender As Object, ByVal e As EventArgs)
    Public Sub OnSelectedPrintPageChanged()
        If PrintView Then RaiseEvent SelectedPrintPageChanged(Me, New EventArgs)
        Me.Focus()
    End Sub
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property SelectedPrintPages As List(Of PrintPage)
        Get
            Return vSelectedPages
        End Get
    End Property

    Public Sub FindRelatedItems()
        Dim cnLevel As ChartItem

        Dim ntLevel As New List(Of ChartItem)

        relatedItems.Clear()

        cnLevel = draggingItem
        While Not (cnLevel Is Nothing)
            If cnLevel.MolecularInfo.Source.Contains(cnLevel.MolecularInfo) Then cnLevel.MolecularInfo.Source.Remove(cnLevel.MolecularInfo)
            ntLevel.Clear()
            '再寻找下一层
            For Each si As ChartItem In smList
                If si.MolecularInfo.Source.Count = 1 And si.MolecularInfo.Source.Contains(cnLevel.MolecularInfo) Then
                    ntLevel.Add(si)
                End If
            Next
            If ntLevel.Count <> 1 Then Exit While
            cnLevel = ntLevel(0)
            cnLevel.AutoFit()
            '排列下一层
            If Not relatedItems.Contains(cnLevel) Then relatedItems.Add(cnLevel)
        End While
        Draw()
    End Sub

    Dim vMenuLocation As PointF

    Public ReadOnly Property MenuLocation() As PointF
        Get
            Return vMenuLocation
        End Get
    End Property

    Dim Grabbing As Boolean = False
    Dim recting As Boolean = False
    Dim draggingPage As PrintPage
    Dim resizingPage As PrintPage
    Dim vSelectedPages As New List(Of PrintPage)
    Public Sub SelectPrintPage(ByVal vPage As PrintPage)
        vSelectedPages.Clear()
        vSelectedPages.Add(vPage)
        Draw()
    End Sub

    Private Sub bpbMain_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bpbMain.MouseDown
        Me.Focus()
        If PrintView Then
            Select Case e.Button
                Case MouseButtons.Left
                    If moving Then
                        oldX = offsetX
                        oldY = offsetY
                        startX = e.X
                        startY = e.Y
                        Grabbing = True
                    Else
                        Dim si As PrintPage
                        Dim vp As PointF
                        vp = New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
                        resizingPage = Nothing
                        For i As Integer = PrintPages.Count - 1 To 0 Step -1
                            si = PrintPages(i)
                            If si.HitControlPoint(vp, vScale) Then
                                startX = vp.X
                                startY = vp.Y
                                dragging = True
                                resizingPage = si
                                resizingPage.StartResize()
                                Exit For
                            End If
                        Next
                        If resizingPage Is Nothing Then
                            draggingPage = Nothing
                            For i As Integer = PrintPages.Count - 1 To 0 Step -1
                                si = PrintPages(i)
                                If si.Hittest(vp) Then
                                    startX = vp.X
                                    startY = vp.Y
                                    dragging = True
                                    draggingPage = si
                                    Select Case ModifierKeys
                                        Case Keys.Control
                                            If Not (vSelectedPages.Contains(si)) Then
                                                vSelectedPages.Add(si)
                                                Draw()
                                                OnSelectedPrintPageChanged()
                                            End If
                                        Case Keys.Shift
                                            If Not (vSelectedPages.Count = 1 And vSelectedPages.Contains(si)) Then
                                                vSelectedPages.Clear()
                                                vSelectedPages.Add(si)
                                                OnSelectedPrintPageChanged()
                                            End If
                                            FindRelatedItems()
                                            Draw()
                                        Case Else
                                            If Not (vSelectedPages.Count = 1 And vSelectedPages.Contains(si)) Then
                                                vSelectedPages.Clear()
                                                vSelectedPages.Add(si)
                                                Draw()
                                                OnSelectedPrintPageChanged()
                                            End If
                                    End Select
                                    For Each ci As PrintPage In vSelectedPages
                                        ci.StartMove()
                                    Next
                                    Exit For
                                End If
                            Next
                            If Not dragging And Not (ModifierKeys = Keys.Control) Then
                                vSelectedPages.Clear()
                                OnSelectedPrintPageChanged()
                                recting = True
                                vp = New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
                                startX = vp.X
                                startY = vp.Y
                                RectSelChanged = False
                                Draw()
                            End If
                        End If
                    End If
            End Select
        Else
            Select Case e.Button
                Case Windows.Forms.MouseButtons.Left
                    If moving Then
                        oldX = offsetX
                        oldY = offsetY
                        startX = e.X
                        startY = e.Y
                        Grabbing = True
                    Else
                        Dim si As ChartItem
                        Dim vp As PointF
                        vp = New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
                        For i As Integer = smList.Count - 1 To 0 Step -1
                            si = smList(i)
                            If si.Hittest(vp) Then

                                startX = vp.X
                                startY = vp.Y
                                dragging = True
                                draggingItem = si
                                Select Case ModifierKeys
                                    Case Keys.Control
                                        If Not (vSelectedItems.Contains(si)) Then
                                            vSelectedItems.Add(si)
                                            Draw()
                                            OnSelectedIndexChanged()
                                        End If
                                    Case Keys.Shift
                                        If Not (vSelectedItems.Count = 1 And vSelectedItems.Contains(si)) Then
                                            vSelectedItems.Clear()
                                            vSelectedItems.Add(si)
                                            OnSelectedIndexChanged()
                                        End If
                                        FindRelatedItems()
                                        Draw()
                                    Case Else
                                        If Not (vSelectedItems.Count = 1 And vSelectedItems.Contains(si)) Then
                                            vSelectedItems.Clear()
                                            vSelectedItems.Add(si)
                                            Draw()
                                            OnSelectedIndexChanged()
                                        End If
                                End Select
                                For Each ci As ChartItem In vSelectedItems
                                    ci.StartMove()
                                Next
                                Exit For
                            End If
                        Next
                        If Not dragging And Not (ModifierKeys = Keys.Control) Then
                            vSelectedItems.Clear()
                            OnSelectedIndexChanged()
                            recting = True
                            vp = New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
                            startX = vp.X
                            startY = vp.Y
                            RectSelChanged = False
                            Draw()
                        End If
                    End If
                Case Windows.Forms.MouseButtons.Right
                    Dim vp As PointF
                    vp = New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
                    If Not (Menu Is Nothing) Then
                        vMenuLocation = vp
                        Me.Menu.Show(Me, e.Location)
                    End If

            End Select
        End If

    End Sub
    Dim rectX As Single
    Dim rectY As Single

    Private Sub bpbMain_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bpbMain.MouseMove
        If dragging Then
            If PrintView Then
                If resizingPage IsNot Nothing Then
                    Dim vp As New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
                    For Each ci As PrintPage In vSelectedPages
                        ci.ResizeBy(vp.X - startX, vp.Y - startY)
                    Next
                Else
                    Dim vp As New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
                    For Each ci As PrintPage In vSelectedPages
                        ci.MoveBy(vp.X - startX, vp.Y - startY)
                    Next
                End If
            Else
                Dim vp As New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
                For Each ci As ChartItem In vSelectedItems
                    ci.MoveBy(vp.X - startX, vp.Y - startY)
                Next
                If ModifierKeys = Keys.Shift Then
                    For Each si As ChartItem In relatedItems
                        si.AutoFit()
                    Next
                End If
            End If
            Draw()
        End If
        If recting Then
            Dim vp As New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
            rectX = vp.X
            rectY = vp.Y
            DrawRect()
        End If
        If Grabbing Then
            offsetX = oldX + (e.X - startX) / vScale
            offsetY = oldY + (e.Y - startY) / vScale
            Draw()
        End If
    End Sub

    Private vSourceMode As Boolean

    Public Property SourceMode() As Boolean
        Get
            Return vSourceMode
        End Get
        Set(ByVal value As Boolean)
            vSourceMode = value
            Draw()
        End Set
    End Property

    Public Event PositionChanged(ByVal sender As Object, ByVal e As EventArgs)
    Private Sub bpbMain_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bpbMain.MouseUp
        If dragging Then
            If PrintView Then
                If resizingPage IsNot Nothing Then
                    Dim vp As New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
                    For Each ci As PrintPage In vSelectedPages
                        ci.ResizeBy(vp.X - startX, vp.Y - startY)
                    Next
                    Draw()
                    dragging = False
                    resizingPage = Nothing
                Else
                    Dim vp As New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
                    For Each ci As PrintPage In vSelectedPages
                        ci.MoveBy(vp.X - startX, vp.Y - startY)
                    Next
                    Draw()
                    dragging = False
                    If (vp.X - startX) <> 0 Or (vp.Y - startY) <> 0 Then
                        RaiseEvent PositionChanged(Me, New EventArgs)
                    End If
                    draggingPage = Nothing
                End If
            Else
                Dim vp As New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
                For Each ci As ChartItem In vSelectedItems
                    ci.MoveBy(vp.X - startX, vp.Y - startY)
                Next
                If ModifierKeys = Keys.Shift Then
                    For Each si As ChartItem In relatedItems
                        si.AutoFit()
                    Next
                End If
                Draw()
                dragging = False
                If (vp.X - startX) <> 0 Or (vp.Y - startY) <> 0 Then
                    RaiseEvent PositionChanged(Me, New EventArgs)
                End If
            End If
        End If
        If recting Then
            Dim vp As New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
            rectX = vp.X
            rectY = vp.Y
            recting = False
            DrawRect()
            If RectSelChanged Then
                RaiseEvent SelectedIndexChanged(Me, New EventArgs)
                RectSelChanged = False
            End If
        End If
        If Grabbing Then
            offsetX = oldX + (e.X - startX) / vScale
            offsetY = oldY + (e.Y - startY) / vScale
            Grabbing = False
            Draw()
        End If
    End Sub

    Friend Sub MoveUp()
        offsetY -= 200
        Draw()
    End Sub
    Friend Sub MoveDown()
        offsetY += 200
        Draw()
    End Sub
    Friend Sub MoveLeft()
        offsetX -= 200
        Draw()
    End Sub
    Friend Sub MoveRight()
        offsetX += 200
        Draw()
    End Sub
    Friend Sub ZoomOut()
        Dim os As Single = vScale
        vScale = vScale / 1.25
        If vScale > 10 Then vScale = 10
        If vScale < 0.01 Then vScale = 0.01
        offsetX += Width / 2 / vScale - Width / 2 / os
        offsetY += Height / 2 / vScale - Height / 2 / os
        Draw()
    End Sub
    Friend Sub ZoomIn()
        Dim os As Single = vScale
        vScale = vScale * 1.25
        If vScale > 10 Then vScale = 10
        If vScale < 0.01 Then vScale = 0.01
        offsetX += Width / 2 / vScale - Width / 2 / os
        offsetY += Height / 2 / vScale - Height / 2 / os
        Draw()
    End Sub
    Friend Sub ZoomReset()
        Dim os As Single = vScale
        vScale = 1.0F
        If vScale > 10 Then vScale = 10
        If vScale < 0.01 Then vScale = 0.01
        offsetX += Width / 2 / vScale - Width / 2 / os
        offsetY += Height / 2 / vScale - Height / 2 / os
        Draw()
    End Sub

    Private Sub frmSummary_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If ModifierKeys = Keys.Control Then Exit Sub
        Select Case e.KeyCode
            Case Keys.Space
                moving = True
            Case Keys.W
                offsetY -= 200
                Draw()
            Case Keys.S
                offsetY += 200
                Draw()
            Case Keys.A
                offsetX -= 200
                Draw()
            Case Keys.D
                offsetX += 200
                Draw()
            Case Keys.Z
                Dim os As Single = vScale
                vScale = vScale * 1.25
                If vScale > 10 Then vScale = 10
                If vScale < 0.01 Then vScale = 0.01
                offsetX += Width / 2 / vScale - Width / 2 / os
                offsetY += Height / 2 / vScale - Height / 2 / os
                Draw()
            Case Keys.X
                Dim os As Single = vScale
                vScale = vScale / 1.25
                If vScale > 10 Then vScale = 10
                If vScale < 0.01 Then vScale = 0.01
                offsetX += Width / 2 / vScale - Width / 2 / os
                offsetY += Height / 2 / vScale - Height / 2 / os
                Draw()
            Case Keys.C
                Dim os As Single = vScale
                vScale = 1.0F
                If vScale > 10 Then vScale = 10
                If vScale < 0.01 Then vScale = 0.01
                offsetX += Width / 2 / vScale - Width / 2 / os
                offsetY += Height / 2 / vScale - Height / 2 / os
                Draw()
        End Select
    End Sub
    Public Sub SavePicture()
        Dim L As Single
        Dim R As Single
        Dim T As Single
        Dim B As Single
        Dim NotSet As Boolean = True
        For Each si As ChartItem In smList
            If NotSet Then
                L = si.Left
                R = si.Right
                T = si.Top
                B = si.Bottom
                NotSet = False
            Else
                L = IIf(L < si.Left, L, si.Left)
                T = IIf(T < si.Top, T, si.Top)
                R = IIf(R > si.Right, R, si.Right)
                B = IIf(B > si.Bottom, B, si.Bottom)
            End If
        Next

        If R - L > 0 And B - T > 0 Then
            sfdPic.FileName = Me.Text
            If sfdPic.ShowDialog = Windows.Forms.DialogResult.OK Then


                Dim ofsX As Single = L - 10

                Dim ofsY As Single = T - 10

                Dim bitmap As New Bitmap(R - L + 20, B - T + 20, Imaging.PixelFormat.Format32bppArgb)
                Dim vg As Graphics = Graphics.FromImage(bitmap)
                Dim hdc As IntPtr = vg.GetHdc
                Dim emf As New System.Drawing.Imaging.Metafile(sfdPic.FileName, hdc, Imaging.EmfType.EmfPlusDual)
                Dim g As Graphics = Graphics.FromImage(emf)
                g.Clear(Color.White)
                'g.FillRectangle(synthenomeBrush, 0, 0, Width, Height)
                g.ResetTransform()
                g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.TranslateTransform(-ofsX, -ofsY)
                For Each si As ChartItem In smList
                    si.Draw(g, True)
                Next
                For Each si As ChartItem In smList
                    si.DrawArrow(g)
                Next
                g.Dispose()
                emf.Dispose()
                vg.ReleaseHdc()
                bitmap.Dispose()
            End If
        End If
    End Sub

    Public Sub SavePictureTo(ByVal vFilename As String)
        Dim L As Single
        Dim R As Single
        Dim T As Single
        Dim B As Single
        Dim NotSet As Boolean = True
        For Each si As ChartItem In smList
            If NotSet Then
                L = si.Left
                R = si.Right
                T = si.Top
                B = si.Bottom
                NotSet = False
            Else
                L = IIf(L < si.Left, L, si.Left)
                T = IIf(T < si.Top, T, si.Top)
                R = IIf(R > si.Right, R, si.Right)
                B = IIf(B > si.Bottom, B, si.Bottom)
            End If
        Next

        If R - L > 0 And B - T > 0 Then
            sfdPic.FileName = Me.Text



            Dim ofsX As Single = L - 10

            Dim ofsY As Single = T - 10

            Dim bitmap As New Bitmap(R - L + 20, B - T + 20, Imaging.PixelFormat.Format32bppArgb)

            Dim vg As Graphics = Graphics.FromImage(bitmap)

            Dim hdc As IntPtr = vg.GetHdc

            'bitmap.GetHbitmap()
            Dim emf As New System.Drawing.Imaging.Metafile(vFilename + ".emf", hdc, Imaging.EmfType.EmfPlusDual)
            Dim g As Graphics = Graphics.FromImage(emf)
            Dim vMap As New Bitmap(bitmap.Width, bitmap.Height, Imaging.PixelFormat.Format32bppArgb)
            Dim eg As Graphics = Graphics.FromImage(vMap)
            g.Clear(Color.White)
            g.ResetTransform()
            g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.TranslateTransform(-ofsX, -ofsY)

            eg.Clear(Color.White)
            eg.ResetTransform()
            eg.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            eg.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            eg.TranslateTransform(-ofsX, -ofsY)

            For Each si As ChartItem In smList
                si.DrawArrow(g)
                si.DrawArrow(eg)
            Next
            For Each si As ChartItem In smList
                si.Draw(g, True)
                si.Draw(eg, True)
            Next
            g.Dispose()
            eg.Dispose()
            vMap.Save(vFilename + ".png", System.Drawing.Imaging.ImageFormat.Png)
            vMap.Dispose()
            'bitmap.Save(vFilename + ".png", System.Drawing.Imaging.ImageFormat.Png)
            emf.Dispose()
            vg.ReleaseHdc()
            bitmap.Dispose()
        End If
    End Sub

    Public Sub CopySelectionAsEMF()

        Dim L As Single
        Dim R As Single
        Dim T As Single
        Dim B As Single
        Dim NotSet As Boolean = True
        For Each si As ChartItem In vSelectedItems
            If NotSet Then
                L = si.Left
                R = si.Right
                T = si.Top
                B = si.Bottom
                NotSet = False
            Else
                L = IIf(L < si.Left, L, si.Left)
                T = IIf(T < si.Top, T, si.Top)
                R = IIf(R > si.Right, R, si.Right)
                B = IIf(B > si.Bottom, B, si.Bottom)
            End If
        Next

        If R - L > 0 And B - T > 0 Then
            sfdPic.FileName = Me.Text



            Dim ofsX As Single = L - 10

            Dim ofsY As Single = T - 10

            Dim bitmap As New Bitmap(R - L + 20, B - T + 20, Imaging.PixelFormat.Format32bppArgb)

            Dim vg As Graphics = Graphics.FromImage(bitmap)

            Dim hdc As IntPtr = vg.GetHdc

            'bitmap.GetHbitmap()
            Dim emf As New System.Drawing.Imaging.Metafile(New System.IO.MemoryStream, hdc, Imaging.EmfType.EmfPlusDual)
            Dim g As Graphics = Graphics.FromImage(emf)
            g.Clear(Color.White)
            g.ResetTransform()
            g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.TranslateTransform(-ofsX, -ofsY)

            Dim diList As New List(Of DNAInfo)
            For Each si As ChartItem In vSelectedItems
                diList.Add(si.MolecularInfo)
            Next

            For Each si As ChartItem In vSelectedItems
                si.DrawArrow(g, diList)
            Next
            For Each si As ChartItem In vSelectedItems
                si.Draw(g, True, True)
            Next
            g.Dispose()
            ClipboardMetafileHelper.PutEnhMetafileOnClipboard(Me.ParentForm.Handle, emf)
            emf.Dispose()
            vg.ReleaseHdc()
            bitmap.Dispose()

        End If

        'If lv_Chart.SelectedItems.Count > 0 AndAlso lv_Chart.SelectedItems(0).MolecularInfo.DNAs.Count = 1 Then
        '    If lv_Chart.SelectedItems.Count > 0 AndAlso lv_Chart.SelectedItems(0).MolecularInfo.DNAs.Count = 1 Then

        '        Dim bmp As New Bitmap(20, 20, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
        '        Dim g As Graphics = Graphics.FromImage(bmp)
        '        Dim sz As SizeF = lv_Chart.SelectedItems(0).DrawMap(g)
        '        g.Dispose()
        '        bmp.Dispose()


        '        bmp = New Bitmap(Math.Ceiling(sz.Width), Math.Ceiling(sz.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb)
        '        g = Graphics.FromImage(bmp)

        '        Dim emf As New System.Drawing.Imaging.Metafile(New System.IO.MemoryStream, g.GetHdc, Imaging.EmfType.EmfPlusDual)
        '        Dim eg As Graphics = Graphics.FromImage(emf)
        '        lv_Chart.SelectedItems(0).DrawMap(eg)
        '        eg.Dispose()
        '        g.ReleaseHdc()
        '        ClipboardMetafileHelper.PutEnhMetafileOnClipboard(Me.ParentForm.Handle, emf)
        '    End If
        'End If
    End Sub


    Public Sub SaveSelectionPictureTo(ByVal vFilename As String)
        Dim L As Single
        Dim R As Single
        Dim T As Single
        Dim B As Single
        Dim NotSet As Boolean = True
        For Each si As ChartItem In vSelectedItems
            If NotSet Then
                L = si.Left
                R = si.Right
                T = si.Top
                B = si.Bottom
                NotSet = False
            Else
                L = IIf(L < si.Left, L, si.Left)
                T = IIf(T < si.Top, T, si.Top)
                R = IIf(R > si.Right, R, si.Right)
                B = IIf(B > si.Bottom, B, si.Bottom)
            End If
        Next

        If R - L > 0 And B - T > 0 Then
            sfdPic.FileName = Me.Text



            Dim ofsX As Single = L - 10

            Dim ofsY As Single = T - 10

            Dim bitmap As New Bitmap(R - L + 20, B - T + 20, Imaging.PixelFormat.Format32bppArgb)

            Dim vg As Graphics = Graphics.FromImage(bitmap)

            Dim hdc As IntPtr = vg.GetHdc

            'bitmap.GetHbitmap()
            Dim emf As New System.Drawing.Imaging.Metafile(vFilename, hdc, Imaging.EmfType.EmfPlusDual)
            Dim g As Graphics = Graphics.FromImage(emf)
            g.Clear(Color.White)
            g.ResetTransform()
            g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.TranslateTransform(-ofsX, -ofsY)
            For Each si As ChartItem In vSelectedItems
                si.DrawArrow(g)
            Next
            For Each si As ChartItem In vSelectedItems
                si.Draw(g, True)
            Next
            g.Dispose()

            emf.Dispose()
            vg.ReleaseHdc()
            bitmap.Dispose()

        End If
    End Sub
    Public Sub SaveFilesTo(ByVal DirectoryPath As String)
        For Each si As ChartItem In smList
            si.SaveGeneFile(DirectoryPath)
        Next
    End Sub

    Private Sub bpbMain_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bpbMain.MouseDoubleClick
        Dim si As ChartItem
        For i As Integer = smList.Count - 1 To 0 Step -1
            si = smList(i)
            Dim vp As New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
            If si.Hittest(vp) Then
                si.AutoFit()
                dragging = False
                Draw()
                Exit For
            End If
        Next
    End Sub

    Private Sub OperationView_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyCode
            Case Keys.Space
                moving = False
        End Select
    End Sub

    Private Sub bpbMain_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        Select Case ModifierKeys
            Case Keys.Control
                Dim os As Single = vScale
                vScale = vScale * (1.25) ^ (e.Delta / 120)
                If vScale > 10 Then vScale = 10
                If vScale < 0.01 Then vScale = 0.01
                offsetX += e.X / vScale - e.X / os
                offsetY += e.Y / vScale - e.Y / os
                Draw()
            Case Keys.Shift
                offsetX += e.Delta / 2 / vScale
                Draw()

            Case Else
                offsetY += e.Delta / 2 / vScale
                Draw()

        End Select

    End Sub

    Private Sub bpbMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles bpbMain.SizeChanged
        Draw()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SavePicture()
    End Sub

    Public Sub AutoArragne()

        Dim cnLevel As New List(Of ChartItem)

        For Each si As ChartItem In smList
            If si.MolecularInfo.Source.Count = 0 Then
                cnLevel.Add(si)
            End If
        Next

        '在顶层排列

        Dim vX As Single
        vX = 0
        Dim w As Single
        For Each si As ChartItem In cnLevel
            w = si.Right - si.Left
            si.Left = vX
            si.Top = 0
            vX += w + 100
        Next

        '寻找下一层
        Dim ntLevel As New List(Of ChartItem)

        For Each si As ChartItem In smList
            For Each ci As ChartItem In cnLevel
                If si.MolecularInfo.Source.Contains(ci.MolecularInfo) Then
                    ntLevel.Add(si)
                    Exit For
                End If
            Next
        Next

        While ntLevel.Count > 0


            cnLevel.Clear()
            cnLevel.AddRange(ntLevel)
            ntLevel.Clear()

            '排列下一层
            For Each si As ChartItem In cnLevel
                si.AutoFit()
            Next

            '再寻找下一层
            For Each si As ChartItem In smList
                For Each ci As ChartItem In cnLevel
                    If si.MolecularInfo.Source.Contains(ci.MolecularInfo) Then
                        ntLevel.Add(si)
                        Exit For
                    End If
                Next
            Next
        End While
        Draw()
    End Sub

    Private Sub AutoArragneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        AutoArragne()
    End Sub
    Public Sub AutoFitChildren()
        Dim cnLevel As New List(Of ChartItem)
        cnLevel.Add(draggingItem)

        Dim ntLevel As New List(Of ChartItem)

        For Each si As ChartItem In smList
            For Each ci As ChartItem In cnLevel
                If si.MolecularInfo.Source.Contains(ci.MolecularInfo) Then
                    ntLevel.Add(si)
                    Exit For
                End If
            Next
        Next

        While ntLevel.Count > 0


            cnLevel.Clear()
            cnLevel.AddRange(ntLevel)
            ntLevel.Clear()

            '排列下一层
            For Each si As ChartItem In cnLevel
                si.AutoFit()
            Next

            '再寻找下一层
            For Each si As ChartItem In smList
                For Each ci As ChartItem In cnLevel
                    If si.MolecularInfo.Source.Contains(ci.MolecularInfo) Then
                        ntLevel.Add(si)
                        Exit For
                    End If
                Next
            Next
        End While
        Draw()
    End Sub

    Public Sub FitChildrenStepByStep()
        Dim cnLevel As New List(Of ChartItem)
        cnLevel.Add(draggingItem)

        Dim ntLevel As New List(Of ChartItem)

        For Each si As ChartItem In smList
            For Each ci As ChartItem In cnLevel
                If si.MolecularInfo.Source.Contains(ci.MolecularInfo) Then
                    ntLevel.Add(si)
                    Exit For
                End If
            Next
        Next

        While ntLevel.Count > 0

            cnLevel.Clear()
            cnLevel.AddRange(ntLevel)
            ntLevel.Clear()

            '排列下一层
            For Each si As ChartItem In cnLevel
                si.AutoFit()
            Next

            If MsgBox("Continue Next Level?", MsgBoxStyle.YesNo, "Fit Children Step by Step") = MsgBoxResult.No Then Exit While

            '再寻找下一层
            For Each si As ChartItem In smList
                For Each ci As ChartItem In cnLevel
                    If si.MolecularInfo.Source.Contains(ci.MolecularInfo) Then
                        ntLevel.Add(si)
                        Exit For
                    End If
                Next
            Next
            Draw()
        End While
    End Sub

    Public Sub SetEnzymes(ByVal sites As List(Of String))
        For Each ci As ChartItem In smList
            ci.ResetEnzyme(sites)
        Next
        EnzymeSites = sites
        Draw()
    End Sub
    Public Sub SetFeatures(ByVal cFeatures As List(Of Nuctions.Feature))
        vFeatures = cFeatures
    End Sub

    Private Sub AutoFitChildrenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        AutoFitChildren()
    End Sub

    Private Sub StepFitChildrenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        FitChildrenStepByStep()
    End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

