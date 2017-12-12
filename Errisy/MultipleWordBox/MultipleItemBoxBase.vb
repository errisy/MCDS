Public Class MultipleItemBoxBase
    Inherits Control
    'this is a control that allows user to select multiple items from a source list.
    '它可以在指定数据和关联时使用
    Private WithEvents _WrapPanel As WrapPanel
    Private _TextBox As TextBox
    Private _AutoComplete As AutoCompletePopup
    Public Overrides Sub OnApplyTemplate()
        _WrapPanel = Template.FindName("_WrapPanel", Me)
        _TextBox = Template.FindName("_TextBox", Me)
        _AutoComplete = Template.FindName("_AutoComplete", Me)
        MyBase.OnApplyTemplate()
    End Sub
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
                           GetType(IEnumerable), GetType(MultipleItemBoxBase),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedItemsSourceChanged)))
    Private Shared Sub SharedItemsSourceChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, MultipleItemBoxBase).ItemsSourceChanged(d, e)
    End Sub
    Private Sub ItemsSourceChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)

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
                             GetType(IList), GetType(MultipleItemBoxBase),
                             New PropertyMetadata(Nothing))
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
                             GetType(DataTemplate), GetType(MultipleItemBoxBase),
                             New PropertyMetadata(Markup.XamlReader.Parse(
<DataTemplate xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
    <Label Content="{Binding }"/>
</DataTemplate>.ToString
        )))
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
                             GetType(DataTemplate), GetType(MultipleItemBoxBase),
                             New PropertyMetadata(Markup.XamlReader.Parse(
<DataTemplate xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
    <Label Content="{Binding }"/>
</DataTemplate>.ToString
        )))
    Public Property ItemContainerTemplate As DataTemplate
        Get
            Return GetValue(ItemContainerTemplateProperty)
        End Get
        Set(ByVal value As DataTemplate)
            SetValue(ItemContainerTemplateProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ItemContainerTemplateProperty As DependencyProperty =
                             DependencyProperty.Register("ItemContainerTemplate",
                             GetType(DataTemplate), GetType(MultipleItemBoxBase),
                             New PropertyMetadata(Nothing))

End Class