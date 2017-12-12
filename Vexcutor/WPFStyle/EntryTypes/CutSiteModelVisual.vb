Imports System.ComponentModel
Imports System.Windows, System.Windows.Media, System.Windows.Data, System.Windows.Input

Public Structure VerticalTriangle
    Public PointerLocation As Point
    Public DirectionalHeight As Double
    Public Width As Double
    Public ReadOnly Property RightPointer As Point
        Get
            Return New Point(PointerLocation.X + Width / 2.0#, PointerLocation.Y + DirectionalHeight)
        End Get
    End Property
    Public ReadOnly Property LeftPointer As Point
        Get
            Return New Point(PointerLocation.X - Width / 2.0#, PointerLocation.Y + DirectionalHeight)
        End Get
    End Property
End Structure
Public Class CutSitePanelVisual
    Inherits PanelVisual
    Private WithEvents _SiteDef As ICutSite
    Public Property SiteDef As ICutSite
        Get
            Return _SiteDef
        End Get
        Set(ByVal value As ICutSite)
            _SiteDef = value
            OnRender()
        End Set
    End Property

    Private Sub _SiteDef_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _SiteDef.PropertyChanged
        If e.PropertyName = "Sequence" Then
            OnRender()
        End If
    End Sub
    Private _TopIndicators As New List(Of CutSiteIndicator)
    Private _BottomIndicators As New List(Of CutSiteIndicator)
    Private _TopNucleotides As New List(Of TextVisual)
    Private _BottomNucleotides As New List(Of TextVisual)
    Protected Overrides Sub OnRender()
        _Visuals.Clear()
        _TopIndicators.Clear()
        _BottomIndicators.Clear()
        _TopNucleotides.Clear()
        _BottomNucleotides.Clear()
        If _SiteDef Is Nothing Then Return
        Dim _Sequence As String = _SiteDef.Sequence
        Dim _SCut As Integer = _SiteDef.SCut
        Dim _ACut As Integer = _SiteDef.ACut
        If _Sequence IsNot Nothing Then
            Dim _Length As Integer = _Sequence.Length
            Dim _FontFamily As New FontFamily("Arial")
            Dim _OffsetX As Double = 12.0#
            Dim _OffsetY As Double = 16.0#
            Dim _FontWidth As Double = 16.0#
            Dim _FontHeight As Double = 20.0#
            For i As Integer = 0 To _Length - 1
                Dim _SN As New TextVisual With {.Foreground = Brushes.Black, .Text = _Sequence(i), .FontFamily = _FontFamily, .FontWeight = FontWeights.Bold, .FontSize = 18.0#, .Location = New Point(_OffsetX + _FontWidth * i, _OffsetY)}
                Dim _AN As New TextVisual With {.Foreground = Brushes.Black, .Text = Nuctions.ComplementChar(_Sequence(i)), .FontFamily = _FontFamily, .FontWeight = FontWeights.Bold, .FontSize = 18.0#, .Location = New Point(_OffsetX + _FontWidth * i, _OffsetY + _FontHeight)}
                _TopNucleotides.Add(_SN)
                _BottomNucleotides.Add(_AN)
                _Visuals.Add(_SN)
                _Visuals.Add(_AN)
            Next
            For i As Integer = 0 To _Length
                Dim _S As New CutSiteIndicator With {.Fill = IIf(i = _SCut, Brushes.Red, Brushes.LightGray), .Selected = AddressOf TopIndicatorSelected, .Triangle = New VerticalTriangle With {.PointerLocation = New Point(_OffsetX - 1.0# + _FontWidth * i, _OffsetY), .Width = 8.0#, .DirectionalHeight = -10.0#}}
                Dim _A As New CutSiteIndicator With {.Fill = IIf(i = _ACut, Brushes.Red, Brushes.LightGray), .Selected = AddressOf BottomIndicatorSelected, .Triangle = New VerticalTriangle With {.PointerLocation = New Point(_OffsetX - 1.0# + _FontWidth * i, _OffsetY + _FontHeight * 2.0#), .Width = 8.0#, .DirectionalHeight = 10.0#}}
                _TopIndicators.Add(_S)
                _BottomIndicators.Add(_A)
                _Visuals.Add(_S)
                _Visuals.Add(_A)
            Next
        End If
    End Sub
    Private Sub TopIndicatorSelected(_Indicator As CutSiteIndicator)
        If _SiteDef Is Nothing Then Return
        Dim _SCut As Integer = _SiteDef.SCut
        If _SCut > -1 AndAlso _SCut < _TopIndicators.Count Then _TopIndicators(_SCut).Fill = Brushes.LightGray
        _SiteDef.SCut = _TopIndicators.IndexOf(_Indicator)
        _Indicator.Fill = Brushes.Red
    End Sub
    Private Sub BottomIndicatorSelected(_Indicator As CutSiteIndicator)
        If _SiteDef Is Nothing Then Return
        Dim _ACut As Integer = _SiteDef.ACut
        If _ACut > -1 AndAlso _ACut < _BottomIndicators.Count Then _BottomIndicators(_ACut).Fill = Brushes.LightGray
        _SiteDef.ACut = _BottomIndicators.IndexOf(_Indicator)
        _Indicator.Fill = Brushes.Red
    End Sub
End Class
Public Class CutSiteIndicator
    Inherits ShapeVisual
    Public Property Triangle As VerticalTriangle
        Get
            Return GetValue(TriangleProperty)
        End Get
        Set(ByVal value As VerticalTriangle)
            SetValue(TriangleProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TriangleProperty As DependencyProperty =
                           DependencyProperty.Register("Triangle",
                           GetType(VerticalTriangle), GetType(CutSiteIndicator),
                           New PropertyMetadata(New VerticalTriangle, New PropertyChangedCallback(AddressOf SharedTriangleChanged)))
    Private Shared Sub SharedTriangleChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, CutSiteIndicator).TriangleChanged(d, e)
    End Sub
    Private Sub TriangleChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim _Geometry As New StreamGeometry
        Dim _Tri = Triangle
        Using context = _Geometry.Open
            context.BeginFigure(_Tri.PointerLocation, True, True)
            context.LineTo(_Tri.LeftPointer, True, False)
            context.LineTo(_Tri.RightPointer, True, False)
        End Using
        SetValue(GeometryProperty, _Geometry)
    End Sub

    Public Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        If _Selected Is Nothing Then Return
        _Selected.Invoke(Me)
    End Sub
    Public Property Selected As Action(Of CutSiteIndicator)
End Class