<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FeatureManageView
    Inherits System.Windows.Forms.UserControl

    'UserControl 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FeatureManageView))
        Me.dgvFeat = New System.Windows.Forms.DataGridView()
        Me.cLabel = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cnType = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.cnNote = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cnID = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.cnLength = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cnSequence = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tspMain = New System.Windows.Forms.ToolStrip()
        Me.tsb_Add = New System.Windows.Forms.ToolStripButton()
        Me.tsb_Delete = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.tsbRemoveNative = New System.Windows.Forms.ToolStripButton()
        Me.tsbSetLocal = New System.Windows.Forms.ToolStripButton()
        Me.tsbSetDelete = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbIncludeStandard = New System.Windows.Forms.ToolStripButton()
        Me.tsbSaveStandard = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbLoad = New System.Windows.Forms.ToolStripButton()
        Me.tsbSave = New System.Windows.Forms.ToolStripButton()
        Me.tsbExport = New System.Windows.Forms.ToolStripButton()
        Me.tsbCopyFeature = New System.Windows.Forms.ToolStripButton()
        Me.tsbPasteFeature = New System.Windows.Forms.ToolStripButton()
        Me.tscbKEGGOrganism = New System.Windows.Forms.ToolStripComboBox()
        Me.tstbGene = New System.Windows.Forms.ToolStripTextBox()
        Me.tsbKEGGFeature = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsb_OK = New System.Windows.Forms.ToolStripButton()
        Me.tsbCancel = New System.Windows.Forms.ToolStripButton()
        Me.ofdFeature = New System.Windows.Forms.OpenFileDialog()
        Me.sfdFeature = New System.Windows.Forms.SaveFileDialog()
        Me.sfdFASTA = New System.Windows.Forms.SaveFileDialog()
        CType(Me.dgvFeat, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tspMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvFeat
        '
        Me.dgvFeat.AllowUserToAddRows = False
        Me.dgvFeat.AllowUserToDeleteRows = False
        Me.dgvFeat.AllowUserToResizeRows = False
        Me.dgvFeat.BackgroundColor = System.Drawing.Color.White
        Me.dgvFeat.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.SeaShell
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.OliveDrab
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvFeat.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvFeat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFeat.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.cLabel, Me.cnType, Me.cnNote, Me.cnID, Me.cnLength, Me.cnSequence, Me.Column1})
        Me.dgvFeat.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvFeat.GridColor = System.Drawing.Color.OrangeRed
        Me.dgvFeat.Location = New System.Drawing.Point(0, 25)
        Me.dgvFeat.Name = "dgvFeat"
        Me.dgvFeat.RowTemplate.Height = 23
        Me.dgvFeat.Size = New System.Drawing.Size(1089, 397)
        Me.dgvFeat.TabIndex = 4
        '
        'cLabel
        '
        Me.cLabel.HeaderText = "Label"
        Me.cLabel.Name = "cLabel"
        Me.cLabel.Width = 150
        '
        'cnType
        '
        Me.cnType.HeaderText = "Type"
        Me.cnType.Items.AddRange(New Object() {"CDS", "exon", "enhancer", "gene", "loci", "misc_feature", "misc_signal", "mutation", "operon", "oriT", "primer_bind", "promoter", "RBS", "recombination_site", "rep_origin", "select_mark", "terminator"})
        Me.cnType.Name = "cnType"
        '
        'cnNote
        '
        Me.cnNote.HeaderText = "Note"
        Me.cnNote.Name = "cnNote"
        Me.cnNote.Width = 150
        '
        'cnID
        '
        Me.cnID.HeaderText = "Source"
        Me.cnID.Items.AddRange(New Object() {"Native", "Local", "Standard", "Delete"})
        Me.cnID.Name = "cnID"
        Me.cnID.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cnID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.cnID.Width = 80
        '
        'cnLength
        '
        Me.cnLength.HeaderText = "Length"
        Me.cnLength.Name = "cnLength"
        Me.cnLength.ReadOnly = True
        '
        'cnSequence
        '
        Me.cnSequence.HeaderText = "Sequence"
        Me.cnSequence.Name = "cnSequence"
        Me.cnSequence.Width = 300
        '
        'Column1
        '
        Me.Column1.HeaderText = "Function"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 160
        '
        'tspMain
        '
        Me.tspMain.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tspMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsb_Add, Me.tsb_Delete, Me.ToolStripButton1, Me.tsbRemoveNative, Me.tsbSetLocal, Me.tsbSetDelete, Me.ToolStripSeparator1, Me.tsbIncludeStandard, Me.tsbSaveStandard, Me.ToolStripSeparator3, Me.tsbLoad, Me.tsbSave, Me.tsbExport, Me.tsbCopyFeature, Me.tsbPasteFeature, Me.tscbKEGGOrganism, Me.tstbGene, Me.tsbKEGGFeature, Me.ToolStripSeparator2, Me.tsb_OK, Me.tsbCancel})
        Me.tspMain.Location = New System.Drawing.Point(0, 0)
        Me.tspMain.Name = "tspMain"
        Me.tspMain.Size = New System.Drawing.Size(1089, 25)
        Me.tspMain.TabIndex = 3
        Me.tspMain.Text = "ToolStrip1"
        '
        'tsb_Add
        '
        Me.tsb_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsb_Add.Image = CType(resources.GetObject("tsb_Add.Image"), System.Drawing.Image)
        Me.tsb_Add.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsb_Add.Name = "tsb_Add"
        Me.tsb_Add.Size = New System.Drawing.Size(23, 22)
        Me.tsb_Add.Text = "+"
        '
        'tsb_Delete
        '
        Me.tsb_Delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsb_Delete.Image = CType(resources.GetObject("tsb_Delete.Image"), System.Drawing.Image)
        Me.tsb_Delete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsb_Delete.Name = "tsb_Delete"
        Me.tsb_Delete.Size = New System.Drawing.Size(23, 22)
        Me.tsb_Delete.Text = "-"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(51, 22)
        Me.ToolStripButton1.Text = "Reduce"
        '
        'tsbRemoveNative
        '
        Me.tsbRemoveNative.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbRemoveNative.Image = CType(resources.GetObject("tsbRemoveNative.Image"), System.Drawing.Image)
        Me.tsbRemoveNative.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbRemoveNative.Name = "tsbRemoveNative"
        Me.tsbRemoveNative.Size = New System.Drawing.Size(49, 22)
        Me.tsbRemoveNative.Text = "-Native"
        '
        'tsbSetLocal
        '
        Me.tsbSetLocal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbSetLocal.Image = CType(resources.GetObject("tsbSetLocal.Image"), System.Drawing.Image)
        Me.tsbSetLocal.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSetLocal.Name = "tsbSetLocal"
        Me.tsbSetLocal.Size = New System.Drawing.Size(56, 22)
        Me.tsbSetLocal.Text = "SetLocal"
        '
        'tsbSetDelete
        '
        Me.tsbSetDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbSetDelete.Image = CType(resources.GetObject("tsbSetDelete.Image"), System.Drawing.Image)
        Me.tsbSetDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSetDelete.Name = "tsbSetDelete"
        Me.tsbSetDelete.Size = New System.Drawing.Size(65, 22)
        Me.tsbSetDelete.Text = "SetDelete"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsbIncludeStandard
        '
        Me.tsbIncludeStandard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbIncludeStandard.Image = CType(resources.GetObject("tsbIncludeStandard.Image"), System.Drawing.Image)
        Me.tsbIncludeStandard.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbIncludeStandard.Name = "tsbIncludeStandard"
        Me.tsbIncludeStandard.Size = New System.Drawing.Size(72, 22)
        Me.tsbIncludeStandard.Text = "Standard>>"
        '
        'tsbSaveStandard
        '
        Me.tsbSaveStandard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbSaveStandard.Image = CType(resources.GetObject("tsbSaveStandard.Image"), System.Drawing.Image)
        Me.tsbSaveStandard.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSaveStandard.Name = "tsbSaveStandard"
        Me.tsbSaveStandard.Size = New System.Drawing.Size(72, 22)
        Me.tsbSaveStandard.Text = ">>Standard"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'tsbLoad
        '
        Me.tsbLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbLoad.Image = CType(resources.GetObject("tsbLoad.Image"), System.Drawing.Image)
        Me.tsbLoad.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbLoad.Name = "tsbLoad"
        Me.tsbLoad.Size = New System.Drawing.Size(37, 22)
        Me.tsbLoad.Text = "Load"
        '
        'tsbSave
        '
        Me.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbSave.Image = CType(resources.GetObject("tsbSave.Image"), System.Drawing.Image)
        Me.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSave.Name = "tsbSave"
        Me.tsbSave.Size = New System.Drawing.Size(36, 22)
        Me.tsbSave.Text = "Save"
        '
        'tsbExport
        '
        Me.tsbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbExport.Image = CType(resources.GetObject("tsbExport.Image"), System.Drawing.Image)
        Me.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbExport.Name = "tsbExport"
        Me.tsbExport.Size = New System.Drawing.Size(44, 22)
        Me.tsbExport.Text = "Export"
        '
        'tsbCopyFeature
        '
        Me.tsbCopyFeature.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbCopyFeature.Image = CType(resources.GetObject("tsbCopyFeature.Image"), System.Drawing.Image)
        Me.tsbCopyFeature.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCopyFeature.Name = "tsbCopyFeature"
        Me.tsbCopyFeature.Size = New System.Drawing.Size(36, 22)
        Me.tsbCopyFeature.Text = "Copy"
        '
        'tsbPasteFeature
        '
        Me.tsbPasteFeature.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbPasteFeature.Image = CType(resources.GetObject("tsbPasteFeature.Image"), System.Drawing.Image)
        Me.tsbPasteFeature.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPasteFeature.Name = "tsbPasteFeature"
        Me.tsbPasteFeature.Size = New System.Drawing.Size(41, 22)
        Me.tsbPasteFeature.Text = "Paste"
        '
        'tscbKEGGOrganism
        '
        Me.tscbKEGGOrganism.Name = "tscbKEGGOrganism"
        Me.tscbKEGGOrganism.Size = New System.Drawing.Size(75, 25)
        Me.tscbKEGGOrganism.Visible = False
        '
        'tstbGene
        '
        Me.tstbGene.Name = "tstbGene"
        Me.tstbGene.Size = New System.Drawing.Size(80, 25)
        Me.tstbGene.Visible = False
        '
        'tsbKEGGFeature
        '
        Me.tsbKEGGFeature.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbKEGGFeature.Image = CType(resources.GetObject("tsbKEGGFeature.Image"), System.Drawing.Image)
        Me.tsbKEGGFeature.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbKEGGFeature.Name = "tsbKEGGFeature"
        Me.tsbKEGGFeature.Size = New System.Drawing.Size(39, 22)
        Me.tsbKEGGFeature.Text = "KEGG"
        Me.tsbKEGGFeature.Visible = False
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'tsb_OK
        '
        Me.tsb_OK.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsb_OK.Image = CType(resources.GetObject("tsb_OK.Image"), System.Drawing.Image)
        Me.tsb_OK.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsb_OK.Name = "tsb_OK"
        Me.tsb_OK.Size = New System.Drawing.Size(25, 22)
        Me.tsb_OK.Text = "OK"
        '
        'tsbCancel
        '
        Me.tsbCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbCancel.Image = CType(resources.GetObject("tsbCancel.Image"), System.Drawing.Image)
        Me.tsbCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCancel.Name = "tsbCancel"
        Me.tsbCancel.Size = New System.Drawing.Size(47, 22)
        Me.tsbCancel.Text = "Cancel"
        '
        'ofdFeature
        '
        Me.ofdFeature.Filter = "Feature File|*.ft"
        '
        'sfdFeature
        '
        Me.sfdFeature.Filter = "Feature File|*.ft"
        '
        'sfdFASTA
        '
        Me.sfdFASTA.Filter = "FASTA format|*.txt"
        '
        'FeatureManageView
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.dgvFeat)
        Me.Controls.Add(Me.tspMain)
        Me.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "FeatureManageView"
        Me.Size = New System.Drawing.Size(1089, 422)
        CType(Me.dgvFeat, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tspMain.ResumeLayout(False)
        Me.tspMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvFeat As System.Windows.Forms.DataGridView
    Friend WithEvents tspMain As System.Windows.Forms.ToolStrip
    Friend WithEvents tsb_Add As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsb_Delete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbIncludeStandard As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbLoad As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsb_OK As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSaveStandard As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ofdFeature As System.Windows.Forms.OpenFileDialog
    Friend WithEvents sfdFeature As System.Windows.Forms.SaveFileDialog
    Friend WithEvents tsbRemoveNative As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbExport As System.Windows.Forms.ToolStripButton
    Friend WithEvents sfdFASTA As System.Windows.Forms.SaveFileDialog
    Friend WithEvents tsbCopyFeature As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbPasteFeature As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSetLocal As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSetDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents tscbKEGGOrganism As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents tstbGene As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tsbKEGGFeature As System.Windows.Forms.ToolStripButton
    Friend WithEvents cLabel As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cnType As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents cnNote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cnID As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents cnLength As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cnSequence As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
