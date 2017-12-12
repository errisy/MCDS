<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OperationView
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
        Me.bpbMain = New MCDS.BufferedPictureBox
        Me.sfdPic = New System.Windows.Forms.SaveFileDialog
        CType(Me.bpbMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'bpbMain
        '
        Me.bpbMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bpbMain.Location = New System.Drawing.Point(0, 0)
        Me.bpbMain.Name = "bpbMain"
        Me.bpbMain.Size = New System.Drawing.Size(705, 539)
        Me.bpbMain.TabIndex = 1
        Me.bpbMain.TabStop = False
        '
        'sfdPic
        '
        Me.sfdPic.Filter = "EMF Image|*.emf"
        '
        'OperationView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.bpbMain)
        Me.Name = "OperationView"
        Me.Size = New System.Drawing.Size(705, 539)
        CType(Me.bpbMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bpbMain As MCDS.BufferedPictureBox
    Friend WithEvents sfdPic As System.Windows.Forms.SaveFileDialog

End Class
