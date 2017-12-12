
Public Class PropertyControl
    Public vRelatedChartItem As ChartItem
    Public vMolecularOperation As Nuctions.MolecularOperationEnum
    Private mApplyMode As Boolean = False
    Private Result As Windows.Forms.DialogResult = Windows.Forms.DialogResult.Cancel
    '设置radiobutton组
    Private RBLig As New RadioButtonMap
    Private RBRec As New RadioButtonMap
    Private TPLib As New Dictionary(Of Nuctions.MolecularOperationEnum, TabPage)
    Private RBMod As New RadioButtonMap
    Private RBTrf As New RadioButtonMap
    Private RBTrm As New RadioButtonMap

    '所属的TC控件组
    Public ParentTC As TabControl

    '所控制的酶切位点视图
    Private WithEvents EnzymeControl As New RestrictionEnzymeView
    Private EnzymeTabPage As New CustomTabPage

    Public Event ValueChange(ByVal sender As Object, ByVal e As EventArgs)

    Public Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

        '转化方法的选项
        RBTrf.Add(Nuctions.TransformationMethod.ChemicalTransformation, rbTransformationChemical)
        RBTrf.Add(Nuctions.TransformationMethod.Electroporation, rbTransformationElectroporation)
        RBTrf.Add(Nuctions.TransformationMethod.Conjugation, rbTransformationConjugation)

        '转化模式的选项
        RBTrm.Add(Nuctions.TransformationMode.AllToOneCell, rbTransformationAIOC)
        RBTrm.Add(Nuctions.TransformationMode.EachPerCell, rbTransformationEDPC)
        RBTrm.Add(Nuctions.TransformationMode.Combinational, rbTransformationCBNT)

        'DNA修饰方法的选项
        RBMod.Add(Nuctions.ModificationMethodEnum.CIAP, Modify_CIAP)
        RBMod.Add(Nuctions.ModificationMethodEnum.Klewnow, Modify_Klewnow)
        RBMod.Add(Nuctions.ModificationMethodEnum.PNK, Modify_PNK)
        RBMod.Add(Nuctions.ModificationMethodEnum.T4DNAP, Modify_T4)

        For Each rb As RadioButton In RBRec.Values
            AddHandler rb.CheckedChanged, AddressOf Me.rbRec_CheckedChanged
        Next

        TPLib.Add(Nuctions.MolecularOperationEnum.Vector, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.Vector))
        TPLib.Add(Nuctions.MolecularOperationEnum.Enzyme, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.Enzyme))
        TPLib.Add(Nuctions.MolecularOperationEnum.PCR, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.PCR))
        TPLib.Add(Nuctions.MolecularOperationEnum.Modify, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.Modify))
        TPLib.Add(Nuctions.MolecularOperationEnum.Gel, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.Gel))
        TPLib.Add(Nuctions.MolecularOperationEnum.Ligation, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.Ligation))
        TPLib.Add(Nuctions.MolecularOperationEnum.Recombination, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.Recombination))
        TPLib.Add(Nuctions.MolecularOperationEnum.Screen, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.Screen))
        TPLib.Add(Nuctions.MolecularOperationEnum.EnzymeAnalysis, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.EnzymeAnalysis))
        TPLib.Add(Nuctions.MolecularOperationEnum.Merge, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.Merge))
        TPLib.Add(Nuctions.MolecularOperationEnum.FreeDesign, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.FreeDesign))
        TPLib.Add(Nuctions.MolecularOperationEnum.HashPicker, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.HashPicker))
        TPLib.Add(Nuctions.MolecularOperationEnum.SequencingResult, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.SequencingResult))
        TPLib.Add(Nuctions.MolecularOperationEnum.Compare, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.Compare))
        TPLib.Add(Nuctions.MolecularOperationEnum.Host, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.Host))
        TPLib.Add(Nuctions.MolecularOperationEnum.Transformation, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.Transformation))
        TPLib.Add(Nuctions.MolecularOperationEnum.Incubation, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.Incubation))
        TPLib.Add(Nuctions.MolecularOperationEnum.Extraction, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.Extraction))
        TPLib.Add(Nuctions.MolecularOperationEnum.Expression, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.Expression))
        TPLib.Add(Nuctions.MolecularOperationEnum.GibsonDesign, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.GibsonDesign))
        TPLib.Add(Nuctions.MolecularOperationEnum.CRISPRCut, TabControl_Operation.TabPages(Nuctions.MolecularOperationEnum.CRISPRCut))


        TabControl_Operation.TabPages.Clear()

        EnzymeTabPage.Controls.Add(EnzymeControl)
        With EnzymeTabPage
            .FontColor = Color.GreenYellow
            .SelectedFontColor = Color.Green
            .SelectedGrad1Color = Color.LightYellow
            .SelectedGrad2Color = Color.Pink
            .Grad1Color = Color.Tomato
            .Grad2Color = Color.Purple
            .SelectBorderColor = Color.Pink
            .BorderColor = Color.Purple
        End With

        EnzymeControl.Dock = DockStyle.Fill
        EnzymeTabPage.BackgroundImageLayout = ImageLayout.Stretch
        EnzymeTabPage.BackgroundImage = My.Resources.grad2
        EnzymeTabPage.Text = "Restriction Enzymes for Digestion"

#If ReaderMode = 1 Then
        llApply.Visible = False
        llApply.Enabled = False
        llCancel.Visible = False
        llCancel.Enabled = False
        rbUnstarted.Enabled = False
        rbInprogress.Enabled = False
        rbFinished.Enabled = False
        rbObsolete.Enabled = False
        dgvEnzymeAnalysis.ContextMenu = Nothing
        tbDesign.Enabled = False
        rtbFreeDesign.Enabled = False
        'tbEnzymes.Enabled = False
#End If
    End Sub

    Public Property RelatedChartItem() As ChartItem
        Get
            Return vRelatedChartItem
        End Get
        Set(ByVal value As ChartItem)
            If value Is Nothing Then
                Me.TabControl_Operation.TabPages.Clear()
            Else
                Me.TabControl_Operation.TabPages.Clear()
                TabControl_Operation.TabPages.Add(TPLib(value.MolecularInfo.MolecularOperation))
                vRelatedChartItem = value
                '确定采取何种操作
                MolecularOperation = value.MolecularInfo.MolecularOperation
                '保留克隆用于取消操作
                DNACopy = value.MolecularInfo.Backup
                '加载信息

                Reload()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(False)> Public Property MolecularOperation() As Nuctions.MolecularOperationEnum
        Get
            Return Me.vMolecularOperation
        End Get
        Set(ByVal value As Nuctions.MolecularOperationEnum)
            vMolecularOperation = value
            ''prepare the window for a specific operation
            'Me.TabControl_Operation.SelectedIndex = value
            Me.TabControl_Operation.TabPages.Clear()
            Me.TabControl_Operation.TabPages.Add(TPLib(value))
        End Set
    End Property

    Public Event RequireSource(ByVal sender As Object, ByVal e As SourceEventArgs)

    Private Sub Prop_Operation_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles Prop_Operation.LinkClicked
        'deal with source
        If ReaderMode Then Exit Sub
        Select Case RelatedChartItem.MolecularInfo.MolecularOperation
            Case Nuctions.MolecularOperationEnum.Vector

            Case Else
                Prop_Operation.LinkColor = Color.Green
                Dim sea As New SourceEventArgs
                sea.Target = Me.RelatedChartItem.MolecularInfo
                RaiseEvent RequireSource(Me, sea)
        End Select

    End Sub

    Public Sub SetSource()
        Prop_Operation.LinkColor = Color.Red
        'Gibson Assembly Node Needs to be updated
        Select Case RelatedChartItem.MolecularInfo.MolecularOperation
            Case Nuctions.MolecularOperationEnum.GibsonDesign
                RefreshGibsonDesignView()
        End Select
        RefreshOperationView()
        ApplyMode = True
    End Sub

    Private Sub RefreshOperationView()
        Dim ui As DNAInfo
        Dim sourcetext As String = ""
        Select Case RelatedChartItem.MolecularInfo.MolecularOperation
            Case Nuctions.MolecularOperationEnum.Vector
                sourcetext = "[N/A]"
            Case Else
                For Each ui In Me.RelatedChartItem.MolecularInfo.Source
                    sourcetext &= ui.Name + ";"
                Next
                If sourcetext.Length = 0 Then sourcetext = "[Click to select]"
        End Select
        Me.Prop_Operation.Text = sourcetext
        Setting_finished = True
        Select Case RelatedChartItem.MolecularInfo.Progress
            Case ProgressEnum.Unstarted
                rbUnstarted.Checked = True
            Case ProgressEnum.Inprogress
                rbInprogress.Checked = True
            Case ProgressEnum.Finished
                rbFinished.Checked = True
            Case ProgressEnum.Obsolete
                rbObsolete.Checked = True
        End Select
        If Me.RelatedChartItem.MolecularInfo.IsKeyName Then
            llName.Text = "NAME"
            llName.LinkColor = Color.DeepPink
        Else
            llName.Text = "<ID>"
            llName.LinkColor = Color.RoyalBlue
        End If
        cbNoMap.Checked = RelatedChartItem.MolecularInfo.NotDrawMap
        Setting_finished = False
        'notify that there are suspended operations.
    End Sub
    Private Sub cbGenome_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbDescribe.CheckedChanged
        If desSetting Then Exit Sub
        If cbDescribe.Checked Then
            RelatedChartItem.MolecularInfo.DescribeType = DescribeEnum.Chromosome
        Else
            RelatedChartItem.MolecularInfo.DescribeType = DescribeEnum.Vecotor
        End If
    End Sub
    Private Sub btn_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'check the name
        Accept()
    End Sub
    Private Sub ChangeValue()
        RaiseEvent ValueChange(Me, New EventArgs)
    End Sub
    Public Sub Accept()
