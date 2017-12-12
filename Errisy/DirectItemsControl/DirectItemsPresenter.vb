Public Class DirectItemsPresenter
    Inherits ContentPresenter
    'Private _Children As New VisualCollection(Me)
    'Protected Overrides ReadOnly Property VisualChildrenCount() As Integer
    '    Get
    '        Return _Children.Count
    '    End Get
    'End Property
    'Protected Overrides Function GetVisualChild(ByVal index As Integer) As Visual
    '    If index < 0 OrElse index >= _Children.Count Then
    '        Throw New ArgumentOutOfRangeException()
    '    End If
    '    Return _Children(index)
    'End Function
    ''DirectItemsPresenter->Content As Panel with Event Default: Nothing
    'Protected Friend Property Content As Panel
    '    Get
    '        Return GetValue(ContentProperty)
    '    End Get
    '    Set(ByVal value As Panel)
    '        SetValue(ContentProperty, value)
    '    End Set
    'End Property
    'Protected Friend Shared ReadOnly ContentProperty As DependencyProperty = _
    '                       DependencyProperty.Register("Content", _
    '                       GetType(Panel), GetType(DirectItemsPresenter), _
    '                       New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedContentChanged)))
    'Private Shared Sub SharedContentChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    '    DirectCast(d, DirectItemsPresenter).ContentChanged(d, e)
    'End Sub
    'Private Sub ContentChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    '    _Children.Clear()
    '    _Children.Add(e.NewValue)
    'End Sub


End Class
