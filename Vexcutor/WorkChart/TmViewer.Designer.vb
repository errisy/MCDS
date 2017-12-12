<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TmViewer
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
        Me.hsbCode = New System.Windows.Forms.HScrollBar
        Me.nudBlock = New System.Windows.Forms.NumericUpDown
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.nudC = New System.Windows.Forms.NumericUpDown
        Me.nudNa = New System.Windows.Forms.NumericUpDown
        Me.nudLow = New System.Windows.Forms.NumericUpDown
        Me.nudHigh = New System.Windows.Forms.NumericUpDown
        Me.Label2 = New System.Windows.Forms.Label
        Me.llClose = New System.Windows.Forms.LinkLabel
        Me.dgvPrimers = New System.Windows.Forms.DataGridView
        Me.nudY = New System.Windows.Forms.NumericUpDown
        Me.nudR = New System.Windows.Forms.NumericUpDown
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.nudTHR = New System.Windows.Forms.NumericUpDown
        Me.bpbView = New MCDS.BufferedPictureBox
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.nudBlock, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudNa, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLow, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHigh, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvPrimers, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudR, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.nudTHR, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bpbView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'hsbCode
        '
        Me.hsbCode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.hsbCode.Location = New System.Drawing.Point(1, 369)
        Me.hsbCode.Name = "hsbCode"
        Me.hsbCode.Size = New System.Drawing.Size(1266, 17)
        Me.hsbCode.TabIndex = 1
        '
        'nudBlock
        '
        Me.nudBlock.Location = New System.Drawing.Point(81, 1)
        Me.nudBlock.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudBlock.Name = "nudBlock"
        Me.nudBlock.Size = New System.Drawing.Size(42, 21)
        Me.nudBlock.TabIndex = 2
        Me.nudBlock.Value = New Decimal(New Integer() {40, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(4, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 12)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Oligo Block"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(131, 4)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(29, 12)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "S/mM"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(223, 4)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(29, 12)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "c/uM"
        '
        'nudC
        '
        Me.nudC.Increment = New Decimal(New Integer() {25, 0, 0, 131072})
        Me.nudC.Location = New System.Drawing.Point(258, 2)
        Me.nudC.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.nudC.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudC.Name = "nudC"
        Me.nudC.Size = New System.Drawing.Size(52, 21)
        Me.nudC.TabIndex = 7
        Me.nudC.Value = New Decimal(New Integer() {625, 0, 0, 0})
        '
        'nudNa
        '
        Me.nudNa.DecimalPlaces = 1
        Me.nudNa.Location = New System.Drawing.Point(166, 2)
        Me.nudNa.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudNa.Minimum = New Decimal(New Integer() {1, 0, 0, 393216})
        Me.nudNa.Name = "nudNa"
        Me.nudNa.Size = New System.Drawing.Size(52, 21)
        Me.nudNa.TabIndex = 6
        Me.nudNa.Value = New Decimal(New Integer() {80, 0, 0, 0})
        '
        'nudLow
        '
        Me.nudLow.Location = New System.Drawing.Point(342, 1)
        Me.nudLow.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me.nudLow.Name = "nudLow"
        Me.nudLow.Size = New System.Drawing.Size(42, 21)
        Me.nudLow.TabIndex = 2
        Me.nudLow.Value = New Decimal(New Integer() {60, 0, 0, 0})
        '
        'nudHigh
        '
        Me.nudHigh.Location = New System.Drawing.Point(413, 1)
        Me.nudHigh.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me.nudHigh.Name = "nudHigh"
        Me.nudHigh.Size = New System.Drawing.Size(42, 21)
        Me.nudHigh.TabIndex = 2
        Me.nudHigh.Value = New Decimal(New Integer() {105, 0, 0, 0})
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(390, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(17, 12)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "to"
        '
        'llClose
        '
        Me.llClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llClose.AutoSize = True
        Me.llClose.Location = New System.Drawing.Point(1232, 3)
        Me.llClose.Name = "llClose"
        Me.llClose.Size = New System.Drawing.Size(35, 12)
        Me.llClose.TabIndex = 8
        Me.llClose.TabStop = True
        Me.llClose.Text = "Close"
        '
        'dgvPrimers
        '
        Me.dgvPrimers.AllowUserToAddRows = False
        Me.dgvPrimers.AllowUserToDeleteRows = False
        Me.dgvPrimers.AllowUserToResizeRows = False
        Me.dgvPrimers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPrimers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column9})
        Me.dgvPrimers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvPrimers.Location = New System.Drawing.Point(0, 0)
        Me.dgvPrimers.Name = "dgvPrimers"
        Me.dgvPrimers.ReadOnly = True
        Me.dgvPrimers.RowTemplate.Height = 23
        Me.dgvPrimers.Size = New System.Drawing.Size(1267, 186)
        Me.dgvPrimers.TabIndex = 11
        '
        'nudY
        '
        Me.nudY.DecimalPlaces = 1
        Me.nudY.Location = New System.Drawing.Point(512, 2)
        Me.nudY.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me.nudY.Name = "nudY"
        Me.nudY.Size = New System.Drawing.Size(50, 21)
        Me.nudY.TabIndex = 2
        Me.nudY.Value = New Decimal(New Integer() {90, 0, 0, 0})
        '
        'nudR
        '
        Me.nudR.DecimalPlaces = 1
        Me.nudR.Location = New System.Drawing.Point(613, 2)
        Me.nudR.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me.nudR.Name = "nudR"
        Me.nudR.Size = New System.Drawing.Size(50, 21)
        Me.nudR.TabIndex = 2
        Me.nudR.Value = New Decimal(New Integer() {95, 0, 0, 0})
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(465, 4)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 12)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Yellow"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(584, 4)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(23, 12)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Red"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(685, 1)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(75, 23)
        Me.btnSearch.TabIndex = 12
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.nudTHR)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnSearch)
        Me.SplitContainer1.Panel1.Controls.Add(Me.bpbView)
        Me.SplitContainer1.Panel1.Controls.Add(Me.llClose)
        Me.SplitContainer1.Panel1.Controls.Add(Me.hsbCode)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label7)
        Me.SplitContainer1.Panel1.Controls.Add(Me.nudBlock)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label8)
        Me.SplitContainer1.Panel1.Controls.Add(Me.nudLow)
        Me.SplitContainer1.Panel1.Controls.Add(Me.nudC)
        Me.SplitContainer1.Panel1.Controls.Add(Me.nudHigh)
        Me.SplitContainer1.Panel1.Controls.Add(Me.nudNa)
        Me.SplitContainer1.Panel1.Controls.Add(Me.nudY)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.nudR)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.dgvPrimers)
        Me.SplitContainer1.Size = New System.Drawing.Size(1267, 576)
        Me.SplitContainer1.SplitterDistance = 386
        Me.SplitContainer1.TabIndex = 13
        '
        'nudTHR
        '
        Me.nudTHR.Location = New System.Drawing.Point(766, 1)
        Me.nudTHR.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudTHR.Name = "nudTHR"
        Me.nudTHR.Size = New System.Drawing.Size(67, 21)
        Me.nudTHR.TabIndex = 13
        Me.nudTHR.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'bpbView
        '
        Me.bpbView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bpbView.Location = New System.Drawing.Point(1, 22)
        Me.bpbView.Name = "bpbView"
        Me.bpbView.Size = New System.Drawing.Size(1266, 346)
        Me.bpbView.TabIndex = 0
        Me.bpbView.TabStop = False
        '
        'Column1
        '
        Me.Column1.HeaderText = "ID"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.HeaderText = "Start"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.HeaderText = "End"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'Column4
        '
        Me.Column4.HeaderText = "Max Tm"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        '
        'Column5
        '
        Me.Column5.HeaderText = "Average Tm"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        '
        'Column6
        '
        Me.Column6.HeaderText = "Yellow Score"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        '
        'Column9
        '
        Me.Column9.HeaderText = "Length"
        Me.Column9.Name = "Column9"
        Me.Column9.ReadOnly = True
        '
        'TmViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Beige
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "TmViewer"
        Me.Size = New System.Drawing.Size(1267, 576)
        CType(Me.nudBlock, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudNa, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLow, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHigh, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvPrimers, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudR, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.nudTHR, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bpbView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bpbView As MCDS.BufferedPictureBox
    Friend WithEvents hsbCode As System.Windows.Forms.HScrollBar
    Friend WithEvents nudBlock As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents nudC As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudNa As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudLow As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudHigh As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents llClose As System.Windows.Forms.LinkLabel
    Friend WithEvents dgvPrimers As System.Windows.Forms.DataGridView
    Friend WithEvents nudY As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudR As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents nudTHR As System.Windows.Forms.NumericUpDown
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column9 As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