#If ReaderMode = 0 Then


        If ReaderMode Then Exit Sub
        If ApplyMode Then
            Dim SelfDefineReloadMap As Boolean = False
            Select Case Me.MolecularOperation
                Case Nuctions.MolecularOperationEnum.Enzyme
                    'get the vectors from source and analyze enzyme cutting.
                    With Me.RelatedChartItem.MolecularInfo
                        .Calculate()
                    End With
                Case Nuctions.MolecularOperationEnum.Gel
                    Dim gelErr As Boolean = False
                    Dim errQ As New Queue(Of String)
                    If Me.Gel_Minimum_Number.Value < 100 Then gelErr = True : errQ.Enqueue("Minimum Value should be over 100")
                    If Me.Gel_Minimum_Number.Value > Me.Gel_Maximum_Number.Value Then gelErr = True : errQ.Enqueue("Minimun Value should be smaller than Maximum Value")
                    If Not gelErr Then
                        Me.RelatedChartItem.MolecularInfo.Gel_Maximun = Me.Gel_Maximum_Number.Value
                        Me.RelatedChartItem.MolecularInfo.Gel_Minimum = Me.Gel_Minimum_Number.Value
                        Me.RelatedChartItem.MolecularInfo.Calculate()
                        Nuctions.AddFeatures(Me.RelatedChartItem.MolecularInfo.DNAs, Features, RelatedChartItem.Parent.Primers)
                        'RefreshDNAView()
                        ApplyMode = False
                        ChangeValue()
                        Me.RelatedChartItem.MolecularInfo.Calculated = True
                    Else
                        Dim fErr As New frmErrorReport
                        fErr.ShowDialog(errQ, Me)
                    End If
                Case Nuctions.MolecularOperationEnum.Ligation
                    With Me.RelatedChartItem.MolecularInfo
                        .Ligation_TriFragment = RBLig.Value
                        .Calculate()
                    End With
                    RefreshLigationView()
                    'RefreshDNAView()
                    Me.ApplyMode = False
                    ChangeValue()
                    Me.RelatedChartItem.MolecularInfo.Calculated = True
                Case Nuctions.MolecularOperationEnum.Modify
                    Me.RelatedChartItem.MolecularInfo.Modify_Method = RBMod.Value
                    RelatedChartItem.MolecularInfo.Calculate()
                    Me.ApplyMode = False
                    RefreshModifyView()
                    ChangeValue()
                    Me.RelatedChartItem.MolecularInfo.Calculated = True
                Case Nuctions.MolecularOperationEnum.PCR
                    Dim errQ As New Queue(Of String)

                    RelatedChartItem.MolecularInfo.PrimerDesignerMode = False
                    'Me.RelatedChartItem.MolecularInfo.PCR_ForwardPrimer = FPrimer
                    'Me.RelatedChartItem.MolecularInfo.PCR_ReversePrimer = RPrimer
                    If tbFP.Text = tbRP.Text Then
                        tbFP.Text += "F"
                        tbRP.Text += "R"
                    End If
                    Dim fp As String = PCR_ForwardPrimer_TextBox.Text
                    Dim rp As String = PCR_ReversePrimer_TextBox.Text
                    Dim idx As Integer
                    idx = fp.LastIndexOf(">")
                    If idx > -1 Then
                        PCR_ForwardPrimer_TextBox.Text = Nuctions.TAGCFilter(fp.Substring(0, idx)) + ">" + Nuctions.TAGCFilter(fp.Substring(idx, fp.Length - idx))
                    Else
                        PCR_ForwardPrimer_TextBox.Text = Nuctions.TAGCFilter(PCR_ForwardPrimer_TextBox.Text)
                    End If
                    idx = rp.LastIndexOf(">")
                    If idx > -1 Then
                        PCR_ReversePrimer_TextBox.Text = Nuctions.TAGCFilter(rp.Substring(0, idx)) + ">" + Nuctions.TAGCFilter(rp.Substring(idx, rp.Length - idx))
                    Else
                        PCR_ReversePrimer_TextBox.Text = Nuctions.TAGCFilter(PCR_ReversePrimer_TextBox.Text)
                    End If
                    Me.RelatedChartItem.MolecularInfo.PCR_FPrimerName = tbFP.Text
                    Me.RelatedChartItem.MolecularInfo.PCR_ForwardPrimer = PCR_ForwardPrimer_TextBox.Text

                    Me.RelatedChartItem.MolecularInfo.PCR_RPrimerName = tbRP.Text
                    Me.RelatedChartItem.MolecularInfo.PCR_ReversePrimer = PCR_ReversePrimer_TextBox.Text

                    'Me.PCR_ForwardPrimer_TextBox.Text = FPrimer
                    'Me.PCR_ReversePrimer_TextBox.Text = RPrimer
                    'If FPrimer.Length < 12 Then pcrErr = True : errQ.Enqueue("Forward Primer is shorter than 12 bp")
                    'If RPrimer.Length < 12 Then pcrErr = True : errQ.Enqueue("Reverse Primer is shorter than 12 bp")
                    Me.RelatedChartItem.MolecularInfo.Calculate()
                'Dim gf As Nuctions.GeneFile
                '    Dim gfCol As New List(Of Nuctions.GeneFile)

                '    'get all the DNAs
                '    For Each mi As DNAInfo In Me.RelatedChartItem.MolecularInfo.Source
                '        If mi.Cells.Count > 0 Then
                '            For Each c As Nuctions.Cell In mi.Cells
                '                gfCol.AddRange(c.DNAs)
                '            Next
                '        End If
                '        For Each gf In mi.DNAs
                '            gfCol.Add(gf)
                '        Next
                '    Next
                '    Dim pmrList As New List(Of String)
                '    pmrList.Add(Nuctions.TAGCFilter(Me.RelatedChartItem.MolecularInfo.PCR_ForwardPrimer))
                '    pmrList.Add(Nuctions.TAGCFilter(Me.RelatedChartItem.MolecularInfo.PCR_ReversePrimer))
                '    Dim gfList As List(Of Nuctions.GeneFile) = Nuctions.PCR(gfCol, pmrList, Me.RelatedChartItem.MolecularInfo.PCR_Overlap)
                '    If Me.RelatedChartItem.MolecularInfo.DNAs.Count > 0 Then Me.RelatedChartItem.MolecularInfo.DNAs.Clear()
                '    For Each gf In gfList
                '        Me.RelatedChartItem.MolecularInfo.DNAs.Add(gf)
                '    Next
                '    Nuctions.AddFeatures(Me.RelatedChartItem.MolecularInfo.DNAs, Features, RelatedChartItem.Parent.Primers)
                '    Me.Result = Windows.Forms.DialogResult.OK

                'ApplyMode = False

                'Me.RelatedChartItem.MolecularInfo.Calculated = True
                '    Dim pDict As New Dictionary(Of String, String) From {{Me.RelatedChartItem.MolecularInfo.PCR_FPrimerName, Me.RelatedChartItem.MolecularInfo.PCR_ForwardPrimer}, {Me.RelatedChartItem.MolecularInfo.PCR_RPrimerName, Me.RelatedChartItem.MolecularInfo.PCR_ReversePrimer}}
                '    Me.RelatedChartItem.Parent.SummarziePirmers(Me.RelatedChartItem, pDict)


                Case Nuctions.MolecularOperationEnum.Screen

                    If Me.Screen_Freatures.Checked Then
                        Me.RelatedChartItem.MolecularInfo.Screen_Mode = Nuctions.ScreenModeEnum.Features

                        With Me.RelatedChartItem.MolecularInfo
                            .Calculate()
                        End With
                    Else
                        Me.RelatedChartItem.MolecularInfo.Screen_Mode = Nuctions.ScreenModeEnum.PCR
                        Dim errQ As New Queue(Of String)
                        Dim scrErr As Boolean = False
                        Dim FPrimer As String = Nuctions.TAGCFilter(Me.Screen_PCR_F.Text)
                        Dim RPrimer As String = Nuctions.TAGCFilter(Me.Screen_PCR_R.Text)
                        Me.PCR_ForwardPrimer_TextBox.Text = FPrimer
                        Me.PCR_ReversePrimer_TextBox.Text = RPrimer
                        Me.RelatedChartItem.MolecularInfo.Screen_FName = tbSCRFP.Text
                        Me.RelatedChartItem.MolecularInfo.Screen_RName = tbSCRRP.Text
                        If FPrimer.Length < 12 Then scrErr = True : errQ.Enqueue("Forward Primer is shorter than 12 bp")
                        If RPrimer.Length < 12 Then scrErr = True : errQ.Enqueue("Reverse Primer is shorter than 12 bp")
                        If Me.Screen_PCR_nudMin.Value < 100 Then scrErr = True : errQ.Enqueue("Minimum Value should be over 100")
                        If Me.Screen_PCR_nudMin.Value > Me.Screen_PCR_nudMax.Value Then scrErr = True : errQ.Enqueue("Minimun Value should be smaller than Maximum Value")
                        'process the PCR Screen
                        With Me.RelatedChartItem.MolecularInfo
                            .Screen_FPrimer = FPrimer
                            .Screen_RPrimer = RPrimer
                            .Screen_PCRMax = Me.Screen_PCR_nudMax.Value
                            .Screen_PCRMin = Me.Screen_PCR_nudMin.Value
                            .Screen_Mode = Nuctions.ScreenModeEnum.PCR
                        End With
                        If scrErr Then
                        Else
                            'For Each mi As DNAInfo In Me.RelatedChartItem.MolecularInfo.Source
                            '    For Each gf In mi.DNAs
                            '        gfCol.Add(gf)
                            '    Next
                            'Next
                            'Dim gfList As List(Of Nuctions.GeneFile) = Nuctions.ScreenPCR(gfCol, Me.RelatedChartItem.MolecularInfo.Screen_FPrimer, Me.RelatedChartItem.MolecularInfo.Screen_RPrimer, Me.RelatedChartItem.MolecularInfo.Screen_PCRMax, Me.RelatedChartItem.MolecularInfo.Screen_PCRMin, Me.RelatedChartItem.MolecularInfo.Screen_OnlyCircular)
                            'If Me.RelatedChartItem.MolecularInfo.DNAs.Count > 0 Then Me.RelatedChartItem.MolecularInfo.DNAs.Clear()
                            'For Each gf In gfList
                            '    Me.RelatedChartItem.MolecularInfo.DNAs.Add(gf)
                            'Next
                            'Nuctions.AddFeatures(Me.RelatedChartItem.MolecularInfo.DNAs, Features)
                            'Me.Result = Windows.Forms.DialogResult.OK
                            'Me.ApplyMode = False
                            'RefreshScreenView()
                            'Dim fts As New System.Text.StringBuilder
                            'fts.Append("Screen PCR ")
                            'fts.Append(Me.Screen_PCR_nudMin.Value.ToString)
                            'fts.Append(" to ")
                            'fts.Append(Me.Screen_PCR_nudMax.Value.ToString)
                            'Me.Prop_Name.Text = fts.ToString
                            ''Me.RelatedChartItem.MolecularInfo.OperationDescription = Me.rtb_Description.Text
                            'RefreshDNAView()
                            'ChangeValue()
                            'Me.RelatedChartItem.MolecularInfo.Calculated = True
                            'Me.RelatedChartItem.Parent.SummarziePirmers()
                            With Me.RelatedChartItem.MolecularInfo
                                .Calculate()
                            End With
                            Dim pDict As New Dictionary(Of String, String) From {{Me.RelatedChartItem.MolecularInfo.Screen_FName, Me.RelatedChartItem.MolecularInfo.Screen_FPrimer},
                                                                                 {Me.RelatedChartItem.MolecularInfo.Screen_RName, Me.RelatedChartItem.MolecularInfo.Screen_RPrimer}}
                            Me.RelatedChartItem.Parent.SummarziePirmers(Me.RelatedChartItem, pDict)
                        End If
                    End If
                    Me.ApplyMode = False
                    'RefreshDNAView()
                    ChangeValue()
                Case Nuctions.MolecularOperationEnum.Vector
                    With Me.RelatedChartItem.MolecularInfo
                        .Calculate()
                    End With
                    Me.ApplyMode = False
                    'RefreshDNAView()
                    ChangeValue()
                Case Nuctions.MolecularOperationEnum.Recombination
                    'Me.RelatedChartItem.MolecularInfo.RecombinationMethod = RBRec.Value
                    'Me.RelatedChartItem.MolecularInfo.IsExhaustiveAssembly = cbRecombinationAssembly.Checked
                    With Me.RelatedChartItem.MolecularInfo
                        .Calculate()
                    End With
                    Me.ApplyMode = False
                    'RefreshDNAView()
                    ChangeValue()
                    Me.RelatedChartItem.MolecularInfo.Calculated = True
                Case Nuctions.MolecularOperationEnum.EnzymeAnalysis
                    Me.RelatedChartItem.MolecularInfo.EnzymeAnalysisParamters = ReadEnzymeAnalysisItems()
                    Me.RelatedChartItem.MolecularInfo.Calculate()
                    RefreshEnzymeAnalysisView()
                    ApplyMode = False
                    ChangeValue()
                    Me.RelatedChartItem.MolecularInfo.Calculated = True
                Case Nuctions.MolecularOperationEnum.Merge
                    Me.RelatedChartItem.MolecularInfo.RecombinationMethod = RBRec.Value
                    Dim gList As New List(Of Nuctions.GeneFile)
                    For Each mi As DNAInfo In Me.RelatedChartItem.MolecularInfo.Source
                        For Each gf As Nuctions.GeneFile In mi.DNAs
                            gList.Add(gf.CloneWithoutFeatures)
                        Next
                    Next
                    Dim rList As List(Of Nuctions.GeneFile)
                    rList = Nuctions.MergeSequence(gList, RelatedChartItem.MolecularInfo.OnlySignificant, RelatedChartItem.MolecularInfo.OnlyExtend)
                    Me.RelatedChartItem.MolecularInfo.DNAs.Clear()
                    For Each gf As Nuctions.GeneFile In rList
                        Me.RelatedChartItem.MolecularInfo.DNAs.Add(gf)
                    Next
                    Nuctions.AddFeatures(Me.RelatedChartItem.MolecularInfo.DNAs, Features, RelatedChartItem.Parent.Primers)
                    Me.ApplyMode = False
                    'RefreshDNAView()
                    ChangeValue()
                    Me.RelatedChartItem.MolecularInfo.Calculated = True
                Case Nuctions.MolecularOperationEnum.FreeDesign
                    Me.RelatedChartItem.MolecularInfo.FreeDesignName = tbDesign.Text
                    Me.RelatedChartItem.MolecularInfo.Name = tbDesign.Text
                    Me.RelatedChartItem.MolecularInfo.FreeDesignCode = rtbFreeDesign.Text
                    Me.RelatedChartItem.MolecularInfo.UseFreeDesigner = cbDesign_UseDesigner.Checked
                    Me.RelatedChartItem.MolecularInfo.Calculate()
                    RefreshFreeDesignView()
                    ChangeValue()
                    Me.RelatedChartItem.MolecularInfo.Calculated = True
                Case Nuctions.MolecularOperationEnum.HashPicker
                    If Not (cpHashPickerChoosenDNA.Choices Is Nothing) Then
                        Dim choices As List(Of String) = Me.RelatedChartItem.MolecularInfo.PickedDNAs
                        choices.Clear()
                        For Each hash As String In cpHashPickerChoosenDNA.Choices.Values
                            If Not choices.Contains(hash) Then choices.Add(hash)
                        Next
                        Me.RelatedChartItem.MolecularInfo.Calculate()
                        'RefreshDNAView()
                        ChangeValue()
                    End If
                Case Nuctions.MolecularOperationEnum.SequencingResult
                    With Me.RelatedChartItem.MolecularInfo
                        .SequencingPrimerName = tbSequencingPrimerName.Text
                        .SequencingPrimer = Nuctions.TAGCFilter(tbSequencingPrimerSequence.Text)
                        .SequencingSequence = Nuctions.TAGCFilter(rtbSequencingResult.Text)
                        .Calculate()
                    End With
                    'RefreshDNAView()
                    RefreshSequencingView()
                    ChangeValue()
                    Dim pDict As New Dictionary(Of String, String) From {{Me.RelatedChartItem.MolecularInfo.SequencingPrimerName, Me.RelatedChartItem.MolecularInfo.SequencingPrimer}}
                    Me.RelatedChartItem.Parent.SummarziePirmers(Me.RelatedChartItem, pDict)
                Case Nuctions.MolecularOperationEnum.Compare
                    With Me.RelatedChartItem.MolecularInfo
                        .Calculate()
                    End With
                    'RefreshDNAView()
                    ChangeValue()
                Case Nuctions.MolecularOperationEnum.Host
                    Me.RelatedChartItem.MolecularInfo.Cells(0).Host.BioFunctions = Nuctions.AnalyzedFeatureCode(tbHostFunction.Text)
                    tbHostFunction.Text = Nuctions.ExpressFeatureFunctions(Me.RelatedChartItem.MolecularInfo.Cells(0).Host.BioFunctions)
                    With Me.RelatedChartItem.MolecularInfo
                        .Calculate()
                    End With
                    'RefreshDNAView()
                    ChangeValue()
                Case Nuctions.MolecularOperationEnum.Transformation
                    With Me.RelatedChartItem.MolecularInfo
                        .Calculate()
                    End With
                    'RefreshDNAView()
                    ChangeValue()
                Case Nuctions.MolecularOperationEnum.Incubation
                    'to read incubation methods from the data settings.
 
                    '
                    ReadIncubationParameters()
                    With Me.RelatedChartItem.MolecularInfo
                        .Calculate()
                    End With
                    'RefreshDNAView()
                    ChangeValue()
                Case Nuctions.MolecularOperationEnum.Extraction
                    With Me.RelatedChartItem.MolecularInfo
                        .Calculate()
                    End With
                    'RefreshDNAView()
                    ChangeValue()
                Case Nuctions.MolecularOperationEnum.GibsonDesign
                    If TypeOf ucWpfGibsonDesignPanel.DataContext Is GibsonDesignViewModel Then
                        Dim Gibson As GibsonDesignViewModel = ucWpfGibsonDesignPanel.DataContext
                        Gibson.cmdOptimize()
                    Else
                        Me.RelatedChartItem.MolecularInfo.Calculate()
                    End If
                    ChangeValue()
                Case Nuctions.MolecularOperationEnum.CRISPRCut
                    Me.RelatedChartItem.MolecularInfo.Calculate()
                    ChangeValue()
            End Select
            RefreshDNAView()
            RelatedChartItem.Reload(RelatedChartItem.MolecularInfo, Me.RelatedChartItem.Parent.EnzymeCol)
            Me.RelatedChartItem.Parent.Draw()
        Else
        End If
