Imports System.Windows
Public Class OperationViewContainer
    Inherits System.Windows.Forms.Integration.WindowsFormsHost
    'OperationViewContainer->ChartItems As List(Of DNAInfo) with Event Default: Nothing
    Public Property ChartItems As List(Of DNAInfo)
        Get
            Return GetValue(ChartItemsProperty)
        End Get
        Set(ByVal value As List(Of DNAInfo))
            SetValue(ChartItemsProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ChartItemsProperty As DependencyProperty = _
                           DependencyProperty.Register("ChartItems", _
                           GetType(List(Of DNAInfo)), GetType(OperationViewContainer), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedChartItemsChanged)))
    Private Shared Sub SharedChartItemsChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, OperationViewContainer).ChartItemsChanged(d, e)
    End Sub
    Private Sub ChartItemsChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If Child IsNot Nothing Then
            DirectCast(Child, OperationView).Hosts = e.NewValue
        End If
    End Sub


End Class
