Public Class ProjectUpdater
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
                Case ProjectServiceStatusEnum.Quotation 'Abandon Refresh Modify Accept
                    rnProjectStatus.Text = "Customer has sent this project to us for quotation. Please check if it is feasible for you to perform this construction. If you find no issue, please click confirm. In case you find inoperable parts, please modify them and click modify."
                    btnAbort.Visibility = Windows.Visibility.Visible
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Visible
                    btnAccept.Visibility = Windows.Visibility.Visible
                Case ProjectServiceStatusEnum.RequestForModify 'Abandon Refresh 
                    rnProjectStatus.Text = "Your modification has been sent to customer for review."
                    btnAbort.Visibility = Windows.Visibility.Visible
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Hidden
                    btnAccept.Visibility = Windows.Visibility.Hidden
                Case ProjectServiceStatusEnum.Modified 'Abandon Refresh Modify Accept
                    rnProjectStatus.Text = "Customer has made more modifications. Please check if it is feasible for you to perform this construction. If you find no issue, please click confirm. In case you find inoperable parts, please modify them and click modify."
                    btnAbort.Visibility = Windows.Visibility.Visible
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Visible
                    btnAccept.Visibility = Windows.Visibility.Visible
                Case ProjectServiceStatusEnum.ContractorConfirmed 'Abandon Refresh
                    rnProjectStatus.Text = "You have confirmed that you can perform this construction. We now will request customer to confirm before you can start."
                    btnAbort.Visibility = Windows.Visibility.Visible
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Hidden
                    btnAccept.Visibility = Windows.Visibility.Hidden
                Case ProjectServiceStatusEnum.CustomerConfirmed 'Abandon Refresh Update
                    rnProjectStatus.Text = "Customer has confirmed. Please start this project."
                    btnAbort.Visibility = Windows.Visibility.Visible
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Visible
                    btnAccept.Visibility = Windows.Visibility.Hidden
                Case ProjectServiceStatusEnum.InProgress 'Abandon Refresh Update Finish
                    rnProjectStatus.Text = "This project is currently in progress. Please update your progress when appliable. When finished, please click finish. If you are unable to continue, or click abandon."
                    btnAbort.Visibility = Windows.Visibility.Visible
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Visible
                    btnAccept.Visibility = Windows.Visibility.Visible
                Case ProjectServiceStatusEnum.ContractorAbandoned 'Modify(Resuem)
                    rnProjectStatus.Text = "You have abandoned this project."
                    elModify.Content = "Resume"
                    btnAbort.Visibility = Windows.Visibility.Hidden
                    btnRefresh.Visibility = Windows.Visibility.Visible
                    btnModify.Visibility = Windows.Visibility.Hidden
                    btnAccept.Visibility = Windows.Visibility.Hidden
                Case ProjectServiceStatusEnum.CustomerAbaondoned '
                    rnProjectStatus.Text = "Customer has abandoned this project."
                    btnAbort.Visibility = Windows.Visibility.Hidden
                    btnRefresh.Visibility = Windows.Visibility.Hidden
                    btnModify.Visibility = Windows.Visibility.Hidden
                    btnAccept.Visibility = Windows.Visibility.Hidden
                Case ProjectServiceStatusEnum.Finished 'Modify(Resuem)
                    rnProjectStatus.Text = "This project was finished. Please arragne shipment as soon as possible."
                    elModify.Content = "Resume"
                    btnAbort.Visibility = Windows.Visibility.Hidden
                    btnRefresh.Visibility = Windows.Visibility.Hidden
                    btnModify.Visibility = Windows.Visibility.Visible
                    btnAccept.Visibility = Windows.Visibility.Hidden
                Case ProjectServiceStatusEnum.Shipped 'Modify(Resuem)
                    rnProjectStatus.Text = "Congratulations! Thank you for joining Synthenome! In case you need to Resume, click [Resume] button."
                    elModify.Content = "Resume"
                    btnAbort.Visibility = Windows.Visibility.Hidden
                    btnRefresh.Visibility = Windows.Visibility.Hidden
                    btnModify.Visibility = Windows.Visibility.Visible
                    btnAccept.Visibility = Windows.Visibility.Hidden
            End Select
        End Set
    End Property
    Public Property ProjectName As String
    Private Async Sub UpdateProgress(sender As Object, e As System.Windows.RoutedEventArgs)
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

                    lbInfo.Content = "Uploading data to Server ..."

                    Dim res = Await Async(Of Boolean?)(Function()
                                                           Try
                                                               Return sData.UpdateProject(u.ID, u.Password, cstOrder)
                                                           Catch ex As Exception
                                                               Return Nothing
                                                           End Try
                                                       End Function)
                    If res Is Nothing Then
                        lbInfo.Content = "Server Error."
                    ElseIf res = True Then
                        lbInfo.Content = "Project Updated."
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

    Private Async Sub ModifyProgress(sender As Object, e As Windows.RoutedEventArgs)
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

                    lbInfo.Content = "Uploading data to Server ..."

                    Dim res = Await Async(Of Boolean?)(Function()
                                                           Try
                                                               Return sData.UpdateProject(u.ID, u.Password, cstOrder)
                                                           Catch ex As Exception
                                                               Return Nothing
                                                           End Try
                                                       End Function)
                    If res Is Nothing Then
                        lbInfo.Content = "Server Error."
                    ElseIf res = True Then
                        lbInfo.Content = "Project Updated."
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

    Private Async Sub ConfirmProgress(sender As Object, e As Windows.RoutedEventArgs)
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

                    lbInfo.Content = "Uploading data to Server ..."

                    Dim res = Await Async(Of Boolean?)(Function()
                                                           Try
                                                               Return sData.ContractorConfirmProject(u.ID, u.Password, Project.QuotationID)
                                                           Catch ex As Exception
                                                               Return Nothing
                                                           End Try
                                                       End Function)
                    If res Is Nothing Then
                        lbInfo.Content = "Server Error."
                    ElseIf res = True Then
                        lbInfo.Content = "Project Updated."
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

    Private Async Function PackedQuery(Of T)(Method As Func(Of SynContract.ISynData, T)) As Threading.Tasks.Task(Of T)
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

                    lbInfo.Content = "Uploading data to Server ..."

                    Dim res = Await Async(Of T)(Function()
                                                    Try
                                                        Return Method.Invoke(sData)
                                                    Catch ex As Exception
                                                        Return Nothing
                                                    End Try
                                                End Function)
                    If res Is Nothing Then
                        lbInfo.Content = "Server Error."
                        Return Nothing
                    Else
                        Return res
                    End If
                Else
                    lbInfo.Content = "Server Error."
                End If
            Catch ex As Exception
                lbInfo.Content = "Server Error."
                Return Nothing
            End Try
            Return Nothing
        End If
        Return Nothing
    End Function


    Private Async Sub Abort(sender As Object, e As Windows.RoutedEventArgs)
        Select Case _WorkControl.ProjectServiceStatus
            Case ProjectServiceStatusEnum.None

            Case ProjectServiceStatusEnum.Quotation 'Abandon Refresh Modify Accept
                'rnProjectStatus.Text = "Customer has sent this project to us for quotation. Please check if it is feasible for you to perform this construction. If you find no issue, please click confirm. In case you find inoperable parts, please modify them and click modify."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.ContractorAbortProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, New SynContract.ConstructionOrder With {.ProjectID = _WorkControl.QuotationID, .Project = data}))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Aborted."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.RequestForModify 'Abandon Refresh 
                'rnProjectStatus.Text = "Your modification has been sent to customer for review."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.ContractorAbortProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, New SynContract.ConstructionOrder With {.ProjectID = _WorkControl.QuotationID, .Project = Data}))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Aborted."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.Modified 'Abandon Refresh Modify Accept
                'rnProjectStatus.Text = "Customer has made more modifications. Please check if it is feasible for you to perform this construction. If you find no issue, please click confirm. In case you find inoperable parts, please modify them and click modify."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.ContractorAbortProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, New SynContract.ConstructionOrder With {.ProjectID = _WorkControl.QuotationID, .Project = Data}))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Aborted."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.ContractorConfirmed 'Abandon Refresh
                'rnProjectStatus.Text = "You have confirmed that you can perform this construction. We now will request customer to confirm before you can start."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.ContractorAbortProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, New SynContract.ConstructionOrder With {.ProjectID = _WorkControl.QuotationID, .Project = Data}))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Aborted."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.CustomerConfirmed 'Abandon Refresh Update
                'rnProjectStatus.Text = "Customer has confirmed. Please start this project."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.ContractorAbortProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, New SynContract.ConstructionOrder With {.ProjectID = _WorkControl.QuotationID, .Project = Data}))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Aborted."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.InProgress 'Abandon Refresh Update Finish
                'rnProjectStatus.Text = "This project is currently in progress. Please update your progress when appliable. When finished, please click finish. If you are unable to continue, or click abandon."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.ContractorAbortProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, New SynContract.ConstructionOrder With {.ProjectID = _WorkControl.QuotationID, .Project = Data}))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Aborted."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.ContractorAbandoned 'Abandon Refresh Update
                'rnProjectStatus.Text = "You have abandoned this project."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.ContractorAbortProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, New SynContract.ConstructionOrder With {.ProjectID = _WorkControl.QuotationID, .Project = Data}))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Aborted."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.CustomerAbaondoned '
                'rnProjectStatus.Text = "Customer has abandoned this project."

            Case ProjectServiceStatusEnum.Finished 'Abandon Refresh Ship
                'rnProjectStatus.Text = "This project was finished. Please arragne shipment as soon as possible."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.ContractorAbortProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, New SynContract.ConstructionOrder With {.ProjectID = _WorkControl.QuotationID, .Project = Data}))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Aborted."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.Shipped '
                'rnProjectStatus.Text = "Congratulations! Thank you for joining Synthenome!"

        End Select
    End Sub

    Private Async Sub Modify(sender As Object, e As Windows.RoutedEventArgs)
        Select Case _WorkControl.ProjectServiceStatus
            Case ProjectServiceStatusEnum.None

            Case ProjectServiceStatusEnum.Quotation 'Abandon Refresh Modify Accept
                'rnProjectStatus.Text = "Customer has sent this project to us for quotation. Please check if it is feasible for you to perform this construction. If you find no issue, please click confirm. In case you find inoperable parts, please modify them and click modify."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.RequestForModifyProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, New SynContract.ConstructionOrder With {.ProjectID = _WorkControl.QuotationID, .Project = data}))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Modified."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.RequestForModify 'Abandon Refresh 
                'rnProjectStatus.Text = "Your modification has been sent to customer for review."
           
            Case ProjectServiceStatusEnum.Modified 'Abandon Refresh Modify Accept
                'rnProjectStatus.Text = "Customer has made more modifications. Please check if it is feasible for you to perform this construction. If you find no issue, please click confirm. In case you find inoperable parts, please modify them and click modify."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.RequestForModifyProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, New SynContract.ConstructionOrder With {.ProjectID = _WorkControl.QuotationID, .Project = data}))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Modified."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.ContractorConfirmed 'Abandon Refresh
                'rnProjectStatus.Text = "You have confirmed that you can perform this construction. We now will request customer to confirm before you can start."

            Case ProjectServiceStatusEnum.CustomerConfirmed 'Abandon Refresh Update
                'rnProjectStatus.Text = "Customer has confirmed. Please start this project."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.RequestForModifyProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, New SynContract.ConstructionOrder With {.ProjectID = _WorkControl.QuotationID, .Project = data}))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Modified."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.InProgress 'Abandon Refresh Update(Modify) Accept(Finish)
                'rnProjectStatus.Text = "This project is currently in progress. Please update your progress when appliable. When finished, please click finish. If you are unable to continue, or click abandon."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.UpdateProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, New SynContract.ConstructionOrder With {.ProjectID = _WorkControl.QuotationID, .Project = data}))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Modified."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.ContractorAbandoned 'Abandon Refresh Modify(Resume) Update 
                'rnProjectStatus.Text = "You have abandoned this project."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.ContractorResumeProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, _WorkControl.QuotationID))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Aborted."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.CustomerAbaondoned '
                'rnProjectStatus.Text = "Customer has abandoned this project."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.ContractorResumeProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, _WorkControl.QuotationID))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Resumed."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.Finished 'Abandon Refresh Ship
                'rnProjectStatus.Text = "This project was finished. Please arragne shipment as soon as possible."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.ContractorResumeProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, _WorkControl.QuotationID))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Resumed."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.Shipped '
                'rnProjectStatus.Text = "Congratulations! Thank you for joining Synthenome!"
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Boolean?)(Function(sData) sData.ContractorResumeProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, _WorkControl.QuotationID))
                If res IsNot Nothing AndAlso res = True Then
                    lbInfo.Content = "Project Resumed."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
        End Select
    End Sub

    Private Async Sub Refresh(sender As Object, e As Windows.RoutedEventArgs)
        Select Case _WorkControl.ProjectServiceStatus
            Case ProjectServiceStatusEnum.None

            Case ProjectServiceStatusEnum.Quotation 'Abandon Refresh Modify Accept
                'rnProjectStatus.Text = "Customer has sent this project to us for quotation. Please check if it is feasible for you to perform this construction. If you find no issue, please click confirm. In case you find inoperable parts, please modify them and click modify."
                Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", _WorkControl.GetWorkSpace}}
                Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
                Dim res = Await PackedQuery(Of Byte())(Function(sData) sData.RefreshProject(LoginManagement.Customer.ID, LoginManagement.Customer.Password, _WorkControl.QuotationID))
                If res IsNot Nothing Then
                    lbInfo.Content = "Project Modified."
                ElseIf TypeOf res Is Byte() Then
                    SettingEntry.LoadFromZXMLBytes(Nothing, res)
                    lbInfo.Content = "Update retreived."
                Else
                    lbInfo.Content = "Username or Password Error."
                End If
            Case ProjectServiceStatusEnum.RequestForModify 'Abandon Refresh 
                'rnProjectStatus.Text = "Your modification has been sent to customer for review."

            Case ProjectServiceStatusEnum.Modified 'Abandon Refresh Modify Accept
                'rnProjectStatus.Text = "Customer has made more modifications. Please check if it is feasible for you to perform this construction. If you find no issue, please click confirm. In case you find inoperable parts, please modify them and click modify."

            Case ProjectServiceStatusEnum.ContractorConfirmed 'Abandon Refresh
                'rnProjectStatus.Text = "You have confirmed that you can perform this construction. We now will request customer to confirm before you can start."

            Case ProjectServiceStatusEnum.CustomerConfirmed 'Abandon Refresh Update
                'rnProjectStatus.Text = "Customer has confirmed. Please start this project."

            Case ProjectServiceStatusEnum.InProgress 'Abandon Refresh Update Finish
                'rnProjectStatus.Text = "This project is currently in progress. Please update your progress when appliable. When finished, please click finish. If you are unable to continue, or click abandon."

            Case ProjectServiceStatusEnum.ContractorAbandoned 'Abandon Refresh Update
                'rnProjectStatus.Text = "You have abandoned this project."

            Case ProjectServiceStatusEnum.CustomerAbaondoned '
                'rnProjectStatus.Text = "Customer has abandoned this project."

            Case ProjectServiceStatusEnum.Finished 'Abandon Refresh Ship
                'rnProjectStatus.Text = "This project was finished. Please arragne shipment as soon as possible."

            Case ProjectServiceStatusEnum.Shipped '
                'rnProjectStatus.Text = "Congratulations! Thank you for joining Synthenome!"

        End Select
    End Sub

    Private Sub Accept(sender As Object, e As Windows.RoutedEventArgs)
        Select Case _WorkControl.ProjectServiceStatus
            Case ProjectServiceStatusEnum.None

            Case ProjectServiceStatusEnum.Quotation 'Abandon Refresh Modify Accept
                'rnProjectStatus.Text = "Customer has sent this project to us for quotation. Please check if it is feasible for you to perform this construction. If you find no issue, please click confirm. In case you find inoperable parts, please modify them and click modify."

            Case ProjectServiceStatusEnum.RequestForModify 'Abandon Refresh 
                'rnProjectStatus.Text = "Your modification has been sent to customer for review."

            Case ProjectServiceStatusEnum.Modified 'Abandon Refresh Modify Accept
                'rnProjectStatus.Text = "Customer has made more modifications. Please check if it is feasible for you to perform this construction. If you find no issue, please click confirm. In case you find inoperable parts, please modify them and click modify."

            Case ProjectServiceStatusEnum.ContractorConfirmed 'Abandon Refresh
                'rnProjectStatus.Text = "You have confirmed that you can perform this construction. We now will request customer to confirm before you can start."

            Case ProjectServiceStatusEnum.CustomerConfirmed 'Abandon Refresh Update
                'rnProjectStatus.Text = "Customer has confirmed. Please start this project."

            Case ProjectServiceStatusEnum.InProgress 'Abandon Refresh Update Finish
                'rnProjectStatus.Text = "This project is currently in progress. Please update your progress when appliable. When finished, please click finish. If you are unable to continue, or click abandon."

            Case ProjectServiceStatusEnum.ContractorAbandoned 'Abandon Refresh Update
                'rnProjectStatus.Text = "You have abandoned this project."

            Case ProjectServiceStatusEnum.CustomerAbaondoned '
                'rnProjectStatus.Text = "Customer has abandoned this project."

            Case ProjectServiceStatusEnum.Finished 'Abandon Refresh Ship
                'rnProjectStatus.Text = "This project was finished. Please arragne shipment as soon as possible."

            Case ProjectServiceStatusEnum.Shipped '
                'rnProjectStatus.Text = "Congratulations! Thank you for joining Synthenome!"

        End Select
    End Sub
End Class
