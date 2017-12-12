''' <summary>
''' 利用Geometry的填充性对AllocationViewModel进行非重叠性的布局测试
''' </summary>
''' <remarks>向GeometryAllocator添加Geometry之后，GeometryAllocator会对所有的Geometry进行合并。
''' 然后可以对新加入的Geometry进行IntersectsWith测试，如果结果为True，说明新加入的Geometry跟已有的Geometry有重叠。</remarks>
Public Class GeometryAllocator
    Private _BaseGeometry As New PathGeometry
    Public Sub Add(newViewModel As AllocationViewModel)
        _BaseGeometry = PathGeometry.Combine(_BaseGeometry, newViewModel.GetSpaceGeometry, GeometryCombineMode.Union, Nothing)
    End Sub
    Public Sub Add(_Geometry As Geometry)
        _BaseGeometry = PathGeometry.Combine(_BaseGeometry, _Geometry, GeometryCombineMode.Union, Nothing)
    End Sub
    Public Function IntersectsWith(newViewModel As AllocationViewModel) As Boolean
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
    Public Shared Operator Or(x As GeometryAllocator, y As GeometryAllocator) As GeometryAllocator
        Return New GeometryAllocator With {._BaseGeometry = PathGeometry.Combine(x._BaseGeometry, y._BaseGeometry, GeometryCombineMode.Union, Nothing)}
    End Operator
End Class

