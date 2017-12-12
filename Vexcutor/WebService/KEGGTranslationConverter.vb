Public Class KEGGTranslationConverter
    Public Shared Function Convert(kt As KEGG.Translation) As Nuctions.Translation
        Dim nt = New Nuctions.Translation
        nt.Organism = kt.Organism


    End Function
End Class
