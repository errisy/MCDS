Public Class PropertyView
    Private vSelectItems As List(Of ChartItem)
    Private TPProperty As TabPage
    Private TPOperation As TabPage
    Friend TPAnalysis As TabPage
    Friend TPMultiple As TabPage
    Private TPEnzyme As TabPage
    Friend TPDNA As TabPage
    Friend TPPCR As TabPage
    Private TPFeature As TabPage
    Private TPFeatureScreen As TabPage
    Private TPPrintPage As TabPage
    Private TPProjectSummary As TabPage
    Friend TPPrimer As TabPage
    Friend TPHost As TabPage
    Private TPPrimerEditor As TabPage
    Private TPSequenceEditor As TabPage

    Private WithEvents CPrpC As PropertyControl
    Private CMIV As MultipleItemView

    '事件列表
    Public Event LoadGeneFile(ByVal sender As Object, ByVal e As EventArgs)
    Public Event LoadSequenceFile(ByVal sender As Object, ByVal e As EventArgs)
    Public Event LoadSequencingResultFile(ByVal sender As Object, ByVal e As EventArgs)
    Public Event ExportProject(ByVal sender As Object, ByVal e As EventArgs)
    Public Event ManageFeatures(ByVal sender As Object, ByVal e As EventArgs)
    Public Event ManageEnzymes(ByVal sender As Object, ByVal e As RestrictionEnzymeView.RESiteEventArgs)

    Public Event LoadExperiment(ByVal sender As Object, ByVal e As EventArgs)
    Public Event SaveExperiment(ByVal sender As Object, ByVal e As EventArgs)
    Public Event SaveExperimentAs(ByVal sender As Object, ByVal e As EventArgs)
    Public Event Close(ByVal sender As Object, ByVal e As EventArgs)

    Public Event ValueChange(ByVal sender As Object, ByVal e As EventArgs)

    '需要重新绘图
    Public Event RequireUpdateView(ByVal sender As Object, ByVal e As EventArgs)


    Public Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        If DesignMode Then Exit Sub

        TPProperty = tcMain.TabPages(0)
        TPOperation = tcMain.TabPages(1)
        TPMultiple = tcMain.TabPages(2)
        TPEnzyme = tcMain.TabPages(3)
        TPDNA = tcMain.TabPages(4)
        TPPCR = tcMain.TabPages(5)
        TPFeature = tcMain.TabPages(6)
        TPFeatureScreen = tcMain.TabPages(7)
        TPPrintPage = tcMain.TabPages(8)
        TPProjectSummary = tcMain.TabPages(9)
        TPPrimer = tcMain.TabPages(10)
        TPHost = tcMain.TabPages(11)
        TPPrimerEditor = tcMain.TabPages(12)
        TPSequenceEditor = tcMain.TabPages(13)

        tcMain.TabPages.Clear()
        tcMain.TabPages.Add(TPProperty)

        CPrpC = PrpC
        CPrpC.ParentTC = tcMain
        CMIV = MIV

        'If ReaderMode Then
        '    llRemarkFeature.Visible = False
        '    llSaveExperiment.Visible = False
        '    llSaveExperimentAs.Visible = False
        'End If
        gvPCR.RequireSequences = AddressOf mvPrimerEditorView.wpfPrimerEditor.GetPrimerSequnces
        mvPrimerEditorView.wpfPrimerEditor.RequireWorkControl = AddressOf ChildGetWorkControl
        mvSequenceEditorView.wpfDNADesigner.RequireWorkControl = AddressOf ChildGetWorkControl
        mvPrimerEditorView.PropertyTab = PrpC
        mvSequenceEditorView.PropertyTab = PrpC
    End Sub

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property SelectItem() As List(Of ChartItem)
        Get
            Return vSelectItems
        End Get
        Set(ByVal value As List(Of ChartItem))
            If Not (value Is Nothing) Then

                Select Case value.Count
                    Case 0
                        If tcMain.TabPages.Contains(TPProperty) Then
                        Else
                            tcMain.TabPages.Add(TPProperty)
                            Dim rmlist As New List(Of CustomTabPage)
                            For Each tp As CustomTabPage In tcMain.TabPages
                                If Not (tp Is TPProperty) Then rmlist.Add(tp)
                            Next
                            For Each tp As CustomTabPage In rmlist
                                tcMain.TabPages.Remove(tp)
                            Next
                        End If
                    Case 1
                        If Not tcMain.TabPages.Contains(TPOperation) Then tcMain.TabPages.Add(TPOperation)
                        If Not tcMain.TabPages.Contains(tpGelImage) Then tcMain.TabPages.Add(tpGelImage)
                        If Not tcMain.TabPages.Contains(tpSequencing) Then tcMain.TabPages.Add(tpSequencing)
                        Dim rmlist As New List(Of CustomTabPage)
                        For Each tpp In tcMain.TabPages
                            rmlist.Add(tpp)
                        Next

                        If rmlist.Contains(TPOperation) Then rmlist.Remove(TPOperation)
                        If rmlist.Contains(tpGelImage) Then rmlist.Remove(tpGelImage)
                        If rmlist.Contains(tpSequencing) Then rmlist.Remove(tpSequencing)
                        For Each tp As CustomTabPage In rmlist
                            tcMain.TabPages.Remove(tp)
                        Next
                        CPrpC.RelatedChartItem = value(0)
                        Dim dnai As DNAInfo = value(0).MolecularInfo
                        If dnai.GelFiles Is Nothing Then dnai.GelFiles = New System.Collections.ObjectModel.ObservableCollection(Of BitImage)
                        If dnai.SCFFiles Is Nothing Then dnai.SCFFiles = New System.Collections.ObjectModel.ObservableCollection(Of SequenceItem)
                        wpfGelImage.DataContext = dnai.GelFiles
                        wpfGelImage.NumberContext = dnai
                        wpfSequencings.DataContext = dnai.SCFFiles
                        wpfSequencings.NumberContext = dnai
                        tcMain.SelectedTab = TPOperation
                        'If tcMain.TabPages.Contains(TPOperation) Then
                        '    If Not (PrpC.RelatedChartItem Is value(0)) Then

                        '        'For Each tp As CustomTabPage In tcMain.TabPages
                        '        '    If Not (tp Is TPOperation) Then rmlist.Add(tp)
                        '        'Next
                        '        rmlist.AddRange(tcMain.TabPages)
                        '        If rmlist.Contains(TPOperation) Then rmlist.Remove(TPOperation)
                        '        If rmlist.Contains(tpGelImage) Then rmlist.Remove(tpGelImage)
                        '        If rmlist.Contains(tpSequencing) Then rmlist.Remove(tpSequencing)
                        '        For Each tp As CustomTabPage In rmlist
                        '            tcMain.TabPages.Remove(tp)
                        '        Next
                        '    End If

                        'Else
                        '    tcMain.TabPages.Add(TPOperation)
                        '    Dim rmlist As New List(Of CustomTabPage)
                        '    For Each tp As CustomTabPage In tcMain.TabPages
                        '        If Not (tp Is TPOperation) Then rmlist.Add(tp)
                        '    Next
                        '    For Each tp As CustomTabPage In rmlist
                        '        tcMain.TabPages.Remove(tp)
                        '    Next
                        '    CPrpC.RelatedChartItem = value(0)
                        'End If
                        Dim sci As ChartItem = value(0)
                        If sci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.FreeDesign Then
                            If Not tcMain.TabPages.Contains(TPSequenceEditor) Then tcMain.TabPages.Add(TPSequenceEditor)
                            If mvSequenceEditorView.wpfDNADesigner.RelatedChartItem IsNot sci Then
                                mvSequenceEditorView.wpfDNADesigner.UnloadView()
                                mvSequenceEditorView.wpfDNADesigner.RelatedChartItem = sci
                                mvSequenceEditorView.wpfDNADesigner.RelatedDNAInfo = sci.MolecularInfo
                                mvSequenceEditorView.wpfDNADesigner.LoadView()
                            End If
                            'Else
                            '    If tcMain.TabPages.Contains(TPSequenceEditor) Then tcMain.TabPages.Remove(TPSequenceEditor)
                            '    mvSequenceEditorView.wpfDNADesigner.UnloadView()
                        End If
                        If sci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.PCR Or sci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.Screen Then
                            mvPrimerEditorView.wpfPrimerEditor.UnloadView()
                            If mvPrimerEditorView.wpfPrimerEditor.RelatedChartItem IsNot sci Then
                                If Not tcMain.TabPages.Contains(TPPrimerEditor) Then tcMain.TabPages.Add(TPPrimerEditor)
                                mvPrimerEditorView.wpfPrimerEditor.RelatedChartItem = sci
                                mvPrimerEditorView.wpfPrimerEditor.RelatedDNAInfo = sci.MolecularInfo
                                mvPrimerEditorView.wpfPrimerEditor.LoadView()
                            End If
                            'Else
                            '    If tcMain.TabPages.Contains(TPPrimerEditor) Then tcMain.TabPages.Remove(TPPrimerEditor)
                            '    mvPrimerEditorView.wpfPrimerEditor.UnloadView()
                        End If
                    Case Else
                        If tcMain.TabPages.Contains(TPMultiple) Then
                            Dim rmlist As New List(Of CustomTabPage)
                            For Each tp As CustomTabPage In tcMain.TabPages
                                If Not (tp Is TPMultiple) Then rmlist.Add(tp)
                            Next
                            For Each tp As CustomTabPage In rmlist
                                tcMain.TabPages.Remove(tp)
                            Next
                            CMIV.SetSelectedItems(value)
                        Else
                            tcMain.TabPages.Add(TPMultiple)
                            Dim rmlist As New List(Of CustomTabPage)
                            For Each tp As CustomTabPage In tcMain.TabPages
                                If Not (tp Is TPMultiple) Then rmlist.Add(tp)
                            Next
                            For Each tp As CustomTabPage In rmlist
                                tcMain.TabPages.Remove(tp)
                            Next
                            CMIV.SetSelectedItems(value)
                        End If
                End Select
            End If
        End Set
    End Property

    Private vSelectedPages As New List(Of PrintPage)

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property SelectPage() As List(Of PrintPage)
        Get
            If DesignMode Then Return New List(Of PrintPage)
            Return vSelectedPages
        End Get
        Set(ByVal value As List(Of PrintPage))
            If DesignMode Then Exit Property
            If value Is Nothing Then
                vSelectedPages.Clear()
            Else
                vSelectedPages = value
            End If

            If tcMain.TabPages.Count > 1 Then
                tcMain.TabPages.Clear()
                tcMain.TabPages.Add(TPPrintPage)
            ElseIf Not tcMain.TabPages.Contains(TPPrintPage) Then
                tcMain.TabPages.Clear()
                tcMain.TabPages.Add(TPPrintPage)
            End If
            If vSelectedPages.Count = 0 Then
                PPV.Visible = False
                llPrintPageCount.Text = "No Page Selected"
            ElseIf vSelectedPages.Count = 1 Then
                PPV.Visible = True
                PPV.RelatedPrintPage = vSelectedPages(0)
            Else
                PPV.Visible = False
                llPrintPageCount.Text = vSelectedPages.Count.ToString + " Pages Selected"
            End If
        End Set
    End Property

    Public Sub SetPrintPageView(ByVal PrintPageView As Boolean)
        If True Then
            tcMain.TabPages.Clear()
            tcMain.TabPages.Add(TPPrintPage)
        Else
            tcMain.TabPages.Clear()
            tcMain.TabPages.Add(TPProperty)
        End If
    End Sub


