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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SequenceViewer))
        Me.pnlMap = New System.Windows.Forms.Panel()
        Me.pbMap = New MCDS.VectorMap()
        Me.pnlSeq = New System.Windows.Forms.Panel()
        Me.vsbSeq = New System.Windows.Forms.VScrollBar()
        Me.pbSeq = New System.Windows.Forms.PictureBox()
        Me.tbF = New System.Windows.Forms.TextBox()
        Me.tbR = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.nudTMFrom = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.tbFKey = New System.Windows.Forms.TextBox()
        Me.tbRKey = New System.Windows.Forms.TextBox()
        Me.infoF = New System.Windows.Forms.Label()
        Me.infoR = New System.Windows.Forms.Label()
        Me.nudNa = New System.Windows.Forms.NumericUpDown()
        Me.nudC = New System.Windows.Forms.NumericUpDown()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.llFF = New System.Windows.Forms.LinkLabel()
        Me.lbSelection = New System.Windows.Forms.Label()
        Me.llFR = New System.Windows.Forms.LinkLabel()
        Me.llPair = New System.Windows.Forms.LinkLabel()
        Me.llSelectPart = New System.Windows.Forms.LinkLabel()
        Me.llRF = New System.Windows.Forms.LinkLabel()
        Me.llRR = New System.Windows.Forms.LinkLabel()
        Me.llSelectWhole = New System.Windows.Forms.LinkLabel()
        Me.llClose = New System.Windows.Forms.LinkLabel()
        Me.pnlPrimer = New System.Windows.Forms.Panel()
        Me.iHostFind = New MCDS.InteropHost()
        Me.nudWithin = New System.Windows.Forms.NumericUpDown()
        Me.llSendR = New System.Windows.Forms.LinkLabel()
        Me.llSendF = New System.Windows.Forms.LinkLabel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmsSendTo = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.pnlVec = New System.Windows.Forms.Panel()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.tbEnzymes = New System.Windows.Forms.TextBox()
        Me.llEnzymeSwitch = New System.Windows.Forms.LinkLabel()
        Me.lbEnzymes = New System.Windows.Forms.Label()
        Me.pnlEnzymes = New System.Windows.Forms.Panel()
        Me.llApplyDNAName = New System.Windows.Forms.LinkLabel()
        Me.tbDNAName = New System.Windows.Forms.TextBox()
        Me.lbDNAName = New System.Windows.Forms.Label()
        Me.lbFileAddress = New System.Windows.Forms.Label()
        Me.pafPrimer = New MCDS.PrimerAnalysisFrame()
        Me.ttHelp = New System.Windows.Forms.ToolTip(Me.components)
        Me.snbPixelPerKBP = New MCDS.ScrollingNumberBox()
        Me.lblPixelPerBP = New System.Windows.Forms.Label()
        Me.pnlMap.SuspendLayout()
        CType(Me.pbMap, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSeq.SuspendLayout()
        CType(Me.pbSeq, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudTMFrom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudNa, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudC, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPrimer.SuspendLayout()
        CType(Me.nudWithin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlVec.SuspendLayout()
        Me.pnlEnzymes.SuspendLayout()
        CType(Me.pafPrimer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlMap
        '
        Me.pnlMap.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlMap.AutoScroll = True
        Me.pnlMap.BackColor = System.Drawing.Color.White
        Me.pnlMap.Controls.Add(Me.pbMap)
        Me.pnlMap.Location = New System.Drawing.Point(610, 0)
        Me.pnlMap.Name = "pnlMap"
        Me.pnlMap.Size = New System.Drawing.Size(711, 407)
        Me.pnlMap.TabIndex = 5
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
        'pnlSeq
        '
        Me.pnlSeq.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pnlSeq.Controls.Add(Me.vsbSeq)
        Me.pnlSeq.Controls.Add(Me.pbSeq)
        Me.pnlSeq.Location = New System.Drawing.Point(0, 0)
        Me.pnlSeq.Name = "pnlSeq"
        Me.pnlSeq.Size = New System.Drawing.Size(610, 407)
        Me.pnlSeq.TabIndex = 0
        '
        'vsbSeq
        '
        Me.vsbSeq.Dock = System.Windows.Forms.DockStyle.Right
        Me.vsbSeq.Location = New System.Drawing.Point(593, 0)
        Me.vsbSeq.Name = "vsbSeq"
        Me.vsbSeq.Size = New System.Drawing.Size(17, 407)
        Me.vsbSeq.TabIndex = 2
        '
        'pbSeq
        '
        Me.pbSeq.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pbSeq.Location = New System.Drawing.Point(0, 0)
        Me.pbSeq.Name = "pbSeq"
        Me.pbSeq.Size = New System.Drawing.Size(610, 407)
        Me.pbSeq.TabIndex = 0
        Me.pbSeq.TabStop = False
        '
        'tbF
        '
        Me.tbF.Location = New System.Drawing.Point(82, 3)
        Me.tbF.Name = "tbF"
        Me.tbF.Size = New System.Drawing.Size(499, 21)
        Me.tbF.TabIndex = 2
        '
        'tbR
        '
        Me.tbR.Location = New System.Drawing.Point(82, 26)
        Me.tbR.Name = "tbR"
        Me.tbR.Size = New System.Drawing.Size(499, 21)
        Me.tbR.TabIndex = 4
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
        Me.nudTMFrom.Location = New System.Drawing.Point(786, 4)
        Me.nudTMFrom.Minimum = New Decimal(New Integer() {40, 0, 0, 0})
        Me.nudTMFrom.Name = "nudTMFrom"
        Me.nudTMFrom.Size = New System.Drawing.Size(44, 21)
        Me.nudTMFrom.TabIndex = 5
        Me.nudTMFrom.Value = New Decimal(New Integer() {59, 0, 0, 0})
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(749, 6)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(35, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Tm >"
        '
        'tbFKey
        '
        Me.tbFKey.Location = New System.Drawing.Point(20, 3)
        Me.tbFKey.Name = "tbFKey"
        Me.tbFKey.Size = New System.Drawing.Size(57, 21)
        Me.tbFKey.TabIndex = 1
        '
        'tbRKey
        '
        Me.tbRKey.Location = New System.Drawing.Point(20, 26)
        Me.tbRKey.Name = "tbRKey"
        Me.tbRKey.Size = New System.Drawing.Size(57, 21)
        Me.tbRKey.TabIndex = 3
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
        Me.nudNa.TabIndex = 7
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
        Me.nudC.TabIndex = 8
        Me.nudC.Value = New Decimal(New Integer() {625, 0, 0, 0})
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(859, 6)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(39, 15)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "S/mM"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(862, 29)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(34, 15)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "c/uM"
        '
        'llFF
        '
        Me.llFF.AutoSize = True
        Me.llFF.Location = New System.Drawing.Point(953, 6)
        Me.llFF.Name = "llFF"
        Me.llFF.Size = New System.Drawing.Size(13, 15)
        Me.llFF.TabIndex = 8
        Me.llFF.TabStop = True
        Me.llFF.Text = "F"
        Me.llFF.Visible = False
        '
        'lbSelection
        '
        Me.lbSelection.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbSelection.AutoSize = True
        Me.lbSelection.BackColor = System.Drawing.Color.Lavender
        Me.lbSelection.Location = New System.Drawing.Point(1208, 32)
        Me.lbSelection.Name = "lbSelection"
        Me.lbSelection.Size = New System.Drawing.Size(10, 15)
        Me.lbSelection.TabIndex = 7
        Me.lbSelection.Text = ":"
        '
        'llFR
        '
        Me.llFR.AutoSize = True
        Me.llFR.Location = New System.Drawing.Point(966, 6)
        Me.llFR.Name = "llFR"
        Me.llFR.Size = New System.Drawing.Size(15, 15)
        Me.llFR.TabIndex = 8
        Me.llFR.TabStop = True
        Me.llFR.Text = "R"
        Me.llFR.Visible = False
        '
        'llPair
        '
        Me.llPair.AutoSize = True
        Me.llPair.Location = New System.Drawing.Point(999, 6)
        Me.llPair.Name = "llPair"
        Me.llPair.Size = New System.Drawing.Size(30, 15)
        Me.llPair.TabIndex = 8
        Me.llPair.TabStop = True
        Me.llPair.Text = "Pair"
        Me.llPair.Visible = False
        '
        'llSelectPart
        '
        Me.llSelectPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llSelectPart.AutoSize = True
        Me.llSelectPart.Enabled = False
        Me.llSelectPart.Location = New System.Drawing.Point(1162, 32)
        Me.llSelectPart.Name = "llSelectPart"
        Me.llSelectPart.Size = New System.Drawing.Size(43, 15)
        Me.llSelectPart.TabIndex = 8
        Me.llSelectPart.TabStop = True
        Me.llSelectPart.Text = "Select"
        '
        'llRF
        '
        Me.llRF.AutoSize = True
        Me.llRF.Location = New System.Drawing.Point(968, 29)
        Me.llRF.Name = "llRF"
        Me.llRF.Size = New System.Drawing.Size(13, 15)
        Me.llRF.TabIndex = 8
        Me.llRF.TabStop = True
        Me.llRF.Text = "F"
        Me.llRF.Visible = False
        '
        'llRR
        '
        Me.llRR.AutoSize = True
        Me.llRR.Location = New System.Drawing.Point(952, 29)
        Me.llRR.Name = "llRR"
        Me.llRR.Size = New System.Drawing.Size(15, 15)
        Me.llRR.TabIndex = 8
        Me.llRR.TabStop = True
        Me.llRR.Text = "R"
        Me.llRR.Visible = False
        '
        'llSelectWhole
        '
        Me.llSelectWhole.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llSelectWhole.AutoSize = True
        Me.llSelectWhole.Location = New System.Drawing.Point(1162, 6)
        Me.llSelectWhole.Name = "llSelectWhole"
        Me.llSelectWhole.Size = New System.Drawing.Size(82, 15)
        Me.llSelectWhole.TabIndex = 8
        Me.llSelectWhole.TabStop = True
        Me.llSelectWhole.Text = "Select Whole"
        Me.llSelectWhole.Visible = False
        '
        'llClose
        '
        Me.llClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llClose.AutoSize = True
        Me.llClose.LinkColor = System.Drawing.Color.Red
        Me.llClose.Location = New System.Drawing.Point(1282, 6)
        Me.llClose.Name = "llClose"
        Me.llClose.Size = New System.Drawing.Size(39, 15)
        Me.llClose.TabIndex = 8
        Me.llClose.TabStop = True
        Me.llClose.Text = "Close"
        '
        'pnlPrimer
        '
        Me.pnlPrimer.Controls.Add(Me.iHostFind)
        Me.pnlPrimer.Controls.Add(Me.nudTMFrom)
        Me.pnlPrimer.Controls.Add(Me.nudWithin)
        Me.pnlPrimer.Controls.Add(Me.nudC)
        Me.pnlPrimer.Controls.Add(Me.nudNa)
        Me.pnlPrimer.Controls.Add(Me.Label1)
        Me.pnlPrimer.Controls.Add(Me.llSendR)
        Me.pnlPrimer.Controls.Add(Me.llSendF)
        Me.pnlPrimer.Controls.Add(Me.llPair)
        Me.pnlPrimer.Controls.Add(Me.tbF)
        Me.pnlPrimer.Controls.Add(Me.llRR)
        Me.pnlPrimer.Controls.Add(Me.tbFKey)
        Me.pnlPrimer.Controls.Add(Me.llFR)
        Me.pnlPrimer.Controls.Add(Me.tbR)
        Me.pnlPrimer.Controls.Add(Me.llRF)
        Me.pnlPrimer.Controls.Add(Me.tbRKey)
        Me.pnlPrimer.Controls.Add(Me.llFF)
        Me.pnlPrimer.Controls.Add(Me.Label5)
        Me.pnlPrimer.Controls.Add(Me.Label4)
        Me.pnlPrimer.Controls.Add(Me.Label3)
        Me.pnlPrimer.Controls.Add(Me.Label7)
        Me.pnlPrimer.Controls.Add(Me.Label8)
        Me.pnlPrimer.Controls.Add(Me.infoF)
        Me.pnlPrimer.Controls.Add(Me.Label2)
        Me.pnlPrimer.Controls.Add(Me.infoR)
        Me.pnlPrimer.Location = New System.Drawing.Point(0, 0)
        Me.pnlPrimer.Name = "pnlPrimer"
        Me.pnlPrimer.Size = New System.Drawing.Size(1032, 47)
        Me.pnlPrimer.TabIndex = 9
        '
        'iHostFind
        '
        Me.iHostFind.Location = New System.Drawing.Point(741, 25)
        Me.iHostFind.Name = "iHostFind"
        Me.iHostFind.Size = New System.Drawing.Size(45, 17)
        Me.iHostFind.TabIndex = 9
        Me.iHostFind.Child = Nothing
        '
        'nudWithin
        '
        Me.nudWithin.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
        Me.nudWithin.Location = New System.Drawing.Point(786, 24)
        Me.nudWithin.Maximum = New Decimal(New Integer() {2000, 0, 0, 0})
        Me.nudWithin.Minimum = New Decimal(New Integer() {50, 0, 0, 0})
        Me.nudWithin.Name = "nudWithin"
        Me.nudWithin.Size = New System.Drawing.Size(48, 21)
        Me.nudWithin.TabIndex = 6
        Me.nudWithin.Value = New Decimal(New Integer() {250, 0, 0, 0})
        '
        'llSendR
        '
        Me.llSendR.AutoSize = True
        Me.llSendR.LinkColor = System.Drawing.Color.Fuchsia
        Me.llSendR.Location = New System.Drawing.Point(981, 29)
        Me.llSendR.Name = "llSendR"
        Me.llSendR.Size = New System.Drawing.Size(15, 15)
        Me.llSendR.TabIndex = 8
        Me.llSendR.TabStop = True
        Me.llSendR.Text = "S"
        Me.llSendR.Visible = False
        '
        'llSendF
        '
        Me.llSendF.AutoSize = True
        Me.llSendF.LinkColor = System.Drawing.Color.Fuchsia
        Me.llSendF.Location = New System.Drawing.Point(981, 6)
        Me.llSendF.Name = "llSendF"
        Me.llSendF.Size = New System.Drawing.Size(15, 15)
        Me.llSendF.TabIndex = 8
        Me.llSendF.TabStop = True
        Me.llSendF.Text = "S"
        Me.llSendF.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(832, 28)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(21, 15)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "bp"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(827, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(20, 15)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "°C"
        '
        'cmsSendTo
        '
        Me.cmsSendTo.Name = "cmsSendTo"
        Me.cmsSendTo.Size = New System.Drawing.Size(61, 4)
        '
        'pnlVec
        '
        Me.pnlVec.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlVec.Controls.Add(Me.Splitter1)
        Me.pnlVec.Controls.Add(Me.pnlMap)
        Me.pnlVec.Controls.Add(Me.pnlSeq)
        Me.pnlVec.Location = New System.Drawing.Point(3, 132)
        Me.pnlVec.Name = "pnlVec"
        Me.pnlVec.Size = New System.Drawing.Size(1321, 407)
        Me.pnlVec.TabIndex = 10
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(0, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 407)
        Me.Splitter1.TabIndex = 1
        Me.Splitter1.TabStop = False
        '
        'tbEnzymes
        '
        Me.tbEnzymes.Location = New System.Drawing.Point(406, 24)
        Me.tbEnzymes.Name = "tbEnzymes"
        Me.tbEnzymes.Size = New System.Drawing.Size(622, 21)
        Me.tbEnzymes.TabIndex = 11
        '
        'llEnzymeSwitch
        '
        Me.llEnzymeSwitch.AutoSize = True
        Me.llEnzymeSwitch.Location = New System.Drawing.Point(1038, 4)
        Me.llEnzymeSwitch.Name = "llEnzymeSwitch"
        Me.llEnzymeSwitch.Size = New System.Drawing.Size(79, 15)
        Me.llEnzymeSwitch.TabIndex = 12
        Me.llEnzymeSwitch.TabStop = True
        Me.llEnzymeSwitch.Text = "EnzymeView"
        Me.llEnzymeSwitch.Visible = False
        '
        'lbEnzymes
        '
        Me.lbEnzymes.AutoSize = True
        Me.lbEnzymes.Location = New System.Drawing.Point(403, 6)
        Me.lbEnzymes.Name = "lbEnzymes"
        Me.lbEnzymes.Size = New System.Drawing.Size(44, 15)
        Me.lbEnzymes.TabIndex = 13
        Me.lbEnzymes.Text = "[None]"
        '
        'pnlEnzymes
        '
        Me.pnlEnzymes.Controls.Add(Me.llApplyDNAName)
        Me.pnlEnzymes.Controls.Add(Me.tbDNAName)
        Me.pnlEnzymes.Controls.Add(Me.lbDNAName)
        Me.pnlEnzymes.Controls.Add(Me.lbFileAddress)
        Me.pnlEnzymes.Controls.Add(Me.tbEnzymes)
        Me.pnlEnzymes.Controls.Add(Me.lbEnzymes)
        Me.pnlEnzymes.Location = New System.Drawing.Point(0, 0)
        Me.pnlEnzymes.Name = "pnlEnzymes"
        Me.pnlEnzymes.Size = New System.Drawing.Size(1032, 48)
        Me.pnlEnzymes.TabIndex = 14
        '
        'llApplyDNAName
        '
        Me.llApplyDNAName.AutoSize = True
        Me.llApplyDNAName.Location = New System.Drawing.Point(314, 26)
        Me.llApplyDNAName.Name = "llApplyDNAName"
        Me.llApplyDNAName.Size = New System.Drawing.Size(86, 15)
        Me.llApplyDNAName.TabIndex = 16
        Me.llApplyDNAName.TabStop = True
        Me.llApplyDNAName.Text = "Change Name"
        '
        'tbDNAName
        '
        Me.tbDNAName.Location = New System.Drawing.Point(76, 24)
        Me.tbDNAName.Name = "tbDNAName"
        Me.tbDNAName.Size = New System.Drawing.Size(232, 21)
        Me.tbDNAName.TabIndex = 15
        '
        'lbDNAName
        '
        Me.lbDNAName.AutoSize = True
        Me.lbDNAName.Location = New System.Drawing.Point(3, 26)
        Me.lbDNAName.Name = "lbDNAName"
        Me.lbDNAName.Size = New System.Drawing.Size(67, 15)
        Me.lbDNAName.TabIndex = 14
        Me.lbDNAName.Text = "DNA Name"
        '
        'lbFileAddress
        '
        Me.lbFileAddress.AutoSize = True
        Me.lbFileAddress.Location = New System.Drawing.Point(3, 6)
        Me.lbFileAddress.Name = "lbFileAddress"
        Me.lbFileAddress.Size = New System.Drawing.Size(115, 15)
        Me.lbFileAddress.TabIndex = 14
        Me.lbFileAddress.Text = "[File Location] - N/A"
        '
        'pafPrimer
        '
        Me.pafPrimer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pafPrimer.Location = New System.Drawing.Point(0, 47)
        Me.pafPrimer.Name = "pafPrimer"
        Me.pafPrimer.ShowSequencing = True
        Me.pafPrimer.Size = New System.Drawing.Size(1324, 86)
        Me.pafPrimer.TabIndex = 6
        Me.pafPrimer.TabStop = False
        '
        'snbPixelPerKBP
        '
        Me.snbPixelPerKBP.IncrementValue = 100
        Me.snbPixelPerKBP.Location = New System.Drawing.Point(1045, 24)
        Me.snbPixelPerKBP.Maximum = 4000
        Me.snbPixelPerKBP.Minimum = 0
        Me.snbPixelPerKBP.Name = "snbPixelPerKBP"
        Me.snbPixelPerKBP.Size = New System.Drawing.Size(45, 21)
        Me.snbPixelPerKBP.TabIndex = 9
        Me.snbPixelPerKBP.Text = "0"
        Me.snbPixelPerKBP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.snbPixelPerKBP.Value = 0
        '
        'lblPixelPerBP
        '
        Me.lblPixelPerBP.AutoSize = True
        Me.lblPixelPerBP.BackColor = System.Drawing.Color.Transparent
        Me.lblPixelPerBP.Location = New System.Drawing.Point(1093, 27)
        Me.lblPixelPerBP.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPixelPerBP.Name = "lblPixelPerBP"
        Me.lblPixelPerBP.Size = New System.Drawing.Size(60, 15)
        Me.lblPixelPerBP.TabIndex = 35
        Me.lblPixelPerBP.Text = "Pixel/Kbp"
        '
        'SequenceViewer
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.snbPixelPerKBP)
        Me.Controls.Add(Me.lblPixelPerBP)
        Me.Controls.Add(Me.llEnzymeSwitch)
        Me.Controls.Add(Me.pnlVec)
        Me.Controls.Add(Me.llClose)
        Me.Controls.Add(Me.llSelectWhole)
        Me.Controls.Add(Me.llSelectPart)
        Me.Controls.Add(Me.lbSelection)
        Me.Controls.Add(Me.pafPrimer)
        Me.Controls.Add(Me.pnlPrimer)
        Me.Controls.Add(Me.pnlEnzymes)
        Me.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "SequenceViewer"
        Me.Size = New System.Drawing.Size(1324, 542)
        Me.pnlMap.ResumeLayout(False)
        CType(Me.pbMap, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSeq.ResumeLayout(False)
        CType(Me.pbSeq, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudTMFrom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudNa, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudC, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPrimer.ResumeLayout(False)
        Me.pnlPrimer.PerformLayout()
        CType(Me.nudWithin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlVec.ResumeLayout(False)
        Me.pnlEnzymes.ResumeLayout(False)
        Me.pnlEnzymes.PerformLayout()
        CType(Me.pafPrimer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbF As System.Windows.Forms.TextBox
    Friend WithEvents tbR As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents pbSeq As System.Windows.Forms.PictureBox
    Friend WithEvents nudTMFrom As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tbFKey As System.Windows.Forms.TextBox
    Friend WithEvents tbRKey As System.Windows.Forms.TextBox
    Friend WithEvents pnlMap As System.Windows.Forms.Panel
    Friend WithEvents pnlSeq As System.Windows.Forms.Panel
    Friend WithEvents pbMap As VectorMap
    Friend WithEvents infoF As System.Windows.Forms.Label
    Friend WithEvents infoR As System.Windows.Forms.Label
    Friend WithEvents nudNa As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudC As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents pafPrimer As MCDS.PrimerAnalysisFrame
    Friend WithEvents llFF As System.Windows.Forms.LinkLabel
    Friend WithEvents lbSelection As System.Windows.Forms.Label
    Friend WithEvents llFR As System.Windows.Forms.LinkLabel
    Friend WithEvents llPair As System.Windows.Forms.LinkLabel
    Friend WithEvents llSelectPart As System.Windows.Forms.LinkLabel
    Friend WithEvents llRF As System.Windows.Forms.LinkLabel
    Friend WithEvents llRR As System.Windows.Forms.LinkLabel
    Friend WithEvents llSelectWhole As System.Windows.Forms.LinkLabel
    Friend WithEvents llClose As System.Windows.Forms.LinkLabel
    Friend WithEvents pnlPrimer As System.Windows.Forms.Panel
    Friend WithEvents vsbSeq As System.Windows.Forms.VScrollBar
    Friend WithEvents pnlVec As System.Windows.Forms.Panel
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents tbEnzymes As System.Windows.Forms.TextBox
    Friend WithEvents llEnzymeSwitch As System.Windows.Forms.LinkLabel
    Friend WithEvents lbEnzymes As System.Windows.Forms.Label
    Friend WithEvents pnlEnzymes As System.Windows.Forms.Panel
    Friend WithEvents tbDNAName As System.Windows.Forms.TextBox
    Friend WithEvents lbDNAName As System.Windows.Forms.Label
    Friend WithEvents lbFileAddress As System.Windows.Forms.Label
    Friend WithEvents llApplyDNAName As System.Windows.Forms.LinkLabel
    Friend WithEvents nudWithin As System.Windows.Forms.NumericUpDown
    Friend WithEvents ttHelp As System.Windows.Forms.ToolTip
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents llSendF As System.Windows.Forms.LinkLabel
    Friend WithEvents llSendR As System.Windows.Forms.LinkLabel
    Friend WithEvents cmsSendTo As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents snbPixelPerKBP As MCDS.ScrollingNumberBox
    Friend WithEvents lblPixelPerBP As System.Windows.Forms.Label
    Friend WithEvents iHostFind As InteropHost
End Class
