<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SequenceWindow
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
        Me.svGene = New MCDS.SequenceViewer
        Me.SuspendLayout()
        '
        'svGene
        '
        Me.svGene.BackColor = System.Drawing.Color.White
        Me.svGene.Dock = System.Windows.Forms.DockStyle.Fill
        Me.svGene.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.svGene.GeneFile = Nothing
        Me.svGene.Location = New System.Drawing.Point(0, 0)
        Me.svGene.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.svGene.Name = "svGene"
        Me.svGene.Size = New System.Drawing.Size(992, 445)
        Me.svGene.TabIndex = 0
        '
        'SequenceWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(992, 445)
        Me.Controls.Add(Me.svGene)
        Me.Name = "SequenceWindow"
        Me.Text = "SequenceWindow"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents svGene As MCDS.SequenceViewer
End Class
