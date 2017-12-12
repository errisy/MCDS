Public Class PrimerSynthesis

    Protected WithEvents _OrderItems As System.Collections.ObjectModel.ObservableCollection(Of SynContract.PrimerOrderItem)

    Private _Order As SynContract.PrimerOrder
    Public Property Order As SynContract.PrimerOrder
        Get
            Return _Order
        End Get
        Set(value As SynContract.PrimerOrder)
            _Order = value
            _OrderItems = _Order.Primers
            DataContext = _Order
            RecalculatePrice()
        End Set
    End Property

    Private _Price As SynthenomePrimerPrice
    Public Property Price As SynthenomePrimerPrice
        Get
            Return _Price
        End Get
        Set(value As SynthenomePrimerPrice)
            _Price = value
            gdPriceInfo.DataContext = _Price
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
                di.Length = di.Sequence.Length
                If di.Length < LoginManagement.Price.LongPrimerLengthStart Then
                    If di.PagePurify Then
                        di.Price = di.Length * LoginManagement.Price.PrimerPrice
                    Else
                        di.Price = di.Length * LoginManagement.Price.PagePurifyPrice
                    End If
                Else
                    If di.PagePurify Then
                        di.Price = di.Length * LoginManagement.Price.LongPrimerPrice
                    Else
                        di.Price = di.Length * LoginManagement.Price.LongPagePurifyPrice
                    End If
                End If
                totalPrice += di.Price
                totalLength += di.Length
                totalItems += 1
            End If
        Next
        _Order.TotalPrice = totalPrice
        _Order.TotalLength = totalLength
        _Order.TotalItems = totalItems
    End Sub

    Private Async Sub OrderNow(sender As Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim u As SynContract.Customer = LoginManagement.Customer
            For Each so In _ItemsSourcePrimer
                If so._IsDynamicSelected Then
                    _Order.PreferredVendor.Add(so.WrappedObject)
                End If
            Next
            For Each so In _ItemsSourceScreenPrimer
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
                                                           Return sData.OrderPrimer(u.ID, u.Password, _Order)
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
    Private _ItemsSourcePrimer As System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
    Private _ItemsSourceScreenPrimer As System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
    Private Sub EnzymePurchaseOrder_Loaded(sender As Object, e As Windows.RoutedEventArgs) Handles Me.Loaded
        Dim mode As SynContract.AdvertisementType
        Dim sp = LoginManagement.Advertisements

        _ItemsSourcePrimer = New System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
        _ItemsSourceScreenPrimer = New System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject(Of SynContract.Company))
        gdPrimer.ItemsSource = _ItemsSourcePrimer
        gdScreenPrimer.ItemsSource = _ItemsSourceScreenPrimer
        mode = SynContract.AdvertisementType.PCR
        If sp IsNot Nothing AndAlso sp.Suppliers.ContainsKey(mode) Then
            For Each cp In sp.Suppliers(mode).Companies
                _ItemsSourcePrimer.Add(New SelectableDynamicObject(Of SynContract.Company)(cp) With {._IsDynamicSelected = False})
            Next
        End If
        mode = SynContract.AdvertisementType.Screen
        If sp IsNot Nothing AndAlso sp.Suppliers.ContainsKey(mode) Then
            For Each cp In sp.Suppliers(mode).Companies
                _ItemsSourceScreenPrimer.Add(New SelectableDynamicObject(Of SynContract.Company)(cp) With {._IsDynamicSelected = False})
            Next
        End If
    End Sub

End Class
