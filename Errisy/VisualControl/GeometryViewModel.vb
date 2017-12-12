Public Class GeometryViewModel
    Inherits AllocationViewModel
    'GeometryViewModel->Fill As Brush Default: Brushes.White
    Public Property Fill As Brush
        Get
            Return GetValue(FillProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(FillProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FillProperty As DependencyProperty = _
                           DependencyProperty.Register("Fill", _
                           GetType(Brush), GetType(GeometryViewModel), _
                           New PropertyMetadata(Brushes.White))
    'GeometryViewModel->Stroke As Brush Default: Brushes.Black
    Public Property Stroke As Brush
        Get
            Return GetValue(StrokeProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(StrokeProperty, value)
        End Set
    End Property
    Public Shared ReadOnly StrokeProperty As DependencyProperty = _
                           DependencyProperty.Register("Stroke", _
                           GetType(Brush), GetType(GeometryViewModel), _
                           New PropertyMetadata(Brushes.Black))
    'GeometryViewModel->StrokeThickness As Double Default: 1#
    Public Property StrokeThickness As Double
        Get
            Return GetValue(StrokeThicknessProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(StrokeThicknessProperty, value)
        End Set
    End Property
    Public Shared ReadOnly StrokeThicknessProperty As DependencyProperty = _
                           DependencyProperty.Register("StrokeThickness", _
                           GetType(Double), GetType(GeometryViewModel), _
                           New PropertyMetadata(1.0#))

    'GeometryViewModel->Geometry As Geometry Default: Nothing
    Public Property Geometry As Geometry
        Get
            Return GetValue(GeometryProperty)
        End Get
        Set(ByVal value As Geometry)
            SetValue(GeometryProperty, value)
        End Set
    End Property
    Public Shared ReadOnly GeometryProperty As DependencyProperty = _
                           DependencyProperty.Register("Geometry", _
                           GetType(Geometry), GetType(GeometryViewModel), _
                           New PropertyMetadata(PathGeometry.Empty))
    Private Shared Sub SharedGeometryChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, GeometryViewModel).GeometryChanged(d, e)
    End Sub
    Protected Overridable Sub GeometryChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)

    End Sub
    Public Function Intersects(otherGeometry As Geometry) As Boolean
        Dim IntersectGeometry = PathGeometry.Combine(Geometry, otherGeometry, GeometryCombineMode.Intersect, Nothing)
        Return IntersectGeometry.GetArea > 0.0#
    End Function

    Public Overrides ReadOnly Property GetSpaceGeometry As Geometry
        Get
            If Geometry Is Nothing Then Return PathGeometry.Empty
            Return Geometry
        End Get
    End Property

    Public Overrides Sub ApplyOffset(Offset As Vector)
        Geometry.Transform = New TranslateTransform(Offset.X, Offset.Y)
    End Sub

    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New GeometryViewModel
    End Function

    'GeometryViewModel->ToolTip As Object Default: Nothing
    Public Property ToolTip As Object
        Get
            Return GetValue(ToolTipProperty)
        End Get
        Set(ByVal value As Object)
            SetValue(ToolTipProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ToolTipProperty As DependencyProperty = _
                           DependencyProperty.Register("ToolTip", _
                           GetType(Object), GetType(GeometryViewModel), _
                           New PropertyMetadata(Nothing))

End Class
