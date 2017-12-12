<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrimerConflict
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
        Me.dgvPrimers = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column0 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvPrimers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvPrimers
        '
        Me.dgvPrimers.AllowUserToAddRows = False
        Me.dgvPrimers.AllowUserToDeleteRows = False
        Me.dgvPrimers.AllowUserToOrderColumns = True
        Me.dgvPrimers.AllowUserToResizeColumns = False
        Me.dgvPrimers.AllowUserToResizeRows = False
        Me.dgvPrimers.BackgroundColor = System.Drawing.Color.White
        Me.dgvPrimers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPrimers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column0, Me.Column2})
        Me.dgvPrimers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvPrimers.GridColor = System.Drawing.Color.OrangeRed
        Me.dgvPrimers.Location = New System.Drawing.Point(0, 0)
        Me.dgvPrimers.Name = "dgvPrimers"
        Me.dgvPrimers.ReadOnly = True
        Me.dgvPrimers.RowHeadersVisible = False
        Me.dgvPrimers.RowTemplate.Height = 23
        Me.dgvPrimers.Size = New System.Drawing.Size(805, 402)
        Me.dgvPrimers.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.Frozen = True
        Me.Column1.HeaderText = "Item ID"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column0
        '
        Me.Column0.Frozen = True
        Me.Column0.HeaderText = "Name"
        Me.Column0.Name = "Column0"
        Me.Column0.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.HeaderText = "Primer"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 600
        '
        'frmPrimerConflict
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(805, 402)
        Me.Controls.Add(Me.dgvPrimers)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "frmPrimerConflict"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Primer Summary"
        CType(Me.dgvPrimers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvPrimers As System.Windows.Forms.DataGridView
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column0 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
