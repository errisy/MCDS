
'this code expresser shall handle all kinds of codes in the same item. It allows user to embed codes inside.

Public Class CodeExpresserBase
    Inherits TextBox
    Public Sub New()
        Keyboard.AddGotKeyboardFocusHandler(Me, AddressOf WhenGotKeyboardFocus)
        Keyboard.AddLostKeyboardFocusHandler(Me, AddressOf WhenostKeyboardFocus)

        'setup default bindings
        SetBinding(ItemsSourceProperty, New Binding("ItemsSource"))
        SetBinding(SelectPreviousProperty, New Binding("SelectPrevious"))
        SetBinding(SelectNextProperty, New Binding("SelectNext"))
        SetBinding(SelectUpProperty, New Binding("SelectUp"))
        SetBinding(SelectDownProperty, New Binding("SelectDown"))

        SetBinding(ControlEnterProperty, New Binding("ControlEnter"))
        SetBinding(ShiftEnterProperty, New Binding("ShiftEnter"))
        SetBinding(EmptyBackProperty, New Binding("EmptyBack"))
        SetBinding(ControlBackProperty, New Binding("ControlBack"))

        _AutoComplete.SetBinding(AutoCompletePopup.ItemTemplateProperty, New Binding With {.Path = New PropertyPath(ItemTemplateProperty)})
        _AutoComplete.SetBinding(AutoCompletePopup.ItemsSourceProperty, New Binding With {.Path = New PropertyPath(ItemsSourceProperty)})
        AutoCompletePopup.SetAutoComplete(Me, _AutoComplete)
    End Sub

    Private _AutoComplete As New AutoCompletePopup
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
                             GetType(Brush), GetType(CodeExpresserBase),
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
                             GetType(String), GetType(CodeExpresserBase),
                             New PropertyMetadata(""))
    Public ReadOnly Property LabelVisibility As Visibility
        Get
            Return GetValue(CodeExpresserBase.LabelVisibilityProperty)
        End Get
    End Property
    Private Shared ReadOnly LabelVisibilityPropertyKey As DependencyPropertyKey =
                                  DependencyProperty.RegisterReadOnly("LabelVisibility",
                                  GetType(Visibility), GetType(CodeExpresserBase),
                                  New PropertyMetadata(Visibility.Visible))
    Public Shared ReadOnly LabelVisibilityProperty As DependencyProperty =
                                 LabelVisibilityPropertyKey.DependencyProperty
    Public ReadOnly Property TextVisibility As Visibility
        Get
            Return GetValue(CodeExpresserBase.TextVisibilityProperty)
        End Get
    End Property
    Private Shared ReadOnly TextVisibilityPropertyKey As DependencyPropertyKey =
                                  DependencyProperty.RegisterReadOnly("TextVisibility",
                                  GetType(Visibility), GetType(CodeExpresserBase),
                                  New PropertyMetadata(Visibility.Hidden))
    Public Shared ReadOnly TextVisibilityProperty As DependencyProperty =
                                 TextVisibilityPropertyKey.DependencyProperty
    Public ReadOnly Property ValueVisibility As Visibility
        Get
            Return GetValue(CodeExpresserBase.ValueVisibilityProperty)
        End Get
    End Property
    Private Shared ReadOnly ValueVisibilityPropertyKey As DependencyPropertyKey =
                                  DependencyProperty.RegisterReadOnly("ValueVisibility",
                                  GetType(Visibility), GetType(CodeExpresserBase),
                                  New PropertyMetadata(Visibility.Hidden))
    Public Shared ReadOnly ValueVisibilityProperty As DependencyProperty =
                                 ValueVisibilityPropertyKey.DependencyProperty
    Public ReadOnly Property ViewVisibility As Visibility
        Get
            Return GetValue(CodeExpresserBase.ViewVisibilityProperty)
        End Get
    End Property
    Private Shared ReadOnly ViewVisibilityPropertyKey As DependencyPropertyKey =
                                  DependencyProperty.RegisterReadOnly("ViewVisibility",
                                  GetType(Visibility), GetType(CodeExpresserBase),
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
                             GetType(IEnumerable), GetType(CodeExpresserBase),
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
                             GetType(ICommand), GetType(CodeExpresserBase),
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
                           GetType(Dictator), GetType(CodeExpresserBase),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedEditDictatorChanged)))
    Private Shared Sub SharedEditDictatorChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, CodeExpresserBase).EditDictatorChanged(d, e)
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
    Private Sub CodeExpresserBase_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If ShouldFocus Then SelectAll() : Focus() : ShouldFocus = False
    End Sub


