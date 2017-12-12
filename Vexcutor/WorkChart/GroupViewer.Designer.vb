<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GroupViewer
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
        Me.tcViewers = New MCDS.TabContainer()
        Me.SuspendLayout()
        '
        'tcViewers
        '
        Me.tcViewers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcViewers.Location = New System.Drawing.Point(0, 0)
        Me.tcViewers.Margin = New System.Windows.Forms.Padding(0)
        Me.tcViewers.Name = "tcViewers"
        Me.tcViewers.Padding = New System.Drawing.Point(0, 0)
        Me.tcViewers.SelectedIndex = 0
        Me.tcViewers.Size = New System.Drawing.Size(908, 448)
        Me.tcViewers.TabIndex = 0
        '
        'GroupViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.tcViewers)
        Me.Name = "GroupViewer"
        Me.Size = New System.Drawing.Size(908, 448)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tcViewers As MCDS.TabContainer

End Class
