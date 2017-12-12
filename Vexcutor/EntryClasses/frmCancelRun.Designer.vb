<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCancelRun
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
        Me.ihCancel = New MCDS.InteropHost()
        Me.ucWPFCancelRun = New MCDS.WPFCancelRun()
        Me.StartPosition = FormStartPosition.CenterParent
        Me.ShowInTaskbar = False
        Me.SuspendLayout()
        '
        'ihCancel
        '
        Me.ihCancel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ihCancel.Location = New System.Drawing.Point(0, 0)
        Me.ihCancel.Name = "ihCancel"
        Me.ihCancel.Size = New System.Drawing.Size(593, 140)
        Me.ihCancel.TabIndex = 0
        Me.ihCancel.Text = "Cancel Run"
        Me.ihCancel.Child = Me.ucWPFCancelRun
        '
        'frmCancelRun
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(593, 140)
        Me.ControlBox = False
        Me.Controls.Add(Me.ihCancel)
        Me.Name = "frmCancelRun"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Cancel Long Calculation"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ihCancel As InteropHost
    Friend ucWPFCancelRun As WPFCancelRun
End Class
