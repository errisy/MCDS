Public Class ToolPurchaseOrder
    Protected WithEvents _OrderItems As System.Collections.ObjectModel.ObservableCollection(Of SynContract.ToolEnzymeOrderItem)

    Private _Order As SynContract.ToolEnzymeOrder
    Public Property Order As SynContract.ToolEnzymeOrder
        Get
            Return _Order
        End Get
        Set(value As SynContract.ToolEnzymeOrder)
            _Order = value
            _OrderItems = _Order.ToolEnzymes
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

    Private Async Sub OrderNow(sender As Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim u As SynContract.Customer = LoginManagement.Customer
            For Each so In _ItemsSourcePCR
                If so._IsDynamicSelected Then
                    _Order.PreferredVendor.Add(so.WrappedObject)
                End If
            Next
            For Each so In _ItemsSourceModification
                If so._IsDynamicSelected Then
                    _Order.PreferredVendor.Add(so.WrappedObject)
                End If
            Next
            For Each so In _ItemsSourceLigation
                If so._IsDynamicSelected Then
                    _Order.PreferredVendor.Add(so.WrappedObject)
                End If
            Next
            For Each so In _ItemsSourceScreen
                If so._IsDynamicSelected Then
                    _Order.PreferredVendor.Add(so.WrappedObject)
                End If
            Next
            Dim vCount As Integer = 0
            For Each vIt In _OrderItems
                If vIt.Valid Then vCount += 1
            Next
            If vCount = 0 Then
                lbInfo.Content = "No valid order found."
                Return
            End If
            lbInfo.Content = "Connecting to Server ..."
            Dim sData = Await Async(Of SynContract.ISynData)(Function() As SynContract.ISynData
                                                                 Try
                                                                     Return WellKnownExtension.GetService(Of SynContract.ISynData)(ServerAddress, ServerPort, ServiceName)
                                                                 Catch ex As Exception
                                                                     Return Nothing
                                                                 End Try
                                                             End Function)
            If TypeOf sData Is SynContract.ISynData Then

                lbInfo.Content = "Uploading data to Server ..."

                Dim res = Await Async(Of Boolean?)(Function()
                                                       Try
                                                           Return sData.OrderTool(u.ID, u.Password, _Order)
                                                       Catch ex As Exception
                                                           Return Nothing
                                                       End Try
                                                   End Function)
                If res Is Nothing Then
                    lbInfo.Content = "Server Error."
                ElseIf res = True Then
                    lbInfo.Content = "Order Sent. We will contact you via email to confirm."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Else
                lbInfo.Content = "Server Error."
            End If
        Catch ex As Exception
            lbInfo.Content = "Server Error."
        End Try
    End Sub

    Private Sub Navigate(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
        Dim company = CTypeDynamic(Of SynContract.Company)(DirectCast(e.Source, System.Windows.FrameworkContentElement).DataContext)
        Process.Start(company.WebAddress)
    End Sub

    Private _ItemsSourceModification As System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
    Private _ItemsSourcePCR As System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
    Private _ItemsSourceLigation As System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
    Private _ItemsSourceScreen As System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))

    Private Sub EnzymePurchaseOrder_Loaded(sender As Object, e As Windows.RoutedEventArgs) Handles Me.Loaded
        Dim mode As SynContract.AdvertisementType
        Dim sp = LoginManagement.Advertisements
        If sp Is Nothing Then Return
        _ItemsSourceModification = New System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
        _ItemsSourcePCR = New System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
        _ItemsSourceLigation = New System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
        _ItemsSourceScreen = New System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))

        gdModification.ItemsSource = _ItemsSourceModification
        gdPCR.ItemsSource = _ItemsSourcePCR
        gdLigation.ItemsSource = _ItemsSourceLigation
        gdScreen.ItemsSource = _ItemsSourceScreen

        mode = SynContract.AdvertisementType.Modify
        If sp.Suppliers.ContainsKey(mode) Then
            For Each cp In sp.Suppliers(mode).Companies
                _ItemsSourceModification.Add(New SelectableDynamicObject(Of SynContract.Company)(cp) With {._IsDynamicSelected = False})
            Next
        End If
        mode = SynContract.AdvertisementType.PCR
        If sp.Suppliers.ContainsKey(mode) Then
            For Each cp In sp.Suppliers(mode).Companies
                _ItemsSourcePCR.Add(New SelectableDynamicObject(Of SynContract.Company)(cp) With {._IsDynamicSelected = False})
            Next
        End If
        mode = SynContract.AdvertisementType.Ligation
        If sp.Suppliers.ContainsKey(mode) Then
            For Each cp In sp.Suppliers(mode).Companies
                _ItemsSourceLigation.Add(New SelectableDynamicObject(Of SynContract.Company)(cp) With {._IsDynamicSelected = False})
            Next
        End If
        mode = SynContract.AdvertisementType.Screen
        If sp.Suppliers.ContainsKey(mode) Then
            For Each cp In sp.Suppliers(mode).Companies
                _ItemsSourceScreen.Add(New SelectableDynamicObject(Of SynContract.Company)(cp) With {._IsDynamicSelected = False})
            Next
        End If
    End Sub
End Class
