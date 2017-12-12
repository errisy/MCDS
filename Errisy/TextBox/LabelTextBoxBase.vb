Public Class LabelTextBoxBase
    Inherits TextBox
    Public Property LabelForegroud As Brush
        Get
            Return GetValue(LabelForegroudProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(LabelForegroudProperty, value)
        End Set
    End Property
    Public Shared ReadOnly LabelForegroudProperty As DependencyProperty =
                             DependencyProperty.Register("LabelForegroud",
                             GetType(Brush), GetType(LabelTextBoxBase),
                             New PropertyMetadata(Brushes.Gray))
    Public Property Label As String
        Get
            Return GetValue(LabelProperty)
        End Get
        Set(ByVal value As String)
            SetValue(LabelProperty, value)
        End Set
    End Property
    Public Shared ReadOnly LabelProperty As DependencyProperty =
                             DependencyProperty.Register("Label",
                             GetType(String), GetType(LabelTextBoxBase),
                             New PropertyMetadata(""))
    Public ReadOnly Property LabelVisibility As Visibility
        Get
            Return GetValue(LabelTextBoxBase.LabelVisibilityProperty)
        End Get
    End Property
    Private Shared ReadOnly LabelVisibilityPropertyKey As DependencyPropertyKey =
                                  DependencyProperty.RegisterReadOnly("LabelVisibility",
                                  GetType(Visibility), GetType(LabelTextBoxBase),
                                  New PropertyMetadata(Visibility.Visible))
    Public Shared ReadOnly LabelVisibilityProperty As DependencyProperty =
                                 LabelVisibilityPropertyKey.DependencyProperty
    Public ReadOnly Property TextVisibility As Visibility
        Get
            Return GetValue(LabelTextBoxBase.TextVisibilityProperty)
        End Get
    End Property
    Private Shared ReadOnly TextVisibilityPropertyKey As DependencyPropertyKey =
                                  DependencyProperty.RegisterReadOnly("TextVisibility",
                                  GetType(Visibility), GetType(LabelTextBoxBase),
                                  New PropertyMetadata(Visibility.Hidden))
    Public Shared ReadOnly TextVisibilityProperty As DependencyProperty =
                                 TextVisibilityPropertyKey.DependencyProperty
    Public ReadOnly Property ValueVisibility As Visibility
        Get
            Return GetValue(LabelTextBoxBase.ValueVisibilityProperty)
        End Get
    End Property
    Private Shared ReadOnly ValueVisibilityPropertyKey As DependencyPropertyKey =
                                  DependencyProperty.RegisterReadOnly("ValueVisibility",
                                  GetType(Visibility), GetType(LabelTextBoxBase),
                                  New PropertyMetadata(Visibility.Hidden))
    Public Shared ReadOnly ValueVisibilityProperty As DependencyProperty =
                                 ValueVisibilityPropertyKey.DependencyProperty
    Public ReadOnly Property ViewVisibility As Visibility
        Get
            Return GetValue(LabelTextBoxBase.ViewVisibilityProperty)
        End Get
    End Property
    Private Shared ReadOnly ViewVisibilityPropertyKey As DependencyPropertyKey =
                                  DependencyProperty.RegisterReadOnly("ViewVisibility",
                                  GetType(Visibility), GetType(LabelTextBoxBase),
                                  New PropertyMetadata(Visibility.Visible))
    Public Shared ReadOnly ViewVisibilityProperty As DependencyProperty =
                                 ViewVisibilityPropertyKey.DependencyProperty
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
                             GetType(IEnumerable), GetType(LabelTextBoxBase),
                             New PropertyMetadata(Nothing))

    Protected Overrides Sub OnTextChanged(e As TextChangedEventArgs)
        If Text Is Nothing OrElse Text = "" Then
            SetValue(LabelVisibilityPropertyKey, Visibility.Visible)
            SetValue(ValueVisibilityPropertyKey, Visibility.Hidden)
        Else
            SetValue(ValueVisibilityPropertyKey, Visibility.Visible)
            SetValue(LabelVisibilityPropertyKey, Visibility.Hidden)
        End If
        MyBase.OnTextChanged(e)
    End Sub
    Protected Overrides Sub OnMouseEnter(e As MouseEventArgs)
        SelectAll()
        Focus()
        MyBase.OnMouseEnter(e)
    End Sub
    Private Shared _AutoCompletePopup As New AutoCompletePopup
    Public Sub New()
        Keyboard.AddGotKeyboardFocusHandler(Me, AddressOf WhenGotKeyboardFocus)
        Keyboard.AddLostKeyboardFocusHandler(Me, AddressOf WhenostKeyboardFocus)
    End Sub
    Private Sub WhenGotKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
        SetValue(ViewVisibilityPropertyKey, Visibility.Hidden)
        SetValue(TextVisibilityPropertyKey, Visibility.Visible)
        AutoCompletePopup.SetAutoComplete(Me, _AutoCompletePopup)
        _AutoCompletePopup.ItemsSource = ItemsSource
        AddHandler _AutoCompletePopup.ItemSelected, AddressOf OnItemSelected
    End Sub
    Private Sub OnItemSelected(sender As Object, e As EventArgs)
        If AutoCompletedCommand IsNot Nothing AndAlso AutoCompletedCommand.CanExecute(Nothing) Then AutoCompletedCommand.Execute(Nothing)
    End Sub
    Public Property AutoCompletedCommand As ICommand
        Get
            Return GetValue(AutoCompletedCommandProperty)
        End Get
        Set(ByVal value As ICommand)
            SetValue(AutoCompletedCommandProperty, value)
        End Set
    End Property
    Public Shared ReadOnly AutoCompletedCommandProperty As DependencyProperty =
                             DependencyProperty.Register("AutoCompletedCommand",
                             GetType(ICommand), GetType(LabelTextBoxBase),
                             New PropertyMetadata(Nothing))
    Private Sub WhenostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
        RemoveHandler _AutoCompletePopup.ItemSelected, AddressOf OnItemSelected
        SetValue(TextVisibilityPropertyKey, Visibility.Hidden)
        SetValue(ViewVisibilityPropertyKey, Visibility.Visible)
        AutoCompletePopup.SetAutoComplete(Me, Nothing)
        _AutoCompletePopup.ItemsSource = Nothing
    End Sub
    Protected Overrides Sub Finalize()
        Keyboard.RemoveGotKeyboardFocusHandler(Me, AddressOf WhenGotKeyboardFocus)
        Keyboard.RemoveLostKeyboardFocusHandler(Me, AddressOf WhenostKeyboardFocus)
        MyBase.Finalize()
    End Sub

