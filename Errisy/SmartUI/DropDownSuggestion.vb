Public Class DropDownSuggestion
    Inherits Control
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
                           GetType(IEnumerable), GetType(DropDownSuggestion),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedItemsSourceChanged)))
    Private Shared Sub SharedItemsSourceChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, Object).ItemsSourceChanged(d, e)
    End Sub
    Private Sub ItemsSourceChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        GenerateItems()
    End Sub
    Public Property Binding As Binding
        Get
            Return GetValue(BindingProperty)
        End Get
        Set(ByVal value As Binding)
            SetValue(BindingProperty, value)
        End Set
    End Property
    Public Shared ReadOnly BindingProperty As DependencyProperty =
                           DependencyProperty.Register("Binding",
                           GetType(Binding), GetType(DropDownSuggestion),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedBindingChanged)))
    Private Shared Sub SharedBindingChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, DropDownSuggestion).BindingChanged(d, e)
    End Sub
    Private Sub BindingChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        GenerateItems()
    End Sub
    Private _Items As New System.Collections.ObjectModel.ObservableCollection(Of String)
    Private _Detector As New BindingValueDetector
    Private Sub GenerateItems()
        _Items.Clear()
        If Binding Is Nothing Then
            For Each item In ItemsSource
                _Items.Add(item.ToString)
            Next
        Else
            BindingOperations.SetBinding(_Detector, BindingValueDetector.ValueProperty, Binding)

            For Each item In ItemsSource
                Binding.Source = item
                _Items.Add(_Detector.Value)
            Next
        End If
    End Sub

    Public Shared Function GetSuggestion(ByVal element As TextBox) As DropDownSuggestion
        Return element.GetValue(SuggestionProperty)
    End Function

    Public Shared Sub SetSuggestion(ByVal element As TextBox, ByVal value As DropDownSuggestion)
        element.SetValue(SuggestionProperty, value)
    End Sub

    Public Shared ReadOnly SuggestionProperty As _
                           DependencyProperty = DependencyProperty.RegisterAttached("Suggestion",
                           GetType(IEnumerable), GetType(DropDownSuggestion),
                           New PropertyMetadata(Nothing, AddressOf SuggestionChanged))
    Private Shared Sub SuggestionChanged(sender As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If Not (TypeOf sender Is TextBox) Then Return
        Dim _TextBox As TextBox = sender
        If TypeOf e.OldValue Is DropDownSuggestion Then
            RemoveHandler _TextBox.KeyDown, AddressOf DirectCast(e.OldValue, DropDownSuggestion).TextBoxKeyDown
        End If
        If TypeOf e.NewValue Is DropDownSuggestion Then
            AddHandler _TextBox.KeyDown, AddressOf DirectCast(e.NewValue, DropDownSuggestion).TextBoxKeyDown
        End If
    End Sub
    Private Sub TextBoxKeyDown(sender As Object, e As KeyEventArgs)
        Select Case e.Key
            Case Key.Up
                MoveUp()
            Case Key.Down
                MoveDown()
            Case Key.Tab
                If System.Windows.Input.Keyboard.Modifiers = ModifierKeys.Shift Then
                    MoveUp()
                Else
                    MoveDown()
                End If
            Case Key.Enter
                SelectItem()
        End Select
    End Sub
    Private Sub MoveUp()

    End Sub
    Private Sub MoveDown()

    End Sub
    Private Sub SelectItem()

    End Sub
End Class

Public Class BindingValueDetector
    Inherits DependencyObject
    Public Property Value As String
        Get
            Return GetValue(ValueProperty)
        End Get
        Set(ByVal value As String)
            SetValue(ValueProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ValueProperty As DependencyProperty =
                             DependencyProperty.Register("Value",
                             GetType(String), GetType(BindingValueDetector),
                             New PropertyMetadata(""))
End Class