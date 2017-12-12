Public Class frm_EnzSpider
    Dim r As Nuctions.RestrictionEnzymes

    Private Sub btn_Load_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Load.Click
        Dim seq As String = Nuctions.TAGCFilter(rtb_Sequence.Text)

        AddSequence(seq, tb_Name.Text)

        rtb_Sequence.Clear()
        tb_Name.Clear()

    End Sub

    Private Sub AddSequence(ByVal vSequence As String, ByVal vName As String)
        Dim index As Integer = dgv_seq.Rows.Add()
        dgv_seq.Rows(index).Cells(0).Value = dgv_seq.Rows.Count
        dgv_seq.Rows(index).Cells(1).Value = True
        dgv_seq.Rows(index).Cells(2).Value = vName
        dgv_seq.Rows(index).Cells(3).Value = vSequence.Length
        dgv_seq.Rows(index).Cells(4).Value = vSequence

        Dim cb As ComboBox
        For Each cb In CB_SEQ
            If cb Is Nothing Then Continue For
            cb.Items.Add("#" + dgv_seq.Rows.Count.ToString + ":" + vName)
        Next

    End Sub

    Private Sub btn_Excute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Excute.Click

        Dim re As Nuctions.RestrictionEnzyme

        Dim lvi As ListViewItem

        Dim recol As New Collection

        If r Is Nothing Then MsgBox("The restriction enzymes has not been loaded!") : Exit Sub

        For Each lvi In lv_Enz.Items
            If lvi.Checked = False Then Continue For
            re = lvi.Tag
            recol.Add(re, re.Name)
        Next


        Dim i As Integer
        For i = 0 To 7
            If CB_AND(i).Checked Then recol = Search(recol, CB_OPERATOR(i), NUD_OCCUR(i), CB_SEQ(i), CB_REGION(i), TB_START(i), TB_END(i))
        Next

        rtb_Sequence.Text = ""
        For Each re In recol
            rtb_Sequence.AppendText(re.Name + ControlChars.NewLine)
        Next

    End Sub

    Private Function Search(ByVal RE_COLLECTION As Collection, ByVal CB_Operator As ComboBox, ByVal NUD_Occur As NumericUpDown, ByVal CB_SEQ As ComboBox, ByVal CB_Region As CheckBox, ByVal TB_Start As TextBox, ByVal TB_End As TextBox) As Collection
        Dim re As Nuctions.RestrictionEnzyme
        Dim row As Integer
        Dim seq As String
        Dim mc As System.Text.RegularExpressions.MatchCollection
        Dim recol As Collection = RE_COLLECTION

        'search engine
        Select Case CB_SEQ.SelectedIndex
            Case 0 'All
                seq = ""
                For row = 0 To dgv_seq.Rows.Count - 1
                    seq &= dgv_seq.Rows(row).Cells(4).Value + " "
                Next
                For Each re In recol
                    mc = re.Reg.Matches(seq)
                    Select Case CB_Operator.SelectedIndex
                        Case 0 '=
                            If Not (mc.Count = NUD_Occur.Value) Then recol.Remove(re.Name)
                        Case 1 '>
                            If Not (mc.Count > NUD_Occur.Value) Then recol.Remove(re.Name)
                        Case 2 '<
                            If Not (mc.Count < NUD_Occur.Value) Then recol.Remove(re.Name)
                    End Select
                Next

            Case 1 'Selected
                seq = ""
                For row = 0 To dgv_seq.Rows.Count - 1
                    seq &= IIf(dgv_seq.Rows(row).Cells(1).Value, dgv_seq.Rows(row).Cells(4).Value + " ", "")
                Next
                For Each re In recol
                    mc = re.Reg.Matches(seq)
                    Select Case CB_Operator.SelectedIndex
                        Case 0 '=
                            If Not (mc.Count = NUD_Occur.Value) Then recol.Remove(re.Name)
                        Case 1 '>
                            If Not (mc.Count > NUD_Occur.Value) Then recol.Remove(re.Name)
                        Case 2 '<
                            If Not (mc.Count < NUD_Occur.Value) Then recol.Remove(re.Name)
                    End Select
                Next
            Case 2 'Unselected
                seq = ""
                For row = 0 To dgv_seq.Rows.Count - 1
                    seq &= IIf(Not dgv_seq.Rows(row).Cells(1).Value, dgv_seq.Rows(row).Cells(4).Value + " ", "")
                Next
                For Each re In recol
                    mc = re.Reg.Matches(seq)
                    Select Case CB_Operator.SelectedIndex
                        Case 0 '=
                            If Not (mc.Count = NUD_Occur.Value) Then recol.Remove(re.Name)
                        Case 1 '>
                            If Not (mc.Count > NUD_Occur.Value) Then recol.Remove(re.Name)
                        Case 2 '<
                            If Not (mc.Count < NUD_Occur.Value) Then recol.Remove(re.Name)
                    End Select
                Next
            Case Else 'Single
                seq = ""

                seq &= dgv_seq.Rows(CB_SEQ.SelectedIndex - 3).Cells(4).Value


                If CB_Region.Checked Then seq = seq.Substring(CInt(TB_Start.Text) - 1, CInt(TB_End.Text) - CInt(TB_Start.Text))

                For Each re In recol
                    mc = re.Reg.Matches(seq)
                    Select Case CB_Operator.SelectedIndex
                        Case 0 '=
                            If Not (mc.Count = NUD_Occur.Value) Then recol.Remove(re.Name)
                        Case 1 '>
                            If Not (mc.Count > NUD_Occur.Value) Then recol.Remove(re.Name)
                        Case 2 '<
                            If Not (mc.Count < NUD_Occur.Value) Then recol.Remove(re.Name)
                    End Select
                Next

        End Select

        Return recol
    End Function
    Private Sub btn_LoadEnzymes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_LoadEnzymes.Click
        If ofdRE.ShowDialog() = Windows.Forms.DialogResult.OK Then
            r = Nuctions.LoadRestrictionEnzymes(ofdRE.FileName)
            Dim re As Nuctions.RestrictionEnzyme
            Dim lvi As ListViewItem
            For Each re In r.RECollection
                lvi = New ListViewItem(re.Name)
                lvi.SubItems.Add(re.Sequence)
                lvi.SubItems.Add(re.OverhangNT)
                lv_Enz.Items.Add(lvi)
                lvi.Tag = re
                lvi.Checked = True
            Next
            lv_Enz.Sorting = SortOrder.Ascending
            lv_Enz.Sort()
        End If
    End Sub



