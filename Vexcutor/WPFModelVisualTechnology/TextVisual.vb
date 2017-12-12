Imports System.Windows, System.Windows.Media, System.Windows.Data, System.Windows.Input

Public Class TextVisual
    Inherits ModelVisual
    Implements ISubstantialVisual
    'TextVisual -> Foreground As Brush Default: Brushes.Black
    Public Property Foreground As Brush
        Get
            Return GetValue(ForegroundProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(ForegroundProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ForegroundProperty As DependencyProperty =
                            DependencyProperty.Register("Foreground",
                            GetType(Brush), GetType(TextVisual),
                            New PropertyMetadata(Brushes.Black, New PropertyChangedCallback(AddressOf SharedTextChanged)))
    'TextVisual->FontFamily As String with Event Default: "Arial"
    Public Property FontFamily As FontFamily
        Get
            Return GetValue(FontFamilyProperty)
        End Get
        Set(ByVal value As FontFamily)
            SetValue(FontFamilyProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FontFamilyProperty As DependencyProperty =
                           DependencyProperty.Register("FontFamily",
                           GetType(FontFamily), GetType(TextVisual),
                           New FrameworkPropertyMetadata(New FontFamily("Arial"), New PropertyChangedCallback(AddressOf SharedTextChanged)))
    'TextVisual->FontSize As Double with Event Default: 16#
    Public Property FontSize As Double
        Get
            Return GetValue(FontSizeProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(FontSizeProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FontSizeProperty As DependencyProperty =
                           DependencyProperty.Register("FontSize",
                           GetType(Double), GetType(TextVisual),
                           New FrameworkPropertyMetadata(16.0#, New PropertyChangedCallback(AddressOf SharedTextChanged)))
    'TextVisual->FontWeight As FontWeight with Event Default: FontWeights.Normal
    Public Property FontWeight As FontWeight
        Get
            Return GetValue(FontWeightProperty)
        End Get
        Set(ByVal value As FontWeight)
            SetValue(FontWeightProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FontWeightProperty As DependencyProperty =
                           DependencyProperty.Register("FontWeight",
                           GetType(FontWeight), GetType(TextVisual),
                           New FrameworkPropertyMetadata(FontWeights.Normal, New PropertyChangedCallback(AddressOf SharedTextChanged)))
    'TextVisual->FontStyle As FontStyle with Event Default: FontStyles.Normal
    Public Property FontStyle As FontStyle
        Get
            Return GetValue(FontStyleProperty)
        End Get
        Set(ByVal value As FontStyle)
            SetValue(FontStyleProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FontStyleProperty As DependencyProperty =
                           DependencyProperty.Register("FontStyle",
                           GetType(FontStyle), GetType(TextVisual),
                           New FrameworkPropertyMetadata(FontStyles.Normal, New PropertyChangedCallback(AddressOf SharedTextChanged)))
    'TextVisual->FontStretch As FontStretch with Event Default: FontStretches.Normal
    Public Property FontStretch As FontStretch
        Get
            Return GetValue(FontStretchProperty)
        End Get
        Set(ByVal value As FontStretch)
            SetValue(FontStretchProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FontStretchProperty As DependencyProperty =
                           DependencyProperty.Register("FontStretch",
                           GetType(FontStretch), GetType(TextVisual),
                           New FrameworkPropertyMetadata(FontStretches.Normal, New PropertyChangedCallback(AddressOf SharedTextChanged)))
    'TextVisual->Text As String Default: ""
    Public Property Text As String
        Get
            Return GetValue(TextProperty)
        End Get
        Set(ByVal value As String)
            SetValue(TextProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TextProperty As DependencyProperty =
                           DependencyProperty.Register("Text",
                           GetType(String), GetType(TextVisual),
                           New PropertyMetadata("", New PropertyChangedCallback(AddressOf SharedTextChanged)))
    'TextVisual->MaxWidth As Double Default: Double.PositiveInfinity
    Public Property MaxWidth As Double
        Get
            Return GetValue(MaxWidthProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(MaxWidthProperty, value)
        End Set
    End Property
    Public Shared ReadOnly MaxWidthProperty As DependencyProperty =
                           DependencyProperty.Register("MaxWidth",
                           GetType(Double), GetType(TextVisual),
                           New PropertyMetadata(Double.PositiveInfinity, New PropertyChangedCallback(AddressOf SharedTextChanged)))
    ''TextVisual->ShouldFitPanelWidth As Boolean Default: False
    'Public Property ShouldFitPanelWidth As Boolean
    '    Get
    '        Return GetValue(ShouldFitPanelWidthProperty)
    '    End Get
    '    Set(ByVal value As Boolean)
    '        SetValue(ShouldFitPanelWidthProperty, value)
    '    End Set
    'End Property
    'Public Shared ReadOnly ShouldFitPanelWidthProperty As DependencyProperty = _
    '                       DependencyProperty.Register("ShouldFitPanelWidth", _
    '                       GetType(Boolean), GetType(TextVisual), _
    '                       New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedTextChanged)))

    'TextVisual->FlowDirection As FlowDirection Default: FlowDirection.LeftToRight
    Public Property FlowDirection As FlowDirection
        Get
            Return GetValue(FlowDirectionProperty)
        End Get
        Set(ByVal value As FlowDirection)
            SetValue(FlowDirectionProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FlowDirectionProperty As DependencyProperty =
                           DependencyProperty.Register("FlowDirection",
                           GetType(FlowDirection), GetType(TextVisual),
                           New PropertyMetadata(FlowDirection.LeftToRight, New PropertyChangedCallback(AddressOf SharedTextChanged)))
    'TextVisual->Location As Point Default: New Point
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
                           GetType(Point), GetType(TextVisual),
                           New PropertyMetadata(New Point, New PropertyChangedCallback(AddressOf SharedTextChanged)))
    Private Shared Sub SharedLocationChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, TextVisual).TextChanged(d, e)
    End Sub
    Private Sub LocationChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        OnRender()
    End Sub
    Protected Overrides Sub OnRender()
        If IsDrawing Then Return
        Using Context = RenderOpen()
            _FormatedText = New FormattedText(Text, Globalization.CultureInfo.CurrentCulture, FlowDirection,
                                               New Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Foreground)
            If Not Double.IsPositiveInfinity(MaxWidth) Then _FormatedText.MaxTextWidth = MaxWidth
            Context.DrawText(_FormatedText, Location + RenderOffset)
            If Not IsAllocating Then OnUpdateRenderBounds()
        End Using
    End Sub
    Private Shared Sub SharedTextChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, TextVisual).TextChanged(d, e)
    End Sub
    Private _FormatedText As FormattedText
    Private Sub TextChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        OnRender()
    End Sub

    Public Function GetAllocationGeometry() As Geometry Implements ISubstantialVisual.GetAllocationGeometry
        If _FormatedText Is Nothing Then _FormatedText = New FormattedText(Text, Globalization.CultureInfo.CurrentCulture, FlowDirection,
                                   New Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Foreground)
        Return New RectangleGeometry(New Rect(Location.X, Location.Y, _FormatedText.Width, _FormatedText.Height))
    End Function

    Public Sub ApplyOffset(offset As Vector) Implements ISubstantialVisual.ApplyOffset
        Location = New Point(Location.X + offset.X, Location.Y + offset.Y)
    End Sub
End Class