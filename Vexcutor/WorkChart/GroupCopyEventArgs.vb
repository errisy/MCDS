Public Class GroupCopyEventArgs
    Public Groups As New List(Of DNAInfo)
    Public Sub New(ByVal vList As List(Of ChartItem))
        Dim DNADict As New Dictionary(Of DNAInfo, DNAInfo)

        Dim cl As DNAInfo
        For Each ci As ChartItem In vList
            cl = ci.MolecularInfo.Clone
            cl.Source.Clear()
            cl.DNAs = New Collection
            For Each gf As Nuctions.GeneFile In ci.MolecularInfo.DNAs
                cl.DNAs.Add(gf.CloneWithoutFeatures)
            Next
            DNADict.Add(ci.MolecularInfo, cl)
        Next
        For Each di As DNAInfo In DNADict.Keys
            For Each si As DNAInfo In di.Source
                If DNADict.ContainsKey(si) Then
                    DNADict(di).Source.Add(DNADict(si))
                End If
            Next
        Next
        For Each di As DNAInfo In DNADict.Values
            Groups.Add(di)
        Next
    End Sub
End Class
