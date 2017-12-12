Public Class MultipleValueBoxBase
    Inherits Control

    Protected _Border As Border
    Protected _Panel As Panel
    Protected WithEvents _TextBox As TextBox
    Protected WithEvents _AutoCompletePopup As AutoCompletePopup
    Private Function AreComponentsAquired() As Boolean
        If _Border Is Nothing Then Return False
        If _Panel Is Nothing Then Return False
        If _TextBox Is Nothing Then Return False
        Return True
    End Function
    Public Overrides Sub OnApplyTemplate()
        _Border = Template.FindName("_Broder", Me)
        _Panel = Template.FindName("_Panel", Me)
        _TextBox = Template.FindName("_TextBox", Me)
        _AutoCompletePopup = Template.FindName("_AutoCompletePopup", Me)
        MyBase.OnApplyTemplate()
    End Sub

    Protected Overrides Sub OnGotFocus(e As RoutedEventArgs)
        If Not AreComponentsAquired() Then Return
        _TextBox.Focus()
        MyBase.OnGotFocus(e)
    End Sub
    Public Property IsReadOnly As Boolean
        Get
            Return GetValue(IsReadOnlyProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsReadOnlyProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsReadOnlyProperty As DependencyProperty =
                           DependencyProperty.Register("IsReadOnly",
                           GetType(Boolean), GetType(MultipleValueBoxBase),
                           New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedIsReadOnlyChanged)))
    Private Shared Sub SharedIsReadOnlyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, MultipleValueBoxBase).IsReadOnlyChanged(d, e)
    End Sub
    Private Sub IsReadOnlyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.NewValue Then
            If Not _Panel.Children.Contains(_TextBox) Then _Panel.Children.Add(_TextBox)
        Else
            If _Panel.Children.Contains(_TextBox) Then _Panel.Children.Remove(_TextBox)
        End If
    End Sub

    'Shared Sub New()
    '    Control.FocusableProperty.OverrideMetadata(GetType(MultipleValueBoxBase), New PropertyMetadata(True))

    'End Sub

    Public Property ItemTemplate As DataTemplate
        Get
            Return GetValue(ItemTemplateProperty)
        End Get
        Set(ByVal value As DataTemplate)
            SetValue(ItemTemplateProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ItemTemplateProperty As DependencyProperty =
                             DependencyProperty.Register("ItemTemplate",
                             GetType(DataTemplate), GetType(MultipleValueBoxBase),
                             New PropertyMetadata(Markup.XamlReader.Parse(
<DataTemplate xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
    <Label Content="{Binding }"/>
</DataTemplate>.ToString
        )))
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
                             GetType(IEnumerable), GetType(MultipleValueBoxBase),
                             New PropertyMetadata(Nothing))
    Public Property PresentationTemplate As DataTemplate
        Get
            Return GetValue(PresentationTemplateProperty)
        End Get
        Set(ByVal value As DataTemplate)
            SetValue(PresentationTemplateProperty, value)
        End Set
    End Property
    Public Shared ReadOnly PresentationTemplateProperty As DependencyProperty =
                             DependencyProperty.Register("PresentationTemplate",
                             GetType(DataTemplate), GetType(MultipleValueBoxBase),
                             New PropertyMetadata(Markup.XamlReader.Parse(
<DataTemplate xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
    <Label Content="{Binding }"/>
</DataTemplate>.ToString
        )))
    Private Sub _TextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles _TextBox.KeyDown

    End Sub

    Private Sub _AutoCompletePopup_ItemSelected(sender As Object, e As ItemSelectedEventArgs) Handles _AutoCompletePopup.ItemSelected
        If ItemTemplate IsNot Nothing Then
            Dim element As FrameworkElement = ItemTemplate.LoadContent
            element.DataContext = e.Item
            If SelectedItems IsNot Nothing Then
                SelectedItems.Add(e.Item)
            End If
        End If
    End Sub
    Public Property SelectedItems As IList
        Get
            Return GetValue(SelectedItemsProperty)
        End Get
        Set(ByVal value As IList)
            SetValue(SelectedItemsProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SelectedItemsProperty As DependencyProperty =
                           DependencyProperty.Register("SelectedItems",
                           GetType(IList), GetType(MultipleValueBoxBase),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedSelectedItemsChanged)))
    Private Shared Sub SharedSelectedItemsChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, MultipleValueBoxBase).SelectedItemsChanged(d, e)
    End Sub
    Private Sub SelectedItemsChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If Not AreAnyTouchesCaptured Then Return
        Dim _Removal As New List(Of Object)
        For Each view In _Panel.Children
            If view IsNot _TextBox Then _Removal.Add(view)
        Next
        For Each view In _Removal
            _Panel.Children.Remove(view)
        Next
        If TypeOf e.NewValue Is IList AndAlso ItemTemplate IsNot Nothing Then
            Dim _List As IList = e.NewValue
            For Each item In _List
                Dim element As FrameworkElement = ItemTemplate.LoadContent
                element.DataContext = item
                _Panel.Children.Insert(_Panel.Children.Count - 1, element)
            Next
        End If
    End Sub
End Class
