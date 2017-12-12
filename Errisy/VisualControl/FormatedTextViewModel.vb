Public Class FormatedTextViewModel
    Inherits AllocationViewModel

    'FormatedTextViewModel->Text As String Default: ""
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
                           GetType(String), GetType(FormatedTextViewModel), _
                           New PropertyMetadata("", New PropertyChangedCallback(AddressOf SharedFormatedTextChanged)))
    'FormatedTextViewModel->FontFamily As String with Event Default: "Arial"
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
                           GetType(FontFamily), GetType(FormatedTextViewModel), _
                           New PropertyMetadata(New FontFamily("Arial"), New PropertyChangedCallback(AddressOf SharedFormatedTextChanged)))
    'FormatedTextViewModel->FontSize As Double with Event Default: 16#
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
                           GetType(Double), GetType(FormatedTextViewModel), _
                           New PropertyMetadata(16.0#, New PropertyChangedCallback(AddressOf SharedFormatedTextChanged)))
    'FormatedTextViewModel->FontWeight As FontWeight with Event Default: FontWeights.Normal
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
                           GetType(FontWeight), GetType(FormatedTextViewModel), _
                           New PropertyMetadata(FontWeights.Normal, New PropertyChangedCallback(AddressOf SharedFormatedTextChanged)))
    'FormatedTextViewModel->FontStyle As FontStyle with Event Default: FontStyles.Normal
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
                           GetType(FontStyle), GetType(FormatedTextViewModel), _
                           New PropertyMetadata(FontStyles.Normal, New PropertyChangedCallback(AddressOf SharedFormatedTextChanged)))
    'FormatedTextViewModel->FontStretch As FontStretch with Event Default: FontStretches.Normal
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
                           GetType(FontStretch), GetType(FormatedTextViewModel), _
                           New PropertyMetadata(FontStretches.Normal, New PropertyChangedCallback(AddressOf SharedFormatedTextChanged)))
    'FormatedTextViewModel->Fill As Brush Default: Brushes.Black
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
                           GetType(Brush), GetType(FormatedTextViewModel), _
                           New PropertyMetadata(Brushes.Black, New PropertyChangedCallback(AddressOf SharedFormatedTextChanged)))
    'FormatedTextViewModel->FormatedText As System.Windows.Media.FormattedText Default: Nothing
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
                           GetType(System.Windows.Media.FormattedText), GetType(FormatedTextViewModel), _
                           New PropertyMetadata(Nothing))
    'FormatedTextViewModel->Location As Point Default: New Point()
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
                           GetType(Point), GetType(FormatedTextViewModel), _
                           New PropertyMetadata(New Point(), New PropertyChangedCallback(AddressOf SharedFormatedTextChanged)))
    'FormatedTextViewModel->FlowDirection As FlowDirection Default: FlowDirection.LeftToRight
    Public Property FlowDirection As FlowDirection
        Get
            Return GetValue(FlowDirectionProperty)
        End Get
        Set(ByVal value As FlowDirection)
            SetValue(FlowDirectionProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FlowDirectionProperty As DependencyProperty = _
                           DependencyProperty.Register("FlowDirection", _
                           GetType(FlowDirection), GetType(FormatedTextViewModel), _
                           New PropertyMetadata(FlowDirection.LeftToRight, New PropertyChangedCallback(AddressOf SharedFormatedTextChanged)))
    Private Shared Sub SharedFormatedTextChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, FormatedTextViewModel).FormatedTextChanged(d, e)
    End Sub
    Private Sub FormatedTextChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim newFormatetText As New System.Windows.Media.FormattedText(
                     Text,
                     System.Globalization.CultureInfo.CurrentCulture,
                     FlowDirection,
                     New Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Fill)
        SetValue(FormatedTextProperty, newFormatetText)
        Dim _Rect As New Rect(Location, New Size(newFormatetText.Width, newFormatetText.Height))
        SetValue(BoundsPropertyKey, _Rect)
        SetValue(HighlightGeometryPropertyKey, New RectangleGeometry With {.Rect = _Rect})
    End Sub
    'FormatedTextViewModel->ShouldShowHighlight As Boolean Default: False
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
                           GetType(Boolean), GetType(FormatedTextViewModel), _
                           New PropertyMetadata(False))
    'FormatedTextViewModel -> HighlightGeometry As Geometry Default: Nothing
    Public ReadOnly Property HighlightGeometry As Geometry
        Get
            Return GetValue(FormatedTextViewModel.HighlightGeometryProperty)
        End Get
    End Property
    Private Shared ReadOnly HighlightGeometryPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("HighlightGeometry", _
                              GetType(Geometry), GetType(FormatedTextViewModel), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly HighlightGeometryProperty As DependencyProperty = _
                             HighlightGeometryPropertyKey.DependencyProperty
    'FormatedTextViewModel -> Bounds As Rect Default: New Rect()
    Public ReadOnly Property Bounds As Rect
        Get
            Return GetValue(FormatedTextViewModel.BoundsProperty)
        End Get
    End Property
    Private Shared ReadOnly BoundsPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Bounds", _
                              GetType(Rect), GetType(FormatedTextViewModel), _
                              New PropertyMetadata(New Rect()))
    Public Shared ReadOnly BoundsProperty As DependencyProperty = _
                             BoundsPropertyKey.DependencyProperty

    'FormatedTextViewModel->HighlightFill As Brushes Default: Brushes.Yellow
    Public Property HighlightFill As Brush
        Get
            Return GetValue(HighlightFillProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(HighlightFillProperty, value)
        End Set
    End Property
    Public Shared ReadOnly HighlightFillProperty As DependencyProperty = _
                           DependencyProperty.Register("HighlightFill", _
                           GetType(Brush), GetType(FormatedTextViewModel), _
                           New PropertyMetadata(Brushes.Yellow))

    Public Overrides ReadOnly Property GetSpaceGeometry As Geometry
        Get
            Return HighlightGeometry
        End Get
    End Property
    Public Overrides Sub ApplyOffset(Offset As Vector)
        Location = Location + Offset
        Dim _Rect As New Rect(Location, New Size(FormatedText.Width, FormatedText.Height))
        SetValue(BoundsPropertyKey, _Rect)
        SetValue(HighlightGeometryPropertyKey, New RectangleGeometry With {.Rect = _Rect})
    End Sub

    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New FormatedTextViewModel
    End Function
End Class
 