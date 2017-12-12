Public Class ConsumerablePurchaseOrder
    Protected WithEvents _OrderItems As System.Collections.ObjectModel.ObservableCollection(Of SynContract.ConsumerableOrderItem)

    Private _Order As SynContract.ConsumerableOrder
    Public Property Order As SynContract.ConsumerableOrder
        Get
            Return _Order
        End Get
        Set(value As SynContract.ConsumerableOrder)
            _Order = value
            _OrderItems = _Order.Consumerables
            DataContext = _Order
            RecalculatePrice()
        End Set
    End Property
    Private Sub _OrderItems_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs) Handles _OrderItems.CollectionChanged
        RecalculatePrice()
    End Sub
    Private Sub ValidCheckChanged(sender As Object, e As System.Windows.RoutedEventArgs)
        RecalculatePrice()
    End Sub
    Private Sub RecalculatePrice()
        Dim totalPrice As Decimal = 0D
        Dim totalLength As Integer = 0
        Dim totalItems As Integer = 0
        For Each di In _OrderItems
            If di.Valid Then
                totalItems += 1
            End If
        Next
        _Order.TotalItems = totalItems
    End Sub
    Private Sub AddItem(sender As Object, e As System.Windows.RoutedEventArgs)
        _OrderItems.Add(New SynContract.ConsumerableOrderItem With {.Name = "<Item>", .Note = "<Note>"})
    End Sub

    Private Sub Navigate(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
        Dim company = CTypeDynamic(Of SynContract.Company)(DirectCast(e.Source, System.Windows.FrameworkContentElement).DataContext)
        Process.Start(company.WebAddress)
    End Sub

    Private _ItemsSourceIncubation As System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
    Private _ItemsSourceExtraction As System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
    Private _ItemsSourceElectrophoresis As System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
    Private _ItemsSourceSequencing As System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
    Private _ItemsSourcePCR As System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
    Private _ItemsSourceExpression As System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
    Private Sub ConsumerablePurchaseOrder_Loaded(sender As Object, e As Windows.RoutedEventArgs) Handles Me.Loaded

        Dim mode As SynContract.AdvertisementType
        'Dim sp = LoginManagement.Advertisements
        _ItemsSourceIncubation = New System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
        _ItemsSourceExtraction = New System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
        _ItemsSourceElectrophoresis = New System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
        _ItemsSourceSequencing = New System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
        _ItemsSourcePCR = New System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
        _ItemsSourceExpression = New System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))

        gdIncubation.ItemsSource = _ItemsSourceIncubation
        gdExtraction.ItemsSource = _ItemsSourceExtraction
        gdElectrophoresis.ItemsSource = _ItemsSourceElectrophoresis
        gdSequencing.ItemsSource = _ItemsSourceSequencing
        gdPCR.ItemsSource = _ItemsSourcePCR
        gdExpression.ItemsSource = _ItemsSourceExpression

        'mode = SynContract.AdvertisementType.Incubation

        'If sp Is Nothing Then Return

        'If sp.Suppliers.ContainsKey(mode) Then
        '    For Each cp In sp.Suppliers(mode).Companies
        '        _ItemsSourceIncubation.Add(New SelectableDynamicObject(Of SynContract.Company)(cp) With {._IsDynamicSelected = False})
        '    Next
        'End If
        'mode = SynContract.AdvertisementType.Extraction
        'If sp.Suppliers.ContainsKey(mode) Then
        '    For Each cp In sp.Suppliers(mode).Companies
        '        _ItemsSourceExtraction.Add(New SelectableDynamicObject(Of SynContract.Company)(cp) With {._IsDynamicSelected = False})
        '    Next
        'End If
        'mode = SynContract.AdvertisementType.Gel
        'If sp.Suppliers.ContainsKey(mode) Then
        '    For Each cp In sp.Suppliers(mode).Companies
        '        _ItemsSourceElectrophoresis.Add(New SelectableDynamicObject(Of SynContract.Company)(cp) With {._IsDynamicSelected = False})
        '    Next
        'End If
        'mode = SynContract.AdvertisementType.SequencingResult
        'If sp.Suppliers.ContainsKey(mode) Then
        '    For Each cp In sp.Suppliers(mode).Companies
        '        _ItemsSourceSequencing.Add(New SelectableDynamicObject(Of SynContract.Company)(cp) With {._IsDynamicSelected = False})
        '    Next
        'End If
        'mode = SynContract.AdvertisementType.PCR
        'If sp.Suppliers.ContainsKey(mode) Then
        '    For Each cp In sp.Suppliers(mode).Companies
        '        _ItemsSourcePCR.Add(New SelectableDynamicObject(Of SynContract.Company)(cp) With {._IsDynamicSelected = False})
        '    Next
        'End If
        'mode = SynContract.AdvertisementType.Expression
        'If sp.Suppliers.ContainsKey(mode) Then
        '    For Each cp In sp.Suppliers(mode).Companies
        '        _ItemsSourceExpression.Add(New SelectableDynamicObject(Of SynContract.Company)(cp) With {._IsDynamicSelected = False})
        '    Next
        'End If
    End Sub
End Class
