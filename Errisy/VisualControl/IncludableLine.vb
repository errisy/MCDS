Public Class IncludableLine
    Inherits IncludableElement
    Private _Line As New DrawingVisual
    Public Sub New()
        MyBase.Visuals.Add(_Line)
        'Set Default Bindings
        SetBinding(StartPointProperty, "StartPoint")
        SetBinding(EndPointProperty, "EndPoint")
        SetBinding(StrokeProperty, "Stroke")
        SetBinding(StrokeThicknessProperty, "StrokeThickness")
        SetBinding(ToolTipProperty, "ToolTip")
    End Sub
    'IncludableLine->StartPoint As Point with Event Default: New Point
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
                           GetType(Point), GetType(IncludableLine), _
                           New PropertyMetadata(New Point, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableLine->EndPoint As Point Default: New Point
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
                           GetType(Point), GetType(IncludableLine), _
                           New PropertyMetadata(New Point, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableLine->StrokeThickness As Double Default: 1#
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
                           GetType(Double), GetType(IncludableLine), _
                           New PropertyMetadata(1.0#, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableLine->Stroke As Brush Default: Brushes.Black
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
                           GetType(Brush), GetType(IncludableLine), _
                           New PropertyMetadata(Brushes.Black, New PropertyChangedCallback(AddressOf SharedDrawChanged)))

    Private Shared Sub SharedDrawChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, IncludableLine).DrawChanged(d, e)
    End Sub
    Private Sub DrawChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim dc = _Line.RenderOpen
        dc.DrawLine(New Pen(Stroke, StrokeThickness), StartPoint, EndPoint)
        dc.Close()
    End Sub
End Class

 