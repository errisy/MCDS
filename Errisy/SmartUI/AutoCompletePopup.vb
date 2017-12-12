Imports System.Windows, System.Windows.Controls, System.Windows.Controls.Primitives, System.Windows.Input, System.Windows.Data

Public Class AutoCompletePopup
    Inherits System.Windows.Controls.Primitives.Popup
    Protected Friend vContainer As New ScrollViewer With {.CanContentScroll = True, .HorizontalScrollBarVisibility = ScrollBarVisibility.Auto}
    Protected Friend WithEvents vPanel As New ListBox
    Public Sub New()
        MyBase.New()
        StaysOpen = False
        'vPanel.SetBinding(ItemsControl.ItemTemplateProperty, New Binding With {.Source = Me, .Path = New PropertyPath(AutoCompletePopup.ItemTemplateProperty), .Mode = BindingMode.TwoWay, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged})
        VirtualizingStackPanel.SetIsVirtualizing(vPanel, True)
        Child = vContainer
        vContainer.Content = vPanel
    End Sub
    'AutoCompletePopup->ItemsTemplate As DataTemplate with Event Default: Nothing
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
                           GetType(DataTemplate), GetType(AutoCompletePopup),
                           New PropertyMetadata(Markup.XamlReader.Parse(
<DataTemplate xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
    <Label Content="{Binding }"/>
</DataTemplate>.ToString), New PropertyChangedCallback(AddressOf SharedItemTemplateChanged)))
    Private Shared Sub SharedItemTemplateChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, AutoCompletePopup).ItemTemplateChanged(d, e)
    End Sub
    Private Sub ItemTemplateChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        vPanel.ItemTemplate = e.NewValue
    End Sub
    'AutoCompletePopup->Values As IEnumerable with Event Default: Nothing
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
                           GetType(IEnumerable), GetType(AutoCompletePopup),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedValuesChanged)))
    Private Shared Sub SharedValuesChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, AutoCompletePopup).ValuesChanged(d, e)
    End Sub
    Private Sub ValuesChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.NewValue Is Nothing Then
            vPanel.ItemsSource = Nothing
        End If
    End Sub
    'AutoCompletePopup->Filter As Func(Of Object, Boolean) with Event Default: Nothing
    'Public Property Filter As Func(Of Object, Boolean)
    '    Get
    '        Return GetValue(FilterProperty)
    '    End Get
    '    Set(ByVal value As Func(Of Object, Boolean))
    '        SetValue(FilterProperty, value)
    '    End Set
    'End Property
    'Public Shared ReadOnly FilterProperty As DependencyProperty =
    '                       DependencyProperty.Register("Filter",
    '                       GetType(Func(Of Object, Boolean)), GetType(AutoCompletePopup),
    '                       New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedFilterChanged)))
    'Private Shared Sub SharedFilterChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    '    DirectCast(d, AutoCompletePopup).FilterChanged(d, e)
    'End Sub
    'Private Sub FilterChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    '    Dim vList As New List(Of Object)
    '    Dim enl As IEnumerable = ItemsSource
    '    If TypeOf e.NewValue Is Func(Of Object, Boolean) Then
    '        Dim f As Func(Of Object, Boolean) = e.NewValue
    '        If TypeOf enl Is IEnumerable Then
    '            For Each obj In enl
    '                If f.Invoke(obj) Then
    '                    vList.Add(obj)
    '                End If
    '            Next
    '        End If
    '        vPanel.ItemsSource = vList
    '        If vPanel.Items.Count > 0 Then vPanel.SelectedIndex = 0
    '    Else
    '        vPanel.ItemsSource = enl
    '        If vPanel.Items.Count > 0 Then vPanel.SelectedIndex = 0
    '    End If
    'End Sub

    Private Sub UpdateFilter(_Filer As Func(Of Object, Boolean))
        Dim vList As New List(Of Object)
        Dim enl As IEnumerable = ItemsSource
        If TypeOf _Filer Is Func(Of Object, Boolean) Then
            Dim f As Func(Of Object, Boolean) = _Filer
            If TypeOf enl Is IEnumerable Then
                For Each obj In enl
                    If f.Invoke(obj) Then
                        vList.Add(obj)
                    End If
                Next
            End If
            vPanel.ItemsSource = vList
            If vPanel.Items.Count > 0 Then vPanel.SelectedIndex = 0
        Else
            vPanel.ItemsSource = enl
            If vPanel.Items.Count > 0 Then vPanel.SelectedIndex = 0
        End If
    End Sub
    'AutoCompletePopup->TextFilter As String with Event Default: ""
    'Public Property TextFilter As String
    '    Get
    '        Return GetValue(TextFilterProperty)
    '    End Get
    '    Set(ByVal value As String)
    '        SetValue(TextFilterProperty, value)
    '    End Set
    'End Property
    'Public Shared ReadOnly TextFilterProperty As DependencyProperty =
    '                       DependencyProperty.Register("TextFilter",
    '                       GetType(String), GetType(AutoCompletePopup),
    '                       New PropertyMetadata("", New PropertyChangedCallback(AddressOf SharedTextFilterChanged)))
    'Private Shared Sub SharedTextFilterChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    '    DirectCast(d, AutoCompletePopup).TextFilterChanged(d, e)
    'End Sub
    'Private Sub TextFilterChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)

    'End Sub
    Public Sub Up()
        If vPanel.HasItems Then
            If vPanel.SelectedIndex > 0 Then
                vPanel.SelectedIndex -= 1
            End If
        End If
    End Sub
    Public Sub Down()
        If vPanel.HasItems Then
            If vPanel.SelectedIndex < vPanel.Items.Count - 1 Then
                vPanel.SelectedIndex += 1
            End If
        End If
    End Sub
    Public Sub SelectItem()
        If TypeOf ConnectedTextBox Is TextBox Then
            ConnectedTextBox.Text = vPanel.SelectedItem.ToString
            ConnectedTextBox.SelectionStart = ConnectedTextBox.Text.Length
            IsOpen = False
            'Keyboard.RemovePreviewKeyDownHandler(Me, AddressOf TextBox_KeyDown)
            RaiseEvent ItemSelected(Me, New ItemSelectedEventArgs(vPanel.SelectedItem))
        End If
    End Sub
    Public Event ItemSelected(sender As Object, e As ItemSelectedEventArgs)

    Protected Overrides Sub OnKeyDown(e As System.Windows.Input.KeyEventArgs)
        Select Case e.Key
            Case Key.Enter
                SelectItem()
        End Select
        MyBase.OnKeyDown(e)
    End Sub

    Public Shared Function GetAutoComplete(ByVal element As TextBox) As AutoCompletePopup
        Return element.GetValue(AutoCompleteProperty)
    End Function

    Public Shared Sub SetAutoComplete(ByVal element As TextBox, ByVal value As AutoCompletePopup)
        element.SetValue(AutoCompleteProperty, value)
    End Sub

    Public Shared ReadOnly AutoCompleteProperty As _
                           DependencyProperty = DependencyProperty.RegisterAttached("AutoComplete",
                           GetType(AutoCompletePopup), GetType(AutoCompletePopup),
                           New PropertyMetadata(Nothing, AddressOf AutoCompleteChanged))
    Private Shared Sub AutoCompleteChanged(sender As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If Not (TypeOf sender Is TextBox) Then Return
        Dim _TextBox As TextBox = sender
        If TypeOf e.OldValue Is AutoCompletePopup Then
            Dim _old As AutoCompletePopup = e.OldValue
            RemoveHandler _TextBox.PreviewKeyDown, AddressOf _old.TextBox_KeyDown
            RemoveHandler _TextBox.TextChanged, AddressOf _old.TextBox_TextChanged
            _old._ConnectedTextBox = Nothing
            BindingOperations.ClearBinding(_old, DataContextProperty)
        End If
        If TypeOf e.NewValue Is AutoCompletePopup Then
            Dim _new As AutoCompletePopup = e.NewValue
            AddHandler _TextBox.PreviewKeyDown, AddressOf _new.TextBox_KeyDown
            AddHandler _TextBox.TextChanged, AddressOf _new.TextBox_TextChanged
            _new.PlacementTarget = _TextBox
            _new._ConnectedTextBox = _TextBox
            _new.SetBinding(DataContextProperty, New Binding With {.Source = _TextBox, .Path = New PropertyPath(DataContextProperty)})
        End If
    End Sub
    Public ReadOnly Property ConnectedTextBox As TextBox
    Private Sub TextBox_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs)
        Dim tb As TextBox = sender
        If tb.IsFocused AndAlso TypeOf ItemsSource Is IEnumerable AndAlso ItemsSource.GetEnumerator.MoveNext Then
            IsOpen = True
            'Keyboard.AddPreviewKeyDownHandler(Me, AddressOf TextBox_KeyDown)
            Dim _text As String = DirectCast(sender, TextBox).Text
            If TypeOf _text Is String Then
                Dim _patterns As String() = _text.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                UpdateFilter(New Func(Of Object, Boolean)(Function(obj)
                                                              Dim value As String = obj.ToString
                                                              For Each _pattern In _patterns
                                                                  If value.IndexOf(_pattern, System.StringComparison.CurrentCultureIgnoreCase) = -1 Then Return False
                                                              Next
                                                              Return True
                                                          End Function))
            End If
        End If
    End Sub
    Public Sub ShowFullList()
        If ItemsSource Is Nothing Then Return
        vPanel.ItemsSource = ItemsSource
        IsOpen = True
    End Sub
    Public Property ShouldHoldEnterKeyDown As Boolean
        Get
            Return GetValue(ShouldHoldEnterKeyDownProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(ShouldHoldEnterKeyDownProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ShouldHoldEnterKeyDownProperty As DependencyProperty =
                             DependencyProperty.Register("ShouldHoldEnterKeyDown",
                             GetType(Boolean), GetType(AutoCompletePopup),
                             New PropertyMetadata(False))
    Private Sub TextBox_KeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles Me.KeyDown
        Select Case e.Key
            Case Key.Up
                If IsOpen Then
                    Up()
                End If
            Case Key.Down
                If IsOpen Then
                    Down()
                End If
            Case Key.Tab
                If IsOpen Then
                    If System.Windows.Input.Keyboard.Modifiers = ModifierKeys.Shift Then
                        Up()
                    Else
                        Down()
                    End If
                End If
            Case Key.Enter
                If IsOpen And vPanel.SelectedIndex > -1 Then
                    SelectItem()
                    e.Handled = True
                ElseIf vPanel.SelectedIndex = -1 And vPanel.Items.Count > 0 Then
                    vPanel.SelectedIndex = 0
                End If
                If Not ShouldHoldEnterKeyDown Then e.Handled = True
        End Select
    End Sub
    Private Sub vPanel_MouseDoubleClick(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles vPanel.MouseDoubleClick
        SelectItem()
    End Sub
    Private Sub vPanel_MouseWheel(sender As Object, e As System.Windows.Input.MouseWheelEventArgs) Handles vPanel.PreviewMouseWheel
        If Keyboard.Modifiers = ModifierKeys.Shift Then
            vContainer.ScrollToHorizontalOffset(vContainer.ContentHorizontalOffset - e.Delta)
        Else
            vContainer.ScrollToVerticalOffset(vContainer.ContentVerticalOffset - e.Delta)
        End If
    End Sub
End Class

Public Class ItemSelectedEventArgs
    Inherits EventArgs
    Private _Value As Object
    Public Sub New(_Item As Object)
        _Value = _Item
    End Sub
    Public ReadOnly Property Item As Object
        Get
            Return _Value
        End Get
    End Property
End Class