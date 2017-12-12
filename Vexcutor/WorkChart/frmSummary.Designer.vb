<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSummary
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSummary))
        Me.sfdPic = New System.Windows.Forms.SaveFileDialog
        Me.cmsFigure = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AutoArragneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AutoFitChildrenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.bpbMain = New Vecute.BufferedPictureBox
        Me.StepFitChildrenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.cmsFigure.SuspendLayout()
        CType(Me.bpbMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'sfdPic
        '
        Me.sfdPic.Filter = "EMF Image|*.emf"
        '
        'cmsFigure
        '
        Me.cmsFigure.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.AutoArragneToolStripMenuItem, Me.AutoFitChildrenToolStripMenuItem, Me.StepFitChildrenToolStripMenuItem})
        Me.cmsFigure.Name = "cmsFigure"
        Me.cmsFigure.Size = New System.Drawing.Size(173, 114)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'AutoArragneToolStripMenuItem
        '
        Me.AutoArragneToolStripMenuItem.Name = "AutoArragneToolStripMenuItem"
        Me.AutoArragneToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.AutoArragneToolStripMenuItem.Text = "Auto Arrange"
        '
        'AutoFitChildrenToolStripMenuItem
        '
        Me.AutoFitChildrenToolStripMenuItem.Name = "AutoFitChildrenToolStripMenuItem"
        Me.AutoFitChildrenToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.AutoFitChildrenToolStripMenuItem.Text = "Auto Fit Children"
        '
        'bpbMain
        '
        Me.bpbMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bpbMain.Location = New System.Drawing.Point(0, 0)
        Me.bpbMain.Name = "bpbMain"
        Me.bpbMain.Size = New System.Drawing.Size(879, 659)
        Me.bpbMain.TabIndex = 0
        Me.bpbMain.TabStop = False
        '
        'StepFitChildrenToolStripMenuItem
        '
        Me.StepFitChildrenToolStripMenuItem.Name = "StepFitChildrenToolStripMenuItem"
        Me.StepFitChildrenToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.StepFitChildrenToolStripMenuItem.Text = "Step Fit Children"
        '
        'frmSummary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(879, 659)
        Me.Controls.Add(Me.bpbMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmSummary"
        Me.Text = "frmSummary"
        Me.cmsFigure.ResumeLayout(False)
        CType(Me.bpbMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bpbMain As Vecute.BufferedPictureBox
    Friend WithEvents sfdPic As System.Windows.Forms.SaveFileDialog
    Friend WithEvents cmsFigure As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AutoArragneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AutoFitChildrenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StepFitChildrenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
