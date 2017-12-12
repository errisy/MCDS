''' <summary>
''' 将子对象当中的IncludableElement中的Visual抽取到控件自身的VisualCollection当中，从在Panel控件当中直接承载子控件的Visuals。
''' </summary>
''' <remarks></remarks>
Public Class VisualContainerPanel
    Inherits Panel
    Private WithEvents _Visuals As New VisualCollection(Me)
    Public ReadOnly Property Visuals As VisualCollection
        Get
            Return _Visuals
        End Get
    End Property
    Protected Overrides Sub OnVisualChildrenChanged(visualAdded As DependencyObject, visualRemoved As DependencyObject)

        If TypeOf visualAdded Is IncludableElement Then DirectCast(visualAdded, IncludableElement).SubmitVisuals(Me)
        If TypeOf visualRemoved Is IncludableElement Then DirectCast(visualRemoved, IncludableElement).RetrieveVisuals(Me)

        MyBase.OnVisualChildrenChanged(visualAdded, visualRemoved)
    End Sub
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

    Protected Overrides Function MeasureOverride(availableSize As Size) As Size
        For Each child As UIElement In Children
            child.Measure(availableSize)
        Next
        If Children.Count = 0 Then
            Return New Size(0.0#, 0.0#)
        Else
            If TypeOf DataContext Is GeometryContainer Then
                Dim gc As GeometryContainer = DataContext
                Return gc.DesiredSize
            Else
                Return New Size(0.0#, 0.0#)
            End If
        End If
    End Function
    Protected Overrides Function ArrangeOverride(finalSize As Size) As Size
        For Each child As UIElement In Children
            If TypeOf child Is FrameworkElement Then
                Dim fe As FrameworkElement = child
                fe.Width = finalSize.Width
                fe.Height = finalSize.Height
            End If
            child.Arrange(New Rect(0.0#, 0.0#, finalSize.Width, finalSize.Height))
        Next
        Return finalSize
    End Function
End Class
''' <summary>
''' 对内含对象不进行任何布局、将全部AvailableSize提供给子对象的Panel
''' </summary>
''' <remarks></remarks>
Public Class ContainerPanel
    Inherits Panel
    Protected Overrides Function MeasureOverride(availableSize As Size) As Size
        For Each child As UIElement In Children
            child.Measure(availableSize)
        Next
        If Children.Count = 0 Then
            Return New Size(0.0#, 0.0#)
        Else
            If TypeOf DataContext Is GeometryContainer Then
                Dim gc As GeometryContainer = DataContext
                Return gc.DesiredSize
            Else
                Return New Size(0.0#, 0.0#)
            End If
        End If
    End Function
    Protected Overrides Function ArrangeOverride(finalSize As Size) As Size
        For Each child As UIElement In Children
            If TypeOf child Is FrameworkElement Then
                Dim fe As FrameworkElement = child
                fe.Width = finalSize.Width
                fe.Height = finalSize.Height
            End If
            child.Arrange(New Rect(0.0#, 0.0#, finalSize.Width, finalSize.Height))
        Next
        Return finalSize
    End Function
    Public ReadOnly Property VisualChildrenCount2 As Integer
        Get
            Return MyBase.VisualChildrenCount()
        End Get
    End Property
End Class
 