<Markup.ContentProperty("Content")>
Public Class ExpanderBase
    Inherits Control
    Private _ContentRow As RowDefinition
    Private WithEvents _ExpanderButton As ExpanderButton

    Protected Overrides Sub OnInitialized(e As EventArgs)
        MyBase.OnInitialized(e)
    End Sub
    Public Overrides Sub OnApplyTemplate()
        _ContentRow = Template.FindName("_ContentRow", Me)
        _ExpanderButton = Template.FindName("_ExpanderButton", Me)
        If IsExpanded Then
            _ContentRow.Height = New GridLength(0, GridUnitType.Auto)
        Else
            _ContentRow.Height = New GridLength(0, GridUnitType.Pixel)
        End If
        MyBase.OnApplyTemplate()
    End Sub
    Public Property Content As Object
        Get
            Return GetValue(ContentProperty)
        End Get
        Set(ByVal value As Object)
            SetValue(ContentProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ContentProperty As DependencyProperty =
                           DependencyProperty.Register("Content",
                           GetType(Object), GetType(ExpanderBase),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedContentChanged)))
    Private Shared Sub SharedContentChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, ExpanderBase).ContentChanged(d, e)
    End Sub
    Private Sub ContentChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.OldValue IsNot Nothing Then RemoveLogicalChild(e.OldValue)
        If e.NewValue IsNot Nothing Then AddLogicalChild(e.NewValue)
    End Sub
    Public Property Header As Object
        Get
            Return GetValue(HeaderProperty)
        End Get
        Set(ByVal value As Object)
            SetValue(HeaderProperty, value)
        End Set
    End Property
    Public Shared ReadOnly HeaderProperty As DependencyProperty =
                           DependencyProperty.Register("Header",
                           GetType(Object), GetType(ExpanderBase),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedHeaderChanged)))
    Private Shared Sub SharedHeaderChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, ExpanderBase).HeaderChanged(d, e)
    End Sub
    Private Sub HeaderChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.OldValue IsNot Nothing Then RemoveLogicalChild(e.OldValue)
        If e.NewValue IsNot Nothing Then AddLogicalChild(e.NewValue)
    End Sub
    Public Property IsExpanded As Boolean
        Get
            Return GetValue(IsExpandedProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsExpandedProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsExpandedProperty As DependencyProperty =
                           DependencyProperty.Register("IsExpanded",
                           GetType(Boolean), GetType(ExpanderBase),
                           New PropertyMetadata(True, New PropertyChangedCallback(AddressOf SharedIsExpandedChanged)))
    Private Shared Sub SharedIsExpandedChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, ExpanderBase).IsExpandedChanged(d, e)
    End Sub
    Private Sub IsExpandedChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.NewValue Then
            _ContentRow.Height = New GridLength(0, GridUnitType.Auto)
        Else
            _ContentRow.Height = New GridLength(0, GridUnitType.Pixel)
        End If
    End Sub
    Private Sub _ExpanderButton_Click(sender As Object, e As RoutedEventArgs) Handles _ExpanderButton.Click
        If _ExpanderButton IsNot Nothing Then
            If _ExpanderButton.IsChecked Then
                _ContentRow.Height = New GridLength(0, GridUnitType.Auto)
            Else
                _ContentRow.Height = New GridLength(0, GridUnitType.Pixel)
            End If
        End If
    End Sub
    'Shared Sub New()
    '    Control.TemplateProperty.OverrideMetadata(GetType(Expander), New FrameworkPropertyMetadata(
    '                                              Markup.XamlReader.Parse(
    '    <ControlTemplate
    '        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    '        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    '        xmlns:e="clr-namespace:Errisy">
    '        <Grid>
    '            <Grid.RowDefinitions>
    '                <RowDefinition Height="20"/>
    '                <RowDefinition Height="Auto" Name="_ContentRow"/>
    '            </Grid.RowDefinitions>
    '            <Grid Grid.Row="0">
    '                <Grid.ColumnDefinitions>
    '                    <ColumnDefinition Width="20"/>
    '                    <ColumnDefinition Width="Auto"/>
    '                </Grid.ColumnDefinitions>
    '                <e:ExpanderButton IsChecked="{TemplateBinding IsExpanded}"/>
    '                <ContentPresenter Grid.Column="1" ContentSource="Header"/>
    '            </Grid>
    '            <ContentPresenter Grid.Row="1" ContentSource="Content"/>
    '        </Grid>
    '    </ControlTemplate>.ToString)))
    'End Sub
End Class
