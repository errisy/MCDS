Imports System.Windows, System.Windows.Input
Public Class EventCommand
    Inherits DependencyObject

    Public Shared Function GetMouseDownCommand(ByVal element As DependencyObject) As ViewModelCommand
        If element Is Nothing Then Return Nothing
        Return element.GetValue(MouseDownCommandProperty)
    End Function

    Public Shared Sub SetMouseDownCommand(ByVal element As UIElement, ByVal value As ViewModelCommand)
        If element Is Nothing Then Return
        element.SetValue(MouseDownCommandProperty, value)
    End Sub

    Public Shared ReadOnly MouseDownCommandProperty As _
                           DependencyProperty = DependencyProperty.RegisterAttached("MouseDownCommand",
                           GetType(ViewModelCommand), GetType(EventCommand),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedMouseDownChanged)))
    Private Shared Sub SharedMouseDownChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim ocmd As ViewModelCommand = e.OldValue
        Dim ncmd As ViewModelCommand = e.NewValue
        If ocmd IsNot Nothing Then
            RemoveHandler DirectCast(d, UIElement).MouseDown, AddressOf ocmd.CallByEvent
        End If
        If ncmd IsNot Nothing Then
            AddHandler DirectCast(d, UIElement).MouseDown, AddressOf ncmd.CallByEvent
        End If
    End Sub

    Public Shared Function GetKeyDownCommand(ByVal element As DependencyObject) As ViewModelCommand
        If element Is Nothing Then Return Nothing
        Return element.GetValue(KeyDownCommandProperty)
    End Function

    Public Shared Sub SetKeyDownCommand(ByVal element As UIElement, ByVal value As ViewModelCommand)
        If element Is Nothing Then Return
        element.SetValue(KeyDownCommandProperty, value)
    End Sub

    Public Shared ReadOnly KeyDownCommandProperty As _
                           DependencyProperty = DependencyProperty.RegisterAttached("KeyDownCommand",
                           GetType(ViewModelCommand), GetType(EventCommand),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedKeyDownChanged)))
    Private Shared Sub SharedKeyDownChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim ocmd As ViewModelCommand = e.OldValue
        Dim ncmd As ViewModelCommand = e.NewValue
        If ocmd IsNot Nothing Then
            RemoveHandler DirectCast(d, UIElement).KeyDown, AddressOf ocmd.CallByEvent
        End If
        If ncmd IsNot Nothing Then
            AddHandler DirectCast(d, UIElement).KeyDown, AddressOf ncmd.CallByEvent
        End If
    End Sub
    Public Shared Function IsKeyEventArgsAndKey(e As Object, ParamArray KeyCodes() As System.Windows.Input.Key) As Boolean
        If Not (TypeOf e Is System.Windows.Input.KeyEventArgs) Then Return False
        Return KeyCodes.Contains(DirectCast(e, System.Windows.Input.KeyEventArgs).Key)
    End Function
End Class