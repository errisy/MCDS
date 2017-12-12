Public Class GeneSynthesisPanel
    Public Sub New()
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Vectors.Tag = New String() {"pUC", "p15a", "Custom"}
    End Sub
    Private _Order As SynContract.GeneSynthesisOrder
    Private WithEvents _OrderItems As System.Collections.ObjectModel.ObservableCollection(Of SynContract.DNAOrderItem)
    Public Property Order As SynContract.GeneSynthesisOrder
        Get
            Return DataContext
        End Get
        Set(value As SynContract.GeneSynthesisOrder)
            _Order = value
            _OrderItems = value.DNAList
            DataContext = value
            Price = LoginManagement.Price
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
        For Each di In _OrderItems
            If di.Valid Then
                If di.Length <= _Price.MinimiumChargeLength Then
                    di.Price = LoginManagement.Price.MinimiumSynthesisPrice
                Else
                    di.Price = di.Length * LoginManagement.Price.WorldWidePrice
                End If
                If di.Vector = "Custom" Then di.Price += LoginManagement.Price.SynthesisSubClonePrice
                totalPrice += di.Price
                totalLength += di.Length
            End If
        Next
        _Order.TotalPrice = totalPrice
        _Order.TotalLength = totalLength
    End Sub
    Private _Price As New SynContract.SynthenomePrice
    Public Property Price As SynContract.SynthenomePrice
        Get
            Return _Price
        End Get
        Set(value As SynContract.SynthenomePrice)
            _Price = value
            gdPriceInfo.DataContext = _Price
            'synVectorColumn.ItemsSource = _Price.SynthesisSubCloneVector
            Vectors.Tag = _Price.SynthesisSubCloneVector
        End Set
    End Property
    Private Async Sub OrderNow(sender As Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim u As SynContract.Customer = LoginManagement.Customer
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
                                                           Return sData.SynthesizeDNA(u.ID, u.Password, _Order)
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
  
End Class
