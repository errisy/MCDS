Public Class ORFSearchDialog

    Public ReadOnly Property HasContext As Boolean
        Get
            Return (TypeOf DataContext Is ORFSearchOptions)
        End Get
    End Property
    Public Property Form As Form
    Private Sub Cancel(sender As Object, e As Windows.RoutedEventArgs)
        Form.DialogResult = DialogResult.Cancel
    End Sub
    Private Sub Accept(sender As Object, e As Windows.RoutedEventArgs)
        Form.DialogResult = DialogResult.OK
    End Sub
End Class
