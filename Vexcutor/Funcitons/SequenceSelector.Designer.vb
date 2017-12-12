<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SequenceViewer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SequenceViewer))
        Me.pnlSeq = New System.Windows.Forms.Panel
        Me.pbSeq = New System.Windows.Forms.PictureBox
        Me.tbF = New System.Windows.Forms.TextBox
        Me.tbR = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.nudTMFrom = New System.Windows.Forms.NumericUpDown
        Me.nudTMTo = New System.Windows.Forms.NumericUpDown
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.pnlMap = New System.Windows.Forms.Panel
        Me.infoF = New System.Windows.Forms.Label
        Me.infoR = New System.Windows.Forms.Label
        Me.nudNa = New System.Windows.Forms.NumericUpDown
        Me.nudC = New System.Windows.Forms.NumericUpDown
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.pbScroll = New System.Windows.Forms.PictureBox
        Me.lbTask = New System.Windows.Forms.Label
        Me.llCancel = New System.Windows.Forms.LinkLabel
        Me.llF = New System.Windows.Forms.LinkLabel
        Me.llProducts = New System.Windows.Forms.LinkLabel
        Me.lbSelection = New System.Windows.Forms.Label
        Me.pafPrimer = New Vecute.PrimerAnalysisFrame
        Me.pbMap = New Vecute.VectorMap
        Me.llR = New System.Windows.Forms.LinkLabel
        Me.llPair = New System.Windows.Forms.LinkLabel
        Me.llSelect = New System.Windows.Forms.LinkLabel
        Me.pnlSeq.SuspendLayout()
        CType(Me.pbSeq, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudTMFrom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudTMTo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlMap.SuspendLayout()
        CType(Me.nudNa, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbScroll, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pafPrimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbMap, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlSeq
        '
        Me.pnlSeq.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pnlSeq.AutoScroll = True
        Me.pnlSeq.Controls.Add(Me.pbScroll)
        Me.pnlSeq.Controls.Add(Me.pbSeq)
        Me.pnlSeq.Location = New System.Drawing.Point(0, 139)
        Me.pnlSeq.Name = "pnlSeq"
        Me.pnlSeq.Size = New System.Drawing.Size(785, 319)
        Me.pnlSeq.TabIndex = 0
        '
        'pbSeq
        '
        Me.pbSeq.Location = New System.Drawing.Point(0, 0)
        Me.pbSeq.Name = "pbSeq"
        Me.pbSeq.Size = New System.Drawing.Size(763, 50)
        Me.pbSeq.TabIndex = 0
        Me.pbSeq.TabStop = False
        '
        'tbF
        '
        Me.tbF.Location = New System.Drawing.Point(82, 3)
        Me.tbF.Name = "tbF"
        Me.tbF.Size = New System.Drawing.Size(499, 21)
        Me.tbF.TabIndex = 1
        '
        'tbR
        '
        Me.tbR.Location = New System.Drawing.Point(82, 26)
        Me.tbR.Name = "tbR"
        Me.tbR.Size = New System.Drawing.Size(499, 21)
        Me.tbR.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(13, 15)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "F"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(15, 15)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "R"
        '
        'nudTMFrom
        '
        Me.nudTMFrom.DecimalPlaces = 1
        Me.nudTMFrom.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
        Me.nudTMFrom.Location = New System.Drawing.Point(789, 4)
        Me.nudTMFrom.Minimum = New Decimal(New Integer() {40, 0, 0, 0})
        Me.nudTMFrom.Name = "nudTMFrom"
        Me.nudTMFrom.Size = New System.Drawing.Size(52, 21)
        Me.nudTMFrom.TabIndex = 3
        Me.nudTMFrom.Value = New Decimal(New Integer() {56, 0, 0, 0})
        '
        'nudTMTo
        '
        Me.nudTMTo.DecimalPlaces = 1
        Me.nudTMTo.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
        Me.nudTMTo.Location = New System.Drawing.Point(789, 27)
        Me.nudTMTo.Minimum = New Decimal(New Integer() {40, 0, 0, 0})
        Me.nudTMTo.Name = "nudTMTo"
        Me.nudTMTo.Size = New System.Drawing.Size(52, 21)
        Me.nudTMTo.TabIndex = 3
        Me.nudTMTo.Value = New Decimal(New Integer() {65, 0, 0, 0})
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(749, 6)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(36, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "From"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(749, 29)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(21, 15)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "To"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(20, 3)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(57, 21)
        Me.TextBox3.TabIndex = 1
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(20, 26)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(57, 21)
        Me.TextBox4.TabIndex = 1
        '
        'pnlMap
        '
        Me.pnlMap.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlMap.AutoScroll = True
        Me.pnlMap.Controls.Add(Me.pbMap)
        Me.pnlMap.Location = New System.Drawing.Point(789, 139)
        Me.pnlMap.Name = "pnlMap"
        Me.pnlMap.Size = New System.Drawing.Size(411, 319)
        Me.pnlMap.TabIndex = 5
        '
        'infoF
        '
        Me.infoF.AutoSize = True
        Me.infoF.Location = New System.Drawing.Point(583, 6)
        Me.infoF.Name = "infoF"
        Me.infoF.Size = New System.Drawing.Size(25, 15)
        Me.infoF.TabIndex = 2
        Me.infoF.Text = "Tm"
        '
        'infoR
        '
        Me.infoR.AutoSize = True
        Me.infoR.Location = New System.Drawing.Point(583, 29)
        Me.infoR.Name = "infoR"
        Me.infoR.Size = New System.Drawing.Size(25, 15)
        Me.infoR.TabIndex = 2
        Me.infoR.Text = "Tm"
        '
        'nudNa
        '
        Me.nudNa.DecimalPlaces = 1
        Me.nudNa.Location = New System.Drawing.Point(895, 4)
        Me.nudNa.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudNa.Minimum = New Decimal(New Integer() {1, 0, 0, 393216})
        Me.nudNa.Name = "nudNa"
        Me.nudNa.Size = New System.Drawing.Size(52, 21)
        Me.nudNa.TabIndex = 3
        Me.nudNa.Value = New Decimal(New Integer() {80, 0, 0, 0})
        '
        'nudC
        '
        Me.nudC.Increment = New Decimal(New Integer() {25, 0, 0, 131072})
        Me.nudC.Location = New System.Drawing.Point(895, 27)
        Me.nudC.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.nudC.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudC.Name = "nudC"
        Me.nudC.Size = New System.Drawing.Size(52, 21)
        Me.nudC.TabIndex = 3
        Me.nudC.Value = New Decimal(New Integer() {625, 0, 0, 0})
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(847, 6)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(39, 15)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "S/mM"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(847, 29)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(34, 15)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "c/uM"
        '
        'pbScroll
        '
        Me.pbScroll.BackColor = System.Drawing.Color.White
        Me.pbScroll.Location = New System.Drawing.Point(0, 0)
        Me.pbScroll.Margin = New System.Windows.Forms.Padding(0)
        Me.pbScroll.Name = "pbScroll"
        Me.pbScroll.Size = New System.Drawing.Size(1, 1)
        Me.pbScroll.TabIndex = 1
        Me.pbScroll.TabStop = False
        '
        'lbTask
        '
        Me.lbTask.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbTask.AutoSize = True
        Me.lbTask.Location = New System.Drawing.Point(987, 3)
        Me.lbTask.Name = "lbTask"
        Me.lbTask.Size = New System.Drawing.Size(35, 15)
        Me.lbTask.TabIndex = 7
        Me.lbTask.Text = "Task"
        '
        'llCancel
        '
        Me.llCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llCancel.AutoSize = True
        Me.llCancel.Location = New System.Drawing.Point(1075, 3)
        Me.llCancel.Name = "llCancel"
        Me.llCancel.Size = New System.Drawing.Size(46, 15)
        Me.llCancel.TabIndex = 8
        Me.llCancel.TabStop = True
        Me.llCancel.Text = "Cancel"
        '
        'llF
        '
        Me.llF.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llF.AutoSize = True
        Me.llF.Location = New System.Drawing.Point(1127, 3)
        Me.llF.Name = "llF"
        Me.llF.Size = New System.Drawing.Size(13, 15)
        Me.llF.TabIndex = 8
        Me.llF.TabStop = True
        Me.llF.Text = "F"
        '
        'llProducts
        '
        Me.llProducts.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llProducts.AutoSize = True
        Me.llProducts.Location = New System.Drawing.Point(1083, 32)
        Me.llProducts.Name = "llProducts"
        Me.llProducts.Size = New System.Drawing.Size(117, 15)
        Me.llProducts.TabIndex = 8
        Me.llProducts.TabStop = True
        Me.llProducts.Text = "View PCR Products"
        '
        'lbSelection
        '
        Me.lbSelection.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbSelection.AutoSize = True
        Me.lbSelection.BackColor = System.Drawing.Color.LightPink
        Me.lbSelection.Location = New System.Drawing.Point(1045, 116)
        Me.lbSelection.Name = "lbSelection"
        Me.lbSelection.Size = New System.Drawing.Size(46, 15)
        Me.lbSelection.TabIndex = 7
        Me.lbSelection.Text = "Select:"
        '
        'pafPrimer
        '
        Me.pafPrimer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pafPrimer.Location = New System.Drawing.Point(0, 47)
        Me.pafPrimer.Name = "pafPrimer"
        Me.pafPrimer.ShowSequencing = True
        Me.pafPrimer.Size = New System.Drawing.Size(1200, 86)
        Me.pafPrimer.TabIndex = 6
        Me.pafPrimer.TabStop = False
        '
        'pbMap
        '
        Me.pbMap.GeneFile = Nothing
        Me.pbMap.Location = New System.Drawing.Point(0, 0)
        Me.pbMap.Name = "pbMap"
        Me.pbMap.RestrictionSite = CType(resources.GetObject("pbMap.RestrictionSite"), System.Collections.Generic.List(Of String))
        Me.pbMap.Size = New System.Drawing.Size(453, 333)
        Me.pbMap.TabIndex = 0
        Me.pbMap.TabStop = False
        '
        'llR
        '
        Me.llR.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llR.AutoSize = True
        Me.llR.Location = New System.Drawing.Point(1146, 3)
        Me.llR.Name = "llR"
        Me.llR.Size = New System.Drawing.Size(15, 15)
        Me.llR.TabIndex = 8
        Me.llR.TabStop = True
        Me.llR.Text = "R"
        '
        'llPair
        '
        Me.llPair.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llPair.AutoSize = True
        Me.llPair.Location = New System.Drawing.Point(1167, 3)
        Me.llPair.Name = "llPair"
        Me.llPair.Size = New System.Drawing.Size(30, 15)
        Me.llPair.TabIndex = 8
        Me.llPair.TabStop = True
        Me.llPair.Text = "Pair"
        '
        'llSelect
        '
        Me.llSelect.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llSelect.AutoSize = True
        Me.llSelect.Location = New System.Drawing.Point(1153, 3)
        Me.llSelect.Name = "llSelect"
        Me.llSelect.Size = New System.Drawing.Size(43, 15)
        Me.llSelect.TabIndex = 8
        Me.llSelect.TabStop = True
        Me.llSelect.Text = "Select"
        Me.llSelect.Visible = False
        '
        'SequenceViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.PaleTurquoise
        Me.Controls.Add(Me.llPair)
        Me.Controls.Add(Me.llR)
        Me.Controls.Add(Me.llF)
        Me.Controls.Add(Me.llProducts)
        Me.Controls.Add(Me.llSelect)
        Me.Controls.Add(Me.llCancel)
        Me.Controls.Add(Me.lbSelection)
        Me.Controls.Add(Me.lbTask)
        Me.Controls.Add(Me.pafPrimer)
        Me.Controls.Add(Me.pnlMap)
        Me.Controls.Add(Me.nudTMTo)
        Me.Controls.Add(Me.nudC)
        Me.Controls.Add(Me.nudNa)
        Me.Controls.Add(Me.nudTMFrom)
        Me.Controls.Add(Me.infoR)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.infoF)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.tbR)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.tbF)
        Me.Controls.Add(Me.pnlSeq)
        Me.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "SequenceViewer"
        Me.Size = New System.Drawing.Size(1200, 463)
        Me.pnlSeq.ResumeLayout(False)
        CType(Me.pbSeq, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudTMFrom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudTMTo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlMap.ResumeLayout(False)
        CType(Me.nudNa, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbScroll, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pafPrimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbMap, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbF As System.Windows.Forms.TextBox
    Friend WithEvents tbR As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents pbSeq As System.Windows.Forms.PictureBox
    Friend WithEvents nudTMFrom As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudTMTo As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents pnlSeq As System.Windows.Forms.Panel
    Friend WithEvents pnlMap As System.Windows.Forms.Panel
    Friend WithEvents pbMap As VectorMap
    Friend WithEvents infoF As System.Windows.Forms.Label
    Friend WithEvents infoR As System.Windows.Forms.Label
    Friend WithEvents nudNa As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudC As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents pafPrimer As Vecute.PrimerAnalysisFrame
    Friend WithEvents pbScroll As System.Windows.Forms.PictureBox
    Friend WithEvents lbTask As System.Windows.Forms.Label
    Friend WithEvents llCancel As System.Windows.Forms.LinkLabel
    Friend WithEvents llF As System.Windows.Forms.LinkLabel
    Friend WithEvents llProducts As System.Windows.Forms.LinkLabel
    Friend WithEvents lbSelection As System.Windows.Forms.Label
    Friend WithEvents llR As System.Windows.Forms.LinkLabel
    Friend WithEvents llPair As System.Windows.Forms.LinkLabel
    Friend WithEvents llSelect As System.Windows.Forms.LinkLabel

End Class
