Imports System.ComponentModel

Public Class CommandModel
    Implements System.ComponentModel.INotifyPropertyChanged
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Private _Hello As New ViewModelCommand(AddressOf cmdHello)
    Public ReadOnly Property Hello As ViewModelCommand
        Get
            Return _Hello
        End Get
    End Property
    Private Sub cmdHello(value As Object)
        MsgBox(value.ToString)
    End Sub
End Class

Public Class ViewModelCommand
    Implements ICommand
    Private _Handler As Action(Of Object)
    Public Sub New(handler As Action(Of Object))
        _Handler = handler
    End Sub
    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        If _Handler IsNot Nothing Then _Handler(parameter)
    End Sub
    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return _Handler IsNot Nothing
    End Function
    Public Sub CallByEvent(sender As Object, e As EventArgs)
        If CanExecute(e) Then Execute(e)
    End Sub
End Class


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


    Public Shared Function GetMouseLeftDownCommand(ByVal element As DependencyObject) As ViewModelCommand
        If element Is Nothing Then Return Nothing
        Return element.GetValue(MouseLeftDownCommandProperty)
    End Function

    Public Shared Sub SetMouseLeftDownCommand(ByVal element As UIElement, ByVal value As ViewModelCommand)
        If element Is Nothing Then Return
        element.SetValue(MouseLeftDownCommandProperty, value)
    End Sub

    Public Shared ReadOnly MouseLeftDownCommandProperty As _
                           DependencyProperty = DependencyProperty.RegisterAttached("MouseLeftDownCommand",
                           GetType(ViewModelCommand), GetType(EventCommand),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedLeftMouseDownChanged)))
    Private Shared Sub SharedLeftMouseDownChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim ocmd As ViewModelCommand = e.OldValue
        Dim ncmd As ViewModelCommand = e.NewValue
        If ocmd IsNot Nothing Then
            RemoveHandler DirectCast(d, UIElement).MouseLeftButtonDown, AddressOf ocmd.CallByEvent
        End If
        If ncmd IsNot Nothing Then
            AddHandler DirectCast(d, UIElement).MouseLeftButtonDown, AddressOf ncmd.CallByEvent
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

    Public Shared Function GetKeyUpCommand(ByVal element As DependencyObject) As ViewModelCommand
        If element Is Nothing Then Return Nothing
        Return element.GetValue(KeyUpCommandProperty)
    End Function
    Public Shared Sub SetKeyUpCommand(ByVal element As UIElement, ByVal value As ViewModelCommand)
        If element Is Nothing Then Return
        element.SetValue(KeyUpCommandProperty, value)
    End Sub

    Public Shared ReadOnly KeyUpCommandProperty As _
                           DependencyProperty = DependencyProperty.RegisterAttached("KeyUpCommand",
                           GetType(ViewModelCommand), GetType(EventCommand),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedKeyUpChanged)))
    Private Shared Sub SharedKeyUpChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim ocmd As ViewModelCommand = e.OldValue
        Dim ncmd As ViewModelCommand = e.NewValue
        If ocmd IsNot Nothing Then
            RemoveHandler DirectCast(d, UIElement).KeyUp, AddressOf ocmd.CallByEvent
        End If
        If ncmd IsNot Nothing Then
            AddHandler DirectCast(d, UIElement).KeyUp, AddressOf ncmd.CallByEvent
        End If
    End Sub

    Public Shared Function GetPreviewKeyDownCommand(ByVal element As DependencyObject) As ViewModelCommand
        If element Is Nothing Then Return Nothing
        Return element.GetValue(PreviewKeyDownCommandProperty)
    End Function
    Public Shared Sub SetPreviewKeyDownCommand(ByVal element As UIElement, ByVal value As ViewModelCommand)
        If element Is Nothing Then Return
        element.SetValue(PreviewKeyDownCommandProperty, value)
    End Sub

    Public Shared ReadOnly PreviewKeyDownCommandProperty As _
                           DependencyProperty = DependencyProperty.RegisterAttached("PreviewKeyDownCommand",
                           GetType(ViewModelCommand), GetType(EventCommand),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedPreviewKeyDownChanged)))
    Private Shared Sub SharedPreviewKeyDownChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim ocmd As ViewModelCommand = e.OldValue
        Dim ncmd As ViewModelCommand = e.NewValue
        If ocmd IsNot Nothing Then
            RemoveHandler DirectCast(d, UIElement).PreviewKeyDown, AddressOf ocmd.CallByEvent
        End If
        If ncmd IsNot Nothing Then
            AddHandler DirectCast(d, UIElement).PreviewKeyDown, AddressOf ncmd.CallByEvent
        End If
    End Sub

    Public Shared Function GetPreviewKeyUpCommand(ByVal element As DependencyObject) As ViewModelCommand
        If element Is Nothing Then Return Nothing
        Return element.GetValue(PreviewKeyUpCommandProperty)
    End Function
    Public Shared Sub SetPreviewKeyUpCommand(ByVal element As UIElement, ByVal value As ViewModelCommand)
        If element Is Nothing Then Return
        element.SetValue(PreviewKeyUpCommandProperty, value)
    End Sub

    Public Shared ReadOnly PreviewKeyUpCommandProperty As _
                           DependencyProperty = DependencyProperty.RegisterAttached("PreviewKeyUpCommand",
                           GetType(ViewModelCommand), GetType(EventCommand),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedPreviewKeyUpChanged)))
    Private Shared Sub SharedPreviewKeyUpChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim ocmd As ViewModelCommand = e.OldValue
        Dim ncmd As ViewModelCommand = e.NewValue
        If ocmd IsNot Nothing Then
            RemoveHandler DirectCast(d, UIElement).PreviewKeyUp, AddressOf ocmd.CallByEvent
        End If
        If ncmd IsNot Nothing Then
            AddHandler DirectCast(d, UIElement).PreviewKeyUp, AddressOf ncmd.CallByEvent
        End If
    End Sub
End Class