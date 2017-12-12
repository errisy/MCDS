Public Interface ISelectionProvider
    Property SelectedView As UIElement
End Interface

Public Module SelectionProvider
    <System.Runtime.CompilerServices.Extension> Public Sub TrySelect(obj As UIElement)
        If TypeOf obj Is UIElement Then
            Dim Provider = obj.Ancestor(Of ISelectionProvider)()
            If Provider IsNot Nothing Then Provider.SelectedView = obj
        End If
    End Sub
End Module

Public Interface ISelectionListener
    Property IsSelected As Boolean
End Interface