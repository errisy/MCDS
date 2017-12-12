Public Class ObjectAllocator
    Private _BaseGeometry As New PathGeometry
    Public Sub Add(newViewModel As AllocationObjectModel)
        _BaseGeometry = PathGeometry.Combine(_BaseGeometry, newViewModel.GetSpaceGeometry, GeometryCombineMode.Union, Nothing)
    End Sub
    Public Sub Add(_Geometry As Geometry)
        _BaseGeometry = PathGeometry.Combine(_BaseGeometry, _Geometry, GeometryCombineMode.Union, Nothing)
    End Sub
    Public Function IntersectsWith(newViewModel As AllocationObjectModel) As Boolean
        Dim _intersect = PathGeometry.Combine(_BaseGeometry, newViewModel.GetSpaceGeometry, GeometryCombineMode.Intersect, Nothing)
        Return _intersect.GetArea > 0.0#
    End Function
    Public ReadOnly Property Offset As Vector
        Get
            Dim renderBounds = _BaseGeometry.GetRenderBounds(New Pen(Brushes.Black, 1.0#))
            Return New Vector(-renderBounds.X, -renderBounds.Y)
        End Get
    End Property
    Public ReadOnly Property Size As Size
        Get
            Dim renderBounds = _BaseGeometry.GetRenderBounds(New Pen(Brushes.Black, 1.0#))
            Return renderBounds.Size
        End Get
    End Property
    Public Shared Operator Or(x As ObjectAllocator, y As ObjectAllocator) As ObjectAllocator
        Return New ObjectAllocator With {._BaseGeometry = PathGeometry.Combine(x._BaseGeometry, y._BaseGeometry, GeometryCombineMode.Union, Nothing)}
    End Operator
End Class
