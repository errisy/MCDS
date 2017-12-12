Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input

Public Class NucleotideObjectModel
    Inherits Errisy.FormatedTextObjectModel

    'NucleotideViewModel->Row As WPFSequenceRow Default: Nothing
    Public Property Row As WPFSequenceGeometryRow

    'NucleotideViewModel->Index As Integer Default: 0
    Public Property Index As Integer

    'NucleotideViewModel->RowIndex As Integer Default: 0
    Public Property RowIndex As Integer

    'NucleotideViewModel->File As Nuctions.GeneFile Default: Nothing
    Public Property File As Nuctions.GeneFile
    Public Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        Fill = Brushes.Blue
    End Sub
End Class

Public Class NucleotideTextModel
    Inherits Errisy.AllocatedColorText
    'NucleotideViewModel->Row As WPFSequenceRow Default: Nothing
    Public Property Row As WPFSequenceGeometryRow

    'NucleotideViewModel->Index As Integer Default: 0
    Public Property Index As Integer

    'NucleotideViewModel->RowIndex As Integer Default: 0
    Public Property RowIndex As Integer

    'NucleotideViewModel->File As Nuctions.GeneFile Default: Nothing
    Public Property File As Nuctions.GeneFile
    Public Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        Color = Brushes.Blue
    End Sub
End Class