#Region "Control Method"
    Private Sub CB_SEQ_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim i As Integer = sender.tag
        If CB_SEQ(i).SelectedIndex < 3 Then
            CB_REGION(i).Checked = False
        Else
            CB_REGION(i).Checked = True
            TB_START(i).Text = "1"
            Dim s_index As Integer = CB_SEQ(i).SelectedIndex
            TB_END(i).Text = dgv_seq.Rows(s_index - 3).Cells(3).Value
        End If
    End Sub

    Private Sub CB_REGION_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim i As Integer = sender.tag
        TB_START(i).Enabled = CB_REGION(i).Checked
        TB_END(i).Enabled = CB_REGION(i).Checked
    End Sub

    Private Sub CB_AND_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim i As Integer = sender.tag
        CB_OPERATOR(i).Enabled = CB_AND(i).Checked
        NUD_OCCUR(i).Enabled = CB_AND(i).Checked
        CB_SEQ(i).Enabled = CB_AND(i).Checked
        CB_REGION(i).Enabled = CB_AND(i).Checked
        TB_START(i).Enabled = CB_REGION(i).checked
        TB_END(i).Enabled = CB_REGION(i).Checked
    End Sub

    Private Sub TB_REGION_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar < "0" Or e.KeyChar > "9" Then e.Handled = True
    End Sub
