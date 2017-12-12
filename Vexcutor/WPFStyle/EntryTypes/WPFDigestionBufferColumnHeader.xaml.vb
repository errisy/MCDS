Public Class WPFDigestionBufferColumnHeader

    Public Property Text As String
        Get
            Return rText.Text
        End Get
        Set(value As String)
            rText.Text = value
        End Set
    End Property
    Public Property RemoveBuffer As Action(Of String)
    Public Sub Remove(sender As Object, e As System.Windows.RoutedEventArgs)
        If RemoveBuffer Is Nothing Then Return
        RemoveBuffer.Invoke(rText.Text)
    End Sub
End Class
