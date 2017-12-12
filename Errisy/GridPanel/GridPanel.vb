Public Class GridPanel
    Inherits Panel
    'GridPanel->RowHeight As Double Default: 20#
    Public Property RowHeight As Double
        Get
            Return GetValue(RowHeightProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(RowHeightProperty, value)
        End Set
    End Property
    Public Shared ReadOnly RowHeightProperty As DependencyProperty = _
                           DependencyProperty.Register("RowHeight", _
                           GetType(Double), GetType(GridPanel), _
                           New PropertyMetadata(20.0#))
    'GridPanel->ColumnWidth As Double Default: 20#
    Public Property ColumnWidth As Double
        Get
            Return GetValue(ColumnWidthProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(ColumnWidthProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ColumnWidthProperty As DependencyProperty = _
                           DependencyProperty.Register("ColumnWidth", _
                           GetType(Double), GetType(GridPanel), _
                           New PropertyMetadata(20.0#))

    Public Shared Function GetRow(ByVal element As DependencyObject) As Integer
        If element Is Nothing Then
            Throw New ArgumentNullException("element")
        End If
        Return element.GetValue(RowProperty)
    End Function
    Public Shared Sub SetRow(ByVal element As DependencyObject, ByVal value As Integer)
        If element Is Nothing Then
            Throw New ArgumentNullException("element")
        End If
        element.SetValue(RowProperty, value)
    End Sub
    Public Shared ReadOnly RowProperty As  _
                           DependencyProperty = DependencyProperty.RegisterAttached("Row", _
                           GetType(Integer), GetType(GridPanel), _
                           New PropertyMetadata(Nothing))
    Public Shared Function GetColumn(ByVal element As DependencyObject) As Integer
        If element Is Nothing Then
            Throw New ArgumentNullException("element")
        End If
        Return element.GetValue(ColumnProperty)
    End Function
    Public Shared Sub SetColumn(ByVal element As DependencyObject, ByVal value As Integer)
        If element Is Nothing Then
            Throw New ArgumentNullException("element")
        End If
        element.SetValue(ColumnProperty, value)
    End Sub
    Public Shared ReadOnly ColumnProperty As  _
                           DependencyProperty = DependencyProperty.RegisterAttached("Column", _
                           GetType(Integer), GetType(GridPanel), _
                           New PropertyMetadata(Nothing))



    Protected Overrides Function MeasureOverride(availableSize As Size) As Size
        For Each ui As UIElement In Children
            ui.Measure(New Size(ColumnWidth, RowHeight))
        Next
        Return MyBase.MeasureOverride(availableSize)
    End Function
    Protected Overrides Function ArrangeOverride(finalSize As Size) As Size
        For Each ui As UIElement In Children
            Dim row As Integer = GetRow(ui)
            Dim column As Integer = GetColumn(ui)
            ui.Arrange(New Rect(New Point(ColumnWidth * column, RowHeight * row), New Size(ColumnWidth, RowHeight)))
        Next
        Return MyBase.ArrangeOverride(finalSize)
    End Function
End Class