#End If
    End Sub

    Private DNACopy As DNAInfo

    Private Sub Reload()
        Dim ci As ChartItem = vRelatedChartItem
        'prevent the selection of itemself
        Dim di As DNAInfo = Me.RelatedChartItem.MolecularInfo
        di.Editing = True

        'General Information
        'Me.MolecularOperation = ci.MolecularInfo.MolecularOperation

        Dim sourcetext As String = ""
        For Each mi As DNAInfo In di.Source
            sourcetext &= mi.Name + ";"
        Next
        desSetting = True
        Me.rtb_Description.Text = di.OperationDescription
        Select Case di.DescribeType
            Case DescribeEnum.Vecotor
                cbDescribe.Checked = False
            Case DescribeEnum.Chromosome
                cbDescribe.Checked = True
        End Select
        cbMainConstruction.Checked = di.IsConstructionNode
        desSetting = False
        pxkSetting = True
        cbRealSize.Checked = di.RealSize
        snbPixelPerKBP.Value = di.PixelPerKBP
        pxkSetting = False
        If ci.MolecularInfo.Creating Then
            Me.Prop_Name.Text = di.MolecularOperation.ToString + " " + " (" + sourcetext + ")"
            Me.Prop_Type.Text = "N/D"
            Me.Prop_Count.Text = "N/D"
            ApplyMode = True
        Else
            Me.Prop_Name.Text = ci.MolecularInfo.Name
            If ci.MolecularInfo.DNAs.Count > 1 Then
                Me.Prop_Type.Text = "Blend"
                Me.Prop_Count.Text = di.DNAs.Count.ToString
            Else
                Me.Prop_Type.Text = "Pure"
                Me.Prop_Count.Text = di.DNAs.Count.ToString
            End If
        End If
        rtbEnzymeAnalysisResults.Visible = False
        ll_ViewDetails.Visible = True
        IsVerifySetting = True
        cbVerify.Checked = di.IsVerificationStep
        IsVerifySetting = False
        'Special Information
        Select Case di.MolecularOperation
            Case Nuctions.MolecularOperationEnum.Vector
                RefreshVectorView()
            Case Nuctions.MolecularOperationEnum.Enzyme
                RefreshEnzymeView()
            Case Nuctions.MolecularOperationEnum.Gel
                RefreshGelView()
            Case Nuctions.MolecularOperationEnum.Ligation
                RefreshLigationView()
            Case Nuctions.MolecularOperationEnum.Modify
                RefreshModifyView()
            Case Nuctions.MolecularOperationEnum.PCR
                RefreshPCRView()
            Case Nuctions.MolecularOperationEnum.Screen
                RefreshScreenView()
            Case Nuctions.MolecularOperationEnum.Recombination
                RefreshRecombinationView()
            Case Nuctions.MolecularOperationEnum.EnzymeAnalysis
                RefreshEnzymeAnalysisView()
            Case Nuctions.MolecularOperationEnum.Merge
                RefreshMerge()
            Case Nuctions.MolecularOperationEnum.FreeDesign
                RefreshFreeDesignView()
            Case Nuctions.MolecularOperationEnum.HashPicker
                RefreshHashPickerView()
            Case Nuctions.MolecularOperationEnum.SequencingResult
                RefreshSequencingView()
            Case Nuctions.MolecularOperationEnum.Compare
                RefreshCompareView()
            Case Nuctions.MolecularOperationEnum.Host
                RefreshHostView()
            Case Nuctions.MolecularOperationEnum.Transformation
                RefreshTransformationView()
            Case Nuctions.MolecularOperationEnum.Incubation
                RefreshIncubationView()
            Case Nuctions.MolecularOperationEnum.Extraction
                RefreshExtractionView()
            Case Nuctions.MolecularOperationEnum.Expression
            Case Nuctions.MolecularOperationEnum.GibsonDesign
                RefreshGibsonDesignView()
            Case Nuctions.MolecularOperationEnum.CRISPRCut
                RefreshCRISPRCutView()
        End Select
        RefreshOperationView()
        RefreshDNAView()
        'prevent the applymode if not creating
        If Not Me.RelatedChartItem.MolecularInfo.Creating Then Me.ApplyMode = False
        Me.RelatedChartItem.Parent.Draw()
        ApplyMode = Not RelatedChartItem.MolecularInfo.Calculated
    End Sub
    Public Sub RefreshCRISPRCutView()
        ucWpfcrisprCutPanel.DataContext = New CRISPRCutDNAInfoViewModel(RelatedChartItem.MolecularInfo)
    End Sub
    Public Sub RefreshGibsonDesignView()
        ucWpfGibsonDesignPanel.DataContext = New GibsonDesignViewModel(Me.RelatedChartItem.MolecularInfo) With {.PropertyControlView = Me}
    End Sub

    Public Sub RefreshRecombinationView()
        'rbRecSetting = True
        'RBRec.Value = Me.RelatedChartItem.MolecularInfo.RecombinationMethod
        'cbRecombinationAssembly.Checked = Me.RelatedChartItem.MolecularInfo.IsExhaustiveAssembly
        'rbRecSetting = False
        ucWpfRecombinationPanel.DataContext = New RecombinationDNAInfoViewModel(Me.RelatedChartItem.MolecularInfo)
    End Sub

    Private Sub Enzyme_Enzymes_LinkLabel_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles Enzyme_Enzymes_LinkLabel.LinkClicked
        If ReaderMode Then Exit Sub
        EnzymeControl.LoadEnzymeItems(SettingEntry.EnzymeCol, RelatedChartItem.MolecularInfo.Enzyme_Enzymes)
        If Me.ParentTC.TabPages.Contains(EnzymeTabPage) Then
            ParentTC.SelectedTab = EnzymeTabPage
        Else
            Me.ParentTC.TabPages.Add(EnzymeTabPage)
            ParentTC.SelectedTab = EnzymeTabPage
        End If
    End Sub


    Public Sub SetEnzymes(ByVal SelCol As List(Of String))
        Me.RelatedChartItem.MolecularInfo.Enzyme_Enzymes = SelCol
        RefreshEnzymeView()
        ApplyMode = True
    End Sub

    Public Sub HideEnzymeTab()
        Me.ParentTC.TabPages.Remove(EnzymeTabPage)
    End Sub

    Private Sub RefreshEnzymeView()
        Dim ei As String
        Dim sourcetext As String = ""
        For Each ei In Me.RelatedChartItem.MolecularInfo.Enzyme_Enzymes
            sourcetext &= ei + " "
        Next
        enzSetting = True
        tbEnzymes.Text = sourcetext
        cbDephosphorylate.Checked = Me.RelatedChartItem.MolecularInfo.DephosphorylateWhenDigestion
        enzSetting = False
        If sourcetext.Length = 0 Then sourcetext = "[Click to select]"
        Me.Enzyme_Enzymes_LinkLabel.Text = sourcetext


        'notify that there are suspended operations.
    End Sub

    Public Property ApplyMode() As Boolean
        Get
            Return mApplyMode
        End Get
        Set(ByVal value As Boolean)
            mApplyMode = value
        End Set
    End Property

    Private Sub DNAView_AfterLabelEdit(sender As Object, e As System.Windows.Forms.LabelEditEventArgs) Handles DNAView.AfterLabelEdit
        Dim gf As Nuctions.GeneFile = DNAView.Items(e.Item).Tag
        If e.Label <> "" Then
            gf.Name = e.Label
        Else
            DNAView.Items(e.Item).Name = gf.Name
        End If
    End Sub

    Private Sub lv_DNA_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DNAView.DoubleClick
        For Each li As ListViewItem In DNAView.SelectedItems
            If TypeOf li.Tag Is Nuctions.GeneFile Then
                Dim gf As Nuctions.GeneFile = DirectCast(li.Tag, Nuctions.GeneFile).Clone
                SettingEntry.MainUIWindow.AddDNAViewTab(gf, RelatedChartItem.Parent.EnzymeCol)
            End If
        Next
    End Sub
    Private Sub AddtoDNAView(dna As Nuctions.GeneFile, op As Nuctions.MolecularOperationEnum)
        Dim ci As ListViewItem
        ci = New ListViewItem
        ci.Name = dna.Name
        ci.Text = dna.Name
        ci.Tag = dna
        ci.ImageIndex = op
        ci.SubItems.Add(dna.Sequence.Length.ToString)
        ci.SubItems.Add(IIf(dna.Iscircular, "Circular", "Linear"))
        ci.SubItems.Add(dna.End_F)
        ci.SubItems.Add(dna.End_R)
        ci.SubItems.Add(dna.Chromosomal)
        DNAView.Items.Add(ci)
    End Sub
    Private vEnvironmentSetting As Boolean = False
    Friend Sub RefreshDNAView()
        Dim cell As Nuctions.Cell = RefreshCellsView()
        Dim dna As Nuctions.GeneFile
        Dim mi As DNAInfo = RelatedChartItem.MolecularInfo
        If cell IsNot Nothing Then
            ViewCellDNAs(cell)
        Else
            If DNAView.Items.Count > 0 Then DNAView.Items.Clear()
            For Each dna In Me.RelatedChartItem.MolecularInfo.DNAs
                AddtoDNAView(dna, mi.MolecularOperation)
            Next
        End If
        If Not CanFind(mi.Host, RelatedChartItem.Parent.Hosts) Then
            RelatedChartItem.Parent.Hosts.Add(RelatedChartItem.MolecularInfo.Host)
        End If
        'TryFind(mi.Host, RelatedChartItem.Parent.Hosts)
        vEnvironmentSetting = True
        LoadEnvironments(cbEnvironment.Items, RelatedChartItem.Parent.Hosts)
        cbEnvironment.SelectedItem = TryFind(mi.Host.Name, cbEnvironment.Items)
        vEnvironmentSetting = False
        Me.Prop_Count.Text = mi.DNAs.Count.ToString
        Me.Prop_Type.Text = IIf(mi.DNAs.Count > 1, "Blend", "Pure")
    End Sub
    Private Sub RefreshVectorView()
        If (Not (Me.RelatedChartItem.MolecularInfo.File_Filename Is Nothing)) AndAlso Me.RelatedChartItem.MolecularInfo.File_Filename.Length > 0 OrElse IO.File.Exists(Me.RelatedChartItem.MolecularInfo.File_Filename) Then
            Try
                Dim fn As IO.FileInfo = New IO.FileInfo(Me.RelatedChartItem.MolecularInfo.File_Filename)
                Me.File_FileName_LinkLabel.Text = fn.Name
                Me.File_Path_Label.Text = fn.FullName
            Catch ex As Exception

            End Try
        Else
            Me.File_FileName_LinkLabel.Text = "<No File>"
            Me.File_Path_Label.Text = "<No File>"
        End If
    End Sub
    Private Sub RefreshLigationView()
        'RBLig.Value = Me.RelatedChartItem.MolecularInfo.Ligation_TriFragment
        ucWpfLigationPanel.DataContext = New LigationViewModel(Me.RelatedChartItem.MolecularInfo)
    End Sub

    Private Sub btnScreenReset_Click(sender As Object, e As EventArgs) Handles btnScreenReset.Click
        RelatedChartItem.MolecularInfo.Screen_Features.Clear()
        RefreshScreenView()
    End Sub

    Private Sub RefreshScreenView()
        scrSetting = True
        If Me.RelatedChartItem.MolecularInfo.Screen_Mode = Nuctions.ScreenModeEnum.Features Then
            Me.Screen_Freatures.Checked = True
            Me.Screen_Features_LinkLabel.Visible = True
            Me.pnlFeature.Visible = True
            Me.Screen_PCR_Panel.Visible = False
            Me.pnlScreenPCR.Visible = False
        Else
            Me.Screen_PCR.Checked = True
            Me.Screen_Features_LinkLabel.Visible = False
            Me.Screen_PCR_Panel.Visible = True
            Me.pnlFeature.Visible = False
            Me.pnlScreenPCR.Visible = True

        End If

        UpdateFeatureScreenLink()

        cbScreenCircular.Checked = Me.RelatedChartItem.MolecularInfo.Screen_OnlyCircular
        Me.Screen_PCR_F.Text = Me.RelatedChartItem.MolecularInfo.Screen_FPrimer
        Me.tbSCRFP.Text = Me.RelatedChartItem.MolecularInfo.Screen_FName
        Me.Screen_PCR_R.Text = Me.RelatedChartItem.MolecularInfo.Screen_RPrimer
        Me.tbSCRRP.Text = Me.RelatedChartItem.MolecularInfo.Screen_RName
        Me.Screen_PCR_nudMax.Value = Me.RelatedChartItem.MolecularInfo.Screen_PCRMax
        Me.Screen_PCR_nudMin.Value = Me.RelatedChartItem.MolecularInfo.Screen_PCRMin
        BeginCalculateScreenPCR()
        scrSetting = False
        'adding primer list to the text box.
        tbSCRFP.Items.Clear()
        tbSCRRP.Items.Clear()
        For Each pi As PrimerInfo In Me.RelatedChartItem.Parent.Primers
            tbSCRFP.Items.Add(pi.Name)
            tbSCRRP.Items.Add(pi.Name)
        Next

        Dim mi As DNAInfo = RelatedChartItem.MolecularInfo

        Dim ftList As New List(Of Nuctions.Feature)
        If mi.IsCellNode Then
            Dim cList As List(Of Nuctions.Cell) = mi.GetSourceCellList
            For Each c As Nuctions.Cell In cList
                Nuctions.AddFeatures(c.DNAs, Features)
                For Each gf As Nuctions.GeneFile In c.DNAs
                    For Each ga As Nuctions.GeneAnnotation In gf.Features
                        If ga.Feature IsNot Nothing AndAlso Not (ftList.Contains(ga.Feature)) Then ftList.Add(ga.Feature)
                    Next
                    'If gf.Matches(ft.Sequence).Count > 0 Or gf.Matches(ft.RCSequence).Count > 0 Then
                    '    ftList.Add(ft)
                    '    Exit For
                    'End If
                Next
            Next
        Else
            Dim gList As List(Of Nuctions.GeneFile) = mi.GetSourceDNAList
            Nuctions.AddFeatures(gList, Features)
            'For Each ft As Nuctions.Feature In Features
            For Each gf As Nuctions.GeneFile In gList
                For Each ga As Nuctions.GeneAnnotation In gf.Features
                    If ga.Feature IsNot Nothing AndAlso Not (ftList.Contains(ga.Feature)) Then ftList.Add(ga.Feature)
                Next
                'If gf.Matches(ft.Sequence).Count > 0 Or gf.Matches(ft.RCSequence).Count > 0 Then
                '    ftList.Add(ft)
                '    Exit For
                'End If
            Next
            'Next
        End If

        Dim ll As LinkLabel
        For Each ll In pnlFeature.Controls
            RemoveHandler ll.MouseClick, AddressOf OnClickFeature
        Next
        pnlFeature.Controls.Clear()
        Dim i As Integer = 0
        For Each ft As Nuctions.Feature In ftList
            ll = New LinkLabel
            ll.Text = ft.Label
            ttDescription.SetToolTip(ll, String.Format("{0}-{1}", ft.Label, ft.Type))
            ll.Height = 24
            ll.Width = 160
            ll.Tag = ft
            ll.Location = New Point((i Mod 3) * 160, (i \ 3) * 28)
            pnlFeature.Controls.Add(ll)
            AddHandler ll.MouseClick, AddressOf OnClickFeature
            i += 1
        Next
    End Sub

    Private Sub UpdateFeatureScreenLink()
        Dim stb As New System.Text.StringBuilder
        For Each ei As FeatureScreenInfo In Me.RelatedChartItem.MolecularInfo.Screen_Features
            stb.Append(ei.Feature.Label + "(" + ei.ScreenMethod.ToString + ");")
        Next
        Dim scrtxt As String = stb.ToString
        If scrtxt.Length = 0 Then
            Me.Screen_Features_LinkLabel.Text = "[Click to select]"
        Else
            Me.Screen_Features_LinkLabel.Text = scrtxt
        End If
    End Sub

    Private Sub RefreshProteinExpressionView()
        'sss()


        Dim mi As DNAInfo = RelatedChartItem.MolecularInfo
        Dim ftList As New List(Of Nuctions.Feature)
        If mi.IsCellNode Then
            Dim cList As List(Of Nuctions.Cell) = mi.GetSourceCellList
            For Each c As Nuctions.Cell In cList
                Nuctions.AddFeatures(c.DNAs, Features)
                For Each gf As Nuctions.GeneFile In c.DNAs
                    For Each ga As Nuctions.GeneAnnotation In gf.Features
                        If ga.Feature IsNot Nothing AndAlso (ga.Feature.Type.ToLower = "cds" Or ga.Feature.Type.ToLower = "gene") AndAlso Not (ftList.Contains(ga.Feature)) Then ftList.Add(ga.Feature)
                    Next
                Next
            Next
        Else
            Dim gList As List(Of Nuctions.GeneFile) = mi.GetSourceDNAList
            Nuctions.AddFeatures(gList, Features)
            For Each gf As Nuctions.GeneFile In gList
                For Each ga As Nuctions.GeneAnnotation In gf.Features
                    If ga.Feature IsNot Nothing AndAlso (ga.Feature.Type.ToLower = "cds" Or ga.Feature.Type.ToLower = "gene") AndAlso Not (ftList.Contains(ga.Feature)) Then ftList.Add(ga.Feature)
                Next
            Next
        End If
        Dim ll As LinkLabel
        For Each ll In pnlExpression.Controls
            RemoveHandler ll.MouseClick, AddressOf OnClickFeature
        Next
        pnlExpression.Controls.Clear()
        Dim i As Integer = 0
        For Each ft As Nuctions.Feature In ftList
            ll = New LinkLabel
            ll.Text = ft.Label
            ttDescription.SetToolTip(ll, String.Format("{0}-{1}{2}", ft.Label, ft.Type))
            ll.Tag = ft
            ll.Location = New Point((i Mod 5) * 100, (i \ 5) * 18)
            ll.Height = 20
            pnlExpression.Controls.Add(ll)
            AddHandler ll.MouseClick, AddressOf OnClickFeature
            i += 1
        Next
    End Sub

    Private Sub OnClickFeature(ByVal sender As Object, ByVal e As MouseEventArgs)
        If ReaderMode Then Exit Sub
        Dim obj As LinkLabel = sender
        Dim ft As Nuctions.Feature = obj.Tag
        Dim Added As Boolean = False
        Dim fsi As FeatureScreenInfo = Nothing

        For Each ei As FeatureScreenInfo In Me.RelatedChartItem.MolecularInfo.Screen_Features
            If ei.Feature Is ft Then
                Added = True
                fsi = ei
                Exit For
            End If
        Next
        If Not Added Then
            fsi = New FeatureScreenInfo
            fsi.Feature = ft
            Select Case e.Button
                Case Windows.Forms.MouseButtons.Left
                    fsi.ScreenMethod = FeatureScreenEnum.Once
                Case Windows.Forms.MouseButtons.Right
                    fsi.ScreenMethod = FeatureScreenEnum.None
                Case Windows.Forms.MouseButtons.Middle
                    fsi.ScreenMethod = FeatureScreenEnum.Maximum
            End Select
            Me.RelatedChartItem.MolecularInfo.Screen_Features.Add(fsi)
        ElseIf Not (fsi Is Nothing) Then
            Me.RelatedChartItem.MolecularInfo.Screen_Features.Remove(fsi)
        End If
        ApplyMode = True
        Accept()
        UpdateFeatureScreenLink()
    End Sub
    Private ModifyUpdating As Boolean = False
    Private Sub RefreshModifyView()
        ModifyUpdating = True
        Select Case Me.RelatedChartItem.MolecularInfo.Modify_Method
            Case Nuctions.ModificationMethodEnum.CIAP
                Me.Modify_CIAP.Checked = True
            Case Nuctions.ModificationMethodEnum.Klewnow
                Me.Modify_Klewnow.Checked = True
            Case Nuctions.ModificationMethodEnum.PNK
                Me.Modify_PNK.Checked = True
            Case Nuctions.ModificationMethodEnum.T4DNAP
                Me.Modify_T4.Checked = True
        End Select
        ModifyUpdating = False
    End Sub
    Private pcrSetting As Boolean = False
    Private Sub RefreshPCRView()
        If Me.RelatedChartItem.MolecularInfo.PCR_FPrimerName Is Nothing OrElse Me.RelatedChartItem.MolecularInfo.PCR_FPrimerName.Length = 0 Then
            Me.RelatedChartItem.MolecularInfo.PCR_FPrimerName = "F"
        End If
        If Me.RelatedChartItem.MolecularInfo.PCR_RPrimerName Is Nothing OrElse Me.RelatedChartItem.MolecularInfo.PCR_RPrimerName.Length = 0 Then
            Me.RelatedChartItem.MolecularInfo.PCR_RPrimerName = "R"
        End If
        Me.PCR_ForwardPrimer_TextBox.Text = Me.RelatedChartItem.MolecularInfo.PCR_ForwardPrimer
        tbFP.Text = Me.RelatedChartItem.MolecularInfo.PCR_FPrimerName
        Me.PCR_ReversePrimer_TextBox.Text = Me.RelatedChartItem.MolecularInfo.PCR_ReversePrimer
        tbRP.Text = Me.RelatedChartItem.MolecularInfo.PCR_RPrimerName

        'adding primer list to the text box.
        tbFP.Items.Clear()
        tbRP.Items.Clear()
        For Each pi As PrimerInfo In Me.RelatedChartItem.Parent.Primers
            tbFP.Items.Add(pi.Name)
            tbRP.Items.Add(pi.Name)
        Next

        pcrSetting = True
        btnOverlap.Text = IIf(Me.RelatedChartItem.MolecularInfo.PCR_Overlap, "OE", "NA")
        pcrSetting = False
        StartAnalyzePrimers()
    End Sub

    Private FreeDesignSetting = False
    Private Sub RefreshFreeDesignView()
        FreeDesignSetting = True
        Me.rtbFreeDesign.Text = RelatedChartItem.MolecularInfo.FreeDesignCode
        Me.tbDesign.Text = RelatedChartItem.MolecularInfo.FreeDesignName
        Me.cbDesign_UseDesigner.Checked = RelatedChartItem.MolecularInfo.UseFreeDesigner
        FreeDesignSetting = False
    End Sub

    Private Sub rtbFreeDesign_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rtbFreeDesign.TextChanged
