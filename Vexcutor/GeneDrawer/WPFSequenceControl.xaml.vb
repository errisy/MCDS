Public Class WPFSequenceControl

    Private Sub LoadContextRow(sender As Object, e As Windows.RoutedEventArgs)
        Dim WPFSequence As WPFSequenceView = DataContext
        Dim _VisualControl As Errisy.VisualObjectControl = sender
        Dim Row As WPFSequenceObjectRow = _VisualControl.DataContext
        'Row.Setup(Row.StartNucleotideIndex , )
        If Not Row.IsLoaded Then
            Row.LoadContent(WPFSequence.Indent, WPFSequence.GroupSpacing, WPFSequence.FontSize, WPFSequence.NucleotidesPerRow, WPFSequence.NucleotidesPerGroup, WPFSequence.EnzymeCuts, WPFSequence.GeneFile.Features, WPFSequence.GeneFile)
            _VisualControl.Height = Row.DesiredSize.Height
        End If
    End Sub
End Class
