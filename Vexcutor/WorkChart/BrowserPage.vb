Public Class BrowserPage

    Public Sub LoadPage(ByVal vURL As String)
        wbMain.Navigate(vURL)
    End Sub

    Private Sub wbMain_Navigated(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles wbMain.Navigated
        If TypeOf Me.Parent Is CustomTabPage Then
            Dim cp As CustomTabPage = Me.Parent
            cp.Text = wbMain.DocumentTitle
        End If
    End Sub
End Class
