Public MustInherit Class EditableLabelBase
    Inherits Control
    Public Sub New()
        MyBase.New()
        Focusable = True
    End Sub
    Private WithEvents _TextBox As TextBox
    Private WithEvents _AutoCompletePopup As AutoCompletePopup
    'Private WithEvents _TextBlock As TextBlock
    Public Overrides Sub OnApplyTemplate()
        _TextBox = Template.FindName("_TextBox", Me)
        If CanAutoComplete Then AddAutoComplete()
        MyBase.OnApplyTemplate()
    End Sub
    'EditableLabelBase->Text As String with Event Default: Nothing
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
                           GetType(String), GetType(EditableLabelBase), _
                           New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, New PropertyChangedCallback(AddressOf SharedTextChanged)))
    Private Shared Sub SharedTextChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, EditableLabelBase).TextChanged(d, e)
    End Sub
    Private Sub TextChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.NewValue IsNot Nothing AndAlso DirectCast(e.NewValue, String).Length > 0 Then
            SetValue(SuggestionVisibilityPropertyKey, Visibility.Hidden)
        ElseIf Not IsEditing Then
            SetValue(SuggestionVisibilityPropertyKey, Visibility.Visible)
        End If
    End Sub
    'EditableLabelBase -> SuggestionVisibility As Visibility Default: Visibility.Visible
    Public ReadOnly Property SuggestionVisibility As Visibility
        Get
            Return GetValue(EditableLabelBase.SuggestionVisibilityProperty)
        End Get
    End Property
    Private Shared ReadOnly SuggestionVisibilityPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("SuggestionVisibility", _
                              GetType(Visibility), GetType(EditableLabelBase), _
                              New PropertyMetadata(Visibility.Visible))
    Public Shared ReadOnly SuggestionVisibilityProperty As DependencyProperty = _
                             SuggestionVisibilityPropertyKey.DependencyProperty
    'EditableLabelBase->IsEditing As Boolean with Event Default: False
    Public Property IsEditing As Boolean
        Get
            Return GetValue(IsEditingProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsEditingProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsEditingProperty As DependencyProperty = _
                           DependencyProperty.Register("IsEditing", _
                           GetType(Boolean), GetType(EditableLabelBase), _
                           New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedIsEditingChanged)))
    Private Shared Sub SharedIsEditingChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, EditableLabelBase).IsEditingChanged(d, e)
    End Sub
    Private Sub IsEditingChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.NewValue Then
            SetValue(SuggestionVisibilityPropertyKey, Visibility.Hidden)
            'If _TextBox IsNot Nothing Then _TextBox.Visibility = Visibility.Visible
        ElseIf Text Is Nothing OrElse Text.Length = 0 Then
            SetValue(SuggestionVisibilityPropertyKey, Visibility.Visible)
            'If _TextBox IsNot Nothing Then _TextBox.Visibility = Visibility.Hidden
        End If
    End Sub
    Public Property CanAutoComplete As Boolean
        Get
            Return GetValue(CanAutoCompleteProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(CanAutoCompleteProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CanAutoCompleteProperty As DependencyProperty =
                           DependencyProperty.Register("CanAutoComplete",
                           GetType(Boolean), GetType(EditableLabelBase),
                           New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedCanAutoCompleteChanged)))
    Private Shared Sub SharedCanAutoCompleteChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, EditableLabelBase).CanAutoCompleteChanged(d, e)
    End Sub
    Private Sub CanAutoCompleteChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If _TextBox Is Nothing Then Return
        If e.NewValue Then
            AddAutoComplete()
        Else
            RemoveAutoComplete()
        End If
    End Sub
    Private Sub AddAutoComplete()
        _AutoCompletePopup = New AutoCompletePopup
        AutoCompletePopup.SetAutoComplete(_TextBox, _AutoCompletePopup)
        _AutoCompletePopup.SetBinding(AutoCompletePopup.ItemTemplateProperty, New Binding With {.Source = Me, .Path = New PropertyPath(AutoCompleteItemsSourceProperty)})
        _AutoCompletePopup.SetBinding(AutoCompletePopup.ItemsSourceProperty, New Binding With {.Source = Me, .Path = New PropertyPath(AutoCompleteItemsSourceProperty)})
    End Sub
    Private Sub RemoveAutoComplete()
        BindingOperations.ClearAllBindings(_AutoCompletePopup)
        AutoCompletePopup.SetAutoComplete(_TextBox, Nothing)
        _AutoCompletePopup = Nothing
    End Sub
    Public Property AutoCompleteItemsSource As IEnumerable
        Get
            Return GetValue(AutoCompleteItemsSourceProperty)
        End Get
        Set(ByVal value As IEnumerable)
            SetValue(AutoCompleteItemsSourceProperty, value)
        End Set
    End Property
    Public Shared ReadOnly AutoCompleteItemsSourceProperty As DependencyProperty =
                             DependencyProperty.Register("AutoCompleteItemsSource",
                             GetType(IEnumerable), GetType(EditableLabelBase),
                             New PropertyMetadata(Nothing))
    Public Property AutoCompleteItemTemplate As DataTemplate
        Get
            Return GetValue(AutoCompleteItemTemplateProperty)
        End Get
        Set(ByVal value As DataTemplate)
            SetValue(AutoCompleteItemTemplateProperty, value)
        End Set
    End Property
    Public Shared ReadOnly AutoCompleteItemTemplateProperty As DependencyProperty =
                             DependencyProperty.Register("AutoCompleteItemTemplate",
                             GetType(DataTemplate), GetType(EditableLabelBase),
                             New PropertyMetadata(Markup.XamlReader.Parse(
<DataTemplate xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
    <Label Content="{Binding }"/>
</DataTemplate>.ToString
        )))
    'EditableLabelBase->Suggestion As String Default: Nothing
    Public Property Suggestion As String
        Get
            Return GetValue(SuggestionProperty)
        End Get
        Set(ByVal value As String)
            SetValue(SuggestionProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SuggestionProperty As DependencyProperty = _
                           DependencyProperty.Register("Suggestion", _
                           GetType(String), GetType(EditableLabelBase), _
                           New PropertyMetadata(Nothing))
    'EditableLabelBase->SuggestionForeground As Brush Default: Brushes.Gray
    Public Property SuggestionForeground As Brush
        Get
            Return GetValue(SuggestionForegroundProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(SuggestionForegroundProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SuggestionForegroundProperty As DependencyProperty = _
                           DependencyProperty.Register("SuggestionForeground", _
                           GetType(Brush), GetType(EditableLabelBase), _
                           New PropertyMetadata(Brushes.Gray))

    Protected Overrides Sub OnPreviewMouseLeftButtonDown(e As MouseButtonEventArgs)
        If e.ClickCount >= 2 Then
            e.Handled = True
            If Not IsEditing Then IsEditing = True
            _TextBox.Focus() : _TextBox.SelectAll()
        End If
        MyBase.OnPreviewMouseLeftButtonDown(e)
    End Sub
    'Protected Overrides Sub OnLostFocus(e As RoutedEventArgs)
    '    IsEditing = False
    '    MyBase.OnLostFocus(e)
    'End Sub

    Protected Overrides Sub OnPreviewKeyDown(e As KeyEventArgs)
        If _AutoCompletePopup IsNot Nothing AndAlso _AutoCompletePopup.IsOpen Then Return
        Select Case e.Key
            Case Key.Enter
                IsEditing = False
                SetValue(TextProperty, _TextBox.Text)
            Case Key.Escape
                IsEditing = False
                _TextBox.Text = Text
        End Select
        MyBase.OnPreviewKeyDown(e)
    End Sub
    Private Sub _TextBox_LostFocus(sender As Object, e As RoutedEventArgs) Handles _TextBox.LostFocus
        IsEditing = False
        _TextBox.Text = Text
    End Sub
End Class

