<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEnzReport
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
        Me.dgv_Report = New System.Windows.Forms.DataGridView
        CType(Me.dgv_Report, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgv_Report
        '
        Me.dgv_Report.AllowUserToAddRows = False
        Me.dgv_Report.AllowUserToDeleteRows = False
        Me.dgv_Report.AllowUserToResizeRows = False
        Me.dgv_Report.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Report.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgv_Report.Location = New System.Drawing.Point(0, 0)
        Me.dgv_Report.Name = "dgv_Report"
        Me.dgv_Report.RowTemplate.Height = 23
        Me.dgv_Report.Size = New System.Drawing.Size(679, 320)
        Me.dgv_Report.TabIndex = 0
        '
        'frmEnzReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(679, 320)
        Me.Controls.Add(Me.dgv_Report)
        Me.Name = "frmEnzReport"
        Me.Text = "frmEnzReport"
        CType(Me.dgv_Report, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgv_Report As System.Windows.Forms.DataGridView
End Class
