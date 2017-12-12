Public MustInherit Class CanvasItemsControlBase
    Inherits ItemsContainerControl
    'CanvasItemsControlBase->HorizontalScrollBarVisibility As ScrollBarVisibility Default: ScrollBarVisibility.Auto
    Public Property HorizontalScrollBarVisibility As ScrollBarVisibility
        Get
            Return GetValue(HorizontalScrollBarVisibilityProperty)
        End Get
        Set(ByVal value As ScrollBarVisibility)
            SetValue(HorizontalScrollBarVisibilityProperty, value)
        End Set
    End Property
    Public Shared ReadOnly HorizontalScrollBarVisibilityProperty As DependencyProperty = _
                           DependencyProperty.Register("HorizontalScrollBarVisibility", _
                           GetType(ScrollBarVisibility), GetType(CanvasItemsControlBase), _
                           New PropertyMetadata(ScrollBarVisibility.Auto))
    'CanvasItemsControlBase->VerticalScrollBarVisibility As ScrollBarVisibility Default: ScrollBarVisibility.Auto
    Public Property VerticalScrollBarVisibility As ScrollBarVisibility
        Get
            Return GetValue(VerticalScrollBarVisibilityProperty)
        End Get
        Set(ByVal value As ScrollBarVisibility)
            SetValue(VerticalScrollBarVisibilityProperty, value)
        End Set
    End Property
    Public Shared ReadOnly VerticalScrollBarVisibilityProperty As DependencyProperty = _
                           DependencyProperty.Register("VerticalScrollBarVisibility", _
                           GetType(ScrollBarVisibility), GetType(CanvasItemsControlBase), _
                           New PropertyMetadata(ScrollBarVisibility.Auto))


End Class

