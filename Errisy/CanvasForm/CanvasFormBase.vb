Public MustInherit Class CanvasFormBase
    Inherits ContentControl

    Private Sub OnBorderThicknessChanged(sender As Object, e As DependencyPropertyChangedEventArgs)
        Dim b As Thickness = BorderThickness
        SetValue(TopBorderSizePropertyKey, New GridLength(b.Top))
        SetValue(BottomBorderSizePropertyKey, New GridLength(b.Bottom))
        SetValue(LeftBorderSizePropertyKey, New GridLength(b.Left))
        SetValue(RightBorderSizePropertyKey, New GridLength(b.Right))
    End Sub
#Region "Dependency Properties"
    'CanvasFormBase -> TopBorderSize As GridLength Default: New GridLength(0.0#)
    Public ReadOnly Property TopBorderSize As GridLength
        Get
            Return GetValue(CanvasFormBase.TopBorderSizeProperty)
        End Get
    End Property
    Private Shared ReadOnly TopBorderSizePropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("TopBorderSize", _
                              GetType(GridLength), GetType(CanvasFormBase), _
                              New PropertyMetadata(New GridLength(4.0#)))
    Public Shared ReadOnly TopBorderSizeProperty As DependencyProperty = _
                             TopBorderSizePropertyKey.DependencyProperty
    'CanvasFormBase -> BottomBorderSize As GridLength Default: New GridLength(0.0#)
    Public ReadOnly Property BottomBorderSize As GridLength
        Get
            Return GetValue(CanvasFormBase.BottomBorderSizeProperty)
        End Get
    End Property
    Private Shared ReadOnly BottomBorderSizePropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("BottomBorderSize", _
                              GetType(GridLength), GetType(CanvasFormBase), _
                              New PropertyMetadata(New GridLength(4.0#)))
    Public Shared ReadOnly BottomBorderSizeProperty As DependencyProperty = _
                             BottomBorderSizePropertyKey.DependencyProperty
    'CanvasFormBase -> LeftBorderSize As GridLength Default: New GridLength(0#)
    Public ReadOnly Property LeftBorderSize As GridLength
        Get
            Return GetValue(CanvasFormBase.LeftBorderSizeProperty)
        End Get
    End Property
    Private Shared ReadOnly LeftBorderSizePropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("LeftBorderSize", _
                              GetType(GridLength), GetType(CanvasFormBase), _
                              New PropertyMetadata(New GridLength(4.0#)))
    Public Shared ReadOnly LeftBorderSizeProperty As DependencyProperty = _
                             LeftBorderSizePropertyKey.DependencyProperty
    'CanvasFormBase -> RightBorderSize As GridLength Default: New GridLength(0#)
    Public ReadOnly Property RightBorderSize As GridLength
        Get
            Return GetValue(CanvasFormBase.RightBorderSizeProperty)
        End Get
    End Property
    Private Shared ReadOnly RightBorderSizePropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("RightBorderSize", _
                              GetType(GridLength), GetType(CanvasFormBase), _
                              New PropertyMetadata(New GridLength(4.0#)))
    Public Shared ReadOnly RightBorderSizeProperty As DependencyProperty = _
                             RightBorderSizePropertyKey.DependencyProperty
    'CanvasFormBase->Header As Object with Event Default: Nothing
    Public Property Header As Object
        Get
            Return GetValue(HeaderProperty)
        End Get
        Set(ByVal value As Object)
            SetValue(HeaderProperty, value)
        End Set
    End Property
    Public Shared ReadOnly HeaderProperty As DependencyProperty = _
                           DependencyProperty.Register("Header", _
                           GetType(Object), GetType(CanvasFormBase), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedHeaderChanged)))
    Private Shared Sub SharedHeaderChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, CanvasFormBase).HeaderChanged(d, e)
    End Sub
    Private Sub HeaderChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.OldValue IsNot Nothing Then RemoveLogicalChild(e.OldValue)
        If TypeOf e.OldValue Is FrameworkElement Then BindingOperations.ClearBinding(DirectCast(e.OldValue, FrameworkElement), FrameworkElement.DataContextProperty)
        If TypeOf e.NewValue Is FrameworkElement Then DirectCast(e.NewValue, FrameworkElement).SetBinding(FrameworkElement.DataContextProperty, New Binding With {.Source = Me, .Path = New PropertyPath(FrameworkElement.DataContextProperty)})
        If e.NewValue IsNot Nothing Then AddLogicalChild(e.NewValue)
    End Sub

    'CanvasFormBase->HeaderHeight As Double with Event Default: 24#
    Public Property HeaderHeight As Double
        Get
            Return GetValue(HeaderHeightProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(HeaderHeightProperty, value)
        End Set
    End Property
    Public Shared ReadOnly HeaderHeightProperty As DependencyProperty = _
                           DependencyProperty.Register("HeaderHeight", _
                           GetType(Double), GetType(CanvasFormBase), _
                           New PropertyMetadata(24.0#, New PropertyChangedCallback(AddressOf SharedHeaderHeightChanged)))
    Private Shared Sub SharedHeaderHeightChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, CanvasFormBase).HeaderHeightChanged(d, e)
    End Sub
    Private Sub HeaderHeightChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        SetValue(HeaderSizePropertyKey, New GridLength(e.NewValue))
    End Sub
    'CanvasFormBase -> HeaderSize As GridLength Default: New GridLength(24#)
    Public ReadOnly Property HeaderSize As GridLength
        Get
            Return GetValue(CanvasFormBase.HeaderSizeProperty)
        End Get
    End Property
    Private Shared ReadOnly HeaderSizePropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("HeaderSize", _
                              GetType(GridLength), GetType(CanvasFormBase), _
                              New PropertyMetadata(New GridLength(24.0#)))
    Public Shared ReadOnly HeaderSizeProperty As DependencyProperty = _
                             HeaderSizePropertyKey.DependencyProperty
    'CanvasFormBase->HeaderBackground As Brush Default: Brushes.LightYellow
    Public Property HeaderBackground As Brush
        Get
            Return GetValue(HeaderBackgroundProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(HeaderBackgroundProperty, value)
        End Set
    End Property
    Public Shared ReadOnly HeaderBackgroundProperty As DependencyProperty = _
                           DependencyProperty.Register("HeaderBackground", _
                           GetType(Brush), GetType(CanvasFormBase), _
                           New PropertyMetadata(Brushes.LightYellow))

    'CanvasFormBase->HasCloseButton As Boolean Default: True
    Public Property HasCloseButton As Boolean
        Get
            Return GetValue(HasCloseButtonProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(HasCloseButtonProperty, value)
        End Set
    End Property
    Public Shared ReadOnly HasCloseButtonProperty As DependencyProperty = _
                           DependencyProperty.Register("HasCloseButton", _
                           GetType(Boolean), GetType(CanvasFormBase), _
                           New PropertyMetadata(True))
#End Region
#Region "Routed Events"


    Protected Sub RaiseFormClose()
        Dim e As RoutedEventArgs
        e = New RoutedEventArgs(FormCloseEvent, Me)
        OnFormClose(e)
        Me.RaiseEvent(e)
    End Sub
    Protected Overridable Sub OnFormClose(e As RoutedEventArgs)
    End Sub

    Public Custom Event FormClose As RoutedEventHandler
        AddHandler(ByVal value As RoutedEventHandler)
            Me.AddHandler(FormCloseEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As RoutedEventHandler)
            Me.RemoveHandler(FormCloseEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event
    Public Shared ReadOnly FormCloseEvent As RoutedEvent = _
                      EventManager.RegisterRoutedEvent("FormClose", _
                      RoutingStrategy.Bubble, _
                      GetType(RoutedEventHandler), GetType(CanvasFormBase))
#End Region


    Shared Sub New()
        ContentControl.BorderThicknessProperty.OverrideMetadata(GetType(CanvasFormBase), New FrameworkPropertyMetadata(New Thickness(4.0#), New PropertyChangedCallback(AddressOf SharedBorderThicknessChanged)))
        ContentControl.BorderBrushProperty.OverrideMetadata(GetType(CanvasFormBase), New FrameworkPropertyMetadata(Brushes.LightBlue))
        ContentControl.VerticalContentAlignmentProperty.OverrideMetadata(GetType(CanvasFormBase), New FrameworkPropertyMetadata(VerticalAlignment.Center))
        ContentControl.WidthProperty.OverrideMetadata(GetType(CanvasFormBase), New FrameworkPropertyMetadata(300.0#))
        ContentControl.HeightProperty.OverrideMetadata(GetType(CanvasFormBase), New FrameworkPropertyMetadata(400.0#))

    End Sub
    Private Shared Sub SharedBorderThicknessChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, CanvasFormBase).OnBorderThicknessChanged(d, e)
    End Sub
End Class

Friend Enum ResizeModeEnum
    None
    Move
    N
    S
    W
    E
    NW
    NE
    SW
    SE
End Enum
 