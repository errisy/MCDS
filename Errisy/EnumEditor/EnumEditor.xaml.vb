Public Class EnumEditor

    Private Sub UpdateEnumValue(sender As Object, e As RoutedEventArgs)
        MyBase.TickItem(DirectCast(e.Source, FrameworkElement).DataContext)
    End Sub
End Class
