<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class pasteurMerger
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
        Me.components = New System.ComponentModel.Container
        Me.ofdFile = New System.Windows.Forms.OpenFileDialog
        Me.fbdSave = New System.Windows.Forms.FolderBrowserDialog
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblBrowse = New System.Windows.Forms.LinkLabel
        Me.dgvTask = New System.Windows.Forms.DataGridView
        Me.cnName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.cnFileA = New System.Windows.Forms.DataGridViewButtonColumn
        Me.cnFileB = New System.Windows.Forms.DataGridViewButtonColumn
        Me.lblAdd = New System.Windows.Forms.LinkLabel
        Me.lblStart = New System.Windows.Forms.LinkLabel
        Me.tmrWatcher = New System.Windows.Forms.Timer(Me.components)
        CType(Me.dgvTask, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ofdFile
        '
        Me.ofdFile.Filter = "Seq Files|*.txt;*.seq"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Save to Folder:"
        '
        'lblBrowse
        '
        Me.lblBrowse.AutoSize = True
        Me.lblBrowse.Location = New System.Drawing.Point(24, 40)
        Me.lblBrowse.Name = "lblBrowse"
        Me.lblBrowse.Size = New System.Drawing.Size(53, 12)
        Me.lblBrowse.TabIndex = 1
        Me.lblBrowse.TabStop = True
        Me.lblBrowse.Text = "[Browse]"
        '
        'dgvTask
        '
        Me.dgvTask.AllowUserToAddRows = False
        Me.dgvTask.AllowUserToResizeRows = False
        Me.dgvTask.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTask.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.cnName, Me.cnFileA, Me.cnFileB})
        Me.dgvTask.Location = New System.Drawing.Point(26, 101)
        Me.dgvTask.Name = "dgvTask"
        Me.dgvTask.RowTemplate.Height = 23
        Me.dgvTask.Size = New System.Drawing.Size(741, 150)
        Me.dgvTask.TabIndex = 2
        '
        'cnName
        '
        Me.cnName.HeaderText = "Task"
        Me.cnName.Name = "cnName"
        Me.cnName.ReadOnly = True
        Me.cnName.Width = 80
        '
        'cnFileA
        '
        Me.cnFileA.HeaderText = "FileA"
        Me.cnFileA.Name = "cnFileA"
        Me.cnFileA.Width = 300
        '
        'cnFileB
        '
        Me.cnFileB.HeaderText = "FileB"
        Me.cnFileB.Name = "cnFileB"
        Me.cnFileB.Width = 300
        '
        'lblAdd
        '
        Me.lblAdd.AutoSize = True
        Me.lblAdd.Location = New System.Drawing.Point(24, 86)
        Me.lblAdd.Name = "lblAdd"
        Me.lblAdd.Size = New System.Drawing.Size(65, 12)
        Me.lblAdd.TabIndex = 1
        Me.lblAdd.TabStop = True
        Me.lblAdd.Text = "[Add Task]"
        '
        'lblStart
        '
        Me.lblStart.AutoSize = True
        Me.lblStart.Location = New System.Drawing.Point(684, 270)
        Me.lblStart.Name = "lblStart"
        Me.lblStart.Size = New System.Drawing.Size(83, 12)
        Me.lblStart.TabIndex = 1
        Me.lblStart.TabStop = True
        Me.lblStart.Text = "[Start Merge]"
        '
        'tmrWatcher
        '
        '
        'pasteurMerger
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(787, 515)
        Me.Controls.Add(Me.dgvTask)
        Me.Controls.Add(Me.lblStart)
        Me.Controls.Add(Me.lblAdd)
        Me.Controls.Add(Me.lblBrowse)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "pasteurMerger"
        Me.Text = "pasteurMerger"
        CType(Me.dgvTask, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ofdFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents fbdSave As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblBrowse As System.Windows.Forms.LinkLabel
    Friend WithEvents dgvTask As System.Windows.Forms.DataGridView
    Friend WithEvents lblAdd As System.Windows.Forms.LinkLabel
    Friend WithEvents lblStart As System.Windows.Forms.LinkLabel
    Friend WithEvents cnName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cnFileA As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents cnFileB As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents tmrWatcher As System.Windows.Forms.Timer
End Class
