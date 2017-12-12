Public Class IncludableElement
    Inherits FrameworkElement
    Private _Visuals As New VisualCollection(Me)
    Private WithEvents _VisualCollection As New System.Collections.ObjectModel.ObservableCollection(Of Visual)
    Public Sub New()
        SetValue(VisualsPropertyKey, _VisualCollection)
    End Sub
    'IncludableElement -> Visuals As System.Collections.ObjectModel.ObservableCollection(Of Visual) Default: Nothing
    ''' <summary>
    ''' Inherited Classes must use this property to add or remove DrawingVisuals.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Visuals As System.Collections.ObjectModel.ObservableCollection(Of Visual)
        Get
            Return GetValue(IncludableElement.VisualsProperty)
        End Get
    End Property
    Private Shared ReadOnly VisualsPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Visuals", _
                              GetType(System.Collections.ObjectModel.ObservableCollection(Of Visual)), GetType(IncludableElement), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly VisualsProperty As DependencyProperty = _
                             VisualsPropertyKey.DependencyProperty
    ' Provide a required override for the VisualChildrenCount property.
    Protected Overrides ReadOnly Property VisualChildrenCount() As Integer
        Get
            Return _Visuals.Count
        End Get
    End Property
    ' Provide a required override for the GetVisualChild method.
    Protected Overrides Function GetVisualChild(ByVal index As Integer) As Visual
        If index < 0 OrElse index >= _Visuals.Count Then
            Throw New ArgumentOutOfRangeException()
        End If
        Return _Visuals(index)
    End Function
    Public Sub SubmitVisuals(VisualContainer As VisualContainerPanel)
        For Each _visual In _VisualCollection
            If _Visuals.Contains(_visual) Then _Visuals.Remove(_visual)
            If Not VisualContainer.Visuals.Contains(_visual) Then VisualContainer.Visuals.Add(_visual)
        Next
    End Sub
    Public Sub RetrieveVisuals(VisualContainer As VisualContainerPanel)
        For Each _visual In _VisualCollection
            If VisualContainer.Visuals.Contains(_visual) Then
                VisualContainer.Visuals.Remove(_visual)
                If Not _Visuals.Contains(_visual) Then _Visuals.Add(_visual)
            End If
        Next
    End Sub
    Private Sub _VisualCollection_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs) Handles _VisualCollection.CollectionChanged
         If TypeOf Parent Is VisualContainerPanel Then
            Dim visualContainer As VisualContainerPanel = Parent
            If e.OldItems IsNot Nothing Then
                For Each it As Visual In e.OldItems
                    visualContainer.Visuals.Remove(it)
                Next
            End If
            If e.NewItems IsNot Nothing Then
                For Each it As Visual In e.NewItems
                    visualContainer.Visuals.Add(it)
                Next
            End If
        Else
            If e.OldItems IsNot Nothing Then
                For Each it As Visual In e.OldItems
                    If _Visuals.Contains(it) Then _Visuals.Remove(it)
                Next
            End If
            If e.NewItems IsNot Nothing Then
                For Each it As Visual In e.NewItems
                    If Not _Visuals.Contains(it) Then _Visuals.Add(it)
                Next
            End If
        End If
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        If TypeOf DataContext Is AllocationViewModel Then
            DirectCast(DataContext, AllocationViewModel).OnMouseDown(e)
        End If
        MyBase.OnMouseDown(e)
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseButtonEventArgs)
        If TypeOf DataContext Is AllocationViewModel Then
            DirectCast(DataContext, AllocationViewModel).OnMouseUp(e)
        End If
        MyBase.OnMouseUp(e)
    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        If TypeOf DataContext Is AllocationViewModel Then
            DirectCast(DataContext, AllocationViewModel).OnMouseMove(e)
        End If
        MyBase.OnMouseMove(e)
    End Sub
    Protected Overrides Sub OnMouseWheel(e As MouseWheelEventArgs)
        If TypeOf DataContext Is AllocationViewModel Then
            DirectCast(DataContext, AllocationViewModel).OnMouseWheel(e)
        End If
        MyBase.OnMouseWheel(e)
    End Sub
End Class

