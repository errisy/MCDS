<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrintPageView
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.cbPaper = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbTitle = New System.Windows.Forms.TextBox()
        Me.snbPageWidth = New MCDS.ScrollingNumberBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.snbPageHeight = New MCDS.ScrollingNumberBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.rtbDescription = New System.Windows.Forms.RichTextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.snbLeft = New MCDS.ScrollingNumberBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.snbTop = New MCDS.ScrollingNumberBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.snbRight = New MCDS.ScrollingNumberBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.snbBottom = New MCDS.ScrollingNumberBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.tbPageID = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.llLocation = New System.Windows.Forms.LinkLabel()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.snbDPI = New MCDS.ScrollingNumberBox()
        Me.llPrintThisPage = New System.Windows.Forms.LinkLabel()
        Me.llDirectPrintThisPage = New System.Windows.Forms.LinkLabel()
        Me.llDeleteThisPage = New System.Windows.Forms.LinkLabel()
        cbPaper = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'cbPaper
        '
        cbPaper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cbPaper.FormattingEnabled = True
        cbPaper.Items.AddRange(New Object() {"A4 - 210mm x 297mm", "B5 - 182mm x 257mm", "Letter - 215.9mm x 279.4mm", "A3 - 297mm x 420mm", "A5 - 148mm x 210mm", "16M - 184mm x 260mm", "32M - 130mm x 184mm", "Big 32M - 140mm x 203mm"})
        cbPaper.Location = New System.Drawing.Point(483, 2)
        cbPaper.Name = "cbPaper"
        cbPaper.Size = New System.Drawing.Size(270, 23)
        cbPaper.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(418, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Page Type"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 1)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(30, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Title"
        '
        'tbTitle
        '
        Me.tbTitle.Location = New System.Drawing.Point(68, 1)
        Me.tbTitle.Name = "tbTitle"
        Me.tbTitle.Size = New System.Drawing.Size(347, 21)
        Me.tbTitle.TabIndex = 2
        '
        'snbPageWidth
        '
        Me.snbPageWidth.IncrementValue = 1
        Me.snbPageWidth.Location = New System.Drawing.Point(483, 28)
        Me.snbPageWidth.Maximum = 200000
        Me.snbPageWidth.Minimum = 5
        Me.snbPageWidth.Name = "snbPageWidth"
        Me.snbPageWidth.Size = New System.Drawing.Size(75, 21)
        Me.snbPageWidth.TabIndex = 3
        Me.snbPageWidth.Text = "0"
        Me.snbPageWidth.Value = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(564, 31)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 15)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "mm"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(607, 31)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(19, 15)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "by"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(736, 31)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(29, 15)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "mm"
        '
        'snbPageHeight
        '
        Me.snbPageHeight.IncrementValue = 1
        Me.snbPageHeight.Location = New System.Drawing.Point(655, 28)
        Me.snbPageHeight.Maximum = 200000
        Me.snbPageHeight.Minimum = 5
        Me.snbPageHeight.Name = "snbPageHeight"
        Me.snbPageHeight.Size = New System.Drawing.Size(75, 21)
        Me.snbPageHeight.TabIndex = 3
        Me.snbPageHeight.Text = "0"
        Me.snbPageHeight.Value = 0
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 49)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(70, 15)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "Description"
        '
        'rtbDescription
        '
        Me.rtbDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rtbDescription.BackColor = System.Drawing.Color.AliceBlue
        Me.rtbDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbDescription.Location = New System.Drawing.Point(5, 64)
        Me.rtbDescription.Name = "rtbDescription"
        Me.rtbDescription.Size = New System.Drawing.Size(410, 215)
        Me.rtbDescription.TabIndex = 4
        Me.rtbDescription.Text = ""
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(442, 60)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(27, 15)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Left"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(564, 60)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(29, 15)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "mm"
        '
        'snbLeft
        '
        Me.snbLeft.IncrementValue = 1
        Me.snbLeft.Location = New System.Drawing.Point(483, 57)
        Me.snbLeft.Maximum = 200000
        Me.snbLeft.Minimum = 5
        Me.snbLeft.Name = "snbLeft"
        Me.snbLeft.Size = New System.Drawing.Size(75, 21)
        Me.snbLeft.TabIndex = 3
        Me.snbLeft.Text = "0"
        Me.snbLeft.Value = 0
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(564, 87)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(29, 15)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "mm"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(442, 87)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(27, 15)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "Top"
        '
        'snbTop
        '
        Me.snbTop.IncrementValue = 1
        Me.snbTop.Location = New System.Drawing.Point(483, 84)
        Me.snbTop.Maximum = 200000
        Me.snbTop.Minimum = 5
        Me.snbTop.Name = "snbTop"
        Me.snbTop.Size = New System.Drawing.Size(75, 21)
        Me.snbTop.TabIndex = 3
        Me.snbTop.Text = "0"
        Me.snbTop.Value = 0
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(736, 61)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(29, 15)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = "mm"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(608, 61)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(36, 15)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Right"
        '
        'snbRight
        '
        Me.snbRight.IncrementValue = 1
        Me.snbRight.Location = New System.Drawing.Point(655, 58)
        Me.snbRight.Maximum = 200000
        Me.snbRight.Minimum = 5
        Me.snbRight.Name = "snbRight"
        Me.snbRight.Size = New System.Drawing.Size(75, 21)
        Me.snbRight.TabIndex = 3
        Me.snbRight.Text = "0"
        Me.snbRight.Value = 0
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(736, 88)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(29, 15)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = "mm"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(608, 88)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(46, 15)
        Me.Label14.TabIndex = 1
        Me.Label14.Text = "Bottom"
        '
        'snbBottom
        '
        Me.snbBottom.IncrementValue = 1
        Me.snbBottom.Location = New System.Drawing.Point(655, 85)
        Me.snbBottom.Maximum = 200000
        Me.snbBottom.Minimum = 5
        Me.snbBottom.Name = "snbBottom"
        Me.snbBottom.Size = New System.Drawing.Size(75, 21)
        Me.snbBottom.TabIndex = 3
        Me.snbBottom.Text = "0"
        Me.snbBottom.Value = 0
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(3, 28)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(51, 15)
        Me.Label15.TabIndex = 1
        Me.Label15.Text = "Page ID"
        '
        'tbPageID
        '
        Me.tbPageID.Location = New System.Drawing.Point(68, 25)
        Me.tbPageID.Name = "tbPageID"
        Me.tbPageID.Size = New System.Drawing.Size(347, 21)
        Me.tbPageID.TabIndex = 2
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(418, 31)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(62, 15)
        Me.Label16.TabIndex = 1
        Me.Label16.Text = "Page Size"
        '
        'llLocation
        '
        Me.llLocation.AutoSize = True
        Me.llLocation.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llLocation.Location = New System.Drawing.Point(421, 150)
        Me.llLocation.Name = "llLocation"
        Me.llLocation.Size = New System.Drawing.Size(143, 15)
        Me.llLocation.TabIndex = 5
        Me.llLocation.TabStop = True
        Me.llLocation.Text = "Page Location (0, 0, 0, 0)"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(448, 119)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(27, 15)
        Me.Label17.TabIndex = 1
        Me.Label17.Text = "DPI"
        '
        'snbDPI
        '
        Me.snbDPI.IncrementValue = 6
        Me.snbDPI.Location = New System.Drawing.Point(483, 116)
        Me.snbDPI.Maximum = 3600
        Me.snbDPI.Minimum = 6
        Me.snbDPI.Name = "snbDPI"
        Me.snbDPI.Size = New System.Drawing.Size(75, 21)
        Me.snbDPI.TabIndex = 3
        Me.snbDPI.Text = "0"
        Me.snbDPI.Value = 0
        '
        'llPrintThisPage
        '
        Me.llPrintThisPage.AutoSize = True
        Me.llPrintThisPage.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.llPrintThisPage.Location = New System.Drawing.Point(421, 183)
        Me.llPrintThisPage.Name = "llPrintThisPage"
        Me.llPrintThisPage.Size = New System.Drawing.Size(91, 15)
        Me.llPrintThisPage.TabIndex = 6
        Me.llPrintThisPage.TabStop = True
        Me.llPrintThisPage.Text = "Print This Page"
        '
        'llDirectPrintThisPage
        '
        Me.llDirectPrintThisPage.AutoSize = True
        Me.llDirectPrintThisPage.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.llDirectPrintThisPage.Location = New System.Drawing.Point(535, 183)
        Me.llDirectPrintThisPage.Name = "llDirectPrintThisPage"
        Me.llDirectPrintThisPage.Size = New System.Drawing.Size(126, 15)
        Me.llDirectPrintThisPage.TabIndex = 6
        Me.llDirectPrintThisPage.TabStop = True
        Me.llDirectPrintThisPage.Text = "Direct Print This Page"
        '
        'llDeleteThisPage
        '
        Me.llDeleteThisPage.AutoSize = True
        Me.llDeleteThisPage.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.llDeleteThisPage.Location = New System.Drawing.Point(421, 214)
        Me.llDeleteThisPage.Name = "llDeleteThisPage"
        Me.llDeleteThisPage.Size = New System.Drawing.Size(102, 15)
        Me.llDeleteThisPage.TabIndex = 6
        Me.llDeleteThisPage.TabStop = True
        Me.llDeleteThisPage.Text = "Delete This Page"
        '
        'PrintPageView
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.llDirectPrintThisPage)
        Me.Controls.Add(Me.llDeleteThisPage)
        Me.Controls.Add(Me.llPrintThisPage)
        Me.Controls.Add(Me.llLocation)
        Me.Controls.Add(Me.rtbDescription)
        Me.Controls.Add(Me.snbBottom)
        Me.Controls.Add(Me.snbRight)
        Me.Controls.Add(Me.snbDPI)
        Me.Controls.Add(Me.snbTop)
        Me.Controls.Add(Me.snbLeft)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.snbPageHeight)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.snbPageWidth)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.tbPageID)
        Me.Controls.Add(Me.tbTitle)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(cbPaper)
        Me.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.Name = "PrintPageView"
        Me.Size = New System.Drawing.Size(1078, 279)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbPaper As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbTitle As System.Windows.Forms.TextBox
    Friend WithEvents snbPageWidth As MCDS.ScrollingNumberBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents snbPageHeight As MCDS.ScrollingNumberBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents rtbDescription As System.Windows.Forms.RichTextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents snbLeft As MCDS.ScrollingNumberBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents snbTop As MCDS.ScrollingNumberBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents snbRight As MCDS.ScrollingNumberBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents snbBottom As MCDS.ScrollingNumberBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents tbPageID As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents llLocation As System.Windows.Forms.LinkLabel
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents snbDPI As MCDS.ScrollingNumberBox
    Friend WithEvents llPrintThisPage As System.Windows.Forms.LinkLabel
    Friend WithEvents llDirectPrintThisPage As System.Windows.Forms.LinkLabel
    Friend WithEvents llDeleteThisPage As LinkLabel
End Class
