<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrimerManageView
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
        Me.dgvPrimers = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.llAdd = New System.Windows.Forms.LinkLabel()
        Me.llRemove = New System.Windows.Forms.LinkLabel()
        Me.llPaste = New System.Windows.Forms.LinkLabel()
        Me.llCopy = New System.Windows.Forms.LinkLabel()
        Me.llCancel = New System.Windows.Forms.LinkLabel()
        Me.llApply = New System.Windows.Forms.LinkLabel()
        CType(Me.dgvPrimers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvPrimers
        '
        Me.dgvPrimers.AllowUserToAddRows = False
        Me.dgvPrimers.AllowUserToDeleteRows = False
        Me.dgvPrimers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvPrimers.BackgroundColor = System.Drawing.Color.White
        Me.dgvPrimers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPrimers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column6, Me.Column5, Me.Column3, Me.Column4, Me.Column7})
        Me.dgvPrimers.Location = New System.Drawing.Point(0, 20)
        Me.dgvPrimers.Name = "dgvPrimers"
        Me.dgvPrimers.RowTemplate.Height = 23
        Me.dgvPrimers.Size = New System.Drawing.Size(967, 296)
        Me.dgvPrimers.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.HeaderText = "Name"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 80
        '
        'Column2
        '
        Me.Column2.HeaderText = "Sequence"
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 300
        '
        'Column6
        '
        Me.Column6.HeaderText = "Length"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        Me.Column6.Width = 60
        '
        'Column5
        '
        Me.Column5.HeaderText = "GC Ratio"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Width = 60
        '
        'Column3
        '
        Me.Column3.HeaderText = "Tm Binding"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Width = 60
        '
        'Column4
        '
        Me.Column4.HeaderText = "Tm Full"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Width = 60
        '
        'Column7
        '
        Me.Column7.FalseValue = "0"
        Me.Column7.HeaderText = "Need Synthesis"
        Me.Column7.Name = "Column7"
        Me.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Column7.TrueValue = "1"
        Me.Column7.Width = 80
        '
        'llAdd
        '
        Me.llAdd.AutoSize = True
        Me.llAdd.Location = New System.Drawing.Point(3, 5)
        Me.llAdd.Name = "llAdd"
        Me.llAdd.Size = New System.Drawing.Size(28, 14)
        Me.llAdd.TabIndex = 1
        Me.llAdd.TabStop = True
        Me.llAdd.Text = "Add"
        '
        'llRemove
        '
        Me.llRemove.AutoSize = True
        Me.llRemove.Location = New System.Drawing.Point(32, 5)
        Me.llRemove.Name = "llRemove"
        Me.llRemove.Size = New System.Drawing.Size(50, 14)
        Me.llRemove.TabIndex = 1
        Me.llRemove.TabStop = True
        Me.llRemove.Text = "Remove"
        '
        'llPaste
        '
        Me.llPaste.AutoSize = True
        Me.llPaste.Location = New System.Drawing.Point(79, 5)
        Me.llPaste.Name = "llPaste"
        Me.llPaste.Size = New System.Drawing.Size(37, 14)
        Me.llPaste.TabIndex = 1
        Me.llPaste.TabStop = True
        Me.llPaste.Text = "Paste"
        '
        'llCopy
        '
        Me.llCopy.AutoSize = True
        Me.llCopy.Location = New System.Drawing.Point(120, 5)
        Me.llCopy.Name = "llCopy"
        Me.llCopy.Size = New System.Drawing.Size(32, 14)
        Me.llCopy.TabIndex = 1
        Me.llCopy.TabStop = True
        Me.llCopy.Text = "Copy"
        '
        'llCancel
        '
        Me.llCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llCancel.AutoSize = True
        Me.llCancel.Location = New System.Drawing.Point(923, 5)
        Me.llCancel.Name = "llCancel"
        Me.llCancel.Size = New System.Drawing.Size(43, 14)
        Me.llCancel.TabIndex = 1
        Me.llCancel.TabStop = True
        Me.llCancel.Text = "Cancel"
        '
        'llApply
        '
        Me.llApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llApply.AutoSize = True
        Me.llApply.Location = New System.Drawing.Point(882, 5)
        Me.llApply.Name = "llApply"
        Me.llApply.Size = New System.Drawing.Size(37, 14)
        Me.llApply.TabIndex = 1
        Me.llApply.TabStop = True
        Me.llApply.Text = "Apply"
        '
        'PrimerManageView
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.Ivory
        Me.Controls.Add(Me.llApply)
        Me.Controls.Add(Me.llCancel)
        Me.Controls.Add(Me.llCopy)
        Me.Controls.Add(Me.llPaste)
        Me.Controls.Add(Me.llRemove)
        Me.Controls.Add(Me.llAdd)
        Me.Controls.Add(Me.dgvPrimers)
        Me.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "PrimerManageView"
        Me.Size = New System.Drawing.Size(967, 316)
        CType(Me.dgvPrimers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvPrimers As System.Windows.Forms.DataGridView
    Friend WithEvents llAdd As System.Windows.Forms.LinkLabel
    Friend WithEvents llRemove As System.Windows.Forms.LinkLabel
    Friend WithEvents llPaste As System.Windows.Forms.LinkLabel
    Friend WithEvents llCopy As System.Windows.Forms.LinkLabel
    Friend WithEvents llCancel As System.Windows.Forms.LinkLabel
    Friend WithEvents llApply As System.Windows.Forms.LinkLabel
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewCheckBoxColumn

End Class
