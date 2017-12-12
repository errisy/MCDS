Public Class GeometryTextViewModel
    Inherits AllocationViewModel
    'GeometryTextViewModel->Text As String Default: ""
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
                           GetType(String), GetType(GeometryTextViewModel), _
                           New PropertyMetadata("", New PropertyChangedCallback(AddressOf SharedGeometryTextChanged)))
    'GeometryTextViewModel->FontFamily As String with Event Default: "Arial"
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
                           GetType(FontFamily), GetType(GeometryTextViewModel), _
                           New PropertyMetadata(New FontFamily("Arial"), New PropertyChangedCallback(AddressOf SharedGeometryTextChanged)))
    'GeometryTextViewModel->FontSize As Double with Event Default: 16#
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
                           GetType(Double), GetType(GeometryTextViewModel), _
                           New PropertyMetadata(16.0#, New PropertyChangedCallback(AddressOf SharedGeometryTextChanged)))
    'GeometryTextViewModel->FontWeight As FontWeight with Event Default: FontWeights.Normal
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
                           GetType(FontWeight), GetType(GeometryTextViewModel), _
                           New PropertyMetadata(FontWeights.Normal, New PropertyChangedCallback(AddressOf SharedGeometryTextChanged)))
    'GeometryTextViewModel->FontStyle As FontStyle with Event Default: FontStyles.Normal
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
                           GetType(FontStyle), GetType(GeometryTextViewModel), _
                           New PropertyMetadata(FontStyles.Normal, New PropertyChangedCallback(AddressOf SharedGeometryTextChanged)))
    'GeometryTextViewModel->FontStretch As FontStretch with Event Default: FontStretches.Normal
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
                           GetType(FontStretch), GetType(GeometryTextViewModel), _
                           New PropertyMetadata(FontStretches.Normal, New PropertyChangedCallback(AddressOf SharedGeometryTextChanged)))
    'GeometryTextViewModel->Fill As Brush Default: Brushes.White
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
                           GetType(Brush), GetType(GeometryTextViewModel), _
                           New PropertyMetadata(Brushes.White))
    'GeometryTextViewModel->Stroke As Brush Default: Brushes.Black
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
                           GetType(Brush), GetType(GeometryTextViewModel), _
                           New PropertyMetadata(Brushes.Black))
    'GeometryTextViewModel->StrokeThickness As Double Default: 1#
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
                           GetType(Double), GetType(GeometryTextViewModel), _
                           New PropertyMetadata(1.0#))

    'GeometryTextViewModel->Geometry As Geometry Default: Nothing
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
                           GetType(Geometry), GetType(GeometryTextViewModel), _
                           New PropertyMetadata(PathGeometry.Empty))

    'GeometryTextViewModel->Location As Point Default: New Point()
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
                           GetType(Point), GetType(GeometryTextViewModel), _
                           New PropertyMetadata(New Point(), New PropertyChangedCallback(AddressOf SharedGeometryTextChanged)))
    'GeometryTextViewModel->FlowDirection As FlowDirection Default: FlowDirection.LeftToRight
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
                           GetType(FlowDirection), GetType(GeometryTextViewModel), _
                           New PropertyMetadata(FlowDirection.LeftToRight, New PropertyChangedCallback(AddressOf SharedGeometryTextChanged)))
    Private Shared Sub SharedGeometryTextChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, GeometryTextViewModel).GeometryTextChanged(d, e)
    End Sub
    Private Sub GeometryTextChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim newFormatedText =
                 New System.Windows.Media.FormattedText(
                     Text,
                     System.Globalization.CultureInfo.CurrentCulture,
                     FlowDirection,
                     New Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Fill)
        SetValue(GeometryProperty, newFormatedText.BuildGeometry(Location))
    End Sub
    Public Overrides ReadOnly Property GetSpaceGeometry As Geometry
        Get
            Return Geometry
        End Get
    End Property

    Public Overrides Sub ApplyOffset(Offset As Vector)
        Location = Location + Offset
    End Sub
    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New GeometryTextViewModel
    End Function
End Class
