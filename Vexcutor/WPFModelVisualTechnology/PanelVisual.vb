Imports System.Collections.ObjectModel, System.Windows.Data, System.Windows.Input, System.Windows.Media, System.Windows.Controls, System.Windows
''' <summary>
''' A Panel moves all its siblings together. You need to inhirit PanelVisual to customize your own composite object.
''' Please overrides OnRender. In OnRender, you need to clear all privious objects in _Visuals when necessary, and create new Visuals and add them to _Visuals.
''' To present the PanelVisual, you need to add it into the ContainerVisual property of VisualPresenter. VisualPresenter is the real FrameworkElement that presents all the DrawingVisuals and provides mouse events.
''' </summary>
<System.Windows.Markup.ContentProperty("Visuals")>
Public Class PanelVisual
    Inherits ModelVisual
    Protected _Visuals As New ModelVisualCollection
    Public Sub New()
        SetValue(VisualsPropertyKey, _Visuals)
        AddHandler _Visuals.CollectionChanged, AddressOf VisualsChanged
    End Sub
    Private Sub VisualsChanged(sender As Object, e As System.Collections.Specialized.NotifyCollectionChangedEventArgs)
        If e.OldItems IsNot Nothing Then
            For Each _Visual As Visual In e.OldItems
                If Children.Contains(_Visual) Then
                    RemoveHandler DirectCast(_Visual, ModelVisual).UpdateRenderBounds, AddressOf VisualUpdateRenderBounds
                    Children.Remove(_Visual)
                End If
            Next
        End If
        Dim _Location As Point = GetValue(LocationProperty)
        Dim _RenderOffset As System.Windows.Vector = GetValue(RenderOffsetProperty)
        Dim _Offset = New System.Windows.Vector(_Location.X + _RenderOffset.X, _Location.Y + _RenderOffset.Y)
        If e.NewItems IsNot Nothing Then
            For Each _Visual As Visual In e.NewItems
                If Not Children.Contains(_Visual) Then
                    Dim _ModelVisual As ModelVisual = _Visual
                    _ModelVisual.RenderOffset = _Offset
                    Children.Add(_Visual)
                    '_RenderBounds.Union(_ModelVisual.ContentBounds)
                    AddHandler DirectCast(_Visual, ModelVisual).UpdateRenderBounds, AddressOf VisualUpdateRenderBounds
                End If
            Next
        End If
        UpdateBounds()
    End Sub
    Private Sub VisualUpdateRenderBounds(sender As ModelVisual, e As EventArgs)
        UpdateBounds()
    End Sub
    Private Sub UpdateBounds()
        OnUpdateRenderBounds()
    End Sub
    'PanelVisual -> Visuals As ModelVisualCollection Default: Nothing
    Public ReadOnly Property Visuals As ModelVisualCollection
        Get
            Return GetValue(ContainerVisual.VisualsProperty)
        End Get
    End Property
    Private Shared ReadOnly VisualsPropertyKey As DependencyPropertyKey =
                              DependencyProperty.RegisterReadOnly("Visuals",
                              GetType(ModelVisualCollection), GetType(PanelVisual),
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly VisualsProperty As DependencyProperty =
                             VisualsPropertyKey.DependencyProperty

    'PanelVisual->Location As Point with Event Default: New Point()
    Public Property Location As Point
        Get
            Return GetValue(LocationProperty)
        End Get
        Set(ByVal value As Point)
            SetValue(LocationProperty, value)
        End Set
    End Property
    Public Shared ReadOnly LocationProperty As DependencyProperty =
                           DependencyProperty.Register("Location",
                           GetType(Point), GetType(PanelVisual),
                           New PropertyMetadata(New Point(), New PropertyChangedCallback(AddressOf SharedLocationChanged)))
    Private Shared Sub SharedLocationChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, PanelVisual).LocationChanged(d, e)
    End Sub
    Protected Overridable Sub LocationChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        OnRender()
    End Sub
    Protected Overrides Sub OnRender()
        Dim _Position = New System.Windows.Vector(Location.X + RenderOffset.X, Location.Y + RenderOffset.Y)
        For Each _Visual In _Visuals
            _Visual.RenderOffset = _Position
        Next
    End Sub
End Class