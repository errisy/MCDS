<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddFeature
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.tbLabel = New System.Windows.Forms.TextBox()
        Me.tbNote = New System.Windows.Forms.TextBox()
        Me.cbType = New System.Windows.Forms.ComboBox()
        Me.ll_OK = New System.Windows.Forms.LinkLabel()
        Me.cbDirection = New System.Windows.Forms.CheckBox()
        Me.nudStart = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.nudEnd = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.llCancel = New System.Windows.Forms.LinkLabel()
        CType(Me.nudStart, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudEnd, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Label"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 12)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Note"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 63)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 12)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Type"
        '
        'tbLabel
        '
        Me.tbLabel.Location = New System.Drawing.Point(101, 6)
        Me.tbLabel.Name = "tbLabel"
        Me.tbLabel.Size = New System.Drawing.Size(179, 21)
        Me.tbLabel.TabIndex = 1
        '
        'tbNote
        '
        Me.tbNote.Location = New System.Drawing.Point(101, 33)
        Me.tbNote.Name = "tbNote"
        Me.tbNote.Size = New System.Drawing.Size(179, 21)
        Me.tbNote.TabIndex = 2
        '
        'cbType
        '
        Me.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbType.FormattingEnabled = True
        Me.cbType.Items.AddRange(New Object() {"CDS", "gene", "exon", "enhancer", "loci", "misc_feature", "misc_signal", "mutation", "operon", "oriT", "primer_bind", "promoter", "RBS", "rep_origin", "terminator"})
        Me.cbType.Location = New System.Drawing.Point(101, 60)
        Me.cbType.Name = "cbType"
        Me.cbType.Size = New System.Drawing.Size(179, 20)
        Me.cbType.TabIndex = 3
        '
        'll_OK
        '
        Me.ll_OK.AutoSize = True
        Me.ll_OK.Location = New System.Drawing.Point(214, 141)
        Me.ll_OK.Name = "ll_OK"
        Me.ll_OK.Size = New System.Drawing.Size(17, 12)
        Me.ll_OK.TabIndex = 5
        Me.ll_OK.TabStop = True
        Me.ll_OK.Text = "OK"
        '
        'cbDirection
        '
        Me.cbDirection.AutoSize = True
        Me.cbDirection.Location = New System.Drawing.Point(12, 140)
        Me.cbDirection.Name = "cbDirection"
        Me.cbDirection.Size = New System.Drawing.Size(132, 16)
        Me.cbDirection.TabIndex = 6
        Me.cbDirection.Text = "Reverse Compentary"
        Me.cbDirection.UseVisualStyleBackColor = True
        '
        'nudStart
        '
        Me.nudStart.Location = New System.Drawing.Point(101, 86)
        Me.nudStart.Name = "nudStart"
        Me.nudStart.Size = New System.Drawing.Size(179, 21)
        Me.nudStart.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 88)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(35, 12)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Start"
        '
        'nudEnd
        '
        Me.nudEnd.Location = New System.Drawing.Point(101, 113)
        Me.nudEnd.Name = "nudEnd"
        Me.nudEnd.Size = New System.Drawing.Size(179, 21)
        Me.nudEnd.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 115)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(23, 12)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "End"
        '
        'llCancel
        '
        Me.llCancel.AutoSize = True
        Me.llCancel.Location = New System.Drawing.Point(241, 141)
        Me.llCancel.Name = "llCancel"
        Me.llCancel.Size = New System.Drawing.Size(41, 12)
        Me.llCancel.TabIndex = 8
        Me.llCancel.TabStop = True
        Me.llCancel.Text = "Cancel"
        '
        'frmAddFeature
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Ivory
        Me.ClientSize = New System.Drawing.Size(294, 161)
        Me.Controls.Add(Me.llCancel)
        Me.Controls.Add(Me.nudEnd)
        Me.Controls.Add(Me.nudStart)
        Me.Controls.Add(Me.cbDirection)
        Me.Controls.Add(Me.ll_OK)
        Me.Controls.Add(Me.cbType)
        Me.Controls.Add(Me.tbNote)
        Me.Controls.Add(Me.tbLabel)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmAddFeature"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Feature Property"
        CType(Me.nudStart, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudEnd, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tbLabel As System.Windows.Forms.TextBox
    Friend WithEvents tbNote As System.Windows.Forms.TextBox
    Friend WithEvents cbType As System.Windows.Forms.ComboBox
    Friend WithEvents ll_OK As System.Windows.Forms.LinkLabel
    Friend WithEvents cbDirection As System.Windows.Forms.CheckBox
    Friend WithEvents nudStart As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents nudEnd As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents llCancel As System.Windows.Forms.LinkLabel
End Class