#Region "对外事件"
    Private Sub llLoadGeneFile_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llLoadGeneFile.LinkClicked
        RaiseEvent LoadGeneFile(Me, New EventArgs)
    End Sub

    Private Sub llLoadSequenceFile_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llLoadSequenceFile.LinkClicked
        RaiseEvent LoadSequenceFile(Me, New EventArgs)

    End Sub

    Private Sub llLoadSequencingResultFile_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llLoadSequencingResultFile.LinkClicked
        RaiseEvent LoadSequencingResultFile(Me, New EventArgs)

    End Sub

    Private Sub llManageFeatures_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llManageFeatures.LinkClicked
        If tcMain.TabPages.Contains(TPFeature) Then

            FMV.LoadCurrentFeatures(Features)
            TPFeature.Select()
        Else
            tcMain.TabPages.Add(TPFeature)
            FMV.LoadCurrentFeatures(Features)
            TPFeature.Select()
        End If
    End Sub

    Private Sub llManageEnzymes_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llManageEnzymes.LinkClicked
        'RaiseEvent ManageEnzymes(Me, New EventArgs)
        If Not tcMain.TabPages.Contains(TPEnzyme) Then
            reView.LoadEnzymeItems(SettingEntry.EnzymeCol, Enzymes)
            tcMain.TabPages.Add(TPEnzyme)
            TPEnzyme.Select()
        End If
    End Sub
    Public Sub CloseEnzymeTab()
        tcMain.TabPages.Remove(TPEnzyme)
    End Sub

    Private Sub llExportPrimerList_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llExportProject.LinkClicked
        RaiseEvent ExportProject(Me, New EventArgs)
    End Sub
    Private Sub llLoadExperiment_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        RaiseEvent LoadExperiment(Me, New EventArgs)
    End Sub

    Private Sub llSaveExperiment_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        RaiseEvent SaveExperiment(Me, New EventArgs)
    End Sub

    Private Sub llSaveExperimentAs_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        RaiseEvent SaveExperimentAs(Me, New EventArgs)
    End Sub

    Private Sub llClose_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llClose.LinkClicked
        RaiseEvent Close(Me, New EventArgs)
    End Sub

    Private Sub CPrpC_RequireSource(ByVal sender As Object, ByVal e As SourceEventArgs) Handles CPrpC.RequireSource
        RaiseEvent RequireSource(sender, e)
    End Sub
    Private Sub CPrpC_ValueChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles CPrpC.ValueChange
        RaiseEvent ValueChange(Me, New EventArgs)
    End Sub
