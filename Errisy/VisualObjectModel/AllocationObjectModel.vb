Public MustInherit Class AllocationObjectModel
    Public MustOverride ReadOnly Property GetSpaceGeometry As Geometry
    Public MustOverride Sub ApplyOffset(Offset As Vector)

    Public Event MouseDown(sender As AllocationObjectModel, e As MouseButtonEventArgs)
    Public Event MouseUp(sender As AllocationObjectModel, e As MouseButtonEventArgs)
    Public Event MouseMove(sender As AllocationObjectModel, e As MouseEventArgs)
    Public Event MouseWheel(sender As AllocationObjectModel, e As MouseWheelEventArgs)
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
    Private _Changed As Boolean = False
    Public ReadOnly Property Changed As Boolean
        Get
            Return _Changed
        End Get
    End Property
    'Public Property IsChangedSuspended As Boolean
    Private _IsChangedSuspended As Boolean = False
    Public Property IsChangedSuspended As Boolean
        Get
            Return _IsChangedSuspended
        End Get
        Set(value As Boolean)
            _IsChangedSuspended = value
        End Set
    End Property

    Public Overridable Sub Change()
        _Changed = True
        If _IsChangedSuspended Then Return
        RaiseEvent ViewModelChanged(Me, New EventArgs)
    End Sub
    Public Event ViewModelChanged(sender As Object, e As EventArgs)
End Class

