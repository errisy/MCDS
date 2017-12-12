Public Class LineViewModel
    Inherits AllocationViewModel
    'LineViewModel->StartPoint As Point Default: New Point()
    Public Property StartPoint As Point
        Get
            Return GetValue(StartPointProperty)
        End Get
        Set(ByVal value As Point)
            SetValue(StartPointProperty, value)
        End Set
    End Property
    Public Shared ReadOnly StartPointProperty As DependencyProperty = _
                           DependencyProperty.Register("StartPoint", _
                           GetType(Point), GetType(LineViewModel), _
                           New PropertyMetadata(New Point(), New PropertyChangedCallback(AddressOf SharedGeometryChanged)))
    'LineViewModel->EndPoint As Point Default: New Point()
    Public Property EndPoint As Point
        Get
            Return GetValue(EndPointProperty)
        End Get
        Set(ByVal value As Point)
            SetValue(EndPointProperty, value)
        End Set
    End Property
    Public Shared ReadOnly EndPointProperty As DependencyProperty = _
                           DependencyProperty.Register("EndPoint", _
                           GetType(Point), GetType(LineViewModel), _
                           New PropertyMetadata(New Point(), New PropertyChangedCallback(AddressOf SharedGeometryChanged)))
    'LineViewModel->Stroke As Brush Default: Brushes.Black
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
                           GetType(Brush), GetType(LineViewModel), _
                           New PropertyMetadata(Brushes.Black, New PropertyChangedCallback(AddressOf SharedGeometryChanged)))
    'LineViewModel->StrokeThickness As Double Default: 1#
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
                           GetType(Double), GetType(LineViewModel), _
                           New PropertyMetadata(1.0#, New PropertyChangedCallback(AddressOf SharedGeometryChanged)))
    'LineViewModel -> Geometry As Geometry Default: PathGeometry.Empty
    Public ReadOnly Property Geometry As Geometry
        Get
            Return GetValue(LineViewModel.GeometryProperty)
        End Get
    End Property
    Private Shared ReadOnly GeometryPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Geometry", _
                              GetType(Geometry), GetType(LineViewModel), _
                              New PropertyMetadata(PathGeometry.Empty))
    Public Shared ReadOnly GeometryProperty As DependencyProperty = _
                             GeometryPropertyKey.DependencyProperty

    Private Shared Sub SharedGeometryChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, LineViewModel).GeometryChanged(d, e)
    End Sub
    Private Sub GeometryChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim _StreamGeometry As New StreamGeometry
        Using _Context = _StreamGeometry.Open
            _Context.BeginFigure(StartPoint, False, False)
            _Context.LineTo(EndPoint, True, False)
        End Using
        SetValue(GeometryPropertyKey, _StreamGeometry)
    End Sub

    Public Overrides Sub ApplyOffset(Offset As Vector)
        Geometry.Transform = New TranslateTransform(Offset.X, Offset.Y)
    End Sub
    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New LineViewModel
    End Function

    Public Overrides ReadOnly Property GetSpaceGeometry As Geometry
        Get
            Return Geometry
        End Get
    End Property
End Class
