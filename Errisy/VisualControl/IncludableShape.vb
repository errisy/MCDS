Public Class IncludableShape
    Inherits IncludableElement
    Private _Path As New DrawingVisual
    Public Sub New()
        MyBase.Visuals.Add(_Path)
        'Set Default Bindings
        SetBinding(FillProperty, "Fill")
        SetBinding(StrokeProperty, "Stroke")
        SetBinding(StrokeThicknessProperty, "StrokeThickness")
        SetBinding(GeometryProperty, "Geometry")
        SetBinding(ToolTipProperty, "ToolTip")
    End Sub
    'IncludableShape->Fill As Brush Default: Brushes.White
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
                           GetType(Brush), GetType(IncludableShape), _
                           New PropertyMetadata(Brushes.White, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableShape->Stroke As Brush Default: Brushes.Black
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
                           GetType(Brush), GetType(IncludableShape), _
                           New PropertyMetadata(Brushes.Black, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableShape->StrokeThickness As Double Default: 1#
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
                           GetType(Double), GetType(IncludableShape), _
                           New PropertyMetadata(1.0#, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableShape->Geometry As Geometry Default: Nothing
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
                           GetType(Geometry), GetType(IncludableShape), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    Private Shared Sub SharedDrawChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, IncludableShape).DrawChanged(d, e)
    End Sub
    Private Sub DrawChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim dc = _Path.RenderOpen
        If TypeOf Geometry Is Geometry Then
            If Not Geometry.IsFrozen And Geometry.CanFreeze Then Geometry.Freeze()
            dc.DrawGeometry(Fill, New Pen(Stroke, StrokeThickness), Geometry)
        End If
        dc.Close()
    End Sub
End Class

 