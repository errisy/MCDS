<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ServiceManager
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ServiceManager))
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Local Services", 1, 1)
        Me.ilService = New System.Windows.Forms.ImageList(Me.components)
        Me.tvLocal = New System.Windows.Forms.TreeView()
        Me.lvStatus = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.llExportConnectionFile = New System.Windows.Forms.LinkLabel()
        Me.sfdServerInfo = New System.Windows.Forms.SaveFileDialog()
        Me.ofdServerInfo = New System.Windows.Forms.OpenFileDialog()
        Me.llStatus = New System.Windows.Forms.LinkLabel()
        Me.llLog = New System.Windows.Forms.LinkLabel()
        Me.dgvLog = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvLog, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ilService
        '
        Me.ilService.ImageStream = CType(resources.GetObject("ilService.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilService.TransparentColor = System.Drawing.Color.Transparent
        Me.ilService.Images.SetKeyName(0, "被动连接")
        Me.ilService.Images.SetKeyName(1, "本地主机")
        Me.ilService.Images.SetKeyName(2, "断开的服务")
        Me.ilService.Images.SetKeyName(3, "连接的服务")
        Me.ilService.Images.SetKeyName(4, "启动的本地服务")
        Me.ilService.Images.SetKeyName(5, "停止的本地服务")
        Me.ilService.Images.SetKeyName(6, "远程主机")
        Me.ilService.Images.SetKeyName(7, "主动连接")
        Me.ilService.Images.SetKeyName(8, "网址")
        '
        'tvLocal
        '
        Me.tvLocal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tvLocal.BackColor = System.Drawing.Color.Black
        Me.tvLocal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tvLocal.Font = New System.Drawing.Font("微软雅黑", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.tvLocal.ForeColor = System.Drawing.Color.Chartreuse
        Me.tvLocal.ImageIndex = 0
        Me.tvLocal.ImageList = Me.ilService
        Me.tvLocal.LineColor = System.Drawing.Color.White
        Me.tvLocal.Location = New System.Drawing.Point(0, 0)
        Me.tvLocal.Margin = New System.Windows.Forms.Padding(0)
        Me.tvLocal.Name = "tvLocal"
        TreeNode1.ImageIndex = 1
        TreeNode1.Name = "tnLocalHost"
        TreeNode1.SelectedImageIndex = 1
        TreeNode1.Text = "Local Services"
        Me.tvLocal.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode1})
        Me.tvLocal.SelectedImageIndex = 0
        Me.tvLocal.Size = New System.Drawing.Size(303, 615)
        Me.tvLocal.TabIndex = 0
        '
        'lvStatus
        '
        Me.lvStatus.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvStatus.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.lvStatus.Location = New System.Drawing.Point(306, 3)
        Me.lvStatus.Name = "lvStatus"
        Me.lvStatus.Size = New System.Drawing.Size(856, 612)
        Me.lvStatus.SmallImageList = Me.ilService
        Me.lvStatus.StateImageList = Me.ilService
        Me.lvStatus.TabIndex = 2
        Me.lvStatus.UseCompatibleStateImageBehavior = False
        Me.lvStatus.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Operator"
        Me.ColumnHeader1.Width = 154
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Last Operation"
        Me.ColumnHeader2.Width = 211
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Time"
        Me.ColumnHeader3.Width = 180
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Last Login"
        Me.ColumnHeader4.Width = 180
        '
        'llExportConnectionFile
        '
        Me.llExportConnectionFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llExportConnectionFile.AutoSize = True
        Me.llExportConnectionFile.Enabled = False
        Me.llExportConnectionFile.LinkColor = System.Drawing.Color.DarkViolet
        Me.llExportConnectionFile.Location = New System.Drawing.Point(3, 615)
        Me.llExportConnectionFile.Name = "llExportConnectionFile"
        Me.llExportConnectionFile.Size = New System.Drawing.Size(154, 19)
        Me.llExportConnectionFile.TabIndex = 3
        Me.llExportConnectionFile.TabStop = True
        Me.llExportConnectionFile.Text = "Export Connection File"
        '
        'sfdServerInfo
        '
        Me.sfdServerInfo.FileName = "GameServer.si"
        Me.sfdServerInfo.Filter = "ServerInfo|*.si"
        Me.sfdServerInfo.Title = "Export Server Connection Code"
        '
        'ofdServerInfo
        '
        Me.ofdServerInfo.Filter = "ServerInfo|*.si"
        Me.ofdServerInfo.Title = "Load Server Connection Code"
        '
        'llStatus
        '
        Me.llStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llStatus.AutoSize = True
        Me.llStatus.LinkColor = System.Drawing.Color.MediumVioletRed
        Me.llStatus.Location = New System.Drawing.Point(302, 615)
        Me.llStatus.Name = "llStatus"
        Me.llStatus.Size = New System.Drawing.Size(99, 19)
        Me.llStatus.TabIndex = 3
        Me.llStatus.TabStop = True
        Me.llStatus.Text = "Service Status"
        '
        'llLog
        '
        Me.llLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llLog.AutoSize = True
        Me.llLog.LinkColor = System.Drawing.Color.Olive
        Me.llLog.Location = New System.Drawing.Point(407, 615)
        Me.llLog.Name = "llLog"
        Me.llLog.Size = New System.Drawing.Size(101, 19)
        Me.llLog.TabIndex = 3
        Me.llLog.TabStop = True
        Me.llLog.Text = "Operation Log"
        '
        'dgvLog
        '
        Me.dgvLog.AllowUserToAddRows = False
        Me.dgvLog.AllowUserToDeleteRows = False
        Me.dgvLog.AllowUserToResizeRows = False
        Me.dgvLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvLog.BackgroundColor = System.Drawing.Color.White
        Me.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLog.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4})
        Me.dgvLog.GridColor = System.Drawing.Color.PaleTurquoise
        Me.dgvLog.Location = New System.Drawing.Point(306, 3)
        Me.dgvLog.Name = "dgvLog"
        Me.dgvLog.RowHeadersVisible = False
        Me.dgvLog.RowTemplate.Height = 23
        Me.dgvLog.Size = New System.Drawing.Size(856, 612)
        Me.dgvLog.TabIndex = 4
        Me.dgvLog.Visible = False
        '
        'Column1
        '
        Me.Column1.HeaderText = "Operator"
        Me.Column1.Name = "Column1"
        '
        'Column2
        '
        Me.Column2.HeaderText = "Operation"
        Me.Column2.Name = "Column2"
        '
        'Column3
        '
        Me.Column3.HeaderText = "Details"
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 400
        '
        'Column4
        '
        Me.Column4.HeaderText = "Time"
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 180
        '
        'ServiceManager
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.dgvLog)
        Me.Controls.Add(Me.llLog)
        Me.Controls.Add(Me.llStatus)
        Me.Controls.Add(Me.llExportConnectionFile)
        Me.Controls.Add(Me.lvStatus)
        Me.Controls.Add(Me.tvLocal)
        Me.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "ServiceManager"
        Me.Size = New System.Drawing.Size(1165, 637)
        CType(Me.dgvLog, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ilService As System.Windows.Forms.ImageList
    Friend WithEvents tvLocal As System.Windows.Forms.TreeView
    Friend WithEvents lvStatus As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents llExportConnectionFile As System.Windows.Forms.LinkLabel
    Friend WithEvents sfdServerInfo As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ofdServerInfo As System.Windows.Forms.OpenFileDialog
    Friend WithEvents llStatus As System.Windows.Forms.LinkLabel
    Friend WithEvents llLog As System.Windows.Forms.LinkLabel
    Friend WithEvents dgvLog As System.Windows.Forms.DataGridView
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
