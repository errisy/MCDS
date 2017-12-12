Imports Errisy
Imports System.Windows, System.Windows.Media, System.Windows.Input, System.Windows.Data
Public MustInherit Class GeneSequenceObjectModel
    Inherits GeometryObjectModel
    'GeneSequenceViewModel -> Location As Point Default: New Point()
    Public Property Location As Point

    'GeneSequenceViewModel -> File As Nuctions.GeneFile Default: Nothing
    Public Property File As Nuctions.GeneFile

    'GeneSequenceViewModel -> RegionStart As Integer Default: 0
    Public Property RegionStart As Integer

    'GeneSequenceViewModel -> RegionEnd As Integer Default: 0
    Public Property RegionEnd As Integer


End Class


 