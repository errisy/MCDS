Imports System.Windows
Public Class TickToggleButtonBase
    Inherits System.Windows.Controls.Primitives.ToggleButton
    Public Sub New()
        'SetBinding(IsReadOnlyProperty, IsReadOnlyBinding)
    End Sub
    'TickToggleButtonBase->IsReadOnly As Boolean Default: False
    Public Property IsReadOnly As Boolean
        Get
            Return GetValue(IsReadOnlyProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsReadOnlyProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsReadOnlyProperty As DependencyProperty = _
                           DependencyProperty.Register("IsReadOnly", _
                           GetType(Boolean), GetType(TickToggleButtonBase), _
                           New PropertyMetadata(False))
    Protected Overrides Sub OnChecked(e As RoutedEventArgs)
        If IsReadOnly Then e.Handled = True
        MyBase.OnChecked(e)
    End Sub
    Protected Overrides Sub OnUnchecked(e As RoutedEventArgs)
        If IsReadOnly Then e.Handled = True
        MyBase.OnUnchecked(e)
    End Sub
End Class
