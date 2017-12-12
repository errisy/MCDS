Public Class FreezableTextBox
    Inherits UserControl
    Private Shared WithEvents _TextBox As New TextBox
    Private WithEvents _TextBlock As New TextBlock
    Private _Run As New Run
    Private Shared _AutoCompletePopup As New AutoCompletePopup
    Public Sub New()
        Keyboard.AddGotKeyboardFocusHandler(Me, AddressOf WhenGotKeyboardFocus)
        'Keyboard.AddLostKeyboardFocusHandler(Me, AddressOf WhenostKeyboardFocus)
        _TextBlock.Inlines.Add(_Run)
        BuildPresentation()
    End Sub
    Private Sub WhenGotKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
        Editing = True
    End Sub
    'Private Sub WhenostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
    '    If Editing And e.NewFocus Is _TextBox Then

    '    Else
    '        Editing = False
    '    End If
    'End Sub
    Protected Overrides Sub Finalize()
        Keyboard.RemoveGotKeyboardFocusHandler(Me, AddressOf WhenGotKeyboardFocus)
        'Keyboard.RemoveLostKeyboardFocusHandler(Me, AddressOf WhenostKeyboardFocus)
        MyBase.Finalize()
    End Sub
    Protected Overrides Sub OnMouseEnter(e As MouseEventArgs)
        Focus()
        MyBase.OnMouseEnter(e)
    End Sub
    Public Property Editing As Boolean
        Get
            Return GetValue(EditingProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(EditingProperty, value)
        End Set
    End Property
    Public Shared ReadOnly EditingProperty As DependencyProperty =
                           DependencyProperty.Register("Editing",
                           GetType(Boolean), GetType(FreezableTextBox),
                           New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedEditingChanged)))
    Private Shared Sub SharedEditingChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, FreezableTextBox).EditingChanged(d, e)
    End Sub
    Private Sub EditingChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.NewValue Then
            EditingFreezableTextBox = Me
            BeginEdit()
        Else
            EndEdit()
        End If
    End Sub
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
                             GetType(String), GetType(FreezableTextBox),
                             New FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault))
    Public Property ItemsSource As IEnumerable
        Get
            Return GetValue(ItemsSourceProperty)
        End Get
        Set(ByVal value As IEnumerable)
            SetValue(ItemsSourceProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ItemsSourceProperty As DependencyProperty =
                             DependencyProperty.Register("ItemsSource",
                             GetType(IEnumerable), GetType(FreezableTextBox),
                             New PropertyMetadata(Nothing))
    Public Sub BeginEdit()

        BuildEditor()
        DestroyPresentation()

    End Sub
    Public Sub EndEdit()
        DestroyEditor()
        BuildPresentation()
    End Sub
    Private Sub BuildEditor()
        With _TextBox
            .FontSize = FontSize
            .FontStyle = FontStyle
            .FontFamily = FontFamily
            .FontWeight = FontWeight
            .FontStretch = FontStretch
            .BorderThickness = New Thickness(0#)
            .Padding = New Thickness(0.0#, 0#, 0.0#, 0#)
            .Margin = New Thickness(0.0#, 0#, -0.0#, 0#)
            .VerticalAlignment = VerticalAlignment.Center
            .HorizontalAlignment = HorizontalAlignment.Stretch
            .VerticalContentAlignment = VerticalAlignment.Center
            .Background = New SolidColorBrush(ColorConverter.ConvertFromString("#CCEFFF"))
        End With
        _TextBox.SetBinding(TextBox.ForegroundProperty, New Binding With {.Path = New PropertyPath(ForegroundProperty), .Source = Me})
        _TextBox.SetBinding(TextBox.TextProperty, New Binding With {.Path = New PropertyPath(TextProperty), .Source = Me, .Mode = BindingMode.TwoWay, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged})
        '_AutoCompletePopup = New AutoCompletePopup With {.ItemsSource = ItemsSource}
        Content = _TextBox
        _AutoCompletePopup.SetBinding(AutoCompletePopup.ItemsSourceProperty, New Binding With {.Path = New PropertyPath(ItemsSourceProperty), .Source = Me})
        'With _AutoCompletePopup
        '    .ItemsSource = ItemsSource
        '    .IsEnabled = True
        'End With
        AutoCompletePopup.SetAutoComplete(_TextBox, _AutoCompletePopup)

        _TextBox.SelectAll()
        _TextBox.Focus()
        'Keyboard.Focus(_TextBox)

        '_AutoCompletePopup.ShowFullList()
    End Sub
    Private Sub DestroyEditor()
        'With _AutoCompletePopup
        '    .IsEnabled = False
        '    .ItemsSource = Nothing
        'End With
        BindingOperations.ClearAllBindings(_AutoCompletePopup)
        BindingOperations.ClearAllBindings(_TextBox)
        AutoCompletePopup.SetAutoComplete(_TextBox, Nothing)
    End Sub
    Private Sub BuildPresentation()
        With _TextBlock
            .FontSize = FontSize
            .FontStyle = FontStyle
            .FontFamily = FontFamily
            .FontWeight = FontWeight
            .FontStretch = FontStretch
            .Foreground = Foreground
            .Padding = New Thickness(2.0#, 0#, 2.0#, 0#)
            .Margin = New Thickness(0.0#, 0#, 0.0#, 0#)
            .VerticalAlignment = VerticalAlignment.Center
            .HorizontalAlignment = HorizontalAlignment.Stretch
            .Background = New SolidColorBrush(ColorConverter.ConvertFromString("#CCEFFF"))
        End With
        _TextBlock.SetBinding(TextBlock.ForegroundProperty, New Binding With {.Path = New PropertyPath(ForegroundProperty), .Source = Me})
        _Run.SetBinding(Run.TextProperty, New Binding With {.Path = New PropertyPath(TextProperty), .Source = Me})
        Content = _TextBlock
    End Sub
    Private Sub DestroyPresentation()
        BindingOperations.ClearAllBindings(_TextBlock)
    End Sub
    Private Sub _TextBlock_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles _TextBlock.MouseLeftButtonDown
        Editing = True
    End Sub

    Private Shared _EditingFreezableTextBox As FreezableTextBox
    Private Shared Property EditingFreezableTextBox As FreezableTextBox
        Get
            Return _EditingFreezableTextBox
        End Get
        Set(value As FreezableTextBox)
            If _EditingFreezableTextBox IsNot Nothing Then _EditingFreezableTextBox.Editing = False
            _EditingFreezableTextBox = value
            If _EditingFreezableTextBox IsNot Nothing Then _EditingFreezableTextBox.Editing = True
        End Set
    End Property
    Shared Sub New()
        FocusableProperty.OverrideMetadata(GetType(FreezableTextBox), New FrameworkPropertyMetadata(True))
        IsTabStopProperty.OverrideMetadata(GetType(FreezableTextBox), New FrameworkPropertyMetadata(True))
    End Sub

End Class
