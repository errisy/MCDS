<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UpdateWindow
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
        Me.ihMain = New MCDS.InteropHost()
        Me.wpfUpdateControl = New MCDS.UpdateControl()
        Me.SuspendLayout()
        '
        'ihMain
        '
        Me.ihMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ihMain.Location = New System.Drawing.Point(0, 0)
        Me.ihMain.Name = "ihMain"
        Me.ihMain.Size = New System.Drawing.Size(781, 446)
        Me.ihMain.TabIndex = 0
        Me.ihMain.Child = Me.wpfUpdateControl
        '
        'UpdateWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(781, 446)
        Me.Controls.Add(Me.ihMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "UpdateWindow"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MCDS Update is Available"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ihMain As InteropHost
    Friend WithEvents wpfUpdateControl As UpdateControl
End Class