#End Region
#Region "Internal Control"
    Public ReadOnly Property ExpressionVisibility As Visibility
        Get
            Return GetValue(CodeExpresserBase.ExpressionVisibilityProperty)
        End Get
    End Property
    Private Shared ReadOnly ExpressionVisibilityPropertyKey As DependencyPropertyKey =
                                  DependencyProperty.RegisterReadOnly("ExpressionVisibility",
                                  GetType(Visibility), GetType(CodeExpresserBase),
                                  New PropertyMetadata(Visibility.Hidden))
    Public Shared ReadOnly ExpressionVisibilityProperty As DependencyProperty =
                                 ExpressionVisibilityPropertyKey.DependencyProperty
    Public Property InternalExpression As Object
        Get
            Return GetValue(InternalExpressionProperty)
        End Get
        Set(ByVal value As Object)
            SetValue(InternalExpressionProperty, value)
        End Set
    End Property
    Public Shared ReadOnly InternalExpressionProperty As DependencyProperty =
                             DependencyProperty.Register("InternalExpression",
                             GetType(Object), GetType(CodeExpresserBase),
                             New PropertyMetadata(Nothing))

    'how to define a code expression here?
    'we can not find the code expression but there is another method to do that.
    'Function.Hook(module.expr(e,f,g,x))Use(m at c[String], d as e[String], f as k[Integer])
    ' algorithm: module.solveequation(1 as x1, 2 at x2, 3 at x3, 4 at y1, 5 at y2, 6 as y3)
    ' when cut things into layers and units, you spend a lot of time on getting them communitate efficiently
    ' use assigner
    'memeber A.B
    'whowt ot set up the right name of the system 
    '
#End Region

#Region "Navigation"
    Protected Overrides Sub OnPreviewKeyDown(e As KeyEventArgs)
        Select Case e.Key
            Case Key.Up
                If SelectUp IsNot Nothing AndAlso SelectUp.CanExecute(Nothing) Then SelectUp.Execute(Nothing) : e.Handled = True
            Case Key.Down
                If SelectDown IsNot Nothing AndAlso SelectDown.CanExecute(Nothing) Then SelectDown.Execute(Nothing) : e.Handled = True
            Case Key.Left
                If SelectionStart = 0 And SelectionLength = 0 Then
                    If SelectPrevious IsNot Nothing AndAlso SelectPrevious.CanExecute(Nothing) Then SelectPrevious.Execute(Nothing) : e.Handled = True
                End If
            Case Key.Right
                If SelectionStart = Text.Length Then
                    If SelectNext IsNot Nothing AndAlso SelectNext.CanExecute(Nothing) Then SelectNext.Execute(Nothing) : e.Handled = True
                End If
            Case Key.Enter
                Select Case Keyboard.Modifiers
                    Case ModifierKeys.Control
                        If ControlEnter IsNot Nothing AndAlso ControlEnter.CanExecute(Nothing) Then ControlEnter.Execute(Nothing)
                    Case ModifierKeys.Shift
                        If ShiftEnter IsNot Nothing AndAlso ShiftEnter.CanExecute(Nothing) Then ShiftEnter.Execute(Nothing)
                    Case ModifierKeys.None

                End Select
            Case Key.Back
                Select Case Keyboard.Modifiers
                    Case ModifierKeys.Control
                        If SelectionLength = 0 AndAlso SelectionStart = 0 AndAlso ControlBack IsNot Nothing AndAlso ControlBack.CanExecute(Nothing) Then ControlBack.Execute(Nothing)
                    Case ModifierKeys.Shift

                    Case ModifierKeys.None
                        If SelectionLength = 0 AndAlso SelectionStart = 0 AndAlso EmptyBack IsNot Nothing AndAlso EmptyBack.CanExecute(Nothing) Then EmptyBack.Execute(Nothing)
                End Select
        End Select
        MyBase.OnKeyDown(e)
    End Sub
    Public Property ControlEnter As ICommand
        Get
            Return GetValue(ControlEnterProperty)
        End Get
        Set(ByVal value As ICommand)
            SetValue(ControlEnterProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ControlEnterProperty As DependencyProperty =
                             DependencyProperty.Register("ControlEnter",
                             GetType(ICommand), GetType(CodeExpresserBase),
                             New PropertyMetadata(Nothing))
    Public Property ShiftEnter As ICommand
        Get
            Return GetValue(ShiftEnterProperty)
        End Get
        Set(ByVal value As ICommand)
            SetValue(ShiftEnterProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ShiftEnterProperty As DependencyProperty =
                             DependencyProperty.Register("ShiftEnter",
                             GetType(ICommand), GetType(CodeExpresserBase),
                             New PropertyMetadata(Nothing))
    Public Property ControlBack As ICommand
        Get
            Return GetValue(ControlBackProperty)
        End Get
        Set(ByVal value As ICommand)
            SetValue(ControlBackProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ControlBackProperty As DependencyProperty =
                             DependencyProperty.Register("ControlBack",
                             GetType(ICommand), GetType(CodeExpresserBase),
                             New PropertyMetadata(Nothing))
    Public Property EmptyBack As ICommand
        Get
            Return GetValue(EmptyBackProperty)
        End Get
        Set(ByVal value As ICommand)
            SetValue(EmptyBackProperty, value)
        End Set
    End Property
    Public Shared ReadOnly EmptyBackProperty As DependencyProperty =
                             DependencyProperty.Register("EmptyBack",
                             GetType(ICommand), GetType(CodeExpresserBase),
                             New PropertyMetadata(Nothing))

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
                             GetType(ICommand), GetType(CodeExpresserBase),
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
                             GetType(ICommand), GetType(CodeExpresserBase),
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
                             GetType(ICommand), GetType(CodeExpresserBase),
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
                             GetType(ICommand), GetType(CodeExpresserBase),
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
                             GetType(DataTemplate), GetType(CodeExpresserBase),
                             New PropertyMetadata(Nothing))
#End Region
End Class