#Region "Accept Request from Model"
    Public Property EditDictator As Dictator
        Get
            Return GetValue(EditDictatorProperty)
        End Get
        Set(ByVal value As Dictator)
            SetValue(EditDictatorProperty, value)
        End Set
    End Property
    Public Shared ReadOnly EditDictatorProperty As DependencyProperty =
                           DependencyProperty.Register("EditDictator",
                           GetType(Dictator), GetType(LabelTextBoxBase),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedEditDictatorChanged)))
    Private Shared Sub SharedEditDictatorChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, LabelTextBoxBase).EditDictatorChanged(d, e)
    End Sub
    Private Sub EditDictatorChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.OldValue IsNot Nothing Then
            DirectCast(e.OldValue, Dictator).RemoveListener(AddressOf OnEditDictator)
        End If
        If e.NewValue IsNot Nothing Then
            DirectCast(e.NewValue, Dictator).AddListener(AddressOf OnEditDictator)
        End If
    End Sub
    Private Sub OnEditDictator(sender As Object, e As EventArgs)
        If IsLoaded Then
            SelectAll()
            Focus()
        Else
            ShouldFocus = True
        End If
    End Sub
    Dim ShouldFocus As Boolean = False
    Private Sub LabelTextBoxBase_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If ShouldFocus Then SelectAll() : Focus() : ShouldFocus = False
    End Sub


#End Region

#Region "Navigation"
    Protected Overrides Sub OnPreviewKeyDown(e As KeyEventArgs)
        Select Case e.Key
            Case Key.Up

            Case Key.Down

            Case Key.Left
                If SelectionStart = 0 And SelectionLength = 0 Then
                    If SelectPrevious IsNot Nothing AndAlso SelectPrevious.CanExecute(Nothing) Then SelectPrevious.Execute(Nothing) : e.Handled = True
                End If
            Case Key.Right
                If SelectionStart = Text.Length Then
                    If SelectNext IsNot Nothing AndAlso SelectNext.CanExecute(Nothing) Then SelectNext.Execute(Nothing) : e.Handled = True
                End If
        End Select
        MyBase.OnKeyDown(e)
    End Sub
    Public Property SelectPrevious As ICommand
        Get
            Return GetValue(SelectPreviousProperty)
        End Get
        Set(ByVal value As ICommand)
            SetValue(SelectPreviousProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SelectPreviousProperty As DependencyProperty =
                             DependencyProperty.Register("SelectPrevious",
                             GetType(ICommand), GetType(LabelTextBoxBase),
                             New PropertyMetadata(Nothing))
    Public Property SelectNext As ICommand
        Get
            Return GetValue(SelectNextProperty)
        End Get
        Set(ByVal value As ICommand)
            SetValue(SelectNextProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SelectNextProperty As DependencyProperty =
                             DependencyProperty.Register("SelectNext",
                             GetType(ICommand), GetType(LabelTextBoxBase),
                             New PropertyMetadata(Nothing))
    Public Property SelectUp As ICommand
        Get
            Return GetValue(SelectUpProperty)
        End Get
        Set(ByVal value As ICommand)
            SetValue(SelectUpProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SelectUpProperty As DependencyProperty =
                             DependencyProperty.Register("SelectUp",
                             GetType(ICommand), GetType(LabelTextBoxBase),
                             New PropertyMetadata(Nothing))
    Public Property SelectDown As ICommand
        Get
            Return GetValue(SelectDownProperty)
        End Get
        Set(ByVal value As ICommand)
            SetValue(SelectDownProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SelectDownProperty As DependencyProperty =
                             DependencyProperty.Register("SelectDown",
                             GetType(ICommand), GetType(LabelTextBoxBase),
                             New PropertyMetadata(Nothing))
#End Region
End Class
