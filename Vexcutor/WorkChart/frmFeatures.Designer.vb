<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFeatures
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFeatures))
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tsb_OK = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.tsb_All = New System.Windows.Forms.ToolStripButton
        Me.tsb_None = New System.Windows.Forms.ToolStripButton
        Me.SView = New System.Windows.Forms.ListView
        Me.ch_Feature = New System.Windows.Forms.ColumnHeader
        Me.ch_Type = New System.Windows.Forms.ColumnHeader
        Me.ch_Length = New System.Windows.Forms.ColumnHeader
        Me.cn_Note = New System.Windows.Forms.ColumnHeader
        Me.ToolStripContainer1.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.BottomToolStripPanel
        '
        Me.ToolStripContainer1.BottomToolStripPanel.Controls.Add(Me.ToolStrip1)
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.SView)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(731, 392)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.Size = New System.Drawing.Size(731, 442)
        Me.ToolStripContainer1.TabIndex = 3
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsb_OK, Me.ToolStripSeparator1, Me.tsb_All, Me.tsb_None})
        Me.ToolStrip1.Location = New System.Drawing.Point(160, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(101, 25)
        Me.ToolStrip1.TabIndex = 3
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsb_OK
        '
        Me.tsb_OK.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsb_OK.Image = CType(resources.GetObject("tsb_OK.Image"), System.Drawing.Image)
        Me.tsb_OK.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsb_OK.Name = "tsb_OK"
        Me.tsb_OK.Size = New System.Drawing.Size(23, 22)
        Me.tsb_OK.Text = "OK"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsb_All
        '
        Me.tsb_All.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsb_All.Image = CType(resources.GetObject("tsb_All.Image"), System.Drawing.Image)
        Me.tsb_All.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsb_All.Name = "tsb_All"
        Me.tsb_All.Size = New System.Drawing.Size(27, 22)
        Me.tsb_All.Text = "All"
        '
        'tsb_None
        '
        Me.tsb_None.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsb_None.Image = CType(resources.GetObject("tsb_None.Image"), System.Drawing.Image)
        Me.tsb_None.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsb_None.Name = "tsb_None"
        Me.tsb_None.Size = New System.Drawing.Size(33, 22)
        Me.tsb_None.Text = "None"
        '
        'SView
        '
        Me.SView.CheckBoxes = True
        Me.SView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ch_Feature, Me.ch_Type, Me.ch_Length, Me.cn_Note})
        Me.SView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SView.Location = New System.Drawing.Point(0, 0)
        Me.SView.Name = "SView"
        Me.SView.Size = New System.Drawing.Size(731, 392)
        Me.SView.TabIndex = 1
        Me.SView.UseCompatibleStateImageBehavior = False
        Me.SView.View = System.Windows.Forms.View.Details
        '
        'ch_Feature
        '
        Me.ch_Feature.Width = 200
        '
        'ch_Type
        '
        Me.ch_Type.Text = "Type"
        Me.ch_Type.Width = 90
        '
        'ch_Length
        '
        Me.ch_Length.Text = "Length"
        '
        'cn_Note
        '
        Me.cn_Note.Text = "Note"
        Me.cn_Note.Width = 300
        '
        'frmFeatures
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(731, 442)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmFeatures"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select Features"
        Me.ToolStripContainer1.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsb_OK As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsb_All As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsb_None As System.Windows.Forms.ToolStripButton
    Friend WithEvents SView As System.Windows.Forms.ListView
    Friend WithEvents ch_Feature As System.Windows.Forms.ColumnHeader
    Friend WithEvents ch_Type As System.Windows.Forms.ColumnHeader
    Friend WithEvents cn_Note As System.Windows.Forms.ColumnHeader
    Friend WithEvents ch_Length As System.Windows.Forms.ColumnHeader
End Class
