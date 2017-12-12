Public Class IncludableFormatedText
    Inherits IncludableElement
    Private _Highlight As New DrawingVisual
    Private _Text As New DrawingVisual
    Public Sub New()
        MyBase.Visuals.Add(_Highlight)
        MyBase.Visuals.Add(_Text)
        'Set Default Bindings
        SetBinding(HighlightGeometryProperty, "HighlightGeometry")
        SetBinding(HighlightBrushProperty, "HighlightBrush")
        SetBinding(HighlightStrokeProperty, "HighlightStroke")
        SetBinding(HighlightStrokeThicknessProperty, "HighlightStrokeThickness")
        SetBinding(ShouldShowHighlightProperty, "ShouldShowHighlight")
        SetBinding(LocationProperty, "Location")
        SetBinding(FormatedTextProperty, "FormatedText")
    End Sub
    'IncludableFormatedText->HighlightGeometry As Geometry Default: Nothing
    Public Property HighlightGeometry As Geometry
        Get
            Return GetValue(HighlightGeometryProperty)
        End Get
        Set(ByVal value As Geometry)
            SetValue(HighlightGeometryProperty, value)
        End Set
    End Property
    Public Shared ReadOnly HighlightGeometryProperty As DependencyProperty = _
                           DependencyProperty.Register("HighlightGeometry", _
                           GetType(Geometry), GetType(IncludableFormatedText), _
                           New PropertyMetadata(PathGeometry.Empty, New PropertyChangedCallback(AddressOf SharedHighlightChanged)))
    'IncludableFormatedText->HighlightBrush As Brush Default: Brushes.Yellow
    Public Property HighlightBrush As Brush
        Get
            Return GetValue(HighlightBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(HighlightBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly HighlightBrushProperty As DependencyProperty = _
                           DependencyProperty.Register("HighlightBrush", _
                           GetType(Brush), GetType(IncludableFormatedText), _
                           New PropertyMetadata(Brushes.Yellow, New PropertyChangedCallback(AddressOf SharedHighlightChanged)))
    'IncludableFormatedText->HighlightStroke As Brush Default: Brushes.Red
    Public Property HighlightStroke As Brush
        Get
            Return GetValue(HighlightStrokeProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(HighlightStrokeProperty, value)
        End Set
    End Property
    Public Shared ReadOnly HighlightStrokeProperty As DependencyProperty = _
                           DependencyProperty.Register("HighlightStroke", _
                           GetType(Brush), GetType(IncludableFormatedText), _
                           New PropertyMetadata(Brushes.Red, New PropertyChangedCallback(AddressOf SharedHighlightChanged)))
    'IncludableFormatedText->HighlightStrokeThickness As Double Default: 1#
    Public Property HighlightStrokeThickness As Double
        Get
            Return GetValue(HighlightStrokeThicknessProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(HighlightStrokeThicknessProperty, value)
        End Set
    End Property
    Public Shared ReadOnly HighlightStrokeThicknessProperty As DependencyProperty = _
                           DependencyProperty.Register("HighlightStrokeThickness", _
                           GetType(Double), GetType(IncludableFormatedText), _
                           New PropertyMetadata(1.0#, New PropertyChangedCallback(AddressOf SharedHighlightChanged)))
    'IncludableFormatedText->ShouldShowHighlight As Boolean with Event Default: False
    Public Property ShouldShowHighlight As Boolean
        Get
            Return GetValue(ShouldShowHighlightProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(ShouldShowHighlightProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ShouldShowHighlightProperty As DependencyProperty = _
                           DependencyProperty.Register("ShouldShowHighlight", _
                           GetType(Boolean), GetType(IncludableFormatedText), _
                           New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedHighlightChanged)))
    Private Shared Sub SharedHighlightChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, IncludableFormatedText).HighlightChanged(d, e)
    End Sub
    Private Sub HighlightChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If ShouldShowHighlight Then
            Dim dc = _Highlight.RenderOpen
            dc.DrawGeometry(HighlightBrush, New Pen(HighlightStroke, HighlightStrokeThickness), HighlightGeometry)
            dc.Close()
        Else
            Dim dc = _Highlight.RenderOpen
            dc.Close()
        End If
    End Sub

    'IncludableFormatedText->Location As Point Default: New Point()
    Public Property Location As Point
        Get
            Return GetValue(LocationProperty)
        End Get
        Set(ByVal value As Point)
            SetValue(LocationProperty, value)
        End Set
    End Property
    Public Shared ReadOnly LocationProperty As DependencyProperty = _
                           DependencyProperty.Register("Location", _
                           GetType(Point), GetType(IncludableFormatedText), _
                           New PropertyMetadata(New Point(), New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableFormatedText->FormatedText As System.Windows.Media.FormattedText Default: Nothing
    Public Property FormatedText As System.Windows.Media.FormattedText
        Get
            Return GetValue(FormatedTextProperty)
        End Get
        Set(ByVal value As System.Windows.Media.FormattedText)
            SetValue(FormatedTextProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FormatedTextProperty As DependencyProperty = _
                           DependencyProperty.Register("FormatedText", _
                           GetType(System.Windows.Media.FormattedText), GetType(IncludableFormatedText), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    Private Shared Sub SharedDrawChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, IncludableFormatedText).DrawChanged(d, e)
    End Sub
    Private Sub DrawChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim dc = _Text.RenderOpen
        dc.DrawText(FormatedText, Location)
        dc.Close()
    End Sub
End Class
