
Public Class FormatedMultipleTextObjectModel(Of T As AllocatedColorText)
    Inherits FormatedMultipleTextObjectModelBase
    Public Sub Add(item As T)
        item.Parent = Me
        _Texts.Add(item)
    End Sub
    'Public ReadOnly Property AllocatedColorTexts As System.Collections.ObjectModel.ObservableCollection(Of T)
    '    Get
    '        Return _Texts
    '    End Get
    'End Property

    Private _HighlightGeometry As Geometry
    Public Overrides ReadOnly Property GetSpaceGeometry As Geometry
        Get
            Return HighlightGeometry
        End Get
    End Property
    Public ReadOnly Property HighlightGeometry As Geometry
        Get
            If _HighlightGeometry Is Nothing Then
                _HighlightGeometry = PathGeometry.Empty
                For Each txt In _Texts
                    Dim hGeo = txt.HighlightGeometry
                    _HighlightGeometry = PathGeometry.Combine(_HighlightGeometry, hGeo, GeometryCombineMode.Union, Nothing)
                Next
            End If
            Return _HighlightGeometry
        End Get
    End Property

    Public Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        For Each t In _Texts
            t.TestMouseDown(e)
        Next
    End Sub
    Public Overrides Sub OnMouseUp(e As MouseButtonEventArgs)
        For Each t In _Texts
            t.TestMouseUp(e)
        Next
    End Sub
    Public Overrides Sub OnMouseMove(e As MouseEventArgs)
        For Each t In _Texts
            t.TestMouseMove(e)
        Next
    End Sub
    Public Overrides Sub OnMouseWheel(e As MouseWheelEventArgs)
        For Each t In _Texts
            t.TestMouseWheel(e)
        Next
    End Sub
End Class
Public MustInherit Class FormatedMultipleTextObjectModelBase
    Inherits AllocationObjectModel
    Public Overrides Sub ApplyOffset(Offset As Vector)
        For Each txt In _Texts
            txt.Location += Offset
        Next
        Change()
    End Sub

    Protected WithEvents _Texts As New System.Collections.ObjectModel.ObservableCollection(Of AllocatedColorText)
    Private Sub _Texts_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs) Handles _Texts.CollectionChanged
        Change()
    End Sub
    Public Overrides Sub Change()
        _AllocatedColorText = Nothing
        MyBase.Change()
    End Sub
    'Public Property FormatedText As System.Windows.Media.FormattedText
    Private _AllocatedColorText As List(Of AllocatedColorText)
    Public ReadOnly Property AllocatedColorTexts As IEnumerable(Of AllocatedColorText)
        Get
            Return _Texts
        End Get
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

Public Class AllocatedColorText
    'Public Property Parent As FormatedMultipleTextObjectModel
    Private _Parent As FormatedMultipleTextObjectModelBase
    Public Property Parent As FormatedMultipleTextObjectModelBase
        Get
            Return _Parent
        End Get
        Set(value As FormatedMultipleTextObjectModelBase)
            _Parent = value
        End Set
    End Property
    'Public Property Text As String
    Private _Text As String
    Public Property Text As String
        Get
            Return _Text
        End Get
        Set(value As String)
            _Text = value
            If _Parent IsNot Nothing Then _Parent.Change()
        End Set
    End Property
    'Public Property Color As Brush
    Private _Color As Brush = Brushes.Black
    Public Property Color As Brush
        Get
            Return _Color
        End Get
        Set(value As Brush)
            _Color = value
            If _Parent IsNot Nothing Then _Parent.Change()
        End Set
    End Property
    'Public Property Location As Point
    Private _Location As Point
    Public Property Location As Point
        Get
            Return _Location
        End Get
        Set(value As Point)
            _Location = value
            If _Parent IsNot Nothing Then _Parent.Change()
        End Set
    End Property
    Private _FormatedText As System.Windows.Media.FormattedText
    Public ReadOnly Property FormatedText As System.Windows.Media.FormattedText
        Get
            If _FormatedText Is Nothing Then
                If _Parent Is Nothing Then Return Nothing
                If _Text Is Nothing Then _Text = ""
                _FormatedText = New System.Windows.Media.FormattedText(
                     _Text,
                     System.Globalization.CultureInfo.CurrentCulture,
                     _Parent.FlowDirection,
                     New Typeface(_Parent.FontFamily, _Parent.FontStyle, _Parent.FontWeight, _Parent.FontStretch), _Parent.FontSize, _Color)
            End If
            Return _FormatedText
        End Get
    End Property
    'Public Property Bounds As Rect?
    Private _Bounds As Rect?
    Public ReadOnly Property Bounds As Rect?
        Get
            If _Bounds Is Nothing Then
                If FormatedText Is Nothing Then Return Nothing
                _Bounds = New Rect(_Location, New Size(FormatedText.Width, FormatedText.Height))
            End If
            Return _Bounds
        End Get
    End Property
    Private _HighlightGeometry As Geometry
    Public ReadOnly Property GetSpaceGeometry As Geometry
        Get
            Return HighlightGeometry
        End Get
    End Property
    Public ReadOnly Property HighlightGeometry As Geometry
        Get
            If _HighlightGeometry Is Nothing Then
                Dim _Rect As New Rect(Location, New Size(FormatedText.Width, FormatedText.Height))
                _HighlightGeometry = New RectangleGeometry With {.Rect = Bounds}
            End If
            Return _HighlightGeometry
        End Get
    End Property
    Public Sub TestMouseDown(e As MouseButtonEventArgs)
        If GetSpaceGeometry Is Nothing Then Return
        If GetSpaceGeometry.FillContains(e.GetPosition(e.Source), 1.0#, ToleranceType.Absolute) Then OnMouseDown(e)
    End Sub
    Public Sub TestMouseUp(e As MouseButtonEventArgs)
        If GetSpaceGeometry Is Nothing Then Return
        If GetSpaceGeometry.FillContains(e.GetPosition(e.Source), 1.0#, ToleranceType.Absolute) Then OnMouseUp(e)
    End Sub
    Public Sub TestMouseMove(e As MouseEventArgs)
        If GetSpaceGeometry Is Nothing Then Return
        If GetSpaceGeometry.FillContains(e.GetPosition(e.Source), 1.0#, ToleranceType.Absolute) Then OnMouseMove(e)
    End Sub
    Public Sub TestMouseWheel(e As MouseWheelEventArgs)
        If GetSpaceGeometry Is Nothing Then Return
        If GetSpaceGeometry.FillContains(e.GetPosition(e.Source), 1.0#, ToleranceType.Absolute) Then OnMouseWheel(e)
    End Sub
    Public Event MouseDown(sender As AllocatedColorText, e As MouseButtonEventArgs)
    Public Event MouseUp(sender As AllocatedColorText, e As MouseButtonEventArgs)
    Public Event MouseMove(sender As AllocatedColorText, e As MouseEventArgs)
    Public Event MouseWheel(sender As AllocatedColorText, e As MouseWheelEventArgs)
    Public Overridable Sub OnMouseDown(e As MouseButtonEventArgs)
        RaiseEvent MouseDown(Me, e)
    End Sub
    Public Overridable Sub OnMouseUp(e As MouseButtonEventArgs)
        RaiseEvent MouseUp(Me, e)
    End Sub
    Public Overridable Sub OnMouseMove(e As MouseEventArgs)
        RaiseEvent MouseMove(Me, e)
    End Sub
    Public Overridable Sub OnMouseWheel(e As MouseWheelEventArgs)
        RaiseEvent MouseWheel(Me, e)
    End Sub
End Class
 