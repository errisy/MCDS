Imports System.Windows.Input
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