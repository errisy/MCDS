<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WorkChart
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WorkChart))
        Me.msChart = New System.Windows.Forms.MenuStrip
        Me.FileFToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LoadVectorLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AddFeatureFToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DeleteItemDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.SetPositionPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.ExperimentLogLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lv_Chart = New System.Windows.Forms.ListView
        Me.cms_ChartItem = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewVToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PropertyPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.EnzymeDigestionEToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PCRRToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ModificationMToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GelSeparationGToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LigationLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TransformationScreenTToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.IconList = New System.Windows.Forms.ImageList(Me.components)
        Me.ofdGeneFile = New System.Windows.Forms.OpenFileDialog
        Me.sfdProject = New System.Windows.Forms.SaveFileDialog
        Me.scMain = New System.Windows.Forms.SplitContainer
        Me.PropertyView1 = New Vecute.PropertyView
        Me.msChart.SuspendLayout()
        Me.cms_ChartItem.SuspendLayout()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'msChart
        '
        Me.msChart.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileFToolStripMenuItem})
        Me.msChart.Location = New System.Drawing.Point(0, 0)
        Me.msChart.Name = "msChart"
        Me.msChart.Size = New System.Drawing.Size(1076, 24)
        Me.msChart.TabIndex = 1
        Me.msChart.Text = "MenuStrip1"
        '
        'FileFToolStripMenuItem
        '
        Me.FileFToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoadVectorLToolStripMenuItem, Me.AddFeatureFToolStripMenuItem, Me.DeleteItemDToolStripMenuItem, Me.ToolStripSeparator2, Me.SetPositionPToolStripMenuItem, Me.ToolStripSeparator3, Me.ExperimentLogLToolStripMenuItem})
        Me.FileFToolStripMenuItem.Name = "FileFToolStripMenuItem"
        Me.FileFToolStripMenuItem.Size = New System.Drawing.Size(95, 20)
        Me.FileFToolStripMenuItem.Text = "Experiment(&E)"
        '
        'LoadVectorLToolStripMenuItem
        '
        Me.LoadVectorLToolStripMenuItem.Name = "LoadVectorLToolStripMenuItem"
        Me.LoadVectorLToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.LoadVectorLToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.LoadVectorLToolStripMenuItem.Text = "Load Sequence(&L)"
        '
        'AddFeatureFToolStripMenuItem
        '
        Me.AddFeatureFToolStripMenuItem.Name = "AddFeatureFToolStripMenuItem"
        Me.AddFeatureFToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.AddFeatureFToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.AddFeatureFToolStripMenuItem.Text = "Define Features(&F)"
        '
        'DeleteItemDToolStripMenuItem
        '
        Me.DeleteItemDToolStripMenuItem.Name = "DeleteItemDToolStripMenuItem"
        Me.DeleteItemDToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.DeleteItemDToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.DeleteItemDToolStripMenuItem.Text = "Delete Item(&D)"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(216, 6)
        '
        'SetPositionPToolStripMenuItem
        '
        Me.SetPositionPToolStripMenuItem.Name = "SetPositionPToolStripMenuItem"
        Me.SetPositionPToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.SetPositionPToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.SetPositionPToolStripMenuItem.Text = "Show Arrows(&A)"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(216, 6)
        '
        'ExperimentLogLToolStripMenuItem
        '
        Me.ExperimentLogLToolStripMenuItem.Name = "ExperimentLogLToolStripMenuItem"
        Me.ExperimentLogLToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4
        Me.ExperimentLogLToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.ExperimentLogLToolStripMenuItem.Text = "Experiment Records(&R)"
        '
        'lv_Chart
        '
        Me.lv_Chart.AutoArrange = False
        Me.lv_Chart.ContextMenuStrip = Me.cms_ChartItem
        Me.lv_Chart.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lv_Chart.LargeImageList = Me.IconList
        Me.lv_Chart.Location = New System.Drawing.Point(0, 0)
        Me.lv_Chart.Name = "lv_Chart"
        Me.lv_Chart.ShowItemToolTips = True
        Me.lv_Chart.Size = New System.Drawing.Size(1076, 427)
        Me.lv_Chart.TabIndex = 2
        Me.lv_Chart.UseCompatibleStateImageBehavior = False
        '
        'cms_ChartItem
        '
        Me.cms_ChartItem.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewVToolStripMenuItem, Me.PropertyPToolStripMenuItem, Me.ToolStripSeparator1, Me.EnzymeDigestionEToolStripMenuItem, Me.PCRRToolStripMenuItem, Me.ModificationMToolStripMenuItem, Me.GelSeparationGToolStripMenuItem, Me.LigationLToolStripMenuItem, Me.TransformationScreenTToolStripMenuItem})
        Me.cms_ChartItem.Name = "cms_ChartItem"
        Me.cms_ChartItem.Size = New System.Drawing.Size(203, 186)
        '
        'ViewVToolStripMenuItem
        '
        Me.ViewVToolStripMenuItem.Name = "ViewVToolStripMenuItem"
        Me.ViewVToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.ViewVToolStripMenuItem.Text = "View(&V)"
        '
        'PropertyPToolStripMenuItem
        '
        Me.PropertyPToolStripMenuItem.Name = "PropertyPToolStripMenuItem"
        Me.PropertyPToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.PropertyPToolStripMenuItem.Text = "Property(&P)"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(199, 6)
        '
        'EnzymeDigestionEToolStripMenuItem
        '
        Me.EnzymeDigestionEToolStripMenuItem.Name = "EnzymeDigestionEToolStripMenuItem"
        Me.EnzymeDigestionEToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.EnzymeDigestionEToolStripMenuItem.Tag = "1"
        Me.EnzymeDigestionEToolStripMenuItem.Text = "Enzyme Digestion...(&E)"
        '
        'PCRRToolStripMenuItem
        '
        Me.PCRRToolStripMenuItem.Name = "PCRRToolStripMenuItem"
        Me.PCRRToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.PCRRToolStripMenuItem.Tag = "2"
        Me.PCRRToolStripMenuItem.Text = "PCR...(&R)"
        '
        'ModificationMToolStripMenuItem
        '
        Me.ModificationMToolStripMenuItem.Name = "ModificationMToolStripMenuItem"
        Me.ModificationMToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.ModificationMToolStripMenuItem.Tag = "3"
        Me.ModificationMToolStripMenuItem.Text = "Modification...(&M)"
        '
        'GelSeparationGToolStripMenuItem
        '
        Me.GelSeparationGToolStripMenuItem.Name = "GelSeparationGToolStripMenuItem"
        Me.GelSeparationGToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.GelSeparationGToolStripMenuItem.Tag = "4"
        Me.GelSeparationGToolStripMenuItem.Text = "Gel Separation...(&G)"
        '
        'LigationLToolStripMenuItem
        '
        Me.LigationLToolStripMenuItem.Name = "LigationLToolStripMenuItem"
        Me.LigationLToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.LigationLToolStripMenuItem.Tag = "5"
        Me.LigationLToolStripMenuItem.Text = "Ligation...(&L)"
        '
        'TransformationScreenTToolStripMenuItem
        '
        Me.TransformationScreenTToolStripMenuItem.Name = "TransformationScreenTToolStripMenuItem"
        Me.TransformationScreenTToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.TransformationScreenTToolStripMenuItem.Tag = "6"
        Me.TransformationScreenTToolStripMenuItem.Text = "Screen...(&T)"
        '
        'IconList
        '
        Me.IconList.ImageStream = CType(resources.GetObject("IconList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.IconList.TransparentColor = System.Drawing.Color.Transparent
        Me.IconList.Images.SetKeyName(0, "DNA")
        Me.IconList.Images.SetKeyName(1, "ENZ")
        Me.IconList.Images.SetKeyName(2, "PCR.png")
        Me.IconList.Images.SetKeyName(3, "MOD.png")
        Me.IconList.Images.SetKeyName(4, "GEL.png")
        Me.IconList.Images.SetKeyName(5, "LIG.png")
        Me.IconList.Images.SetKeyName(6, "TSF.png")
        '
        'ofdGeneFile
        '
        Me.ofdGeneFile.Filter = "GeneBank Files|*.gb"
        '
        'sfdProject
        '
        Me.sfdProject.Filter = "Vecute Files|*.stone"
        '
        'scMain
        '
        Me.scMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scMain.Location = New System.Drawing.Point(0, 24)
        Me.scMain.Name = "scMain"
        Me.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.lv_Chart)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.PropertyView1)
        Me.scMain.Size = New System.Drawing.Size(1076, 620)
        Me.scMain.SplitterDistance = 427
        Me.scMain.TabIndex = 3
        '
        'PropertyView1
        '
        Me.PropertyView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PropertyView1.Location = New System.Drawing.Point(0, 0)
        Me.PropertyView1.Name = "PropertyView1"
        Me.PropertyView1.Size = New System.Drawing.Size(1076, 189)
        Me.PropertyView1.TabIndex = 0
        '
        'WorkChart
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1076, 644)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.msChart)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.msChart
        Me.Name = "WorkChart"
        Me.Text = "WorkChart"
        Me.msChart.ResumeLayout(False)
        Me.msChart.PerformLayout()
        Me.cms_ChartItem.ResumeLayout(False)
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        Me.scMain.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents msChart As System.Windows.Forms.MenuStrip
    Friend WithEvents FileFToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadVectorLToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lv_Chart As System.Windows.Forms.ListView
    Friend WithEvents IconList As System.Windows.Forms.ImageList
    Friend WithEvents ofdGeneFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents cms_ChartItem As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ViewVToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PropertyPToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents EnzymeDigestionEToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PCRRToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ModificationMToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GelSeparationGToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LigationLToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TransformationScreenTToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddFeatureFToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sfdProject As System.Windows.Forms.SaveFileDialog
    Friend WithEvents DeleteItemDToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetPositionPToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExperimentLogLToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents scMain As System.Windows.Forms.SplitContainer
    Friend WithEvents PropertyView1 As Vecute.PropertyView
End Class
