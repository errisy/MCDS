Public Class ClosableItemPresenterBase
    Inherits ContentControl
    Public Property RemoveCommand As ICommand
        Get
            Return GetValue(RemoveCommandProperty)
        End Get
        Set(ByVal value As ICommand)
            SetValue(RemoveCommandProperty, value)
        End Set
    End Property
    Public Shared ReadOnly RemoveCommandProperty As DependencyProperty =
                             DependencyProperty.Register("RemoveCommand",
                             GetType(ICommand), GetType(ClosableItemPresenterBase),
                             New PropertyMetadata(Nothing))
End Class
