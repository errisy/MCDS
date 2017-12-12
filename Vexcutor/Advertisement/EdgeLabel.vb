
Imports System.Windows.Markup, System.Windows, System.Windows.Data, System.Windows.Media, System.Windows.Controls, System.Windows.Shapes

<ContentProperty("Content")>
Public Class EdgeLabel
    Inherits Shape
    Public Sub New()
        MyBase.New()
        Dim dpd As System.ComponentModel.DependencyPropertyDescriptor = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(FlowDirectionProperty, GetType(EdgeLabel))
        dpd.AddValueChanged(Me, AddressOf FlowDirectionChanged)

    End Sub
    Private Sub FlowDirectionChanged(sender As Object, e As EventArgs)
        Redraw()
    End Sub
    'EdgeLabel->Content As String with Event Default: ""
    Public Property Content As String
        Get
            Return GetValue(ContentProperty)
        End Get
        Set(ByVal value As String)
            SetValue(ContentProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ContentProperty As DependencyProperty = _
                           DependencyProperty.Register("Content", _
                           GetType(String), GetType(EdgeLabel), _
                           New FrameworkPropertyMetadata("", 31, New PropertyChangedCallback(AddressOf SharedContentChanged)))
    Private Shared Sub SharedContentChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, EdgeLabel).ContentChanged(d, e)
    End Sub
    Private Sub ContentChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If TypeOf e.NewValue Is String Then
            Redraw()
        End If
    End Sub
    'EdgeLabel->Foreground As Brush with Event Default: Brushes.Black
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
                           GetType(Brush), GetType(EdgeLabel), _
                           New FrameworkPropertyMetadata(Brushes.Black, 31, New PropertyChangedCallback(AddressOf SharedForegroundChanged)))
    Private Shared Sub SharedForegroundChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, EdgeLabel).ForegroundChanged(d, e)
    End Sub
    Private Sub ForegroundChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Redraw()
    End Sub
    'EdgeLabel->FontFamily As String with Event Default: "Arial"
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
                           GetType(FontFamily), GetType(EdgeLabel), _
                           New FrameworkPropertyMetadata(New FontFamily("Arial"), 31, New PropertyChangedCallback(AddressOf SharedFontFamilyChanged)))
    Private Shared Sub SharedFontFamilyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, EdgeLabel).FontFamilyChanged(d, e)
    End Sub
    Private Sub FontFamilyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Redraw()
    End Sub
    'EdgeLabel->FontSize As Double with Event Default: 16#
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
                           GetType(Double), GetType(EdgeLabel), _
                           New FrameworkPropertyMetadata(16.0#, 31, New PropertyChangedCallback(AddressOf SharedFontSizeChanged)))
    Private Shared Sub SharedFontSizeChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, EdgeLabel).FontSizeChanged(d, e)
    End Sub
    Private Sub FontSizeChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Redraw()
    End Sub
    'EdgeLabel->FontWeight As FontWeight with Event Default: FontWeights.Normal
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
                           GetType(FontWeight), GetType(EdgeLabel), _
                           New FrameworkPropertyMetadata(FontWeights.Normal, 31, New PropertyChangedCallback(AddressOf SharedFontWeightChanged)))
    Private Shared Sub SharedFontWeightChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, EdgeLabel).FontWeightChanged(d, e)
    End Sub
    Private Sub FontWeightChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Redraw()
    End Sub
    'EdgeLabel->FontStyle As FontStyle with Event Default: FontStyles.Normal
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
                           GetType(FontStyle), GetType(EdgeLabel), _
                           New FrameworkPropertyMetadata(FontStyles.Normal, 31, New PropertyChangedCallback(AddressOf SharedFontStyleChanged)))
    Private Shared Sub SharedFontStyleChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, EdgeLabel).FontStyleChanged(d, e)
    End Sub
    Private Sub FontStyleChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Redraw()
    End Sub
    'EdgeLabel->FontStretch As FontStretch with Event Default: FontStretches.Normal
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
                           GetType(FontStretch), GetType(EdgeLabel), _
                           New FrameworkPropertyMetadata(FontStretches.Normal, 31, New PropertyChangedCallback(AddressOf SharedFontStretchChanged)))
    Private Shared Sub SharedFontStretchChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, EdgeLabel).FontStretchChanged(d, e)
    End Sub
    Private Sub FontStretchChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Redraw()
    End Sub
    'EdgeLabel->VerticalLocation As VerticalAlignment with Event Default: VerticalAlignment.Top
    Public Property VerticalLocation As VerticalAlignment
        Get
            Return GetValue(VerticalLocationProperty)
        End Get
        Set(ByVal value As VerticalAlignment)
            SetValue(VerticalLocationProperty, value)
        End Set
    End Property
    Public Shared ReadOnly VerticalLocationProperty As DependencyProperty = _
                           DependencyProperty.Register("VerticalLocation", _
                           GetType(VerticalAlignment), GetType(EdgeLabel), _
                           New FrameworkPropertyMetadata(VerticalAlignment.Top, 31, New PropertyChangedCallback(AddressOf SharedVerticalLocationChanged)))
    Private Shared Sub SharedVerticalLocationChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, EdgeLabel).VerticalLocationChanged(d, e)
    End Sub
    Private Sub VerticalLocationChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Redraw()
    End Sub
    'EdgeLabel->HorizontalLocation As HorizontalAlignment with Event Default: HorizontalAlignment.Left
    Public Property HorizontalLocation As HorizontalAlignment
        Get
            Return GetValue(HorizontalLocationProperty)
        End Get
        Set(ByVal value As HorizontalAlignment)
            SetValue(HorizontalLocationProperty, value)
        End Set
    End Property
    Public Shared ReadOnly HorizontalLocationProperty As DependencyProperty = _
                           DependencyProperty.Register("HorizontalLocation", _
                           GetType(HorizontalAlignment), GetType(EdgeLabel), _
                           New FrameworkPropertyMetadata(HorizontalAlignment.Left, 31, New PropertyChangedCallback(AddressOf SharedHorizontalLocationChanged)))
    Private Shared Sub SharedHorizontalLocationChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, EdgeLabel).HorizontalLocationChanged(d, e)
    End Sub
    Private Sub HorizontalLocationChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Redraw()
    End Sub


    Private Sub Redraw()
        Using x = Dispatcher.DisableProcessing
            Dim ft As New System.Windows.Media.FormattedText(CStr(GetValue(ContentProperty)),
                                                             System.Globalization.CultureInfo.CurrentCulture,
                                                             GetValue(FlowDirectionProperty),
                                                            New System.Windows.Media.Typeface(GetValue(FontFamilyProperty), GetValue(FontStyleProperty), GetValue(FontWeightProperty), GetValue(FontStretchProperty)),
                                                            GetValue(FontSizeProperty),
                                                           GetValue(ForegroundProperty))
            dGeometry.Clear()
            Dim oX As Double = 0.0#
            Dim oY As Double = 0.0#
            Select Case HorizontalLocation
                Case Windows.HorizontalAlignment.Left
                Case Windows.HorizontalAlignment.Center
                    oX = -ft.Width / 2
                Case Windows.HorizontalAlignment.Right
                    oX = -ft.Width
                Case Windows.HorizontalAlignment.Stretch

            End Select
            Select Case VerticalLocation
                Case Windows.VerticalAlignment.Top
                Case Windows.VerticalAlignment.Center
                    oY = -ft.Height / 2
                Case Windows.VerticalAlignment.Bottom
                    oY = -ft.Height
                Case Windows.VerticalAlignment.Stretch

            End Select
            dGeometry.AddGeometry(ft.BuildGeometry(New Point(oX, oY)))
        End Using
    End Sub
    Private dGeometry As New System.Windows.Media.PathGeometry
    Protected Overrides ReadOnly Property DefiningGeometry As System.Windows.Media.Geometry
        Get
            Return dGeometry
        End Get
    End Property
End Class

