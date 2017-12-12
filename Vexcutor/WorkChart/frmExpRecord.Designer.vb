<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExpRecord
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExpRecord))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tsb_Add = New System.Windows.Forms.ToolStripButton
        Me.tsb_Remove = New System.Windows.Forms.ToolStripButton
        Me.dgvRecord = New System.Windows.Forms.DataGridView
        Me.cnID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.cnTime = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.cnLastID = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.cnOprtIndex = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.cnSuccess = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.cnOperation = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.cnNext = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.ToolStrip1.SuspendLayout()
        CType(Me.dgvRecord, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsb_Add, Me.tsb_Remove})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1028, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsb_Add
        '
        Me.tsb_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsb_Add.Image = CType(resources.GetObject("tsb_Add.Image"), System.Drawing.Image)
        Me.tsb_Add.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsb_Add.Name = "tsb_Add"
        Me.tsb_Add.Size = New System.Drawing.Size(51, 22)
        Me.tsb_Add.Text = "Add Row"
        '
        'tsb_Remove
        '
        Me.tsb_Remove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsb_Remove.Image = CType(resources.GetObject("tsb_Remove.Image"), System.Drawing.Image)
        Me.tsb_Remove.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsb_Remove.Name = "tsb_Remove"
        Me.tsb_Remove.Size = New System.Drawing.Size(69, 22)
        Me.tsb_Remove.Text = "Remove Row"
        '
        'dgvRecord
        '
        Me.dgvRecord.AllowUserToAddRows = False
        Me.dgvRecord.AllowUserToResizeColumns = False
        Me.dgvRecord.AllowUserToResizeRows = False
        Me.dgvRecord.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvRecord.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.cnID, Me.cnTime, Me.cnLastID, Me.cnOprtIndex, Me.cnSuccess, Me.cnOperation, Me.cnNext})
        Me.dgvRecord.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvRecord.Location = New System.Drawing.Point(0, 25)
        Me.dgvRecord.Name = "dgvRecord"
        Me.dgvRecord.RowTemplate.Height = 23
        Me.dgvRecord.Size = New System.Drawing.Size(1028, 241)
        Me.dgvRecord.TabIndex = 2
        '
        'cnID
        '
        Me.cnID.Frozen = True
        Me.cnID.HeaderText = "ID"
        Me.cnID.Name = "cnID"
        Me.cnID.ReadOnly = True
        Me.cnID.Width = 30
        '
        'cnTime
        '
        Me.cnTime.Frozen = True
        Me.cnTime.HeaderText = "Date/Time"
        Me.cnTime.Name = "cnTime"
        Me.cnTime.ReadOnly = True
        Me.cnTime.Width = 120
        '
        'cnLastID
        '
        Me.cnLastID.HeaderText = "IH"
        Me.cnLastID.Items.AddRange(New Object() {"-"})
        Me.cnLastID.Name = "cnLastID"
        Me.cnLastID.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cnLastID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.cnLastID.Width = 50
        '
        'cnOprtIndex
        '
        Me.cnOprtIndex.HeaderText = "Index"
        Me.cnOprtIndex.Items.AddRange(New Object() {"Not Shown"})
        Me.cnOprtIndex.Name = "cnOprtIndex"
        Me.cnOprtIndex.Width = 180
        '
        'cnSuccess
        '
        Me.cnSuccess.HeaderText = "OK"
        Me.cnSuccess.Name = "cnSuccess"
        Me.cnSuccess.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cnSuccess.Width = 30
        '
        'cnOperation
        '
        Me.cnOperation.HeaderText = "Operations & Results"
        Me.cnOperation.Name = "cnOperation"
        Me.cnOperation.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cnOperation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.cnOperation.Width = 370
        '
        'cnNext
        '
        Me.cnNext.HeaderText = "NextStep"
        Me.cnNext.Items.AddRange(New Object() {"Not Shown"})
        Me.cnNext.Name = "cnNext"
        Me.cnNext.Width = 180
        '
        'frmExpRecord
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 266)
        Me.ControlBox = False
        Me.Controls.Add(Me.dgvRecord)
        Me.Controls.Add(Me.ToolStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.Name = "frmExpRecord"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "frmExpRecord"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.dgvRecord, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsb_Add As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsb_Remove As System.Windows.Forms.ToolStripButton
    Friend WithEvents dgvRecord As System.Windows.Forms.DataGridView
    Friend WithEvents cnID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cnTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cnLastID As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents cnOprtIndex As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents cnSuccess As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents cnOperation As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cnNext As System.Windows.Forms.DataGridViewComboBoxColumn
End Class
