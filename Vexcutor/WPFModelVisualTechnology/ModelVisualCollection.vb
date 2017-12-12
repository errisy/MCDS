Public Class ModelVisualCollection
    Inherits System.Collections.ObjectModel.ObservableCollection(Of ModelVisual)
    Protected Overrides Sub ClearItems()
        Dim removeList As New List(Of ModelVisual)(Me)
        For Each it In removeList
            Me.Remove(it)
        Next
    End Sub
End Class
