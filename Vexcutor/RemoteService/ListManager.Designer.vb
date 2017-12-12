<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListManager
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
        Me.dgvView = New System.Windows.Forms.DataGridView()
        Me.llAdd = New System.Windows.Forms.LinkLabel()
        Me.llDelete = New System.Windows.Forms.LinkLabel()
        Me.tbFilter = New System.Windows.Forms.TextBox()
        Me.llSave = New System.Windows.Forms.LinkLabel()
        Me.llRefresh = New System.Windows.Forms.LinkLabel()
        Me.llClose = New System.Windows.Forms.LinkLabel()
        Me.llSearch = New System.Windows.Forms.LinkLabel()
        CType(Me.dgvView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvView
        '
        Me.dgvView.AllowUserToAddRows = False
        Me.dgvView.AllowUserToDeleteRows = False
        Me.dgvView.AllowUserToOrderColumns = True
        Me.dgvView.AllowUserToResizeRows = False
        Me.dgvView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvView.BackgroundColor = System.Drawing.Color.White
        Me.dgvView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable
        Me.dgvView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvView.GridColor = System.Drawing.Color.DeepSkyBlue
        Me.dgvView.Location = New System.Drawing.Point(0, 14)
        Me.dgvView.Name = "dgvView"
        Me.dgvView.RowHeadersVisible = False
        Me.dgvView.RowTemplate.Height = 24
        Me.dgvView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvView.Size = New System.Drawing.Size(524, 612)
        Me.dgvView.TabIndex = 7
        '
        'llAdd
        '
        Me.llAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llAdd.AutoSize = True
        Me.llAdd.BackColor = System.Drawing.Color.Transparent
        Me.llAdd.Font = New System.Drawing.Font("Arial Narrow", 10.5!, System.Drawing.FontStyle.Bold)
        Me.llAdd.LinkColor = System.Drawing.Color.GreenYellow
        Me.llAdd.Location = New System.Drawing.Point(3, 626)
        Me.llAdd.Name = "llAdd"
        Me.llAdd.Size = New System.Drawing.Size(31, 17)
        Me.llAdd.TabIndex = 12
        Me.llAdd.TabStop = True
        Me.llAdd.Text = "Add"
        '
        'llDelete
        '
        Me.llDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llDelete.AutoSize = True
        Me.llDelete.BackColor = System.Drawing.Color.Transparent
        Me.llDelete.Font = New System.Drawing.Font("Arial Narrow", 10.5!, System.Drawing.FontStyle.Bold)
        Me.llDelete.LinkColor = System.Drawing.Color.Aqua
        Me.llDelete.Location = New System.Drawing.Point(40, 626)
        Me.llDelete.Name = "llDelete"
        Me.llDelete.Size = New System.Drawing.Size(41, 17)
        Me.llDelete.TabIndex = 12
        Me.llDelete.TabStop = True
        Me.llDelete.Text = "Delete"
        '
        'tbFilter
        '
        Me.tbFilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbFilter.BackColor = System.Drawing.Color.LightCyan
        Me.tbFilter.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tbFilter.Location = New System.Drawing.Point(0, 0)
        Me.tbFilter.Name = "tbFilter"
        Me.tbFilter.Size = New System.Drawing.Size(524, 14)
        Me.tbFilter.TabIndex = 13
        '
        'llSave
        '
        Me.llSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llSave.AutoSize = True
        Me.llSave.BackColor = System.Drawing.Color.Transparent
        Me.llSave.Font = New System.Drawing.Font("Arial Narrow", 10.5!, System.Drawing.FontStyle.Bold)
        Me.llSave.LinkColor = System.Drawing.Color.Yellow
        Me.llSave.Location = New System.Drawing.Point(442, 626)
        Me.llSave.Name = "llSave"
        Me.llSave.Size = New System.Drawing.Size(34, 17)
        Me.llSave.TabIndex = 12
        Me.llSave.TabStop = True
        Me.llSave.Text = "Save"
        '
        'llRefresh
        '
        Me.llRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llRefresh.AutoSize = True
        Me.llRefresh.BackColor = System.Drawing.Color.Transparent
        Me.llRefresh.Font = New System.Drawing.Font("Arial Narrow", 10.5!, System.Drawing.FontStyle.Bold)
        Me.llRefresh.LinkColor = System.Drawing.Color.White
        Me.llRefresh.Location = New System.Drawing.Point(387, 626)
        Me.llRefresh.Name = "llRefresh"
        Me.llRefresh.Size = New System.Drawing.Size(49, 17)
        Me.llRefresh.TabIndex = 12
        Me.llRefresh.TabStop = True
        Me.llRefresh.Text = "Refresh"
        '
        'llClose
        '
        Me.llClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llClose.AutoSize = True
        Me.llClose.BackColor = System.Drawing.Color.Transparent
        Me.llClose.Font = New System.Drawing.Font("Arial Narrow", 10.5!, System.Drawing.FontStyle.Bold)
        Me.llClose.LinkColor = System.Drawing.Color.OrangeRed
        Me.llClose.Location = New System.Drawing.Point(482, 626)
        Me.llClose.Name = "llClose"
        Me.llClose.Size = New System.Drawing.Size(39, 17)
        Me.llClose.TabIndex = 12
        Me.llClose.TabStop = True
        Me.llClose.Text = "Close"
        '
        'llSearch
        '
        Me.llSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llSearch.AutoSize = True
        Me.llSearch.BackColor = System.Drawing.Color.Transparent
        Me.llSearch.Font = New System.Drawing.Font("Arial Narrow", 10.5!, System.Drawing.FontStyle.Bold)
        Me.llSearch.LinkColor = System.Drawing.Color.DarkViolet
        Me.llSearch.Location = New System.Drawing.Point(336, 626)
        Me.llSearch.Name = "llSearch"
        Me.llSearch.Size = New System.Drawing.Size(45, 17)
        Me.llSearch.TabIndex = 12
        Me.llSearch.TabStop = True
        Me.llSearch.Text = "Search"
        '
        'ListManager
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.Black
        Me.Controls.Add(Me.tbFilter)
        Me.Controls.Add(Me.llDelete)
        Me.Controls.Add(Me.llClose)
        Me.Controls.Add(Me.llSearch)
        Me.Controls.Add(Me.llRefresh)
        Me.Controls.Add(Me.llSave)
        Me.Controls.Add(Me.llAdd)
        Me.Controls.Add(Me.dgvView)
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "ListManager"
        Me.Size = New System.Drawing.Size(524, 643)
        CType(Me.dgvView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvView As System.Windows.Forms.DataGridView
    Friend WithEvents llAdd As System.Windows.Forms.LinkLabel
    Friend WithEvents llDelete As System.Windows.Forms.LinkLabel
    Friend WithEvents tbFilter As System.Windows.Forms.TextBox
    Friend WithEvents llSave As System.Windows.Forms.LinkLabel
    Friend WithEvents llRefresh As System.Windows.Forms.LinkLabel
    Friend WithEvents llClose As System.Windows.Forms.LinkLabel
    Friend WithEvents llSearch As System.Windows.Forms.LinkLabel

End Class
