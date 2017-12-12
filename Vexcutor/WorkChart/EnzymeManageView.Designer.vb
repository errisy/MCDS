<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EnzymeManageView
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
        Me.dgvManage = New System.Windows.Forms.DataGridView()
        Me.llAdd = New System.Windows.Forms.LinkLabel()
        Me.llRemove = New System.Windows.Forms.LinkLabel()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvManage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvManage
        '
        Me.dgvManage.AllowUserToAddRows = False
        Me.dgvManage.AllowUserToDeleteRows = False
        Me.dgvManage.AllowUserToOrderColumns = True
        Me.dgvManage.AllowUserToResizeRows = False
        Me.dgvManage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvManage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvManage.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7})
        Me.dgvManage.Location = New System.Drawing.Point(0, 18)
        Me.dgvManage.Name = "dgvManage"
        Me.dgvManage.RowTemplate.Height = 23
        Me.dgvManage.Size = New System.Drawing.Size(1115, 522)
        Me.dgvManage.TabIndex = 1
        '
        'llAdd
        '
        Me.llAdd.AutoSize = True
        Me.llAdd.Location = New System.Drawing.Point(3, 3)
        Me.llAdd.Name = "llAdd"
        Me.llAdd.Size = New System.Drawing.Size(23, 12)
        Me.llAdd.TabIndex = 2
        Me.llAdd.TabStop = True
        Me.llAdd.Text = "Add"
        '
        'llRemove
        '
        Me.llRemove.AutoSize = True
        Me.llRemove.Location = New System.Drawing.Point(32, 3)
        Me.llRemove.Name = "llRemove"
        Me.llRemove.Size = New System.Drawing.Size(41, 12)
        Me.llRemove.TabIndex = 2
        Me.llRemove.TabStop = True
        Me.llRemove.Text = "Remove"
        '
        'Column1
        '
        Me.Column1.HeaderText = "Name"
        Me.Column1.Name = "Column1"
        '
        'Column2
        '
        Me.Column2.HeaderText = "Type"
        Me.Column2.Name = "Column2"
        '
        'Column3
        '
        Me.Column3.HeaderText = "B"
        Me.Column3.Name = "Column3"
        '
        'Column4
        '
        Me.Column4.HeaderText = "P"
        Me.Column4.Name = "Column4"
        '
        'Column5
        '
        Me.Column5.HeaderText = "L"
        Me.Column5.Name = "Column5"
        '
        'Column6
        '
        Me.Column6.HeaderText = "R"
        Me.Column6.Name = "Column6"
        '
        'Column7
        '
        Me.Column7.HeaderText = "Core"
        Me.Column7.Name = "Column7"
        '
        'EnzymeManageView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.llRemove)
        Me.Controls.Add(Me.llAdd)
        Me.Controls.Add(Me.dgvManage)
        Me.Name = "EnzymeManageView"
        Me.Size = New System.Drawing.Size(1115, 540)
        CType(Me.dgvManage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvManage As System.Windows.Forms.DataGridView
    Friend WithEvents llAdd As System.Windows.Forms.LinkLabel
    Friend WithEvents llRemove As System.Windows.Forms.LinkLabel
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