#End Region



    Private Sub reView_CloseTab(ByVal sender As Object, ByVal e As System.EventArgs) Handles reView.CloseTab
        CloseEnzymeTab()
    End Sub

    Private Sub reView_SetRestrictSite(ByVal sender As Object, ByVal e As RestrictionEnzymeView.RESiteEventArgs) Handles reView.SetRestrictSite
        RaiseEvent ManageEnzymes(Me, e)
    End Sub

    Public Event RequireEnzymeSite(ByVal sender As Object, ByVal e As RestrictionEnzymeView.RESiteEventArgs)

    Public ReadOnly Property Enzymes() As List(Of String)
        Get
            Dim e As New RestrictionEnzymeView.RESiteEventArgs
            RaiseEvent RequireEnzymeSite(Me, e)
            Return e.RESites
        End Get
    End Property

    Public ReadOnly Property Features() As List(Of Nuctions.Feature)
        Get
            Dim e As New FeatureEventArgs
            RaiseEvent ReqireFeatures(Me, e)
            Return e.Features
        End Get
    End Property
    Friend Event ReqireWorkControl(ByVal sender As Object, ByVal e As WorkControlEventArgs)
    Friend ReadOnly Property WorkControl As WorkControl
        Get
            Dim wce As New WorkControlEventArgs
            RaiseEvent ReqireWorkControl(Me, wce)
            Return wce.WorkControl
        End Get
    End Property
    Private Function ChildGetWorkControl() As WorkControl
        Return WorkControl
    End Function

    Private Sub PrpC_ReqireFeatures(ByVal sender As Object, ByVal e As FeatureEventArgs) Handles PrpC.ReqireFeatures
        e.Features = Features
    End Sub

    Private Sub PrpC_ReqireWorkControl(sender As Object, e As WorkControlEventArgs) Handles PrpC.ReqireWorkControl
        e.WorkControl = WorkControl
    End Sub

    Private Sub PrpC_RequireDNAView(ByVal sender As Object, ByVal e As DNAViewEventArgs) Handles PrpC.RequireDNAView
        If tcMain.TabPages.Contains(TPDNA) Then
            gvDNA.ShowGeneFiles(e.DNAs, Enzymes)
        Else
            tcMain.TabPages.Add(TPDNA)
            gvDNA.ShowGeneFiles(e.DNAs, Enzymes)
        End If
    End Sub
    Private Sub PrpC_RequireCellView(ByVal sender As Object, ByVal e As CellViewEventArgs) Handles PrpC.RequireCellView
        If tcMain.TabPages.Contains(TPDNA) Then
            gvDNA.ShowCells(e.Cells, e.Enzymes)
        Else
            tcMain.TabPages.Add(TPDNA)
            gvDNA.ShowCells(e.Cells, e.Enzymes)
        End If
    End Sub

    Private Sub PrpC_RequireFeatureScreenView(ByVal sender As Object, ByVal e As System.EventArgs) Handles PrpC.RequireFeatureScreenView
        If PrpC.RelatedChartItem Is Nothing Then Exit Sub
        If tcMain.TabPages.Contains(TPFeatureScreen) Then
            Dim ci As ChartItem = PrpC.RelatedChartItem
            FSV.SetFeaturesInfo(ci.MolecularInfo.Screen_Features, Me.Features)
            TPFeatureScreen.Select()
        Else
            Dim ci As ChartItem = PrpC.RelatedChartItem
            FSV.SetFeaturesInfo(ci.MolecularInfo.Screen_Features, Me.Features)
            tcMain.TabPages.Add(TPFeatureScreen)
            TPFeatureScreen.Select()
        End If

    End Sub

    Private Sub PrpC_RequirePCRView(ByVal sender As Object, ByVal e As PCRViewEventArgs) Handles PrpC.RequirePCRView, mvPrimerEditorView.RequirePCRView
        If tcMain.TabPages.Contains(TPPCR) Then
            gvPCR.ShowPCR(e.DNAs, Enzymes, e.Primers)
        Else
            tcMain.TabPages.Add(TPPCR)
            gvPCR.ShowPCR(e.DNAs, Enzymes, e.Primers)
        End If
    End Sub

    Private Sub gvPCR_PCR(ByVal sender As Object, ByVal e As PCREventArgs) Handles gvPCR.PCR
        Select Case PrpC.RelatedChartItem.MolecularInfo.MolecularOperation
            Case Nuctions.MolecularOperationEnum.PCR
                Select Case e.Target
                    Case "F"
                        PrpC.PCR_ForwardPrimer_TextBox.Text = e.Primer
                        PrpC.tbFP.Text = e.Key
                    Case "R"
                        PrpC.PCR_ReversePrimer_TextBox.Text = e.Primer
                        PrpC.tbRP.Text = e.Key
                End Select
            Case Nuctions.MolecularOperationEnum.Screen
                Select Case e.Target
                    Case "F"
                        PrpC.Screen_PCR_F.Text = e.Primer
                        PrpC.tbSCRFP.Text = e.Key
                    Case "R"
                        PrpC.Screen_PCR_R.Text = e.Primer
                        PrpC.tbSCRRP.Text = e.Key
                End Select
        End Select

    End Sub

    Private Sub gvPCR_SelectSequence(ByVal sender As Object, ByVal e As SelectEventArgs) Handles gvPCR.SelectSequence

    End Sub

    Public Event ReqireFeatures(ByVal sender As Object, ByVal e As FeatureEventArgs)

    Private Sub FMV_CloseTab(ByVal sender As Object, ByVal e As System.EventArgs) Handles FMV.CloseTab
        If tcMain.TabPages.Contains(TPFeature) Then
            tcMain.TabPages.Remove(TPFeature)
        End If
    End Sub

    Private Sub FMV_UpdateFeature(ByVal sender As Object, ByVal e As FeatureEventArgs) Handles FMV.UpdateFeature
        RaiseEvent ManageFeatures(Me, e)
    End Sub

    '
    Private Sub FSV_CloseTab(ByVal sender As Object, ByVal e As System.EventArgs) Handles FSV.CloseTab
        If tcMain.TabPages.Contains(TPFeatureScreen) Then
            tcMain.TabPages.Remove(TPFeatureScreen)
        End If
    End Sub

    Private Sub FSV_UpdateFeatures(ByVal sender As Object, ByVal e As FeatureScreenEventArgs) Handles FSV.UpdateFeatures
        PrpC.SetFeatureScreen(e.FeatureScreenInfos)
    End Sub

    Private Sub PrpC_RequireSelectDNAView(ByVal sender As Object, ByVal e As DNAViewEventArgs) Handles PrpC.RequireSelectDNAView

        If tcMain.Contains(TPDNA) Then
            For Each enz As String In Enzymes
                If Not e.Enzymes.Contains(enz) Then e.Enzymes.Add(enz)
            Next
            gvDNA.ShowSelect(e.DNAs, e.Enzymes)
            TPDNA.Select()
        Else
            For Each enz As String In Enzymes
                If Not e.Enzymes.Contains(enz) Then e.Enzymes.Add(enz)
            Next
            tcMain.TabPages.Add(TPDNA)
            gvDNA.ShowSelect(e.DNAs, e.Enzymes)
            TPDNA.Select()
        End If
    End Sub

    Private Sub gvDNA_SelectSequence(ByVal sender As Object, ByVal e As SelectEventArgs) Handles gvDNA.SelectSequence
        PrpC.AddEnzymeAnalysisSequence(e.GeneFile, e.Region)
    End Sub

    Public Function CopySelectSequence() As String
        If tcMain.SelectedTab Is TPDNA Then
            Return gvDNA.CopySequence
        ElseIf tcMain.SelectedTab Is TPPCR Then
            Return gvPCR.CopySequence
        Else
            Return ""
        End If
    End Function

    Public Event LoadSequenceEvent(ByVal sender As Object, ByVal e As LoadSequenceEventArgs)

    Public Event ExportSummary(ByVal sender As Object, ByVal e As EventArgs)

    Private Sub llSummary_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        RaiseEvent ExportSummary(Me, New EventArgs)
    End Sub

    Public Event RemarkFeature(ByVal sender As Object, ByVal e As EventArgs)
    Private Sub llRemarkFeature_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llRemarkFeature.LinkClicked
        RaiseEvent RemarkFeature(Me, New EventArgs)
    End Sub

    Public Sub ShowEnzymes()
        Dim stb As New System.Text.StringBuilder
        For Each ez As String In Enzymes
            stb.Append(ez)
            stb.Append(" ")
        Next
        lbEnzymes.Text = stb.ToString
    End Sub
    Public Event RequireSource(ByVal sender As Object, ByVal e As SourceEventArgs)

    Public Event AddPrintPage(ByVal sender As Object, ByVal e As EventArgs)
    Public Event PrintSelectedPages(sender As Object, e As EventArgs)
    Public Event DeleteSeletedPages(sender As Object, e As EventArgs)
    Public Event PrintAllPages(sender As Object, e As EventArgs)
    Public Event DirectPrintSelectedPages(sender As Object, e As EventArgs)
    Public Event DirectPrintAllPages(sender As Object, e As EventArgs)
    Private Sub llAddPage_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llAddPage.LinkClicked
        RaiseEvent AddPrintPage(Me, New EventArgs)
    End Sub

    Private Sub PPV_DirectPrintSelectedPage(sender As Object, e As System.EventArgs) Handles PPV.DirectPrintSelectedPage
        RaiseEvent DirectPrintSelectedPages(Me, New EventArgs)
    End Sub

    Private Sub PPV_PrintSelectedPage(sender As Object, e As System.EventArgs) Handles PPV.PrintSelectedPage
        RaiseEvent PrintSelectedPages(sender, e)
    End Sub
    Private Sub PPV_DeleteSeletedPage(sender As Object, e As System.EventArgs) Handles PPV.DeleteSeletedPage
        RaiseEvent DeleteSeletedPages(sender, e)
    End Sub
    Private Sub PPV_RequireUpdateView(ByVal sender As Object, ByVal e As System.EventArgs) Handles PPV.RequireUpdateView
        RaiseEvent RequireUpdateView(sender, e)
    End Sub

    Public Event RequireSummary(ByVal sender As Object, ByVal e As SummaryEventArgs)

     
    Private Sub llProjectSummary_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llProjectSummary.LinkClicked
        PresentSummary(SummarySectionEnum.Project)

    End Sub
    Public Sub PresentSummary(sec As SummarySectionEnum)
        Dim summary As New SummaryEventArgs
        RaiseEvent RequireSummary(Me, summary)
        If Not tcMain.TabPages.Contains(TPProjectSummary) Then tcMain.TabPages.Add(TPProjectSummary)
        Dim wc = ChildGetWorkControl()
        tcMain.SelectedTab = TPProjectSummary
        Dim sm As New SummaryModel With {.Name = summary.ProjectName}
        sm.ProjectSummary = summary.Summary
        For Each ent In summary.Primers
            sm.Primers.Add(New PrimerSearchEntry With {.Name = ent.Key, .Sequence = Nuctions.TAGCFilter(ent.Value), .Synthesis = True})
        Next
        PrimerRecordManager.SearchPrimers(sm.Primers)
        For Each ent In summary.Sequences
            sm.SyntheticDNAs.Add(New SyntheticDNAModel With {.Name = ent.Value.Name, .Sequence = Nuctions.TAGCFilter(ent.Value.Sequence)})
        Next
        For Each ent In summary.Enzymes
            sm.RestrictionEnzymes.Add(New EnzymeModel With {.Name = ent})
        Next
        For Each ent In summary.ToolEnzymes
            sm.ModificationEnzymes.Add(New EnzymeModel With {.Name = ent})
        Next
        Me.wpfSummaryBox.DataContext = sm
    End Sub

    Public Event RequireHost(sender As Object, e As HostEventArgs)
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property Hosts As List(Of Nuctions.Host)
        Get
            Dim e As New HostEventArgs
            RaiseEvent RequireHost(Me, e)
            Return e.Hosts
        End Get
    End Property
    Public Event RequirePrimer(sender As Object, e As PrimerEventArgs)
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property Primers As List(Of PrimerInfo)
        Get
            Dim e As New PrimerEventArgs
            RaiseEvent RequirePrimer(Me, e)
            Return e.Primers
        End Get
    End Property

    Private Sub llManagePrimers_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llManagePrimers.LinkClicked
        If tcMain.TabPages.Contains(TPPrimer) Then
            PMV.Primers = Primers
            TPPrimer.Select()
        Else
            tcMain.TabPages.Add(TPPrimer)
            PMV.Primers = Primers
            TPPrimer.Select()
        End If
    End Sub

    Private Sub llManageHosts_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llManageHosts.LinkClicked
        If tcMain.TabPages.Contains(TPHost) Then
            HMV.LoadHosts(Hosts)
            TPHost.Select()
        Else
            tcMain.TabPages.Add(TPHost)
            HMV.LoadHosts(Hosts)
            TPHost.Select()
        End If
    End Sub
    Public Event IncludeCommonDefination(sender As Object, e As EventArgs)
    Private Sub llIncludeCommon_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llIncludeCommon.LinkClicked
        RaiseEvent IncludeCommonDefination(Me, New EventArgs)
    End Sub

    Private Sub llPrintSelectedPages_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llPrintSelectedPages.LinkClicked
        RaiseEvent PrintSelectedPages(Me, New EventArgs)
    End Sub

    Private Sub llPrintAllPages_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llPrintAllPages.LinkClicked
        RaiseEvent PrintAllPages(Me, New EventArgs)
    End Sub

    Private Sub llDirectPrintSel_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llDirectPrintSel.LinkClicked

    End Sub

    Private Sub llDirectPrintAll_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llDirectPrintAll.LinkClicked

    End Sub
End Class

Public Class LoadSequenceEventArgs
    Inherits EventArgs
    Public Sequence As String
End Class

Public Class SummaryEventArgs
    Inherits EventArgs
    Public ProjectName As String
    Public Summary As String
    Public Append As String
    Public ProjectInformation As String = ""
    Public Vectors As New Dictionary(Of String, Nuctions.GeneFile)
    Public Strains As New Dictionary(Of String, String)
    Public Enzymes As New HashSet(Of String)
    Public ToolEnzymes As New HashSet(Of String)
    Public Primers As New Dictionary(Of String, String)
    Public Sequences As New Dictionary(Of String, Nuctions.GeneFile)
    Public ExperimentalProcedure As String = ""
End Class
