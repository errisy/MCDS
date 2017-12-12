Public Class BriefComboBox
    Private Sub UpdateSelectionValue(sender As Object, e As RoutedEventArgs)
        MyBase.TickItem(DirectCast(e.Source, FrameworkElement).DataContext)
    End Sub
End Class
