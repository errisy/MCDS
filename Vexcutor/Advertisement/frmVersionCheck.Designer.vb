<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVersionCheck
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
        Me.InteropHost1 = New MCDS.InteropHost()
        Me.VersionCheckInfo = New MCDS.VersionCheck()
        Me.SuspendLayout()
        '
        'InteropHost1
        '
        Me.InteropHost1.BackColor = System.Drawing.Color.White
        Me.InteropHost1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InteropHost1.Location = New System.Drawing.Point(0, 0)
        Me.InteropHost1.Name = "InteropHost1"
        Me.InteropHost1.Size = New System.Drawing.Size(680, 377)
        Me.InteropHost1.TabIndex = 1
        Me.InteropHost1.Text = "InteropHost1"
        Me.InteropHost1.Child = Me.VersionCheckInfo
        '
        'frmVersionCheck
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(680, 377)
        Me.Controls.Add(Me.InteropHost1)
        Me.Name = "frmVersionCheck"
        Me.Text = "New Version Available!"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents InteropHost1 As MCDS.InteropHost
    Friend VersionCheckInfo As MCDS.VersionCheck
End Class
