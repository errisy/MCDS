Public Class CodonLibraryManager
    Private IndexList As New List(Of String)
    Private vData As Dictionary(Of String, Nuctions.Translation)
    Private SetDefault As SetDefaultCodon
    Private NavigateToSource As frmMain.WebPageDelegate
    Public Sub LoadData(ByVal vCodonDatabase As Dictionary(Of String, Nuctions.Translation), ByVal vDefault As Nuctions.Translation, ByVal vSetDefault As SetDefaultCodon, ByVal vNav As frmMain.WebPageDelegate)
        vData = vCodonDatabase
        For Each vKey As String In vCodonDatabase.Keys
            IndexList.Add(vKey)
        Next
        bpbList.DataSource = IndexList
        For Each c As String In IndexList
            If vDefault.Organism = c Then bpbList.DoubleSelectIndex = IndexList.IndexOf(c)
        Next
        SetDefault = vSetDefault
        If IndexList.Count > 0 Then
            bpbList.Index = 0
        Else
            bpbList.Index = -1
        End If
        NavigateToSource = vNav
    End Sub
    Public Delegate Sub SetDefaultCodon(ByVal Codon As Nuctions.Translation)

    Private Sub bpbList_LineDoubleSelect(ByVal sender As Object, ByVal e As System.EventArgs) Handles bpbList.LineDoubleSelect
        If Not (SetDefault Is Nothing) AndAlso Not (vData Is Nothing) AndAlso Not (IndexList Is Nothing) Then
            SetDefault.Invoke(vData(IndexList(bpbList.DoubleSelectIndex)))
        End If
    End Sub

    Private Sub bpbList_LineSelectByClickOrDataSource(ByVal sender As Object, ByVal e As System.EventArgs) Handles bpbList.LineSelectByClickOrDataSource
        If vData Is Nothing Then Exit Sub
        If bpbList.Index < 0 Then Exit Sub
        Dim vKey As String = IndexList(bpbList.Index)
        Dim tsl As Nuctions.Translation = vData(vKey)
        tbOrganism.Text = tsl.Organism
        Dim c As String
        Dim vCode As Nuctions.Codon
        Dim stb As New System.Text.StringBuilder


        For i3 As Integer = 0 To 3 Step 1
            For i2 As Integer = 0 To 48 Step 16
                For i1 As Integer = 0 To 12 Step 4
                    c = Nuctions.ParseNumberToCodon(i1 + i2 + i3, 3)
                    vCode = tsl.CodeTable(c)
                    stb.Append(String.Format("{0} {1} {2}    ", c, vCode.ShortName, vCode.GetRatio(c).ToString("0.00")))
                Next
                stb.AppendLine()
            Next
            stb.AppendLine()
        Next
        rtbCodonDefine.Text = stb.ToString
    End Sub

    Private Sub llAdd_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llAdd.LinkClicked
        If tbOrganism.Text.Length = 0 Then
            MsgBox("Organism Name should not be empty!")
            Exit Sub
        End If
        Dim vKey As String = tbOrganism.Text
        Dim regCode As New System.Text.RegularExpressions.Regex("([TAGUC][TAGUC][TAGUC])\s+([a-z\*\-]+)\s+([0-9\.]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim mc As System.Text.RegularExpressions.MatchCollection = regCode.Matches(rtbCodonDefine.Text)
        If mc.Count = 64 Then
            Try
                Dim tb As New Nuctions.Translation
                tb.Organism = vKey
                Dim c As Nuctions.Codon
                Dim gc As Nuctions.GeneticCode
                For Each m As System.Text.RegularExpressions.Match In mc
                    c = Nuctions.AnminoAcidParse(m.Groups(2).Value)
                    If Not tb.AnimoTable.ContainsKey(c.ShortName) Then tb.AnimoTable.AddCodon(c)
                    gc = New Nuctions.GeneticCode
                    gc.Name = m.Groups(1).Value.ToUpper.Replace("U", "T")
                    gc.ratio = CSng(m.Groups(3).Value)
                    tb.AnimoTable(c.ShortName).CodeList.Add(gc)
                    tb.CodeTable.Add(gc.Name, tb.AnimoTable(c.ShortName))
                Next
                For Each cd As Nuctions.Codon In tb.AnimoTable.Values
                    cd.CodeList.Sort()
                Next
                vData.Add(vKey, tb)
                IndexList.Add(vKey)
                bpbList.ReDraw()
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        Else
            MsgBox("Error: " + IIf(mc.Count < 64, "Less", "More") + " than 64 Codons are included in the table.")
        End If
    End Sub

    Private Sub llChange_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llChange.LinkClicked
        If bpbList.Index < 0 Then Exit Sub
        Dim vKey As String = IndexList(bpbList.Index)
        Dim tsl As Nuctions.Translation = vData(vKey)
        If tsl.Organism <> tbOrganism.Text Then
            'change the key also
            tsl.Organism = tbOrganism.Text
            vData.Remove(vKey)
            vKey = tsl.Organism
            vData.Add(vKey, tsl)
            IndexList(bpbList.Index) = vKey
            bpbList.ReDraw()
        End If
        'change the codons
        Dim regCode As New System.Text.RegularExpressions.Regex("([TAGUC][TAGUC][TAGUC])\s+([a-z\*\-]+)\s+([0-9\.]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim mc As System.Text.RegularExpressions.MatchCollection = regCode.Matches(rtbCodonDefine.Text)
        If mc.Count = 64 Then
            Try
                Dim tb As New Nuctions.Translation
                tb.Organism = vKey
                Dim c As Nuctions.Codon
                Dim gc As Nuctions.GeneticCode
                For Each m As System.Text.RegularExpressions.Match In mc
                    c = Nuctions.AnminoAcidParse(m.Groups(2).Value)
                    If Not tb.AnimoTable.ContainsKey(c.ShortName) Then tb.AnimoTable.AddCodon(c)
                    gc = New Nuctions.GeneticCode
                    gc.Name = m.Groups(1).Value.ToUpper.Replace("U", "T")
                    gc.ratio = CSng(m.Groups(2).Value)
                    tb.AnimoTable(c.ShortName).CodeList.Add(gc)
                    tb.CodeTable.Add(gc.Name, tb.AnimoTable(c.ShortName))
                Next
                vData(vKey) = tb
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        Else
            MsgBox("Error: " + IIf(mc.Count < 64, "Less", "More") + " than 64 Codons are included in the table.")
        End If
    End Sub

    Private Sub llRemove_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llRemove.LinkClicked
        If bpbList.Index < 0 Then Exit Sub
        Dim vKey As String = IndexList(bpbList.Index)
        vData.Remove(vKey)
        IndexList.Remove(vKey)
        bpbList.ReDraw()
    End Sub

    Private Sub llSource_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llSource.LinkClicked
        Process.Start("http://synthenome.com/help/codonmanage.htm")
    End Sub
End Class
