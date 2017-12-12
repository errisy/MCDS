Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Controls.Primitives, System.Windows.Data
Public MustInherit Class ModelVisual
    Inherits DrawingVisual
    'ModelVisual->ToolTip As Object Default: Nothing
    Public Property ToolTip As Object
        Get
            Return GetValue(ToolTipProperty)
        End Get
        Set(ByVal value As Object)
            SetValue(ToolTipProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ToolTipProperty As DependencyProperty =
                           DependencyProperty.Register("ToolTip",
                           GetType(Object), GetType(ModelVisual),
                           New PropertyMetadata(Nothing))
    'ModelVisual->IsDrawing As Boolean with Event Default: False
    Public Property IsDrawing As Boolean
        Get
            Return GetValue(IsDrawingProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsDrawingProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsDrawingProperty As DependencyProperty =
                           DependencyProperty.Register("IsDrawing",
                           GetType(Boolean), GetType(ModelVisual),
                           New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedIsDrawingChanged)))
    Protected Shared Sub SharedIsDrawingChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, ModelVisual).IsDrawingChanged(d, e)
    End Sub
    Private Sub IsDrawingChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If IsDrawing Then Return
        OnRender()
    End Sub
    'ModelVisual->IsAllocating As Boolean Default: False
    Public Property IsAllocating As Boolean
        Get
            Return GetValue(IsAllocatingProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsAllocatingProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsAllocatingProperty As DependencyProperty =
                           DependencyProperty.Register("IsAllocating",
                           GetType(Boolean), GetType(ModelVisual),
                           New PropertyMetadata(False))
    'ModelVisual->ShouldBubbleMouseEvents As Boolean Default: True
    Public Property ShouldBubbleMouseEvents As Boolean
        Get
            Return GetValue(ShouldBubbleMouseEventsProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(ShouldBubbleMouseEventsProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ShouldBubbleMouseEventsProperty As DependencyProperty =
                           DependencyProperty.Register("ShouldBubbleMouseEvents",
                           GetType(Boolean), GetType(ModelVisual),
                           New PropertyMetadata(True))
    'ModelVisual->RenderOffset As Vector with Event Default: New Vector()
    Public Property RenderOffset As System.Windows.Vector
        Get
            Return GetValue(RenderOffsetProperty)
        End Get
        Set(ByVal value As System.Windows.Vector)
            SetValue(RenderOffsetProperty, value)
        End Set
    End Property
    Public Shared ReadOnly RenderOffsetProperty As DependencyProperty =
                           DependencyProperty.Register("RenderOffset",
                           GetType(System.Windows.Vector), GetType(ModelVisual),
                           New PropertyMetadata(New System.Windows.Vector(), New PropertyChangedCallback(AddressOf SharedRenderOffsetChanged)))
    Private Shared Sub SharedRenderOffsetChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, ModelVisual).RenderOffsetChanged(d, e)
    End Sub
    Private Sub RenderOffsetChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        OnRender()
    End Sub

    ''' <summary>
    ''' Use RenderOpen method here to update the visual. RenderOffsetChanged and IsDrawingChanged should call this method.
    ''' </summary>
    ''' <remarks></remarks>
    Protected MustOverride Sub OnRender()
    Protected Overridable Sub OnUpdateRenderBounds()
        RaiseEvent UpdateRenderBounds(Me, New EventArgs)
    End Sub
    'Public MustOverride ReadOnly Property RenderBounds As Rect
    'Public MustOverride ReadOnly Property AllocationGeometry As Geometry

    Public Event UpdateRenderBounds(sendera As ModelVisual, e As EventArgs)
    Public Event MouseDown(sender As ModelVisual, e As MouseButtonEventArgs)
    Public Event MouseUp(sender As ModelVisual, e As MouseButtonEventArgs)
    Public Event MouseMove(sender As ModelVisual, e As MouseEventArgs)
    Public Event MouseWheel(sender As ModelVisual, e As MouseWheelEventArgs)
    Public Overridable Sub OnMouseDown(e As MouseButtonEventArgs)
        RaiseEvent MouseDown(Me, e)
        If ShouldBubbleMouseEvents Then
            If TypeOf Parent Is PanelVisual Then
                DirectCast(Parent, PanelVisual).OnMouseDown(e)
            End If
        End If
    End Sub
    Public Overridable Sub OnMouseUp(e As MouseButtonEventArgs)
        RaiseEvent MouseUp(Me, e)
        If ShouldBubbleMouseEvents Then
            If TypeOf Parent Is PanelVisual Then
                DirectCast(Parent, PanelVisual).OnMouseUp(e)
            End If
        End If
    End Sub
    Public Overridable Sub OnMouseMove(e As MouseEventArgs)
        RaiseEvent MouseMove(Me, e)
        If ShouldBubbleMouseEvents Then
            If TypeOf Parent Is PanelVisual Then
                DirectCast(Parent, PanelVisual).OnMouseMove(e)
            End If
        End If
    End Sub
    Public Overridable Sub OnMouseWheel(e As MouseWheelEventArgs)
        RaiseEvent MouseWheel(Me, e)
        If ShouldBubbleMouseEvents Then
            If TypeOf Parent Is PanelVisual Then
                DirectCast(Parent, PanelVisual).OnMouseWheel(e)
            End If
        End If
    End Sub
End Class



