Imports System.Windows.Markup, System.Windows, System.Windows.Data, System.Windows.Media, System.Windows.Controls, System.Windows.Shapes, System.Windows.Documents
Public Class WPFContainer
    Inherits DependencyObject
    Private Sub New()
    End Sub
    Public Shared Function GetWinForm(ByVal element As DependencyObject) As Form
        Return element.GetValue(WinFormProperty)
    End Function
    Public Shared Sub SetWinForm(ByVal element As DependencyObject, ByVal value As Form)
        element.SetValue(WinFormProperty, value)
    End Sub
    Public Shared ReadOnly WinFormProperty As  _
                           DependencyProperty = DependencyProperty.RegisterAttached("WinForm", _
                           GetType(Form), GetType(WPFContainer), _
                           New FrameworkPropertyMetadata(Nothing))
End Class
Public Class InteropHost
    Inherits System.Windows.Forms.Integration.ElementHost
    Private Sub InteropHost_ChildChanged(sender As Object, e As System.Windows.Forms.Integration.ChildChangedEventArgs) Handles Me.ChildChanged
        If TypeOf e.PreviousChild Is UIElement Then
            WPFContainer.SetWinForm(e.PreviousChild, Nothing)
        End If
        If TypeOf Child Is UIElement Then
            Dim f = FindForm()
            If TypeOf f Is Form Then WPFContainer.SetWinForm(Child, f)
        End If
    End Sub
    Protected Overrides Sub OnParentChanged(e As System.EventArgs)
        If TypeOf Child Is UIElement Then
            Dim f = FindForm()
            If TypeOf f Is Form Then WPFContainer.SetWinForm(Child, f)
        End If
        MyBase.OnParentChanged(e)
    End Sub
End Class