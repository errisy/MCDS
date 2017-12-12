Public Class AllocationViewModelCollection
    Inherits System.Collections.ObjectModel.ObservableCollection(Of AllocationViewModel)
    Public Sub ApplyOffset(Offset As Vector)
        For Each vm In Me
            vm.ApplyOffset(Offset)
        Next
    End Sub
    Public Sub FreezeAll()
        For Each vm In Me
            vm.Freeze()
        Next
    End Sub
End Class
