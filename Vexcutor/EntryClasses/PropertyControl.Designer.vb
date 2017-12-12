<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PropertyControl
    Inherits System.Windows.Forms.UserControl

    'UserControl 重写 Dispose，以清理组件列表。
    '<System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PropertyControl))
        Me.rtb_Description = New System.Windows.Forms.RichTextBox()
        Me.ll_ViewDetails = New System.Windows.Forms.LinkLabel()
        Me.DNAView = New System.Windows.Forms.ListView()
        Me.ch_Name = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ch_Size = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cn_Cir = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ch_ENDS = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ch_Phos = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chChromo = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Prop_Name = New System.Windows.Forms.TextBox()
        Me.Prop_Count = New System.Windows.Forms.LinkLabel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Prop_Type = New System.Windows.Forms.LinkLabel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Prop_Operation = New System.Windows.Forms.LinkLabel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.llName = New System.Windows.Forms.LinkLabel()
        Me.TabControl_Operation = New System.Windows.Forms.TabControl()
        Me.tpFile = New System.Windows.Forms.TabPage()
        Me.File_FileName_LinkLabel = New System.Windows.Forms.LinkLabel()
        Me.File_Path_Label = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tpEnzyme = New System.Windows.Forms.TabPage()
        Me.cbDephosphorylate = New System.Windows.Forms.CheckBox()
        Me.tbEnzymes = New System.Windows.Forms.TextBox()
        Me.Enzyme_Enzymes_LinkLabel = New System.Windows.Forms.LinkLabel()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.tpPCR = New System.Windows.Forms.TabPage()
        Me.btnOverlap = New System.Windows.Forms.Button()
        Me.lblRCount = New System.Windows.Forms.Label()
        Me.lblFCount = New System.Windows.Forms.Label()
        Me.pafPCR = New MCDS.PrimerAnalysisFrame()
        Me.btnBothPrimer = New System.Windows.Forms.Button()
        Me.tbRP = New System.Windows.Forms.ComboBox()
        Me.PCR_ReversePrimer_TextBox = New System.Windows.Forms.TextBox()
        Me.tbFP = New System.Windows.Forms.ComboBox()
        Me.PCR_ForwardPrimer_TextBox = New System.Windows.Forms.TextBox()
        Me.tpModify = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Modify_PNK = New System.Windows.Forms.RadioButton()
        Me.Modify_CIAP = New System.Windows.Forms.RadioButton()
        Me.Modify_Klewnow = New System.Windows.Forms.RadioButton()
        Me.Modify_T4 = New System.Windows.Forms.RadioButton()
        Me.tpGel = New System.Windows.Forms.TabPage()
        Me.cbGel_Solution = New System.Windows.Forms.CheckBox()
        Me.cbAuto = New System.Windows.Forms.CheckBox()
        Me.pnlGel = New System.Windows.Forms.Panel()
        Me.Gel_Maximum_Number = New System.Windows.Forms.NumericUpDown()
        Me.Gel_Minimum_Number = New System.Windows.Forms.NumericUpDown()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.tpLigation = New System.Windows.Forms.TabPage()
        Me.ihLigation = New MCDS.InteropHost()
        Me.ucWpfLigationPanel = New MCDS.WPFLigationPanel()
        Me.tpScreen = New System.Windows.Forms.TabPage()
        Me.btnScreenReset = New System.Windows.Forms.Button()
        Me.cbScreenCircular = New System.Windows.Forms.CheckBox()
        Me.Screen_PCR_Panel = New System.Windows.Forms.Panel()
        Me.pnlScreenPCR = New System.Windows.Forms.Panel()
        Me.Screen_PCR_nudMax = New System.Windows.Forms.NumericUpDown()
        Me.Screen_PCR_nudMin = New System.Windows.Forms.NumericUpDown()
        Me.Screen_PCR_lblMax = New System.Windows.Forms.Label()
        Me.Screen_PCR_lblMin = New System.Windows.Forms.Label()
        Me.Screen_PCR_RCF = New System.Windows.Forms.Button()
        Me.tbSCRRP = New System.Windows.Forms.ComboBox()
        Me.Screen_PCR_R = New System.Windows.Forms.TextBox()
        Me.tbSCRFP = New System.Windows.Forms.ComboBox()
        Me.Screen_PCR_F = New System.Windows.Forms.TextBox()
        Me.Screen_PCR = New System.Windows.Forms.RadioButton()
        Me.Screen_Freatures = New System.Windows.Forms.RadioButton()
        Me.Screen_Features_LinkLabel = New System.Windows.Forms.LinkLabel()
        Me.pnlFeature = New System.Windows.Forms.Panel()
        Me.tpRec = New System.Windows.Forms.TabPage()
        Me.ihRecombination = New MCDS.InteropHost()
        Me.ucWpfRecombinationPanel = New MCDS.WPFRecombinationPanel()
        Me.tpEnzymeAnalysis = New System.Windows.Forms.TabPage()
        Me.llAnalysisRemove = New System.Windows.Forms.LinkLabel()
        Me.llAnalysisAdd = New System.Windows.Forms.LinkLabel()
        Me.dgvEnzymeAnalysis = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cmsEnzyme = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tpSequenceMerge = New System.Windows.Forms.TabPage()
        Me.cbMergeExtend = New System.Windows.Forms.CheckBox()
        Me.cbMergeSignificant = New System.Windows.Forms.CheckBox()
        Me.tbFreeDesign = New System.Windows.Forms.TabPage()
        Me.cbDesign_UseDesigner = New System.Windows.Forms.CheckBox()
        Me.tbDesign = New System.Windows.Forms.TextBox()
        Me.rtbFreeDesign = New System.Windows.Forms.RichTextBox()
        Me.tpHashPicker = New System.Windows.Forms.TabPage()
        Me.cpHashPickerListedDNA = New MCDS.ChoicePanel()
        Me.cpHashPickerChoosenDNA = New MCDS.ChoicePanel()
        Me.lblHashPickerList = New System.Windows.Forms.Label()
        Me.lblHashPickerChoosen = New System.Windows.Forms.Label()
        Me.tpSequencingResult = New System.Windows.Forms.TabPage()
        Me.cbSequencingResultOption = New System.Windows.Forms.ComboBox()
        Me.lbl_Sequencing_NameTag = New System.Windows.Forms.Label()
        Me.rtbSequencingResult = New System.Windows.Forms.RichTextBox()
        Me.tbSequencingPrimerName = New System.Windows.Forms.ComboBox()
        Me.tbSequencingPrimerSequence = New System.Windows.Forms.TextBox()
        Me.tpComparer = New System.Windows.Forms.TabPage()
        Me.cbCompareResult = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cpCompareList = New MCDS.ChoicePanel()
        Me.cpCompareChoice = New MCDS.ChoicePanel()
        Me.tpHost = New System.Windows.Forms.TabPage()
        Me.tbChromosomeFragmentName = New System.Windows.Forms.TextBox()
        Me.tbHostFunction = New System.Windows.Forms.TextBox()
        Me.tbHostName = New System.Windows.Forms.TextBox()
        Me.lbFragmentName = New System.Windows.Forms.Label()
        Me.llHostFunctions = New System.Windows.Forms.Label()
        Me.lbHostName = New System.Windows.Forms.Label()
        Me.rtbChromosomoFragment = New System.Windows.Forms.RichTextBox()
        Me.llRemoveChromosomeFragment = New System.Windows.Forms.LinkLabel()
        Me.llAddChromosomeFragment = New System.Windows.Forms.LinkLabel()
        Me.tpTransformation = New System.Windows.Forms.TabPage()
        Me.gbTransformationMode = New System.Windows.Forms.GroupBox()
        Me.rbTransformationCBNT = New System.Windows.Forms.RadioButton()
        Me.rbTransformationEDPC = New System.Windows.Forms.RadioButton()
        Me.rbTransformationAIOC = New System.Windows.Forms.RadioButton()
        Me.gbTransfromation = New System.Windows.Forms.GroupBox()
        Me.rbTransformationChemical = New System.Windows.Forms.RadioButton()
        Me.rbTransformationConjugation = New System.Windows.Forms.RadioButton()
        Me.rbTransformationElectroporation = New System.Windows.Forms.RadioButton()
        Me.tpIncubation = New System.Windows.Forms.TabPage()
        Me.llCommonStep = New System.Windows.Forms.LinkLabel()
        Me.llRemoveIncubation = New System.Windows.Forms.LinkLabel()
        Me.llAddIncubation = New System.Windows.Forms.LinkLabel()
        Me.dgvIncubation = New System.Windows.Forms.DataGridView()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tpExtraction = New System.Windows.Forms.TabPage()
        Me.cbExtractionSequencingVerify = New System.Windows.Forms.CheckBox()
        Me.cbExtractionIncludeVerification = New System.Windows.Forms.CheckBox()
        Me.tpProteinExpression = New System.Windows.Forms.TabPage()
        Me.pnlExpression = New System.Windows.Forms.Panel()
        Me.tpGibsonDesign = New System.Windows.Forms.TabPage()
        Me.ihGibsonDesign = New MCDS.InteropHost()
        Me.ucWpfGibsonDesignPanel = New MCDS.WPFGibsonDesignPanel()
        Me.tpCRISPRCut = New System.Windows.Forms.TabPage()
        Me.ihCRISPRCut = New MCDS.InteropHost()
        Me.ucWpfcrisprCutPanel = New MCDS.WPFCRISPRCutPanel()
        Me.llApply = New System.Windows.Forms.LinkLabel()
        Me.llCancel = New System.Windows.Forms.LinkLabel()
        Me.cbRealSize = New System.Windows.Forms.CheckBox()
        Me.ofdVector = New System.Windows.Forms.OpenFileDialog()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.rbObsolete = New System.Windows.Forms.RadioButton()
        Me.rbFinished = New System.Windows.Forms.RadioButton()
        Me.rbInprogress = New System.Windows.Forms.RadioButton()
        Me.rbUnstarted = New System.Windows.Forms.RadioButton()
        Me.lblPixelPerBP = New System.Windows.Forms.Label()
        Me.ttDescription = New System.Windows.Forms.ToolTip(Me.components)
        Me.rtbEnzymeAnalysisResults = New System.Windows.Forms.RichTextBox()
        Me.cbDescribe = New System.Windows.Forms.CheckBox()
        Me.snbPixelPerKBP = New MCDS.ScrollingNumberBox()
        Me.llHelp = New System.Windows.Forms.LinkLabel()
        Me.cbEnvironment = New System.Windows.Forms.ComboBox()
        Me.cbVerify = New System.Windows.Forms.CheckBox()
        Me.lvCells = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cbMainConstruction = New System.Windows.Forms.CheckBox()
        Me.cbNoMap = New System.Windows.Forms.CheckBox()
        Me.TabControl_Operation.SuspendLayout()
        Me.tpFile.SuspendLayout()
        Me.tpEnzyme.SuspendLayout()
        Me.tpPCR.SuspendLayout()
        CType(Me.pafPCR, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpModify.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.tpGel.SuspendLayout()
        CType(Me.Gel_Maximum_Number, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Gel_Minimum_Number, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpLigation.SuspendLayout()
        Me.tpScreen.SuspendLayout()
        Me.Screen_PCR_Panel.SuspendLayout()
        CType(Me.Screen_PCR_nudMax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Screen_PCR_nudMin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpRec.SuspendLayout()
        Me.tpEnzymeAnalysis.SuspendLayout()
        CType(Me.dgvEnzymeAnalysis, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmsEnzyme.SuspendLayout()
        Me.tpSequenceMerge.SuspendLayout()
        Me.tbFreeDesign.SuspendLayout()
        Me.tpHashPicker.SuspendLayout()
        Me.tpSequencingResult.SuspendLayout()
        Me.tpComparer.SuspendLayout()
        Me.tpHost.SuspendLayout()
        Me.tpTransformation.SuspendLayout()
        Me.gbTransformationMode.SuspendLayout()
        Me.gbTransfromation.SuspendLayout()
        Me.tpIncubation.SuspendLayout()
        CType(Me.dgvIncubation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpExtraction.SuspendLayout()
        Me.tpProteinExpression.SuspendLayout()
        Me.tpGibsonDesign.SuspendLayout()
        Me.tpCRISPRCut.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'rtb_Description
        '
        Me.rtb_Description.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rtb_Description.BackColor = System.Drawing.Color.Azure
        Me.rtb_Description.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtb_Description.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtb_Description.Location = New System.Drawing.Point(9, 248)
        Me.rtb_Description.Name = "rtb_Description"
        Me.rtb_Description.Size = New System.Drawing.Size(497, 89)
        Me.rtb_Description.TabIndex = 27
        Me.rtb_Description.Text = ""
        Me.ttDescription.SetToolTip(Me.rtb_Description, "Descriptions of this operation.")
        '
        'll_ViewDetails
        '
        Me.ll_ViewDetails.AutoSize = True
        Me.ll_ViewDetails.BackColor = System.Drawing.Color.Transparent
        Me.ll_ViewDetails.Location = New System.Drawing.Point(512, 35)
        Me.ll_ViewDetails.Name = "ll_ViewDetails"
        Me.ll_ViewDetails.Size = New System.Drawing.Size(64, 14)
        Me.ll_ViewDetails.TabIndex = 24
        Me.ll_ViewDetails.TabStop = True
        Me.ll_ViewDetails.Text = "View DNAs"
        Me.ttDescription.SetToolTip(Me.ll_ViewDetails, "Click to see the DNA products in another tab.")
        '
        'DNAView
        '
        Me.DNAView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DNAView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ch_Name, Me.ch_Size, Me.cn_Cir, Me.ch_ENDS, Me.ch_Phos, Me.chChromo})
        Me.DNAView.Location = New System.Drawing.Point(512, 50)
        Me.DNAView.Name = "DNAView"
        Me.DNAView.Size = New System.Drawing.Size(497, 287)
        Me.DNAView.TabIndex = 22
        Me.ttDescription.SetToolTip(Me.DNAView, "DNA products of this operation." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Double click one DNA to open a main tab for DNA " &
        "for Editing.")
        Me.DNAView.UseCompatibleStateImageBehavior = False
        Me.DNAView.View = System.Windows.Forms.View.Details
        '
        'ch_Name
        '
        Me.ch_Name.Text = "DNA"
        Me.ch_Name.Width = 100
        '
        'ch_Size
        '
        Me.ch_Size.Text = "Size"
        '
        'cn_Cir
        '
        Me.cn_Cir.Text = "Shape"
        Me.cn_Cir.Width = 70
        '
        'ch_ENDS
        '
        Me.ch_ENDS.Text = "F End"
        Me.ch_ENDS.Width = 80
        '
        'ch_Phos
        '
        Me.ch_Phos.Text = "R End"
        Me.ch_Phos.Width = 80
        '
        'chChromo
        '
        Me.chChromo.Text = "Chromosome"
        Me.chChromo.Width = 80
        '
        'Prop_Name
        '
        Me.Prop_Name.Location = New System.Drawing.Point(55, 0)
        Me.Prop_Name.Name = "Prop_Name"
        Me.Prop_Name.Size = New System.Drawing.Size(161, 20)
        Me.Prop_Name.TabIndex = 21
        '
        'Prop_Count
        '
        Me.Prop_Count.AutoSize = True
        Me.Prop_Count.BackColor = System.Drawing.Color.Transparent
        Me.Prop_Count.Font = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Prop_Count.LinkColor = System.Drawing.Color.Red
        Me.Prop_Count.Location = New System.Drawing.Point(415, 3)
        Me.Prop_Count.Name = "Prop_Count"
        Me.Prop_Count.Size = New System.Drawing.Size(47, 12)
        Me.Prop_Count.TabIndex = 16
        Me.Prop_Count.TabStop = True
        Me.Prop_Count.Text = "Number"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(374, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(35, 14)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Count"
        '
        'Prop_Type
        '
        Me.Prop_Type.AutoSize = True
        Me.Prop_Type.BackColor = System.Drawing.Color.Transparent
        Me.Prop_Type.Font = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Prop_Type.LinkColor = System.Drawing.Color.Red
        Me.Prop_Type.Location = New System.Drawing.Point(257, 3)
        Me.Prop_Type.Name = "Prop_Type"
        Me.Prop_Type.Size = New System.Drawing.Size(89, 12)
        Me.Prop_Type.TabIndex = 18
        Me.Prop_Type.TabStop = True
        Me.Prop_Type.Text = "Mixture/Pure"
        Me.Prop_Type.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(223, 3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(33, 14)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "Type "
        Me.Label3.Visible = False
        '
        'Prop_Operation
        '
        Me.Prop_Operation.AutoSize = True
        Me.Prop_Operation.BackColor = System.Drawing.Color.Transparent
        Me.Prop_Operation.Font = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Prop_Operation.LinkColor = System.Drawing.Color.Red
        Me.Prop_Operation.Location = New System.Drawing.Point(53, 24)
        Me.Prop_Operation.Name = "Prop_Operation"
        Me.Prop_Operation.Size = New System.Drawing.Size(103, 12)
        Me.Prop_Operation.TabIndex = 17
        Me.Prop_Operation.TabStop = True
        Me.Prop_Operation.Text = "Operation/File"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(6, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 14)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Source"
        '
        'llName
        '
        Me.llName.AutoSize = True
        Me.llName.BackColor = System.Drawing.Color.Transparent
        Me.llName.LinkColor = System.Drawing.Color.RoyalBlue
        Me.llName.Location = New System.Drawing.Point(6, 3)
        Me.llName.Name = "llName"
        Me.llName.Size = New System.Drawing.Size(28, 14)
        Me.llName.TabIndex = 14
        Me.llName.TabStop = True
        Me.llName.Text = "<ID>"
        '
        'TabControl_Operation
        '
        Me.TabControl_Operation.Controls.Add(Me.tpFile)
        Me.TabControl_Operation.Controls.Add(Me.tpEnzyme)
        Me.TabControl_Operation.Controls.Add(Me.tpPCR)
        Me.TabControl_Operation.Controls.Add(Me.tpModify)
        Me.TabControl_Operation.Controls.Add(Me.tpGel)
        Me.TabControl_Operation.Controls.Add(Me.tpLigation)
        Me.TabControl_Operation.Controls.Add(Me.tpScreen)
        Me.TabControl_Operation.Controls.Add(Me.tpRec)
        Me.TabControl_Operation.Controls.Add(Me.tpEnzymeAnalysis)
        Me.TabControl_Operation.Controls.Add(Me.tpSequenceMerge)
        Me.TabControl_Operation.Controls.Add(Me.tbFreeDesign)
        Me.TabControl_Operation.Controls.Add(Me.tpHashPicker)
        Me.TabControl_Operation.Controls.Add(Me.tpSequencingResult)
        Me.TabControl_Operation.Controls.Add(Me.tpComparer)
        Me.TabControl_Operation.Controls.Add(Me.tpHost)
        Me.TabControl_Operation.Controls.Add(Me.tpTransformation)
        Me.TabControl_Operation.Controls.Add(Me.tpIncubation)
        Me.TabControl_Operation.Controls.Add(Me.tpExtraction)
        Me.TabControl_Operation.Controls.Add(Me.tpProteinExpression)
        Me.TabControl_Operation.Controls.Add(Me.tpGibsonDesign)
        Me.TabControl_Operation.Controls.Add(Me.tpCRISPRCut)
        Me.TabControl_Operation.Location = New System.Drawing.Point(4, 39)
        Me.TabControl_Operation.Name = "TabControl_Operation"
        Me.TabControl_Operation.SelectedIndex = 0
        Me.TabControl_Operation.Size = New System.Drawing.Size(505, 210)
        Me.TabControl_Operation.TabIndex = 11
        '
        'tpFile
        '
        Me.tpFile.Controls.Add(Me.File_FileName_LinkLabel)
        Me.tpFile.Controls.Add(Me.File_Path_Label)
        Me.tpFile.Controls.Add(Me.Label18)
        Me.tpFile.Controls.Add(Me.Label5)
        Me.tpFile.Location = New System.Drawing.Point(4, 23)
        Me.tpFile.Name = "tpFile"
        Me.tpFile.Size = New System.Drawing.Size(497, 183)
        Me.tpFile.TabIndex = 6
        Me.tpFile.Text = "File"
        Me.tpFile.UseVisualStyleBackColor = True
        '
        'File_FileName_LinkLabel
        '
        Me.File_FileName_LinkLabel.AutoSize = True
        Me.File_FileName_LinkLabel.Location = New System.Drawing.Point(101, 27)
        Me.File_FileName_LinkLabel.Name = "File_FileName_LinkLabel"
        Me.File_FileName_LinkLabel.Size = New System.Drawing.Size(76, 14)
        Me.File_FileName_LinkLabel.TabIndex = 1
        Me.File_FileName_LinkLabel.TabStop = True
        Me.File_FileName_LinkLabel.Text = "[Filename text]"
        Me.ttDescription.SetToolTip(Me.File_FileName_LinkLabel, "The original file address." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Click to choose a different file.")
        '
        'File_Path_Label
        '
        Me.File_Path_Label.AutoSize = True
        Me.File_Path_Label.Location = New System.Drawing.Point(101, 54)
        Me.File_Path_Label.Name = "File_Path_Label"
        Me.File_Path_Label.Size = New System.Drawing.Size(28, 14)
        Me.File_Path_Label.TabIndex = 0
        Me.File_Path_Label.Text = "Path"
        Me.ttDescription.SetToolTip(Me.File_Path_Label, "Original location of this file.")
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(21, 54)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(28, 14)
        Me.Label18.TabIndex = 0
        Me.Label18.Text = "Path"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(22, 27)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 14)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Filename"
        '
        'tpEnzyme
        '
        Me.tpEnzyme.Controls.Add(Me.cbDephosphorylate)
        Me.tpEnzyme.Controls.Add(Me.tbEnzymes)
        Me.tpEnzyme.Controls.Add(Me.Enzyme_Enzymes_LinkLabel)
        Me.tpEnzyme.Controls.Add(Me.Label12)
        Me.tpEnzyme.Location = New System.Drawing.Point(4, 22)
        Me.tpEnzyme.Name = "tpEnzyme"
        Me.tpEnzyme.Padding = New System.Windows.Forms.Padding(3)
        Me.tpEnzyme.Size = New System.Drawing.Size(497, 184)
        Me.tpEnzyme.TabIndex = 0
        Me.tpEnzyme.Text = "Enzyme"
        Me.tpEnzyme.UseVisualStyleBackColor = True
        '
        'cbDephosphorylate
        '
        Me.cbDephosphorylate.AutoSize = True
        Me.cbDephosphorylate.Location = New System.Drawing.Point(5, 91)
        Me.cbDephosphorylate.Name = "cbDephosphorylate"
        Me.cbDephosphorylate.Size = New System.Drawing.Size(108, 18)
        Me.cbDephosphorylate.TabIndex = 4
        Me.cbDephosphorylate.Text = "Dephosphorylate"
        Me.cbDephosphorylate.UseVisualStyleBackColor = True
        '
        'tbEnzymes
        '
        Me.tbEnzymes.Location = New System.Drawing.Point(5, 51)
        Me.tbEnzymes.Name = "tbEnzymes"
        Me.tbEnzymes.Size = New System.Drawing.Size(486, 20)
        Me.tbEnzymes.TabIndex = 3
        Me.ttDescription.SetToolTip(Me.tbEnzymes, "Type the enzyme names and press enter to apply.")
        '
        'Enzyme_Enzymes_LinkLabel
        '
        Me.Enzyme_Enzymes_LinkLabel.AutoSize = True
        Me.Enzyme_Enzymes_LinkLabel.Location = New System.Drawing.Point(3, 26)
        Me.Enzyme_Enzymes_LinkLabel.Name = "Enzyme_Enzymes_LinkLabel"
        Me.Enzyme_Enzymes_LinkLabel.Size = New System.Drawing.Size(79, 14)
        Me.Enzyme_Enzymes_LinkLabel.TabIndex = 2
        Me.Enzyme_Enzymes_LinkLabel.TabStop = True
        Me.Enzyme_Enzymes_LinkLabel.Text = "[Click to select]"
        Me.ttDescription.SetToolTip(Me.Enzyme_Enzymes_LinkLabel, "Click to select or unselect enzyme in an enzyme tab.")
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(6, 3)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(51, 14)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Enzymes"
        '
        'tpPCR
        '
        Me.tpPCR.Controls.Add(Me.btnOverlap)
        Me.tpPCR.Controls.Add(Me.lblRCount)
        Me.tpPCR.Controls.Add(Me.lblFCount)
        Me.tpPCR.Controls.Add(Me.pafPCR)
        Me.tpPCR.Controls.Add(Me.btnBothPrimer)
        Me.tpPCR.Controls.Add(Me.tbRP)
        Me.tpPCR.Controls.Add(Me.PCR_ReversePrimer_TextBox)
        Me.tpPCR.Controls.Add(Me.tbFP)
        Me.tpPCR.Controls.Add(Me.PCR_ForwardPrimer_TextBox)
        Me.tpPCR.Location = New System.Drawing.Point(4, 22)
        Me.tpPCR.Name = "tpPCR"
        Me.tpPCR.Padding = New System.Windows.Forms.Padding(3)
        Me.tpPCR.Size = New System.Drawing.Size(497, 184)
        Me.tpPCR.TabIndex = 1
        Me.tpPCR.Text = "PCR"
        Me.tpPCR.UseVisualStyleBackColor = True
        '
        'btnOverlap
        '
        Me.btnOverlap.Location = New System.Drawing.Point(461, 0)
        Me.btnOverlap.Name = "btnOverlap"
        Me.btnOverlap.Size = New System.Drawing.Size(15, 39)
        Me.btnOverlap.TabIndex = 5
        Me.btnOverlap.Text = "--"
        Me.ttDescription.SetToolTip(Me.btnOverlap, "Click to swith between Normal mode and Overlap extension mode.")
        Me.btnOverlap.UseVisualStyleBackColor = True
        '
        'lblRCount
        '
        Me.lblRCount.AutoSize = True
        Me.lblRCount.Location = New System.Drawing.Point(442, 22)
        Me.lblRCount.Name = "lblRCount"
        Me.lblRCount.Size = New System.Drawing.Size(13, 14)
        Me.lblRCount.TabIndex = 0
        Me.lblRCount.Text = "0"
        '
        'lblFCount
        '
        Me.lblFCount.AutoSize = True
        Me.lblFCount.Location = New System.Drawing.Point(442, 3)
        Me.lblFCount.Name = "lblFCount"
        Me.lblFCount.Size = New System.Drawing.Size(13, 14)
        Me.lblFCount.TabIndex = 0
        Me.lblFCount.Text = "0"
        '
        'pafPCR
        '
        Me.pafPCR.Location = New System.Drawing.Point(0, 40)
        Me.pafPCR.Name = "pafPCR"
        Me.pafPCR.ShowSequencing = False
        Me.pafPCR.Size = New System.Drawing.Size(497, 145)
        Me.pafPCR.TabIndex = 3
        Me.pafPCR.TabStop = False
        Me.ttDescription.SetToolTip(Me.pafPCR, "Primer details.")
        '
        'btnBothPrimer
        '
        Me.btnBothPrimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBothPrimer.Location = New System.Drawing.Point(476, 0)
        Me.btnBothPrimer.Name = "btnBothPrimer"
        Me.btnBothPrimer.Size = New System.Drawing.Size(21, 40)
        Me.btnBothPrimer.TabIndex = 6
        Me.btnBothPrimer.Text = "PR"
        Me.ttDescription.SetToolTip(Me.btnBothPrimer, "Click to Open Primer Design Tab.")
        Me.btnBothPrimer.UseVisualStyleBackColor = True
        '
        'tbRP
        '
        Me.tbRP.Location = New System.Drawing.Point(0, 19)
        Me.tbRP.Name = "tbRP"
        Me.tbRP.Size = New System.Drawing.Size(68, 22)
        Me.tbRP.TabIndex = 3
        Me.ttDescription.SetToolTip(Me.tbRP, resources.GetString("tbRP.ToolTip"))
        '
        'PCR_ReversePrimer_TextBox
        '
        Me.PCR_ReversePrimer_TextBox.Location = New System.Drawing.Point(67, 19)
        Me.PCR_ReversePrimer_TextBox.Name = "PCR_ReversePrimer_TextBox"
        Me.PCR_ReversePrimer_TextBox.Size = New System.Drawing.Size(368, 20)
        Me.PCR_ReversePrimer_TextBox.TabIndex = 4
        Me.ttDescription.SetToolTip(Me.PCR_ReversePrimer_TextBox, "Reverse Primer Sequence." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please use > separate binding part and attached part.")
        '
        'tbFP
        '
        Me.tbFP.Location = New System.Drawing.Point(0, 0)
        Me.tbFP.Name = "tbFP"
        Me.tbFP.Size = New System.Drawing.Size(68, 22)
        Me.tbFP.TabIndex = 1
        Me.ttDescription.SetToolTip(Me.tbFP, resources.GetString("tbFP.ToolTip"))
        '
        'PCR_ForwardPrimer_TextBox
        '
        Me.PCR_ForwardPrimer_TextBox.Location = New System.Drawing.Point(67, 0)
        Me.PCR_ForwardPrimer_TextBox.Name = "PCR_ForwardPrimer_TextBox"
        Me.PCR_ForwardPrimer_TextBox.Size = New System.Drawing.Size(368, 20)
        Me.PCR_ForwardPrimer_TextBox.TabIndex = 2
        Me.ttDescription.SetToolTip(Me.PCR_ForwardPrimer_TextBox, "Forward Primer Sequence." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please use > separate binding part and attached part.")
        '
        'tpModify
        '
        Me.tpModify.Controls.Add(Me.GroupBox1)
        Me.tpModify.Location = New System.Drawing.Point(4, 22)
        Me.tpModify.Name = "tpModify"
        Me.tpModify.Size = New System.Drawing.Size(497, 184)
        Me.tpModify.TabIndex = 3
        Me.tpModify.Text = "Modify"
        Me.tpModify.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Modify_PNK)
        Me.GroupBox1.Controls.Add(Me.Modify_CIAP)
        Me.GroupBox1.Controls.Add(Me.Modify_Klewnow)
        Me.GroupBox1.Controls.Add(Me.Modify_T4)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(497, 184)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Modification Methods"
        '
        'Modify_PNK
        '
        Me.Modify_PNK.AutoSize = True
        Me.Modify_PNK.Location = New System.Drawing.Point(239, 42)
        Me.Modify_PNK.Name = "Modify_PNK"
        Me.Modify_PNK.Size = New System.Drawing.Size(109, 18)
        Me.Modify_PNK.TabIndex = 0
        Me.Modify_PNK.TabStop = True
        Me.Modify_PNK.Text = "PNK Phosphorate"
        Me.ttDescription.SetToolTip(Me.Modify_PNK, resources.GetString("Modify_PNK.ToolTip"))
        Me.Modify_PNK.UseVisualStyleBackColor = True
        '
        'Modify_CIAP
        '
        Me.Modify_CIAP.AutoSize = True
        Me.Modify_CIAP.Location = New System.Drawing.Point(17, 20)
        Me.Modify_CIAP.Name = "Modify_CIAP"
        Me.Modify_CIAP.Size = New System.Drawing.Size(125, 18)
        Me.Modify_CIAP.TabIndex = 0
        Me.Modify_CIAP.TabStop = True
        Me.Modify_CIAP.Text = "CIAP Dephosphorate"
        Me.ttDescription.SetToolTip(Me.Modify_CIAP, resources.GetString("Modify_CIAP.ToolTip"))
        Me.Modify_CIAP.UseVisualStyleBackColor = True
        '
        'Modify_Klewnow
        '
        Me.Modify_Klewnow.AutoSize = True
        Me.Modify_Klewnow.Location = New System.Drawing.Point(239, 20)
        Me.Modify_Klewnow.Name = "Modify_Klewnow"
        Me.Modify_Klewnow.Size = New System.Drawing.Size(171, 18)
        Me.Modify_Klewnow.TabIndex = 0
        Me.Modify_Klewnow.TabStop = True
        Me.Modify_Klewnow.Text = "Blunt with Klewnow Fragment"
        Me.ttDescription.SetToolTip(Me.Modify_Klewnow, "Use Klewnow Fragment to blunt DNA." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note: Klewnow Fragment will only fill the 5' " &
        "overhang, but leave the 3' overhang.")
        Me.Modify_Klewnow.UseVisualStyleBackColor = True
        '
        'Modify_T4
        '
        Me.Modify_T4.AutoSize = True
        Me.Modify_T4.Location = New System.Drawing.Point(17, 42)
        Me.Modify_T4.Name = "Modify_T4"
        Me.Modify_T4.Size = New System.Drawing.Size(171, 18)
        Me.Modify_T4.TabIndex = 0
        Me.Modify_T4.TabStop = True
        Me.Modify_T4.Text = "Blunt with T4 DNA polymerase"
        Me.ttDescription.SetToolTip(Me.Modify_T4, resources.GetString("Modify_T4.ToolTip"))
        Me.Modify_T4.UseVisualStyleBackColor = True
        '
        'tpGel
        '
        Me.tpGel.Controls.Add(Me.cbGel_Solution)
        Me.tpGel.Controls.Add(Me.cbAuto)
        Me.tpGel.Controls.Add(Me.pnlGel)
        Me.tpGel.Controls.Add(Me.Gel_Maximum_Number)
        Me.tpGel.Controls.Add(Me.Gel_Minimum_Number)
        Me.tpGel.Controls.Add(Me.Label9)
        Me.tpGel.Controls.Add(Me.Label8)
        Me.tpGel.Location = New System.Drawing.Point(4, 23)
        Me.tpGel.Name = "tpGel"
        Me.tpGel.Size = New System.Drawing.Size(497, 183)
        Me.tpGel.TabIndex = 2
        Me.tpGel.Text = "Gel"
        Me.tpGel.UseVisualStyleBackColor = True
        '
        'cbGel_Solution
        '
        Me.cbGel_Solution.AutoSize = True
        Me.cbGel_Solution.Location = New System.Drawing.Point(234, 10)
        Me.cbGel_Solution.Name = "cbGel_Solution"
        Me.cbGel_Solution.Size = New System.Drawing.Size(115, 18)
        Me.cbGel_Solution.TabIndex = 4
        Me.cbGel_Solution.Text = "Solution Extraction"
        Me.cbGel_Solution.UseVisualStyleBackColor = True
        '
        'cbAuto
        '
        Me.cbAuto.AutoSize = True
        Me.cbAuto.Checked = True
        Me.cbAuto.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbAuto.Location = New System.Drawing.Point(398, 10)
        Me.cbAuto.Name = "cbAuto"
        Me.cbAuto.Size = New System.Drawing.Size(86, 18)
        Me.cbAuto.TabIndex = 3
        Me.cbAuto.Text = "AutoChoose"
        Me.ttDescription.SetToolTip(Me.cbAuto, "When ticked, Gel Separation Algorithm will automatically choose the only one cand" &
        "idate, ignoring the length, if there is only one.")
        Me.cbAuto.UseVisualStyleBackColor = True
        '
        'pnlGel
        '
        Me.pnlGel.Location = New System.Drawing.Point(0, 35)
        Me.pnlGel.Name = "pnlGel"
        Me.pnlGel.Size = New System.Drawing.Size(497, 150)
        Me.pnlGel.TabIndex = 2
        Me.ttDescription.SetToolTip(Me.pnlGel, "Click to select the DNA.")
        '
        'Gel_Maximum_Number
        '
        Me.Gel_Maximum_Number.Increment = New Decimal(New Integer() {50, 0, 0, 0})
        Me.Gel_Maximum_Number.Location = New System.Drawing.Point(156, 8)
        Me.Gel_Maximum_Number.Maximum = New Decimal(New Integer() {200000, 0, 0, 0})
        Me.Gel_Maximum_Number.Minimum = New Decimal(New Integer() {50, 0, 0, 0})
        Me.Gel_Maximum_Number.Name = "Gel_Maximum_Number"
        Me.Gel_Maximum_Number.Size = New System.Drawing.Size(72, 20)
        Me.Gel_Maximum_Number.TabIndex = 1
        Me.ttDescription.SetToolTip(Me.Gel_Maximum_Number, "Set the maximum length.")
        Me.Gel_Maximum_Number.Value = New Decimal(New Integer() {1500, 0, 0, 0})
        '
        'Gel_Minimum_Number
        '
        Me.Gel_Minimum_Number.Increment = New Decimal(New Integer() {50, 0, 0, 0})
        Me.Gel_Minimum_Number.Location = New System.Drawing.Point(38, 8)
        Me.Gel_Minimum_Number.Maximum = New Decimal(New Integer() {200000, 0, 0, 0})
        Me.Gel_Minimum_Number.Minimum = New Decimal(New Integer() {50, 0, 0, 0})
        Me.Gel_Minimum_Number.Name = "Gel_Minimum_Number"
        Me.Gel_Minimum_Number.Size = New System.Drawing.Size(72, 20)
        Me.Gel_Minimum_Number.TabIndex = 1
        Me.ttDescription.SetToolTip(Me.Gel_Minimum_Number, "Set the minimum length.")
        Me.Gel_Minimum_Number.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(131, 10)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(18, 14)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "To"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(3, 10)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(31, 14)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "From"
        '
        'tpLigation
        '
        Me.tpLigation.Controls.Add(Me.ihLigation)
        Me.tpLigation.Location = New System.Drawing.Point(4, 23)
        Me.tpLigation.Name = "tpLigation"
        Me.tpLigation.Size = New System.Drawing.Size(497, 183)
        Me.tpLigation.TabIndex = 7
        Me.tpLigation.Text = "Ligation"
        Me.tpLigation.UseVisualStyleBackColor = True
        '
        'ihLigation
        '
        Me.ihLigation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ihLigation.Location = New System.Drawing.Point(0, 0)
        Me.ihLigation.Name = "ihLigation"
        Me.ihLigation.Size = New System.Drawing.Size(497, 183)
        Me.ihLigation.TabIndex = 1
        Me.ihLigation.Text = "Ligation"
        Me.ihLigation.Child = Me.ucWpfLigationPanel
        '
        'tpScreen
        '
        Me.tpScreen.Controls.Add(Me.btnScreenReset)
        Me.tpScreen.Controls.Add(Me.cbScreenCircular)
        Me.tpScreen.Controls.Add(Me.Screen_PCR_Panel)
        Me.tpScreen.Controls.Add(Me.Screen_PCR)
        Me.tpScreen.Controls.Add(Me.Screen_Freatures)
        Me.tpScreen.Controls.Add(Me.Screen_Features_LinkLabel)
        Me.tpScreen.Controls.Add(Me.pnlFeature)
        Me.tpScreen.Location = New System.Drawing.Point(4, 22)
        Me.tpScreen.Name = "tpScreen"
        Me.tpScreen.Size = New System.Drawing.Size(497, 184)
        Me.tpScreen.TabIndex = 5
        Me.tpScreen.Text = "Screen"
        Me.tpScreen.UseVisualStyleBackColor = True
        '
        'btnScreenReset
        '
        Me.btnScreenReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnScreenReset.Location = New System.Drawing.Point(328, 1)
        Me.btnScreenReset.Margin = New System.Windows.Forms.Padding(0)
        Me.btnScreenReset.Name = "btnScreenReset"
        Me.btnScreenReset.Size = New System.Drawing.Size(49, 22)
        Me.btnScreenReset.TabIndex = 11
        Me.btnScreenReset.Text = "Reset"
        Me.btnScreenReset.UseVisualStyleBackColor = True
        '
        'cbScreenCircular
        '
        Me.cbScreenCircular.AutoSize = True
        Me.cbScreenCircular.Location = New System.Drawing.Point(392, 4)
        Me.cbScreenCircular.Name = "cbScreenCircular"
        Me.cbScreenCircular.Size = New System.Drawing.Size(88, 18)
        Me.cbScreenCircular.TabIndex = 9
        Me.cbScreenCircular.Text = "Only Circular"
        Me.ttDescription.SetToolTip(Me.cbScreenCircular, "Tick to ignore all linear DNAs." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "If you want screen a circular plasmid from many " &
        "ligation products, tick this.")
        Me.cbScreenCircular.UseVisualStyleBackColor = True
        '
        'Screen_PCR_Panel
        '
        Me.Screen_PCR_Panel.Controls.Add(Me.pnlScreenPCR)
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_nudMax)
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_nudMin)
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_lblMax)
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_lblMin)
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_RCF)
        Me.Screen_PCR_Panel.Controls.Add(Me.tbSCRRP)
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_R)
        Me.Screen_PCR_Panel.Controls.Add(Me.tbSCRFP)
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_F)
        Me.Screen_PCR_Panel.Location = New System.Drawing.Point(3, 26)
        Me.Screen_PCR_Panel.Name = "Screen_PCR_Panel"
        Me.Screen_PCR_Panel.Size = New System.Drawing.Size(491, 156)
        Me.Screen_PCR_Panel.TabIndex = 8
        Me.ttDescription.SetToolTip(Me.Screen_PCR_Panel, "Reverse Primer Sequence" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please use > separate binding part and attached part.")
        Me.Screen_PCR_Panel.Visible = False
        '
        'pnlScreenPCR
        '
        Me.pnlScreenPCR.Location = New System.Drawing.Point(0, 55)
        Me.pnlScreenPCR.Name = "pnlScreenPCR"
        Me.pnlScreenPCR.Size = New System.Drawing.Size(491, 100)
        Me.pnlScreenPCR.TabIndex = 0
        '
        'Screen_PCR_nudMax
        '
        Me.Screen_PCR_nudMax.Increment = New Decimal(New Integer() {50, 0, 0, 0})
        Me.Screen_PCR_nudMax.Location = New System.Drawing.Point(427, 5)
        Me.Screen_PCR_nudMax.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.Screen_PCR_nudMax.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.Screen_PCR_nudMax.Name = "Screen_PCR_nudMax"
        Me.Screen_PCR_nudMax.Size = New System.Drawing.Size(61, 20)
        Me.Screen_PCR_nudMax.TabIndex = 21
        Me.ttDescription.SetToolTip(Me.Screen_PCR_nudMax, "The maximum length that a valid PCR product can have." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Use mouse wheel, click the" &
        " arrow or type numbers to change value.")
        Me.Screen_PCR_nudMax.Value = New Decimal(New Integer() {800, 0, 0, 0})
        '
        'Screen_PCR_nudMin
        '
        Me.Screen_PCR_nudMin.Increment = New Decimal(New Integer() {50, 0, 0, 0})
        Me.Screen_PCR_nudMin.Location = New System.Drawing.Point(427, 32)
        Me.Screen_PCR_nudMin.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.Screen_PCR_nudMin.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.Screen_PCR_nudMin.Name = "Screen_PCR_nudMin"
        Me.Screen_PCR_nudMin.Size = New System.Drawing.Size(61, 20)
        Me.Screen_PCR_nudMin.TabIndex = 22
        Me.ttDescription.SetToolTip(Me.Screen_PCR_nudMin, "The minimum length that a valid PCR product can have." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Use mouse wheel, click the" &
        " arrow or type numbers to change value.")
        Me.Screen_PCR_nudMin.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'Screen_PCR_lblMax
        '
        Me.Screen_PCR_lblMax.AutoSize = True
        Me.Screen_PCR_lblMax.Location = New System.Drawing.Point(374, 7)
        Me.Screen_PCR_lblMax.Name = "Screen_PCR_lblMax"
        Me.Screen_PCR_lblMax.Size = New System.Drawing.Size(51, 14)
        Me.Screen_PCR_lblMax.TabIndex = 19
        Me.Screen_PCR_lblMax.Text = "Maximum"
        '
        'Screen_PCR_lblMin
        '
        Me.Screen_PCR_lblMin.AutoSize = True
        Me.Screen_PCR_lblMin.Location = New System.Drawing.Point(374, 37)
        Me.Screen_PCR_lblMin.Name = "Screen_PCR_lblMin"
        Me.Screen_PCR_lblMin.Size = New System.Drawing.Size(47, 14)
        Me.Screen_PCR_lblMin.TabIndex = 20
        Me.Screen_PCR_lblMin.Text = "Minimum"
        '
        'Screen_PCR_RCF
        '
        Me.Screen_PCR_RCF.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Screen_PCR_RCF.Location = New System.Drawing.Point(334, 7)
        Me.Screen_PCR_RCF.Name = "Screen_PCR_RCF"
        Me.Screen_PCR_RCF.Size = New System.Drawing.Size(34, 42)
        Me.Screen_PCR_RCF.TabIndex = 18
        Me.Screen_PCR_RCF.Text = "PR"
        Me.ttDescription.SetToolTip(Me.Screen_PCR_RCF, "Click to open Primer Design Tab.")
        Me.Screen_PCR_RCF.UseVisualStyleBackColor = True
        '
        'tbSCRRP
        '
        Me.tbSCRRP.Location = New System.Drawing.Point(3, 31)
        Me.tbSCRRP.Name = "tbSCRRP"
        Me.tbSCRRP.Size = New System.Drawing.Size(68, 22)
        Me.tbSCRRP.TabIndex = 16
        Me.ttDescription.SetToolTip(Me.tbSCRRP, resources.GetString("tbSCRRP.ToolTip"))
        '
        'Screen_PCR_R
        '
        Me.Screen_PCR_R.Location = New System.Drawing.Point(73, 31)
        Me.Screen_PCR_R.Name = "Screen_PCR_R"
        Me.Screen_PCR_R.Size = New System.Drawing.Size(255, 20)
        Me.Screen_PCR_R.TabIndex = 16
        '
        'tbSCRFP
        '
        Me.tbSCRFP.Location = New System.Drawing.Point(3, 4)
        Me.tbSCRFP.Name = "tbSCRFP"
        Me.tbSCRFP.Size = New System.Drawing.Size(68, 22)
        Me.tbSCRFP.TabIndex = 15
        Me.ttDescription.SetToolTip(Me.tbSCRFP, resources.GetString("tbSCRFP.ToolTip"))
        '
        'Screen_PCR_F
        '
        Me.Screen_PCR_F.Location = New System.Drawing.Point(73, 4)
        Me.Screen_PCR_F.Name = "Screen_PCR_F"
        Me.Screen_PCR_F.Size = New System.Drawing.Size(255, 20)
        Me.Screen_PCR_F.TabIndex = 15
        Me.ttDescription.SetToolTip(Me.Screen_PCR_F, "Forward Primer Sequence" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please use > separate binding part and attached part.")
        '
        'Screen_PCR
        '
        Me.Screen_PCR.AutoSize = True
        Me.Screen_PCR.Location = New System.Drawing.Point(107, 4)
        Me.Screen_PCR.Name = "Screen_PCR"
        Me.Screen_PCR.Size = New System.Drawing.Size(45, 18)
        Me.Screen_PCR.TabIndex = 7
        Me.Screen_PCR.Text = "PCR"
        Me.ttDescription.SetToolTip(Me.Screen_PCR, resources.GetString("Screen_PCR.ToolTip"))
        Me.Screen_PCR.UseVisualStyleBackColor = True
        '
        'Screen_Freatures
        '
        Me.Screen_Freatures.AutoSize = True
        Me.Screen_Freatures.Checked = True
        Me.Screen_Freatures.Location = New System.Drawing.Point(6, 3)
        Me.Screen_Freatures.Name = "Screen_Freatures"
        Me.Screen_Freatures.Size = New System.Drawing.Size(68, 18)
        Me.Screen_Freatures.TabIndex = 7
        Me.Screen_Freatures.TabStop = True
        Me.Screen_Freatures.Text = "Features"
        Me.Screen_Freatures.UseVisualStyleBackColor = True
        '
        'Screen_Features_LinkLabel
        '
        Me.Screen_Features_LinkLabel.AutoSize = True
        Me.Screen_Features_LinkLabel.ForeColor = System.Drawing.Color.Magenta
        Me.Screen_Features_LinkLabel.LinkColor = System.Drawing.Color.DarkOliveGreen
        Me.Screen_Features_LinkLabel.Location = New System.Drawing.Point(4, 30)
        Me.Screen_Features_LinkLabel.Name = "Screen_Features_LinkLabel"
        Me.Screen_Features_LinkLabel.Size = New System.Drawing.Size(79, 14)
        Me.Screen_Features_LinkLabel.TabIndex = 6
        Me.Screen_Features_LinkLabel.TabStop = True
        Me.Screen_Features_LinkLabel.Text = "[Click to select]"
        '
        'pnlFeature
        '
        Me.pnlFeature.AutoScroll = True
        Me.pnlFeature.Location = New System.Drawing.Point(3, 47)
        Me.pnlFeature.Name = "pnlFeature"
        Me.pnlFeature.Size = New System.Drawing.Size(491, 135)
        Me.pnlFeature.TabIndex = 10
        Me.ttDescription.SetToolTip(Me.pnlFeature, "Click to Choose the Feature." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "One the DNA with selected freatures will be screene" &
        "d out." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "For example, you can Use Amp feature to screen all vector against the am" &
        "picillin.")
        '
        'tpRec
        '
        Me.tpRec.Controls.Add(Me.ihRecombination)
        Me.tpRec.Location = New System.Drawing.Point(4, 23)
        Me.tpRec.Name = "tpRec"
        Me.tpRec.Size = New System.Drawing.Size(497, 183)
        Me.tpRec.TabIndex = 8
        Me.tpRec.Text = "Recombination"
        Me.tpRec.UseVisualStyleBackColor = True
        '
        'ihRecombination
        '
        Me.ihRecombination.BackColor = System.Drawing.Color.White
        Me.ihRecombination.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ihRecombination.Location = New System.Drawing.Point(0, 0)
        Me.ihRecombination.Name = "ihRecombination"
        Me.ihRecombination.Size = New System.Drawing.Size(497, 183)
        Me.ihRecombination.TabIndex = 2
        Me.ihRecombination.Text = "InteropHost1"
        Me.ihRecombination.Child = Me.ucWpfRecombinationPanel
        '
        'tpEnzymeAnalysis
        '
        Me.tpEnzymeAnalysis.Controls.Add(Me.llAnalysisRemove)
        Me.tpEnzymeAnalysis.Controls.Add(Me.llAnalysisAdd)
        Me.tpEnzymeAnalysis.Controls.Add(Me.dgvEnzymeAnalysis)
        Me.tpEnzymeAnalysis.Location = New System.Drawing.Point(4, 22)
        Me.tpEnzymeAnalysis.Name = "tpEnzymeAnalysis"
        Me.tpEnzymeAnalysis.Size = New System.Drawing.Size(497, 184)
        Me.tpEnzymeAnalysis.TabIndex = 9
        Me.tpEnzymeAnalysis.Text = "Enzyme Analysis"
        Me.tpEnzymeAnalysis.UseVisualStyleBackColor = True
        '
        'llAnalysisRemove
        '
        Me.llAnalysisRemove.AutoSize = True
        Me.llAnalysisRemove.Location = New System.Drawing.Point(397, 0)
        Me.llAnalysisRemove.Name = "llAnalysisRemove"
        Me.llAnalysisRemove.Size = New System.Drawing.Size(98, 14)
        Me.llAnalysisRemove.TabIndex = 1
        Me.llAnalysisRemove.TabStop = True
        Me.llAnalysisRemove.Text = "Remove Sequence"
        '
        'llAnalysisAdd
        '
        Me.llAnalysisAdd.AutoSize = True
        Me.llAnalysisAdd.Location = New System.Drawing.Point(296, 0)
        Me.llAnalysisAdd.Name = "llAnalysisAdd"
        Me.llAnalysisAdd.Size = New System.Drawing.Size(79, 14)
        Me.llAnalysisAdd.TabIndex = 1
        Me.llAnalysisAdd.TabStop = True
        Me.llAnalysisAdd.Text = "Add Sequence"
        '
        'dgvEnzymeAnalysis
        '
        Me.dgvEnzymeAnalysis.AllowUserToAddRows = False
        Me.dgvEnzymeAnalysis.AllowUserToDeleteRows = False
        Me.dgvEnzymeAnalysis.AllowUserToOrderColumns = True
        Me.dgvEnzymeAnalysis.AllowUserToResizeRows = False
        Me.dgvEnzymeAnalysis.BackgroundColor = System.Drawing.Color.White
        Me.dgvEnzymeAnalysis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEnzymeAnalysis.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column5, Me.Column2, Me.Column3, Me.Column4})
        Me.dgvEnzymeAnalysis.ContextMenuStrip = Me.cmsEnzyme
        Me.dgvEnzymeAnalysis.GridColor = System.Drawing.Color.DarkViolet
        Me.dgvEnzymeAnalysis.Location = New System.Drawing.Point(0, 14)
        Me.dgvEnzymeAnalysis.Name = "dgvEnzymeAnalysis"
        Me.dgvEnzymeAnalysis.RowHeadersVisible = False
        Me.dgvEnzymeAnalysis.RowTemplate.Height = 23
        Me.dgvEnzymeAnalysis.Size = New System.Drawing.Size(497, 170)
        Me.dgvEnzymeAnalysis.TabIndex = 0
        Me.ttDescription.SetToolTip(Me.dgvEnzymeAnalysis, "Click Add Sequence to Choose the Sequences and set conditions.")
        '
        'Column1
        '
        Me.Column1.HeaderText = "Name"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column1.Width = 130
        '
        'Column5
        '
        Me.Column5.HeaderText = "Region"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column2
        '
        Me.Column2.HeaderText = "Use"
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 40
        '
        'Column3
        '
        Me.Column3.HeaderText = "Method"
        Me.Column3.Items.AddRange(New Object() {"=", ">", "<"})
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 60
        '
        'Column4
        '
        Me.Column4.HeaderText = "Value"
        Me.Column4.Name = "Column4"
        '
        'cmsEnzyme
        '
        Me.cmsEnzyme.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddToolStripMenuItem, Me.RemoveToolStripMenuItem})
        Me.cmsEnzyme.Name = "cmsEnzyme"
        Me.cmsEnzyme.Size = New System.Drawing.Size(184, 48)
        '
        'AddToolStripMenuItem
        '
        Me.AddToolStripMenuItem.Name = "AddToolStripMenuItem"
        Me.AddToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.AddToolStripMenuItem.Text = "Add Sequence"
        '
        'RemoveToolStripMenuItem
        '
        Me.RemoveToolStripMenuItem.Name = "RemoveToolStripMenuItem"
        Me.RemoveToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.RemoveToolStripMenuItem.Text = "Remove Sequence"
        '
        'tpSequenceMerge
        '
        Me.tpSequenceMerge.Controls.Add(Me.cbMergeExtend)
        Me.tpSequenceMerge.Controls.Add(Me.cbMergeSignificant)
        Me.tpSequenceMerge.Location = New System.Drawing.Point(4, 22)
        Me.tpSequenceMerge.Name = "tpSequenceMerge"
        Me.tpSequenceMerge.Size = New System.Drawing.Size(497, 184)
        Me.tpSequenceMerge.TabIndex = 10
        Me.tpSequenceMerge.Text = "Merge"
        Me.tpSequenceMerge.UseVisualStyleBackColor = True
        '
        'cbMergeExtend
        '
        Me.cbMergeExtend.AutoSize = True
        Me.cbMergeExtend.Location = New System.Drawing.Point(314, 16)
        Me.cbMergeExtend.Name = "cbMergeExtend"
        Me.cbMergeExtend.Size = New System.Drawing.Size(120, 18)
        Me.cbMergeExtend.TabIndex = 0
        Me.cbMergeExtend.Text = "Only Extend Length"
        Me.ttDescription.SetToolTip(Me.cbMergeExtend, "Only generate longer sequences then the input.")
        Me.cbMergeExtend.UseVisualStyleBackColor = True
        '
        'cbMergeSignificant
        '
        Me.cbMergeSignificant.AutoSize = True
        Me.cbMergeSignificant.Location = New System.Drawing.Point(13, 16)
        Me.cbMergeSignificant.Name = "cbMergeSignificant"
        Me.cbMergeSignificant.Size = New System.Drawing.Size(134, 18)
        Me.cbMergeSignificant.TabIndex = 0
        Me.cbMergeSignificant.Text = "Only Significant Merge"
        Me.ttDescription.SetToolTip(Me.cbMergeSignificant, "The shortest length of match shall be at least 60% of the length of the longest m" &
        "atch." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This will reduces the number of mergeings caused by undesired small match" &
        "es (such as short repeats in DNA).")
        Me.cbMergeSignificant.UseVisualStyleBackColor = True
        '
        'tbFreeDesign
        '
        Me.tbFreeDesign.Controls.Add(Me.cbDesign_UseDesigner)
        Me.tbFreeDesign.Controls.Add(Me.tbDesign)
        Me.tbFreeDesign.Controls.Add(Me.rtbFreeDesign)
        Me.tbFreeDesign.Location = New System.Drawing.Point(4, 22)
        Me.tbFreeDesign.Name = "tbFreeDesign"
        Me.tbFreeDesign.Size = New System.Drawing.Size(497, 184)
        Me.tbFreeDesign.TabIndex = 12
        Me.tbFreeDesign.Text = "Free Design"
        Me.tbFreeDesign.UseVisualStyleBackColor = True
        '
        'cbDesign_UseDesigner
        '
        Me.cbDesign_UseDesigner.AutoSize = True
        Me.cbDesign_UseDesigner.Location = New System.Drawing.Point(419, 2)
        Me.cbDesign_UseDesigner.Name = "cbDesign_UseDesigner"
        Me.cbDesign_UseDesigner.Size = New System.Drawing.Size(69, 18)
        Me.cbDesign_UseDesigner.TabIndex = 2
        Me.cbDesign_UseDesigner.Text = "Designer"
        Me.cbDesign_UseDesigner.UseVisualStyleBackColor = True
        '
        'tbDesign
        '
        Me.tbDesign.Location = New System.Drawing.Point(3, 2)
        Me.tbDesign.Name = "tbDesign"
        Me.tbDesign.Size = New System.Drawing.Size(413, 20)
        Me.tbDesign.TabIndex = 1
        Me.ttDescription.SetToolTip(Me.tbDesign, "The Name of Free Designed DNA.")
        '
        'rtbFreeDesign
        '
        Me.rtbFreeDesign.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbFreeDesign.Font = New System.Drawing.Font("Lucida Console", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbFreeDesign.Location = New System.Drawing.Point(3, 23)
        Me.rtbFreeDesign.Name = "rtbFreeDesign"
        Me.rtbFreeDesign.Size = New System.Drawing.Size(491, 158)
        Me.rtbFreeDesign.TabIndex = 0
        Me.rtbFreeDesign.Text = ""
        Me.ttDescription.SetToolTip(Me.rtbFreeDesign, resources.GetString("rtbFreeDesign.ToolTip"))
        '
        'tpHashPicker
        '
        Me.tpHashPicker.Controls.Add(Me.cpHashPickerListedDNA)
        Me.tpHashPicker.Controls.Add(Me.cpHashPickerChoosenDNA)
        Me.tpHashPicker.Controls.Add(Me.lblHashPickerList)
        Me.tpHashPicker.Controls.Add(Me.lblHashPickerChoosen)
        Me.tpHashPicker.Location = New System.Drawing.Point(4, 22)
        Me.tpHashPicker.Name = "tpHashPicker"
        Me.tpHashPicker.Size = New System.Drawing.Size(497, 184)
        Me.tpHashPicker.TabIndex = 11
        Me.tpHashPicker.Text = "HashPicker"
        Me.tpHashPicker.UseVisualStyleBackColor = True
        '
        'cpHashPickerListedDNA
        '
        Me.cpHashPickerListedDNA.Choices = CType(resources.GetObject("cpHashPickerListedDNA.Choices"), System.Collections.Generic.Dictionary(Of Object, String))
        Me.cpHashPickerListedDNA.Location = New System.Drawing.Point(1, 71)
        Me.cpHashPickerListedDNA.Name = "cpHashPickerListedDNA"
        Me.cpHashPickerListedDNA.Size = New System.Drawing.Size(496, 113)
        Me.cpHashPickerListedDNA.TabIndex = 2
        '
        'cpHashPickerChoosenDNA
        '
        Me.cpHashPickerChoosenDNA.Choices = CType(resources.GetObject("cpHashPickerChoosenDNA.Choices"), System.Collections.Generic.Dictionary(Of Object, String))
        Me.cpHashPickerChoosenDNA.Location = New System.Drawing.Point(1, 18)
        Me.cpHashPickerChoosenDNA.Name = "cpHashPickerChoosenDNA"
        Me.cpHashPickerChoosenDNA.Size = New System.Drawing.Size(496, 35)
        Me.cpHashPickerChoosenDNA.TabIndex = 2
        '
        'lblHashPickerList
        '
        Me.lblHashPickerList.AutoSize = True
        Me.lblHashPickerList.Location = New System.Drawing.Point(3, 56)
        Me.lblHashPickerList.Name = "lblHashPickerList"
        Me.lblHashPickerList.Size = New System.Drawing.Size(78, 14)
        Me.lblHashPickerList.TabIndex = 0
        Me.lblHashPickerList.Text = "Listed DNA(s):"
        '
        'lblHashPickerChoosen
        '
        Me.lblHashPickerChoosen.AutoSize = True
        Me.lblHashPickerChoosen.Location = New System.Drawing.Point(3, 4)
        Me.lblHashPickerChoosen.Name = "lblHashPickerChoosen"
        Me.lblHashPickerChoosen.Size = New System.Drawing.Size(92, 14)
        Me.lblHashPickerChoosen.TabIndex = 0
        Me.lblHashPickerChoosen.Text = "Choosen DNA(s):"
        '
        'tpSequencingResult
        '
        Me.tpSequencingResult.Controls.Add(Me.cbSequencingResultOption)
        Me.tpSequencingResult.Controls.Add(Me.lbl_Sequencing_NameTag)
        Me.tpSequencingResult.Controls.Add(Me.rtbSequencingResult)
        Me.tpSequencingResult.Controls.Add(Me.tbSequencingPrimerName)
        Me.tpSequencingResult.Controls.Add(Me.tbSequencingPrimerSequence)
        Me.tpSequencingResult.Location = New System.Drawing.Point(4, 22)
        Me.tpSequencingResult.Name = "tpSequencingResult"
        Me.tpSequencingResult.Size = New System.Drawing.Size(497, 184)
        Me.tpSequencingResult.TabIndex = 14
        Me.tpSequencingResult.Text = "Sequencing"
        Me.tpSequencingResult.UseVisualStyleBackColor = True
        '
        'cbSequencingResultOption
        '
        Me.cbSequencingResultOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSequencingResultOption.FormattingEnabled = True
        Me.cbSequencingResultOption.Items.AddRange(New Object() {"Unchecked", "Correct", "PointMutation", "FragmentInsertion", "FragmentLoss", "NoneMatch"})
        Me.cbSequencingResultOption.Location = New System.Drawing.Point(389, 3)
        Me.cbSequencingResultOption.Name = "cbSequencingResultOption"
        Me.cbSequencingResultOption.Size = New System.Drawing.Size(105, 22)
        Me.cbSequencingResultOption.TabIndex = 20
        '
        'lbl_Sequencing_NameTag
        '
        Me.lbl_Sequencing_NameTag.AutoSize = True
        Me.lbl_Sequencing_NameTag.Location = New System.Drawing.Point(3, 6)
        Me.lbl_Sequencing_NameTag.Name = "lbl_Sequencing_NameTag"
        Me.lbl_Sequencing_NameTag.Size = New System.Drawing.Size(37, 14)
        Me.lbl_Sequencing_NameTag.TabIndex = 19
        Me.lbl_Sequencing_NameTag.Text = "Primer"
        '
        'rtbSequencingResult
        '
        Me.rtbSequencingResult.BackColor = System.Drawing.Color.Ivory
        Me.rtbSequencingResult.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbSequencingResult.Font = New System.Drawing.Font("Lucida Console", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbSequencingResult.Location = New System.Drawing.Point(4, 30)
        Me.rtbSequencingResult.Name = "rtbSequencingResult"
        Me.rtbSequencingResult.Size = New System.Drawing.Size(490, 151)
        Me.rtbSequencingResult.TabIndex = 18
        Me.rtbSequencingResult.Text = ""
        '
        'tbSequencingPrimerName
        '
        Me.tbSequencingPrimerName.Location = New System.Drawing.Point(47, 3)
        Me.tbSequencingPrimerName.Name = "tbSequencingPrimerName"
        Me.tbSequencingPrimerName.Size = New System.Drawing.Size(66, 22)
        Me.tbSequencingPrimerName.TabIndex = 17
        '
        'tbSequencingPrimerSequence
        '
        Me.tbSequencingPrimerSequence.Location = New System.Drawing.Point(117, 3)
        Me.tbSequencingPrimerSequence.Name = "tbSequencingPrimerSequence"
        Me.tbSequencingPrimerSequence.Size = New System.Drawing.Size(268, 20)
        Me.tbSequencingPrimerSequence.TabIndex = 16
        '
        'tpComparer
        '
        Me.tpComparer.Controls.Add(Me.cbCompareResult)
        Me.tpComparer.Controls.Add(Me.Label6)
        Me.tpComparer.Controls.Add(Me.Label7)
        Me.tpComparer.Controls.Add(Me.cpCompareList)
        Me.tpComparer.Controls.Add(Me.cpCompareChoice)
        Me.tpComparer.Location = New System.Drawing.Point(4, 22)
        Me.tpComparer.Name = "tpComparer"
        Me.tpComparer.Size = New System.Drawing.Size(497, 184)
        Me.tpComparer.TabIndex = 13
        Me.tpComparer.Text = "Comparer"
        Me.tpComparer.UseVisualStyleBackColor = True
        '
        'cbCompareResult
        '
        Me.cbCompareResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCompareResult.FormattingEnabled = True
        Me.cbCompareResult.Items.AddRange(New Object() {"Unchecked", "Correct", "PointMutation", "FragmentInsertion", "FragmentLoss", "NoneMatch"})
        Me.cbCompareResult.Location = New System.Drawing.Point(389, 4)
        Me.cbCompareResult.Name = "cbCompareResult"
        Me.cbCompareResult.Size = New System.Drawing.Size(105, 22)
        Me.cbCompareResult.TabIndex = 21
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(2, 67)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(78, 14)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Listed DNA(s):"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(2, 7)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(92, 14)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Choosen DNA(s):"
        '
        'cpCompareList
        '
        Me.cpCompareList.Choices = CType(resources.GetObject("cpCompareList.Choices"), System.Collections.Generic.Dictionary(Of Object, String))
        Me.cpCompareList.Location = New System.Drawing.Point(0, 82)
        Me.cpCompareList.Name = "cpCompareList"
        Me.cpCompareList.Size = New System.Drawing.Size(496, 100)
        Me.cpCompareList.TabIndex = 5
        '
        'cpCompareChoice
        '
        Me.cpCompareChoice.Choices = CType(resources.GetObject("cpCompareChoice.Choices"), System.Collections.Generic.Dictionary(Of Object, String))
        Me.cpCompareChoice.Location = New System.Drawing.Point(0, 29)
        Me.cpCompareChoice.Name = "cpCompareChoice"
        Me.cpCompareChoice.Size = New System.Drawing.Size(496, 35)
        Me.cpCompareChoice.TabIndex = 6
        '
        'tpHost
        '
        Me.tpHost.Controls.Add(Me.tbChromosomeFragmentName)
        Me.tpHost.Controls.Add(Me.tbHostFunction)
        Me.tpHost.Controls.Add(Me.tbHostName)
        Me.tpHost.Controls.Add(Me.lbFragmentName)
        Me.tpHost.Controls.Add(Me.llHostFunctions)
        Me.tpHost.Controls.Add(Me.lbHostName)
        Me.tpHost.Controls.Add(Me.rtbChromosomoFragment)
        Me.tpHost.Controls.Add(Me.llRemoveChromosomeFragment)
        Me.tpHost.Controls.Add(Me.llAddChromosomeFragment)
        Me.tpHost.Location = New System.Drawing.Point(4, 22)
        Me.tpHost.Name = "tpHost"
        Me.tpHost.Size = New System.Drawing.Size(497, 184)
        Me.tpHost.TabIndex = 15
        Me.tpHost.Text = "Host"
        Me.tpHost.UseVisualStyleBackColor = True
        '
        'tbChromosomeFragmentName
        '
        Me.tbChromosomeFragmentName.Location = New System.Drawing.Point(107, 22)
        Me.tbChromosomeFragmentName.Name = "tbChromosomeFragmentName"
        Me.tbChromosomeFragmentName.Size = New System.Drawing.Size(210, 20)
        Me.tbChromosomeFragmentName.TabIndex = 4
        '
        'tbHostFunction
        '
        Me.tbHostFunction.Location = New System.Drawing.Point(291, 1)
        Me.tbHostFunction.Name = "tbHostFunction"
        Me.tbHostFunction.Size = New System.Drawing.Size(203, 20)
        Me.tbHostFunction.TabIndex = 4
        '
        'tbHostName
        '
        Me.tbHostName.Location = New System.Drawing.Point(47, 1)
        Me.tbHostName.Name = "tbHostName"
        Me.tbHostName.Size = New System.Drawing.Size(161, 20)
        Me.tbHostName.TabIndex = 4
        '
        'lbFragmentName
        '
        Me.lbFragmentName.AutoSize = True
        Me.lbFragmentName.Location = New System.Drawing.Point(3, 25)
        Me.lbFragmentName.Name = "lbFragmentName"
        Me.lbFragmentName.Size = New System.Drawing.Size(82, 14)
        Me.lbFragmentName.TabIndex = 3
        Me.lbFragmentName.Text = "Fragment Name"
        '
        'llHostFunctions
        '
        Me.llHostFunctions.AutoSize = True
        Me.llHostFunctions.Location = New System.Drawing.Point(214, 4)
        Me.llHostFunctions.Name = "llHostFunctions"
        Me.llHostFunctions.Size = New System.Drawing.Size(54, 14)
        Me.llHostFunctions.TabIndex = 3
        Me.llHostFunctions.Text = "Functions"
        '
        'lbHostName
        '
        Me.lbHostName.AutoSize = True
        Me.lbHostName.Location = New System.Drawing.Point(3, 4)
        Me.lbHostName.Name = "lbHostName"
        Me.lbHostName.Size = New System.Drawing.Size(34, 14)
        Me.lbHostName.TabIndex = 3
        Me.lbHostName.Text = "Name"
        '
        'rtbChromosomoFragment
        '
        Me.rtbChromosomoFragment.Location = New System.Drawing.Point(3, 49)
        Me.rtbChromosomoFragment.Name = "rtbChromosomoFragment"
        Me.rtbChromosomoFragment.Size = New System.Drawing.Size(491, 132)
        Me.rtbChromosomoFragment.TabIndex = 1
        Me.rtbChromosomoFragment.Text = ""
        '
        'llRemoveChromosomeFragment
        '
        Me.llRemoveChromosomeFragment.AutoSize = True
        Me.llRemoveChromosomeFragment.Location = New System.Drawing.Point(453, 25)
        Me.llRemoveChromosomeFragment.Name = "llRemoveChromosomeFragment"
        Me.llRemoveChromosomeFragment.Size = New System.Drawing.Size(46, 14)
        Me.llRemoveChromosomeFragment.TabIndex = 0
        Me.llRemoveChromosomeFragment.TabStop = True
        Me.llRemoveChromosomeFragment.Text = "Remove"
        '
        'llAddChromosomeFragment
        '
        Me.llAddChromosomeFragment.AutoSize = True
        Me.llAddChromosomeFragment.Location = New System.Drawing.Point(323, 25)
        Me.llAddChromosomeFragment.Name = "llAddChromosomeFragment"
        Me.llAddChromosomeFragment.Size = New System.Drawing.Size(75, 14)
        Me.llAddChromosomeFragment.TabIndex = 0
        Me.llAddChromosomeFragment.TabStop = True
        Me.llAddChromosomeFragment.Text = "Add Fragment"
        '
        'tpTransformation
        '
        Me.tpTransformation.Controls.Add(Me.gbTransformationMode)
        Me.tpTransformation.Controls.Add(Me.gbTransfromation)
        Me.tpTransformation.Location = New System.Drawing.Point(4, 22)
        Me.tpTransformation.Name = "tpTransformation"
        Me.tpTransformation.Size = New System.Drawing.Size(497, 184)
        Me.tpTransformation.TabIndex = 16
        Me.tpTransformation.Text = "Transformation"
        Me.tpTransformation.UseVisualStyleBackColor = True
        '
        'gbTransformationMode
        '
        Me.gbTransformationMode.Controls.Add(Me.rbTransformationCBNT)
        Me.gbTransformationMode.Controls.Add(Me.rbTransformationEDPC)
        Me.gbTransformationMode.Controls.Add(Me.rbTransformationAIOC)
        Me.gbTransformationMode.Location = New System.Drawing.Point(262, 13)
        Me.gbTransformationMode.Name = "gbTransformationMode"
        Me.gbTransformationMode.Size = New System.Drawing.Size(200, 100)
        Me.gbTransformationMode.TabIndex = 3
        Me.gbTransformationMode.TabStop = False
        Me.gbTransformationMode.Text = "Mode"
        '
        'rbTransformationCBNT
        '
        Me.rbTransformationCBNT.AutoSize = True
        Me.rbTransformationCBNT.Location = New System.Drawing.Point(6, 64)
        Me.rbTransformationCBNT.Name = "rbTransformationCBNT"
        Me.rbTransformationCBNT.Size = New System.Drawing.Size(91, 18)
        Me.rbTransformationCBNT.TabIndex = 2
        Me.rbTransformationCBNT.TabStop = True
        Me.rbTransformationCBNT.Text = "Combinational"
        Me.rbTransformationCBNT.UseVisualStyleBackColor = True
        '
        'rbTransformationEDPC
        '
        Me.rbTransformationEDPC.AutoSize = True
        Me.rbTransformationEDPC.Location = New System.Drawing.Point(6, 42)
        Me.rbTransformationEDPC.Name = "rbTransformationEDPC"
        Me.rbTransformationEDPC.Size = New System.Drawing.Size(112, 18)
        Me.rbTransformationEDPC.TabIndex = 1
        Me.rbTransformationEDPC.TabStop = True
        Me.rbTransformationEDPC.Text = "Each DNA per Cell"
        Me.rbTransformationEDPC.UseVisualStyleBackColor = True
        '
        'rbTransformationAIOC
        '
        Me.rbTransformationAIOC.AutoSize = True
        Me.rbTransformationAIOC.Location = New System.Drawing.Point(6, 20)
        Me.rbTransformationAIOC.Name = "rbTransformationAIOC"
        Me.rbTransformationAIOC.Size = New System.Drawing.Size(100, 18)
        Me.rbTransformationAIOC.TabIndex = 0
        Me.rbTransformationAIOC.TabStop = True
        Me.rbTransformationAIOC.Text = "All into One Cell"
        Me.rbTransformationAIOC.UseVisualStyleBackColor = True
        '
        'gbTransfromation
        '
        Me.gbTransfromation.Controls.Add(Me.rbTransformationChemical)
        Me.gbTransfromation.Controls.Add(Me.rbTransformationConjugation)
        Me.gbTransfromation.Controls.Add(Me.rbTransformationElectroporation)
        Me.gbTransfromation.Location = New System.Drawing.Point(8, 13)
        Me.gbTransfromation.Name = "gbTransfromation"
        Me.gbTransfromation.Size = New System.Drawing.Size(200, 100)
        Me.gbTransfromation.TabIndex = 2
        Me.gbTransfromation.TabStop = False
        Me.gbTransfromation.Text = "Methods"
        '
        'rbTransformationChemical
        '
        Me.rbTransformationChemical.AutoSize = True
        Me.rbTransformationChemical.Location = New System.Drawing.Point(6, 20)
        Me.rbTransformationChemical.Name = "rbTransformationChemical"
        Me.rbTransformationChemical.Size = New System.Drawing.Size(150, 18)
        Me.rbTransformationChemical.TabIndex = 1
        Me.rbTransformationChemical.TabStop = True
        Me.rbTransformationChemical.Text = "Chemical Transoformation"
        Me.rbTransformationChemical.UseVisualStyleBackColor = True
        '
        'rbTransformationConjugation
        '
        Me.rbTransformationConjugation.AutoSize = True
        Me.rbTransformationConjugation.Location = New System.Drawing.Point(6, 64)
        Me.rbTransformationConjugation.Name = "rbTransformationConjugation"
        Me.rbTransformationConjugation.Size = New System.Drawing.Size(81, 18)
        Me.rbTransformationConjugation.TabIndex = 1
        Me.rbTransformationConjugation.TabStop = True
        Me.rbTransformationConjugation.Text = "Conjugation"
        Me.rbTransformationConjugation.UseVisualStyleBackColor = True
        '
        'rbTransformationElectroporation
        '
        Me.rbTransformationElectroporation.AutoSize = True
        Me.rbTransformationElectroporation.Location = New System.Drawing.Point(6, 42)
        Me.rbTransformationElectroporation.Name = "rbTransformationElectroporation"
        Me.rbTransformationElectroporation.Size = New System.Drawing.Size(97, 18)
        Me.rbTransformationElectroporation.TabIndex = 1
        Me.rbTransformationElectroporation.TabStop = True
        Me.rbTransformationElectroporation.Text = "Electroporation"
        Me.rbTransformationElectroporation.UseVisualStyleBackColor = True
        '
        'tpIncubation
        '
        Me.tpIncubation.Controls.Add(Me.llCommonStep)
        Me.tpIncubation.Controls.Add(Me.llRemoveIncubation)
        Me.tpIncubation.Controls.Add(Me.llAddIncubation)
        Me.tpIncubation.Controls.Add(Me.dgvIncubation)
        Me.tpIncubation.Location = New System.Drawing.Point(4, 22)
        Me.tpIncubation.Name = "tpIncubation"
        Me.tpIncubation.Size = New System.Drawing.Size(497, 184)
        Me.tpIncubation.TabIndex = 17
        Me.tpIncubation.Text = "Incubation"
        Me.tpIncubation.UseVisualStyleBackColor = True
        '
        'llCommonStep
        '
        Me.llCommonStep.AutoSize = True
        Me.llCommonStep.Location = New System.Drawing.Point(83, 0)
        Me.llCommonStep.Name = "llCommonStep"
        Me.llCommonStep.Size = New System.Drawing.Size(74, 14)
        Me.llCommonStep.TabIndex = 2
        Me.llCommonStep.TabStop = True
        Me.llCommonStep.Text = "Plate - Culture"
        '
        'llRemoveIncubation
        '
        Me.llRemoveIncubation.AutoSize = True
        Me.llRemoveIncubation.Location = New System.Drawing.Point(33, 0)
        Me.llRemoveIncubation.Name = "llRemoveIncubation"
        Me.llRemoveIncubation.Size = New System.Drawing.Size(46, 14)
        Me.llRemoveIncubation.TabIndex = 1
        Me.llRemoveIncubation.TabStop = True
        Me.llRemoveIncubation.Text = "Remove"
        '
        'llAddIncubation
        '
        Me.llAddIncubation.AutoSize = True
        Me.llAddIncubation.Location = New System.Drawing.Point(4, 0)
        Me.llAddIncubation.Name = "llAddIncubation"
        Me.llAddIncubation.Size = New System.Drawing.Size(27, 14)
        Me.llAddIncubation.TabIndex = 1
        Me.llAddIncubation.TabStop = True
        Me.llAddIncubation.Text = "Add"
        '
        'dgvIncubation
        '
        Me.dgvIncubation.AllowUserToAddRows = False
        Me.dgvIncubation.AllowUserToDeleteRows = False
        Me.dgvIncubation.AllowUserToOrderColumns = True
        Me.dgvIncubation.AllowUserToResizeRows = False
        Me.dgvIncubation.BackgroundColor = System.Drawing.Color.White
        Me.dgvIncubation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvIncubation.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column6, Me.Column7, Me.Column8, Me.Column9, Me.Column10, Me.Column11})
        Me.dgvIncubation.Location = New System.Drawing.Point(3, 15)
        Me.dgvIncubation.Name = "dgvIncubation"
        Me.dgvIncubation.RowHeadersVisible = False
        Me.dgvIncubation.RowTemplate.Height = 23
        Me.dgvIncubation.Size = New System.Drawing.Size(491, 166)
        Me.dgvIncubation.TabIndex = 0
        '
        'Column6
        '
        Me.Column6.HeaderText = "Medium"
        Me.Column6.Name = "Column6"
        Me.Column6.Width = 80
        '
        'Column7
        '
        Me.Column7.FalseValue = "0"
        Me.Column7.HeaderText = "On Plate"
        Me.Column7.IndeterminateValue = "0"
        Me.Column7.Name = "Column7"
        Me.Column7.TrueValue = "1"
        Me.Column7.Width = 60
        '
        'Column8
        '
        Me.Column8.HeaderText = "Temperature"
        Me.Column8.Name = "Column8"
        Me.Column8.Width = 80
        '
        'Column9
        '
        Me.Column9.HeaderText = "Antibiotics"
        Me.Column9.Name = "Column9"
        Me.Column9.Width = 80
        '
        'Column10
        '
        Me.Column10.HeaderText = "Inducer"
        Me.Column10.Name = "Column10"
        Me.Column10.Width = 80
        '
        'Column11
        '
        Me.Column11.FillWeight = 60.0!
        Me.Column11.HeaderText = "Time"
        Me.Column11.Name = "Column11"
        Me.Column11.Width = 80
        '
        'tpExtraction
        '
        Me.tpExtraction.Controls.Add(Me.cbExtractionSequencingVerify)
        Me.tpExtraction.Controls.Add(Me.cbExtractionIncludeVerification)
        Me.tpExtraction.Location = New System.Drawing.Point(4, 22)
        Me.tpExtraction.Name = "tpExtraction"
        Me.tpExtraction.Size = New System.Drawing.Size(497, 184)
        Me.tpExtraction.TabIndex = 18
        Me.tpExtraction.Text = "Extraction"
        Me.tpExtraction.UseVisualStyleBackColor = True
        '
        'cbExtractionSequencingVerify
        '
        Me.cbExtractionSequencingVerify.AutoSize = True
        Me.cbExtractionSequencingVerify.Location = New System.Drawing.Point(3, 57)
        Me.cbExtractionSequencingVerify.Name = "cbExtractionSequencingVerify"
        Me.cbExtractionSequencingVerify.Size = New System.Drawing.Size(123, 18)
        Me.cbExtractionSequencingVerify.TabIndex = 0
        Me.cbExtractionSequencingVerify.Text = "Sequencing Verified"
        Me.cbExtractionSequencingVerify.UseVisualStyleBackColor = True
        '
        'cbExtractionIncludeVerification
        '
        Me.cbExtractionIncludeVerification.AutoSize = True
        Me.cbExtractionIncludeVerification.Location = New System.Drawing.Point(3, 35)
        Me.cbExtractionIncludeVerification.Name = "cbExtractionIncludeVerification"
        Me.cbExtractionIncludeVerification.Size = New System.Drawing.Size(146, 18)
        Me.cbExtractionIncludeVerification.TabIndex = 0
        Me.cbExtractionIncludeVerification.Text = "Include Verfication Steps"
        Me.cbExtractionIncludeVerification.UseVisualStyleBackColor = True
        '
        'tpProteinExpression
        '
        Me.tpProteinExpression.Controls.Add(Me.pnlExpression)
        Me.tpProteinExpression.Location = New System.Drawing.Point(4, 22)
        Me.tpProteinExpression.Name = "tpProteinExpression"
        Me.tpProteinExpression.Size = New System.Drawing.Size(497, 184)
        Me.tpProteinExpression.TabIndex = 19
        Me.tpProteinExpression.Text = "Protein Expression"
        Me.tpProteinExpression.UseVisualStyleBackColor = True
        '
        'pnlExpression
        '
        Me.pnlExpression.Location = New System.Drawing.Point(0, 30)
        Me.pnlExpression.Name = "pnlExpression"
        Me.pnlExpression.Size = New System.Drawing.Size(497, 150)
        Me.pnlExpression.TabIndex = 3
        Me.ttDescription.SetToolTip(Me.pnlExpression, "Click to select the DNA.")
        '
        'tpGibsonDesign
        '
        Me.tpGibsonDesign.Controls.Add(Me.ihGibsonDesign)
        Me.tpGibsonDesign.Location = New System.Drawing.Point(4, 22)
        Me.tpGibsonDesign.Name = "tpGibsonDesign"
        Me.tpGibsonDesign.Size = New System.Drawing.Size(497, 184)
        Me.tpGibsonDesign.TabIndex = 19
        Me.tpGibsonDesign.Text = "Gibson Design"
        Me.tpGibsonDesign.UseVisualStyleBackColor = True
        '
        'ihGibsonDesign
        '
        Me.ihGibsonDesign.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ihGibsonDesign.Location = New System.Drawing.Point(0, 0)
        Me.ihGibsonDesign.Name = "ihGibsonDesign"
        Me.ihGibsonDesign.Size = New System.Drawing.Size(497, 184)
        Me.ihGibsonDesign.TabIndex = 0
        Me.ihGibsonDesign.Text = "Gibson Design"
        Me.ihGibsonDesign.Child = Me.ucWpfGibsonDesignPanel
        '
        'tpCRISPRCut
        '
        Me.tpCRISPRCut.Controls.Add(Me.ihCRISPRCut)
        Me.tpCRISPRCut.Location = New System.Drawing.Point(4, 22)
        Me.tpCRISPRCut.Name = "tpCRISPRCut"
        Me.tpCRISPRCut.Size = New System.Drawing.Size(497, 184)
        Me.tpCRISPRCut.TabIndex = 19
        Me.tpCRISPRCut.Text = "CRISPR Cut"
        Me.tpCRISPRCut.UseVisualStyleBackColor = True
        '
        'ihCRISPRCut
        '
        Me.ihCRISPRCut.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ihCRISPRCut.Location = New System.Drawing.Point(0, 0)
        Me.ihCRISPRCut.Name = "ihCRISPRCut"
        Me.ihCRISPRCut.Size = New System.Drawing.Size(497, 184)
        Me.ihCRISPRCut.TabIndex = 0
        Me.ihCRISPRCut.Text = "CRISPR Cut"
        Me.ihCRISPRCut.Child = Me.ucWpfcrisprCutPanel
        '
        'llApply
        '
        Me.llApply.AutoSize = True
        Me.llApply.BackColor = System.Drawing.Color.Transparent
        Me.llApply.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llApply.LinkColor = System.Drawing.Color.Red
        Me.llApply.Location = New System.Drawing.Point(510, -2)
        Me.llApply.Name = "llApply"
        Me.llApply.Size = New System.Drawing.Size(49, 19)
        Me.llApply.TabIndex = 28
        Me.llApply.TabStop = True
        Me.llApply.Text = "Apply"
        Me.ttDescription.SetToolTip(Me.llApply, "Calculate the products.")
        '
        'llCancel
        '
        Me.llCancel.ActiveLinkColor = System.Drawing.Color.Green
        Me.llCancel.AutoSize = True
        Me.llCancel.BackColor = System.Drawing.Color.Transparent
        Me.llCancel.DisabledLinkColor = System.Drawing.Color.Green
        Me.llCancel.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llCancel.ForeColor = System.Drawing.Color.Green
        Me.llCancel.LinkColor = System.Drawing.Color.Green
        Me.llCancel.Location = New System.Drawing.Point(565, -1)
        Me.llCancel.Name = "llCancel"
        Me.llCancel.Size = New System.Drawing.Size(53, 19)
        Me.llCancel.TabIndex = 28
        Me.llCancel.TabStop = True
        Me.llCancel.Text = "Cancel"
        Me.ttDescription.SetToolTip(Me.llCancel, "Abandon the changes")
        '
        'cbRealSize
        '
        Me.cbRealSize.AutoSize = True
        Me.cbRealSize.Location = New System.Drawing.Point(407, 23)
        Me.cbRealSize.Name = "cbRealSize"
        Me.cbRealSize.Size = New System.Drawing.Size(91, 18)
        Me.cbRealSize.TabIndex = 29
        Me.cbRealSize.Text = "100 pixel/Kbp"
        Me.cbRealSize.UseVisualStyleBackColor = True
        Me.cbRealSize.Visible = False
        '
        'ofdVector
        '
        Me.ofdVector.Filter = "Sequence File|*.gb;*.txt;*.seq"
        '
        'GroupBox4
        '
        Me.GroupBox4.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox4.Controls.Add(Me.rbObsolete)
        Me.GroupBox4.Controls.Add(Me.rbFinished)
        Me.GroupBox4.Controls.Add(Me.rbInprogress)
        Me.GroupBox4.Controls.Add(Me.rbUnstarted)
        Me.GroupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox4.Location = New System.Drawing.Point(651, 0)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(358, 32)
        Me.GroupBox4.TabIndex = 31
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Status"
        Me.ttDescription.SetToolTip(Me.GroupBox4, "Tags for labeling the the progress of wet experiment.")
        '
        'rbObsolete
        '
        Me.rbObsolete.AutoSize = True
        Me.rbObsolete.Location = New System.Drawing.Point(283, 10)
        Me.rbObsolete.Name = "rbObsolete"
        Me.rbObsolete.Size = New System.Drawing.Size(68, 18)
        Me.rbObsolete.TabIndex = 0
        Me.rbObsolete.Tag = "3"
        Me.rbObsolete.Text = "Obsolete"
        Me.ttDescription.SetToolTip(Me.rbObsolete, "This operation design is obsolete.")
        Me.rbObsolete.UseVisualStyleBackColor = True
        '
        'rbFinished
        '
        Me.rbFinished.AutoSize = True
        Me.rbFinished.Checked = True
        Me.rbFinished.Location = New System.Drawing.Point(195, 10)
        Me.rbFinished.Name = "rbFinished"
        Me.rbFinished.Size = New System.Drawing.Size(65, 18)
        Me.rbFinished.TabIndex = 0
        Me.rbFinished.TabStop = True
        Me.rbFinished.Tag = "2"
        Me.rbFinished.Text = "Finished"
        Me.ttDescription.SetToolTip(Me.rbFinished, "The wet experiment of this operation is done.")
        Me.rbFinished.UseVisualStyleBackColor = True
        '
        'rbInprogress
        '
        Me.rbInprogress.AutoSize = True
        Me.rbInprogress.Location = New System.Drawing.Point(100, 10)
        Me.rbInprogress.Name = "rbInprogress"
        Me.rbInprogress.Size = New System.Drawing.Size(80, 18)
        Me.rbInprogress.TabIndex = 0
        Me.rbInprogress.Tag = "1"
        Me.rbInprogress.Text = "In Progress"
        Me.ttDescription.SetToolTip(Me.rbInprogress, "The wet experiment of this operation is in progress.")
        Me.rbInprogress.UseVisualStyleBackColor = True
        '
        'rbUnstarted
        '
        Me.rbUnstarted.AutoSize = True
        Me.rbUnstarted.Location = New System.Drawing.Point(5, 10)
        Me.rbUnstarted.Name = "rbUnstarted"
        Me.rbUnstarted.Size = New System.Drawing.Size(79, 18)
        Me.rbUnstarted.TabIndex = 0
        Me.rbUnstarted.Tag = "0"
        Me.rbUnstarted.Text = "Not Started"
        Me.ttDescription.SetToolTip(Me.rbUnstarted, "The wet experiment of this operation has not been started.")
        Me.rbUnstarted.UseVisualStyleBackColor = True
        '
        'lblPixelPerBP
        '
        Me.lblPixelPerBP.AutoSize = True
        Me.lblPixelPerBP.BackColor = System.Drawing.Color.Transparent
        Me.lblPixelPerBP.Location = New System.Drawing.Point(450, 23)
        Me.lblPixelPerBP.Name = "lblPixelPerBP"
        Me.lblPixelPerBP.Size = New System.Drawing.Size(51, 14)
        Me.lblPixelPerBP.TabIndex = 33
        Me.lblPixelPerBP.Text = "Pixel/Kbp"
        Me.ttDescription.SetToolTip(Me.lblPixelPerBP, "pixels per 1000base pairs.")
        '
        'ttDescription
        '
        Me.ttDescription.AutomaticDelay = 300
        Me.ttDescription.AutoPopDelay = 4000
        Me.ttDescription.InitialDelay = 300
        Me.ttDescription.IsBalloon = True
        Me.ttDescription.ReshowDelay = 60
        '
        'rtbEnzymeAnalysisResults
        '
        Me.rtbEnzymeAnalysisResults.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rtbEnzymeAnalysisResults.BackColor = System.Drawing.Color.LemonChiffon
        Me.rtbEnzymeAnalysisResults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbEnzymeAnalysisResults.Font = New System.Drawing.Font("Arial", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbEnzymeAnalysisResults.Location = New System.Drawing.Point(512, 50)
        Me.rtbEnzymeAnalysisResults.Name = "rtbEnzymeAnalysisResults"
        Me.rtbEnzymeAnalysisResults.Size = New System.Drawing.Size(497, 287)
        Me.rtbEnzymeAnalysisResults.TabIndex = 35
        Me.rtbEnzymeAnalysisResults.Text = ""
        Me.ttDescription.SetToolTip(Me.rtbEnzymeAnalysisResults, "Enzymes that meet all the conditions of Restriction Enzyme Analysis.")
        Me.rtbEnzymeAnalysisResults.Visible = False
        '
        'cbDescribe
        '
        Me.cbDescribe.AutoSize = True
        Me.cbDescribe.Location = New System.Drawing.Point(591, 34)
        Me.cbDescribe.Name = "cbDescribe"
        Me.cbDescribe.Size = New System.Drawing.Size(89, 18)
        Me.cbDescribe.TabIndex = 37
        Me.cbDescribe.Text = "Chromosome"
        Me.ttDescription.SetToolTip(Me.cbDescribe, "Describe this item as on the chromosome.")
        Me.cbDescribe.UseVisualStyleBackColor = True
        '
        'snbPixelPerKBP
        '
        Me.snbPixelPerKBP.IncrementValue = 100
        Me.snbPixelPerKBP.Location = New System.Drawing.Point(397, 18)
        Me.snbPixelPerKBP.Maximum = 4000
        Me.snbPixelPerKBP.Minimum = 0
        Me.snbPixelPerKBP.Name = "snbPixelPerKBP"
        Me.snbPixelPerKBP.Size = New System.Drawing.Size(52, 20)
        Me.snbPixelPerKBP.TabIndex = 34
        Me.snbPixelPerKBP.Text = "0"
        Me.ttDescription.SetToolTip(Me.snbPixelPerKBP, resources.GetString("snbPixelPerKBP.ToolTip"))
        Me.snbPixelPerKBP.Value = 0
        '
        'llHelp
        '
        Me.llHelp.AutoSize = True
        Me.llHelp.Location = New System.Drawing.Point(980, 35)
        Me.llHelp.Name = "llHelp"
        Me.llHelp.Size = New System.Drawing.Size(28, 14)
        Me.llHelp.TabIndex = 36
        Me.llHelp.TabStop = True
        Me.llHelp.Text = "Help"
        '
        'cbEnvironment
        '
        Me.cbEnvironment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbEnvironment.FormattingEnabled = True
        Me.cbEnvironment.Location = New System.Drawing.Point(270, 19)
        Me.cbEnvironment.Name = "cbEnvironment"
        Me.cbEnvironment.Size = New System.Drawing.Size(121, 22)
        Me.cbEnvironment.TabIndex = 38
        '
        'cbVerify
        '
        Me.cbVerify.AutoSize = True
        Me.cbVerify.Location = New System.Drawing.Point(695, 34)
        Me.cbVerify.Name = "cbVerify"
        Me.cbVerify.Size = New System.Drawing.Size(105, 18)
        Me.cbVerify.TabIndex = 39
        Me.cbVerify.Text = "Verification Step"
        Me.cbVerify.UseVisualStyleBackColor = True
        '
        'lvCells
        '
        Me.lvCells.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvCells.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvCells.Location = New System.Drawing.Point(1015, 50)
        Me.lvCells.Name = "lvCells"
        Me.lvCells.Size = New System.Drawing.Size(204, 287)
        Me.lvCells.TabIndex = 40
        Me.lvCells.UseCompatibleStateImageBehavior = False
        Me.lvCells.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Cell"
        Me.ColumnHeader1.Width = 120
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "DNAs"
        '
        'cbMainConstruction
        '
        Me.cbMainConstruction.AutoSize = True
        Me.cbMainConstruction.Location = New System.Drawing.Point(224, 2)
        Me.cbMainConstruction.Name = "cbMainConstruction"
        Me.cbMainConstruction.Size = New System.Drawing.Size(115, 18)
        Me.cbMainConstruction.TabIndex = 41
        Me.cbMainConstruction.Text = "Construction Node"
        Me.cbMainConstruction.UseVisualStyleBackColor = True
        '
        'cbNoMap
        '
        Me.cbNoMap.AutoSize = True
        Me.cbNoMap.Location = New System.Drawing.Point(827, 34)
        Me.cbNoMap.Name = "cbNoMap"
        Me.cbNoMap.Size = New System.Drawing.Size(62, 18)
        Me.cbNoMap.TabIndex = 42
        Me.cbNoMap.Text = "No Map"
        Me.cbNoMap.UseVisualStyleBackColor = True
        '
        'PropertyControl
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.White
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Controls.Add(Me.lvCells)
        Me.Controls.Add(Me.cbEnvironment)
        Me.Controls.Add(Me.snbPixelPerKBP)
        Me.Controls.Add(Me.llHelp)
        Me.Controls.Add(Me.lblPixelPerBP)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.rtbEnzymeAnalysisResults)
        Me.Controls.Add(Me.llApply)
        Me.Controls.Add(Me.cbRealSize)
        Me.Controls.Add(Me.rtb_Description)
        Me.Controls.Add(Me.llCancel)
        Me.Controls.Add(Me.DNAView)
        Me.Controls.Add(Me.ll_ViewDetails)
        Me.Controls.Add(Me.Prop_Name)
        Me.Controls.Add(Me.Prop_Type)
        Me.Controls.Add(Me.Prop_Count)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Prop_Operation)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TabControl_Operation)
        Me.Controls.Add(Me.llName)
        Me.Controls.Add(Me.cbDescribe)
        Me.Controls.Add(Me.cbMainConstruction)
        Me.Controls.Add(Me.cbNoMap)
        Me.Controls.Add(Me.cbVerify)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Name = "PropertyControl"
        Me.Size = New System.Drawing.Size(1221, 344)
        Me.TabControl_Operation.ResumeLayout(False)
        Me.tpFile.ResumeLayout(False)
        Me.tpFile.PerformLayout()
        Me.tpEnzyme.ResumeLayout(False)
        Me.tpEnzyme.PerformLayout()
        Me.tpPCR.ResumeLayout(False)
        Me.tpPCR.PerformLayout()
        CType(Me.pafPCR, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpModify.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tpGel.ResumeLayout(False)
        Me.tpGel.PerformLayout()
        CType(Me.Gel_Maximum_Number, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Gel_Minimum_Number, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpLigation.ResumeLayout(False)
        Me.tpScreen.ResumeLayout(False)
        Me.tpScreen.PerformLayout()
        Me.Screen_PCR_Panel.ResumeLayout(False)
        Me.Screen_PCR_Panel.PerformLayout()
        CType(Me.Screen_PCR_nudMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Screen_PCR_nudMin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpRec.ResumeLayout(False)
        Me.tpEnzymeAnalysis.ResumeLayout(False)
        Me.tpEnzymeAnalysis.PerformLayout()
        CType(Me.dgvEnzymeAnalysis, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmsEnzyme.ResumeLayout(False)
        Me.tpSequenceMerge.ResumeLayout(False)
        Me.tpSequenceMerge.PerformLayout()
        Me.tbFreeDesign.ResumeLayout(False)
        Me.tbFreeDesign.PerformLayout()
        Me.tpHashPicker.ResumeLayout(False)
        Me.tpHashPicker.PerformLayout()
        Me.tpSequencingResult.ResumeLayout(False)
        Me.tpSequencingResult.PerformLayout()
        Me.tpComparer.ResumeLayout(False)
        Me.tpComparer.PerformLayout()
        Me.tpHost.ResumeLayout(False)
        Me.tpHost.PerformLayout()
        Me.tpTransformation.ResumeLayout(False)
        Me.gbTransformationMode.ResumeLayout(False)
        Me.gbTransformationMode.PerformLayout()
        Me.gbTransfromation.ResumeLayout(False)
        Me.gbTransfromation.PerformLayout()
        Me.tpIncubation.ResumeLayout(False)
        Me.tpIncubation.PerformLayout()
        CType(Me.dgvIncubation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpExtraction.ResumeLayout(False)
        Me.tpExtraction.PerformLayout()
        Me.tpProteinExpression.ResumeLayout(False)
        Me.tpGibsonDesign.ResumeLayout(False)
        Me.tpCRISPRCut.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents rtb_Description As System.Windows.Forms.RichTextBox
    Friend WithEvents ll_ViewDetails As System.Windows.Forms.LinkLabel
    Friend WithEvents DNAView As System.Windows.Forms.ListView
    Friend WithEvents ch_Name As System.Windows.Forms.ColumnHeader
    Friend WithEvents ch_Size As System.Windows.Forms.ColumnHeader
    Friend WithEvents cn_Cir As System.Windows.Forms.ColumnHeader
    Friend WithEvents ch_ENDS As System.Windows.Forms.ColumnHeader
    Friend WithEvents ch_Phos As System.Windows.Forms.ColumnHeader
    Friend WithEvents Prop_Name As System.Windows.Forms.TextBox
    Friend WithEvents Prop_Count As System.Windows.Forms.LinkLabel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Prop_Type As System.Windows.Forms.LinkLabel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Prop_Operation As System.Windows.Forms.LinkLabel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents llName As System.Windows.Forms.LinkLabel
    Friend WithEvents TabControl_Operation As System.Windows.Forms.TabControl
    Friend WithEvents tpFile As System.Windows.Forms.TabPage
    Friend WithEvents File_FileName_LinkLabel As System.Windows.Forms.LinkLabel
    Friend WithEvents File_Path_Label As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tpEnzyme As System.Windows.Forms.TabPage
    Friend WithEvents Enzyme_Enzymes_LinkLabel As System.Windows.Forms.LinkLabel
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents tpPCR As System.Windows.Forms.TabPage
    Friend WithEvents PCR_ReversePrimer_TextBox As System.Windows.Forms.TextBox
    Friend WithEvents PCR_ForwardPrimer_TextBox As System.Windows.Forms.TextBox
    Friend WithEvents tpModify As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Modify_PNK As System.Windows.Forms.RadioButton
    Friend WithEvents Modify_CIAP As System.Windows.Forms.RadioButton
    Friend WithEvents Modify_Klewnow As System.Windows.Forms.RadioButton
    Friend WithEvents Modify_T4 As System.Windows.Forms.RadioButton
    Friend WithEvents tpGel As System.Windows.Forms.TabPage
    Friend WithEvents Gel_Maximum_Number As System.Windows.Forms.NumericUpDown
    Friend WithEvents Gel_Minimum_Number As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tpLigation As System.Windows.Forms.TabPage
    Friend WithEvents tpScreen As System.Windows.Forms.TabPage
    Friend WithEvents Screen_PCR_Panel As System.Windows.Forms.Panel
    Friend WithEvents Screen_PCR_nudMax As System.Windows.Forms.NumericUpDown
    Friend WithEvents Screen_PCR_nudMin As System.Windows.Forms.NumericUpDown
    Friend WithEvents Screen_PCR_lblMax As System.Windows.Forms.Label
    Friend WithEvents Screen_PCR_lblMin As System.Windows.Forms.Label
    Friend WithEvents Screen_PCR_RCF As System.Windows.Forms.Button
    Friend WithEvents Screen_PCR_R As System.Windows.Forms.TextBox
    Friend WithEvents Screen_PCR_F As System.Windows.Forms.TextBox
    Friend WithEvents Screen_PCR As System.Windows.Forms.RadioButton
    Friend WithEvents Screen_Freatures As System.Windows.Forms.RadioButton
    Friend WithEvents Screen_Features_LinkLabel As System.Windows.Forms.LinkLabel
    Friend WithEvents tpRec As System.Windows.Forms.TabPage
    Friend WithEvents llApply As System.Windows.Forms.LinkLabel
    Friend WithEvents llCancel As System.Windows.Forms.LinkLabel
    Friend WithEvents pafPCR As MCDS.PrimerAnalysisFrame
    Friend WithEvents btnBothPrimer As System.Windows.Forms.Button
    Friend WithEvents tbRP As System.Windows.Forms.ComboBox
    Friend WithEvents tbFP As System.Windows.Forms.ComboBox
    Friend WithEvents cbScreenCircular As System.Windows.Forms.CheckBox
    Friend WithEvents tpEnzymeAnalysis As System.Windows.Forms.TabPage
    Friend WithEvents tpSequenceMerge As System.Windows.Forms.TabPage
    Friend WithEvents tpHashPicker As System.Windows.Forms.TabPage
    Friend WithEvents dgvEnzymeAnalysis As System.Windows.Forms.DataGridView
    Friend WithEvents cmsEnzyme As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tbSCRRP As System.Windows.Forms.ComboBox
    Friend WithEvents tbSCRFP As System.Windows.Forms.ComboBox
    Friend WithEvents cbRealSize As System.Windows.Forms.CheckBox
    Friend WithEvents lblRCount As System.Windows.Forms.Label
    Friend WithEvents lblFCount As System.Windows.Forms.Label
    Friend WithEvents ofdVector As System.Windows.Forms.OpenFileDialog
    Friend WithEvents tbEnzymes As System.Windows.Forms.TextBox
    Friend WithEvents pnlGel As System.Windows.Forms.Panel
    Friend WithEvents pnlFeature As System.Windows.Forms.Panel
    Friend WithEvents cbMergeSignificant As System.Windows.Forms.CheckBox
    Friend WithEvents cbMergeExtend As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents rbFinished As System.Windows.Forms.RadioButton
    Friend WithEvents rbInprogress As System.Windows.Forms.RadioButton
    Friend WithEvents rbUnstarted As System.Windows.Forms.RadioButton
    Friend WithEvents btnOverlap As System.Windows.Forms.Button
    Friend WithEvents tbFreeDesign As System.Windows.Forms.TabPage
    Friend WithEvents rtbFreeDesign As System.Windows.Forms.RichTextBox
    Friend WithEvents tbDesign As System.Windows.Forms.TextBox
    Friend WithEvents cbAuto As System.Windows.Forms.CheckBox
    Friend WithEvents lblPixelPerBP As System.Windows.Forms.Label
    Friend WithEvents snbPixelPerKBP As MCDS.ScrollingNumberBox
    Friend WithEvents lblHashPickerList As System.Windows.Forms.Label
    Friend WithEvents lblHashPickerChoosen As System.Windows.Forms.Label
    Friend WithEvents cpHashPickerListedDNA As MCDS.ChoicePanel
    Friend WithEvents cpHashPickerChoosenDNA As MCDS.ChoicePanel
    Friend WithEvents tpComparer As System.Windows.Forms.TabPage
    Friend WithEvents tpSequencingResult As System.Windows.Forms.TabPage
    Friend WithEvents lbl_Sequencing_NameTag As System.Windows.Forms.Label
    Friend WithEvents rtbSequencingResult As System.Windows.Forms.RichTextBox
    Friend WithEvents tbSequencingPrimerName As System.Windows.Forms.ComboBox
    Friend WithEvents tbSequencingPrimerSequence As System.Windows.Forms.TextBox
    Friend WithEvents cpCompareList As MCDS.ChoicePanel
    Friend WithEvents cpCompareChoice As MCDS.ChoicePanel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents rbObsolete As System.Windows.Forms.RadioButton
    Friend WithEvents llAnalysisRemove As System.Windows.Forms.LinkLabel
    Friend WithEvents llAnalysisAdd As System.Windows.Forms.LinkLabel
    Friend WithEvents ttDescription As System.Windows.Forms.ToolTip
    Friend WithEvents rtbEnzymeAnalysisResults As System.Windows.Forms.RichTextBox
    Friend WithEvents llHelp As System.Windows.Forms.LinkLabel
    Friend WithEvents cbDescribe As System.Windows.Forms.CheckBox
    Friend WithEvents cbSequencingResultOption As System.Windows.Forms.ComboBox
    Friend WithEvents cbCompareResult As System.Windows.Forms.ComboBox
    Friend WithEvents chChromo As System.Windows.Forms.ColumnHeader
    Friend WithEvents cbEnvironment As System.Windows.Forms.ComboBox
    Friend WithEvents tpHost As System.Windows.Forms.TabPage
    Friend WithEvents tpTransformation As System.Windows.Forms.TabPage
    Friend WithEvents tpIncubation As System.Windows.Forms.TabPage
    Friend WithEvents tpExtraction As System.Windows.Forms.TabPage
    Friend WithEvents rtbChromosomoFragment As System.Windows.Forms.RichTextBox
    Friend WithEvents llAddChromosomeFragment As System.Windows.Forms.LinkLabel
    Friend WithEvents llRemoveIncubation As System.Windows.Forms.LinkLabel
    Friend WithEvents llAddIncubation As System.Windows.Forms.LinkLabel
    Friend WithEvents dgvIncubation As System.Windows.Forms.DataGridView
    Friend WithEvents tbHostName As System.Windows.Forms.TextBox
    Friend WithEvents lbHostName As System.Windows.Forms.Label
    Friend WithEvents cbVerify As System.Windows.Forms.CheckBox
    Friend WithEvents llRemoveChromosomeFragment As System.Windows.Forms.LinkLabel
    Friend WithEvents tbChromosomeFragmentName As System.Windows.Forms.TextBox
    Friend WithEvents rbTransformationConjugation As System.Windows.Forms.RadioButton
    Friend WithEvents rbTransformationElectroporation As System.Windows.Forms.RadioButton
    Friend WithEvents rbTransformationChemical As System.Windows.Forms.RadioButton
    Friend WithEvents lbFragmentName As System.Windows.Forms.Label
    Friend WithEvents tbHostFunction As System.Windows.Forms.TextBox
    Friend WithEvents llHostFunctions As System.Windows.Forms.Label
    Friend WithEvents lvCells As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents cbDephosphorylate As System.Windows.Forms.CheckBox
    Friend WithEvents cbExtractionSequencingVerify As System.Windows.Forms.CheckBox
    Friend WithEvents cbExtractionIncludeVerification As System.Windows.Forms.CheckBox
    Friend WithEvents cbGel_Solution As System.Windows.Forms.CheckBox
    Friend WithEvents llCommonStep As System.Windows.Forms.LinkLabel
    Friend WithEvents cbMainConstruction As System.Windows.Forms.CheckBox
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Column8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents pnlScreenPCR As System.Windows.Forms.Panel
    Friend WithEvents cbNoMap As System.Windows.Forms.CheckBox
    Friend WithEvents gbTransformationMode As System.Windows.Forms.GroupBox
    Friend WithEvents rbTransformationCBNT As System.Windows.Forms.RadioButton
    Friend WithEvents rbTransformationEDPC As System.Windows.Forms.RadioButton
    Friend WithEvents rbTransformationAIOC As System.Windows.Forms.RadioButton
    Friend WithEvents gbTransfromation As System.Windows.Forms.GroupBox
    'Friend WithEvents wbPromotion As System.Windows.Forms.WebBrowser
    Friend WithEvents tpProteinExpression As System.Windows.Forms.TabPage
    Friend WithEvents pnlExpression As System.Windows.Forms.Panel
    Friend WithEvents cbDesign_UseDesigner As System.Windows.Forms.CheckBox
    Friend WithEvents btnScreenReset As System.Windows.Forms.Button
    Friend WithEvents ihRecombination As InteropHost
    Friend WithEvents tpGibsonDesign As System.Windows.Forms.TabPage
    Friend WithEvents tpCRISPRCut As System.Windows.Forms.TabPage
    Friend ucWpfRecombinationPanel As WPFRecombinationPanel
    Friend WithEvents ihGibsonDesign As InteropHost
    Friend WithEvents ihCRISPRCut As InteropHost
    Friend ucWpfGibsonDesignPanel As WPFGibsonDesignPanel
    Friend ucWpfcrisprCutPanel As WPFCRISPRCutPanel
    Friend WithEvents ihLigation As InteropHost
    Friend ucWpfLigationPanel As WPFLigationPanel
End Class
