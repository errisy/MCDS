Public Class frmProperty
    '    Public RelatedChartItem As ChartItem
    '    Public vMolecularOperation As Nuctions.MolecularOperationEnum
    '    Private mApplyMode As Boolean = False
    '    Private Result As Windows.Forms.DialogResult = Windows.Forms.DialogResult.Cancel

    '    Public Property MolecularOperation() As Nuctions.MolecularOperationEnum
    '        Get
    '            Return Me.vMolecularOperation
    '        End Get
    '        Set(ByVal value As Nuctions.MolecularOperationEnum)
    '            Me.vMolecularOperation = value
    '            'prepare the window for a specific operation
    '            Me.TabControl_Operation.SelectedIndex = value
    '            Dim i As Integer

    '            For i = 0 To 6
    '                If i = value Then Continue For
    '                TabControl_Operation.TabPages(i).Tag = False
    '            Next
    '            Dim tp As TabPage
    '            For Each tp In TabControl_Operation.TabPages
    '                If (Not tp.Tag Is Nothing) And (tp.Tag = False) Then TabControl_Operation.TabPages.Remove(tp)
    '            Next
    '        End Set
    '    End Property

    '    Private Sub File_FileName_LinkLabel_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles File_FileName_LinkLabel.LinkClicked
    '        If Not ofdGeneFile.ShowDialog = Windows.Forms.DialogResult.OK Then Exit Sub
    '        Dim a As IO.FileInfo = New IO.FileInfo(ofdGeneFile.FileName)
    '        Me.File_Path_Label.Text = a.FullName
    '        Me.File_FileName_LinkLabel.Text = a.Name
    '        ApplyMode = True
    '    End Sub

    '    Private Sub Prop_Operation_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles Prop_Operation.LinkClicked
    '        'deal with source
    '        If Not (Me.MolecularOperation = Nuctions.MolecularOperationEnum.Vector) Then
    '            Dim fi As New frmItems
    '            fi.LoadItems(Me.RelatedChartItem.ItemCol, Me.RelatedChartItem.Source)
    '            If fi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then RefreshOperationView() : Me.ApplyMode = True
    '        End If
    '    End Sub
    '    Private Sub RefreshOperationView()
    '        Dim ui As ChartItem
    '        Dim sourcetext As String = ""
    '        For Each ui In Me.RelatedChartItem.Source
    '            sourcetext &= ui.Name + ";"
    '        Next
    '        If sourcetext.Length = 0 Then sourcetext = "[Click to select]"
    '        Me.Prop_Operation.Text = sourcetext
    '        'notify that there are suspended operations.
    '    End Sub

    '    Private Sub btn_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OK.Click
    '        'check the name
    '        Accept()
    '    End Sub
    '    Private Sub Accept()

    '        If ApplyMode Then

    '            Select Case Me.MolecularOperation
    '                Case Nuctions.MolecularOperationEnum.Enzyme
    '                    'get the vectors from source and analyze enzyme cutting.
    '                    Dim gf As Nuctions.GeneFile
    '                    Dim EARC As Nuctions.EnzymeAnalysis.EnzymeAnalysisResult
    '                    Dim earcCol As New Collection
    '                    Dim info As String
    '                    Dim infoQ As New Queue(Of String)
    '                    Dim dnaList As List(Of Nuctions.GeneFile)
    '                    Dim DNA As Nuctions.GeneFile
    '                    Dim ci As ChartItem
    '                    Dim SourceDNA As New Collection
    '                    For Each ci In Me.RelatedChartItem.Source
    '                        For Each gf In ci.DNAs
    '                            SourceDNA.Add(gf)
    '                        Next
    '                    Next
    '                    For Each gf In SourceDNA
    '                        EARC = New Nuctions.EnzymeAnalysis.EnzymeAnalysisResult(Me.RelatedChartItem.Enzyme_Enzymes, gf)
    '                        For Each info In EARC.Confliction
    '                            infoQ.Enqueue(info)
    '                        Next
    '                        earcCol.Add(EARC)
    '                    Next
    '                    If infoQ.Count = 0 Then
    '                        If RelatedChartItem.DNAs.Count > 0 Then RelatedChartItem.DNAs.Clear()
    '                        For Each EARC In earcCol
    '                            dnaList = EARC.CutDNA()
    '                            For Each DNA In dnaList
    '                                Me.RelatedChartItem.DNAs.Add(DNA)
    '                            Next
    '                        Next
    '                        Nuctions.AddFeatures(Me.RelatedChartItem.DNAs, Me.RelatedChartItem.FeatureCol)
    '                        'refresh the DNA view:
    '                        RefreshDNAView()
    '                        Me.Result = Windows.Forms.DialogResult.OK
    '                        Me.Prop_Name.Text = "(" + Me.RelatedChartItem.ListView.Items.Count.ToString() + ")"
    '                        For Each ez As Nuctions.RestrictionEnzyme In Me.RelatedChartItem.Enzyme_Enzymes
    '                            Me.Prop_Name.Text += " " + ez.Name
    '                        Next
    '                        Me.Prop_Name.Text += " Cut "
    '                        For Each si As ChartItem In Me.RelatedChartItem.Source
    '                            Me.Prop_Name.Text += " " + si.Text
    '                        Next
    '                        Me.ApplyMode = False
    '                    Else
    '                        infoQ.Enqueue("Failed to Cut the vectors")
    '                        Dim fER As New frmErrorReport
    '                        fER.ShowDialog(infoQ, Me)
    '                    End If
    '                Case Nuctions.MolecularOperationEnum.Gel
    '                    Dim gelErr As Boolean = False
    '                    Dim errQ As New Queue(Of String)
    '                    If Me.Gel_Minimum_Number.Value < 100 Then gelErr = True : errQ.Enqueue("Minimum Value should be over 100")
    '                    If Me.Gel_Minimum_Number.Value > Me.Gel_Maximum_Number.Value Then gelErr = True : errQ.Enqueue("Minimun Value should be smaller than Maximum Value")
    '                    If Not gelErr Then
    '                        Dim gf As Nuctions.GeneFile
    '                        Dim gfCol As New Collection
    '                        Dim ci As ChartItem
    '                        'get all the DNAs
    '                        For Each ci In Me.RelatedChartItem.Source
    '                            For Each gf In ci.DNAs
    '                                gfCol.Add(gf)
    '                            Next
    '                        Next
    '                        Me.RelatedChartItem.Gel_Maximun = Me.Gel_Maximum_Number.Value
    '                        Me.RelatedChartItem.Gel_Minimum = Me.Gel_Minimum_Number.Value
    '                        Dim gelCol As List(Of Nuctions.GeneFile) = Nuctions.ScreenLength(gfCol, Me.RelatedChartItem.Gel_Minimum, Me.RelatedChartItem.Gel_Maximun)
    '                        If Me.RelatedChartItem.DNAs.Count > 0 Then Me.RelatedChartItem.DNAs.Clear()
    '                        For Each gf In gelCol
    '                            Me.RelatedChartItem.DNAs.Add(gf)
    '                        Next
    '                        Nuctions.AddFeatures(Me.RelatedChartItem.DNAs, Me.RelatedChartItem.FeatureCol)
    '                        RefreshDNAView()
    '                        Me.Result = Windows.Forms.DialogResult.OK
    '                        ApplyMode = False
    '                    Else
    '                        Dim fErr As New frmErrorReport
    '                        fErr.ShowDialog(errQ, Me)
    '                    End If
    '                Case Nuctions.MolecularOperationEnum.Ligation
    '                    Dim gf As Nuctions.GeneFile
    '                    Dim gfCol As New Collection
    '                    Dim ci As ChartItem
    '                    'get all the DNAs
    '                    For Each ci In Me.RelatedChartItem.Source
    '                        For Each gf In ci.DNAs
    '                            gfCol.Add(gf)
    '                        Next
    '                    Next
    '                    'ligate
    '                    Me.RelatedChartItem.Ligation_TriFragment = Me.Ligation_TriFrag.Checked
    '                    Dim lgCol As List(Of Nuctions.GeneFile)
    '                    If cbML.Checked Then
    '                        lgCol = Nuctions.MultipleLinearLigate(gfCol)
    '                    Else
    '                        lgCol = Nuctions.LigateDNA(gfCol, Me.RelatedChartItem.Ligation_TriFragment)
    '                    End If
    '                    If Me.RelatedChartItem.DNAs.Count > 0 Then Me.RelatedChartItem.DNAs.Clear()
    '                    For Each gf In lgCol
    '                        Me.RelatedChartItem.DNAs.Add(gf)
    '                    Next
    '                    Nuctions.AddFeatures(Me.RelatedChartItem.DNAs, Me.RelatedChartItem.FeatureCol)
    '                    RefreshLigationView()
    '                    RefreshDNAView()
    '                    Me.Result = Windows.Forms.DialogResult.OK
    '                    Me.ApplyMode = False
    '                Case Nuctions.MolecularOperationEnum.Modify
    '                    If Me.Modify_CIAP.Checked Then Me.RelatedChartItem.Modify_Method = Nuctions.ModificationMethodEnum.CIAP
    '                    If Me.Modify_Klewnow.Checked Then Me.RelatedChartItem.Modify_Method = Nuctions.ModificationMethodEnum.Klewnow
    '                    If Me.Modify_PNK.Checked Then Me.RelatedChartItem.Modify_Method = Nuctions.ModificationMethodEnum.PNK
    '                    If Me.Modify_T4.Checked Then Me.RelatedChartItem.Modify_Method = Nuctions.ModificationMethodEnum.T4DNAP
    '                    Dim gf As Nuctions.GeneFile
    '                    Dim gfCol As New Collection
    '                    Dim ci As ChartItem
    '                    'get all the DNAs
    '                    For Each ci In Me.RelatedChartItem.Source
    '                        For Each gf In ci.DNAs
    '                            gfCol.Add(gf)
    '                        Next
    '                    Next
    '                    Dim lgCol As List(Of Nuctions.GeneFile) = Nuctions.ModifyDNA(gfCol, Me.RelatedChartItem.Modify_Method)
    '                    If Me.RelatedChartItem.DNAs.Count > 0 Then Me.RelatedChartItem.DNAs.Clear()
    '                    For Each gf In lgCol
    '                        Me.RelatedChartItem.DNAs.Add(gf)
    '                    Next
    '                    Nuctions.AddFeatures(Me.RelatedChartItem.DNAs, Me.RelatedChartItem.FeatureCol)
    '                    Me.Result = Windows.Forms.DialogResult.OK
    '                    Me.ApplyMode = False
    '                    RefreshModifyView()
    '                    RefreshDNAView()
    '                Case Nuctions.MolecularOperationEnum.PCR
    '                    Dim errQ As New Queue(Of String)
    '                    Dim pcrErr As Boolean = False
    '                    Dim FPrimer As String = Nuctions.TAGCFilter(Me.PCR_ForwardPrimer_TextBox.Text)
    '                    Dim RPrimer As String = Nuctions.TAGCFilter(Me.PCR_ReversePrimer_TextBox.Text)
    '                    Me.PCR_ForwardPrimer_TextBox.Text = FPrimer
    '                    Me.PCR_ReversePrimer_TextBox.Text = RPrimer
    '                    If FPrimer.Length < 12 Then pcrErr = True : errQ.Enqueue("Forward Primer is shorter than 12 bp")
    '                    If RPrimer.Length < 12 Then pcrErr = True : errQ.Enqueue("Reverse Primer is shorter than 12 bp")
    '                    If Not pcrErr Then
    '                        Dim gf As Nuctions.GeneFile
    '                        Dim gfCol As New Collection
    '                        Dim ci As ChartItem
    '                        'get all the DNAs
    '                        For Each ci In Me.RelatedChartItem.Source
    '                            For Each gf In ci.DNAs
    '                                gfCol.Add(gf)
    '                            Next
    '                        Next
    '                        Me.RelatedChartItem.PCR_ForwardPrimer = FPrimer
    '                        Me.RelatedChartItem.PCR_ReversePrimer = RPrimer
    '                        Dim gfList As List(Of Nuctions.GeneFile) = Nuctions.PCR(gfCol, Me.RelatedChartItem.PCR_ForwardPrimer, Me.RelatedChartItem.PCR_ReversePrimer)
    '                        If Me.RelatedChartItem.DNAs.Count > 0 Then Me.RelatedChartItem.DNAs.Clear()
    '                        For Each gf In gfList
    '                            Me.RelatedChartItem.DNAs.Add(gf)
    '                        Next
    '                        Nuctions.AddFeatures(Me.RelatedChartItem.DNAs, Me.RelatedChartItem.FeatureCol)
    '                        Me.Result = Windows.Forms.DialogResult.OK
    '                        RefreshDNAView()
    '                        ApplyMode = False
    '                    Else
    '                        Dim fErr As New frmErrorReport
    '                        fErr.ShowDialog(errQ, Me)
    '                    End If
    '                Case Nuctions.MolecularOperationEnum.Screen
    '                    Dim gf As Nuctions.GeneFile
    '                    Dim gfCol As New Collection
    '                    Dim ci As ChartItem
    '                    'get all the DNAs
    '                    If Me.Screen_Freatures.Checked Then
    '                        Me.RelatedChartItem.Screen_Mode = Nuctions.ScreenModeEnum.Features
    '                        For Each ci In Me.RelatedChartItem.Source
    '                            For Each gf In ci.DNAs
    '                                gfCol.Add(gf)
    '                            Next
    '                        Next
    '                        Dim gfList As List(Of Nuctions.GeneFile) = Nuctions.ScreenFeature(gfCol, Me.RelatedChartItem.Screen_Features)
    '                        If Me.RelatedChartItem.DNAs.Count > 0 Then Me.RelatedChartItem.DNAs.Clear()
    '                        For Each gf In gfList
    '                            Me.RelatedChartItem.DNAs.Add(gf)
    '                        Next
    '                        'Screen needs not additional features
    '                        Nuctions.AddFeatures(Me.RelatedChartItem.DNAs, Me.RelatedChartItem.FeatureCol)
    '                        Me.Result = Windows.Forms.DialogResult.OK
    '                        Me.ApplyMode = False
    '                        RefreshScreenView()
    '                        RefreshDNAView()
    '                    Else
    '                        Dim errQ As New Queue(Of String)
    '                        Dim scrErr As Boolean = False
    '                        Dim FPrimer As String = Nuctions.TAGCFilter(Me.Screen_PCR_F.Text)
    '                        Dim RPrimer As String = Nuctions.TAGCFilter(Me.Screen_PCR_R.Text)
    '                        Me.PCR_ForwardPrimer_TextBox.Text = FPrimer
    '                        Me.PCR_ReversePrimer_TextBox.Text = RPrimer
    '                        If FPrimer.Length < 12 Then scrErr = True : errQ.Enqueue("Forward Primer is shorter than 12 bp")
    '                        If RPrimer.Length < 12 Then scrErr = True : errQ.Enqueue("Reverse Primer is shorter than 12 bp")
    '                        If Me.Screen_PCR_nudMin.Value < 100 Then scrErr = True : errQ.Enqueue("Minimum Value should be over 100")
    '                        If Me.Screen_PCR_nudMin.Value > Me.Screen_PCR_nudMax.Value Then scrErr = True : errQ.Enqueue("Minimun Value should be smaller than Maximum Value")
    '                        If scrErr Then
    '                            Dim fErr As New frmErrorReport
    '                            fErr.ShowDialog(errQ, Me)
    '                        Else
    '                            'process the PCR Screen
    '                            With Me.RelatedChartItem
    '                                .Screen_FPrimer = FPrimer
    '                                .Screen_RPrimer = RPrimer
    '                                .Screen_PCRMax = Me.Screen_PCR_nudMax.Value
    '                                .Screen_PCRMin = Me.Screen_PCR_nudMin.Value
    '                                .Screen_Mode = Nuctions.ScreenModeEnum.PCR
    '                            End With
    '                            For Each ci In Me.RelatedChartItem.Source
    '                                For Each gf In ci.DNAs
    '                                    gfCol.Add(gf)
    '                                Next
    '                            Next
    '                            Dim gfList As List(Of Nuctions.GeneFile) = Nuctions.ScreenPCR(gfCol, Me.RelatedChartItem.Screen_FPrimer, Me.RelatedChartItem.Screen_RPrimer, Me.RelatedChartItem.Screen_PCRMax, Me.RelatedChartItem.Screen_PCRMin)
    '                            If Me.RelatedChartItem.DNAs.Count > 0 Then Me.RelatedChartItem.DNAs.Clear()
    '                            For Each gf In gfList
    '                                Me.RelatedChartItem.DNAs.Add(gf)
    '                            Next
    '                            Nuctions.AddFeatures(Me.RelatedChartItem.DNAs, Me.RelatedChartItem.FeatureCol)
    '                            Me.Result = Windows.Forms.DialogResult.OK
    '                            Me.ApplyMode = False
    '                            RefreshScreenView()
    '                            RefreshDNAView()
    '                        End If
    '                    End If
    '                Case Nuctions.MolecularOperationEnum.Vector
    '                    Me.RelatedChartItem.File_Filename = Me.File_Path_Label.Text
    '                    Dim gf As Nuctions.GeneFile = Nuctions.GeneFile.LoadFromGeneBankFile(Me.RelatedChartItem.File_Filename)
    '                    Dim errQ As New Queue(Of String)
    '                    Dim vecErr As Boolean
    '                    If Me.RelatedChartItem.ItemCol.Contains(gf.Name) Then vecErr = True
    '                    If vecErr Then
    '                        Dim fErr As New frmErrorReport
    '                        fErr.ShowDialog(errQ, Me)
    '                    Else
    '                        Dim gn As Nuctions.GeneAnnotation
    '                        Dim ft As Nuctions.Feature

    '                        For Each gn In gf.Features.Values
    '                            'Add the annotation to the collection so that we can store the features
    '                            'The features are useful in the ligation and screen
    '                            ft = New Nuctions.Feature(gn.Label + " <" + gf.Name + "> (" + Me.RelatedChartItem.FeatureCol.Count.ToString + ")", gn.GetSuqence, gn.Label + " <" + gf.Name + ">", gn.Type, gn.Note)
    '                            Me.RelatedChartItem.FeatureCol.Add(ft, ft.Name)
    '                        Next
    '                        If Me.RelatedChartItem.DNAs.Count > 0 Then Me.RelatedChartItem.DNAs.Clear()
    '                        Me.RelatedChartItem.DNAs.Add(gf)
    '                        Me.Result = Windows.Forms.DialogResult.OK
    '                        ApplyMode = False
    '                        RefreshDNAView()
    '                    End If
    '            End Select
    '        Else

    '            Me.Close()
    '        End If
    '    End Sub

    '    Private Sub Enzyme_Enzymes_LinkLabel_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles Enzyme_Enzymes_LinkLabel.LinkClicked
    '        Dim ef As New frmEnzymes
    '        ef.LoadEnzymeItems(frmMain.EnzymeCollection, RelatedChartItem.Enzyme_Enzymes)
    '        If ef.ShowDialog = Windows.Forms.DialogResult.OK Then RefreshEnzymeView() : Me.ApplyMode = True
    '    End Sub
    '    Private Sub RefreshEnzymeView()
    '        Dim ei As Nuctions.RestrictionEnzyme
    '        Dim sourcetext As String = ""
    '        For Each ei In Me.RelatedChartItem.Enzyme_Enzymes
    '            sourcetext &= ei.Name + ";"
    '        Next
    '        If sourcetext.Length = 0 Then sourcetext = "[Click to select]"
    '        Me.Enzyme_Enzymes_LinkLabel.Text = sourcetext
    '        'notify that there are suspended operations.


    '    End Sub
    '    Private Sub frmProperty_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    '        'help to give the result if the operation is not applied.
    '        Dim ui As ChartItem
    '        For Each ui In Me.RelatedChartItem.ItemCol
    '            If (Me.Prop_Name.Text = ui.Name) And Not (Me.RelatedChartItem Is ui) And Not (Me.RelatedChartItem.Creating And Me.Result = Windows.Forms.DialogResult.Cancel) Then e.Cancel = True : MsgBox("Name conflicting!") : Exit Sub
    '        Next
    '        'delete the old name index when the name exists

    '        If Me.RelatedChartItem.ItemCol.Contains(Me.RelatedChartItem.Name) Then Me.RelatedChartItem.ItemCol.Remove(Me.RelatedChartItem.Name)
    '        Me.RelatedChartItem.Name = Me.Prop_Name.Text
    '        'add with the new name
    '        If Not Me.RelatedChartItem.Creating Then Me.RelatedChartItem.ItemCol.Add(Me.RelatedChartItem, Me.RelatedChartItem.Name)

    '        Me.RelatedChartItem.Text = Me.Prop_Name.Text
    '        Me.RelatedChartItem.OperationDescription = Me.rtb_Description.Text

    '        Me.RelatedChartItem.Editing = False
    '        Me.DialogResult = Me.Result
    '    End Sub

    '    Public Property ApplyMode() As Boolean
    '        Get
    '            Return mApplyMode
    '        End Get
    '        Set(ByVal value As Boolean)
    '            mApplyMode = value
    '            btn_OK.Text = IIf(value, "Apply", "OK")
    '        End Set
    '    End Property

    '    Private Sub btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
    '        ' this button in fact dose not do any thing.
    '        If Me.RelatedChartItem.Creating Then Result = Windows.Forms.DialogResult.Cancel
    '        Me.Close()
    '    End Sub

    '    Private Sub lv_DNA_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DNAView.DoubleClick
    '        Dim lv_Chart As ListView = sender
    '        If lv_Chart.SelectedItems.Count = 1 Then
    '            Dim ci As ChartItem = lv_Chart.SelectedItems(0)
    '            If ci.DNAs.Count = 1 Then
    '                Dim gf As Nuctions.GeneFile = ci.DNAs(1)
    '                gf.WriteToFile("Temp.gb")
    '                Nuctions.ShowGBFile("Temp.gb")
    '            End If
    '        End If
    '    End Sub
    '    Public Shadows Function ShowDialog(ByVal vChartItem As ChartItem, ByVal vParent As System.Windows.Forms.IWin32Window) As System.Windows.Forms.DialogResult
    '        'Dim fp As frmProperty = Me
    '        Dim ci As ChartItem = vChartItem

    '        'prevent the selection of itemself
    '        ci.Editing = True
    '        Me.RelatedChartItem = ci

    '        'General Information
    '        Me.MolecularOperation = ci.MolecularOperation

    '        Dim ui As ChartItem
    '        Dim sourcetext As String = ""
    '        For Each ui In Me.RelatedChartItem.Source
    '            sourcetext &= ui.Name + ";"
    '        Next
    '        Me.rtb_Description.Text = Me.RelatedChartItem.OperationDescription

    '        If ci.Creating Then
    '            Me.Prop_Name.Text = ci.MolecularOperation.ToString + " " + ci.ItemCol.Count.ToString + " (" + sourcetext + ")"
    '            Me.Prop_Type.Text = "N/D"
    '            Me.Prop_Count.Text = "N/D"
    '            ApplyMode = True
    '        Else
    '            Me.Prop_Name.Text = ci.Text
    '            If ci.DNAs.Count > 1 Then
    '                Me.Prop_Type.Text = "Blend"
    '                Me.Prop_Count.Text = ci.DNAs.Count.ToString
    '            Else
    '                Me.Prop_Type.Text = "Pure"
    '                Me.Prop_Count.Text = ci.DNAs.Count.ToString
    '            End If
    '        End If

    '        'Special Information
    '        Select Case ci.MolecularOperation
    '            Case Nuctions.MolecularOperationEnum.Vector
    '                RefreshVectorView()
    '            Case Nuctions.MolecularOperationEnum.Enzyme
    '                RefreshEnzymeView()
    '            Case Nuctions.MolecularOperationEnum.Gel
    '                RefreshGelView()
    '            Case Nuctions.MolecularOperationEnum.Ligation
    '                RefreshLigationView()
    '            Case Nuctions.MolecularOperationEnum.Modify
    '                RefreshModifyView()
    '            Case Nuctions.MolecularOperationEnum.PCR
    '                RefreshPCRView()
    '            Case Nuctions.MolecularOperationEnum.Screen
    '                RefreshScreenView()
    '        End Select
    '        RefreshOperationView()
    '        RefreshDNAView()
    '        'prevent the applymode if not creating
    '        If Not Me.RelatedChartItem.Creating Then Me.ApplyMode = False
    '        Return MyBase.ShowDialog(vParent)
    '    End Function
    '    Private Sub RefreshDNAView()
    '        Dim dna As Nuctions.GeneFile
    '        Dim ci As ChartItem
    '        If DNAView.Items.Count > 0 Then DNAView.Items.Clear()
    '        For Each dna In Me.RelatedChartItem.DNAs
    '            ci = New ChartItem
    '            ci.Name = dna.Name
    '            ci.Text = dna.Name
    '            ci.DNAs.Add(dna)
    '            ci.MolecularOperation = Me.RelatedChartItem.MolecularOperation
    '            ci.SubItems.Add(dna.Sequence.Length.ToString)
    '            ci.SubItems.Add(IIf(dna.Iscircular, "Circular", "Linear"))
    '            ci.SubItems.Add(IIf(dna.Iscircular, "N/D", "F:" + dna.End_F + "; R:" + dna.End_R + ";"))
    '            ci.SubItems.Add(IIf(dna.Iscircular, "N/D", "F:" + dna.Phos_F.ToString + "; R:" + dna.Phos_R.ToString + ";"))
    '            DNAView.Items.Add(ci)
    '        Next
    '        Me.Prop_Count.Text = Me.RelatedChartItem.DNAs.Count.ToString
    '        Me.Prop_Type.Text = IIf(Me.RelatedChartItem.DNAs.Count > 1, "Blend", "Pure")
    '    End Sub
    '    Private Sub RefreshVectorView()
    '        Dim fn As IO.FileInfo = New IO.FileInfo(Me.RelatedChartItem.File_Filename)
    '        Me.File_FileName_LinkLabel.Text = fn.Name
    '        Me.File_Path_Label.Text = fn.FullName
    '    End Sub
    '    Private Sub RefreshLigationView()
    '        Me.Ligation_TriFrag.Checked = Me.RelatedChartItem.Ligation_TriFragment
    '    End Sub
    '    Private Sub RefreshScreenView()
    '        If Me.RelatedChartItem.Screen_Mode = Nuctions.ScreenModeEnum.Features Then
    '            Me.Screen_Freatures.Checked = True
    '            Me.Screen_Features_LinkLabel.Visible = True
    '            Me.Screen_PCR_Panel.Visible = False
    '        Else
    '            Me.Screen_PCR.Checked = True
    '            Me.Screen_Features_LinkLabel.Visible = False
    '            Me.Screen_PCR_Panel.Visible = True
    '        End If
    '        Dim ei As Nuctions.Feature
    '        Dim sourcetext As String = ""
    '        For Each ei In Me.RelatedChartItem.Screen_Features
    '            sourcetext &= ei.Label + ";"
    '        Next
    '        If sourcetext.Length = 0 Then sourcetext = "[Click to select]"
    '        Me.Screen_Features_LinkLabel.Text = sourcetext
    '        Me.Screen_PCR_F.Text = Me.RelatedChartItem.Screen_FPrimer
    '        Me.Screen_PCR_R.Text = Me.RelatedChartItem.Screen_RPrimer
    '        Me.Screen_PCR_nudMax.Value = Me.RelatedChartItem.Screen_PCRMax
    '        Me.Screen_PCR_nudMin.Value = Me.RelatedChartItem.Screen_PCRMin

    '    End Sub
    '    Private Sub RefreshModifyView()
    '        Select Case Me.RelatedChartItem.Modify_Method
    '            Case Nuctions.ModificationMethodEnum.CIAP
    '                Me.Modify_CIAP.Checked = True
    '            Case Nuctions.ModificationMethodEnum.Klewnow
    '                Me.Modify_Klewnow.Checked = True
    '            Case Nuctions.ModificationMethodEnum.PNK
    '                Me.Modify_PNK.Checked = True
    '            Case Nuctions.ModificationMethodEnum.T4DNAP
    '                Me.Modify_T4.Checked = True
    '        End Select
    '    End Sub
    '    Private Sub RefreshPCRView()
    '        Me.PCR_ForwardPrimer_TextBox.Text = Me.RelatedChartItem.PCR_ForwardPrimer
    '        Me.PCR_ReversePrimer_TextBox.Text = Me.RelatedChartItem.PCR_ReversePrimer
    '    End Sub
    '    Private Sub RefreshGelView()
    '        Me.Gel_Maximum_Number.Value = Me.RelatedChartItem.Gel_Maximun
    '        Me.Gel_Minimum_Number.Value = Me.RelatedChartItem.Gel_Minimum
    '    End Sub
    '    Private Sub Screen_Features_LinkLabel_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles Screen_Features_LinkLabel.LinkClicked
    '        Dim ef As New frmFeatures
    '        ef.LoadFeatureItems(Me.RelatedChartItem.FeatureCol, Me.RelatedChartItem.Screen_Features)
    '        If ef.ShowDialog = Windows.Forms.DialogResult.OK Then RefreshScreenView() : Me.ApplyMode = True
    '    End Sub

    '    Private Sub PCR_Primer_TextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PCR_ForwardPrimer_TextBox.TextChanged, PCR_ReversePrimer_TextBox.TextChanged
    '        Me.ApplyMode = True
    '    End Sub

    '    Private Sub ll_ViewLarge_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ll_ViewLarge.LinkClicked
    '        DNAView.View = View.LargeIcon
    '    End Sub

    '    Private Sub ll_ViewDetails_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ll_ViewDetails.LinkClicked
    '        DNAView.View = View.Details
    '    End Sub

    '    Private Sub btn_RCR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_RCR.Click
    '        Me.PCR_ReversePrimer_TextBox.Text = Nuctions.ReverseComplement(Me.PCR_ReversePrimer_TextBox.Text)
    '    End Sub

    '    Private Sub btn_RCF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_RCF.Click
    '        Me.PCR_ForwardPrimer_TextBox.Text = Nuctions.ReverseComplement(Me.PCR_ForwardPrimer_TextBox.Text)
    '    End Sub

    '    Private Sub frmProperty_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
    '        Select Case e.KeyCode
    '            Case Keys.F1
    '                Me.Prop_Name.Focus()
    '                Me.Prop_Name.SelectAll()
    '            Case Keys.F2
    '                Me.rtb_Description.Focus()
    '                Me.rtb_Description.SelectAll()
    '            Case Keys.Enter
    '                If Control.ModifierKeys = Keys.Control Then
    '                    Accept()
    '                End If
    '            Case Keys.Escape
    '                Me.Close()
    '        End Select
    '    End Sub


    '    Private Sub Screen_Freatures_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_Freatures.CheckedChanged
    '        Me.Screen_Features_LinkLabel.Visible = True
    '        Me.Screen_PCR_Panel.Visible = False
    '    End Sub

    '    Private Sub Screen_PCR_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_PCR.CheckedChanged
    '        Me.Screen_PCR_Panel.Visible = True
    '        Me.Screen_Features_LinkLabel.Visible = False
    '    End Sub

    '    Private Sub Screen_PCR_RCF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_PCR_RCF.Click
    '        Me.Screen_PCR_F.Text = Nuctions.ReverseComplement(Me.Screen_PCR_F.Text)
    '    End Sub

    '    Private Sub Screen_PCR_RCR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_PCR_RCR.Click
    '        Me.Screen_PCR_R.Text = Nuctions.ReverseComplement(Me.Screen_PCR_R.Text)
    '    End Sub

    '    Private Sub Screen_PCR_F_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_PCR_F.TextChanged
    '        Me.ApplyMode = True
    '    End Sub

    '    Private Sub Screen_PCR_R_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_PCR_R.TextChanged
    '        Me.ApplyMode = True
    '    End Sub

    '    Private Sub Screen_PCR_nudMax_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_PCR_nudMax.ValueChanged
    '        Me.ApplyMode = True
    '    End Sub

    '    Private Sub Screen_PCR_nudMin_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_PCR_nudMin.ValueChanged
    '        Me.ApplyMode = True
    '    End Sub
End Class