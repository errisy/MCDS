Public Class IncludableText
    Inherits IncludableElement
    Private _Text As New DrawingVisual
    Public Sub New()
        MyBase.Visuals.Add(_Text)
    End Sub
    'IncludableText->Text As String Default: ""
    Public Property Text As String
        Get
            Return GetValue(TextProperty)
        End Get
        Set(ByVal value As String)
            SetValue(TextProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TextProperty As DependencyProperty = _
                           DependencyProperty.Register("Text", _
                           GetType(String), GetType(IncludableText), _
                           New PropertyMetadata("", New PropertyChangedCallback(AddressOf SharedDrawChanged)))

    'IncludableText -> Foreground As Brush Default: Brushes.Black
    Public Property Foreground As Brush
        Get
            Return GetValue(ForegroundProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(ForegroundProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ForegroundProperty As DependencyProperty = _
                            DependencyProperty.Register("Foreground", _
                            GetType(Brush), GetType(IncludableText), _
                            New PropertyMetadata(Brushes.Black, New PropertyChangedCallback(AddressOf SharedDrawChanged)))

    'IncludableText->FontFamily As String with Event Default: "Arial"
    Public Property FontFamily As FontFamily
        Get
            Return GetValue(FontFamilyProperty)
        End Get
        Set(ByVal value As FontFamily)
            SetValue(FontFamilyProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FontFamilyProperty As DependencyProperty = _
                           DependencyProperty.Register("FontFamily", _
                           GetType(FontFamily), GetType(IncludableText), _
                           New FrameworkPropertyMetadata(New FontFamily("Arial"), New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableText->FontSize As Double with Event Default: 16#
    Public Property FontSize As Double
        Get
            Return GetValue(FontSizeProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(FontSizeProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FontSizeProperty As DependencyProperty = _
                           DependencyProperty.Register("FontSize", _
                           GetType(Double), GetType(IncludableText), _
                           New FrameworkPropertyMetadata(16.0#, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableText->FontWeight As FontWeight with Event Default: FontWeights.Normal
    Public Property FontWeight As FontWeight
        Get
            Return GetValue(FontWeightProperty)
        End Get
        Set(ByVal value As FontWeight)
            SetValue(FontWeightProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FontWeightProperty As DependencyProperty = _
                           DependencyProperty.Register("FontWeight", _
                           GetType(FontWeight), GetType(IncludableText), _
                           New FrameworkPropertyMetadata(FontWeights.Normal, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableText->FontStyle As FontStyle with Event Default: FontStyles.Normal
    Public Property FontStyle As FontStyle
        Get
            Return GetValue(FontStyleProperty)
        End Get
        Set(ByVal value As FontStyle)
            SetValue(FontStyleProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FontStyleProperty As DependencyProperty = _
                           DependencyProperty.Register("FontStyle", _
                           GetType(FontStyle), GetType(IncludableText), _
                           New FrameworkPropertyMetadata(FontStyles.Normal, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableText->FontStretch As FontStretch with Event Default: FontStretches.Normal
    Public Property FontStretch As FontStretch
        Get
            Return GetValue(FontStretchProperty)
        End Get
        Set(ByVal value As FontStretch)
            SetValue(FontStretchProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FontStretchProperty As DependencyProperty = _
                           DependencyProperty.Register("FontStretch", _
                           GetType(FontStretch), GetType(IncludableText), _
                           New FrameworkPropertyMetadata(FontStretches.Normal, New PropertyChangedCallback(AddressOf SharedDrawChanged)))
    'IncludableText->Location As Point Default: New Point()
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
                           GetType(Point), GetType(IncludableText), _
                           New PropertyMetadata(New Point(), New PropertyChangedCallback(AddressOf SharedDrawChanged)))

    Private Shared Sub SharedDrawChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, IncludableText).DrawChanged(d, e)
    End Sub
    Private Sub DrawChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim dc = _Text.RenderOpen
        dc.DrawText(New FormattedText(Text,
                                      Globalization.CultureInfo.CurrentCulture,
                                      FlowDirection,
                                      New Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                                      FontSize,
                                      Foreground),
                                  Location)
        dc.Close()
    End Sub

End Class
