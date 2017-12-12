Public Class CanvasEditor
    Inherits Panel
    Implements IZoom, ISelectionProvider

#Region "Dependency Properties"

    'CanvasEditor->ViewOffset As Vector with Event Default: New Vector(0#,0#)
    Public Property ViewOffset As Vector
        Get
            Return GetValue(ViewOffsetProperty)
        End Get
        Set(ByVal value As Vector)
            SetValue(ViewOffsetProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ViewOffsetProperty As DependencyProperty = _
                           DependencyProperty.Register("ViewOffset", _
                           GetType(Vector), GetType(CanvasEditor), _
                           New FrameworkPropertyMetadata(New Vector(0.0#, 0.0#), New PropertyChangedCallback(AddressOf SharedViewOffsetChanged)))
    Private Shared Sub SharedViewOffsetChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, CanvasEditor).ViewOffsetChanged(d, e)
    End Sub
    Private Sub ViewOffsetChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If IsWheelZooming Then Return
        Dim vec As Vector = e.NewValue
        Using x = Dispatcher.DisableProcessing
            RenderTransform = New MatrixTransform(New Matrix With {.OffsetX = vec.X, .OffsetY = vec.Y, .M11 = Zoom, .M22 = Zoom})
        End Using
    End Sub
    Public Property Zoom As Double
        Get
            Return GetValue(ZoomProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(ZoomProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ZoomProperty As DependencyProperty = _
                           DependencyProperty.Register("Zoom", _
                           GetType(Double), GetType(CanvasEditor), _
                           New FrameworkPropertyMetadata(1.0#, New PropertyChangedCallback(AddressOf SharedZoomChanged)))
    Private Shared Sub SharedZoomChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, CanvasEditor).ZoomChanged(d, e)
    End Sub
    Private Sub ZoomChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If IsWheelZooming Then Return
        RenderTransform = New MatrixTransform(New Matrix With {.OffsetX = ViewOffset.X, .OffsetY = ViewOffset.Y, .M11 = e.NewValue, .M22 = e.NewValue})
    End Sub
    'CanvasEditor->MinZoom As Double Default: 0.1#
    Public Property MinZoom As Double
        Get
            Return GetValue(MinZoomProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(MinZoomProperty, value)
        End Set
    End Property
    Public Shared ReadOnly MinZoomProperty As DependencyProperty = _
                           DependencyProperty.Register("MinZoom", _
                           GetType(Double), GetType(CanvasEditor), _
                           New PropertyMetadata(0.1#))
    'CanvasEditor->MaxZoom As Double Default: 10.0#
    Public Property MaxZoom As Double
        Get
            Return GetValue(MaxZoomProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(MaxZoomProperty, value)
        End Set
    End Property
    Public Shared ReadOnly MaxZoomProperty As DependencyProperty = _
                           DependencyProperty.Register("MaxZoom", _
                           GetType(Double), GetType(CanvasEditor), _
                           New PropertyMetadata(10.0#))
    'CanvasEditor->MouseWheelZoomRate As Double Default: 1.05#
    Public Property MouseWheelZoomRate As Double
        Get
            Return GetValue(MouseWheelZoomRateProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(MouseWheelZoomRateProperty, value)
        End Set
    End Property
    Public Shared ReadOnly MouseWheelZoomRateProperty As DependencyProperty = _
                           DependencyProperty.Register("MouseWheelZoomRate", _
                           GetType(Double), GetType(CanvasEditor), _
                           New PropertyMetadata(1.05#))


    'CanvasEditor->CanDragOffsetByMouseMiddleButton As Boolean Default: True
    Public Property CanDragOffsetByMouseMiddleButton As Boolean
        Get
            Return GetValue(CanDragOffsetByMouseMiddleButtonProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(CanDragOffsetByMouseMiddleButtonProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CanDragOffsetByMouseMiddleButtonProperty As DependencyProperty = _
                           DependencyProperty.Register("CanDragOffsetByMouseMiddleButton", _
                           GetType(Boolean), GetType(CanvasEditor), _
                           New PropertyMetadata(True))
    'CanvasEditor->CanZoomByMouseWheel As Boolean with Event Default: True
    Public Property CanZoomByMouseWheel As Boolean
        Get
            Return GetValue(CanZoomByMouseWheelProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(CanZoomByMouseWheelProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CanZoomByMouseWheelProperty As DependencyProperty = _
                           DependencyProperty.Register("CanZoomByMouseWheel", _
                           GetType(Boolean), GetType(CanvasEditor), _
                           New PropertyMetadata(True))
    'CanvasEditor->ResetZoomByRightDoubleClick As Boolean Default: True
    Public Property ResetZoomByRightDoubleClick As Boolean
        Get
            Return GetValue(ResetZoomByRightDoubleClickProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(ResetZoomByRightDoubleClickProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ResetZoomByRightDoubleClickProperty As DependencyProperty = _
                           DependencyProperty.Register("ResetZoomByRightDoubleClick", _
                           GetType(Boolean), GetType(CanvasEditor), _
                           New PropertyMetadata(True))

#End Region
#Region "Arrange and Measure"
    Protected Overrides Function ArrangeOverride(finalSize As Size) As Size
        Using x = Dispatcher.DisableProcessing
            For Each view As UIElement In InternalChildren
                view.Arrange(New Rect(CanvasEditor.GetLeft(view), CanvasEditor.GetTop(view), view.DesiredSize.Width, view.DesiredSize.Height))
            Next
        End Using
        Return New Size(finalSize.Width, finalSize.Height)
    End Function
    Protected Overrides Function MeasureOverride(availableSize As Size) As Size
        Dim MaxWidth As Double = 0
        Dim MaxHeight As Double = 0
        Dim InfinitySize As New Size(Double.PositiveInfinity, Double.PositiveInfinity)
        For Each view As UIElement In InternalChildren
            view.Measure(InfinitySize)
            If MaxWidth < CanvasEditor.GetLeft(view) + view.DesiredSize.Width Then MaxWidth = CanvasEditor.GetLeft(view) + view.DesiredSize.Width
            If MaxHeight < CanvasEditor.GetTop(view) + view.DesiredSize.Height Then MaxHeight = CanvasEditor.GetTop(view) + view.DesiredSize.Height
        Next
        Return New Size(Math.Min(availableSize.Width, MaxWidth), Math.Min(availableSize.Height, MaxHeight))
    End Function
#End Region

#Region "Move and Zoom"
    Private IsDraggingOffset As Boolean = False
    Private BeginMouse As Point
    Private BeginOffset As Vector
    Protected Overrides Sub OnVisualParentChanged(oldParent As DependencyObject)
        If MouseWheelProvider IsNot Nothing Then
            RemoveHandler MouseWheelProvider.MouseWheel, AddressOf ProviderPreviewMouseWheel
            RemoveHandler MouseWheelProvider.MouseDown, AddressOf ProviderPreviewMouseDown
            RemoveHandler MouseWheelProvider.MouseMove, AddressOf ProviderPreviewMouseMove
            RemoveHandler MouseWheelProvider.MouseUp, AddressOf ProviderPreviewMouseUp
            RemoveHandler MouseWheelProvider.MouseDoubleDown, AddressOf ProviderPreviewMouseDoubleDown
        End If
        MouseWheelProvider = Ancestor(Of IMouseInputProvider)()
        If MouseWheelProvider IsNot Nothing Then
            AddHandler MouseWheelProvider.MouseWheel, AddressOf ProviderPreviewMouseWheel
            AddHandler MouseWheelProvider.MouseDown, AddressOf ProviderPreviewMouseDown
            AddHandler MouseWheelProvider.MouseMove, AddressOf ProviderPreviewMouseMove
            AddHandler MouseWheelProvider.MouseUp, AddressOf ProviderPreviewMouseUp
            AddHandler MouseWheelProvider.MouseDoubleDown, AddressOf ProviderPreviewMouseDoubleDown
        End If
        MyBase.OnVisualParentChanged(oldParent)
    End Sub
    Protected Sub ProviderPreviewMouseDown(sender As Object, e As MouseButtonEventArgs)
        If CanDragOffsetByMouseMiddleButton AndAlso e.ChangedButton = MouseButton.Middle Then
            BeginOffset = ViewOffset
            BeginMouse = PointToScreen(e.GetPosition(Me))
            CaptureMouse()
            IsDraggingOffset = True
        End If
    End Sub
    Protected Sub ProviderPreviewMouseDoubleDown(sender As Object, e As MouseButtonEventArgs)
        If ResetZoomByRightDoubleClick AndAlso e.ChangedButton = MouseButton.Right Then
            IsWheelZooming = True
            RenderTransform = New MatrixTransform
            ViewOffset = New Vector(0.0#, 0.0#)
            Zoom = 1.0#
            IsWheelZooming = False
        End If
    End Sub
    Protected Sub ProviderPreviewMouseMove(sender As Object, e As MouseEventArgs)
        If CanDragOffsetByMouseMiddleButton AndAlso IsDraggingOffset AndAlso e.MiddleButton = MouseButtonState.Pressed Then
            ApplyViewOffset(BeginOffset, PointToScreen(e.GetPosition(Me)) - BeginMouse)
        End If
    End Sub
    Protected Sub ProviderPreviewMouseUp(sender As Object, e As MouseButtonEventArgs)
        If CanDragOffsetByMouseMiddleButton AndAlso IsDraggingOffset Then
            ApplyViewOffset(BeginOffset, PointToScreen(e.GetPosition(Me)) - BeginMouse)
            ReleaseMouseCapture()
            IsDraggingOffset = False
        End If
    End Sub
    Private MouseWheelProvider As IMouseInputProvider

    Private IsWheelZooming As Boolean = False
    Private Sub ProviderPreviewMouseWheel(sender As Object, e As MouseWheelEventArgs)
        If CanZoomByMouseWheel AndAlso Keyboard.Modifiers = ModifierKeys.Control Then
            IsWheelZooming = True
            Dim pos As Point = e.GetPosition(Me.Ancestor(Of IMouseInputProvider))
            Dim oZoom = Zoom
            Dim nZoom = oZoom * MouseWheelZoomRate ^ (e.Delta / 120)
            ZoomAt(pos, oZoom, nZoom)
            IsWheelZooming = False
        End If
    End Sub
    Private Sub ZoomAt(MousePos As Point, oZoom As Double, nZoom As Double)
        '(X, Y) -> Offset0 + (X, Y) * oZoom
        '(X, Y) -> Offset1 + (X, Y) * nZoom
        'oOffset + (X, Y) * (oZoom - nZoom) = nOffset 
        '(Pos - oOffset)/oZoom = (X, Y)
        Using x = Dispatcher.DisableProcessing
            Dim Locus = MousePos - ViewOffset
            Dim nMatrix = New Matrix With {.M11 = nZoom, .M22 = nZoom, .OffsetX = ViewOffset.X + Locus.X * (1 - nZoom / oZoom), .OffsetY = ViewOffset.Y + Locus.Y * (1 - nZoom / oZoom)}
            Zoom = nZoom
            ViewOffset = New Vector(nMatrix.OffsetX, nMatrix.OffsetY)
            RenderTransform = New MatrixTransform With {.Matrix = nMatrix}
        End Using
    End Sub
    Private Sub ApplyViewOffset(oOffset As Vector, dOffset As Vector)
        'dOffset = CalibrateMouseOffset(dOffset)
        ViewOffset = oOffset + dOffset
    End Sub
#End Region

#Region "Attached Properties"
    Public Shared Function GetLeft(ByVal element As DependencyObject) As Double
        If element Is Nothing Then
            Throw New ArgumentNullException("element")
        End If
        Return element.GetValue(LeftProperty)
    End Function
    Public Shared Sub SetLeft(ByVal element As DependencyObject, ByVal value As Double)
        If element Is Nothing Then
            Throw New ArgumentNullException("element")
        End If
        element.SetValue(LeftProperty, value)
    End Sub
    Public Shared ReadOnly LeftProperty As  _
                           DependencyProperty = DependencyProperty.RegisterAttached("Left", _
                           GetType(Double), GetType(CanvasEditor), _
                           New FrameworkPropertyMetadata(0.0#, FrameworkPropertyMetadataOptions.AffectsParentMeasure Or FrameworkPropertyMetadataOptions.AffectsParentArrange))
    Public Shared Function GetTop(ByVal element As DependencyObject) As Double
        If element Is Nothing Then
            Throw New ArgumentNullException("element")
        End If
        Return element.GetValue(TopProperty)
    End Function
    Public Shared Sub SetTop(ByVal element As DependencyObject, ByVal value As Double)
        If element Is Nothing Then
            Throw New ArgumentNullException("element")
        End If
        element.SetValue(TopProperty, value)
    End Sub
    Public Shared ReadOnly TopProperty As  _
                           DependencyProperty = DependencyProperty.RegisterAttached("Top", _
                           GetType(Double), GetType(CanvasEditor), _
                           New FrameworkPropertyMetadata(0.0#, FrameworkPropertyMetadataOptions.AffectsParentMeasure Or FrameworkPropertyMetadataOptions.AffectsParentArrange))
#End Region


#Region "IZoom"
    Public Function CalibrateMouseOffset(Offset As Vector) As Vector Implements IZoom.CalibrateMouseOffset
        Return New Vector(Offset.X / Zoom, Offset.Y / Zoom)
    End Function
    Public Function TranslateToCoordinate(Position As Point) As Point Implements IZoom.TranslateToCoordinate
        Dim vo = ViewOffset
        Dim z = Zoom
        Return New Point((Position.X + vo.X) / z, (Position.Y + vo.Y) / z)
    End Function
#End Region

#Region "ISelectionProvider"
    'CanvasEditor->SelectedView As UIElement with Event Default: Nothing
    Public Property SelectedView As UIElement Implements ISelectionProvider.SelectedView
        Get
            Return GetValue(SelectedViewProperty)
        End Get
        Set(ByVal value As UIElement)
            SetValue(SelectedViewProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SelectedViewProperty As DependencyProperty = _
                           DependencyProperty.Register("SelectedView", _
                           GetType(UIElement), GetType(CanvasEditor), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedSelectedViewChanged)))
    Private Shared Sub SharedSelectedViewChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, CanvasEditor).SelectedViewChanged(d, e)
    End Sub
    Private Sub SelectedViewChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Using x = Dispatcher.DisableProcessing
            If TypeOf e.OldValue Is UIElement Then CanvasEditor.SetZIndex(e.OldValue, 0)
            If TypeOf e.OldValue Is FrameworkElement AndAlso TypeOf DirectCast(e.OldValue, FrameworkElement).DataContext Is ISelectionListener Then _
                DirectCast(DirectCast(e.OldValue, FrameworkElement).DataContext, ISelectionListener).IsSelected = False
            If TypeOf e.NewValue Is UIElement Then CanvasEditor.SetZIndex(e.NewValue, 1)
            If TypeOf e.NewValue Is FrameworkElement AndAlso TypeOf DirectCast(e.NewValue, FrameworkElement).DataContext Is ISelectionListener Then _
                DirectCast(DirectCast(e.NewValue, FrameworkElement).DataContext, ISelectionListener).IsSelected = True
        End Using
    End Sub
#End Region


End Class
