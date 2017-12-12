<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSaveToServer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSaveToServer))
        Me.mtbPassword = New System.Windows.Forms.TextBox()
        Me.tbName = New System.Windows.Forms.TextBox()
        Me.cbSaveLoginInfo = New System.Windows.Forms.CheckBox()
        Me.lbUserName = New System.Windows.Forms.Label()
        Me.lbIP = New System.Windows.Forms.Label()
        Me.cbIP = New System.Windows.Forms.ComboBox()
        Me.lbPassword = New System.Windows.Forms.Label()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.tbFileName = New System.Windows.Forms.TextBox()
        Me.lbFileName = New System.Windows.Forms.Label()
        Me.lbExtension = New System.Windows.Forms.Label()
        Me.lbInfo = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'mtbPassword
        '
        Me.mtbPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mtbPassword.Location = New System.Drawing.Point(614, 5)
        Me.mtbPassword.Name = "mtbPassword"
        Me.mtbPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.mtbPassword.Size = New System.Drawing.Size(100, 21)
        Me.mtbPassword.TabIndex = 11
        '
        'tbName
        '
        Me.tbName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbName.Location = New System.Drawing.Point(449, 5)
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(100, 21)
        Me.tbName.TabIndex = 10
        '
        'cbSaveLoginInfo
        '
        Me.cbSaveLoginInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbSaveLoginInfo.AutoSize = True
        Me.cbSaveLoginInfo.Checked = True
        Me.cbSaveLoginInfo.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbSaveLoginInfo.Location = New System.Drawing.Point(252, 7)
        Me.cbSaveLoginInfo.Name = "cbSaveLoginInfo"
        Me.cbSaveLoginInfo.Size = New System.Drawing.Size(126, 16)
        Me.cbSaveLoginInfo.TabIndex = 13
        Me.cbSaveLoginInfo.Text = "Save Account Info"
        Me.cbSaveLoginInfo.UseVisualStyleBackColor = True
        '
        'lbUserName
        '
        Me.lbUserName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbUserName.AutoSize = True
        Me.lbUserName.Location = New System.Drawing.Point(384, 9)
        Me.lbUserName.Name = "lbUserName"
        Me.lbUserName.Size = New System.Drawing.Size(59, 12)
        Me.lbUserName.TabIndex = 0
        Me.lbUserName.Text = "User Name"
        '
        'lbIP
        '
        Me.lbIP.AutoSize = True
        Me.lbIP.Location = New System.Drawing.Point(4, 8)
        Me.lbIP.Name = "lbIP"
        Me.lbIP.Size = New System.Drawing.Size(59, 12)
        Me.lbIP.TabIndex = 0
        Me.lbIP.Text = "Server IP"
        '
        'cbIP
        '
        Me.cbIP.FormattingEnabled = True
        Me.cbIP.Location = New System.Drawing.Point(69, 5)
        Me.cbIP.Name = "cbIP"
        Me.cbIP.Size = New System.Drawing.Size(159, 20)
        Me.cbIP.TabIndex = 9
        '
        'lbPassword
        '
        Me.lbPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbPassword.AutoSize = True
        Me.lbPassword.Location = New System.Drawing.Point(555, 9)
        Me.lbPassword.Name = "lbPassword"
        Me.lbPassword.Size = New System.Drawing.Size(53, 12)
        Me.lbPassword.TabIndex = 0
        Me.lbPassword.Text = "Password"
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(533, 95)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 14
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(628, 95)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 15
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'tbFileName
        '
        Me.tbFileName.Location = New System.Drawing.Point(69, 49)
        Me.tbFileName.Name = "tbFileName"
        Me.tbFileName.Size = New System.Drawing.Size(259, 21)
        Me.tbFileName.TabIndex = 12
        '
        'lbFileName
        '
        Me.lbFileName.AutoSize = True
        Me.lbFileName.Location = New System.Drawing.Point(4, 52)
        Me.lbFileName.Name = "lbFileName"
        Me.lbFileName.Size = New System.Drawing.Size(59, 12)
        Me.lbFileName.TabIndex = 0
        Me.lbFileName.Text = "File Name"
        '
        'lbExtension
        '
        Me.lbExtension.AutoSize = True
        Me.lbExtension.Location = New System.Drawing.Point(334, 52)
        Me.lbExtension.Name = "lbExtension"
        Me.lbExtension.Size = New System.Drawing.Size(17, 12)
        Me.lbExtension.TabIndex = 0
        Me.lbExtension.Text = ".*"
        '
        'lbInfo
        '
        Me.lbInfo.AutoSize = True
        Me.lbInfo.ForeColor = System.Drawing.Color.Red
        Me.lbInfo.Location = New System.Drawing.Point(4, 95)
        Me.lbInfo.Name = "lbInfo"
        Me.lbInfo.Size = New System.Drawing.Size(0, 12)
        Me.lbInfo.TabIndex = 16
        '
        'frmSaveToServer
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(715, 130)
        Me.Controls.Add(Me.lbInfo)
        Me.Controls.Add(Me.lbExtension)
        Me.Controls.Add(Me.tbFileName)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lbPassword)
        Me.Controls.Add(Me.mtbPassword)
        Me.Controls.Add(Me.tbName)
        Me.Controls.Add(Me.cbSaveLoginInfo)
        Me.Controls.Add(Me.lbUserName)
        Me.Controls.Add(Me.lbFileName)
        Me.Controls.Add(Me.lbIP)
        Me.Controls.Add(Me.cbIP)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSaveToServer"
        Me.Text = "Save to Server"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents mtbPassword As System.Windows.Forms.TextBox
    Friend WithEvents tbName As System.Windows.Forms.TextBox
    Friend WithEvents cbSaveLoginInfo As System.Windows.Forms.CheckBox
    Friend WithEvents lbUserName As System.Windows.Forms.Label
    Friend WithEvents lbIP As System.Windows.Forms.Label
    Friend WithEvents cbIP As System.Windows.Forms.ComboBox
    Friend WithEvents lbPassword As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents tbFileName As System.Windows.Forms.TextBox
    Friend WithEvents lbFileName As System.Windows.Forms.Label
    Friend WithEvents lbExtension As System.Windows.Forms.Label
    Friend WithEvents lbInfo As System.Windows.Forms.Label
End Class