#End Region

    Private CB_AND(8) As CheckBox
    Private CB_OPERATOR(8) As ComboBox
    Private NUD_OCCUR(8) As NumericUpDown
    Private LBL_IN(8) As Label
    Private CB_SEQ(8) As ComboBox
    Private CB_REGION(8) As CheckBox
    Private LBL_FROM(8) As Label
    Private TB_START(8) As TextBox
    Private LBL_TO(8) As Label
    Private TB_END(8) As TextBox

    Private Sub frm_EnzSpider_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Integer
        Me.SuspendLayout()
        For i = 0 To 7
            'CB_AND
            CB_AND(i) = New CheckBox
            CB_AND(i).Text = "AND"
            CB_AND(i).Tag = i
            CB_AND(i).AutoSize = True
            CB_AND(i).Location = New Point(15, 234 + 26 * i)
            'CB_OPOERATOR
            CB_OPERATOR(i) = New ComboBox
            CB_OPERATOR(i).Items.Add("=")
            CB_OPERATOR(i).Items.Add(">")
            CB_OPERATOR(i).Items.Add("<")
            CB_OPERATOR(i).SelectedIndex = 0
            CB_OPERATOR(i).Tag = i
            CB_OPERATOR(i).Size = New Size(40, 20)
            CB_OPERATOR(i).Location = New Point(63, 233 + 26 * i)
            'NUD_OCCUR
            NUD_OCCUR(i) = New NumericUpDown
            NUD_OCCUR(i).Maximum = 100
            NUD_OCCUR(i).Minimum = -100
            NUD_OCCUR(i).Size = New Size(40, 20)
            NUD_OCCUR(i).Location = New Point(113, 233 + 26 * i)
            'LBL_IN
            LBL_IN(i) = New Label
            LBL_IN(i).Text = "IN"
            LBL_IN(i).AutoSize = True
            LBL_IN(i).Location = New Point(163, 237 + 26 * i)
            'CB_SEQ
            CB_SEQ(i) = New ComboBox
            CB_SEQ(i).Items.Add("All")
            CB_SEQ(i).Items.Add("Selected")
            CB_SEQ(i).Items.Add("Unselected")
            CB_SEQ(i).SelectedIndex = 0
            CB_SEQ(i).Tag = i
            CB_SEQ(i).Size = New Size(121, 20)
            CB_SEQ(i).Location = New Point(196, 234 + 26 * i)
            'CB_REGION
            CB_REGION(i) = New CheckBox
            CB_REGION(i).Text = "Region"
            CB_REGION(i).Tag = i
            CB_REGION(i).AutoSize = True
            CB_REGION(i).Location = New Point(333, 236 + 26 * i)
            'LBL_FROM
            LBL_FROM(i) = New Label
            LBL_FROM(i).Text = "From"
            LBL_FROM(i).AutoSize = True
            LBL_FROM(i).Location = New Point(399, 237 + 26 * i)
            'TB_START
            TB_START(i) = New TextBox
            TB_START(i).Size = New Size(51, 21)
            TB_START(i).Location = New Point(434, 233 + 26 * i)
            'LBL_TO
            LBL_TO(i) = New Label
            LBL_TO(i).Text = "To"
            LBL_TO(i).AutoSize = True
            LBL_TO(i).Location = New Point(493, 237 + 26 * i)
            'TB_END
            TB_END(i) = New TextBox
            TB_END(i).Size = New Size(51, 21)
            TB_END(i).Location = New Point(516, 233 + 26 * i)
            'ADD_EVENTS
            AddHandler CB_AND(i).CheckedChanged, AddressOf CB_AND_CheckedChanged
            AddHandler TB_START(i).KeyPress, AddressOf TB_REGION_KeyPress
            AddHandler TB_END(i).KeyPress, AddressOf TB_REGION_KeyPress
            AddHandler CB_REGION(i).CheckedChanged, AddressOf CB_REGION_CheckedChanged
            AddHandler CB_SEQ(i).SelectedIndexChanged, AddressOf CB_SEQ_SelectedIndexChanged
            'ADD_CONTROLS
            Me.Controls.Add(CB_AND(i))
            Me.Controls.Add(CB_OPERATOR(i))
            Me.Controls.Add(NUD_OCCUR(i))
            Me.Controls.Add(LBL_IN(i))
            Me.Controls.Add(CB_SEQ(i))
            Me.Controls.Add(CB_REGION(i))
            Me.Controls.Add(TB_START(i))
            Me.Controls.Add(LBL_TO(i))
            Me.Controls.Add(TB_END(i))

            'PRESET
            CB_AND(i).Checked = True
            CB_AND(i).Checked = False
        Next
        CB_AND(0).Checked = True

        Me.ResumeLayout(True)

    End Sub

    Private Sub btn_LoadFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_LoadFiles.Click
        'load multiple files from files.

        If Not ofdLoadFiles.ShowDialog = Windows.Forms.DialogResult.OK Then Exit Sub
        Dim filename, content As String
        Dim regGB As New System.Text.RegularExpressions.Regex("ORIGIN([\satgcATGC1234567890]+)\/\/", System.Text.RegularExpressions.RegexOptions.None)


        For Each filename In ofdLoadFiles.FileNames
            Dim f As New System.IO.FileInfo(filename)
            filename = filename.ToUpper()
            If filename.LastIndexOf(".GB") Then
                If System.IO.File.Exists(filename) Then
                    content = System.IO.File.ReadAllText(filename)
                    content = regGB.Match(content).Groups(0).Value
                    AddSequence(Nuctions.TAGCFilter(content), f.Name)
                End If
            ElseIf filename.LastIndexOf(".TXT") Or filename.LastIndexOf(".SEQ") Then
                If System.IO.File.Exists(filename) Then
                    content = System.IO.File.ReadAllText(filename)
                    AddSequence(Nuctions.TAGCFilter(content), f.Name)
                End If
            End If
        Next


    End Sub

    Private Sub btn_Report_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Report.Click
        'generate an emzyme report
        Dim re As Nuctions.RestrictionEnzyme

        Dim lvi As ListViewItem

        Dim recol As New Collection

        If r Is Nothing Then MsgBox("The restriction enzymes has not been loaded!") : Exit Sub
        Dim fm As New frmEnzReport
        fm.dgv_Report.Columns.Add("clmn_Sequences", "Sequences")

        For Each lvi In lv_Enz.Items
            If lvi.Checked = False Then Continue For
            re = lvi.Tag
            recol.Add(re, re.Name)
            fm.dgv_Report.Columns.Add("clmn_" + re.Name, re.Name)
        Next

        Dim i, cellid, occur As Integer
        Dim row As DataGridViewRow
        For i = 0 To dgv_seq.Rows.Count - 1
            If Not dgv_seq.Rows(i).Cells(1).Value Then Continue For
            row = fm.dgv_Report.Rows(fm.dgv_Report.Rows.Add)
            row.Cells(0).Value = dgv_seq.Rows(i).Cells(2).Value
            cellid = 0
            For Each re In recol
                cellid += 1
                occur = re.Reg.Matches(dgv_seq.Rows(i).Cells(4).Value).Count
                row.Cells(cellid).Value = occur.ToString
                Select Case occur
                    Case 0
                        row.Cells(cellid).Style.BackColor = Color.Yellow
                    Case 1
                        row.Cells(cellid).Style.BackColor = Color.Tomato
                    Case 2
                        row.Cells(cellid).Style.BackColor = Color.Red
                    Case Else
                        row.Cells(cellid).Style.BackColor = Color.DarkRed
                End Select

            Next
        Next

        fm.Show()

    End Sub

    Private Sub btn_Design_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Design.Click
        If (CB_SEQ(0).SelectedIndex - 3) < 0 Then MsgBox("Unable to design! You should first select an vector in the first line and describe its MCS region.") : Exit Sub
        Dim vector As String = dgv_seq.Rows(CB_SEQ(0).SelectedIndex - 3).Cells(4).Value.ToString
        Dim vstart As Integer = CInt(TB_START(0).Text) - 1, vend As Integer = CInt(TB_END(0).Text) - 1
        Dim MCS As String = vector.Substring(vstart, vend - vstart)
        Dim VEC As String = vector.Remove(vstart, vend - vstart).Insert(vstart, " ")
        Dim re As Nuctions.RestrictionEnzyme

        Dim lvi As ListViewItem

        Dim recol As New Collection

        If r Is Nothing Then MsgBox("The restriction enzymes has not been loaded!") : Exit Sub
        Dim fm As New frmEnzReport
        fm.dgv_Report.Columns.Add("clmn_Sequences", "Sequences")

        For Each lvi In lv_Enz.Items
            If lvi.Checked = False Then Continue For
            re = lvi.Tag
            If re.Reg.IsMatch(VEC) Then Continue For
            If cb_MCS.Checked And Not re.Reg.IsMatch(MCS) Then Continue For
            recol.Add(re, re.Name)
            fm.dgv_Report.Columns.Add("clmn_" + re.Name, IIf(re.Reg.IsMatch(MCS), re.Name + "(MCS)", re.Name))

        Next

        Dim i, cellid, occur As Integer
        Dim row As DataGridViewRow

        For i = 0 To dgv_seq.Rows.Count - 1
            If i = CB_SEQ(0).SelectedIndex - 3 Then Continue For ' do not consider the vector
            If Not dgv_seq.Rows(i).Cells(1).Value Then Continue For
            row = fm.dgv_Report.Rows(fm.dgv_Report.Rows.Add)
            row.Cells(0).Value = dgv_seq.Rows(i).Cells(2).Value
            cellid = 0
            For Each re In recol
                cellid += 1
                occur = re.Reg.Matches(dgv_seq.Rows(i).Cells(4).Value).Count
                row.Cells(cellid).Value = occur.ToString
                Select Case occur
                    Case 0
                        row.Cells(cellid).Style.BackColor = Color.Yellow
                    Case 1
                        row.Cells(cellid).Style.BackColor = Color.Tomato
                    Case 2
                        row.Cells(cellid).Style.BackColor = Color.Red
                    Case Else
                        row.Cells(cellid).Style.BackColor = Color.DarkRed
                End Select

            Next
        Next

        fm.Show()
    End Sub

    Private Sub btn_Standard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Standard.Click
        'Selected the standard enzymes
        Dim re, rex As Nuctions.RestrictionEnzyme

        Dim lvi As ListViewItem

        Dim recol As New Collection

        If r Is Nothing Then MsgBox("The restriction enzymes has not been loaded!") : Exit Sub

        For Each lvi In lv_Enz.Items
            re = lvi.Tag
            re.Tag = lvi
            lvi.Checked = False
            recol.Add(re, re.Name)
        Next

        Dim homocol As New Collection
        Dim homo As Collection
        For Each re In recol
            recol.Remove(re.Name) 'delete itself from the collection
            homo = New Collection
            homo.Add(re, re.Name)
            If re.OverhangNT = "" Then Continue For ' we do not need blunt enzymes
            For Each rex In recol
                'find if there are enzymes with same tails
                If re.OverhangNT = rex.OverhangNT Then re.Tag.Checked = True : rex.Tag.checked = True : recol.Remove(rex.Name) : homo.Add(rex, rex.Name) ' remove the homocuters
            Next
            homocol.Add(homo, re.Name)
        Next
        For Each homo In homocol
            If homo.Count > 1 Then
                For Each re In homo
                    rtb_Sequence.AppendText(re.Name + " ")
                Next
                rtb_Sequence.AppendText(ControlChars.NewLine)
            End If
        Next

    End Sub

    Private Sub btn_SelAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SelAll.Click
        Dim lvi As ListViewItem
        For Each lvi In lv_Enz.Items
            lvi.Checked = True
        Next
    End Sub

    Private Sub btn_UnSelAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_UnSelAll.Click
        Dim lvi As ListViewItem
        For Each lvi In lv_Enz.Items
            lvi.Checked = False
        Next
    End Sub
End Class