<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNucleotideEnzyme
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
        Me.btnLoadRE = New System.Windows.Forms.Button
        Me.btnLoadCodon = New System.Windows.Forms.Button
        Me.rtbOut = New System.Windows.Forms.RichTextBox
        Me.ofdOpenCodon = New System.Windows.Forms.OpenFileDialog
        Me.tbAnimo = New System.Windows.Forms.TextBox
        Me.btCalculate = New System.Windows.Forms.Button
        Me.ofdRE = New System.Windows.Forms.OpenFileDialog
        Me.SuspendLayout()
        '
        'btnLoadRE
        '
        Me.btnLoadRE.Location = New System.Drawing.Point(24, 70)
        Me.btnLoadRE.Name = "btnLoadRE"
        Me.btnLoadRE.Size = New System.Drawing.Size(106, 23)
        Me.btnLoadRE.TabIndex = 1
        Me.btnLoadRE.Text = "LoadRE"
        Me.btnLoadRE.UseVisualStyleBackColor = True
        '
        'btnLoadCodon
        '
        Me.btnLoadCodon.Location = New System.Drawing.Point(24, 28)
        Me.btnLoadCodon.Name = "btnLoadCodon"
        Me.btnLoadCodon.Size = New System.Drawing.Size(106, 23)
        Me.btnLoadCodon.TabIndex = 1
        Me.btnLoadCodon.Text = "LoadCodon"
        Me.btnLoadCodon.UseVisualStyleBackColor = True
        '
        'rtbOut
        '
        Me.rtbOut.Location = New System.Drawing.Point(152, 93)
        Me.rtbOut.Name = "rtbOut"
        Me.rtbOut.Size = New System.Drawing.Size(492, 328)
        Me.rtbOut.TabIndex = 2
        Me.rtbOut.Text = ""
        '
        'tbAnimo
        '
        Me.tbAnimo.Location = New System.Drawing.Point(152, 28)
        Me.tbAnimo.Name = "tbAnimo"
        Me.tbAnimo.Size = New System.Drawing.Size(492, 21)
        Me.tbAnimo.TabIndex = 3
        '
        'btCalculate
        '
        Me.btCalculate.Location = New System.Drawing.Point(24, 114)
        Me.btCalculate.Name = "btCalculate"
        Me.btCalculate.Size = New System.Drawing.Size(106, 23)
        Me.btCalculate.TabIndex = 1
        Me.btCalculate.Text = "Calculate"
        Me.btCalculate.UseVisualStyleBackColor = True
        '
        'frmNucleotideEnzyme
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(656, 441)
        Me.Controls.Add(Me.tbAnimo)
        Me.Controls.Add(Me.rtbOut)
        Me.Controls.Add(Me.btnLoadCodon)
        Me.Controls.Add(Me.btCalculate)
        Me.Controls.Add(Me.btnLoadRE)
        Me.Name = "frmNucleotideEnzyme"
        Me.Text = "frmNucleotideEnzyme"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnLoadRE As System.Windows.Forms.Button
    Friend WithEvents btnLoadCodon As System.Windows.Forms.Button
    Friend WithEvents rtbOut As System.Windows.Forms.RichTextBox
    Friend WithEvents ofdOpenCodon As System.Windows.Forms.OpenFileDialog
    Friend WithEvents tbAnimo As System.Windows.Forms.TextBox
    Friend WithEvents btCalculate As System.Windows.Forms.Button
    Friend WithEvents ofdRE As System.Windows.Forms.OpenFileDialog
End Class
