<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmORF
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
        Me.ehORF = New System.Windows.Forms.Integration.ElementHost()
        Me.SuspendLayout()
        '
        'ehORF
        '
        Me.ehORF.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ehORF.Location = New System.Drawing.Point(0, 0)
        Me.ehORF.Name = "ehORF"
        Me.ehORF.Size = New System.Drawing.Size(604, 161)
        Me.ehORF.TabIndex = 0
        Me.ehORF.Text = "ORF Searcher"
        Me.ehORF.Child = Nothing
        '
        'frmORF
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(604, 161)
        Me.Controls.Add(Me.ehORF)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "frmORF"
        Me.Text = "ORF Search Options"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ehORF As Integration.ElementHost
End Class
