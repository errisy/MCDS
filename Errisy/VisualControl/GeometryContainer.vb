Public Class GeometryContainer
    Inherits Freezable
    Private _GeometryModels As New System.Collections.ObjectModel.ObservableCollection(Of AllocationViewModel)
    Public Sub New()
        SetValue(GeometryModelsPropertyKey, _GeometryModels)
    End Sub
    'GeometryContainer -> DesiredSize As Size Default: New Size()
    Public ReadOnly Property DesiredSize As Size
        Get
            Return GetValue(GeometryContainer.DesiredSizeProperty)
        End Get
    End Property
    Protected Shared ReadOnly DesiredSizePropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("DesiredSize", _
                              GetType(Size), GetType(GeometryContainer), _
                              New PropertyMetadata(New Size()))
    Public Shared ReadOnly DesiredSizeProperty As DependencyProperty = _
                             DesiredSizePropertyKey.DependencyProperty
    'GeometryContainer -> GeometryModels As System.Collections.ObjectModel.ObservableCollection(of AllocationViewModel) Default: Nothing
    Public ReadOnly Property GeometryModels As System.Collections.ObjectModel.ObservableCollection(Of AllocationViewModel)
        Get
            Return GetValue(GeometryContainer.GeometryModelsProperty)
        End Get
    End Property
    Protected Shared ReadOnly GeometryModelsPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("GeometryModels", _
                              GetType(System.Collections.ObjectModel.ObservableCollection(Of AllocationViewModel)), GetType(GeometryContainer), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly GeometryModelsProperty As DependencyProperty = _
                             GeometryModelsPropertyKey.DependencyProperty
    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New GeometryContainer
    End Function
    Protected Sub ApplyOffset(vector As Vector)
        For Each _Geometry In _GeometryModels
            _Geometry.ApplyOffset(vector)
        Next
    End Sub
    Protected Overrides Function FreezeCore(isChecking As Boolean) As Boolean
        For Each _Geometry In _GeometryModels
            If TypeOf _Geometry Is GeometryViewModel Then
                DirectCast(_Geometry, GeometryViewModel).Freeze()
                DirectCast(_Geometry, GeometryViewModel).Geometry.Freeze()
            End If
        Next
        Return MyBase.FreezeCore(isChecking)
    End Function
End Class
