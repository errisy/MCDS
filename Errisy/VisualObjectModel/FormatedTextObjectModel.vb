Public Class FormatedTextObjectModel
    Inherits AllocationObjectModel
    Public Overrides Sub ApplyOffset(Offset As Vector)
        Location += Offset
        Change()
    End Sub
    Public Overrides Sub Change()
        _FormatedText = Nothing
        MyBase.Change()
    End Sub
    Private _HighlightGeometry As Geometry
    Public Overrides ReadOnly Property GetSpaceGeometry As Geometry
        Get
            Return HighlightGeometry
        End Get
    End Property
    Public ReadOnly Property HighlightGeometry As Geometry
        Get
            If _HighlightGeometry Is Nothing Then
                Dim _Rect As New Rect(Location, New Size(FormatedText.Width, FormatedText.Height))
                _HighlightGeometry = New RectangleGeometry With {.Rect = _Rect}
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
    Private _FormatedText As System.Windows.Media.FormattedText
    Public ReadOnly Property FormatedText As System.Windows.Media.FormattedText
        Get
            If _FormatedText Is Nothing Then
                If _Text Is Nothing Then _Text = ""
                _FormatedText = New System.Windows.Media.FormattedText(
                     _Text,
                     System.Globalization.CultureInfo.CurrentCulture,
                     _FlowDirection,
                     New Typeface(_FontFamily, _FontStyle, _FontWeight, _FontStretch), _FontSize, _Fill)
            End If
            Return _FormatedText
        End Get
    End Property
    'Public Property Bounds As Rect?
    Private _Bounds As Rect?
    Public ReadOnly Property Bounds As Rect?
        Get
            If _Bounds Is Nothing Then
                _Bounds = New Rect(_Location, New Size(FormatedText.Width, FormatedText.Height))
            End If
            Return _Bounds
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
    'Public Property ShouldShowHighlight As Boolean
    Private _ShouldShowHighlight As Boolean = False
    Public Property ShouldShowHighlight As Boolean
        Get
            Return _ShouldShowHighlight
        End Get
        Set(value As Boolean)
            _ShouldShowHighlight = value
            Change()
        End Set
    End Property
    'Public Property HighlightFill As Brush
    Private _HighlightFill As Brush = Brushes.Yellow
    Public Property HighlightFill As Brush
        Get
            Return _HighlightFill
        End Get
        Set(value As Brush)
            _HighlightFill = value
            Change()
        End Set
    End Property
    'Public Property HighlightStroke As Brush
    Private _HighlightStroke As Brush
    Public Property HighlightStroke As Brush
        Get
            Return _HighlightStroke
        End Get
        Set(value As Brush)
            _HighlightStroke = value
            Change()
        End Set
    End Property
    'Public Property HighlightStrokeThickness As Double
    Private _HighlightStrokeThickness As Double
    Public Property HighlightStrokeThickness As Double
        Get
            Return _HighlightStrokeThickness
        End Get
        Set(value As Double)
            _HighlightStrokeThickness = value
            Change()
        End Set
    End Property


End Class
