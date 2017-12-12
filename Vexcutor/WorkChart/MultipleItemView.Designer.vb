<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MultipleItemView
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
        Me.DNAView = New System.Windows.Forms.ListView()
        Me.ch_Name = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ch_Size = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cn_Cir = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ch_ENDS = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ch_Phos = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.llDNA = New System.Windows.Forms.LinkLabel()
        Me.llOperation = New System.Windows.Forms.LinkLabel()
        Me.rbFinished = New System.Windows.Forms.RadioButton()
        Me.rbInprogress = New System.Windows.Forms.RadioButton()
        Me.rbUnstarted = New System.Windows.Forms.RadioButton()
        Me.rbObsolete = New System.Windows.Forms.RadioButton()
        Me.cbDescribe = New System.Windows.Forms.CheckBox()
        Me.llCalculate = New System.Windows.Forms.LinkLabel()
        Me.SuspendLayout()
        '
        'DNAView
        '
        Me.DNAView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DNAView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ch_Name, Me.ch_Size, Me.cn_Cir, Me.ch_ENDS, Me.ch_Phos})
        Me.DNAView.Location = New System.Drawing.Point(0, 20)
        Me.DNAView.Name = "DNAView"
        Me.DNAView.Size = New System.Drawing.Size(871, 318)
        Me.DNAView.TabIndex = 23
        Me.DNAView.UseCompatibleStateImageBehavior = False
        Me.DNAView.View = System.Windows.Forms.View.Details
        '
        'ch_Name
        '
        Me.ch_Name.Text = "DNA"
        Me.ch_Name.Width = 100
        '
        'ch_Size
        '
        Me.ch_Size.Text = "Size"
        '
        'cn_Cir
        '
        Me.cn_Cir.Text = "Shape"
        Me.cn_Cir.Width = 70
        '
        'ch_ENDS
        '
        Me.ch_ENDS.Text = "F End"
        Me.ch_ENDS.Width = 120
        '
        'ch_Phos
        '
        Me.ch_Phos.Text = "R End"
        Me.ch_Phos.Width = 120
        '
        'llDNA
        '
        Me.llDNA.AutoSize = True
        Me.llDNA.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llDNA.Location = New System.Drawing.Point(122, 0)
        Me.llDNA.Name = "llDNA"
        Me.llDNA.Size = New System.Drawing.Size(34, 17)
        Me.llDNA.TabIndex = 24
        Me.llDNA.TabStop = True
        Me.llDNA.Text = "DNA"
        '
        'llOperation
        '
        Me.llOperation.AutoSize = True
        Me.llOperation.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llOperation.Location = New System.Drawing.Point(3, 0)
        Me.llOperation.Name = "llOperation"
        Me.llOperation.Size = New System.Drawing.Size(65, 17)
        Me.llOperation.TabIndex = 24
        Me.llOperation.TabStop = True
        Me.llOperation.Text = "Operation"
        '
        'rbFinished
        '
        Me.rbFinished.AutoSize = True
        Me.rbFinished.Location = New System.Drawing.Point(709, 1)
        Me.rbFinished.Name = "rbFinished"
        Me.rbFinished.Size = New System.Drawing.Size(71, 16)
        Me.rbFinished.TabIndex = 27
        Me.rbFinished.TabStop = True
        Me.rbFinished.Tag = "2"
        Me.rbFinished.Text = "Finished"
        Me.rbFinished.UseVisualStyleBackColor = True
        '
        'rbInprogress
        '
        Me.rbInprogress.AutoSize = True
        Me.rbInprogress.Location = New System.Drawing.Point(614, 1)
        Me.rbInprogress.Name = "rbInprogress"
        Me.rbInprogress.Size = New System.Drawing.Size(89, 16)
        Me.rbInprogress.TabIndex = 26
        Me.rbInprogress.TabStop = True
        Me.rbInprogress.Tag = "1"
        Me.rbInprogress.Text = "In Progress"
        Me.rbInprogress.UseVisualStyleBackColor = True
        '
        'rbUnstarted
        '
        Me.rbUnstarted.AutoSize = True
        Me.rbUnstarted.Location = New System.Drawing.Point(519, 1)
        Me.rbUnstarted.Name = "rbUnstarted"
        Me.rbUnstarted.Size = New System.Drawing.Size(89, 16)
        Me.rbUnstarted.TabIndex = 25
        Me.rbUnstarted.TabStop = True
        Me.rbUnstarted.Tag = "0"
        Me.rbUnstarted.Text = "Not Started"
        Me.rbUnstarted.UseVisualStyleBackColor = True
        '
        'rbObsolete
        '
        Me.rbObsolete.AutoSize = True
        Me.rbObsolete.Location = New System.Drawing.Point(791, 1)
        Me.rbObsolete.Name = "rbObsolete"
        Me.rbObsolete.Size = New System.Drawing.Size(71, 16)
        Me.rbObsolete.TabIndex = 27
        Me.rbObsolete.TabStop = True
        Me.rbObsolete.Tag = "3"
        Me.rbObsolete.Text = "Obsolete"
        Me.rbObsolete.UseVisualStyleBackColor = True
        '
        'cbDescribe
        '
        Me.cbDescribe.AutoSize = True
        Me.cbDescribe.Location = New System.Drawing.Point(417, 2)
        Me.cbDescribe.Name = "cbDescribe"
        Me.cbDescribe.Size = New System.Drawing.Size(84, 16)
        Me.cbDescribe.TabIndex = 38
        Me.cbDescribe.Text = "Chromosome"
        Me.cbDescribe.UseVisualStyleBackColor = True
        '
        'llCalculate
        '
        Me.llCalculate.AutoSize = True
        Me.llCalculate.Font = New System.Drawing.Font("Calibri", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llCalculate.Location = New System.Drawing.Point(298, 0)
        Me.llCalculate.Name = "llCalculate"
        Me.llCalculate.Size = New System.Drawing.Size(60, 17)
        Me.llCalculate.TabIndex = 39
        Me.llCalculate.TabStop = True
        Me.llCalculate.Text = "Calculate"
        '
        'MultipleItemView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Controls.Add(Me.llCalculate)
        Me.Controls.Add(Me.cbDescribe)
        Me.Controls.Add(Me.rbObsolete)
        Me.Controls.Add(Me.rbFinished)
        Me.Controls.Add(Me.rbInprogress)
        Me.Controls.Add(Me.rbUnstarted)
        Me.Controls.Add(Me.llOperation)
        Me.Controls.Add(Me.llDNA)
        Me.Controls.Add(Me.DNAView)
        Me.DoubleBuffered = True
        Me.Name = "MultipleItemView"
        Me.Size = New System.Drawing.Size(871, 338)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DNAView As System.Windows.Forms.ListView
    Friend WithEvents ch_Name As System.Windows.Forms.ColumnHeader
    Friend WithEvents ch_Size As System.Windows.Forms.ColumnHeader
    Friend WithEvents cn_Cir As System.Windows.Forms.ColumnHeader
    Friend WithEvents ch_ENDS As System.Windows.Forms.ColumnHeader
    Friend WithEvents ch_Phos As System.Windows.Forms.ColumnHeader
    Friend WithEvents llDNA As System.Windows.Forms.LinkLabel
    Friend WithEvents llOperation As System.Windows.Forms.LinkLabel
    Friend WithEvents rbFinished As System.Windows.Forms.RadioButton
    Friend WithEvents rbInprogress As System.Windows.Forms.RadioButton
    Friend WithEvents rbUnstarted As System.Windows.Forms.RadioButton
    Friend WithEvents rbObsolete As System.Windows.Forms.RadioButton
    Friend WithEvents cbDescribe As System.Windows.Forms.CheckBox
    Friend WithEvents llCalculate As System.Windows.Forms.LinkLabel

End Class
