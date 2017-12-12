''' <summary>
''' 一个可以点击叉来关闭的TabItem
''' </summary>
''' <remarks></remarks>
Public Class ClosableTabItem

    Private Sub TryCloseTab(sender As Object, e As RoutedEventArgs)
        RaiseTabClose()
    End Sub
End Class
