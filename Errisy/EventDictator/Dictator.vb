Public Class Dictator

    Public Sub Broadcast()
        Invoke(Me, Nothing)
    End Sub
    Public Sub Broadcast(sender As Object, e As EventArgs)
        Invoke(sender, e)
    End Sub
    Private _ShouldBroadcast As Boolean = False
    Private _Sender As Object
    Private _EventArgs As EventArgs
    Private Sub Invoke(sender As Object, e As EventArgs)
        If DictateEvent Is Nothing OrElse DictateEvent.GetInvocationList.Count = 0 Then
            _ShouldBroadcast = True
            _Sender = sender
            _EventArgs = e
        Else
            RaiseEvent Dictate(_Sender, _EventArgs)
        End If
    End Sub

    Public Event Dictate As EventHandler
    Public Sub AddListener(handler As EventHandler)
        AddHandler Dictate, handler
        If _ShouldBroadcast Then RaiseEvent Dictate(_Sender, _EventArgs) : _ShouldBroadcast = False
    End Sub
    Public Sub RemoveListener(handler As EventHandler)
        RemoveHandler Dictate, handler
    End Sub
End Class