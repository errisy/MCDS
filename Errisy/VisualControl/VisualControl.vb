Public Class VisualControl
    Inherits System.Windows.FrameworkElement
    Private _VisualCollection As New VisualCollection(Me)
    Private _GeometryContainer As GeometryContainer
    Private WithEvents _GeometryModels As System.Collections.ObjectModel.ObservableCollection(Of AllocationViewModel)
    Private _Table As New Hashtable
    Shared Sub New()
        FrameworkElement.HorizontalAlignmentProperty.OverrideMetadata(GetType(VisualControl), New FrameworkPropertyMetadata(HorizontalAlignment.Left))
        FrameworkElement.VerticalAlignmentProperty.OverrideMetadata(GetType(VisualControl), New FrameworkPropertyMetadata(VerticalAlignment.Top))
    End Sub
    Protected Overrides Function GetVisualChild(index As Integer) As Visual
        Return _VisualCollection(index)
    End Function
    Protected Overrides ReadOnly Property VisualChildrenCount As Integer
        Get
            Return _VisualCollection.Count
        End Get
    End Property
    'VisualControl->GeometryContainer As GeometryContainer with Event Default: Nothing
    Public Property GeometryContainer As GeometryContainer
        Get
            Return GetValue(GeometryContainerProperty)
        End Get
        Set(ByVal value As GeometryContainer)
            SetValue(GeometryContainerProperty, value)
        End Set
    End Property
    Public Shared ReadOnly GeometryContainerProperty As DependencyProperty = _
                           DependencyProperty.Register("GeometryContainer", _
                           GetType(GeometryContainer), GetType(VisualControl), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedGeometryContainerChanged)))
    Private Shared Sub SharedGeometryContainerChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, VisualControl).GeometryContainerChanged(d, e)
    End Sub
    Private Sub GeometryContainerChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If TypeOf e.NewValue Is GeometryContainer Then
            If _GeometryModels IsNot Nothing Then ClearVisuals(_GeometryModels)
            _GeometryModels = DirectCast(e.NewValue, GeometryContainer).GeometryModels
            InitializeVisuals(_GeometryModels)
        Else
            If _GeometryModels IsNot Nothing Then ClearVisuals(_GeometryModels)
            _GeometryModels = Nothing
        End If
    End Sub
    Protected Overrides Function MeasureOverride(availableSize As Size) As Size
        If GeometryContainer Is Nothing Then
            Return New Size()
        Else
            Debug.WriteLine(GeometryContainer.DesiredSize.ToString)
            Return GeometryContainer.DesiredSize
        End If
    End Function
    Protected Overrides Function ArrangeOverride(finalSize As Size) As Size
        If GeometryContainer Is Nothing Then
            Me.Width = 0.0#
            Me.Height = 0.0#
            Return New Size()
        Else
            Dim _DesiredSize = GeometryContainer.DesiredSize
            Width = _DesiredSize.Width
            Height = _DesiredSize.Height
            Return _DesiredSize
        End If
    End Function
    Private Sub InitializeVisuals(_Models As System.Collections.ObjectModel.ObservableCollection(Of AllocationViewModel))
        For Each it In _Models
            Dim _Visual = GenerateVisual(it)
            _VisualCollection.Add(_Visual)
            _Table.Add(it, _Visual)
            AddHandler it.ViewModelChanged, AddressOf ViewModelChanged
        Next
    End Sub
    Private Sub ClearVisuals(_Models As System.Collections.ObjectModel.ObservableCollection(Of AllocationViewModel))
        For Each it In _Models
            If _Table.ContainsKey(it) Then
                Dim _Visual = _Table(it)
                _VisualCollection.Remove(_Visual)
                _Table.Remove(it)
                RemoveHandler it.ViewModelChanged, AddressOf ViewModelChanged
            End If
        Next
        _VisualCollection.Clear()
    End Sub
    Private Sub _GeometryModels_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs) Handles _GeometryModels.CollectionChanged
        If e.OldItems IsNot Nothing Then
            For Each it As AllocationViewModel In e.OldItems
                If _Table.ContainsKey(it) Then
                    Dim _Visual = _Table(it)
                    _VisualCollection.Remove(_Visual)
                    _Table.Remove(it)
                    RemoveHandler it.ViewModelChanged, AddressOf ViewModelChanged
                End If
            Next
        End If
        If e.NewItems IsNot Nothing Then
            For Each it As AllocationViewModel In e.NewItems
                Dim _Visual = GenerateVisual(it)
                _VisualCollection.Insert(e.NewStartingIndex, _Visual)
                _Table.Add(it, _Visual)
                AddHandler it.ViewModelChanged, AddressOf ViewModelChanged
            Next
        End If
    End Sub
    Private Sub ViewModelChanged(sender As Object, e As EventArgs)
        Dim _Visual As DrawingVisual = _Table(sender)
        Dim _VisualIndex As Integer = _VisualCollection.IndexOf(_Visual)
        _VisualCollection.Remove(_Visual)
        _Visual = GenerateVisual(sender)
        If _VisualIndex > -1 Then _VisualCollection.Insert(_VisualIndex, _Visual)
        _Table(sender) = _Visual
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        For i As Integer = _GeometryModels.Count - 1 To 0 Step -1
            If _GeometryModels(i).GetSpaceGeometry.FillContains(e.GetPosition(Me), 1.0#, ToleranceType.Absolute) Then
                _GeometryModels(i).OnMouseDown(e)
                Exit For
            End If
        Next
        MyBase.OnMouseDown(e)
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseButtonEventArgs)
        For i As Integer = _GeometryModels.Count - 1 To 0 Step -1
            If _GeometryModels(i).GetSpaceGeometry.FillContains(e.GetPosition(Me), 1.0#, ToleranceType.Relative) Then
                _GeometryModels(i).OnMouseUp(e)
                Exit For
            End If
        Next
        MyBase.OnMouseUp(e)
    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        For i As Integer = _GeometryModels.Count - 1 To 0 Step -1
            If _GeometryModels(i).GetSpaceGeometry.FillContains(e.GetPosition(Me), 1.0#, ToleranceType.Relative) Then
                _GeometryModels(i).OnMouseMove(e)
                Exit For
            End If
        Next
        MyBase.OnMouseMove(e)
    End Sub
    Protected Overrides Sub OnMouseWheel(e As MouseWheelEventArgs)
        For i As Integer = _GeometryModels.Count - 1 To 0 Step -1
            If _GeometryModels(i).GetSpaceGeometry.FillContains(e.GetPosition(Me), 1.0#, ToleranceType.Relative) Then
                _GeometryModels(i).OnMouseWheel(e)
                Exit For
            End If
        Next
        MyBase.OnMouseWheel(e)
    End Sub
    Private Shared Function GenerateVisual(_ViewModel As AllocationViewModel) As DrawingVisual
        If TypeOf _ViewModel Is GeometryViewModel Then
            Dim model As GeometryViewModel = _ViewModel
            Dim drawing As New DrawingVisual
            Using dc = drawing.RenderOpen
                dc.DrawGeometry(model.Fill, New Pen(model.Stroke, model.StrokeThickness), model.Geometry)
            End Using
            Return drawing
        ElseIf TypeOf _ViewModel Is FormatedTextViewModel Then
            Dim model As FormatedTextViewModel = _ViewModel
            Dim drawing As New DrawingVisual
            Using dc = drawing.RenderOpen
                dc.DrawText(model.FormatedText, model.Location)
            End Using
            Return drawing
        ElseIf TypeOf _ViewModel Is LineViewModel Then
            Dim model As LineViewModel = _ViewModel
            Dim drawing As New DrawingVisual
            Using dc = drawing.RenderOpen
                dc.DrawGeometry(Nothing, New Pen(model.Stroke, model.StrokeThickness), model.Geometry)
            End Using
            Return drawing
        ElseIf TypeOf _ViewModel Is GeometryTextViewModel Then
            Dim model As GeometryTextViewModel = _ViewModel
            Dim drawing As New DrawingVisual
            Using dc = drawing.RenderOpen
                dc.DrawGeometry(model.Fill, New Pen(model.Stroke, model.StrokeThickness), model.Geometry)
            End Using
            Return drawing
        End If
    End Function
End Class

Public Class GeometryVisual
    Inherits DrawingVisual
    Public Sub New(_ViewModel As AllocationViewModel)

        SetValue(ViewModelPropertyKey, _ViewModel)
    End Sub
    'GeometryVisual -> ViewModel As GeometryViewModel Default: Nothing
    Public ReadOnly Property ViewModel As GeometryViewModel
        Get
            Return GetValue(GeometryVisual.ViewModelProperty)
        End Get
    End Property
    Private Shared ReadOnly ViewModelPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("ViewModel", _
                              GetType(GeometryViewModel), GetType(GeometryVisual), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly ViewModelProperty As DependencyProperty = _
                             ViewModelPropertyKey.DependencyProperty

End Class