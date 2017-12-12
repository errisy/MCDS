Imports System.Windows, System.Windows.Media, System.Windows.Data, System.Windows.Input
Public Class ShapeVisual
    Inherits ModelVisual
    Implements ISubstantialVisual
    'ShapeVisual->Fill As Brush Default: Brushes.White
    Public Property Fill As Brush
        Get
            Return GetValue(FillProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(FillProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FillProperty As DependencyProperty =
                           DependencyProperty.Register("Fill",
                           GetType(Brush), GetType(ShapeVisual),
                           New PropertyMetadata(Brushes.White, New PropertyChangedCallback(AddressOf SharedRenderChanged)))
    'ShapeVisual->Stroke As Brush Default: Brushes.Black
    Public Property Stroke As Brush
        Get
            Return GetValue(StrokeProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(StrokeProperty, value)
        End Set
    End Property
    Public Shared ReadOnly StrokeProperty As DependencyProperty =
                           DependencyProperty.Register("Stroke",
                           GetType(Brush), GetType(ShapeVisual),
                           New PropertyMetadata(Brushes.Black, New PropertyChangedCallback(AddressOf SharedRenderChanged)))
    'ShapeVisual->StrokeThickness As Double Default: 1#
    Public Property StrokeThickness As Double
        Get
            Return GetValue(StrokeThicknessProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(StrokeThicknessProperty, value)
        End Set
    End Property
    Public Shared ReadOnly StrokeThicknessProperty As DependencyProperty =
                           DependencyProperty.Register("StrokeThickness",
                           GetType(Double), GetType(ShapeVisual),
                           New PropertyMetadata(1.0#, New PropertyChangedCallback(AddressOf SharedRenderChanged)))
    Private Shared Sub SharedRenderChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, ShapeVisual).RenderChanged(d, e)
    End Sub
    Private Sub RenderChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If IsDrawing Then Return
        OnRender()
    End Sub
    'ShapeVisual->Geometry As Geometry with Event Default: PathGeometry.Empty
    Public Property Geometry As Geometry
        Get
            Return GetValue(GeometryProperty)
        End Get
        Set(ByVal value As Geometry)
            SetValue(GeometryProperty, value)
        End Set
    End Property
    Public Shared ReadOnly GeometryProperty As DependencyProperty =
                           DependencyProperty.Register("Geometry",
                           GetType(Geometry), GetType(ShapeVisual),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedGeometryChanged)))
    Private Shared Sub SharedGeometryChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, ShapeVisual).GeometryChanged(d, e)
    End Sub
    Private Sub GeometryChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.OldValue IsNot Nothing Then
            Dim oldGeometry As Geometry = e.OldValue
            If Not oldGeometry.IsFrozen Then RemoveHandler oldGeometry.Changed, AddressOf GeometryValueChanged
        End If
        If e.NewValue Is Nothing Then
            _Geometry = PathGeometry.Empty
            _GeometryBaseOffset = New Vector
        Else
            Dim newGeometry As Geometry = e.NewValue
            _Geometry = newGeometry.Clone
            If TypeOf newGeometry.Transform Is MatrixTransform Then
                Dim _Matrix As MatrixTransform = newGeometry.Transform
                _GeometryBaseOffset = New Vector(_Matrix.Value.OffsetX, _Matrix.Value.OffsetY)
            ElseIf TypeOf newGeometry.Transform Is TranslateTransform Then
                Dim _Translate As TranslateTransform = newGeometry.Transform
                _GeometryBaseOffset = New Vector(_Translate.X, _Translate.Y)
            End If
            If Not newGeometry.IsFrozen Then AddHandler newGeometry.Changed, AddressOf GeometryValueChanged
        End If
        If IsDrawing Then Return
        OnRender()
    End Sub
    Private Sub GeometryValueChanged(sender As Object, e As EventArgs)
        Dim newGeometry As Geometry = GetValue(GeometryProperty)
        _Geometry = newGeometry.Clone
        Dim _Translate As TranslateTransform = newGeometry.Transform
        _GeometryBaseOffset = New Vector(_Translate.X, _Translate.Y)
    End Sub

    Private _GeometryBaseOffset As Vector
    Private _Geometry As Geometry = PathGeometry.Empty

    Protected Overrides Sub OnRender()
        If _Geometry Is Nothing OrElse _Geometry.IsEmpty Then Return
        Dim _Offset = GetValue(RenderOffsetProperty)
        _Geometry.Transform = New TranslateTransform(_GeometryBaseOffset.X + _Offset.X, _GeometryBaseOffset.Y + _Offset.Y)
        Dim _Pen As New Pen(Stroke, StrokeThickness)
        Using Context = RenderOpen()
            Context.DrawGeometry(Fill, _Pen, _Geometry)
            If Not IsAllocating Then OnUpdateRenderBounds()
        End Using
    End Sub

    Public Sub ApplyOffset(offset As Vector) Implements ISubstantialVisual.ApplyOffset
        Dim vGeometry As Geometry = Geometry
        If vGeometry IsNot Nothing AndAlso Not vGeometry.IsFrozen Then
            vGeometry.Transform = New TranslateTransform(offset.X, offset.Y)
        End If
    End Sub

    Public Function GetAllocationGeometry() As Geometry Implements ISubstantialVisual.GetAllocationGeometry
        If Geometry Is Nothing Then Return PathGeometry.Empty
        Return Geometry
    End Function
End Class
