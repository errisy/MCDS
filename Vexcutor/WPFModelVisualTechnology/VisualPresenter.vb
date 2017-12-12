Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Controls.Primitives, System.Windows.Data
''' <summary>
''' VisualPresenter can present the Visuals derived from ModelVisuals (TextVisual, ShapeVisual, etc.). You need to add the Visuals for presenting into the ContainerVisual property.
''' </summary>
<System.Windows.Markup.ContentProperty("ContainerVisual")>
Public Class VisualPresenter
    Inherits FrameworkElement
    Private _VisualCollection As New System.Windows.Media.VisualCollection(Me)
    Private _Table As New Hashtable
    Public Sub New()
        SetValue(ContainerVisualProperty, New ContainerVisual)
    End Sub
    Shared Sub New()
        FrameworkElement.HorizontalAlignmentProperty.OverrideMetadata(GetType(VisualPresenter), New FrameworkPropertyMetadata(HorizontalAlignment.Left))
        FrameworkElement.VerticalAlignmentProperty.OverrideMetadata(GetType(VisualPresenter), New FrameworkPropertyMetadata(VerticalAlignment.Top))
    End Sub
    Protected Overrides Function GetVisualChild(index As Integer) As Visual
        Return _VisualCollection(index)
    End Function
    Protected Overrides ReadOnly Property VisualChildrenCount As Integer
        Get
            Return _VisualCollection.Count
        End Get
    End Property
    'VisualPresenter->ContainerDesiredSize As Size with Event Default: New Size()
    Public Property ContainerDesiredSize As Size
        Get
            Return GetValue(ContainerDesiredSizeProperty)
        End Get
        Set(ByVal value As Size)
            SetValue(ContainerDesiredSizeProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ContainerDesiredSizeProperty As DependencyProperty =
                           DependencyProperty.Register("ContainerDesiredSize",
                           GetType(Size), GetType(VisualPresenter),
                           New PropertyMetadata(New Size(), New PropertyChangedCallback(AddressOf SharedContainerDesiredSizeChanged)))
    Private Shared Sub SharedContainerDesiredSizeChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, VisualPresenter).ContainerDesiredSizeChanged(d, e)
    End Sub
    Private Sub ContainerDesiredSizeChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        InvalidateMeasure()
    End Sub

    'VisualPresenter->ContainerVisual As ContainerVisual with Event Default: Nothing
    Public Property ContainerVisual As ContainerVisual
        Get
            Return GetValue(ContainerVisualProperty)
        End Get
        Set(ByVal value As ContainerVisual)
            SetValue(ContainerVisualProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ContainerVisualProperty As DependencyProperty =
                           DependencyProperty.Register("ContainerVisual",
                           GetType(ContainerVisual), GetType(VisualPresenter),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedContainerVisualChanged)))
    Private Shared Sub SharedContainerVisualChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, VisualPresenter).ContainerVisualChanged(d, e)
    End Sub
    Private Sub ContainerVisualChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.NewValue Is Nothing Then
            _VisualCollection.Clear()
        Else
            _VisualCollection.Clear()
            _VisualCollection.Add(e.NewValue)
            SetBinding(ContainerDesiredSizeProperty, New Binding() With {.Path = New PropertyPath(ContainerVisual.DesiredSizeProperty), .Source = e.NewValue})
        End If
    End Sub
    Protected Overrides Function MeasureOverride(availableSize As Size) As Size
        If ContainerVisual Is Nothing Then
            Return New Size()
        Else
            Return ContainerVisual.DesiredSize
        End If
    End Function
    Protected Overrides Function ArrangeOverride(finalSize As Size) As Size
        If ContainerVisual Is Nothing Then
            Me.Width = 0.0#
            Me.Height = 0.0#
            Return New Size()
        Else
            Dim _DesiredSize = ContainerVisual.DesiredSize
            Width = _DesiredSize.Width
            Height = _DesiredSize.Height
            Return _DesiredSize
        End If
    End Function
    Protected Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        Dim _MousePosition = e.GetPosition(e.Source)
        For Each _Visual As ModelVisual In _VisualCollection
            Dim _HitTestResult = _Visual.HitTest(_MousePosition)
            If TypeOf _HitTestResult.VisualHit Is ModelVisual Then
                DirectCast(_HitTestResult.VisualHit, ModelVisual).OnMouseDown(e)
                Exit For
            End If
        Next
        MyBase.OnMouseDown(e)
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseButtonEventArgs)
        Dim _MousePosition = e.GetPosition(e.Source)
        For Each _Visual As ModelVisual In _VisualCollection
            Dim _HitTestResult = _Visual.HitTest(_MousePosition)
            If TypeOf _HitTestResult.VisualHit Is ModelVisual Then
                DirectCast(_HitTestResult.VisualHit, ModelVisual).OnMouseUp(e)
                Exit For
            End If
        Next
        MyBase.OnMouseUp(e)
    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        Dim _MousePosition = e.GetPosition(e.Source)
        For Each _Visual As ModelVisual In _VisualCollection
            Dim _HitTestResult = _Visual.HitTest(_MousePosition)
            If TypeOf _HitTestResult.VisualHit Is ModelVisual Then
                Dim _Model As ModelVisual = _HitTestResult.VisualHit
                _Model.OnMouseMove(e)
                ToolTip = _Model.ToolTip
                Exit For
            End If
        Next
        MyBase.OnMouseMove(e)
    End Sub
    Protected Overrides Sub OnMouseWheel(e As MouseWheelEventArgs)
        Dim _MousePosition = e.GetPosition(e.Source)
        For Each _Visual As ModelVisual In _VisualCollection
            Dim _HitTestResult = _Visual.HitTest(_MousePosition)
            If TypeOf _HitTestResult.VisualHit Is ModelVisual Then
                DirectCast(_HitTestResult.VisualHit, ModelVisual).OnMouseWheel(e)
                Exit For
            End If
        Next
        MyBase.OnMouseWheel(e)
    End Sub



End Class
