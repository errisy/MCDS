<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFeatureManage
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFeatureManage))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tsb_Add = New System.Windows.Forms.ToolStripButton
        Me.tsb_Delete = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.tsb_OK = New System.Windows.Forms.ToolStripButton
        Me.dgvFeat = New System.Windows.Forms.DataGridView
        Me.cLabel = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.cUSE = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.cnType = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.cnNote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.cnLength = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.cnID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.cnSequence = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ToolStrip1.SuspendLayout()
        CType(Me.dgvFeat, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsb_Add, Me.tsb_Delete, Me.ToolStripSeparator1, Me.tsb_OK})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(859, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsb_Add
        '
        Me.tsb_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsb_Add.Image = CType(resources.GetObject("tsb_Add.Image"), System.Drawing.Image)
        Me.tsb_Add.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsb_Add.Name = "tsb_Add"
        Me.tsb_Add.Size = New System.Drawing.Size(27, 22)
        Me.tsb_Add.Text = "Add"
        '
        'tsb_Delete
        '
        Me.tsb_Delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsb_Delete.Image = CType(resources.GetObject("tsb_Delete.Image"), System.Drawing.Image)
        Me.tsb_Delete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsb_Delete.Name = "tsb_Delete"
        Me.tsb_Delete.Size = New System.Drawing.Size(45, 22)
        Me.tsb_Delete.Text = "Delete"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsb_OK
        '
        Me.tsb_OK.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsb_OK.Image = CType(resources.GetObject("tsb_OK.Image"), System.Drawing.Image)
        Me.tsb_OK.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsb_OK.Name = "tsb_OK"
        Me.tsb_OK.Size = New System.Drawing.Size(23, 22)
        Me.tsb_OK.Text = "OK"
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
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.OliveDrab
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvFeat.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvFeat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFeat.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.cLabel, Me.cUSE, Me.cnType, Me.cnNote, Me.cnLength, Me.cnID, Me.cnSequence})
        Me.dgvFeat.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvFeat.GridColor = System.Drawing.Color.OrangeRed
        Me.dgvFeat.Location = New System.Drawing.Point(0, 25)
        Me.dgvFeat.Name = "dgvFeat"
        Me.dgvFeat.RowTemplate.Height = 23
        Me.dgvFeat.Size = New System.Drawing.Size(859, 354)
        Me.dgvFeat.TabIndex = 2
        '
        'cLabel
        '
        Me.cLabel.HeaderText = "Label"
        Me.cLabel.Name = "cLabel"
        Me.cLabel.Width = 120
        '
        'cUSE
        '
        Me.cUSE.HeaderText = "USE"
        Me.cUSE.Name = "cUSE"
        Me.cUSE.Width = 30
        '
        'cnType
        '
        Me.cnType.HeaderText = "Type"
        Me.cnType.Items.AddRange(New Object() {"CDS", "exon", "enhancer", "loci", "misc_feature", "misc_signal", "mutation", "operon", "oriT", "primer_bind", "promoter", "RBS", "rep_origin", "terminator"})
        Me.cnType.Name = "cnType"
        '
        'cnNote
        '
        Me.cnNote.HeaderText = "Note"
        Me.cnNote.Name = "cnNote"
        '
        'cnLength
        '
        Me.cnLength.HeaderText = "Length"
        Me.cnLength.Name = "cnLength"
        Me.cnLength.ReadOnly = True
        '
        'cnID
        '
        Me.cnID.HeaderText = "ID"
        Me.cnID.Name = "cnID"
        Me.cnID.ReadOnly = True
        '
        'cnSequence
        '
        Me.cnSequence.HeaderText = "Sequence"
        Me.cnSequence.Name = "cnSequence"
        Me.cnSequence.Width = 260
        '
        'frmFeatureManage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(859, 379)
        Me.Controls.Add(Me.dgvFeat)
        Me.Controls.Add(Me.ToolStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "frmFeatureManage"
        Me.Text = "Feature Manage"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.dgvFeat, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsb_Add As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsb_Delete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsb_OK As System.Windows.Forms.ToolStripButton
    Friend WithEvents dgvFeat As System.Windows.Forms.DataGridView
    Friend WithEvents cLabel As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cUSE As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents cnType As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents cnNote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cnLength As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cnID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cnSequence As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
