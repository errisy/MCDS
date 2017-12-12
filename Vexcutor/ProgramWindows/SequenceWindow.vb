Public Class SequenceWindow

    Public Property GeneFile() As Nuctions.GeneFile
        Get
            Return svGene.GeneFile
        End Get
        Set(ByVal value As Nuctions.GeneFile)
            svGene.GeneFile = value
        End Set
    End Property

    Public Property RestrictionSites() As List(Of String)
        Get
            Return svGene.RestrictionSites
        End Get
        Set(ByVal value As List(Of String))
            svGene.RestrictionSites = value
        End Set
    End Property

End Class