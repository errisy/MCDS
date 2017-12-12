Public Class IncludableTriangle
    Inherits IncludableElement
    Private _Triangle As New DrawingVisual
    Public Sub New()
        MyBase.Visuals.Add(_Triangle)
    End Sub
    'IncludableTriangle->Fill As Brush Default: Brushes.White
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
                           GetType(Brush), GetType(IncludableTriangle), _
                           New PropertyMetadata(Brushes.White, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableTriangle->Stroke As Brush Default: Brushes.Black
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
                           GetType(Brush), GetType(IncludableTriangle), _
                           New PropertyMetadata(Brushes.Black, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableTriangle->StrokeThickness As Double Default: 1#
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
                           GetType(Double), GetType(IncludableTriangle), _
                           New PropertyMetadata(1.0#, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableTriangle->APoint As Point Default: New Point
    Public Property APoint As Point
        Get
            Return GetValue(APointProperty)
        End Get
        Set(ByVal value As Point)
            SetValue(APointProperty, value)
        End Set
    End Property
    Public Shared ReadOnly APointProperty As DependencyProperty = _
                           DependencyProperty.Register("APoint", _
                           GetType(Point), GetType(IncludableTriangle), _
                           New PropertyMetadata(New Point, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableTriangle->BPoint As Point Default: New Point
    Public Property BPoint As Point
        Get
            Return GetValue(BPointProperty)
        End Get
        Set(ByVal value As Point)
            SetValue(BPointProperty, value)
        End Set
    End Property
    Public Shared ReadOnly BPointProperty As DependencyProperty = _
                           DependencyProperty.Register("BPoint", _
                           GetType(Point), GetType(IncludableTriangle), _
                           New PropertyMetadata(New Point, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableTriangle->CPoint As Point Default: New Point
    Public Property CPoint As Point
        Get
            Return GetValue(CPointProperty)
        End Get
        Set(ByVal value As Point)
            SetValue(CPointProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CPointProperty As DependencyProperty = _
                           DependencyProperty.Register("CPoint", _
                           GetType(Point), GetType(IncludableTriangle), _
                           New PropertyMetadata(New Point, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    Private Shared Sub SharedDrawChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, IncludableTriangle).DrawChanged(d, e)
    End Sub
    Private Sub DrawChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim dc = _Triangle.RenderOpen
        Dim geo As New StreamGeometry With {.FillRule = FillRule.Nonzero}
        Using geometryContext = geo.Open
            geometryContext.BeginFigure(APoint, True, True)
            geometryContext.LineTo(BPoint, True, False)
            geometryContext.LineTo(CPoint, True, False)
        End Using
        geo.Freeze()
        dc.DrawGeometry(Fill, New Pen(Stroke, StrokeThickness), geo)
        dc.Close()
    End Sub
End Class
