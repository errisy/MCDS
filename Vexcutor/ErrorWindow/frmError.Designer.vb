<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmError
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
        Me.Host = New MCDS.InteropHost()
        Me.VexcutorErrorInfo = New MCDS.VexcutorError()
        Me.SuspendLayout()
        '
        'InteropHost1
        '
        Me.Host.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Host.Location = New System.Drawing.Point(0, 0)
        Me.Host.Name = "InteropHost1"
        Me.Host.Size = New System.Drawing.Size(649, 302)
        Me.Host.TabIndex = 0
        Me.Host.Text = "InteropHost1"
        Me.Host.Child = Me.VexcutorErrorInfo
        '
        'frmError
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(649, 302)
        Me.Controls.Add(Me.Host)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmError"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Error"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Host As MCDS.InteropHost
    Friend VexcutorErrorInfo As MCDS.VexcutorError
End Class
