Public Class EnzymePurchaseOrder
    Protected WithEvents _OrderItems As System.Collections.ObjectModel.ObservableCollection(Of SynContract.EnzymeOrderItem)

    Private _Order As SynContract.EnzymeOrder
    Public Property Order As SynContract.EnzymeOrder
        Get
            Return _Order
        End Get
        Set(value As SynContract.EnzymeOrder)
            _Order = value
            _OrderItems = _Order.Enzymes
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



    Private Sub Navigate(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
        Dim company = CTypeDynamic(Of SynContract.Company)(DirectCast(e.Source, System.Windows.FrameworkContentElement).DataContext)
        Process.Start(company.WebAddress)
    End Sub

    Private _ItemsSourceEnzyme As System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
    Private Sub EnzymePurchaseOrder_Loaded(sender As Object, e As Windows.RoutedEventArgs) Handles Me.Loaded
        'Dim mode As SynContract.AdvertisementType = SynContract.AdvertisementType.Enzyme
        ''Dim sp = LoginManagement.Advertisements
        'If sp Is Nothing Then Return
        '_ItemsSourceEnzyme = New System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
        'gdDigestion.ItemsSource = _ItemsSourceEnzyme
        'If sp.Suppliers.ContainsKey(mode) Then
        '    For Each cp In sp.Suppliers(mode).Companies
        '        _ItemsSourceEnzyme.Add(New SelectableDynamicObject(Of SynContract.Company)(cp) With {._IsDynamicSelected = False})
        '    Next
        'End If
    End Sub
End Class
