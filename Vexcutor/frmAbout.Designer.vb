<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbout
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
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
        Me.vlblHost = New System.Windows.Forms.Label()
        Me.lblLSTM = New System.Windows.Forms.Label()
        Me.lblLicense = New System.Windows.Forms.Label()
        Me.lblSNM = New System.Windows.Forms.Label()
        Me.lblSN = New System.Windows.Forms.Label()
        Me.llOK = New System.Windows.Forms.LinkLabel()
        Me.lblICM = New System.Windows.Forms.Label()
        Me.lblIC = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblEdition = New System.Windows.Forms.Label()
        Me.llSynthenome = New System.Windows.Forms.LinkLabel()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vlblHost
        '
        Me.vlblHost.AutoSize = True
        Me.vlblHost.Font = New System.Drawing.Font("Arial Narrow", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.vlblHost.ForeColor = System.Drawing.Color.Yellow
        Me.vlblHost.Location = New System.Drawing.Point(160, 3)
        Me.vlblHost.Name = "vlblHost"
        Me.vlblHost.Size = New System.Drawing.Size(465, 37)
        Me.vlblHost.TabIndex = 1
        Me.vlblHost.Text = "Molecular Cloning Designer Simulator"
        '
        'lblLSTM
        '
        Me.lblLSTM.AutoSize = True
        Me.lblLSTM.ForeColor = System.Drawing.Color.Olive
        Me.lblLSTM.Location = New System.Drawing.Point(163, 71)
        Me.lblLSTM.Name = "lblLSTM"
        Me.lblLSTM.Size = New System.Drawing.Size(79, 20)
        Me.lblLSTM.TabIndex = 2
        Me.lblLSTM.Text = "Licensed to"
        '
        'lblLicense
        '
        Me.lblLicense.AutoSize = True
        Me.lblLicense.ForeColor = System.Drawing.Color.GreenYellow
        Me.lblLicense.Location = New System.Drawing.Point(180, 91)
        Me.lblLicense.Name = "lblLicense"
        Me.lblLicense.Size = New System.Drawing.Size(127, 20)
        Me.lblLicense.TabIndex = 3
        Me.lblLicense.Text = "All Academic Users"
        '
        'lblSNM
        '
        Me.lblSNM.AutoSize = True
        Me.lblSNM.ForeColor = System.Drawing.Color.Olive
        Me.lblSNM.Location = New System.Drawing.Point(166, 159)
        Me.lblSNM.Name = "lblSNM"
        Me.lblSNM.Size = New System.Drawing.Size(94, 20)
        Me.lblSNM.TabIndex = 4
        Me.lblSNM.Text = "Serial Number"
        '
        'lblSN
        '
        Me.lblSN.AutoSize = True
        Me.lblSN.ForeColor = System.Drawing.Color.GreenYellow
        Me.lblSN.Location = New System.Drawing.Point(180, 179)
        Me.lblSN.Name = "lblSN"
        Me.lblSN.Size = New System.Drawing.Size(293, 20)
        Me.lblSN.TabIndex = 5
        Me.lblSN.Text = "XXXX-XXXX-XXXX-XXXX-XXXX-XXXX-XXXX-XXXX"
        '
        'llOK
        '
        Me.llOK.AutoSize = True
        Me.llOK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.llOK.Font = New System.Drawing.Font("Arial Narrow", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llOK.ForeColor = System.Drawing.Color.Aqua
        Me.llOK.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.llOK.LinkColor = System.Drawing.Color.Orange
        Me.llOK.Location = New System.Drawing.Point(758, 371)
        Me.llOK.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.llOK.Name = "llOK"
        Me.llOK.Size = New System.Drawing.Size(30, 22)
        Me.llOK.TabIndex = 0
        Me.llOK.TabStop = True
        Me.llOK.Text = "OK"
        '
        'lblICM
        '
        Me.lblICM.AutoSize = True
        Me.lblICM.ForeColor = System.Drawing.Color.Olive
        Me.lblICM.Location = New System.Drawing.Point(163, 115)
        Me.lblICM.Name = "lblICM"
        Me.lblICM.Size = New System.Drawing.Size(101, 20)
        Me.lblICM.TabIndex = 2
        Me.lblICM.Text = "Current Version"
        '
        'lblIC
        '
        Me.lblIC.AutoSize = True
        Me.lblIC.ForeColor = System.Drawing.Color.GreenYellow
        Me.lblIC.Location = New System.Drawing.Point(180, 135)
        Me.lblIC.Name = "lblIC"
        Me.lblIC.Size = New System.Drawing.Size(32, 20)
        Me.lblIC.TabIndex = 2
        Me.lblIC.Text = "Any"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.White
        Me.PictureBox1.Image = Global.MCDS.My.Resources.Resources.Synthenome
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(157, 407)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PictureBox1.TabIndex = 6
        Me.PictureBox1.TabStop = False
        '
        'lblEdition
        '
        Me.lblEdition.AutoSize = True
        Me.lblEdition.ForeColor = System.Drawing.Color.Yellow
        Me.lblEdition.Location = New System.Drawing.Point(669, 20)
        Me.lblEdition.Name = "lblEdition"
        Me.lblEdition.Size = New System.Drawing.Size(128, 20)
        Me.lblEdition.TabIndex = 5
        Me.lblEdition.Text = "Professional Edition"
        '
        'llSynthenome
        '
        Me.llSynthenome.AutoSize = True
        Me.llSynthenome.ForeColor = System.Drawing.Color.GreenYellow
        Me.llSynthenome.LinkColor = System.Drawing.Color.DeepSkyBlue
        Me.llSynthenome.Location = New System.Drawing.Point(163, 43)
        Me.llSynthenome.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.llSynthenome.Name = "llSynthenome"
        Me.llSynthenome.Size = New System.Drawing.Size(127, 20)
        Me.llSynthenome.TabIndex = 0
        Me.llSynthenome.TabStop = True
        Me.llSynthenome.Text = "errisy@hotmail.com"
        '
        'frmAbout
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(800, 400)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblEdition)
        Me.Controls.Add(Me.lblSN)
        Me.Controls.Add(Me.lblSNM)
        Me.Controls.Add(Me.lblLicense)
        Me.Controls.Add(Me.lblIC)
        Me.Controls.Add(Me.lblICM)
        Me.Controls.Add(Me.lblLSTM)
        Me.Controls.Add(Me.vlblHost)
        Me.Controls.Add(Me.llOK)
        Me.Controls.Add(Me.llSynthenome)
        Me.Controls.Add(Me.PictureBox1)
        Me.Font = New System.Drawing.Font("Arial Narrow", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.Name = "frmAbout"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmAbout"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vlblHost As System.Windows.Forms.Label
    Friend WithEvents lblLSTM As System.Windows.Forms.Label
    Friend WithEvents lblLicense As System.Windows.Forms.Label
    Friend WithEvents lblSNM As System.Windows.Forms.Label
    Friend WithEvents lblSN As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents llOK As System.Windows.Forms.LinkLabel
    Friend WithEvents lblICM As System.Windows.Forms.Label
    Friend WithEvents lblIC As System.Windows.Forms.Label
    Friend WithEvents lblEdition As Label
    Friend WithEvents llSynthenome As LinkLabel
End Class
