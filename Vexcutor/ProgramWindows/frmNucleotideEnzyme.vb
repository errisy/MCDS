Public Class frmNucleotideEnzyme
    Private t As Nuctions.Translation
    Private r As Nuctions.RestrictionEnzymes

    Private Sub btnLoadCodon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadCodon.Click
        If ofdOpenCodon.ShowDialog() = Windows.Forms.DialogResult.OK Then
            t = Nuctions.LoadCodon(ofdOpenCodon.FileName)
        End If
    End Sub

    Private Sub btnLoadRE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadRE.Click
        If ofdRE.ShowDialog() = Windows.Forms.DialogResult.OK Then
            r = Nuctions.LoadRestrictionEnzymes(ofdRE.FileName)
        End If
    End Sub

    Private Sub btCalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btCalculate.Click
        Dim input As String = tbAnimo.Text
        Dim i As Integer

        For i = 0 To input.Length - 1
            If Not t.AnimoTable.ContainsKey(input.Chars(i)) Then rtbOut.AppendText("Error in input: illegal character!") : Exit Sub
        Next

        Dim substr, gencode As String
        Dim c1, c2, c3 As Nuctions.GeneticCode
        Dim re As Nuctions.RestrictionEnzyme
        rtbOut.AppendText("Running..." + ControlChars.NewLine)

        For i = 0 To input.Length - 4

            substr = input.Substring(i, 3)
            gencode = ""
            For Each c1 In t.AnimoTable(substr.Chars(0)).CodeList
                For Each c2 In t.AnimoTable(substr.Chars(1)).CodeList
                    For Each c3 In t.AnimoTable(substr.Chars(2)).CodeList
                        gencode = c1.Name & c2.Name & c3.Name
                        For Each re In r.RECollection
                            If re.Reg.IsMatch(gencode) Then rtbOut.AppendText(substr + "-->" + gencode + " by " + re.Name + ControlChars.NewLine)
                        Next
                    Next
                Next
            Next
        Next
        rtbOut.AppendText("End " + ControlChars.NewLine)

    End Sub
End Class