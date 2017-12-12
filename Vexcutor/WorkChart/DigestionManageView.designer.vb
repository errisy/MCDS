<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DigestionManageView
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
        Me.llRemoveEnzymeInfo = New System.Windows.Forms.LinkLabel()
        Me.llAddEnzyme = New System.Windows.Forms.LinkLabel()
        Me.llCancel = New System.Windows.Forms.LinkLabel()
        Me.llOK = New System.Windows.Forms.LinkLabel()
        Me.llApplyBuffers = New System.Windows.Forms.LinkLabel()
        Me.dgvEnzymeInfo = New System.Windows.Forms.DataGridView()
        Me.clNames = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clTemp = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clAdditives = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbBuffers = New System.Windows.Forms.TextBox()
        Me.tbName = New System.Windows.Forms.TextBox()
        Me.llHelpDigestionBuffer = New System.Windows.Forms.LinkLabel()
        Me.llRemove = New System.Windows.Forms.LinkLabel()
        Me.blvList = New Vecute.BufferedListView()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        CType(Me.dgvEnzymeInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.blvList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'llRemoveEnzymeInfo
        '
        Me.llRemoveEnzymeInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llRemoveEnzymeInfo.AutoSize = True
        Me.llRemoveEnzymeInfo.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llRemoveEnzymeInfo.Location = New System.Drawing.Point(779, 57)
        Me.llRemoveEnzymeInfo.Name = "llRemoveEnzymeInfo"
        Me.llRemoveEnzymeInfo.Size = New System.Drawing.Size(122, 15)
        Me.llRemoveEnzymeInfo.TabIndex = 9
        Me.llRemoveEnzymeInfo.TabStop = True
        Me.llRemoveEnzymeInfo.Text = "Remove Enzyme Info"
        '
        'llAddEnzyme
        '
        Me.llAddEnzyme.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llAddEnzyme.AutoSize = True
        Me.llAddEnzyme.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llAddEnzyme.Location = New System.Drawing.Point(676, 57)
        Me.llAddEnzyme.Name = "llAddEnzyme"
        Me.llAddEnzyme.Size = New System.Drawing.Size(97, 15)
        Me.llAddEnzyme.TabIndex = 11
        Me.llAddEnzyme.TabStop = True
        Me.llAddEnzyme.Text = "Add Enzyme Info"
        '
        'llCancel
        '
        Me.llCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llCancel.AutoSize = True
        Me.llCancel.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llCancel.Location = New System.Drawing.Point(855, 502)
        Me.llCancel.Name = "llCancel"
        Me.llCancel.Size = New System.Drawing.Size(46, 15)
        Me.llCancel.TabIndex = 10
        Me.llCancel.TabStop = True
        Me.llCancel.Text = "Cancel"
        '
        'llOK
        '
        Me.llOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llOK.AutoSize = True
        Me.llOK.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llOK.Location = New System.Drawing.Point(798, 502)
        Me.llOK.Name = "llOK"
        Me.llOK.Size = New System.Drawing.Size(51, 15)
        Me.llOK.TabIndex = 13
        Me.llOK.TabStop = True
        Me.llOK.Text = "Change"
        '
        'llApplyBuffers
        '
        Me.llApplyBuffers.AutoSize = True
        Me.llApplyBuffers.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llApplyBuffers.Location = New System.Drawing.Point(270, 57)
        Me.llApplyBuffers.Name = "llApplyBuffers"
        Me.llApplyBuffers.Size = New System.Drawing.Size(140, 15)
        Me.llApplyBuffers.TabIndex = 12
        Me.llApplyBuffers.TabStop = True
        Me.llApplyBuffers.Text = "Change Buffer Columns"
        '
        'dgvEnzymeInfo
        '
        Me.dgvEnzymeInfo.AllowUserToAddRows = False
        Me.dgvEnzymeInfo.AllowUserToDeleteRows = False
        Me.dgvEnzymeInfo.AllowUserToResizeRows = False
        Me.dgvEnzymeInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvEnzymeInfo.BackgroundColor = System.Drawing.Color.White
        Me.dgvEnzymeInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEnzymeInfo.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clNames, Me.clTemp, Me.clAdditives})
        Me.dgvEnzymeInfo.GridColor = System.Drawing.Color.DarkOrange
        Me.dgvEnzymeInfo.Location = New System.Drawing.Point(273, 75)
        Me.dgvEnzymeInfo.Name = "dgvEnzymeInfo"
        Me.dgvEnzymeInfo.RowHeadersVisible = False
        Me.dgvEnzymeInfo.RowTemplate.Height = 23
        Me.dgvEnzymeInfo.Size = New System.Drawing.Size(625, 424)
        Me.dgvEnzymeInfo.TabIndex = 8
        '
        'clNames
        '
        Me.clNames.HeaderText = "Names"
        Me.clNames.Name = "clNames"
        '
        'clTemp
        '
        Me.clTemp.HeaderText = "Temperature"
        Me.clTemp.Name = "clTemp"
        '
        'clAdditives
        '
        Me.clAdditives.HeaderText = "Additives"
        Me.clAdditives.Name = "clAdditives"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(271, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 12)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Buffers"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(271, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 12)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Name"
        '
        'tbBuffers
        '
        Me.tbBuffers.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbBuffers.Location = New System.Drawing.Point(322, 30)
        Me.tbBuffers.Name = "tbBuffers"
        Me.tbBuffers.Size = New System.Drawing.Size(576, 21)
        Me.tbBuffers.TabIndex = 4
        '
        'tbName
        '
        Me.tbName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbName.Location = New System.Drawing.Point(322, 1)
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(576, 21)
        Me.tbName.TabIndex = 5
        '
        'llHelpDigestionBuffer
        '
        Me.llHelpDigestionBuffer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llHelpDigestionBuffer.AutoSize = True
        Me.llHelpDigestionBuffer.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llHelpDigestionBuffer.Location = New System.Drawing.Point(270, 502)
        Me.llHelpDigestionBuffer.Name = "llHelpDigestionBuffer"
        Me.llHelpDigestionBuffer.Size = New System.Drawing.Size(33, 15)
        Me.llHelpDigestionBuffer.TabIndex = 11
        Me.llHelpDigestionBuffer.TabStop = True
        Me.llHelpDigestionBuffer.Text = "Help"
        '
        'llRemove
        '
        Me.llRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llRemove.AutoSize = True
        Me.llRemove.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llRemove.Location = New System.Drawing.Point(739, 502)
        Me.llRemove.Name = "llRemove"
        Me.llRemove.Size = New System.Drawing.Size(53, 15)
        Me.llRemove.TabIndex = 13
        Me.llRemove.TabStop = True
        Me.llRemove.Text = "Remove"
        '
        'blvList
        '
        Me.blvList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.blvList.CurrentViewStart = 0
        Me.blvList.DataSource = Nothing
        Me.blvList.DoubleSelectIndex = -1
        Me.blvList.Location = New System.Drawing.Point(0, 0)
        Me.blvList.Name = "blvList"
        Me.blvList.Size = New System.Drawing.Size(267, 517)
        Me.blvList.TabIndex = 14
        Me.blvList.TabStop = False
        '
        'LinkLabel1
        '
        Me.LinkLabel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel1.Location = New System.Drawing.Point(705, 502)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(28, 15)
        Me.LinkLabel1.TabIndex = 13
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Add"
        '
        'DigestionManageView
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.Honeydew
        Me.Controls.Add(Me.blvList)
        Me.Controls.Add(Me.llRemoveEnzymeInfo)
        Me.Controls.Add(Me.llHelpDigestionBuffer)
        Me.Controls.Add(Me.llAddEnzyme)
        Me.Controls.Add(Me.llCancel)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.llRemove)
        Me.Controls.Add(Me.llOK)
        Me.Controls.Add(Me.llApplyBuffers)
        Me.Controls.Add(Me.dgvEnzymeInfo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbBuffers)
        Me.Controls.Add(Me.tbName)
        Me.Name = "DigestionManageView"
        Me.Size = New System.Drawing.Size(901, 517)
        CType(Me.dgvEnzymeInfo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.blvList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents llRemoveEnzymeInfo As System.Windows.Forms.LinkLabel
    Friend WithEvents llAddEnzyme As System.Windows.Forms.LinkLabel
    Friend WithEvents llCancel As System.Windows.Forms.LinkLabel
    Friend WithEvents llOK As System.Windows.Forms.LinkLabel
    Friend WithEvents llApplyBuffers As System.Windows.Forms.LinkLabel
    Friend WithEvents dgvEnzymeInfo As System.Windows.Forms.DataGridView
    Friend WithEvents clNames As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clTemp As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clAdditives As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbBuffers As System.Windows.Forms.TextBox
    Friend WithEvents tbName As System.Windows.Forms.TextBox
    Friend WithEvents blvList As Vecute.BufferedListView
    Friend WithEvents llHelpDigestionBuffer As System.Windows.Forms.LinkLabel
    Friend WithEvents llRemove As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel

End Class
