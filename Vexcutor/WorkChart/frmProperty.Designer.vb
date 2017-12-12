<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProperty
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProperty))
        Me.TabControl_Operation = New System.Windows.Forms.TabControl
        Me.tpFile = New System.Windows.Forms.TabPage
        Me.File_FileName_LinkLabel = New System.Windows.Forms.LinkLabel
        Me.File_Path_Label = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.tpEnzyme = New System.Windows.Forms.TabPage
        Me.Enzyme_Enzymes_LinkLabel = New System.Windows.Forms.LinkLabel
        Me.Label12 = New System.Windows.Forms.Label
        Me.tpPCR = New System.Windows.Forms.TabPage
        Me.btn_RCR = New System.Windows.Forms.Button
        Me.btn_RCF = New System.Windows.Forms.Button
        Me.PCR_ReversePrimer_TextBox = New System.Windows.Forms.TextBox
        Me.PCR_ForwardPrimer_TextBox = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.tpModify = New System.Windows.Forms.TabPage
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Modify_PNK = New System.Windows.Forms.RadioButton
        Me.Modify_CIAP = New System.Windows.Forms.RadioButton
        Me.Modify_Klewnow = New System.Windows.Forms.RadioButton
        Me.Modify_T4 = New System.Windows.Forms.RadioButton
        Me.tpGel = New System.Windows.Forms.TabPage
        Me.Gel_Maximum_Number = New System.Windows.Forms.NumericUpDown
        Me.Gel_Minimum_Number = New System.Windows.Forms.NumericUpDown
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.tpLigation = New System.Windows.Forms.TabPage
        Me.cbML = New System.Windows.Forms.CheckBox
        Me.Ligation_TriFrag = New System.Windows.Forms.CheckBox
        Me.tpScreen = New System.Windows.Forms.TabPage
        Me.Screen_PCR_Panel = New System.Windows.Forms.Panel
        Me.Screen_PCR_nudMax = New System.Windows.Forms.NumericUpDown
        Me.Screen_PCR_nudMin = New System.Windows.Forms.NumericUpDown
        Me.Screen_PCR_lblMax = New System.Windows.Forms.Label
        Me.Screen_PCR_lblMin = New System.Windows.Forms.Label
        Me.Screen_PCR_RCR = New System.Windows.Forms.Button
        Me.Screen_PCR_RCF = New System.Windows.Forms.Button
        Me.Screen_PCR_R = New System.Windows.Forms.TextBox
        Me.Screen_PCR_F = New System.Windows.Forms.TextBox
        Me.Screen_PCR = New System.Windows.Forms.RadioButton
        Me.Screen_Freatures = New System.Windows.Forms.RadioButton
        Me.Screen_Features_LinkLabel = New System.Windows.Forms.LinkLabel
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Prop_Operation = New System.Windows.Forms.LinkLabel
        Me.Label3 = New System.Windows.Forms.Label
        Me.Prop_Type = New System.Windows.Forms.LinkLabel
        Me.Label4 = New System.Windows.Forms.Label
        Me.Prop_Count = New System.Windows.Forms.LinkLabel
        Me.btn_Cancel = New System.Windows.Forms.Button
        Me.btn_OK = New System.Windows.Forms.Button
        Me.LinkLabel5 = New System.Windows.Forms.LinkLabel
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.ListView3 = New System.Windows.Forms.ListView
        Me.ofdGeneFile = New System.Windows.Forms.OpenFileDialog
        Me.Prop_Name = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.DNAView = New System.Windows.Forms.ListView
        Me.ch_Name = New System.Windows.Forms.ColumnHeader
        Me.ch_Size = New System.Windows.Forms.ColumnHeader
        Me.cn_Cir = New System.Windows.Forms.ColumnHeader
        Me.ch_ENDS = New System.Windows.Forms.ColumnHeader
        Me.ch_Phos = New System.Windows.Forms.ColumnHeader
        Me.LargeIconList = New System.Windows.Forms.ImageList(Me.components)
        Me.SmallIconList = New System.Windows.Forms.ImageList(Me.components)
        Me.ll_ViewLarge = New System.Windows.Forms.LinkLabel
        Me.ll_ViewDetails = New System.Windows.Forms.LinkLabel
        Me.Label10 = New System.Windows.Forms.Label
        Me.rtb_Description = New System.Windows.Forms.RichTextBox
        Me.TabControl_Operation.SuspendLayout()
        Me.tpFile.SuspendLayout()
        Me.tpEnzyme.SuspendLayout()
        Me.tpPCR.SuspendLayout()
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
        Me.SuspendLayout()
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
        Me.TabControl_Operation.Location = New System.Drawing.Point(12, 81)
        Me.TabControl_Operation.Name = "TabControl_Operation"
        Me.TabControl_Operation.SelectedIndex = 0
        Me.TabControl_Operation.Size = New System.Drawing.Size(505, 121)
        Me.TabControl_Operation.TabIndex = 0
        '
        'tpFile
        '
        Me.tpFile.Controls.Add(Me.File_FileName_LinkLabel)
        Me.tpFile.Controls.Add(Me.File_Path_Label)
        Me.tpFile.Controls.Add(Me.Label18)
        Me.tpFile.Controls.Add(Me.Label5)
        Me.tpFile.Location = New System.Drawing.Point(4, 21)
        Me.tpFile.Name = "tpFile"
        Me.tpFile.Size = New System.Drawing.Size(497, 96)
        Me.tpFile.TabIndex = 6
        Me.tpFile.Text = "File"
        Me.tpFile.UseVisualStyleBackColor = True
        '
        'File_FileName_LinkLabel
        '
        Me.File_FileName_LinkLabel.AutoSize = True
        Me.File_FileName_LinkLabel.Location = New System.Drawing.Point(101, 27)
        Me.File_FileName_LinkLabel.Name = "File_FileName_LinkLabel"
        Me.File_FileName_LinkLabel.Size = New System.Drawing.Size(95, 12)
        Me.File_FileName_LinkLabel.TabIndex = 1
        Me.File_FileName_LinkLabel.TabStop = True
        Me.File_FileName_LinkLabel.Text = "[Filename text]"
        '
        'File_Path_Label
        '
        Me.File_Path_Label.AutoSize = True
        Me.File_Path_Label.Location = New System.Drawing.Point(101, 54)
        Me.File_Path_Label.Name = "File_Path_Label"
        Me.File_Path_Label.Size = New System.Drawing.Size(29, 12)
        Me.File_Path_Label.TabIndex = 0
        Me.File_Path_Label.Text = "Path"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(21, 54)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(29, 12)
        Me.Label18.TabIndex = 0
        Me.Label18.Text = "Path"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(22, 27)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 12)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Filename"
        '
        'tpEnzyme
        '
        Me.tpEnzyme.Controls.Add(Me.Enzyme_Enzymes_LinkLabel)
        Me.tpEnzyme.Controls.Add(Me.Label12)
        Me.tpEnzyme.Location = New System.Drawing.Point(4, 21)
        Me.tpEnzyme.Name = "tpEnzyme"
        Me.tpEnzyme.Padding = New System.Windows.Forms.Padding(3)
        Me.tpEnzyme.Size = New System.Drawing.Size(497, 96)
        Me.tpEnzyme.TabIndex = 0
        Me.tpEnzyme.Text = "Enzyme"
        Me.tpEnzyme.UseVisualStyleBackColor = True
        '
        'Enzyme_Enzymes_LinkLabel
        '
        Me.Enzyme_Enzymes_LinkLabel.AutoSize = True
        Me.Enzyme_Enzymes_LinkLabel.Location = New System.Drawing.Point(19, 47)
        Me.Enzyme_Enzymes_LinkLabel.Name = "Enzyme_Enzymes_LinkLabel"
        Me.Enzyme_Enzymes_LinkLabel.Size = New System.Drawing.Size(107, 12)
        Me.Enzyme_Enzymes_LinkLabel.TabIndex = 2
        Me.Enzyme_Enzymes_LinkLabel.TabStop = True
        Me.Enzyme_Enzymes_LinkLabel.Text = "[Click to select]"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(19, 24)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(47, 12)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Enzymes"
        '
        'tpPCR
        '
        Me.tpPCR.Controls.Add(Me.btn_RCR)
        Me.tpPCR.Controls.Add(Me.btn_RCF)
        Me.tpPCR.Controls.Add(Me.PCR_ReversePrimer_TextBox)
        Me.tpPCR.Controls.Add(Me.PCR_ForwardPrimer_TextBox)
        Me.tpPCR.Controls.Add(Me.Label7)
        Me.tpPCR.Controls.Add(Me.Label6)
        Me.tpPCR.Location = New System.Drawing.Point(4, 21)
        Me.tpPCR.Name = "tpPCR"
        Me.tpPCR.Padding = New System.Windows.Forms.Padding(3)
        Me.tpPCR.Size = New System.Drawing.Size(497, 96)
        Me.tpPCR.TabIndex = 1
        Me.tpPCR.Text = "PCR"
        Me.tpPCR.UseVisualStyleBackColor = True
        '
        'btn_RCR
        '
        Me.btn_RCR.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_RCR.Location = New System.Drawing.Point(444, 56)
        Me.btn_RCR.Name = "btn_RCR"
        Me.btn_RCR.Size = New System.Drawing.Size(33, 21)
        Me.btn_RCR.TabIndex = 2
        Me.btn_RCR.Text = "RC"
        Me.btn_RCR.UseVisualStyleBackColor = True
        '
        'btn_RCF
        '
        Me.btn_RCF.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_RCF.Location = New System.Drawing.Point(444, 24)
        Me.btn_RCF.Name = "btn_RCF"
        Me.btn_RCF.Size = New System.Drawing.Size(33, 22)
        Me.btn_RCF.TabIndex = 2
        Me.btn_RCF.Text = "RC"
        Me.btn_RCF.UseVisualStyleBackColor = True
        '
        'PCR_ReversePrimer_TextBox
        '
        Me.PCR_ReversePrimer_TextBox.Location = New System.Drawing.Point(105, 56)
        Me.PCR_ReversePrimer_TextBox.Name = "PCR_ReversePrimer_TextBox"
        Me.PCR_ReversePrimer_TextBox.Size = New System.Drawing.Size(330, 21)
        Me.PCR_ReversePrimer_TextBox.TabIndex = 1
        '
        'PCR_ForwardPrimer_TextBox
        '
        Me.PCR_ForwardPrimer_TextBox.Location = New System.Drawing.Point(105, 25)
        Me.PCR_ForwardPrimer_TextBox.Name = "PCR_ForwardPrimer_TextBox"
        Me.PCR_ForwardPrimer_TextBox.Size = New System.Drawing.Size(330, 21)
        Me.PCR_ForwardPrimer_TextBox.TabIndex = 1
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(10, 59)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(89, 12)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Reverse Primer"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(11, 28)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(89, 12)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Forward Primer"
        '
        'tpModify
        '
        Me.tpModify.Controls.Add(Me.GroupBox1)
        Me.tpModify.Location = New System.Drawing.Point(4, 21)
        Me.tpModify.Name = "tpModify"
        Me.tpModify.Size = New System.Drawing.Size(497, 96)
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
        Me.GroupBox1.Location = New System.Drawing.Point(14, 15)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(465, 66)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Modification Methods"
        '
        'Modify_PNK
        '
        Me.Modify_PNK.AutoSize = True
        Me.Modify_PNK.Location = New System.Drawing.Point(239, 42)
        Me.Modify_PNK.Name = "Modify_PNK"
        Me.Modify_PNK.Size = New System.Drawing.Size(113, 16)
        Me.Modify_PNK.TabIndex = 0
        Me.Modify_PNK.TabStop = True
        Me.Modify_PNK.Text = "PNK Phosphorate"
        Me.Modify_PNK.UseVisualStyleBackColor = True
        '
        'Modify_CIAP
        '
        Me.Modify_CIAP.AutoSize = True
        Me.Modify_CIAP.Location = New System.Drawing.Point(15, 42)
        Me.Modify_CIAP.Name = "Modify_CIAP"
        Me.Modify_CIAP.Size = New System.Drawing.Size(131, 16)
        Me.Modify_CIAP.TabIndex = 0
        Me.Modify_CIAP.TabStop = True
        Me.Modify_CIAP.Text = "CIAP Dephosphorate"
        Me.Modify_CIAP.UseVisualStyleBackColor = True
        '
        'Modify_Klewnow
        '
        Me.Modify_Klewnow.AutoSize = True
        Me.Modify_Klewnow.Location = New System.Drawing.Point(239, 20)
        Me.Modify_Klewnow.Name = "Modify_Klewnow"
        Me.Modify_Klewnow.Size = New System.Drawing.Size(185, 16)
        Me.Modify_Klewnow.TabIndex = 0
        Me.Modify_Klewnow.TabStop = True
        Me.Modify_Klewnow.Text = "Blunt with Klewnow Fragment"
        Me.Modify_Klewnow.UseVisualStyleBackColor = True
        '
        'Modify_T4
        '
        Me.Modify_T4.AutoSize = True
        Me.Modify_T4.Location = New System.Drawing.Point(15, 20)
        Me.Modify_T4.Name = "Modify_T4"
        Me.Modify_T4.Size = New System.Drawing.Size(191, 16)
        Me.Modify_T4.TabIndex = 0
        Me.Modify_T4.TabStop = True
        Me.Modify_T4.Text = "Blunt with T4 DNA polymerase"
        Me.Modify_T4.UseVisualStyleBackColor = True
        '
        'tpGel
        '
        Me.tpGel.Controls.Add(Me.Gel_Maximum_Number)
        Me.tpGel.Controls.Add(Me.Gel_Minimum_Number)
        Me.tpGel.Controls.Add(Me.Label9)
        Me.tpGel.Controls.Add(Me.Label8)
        Me.tpGel.Location = New System.Drawing.Point(4, 21)
        Me.tpGel.Name = "tpGel"
        Me.tpGel.Size = New System.Drawing.Size(497, 96)
        Me.tpGel.TabIndex = 2
        Me.tpGel.Text = "Gel"
        Me.tpGel.UseVisualStyleBackColor = True
        '
        'Gel_Maximum_Number
        '
        Me.Gel_Maximum_Number.Increment = New Decimal(New Integer() {50, 0, 0, 0})
        Me.Gel_Maximum_Number.Location = New System.Drawing.Point(311, 25)
        Me.Gel_Maximum_Number.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.Gel_Maximum_Number.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.Gel_Maximum_Number.Name = "Gel_Maximum_Number"
        Me.Gel_Maximum_Number.Size = New System.Drawing.Size(120, 21)
        Me.Gel_Maximum_Number.TabIndex = 1
        Me.Gel_Maximum_Number.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'Gel_Minimum_Number
        '
        Me.Gel_Minimum_Number.Increment = New Decimal(New Integer() {50, 0, 0, 0})
        Me.Gel_Minimum_Number.Location = New System.Drawing.Point(92, 25)
        Me.Gel_Minimum_Number.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.Gel_Minimum_Number.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.Gel_Minimum_Number.Name = "Gel_Minimum_Number"
        Me.Gel_Minimum_Number.Size = New System.Drawing.Size(120, 21)
        Me.Gel_Minimum_Number.TabIndex = 1
        Me.Gel_Minimum_Number.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(252, 27)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(47, 12)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Maximum"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(29, 27)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(47, 12)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Minimum"
        '
        'tpLigation
        '
        Me.tpLigation.Controls.Add(Me.cbML)
        Me.tpLigation.Controls.Add(Me.Ligation_TriFrag)
        Me.tpLigation.Location = New System.Drawing.Point(4, 21)
        Me.tpLigation.Name = "tpLigation"
        Me.tpLigation.Size = New System.Drawing.Size(497, 96)
        Me.tpLigation.TabIndex = 7
        Me.tpLigation.Text = "Ligation"
        Me.tpLigation.UseVisualStyleBackColor = True
        '
        'cbML
        '
        Me.cbML.AutoSize = True
        Me.cbML.Location = New System.Drawing.Point(21, 56)
        Me.cbML.Name = "cbML"
        Me.cbML.Size = New System.Drawing.Size(168, 16)
        Me.cbML.TabIndex = 5
        Me.cbML.Text = "Multiple Linear Ligation"
        Me.cbML.UseVisualStyleBackColor = True
        '
        'Ligation_TriFrag
        '
        Me.Ligation_TriFrag.AutoSize = True
        Me.Ligation_TriFrag.Location = New System.Drawing.Point(21, 34)
        Me.Ligation_TriFrag.Name = "Ligation_TriFrag"
        Me.Ligation_TriFrag.Size = New System.Drawing.Size(216, 16)
        Me.Ligation_TriFrag.TabIndex = 5
        Me.Ligation_TriFrag.Text = "Consider ligation of 3 framgents"
        Me.Ligation_TriFrag.UseVisualStyleBackColor = True
        '
        'tpScreen
        '
        Me.tpScreen.Controls.Add(Me.Screen_PCR_Panel)
        Me.tpScreen.Controls.Add(Me.Screen_PCR)
        Me.tpScreen.Controls.Add(Me.Screen_Freatures)
        Me.tpScreen.Controls.Add(Me.Screen_Features_LinkLabel)
        Me.tpScreen.Location = New System.Drawing.Point(4, 21)
        Me.tpScreen.Name = "tpScreen"
        Me.tpScreen.Size = New System.Drawing.Size(497, 96)
        Me.tpScreen.TabIndex = 5
        Me.tpScreen.Text = "Screen"
        Me.tpScreen.UseVisualStyleBackColor = True
        '
        'Screen_PCR_Panel
        '
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_nudMax)
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_nudMin)
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_lblMax)
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_lblMin)
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_RCR)
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_RCF)
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_R)
        Me.Screen_PCR_Panel.Controls.Add(Me.Screen_PCR_F)
        Me.Screen_PCR_Panel.Location = New System.Drawing.Point(3, 32)
        Me.Screen_PCR_Panel.Name = "Screen_PCR_Panel"
        Me.Screen_PCR_Panel.Size = New System.Drawing.Size(491, 64)
        Me.Screen_PCR_Panel.TabIndex = 8
        Me.Screen_PCR_Panel.Visible = False
        '
        'Screen_PCR_nudMax
        '
        Me.Screen_PCR_nudMax.Increment = New Decimal(New Integer() {50, 0, 0, 0})
        Me.Screen_PCR_nudMax.Location = New System.Drawing.Point(358, 8)
        Me.Screen_PCR_nudMax.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.Screen_PCR_nudMax.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.Screen_PCR_nudMax.Name = "Screen_PCR_nudMax"
        Me.Screen_PCR_nudMax.Size = New System.Drawing.Size(120, 21)
        Me.Screen_PCR_nudMax.TabIndex = 21
        Me.Screen_PCR_nudMax.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'Screen_PCR_nudMin
        '
        Me.Screen_PCR_nudMin.Increment = New Decimal(New Integer() {50, 0, 0, 0})
        Me.Screen_PCR_nudMin.Location = New System.Drawing.Point(358, 36)
        Me.Screen_PCR_nudMin.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.Screen_PCR_nudMin.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.Screen_PCR_nudMin.Name = "Screen_PCR_nudMin"
        Me.Screen_PCR_nudMin.Size = New System.Drawing.Size(120, 21)
        Me.Screen_PCR_nudMin.TabIndex = 22
        Me.Screen_PCR_nudMin.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'Screen_PCR_lblMax
        '
        Me.Screen_PCR_lblMax.AutoSize = True
        Me.Screen_PCR_lblMax.Location = New System.Drawing.Point(297, 10)
        Me.Screen_PCR_lblMax.Name = "Screen_PCR_lblMax"
        Me.Screen_PCR_lblMax.Size = New System.Drawing.Size(47, 12)
        Me.Screen_PCR_lblMax.TabIndex = 19
        Me.Screen_PCR_lblMax.Text = "Maximum"
        '
        'Screen_PCR_lblMin
        '
        Me.Screen_PCR_lblMin.AutoSize = True
        Me.Screen_PCR_lblMin.Location = New System.Drawing.Point(297, 39)
        Me.Screen_PCR_lblMin.Name = "Screen_PCR_lblMin"
        Me.Screen_PCR_lblMin.Size = New System.Drawing.Size(47, 12)
        Me.Screen_PCR_lblMin.TabIndex = 20
        Me.Screen_PCR_lblMin.Text = "Minimum"
        '
        'Screen_PCR_RCR
        '
        Me.Screen_PCR_RCR.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Screen_PCR_RCR.Location = New System.Drawing.Point(220, 37)
        Me.Screen_PCR_RCR.Name = "Screen_PCR_RCR"
        Me.Screen_PCR_RCR.Size = New System.Drawing.Size(34, 21)
        Me.Screen_PCR_RCR.TabIndex = 17
        Me.Screen_PCR_RCR.Text = "RC"
        Me.Screen_PCR_RCR.UseVisualStyleBackColor = True
        '
        'Screen_PCR_RCF
        '
        Me.Screen_PCR_RCF.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Screen_PCR_RCF.Location = New System.Drawing.Point(220, 8)
        Me.Screen_PCR_RCF.Name = "Screen_PCR_RCF"
        Me.Screen_PCR_RCF.Size = New System.Drawing.Size(34, 23)
        Me.Screen_PCR_RCF.TabIndex = 18
        Me.Screen_PCR_RCF.Text = "RC"
        Me.Screen_PCR_RCF.UseVisualStyleBackColor = True
        '
        'Screen_PCR_R
        '
        Me.Screen_PCR_R.Location = New System.Drawing.Point(14, 37)
        Me.Screen_PCR_R.Name = "Screen_PCR_R"
        Me.Screen_PCR_R.Size = New System.Drawing.Size(200, 21)
        Me.Screen_PCR_R.TabIndex = 16
        '
        'Screen_PCR_F
        '
        Me.Screen_PCR_F.Location = New System.Drawing.Point(14, 10)
        Me.Screen_PCR_F.Name = "Screen_PCR_F"
        Me.Screen_PCR_F.Size = New System.Drawing.Size(200, 21)
        Me.Screen_PCR_F.TabIndex = 15
        '
        'Screen_PCR
        '
        Me.Screen_PCR.AutoSize = True
        Me.Screen_PCR.Location = New System.Drawing.Point(329, 17)
        Me.Screen_PCR.Name = "Screen_PCR"
        Me.Screen_PCR.Size = New System.Drawing.Size(41, 16)
        Me.Screen_PCR.TabIndex = 7
        Me.Screen_PCR.Text = "PCR"
        Me.Screen_PCR.UseVisualStyleBackColor = True
        '
        'Screen_Freatures
        '
        Me.Screen_Freatures.AutoSize = True
        Me.Screen_Freatures.Checked = True
        Me.Screen_Freatures.Location = New System.Drawing.Point(23, 17)
        Me.Screen_Freatures.Name = "Screen_Freatures"
        Me.Screen_Freatures.Size = New System.Drawing.Size(71, 16)
        Me.Screen_Freatures.TabIndex = 7
        Me.Screen_Freatures.TabStop = True
        Me.Screen_Freatures.Text = "Features"
        Me.Screen_Freatures.UseVisualStyleBackColor = True
        '
        'Screen_Features_LinkLabel
        '
        Me.Screen_Features_LinkLabel.AutoSize = True
        Me.Screen_Features_LinkLabel.Location = New System.Drawing.Point(21, 47)
        Me.Screen_Features_LinkLabel.Name = "Screen_Features_LinkLabel"
        Me.Screen_Features_LinkLabel.Size = New System.Drawing.Size(107, 12)
        Me.Screen_Features_LinkLabel.TabIndex = 6
        Me.Screen_Features_LinkLabel.TabStop = True
        Me.Screen_Features_LinkLabel.Text = "[Click to select]"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 12)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 12)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Source"
        '
        'Prop_Operation
        '
        Me.Prop_Operation.AutoSize = True
        Me.Prop_Operation.Font = New System.Drawing.Font("ËÎÌו", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Prop_Operation.LinkColor = System.Drawing.Color.Red
        Me.Prop_Operation.Location = New System.Drawing.Point(69, 54)
        Me.Prop_Operation.Name = "Prop_Operation"
        Me.Prop_Operation.Size = New System.Drawing.Size(103, 12)
        Me.Prop_Operation.TabIndex = 2
        Me.Prop_Operation.TabStop = True
        Me.Prop_Operation.Text = "Operation/File"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(293, 25)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 12)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Type"
        '
        'Prop_Type
        '
        Me.Prop_Type.AutoSize = True
        Me.Prop_Type.Font = New System.Drawing.Font("ËÎÌו", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Prop_Type.LinkColor = System.Drawing.Color.Red
        Me.Prop_Type.Location = New System.Drawing.Point(348, 25)
        Me.Prop_Type.Name = "Prop_Type"
        Me.Prop_Type.Size = New System.Drawing.Size(89, 12)
        Me.Prop_Type.TabIndex = 2
        Me.Prop_Type.TabStop = True
        Me.Prop_Type.Text = "Mixture/Pure"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(293, 54)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(35, 12)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Count"
        '
        'Prop_Count
        '
        Me.Prop_Count.AutoSize = True
        Me.Prop_Count.Font = New System.Drawing.Font("ËÎÌו", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Prop_Count.LinkColor = System.Drawing.Color.Red
        Me.Prop_Count.Location = New System.Drawing.Point(348, 54)
        Me.Prop_Count.Name = "Prop_Count"
        Me.Prop_Count.Size = New System.Drawing.Size(47, 12)
        Me.Prop_Count.TabIndex = 2
        Me.Prop_Count.TabStop = True
        Me.Prop_Count.Text = "Number"
        '
        'btn_Cancel
        '
        Me.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_Cancel.Location = New System.Drawing.Point(324, 604)
        Me.btn_Cancel.Name = "btn_Cancel"
        Me.btn_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.btn_Cancel.TabIndex = 3
        Me.btn_Cancel.Text = "Cancel"
        Me.btn_Cancel.UseVisualStyleBackColor = True
        '
        'btn_OK
        '
        Me.btn_OK.Location = New System.Drawing.Point(430, 604)
        Me.btn_OK.Name = "btn_OK"
        Me.btn_OK.Size = New System.Drawing.Size(75, 23)
        Me.btn_OK.TabIndex = 3
        Me.btn_OK.Text = "OK"
        Me.btn_OK.UseVisualStyleBackColor = True
        '
        'LinkLabel5
        '
        Me.LinkLabel5.AutoSize = True
        Me.LinkLabel5.Location = New System.Drawing.Point(19, 47)
        Me.LinkLabel5.Name = "LinkLabel5"
        Me.LinkLabel5.Size = New System.Drawing.Size(107, 12)
        Me.LinkLabel5.TabIndex = 2
        Me.LinkLabel5.TabStop = True
        Me.LinkLabel5.Text = "[Click to select]"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(19, 76)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(53, 12)
        Me.Label14.TabIndex = 1
        Me.Label14.Text = "Products"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(19, 24)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(47, 12)
        Me.Label15.TabIndex = 1
        Me.Label15.Text = "Enzymes"
        '
        'ListView3
        '
        Me.ListView3.Location = New System.Drawing.Point(21, 91)
        Me.ListView3.Name = "ListView3"
        Me.ListView3.Size = New System.Drawing.Size(453, 263)
        Me.ListView3.TabIndex = 0
        Me.ListView3.UseCompatibleStateImageBehavior = False
        '
        'ofdGeneFile
        '
        Me.ofdGeneFile.Filter = "GeneBank Files|*.gb"
        '
        'Prop_Name
        '
        Me.Prop_Name.Location = New System.Drawing.Point(71, 22)
        Me.Prop_Name.Name = "Prop_Name"
        Me.Prop_Name.Size = New System.Drawing.Size(175, 21)
        Me.Prop_Name.TabIndex = 4
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(14, 205)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(71, 12)
        Me.Label16.TabIndex = 7
        Me.Label16.Text = "Product DNA"
        '
        'DNAView
        '
        Me.DNAView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ch_Name, Me.ch_Size, Me.cn_Cir, Me.ch_ENDS, Me.ch_Phos})
        Me.DNAView.LargeImageList = Me.LargeIconList
        Me.DNAView.Location = New System.Drawing.Point(16, 220)
        Me.DNAView.Name = "DNAView"
        Me.DNAView.Size = New System.Drawing.Size(497, 253)
        Me.DNAView.SmallImageList = Me.SmallIconList
        Me.DNAView.TabIndex = 6
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
        Me.ch_ENDS.Width = 120
        '
        'ch_Phos
        '
        Me.ch_Phos.Text = "Phosphate"
        Me.ch_Phos.Width = 120
        '
        'LargeIconList
        '
        Me.LargeIconList.ImageStream = CType(resources.GetObject("LargeIconList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.LargeIconList.TransparentColor = System.Drawing.Color.Transparent
        Me.LargeIconList.Images.SetKeyName(0, "DNA.png")
        Me.LargeIconList.Images.SetKeyName(1, "ENZ.png")
        Me.LargeIconList.Images.SetKeyName(2, "PCR.png")
        Me.LargeIconList.Images.SetKeyName(3, "MOD.png")
        Me.LargeIconList.Images.SetKeyName(4, "GEL.png")
        Me.LargeIconList.Images.SetKeyName(5, "LIG.png")
        Me.LargeIconList.Images.SetKeyName(6, "TSF.png")
        '
        'SmallIconList
        '
        Me.SmallIconList.ImageStream = CType(resources.GetObject("SmallIconList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.SmallIconList.TransparentColor = System.Drawing.Color.Transparent
        Me.SmallIconList.Images.SetKeyName(0, "DNA16.png")
        Me.SmallIconList.Images.SetKeyName(1, "ENZ16.png")
        Me.SmallIconList.Images.SetKeyName(2, "PCR16.png")
        Me.SmallIconList.Images.SetKeyName(3, "MOD16.png")
        Me.SmallIconList.Images.SetKeyName(4, "GEL16.png")
        Me.SmallIconList.Images.SetKeyName(5, "LIG16.png")
        Me.SmallIconList.Images.SetKeyName(6, "TSF16.png")
        '
        'll_ViewLarge
        '
        Me.ll_ViewLarge.AutoSize = True
        Me.ll_ViewLarge.Location = New System.Drawing.Point(381, 205)
        Me.ll_ViewLarge.Name = "ll_ViewLarge"
        Me.ll_ViewLarge.Size = New System.Drawing.Size(59, 12)
        Me.ll_ViewLarge.TabIndex = 8
        Me.ll_ViewLarge.TabStop = True
        Me.ll_ViewLarge.Text = "LargeIcon"
        '
        'll_ViewDetails
        '
        Me.ll_ViewDetails.AutoSize = True
        Me.ll_ViewDetails.Location = New System.Drawing.Point(458, 205)
        Me.ll_ViewDetails.Name = "ll_ViewDetails"
        Me.ll_ViewDetails.Size = New System.Drawing.Size(47, 12)
        Me.ll_ViewDetails.TabIndex = 8
        Me.ll_ViewDetails.TabStop = True
        Me.ll_ViewDetails.Text = "Details"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(14, 476)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(131, 12)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Operation Description"
        '
        'rtb_Description
        '
        Me.rtb_Description.BackColor = System.Drawing.Color.Cornsilk
        Me.rtb_Description.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtb_Description.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtb_Description.Location = New System.Drawing.Point(16, 491)
        Me.rtb_Description.Name = "rtb_Description"
        Me.rtb_Description.Size = New System.Drawing.Size(497, 96)
        Me.rtb_Description.TabIndex = 10
        Me.rtb_Description.Text = ""
        '
        'frmProperty
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(529, 639)
        Me.Controls.Add(Me.rtb_Description)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.ll_ViewDetails)
        Me.Controls.Add(Me.ll_ViewLarge)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.DNAView)
        Me.Controls.Add(Me.Prop_Name)
        Me.Controls.Add(Me.btn_OK)
        Me.Controls.Add(Me.btn_Cancel)
        Me.Controls.Add(Me.Prop_Count)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Prop_Type)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Prop_Operation)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TabControl_Operation)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmProperty"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Property"
        Me.TabControl_Operation.ResumeLayout(False)
        Me.tpFile.ResumeLayout(False)
        Me.tpFile.PerformLayout()
        Me.tpEnzyme.ResumeLayout(False)
        Me.tpEnzyme.PerformLayout()
        Me.tpPCR.ResumeLayout(False)
        Me.tpPCR.PerformLayout()
        Me.tpModify.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tpGel.ResumeLayout(False)
        Me.tpGel.PerformLayout()
        CType(Me.Gel_Maximum_Number, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Gel_Minimum_Number, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpLigation.ResumeLayout(False)
        Me.tpLigation.PerformLayout()
        Me.tpScreen.ResumeLayout(False)
        Me.tpScreen.PerformLayout()
        Me.Screen_PCR_Panel.ResumeLayout(False)
        Me.Screen_PCR_Panel.PerformLayout()
        CType(Me.Screen_PCR_nudMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Screen_PCR_nudMin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TabControl_Operation As System.Windows.Forms.TabControl
    Friend WithEvents tpEnzyme As System.Windows.Forms.TabPage
    Friend WithEvents tpPCR As System.Windows.Forms.TabPage
    Friend WithEvents tpGel As System.Windows.Forms.TabPage
    Friend WithEvents tpModify As System.Windows.Forms.TabPage
    Friend WithEvents tpScreen As System.Windows.Forms.TabPage
    Friend WithEvents tpFile As System.Windows.Forms.TabPage
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Prop_Operation As System.Windows.Forms.LinkLabel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Prop_Type As System.Windows.Forms.LinkLabel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Prop_Count As System.Windows.Forms.LinkLabel
    Friend WithEvents File_FileName_LinkLabel As System.Windows.Forms.LinkLabel
    Friend WithEvents PCR_ReversePrimer_TextBox As System.Windows.Forms.TextBox
    Friend WithEvents PCR_ForwardPrimer_TextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Gel_Maximum_Number As System.Windows.Forms.NumericUpDown
    Friend WithEvents Gel_Minimum_Number As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents btn_Cancel As System.Windows.Forms.Button
    Friend WithEvents btn_OK As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Modify_PNK As System.Windows.Forms.RadioButton
    Friend WithEvents Modify_CIAP As System.Windows.Forms.RadioButton
    Friend WithEvents Modify_Klewnow As System.Windows.Forms.RadioButton
    Friend WithEvents Modify_T4 As System.Windows.Forms.RadioButton
    Friend WithEvents Enzyme_Enzymes_LinkLabel As System.Windows.Forms.LinkLabel
    Friend WithEvents Screen_Features_LinkLabel As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel5 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents ListView3 As System.Windows.Forms.ListView
    Friend WithEvents File_Path_Label As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents ofdGeneFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents tpLigation As System.Windows.Forms.TabPage
    Friend WithEvents Prop_Name As System.Windows.Forms.TextBox
    Friend WithEvents Ligation_TriFrag As System.Windows.Forms.CheckBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents DNAView As System.Windows.Forms.ListView
    Friend WithEvents LargeIconList As System.Windows.Forms.ImageList
    Friend WithEvents SmallIconList As System.Windows.Forms.ImageList
    Friend WithEvents ch_Name As System.Windows.Forms.ColumnHeader
    Friend WithEvents ch_Size As System.Windows.Forms.ColumnHeader
    Friend WithEvents cn_Cir As System.Windows.Forms.ColumnHeader
    Friend WithEvents ch_ENDS As System.Windows.Forms.ColumnHeader
    Friend WithEvents ch_Phos As System.Windows.Forms.ColumnHeader
    Friend WithEvents ll_ViewLarge As System.Windows.Forms.LinkLabel
    Friend WithEvents ll_ViewDetails As System.Windows.Forms.LinkLabel
    Friend WithEvents btn_RCR As System.Windows.Forms.Button
    Friend WithEvents btn_RCF As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents rtb_Description As System.Windows.Forms.RichTextBox
    Friend WithEvents Screen_PCR As System.Windows.Forms.RadioButton
    Friend WithEvents Screen_Freatures As System.Windows.Forms.RadioButton
    Friend WithEvents Screen_PCR_Panel As System.Windows.Forms.Panel
    Friend WithEvents Screen_PCR_nudMax As System.Windows.Forms.NumericUpDown
    Friend WithEvents Screen_PCR_nudMin As System.Windows.Forms.NumericUpDown
    Friend WithEvents Screen_PCR_lblMax As System.Windows.Forms.Label
    Friend WithEvents Screen_PCR_lblMin As System.Windows.Forms.Label
    Friend WithEvents Screen_PCR_RCR As System.Windows.Forms.Button
    Friend WithEvents Screen_PCR_RCF As System.Windows.Forms.Button
    Friend WithEvents Screen_PCR_R As System.Windows.Forms.TextBox
    Friend WithEvents Screen_PCR_F As System.Windows.Forms.TextBox
    Friend WithEvents cbML As System.Windows.Forms.CheckBox
End Class
