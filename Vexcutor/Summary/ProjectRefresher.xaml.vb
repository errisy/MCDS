Public Class ProjectRefresher
    Private Project As WorkSpace
    Private _WorkControl As WorkControl
    Public Property HostWorkControl As WorkControl
        Get
            Return _WorkControl
        End Get
        Set(value As WorkControl)
            _WorkControl = value
            Project = _WorkControl.GetWorkSpace
            Select Case _WorkControl.ProjectServiceStatus
                Case ProjectServiceStatusEnum.None
                    btnAbort.Visibility = Windows.Visibility.Hidden
                    btnRefresh.Visibility = Windows.Visibility.Hidden
                    btnModify.Visibility = Windows.Visibility.Hidden
                    btnAccept.Visibility = Windows.Visibility.Hidden
                Case ProjectServiceStatusEnum.Quotation  'Abandon Refresh
                    rnProjectStatus.Text = "Synthenome has received this quotation. Our engineers will check this project and get back to you soon."
                    btnAbort.Visibility = Windows.Visibility.Visible
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Hidden
                    btnAccept.Visibility = Windows.Visibility.Hidden
                Case ProjectServiceStatusEnum.RequestForModify 'Abandon Refresh Modify Accept
                    rnProjectStatus.Text = "Synthenome's engineers has modified this projects to make it more feasible/reasonable. If you agree, please accept it. If you think there are some improper modifications, please revise this project and send back to us."
                    btnAbort.Visibility = Windows.Visibility.Visible
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Hidden
                    btnAccept.Visibility = Windows.Visibility.Hidden
                Case ProjectServiceStatusEnum.Modified 'Abandon Refresh
                    rnProjectStatus.Text = "You have submitted your revise to us. Our engineers will work on it soon and get back to you."
                    btnAbort.Visibility = Windows.Visibility.Visible
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Hidden
                    btnAccept.Visibility = Windows.Visibility.Hidden
                Case ProjectServiceStatusEnum.ContractorConfirmed 'Abandon Refresh Modify Accept
                    rnProjectStatus.Text = "Our engineers have confirmed that this project is feasible. Please confirm so that we can start the construction."
                    btnAbort.Visibility = Windows.Visibility.Visible
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Visible
                    btnAccept.Visibility = Windows.Visibility.Visible
                Case ProjectServiceStatusEnum.CustomerConfirmed 'Abandon Refresh
                    rnProjectStatus.Text = "You have confirmed. This project is now in progress."
                    btnAbort.Visibility = Windows.Visibility.Visible
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Hidden
                    btnAccept.Visibility = Windows.Visibility.Hidden
                Case ProjectServiceStatusEnum.InProgress 'Abandon Refresh
                    rnProjectStatus.Text = "This project is now in progress. If you want to make any further changes or stop, please click abandon. Fee for all initiated steps will be charged."
                    btnAbort.Visibility = Windows.Visibility.Visible
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Hidden
                    btnAccept.Visibility = Windows.Visibility.Hidden
                Case ProjectServiceStatusEnum.CustomerAbaondoned 'Abandon Refresh Accept
                    rnProjectStatus.Text = "Synthenome has abandoned this project."
                    btnAbort.Visibility = Windows.Visibility.Visible
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Hidden
                    btnAccept.Visibility = Windows.Visibility.Visible
                Case ProjectServiceStatusEnum.CustomerAbaondoned '
                    rnProjectStatus.Text = "You have abandoned this project."
                    btnAbort.Visibility = Windows.Visibility.Hidden
                    btnRefresh.Visibility = Windows.Visibility.Hidden
                    btnModify.Visibility = Windows.Visibility.Hidden
                    btnAccept.Visibility = Windows.Visibility.Hidden
                Case ProjectServiceStatusEnum.Finished 'Abandon Refresh
                    rnProjectStatus.Text = "This project was finished. Shipment will be arranged soon."
                    btnAbort.Visibility = Windows.Visibility.Visible
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Hidden
                    btnAccept.Visibility = Windows.Visibility.Hidden
                Case ProjectServiceStatusEnum.Shipped '
                    rnProjectStatus.Text = "This project was finished and the constructions have been shipped to you. Thank you for choosing synthenome!"
                    btnAbort.Visibility = Windows.Visibility.Hidden
                    btnRefresh.Visibility = Windows.Visibility.Hidden
                    btnModify.Visibility = Windows.Visibility.Hidden
                    btnAccept.Visibility = Windows.Visibility.Hidden
            End Select
        End Set
    End Property
    Public Property ProjectName As String
    Private Async Sub GetLatestVersion()
        If Project IsNot Nothing Then
            Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", Project}}
            Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
            Dim cstOrder As New SynContract.ConstructionOrder With {.Name = ProjectName, .Project = data}
            Try
                Dim u As SynContract.Customer = LoginManagement.Customer
                lbInfo.Content = "Connecting Server ..."
                Dim sData = Await Async(Of SynContract.ISynData)(Function() As SynContract.ISynData
                                                                     Try
                                                                         Return WellKnownExtension.GetService(Of SynContract.ISynData)(ServerAddress, ServerPort, ServiceName)
                                                                     Catch ex As Exception
                                                                         Return Nothing
                                                                     End Try
                                                                 End Function)
                If TypeOf sData Is SynContract.ISynData Then
                    lbInfo.Content = "Retreiving data from Server ..."
                    Dim res = Await Async(Of Byte())(Function()
                                                         Try
                                                             Return sData.RefreshProject(u.ID, u.Password, Project.QuotationID)
                                                         Catch ex As Exception
                                                             Return Nothing
                                                         End Try
                                                     End Function)
                    If res Is Nothing Then
                        lbInfo.Content = "Server Error."
                    ElseIf TypeOf res Is Byte() Then
                        SettingEntry.LoadFromZXMLBytes(Nothing, res)
                        lbInfo.Content = "Update retreived."
                    Else
                        lbInfo.Content = "Username or Password Error."
                    End If
                Else
                    lbInfo.Content = "Server Error."
                End If
            Catch ex As Exception
                lbInfo.Content = "Server Error."
            End Try
        End If
    End Sub

    Private Sub Abort(sender As Object, e As Windows.RoutedEventArgs)
        Select Case _WorkControl.ProjectServiceStatus
            Case ProjectServiceStatusEnum.None

            Case ProjectServiceStatusEnum.Quotation  'Abandon Refresh
                'rnProjectStatus.Text = "Synthenome has received this quotation. Our engineers will check this project and get back to you soon."

            Case ProjectServiceStatusEnum.RequestForModify 'Abandon Refresh Modify Accept
                'rnProjectStatus.Text = "Synthenome's engineers has modified this projects to make it more feasible/reasonable. If you agree, please accept it. If you think there are some improper modifications, please revise this project and send back to us."

            Case ProjectServiceStatusEnum.Modified 'Abandon Refresh
                'rnProjectStatus.Text = "You have submitted your revise to us. Our engineers will work on it soon and get back to you."

            Case ProjectServiceStatusEnum.ContractorConfirmed 'Abandon Refresh Modify Accept
                'rnProjectStatus.Text = "Our engineers have confirmed that this project is feasible. Please confirm so that we can start the construction."

            Case ProjectServiceStatusEnum.CustomerConfirmed 'Abandon Refresh
                'rnProjectStatus.Text = "You have confirmed. This project is now in progress."

            Case ProjectServiceStatusEnum.InProgress 'Abandon Refresh
                'rnProjectStatus.Text = "This project is now in progress. If you want to make any further changes or stop, please click abandon. Fee for all initiated steps will be charged."

            Case ProjectServiceStatusEnum.CustomerAbaondoned 'Abandon Refresh Accept
                'rnProjectStatus.Text = "Synthenome has abandoned this project."

            Case ProjectServiceStatusEnum.CustomerAbaondoned '
                'rnProjectStatus.Text = "You have abandoned this project."

            Case ProjectServiceStatusEnum.Finished 'Abandon Refresh
                'rnProjectStatus.Text = "This project was finished. Shipment will be arranged soon."

            Case ProjectServiceStatusEnum.Shipped '
                'rnProjectStatus.Text = "This project was finished and the constructions have been shipped to you. Thank you for choosing synthenome!"

        End Select
    End Sub

    Private Sub Modify(sender As Object, e As Windows.RoutedEventArgs)
        Select Case _WorkControl.ProjectServiceStatus
            Case ProjectServiceStatusEnum.None

            Case ProjectServiceStatusEnum.Quotation  'Abandon Refresh
                'rnProjectStatus.Text = "Synthenome has received this quotation. Our engineers will check this project and get back to you soon."

            Case ProjectServiceStatusEnum.RequestForModify 'Abandon Refresh Modify Accept
                'rnProjectStatus.Text = "Synthenome's engineers has modified this projects to make it more feasible/reasonable. If you agree, please accept it. If you think there are some improper modifications, please revise this project and send back to us."

            Case ProjectServiceStatusEnum.Modified 'Abandon Refresh
                'rnProjectStatus.Text = "You have submitted your revise to us. Our engineers will work on it soon and get back to you."

            Case ProjectServiceStatusEnum.ContractorConfirmed 'Abandon Refresh Modify Accept
                'rnProjectStatus.Text = "Our engineers have confirmed that this project is feasible. Please confirm so that we can start the construction."

            Case ProjectServiceStatusEnum.CustomerConfirmed 'Abandon Refresh
                'rnProjectStatus.Text = "You have confirmed. This project is now in progress."

            Case ProjectServiceStatusEnum.InProgress 'Abandon Refresh
                'rnProjectStatus.Text = "This project is now in progress. If you want to make any further changes or stop, please click abandon. Fee for all initiated steps will be charged."

            Case ProjectServiceStatusEnum.CustomerAbaondoned 'Abandon Refresh Accept
                'rnProjectStatus.Text = "Synthenome has abandoned this project."

            Case ProjectServiceStatusEnum.CustomerAbaondoned '
                'rnProjectStatus.Text = "You have abandoned this project."

            Case ProjectServiceStatusEnum.Finished 'Abandon Refresh
                'rnProjectStatus.Text = "This project was finished. Shipment will be arranged soon."

            Case ProjectServiceStatusEnum.Shipped '
                'rnProjectStatus.Text = "This project was finished and the constructions have been shipped to you. Thank you for choosing synthenome!"

        End Select
    End Sub

    Private Sub Refresh(sender As Object, e As Windows.RoutedEventArgs)
        GetLatestVersion()
    End Sub

    Private Sub Accept(sender As Object, e As Windows.RoutedEventArgs)
        Select Case _WorkControl.ProjectServiceStatus
            Case ProjectServiceStatusEnum.None

            Case ProjectServiceStatusEnum.Quotation  'Abandon Refresh
                'rnProjectStatus.Text = "Synthenome has received this quotation. Our engineers will check this project and get back to you soon."

            Case ProjectServiceStatusEnum.RequestForModify 'Abandon Refresh Modify Accept
                'rnProjectStatus.Text = "Synthenome's engineers has modified this projects to make it more feasible/reasonable. If you agree, please accept it. If you think there are some improper modifications, please revise this project and send back to us."

            Case ProjectServiceStatusEnum.Modified 'Abandon Refresh
                'rnProjectStatus.Text = "You have submitted your revise to us. Our engineers will work on it soon and get back to you."

            Case ProjectServiceStatusEnum.ContractorConfirmed 'Abandon Refresh Modify Accept
                'rnProjectStatus.Text = "Our engineers have confirmed that this project is feasible. Please confirm so that we can start the construction."

            Case ProjectServiceStatusEnum.CustomerConfirmed 'Abandon Refresh
                'rnProjectStatus.Text = "You have confirmed. This project is now in progress."

            Case ProjectServiceStatusEnum.InProgress 'Abandon Refresh
                'rnProjectStatus.Text = "This project is now in progress. If you want to make any further changes or stop, please click abandon. Fee for all initiated steps will be charged."

            Case ProjectServiceStatusEnum.CustomerAbaondoned 'Abandon Refresh Accept
                'rnProjectStatus.Text = "Synthenome has abandoned this project."

            Case ProjectServiceStatusEnum.CustomerAbaondoned '
                'rnProjectStatus.Text = "You have abandoned this project."

            Case ProjectServiceStatusEnum.Finished 'Abandon Refresh
                'rnProjectStatus.Text = "This project was finished. Shipment will be arranged soon."

            Case ProjectServiceStatusEnum.Shipped '
                'rnProjectStatus.Text = "This project was finished and the constructions have been shipped to you. Thank you for choosing synthenome!"

        End Select
    End Sub
End Class
