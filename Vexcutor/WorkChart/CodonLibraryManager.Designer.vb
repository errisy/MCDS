<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CodonLibraryManager
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
        Me.bpbList = New MCDS.BufferedListView()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.llRemove = New System.Windows.Forms.LinkLabel()
        Me.llAdd = New System.Windows.Forms.LinkLabel()
        Me.llChange = New System.Windows.Forms.LinkLabel()
        Me.llSource = New System.Windows.Forms.LinkLabel()
        Me.rtbCodonDefine = New System.Windows.Forms.RichTextBox()
        Me.llCodon = New System.Windows.Forms.Label()
        Me.lbName = New System.Windows.Forms.Label()
        Me.tbOrganism = New System.Windows.Forms.TextBox()
        CType(Me.bpbList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'bpbList
        '
        Me.bpbList.CurrentViewStart = 0
        Me.bpbList.DataSource = Nothing
        Me.bpbList.Dock = System.Windows.Forms.DockStyle.Left
        Me.bpbList.DoubleSelectIndex = -1
        Me.bpbList.Location = New System.Drawing.Point(0, 0)
        Me.bpbList.Name = "bpbList"
        Me.bpbList.Size = New System.Drawing.Size(265, 535)
        Me.bpbList.TabIndex = 0
        Me.bpbList.TabStop = False
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(265, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 535)
        Me.Splitter1.TabIndex = 1
        Me.Splitter1.TabStop = False
        '
        'pnlMain
        '
        Me.pnlMain.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlMain.Controls.Add(Me.llRemove)
        Me.pnlMain.Controls.Add(Me.llAdd)
        Me.pnlMain.Controls.Add(Me.llChange)
        Me.pnlMain.Controls.Add(Me.llSource)
        Me.pnlMain.Controls.Add(Me.rtbCodonDefine)
        Me.pnlMain.Controls.Add(Me.llCodon)
        Me.pnlMain.Controls.Add(Me.lbName)
        Me.pnlMain.Controls.Add(Me.tbOrganism)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(268, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(669, 535)
        Me.pnlMain.TabIndex = 2
        '
        'llRemove
        '
        Me.llRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llRemove.AutoSize = True
        Me.llRemove.Location = New System.Drawing.Point(-2, 522)
        Me.llRemove.Name = "llRemove"
        Me.llRemove.Size = New System.Drawing.Size(41, 12)
        Me.llRemove.TabIndex = 3
        Me.llRemove.TabStop = True
        Me.llRemove.Text = "Remove"
        '
        'llAdd
        '
        Me.llAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llAdd.AutoSize = True
        Me.llAdd.Location = New System.Drawing.Point(599, 522)
        Me.llAdd.Name = "llAdd"
        Me.llAdd.Size = New System.Drawing.Size(23, 12)
        Me.llAdd.TabIndex = 3
        Me.llAdd.TabStop = True
        Me.llAdd.Text = "Add"
        '
        'llChange
        '
        Me.llChange.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llChange.AutoSize = True
        Me.llChange.Location = New System.Drawing.Point(628, 522)
        Me.llChange.Name = "llChange"
        Me.llChange.Size = New System.Drawing.Size(41, 12)
        Me.llChange.TabIndex = 3
        Me.llChange.TabStop = True
        Me.llChange.Text = "Change"
        '
        'llSource
        '
        Me.llSource.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llSource.AutoSize = True
        Me.llSource.Location = New System.Drawing.Point(637, 24)
        Me.llSource.Name = "llSource"
        Me.llSource.Size = New System.Drawing.Size(29, 12)
        Me.llSource.TabIndex = 3
        Me.llSource.TabStop = True
        Me.llSource.Text = "Help"
        '
        'rtbCodonDefine
        '
        Me.rtbCodonDefine.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbCodonDefine.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbCodonDefine.Location = New System.Drawing.Point(0, 42)
        Me.rtbCodonDefine.Name = "rtbCodonDefine"
        Me.rtbCodonDefine.Size = New System.Drawing.Size(666, 478)
        Me.rtbCodonDefine.TabIndex = 2
        Me.rtbCodonDefine.Text = ""
        '
        'llCodon
        '
        Me.llCodon.AutoSize = True
        Me.llCodon.Location = New System.Drawing.Point(3, 27)
        Me.llCodon.Name = "llCodon"
        Me.llCodon.Size = New System.Drawing.Size(161, 12)
        Me.llCodon.TabIndex = 1
        Me.llCodon.Text = "Codon Defination and Usage"
        '
        'lbName
        '
        Me.lbName.AutoSize = True
        Me.lbName.Location = New System.Drawing.Point(3, 3)
        Me.lbName.Name = "lbName"
        Me.lbName.Size = New System.Drawing.Size(53, 12)
        Me.lbName.TabIndex = 1
        Me.lbName.Text = "Organism"
        '
        'tbOrganism
        '
        Me.tbOrganism.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbOrganism.Location = New System.Drawing.Point(62, 0)
        Me.tbOrganism.Name = "tbOrganism"
        Me.tbOrganism.Size = New System.Drawing.Size(604, 21)
        Me.tbOrganism.TabIndex = 0
        '
        'CodonLibraryManager
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Controls.Add(Me.pnlMain)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.bpbList)
        Me.Name = "CodonLibraryManager"
        Me.Size = New System.Drawing.Size(937, 535)
        CType(Me.bpbList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlMain.ResumeLayout(False)
        Me.pnlMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bpbList As MCDS.BufferedListView
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents llSource As System.Windows.Forms.LinkLabel
    Friend WithEvents rtbCodonDefine As System.Windows.Forms.RichTextBox
    Friend WithEvents llCodon As System.Windows.Forms.Label
    Friend WithEvents lbName As System.Windows.Forms.Label
    Friend WithEvents tbOrganism As System.Windows.Forms.TextBox
    Friend WithEvents llRemove As System.Windows.Forms.LinkLabel
    Friend WithEvents llAdd As System.Windows.Forms.LinkLabel
    Friend WithEvents llChange As System.Windows.Forms.LinkLabel

End Class
