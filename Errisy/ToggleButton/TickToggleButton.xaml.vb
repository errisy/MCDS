Public Class TickToggleButton
    Private Sub TickToggleButton_Checked(sender As Object, e As RoutedEventArgs) Handles Me.Checked
        Dim dc = DataContext
        Stop
    End Sub

    Private Sub TickToggleButton_DataContextChanged(sender As Object, e As DependencyPropertyChangedEventArgs) Handles Me.DataContextChanged
        Dim dc = DataContext
        Stop
    End Sub

    Private Sub TickToggleButton_Unchecked(sender As Object, e As RoutedEventArgs) Handles Me.Unchecked
        Dim dc = DataContext
        Stop
    End Sub
End Class