#If ReaderMode = 0 Then
        If Not FreeDesignSetting Then
            RelatedChartItem.MolecularInfo.UseFreeDesigner = False
            Me.cbDesign_UseDesigner.Checked = False
            Dim EnzList As New List(Of String)
            Dim FreeDesignCode = rtbFreeDesign.Text.Clone
            Nuctions.ParseCode(tbDesign.Text, FreeDesignCode, Features, Me.RelatedChartItem.MolecularInfo.DNAs, EnzList)
            RefreshDNAView()
            Dim EnzCol As New List(Of String)
            EnzCol.AddRange(Me.RelatedChartItem.Parent.EnzymeCol)
            For Each ez As String In EnzList
                If Not (EnzCol.Contains(ez)) Then EnzCol.Add(ez)
            Next
            Me.RelatedChartItem.Reload(Me.RelatedChartItem.MolecularInfo, EnzCol)
            Me.RelatedChartItem.Parent.Draw()
        End If
#End If
    End Sub

    Private Sub StartAnalyzePrimers()
        Dim pmrlist As New Dictionary(Of String, String)
        Dim pfk As String
        Dim prk As String
        If tbFP.Text = tbRP.Text Then
            pfk = tbFP.Text + "F"
            prk = tbRP.Text + "R"
        Else
            pfk = tbFP.Text
            prk = tbRP.Text
        End If
        pmrlist.Add(pfk, Nuctions.TAGCFilter(PCR_ForwardPrimer_TextBox.Text))
        pmrlist.Add(prk, Nuctions.TAGCFilter(PCR_ReversePrimer_TextBox.Text))
        Dim glist As New List(Of Nuctions.GeneFile)
        For Each sr As DNAInfo In Me.RelatedChartItem.MolecularInfo.Source
            For Each gf As Nuctions.GeneFile In sr.DNAs
                glist.Add(gf)
            Next
        Next
        anaPlist = pmrlist
        anaGlist = glist
        Try
            AnalyzePrimers()
            'If thr Is Nothing Then
            '    thr = New Threading.Thread(AddressOf AnalyzePrimers)
            '    thr.Start()
            'Else
            '    thr.Abort()
            '    thr = New Threading.Thread(AddressOf AnalyzePrimers)
            '    thr.Start()
            'End If
        Catch ex As Exception

        End Try

    End Sub

    Dim thr As Threading.Thread
    Dim anaPlist As Dictionary(Of String, String)
    Dim anaGlist As List(Of Nuctions.GeneFile)


    Private Sub AnalyzePrimers()
        pafPCR.AnalyzePrimers(anaPlist, anaGlist, 80 * 0.001, 625 * 0.000000001)
    End Sub

    Private gelSetting As Boolean = False
    Private Sub RefreshGelView()
        gelSetting = True
        cbGel_Solution.Checked = Me.RelatedChartItem.MolecularInfo.SolutionExtration
        Me.Gel_Maximum_Number.Value = Me.RelatedChartItem.MolecularInfo.Gel_Maximun
        Me.Gel_Minimum_Number.Value = Me.RelatedChartItem.MolecularInfo.Gel_Minimum
        '自动列出一系列不同长度的选项
        pnlGel.Controls.Clear()
        Dim ll As LinkLabel
        For Each ll In pnlGel.Controls
            RemoveHandler ll.LinkClicked, AddressOf OnSelectGelLength
        Next
        Dim i As Integer = 0
        Dim nList As New List(Of Integer)
        For Each di As DNAInfo In Me.RelatedChartItem.MolecularInfo.Source
            For Each dna As Nuctions.GeneFile In di.DNAs
                If dna.Length >= 100 And dna.Length <= 200000 And (Not nList.Contains(dna.Length)) Then nList.Add(dna.Length)
            Next
        Next
        nList.Sort()
        For Each j As Integer In nList
            ll = New LinkLabel
            ll.Text = j.ToString
            pnlGel.Controls.Add(ll)
            ll.Location = New Point((i Mod 5) * 100, (i \ 5) * 20)
            i += 1
            AddHandler ll.LinkClicked, AddressOf OnSelectGelLength
        Next
        gelSetting = False
    End Sub
    Private Sub OnSelectGelLength(ByVal sender As Object, ByVal e As EventArgs)
        If ReaderMode Then Exit Sub
        Dim ll As LinkLabel = sender
        Dim ln As Integer = CInt(ll.Text)
        If ln >= 100 And ln <= 200000 Then
            Me.Gel_Maximum_Number.Value = ln
            Me.Gel_Minimum_Number.Value = ln
        End If
        Accept()
    End Sub

    Public Event RequireFeatureScreenView(ByVal sender As Object, ByVal e As EventArgs)

    Private Sub Screen_Features_LinkLabel_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles Screen_Features_LinkLabel.LinkClicked
        Select Case e.Button
            Case MouseButtons.Left
                RaiseEvent RequireFeatureScreenView(Me, New EventArgs)
            Case MouseButtons.Right
                Me.RelatedChartItem.MolecularInfo.Screen_Features.Clear()
                ApplyMode = True
                Accept()
                UpdateFeatureScreenLink()
        End Select
    End Sub

    Public Sub SetFeatureScreen(ByVal fList As List(Of FeatureScreenInfo))

        Dim stb As New System.Text.StringBuilder
        Me.RelatedChartItem.MolecularInfo.Screen_Features = fList
        For Each ei As FeatureScreenInfo In Me.RelatedChartItem.MolecularInfo.Screen_Features
            stb.Append(ei.Feature.Label + "(" + ei.ScreenMethod.ToString + ");")
        Next
        Dim scrtxt As String = stb.ToString
        If scrtxt.Length = 0 Then
            Me.Screen_Features_LinkLabel.Text = "[Click to select]"
        Else
            Me.Screen_Features_LinkLabel.Text = scrtxt
        End If
        ApplyMode = True
    End Sub

    Private EnzRgx As New System.Text.RegularExpressions.Regex("\[([\w\s\-\.]+)\]")
    Private EnzRgxF As New System.Text.RegularExpressions.Regex("\[([\w\s\-\.]+)(|\:([ATGC\s]+))\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    Private EnzRgxR As New System.Text.RegularExpressions.Regex("\<([\w\s\-\.]+)(|\:([ATGC\s]+))\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    Private Sub PCRPrimerKeyDown(_PrimerBox As TextBox)

        Dim value As String = _PrimerBox.Text
        If EnzRgx.IsMatch(value) Then
            For Each m As System.Text.RegularExpressions.Match In EnzRgx.Matches(_PrimerBox.Text)
                For Each re As Nuctions.RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
                    If re.Name.ToLower = m.Groups(1).Value.ToLower Then
                        _PrimerBox.Text = ReplaceSectionWithValue(_PrimerBox.Text, m.Index, m.Length, re.Sequence)
                        _PrimerBox.SelectionStart = m.Index + re.Sequence.Length
                    End If
                Next
                For Each rc In SettingEntry.RecombinationSiteDict.Values
                    If rc.Name.ToLower = m.Groups(1).Value.ToLower Then
                        _PrimerBox.Text = ReplaceSectionWithValue(_PrimerBox.Text, m.Index, m.Length, rc.Sequence)
                        _PrimerBox.SelectionStart = m.Index + rc.Sequence.Length
                    End If
                Next
            Next
        End If
        If EnzRgxF.IsMatch(value) Then
            For Each m As System.Text.RegularExpressions.Match In EnzRgxF.Matches(_PrimerBox.Text)
                For Each re As Nuctions.RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
                    If re.Name.ToLower = m.Groups(1).Value.ToLower Then
                        _PrimerBox.Text = ReplaceSectionWithValue(_PrimerBox.Text, m.Index, m.Length, ReplaceNwithPattern(re.Sequence, m.Groups(3).Value))
                        _PrimerBox.SelectionStart = m.Index + re.Sequence.Length
                    End If
                Next
                For Each rc In SettingEntry.RecombinationSiteDict.Values
                    If rc.Name.ToLower = m.Groups(1).Value.ToLower Then
                        _PrimerBox.Text = ReplaceSectionWithValue(_PrimerBox.Text, m.Index, m.Length, ReplaceNwithPattern(rc.Sequence, m.Groups(3).Value))
                        _PrimerBox.SelectionStart = m.Index + rc.Sequence.Length
                    End If
                Next
            Next
        End If
        If EnzRgxR.IsMatch(value) Then
            For Each m As System.Text.RegularExpressions.Match In EnzRgxR.Matches(_PrimerBox.Text)
                For Each re As Nuctions.RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
                    If re.Name.ToLower = m.Groups(1).Value.ToLower Then
                        _PrimerBox.Text = ReplaceSectionWithValue(_PrimerBox.Text, m.Index, m.Length, Nuctions.ReverseComplementN(ReplaceNwithPattern(re.Sequence, m.Groups(3).Value)))
                        _PrimerBox.SelectionStart = m.Index + re.Sequence.Length
                    End If
                Next
                For Each rc In SettingEntry.RecombinationSiteDict.Values
                    If rc.Name.ToLower = m.Groups(1).Value.ToLower Then
                        _PrimerBox.Text = ReplaceSectionWithValue(_PrimerBox.Text, m.Index, m.Length, Nuctions.ReverseComplementN(ReplaceNwithPattern(rc.Sequence, m.Groups(3).Value)))
                        _PrimerBox.SelectionStart = m.Index + rc.Sequence.Length
                    End If
                Next
            Next
        End If
    End Sub
    Shared Function ReplaceNwithPattern(sequence As String, pattern As String) As String
        Dim stb As New System.Text.StringBuilder
        Dim i As Integer = 0
        Dim ptn As String = Nuctions.TAGCFilter(pattern)
        For Each c In sequence.ToUpper.ToCharArray
            If c = "N"c Then
                If ptn.Length > 0 Then
                    stb.Append(ptn(i))
                    i += 1
                    If i >= ptn.Length Then i = 0
                Else
                    stb.Append("N"c)
                End If
            Else
                stb.Append(c)
            End If
        Next
        Return stb.ToString
    End Function
    Shared Function ReplaceSectionWithValue(sequence As String, index As Integer, length As Integer, pattern As String) As String
        Return String.Format("{0}{1}{2}", sequence.Substring(0, index), pattern, sequence.Substring(index + length))
    End Function
    Private Sub Screen_PCR_F_KeyDown(sender As Object, e As KeyEventArgs) Handles Screen_PCR_F.KeyDown
        If e.KeyCode = Keys.Enter Then PCRPrimerKeyDown(sender)
    End Sub

    Private Sub Screen_PCR_R_KeyDown(sender As Object, e As KeyEventArgs) Handles Screen_PCR_R.KeyDown
        If e.KeyCode = Keys.Enter Then PCRPrimerKeyDown(sender)
    End Sub
    Private Sub PCR_ForwardPrimer_TextBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles PCR_ForwardPrimer_TextBox.KeyDown
        If e.KeyCode = Keys.Enter Then PCRPrimerKeyDown(sender)
        'If EnzRgx.IsMatch(PCR_ForwardPrimer_TextBox.Text) Then
        '    For Each m As System.Text.RegularExpressions.Match In EnzRgx.Matches(PCR_ForwardPrimer_TextBox.Text)
        '        For Each re As Nuctions.RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
        '            If re.Name.ToLower = m.Groups(1).Value.ToLower Then
        '                PCR_ForwardPrimer_TextBox.Text = PCR_ForwardPrimer_TextBox.Text.Replace(m.Groups(0).Value, re.Sequence)
        '                PCR_ForwardPrimer_TextBox.SelectionStart = m.Index + re.Sequence.Length
        '            End If
        '        Next
        '    Next
        'End If
        'ss
    End Sub

    Private Sub PCR_ReversePrimer_TextBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles PCR_ReversePrimer_TextBox.KeyDown
        If e.KeyCode = Keys.Enter Then PCRPrimerKeyDown(sender)
        'If EnzRgx.IsMatch(PCR_ReversePrimer_TextBox.Text) Then
        '    For Each m As System.Text.RegularExpressions.Match In EnzRgx.Matches(PCR_ReversePrimer_TextBox.Text)
        '        For Each re As Nuctions.RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
        '            If re.Name.ToLower = m.Groups(1).Value.ToLower Then
        '                PCR_ReversePrimer_TextBox.Text = PCR_ReversePrimer_TextBox.Text.Replace(m.Groups(0).Value, re.Sequence)
        '                PCR_ReversePrimer_TextBox.SelectionStart = m.Index + re.Sequence.Length
        '            End If
        '        Next
        '    Next
        'End If
        'ss
    End Sub

    Private Sub ll_ViewLarge_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        DNAView.View = View.LargeIcon
    End Sub

    Public Event RequireDNAView(ByVal sender As Object, ByVal e As DNAViewEventArgs)

    Public Event RequirePCRView(ByVal sender As Object, ByVal e As PCRViewEventArgs)

    Public Event RequireCellView(ByVal sender As Object, ByVal e As CellViewEventArgs)

    Private Sub ll_ViewDetails_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ll_ViewDetails.LinkClicked
        '在酶切位点分析模式下 不能查看DNA
        If Me.RelatedChartItem.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.EnzymeAnalysis Then Exit Sub
        If RelatedChartItem.MolecularInfo.Cells.Count > 0 Then
            Dim cList As New List(Of Nuctions.Cell)

            If RelatedChartItem.MolecularInfo.DNAs.Count > 0 Then
                Dim c As New Nuctions.Cell With {.Host = New Nuctions.Host With {.Name = "In Vitro"}}
                For Each g As Nuctions.GeneFile In RelatedChartItem.MolecularInfo.DNAs
                    c.Add(g)
                Next
                cList.Add(c)
            End If
            cList.AddRange(RelatedChartItem.MolecularInfo.Cells)
            RaiseEvent RequireCellView(Me, New CellViewEventArgs(cList, RelatedChartItem.MolecularInfo.FetchedEnzymes))
        Else
            Dim glist As New List(Of Nuctions.GeneFile)
            For Each g As Nuctions.GeneFile In RelatedChartItem.MolecularInfo.DNAs
                glist.Add(g)
            Next
            RaiseEvent RequireDNAView(Me, New DNAViewEventArgs(glist, RelatedChartItem.MolecularInfo.FetchedEnzymes))
        End If

    End Sub

    Private Sub btn_RCF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBothPrimer.Click
        If ReaderMode Then Exit Sub
        Dim glist As New List(Of Nuctions.GeneFile)
        For Each di As DNAInfo In RelatedChartItem.MolecularInfo.Source
            For Each g As Nuctions.GeneFile In di.DNAs
                glist.Add(g)
            Next
            For Each c As Nuctions.Cell In di.Cells
                glist.AddRange(c.DNAs)
            Next
        Next
        Dim primers As New Dictionary(Of String, String)

        If tbFP.Text = tbRP.Text Then
            tbFP.Text += "F"
            tbRP.Text += "R"
        End If
        Dim fp As String = PCR_ForwardPrimer_TextBox.Text
        Dim rp As String = PCR_ReversePrimer_TextBox.Text
        Dim idx As Integer
        idx = fp.LastIndexOf(">")
        If idx > -1 Then
            PCR_ForwardPrimer_TextBox.Text = Nuctions.TAGCFilter(fp.Substring(0, idx)) + ">" + Nuctions.TAGCFilter(fp.Substring(idx, fp.Length - idx))
        End If
        idx = rp.LastIndexOf(">")
        If idx > -1 Then
            PCR_ReversePrimer_TextBox.Text = Nuctions.TAGCFilter(rp.Substring(0, idx)) + ">" + Nuctions.TAGCFilter(rp.Substring(idx, rp.Length - idx))
        End If
        primers.Add(tbFP.Text, PCR_ForwardPrimer_TextBox.Text)
        primers.Add(tbRP.Text, PCR_ReversePrimer_TextBox.Text)

        RaiseEvent RequirePCRView(Me, New PCRViewEventArgs(glist, primers))
    End Sub

    Private Sub frmProperty_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.F1
                Me.Prop_Name.Focus()
                Me.Prop_Name.SelectAll()
            Case Keys.F2
                Me.rtb_Description.Focus()
                Me.rtb_Description.SelectAll()
            Case Keys.Enter
                If Control.ModifierKeys = Keys.Control Then
                    Accept()
                End If
            Case Keys.Escape
                'Me.Close()
        End Select
    End Sub


    Private Sub Screen_Freatures_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_Freatures.CheckedChanged
        Screen_ViewChanged()
    End Sub

    Private Sub Screen_PCR_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_PCR.CheckedChanged
        Screen_ViewChanged()
    End Sub

    Private Sub Screen_ViewChanged()
        If scrSetting Then Exit Sub
        If Screen_PCR.Checked Then
            If RelatedChartItem IsNot Nothing Then RelatedChartItem.MolecularInfo.Screen_Mode = Nuctions.ScreenModeEnum.PCR
            Me.Screen_PCR_Panel.Visible = True
            Me.Screen_Features_LinkLabel.Visible = False
            Me.pnlFeature.Visible = False
            Me.pnlScreenPCR.Visible = True
            ApplyMode = True
            BeginCalculateScreenPCR()
        ElseIf Screen_Freatures.Checked Then
            If RelatedChartItem IsNot Nothing Then RelatedChartItem.MolecularInfo.Screen_Mode = Nuctions.ScreenModeEnum.Features
            Me.Screen_Features_LinkLabel.Visible = True
            Me.Screen_PCR_Panel.Visible = False
            Me.pnlFeature.Visible = True
            Me.pnlScreenPCR.Visible = False
            ApplyMode = True
        End If
    End Sub

    Private Sub Screen_PCR_RCF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_PCR_RCF.Click
        'If ReaderMode Then Exit Sub
        Dim glist As New List(Of Nuctions.GeneFile)
        For Each di As DNAInfo In RelatedChartItem.MolecularInfo.Source
            For Each g As Nuctions.GeneFile In di.DNAs
                glist.Add(g)
            Next
        Next
        Dim primers As New Dictionary(Of String, String)

        If tbSCRFP.Text = tbSCRRP.Text Then
            tbSCRFP.Text += "F"
            tbSCRFP.Text += "R"
        End If
        Dim fp As String = Screen_PCR_F.Text
        Dim rp As String = Screen_PCR_R.Text
        Dim idx As Integer
        idx = fp.LastIndexOf(">")
        If idx > -1 Then
            PCR_ForwardPrimer_TextBox.Text = Nuctions.TAGCFilter(fp.Substring(0, idx)) + ">" + Nuctions.TAGCFilter(fp.Substring(idx, fp.Length - idx))
        End If
        idx = rp.LastIndexOf(">")
        If idx > -1 Then
            PCR_ReversePrimer_TextBox.Text = Nuctions.TAGCFilter(rp.Substring(0, idx)) + ">" + Nuctions.TAGCFilter(rp.Substring(idx, rp.Length - idx))
        End If
        primers.Add(tbSCRFP.Text, Screen_PCR_F.Text)
        primers.Add(tbSCRRP.Text, Screen_PCR_R.Text)

        Me.RelatedChartItem.MolecularInfo.PCR_RPrimerName = PCR_ForwardPrimer_TextBox.Text
        RaiseEvent RequirePCRView(Me, New PCRViewEventArgs(glist, primers))
    End Sub

    Private Sub Screen_PCR_F_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_PCR_F.TextChanged
        If scrSetting Then Exit Sub
        Me.ApplyMode = True
        BeginCalculateScreenPCR()
    End Sub

    Private Sub Screen_PCR_R_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_PCR_R.TextChanged
        If scrSetting Then Exit Sub
        Me.ApplyMode = True
        BeginCalculateScreenPCR()
    End Sub

    Private Sub Screen_PCR_nudMax_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_PCR_nudMax.ValueChanged
        Me.ApplyMode = True
    End Sub

    Private Sub Screen_PCR_nudMin_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_PCR_nudMin.ValueChanged
        Me.ApplyMode = True
    End Sub

    Private Sub llApply_DoubleClick(sender As Object, e As System.EventArgs) Handles llApply.DoubleClick
        mApplyMode = True
        Accept()
        WorkControl.RecalculateAllChildren()
    End Sub

    Private Sub llApply_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llApply.LinkClicked
        mApplyMode = True
        Accept()
    End Sub

    Private Sub llCancel_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llCancel.LinkClicked
        Cancel()
    End Sub

    Public Sub Cancel()
        If DNACopy Is Nothing Then Exit Sub
        RelatedChartItem.MolecularInfo.Recover(DNACopy)
        RelatedChartItem.Reload(RelatedChartItem.MolecularInfo, Me.RelatedChartItem.Parent.EnzymeCol)
        Reload()
    End Sub
    Private Sub Prop_Name_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Prop_Name.TextChanged
        RelatedChartItem.MolecularInfo.Name = Prop_Name.Text
        If Not (Me.RelatedChartItem Is Nothing) AndAlso Not (Me.RelatedChartItem.MolecularInfo Is Nothing) Then
            With Me.RelatedChartItem.MolecularInfo
                Select Case .MolecularOperation
                    Case Nuctions.MolecularOperationEnum.FreeDesign
                        RelatedChartItem.MolecularInfo.FreeDesignName = RelatedChartItem.MolecularInfo.Name
                        If .DNAs.Count = 1 Then
                            Dim gf As Nuctions.GeneFile = .DNAs(1)
                            gf.Name = RelatedChartItem.MolecularInfo.Name
                        End If
                    Case Nuctions.MolecularOperationEnum.Vector
                        If .DNAs.Count = 1 Then
                            Dim gf As Nuctions.GeneFile = .DNAs(1)
                            gf.Name = RelatedChartItem.MolecularInfo.Name
                        End If
                End Select
            End With
        End If
        ChangeValue()
    End Sub

    Private Sub Gel_Minimum_Number_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Gel_Minimum_Number.ValueChanged, Gel_Maximum_Number.ValueChanged
        ApplyMode = True
    End Sub

    Private Sub PropertyControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        DNAView.SmallImageList = SettingEntry.SmallIconList
    End Sub

    Private Sub EnzymeControl_CloseTab(ByVal sender As Object, ByVal e As System.EventArgs) Handles EnzymeControl.CloseTab
        HideEnzymeTab()
    End Sub

    Private Sub EnzymeControl_SetRestrictSite(ByVal sender As Object, ByVal e As RestrictionEnzymeView.RESiteEventArgs) Handles EnzymeControl.SetRestrictSite
        SetEnzymes(e.RESites)
    End Sub

    Private rbRecSetting As Boolean = False
    Private Sub rbRec_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If rbRecSetting Then Exit Sub
        ApplyMode = True
    End Sub

    Protected Overrides Sub Finalize()
        For Each rb As RadioButton In RBRec.Values
            RemoveHandler rb.CheckedChanged, AddressOf Me.rbRec_CheckedChanged
        Next
        MyBase.Finalize()
    End Sub

    Private scrSetting As Boolean = False

    Private Sub cbScreenCircular_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbScreenCircular.CheckedChanged
        If scrSetting Then Exit Sub
        Me.RelatedChartItem.MolecularInfo.Screen_OnlyCircular = cbScreenCircular.Checked
        Me.ApplyMode = True
    End Sub

    Public Event RequireSelectDNAView(ByVal sender As Object, ByVal e As DNAViewEventArgs)

    Private Sub AddToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddToolStripMenuItem.Click
        AnalysisAddSequence()
    End Sub

    Private Sub AnalysisAddSequence()
#If ReaderMode = 0 Then
        Dim glist As New List(Of Nuctions.GeneFile)
        For Each dnai As DNAInfo In Me.RelatedChartItem.MolecularInfo.Source
            For Each gf As Nuctions.GeneFile In dnai.DNAs
                glist.Add(gf)
            Next
        Next
        RaiseEvent RequireSelectDNAView(Me, New DNAViewEventArgs(glist, Me.RelatedChartItem.MolecularInfo.FetchedEnzymes))
#End If

    End Sub
    Private Sub AnalysisRemoveSequence()
#If ReaderMode = 0 Then
        If dgvEnzymeAnalysis.SelectedRows.Count > 0 Then
            Dim rmList As New List(Of DataGridViewRow)
            For Each row As DataGridViewRow In dgvEnzymeAnalysis.SelectedRows
                rmList.Add(row)
            Next
            For Each row As DataGridViewRow In rmList
                dgvEnzymeAnalysis.Rows.Remove(row)
            Next
        ElseIf dgvEnzymeAnalysis.SelectedCells.Count > 0 Then
            Dim rmList As New List(Of DataGridViewRow)
            For Each c As DataGridViewCell In dgvEnzymeAnalysis.SelectedCells
                If c.RowIndex > -1 AndAlso Not rmList.Contains(c.OwningRow) Then rmList.Add(c.OwningRow)
            Next
            For Each row As DataGridViewRow In rmList
                dgvEnzymeAnalysis.Rows.Remove(row)
            Next
        End If
#End If
    End Sub

    Public Sub AddEnzymeAnalysisSequence(ByVal gf As Nuctions.GeneFile, ByVal vRegion As String)
        Dim i As Integer = dgvEnzymeAnalysis.Rows.Add
        Dim row As DataGridViewRow
        row = dgvEnzymeAnalysis.Rows(i)
        row.Tag = gf
        row.Cells(0).Value = gf.Name
        row.Cells(1).Value = vRegion
        row.Cells(2).Value = True
        row.Cells(3).Value = "="
        row.Cells(4).Value = "0"
        ApplyMode = True
    End Sub

    Public Sub RefreshEnzymeAnalysisView()
        eaSetting = True
        rtbEnzymeAnalysisResults.Visible = True
        ll_ViewDetails.Visible = False
        dgvEnzymeAnalysis.Rows.Clear()
        For Each eai As EnzymeAnalysisItem In Me.RelatedChartItem.MolecularInfo.EnzymeAnalysisParamters
            Dim i As Integer = dgvEnzymeAnalysis.Rows.Add
            Dim row As DataGridViewRow
            row = dgvEnzymeAnalysis.Rows(i)
            row.Tag = eai.GeneFile
            row.Cells(0).Value = eai.GeneFile.Name
            row.Cells(1).Value = eai.Region
            row.Cells(2).Value = eai.Use
            Select Case eai.Method
                Case EnzymeAnalysisEnum.Equal
                    row.Cells(3).Value = "="
                Case EnzymeAnalysisEnum.Greater
                    row.Cells(3).Value = ">"
                Case EnzymeAnalysisEnum.Less
                    row.Cells(3).Value = "<"
            End Select
            row.Cells(4).Value = eai.Value.ToString
        Next
        Dim stb As New System.Text.StringBuilder
        For Each es As String In Me.RelatedChartItem.MolecularInfo.FetchedEnzymes
            stb.Append(es)
            stb.Append(" ")
        Next
        rtbEnzymeAnalysisResults.Text = stb.ToString
        eaSetting = False
    End Sub

    Public Function ReadEnzymeAnalysisItems() As List(Of EnzymeAnalysisItem)
        Dim row As DataGridViewRow
        Dim eai As EnzymeAnalysisItem
        Dim eList As New List(Of EnzymeAnalysisItem)
        For Each row In dgvEnzymeAnalysis.Rows
            eai = New EnzymeAnalysisItem
            eai.GeneFile = row.Tag
            eai.Region = row.Cells(1).Value
            eai.Use = row.Cells(2).Value
            Select Case row.Cells(3).Value
                Case "="
                    eai.Method = EnzymeAnalysisEnum.Equal
                Case ">"
                    eai.Method = EnzymeAnalysisEnum.Greater
                Case "<"
                    eai.Method = EnzymeAnalysisEnum.Less
            End Select
            Dim i As Integer
            Try
                i = CInt(row.Cells(4).Value)
            Catch ex As Exception
                row.Cells(4).Value = 0
                i = 0
            End Try
            eai.Value = i
            eList.Add(eai)
        Next
        Return eList
    End Function

    Dim eaSetting As Boolean = False
    Private Sub dgvEnzymeAnalysis_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvEnzymeAnalysis.CellValueChanged
        If eaSetting Then Exit Sub
        If ReaderMode Then Exit Sub
        ApplyMode = True
    End Sub

    Private Sub RemoveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveToolStripMenuItem.Click
        AnalysisRemoveSequence()
    End Sub
    Friend Event ReqireWorkControl(ByVal sender As Object, ByVal e As WorkControlEventArgs)
    Friend ReadOnly Property WorkControl As WorkControl
        Get
            Dim wce As New WorkControlEventArgs
            RaiseEvent ReqireWorkControl(Me, wce)
            Return wce.WorkControl
        End Get
    End Property
    Public Event ReqireFeatures(ByVal sender As Object, ByVal e As FeatureEventArgs)
    Public ReadOnly Property Features() As List(Of Nuctions.Feature)
        Get
            Dim fe As New FeatureEventArgs
            RaiseEvent ReqireFeatures(Me, fe)
            Return fe.Features
        End Get
    End Property

    Private Sub rbLig2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ApplyMode = True
    End Sub

    Private Sub tbSCRFP_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbSCRFP.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim tb As TextBox = Screen_PCR_F
            Dim sd As ComboBox = sender
            Select Case sd.Text.ToLower
                Case "m13f"
                    tb.Text = ">TGTAAAACGACGGCCAGT"
                    sd.Text = "M13F"
                Case "m13r"
                    tb.Text = ">CAGGAAACAGCTATGACC"
                    sd.Text = "M13R"
                Case "m13-20"
                    tb.Text = ">GTAAAACGACGGCCAGT"
                    sd.Text = "M13-20"
                Case "m13-26"
                    tb.Text = ">CAGGAAACAGCTATGAC"
                    sd.Text = "M13-26"
                Case "m13-40"
                    tb.Text = ">GTTTTCCCAGTCACGAC"
                    sd.Text = "M13-40"
                Case "m13-46"
                    tb.Text = ">GCCAGGGTTTTCCCAGTCACGA"
                    sd.Text = "M13-46"
                Case "m13-47"
                    tb.Text = ">CGCCAGGGTTTTCCCAGTCACGAC"
                    sd.Text = "M13-47"
                Case "m13-48"
                    tb.Text = ">AGCGGATAACAATTTCACACAGGA"
                    sd.Text = "M13-48"
                Case "m13-96"
                    tb.Text = ">CCCTCATAGTTAGCGTAACG"
                    sd.Text = "M13-96"
                Case "t7pro", "t7 promoter"
                    tb.Text = ">TAATACGACTCACTATAGGG"
                    sd.Text = "T7Pro"
                Case "t7ter", "t7 terminator"
                    tb.Text = ">TGCTAGTTATTGCTCAGCGG"
                    sd.Text = "T7Ter"
                Case "sp6"
                    tb.Text = ">CATACGATTTAGGTGACACTATAG"
                    sd.Text = "SP6"
                Case "t7"
                    tb.Text = ">TAATACGACTCACTATAGGGAGA"
                    sd.Text = "T7"
                Case "t3"
                    tb.Text = ">GCGCGAAATTAACCCTCACTAAAG"
                    sd.Text = "T3"
                Case Else
                    For Each ci As ChartItem In Me.RelatedChartItem.Parent.Items
                        If ci Is RelatedChartItem Then Continue For
                        If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.PCR Then
                            If ci.MolecularInfo.PCR_FPrimerName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.PCR_ForwardPrimer : sd.Text = ci.MolecularInfo.PCR_FPrimerName
                            If ci.MolecularInfo.PCR_RPrimerName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.PCR_ReversePrimer : sd.Text = ci.MolecularInfo.PCR_RPrimerName
                        ElseIf ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.Screen And ci.MolecularInfo.Screen_Mode = Nuctions.ScreenModeEnum.PCR Then
                            If ci.MolecularInfo.Screen_FName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.Screen_FPrimer : sd.Text = ci.MolecularInfo.Screen_FName
                            If ci.MolecularInfo.Screen_RName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.Screen_RPrimer : sd.Text = ci.MolecularInfo.Screen_RName
                        End If
                    Next
            End Select
        End If
    End Sub

    Private Sub tbSCRFP_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbSCRFP.TextChanged
        ApplyMode = True
    End Sub

    Private Sub tbSCRRP_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbSCRRP.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim tb As TextBox = Screen_PCR_R
            Dim sd As ComboBox = sender
            Select Case sd.Text.ToLower
                Case "m13f"
                    tb.Text = ">TGTAAAACGACGGCCAGT"
                    sd.Text = "M13F"
                Case "m13r"
                    tb.Text = ">CAGGAAACAGCTATGACC"
                    sd.Text = "M13R"
                Case "m13-20"
                    tb.Text = ">GTAAAACGACGGCCAGT"
                    sd.Text = "M13-20"
                Case "m13-26"
                    tb.Text = ">CAGGAAACAGCTATGAC"
                    sd.Text = "M13-26"
                Case "m13-40"
                    tb.Text = ">GTTTTCCCAGTCACGAC"
                    sd.Text = "M13-40"
                Case "m13-46"
                    tb.Text = ">GCCAGGGTTTTCCCAGTCACGA"
                    sd.Text = "M13-46"
                Case "m13-47"
                    tb.Text = ">CGCCAGGGTTTTCCCAGTCACGAC"
                    sd.Text = "M13-47"
                Case "m13-48"
                    tb.Text = ">AGCGGATAACAATTTCACACAGGA"
                    sd.Text = "M13-48"
                Case "m13-96"
                    tb.Text = ">CCCTCATAGTTAGCGTAACG"
                    sd.Text = "M13-96"
                Case "t7pro", "t7 promoter"
                    tb.Text = ">TAATACGACTCACTATAGGG"
                    sd.Text = "T7Pro"
                Case "t7ter", "t7 terminator"
                    tb.Text = ">TGCTAGTTATTGCTCAGCGG"
                    sd.Text = "T7Ter"
                Case "sp6"
                    tb.Text = ">CATACGATTTAGGTGACACTATAG"
                    sd.Text = "SP6"
                Case "t7"
                    tb.Text = ">TAATACGACTCACTATAGGGAGA"
                    sd.Text = "T7"
                Case "t3"
                    tb.Text = ">GCGCGAAATTAACCCTCACTAAAG"
                    sd.Text = "T3"
                Case Else
                    For Each ci As ChartItem In Me.RelatedChartItem.Parent.Items
                        If ci Is RelatedChartItem Then Continue For
                        If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.PCR Then
                            If ci.MolecularInfo.PCR_FPrimerName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.PCR_ForwardPrimer : sd.Text = ci.MolecularInfo.PCR_FPrimerName
                            If ci.MolecularInfo.PCR_RPrimerName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.PCR_ReversePrimer : sd.Text = ci.MolecularInfo.PCR_RPrimerName
                        ElseIf ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.Screen And ci.MolecularInfo.Screen_Mode = Nuctions.ScreenModeEnum.PCR Then
                            If ci.MolecularInfo.Screen_FName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.Screen_FPrimer : sd.Text = ci.MolecularInfo.Screen_FName
                            If ci.MolecularInfo.Screen_RName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.Screen_RPrimer : sd.Text = ci.MolecularInfo.Screen_RName
                        End If
                    Next
            End Select
        End If
    End Sub

    Private Sub tbSCRRP_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbSCRRP.TextChanged
        ApplyMode = True
    End Sub

    Dim desSetting As Boolean = False

    Private Sub rtb_Description_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles rtb_Description.PreviewKeyDown
        'If ModifierKeys = Keys.Control AndAlso e.KeyCode = Keys.V Then rtb_Description.Paste()
    End Sub
    Private Sub rtb_Description_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rtb_Description.TextChanged
        If desSetting Then Exit Sub
        Me.RelatedChartItem.MolecularInfo.OperationDescription = rtb_Description.Text
    End Sub

    '管理显示尺寸的设置
    Dim pxkSetting As Boolean = False
    Private Sub cbRealSize_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbRealSize.CheckedChanged
        If pxkSetting Then Exit Sub
        Me.RelatedChartItem.MolecularInfo.RealSize = cbRealSize.Checked
        Me.RelatedChartItem.Reload(RelatedChartItem.MolecularInfo, Me.RelatedChartItem.Parent.EnzymeCol)
        Me.RelatedChartItem.Parent.Draw()
        Me.RelatedChartItem.Parent.Update()
        'RefreshDNAView()
    End Sub
    Private Sub snbPixelPerKBP_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles snbPixelPerKBP.ValueChanged
        If pxkSetting Then Exit Sub
        If Not (Me.RelatedChartItem Is Nothing) Then
            Me.RelatedChartItem.MolecularInfo.PixelPerKBP = CInt(snbPixelPerKBP.Value)
            Me.RelatedChartItem.Reload(RelatedChartItem.MolecularInfo, Me.RelatedChartItem.Parent.EnzymeCol)
            Me.RelatedChartItem.Parent.Draw()
            Me.RelatedChartItem.Parent.Update()
            RefreshDNAView()
        End If
    End Sub

    Private Sub File_FileName_LinkLabel_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles File_FileName_LinkLabel.LinkClicked
        If ReaderMode Then Exit Sub
        Try
            If IO.File.Exists(RelatedChartItem.MolecularInfo.File_Filename) Then
                ofdVector.FileName = RelatedChartItem.MolecularInfo.File_Filename
            End If
        Catch ex As Exception

        End Try

        Try
            If ofdVector.ShowDialog = DialogResult.OK Then
                RelatedChartItem.MolecularInfo.File_Filename = ofdVector.FileName
                Dim fi As New IO.FileInfo(RelatedChartItem.MolecularInfo.File_Filename)
                If fi.Extension.ToLower = ".gb" Then
                    RelatedChartItem.MolecularInfo.DNAs.Clear()
                    RelatedChartItem.MolecularInfo.DNAs.Add(Nuctions.GeneFile.LoadFromGeneBankFile(fi.FullName))
                Else
                    RelatedChartItem.MolecularInfo.DNAs.Clear()
                    Dim gf As New Nuctions.GeneFile
                    gf.Sequence = Nuctions.TAGCFilter(IO.File.ReadAllText(fi.FullName))
                    gf.End_F = "*B"
                    gf.End_R = "*B"
                    RelatedChartItem.MolecularInfo.DNAs.Add(gf)
                End If
                RelatedChartItem.Reload(RelatedChartItem.MolecularInfo, Me.RelatedChartItem.Parent.EnzymeCol)
                RelatedChartItem.Parent.Draw()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Dim enzSetting As Boolean = False

    Private Sub tbEnzymes_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbEnzymes.KeyDown
        If ReaderMode Then Exit Sub
        If e.KeyCode = Keys.Enter Then
            Accept()
            enzSetting = True
            If RelatedChartItem.MolecularInfo.Enzyme_Enzymes.Count > 0 Then
                tbEnzymes.Text = Enzyme_Enzymes_LinkLabel.Text
            Else
                tbEnzymes.Text = ""
            End If

            enzSetting = False
        End If
    End Sub
    Private Sub tbEnzymes_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbEnzymes.TextChanged
        If enzSetting Then Exit Sub
        If ReaderMode Then Exit Sub
        Dim str As String() = tbEnzymes.Text.ToLower.Split(" ")
        Dim stList As New List(Of String)
        stList.AddRange(str)

        Dim stb As New System.Text.StringBuilder
        Me.RelatedChartItem.MolecularInfo.Enzyme_Enzymes.Clear()
        For Each key As Nuctions.RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
            If stList.IndexOf(key.Name.ToLower) > -1 Then
                Me.RelatedChartItem.MolecularInfo.Enzyme_Enzymes.Add(key.Name)
                stb.Append(key.Name)
                stb.Append(" ")
            End If
        Next
        Me.Enzyme_Enzymes_LinkLabel.Text = stb.ToString
        If Me.Enzyme_Enzymes_LinkLabel.Text.Length = 0 Then
            Me.Enzyme_Enzymes_LinkLabel.Text = "[Click to Select]"
        End If
        ApplyMode = True
        ChangeValue()
    End Sub

    Private Sub PCR_Primer_TextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles PCR_ForwardPrimer_TextBox.TextChanged, PCR_ReversePrimer_TextBox.TextChanged, tbRP.TextChanged, tbFP.TextChanged
        Me.ApplyMode = True
        lblFCount.Text = Nuctions.TAGCFilter(PCR_ForwardPrimer_TextBox.Text).Length.ToString
        lblRCount.Text = Nuctions.TAGCFilter(PCR_ReversePrimer_TextBox.Text).Length.ToString
    End Sub
    Private Sub PCR_ForwardPrimer_TextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PCR_ForwardPrimer_TextBox.TextChanged
        Me.ApplyMode = True
        lblFCount.Text = Nuctions.TAGCFilter(PCR_ForwardPrimer_TextBox.Text).Length.ToString
        StartAnalyzePrimers()
    End Sub

    Private Sub PCR_ReversePrimer_TextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PCR_ReversePrimer_TextBox.TextChanged
        Me.ApplyMode = True
        lblRCount.Text = Nuctions.TAGCFilter(PCR_ReversePrimer_TextBox.Text).Length.ToString
        StartAnalyzePrimers()
    End Sub

    Private Sub tbFP_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbFP.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim tb As TextBox = PCR_ForwardPrimer_TextBox
            Dim sd As ComboBox = sender
            Select Case tbFP.Text.ToLower
                Case "m13f"
                    tb.Text = ">TGTAAAACGACGGCCAGT"
                    sd.Text = "M13F"
                Case "m13r"
                    tb.Text = ">CAGGAAACAGCTATGACC"
                    sd.Text = "M13R"
                Case "m13-20"
                    tb.Text = ">GTAAAACGACGGCCAGT"
                    sd.Text = "M13-20"
                Case "m13-26"
                    tb.Text = ">CAGGAAACAGCTATGAC"
                    sd.Text = "M13-26"
                Case "m13-40"
                    tb.Text = ">GTTTTCCCAGTCACGAC"
                    sd.Text = "M13-40"
                Case "m13-46"
                    tb.Text = ">GCCAGGGTTTTCCCAGTCACGA"
                    sd.Text = "M13-46"
                Case "m13-47"
                    tb.Text = ">CGCCAGGGTTTTCCCAGTCACGAC"
                    sd.Text = "M13-47"
                Case "m13-48"
                    tb.Text = ">AGCGGATAACAATTTCACACAGGA"
                    sd.Text = "M13-48"
                Case "m13-96"
                    tb.Text = ">CCCTCATAGTTAGCGTAACG"
                    sd.Text = "M13-96"
                Case "t7pro", "t7 promoter"
                    tb.Text = ">TAATACGACTCACTATAGGG"
                    sd.Text = "T7Pro"
                Case "t7ter", "t7 terminator"
                    tb.Text = ">TGCTAGTTATTGCTCAGCGG"
                    sd.Text = "T7Ter"
                Case "sp6"
                    tb.Text = ">CATACGATTTAGGTGACACTATAG"
                    sd.Text = "SP6"
                Case "t7"
                    tb.Text = ">TAATACGACTCACTATAGGGAGA"
                    sd.Text = "T7"
                Case "t3"
                    tb.Text = ">GCGCGAAATTAACCCTCACTAAAG"
                    sd.Text = "T3"
                Case Else
                    For Each ci As ChartItem In Me.RelatedChartItem.Parent.Items
                        If ci Is RelatedChartItem Then Continue For
                        If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.PCR Then
                            If ci.MolecularInfo.PCR_FPrimerName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.PCR_ForwardPrimer : sd.Text = ci.MolecularInfo.PCR_FPrimerName
                            If ci.MolecularInfo.PCR_RPrimerName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.PCR_ReversePrimer : sd.Text = ci.MolecularInfo.PCR_RPrimerName
                        ElseIf ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.Screen And ci.MolecularInfo.Screen_Mode = Nuctions.ScreenModeEnum.PCR Then
                            If ci.MolecularInfo.Screen_FName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.Screen_FPrimer : sd.Text = ci.MolecularInfo.Screen_FName
                            If ci.MolecularInfo.Screen_RName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.Screen_RPrimer : sd.Text = ci.MolecularInfo.Screen_RName
                        End If
                    Next
            End Select
        End If
    End Sub

    Private Sub tbFP_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles tbFP.SelectedIndexChanged
        If tbFP.SelectedIndex > -1 Then
            PCR_ForwardPrimer_TextBox.Text = Me.RelatedChartItem.Parent.Primers(tbFP.SelectedIndex).Sequence
        End If
    End Sub

    Private Sub tbFP_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbFP.TextChanged
        Me.ApplyMode = True
    End Sub

    Private Sub tbRP_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbRP.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim tb As TextBox = PCR_ReversePrimer_TextBox
            Dim sd As ComboBox = sender
            Select Case sd.Text.ToLower
                Case "m13f"
                    tb.Text = ">TGTAAAACGACGGCCAGT"
                    sd.Text = "M13F"
                Case "m13r"
                    tb.Text = ">CAGGAAACAGCTATGACC"
                    sd.Text = "M13R"
                Case "m13-20"
                    tb.Text = ">GTAAAACGACGGCCAGT"
                    sd.Text = "M13-20"
                Case "m13-26"
                    tb.Text = ">CAGGAAACAGCTATGAC"
                    sd.Text = "M13-26"
                Case "m13-40"
                    tb.Text = ">GTTTTCCCAGTCACGAC"
                    sd.Text = "M13-40"
                Case "m13-46"
                    tb.Text = ">GCCAGGGTTTTCCCAGTCACGA"
                    sd.Text = "M13-46"
                Case "m13-47"
                    tb.Text = ">CGCCAGGGTTTTCCCAGTCACGAC"
                    sd.Text = "M13-47"
                Case "m13-48"
                    tb.Text = ">AGCGGATAACAATTTCACACAGGA"
                    sd.Text = "M13-48"
                Case "m13-96"
                    tb.Text = ">CCCTCATAGTTAGCGTAACG"
                    sd.Text = "M13-96"
                Case "t7pro", "t7 promoter"
                    tb.Text = ">TAATACGACTCACTATAGGG"
                    sd.Text = "T7Pro"
                Case "t7ter", "t7 terminator"
                    tb.Text = ">TGCTAGTTATTGCTCAGCGG"
                    sd.Text = "T7Ter"
                Case "sp6"
                    tb.Text = ">CATACGATTTAGGTGACACTATAG"
                    sd.Text = "SP6"
                Case "t7"
                    tb.Text = ">TAATACGACTCACTATAGGGAGA"
                    sd.Text = "T7"
                Case "t3"
                    tb.Text = ">GCGCGAAATTAACCCTCACTAAAG"
                    sd.Text = "T3"
                Case Else
                    For Each ci As ChartItem In Me.RelatedChartItem.Parent.Items
                        If ci Is RelatedChartItem Then Continue For
                        If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.PCR Then
                            If ci.MolecularInfo.PCR_FPrimerName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.PCR_ForwardPrimer : sd.Text = ci.MolecularInfo.PCR_FPrimerName
                            If ci.MolecularInfo.PCR_RPrimerName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.PCR_ReversePrimer : sd.Text = ci.MolecularInfo.PCR_RPrimerName
                        ElseIf ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.Screen And ci.MolecularInfo.Screen_Mode = Nuctions.ScreenModeEnum.PCR Then
                            If ci.MolecularInfo.Screen_FName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.Screen_FPrimer : sd.Text = ci.MolecularInfo.Screen_FName
                            If ci.MolecularInfo.Screen_RName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.Screen_RPrimer : sd.Text = ci.MolecularInfo.Screen_RName
                        End If
                    Next
            End Select
        End If
    End Sub

    Private Sub tbRP_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles tbRP.SelectedIndexChanged
        If tbRP.SelectedIndex > -1 Then
            PCR_ReversePrimer_TextBox.Text = Me.RelatedChartItem.Parent.Primers(tbRP.SelectedIndex).Sequence
        End If
    End Sub

    Private Sub tbRP_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbRP.TextChanged
        Me.ApplyMode = True
    End Sub
    Private Sub RefreshMerge()
        mgSetting = True
        cbMergeSignificant.Checked = RelatedChartItem.MolecularInfo.OnlySignificant
        cbMergeExtend.Checked = RelatedChartItem.MolecularInfo.OnlyExtend
        mgSetting = False
    End Sub
    Dim mgSetting As Boolean = False
    Private Sub cbMergeSignificant_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMergeSignificant.CheckedChanged
        If mgSetting Then Exit Sub
        RelatedChartItem.MolecularInfo.OnlySignificant = cbMergeSignificant.Checked
        ApplyMode = True
    End Sub

    Private Sub cbMergeExtend_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMergeExtend.CheckedChanged
        If mgSetting Then Exit Sub
        RelatedChartItem.MolecularInfo.OnlyExtend = cbMergeExtend.Checked
        ApplyMode = True
    End Sub

    Dim Setting_finished As Boolean = False

    Private Sub rbProgress_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUnstarted.CheckedChanged, rbInprogress.CheckedChanged, rbFinished.CheckedChanged, rbObsolete.CheckedChanged
        If Setting_finished Then Exit Sub
        Dim sdr As RadioButton = sender
        If sdr.Tag Is Nothing Then Exit Sub
        RelatedChartItem.MolecularInfo.Progress = sdr.Tag
        RelatedChartItem.Reload(RelatedChartItem.MolecularInfo, Me.RelatedChartItem.Parent.EnzymeCol)
        Me.RelatedChartItem.Parent.Draw()
    End Sub

    Private Sub btnOverlap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOverlap.Click
        If ReaderMode Then Exit Sub
        Me.RelatedChartItem.MolecularInfo.PCR_Overlap = Not Me.RelatedChartItem.MolecularInfo.PCR_Overlap
        btnOverlap.Text = IIf(Me.RelatedChartItem.MolecularInfo.PCR_Overlap, "OE", "NA")
    End Sub


    Private Sub RefreshHashPickerView()
        Dim scr As List(Of Nuctions.GeneFile) = Me.RelatedChartItem.MolecularInfo.GetSourceDNAList
        If scr.Count > 0 Then
            Dim MD5 As New System.Security.Cryptography.MD5CryptoServiceProvider
            Dim ListedHashDict As New Dictionary(Of Object, String)
            Dim SelectedDict As New Dictionary(Of Object, String)
            Dim Hash As String = ""
            Dim Selected As List(Of String) = Me.RelatedChartItem.MolecularInfo.PickedDNAs
            For Each gf As Nuctions.GeneFile In scr
                Hash = gf.GetHash
                If Selected.Contains(Hash) Then
                    SelectedDict.Add(gf, Hash)
                Else
                    ListedHashDict.Add(gf, Hash)
                End If
            Next
            cpHashPickerChoosenDNA.Choices = SelectedDict
            cpHashPickerListedDNA.Choices = ListedHashDict
        End If
    End Sub

    Dim vSequencingSetting As Boolean = False

    Private Sub RefreshSequencingView()
        vSequencingSetting = True
        tbSequencingPrimerName.Text = Me.RelatedChartItem.MolecularInfo.SequencingPrimerName
        tbSequencingPrimerSequence.Text = Me.RelatedChartItem.MolecularInfo.SequencingPrimer
        rtbSequencingResult.Text = Me.RelatedChartItem.MolecularInfo.SequencingSequence
        cbSequencingResultOption.Text = Me.RelatedChartItem.MolecularInfo.SequencingResultComment.ToString
        tbSequencingPrimerName.Items.Clear()
        For Each pi As PrimerInfo In Me.RelatedChartItem.Parent.Primers
            tbSequencingPrimerName.Items.Add(pi.Name)
        Next
        vSequencingSetting = False
    End Sub

    Dim vCompareSetting As Boolean = False
    Private Sub RefreshCompareView()
        Dim scr As List(Of Nuctions.GeneFile) = Me.RelatedChartItem.MolecularInfo.GetOriginalDNAList
        If scr.Count > 0 Then
            vCompareSetting = True
            Dim ListedDict As New Dictionary(Of Object, String)
            Dim SelectedDict As New Dictionary(Of Object, String)
            If Not (Me.RelatedChartItem.MolecularInfo.CompareSelectedGeneFile Is Nothing) Then
                If Me.RelatedChartItem.MolecularInfo.CompareSelectedGeneFile.Name Is Nothing Then Me.RelatedChartItem.MolecularInfo.CompareSelectedGeneFile.Name = "???"
                SelectedDict.Add(Me.RelatedChartItem.MolecularInfo.CompareSelectedGeneFile, Me.RelatedChartItem.MolecularInfo.CompareSelectedGeneFile.Name)
            End If
            For Each gf As Nuctions.GeneFile In scr
                If gf.Name Is Nothing Then gf.Name = "???"
                ListedDict.Add(gf, gf.Name)
            Next
            cpCompareChoice.Choices = SelectedDict
            cpCompareList.Choices = ListedDict
            cbCompareResult.Text = Me.RelatedChartItem.MolecularInfo.CompareResultComment.ToString
            vCompareSetting = False
        End If
    End Sub

    Private Sub cpHashPickerChoosenDNA_ItemChoosen(ByVal sender As Object, ByVal e As ChoicePanel.ItemChoosenEventArgs) Handles cpHashPickerChoosenDNA.ItemChoosen
        If ReaderMode Then Exit Sub
        cpHashPickerChoosenDNA.RemoveItem(e.Item)
        cpHashPickerListedDNA.AddItem(e.Item, e.Text)
        ApplyMode = True
        Accept()
    End Sub

    Private Sub cpHashPickerListedDNA_ItemChoosen(ByVal sender As Object, ByVal e As ChoicePanel.ItemChoosenEventArgs) Handles cpHashPickerListedDNA.ItemChoosen
        If ReaderMode Then Exit Sub
        cpHashPickerChoosenDNA.AddItem(e.Item, e.Text)
        cpHashPickerListedDNA.RemoveItem(e.Item)
        ApplyMode = True
        Accept()
    End Sub

    Private Sub Modify_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Modify_T4.CheckedChanged, Modify_CIAP.CheckedChanged, Modify_PNK.CheckedChanged, Modify_Klewnow.CheckedChanged
        If ModifyUpdating Then Exit Sub
        If ReaderMode Then Exit Sub
        ApplyMode = True
        Accept()
    End Sub

    Private Sub tbSequencingPrimerName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbSequencingPrimerName.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim tb As TextBox = tbSequencingPrimerSequence
            Dim sd As ComboBox = tbSequencingPrimerName
            Select Case tbFP.Text.ToLower
                Case "m13f"
                    tb.Text = ">TGTAAAACGACGGCCAGT"
                    sd.Text = "M13F"
                Case "m13r"
                    tb.Text = ">CAGGAAACAGCTATGACC"
                    sd.Text = "M13R"
                Case "m13-20"
                    tb.Text = ">GTAAAACGACGGCCAGT"
                    sd.Text = "M13-20"
                Case "m13-26"
                    tb.Text = ">CAGGAAACAGCTATGAC"
                    sd.Text = "M13-26"
                Case "m13-40"
                    tb.Text = ">GTTTTCCCAGTCACGAC"
                    sd.Text = "M13-40"
                Case "m13-46"
                    tb.Text = ">GCCAGGGTTTTCCCAGTCACGA"
                    sd.Text = "M13-46"
                Case "m13-47"
                    tb.Text = ">CGCCAGGGTTTTCCCAGTCACGAC"
                    sd.Text = "M13-47"
                Case "m13-48"
                    tb.Text = ">AGCGGATAACAATTTCACACAGGA"
                    sd.Text = "M13-48"
                Case "m13-96"
                    tb.Text = ">CCCTCATAGTTAGCGTAACG"
                    sd.Text = "M13-96"
                Case "t7pro", "t7 promoter"
                    tb.Text = ">TAATACGACTCACTATAGGG"
                    sd.Text = "T7Pro"
                Case "t7ter", "t7 terminator"
                    tb.Text = ">TGCTAGTTATTGCTCAGCGG"
                    sd.Text = "T7Ter"
                Case "sp6"
                    tb.Text = ">CATACGATTTAGGTGACACTATAG"
                    sd.Text = "SP6"
                Case "t7"
                    tb.Text = ">TAATACGACTCACTATAGGGAGA"
                    sd.Text = "T7"
                Case "t3"
                    tb.Text = ">GCGCGAAATTAACCCTCACTAAAG"
                    sd.Text = "T3"
                Case Else
                    For Each ci As ChartItem In Me.RelatedChartItem.Parent.Items
                        If ci Is RelatedChartItem Then Continue For
                        If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.PCR Then
                            If ci.MolecularInfo.PCR_FPrimerName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.PCR_ForwardPrimer : sd.Text = ci.MolecularInfo.PCR_FPrimerName
                            If ci.MolecularInfo.PCR_RPrimerName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.PCR_ReversePrimer : sd.Text = ci.MolecularInfo.PCR_RPrimerName
                        ElseIf ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.Screen And ci.MolecularInfo.Screen_Mode = Nuctions.ScreenModeEnum.PCR Then
                            If ci.MolecularInfo.Screen_FName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.Screen_FPrimer : sd.Text = ci.MolecularInfo.Screen_FName
                            If ci.MolecularInfo.Screen_RName.ToLower = sd.Text.ToLower Then tb.Text = ci.MolecularInfo.Screen_RPrimer : sd.Text = ci.MolecularInfo.Screen_RName
                        End If
                    Next
            End Select
        End If
    End Sub

    Private Sub tbSequencingPrimerName_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles tbSequencingPrimerName.SelectedIndexChanged
        If vSequencingSetting Then Exit Sub
        If tbSequencingPrimerName.SelectedIndex > -1 Then
            tbSequencingPrimerSequence.Text = Me.RelatedChartItem.Parent.Primers(tbSequencingPrimerName.SelectedIndex).Sequence
        End If
    End Sub


    Private Sub tbSequencingPrimerName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbSequencingPrimerName.TextChanged
        If vSequencingSetting Then Exit Sub
        If ReaderMode Then Exit Sub
        ApplyMode = True
        'Me.RelatedChartItem.MolecularInfo.SequencingPrimerName = tbSequencingPrimerName.Text
    End Sub

    Private Sub tbSequencingPrimerSequence_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbSequencingPrimerSequence.TextChanged
        If vSequencingSetting Then Exit Sub
        If ReaderMode Then Exit Sub
        ApplyMode = True
        'Me.RelatedChartItem.MolecularInfo.SequencingPrimer = Nuctions.TAGCFilter(tbSequencingPrimerSequence.Text)
    End Sub

    Private Sub rtbSequencingResult_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rtbSequencingResult.TextChanged
        If vSequencingSetting Then Exit Sub
        If ReaderMode Then Exit Sub
        ApplyMode = True
        'Me.RelatedChartItem.MolecularInfo.SequencingSequence = Nuctions.TAGCFilter(rtbSequencingResult.Text)
    End Sub

    Private Sub cpCompareChoice_ItemChoosen(ByVal sender As Object, ByVal e As ChoicePanel.ItemChoosenEventArgs) Handles cpCompareChoice.ItemChoosen
        If ReaderMode Then Exit Sub
        cpCompareChoice.RemoveItem(e.Item)
        Me.RelatedChartItem.MolecularInfo.CompareSelectedGeneFile = Nothing
        Me.RelatedChartItem.MolecularInfo.DNAs.Clear()
        For Each gf As Nuctions.GeneFile In cpCompareList.Choices.Keys
            Me.RelatedChartItem.MolecularInfo.DNAs.Add(gf)
        Next
        ApplyMode = True
        Accept()
    End Sub

    Private Sub cpCompareList_ItemChoosen(ByVal sender As Object, ByVal e As ChoicePanel.ItemChoosenEventArgs) Handles cpCompareList.ItemChoosen
        If ReaderMode Then Exit Sub
        cpCompareChoice.Choices.Clear()
        cpCompareChoice.AddItem(e.Item, e.Text)
        Me.RelatedChartItem.MolecularInfo.CompareSelectedGeneFile = e.Item
        Me.RelatedChartItem.MolecularInfo.DNAs.Clear()
        For Each gf As Nuctions.GeneFile In cpCompareList.Choices.Keys
            If gf Is e.Item Then Continue For
            Me.RelatedChartItem.MolecularInfo.DNAs.Add(gf)
        Next
        ApplyMode = True
        Accept()
    End Sub


    Private Sub llAnalysisAdd_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llAnalysisAdd.LinkClicked
        AnalysisAddSequence()
    End Sub

    Private Sub llAnalysisRemove_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llAnalysisRemove.LinkClicked
        AnalysisRemoveSequence()
    End Sub

    Private Sub llHelp_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llHelp.LinkClicked
        Select Case Me.RelatedChartItem.MolecularInfo.MolecularOperation
            Case Nuctions.MolecularOperationEnum.Vector
                Process.Start("http://www.synthenome.com/help/loadDNA.htm")
            Case Nuctions.MolecularOperationEnum.Enzyme
                Process.Start("http://www.synthenome.com/help/enzymedigestion.htm")
            Case Nuctions.MolecularOperationEnum.PCR
                Process.Start("http://synthenome.com/help/PCR.htm")
            Case Nuctions.MolecularOperationEnum.Gel
                Process.Start("http://synthenome.com/help/gel.htm")
            Case Nuctions.MolecularOperationEnum.Ligation
                Process.Start("http://www.synthenome.com/help/ligation.htm")
            Case Nuctions.MolecularOperationEnum.Modify
                Process.Start("http://synthenome.com/help/modify.htm")
            Case Nuctions.MolecularOperationEnum.FreeDesign
                Process.Start("http://www.synthenome.com/help/freedesign.htm")
            Case Nuctions.MolecularOperationEnum.Compare
                Process.Start("http://synthenome.com/help/compare.htm")
            Case Nuctions.MolecularOperationEnum.Merge
                Process.Start("http://synthenome.com/help/merge.htm")
            Case Nuctions.MolecularOperationEnum.SequencingResult
                Process.Start("http://synthenome.com/help/sequencing.htm")
            Case Nuctions.MolecularOperationEnum.Recombination
                Process.Start("http://synthenome.com/help/recombination.htm")
            Case Nuctions.MolecularOperationEnum.HashPicker
                Process.Start("http://synthenome.com/help/hashpicker.htm")
            Case Nuctions.MolecularOperationEnum.Screen
                Process.Start("http://synthenome.com/help/screen.htm")
            Case Nuctions.MolecularOperationEnum.EnzymeAnalysis
                Process.Start("http://synthenome.com/help/analysis.htm")
        End Select
    End Sub


    Private Sub cbSequencingResultOption_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbSequencingResultOption.SelectedIndexChanged
        If Not vSequencingSetting Then
            Dim res As SequenceResultOptions
            If [Enum].TryParse(Of SequenceResultOptions)(cbSequencingResultOption.Text, res) Then
                Me.RelatedChartItem.MolecularInfo.SequencingResultComment = res
            Else
                Me.RelatedChartItem.MolecularInfo.SequencingResultComment = SequenceResultOptions.Unchecked
            End If
        End If
    End Sub

    Private Sub llName_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llName.LinkClicked
        Me.RelatedChartItem.MolecularInfo.IsKeyName = Not Me.RelatedChartItem.MolecularInfo.IsKeyName
        If Me.RelatedChartItem.MolecularInfo.IsKeyName Then
            llName.Text = "NAME"
            llName.LinkColor = Color.DeepPink
        Else
            llName.Text = "<ID>"
            llName.LinkColor = Color.RoyalBlue
        End If
    End Sub

    Private Sub cbCompareResult_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbCompareResult.SelectedIndexChanged
        If Not vCompareSetting Then
            Dim res As SequenceResultOptions
            If [Enum].TryParse(Of SequenceResultOptions)(cbCompareResult.Text, res) Then
                Me.RelatedChartItem.MolecularInfo.CompareResultComment = res
            Else
                Me.RelatedChartItem.MolecularInfo.CompareResultComment = SequenceResultOptions.Unchecked
            End If
        End If
    End Sub

#Region "培养"

    Private Sub llAddIncubation_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llAddIncubation.LinkClicked
        Dim row As DataGridViewRow = dgvIncubation.Rows(dgvIncubation.Rows.Add())
        row.Cells(0).Value = "LB"
        row.Cells(1).Value = 1
        row.Cells(2).Value = "37"
    End Sub
    Private Sub llRemoveIncubation_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llRemoveIncubation.LinkClicked
        Dim dels As List(Of DataGridViewRow) = FacilityFunctions.GetDGVRows(dgvIncubation)
        For Each r As DataGridViewRow In dels
            dgvIncubation.Rows.Remove(r)
        Next
    End Sub
    Private Sub ReadIncubationParameters()
        Me.RelatedChartItem.MolecularInfo.Incubation = New List(Of IncubationStep)
        Dim ib As IncubationStep
        For Each r As DataGridViewRow In dgvIncubation.Rows
            ib = New IncubationStep
            If r.Cells(0).Value Is Nothing OrElse r.Cells(0).Value = "" Then
                ib.Medium = "LB"
            Else
                ib.Medium = r.Cells(0).Value
            End If
            If r.Cells(1).Value Is Nothing Then
                ib.IsPlate = False
            Else
                ib.IsPlate = (r.Cells(1).Value = 1)
            End If
            If r.Cells(2).Value Is Nothing Then
                ib.Temperature = "37.0"
            ElseIf (TypeOf r.Cells(2).Value Is Single) Or (TypeOf r.Cells(2).Value Is Integer) Or (TypeOf r.Cells(2).Value Is Double) Then
                ib.Temperature = r.Cells(2).Value
            ElseIf (TypeOf r.Cells(2).Value Is String) Then
                If Single.TryParse(r.Cells(2).Value, ib.Temperature) Then
                Else
                    ib.Temperature = 37.0F
                End If
            Else
                ib.Temperature = 37.0F
            End If
            If r.Cells(3).Value Is Nothing OrElse r.Cells(3).Value = "" Then
                ib.Antibiotics = New List(Of Nuctions.Antibiotics)
            Else
                ib.Antibiotics = Nuctions.ParseAntibiotics(r.Cells(3).Value)
            End If
            If r.Cells(4).Value Is Nothing OrElse r.Cells(4).Value = "" Then
                ib.Inducer = ""
            Else
                ib.Inducer = r.Cells(4).Value
            End If
            If r.Cells(5).Value Is Nothing OrElse r.Cells(5).Value = "" Then
                ib.Time = TimeSpan.Zero
            Else
                ib.Time = TimeSpan.Zero
                TimeSpan.TryParse(r.Cells(5).Value, ib.Time)
            End If
            Me.RelatedChartItem.MolecularInfo.Incubation.Add(ib)
        Next
    End Sub
#End Region

#Region "宿主"
    Private Sub llAddChromosomeFragment_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llAddChromosomeFragment.LinkClicked
        Dim g As New Nuctions.GeneFile
        g.Name = tbChromosomeFragmentName.Text
        tbChromosomeFragmentName.Text = ""
        g.Sequence = Nuctions.TAGCFilter(rtbChromosomoFragment.Text)
        rtbChromosomoFragment.Text = ""
        g.Chromosomal = True
        RelatedChartItem.MolecularInfo.Cells(0).DNAs.Add(g)
        For Each c As Nuctions.Cell In RelatedChartItem.MolecularInfo.Cells
            Nuctions.AddFeatures(c.DNAs, Features)
        Next
        RefreshDNAView()
    End Sub
    Private Sub llRemoveChromosomeFragment_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llRemoveChromosomeFragment.LinkClicked
        Dim lvi As New List(Of ListViewItem)
        For Each it In DNAView.SelectedItems
            lvi.Add(it)
        Next

        For Each ci As ListViewItem In lvi
            'RelatedChartItem.MolecularInfo.DNAs.Remove(ci.Tag)
            For Each cl In RelatedChartItem.MolecularInfo.Cells
                If cl.DNAs.Contains(ci.Tag) Then cl.DNAs.Remove(ci.Tag)
            Next
        Next
        For Each ci As ListViewItem In lvi
            DNAView.Items.Remove(ci)
        Next
    End Sub
#End Region


    Private IsVerifySetting As Boolean = False
    Private Sub cbVerify_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles cbVerify.CheckedChanged
        If Not IsVerifySetting Then
            Me.RelatedChartItem.MolecularInfo.IsVerificationStep = cbVerify.Checked
        End If
    End Sub

    Private Sub RefreshHostView()
        vHostSetting = True
        If Me.RelatedChartItem.MolecularInfo.Cells.Count < 1 Then
            Me.RelatedChartItem.MolecularInfo.Cells.Add(New Nuctions.Cell)
        ElseIf Me.RelatedChartItem.MolecularInfo.Cells.Count > 1 Then
            While Me.RelatedChartItem.MolecularInfo.Cells.Count > 1
                Me.RelatedChartItem.MolecularInfo.Cells.Remove(Me.RelatedChartItem.MolecularInfo.Cells.Last)
            End While
        End If
        tbHostName.Text = Me.RelatedChartItem.MolecularInfo.Cells(0).Host.Name
        tbHostFunction.Text = Nuctions.ExpressFeatureFunctions(Me.RelatedChartItem.MolecularInfo.Cells(0).Host.BioFunctions)
        vHostSetting = False
    End Sub

    Private vTransformationSetting As Boolean = False
    Private Sub RefreshTransformationView()
        vTransformationSetting = True
        RBTrf.Value = RelatedChartItem.MolecularInfo.TransformationMethod
        RBTrm.Value = RelatedChartItem.MolecularInfo.TransformationMode
        vTransformationSetting = False
    End Sub
    Private vIncubationSetting As Boolean = False
    Private Sub RefreshIncubationView()
        'load the steps
        dgvIncubation.Rows.Clear()
        Dim row As DataGridViewRow

        For Each ib As IncubationStep In Me.RelatedChartItem.MolecularInfo.Incubation
            row = dgvIncubation.Rows(dgvIncubation.Rows.Add)
            row.Cells(0).Value = ib.Medium
            row.Cells(1).Value = IIf(ib.IsPlate, 1, 0)
            row.Cells(2).Value = ib.Temperature
            row.Cells(3).Value = Nuctions.ExpressAntibiotics(ib.Antibiotics)
            row.Cells(4).Value = ib.Inducer
            row.Cells(5).Value = IIf(ib.Time = TimeSpan.Zero, "--:--:--", ib.Time.ToString("g"))
        Next
    End Sub
    Private Sub RefreshExtractionView()
        'nothing to do
        extSetting = True
        cbExtractionIncludeVerification.Checked = Me.RelatedChartItem.MolecularInfo.IncludeVerification
        cbExtractionSequencingVerify.Checked = Me.RelatedChartItem.MolecularInfo.SequencingVerify
        extSetting = False
    End Sub

    Private Sub rbTransformation_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbTransformationChemical.CheckedChanged, rbTransformationElectroporation.CheckedChanged, rbTransformationConjugation.CheckedChanged
        RelatedChartItem.MolecularInfo.TransformationMethod = RBTrf.Value
        Accept()
    End Sub
    Private vHostSetting As Boolean = False
    Private Sub tbHostName_TextChanged(sender As System.Object, e As System.EventArgs) Handles tbHostName.TextChanged
        If vHostSetting Then Exit Sub
        Me.RelatedChartItem.MolecularInfo.Cells(0).Host.Name = tbHostName.Text
    End Sub
    Private Sub tbHostFunction_TextChanged(sender As System.Object, e As System.EventArgs) Handles tbHostFunction.TextChanged
        If vHostSetting Then Exit Sub
        Me.RelatedChartItem.MolecularInfo.Cells(0).Host.BioFunctions = Nuctions.AnalyzedFeatureCode(tbHostFunction.Text)
    End Sub

    Private Sub cbEnvironment_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbEnvironment.SelectedIndexChanged
        If vEnvironmentSetting Then Exit Sub
        For Each h As Nuctions.Host In Me.RelatedChartItem.Parent.Hosts
            If h.Name = cbEnvironment.Text Then
                Me.RelatedChartItem.MolecularInfo.Cells = New List(Of Nuctions.Cell) From {New Nuctions.Cell}
                With Me.RelatedChartItem.MolecularInfo.Cells(0).Host
                    .Name = h.Name
                    For Each ff As Nuctions.FeatureFunction In h.BioFunctions
                        .BioFunctions.Add(ff)
                    Next
                    .Description = h.Description
                End With
            End If
        Next
        If Me.RelatedChartItem.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.Host Then
            tbHostName.Text = Me.RelatedChartItem.MolecularInfo.Cells(0).Host.Name
            tbHostFunction.Text = Nuctions.ExpressFeatureFunctions(Me.RelatedChartItem.MolecularInfo.Cells(0).Host.BioFunctions)
        End If
    End Sub

    Private Function RefreshCellsView() As Nuctions.Cell
        lvCellsSetting = True
        Dim firstcell As Nuctions.Cell = Nothing
        Select Case RelatedChartItem.MolecularInfo.MolecularOperation
            Case Nuctions.MolecularOperationEnum.Host, Nuctions.MolecularOperationEnum.Transformation, Nuctions.MolecularOperationEnum.Incubation
                lvCells.Visible = True
                lvCells.Items.Clear()
                Dim lvi As ListViewItem
                For Each c As Nuctions.Cell In RelatedChartItem.MolecularInfo.Cells
                    If firstcell Is Nothing Then firstcell = c
                    lvi = lvCells.Items.Add(c.Host.Name)
                    lvi.Tag = c
                    lvi.SubItems.Add(c.DNAs.Count.ToString)
                Next
            Case Nuctions.MolecularOperationEnum.Recombination, Nuctions.MolecularOperationEnum.Screen, Nuctions.MolecularOperationEnum.HashPicker, Nuctions.MolecularOperationEnum.Enzyme
                lvCells.Items.Clear()
                If RelatedChartItem.MolecularInfo.Cells.Count > 0 Then
                    lvCells.Visible = True
                    Dim lvi As ListViewItem
                    For Each c As Nuctions.Cell In RelatedChartItem.MolecularInfo.Cells
                        If firstcell Is Nothing Then firstcell = c
                        lvi = lvCells.Items.Add(c.Host.Name)
                        lvi.Tag = c
                        lvi.SubItems.Add(c.DNAs.Count.ToString)
                    Next
                End If
            Case Else
                lvCells.Visible = False
        End Select
        lvCellsSetting = False
        Return firstcell
    End Function
    Private lvCellsSetting As Boolean = False
    Private Sub lvCells_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvCells.SelectedIndexChanged
        If lvCellsSetting Then Exit Sub
        If lvCells.SelectedItems.Count > 0 Then
            ViewCellDNAs(DirectCast(lvCells.SelectedItems(0).Tag, Nuctions.Cell))
        End If
    End Sub

    Private Sub ViewCellDNAs(cell As Nuctions.Cell)
        If DNAView.Items.Count > 0 Then DNAView.Items.Clear()
        For Each dna In cell.DNAs
            AddtoDNAView(dna, 0)
        Next
    End Sub

    Private Sub cbDephosphorylate_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles cbDephosphorylate.CheckedChanged
        If enzSetting Then Exit Sub
        Me.RelatedChartItem.MolecularInfo.DephosphorylateWhenDigestion = cbDephosphorylate.Checked
        ApplyMode = True
        Accept()
    End Sub

    Dim extSetting As Boolean = False
    Private Sub cbExtractionIncludeVerification_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles cbExtractionIncludeVerification.CheckedChanged
        If extSetting Then Exit Sub
        Me.RelatedChartItem.MolecularInfo.IncludeVerification = cbExtractionIncludeVerification.Checked
    End Sub

    Private Sub cbExtractionSequencingVerify_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles cbExtractionSequencingVerify.CheckedChanged
        If extSetting Then Exit Sub
        Me.RelatedChartItem.MolecularInfo.SequencingVerify = cbExtractionSequencingVerify.Checked
    End Sub

    Private Sub cbGel_Solution_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles cbGel_Solution.CheckedChanged
        If gelSetting Then Exit Sub
        Me.RelatedChartItem.MolecularInfo.SolutionExtration = cbGel_Solution.Checked
        ApplyMode = True
        Accept()
    End Sub

    Private Sub llCommonStep_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llCommonStep.LinkClicked
        Dim row As DataGridViewRow
        row = dgvIncubation.Rows(dgvIncubation.Rows.Add())
        row.Cells(0).Value = "LB"
        row.Cells(1).Value = 1
        row.Cells(2).Value = "37"
        row = dgvIncubation.Rows(dgvIncubation.Rows.Add())
        row.Cells(0).Value = "LB"
        row.Cells(1).Value = 0
        row.Cells(2).Value = "37"
    End Sub

    Private Sub cbMainConstruction_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles cbMainConstruction.CheckedChanged
        If desSetting Then Exit Sub
        Me.RelatedChartItem.MolecularInfo.IsConstructionNode = cbMainConstruction.Checked
        If Me.RelatedChartItem.MolecularInfo.IsConstructionNode Then
            Me.RelatedChartItem.MolecularInfo.IsKeyName = True
            llName.Text = "NAME"
            llName.LinkColor = Color.DeepPink
        End If
        Me.RelatedChartItem.Reload(RelatedChartItem.MolecularInfo, Me.RelatedChartItem.Parent.EnzymeCol)
        Me.RelatedChartItem.Parent.Draw()
        Me.RelatedChartItem.Parent.Update()
    End Sub

    Private Sub dgvIncubation_CellEndEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvIncubation.CellEndEdit
        Select Case e.ColumnIndex
            Case 3 'antibiotics 
                If dgvIncubation.Rows(e.RowIndex).Cells(3).Value Is Nothing Then dgvIncubation.Rows(e.RowIndex).Cells(3).Value = ""
                dgvIncubation.Rows(e.RowIndex).Cells(3).Value = Nuctions.ExpressAntibiotics(Nuctions.ParseAntibiotics(dgvIncubation.Rows(e.RowIndex).Cells(3).Value))
            Case 5 'timespan
                Dim ts As TimeSpan
                If TimeSpan.TryParse(dgvIncubation.Rows(e.RowIndex).Cells(5).Value, ts) Then
                    dgvIncubation.Rows(e.RowIndex).Cells(5).Value = ts.ToString("g")
                Else
                    dgvIncubation.Rows(e.RowIndex).Cells(5).Value = "--:--:--"
                End If
        End Select
    End Sub

 

#Region "PCR Screen Products Calculation"
    Dim pspcpmr As New List(Of String)
    Dim bufferedpmr As New List(Of String)
    Private Sub BeginCalculateScreenPCR()
        Try
            Dim F As String = Nuctions.TAGCFilter(Screen_PCR_F.Text)
            Dim R As String = Nuctions.TAGCFilter(Screen_PCR_R.Text)
            If bufferedpmr.Contains(F) AndAlso bufferedpmr.Contains(R) Then
                '相同的引物 不需要重新计算
                Exit Sub
            Else
                bufferedpmr.Clear()
                bufferedpmr.Add(F)
                bufferedpmr.Add(R)
            End If
            pspcpmr.Clear()
            pspcpmr.Add(F)
            pspcpmr.Add(R)
            For Each obj As Control In pnlScreenPCR.Controls
                If TypeOf obj Is LinkLabel Then
                    RemoveHandler obj.Click, AddressOf OnScreenLinkClick
                End If
            Next
            pnlScreenPCR.Controls.Clear()
            Dim lbl As New Label With {.Text = "Calculating Products ... ", .Location = New Point(2, 2)}
            pnlScreenPCR.Controls.Add(lbl)
            Dim thr As New Threading.Thread(AddressOf StartCalculatePCR)
            thr.Start()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub StartCalculatePCR()
        Try
            Dim products As List(Of Nuctions.GeneFile)
            If RelatedChartItem.MolecularInfo.IsCellNode Then
                products = Nuctions.PCR(RelatedChartItem.MolecularInfo.GetSourceCellDNAList, pspcpmr)
            Else
                products = Nuctions.PCR(RelatedChartItem.MolecularInfo.GetSourceDNAList, pspcpmr)
            End If
            Dim lengths As New List(Of Integer)
            For Each g As Nuctions.GeneFile In products
                If Not lengths.Contains(g.Length) Then lengths.Add(g.Length)
            Next
            ScreenPCRCallBack(lengths)
        Catch ex As Exception

        End Try
    End Sub
    Private Delegate Sub ScreenPCRCallBackDelegate(products As List(Of Integer))
    Private Sub ScreenPCRCallBack(products As List(Of Integer))
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New ScreenPCRCallBackDelegate(AddressOf ScreenPCRCallBack), New Object() {products})
            Else
                For Each obj As Control In pnlScreenPCR.Controls
                    If TypeOf obj Is LinkLabel Then
                        RemoveHandler obj.Click, AddressOf OnScreenLinkClick
                    End If
                Next
                pnlScreenPCR.Controls.Clear()
                products.Sort()
                Dim ll As LinkLabel
                Dim j As Integer = 0
                For Each i As Integer In products
                    ll = New LinkLabel With {.Text = i.ToString + "bp", .Top = (j \ 5) * 16, .Left = (j Mod 5) * 80, .AutoSize = True}
                    AddHandler ll.Click, AddressOf OnScreenLinkClick
                    pnlScreenPCR.Controls.Add(ll)
                    j += 1
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub OnScreenLinkClick(sender As Object, e As EventArgs)
        If TypeOf sender Is LinkLabel Then
            Dim i As Integer
            Dim ll As LinkLabel = sender
            If Integer.TryParse(ll.Text.Substring(0, ll.Text.Length - 2), i) Then
                Screen_PCR_nudMax.Value = i
                Screen_PCR_nudMin.Value = i
            End If
            ApplyMode = True
            Accept()
        End If
    End Sub
#End Region


    Private Sub cbNoMap_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles cbNoMap.CheckedChanged
        If Setting_finished Then Exit Sub
        Me.RelatedChartItem.MolecularInfo.NotDrawMap = cbNoMap.Checked
        ApplyMode = True
        Accept()
    End Sub

    Private Sub rbTransformationMode_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbTransformationAIOC.CheckedChanged, rbTransformationEDPC.CheckedChanged, rbTransformationCBNT.CheckedChanged
        RelatedChartItem.MolecularInfo.TransformationMode = RBTrm.Value
        Accept()
    End Sub


End Class

Public Class EnzymeAnalysisItem
    Public GeneFile As Nuctions.GeneFile
    Public Region As String
    Public Use As Boolean = False
    Public Method As EnzymeAnalysisEnum
    Public Value As Integer
End Class
Public Enum EnzymeAnalysisEnum As Integer
    Equal = 0
    Greater = 1
    Less = 2
End Enum