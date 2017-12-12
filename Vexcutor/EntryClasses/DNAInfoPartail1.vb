Partial Public Class DNAInfo
    Public Sub CRISPRCut()
        Dim cList As New List(Of Nuctions.Cell)
        Dim dList As New List(Of DNAInfo)
        For Each s As DNAInfo In Source
            Select Case s.MolecularOperation
                Case Nuctions.MolecularOperationEnum.Host, Nuctions.MolecularOperationEnum.Incubation, Nuctions.MolecularOperationEnum.Transformation
                    'cList.AddRange(s.Cells)
                Case Nuctions.MolecularOperationEnum.Enzyme, Nuctions.MolecularOperationEnum.Recombination
                    If Cells.Count > 0 Then
                        'cList.AddRange(s.Cells)
                    Else
                        dList.Add(s)
                    End If
                Case Else
                    dList.Add(s)
            End Select
        Next
        Dim gRNAList As New List(Of Nuctions.GeneFile)
        Dim Substrates As New List(Of Nuctions.GeneFile)

        Dim rmList As New List(Of DNAInfo)
        For Each crisprScr In CRISPRgRNA
            If Not dList.Contains(crisprScr) Then
                rmList.Add(crisprScr)
            Else
                For Each dna In crisprScr.DNAs
                    gRNAList.Add(dna)
                Next
            End If
        Next
        For Each crisprScr In rmList
            CRISPRgRNA.Remove(crisprScr)
        Next
        For Each scr In dList
            If Not CRISPRgRNA.Contains(scr) Then
                For Each dna In scr.DNAs
                    Substrates.Add(dna)
                Next
            End If
        Next

        Dim results = Nuctions.CRISPRDigest(Substrates, gRNAList)
        LoadResultDNAList(results)
        Calculated = True
    End Sub
End Class
