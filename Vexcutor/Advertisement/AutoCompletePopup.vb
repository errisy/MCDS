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
    Public Shared ReadOnly ItemTemplateProperty As DependencyProperty = _
                           DependencyProperty.Register("ItemTemplate", _
                           GetType(DataTemplate), GetType(AutoCompletePopup), _
                           New PropertyMetadata(Nothing)) ', New PropertyChangedCallback(AddressOf SharedItemTemplateChanged)))
    'Private Shared Sub SharedItemTemplateChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    '    DirectCast(d, AutoCompletePopup).ItemTemplateChanged(d, e)
    'End Sub
    'Private Sub ItemTemplateChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    '    vPanel.ItemTemplate = e.NewValue
    'End Sub
    'AutoCompletePopup->Values As IEnumerable with Event Default: Nothing
    Public Property Values As IEnumerable
        Get
            Return GetValue(ValuesProperty)
        End Get
        Set(ByVal value As IEnumerable)
            SetValue(ValuesProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ValuesProperty As DependencyProperty = _
                           DependencyProperty.Register("Values", _
                           GetType(IEnumerable), GetType(AutoCompletePopup), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedValuesChanged)))
    Private Shared Sub SharedValuesChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, AutoCompletePopup).ValuesChanged(d, e)
    End Sub
    Private Sub ValuesChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)

    End Sub
    'AutoCompletePopup->Filter As Func(Of Object, Boolean) with Event Default: Nothing
    Public Property Filter As Func(Of Object, Boolean)
        Get
            Return GetValue(FilterProperty)
        End Get
        Set(ByVal value As Func(Of Object, Boolean))
            SetValue(FilterProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FilterProperty As DependencyProperty = _
                           DependencyProperty.Register("Filter", _
                           GetType(Func(Of Object, Boolean)), GetType(AutoCompletePopup), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedFilterChanged)))
    Private Shared Sub SharedFilterChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, AutoCompletePopup).FilterChanged(d, e)
    End Sub
    Private Sub FilterChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim vList As New List(Of Object)
        Dim enl As IEnumerable = GetValue(ValuesProperty)
        If TypeOf e.NewValue Is Func(Of Object, Boolean) Then
            Dim f As Func(Of Object, Boolean) = e.NewValue
            If TypeOf enl Is IEnumerable Then
                For Each obj In enl
                    If f.Invoke(obj) Then
                        vList.Add(obj)
                    End If
                Next
            End If
            vPanel.ItemsSource = vList
        Else
            vPanel.ItemsSource = enl
        End If
    End Sub
    'AutoCompletePopup->TextFilter As String with Event Default: ""
    Public Property TextFilter As String
        Get
            Return GetValue(TextFilterProperty)
        End Get
        Set(ByVal value As String)
            SetValue(TextFilterProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TextFilterProperty As DependencyProperty = _
                           DependencyProperty.Register("TextFilter", _
                           GetType(String), GetType(AutoCompletePopup), _
                           New PropertyMetadata("", New PropertyChangedCallback(AddressOf SharedTextFilterChanged)))
    Private Shared Sub SharedTextFilterChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, AutoCompletePopup).TextFilterChanged(d, e)
    End Sub
    Private Sub TextFilterChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If TypeOf e.NewValue Is String Then
            Dim s As String = e.NewValue
            Filter = New Func(Of Object, Boolean)(Function(obj)
                                                      Return obj.ToString.IndexOf(s, System.StringComparison.CurrentCultureIgnoreCase) > -1
                                                  End Function)
        End If
    End Sub
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
            ConnectedTextBox.Text = vPanel.SelectedItem
            ConnectedTextBox.SelectionStart = ConnectedTextBox.Text.Length
            IsOpen = False
        End If
    End Sub
    Protected Overrides Sub OnKeyDown(e As System.Windows.Input.KeyEventArgs)
        Select Case e.Key
            Case Key.Enter
                SelectItem()
        End Select
        MyBase.OnKeyDown(e)
    End Sub
    'AutoCompletePopup->ConnectedTextBox As TextBox with Event Default: Nothing
    Public Property ConnectedTextBox As TextBox
        Get
            Return GetValue(ConnectedTextBoxProperty)
        End Get
        Set(ByVal value As TextBox)
            SetValue(ConnectedTextBoxProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ConnectedTextBoxProperty As DependencyProperty = _
                           DependencyProperty.Register("ConnectedTextBox", _
                           GetType(TextBox), GetType(AutoCompletePopup), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedConnectedTextBoxChanged)))
    Private Shared Sub SharedConnectedTextBoxChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, AutoCompletePopup).ConnectedTextBoxChanged(d, e)
    End Sub
    Private Sub ConnectedTextBoxChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If TypeOf e.OldValue Is TextBox Then
            RemoveHandler CType(e.OldValue, TextBox).TextChanged, AddressOf TextBox_TextChanged
            RemoveHandler CType(e.OldValue, TextBox).PreviewKeyDown, AddressOf TextBox_KeyDown
            BindingOperations.ClearBinding(Me, TextFilterProperty)
        End If
        If TypeOf e.NewValue Is TextBox Then
            AddHandler CType(e.NewValue, TextBox).TextChanged, AddressOf TextBox_TextChanged
            AddHandler CType(e.NewValue, TextBox).PreviewKeyDown, AddressOf TextBox_KeyDown
            SetBinding(TextFilterProperty, New Binding With {.Source = e.NewValue, .Path = New PropertyPath(TextBox.TextProperty), .Mode = BindingMode.TwoWay, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged})
            PlacementTarget = e.NewValue
        End If
    End Sub
    Private Sub TextBox_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs)
        Dim tb As TextBox = sender
        If tb.IsFocused Then
            IsOpen = True
        End If
    End Sub
    Private Sub TextBox_KeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles Me.KeyDown
        Select Case e.Key
            Case Key.Up
                If IsOpen Then
                    Up()
                End If
            Case Key.Down, Key.Tab
                If IsOpen Then
                    Down()
                End If
            Case Key.Enter
                If IsOpen And vPanel.SelectedIndex > -1 Then
                    SelectItem()
                    e.Handled = True
                ElseIf vPanel.SelectedIndex = -1 And vPanel.Items.Count > 0 Then
                    vPanel.SelectedIndex = 0
                End If
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
