<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FeatureScreenView
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
        Me.dgvFeatures = New System.Windows.Forms.DataGridView
        Me.llApply = New System.Windows.Forms.LinkLabel
        Me.llCancel = New System.Windows.Forms.LinkLabel
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column3 = New System.Windows.Forms.DataGridViewComboBoxColumn
        CType(Me.dgvFeatures, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvFeatures
        '
        Me.dgvFeatures.AllowUserToAddRows = False
        Me.dgvFeatures.AllowUserToDeleteRows = False
        Me.dgvFeatures.AllowUserToOrderColumns = True
        Me.dgvFeatures.AllowUserToResizeRows = False
        Me.dgvFeatures.BackgroundColor = System.Drawing.Color.White
        Me.dgvFeatures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFeatures.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column4, Me.Column5, Me.Column3})
        Me.dgvFeatures.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvFeatures.GridColor = System.Drawing.Color.OrangeRed
        Me.dgvFeatures.Location = New System.Drawing.Point(0, 0)
        Me.dgvFeatures.Name = "dgvFeatures"
        Me.dgvFeatures.RowTemplate.Height = 23
        Me.dgvFeatures.Size = New System.Drawing.Size(1009, 451)
        Me.dgvFeatures.TabIndex = 0
        '
        'llApply
        '
        Me.llApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llApply.AutoSize = True
        Me.llApply.Location = New System.Drawing.Point(954, 3)
        Me.llApply.Name = "llApply"
        Me.llApply.Size = New System.Drawing.Size(35, 12)
        Me.llApply.TabIndex = 1
        Me.llApply.TabStop = True
        Me.llApply.Text = "Apply"
        '
        'llCancel
        '
        Me.llCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llCancel.AutoSize = True
        Me.llCancel.Location = New System.Drawing.Point(907, 3)
        Me.llCancel.Name = "llCancel"
        Me.llCancel.Size = New System.Drawing.Size(41, 12)
        Me.llCancel.TabIndex = 1
        Me.llCancel.TabStop = True
        Me.llCancel.Text = "Cancel"
        '
        'Column1
        '
        Me.Column1.HeaderText = "Feature Name"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 250
        '
        'Column4
        '
        Me.Column4.HeaderText = "Note"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Width = 250
        '
        'Column5
        '
        Me.Column5.HeaderText = "Length"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.HeaderText = "Parameter"
        Me.Column3.Items.AddRange(New Object() {"NotEngaged", "None", "Once", "OnceOrMore", "Maximum"})
        Me.Column3.Name = "Column3"
        Me.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column3.Width = 200
        '
        'FeatureScreenView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.llCancel)
        Me.Controls.Add(Me.llApply)
        Me.Controls.Add(Me.dgvFeatures)
        Me.Name = "FeatureScreenView"
        Me.Size = New System.Drawing.Size(1009, 451)
        CType(Me.dgvFeatures, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvFeatures As System.Windows.Forms.DataGridView
    Friend WithEvents llApply As System.Windows.Forms.LinkLabel
    Friend WithEvents llCancel As System.Windows.Forms.LinkLabel
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewComboBoxColumn

End Class
