<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RestrictionEnzymeView
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
        Me.SView = New System.Windows.Forms.ListView
        Me.cn_Name = New System.Windows.Forms.ColumnHeader
        Me.cn_PATTERN = New System.Windows.Forms.ColumnHeader
        Me.cn_Cut = New System.Windows.Forms.ColumnHeader
        Me.chPalinDromic = New System.Windows.Forms.ColumnHeader
        Me.tbEnzymes = New System.Windows.Forms.TextBox
        Me.llApply = New System.Windows.Forms.LinkLabel
        Me.llCancel = New System.Windows.Forms.LinkLabel
        Me.SuspendLayout()
        '
        'SView
        '
        Me.SView.CheckBoxes = True
        Me.SView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.cn_Name, Me.cn_PATTERN, Me.cn_Cut, Me.chPalinDromic})
        Me.SView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SView.Location = New System.Drawing.Point(0, 0)
        Me.SView.Name = "SView"
        Me.SView.Size = New System.Drawing.Size(872, 379)
        Me.SView.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.SView.TabIndex = 15
        Me.SView.UseCompatibleStateImageBehavior = False
        Me.SView.View = System.Windows.Forms.View.Details
        '
        'cn_Name
        '
        Me.cn_Name.Text = "Name"
        Me.cn_Name.Width = 105
        '
        'cn_PATTERN
        '
        Me.cn_PATTERN.Text = "Pattern"
        Me.cn_PATTERN.Width = 220
        '
        'cn_Cut
        '
        Me.cn_Cut.Text = "Cut"
        Me.cn_Cut.Width = 100
        '
        'chPalinDromic
        '
        Me.chPalinDromic.Text = "IsPanlindromic"
        Me.chPalinDromic.Width = 132
        '
        'tbEnzymes
        '
        Me.tbEnzymes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbEnzymes.Location = New System.Drawing.Point(0, 0)
        Me.tbEnzymes.Name = "tbEnzymes"
        Me.tbEnzymes.Size = New System.Drawing.Size(759, 21)
        Me.tbEnzymes.TabIndex = 16
        '
        'llApply
        '
        Me.llApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llApply.AutoSize = True
        Me.llApply.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llApply.Location = New System.Drawing.Point(765, 1)
        Me.llApply.Name = "llApply"
        Me.llApply.Size = New System.Drawing.Size(45, 19)
        Me.llApply.TabIndex = 17
        Me.llApply.TabStop = True
        Me.llApply.Text = "Apply"
        '
        'llCancel
        '
        Me.llCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llCancel.AutoSize = True
        Me.llCancel.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llCancel.Location = New System.Drawing.Point(816, 1)
        Me.llCancel.Name = "llCancel"
        Me.llCancel.Size = New System.Drawing.Size(53, 19)
        Me.llCancel.TabIndex = 17
        Me.llCancel.TabStop = True
        Me.llCancel.Text = "Cancel"
        '
        'RestrictionEnzymeView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.llCancel)
        Me.Controls.Add(Me.llApply)
        Me.Controls.Add(Me.tbEnzymes)
        Me.Controls.Add(Me.SView)
        Me.Name = "RestrictionEnzymeView"
        Me.Size = New System.Drawing.Size(872, 379)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SView As System.Windows.Forms.ListView
    Friend WithEvents cn_Name As System.Windows.Forms.ColumnHeader
    Friend WithEvents cn_PATTERN As System.Windows.Forms.ColumnHeader
    Friend WithEvents cn_Cut As System.Windows.Forms.ColumnHeader
    Friend WithEvents tbEnzymes As System.Windows.Forms.TextBox
    Friend WithEvents llApply As System.Windows.Forms.LinkLabel
    Friend WithEvents llCancel As System.Windows.Forms.LinkLabel
    Friend WithEvents chPalinDromic As System.Windows.Forms.ColumnHeader

End Class
