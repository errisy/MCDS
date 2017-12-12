Public Class KEGGOrganismCodonModel
    Inherits KEGG.Organism
    Private _Status As OrganismStatusEnum
    Public Property Status As OrganismStatusEnum
        Get
            Return _Status
        End Get
        Set(value As OrganismStatusEnum)
            _Status = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("Status"))
        End Set
    End Property
    Public Sub New()
    End Sub
    Public Sub New(_model As KEGG.Organism)
        Name = _model.Name
        ID = _model.ID
        Code = _model.Code
        Taxonomy = _model.Taxonomy
        Status = IIf(IsCodeAnalyzed(), OrganismStatusEnum.Analyzed, OrganismStatusEnum.NotAnalyzed)
    End Sub
    Private Function IsCodeAnalyzed() As Boolean
        Dim dir As New System.IO.DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory + "\KEGGCodonUsage")
        If Not dir.Exists Then dir.Create()
        Dim filename As String = System.AppDomain.CurrentDomain.BaseDirectory + "\KEGGCodonUsage\" + Code + ".tsl"
        Return IO.File.Exists(filename)
    End Function
End Class
