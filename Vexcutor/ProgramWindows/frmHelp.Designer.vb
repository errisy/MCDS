<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHelp
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
        Me.wbHelp = New System.Windows.Forms.WebBrowser
        Me.SuspendLayout()
        '
        'wbHelp
        '
        Me.wbHelp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.wbHelp.Location = New System.Drawing.Point(0, 0)
        Me.wbHelp.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbHelp.Name = "wbHelp"
        Me.wbHelp.Size = New System.Drawing.Size(688, 500)
        Me.wbHelp.TabIndex = 0
        '
        'frmHelp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(688, 500)
        Me.Controls.Add(Me.wbHelp)
        Me.Name = "frmHelp"
        Me.Text = "frmHelp"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents wbHelp As System.Windows.Forms.WebBrowser
End Class
