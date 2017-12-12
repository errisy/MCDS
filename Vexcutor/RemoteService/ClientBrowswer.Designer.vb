<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ClientBrowswer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ClientBrowswer))
        Me.cbIP = New System.Windows.Forms.ComboBox()
        Me.lbIP = New System.Windows.Forms.Label()
        Me.llConnect = New System.Windows.Forms.LinkLabel()
        Me.llSave = New System.Windows.Forms.LinkLabel()
        Me.scBrowser = New System.Windows.Forms.SplitContainer()
        Me.cbProject = New System.Windows.Forms.CheckBox()
        Me.cbVector = New System.Windows.Forms.CheckBox()
        Me.lvResults = New System.Windows.Forms.ListView()
        Me.chName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chLocation = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ilFiles = New System.Windows.Forms.ImageList(Me.components)
        Me.rtbQuery = New System.Windows.Forms.RichTextBox()
        Me.llSearch = New System.Windows.Forms.LinkLabel()
        Me.rbFeature = New System.Windows.Forms.RadioButton()
        Me.rbSequence = New System.Windows.Forms.RadioButton()
        Me.rbName = New System.Windows.Forms.RadioButton()
        Me.tvFolder = New System.Windows.Forms.TreeView()
        Me.tbName = New System.Windows.Forms.TextBox()
        Me.lbUserName = New System.Windows.Forms.Label()
        Me.lbPassword = New System.Windows.Forms.Label()
        Me.cbSaveLoginInfo = New System.Windows.Forms.CheckBox()
        Me.mtbPassword = New System.Windows.Forms.TextBox()
        Me.lbStatus = New System.Windows.Forms.Label()
        Me.cmsVector = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmOpenVector = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ShareOptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsProject = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmOpenProject = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ShareOptionsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.llDelete = New System.Windows.Forms.LinkLabel()
        Me.tsShare = New System.Windows.Forms.ToolStrip()
        Me.tsbUpdatePermission = New System.Windows.Forms.ToolStripButton()
        Me.tsbEveryone = New System.Windows.Forms.ToolStripButton()
        Me.tsbUsers = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tslFilename = New System.Windows.Forms.ToolStripLabel()
        CType(Me.scBrowser, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scBrowser.Panel1.SuspendLayout()
        Me.scBrowser.Panel2.SuspendLayout()
        Me.scBrowser.SuspendLayout()
        Me.cmsVector.SuspendLayout()
        Me.cmsProject.SuspendLayout()
        Me.tsShare.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbIP
        '
        Me.cbIP.FormattingEnabled = True
        Me.cbIP.Location = New System.Drawing.Point(68, 3)
        Me.cbIP.Name = "cbIP"
        Me.cbIP.Size = New System.Drawing.Size(159, 20)
        Me.cbIP.TabIndex = 1
        '
        'lbIP
        '
        Me.lbIP.AutoSize = True
        Me.lbIP.Location = New System.Drawing.Point(3, 6)
        Me.lbIP.Name = "lbIP"
        Me.lbIP.Size = New System.Drawing.Size(59, 12)
        Me.lbIP.TabIndex = 1
        Me.lbIP.Text = "Server IP"
        '
        'llConnect
        '
        Me.llConnect.AutoSize = True
        Me.llConnect.Location = New System.Drawing.Point(233, 6)
        Me.llConnect.Name = "llConnect"
        Me.llConnect.Size = New System.Drawing.Size(47, 12)
        Me.llConnect.TabIndex = 4
        Me.llConnect.TabStop = True
        Me.llConnect.Text = "Connect"
        '
        'llSave
        '
        Me.llSave.AutoSize = True
        Me.llSave.Location = New System.Drawing.Point(286, 6)
        Me.llSave.Name = "llSave"
        Me.llSave.Size = New System.Drawing.Size(29, 12)
        Me.llSave.TabIndex = 5
        Me.llSave.TabStop = True
        Me.llSave.Text = "Save"
        '
        'scBrowser
        '
        Me.scBrowser.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scBrowser.Location = New System.Drawing.Point(0, 26)
        Me.scBrowser.Margin = New System.Windows.Forms.Padding(0)
        Me.scBrowser.Name = "scBrowser"
        '
        'scBrowser.Panel1
        '
        Me.scBrowser.Panel1.Controls.Add(Me.cbProject)
        Me.scBrowser.Panel1.Controls.Add(Me.cbVector)
        Me.scBrowser.Panel1.Controls.Add(Me.lvResults)
        Me.scBrowser.Panel1.Controls.Add(Me.rtbQuery)
        Me.scBrowser.Panel1.Controls.Add(Me.llSearch)
        Me.scBrowser.Panel1.Controls.Add(Me.rbFeature)
        Me.scBrowser.Panel1.Controls.Add(Me.rbSequence)
        Me.scBrowser.Panel1.Controls.Add(Me.rbName)
        '
        'scBrowser.Panel2
        '
        Me.scBrowser.Panel2.Controls.Add(Me.tvFolder)
        Me.scBrowser.Size = New System.Drawing.Size(978, 499)
        Me.scBrowser.SplitterDistance = 466
        Me.scBrowser.TabIndex = 3
        '
        'cbProject
        '
        Me.cbProject.AutoSize = True
        Me.cbProject.Checked = True
        Me.cbProject.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbProject.Location = New System.Drawing.Point(286, 6)
        Me.cbProject.Name = "cbProject"
        Me.cbProject.Size = New System.Drawing.Size(66, 16)
        Me.cbProject.TabIndex = 0
        Me.cbProject.Text = "Project"
        Me.cbProject.UseVisualStyleBackColor = True
        '
        'cbVector
        '
        Me.cbVector.AutoSize = True
        Me.cbVector.Checked = True
        Me.cbVector.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbVector.Location = New System.Drawing.Point(220, 6)
        Me.cbVector.Name = "cbVector"
        Me.cbVector.Size = New System.Drawing.Size(60, 16)
        Me.cbVector.TabIndex = 0
        Me.cbVector.Text = "Vector"
        Me.cbVector.UseVisualStyleBackColor = True
        '
        'lvResults
        '
        Me.lvResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvResults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvResults.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chName, Me.chID, Me.chLocation, Me.ColumnHeader1})
        Me.lvResults.LargeImageList = Me.ilFiles
        Me.lvResults.Location = New System.Drawing.Point(0, 122)
        Me.lvResults.MultiSelect = False
        Me.lvResults.Name = "lvResults"
        Me.lvResults.Size = New System.Drawing.Size(464, 374)
        Me.lvResults.SmallImageList = Me.ilFiles
        Me.lvResults.TabIndex = 0
        Me.lvResults.UseCompatibleStateImageBehavior = False
        Me.lvResults.View = System.Windows.Forms.View.Details
        '
        'chName
        '
        Me.chName.Text = "Name"
        Me.chName.Width = 100
        '
        'chID
        '
        Me.chID.Text = "Score"
        Me.chID.Width = 50
        '
        'chLocation
        '
        Me.chLocation.Text = "Location"
        Me.chLocation.Width = 260
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "ID"
        '
        'ilFiles
        '
        Me.ilFiles.ImageStream = CType(resources.GetObject("ilFiles.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilFiles.TransparentColor = System.Drawing.Color.Transparent
        Me.ilFiles.Images.SetKeyName(0, "Root")
        Me.ilFiles.Images.SetKeyName(1, "Vector.png")
        Me.ilFiles.Images.SetKeyName(2, "Vector Shared.png")
        Me.ilFiles.Images.SetKeyName(3, "Vector Everyone.png")
        Me.ilFiles.Images.SetKeyName(4, "Project.png")
        Me.ilFiles.Images.SetKeyName(5, "Project Shared2.png")
        Me.ilFiles.Images.SetKeyName(6, "Project Everyone.png")
        '
        'rtbQuery
        '
        Me.rtbQuery.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbQuery.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbQuery.Location = New System.Drawing.Point(3, 22)
        Me.rtbQuery.Name = "rtbQuery"
        Me.rtbQuery.Size = New System.Drawing.Size(460, 97)
        Me.rtbQuery.TabIndex = 0
        Me.rtbQuery.Text = ""
        '
        'llSearch
        '
        Me.llSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llSearch.AutoSize = True
        Me.llSearch.Location = New System.Drawing.Point(416, 7)
        Me.llSearch.Name = "llSearch"
        Me.llSearch.Size = New System.Drawing.Size(41, 12)
        Me.llSearch.TabIndex = 0
        Me.llSearch.TabStop = True
        Me.llSearch.Text = "Search"
        '
        'rbFeature
        '
        Me.rbFeature.AutoSize = True
        Me.rbFeature.Location = New System.Drawing.Point(135, 5)
        Me.rbFeature.Name = "rbFeature"
        Me.rbFeature.Size = New System.Drawing.Size(65, 16)
        Me.rbFeature.TabIndex = 0
        Me.rbFeature.Text = "Feature"
        Me.rbFeature.UseVisualStyleBackColor = True
        '
        'rbSequence
        '
        Me.rbSequence.AutoSize = True
        Me.rbSequence.Location = New System.Drawing.Point(58, 5)
        Me.rbSequence.Name = "rbSequence"
        Me.rbSequence.Size = New System.Drawing.Size(71, 16)
        Me.rbSequence.TabIndex = 0
        Me.rbSequence.Text = "Sequence"
        Me.rbSequence.UseVisualStyleBackColor = True
        '
        'rbName
        '
        Me.rbName.AutoSize = True
        Me.rbName.Checked = True
        Me.rbName.Location = New System.Drawing.Point(5, 5)
        Me.rbName.Name = "rbName"
        Me.rbName.Size = New System.Drawing.Size(47, 16)
        Me.rbName.TabIndex = 0
        Me.rbName.TabStop = True
        Me.rbName.Text = "Name"
        Me.rbName.UseVisualStyleBackColor = True
        '
        'tvFolder
        '
        Me.tvFolder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvFolder.ImageIndex = 0
        Me.tvFolder.ImageList = Me.ilFiles
        Me.tvFolder.Location = New System.Drawing.Point(0, 0)
        Me.tvFolder.Name = "tvFolder"
        Me.tvFolder.SelectedImageIndex = 0
        Me.tvFolder.Size = New System.Drawing.Size(508, 499)
        Me.tvFolder.StateImageList = Me.ilFiles
        Me.tvFolder.TabIndex = 0
        '
        'tbName
        '
        Me.tbName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbName.Location = New System.Drawing.Point(695, 2)
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(100, 21)
        Me.tbName.TabIndex = 2
        '
        'lbUserName
        '
        Me.lbUserName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbUserName.AutoSize = True
        Me.lbUserName.Location = New System.Drawing.Point(630, 6)
        Me.lbUserName.Name = "lbUserName"
        Me.lbUserName.Size = New System.Drawing.Size(59, 12)
        Me.lbUserName.TabIndex = 1
        Me.lbUserName.Text = "User Name"
        '
        'lbPassword
        '
        Me.lbPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbPassword.AutoSize = True
        Me.lbPassword.Location = New System.Drawing.Point(801, 6)
        Me.lbPassword.Name = "lbPassword"
        Me.lbPassword.Size = New System.Drawing.Size(53, 12)
        Me.lbPassword.TabIndex = 1
        Me.lbPassword.Text = "Password"
        '
        'cbSaveLoginInfo
        '
        Me.cbSaveLoginInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbSaveLoginInfo.AutoSize = True
        Me.cbSaveLoginInfo.Checked = True
        Me.cbSaveLoginInfo.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbSaveLoginInfo.Location = New System.Drawing.Point(498, 4)
        Me.cbSaveLoginInfo.Name = "cbSaveLoginInfo"
        Me.cbSaveLoginInfo.Size = New System.Drawing.Size(126, 16)
        Me.cbSaveLoginInfo.TabIndex = 7
        Me.cbSaveLoginInfo.Text = "Save Account Info"
        Me.cbSaveLoginInfo.UseVisualStyleBackColor = True
        '
        'mtbPassword
        '
        Me.mtbPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mtbPassword.Location = New System.Drawing.Point(860, 3)
        Me.mtbPassword.Name = "mtbPassword"
        Me.mtbPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.mtbPassword.Size = New System.Drawing.Size(100, 21)
        Me.mtbPassword.TabIndex = 3
        '
        'lbStatus
        '
        Me.lbStatus.AutoSize = True
        Me.lbStatus.Location = New System.Drawing.Point(368, 6)
        Me.lbStatus.Name = "lbStatus"
        Me.lbStatus.Size = New System.Drawing.Size(41, 12)
        Me.lbStatus.TabIndex = 9
        Me.lbStatus.Text = "Status"
        '
        'cmsVector
        '
        Me.cmsVector.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmOpenVector, Me.LoadToToolStripMenuItem, Me.ToolStripSeparator1, Me.ShareOptionsToolStripMenuItem})
        Me.cmsVector.Name = "cmsVector"
        Me.cmsVector.Size = New System.Drawing.Size(160, 76)
        '
        'tsmOpenVector
        '
        Me.tsmOpenVector.Name = "tsmOpenVector"
        Me.tsmOpenVector.Size = New System.Drawing.Size(159, 22)
        Me.tsmOpenVector.Text = "Open Vector"
        '
        'LoadToToolStripMenuItem
        '
        Me.LoadToToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem})
        Me.LoadToToolStripMenuItem.Name = "LoadToToolStripMenuItem"
        Me.LoadToToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
        Me.LoadToToolStripMenuItem.Text = "Load To"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(102, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(156, 6)
        '
        'ShareOptionsToolStripMenuItem
        '
        Me.ShareOptionsToolStripMenuItem.Name = "ShareOptionsToolStripMenuItem"
        Me.ShareOptionsToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
        Me.ShareOptionsToolStripMenuItem.Text = "Share Options"
        '
        'cmsProject
        '
        Me.cmsProject.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmOpenProject, Me.ToolStripSeparator2, Me.ShareOptionsToolStripMenuItem1})
        Me.cmsProject.Name = "cmsVector"
        Me.cmsProject.Size = New System.Drawing.Size(160, 54)
        '
        'tsmOpenProject
        '
        Me.tsmOpenProject.Name = "tsmOpenProject"
        Me.tsmOpenProject.Size = New System.Drawing.Size(159, 22)
        Me.tsmOpenProject.Text = "Open Project"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(156, 6)
        '
        'ShareOptionsToolStripMenuItem1
        '
        Me.ShareOptionsToolStripMenuItem1.Name = "ShareOptionsToolStripMenuItem1"
        Me.ShareOptionsToolStripMenuItem1.Size = New System.Drawing.Size(159, 22)
        Me.ShareOptionsToolStripMenuItem1.Text = "Share Options"
        '
        'llDelete
        '
        Me.llDelete.AutoSize = True
        Me.llDelete.Location = New System.Drawing.Point(321, 6)
        Me.llDelete.Name = "llDelete"
        Me.llDelete.Size = New System.Drawing.Size(41, 12)
        Me.llDelete.TabIndex = 6
        Me.llDelete.TabStop = True
        Me.llDelete.Text = "Delete"
        '
        'tsShare
        '
        Me.tsShare.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.tsShare.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsShare.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbUpdatePermission, Me.tsbEveryone, Me.tsbUsers, Me.tslFilename})
        Me.tsShare.Location = New System.Drawing.Point(0, 530)
        Me.tsShare.Name = "tsShare"
        Me.tsShare.Size = New System.Drawing.Size(978, 25)
        Me.tsShare.TabIndex = 10
        Me.tsShare.Text = "Share Options"
        '
        'tsbUpdatePermission
        '
        Me.tsbUpdatePermission.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbUpdatePermission.Image = Global.MCDS.My.Resources.Resources.Update
        Me.tsbUpdatePermission.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbUpdatePermission.Name = "tsbUpdatePermission"
        Me.tsbUpdatePermission.Size = New System.Drawing.Size(23, 22)
        Me.tsbUpdatePermission.Text = "Update"
        '
        'tsbEveryone
        '
        Me.tsbEveryone.CheckOnClick = True
        Me.tsbEveryone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbEveryone.Image = Global.MCDS.My.Resources.Resources.Everyone
        Me.tsbEveryone.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEveryone.Name = "tsbEveryone"
        Me.tsbEveryone.Size = New System.Drawing.Size(23, 22)
        Me.tsbEveryone.Text = "Share with Everyone"
        '
        'tsbUsers
        '
        Me.tsbUsers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbUsers.Image = Global.MCDS.My.Resources.Resources.Share
        Me.tsbUsers.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbUsers.Name = "tsbUsers"
        Me.tsbUsers.Size = New System.Drawing.Size(29, 22)
        Me.tsbUsers.Text = "Share with Users"
        '
        'tslFilename
        '
        Me.tslFilename.Name = "tslFilename"
        Me.tslFilename.Size = New System.Drawing.Size(161, 22)
        Me.tslFilename.Text = "tcp:\\192.168.0.1\1\test.vxt"
        '
        'ClientBrowswer
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Controls.Add(Me.tsShare)
        Me.Controls.Add(Me.mtbPassword)
        Me.Controls.Add(Me.tbName)
        Me.Controls.Add(Me.scBrowser)
        Me.Controls.Add(Me.llSave)
        Me.Controls.Add(Me.cbSaveLoginInfo)
        Me.Controls.Add(Me.llConnect)
        Me.Controls.Add(Me.llDelete)
        Me.Controls.Add(Me.lbStatus)
        Me.Controls.Add(Me.lbPassword)
        Me.Controls.Add(Me.lbUserName)
        Me.Controls.Add(Me.lbIP)
        Me.Controls.Add(Me.cbIP)
        Me.Name = "ClientBrowswer"
        Me.Size = New System.Drawing.Size(978, 555)
        Me.scBrowser.Panel1.ResumeLayout(False)
        Me.scBrowser.Panel1.PerformLayout()
        Me.scBrowser.Panel2.ResumeLayout(False)
        CType(Me.scBrowser, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scBrowser.ResumeLayout(False)
        Me.cmsVector.ResumeLayout(False)
        Me.cmsProject.ResumeLayout(False)
        Me.tsShare.ResumeLayout(False)
        Me.tsShare.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbIP As System.Windows.Forms.ComboBox
    Friend WithEvents lbIP As System.Windows.Forms.Label
    Friend WithEvents llConnect As System.Windows.Forms.LinkLabel
    Friend WithEvents llSave As System.Windows.Forms.LinkLabel
    Friend WithEvents scBrowser As System.Windows.Forms.SplitContainer
    Friend WithEvents rtbQuery As System.Windows.Forms.RichTextBox
    Friend WithEvents llSearch As System.Windows.Forms.LinkLabel
    Friend WithEvents lvResults As System.Windows.Forms.ListView
    Friend WithEvents rbSequence As System.Windows.Forms.RadioButton
    Friend WithEvents rbName As System.Windows.Forms.RadioButton
    Friend WithEvents chName As System.Windows.Forms.ColumnHeader
    Friend WithEvents chID As System.Windows.Forms.ColumnHeader
    Friend WithEvents chLocation As System.Windows.Forms.ColumnHeader
    Friend WithEvents ilFiles As System.Windows.Forms.ImageList
    Friend WithEvents tbName As System.Windows.Forms.TextBox
    Friend WithEvents lbUserName As System.Windows.Forms.Label
    Friend WithEvents lbPassword As System.Windows.Forms.Label
    Friend WithEvents cbSaveLoginInfo As System.Windows.Forms.CheckBox
    Friend WithEvents mtbPassword As System.Windows.Forms.TextBox
    Friend WithEvents lbStatus As System.Windows.Forms.Label
    Friend WithEvents tvFolder As System.Windows.Forms.TreeView
    Friend WithEvents cmsVector As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tsmOpenVector As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadToToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsProject As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tsmOpenProject As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents llDelete As System.Windows.Forms.LinkLabel
    Friend WithEvents rbFeature As System.Windows.Forms.RadioButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ShareOptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ShareOptionsToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cbProject As System.Windows.Forms.CheckBox
    Friend WithEvents cbVector As System.Windows.Forms.CheckBox
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tsShare As System.Windows.Forms.ToolStrip
    Friend WithEvents tslFilename As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsbEveryone As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbUsers As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsbUpdatePermission As System.Windows.Forms.ToolStripButton

End Class
