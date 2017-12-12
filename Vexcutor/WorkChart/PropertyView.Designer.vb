<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PropertyView
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
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tcMain = New MCDS.TabContainer()
        Me.TabPage1 = New MCDS.CustomTabPage()
        Me.llManageHosts = New System.Windows.Forms.LinkLabel()
        Me.llManagePrimers = New System.Windows.Forms.LinkLabel()
        Me.lbFileAddress = New System.Windows.Forms.Label()
        Me.rtbSummary = New System.Windows.Forms.TextBox()
        Me.lbEnzymes = New System.Windows.Forms.Label()
        Me.llLoadSequencingResultFile = New System.Windows.Forms.LinkLabel()
        Me.llLoadSequenceFile = New System.Windows.Forms.LinkLabel()
        Me.llClose = New System.Windows.Forms.LinkLabel()
        Me.llProjectSummary = New System.Windows.Forms.LinkLabel()
        Me.llExportProject = New System.Windows.Forms.LinkLabel()
        Me.llManageEnzymes = New System.Windows.Forms.LinkLabel()
        Me.llIncludeCommon = New System.Windows.Forms.LinkLabel()
        Me.llRemarkFeature = New System.Windows.Forms.LinkLabel()
        Me.llManageFeatures = New System.Windows.Forms.LinkLabel()
        Me.llLoadGeneFile = New System.Windows.Forms.LinkLabel()
        Me.TabPage2 = New MCDS.CustomTabPage()
        Me.PrpC = New MCDS.PropertyControl()
        Me.TabPage3 = New MCDS.CustomTabPage()
        Me.MIV = New MCDS.MultipleItemView()
        Me.TabPage4 = New MCDS.CustomTabPage()
        Me.reView = New MCDS.RestrictionEnzymeView()
        Me.TabPage5 = New MCDS.CustomTabPage()
        Me.gvDNA = New MCDS.GroupViewer()
        Me.TabPage6 = New MCDS.CustomTabPage()
        Me.gvPCR = New MCDS.GroupViewer()
        Me.TabPage7 = New MCDS.CustomTabPage()
        Me.FMV = New MCDS.FeatureManageView()
        Me.TabPage8 = New MCDS.CustomTabPage()
        Me.FSV = New MCDS.FeatureScreenView()
        Me.TabPage9 = New MCDS.CustomTabPage()
        Me.PPV = New MCDS.PrintPageView()
        Me.llPrintPageCount = New System.Windows.Forms.LinkLabel()
        Me.llDirectPrintAll = New System.Windows.Forms.LinkLabel()
        Me.llPrintAllPages = New System.Windows.Forms.LinkLabel()
        Me.llDirectPrintSel = New System.Windows.Forms.LinkLabel()
        Me.llPrintSelectedPages = New System.Windows.Forms.LinkLabel()
        Me.llAddPage = New System.Windows.Forms.LinkLabel()
        Me.TabPage10 = New MCDS.CustomTabPage()
        Me.ehSummary = New System.Windows.Forms.Integration.ElementHost()
        Me.wpfSummaryBox = New MCDS.wpfSummary()
        Me.TabPage11 = New MCDS.CustomTabPage()
        Me.PMV = New MCDS.PrimerManageView()
        Me.TabPage12 = New MCDS.CustomTabPage()
        Me.HMV = New MCDS.HostManageView()
        Me.tpPrimerEditorView = New MCDS.EditorTabpage()
        Me.mvPrimerEditorView = New MCDS.PrimerEditorView()
        Me.tpSequenceEditorView = New MCDS.EditorTabpage()
        Me.mvSequenceEditorView = New MCDS.SequenceEditorView()
        Me.tpSequencing = New MCDS.CustomTabPage()
        Me.ihSequencing = New MCDS.InteropHost()
        Me.wpfSequencings = New MCDS.MultipleSequencingResults()
        Me.tpGelImage = New MCDS.CustomTabPage()
        Me.ihGelImage = New MCDS.InteropHost()
        Me.wpfGelImage = New MCDS.MultipleImages()
        Me.tcMain.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        Me.TabPage8.SuspendLayout()
        Me.TabPage9.SuspendLayout()
        Me.TabPage10.SuspendLayout()
        Me.TabPage11.SuspendLayout()
        Me.TabPage12.SuspendLayout()
        Me.tpPrimerEditorView.SuspendLayout()
        Me.tpSequenceEditorView.SuspendLayout()
        Me.tpSequencing.SuspendLayout()
        Me.tpGelImage.SuspendLayout()
        Me.SuspendLayout()
        '
        'tcMain
        '
        Me.tcMain.Controls.Add(Me.TabPage1)
        Me.tcMain.Controls.Add(Me.TabPage2)
        Me.tcMain.Controls.Add(Me.TabPage3)
        Me.tcMain.Controls.Add(Me.TabPage4)
        Me.tcMain.Controls.Add(Me.TabPage5)
        Me.tcMain.Controls.Add(Me.TabPage6)
        Me.tcMain.Controls.Add(Me.TabPage7)
        Me.tcMain.Controls.Add(Me.TabPage8)
        Me.tcMain.Controls.Add(Me.TabPage9)
        Me.tcMain.Controls.Add(Me.TabPage10)
        Me.tcMain.Controls.Add(Me.TabPage11)
        Me.tcMain.Controls.Add(Me.TabPage12)
        Me.tcMain.Controls.Add(Me.tpPrimerEditorView)
        Me.tcMain.Controls.Add(Me.tpSequenceEditorView)
        Me.tcMain.Controls.Add(Me.tpSequencing)
        Me.tcMain.Controls.Add(Me.tpGelImage)
        Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcMain.Location = New System.Drawing.Point(0, 0)
        Me.tcMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.Padding = New System.Drawing.Point(0, 0)
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(1036, 408)
        Me.tcMain.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.llManageHosts)
        Me.TabPage1.Controls.Add(Me.llManagePrimers)
        Me.TabPage1.Controls.Add(Me.lbFileAddress)
        Me.TabPage1.Controls.Add(Me.rtbSummary)
        Me.TabPage1.Controls.Add(Me.lbEnzymes)
        Me.TabPage1.Controls.Add(Me.llLoadSequencingResultFile)
        Me.TabPage1.Controls.Add(Me.llLoadSequenceFile)
        Me.TabPage1.Controls.Add(Me.llClose)
        Me.TabPage1.Controls.Add(Me.llProjectSummary)
        Me.TabPage1.Controls.Add(Me.llExportProject)
        Me.TabPage1.Controls.Add(Me.llManageEnzymes)
        Me.TabPage1.Controls.Add(Me.llIncludeCommon)
        Me.TabPage1.Controls.Add(Me.llRemarkFeature)
        Me.TabPage1.Controls.Add(Me.llManageFeatures)
        Me.TabPage1.Controls.Add(Me.llLoadGeneFile)
        Me.TabPage1.Location = New System.Drawing.Point(4, 26)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1028, 378)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Project Property"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'llManageHosts
        '
        Me.llManageHosts.ActiveLinkColor = System.Drawing.Color.Plum
        Me.llManageHosts.AutoSize = True
        Me.llManageHosts.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llManageHosts.LinkColor = System.Drawing.Color.SeaGreen
        Me.llManageHosts.Location = New System.Drawing.Point(225, 46)
        Me.llManageHosts.Name = "llManageHosts"
        Me.llManageHosts.Size = New System.Drawing.Size(89, 17)
        Me.llManageHosts.TabIndex = 6
        Me.llManageHosts.TabStop = True
        Me.llManageHosts.Text = "Manage Hosts"
        '
        'llManagePrimers
        '
        Me.llManagePrimers.AutoSize = True
        Me.llManagePrimers.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llManagePrimers.LinkColor = System.Drawing.Color.SeaGreen
        Me.llManagePrimers.Location = New System.Drawing.Point(118, 46)
        Me.llManagePrimers.Name = "llManagePrimers"
        Me.llManagePrimers.Size = New System.Drawing.Size(101, 17)
        Me.llManagePrimers.TabIndex = 6
        Me.llManagePrimers.TabStop = True
        Me.llManagePrimers.Text = "Manage Primers"
        '
        'lbFileAddress
        '
        Me.lbFileAddress.AutoSize = True
        Me.lbFileAddress.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbFileAddress.Location = New System.Drawing.Point(6, 24)
        Me.lbFileAddress.Name = "lbFileAddress"
        Me.lbFileAddress.Size = New System.Drawing.Size(114, 14)
        Me.lbFileAddress.TabIndex = 5
        Me.lbFileAddress.Text = "[File Location] - N/A"
        '
        'rtbSummary
        '
        Me.rtbSummary.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.rtbSummary.Location = New System.Drawing.Point(5, 83)
        Me.rtbSummary.Name = "rtbSummary"
        Me.rtbSummary.Size = New System.Drawing.Size(1016, 289)
        Me.rtbSummary.TabIndex = 4
        Me.rtbSummary.Text = ""
        Me.rtbSummary.Multiline = True
        '
        'lbEnzymes
        '
        Me.lbEnzymes.AutoSize = True
        Me.lbEnzymes.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbEnzymes.Location = New System.Drawing.Point(118, 63)
        Me.lbEnzymes.Name = "lbEnzymes"
        Me.lbEnzymes.Size = New System.Drawing.Size(66, 17)
        Me.lbEnzymes.TabIndex = 3
        Me.lbEnzymes.Text = "<Enzyme>"
        '
        'llLoadSequencingResultFile
        '
        Me.llLoadSequencingResultFile.AutoSize = True
        Me.llLoadSequencingResultFile.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llLoadSequencingResultFile.LinkColor = System.Drawing.Color.IndianRed
        Me.llLoadSequencingResultFile.Location = New System.Drawing.Point(222, 3)
        Me.llLoadSequencingResultFile.Name = "llLoadSequencingResultFile"
        Me.llLoadSequencingResultFile.Size = New System.Drawing.Size(162, 17)
        Me.llLoadSequencingResultFile.TabIndex = 2
        Me.llLoadSequencingResultFile.TabStop = True
        Me.llLoadSequencingResultFile.Text = "Load Sequencing Result File"
        '
        'llLoadSequenceFile
        '
        Me.llLoadSequenceFile.AutoSize = True
        Me.llLoadSequenceFile.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llLoadSequenceFile.LinkColor = System.Drawing.Color.IndianRed
        Me.llLoadSequenceFile.Location = New System.Drawing.Point(102, 3)
        Me.llLoadSequenceFile.Name = "llLoadSequenceFile"
        Me.llLoadSequenceFile.Size = New System.Drawing.Size(114, 17)
        Me.llLoadSequenceFile.TabIndex = 2
        Me.llLoadSequenceFile.TabStop = True
        Me.llLoadSequenceFile.Text = "Load Sequence File"
        '
        'llClose
        '
        Me.llClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llClose.AutoSize = True
        Me.llClose.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llClose.LinkColor = System.Drawing.Color.Red
        Me.llClose.Location = New System.Drawing.Point(984, 3)
        Me.llClose.Name = "llClose"
        Me.llClose.Size = New System.Drawing.Size(37, 17)
        Me.llClose.TabIndex = 2
        Me.llClose.TabStop = True
        Me.llClose.Text = "Close"
        '
        'llProjectSummary
        '
        Me.llProjectSummary.AutoSize = True
        Me.llProjectSummary.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llProjectSummary.LinkColor = System.Drawing.Color.DarkKhaki
        Me.llProjectSummary.Location = New System.Drawing.Point(484, 3)
        Me.llProjectSummary.Name = "llProjectSummary"
        Me.llProjectSummary.Size = New System.Drawing.Size(92, 17)
        Me.llProjectSummary.TabIndex = 2
        Me.llProjectSummary.TabStop = True
        Me.llProjectSummary.Text = "View Summary"
        '
        'llExportProject
        '
        Me.llExportProject.AutoSize = True
        Me.llExportProject.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llExportProject.LinkColor = System.Drawing.Color.DarkGoldenrod
        Me.llExportProject.Location = New System.Drawing.Point(390, 3)
        Me.llExportProject.Name = "llExportProject"
        Me.llExportProject.Size = New System.Drawing.Size(88, 17)
        Me.llExportProject.TabIndex = 2
        Me.llExportProject.TabStop = True
        Me.llExportProject.Text = "Export Project"
        Me.llExportProject.Visible = False
        '
        'llManageEnzymes
        '
        Me.llManageEnzymes.AutoSize = True
        Me.llManageEnzymes.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llManageEnzymes.LinkColor = System.Drawing.Color.MediumTurquoise
        Me.llManageEnzymes.Location = New System.Drawing.Point(5, 63)
        Me.llManageEnzymes.Name = "llManageEnzymes"
        Me.llManageEnzymes.Size = New System.Drawing.Size(107, 17)
        Me.llManageEnzymes.TabIndex = 2
        Me.llManageEnzymes.TabStop = True
        Me.llManageEnzymes.Text = "Manage Enzymes"
        '
        'llIncludeCommon
        '
        Me.llIncludeCommon.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llIncludeCommon.AutoSize = True
        Me.llIncludeCommon.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llIncludeCommon.LinkColor = System.Drawing.Color.DodgerBlue
        Me.llIncludeCommon.Location = New System.Drawing.Point(691, 63)
        Me.llIncludeCommon.Name = "llIncludeCommon"
        Me.llIncludeCommon.Size = New System.Drawing.Size(169, 17)
        Me.llIncludeCommon.TabIndex = 2
        Me.llIncludeCommon.TabStop = True
        Me.llIncludeCommon.Text = "Include Common Definations"
        '
        'llRemarkFeature
        '
        Me.llRemarkFeature.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llRemarkFeature.AutoSize = True
        Me.llRemarkFeature.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llRemarkFeature.LinkColor = System.Drawing.Color.DodgerBlue
        Me.llRemarkFeature.Location = New System.Drawing.Point(917, 44)
        Me.llRemarkFeature.Name = "llRemarkFeature"
        Me.llRemarkFeature.Size = New System.Drawing.Size(104, 17)
        Me.llRemarkFeature.TabIndex = 2
        Me.llRemarkFeature.TabStop = True
        Me.llRemarkFeature.Text = "Remark Features"
        '
        'llManageFeatures
        '
        Me.llManageFeatures.AutoSize = True
        Me.llManageFeatures.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llManageFeatures.LinkColor = System.Drawing.Color.SeaGreen
        Me.llManageFeatures.Location = New System.Drawing.Point(5, 45)
        Me.llManageFeatures.Name = "llManageFeatures"
        Me.llManageFeatures.Size = New System.Drawing.Size(107, 17)
        Me.llManageFeatures.TabIndex = 2
        Me.llManageFeatures.TabStop = True
        Me.llManageFeatures.Text = "Manage Features"
        '
        'llLoadGeneFile
        '
        Me.llLoadGeneFile.AutoSize = True
        Me.llLoadGeneFile.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llLoadGeneFile.LinkColor = System.Drawing.Color.IndianRed
        Me.llLoadGeneFile.Location = New System.Drawing.Point(6, 3)
        Me.llLoadGeneFile.Name = "llLoadGeneFile"
        Me.llLoadGeneFile.Size = New System.Drawing.Size(90, 17)
        Me.llLoadGeneFile.TabIndex = 2
        Me.llLoadGeneFile.TabStop = True
        Me.llLoadGeneFile.Text = "Load Gene File"
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.White
        Me.TabPage2.Controls.Add(Me.PrpC)
        Me.TabPage2.Location = New System.Drawing.Point(4, 26)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(1028, 378)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Operation Property"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'PrpC
        '
        Me.PrpC.ApplyMode = True
        Me.PrpC.BackColor = System.Drawing.Color.White
        Me.PrpC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PrpC.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PrpC.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.PrpC.Location = New System.Drawing.Point(3, 3)
        Me.PrpC.MolecularOperation = MCDS.Nuctions.MolecularOperationEnum.Vector
        Me.PrpC.Name = "PrpC"
        Me.PrpC.RelatedChartItem = Nothing
        Me.PrpC.Size = New System.Drawing.Size(1022, 372)
        Me.PrpC.TabIndex = 0
        '
        'TabPage3
        '
        Me.TabPage3.BackColor = System.Drawing.Color.White
        Me.TabPage3.Controls.Add(Me.MIV)
        Me.TabPage3.Location = New System.Drawing.Point(4, 26)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(1028, 378)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Multiple DNAs"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'MIV
        '
        Me.MIV.BackColor = System.Drawing.Color.White
        Me.MIV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.MIV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MIV.Location = New System.Drawing.Point(0, 0)
        Me.MIV.Name = "MIV"
        Me.MIV.Size = New System.Drawing.Size(1028, 378)
        Me.MIV.TabIndex = 0
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.reView)
        Me.TabPage4.Location = New System.Drawing.Point(4, 26)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(1028, 378)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Enzymes"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'reView
        '
        Me.reView.BackColor = System.Drawing.Color.White
        Me.reView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.reView.Location = New System.Drawing.Point(0, 0)
        Me.reView.Name = "reView"
        Me.reView.Size = New System.Drawing.Size(1028, 378)
        Me.reView.TabIndex = 0
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.gvDNA)
        Me.TabPage5.Location = New System.Drawing.Point(4, 26)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(1028, 378)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "DNA"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'gvDNA
        '
        Me.gvDNA.BackColor = System.Drawing.Color.White
        Me.gvDNA.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gvDNA.Location = New System.Drawing.Point(0, 0)
        Me.gvDNA.Name = "gvDNA"
        Me.gvDNA.Size = New System.Drawing.Size(1028, 378)
        Me.gvDNA.TabIndex = 0
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.gvPCR)
        Me.TabPage6.Location = New System.Drawing.Point(4, 26)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(1028, 378)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "PCR"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'gvPCR
        '
        Me.gvPCR.BackColor = System.Drawing.Color.White
        Me.gvPCR.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gvPCR.Location = New System.Drawing.Point(0, 0)
        Me.gvPCR.Name = "gvPCR"
        Me.gvPCR.Size = New System.Drawing.Size(1028, 378)
        Me.gvPCR.TabIndex = 0
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.FMV)
        Me.TabPage7.Location = New System.Drawing.Point(4, 26)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Size = New System.Drawing.Size(1028, 378)
        Me.TabPage7.TabIndex = 6
        Me.TabPage7.Text = "Features"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'FMV
        '
        Me.FMV.BackColor = System.Drawing.Color.White
        Me.FMV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FMV.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FMV.Location = New System.Drawing.Point(0, 0)
        Me.FMV.Name = "FMV"
        Me.FMV.Size = New System.Drawing.Size(1028, 378)
        Me.FMV.TabIndex = 0
        '
        'TabPage8
        '
        Me.TabPage8.Controls.Add(Me.FSV)
        Me.TabPage8.Location = New System.Drawing.Point(4, 26)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Size = New System.Drawing.Size(1028, 378)
        Me.TabPage8.TabIndex = 7
        Me.TabPage8.Text = "Feature Screen"
        Me.TabPage8.UseVisualStyleBackColor = True
        '
        'FSV
        '
        Me.FSV.BackColor = System.Drawing.Color.White
        Me.FSV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FSV.Location = New System.Drawing.Point(0, 0)
        Me.FSV.Name = "FSV"
        Me.FSV.Size = New System.Drawing.Size(1028, 378)
        Me.FSV.TabIndex = 0
        '
        'TabPage9
        '
        Me.TabPage9.Controls.Add(Me.PPV)
        Me.TabPage9.Controls.Add(Me.llPrintPageCount)
        Me.TabPage9.Controls.Add(Me.llDirectPrintAll)
        Me.TabPage9.Controls.Add(Me.llPrintAllPages)
        Me.TabPage9.Controls.Add(Me.llDirectPrintSel)
        Me.TabPage9.Controls.Add(Me.llPrintSelectedPages)
        Me.TabPage9.Controls.Add(Me.llAddPage)
        Me.TabPage9.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.TabPage9.Location = New System.Drawing.Point(4, 26)
        Me.TabPage9.Name = "TabPage9"
        Me.TabPage9.Size = New System.Drawing.Size(1028, 378)
        Me.TabPage9.TabIndex = 8
        Me.TabPage9.Text = "Print Page"
        Me.TabPage9.UseVisualStyleBackColor = True
        '
        'PPV
        '
        Me.PPV.BackColor = System.Drawing.Color.White
        Me.PPV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PPV.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.PPV.Location = New System.Drawing.Point(0, 0)
        Me.PPV.Name = "PPV"
        Me.PPV.RelatedPrintPage = Nothing
        Me.PPV.Size = New System.Drawing.Size(1028, 378)
        Me.PPV.TabIndex = 0
        '
        'llPrintPageCount
        '
        Me.llPrintPageCount.AutoSize = True
        Me.llPrintPageCount.Location = New System.Drawing.Point(14, 10)
        Me.llPrintPageCount.Name = "llPrintPageCount"
        Me.llPrintPageCount.Size = New System.Drawing.Size(97, 15)
        Me.llPrintPageCount.TabIndex = 1
        Me.llPrintPageCount.TabStop = True
        Me.llPrintPageCount.Text = "0 Page Selected"
        '
        'llDirectPrintAll
        '
        Me.llDirectPrintAll.AutoSize = True
        Me.llDirectPrintAll.Location = New System.Drawing.Point(352, 37)
        Me.llDirectPrintAll.Name = "llDirectPrintAll"
        Me.llDirectPrintAll.Size = New System.Drawing.Size(121, 15)
        Me.llDirectPrintAll.TabIndex = 2
        Me.llDirectPrintAll.TabStop = True
        Me.llDirectPrintAll.Text = "Direct Print All Pages"
        '
        'llPrintAllPages
        '
        Me.llPrintAllPages.AutoSize = True
        Me.llPrintAllPages.Location = New System.Drawing.Point(188, 37)
        Me.llPrintAllPages.Name = "llPrintAllPages"
        Me.llPrintAllPages.Size = New System.Drawing.Size(86, 15)
        Me.llPrintAllPages.TabIndex = 2
        Me.llPrintAllPages.TabStop = True
        Me.llPrintAllPages.Text = "Print All Pages"
        '
        'llDirectPrintSel
        '
        Me.llDirectPrintSel.AutoSize = True
        Me.llDirectPrintSel.Location = New System.Drawing.Point(352, 10)
        Me.llDirectPrintSel.Name = "llDirectPrintSel"
        Me.llDirectPrintSel.Size = New System.Drawing.Size(157, 15)
        Me.llDirectPrintSel.TabIndex = 2
        Me.llDirectPrintSel.TabStop = True
        Me.llDirectPrintSel.Text = "Direct Print Selected Pages"
        '
        'llPrintSelectedPages
        '
        Me.llPrintSelectedPages.AutoSize = True
        Me.llPrintSelectedPages.Location = New System.Drawing.Point(188, 10)
        Me.llPrintSelectedPages.Name = "llPrintSelectedPages"
        Me.llPrintSelectedPages.Size = New System.Drawing.Size(122, 15)
        Me.llPrintSelectedPages.TabIndex = 2
        Me.llPrintSelectedPages.TabStop = True
        Me.llPrintSelectedPages.Text = "Print Selected Pages"
        '
        'llAddPage
        '
        Me.llAddPage.AutoSize = True
        Me.llAddPage.Location = New System.Drawing.Point(14, 37)
        Me.llAddPage.Name = "llAddPage"
        Me.llAddPage.Size = New System.Drawing.Size(98, 15)
        Me.llAddPage.TabIndex = 2
        Me.llAddPage.TabStop = True
        Me.llAddPage.Text = "Add a Print Page"
        '
        'TabPage10
        '
        Me.TabPage10.Controls.Add(Me.ehSummary)
        Me.TabPage10.Location = New System.Drawing.Point(4, 26)
        Me.TabPage10.Name = "TabPage10"
        Me.TabPage10.Size = New System.Drawing.Size(1028, 378)
        Me.TabPage10.TabIndex = 9
        Me.TabPage10.Text = "Summary"
        Me.TabPage10.UseVisualStyleBackColor = True
        '
        'ehSummary
        '
        Me.ehSummary.BackColor = System.Drawing.Color.White
        Me.ehSummary.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ehSummary.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.ehSummary.Location = New System.Drawing.Point(0, 0)
        Me.ehSummary.Name = "ehSummary"
        Me.ehSummary.Size = New System.Drawing.Size(1028, 378)
        Me.ehSummary.TabIndex = 0
        Me.ehSummary.Child = Me.wpfSummaryBox
        '
        'TabPage11
        '
        Me.TabPage11.Controls.Add(Me.PMV)
        Me.TabPage11.Location = New System.Drawing.Point(4, 26)
        Me.TabPage11.Name = "TabPage11"
        Me.TabPage11.Size = New System.Drawing.Size(1028, 378)
        Me.TabPage11.TabIndex = 10
        Me.TabPage11.Text = "Primer"
        Me.TabPage11.UseVisualStyleBackColor = True
        '
        'PMV
        '
        Me.PMV.BackColor = System.Drawing.Color.Ivory
        Me.PMV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PMV.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PMV.Location = New System.Drawing.Point(0, 0)
        Me.PMV.Name = "PMV"
        Me.PMV.Size = New System.Drawing.Size(1028, 378)
        Me.PMV.TabIndex = 0
        '
        'TabPage12
        '
        Me.TabPage12.Controls.Add(Me.HMV)
        Me.TabPage12.Location = New System.Drawing.Point(4, 26)
        Me.TabPage12.Name = "TabPage12"
        Me.TabPage12.Size = New System.Drawing.Size(1028, 378)
        Me.TabPage12.TabIndex = 11
        Me.TabPage12.Text = "Hosts"
        Me.TabPage12.UseVisualStyleBackColor = True
        '
        'HMV
        '
        Me.HMV.BackColor = System.Drawing.Color.LemonChiffon
        Me.HMV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HMV.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HMV.Location = New System.Drawing.Point(0, 0)
        Me.HMV.Name = "HMV"
        Me.HMV.Size = New System.Drawing.Size(1028, 378)
        Me.HMV.TabIndex = 0
        '
        'tpPrimerEditorView
        '
        Me.tpPrimerEditorView.Controls.Add(Me.mvPrimerEditorView)
        Me.tpPrimerEditorView.Location = New System.Drawing.Point(4, 26)
        Me.tpPrimerEditorView.Name = "tpPrimerEditorView"
        Me.tpPrimerEditorView.Size = New System.Drawing.Size(1028, 378)
        Me.tpPrimerEditorView.TabIndex = 12
        Me.tpPrimerEditorView.Text = "Primer Editor"
        Me.tpPrimerEditorView.UseVisualStyleBackColor = True
        '
        'mvPrimerEditorView
        '
        Me.mvPrimerEditorView.BackColor = System.Drawing.Color.White
        Me.mvPrimerEditorView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mvPrimerEditorView.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.mvPrimerEditorView.Location = New System.Drawing.Point(0, 0)
        Me.mvPrimerEditorView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.mvPrimerEditorView.Name = "mvPrimerEditorView"
        Me.mvPrimerEditorView.PropertyTab = Nothing
        Me.mvPrimerEditorView.Size = New System.Drawing.Size(1028, 378)
        Me.mvPrimerEditorView.TabIndex = 0
        '
        'tpSequenceEditorView
        '
        Me.tpSequenceEditorView.Controls.Add(Me.mvSequenceEditorView)
        Me.tpSequenceEditorView.Location = New System.Drawing.Point(4, 26)
        Me.tpSequenceEditorView.Name = "tpSequenceEditorView"
        Me.tpSequenceEditorView.Size = New System.Drawing.Size(1028, 378)
        Me.tpSequenceEditorView.TabIndex = 13
        Me.tpSequenceEditorView.Text = "DNA Editor"
        Me.tpSequenceEditorView.UseVisualStyleBackColor = True
        '
        'mvSequenceEditorView
        '
        Me.mvSequenceEditorView.BackColor = System.Drawing.Color.White
        Me.mvSequenceEditorView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mvSequenceEditorView.Location = New System.Drawing.Point(0, 0)
        Me.mvSequenceEditorView.Name = "mvSequenceEditorView"
        Me.mvSequenceEditorView.PropertyTab = Nothing
        Me.mvSequenceEditorView.Size = New System.Drawing.Size(1028, 378)
        Me.mvSequenceEditorView.TabIndex = 0
        '
        'tpSequencing
        '
        Me.tpSequencing.Controls.Add(Me.ihSequencing)
        Me.tpSequencing.Location = New System.Drawing.Point(4, 26)
        Me.tpSequencing.Name = "tpSequencing"
        Me.tpSequencing.Size = New System.Drawing.Size(1028, 378)
        Me.tpSequencing.TabIndex = 14
        Me.tpSequencing.Text = "Sequencing Results"
        '
        'ihSequencing
        '
        Me.ihSequencing.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ihSequencing.Location = New System.Drawing.Point(0, 0)
        Me.ihSequencing.Name = "ihSequencing"
        Me.ihSequencing.Size = New System.Drawing.Size(1028, 378)
        Me.ihSequencing.TabIndex = 0
        Me.ihSequencing.Child = Me.wpfSequencings
        '
        'tpGelImage
        '
        Me.tpGelImage.Controls.Add(Me.ihGelImage)
        Me.tpGelImage.Location = New System.Drawing.Point(4, 26)
        Me.tpGelImage.Name = "tpGelImage"
        Me.tpGelImage.Size = New System.Drawing.Size(1028, 378)
        Me.tpGelImage.TabIndex = 15
        Me.tpGelImage.Text = "Gel Figures"
        '
        'ihGelImage
        '
        Me.ihGelImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ihGelImage.Location = New System.Drawing.Point(0, 0)
        Me.ihGelImage.Name = "ihGelImage"
        Me.ihGelImage.Size = New System.Drawing.Size(1028, 378)
        Me.ihGelImage.TabIndex = 0
        Me.ihGelImage.Child = Me.wpfGelImage
        '
        'PropertyView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.tcMain)
        Me.Name = "PropertyView"
        Me.Size = New System.Drawing.Size(1036, 408)
        Me.tcMain.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage7.ResumeLayout(False)
        Me.TabPage8.ResumeLayout(False)
        Me.TabPage9.ResumeLayout(False)
        Me.TabPage9.PerformLayout()
        Me.TabPage10.ResumeLayout(False)
        Me.TabPage11.ResumeLayout(False)
        Me.TabPage12.ResumeLayout(False)
        Me.tpPrimerEditorView.ResumeLayout(False)
        Me.tpSequenceEditorView.ResumeLayout(False)
        Me.tpSequencing.ResumeLayout(False)
        Me.tpGelImage.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tcMain As TabContainer
    Friend WithEvents PrpC As MCDS.PropertyControl
    Friend WithEvents llLoadSequencingResultFile As System.Windows.Forms.LinkLabel
    Friend WithEvents llLoadSequenceFile As System.Windows.Forms.LinkLabel
    Friend WithEvents llManageEnzymes As System.Windows.Forms.LinkLabel
    Friend WithEvents llManageFeatures As System.Windows.Forms.LinkLabel
    Friend WithEvents llLoadGeneFile As System.Windows.Forms.LinkLabel
    Friend WithEvents MIV As MCDS.MultipleItemView
    Friend WithEvents llExportProject As System.Windows.Forms.LinkLabel
    Friend WithEvents llClose As System.Windows.Forms.LinkLabel
    Friend WithEvents TabPage1 As MCDS.CustomTabPage
    Friend WithEvents TabPage2 As MCDS.CustomTabPage
    Friend WithEvents TabPage3 As MCDS.CustomTabPage
    Friend WithEvents reView As MCDS.RestrictionEnzymeView
    Friend WithEvents gvDNA As MCDS.GroupViewer
    Friend WithEvents gvPCR As MCDS.GroupViewer
    Friend WithEvents FMV As MCDS.FeatureManageView
    Friend WithEvents FSV As MCDS.FeatureScreenView
    Friend WithEvents TabPage4 As MCDS.CustomTabPage
    Friend WithEvents TabPage5 As MCDS.CustomTabPage
    Friend WithEvents TabPage6 As MCDS.CustomTabPage
    Friend WithEvents TabPage7 As MCDS.CustomTabPage
    Friend WithEvents TabPage8 As MCDS.CustomTabPage
    Friend WithEvents lbEnzymes As System.Windows.Forms.Label
    Friend WithEvents llRemarkFeature As System.Windows.Forms.LinkLabel
    Friend WithEvents rtbSummary As System.Windows.Forms.TextBox
    Friend WithEvents TabPage9 As MCDS.CustomTabPage
    Friend WithEvents PPV As MCDS.PrintPageView
    Friend WithEvents llPrintPageCount As System.Windows.Forms.LinkLabel
    Friend WithEvents llAddPage As System.Windows.Forms.LinkLabel
    Friend WithEvents llProjectSummary As System.Windows.Forms.LinkLabel
    Friend WithEvents TabPage10 As MCDS.CustomTabPage
    Friend WithEvents lbFileAddress As System.Windows.Forms.Label
    Friend WithEvents llManagePrimers As System.Windows.Forms.LinkLabel
    Friend WithEvents TabPage11 As MCDS.CustomTabPage
    Friend WithEvents PMV As MCDS.PrimerManageView
    Friend WithEvents TabPage12 As MCDS.CustomTabPage
    Friend WithEvents HMV As MCDS.HostManageView
    Friend WithEvents llManageHosts As System.Windows.Forms.LinkLabel
    Friend WithEvents llIncludeCommon As System.Windows.Forms.LinkLabel
    Friend WithEvents llPrintAllPages As System.Windows.Forms.LinkLabel
    Friend WithEvents llPrintSelectedPages As System.Windows.Forms.LinkLabel
    Friend WithEvents llDirectPrintAll As System.Windows.Forms.LinkLabel
    Friend WithEvents llDirectPrintSel As System.Windows.Forms.LinkLabel
    Friend WithEvents tpPrimerEditorView As MCDS.EditorTabpage
    Friend WithEvents tpSequenceEditorView As MCDS.EditorTabpage
    Friend WithEvents mvPrimerEditorView As MCDS.PrimerEditorView
    Friend WithEvents mvSequenceEditorView As MCDS.SequenceEditorView
    Friend WithEvents ehSummary As System.Windows.Forms.Integration.ElementHost
    Friend wpfSummaryBox As MCDS.wpfSummary
    Friend WithEvents tpSequencing As CustomTabPage
    Friend WithEvents ihSequencing As InteropHost
    Friend WithEvents tpGelImage As CustomTabPage
    Friend WithEvents ihGelImage As InteropHost
    Friend WithEvents wpfGelImage As MultipleImages
    Friend WithEvents wpfSequencings As MultipleSequencingResults
End Class
