Imports System.Windows
Public Class UserAgreement
    Private Sub ExitVexcutor(sender As Object, e As RoutedEventArgs)
        WPFContainer.GetWinForm(Me).DialogResult = DialogResult.Cancel
    End Sub
    Private Sub AcceptVexcutor(sender As Object, e As RoutedEventArgs)
        DirectCast(WPFContainer.GetWinForm(Me), frmRegistration).Accept()
    End Sub
End Class
