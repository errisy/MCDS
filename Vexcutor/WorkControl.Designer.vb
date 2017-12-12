<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WorkControl
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WorkControl))
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.lv_Chart = New MCDS.OperationView()
        Me.pvMain = New MCDS.PropertyView()
        Me.cms_ChartItem = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmCopyGroup = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmPasteGroup = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopySelectedVectorMapDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopySelectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewVToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopySequenceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetConstructionDescriptionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConvertToFreeDesignToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConvertToVectorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConvertToHostToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewTmBToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AnalyzeCDSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.EnzymeAnalysisAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EnzymeDigestionEToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GelSeparationGToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HashPickHToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LigationLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MergeXToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModificationMToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PCRRToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RecombinationCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TransformationScreenTToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FreeDesignFToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SequencingSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CompareToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tssOperations = New System.Windows.Forms.ToolStripSeparator()
        Me.RecalculateAllChildrenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tssRecalculate = New System.Windows.Forms.ToolStripSeparator()
        Me.AutoArragneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AutoFitChildrenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StepFitChildrenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemarkFeaturesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExportSelectionWToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportAllZToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportSelectedVectorJToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.sfdProject = New System.Windows.Forms.SaveFileDialog()
        Me.ofdGeneFile = New System.Windows.Forms.OpenFileDialog()
        Me.ofdSequence = New System.Windows.Forms.OpenFileDialog()
        Me.ofdSequencingResult = New System.Windows.Forms.OpenFileDialog()
        Me.sfdExport = New System.Windows.Forms.SaveFileDialog()
        Me.sfdEMF = New System.Windows.Forms.SaveFileDialog()
        Me.sfdGeneBank = New System.Windows.Forms.SaveFileDialog()
        CType(Me.scMain,System.ComponentModel.ISupportInitialize).BeginInit
        Me.scMain.Panel1.SuspendLayout
        Me.scMain.Panel2.SuspendLayout
        Me.scMain.SuspendLayout
        Me.cms_ChartItem.SuspendLayout
        Me.SuspendLayout
        '
        'scMain
        '
        Me.scMain.BackColor = System.Drawing.Color.Lavender
        Me.scMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scMain.Location = New System.Drawing.Point(0, 0)
        Me.scMain.Name = "scMain"
        Me.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.lv_Chart)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.pvMain)
        Me.scMain.Size = New System.Drawing.Size(968, 505)
        Me.scMain.SplitterDistance = 362
        Me.scMain.TabIndex = 4
        '
        'lv_Chart
        '
        Me.lv_Chart.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lv_Chart.Location = New System.Drawing.Point(0, 0)
        Me.lv_Chart.Name = "lv_Chart"
        Me.lv_Chart.Offset = CType(resources.GetObject("lv_Chart.Offset"),System.Drawing.PointF)
        Me.lv_Chart.ScaleValue = 1!
        Me.lv_Chart.Size = New System.Drawing.Size(968, 362)
        Me.lv_Chart.SourceMode = false
        Me.lv_Chart.TabIndex = 0
        '
        'pvMain
        '
        Me.pvMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pvMain.Location = New System.Drawing.Point(0, 0)
        Me.pvMain.Name = "pvMain"
        Me.pvMain.SelectItem = Nothing
        Me.pvMain.Size = New System.Drawing.Size(968, 139)
        Me.pvMain.TabIndex = 0
        '
        'cms_ChartItem
        '
        Me.cms_ChartItem.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmCopyGroup, Me.tsmPasteGroup, Me.CopySelectedVectorMapDToolStripMenuItem, Me.CopySelectionToolStripMenuItem, Me.ViewVToolStripMenuItem, Me.CopySequenceToolStripMenuItem, Me.GetConstructionDescriptionToolStripMenuItem, Me.ConvertToFreeDesignToolStripMenuItem, Me.ConvertToVectorToolStripMenuItem, Me.ConvertToHostToolStripMenuItem, Me.ViewTmBToolStripMenuItem, Me.AnalyzeCDSToolStripMenuItem, Me.ToolStripSeparator1, Me.EnzymeAnalysisAToolStripMenuItem, Me.EnzymeDigestionEToolStripMenuItem, Me.GelSeparationGToolStripMenuItem, Me.HashPickHToolStripMenuItem, Me.LigationLToolStripMenuItem, Me.MergeXToolStripMenuItem, Me.ModificationMToolStripMenuItem, Me.PCRRToolStripMenuItem, Me.RecombinationCToolStripMenuItem, Me.TransformationScreenTToolStripMenuItem, Me.FreeDesignFToolStripMenuItem, Me.SequencingSToolStripMenuItem, Me.CompareToolStripMenuItem, Me.tssOperations, Me.RecalculateAllChildrenToolStripMenuItem, Me.tssRecalculate, Me.AutoArragneToolStripMenuItem, Me.AutoFitChildrenToolStripMenuItem, Me.StepFitChildrenToolStripMenuItem, Me.RemarkFeaturesToolStripMenuItem, Me.ToolStripSeparator4, Me.ExportSelectionWToolStripMenuItem, Me.ExportAllZToolStripMenuItem, Me.ExportToolStripMenuItem, Me.ExportSelectedVectorJToolStripMenuItem})
        Me.cms_ChartItem.Name = "cms_ChartItem"
        Me.cms_ChartItem.Size = New System.Drawing.Size(250, 754)
        '
        'tsmCopyGroup
        '
        Me.tsmCopyGroup.Name = "tsmCopyGroup"
        Me.tsmCopyGroup.Size = New System.Drawing.Size(249, 22)
        Me.tsmCopyGroup.Text = "Copy Group"
        '
        'tsmPasteGroup
        '
        Me.tsmPasteGroup.Name = "tsmPasteGroup"
        Me.tsmPasteGroup.Size = New System.Drawing.Size(249, 22)
        Me.tsmPasteGroup.Text = "Paste Group"
        '
        'CopySelectedVectorMapDToolStripMenuItem
        '
        Me.CopySelectedVectorMapDToolStripMenuItem.Name = "CopySelectedVectorMapDToolStripMenuItem"
        Me.CopySelectedVectorMapDToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.CopySelectedVectorMapDToolStripMenuItem.Text = "Copy Selected Vector Map(&D)"
        '
        'CopySelectionToolStripMenuItem
        '
        Me.CopySelectionToolStripMenuItem.Name = "CopySelectionToolStripMenuItem"
        Me.CopySelectionToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.CopySelectionToolStripMenuItem.Text = "Copy EMF"
        '
        'ViewVToolStripMenuItem
        '
        Me.ViewVToolStripMenuItem.Name = "ViewVToolStripMenuItem"
        Me.ViewVToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.ViewVToolStripMenuItem.Text = "View(&V)"
        '
        'CopySequenceToolStripMenuItem
        '
        Me.CopySequenceToolStripMenuItem.Name = "CopySequenceToolStripMenuItem"
        Me.CopySequenceToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.CopySequenceToolStripMenuItem.Text = "Copy Sequence"
        '
        'GetConstructionDescriptionToolStripMenuItem
        '
        Me.GetConstructionDescriptionToolStripMenuItem.Name = "GetConstructionDescriptionToolStripMenuItem"
        Me.GetConstructionDescriptionToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.GetConstructionDescriptionToolStripMenuItem.Text = "Get Construction Description"
        '
        'ConvertToFreeDesignToolStripMenuItem
        '
        Me.ConvertToFreeDesignToolStripMenuItem.Name = "ConvertToFreeDesignToolStripMenuItem"
        Me.ConvertToFreeDesignToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.ConvertToFreeDesignToolStripMenuItem.Text = "Convert to Free Design"
        '
        'ConvertToVectorToolStripMenuItem
        '
        Me.ConvertToVectorToolStripMenuItem.Name = "ConvertToVectorToolStripMenuItem"
        Me.ConvertToVectorToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.ConvertToVectorToolStripMenuItem.Text = "Convert To Vector"
        '
        'ConvertToHostToolStripMenuItem
        '
        Me.ConvertToHostToolStripMenuItem.Name = "ConvertToHostToolStripMenuItem"
        Me.ConvertToHostToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.ConvertToHostToolStripMenuItem.Text = "Convert To Host"
        '
        'ViewTmBToolStripMenuItem
        '
        Me.ViewTmBToolStripMenuItem.Name = "ViewTmBToolStripMenuItem"
        Me.ViewTmBToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.ViewTmBToolStripMenuItem.Text = "View Tm(B)"
        '
        'AnalyzeCDSToolStripMenuItem
        '
        Me.AnalyzeCDSToolStripMenuItem.Name = "AnalyzeCDSToolStripMenuItem"
        Me.AnalyzeCDSToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.AnalyzeCDSToolStripMenuItem.Text = "Analyze CDS(S)"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(246, 6)
        '
        'EnzymeAnalysisAToolStripMenuItem
        '
        Me.EnzymeAnalysisAToolStripMenuItem.Name = "EnzymeAnalysisAToolStripMenuItem"
        Me.EnzymeAnalysisAToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.EnzymeAnalysisAToolStripMenuItem.Tag = "8"
        Me.EnzymeAnalysisAToolStripMenuItem.Text = "Enzyme Analysis...(&A)"
        '
        'EnzymeDigestionEToolStripMenuItem
        '
        Me.EnzymeDigestionEToolStripMenuItem.Name = "EnzymeDigestionEToolStripMenuItem"
        Me.EnzymeDigestionEToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.EnzymeDigestionEToolStripMenuItem.Tag = "1"
        Me.EnzymeDigestionEToolStripMenuItem.Text = "Enzyme Digestion...(&E)"
        '
        'GelSeparationGToolStripMenuItem
        '
        Me.GelSeparationGToolStripMenuItem.Name = "GelSeparationGToolStripMenuItem"
        Me.GelSeparationGToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.GelSeparationGToolStripMenuItem.Tag = "4"
        Me.GelSeparationGToolStripMenuItem.Text = "Gel Separation...(&G)"
        '
        'HashPickHToolStripMenuItem
        '
        Me.HashPickHToolStripMenuItem.Name = "HashPickHToolStripMenuItem"
        Me.HashPickHToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.HashPickHToolStripMenuItem.Tag = "11"
        Me.HashPickHToolStripMenuItem.Text = "Hash Pick...(H)"
        '
        'LigationLToolStripMenuItem
        '
        Me.LigationLToolStripMenuItem.Name = "LigationLToolStripMenuItem"
        Me.LigationLToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.LigationLToolStripMenuItem.Tag = "5"
        Me.LigationLToolStripMenuItem.Text = "Ligation...(&L)"
        '
        'MergeXToolStripMenuItem
        '
        Me.MergeXToolStripMenuItem.Name = "MergeXToolStripMenuItem"
        Me.MergeXToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.MergeXToolStripMenuItem.Tag = "9"
        Me.MergeXToolStripMenuItem.Text = "Merge...(&X)"
        '
        'ModificationMToolStripMenuItem
        '
        Me.ModificationMToolStripMenuItem.Name = "ModificationMToolStripMenuItem"
        Me.ModificationMToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.ModificationMToolStripMenuItem.Tag = "3"
        Me.ModificationMToolStripMenuItem.Text = "Modification...(&M)"
        '
        'PCRRToolStripMenuItem
        '
        Me.PCRRToolStripMenuItem.Name = "PCRRToolStripMenuItem"
        Me.PCRRToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.PCRRToolStripMenuItem.Tag = "2"
        Me.PCRRToolStripMenuItem.Text = "PCR...(&P)"
        '
        'RecombinationCToolStripMenuItem
        '
        Me.RecombinationCToolStripMenuItem.Name = "RecombinationCToolStripMenuItem"
        Me.RecombinationCToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.RecombinationCToolStripMenuItem.Tag = "7"
        Me.RecombinationCToolStripMenuItem.Text = "Recombination...(&R)"
        '
        'TransformationScreenTToolStripMenuItem
        '
        Me.TransformationScreenTToolStripMenuItem.Name = "TransformationScreenTToolStripMenuItem"
        Me.TransformationScreenTToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.TransformationScreenTToolStripMenuItem.Tag = "6"
        Me.TransformationScreenTToolStripMenuItem.Text = "Screen...(&T)"
        '
        'FreeDesignFToolStripMenuItem
        '
        Me.FreeDesignFToolStripMenuItem.Name = "FreeDesignFToolStripMenuItem"
        Me.FreeDesignFToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.FreeDesignFToolStripMenuItem.Tag = "10"
        Me.FreeDesignFToolStripMenuItem.Text = "Free Design...(&F)"
        '
        'SequencingSToolStripMenuItem
        '
        Me.SequencingSToolStripMenuItem.Name = "SequencingSToolStripMenuItem"
        Me.SequencingSToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.SequencingSToolStripMenuItem.Tag = "12"
        Me.SequencingSToolStripMenuItem.Text = "Sequencing...(&S)"
        '
        'CompareToolStripMenuItem
        '
        Me.CompareToolStripMenuItem.Name = "CompareToolStripMenuItem"
        Me.CompareToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.CompareToolStripMenuItem.Tag = "13"
        Me.CompareToolStripMenuItem.Text = "Compare...(&C)"
        '
        'tssOperations
        '
        Me.tssOperations.Name = "tssOperations"
        Me.tssOperations.Size = New System.Drawing.Size(246, 6)
        '
        'RecalculateAllChildrenToolStripMenuItem
        '
        Me.RecalculateAllChildrenToolStripMenuItem.Name = "RecalculateAllChildrenToolStripMenuItem"
        Me.RecalculateAllChildrenToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.RecalculateAllChildrenToolStripMenuItem.Text = "Recalculate All Children"
        '
        'tssRecalculate
        '
        Me.tssRecalculate.Name = "tssRecalculate"
        Me.tssRecalculate.Size = New System.Drawing.Size(246, 6)
        '
        'AutoArragneToolStripMenuItem
        '
        Me.AutoArragneToolStripMenuItem.Name = "AutoArragneToolStripMenuItem"
        Me.AutoArragneToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.AutoArragneToolStripMenuItem.Text = "Auto Arrange"
        '
        'AutoFitChildrenToolStripMenuItem
        '
        Me.AutoFitChildrenToolStripMenuItem.Name = "AutoFitChildrenToolStripMenuItem"
        Me.AutoFitChildrenToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.AutoFitChildrenToolStripMenuItem.Text = "Auto Fit Children"
        '
        'StepFitChildrenToolStripMenuItem
        '
        Me.StepFitChildrenToolStripMenuItem.Name = "StepFitChildrenToolStripMenuItem"
        Me.StepFitChildrenToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.StepFitChildrenToolStripMenuItem.Text = "Step Fit Children"
        '
        'RemarkFeaturesToolStripMenuItem
        '
        Me.RemarkFeaturesToolStripMenuItem.Name = "RemarkFeaturesToolStripMenuItem"
        Me.RemarkFeaturesToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.RemarkFeaturesToolStripMenuItem.Text = "Remark Features"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(246, 6)
        '
        'ExportSelectionWToolStripMenuItem
        '
        Me.ExportSelectionWToolStripMenuItem.Name = "ExportSelectionWToolStripMenuItem"
        Me.ExportSelectionWToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.ExportSelectionWToolStripMenuItem.Text = "Export Selection(&W)"
        '
        'ExportAllZToolStripMenuItem
        '
        Me.ExportAllZToolStripMenuItem.Name = "ExportAllZToolStripMenuItem"
        Me.ExportAllZToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.ExportAllZToolStripMenuItem.Text = "Export All(&Z)"
        '
        'ExportToolStripMenuItem
        '
        Me.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem"
        Me.ExportToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.ExportToolStripMenuItem.Text = "Export Genebank/Fasta File(&U)"
        '
        'ExportSelectedVectorJToolStripMenuItem
        '
        Me.ExportSelectedVectorJToolStripMenuItem.Name = "ExportSelectedVectorJToolStripMenuItem"
        Me.ExportSelectedVectorJToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.ExportSelectedVectorJToolStripMenuItem.Text = "Export Selected Vector(&J)"
        '
        'sfdProject
        '
        Me.sfdProject.Filter = "Vecute Files|*.vxt"
        '
        'ofdGeneFile
        '
        Me.ofdGeneFile.Filter = "DNA Files|*.vct;*.gb;"
        '
        'ofdSequence
        '
        Me.ofdSequence.Filter = "Sequence Files|*.txt;*.seq"
        '
        'ofdSequencingResult
        '
        Me.ofdSequencingResult.Filter = "Sequencing Result|*.ab1;"
        Me.ofdSequencingResult.Multiselect = True
        '
        'sfdExport
        '
        Me.sfdExport.Filter = "TEXT File|*.txt"
        '
        'sfdEMF
        '
        Me.sfdEMF.Filter = "EMF File|*.emf"
        '
        'sfdGeneBank
        '
        Me.sfdGeneBank.Filter = "GeneBank File|*.gb|FASTA File|*.fasta"
        '
        'WorkControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.scMain)
        Me.Name = "WorkControl"
        Me.Size = New System.Drawing.Size(968, 505)
        Me.scMain.Panel1.ResumeLayout(false)
        Me.scMain.Panel2.ResumeLayout(false)
        CType(Me.scMain,System.ComponentModel.ISupportInitialize).EndInit
        Me.scMain.ResumeLayout(false)
        Me.cms_ChartItem.ResumeLayout(false)
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents scMain As System.Windows.Forms.SplitContainer
    Friend WithEvents pvMain As MCDS.PropertyView
    Friend WithEvents sfdProject As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ofdGeneFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents cms_ChartItem As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ViewVToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents EnzymeDigestionEToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PCRRToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ModificationMToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GelSeparationGToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LigationLToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TransformationScreenTToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RecombinationCToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EnzymeAnalysisAToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MergeXToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ofdSequence As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ofdSequencingResult As System.Windows.Forms.OpenFileDialog
    Friend WithEvents sfdExport As System.Windows.Forms.SaveFileDialog
    Friend WithEvents tssOperations As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents RecalculateAllChildrenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lv_Chart As MCDS.OperationView
    Friend WithEvents tssRecalculate As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AutoArragneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AutoFitChildrenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StepFitChildrenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmCopyGroup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmPasteGroup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopySequenceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemarkFeaturesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExportSelectionWToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sfdEMF As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ExportAllZToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewTmBToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AnalyzeCDSToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopySelectedVectorMapDToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportSelectedVectorJToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HashPickHToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FreeDesignFToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SequencingSToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CompareToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetConstructionDescriptionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConvertToFreeDesignToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConvertToVectorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConvertToHostToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopySelectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sfdGeneBank As System.Windows.Forms.SaveFileDialog

End Class
