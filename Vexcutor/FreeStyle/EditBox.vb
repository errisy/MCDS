Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes
Public Class EditBox
    Inherits TextBox
    Public Sub New()
        With Me
            .AcceptsReturn = True
            .AcceptsTab = True
            .MaxLines = 1
            .Background = Brushes.Transparent
            '.FontSize = 14D
        End With
    End Sub
    Public Property SenseTypes As New List(Of Object)
    Protected Overrides Sub OnKeyDown(e As System.Windows.Input.KeyEventArgs)
        Select Case e.Key
            Case Key.Escape
                'cancel
                Unfocus()
        End Select
        MyBase.OnKeyDown(e)
    End Sub
    Private MiddleYellow As New SolidColorBrush(Color.FromArgb(128, 255, 255, 0))
    Public Sub ActivateColor()
        Background = MiddleYellow
    End Sub
    Public Sub DeactivateColor()
        Background = Brushes.White
    End Sub
    Protected Overrides Sub OnGotFocus(e As System.Windows.RoutedEventArgs)
        ActivateColor()
        MyBase.OnGotFocus(e)
    End Sub
    Protected Overrides Sub OnLostFocus(e As System.Windows.RoutedEventArgs)
        DeactivateColor()
        MyBase.OnLostFocus(e)
    End Sub
    Public Sub Unfocus()
        MoveFocus(New TraversalRequest(FocusNavigationDirection.Next))
    End Sub
End Class