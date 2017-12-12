<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrimerEditorView
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
        Me.pafPrimerDesign = New MCDS.PrimerAnalysisFrame()
        Me.ElementHost1 = New System.Windows.Forms.Integration.ElementHost()
        Me.wpfPrimerEditor = New MCDS.PrimerEditor()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        CType(Me.pafPrimerDesign, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pafPrimerDesign
        '
        Me.pafPrimerDesign.Dock = System.Windows.Forms.DockStyle.Right
        Me.pafPrimerDesign.Location = New System.Drawing.Point(931, 0)
        Me.pafPrimerDesign.Name = "pafPrimerDesign"
        Me.pafPrimerDesign.ShowSequencing = True
        Me.pafPrimerDesign.Size = New System.Drawing.Size(316, 439)
        Me.pafPrimerDesign.TabIndex = 1
        Me.pafPrimerDesign.TabStop = False
        '
        'ElementHost1
        '
        Me.ElementHost1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ElementHost1.Location = New System.Drawing.Point(0, 0)
        Me.ElementHost1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ElementHost1.Name = "ElementHost1"
        Me.ElementHost1.Size = New System.Drawing.Size(931, 439)
        Me.ElementHost1.TabIndex = 2
        Me.ElementHost1.Text = "ElementHost1"
        Me.ElementHost1.Child = Me.wpfPrimerEditor
        '
        'Splitter1
        '
        Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Splitter1.Location = New System.Drawing.Point(928, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 439)
        Me.Splitter1.TabIndex = 3
        Me.Splitter1.TabStop = False
        '
        'PrimerEditorView
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.ElementHost1)
        Me.Controls.Add(Me.pafPrimerDesign)
        Me.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "PrimerEditorView"
        Me.Size = New System.Drawing.Size(1247, 439)
        CType(Me.pafPrimerDesign, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pafPrimerDesign As MCDS.PrimerAnalysisFrame
    Friend WithEvents ElementHost1 As System.Windows.Forms.Integration.ElementHost
    Friend wpfPrimerEditor As MCDS.PrimerEditor
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter

End Class
