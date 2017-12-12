Public Class ColorToggleButtonBase
    Inherits Control
    'ColorToggleButtonBase->CheckedBrush As Brush Default: Brushes.Red
    Public Property CheckedBrush As Brush
        Get
            Return GetValue(CheckedBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(CheckedBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CheckedBrushProperty As DependencyProperty = _
                           DependencyProperty.Register("CheckedBrush", _
                           GetType(Brush), GetType(ColorToggleButtonBase), _
                           New PropertyMetadata(Brushes.Red))
    'ColorToggleButtonBase->UncheckedBrush As Brush Default: Brushes.White
    Public Property UncheckedBrush As Brush
        Get
            Return GetValue(UncheckedBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(UncheckedBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly UncheckedBrushProperty As DependencyProperty = _
                           DependencyProperty.Register("UncheckedBrush", _
                           GetType(Brush), GetType(ColorToggleButtonBase), _
                           New PropertyMetadata(Brushes.White))
    'ColorToggleButtonBase -> FillBrush As Brush Default: Brushes.White
    Public ReadOnly Property FillBrush As Brush
        Get
            Return GetValue(ColorToggleButtonBase.FillBrushProperty)
        End Get
    End Property
    Private Shared ReadOnly FillBrushPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("FillBrush", _
                              GetType(Brush), GetType(ColorToggleButtonBase), _
                              New FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender))
    Public Shared ReadOnly FillBrushProperty As DependencyProperty = _
                             FillBrushPropertyKey.DependencyProperty
    'ColorToggleButtonBase->Value As Boolean with Event Default: False
    Public Property Value As Boolean
        Get
            Return GetValue(ValueProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(ValueProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ValueProperty As DependencyProperty = _
                           DependencyProperty.Register("Value", _
                           GetType(Boolean), GetType(ColorToggleButtonBase), _
                           New FrameworkPropertyMetadata(False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, New PropertyChangedCallback(AddressOf SharedValueChanged)))
    'ColorToggleButtonBase->Text As String Default: ""
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
                           GetType(String), GetType(ColorToggleButtonBase), _
                           New PropertyMetadata(""))

    Private Shared Sub SharedValueChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, ColorToggleButtonBase).ValueChanged(d, e)
    End Sub
    Private Sub ValueChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.NewValue Then
            SetValue(FillBrushPropertyKey, GetValue(CheckedBrushProperty))
        Else
            SetValue(FillBrushPropertyKey, GetValue(UncheckedBrushProperty))
        End If
    End Sub
    Private WithEvents _Rect As Rectangle
    Private WithEvents _Text As TextBlock
    Public Overrides Sub OnApplyTemplate()
        _Rect = Template.FindName("_Rect", Me)
        _Text = Template.FindName("_Text", Me)
        MyBase.OnApplyTemplate()
    End Sub
    Shared Sub New()
        ColorToggleButtonBase.BorderBrushProperty.OverrideMetadata(GetType(ColorToggleButtonBase), New FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender))
    End Sub

    Private Sub _Rect_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles _Rect.MouseDown, _Text.MouseDown
        SetValue(ValueProperty, Not GetValue(ValueProperty))
        BindingOperations.GetBindingExpression(Me, ValueProperty).UpdateSource()
    End Sub
End Class
