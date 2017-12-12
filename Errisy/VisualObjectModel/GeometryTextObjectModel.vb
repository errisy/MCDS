Public Class GeometryTextObjectModel
    Inherits AllocationObjectModel
    Public Overrides Sub ApplyOffset(Offset As Vector)
        Location += Offset
        Change()
    End Sub
    Public Overrides Sub Change()
        _Geometry = Nothing
        _HighlightGeometry = Nothing
        MyBase.Change()
    End Sub
    Private _HighlightGeometry As Geometry
    Public Overrides ReadOnly Property GetSpaceGeometry As Geometry
        Get
            If _Geometry Is Nothing Then
                Dim _FormatedText = New System.Windows.Media.FormattedText(
                     Text,
                     System.Globalization.CultureInfo.CurrentCulture,
                     FlowDirection,
                     New Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Fill)
                _Geometry = _FormatedText.BuildGeometry(Location)
                _HighlightGeometry = _FormatedText.BuildHighlightGeometry(Location)
            End If
            Return _Geometry
        End Get
    End Property
    Public ReadOnly Property HighlightGeometry As Geometry
        Get
            If _HighlightGeometry Is Nothing Then
                Dim _FormatedText = New System.Windows.Media.FormattedText(
                     Text,
                     System.Globalization.CultureInfo.CurrentCulture,
                     FlowDirection,
                     New Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Fill)
                _Geometry = _FormatedText.BuildGeometry(Location)
                _HighlightGeometry = _FormatedText.BuildHighlightGeometry(Location)
            End If
            Return _HighlightGeometry
        End Get
    End Property
    'Public Property Text As String
    Private _Text As String
    Public Property Text As String
        Get
            Return _Text
        End Get
        Set(value As String)
            _Text = value
            Change()
        End Set
    End Property
    'Public Property FontFamily As String
    Private _FontFamily As FontFamily = New FontFamily("Arial")
    Public Property FontFamily As FontFamily
        Get
            Return _FontFamily
        End Get
        Set(value As FontFamily)
            _FontFamily = value
            Change()
        End Set
    End Property
    'Public Property FontSize As Double
    Private _FontSize As Double = 16.0#
    Public Property FontSize As Double
        Get
            Return _FontSize
        End Get
        Set(value As Double)
            _FontSize = value
            Change()
        End Set
    End Property
    'Public Property FontWeight As FontWeight
    Private _FontWeight As FontWeight = FontWeights.Normal
    Public Property FontWeight As FontWeight
        Get
            Return _FontWeight
        End Get
        Set(value As FontWeight)
            _FontWeight = value
            Change()
        End Set
    End Property
    'Public Property FontStyle As FontStyle
    Private _FontStyle As FontStyle = FontStyles.Normal
    Public Property FontStyle As FontStyle
        Get
            Return _FontStyle
        End Get
        Set(value As FontStyle)
            _FontStyle = value
            Change()
        End Set
    End Property
    'Public Property FontStretch As FontStretch
    Private _FontStretch As FontStretch = FontStretches.Normal
    Public Property FontStretch As FontStretch
        Get
            Return _FontStretch
        End Get
        Set(value As FontStretch)
            _FontStretch = value
            Change()
        End Set
    End Property
    'Public Property FlowDirection As FlowDirection
    Private _FlowDirection As FlowDirection
    Public Property FlowDirection As FlowDirection
        Get
            Return _FlowDirection
        End Get
        Set(value As FlowDirection)
            _FlowDirection = value
            Change()
        End Set
    End Property

    'Public Property Fill As Brush
    Private _Fill As Brush = Brushes.Black
    Public Property Fill As Brush
        Get
            Return _Fill
        End Get
        Set(value As Brush)
            _Fill = value
            Change()
        End Set
    End Property
    'Public Property FormatedText As System.Windows.Media.FormattedText
    Private _Geometry As Geometry
    Public ReadOnly Property Geometry As Geometry
        Get
            If _Geometry Is Nothing Then
                Dim _FormatedText = New System.Windows.Media.FormattedText(
                     Text,
                     System.Globalization.CultureInfo.CurrentCulture,
                     FlowDirection,
                     New Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Fill)
                _Geometry = _FormatedText.BuildGeometry(Location)
                _HighlightGeometry = _FormatedText.BuildHighlightGeometry(Location)
            End If
            Return _Geometry
        End Get
    End Property
    'Public Property Location As Point
    Private _Location As Point
    Public Property Location As Point
        Get
            Return _Location
        End Get
        Set(value As Point)
            _Location = value
            Change()
        End Set
    End Property
    'Public Property Stroke As Brush
    Private _Stroke As Brush = Brushes.Black
    Public Property Stroke As Brush
        Get
            Return _Stroke
        End Get
        Set(value As Brush)
            _Stroke = value
            Change()
        End Set
    End Property
    'Public Property StrokeThickness As Double
    Private _StrokeThickness As Double = 1.0#
    Public Property StrokeThickness As Double
        Get
            Return _StrokeThickness
        End Get
        Set(value As Double)
            _StrokeThickness = value
            Change()
        End Set
    End Property
End Class
