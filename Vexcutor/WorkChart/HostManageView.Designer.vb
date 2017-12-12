<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HostManageView
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
        Me.dgvHost = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.llAdd = New System.Windows.Forms.LinkLabel()
        Me.llDelete = New System.Windows.Forms.LinkLabel()
        Me.llCopy = New System.Windows.Forms.LinkLabel()
        Me.llPaste = New System.Windows.Forms.LinkLabel()
        Me.llApply = New System.Windows.Forms.LinkLabel()
        Me.llCancel = New System.Windows.Forms.LinkLabel()
        Me.llCommonStrains = New System.Windows.Forms.LinkLabel()
        CType(Me.dgvHost, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvHost
        '
        Me.dgvHost.AllowUserToAddRows = False
        Me.dgvHost.AllowUserToDeleteRows = False
        Me.dgvHost.AllowUserToOrderColumns = True
        Me.dgvHost.AllowUserToResizeRows = False
        Me.dgvHost.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvHost.BackgroundColor = System.Drawing.Color.White
        Me.dgvHost.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvHost.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column3, Me.Column2})
        Me.dgvHost.GridColor = System.Drawing.Color.Tomato
        Me.dgvHost.Location = New System.Drawing.Point(0, 21)
        Me.dgvHost.Name = "dgvHost"
        Me.dgvHost.RowTemplate.Height = 23
        Me.dgvHost.Size = New System.Drawing.Size(933, 339)
        Me.dgvHost.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.HeaderText = "Host Name"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 200
        '
        'Column3
        '
        Me.Column3.HeaderText = "Functions"
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 250
        '
        'Column2
        '
        Me.Column2.HeaderText = "Description"
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 300
        '
        'llAdd
        '
        Me.llAdd.AutoSize = True
        Me.llAdd.Location = New System.Drawing.Point(3, 6)
        Me.llAdd.Name = "llAdd"
        Me.llAdd.Size = New System.Drawing.Size(28, 14)
        Me.llAdd.TabIndex = 1
        Me.llAdd.TabStop = True
        Me.llAdd.Text = "Add"
        '
        'llDelete
        '
        Me.llDelete.AutoSize = True
        Me.llDelete.Location = New System.Drawing.Point(37, 6)
        Me.llDelete.Name = "llDelete"
        Me.llDelete.Size = New System.Drawing.Size(44, 14)
        Me.llDelete.TabIndex = 1
        Me.llDelete.TabStop = True
        Me.llDelete.Text = "Delete"
        '
        'llCopy
        '
        Me.llCopy.AutoSize = True
        Me.llCopy.Location = New System.Drawing.Point(87, 6)
        Me.llCopy.Name = "llCopy"
        Me.llCopy.Size = New System.Drawing.Size(32, 14)
        Me.llCopy.TabIndex = 1
        Me.llCopy.TabStop = True
        Me.llCopy.Text = "Copy"
        '
        'llPaste
        '
        Me.llPaste.AutoSize = True
        Me.llPaste.Location = New System.Drawing.Point(125, 6)
        Me.llPaste.Name = "llPaste"
        Me.llPaste.Size = New System.Drawing.Size(37, 14)
        Me.llPaste.TabIndex = 1
        Me.llPaste.TabStop = True
        Me.llPaste.Text = "Paste"
        '
        'llApply
        '
        Me.llApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llApply.AutoSize = True
        Me.llApply.Location = New System.Drawing.Point(895, 6)
        Me.llApply.Name = "llApply"
        Me.llApply.Size = New System.Drawing.Size(37, 14)
        Me.llApply.TabIndex = 1
        Me.llApply.TabStop = True
        Me.llApply.Text = "Apply"
        '
        'llCancel
        '
        Me.llCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llCancel.AutoSize = True
        Me.llCancel.Location = New System.Drawing.Point(854, 6)
        Me.llCancel.Name = "llCancel"
        Me.llCancel.Size = New System.Drawing.Size(43, 14)
        Me.llCancel.TabIndex = 1
        Me.llCancel.TabStop = True
        Me.llCancel.Text = "Cancel"
        '
        'llCommonStrains
        '
        Me.llCommonStrains.AutoSize = True
        Me.llCommonStrains.Location = New System.Drawing.Point(168, 6)
        Me.llCommonStrains.Name = "llCommonStrains"
        Me.llCommonStrains.Size = New System.Drawing.Size(95, 14)
        Me.llCommonStrains.TabIndex = 1
        Me.llCommonStrains.TabStop = True
        Me.llCommonStrains.Text = "Common Strains"
        '
        'HostManageView
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.LemonChiffon
        Me.Controls.Add(Me.llCancel)
        Me.Controls.Add(Me.llApply)
        Me.Controls.Add(Me.llPaste)
        Me.Controls.Add(Me.llCopy)
        Me.Controls.Add(Me.llDelete)
        Me.Controls.Add(Me.llCommonStrains)
        Me.Controls.Add(Me.llAdd)
        Me.Controls.Add(Me.dgvHost)
        Me.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "HostManageView"
        Me.Size = New System.Drawing.Size(933, 360)
        CType(Me.dgvHost, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvHost As System.Windows.Forms.DataGridView
    Friend WithEvents llAdd As System.Windows.Forms.LinkLabel
    Friend WithEvents llDelete As System.Windows.Forms.LinkLabel
    Friend WithEvents llCopy As System.Windows.Forms.LinkLabel
    Friend WithEvents llPaste As System.Windows.Forms.LinkLabel
    Friend WithEvents llApply As System.Windows.Forms.LinkLabel
    Friend WithEvents llCancel As System.Windows.Forms.LinkLabel
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents llCommonStrains As System.Windows.Forms.LinkLabel

End Class
