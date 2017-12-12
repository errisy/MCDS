<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPCR
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
        Me.tbForwardPrimer = New System.Windows.Forms.TextBox
        Me.tbReversePrimer = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.rtbTemplate = New System.Windows.Forms.RichTextBox
        Me.btnCalculateProduct = New System.Windows.Forms.Button
        Me.rtbProduct = New System.Windows.Forms.RichTextBox
        Me.btnCopyProduct = New System.Windows.Forms.Button
        Me.btnHelp = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'tbForwardPrimer
        '
        Me.tbForwardPrimer.Location = New System.Drawing.Point(136, 12)
        Me.tbForwardPrimer.Name = "tbForwardPrimer"
        Me.tbForwardPrimer.Size = New System.Drawing.Size(502, 21)
        Me.tbForwardPrimer.TabIndex = 0
        '
        'tbReversePrimer
        '
        Me.tbReversePrimer.Location = New System.Drawing.Point(136, 49)
        Me.tbReversePrimer.Name = "tbReversePrimer"
        Me.tbReversePrimer.Size = New System.Drawing.Size(502, 21)
        Me.tbReversePrimer.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(37, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 12)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Forward Primer"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(37, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 12)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Reverse Primer"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(37, 90)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 12)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Template"
        '
        'rtbTemplate
        '
        Me.rtbTemplate.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbTemplate.Location = New System.Drawing.Point(136, 90)
        Me.rtbTemplate.Name = "rtbTemplate"
        Me.rtbTemplate.Size = New System.Drawing.Size(502, 96)
        Me.rtbTemplate.TabIndex = 3
        Me.rtbTemplate.Text = ""
        '
        'btnCalculateProduct
        '
        Me.btnCalculateProduct.Location = New System.Drawing.Point(39, 195)
        Me.btnCalculateProduct.Name = "btnCalculateProduct"
        Me.btnCalculateProduct.Size = New System.Drawing.Size(75, 23)
        Me.btnCalculateProduct.TabIndex = 4
        Me.btnCalculateProduct.Text = "Product"
        Me.btnCalculateProduct.UseVisualStyleBackColor = True
        '
        'rtbProduct
        '
        Me.rtbProduct.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbProduct.Location = New System.Drawing.Point(136, 195)
        Me.rtbProduct.Name = "rtbProduct"
        Me.rtbProduct.Size = New System.Drawing.Size(502, 96)
        Me.rtbProduct.TabIndex = 3
        Me.rtbProduct.Text = ""
        '
        'btnCopyProduct
        '
        Me.btnCopyProduct.Location = New System.Drawing.Point(39, 224)
        Me.btnCopyProduct.Name = "btnCopyProduct"
        Me.btnCopyProduct.Size = New System.Drawing.Size(75, 23)
        Me.btnCopyProduct.TabIndex = 4
        Me.btnCopyProduct.Text = "Copy"
        Me.btnCopyProduct.UseVisualStyleBackColor = True
        '
        'btnHelp
        '
        Me.btnHelp.Location = New System.Drawing.Point(466, 307)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(75, 23)
        Me.btnHelp.TabIndex = 4
        Me.btnHelp.Text = "Help"
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(563, 307)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 4
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmPCR
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(689, 342)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.btnCopyProduct)
        Me.Controls.Add(Me.btnCalculateProduct)
        Me.Controls.Add(Me.rtbProduct)
        Me.Controls.Add(Me.rtbTemplate)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbReversePrimer)
        Me.Controls.Add(Me.tbForwardPrimer)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "frmPCR"
        Me.Text = "PCR Kit"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbForwardPrimer As System.Windows.Forms.TextBox
    Friend WithEvents tbReversePrimer As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents rtbTemplate As System.Windows.Forms.RichTextBox
    Friend WithEvents btnCalculateProduct As System.Windows.Forms.Button
    Friend WithEvents rtbProduct As System.Windows.Forms.RichTextBox
    Friend WithEvents btnCopyProduct As System.Windows.Forms.Button
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
