''' <summary>
''' AllocationViewModel是提供图形布局和绘制的ViewModel
''' </summary>
''' <remarks>AllocationViewModel是提供图形布局和绘制的ViewModel，用于在原类型和图形之间实现布局逻辑和鼠标事件。</remarks>
Public MustInherit Class AllocationViewModel
    Inherits Freezable
    Public MustOverride ReadOnly Property GetSpaceGeometry As Geometry
    Public MustOverride Sub ApplyOffset(Offset As Vector)

    Public Event MouseDown(sender As GeometryViewModel, e As MouseButtonEventArgs)
    Public Event MouseUp(sender As GeometryViewModel, e As MouseButtonEventArgs)
    Public Event MouseMove(sender As GeometryViewModel, e As MouseEventArgs)
    Public Event MouseWheel(sender As GeometryViewModel, e As MouseWheelEventArgs)
    Public Overridable Sub OnMouseDown(e As MouseButtonEventArgs)
        RaiseEvent MouseDown(Me, e)
    End Sub
    Public Overridable Sub OnMouseUp(e As MouseButtonEventArgs)
        RaiseEvent MouseUp(Me, e)
    End Sub
    Public Overridable Sub OnMouseMove(e As MouseEventArgs)
        RaiseEvent MouseMove(Me, e)
    End Sub
    Public Overridable Sub OnMouseWheel(e As MouseWheelEventArgs)
        RaiseEvent MouseWheel(Me, e)
    End Sub
    Protected Overrides Sub OnChanged()
        RaiseEvent ViewModelChanged(Me, New EventArgs)
        MyBase.OnChanged()
    End Sub
    Public Event ViewModelChanged(sender As Object, e As EventArgs)

End Class

