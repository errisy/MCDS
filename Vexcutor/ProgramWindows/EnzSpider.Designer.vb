<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_EnzSpider
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
        Me.btn_Excute = New System.Windows.Forms.Button
        Me.rtb_Sequence = New System.Windows.Forms.RichTextBox
        Me.btn_Load = New System.Windows.Forms.Button
        Me.btn_Report = New System.Windows.Forms.Button
        Me.btn_Design = New System.Windows.Forms.Button
        Me.tb_Name = New System.Windows.Forms.TextBox
        Me.dgv_seq = New System.Windows.Forms.DataGridView
        Me.clm_Index = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clm_Selected = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.clm_Name = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clm_Length = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clm_Seq = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.btn_LoadEnzymes = New System.Windows.Forms.Button
        Me.ofdRE = New System.Windows.Forms.OpenFileDialog
        Me.btn_LoadFiles = New System.Windows.Forms.Button
        Me.ofdLoadFiles = New System.Windows.Forms.OpenFileDialog
        Me.lv_Enz = New System.Windows.Forms.ListView
        Me.cn_Name = New System.Windows.Forms.ColumnHeader
        Me.cn_PATTERN = New System.Windows.Forms.ColumnHeader
        Me.cn_Cut = New System.Windows.Forms.ColumnHeader
        Me.cb_MCS = New System.Windows.Forms.CheckBox
        Me.btn_Standard = New System.Windows.Forms.Button
        Me.btn_SelAll = New System.Windows.Forms.Button
        Me.btn_UnSelAll = New System.Windows.Forms.Button
        CType(Me.dgv_seq, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btn_Excute
        '
        Me.btn_Excute.Location = New System.Drawing.Point(604, 232)
        Me.btn_Excute.Name = "btn_Excute"
        Me.btn_Excute.Size = New System.Drawing.Size(75, 74)
        Me.btn_Excute.TabIndex = 3
        Me.btn_Excute.Text = "Excute"
        Me.btn_Excute.UseVisualStyleBackColor = True
        '
        'rtb_Sequence
        '
        Me.rtb_Sequence.Location = New System.Drawing.Point(49, 446)
        Me.rtb_Sequence.Name = "rtb_Sequence"
        Me.rtb_Sequence.Size = New System.Drawing.Size(630, 154)
        Me.rtb_Sequence.TabIndex = 4
        Me.rtb_Sequence.Text = ""
        '
        'btn_Load
        '
        Me.btn_Load.Location = New System.Drawing.Point(208, 606)
        Me.btn_Load.Name = "btn_Load"
        Me.btn_Load.Size = New System.Drawing.Size(75, 23)
        Me.btn_Load.TabIndex = 5
        Me.btn_Load.Text = "Load"
        Me.btn_Load.UseVisualStyleBackColor = True
        '
        'btn_Report
        '
        Me.btn_Report.Location = New System.Drawing.Point(604, 12)
        Me.btn_Report.Name = "btn_Report"
        Me.btn_Report.Size = New System.Drawing.Size(75, 23)
        Me.btn_Report.TabIndex = 6
        Me.btn_Report.Text = "EnzReport"
        Me.btn_Report.UseVisualStyleBackColor = True
        '
        'btn_Design
        '
        Me.btn_Design.Location = New System.Drawing.Point(604, 41)
        Me.btn_Design.Name = "btn_Design"
        Me.btn_Design.Size = New System.Drawing.Size(75, 24)
        Me.btn_Design.TabIndex = 7
        Me.btn_Design.Text = "Design"
        Me.btn_Design.UseVisualStyleBackColor = True
        '
        'tb_Name
        '
        Me.tb_Name.Location = New System.Drawing.Point(53, 606)
        Me.tb_Name.Name = "tb_Name"
        Me.tb_Name.Size = New System.Drawing.Size(141, 21)
        Me.tb_Name.TabIndex = 11
        '
        'dgv_seq
        '
        Me.dgv_seq.AllowUserToAddRows = False
        Me.dgv_seq.AllowUserToDeleteRows = False
        Me.dgv_seq.AllowUserToResizeColumns = False
        Me.dgv_seq.AllowUserToResizeRows = False
        Me.dgv_seq.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_seq.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clm_Index, Me.clm_Selected, Me.clm_Name, Me.clm_Length, Me.clm_Seq})
        Me.dgv_seq.Location = New System.Drawing.Point(49, 12)
        Me.dgv_seq.MultiSelect = False
        Me.dgv_seq.Name = "dgv_seq"
        Me.dgv_seq.RowTemplate.Height = 23
        Me.dgv_seq.Size = New System.Drawing.Size(549, 187)
        Me.dgv_seq.TabIndex = 12
        '
        'clm_Index
        '
        Me.clm_Index.FillWeight = 40.0!
        Me.clm_Index.HeaderText = "Index"
        Me.clm_Index.Name = "clm_Index"
        Me.clm_Index.ReadOnly = True
        Me.clm_Index.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'clm_Selected
        '
        Me.clm_Selected.FillWeight = 50.0!
        Me.clm_Selected.HeaderText = "Selected"
        Me.clm_Selected.Name = "clm_Selected"
        '
        'clm_Name
        '
        Me.clm_Name.HeaderText = "Name"
        Me.clm_Name.Name = "clm_Name"
        Me.clm_Name.ReadOnly = True
        Me.clm_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'clm_Length
        '
        Me.clm_Length.HeaderText = "Length"
        Me.clm_Length.Name = "clm_Length"
        Me.clm_Length.ReadOnly = True
        Me.clm_Length.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'clm_Seq
        '
        Me.clm_Seq.HeaderText = "Sequence"
        Me.clm_Seq.Name = "clm_Seq"
        Me.clm_Seq.ReadOnly = True
        Me.clm_Seq.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'btn_LoadEnzymes
        '
        Me.btn_LoadEnzymes.Location = New System.Drawing.Point(409, 606)
        Me.btn_LoadEnzymes.Name = "btn_LoadEnzymes"
        Me.btn_LoadEnzymes.Size = New System.Drawing.Size(131, 23)
        Me.btn_LoadEnzymes.TabIndex = 13
        Me.btn_LoadEnzymes.Text = "Load Enzymes"
        Me.btn_LoadEnzymes.UseVisualStyleBackColor = True
        '
        'btn_LoadFiles
        '
        Me.btn_LoadFiles.Location = New System.Drawing.Point(289, 606)
        Me.btn_LoadFiles.Name = "btn_LoadFiles"
        Me.btn_LoadFiles.Size = New System.Drawing.Size(114, 23)
        Me.btn_LoadFiles.TabIndex = 5
        Me.btn_LoadFiles.Text = "Load Files"
        Me.btn_LoadFiles.UseVisualStyleBackColor = True
        '
        'ofdLoadFiles
        '
        Me.ofdLoadFiles.FileName = "OpenFileDialog1"
        Me.ofdLoadFiles.Filter = """Genbank or Sequence Files|*.gb;*.txt;*.seq"""
        Me.ofdLoadFiles.Multiselect = True
        '
        'lv_Enz
        '
        Me.lv_Enz.CheckBoxes = True
        Me.lv_Enz.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.cn_Name, Me.cn_PATTERN, Me.cn_Cut})
        Me.lv_Enz.Location = New System.Drawing.Point(690, 14)
        Me.lv_Enz.Name = "lv_Enz"
        Me.lv_Enz.Size = New System.Drawing.Size(250, 586)
        Me.lv_Enz.TabIndex = 14
        Me.lv_Enz.UseCompatibleStateImageBehavior = False
        Me.lv_Enz.View = System.Windows.Forms.View.Details
        '
        'cn_Name
        '
        Me.cn_Name.Text = "Name"
        Me.cn_Name.Width = 72
        '
        'cn_PATTERN
        '
        Me.cn_PATTERN.Text = "Pattern"
        Me.cn_PATTERN.Width = 99
        '
        'cn_Cut
        '
        Me.cn_Cut.Text = "Cut"
        '
        'cb_MCS
        '
        Me.cb_MCS.AutoSize = True
        Me.cb_MCS.Checked = True
        Me.cb_MCS.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cb_MCS.Location = New System.Drawing.Point(604, 71)
        Me.cb_MCS.Name = "cb_MCS"
        Me.cb_MCS.Size = New System.Drawing.Size(72, 16)
        Me.cb_MCS.TabIndex = 15
        Me.cb_MCS.Text = "Only MCS"
        Me.cb_MCS.UseVisualStyleBackColor = True
        '
        'btn_Standard
        '
        Me.btn_Standard.Location = New System.Drawing.Point(604, 175)
        Me.btn_Standard.Name = "btn_Standard"
        Me.btn_Standard.Size = New System.Drawing.Size(75, 24)
        Me.btn_Standard.TabIndex = 7
        Me.btn_Standard.Text = "Standard"
        Me.btn_Standard.UseVisualStyleBackColor = True
        '
        'btn_SelAll
        '
        Me.btn_SelAll.Location = New System.Drawing.Point(690, 605)
        Me.btn_SelAll.Name = "btn_SelAll"
        Me.btn_SelAll.Size = New System.Drawing.Size(95, 24)
        Me.btn_SelAll.TabIndex = 16
        Me.btn_SelAll.Text = "Select All"
        Me.btn_SelAll.UseVisualStyleBackColor = True
        '
        'btn_UnSelAll
        '
        Me.btn_UnSelAll.Location = New System.Drawing.Point(791, 605)
        Me.btn_UnSelAll.Name = "btn_UnSelAll"
        Me.btn_UnSelAll.Size = New System.Drawing.Size(91, 24)
        Me.btn_UnSelAll.TabIndex = 16
        Me.btn_UnSelAll.Text = "Unselect All"
        Me.btn_UnSelAll.UseVisualStyleBackColor = True
        '
        'frm_EnzSpider
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(952, 635)
        Me.Controls.Add(Me.btn_UnSelAll)
        Me.Controls.Add(Me.btn_SelAll)
        Me.Controls.Add(Me.cb_MCS)
        Me.Controls.Add(Me.lv_Enz)
        Me.Controls.Add(Me.btn_LoadEnzymes)
        Me.Controls.Add(Me.dgv_seq)
        Me.Controls.Add(Me.tb_Name)
        Me.Controls.Add(Me.btn_Standard)
        Me.Controls.Add(Me.btn_Design)
        Me.Controls.Add(Me.btn_Report)
        Me.Controls.Add(Me.btn_LoadFiles)
        Me.Controls.Add(Me.btn_Load)
        Me.Controls.Add(Me.rtb_Sequence)
        Me.Controls.Add(Me.btn_Excute)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frm_EnzSpider"
        Me.Text = "EnzSpider"
        CType(Me.dgv_seq, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btn_Excute As System.Windows.Forms.Button
    Friend WithEvents rtb_Sequence As System.Windows.Forms.RichTextBox
    Friend WithEvents btn_Load As System.Windows.Forms.Button
    Friend WithEvents btn_Report As System.Windows.Forms.Button
    Friend WithEvents btn_Design As System.Windows.Forms.Button
    Friend WithEvents tb_Name As System.Windows.Forms.TextBox
    Friend WithEvents dgv_seq As System.Windows.Forms.DataGridView
    Friend WithEvents btn_LoadEnzymes As System.Windows.Forms.Button
    Friend WithEvents ofdRE As System.Windows.Forms.OpenFileDialog
    Friend WithEvents clm_Index As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clm_Selected As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents clm_Name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clm_Length As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clm_Seq As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btn_LoadFiles As System.Windows.Forms.Button
    Friend WithEvents ofdLoadFiles As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lv_Enz As System.Windows.Forms.ListView
    Friend WithEvents cn_Name As System.Windows.Forms.ColumnHeader
    Friend WithEvents cn_PATTERN As System.Windows.Forms.ColumnHeader
    Friend WithEvents cn_Cut As System.Windows.Forms.ColumnHeader
    Friend WithEvents cb_MCS As System.Windows.Forms.CheckBox
    Friend WithEvents btn_Standard As System.Windows.Forms.Button
    Friend WithEvents btn_SelAll As System.Windows.Forms.Button
    Friend WithEvents btn_UnSelAll As System.Windows.Forms.Button
End Class
