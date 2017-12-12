Public Class ObjectContainer
    Private _GeometryModels As New System.Collections.ObjectModel.ObservableCollection(Of AllocationObjectModel)
    Public Property DesiredSize As Size
    'GeometryContainer -> ObjectModels As System.Collections.ObjectModel.ObservableCollection(of AllocationObjectModel) Default: Nothing
    Public ReadOnly Property ObjectModels As System.Collections.ObjectModel.ObservableCollection(Of AllocationObjectModel)
        Get
            Return _GeometryModels
        End Get
    End Property
    Protected Sub ApplyOffset(vector As Vector)
        For Each _Geometry In _GeometryModels
            _Geometry.ApplyOffset(vector)
        Next
    End Sub
End Class
