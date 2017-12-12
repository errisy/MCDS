Public Class VisualPage
    'Private _ContentHost As Grid
    'Public Overrides Sub OnApplyTemplate()
    '    _ContentHost = Template.FindName("ContentHost", Me)
    '    SetValue(ContentHostPropertyKey, _ContentHost)
    '    MyBase.OnApplyTemplate()
    'End Sub
    ''VisualPage -> ContentHost As Grid Default: Nothing
    'Public ReadOnly Property ContentHost As Grid
    '    Get
    '        Return GetValue(VisualPage.ContentHostProperty)
    '    End Get
    'End Property
    'Private Shared ReadOnly ContentHostPropertyKey As DependencyPropertyKey = _
    '                          DependencyProperty.RegisterReadOnly("ContentHost", _
    '                          GetType(Grid), GetType(VisualPage), _
    '                          New PropertyMetadata(Nothing))
    'Public Shared ReadOnly ContentHostProperty As DependencyProperty = _
    '                         ContentHostPropertyKey.DependencyProperty

End Class
