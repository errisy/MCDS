<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOnline
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOnline))
        Me.iHost = New MCDS.InteropHost()
        Me.OnlineDatabaseImporter1 = New MCDS.OnlineDatabaseImporter()
        Me.SuspendLayout()
        '
        'iHost
        '
        Me.iHost.Dock = System.Windows.Forms.DockStyle.Fill
        Me.iHost.Location = New System.Drawing.Point(0, 0)
        Me.iHost.Name = "iHost"
        Me.iHost.Size = New System.Drawing.Size(1076, 529)
        Me.iHost.TabIndex = 0
        Me.iHost.Child = Me.OnlineDatabaseImporter1
        '
        'frmOnline
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1076, 529)
        Me.Controls.Add(Me.iHost)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmOnline"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Genes from Online Database"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents iHost As InteropHost
    Friend OnlineDatabaseImporter1 As OnlineDatabaseImporter
End Class
