<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmErrorReport
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
        Me.lvError = New System.Windows.Forms.ListView
        Me.cn_Error = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'lvError
        '
        Me.lvError.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.cn_Error})
        Me.lvError.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvError.Location = New System.Drawing.Point(0, 0)
        Me.lvError.Name = "lvError"
        Me.lvError.Size = New System.Drawing.Size(454, 344)
        Me.lvError.TabIndex = 0
        Me.lvError.UseCompatibleStateImageBehavior = False
        Me.lvError.View = System.Windows.Forms.View.Details
        '
        'cn_Error
        '
        Me.cn_Error.Text = "Error"
        Me.cn_Error.Width = 450
        '
        'frmErrorReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(454, 344)
        Me.Controls.Add(Me.lvError)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmErrorReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "ERROR REPORT"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvError As System.Windows.Forms.ListView
    Friend WithEvents cn_Error As System.Windows.Forms.ColumnHeader
End Class
