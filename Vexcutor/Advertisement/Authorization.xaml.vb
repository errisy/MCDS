Public Class Authorization


    Private Async Sub AddUser(sender As Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim u As SynContract.Customer = LoginManagement.Customer
            Dim uList As New List(Of String)
            uList.Add(tbAuthorize.Text)
            lbInfo.Content = "Connecting to Server ..."
            Dim sData = Await Async(Of SynContract.ISynData)(Function() As SynContract.ISynData
                                                                 Try
                                                                     Return WellKnownExtension.GetService(Of SynContract.ISynData)(ServerAddress, ServerPort, ServiceName)
                                                                 Catch ex As Exception
                                                                     Return Nothing
                                                                 End Try
                                                             End Function)
            If TypeOf sData Is SynContract.ISynData Then

                lbInfo.Content = "Authorizing..."

                Dim res = Await Async(Of Boolean?)(Function()
                                                       Try
                                                           Return sData.Authorize(u.ID, u.Password, uList)
                                                       Catch ex As Exception
                                                           Return Nothing
                                                       End Try
                                                   End Function)
                If res Is Nothing Then
                    lbInfo.Content = "Server Error."
                ElseIf res = True Then
                    Dim auths = Await Async(Of List(Of SynContract.Authorizee))(Function()
                                                                                    Try
                                                                                        Return sData.GetAuthorizees(u.ID, u.Password)
                                                                                    Catch ex As Exception
                                                                                        Return Nothing
                                                                                    End Try
                                                                                End Function)
                    dgAuth.ItemsSource = auths
                    lbInfo.Content = "Success!"
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
    Private Async Sub RemoveUser(sender As Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim u As SynContract.Customer = LoginManagement.Customer
            Dim uList As New List(Of String)
            Dim az = DirectCast(DirectCast(e.Source, System.Windows.FrameworkElement).DataContext, SynContract.Authorizee)
            uList.Add(az.Username)
            lbInfo.Content = "Connecting to Server ..."
            Dim sData = Await Async(Of SynContract.ISynData)(Function() As SynContract.ISynData
                                                                 Try
                                                                     Return WellKnownExtension.GetService(Of SynContract.ISynData)(ServerAddress, ServerPort, ServiceName)
                                                                 Catch ex As Exception
                                                                     Return Nothing
                                                                 End Try
                                                             End Function)
            If TypeOf sData Is SynContract.ISynData Then

                lbInfo.Content = "Deauthorizing..."

                Dim res = Await Async(Of Boolean?)(Function()
                                                       Try
                                                           Return sData.Deauthorize(u.ID, u.Password, uList)
                                                       Catch ex As Exception
                                                           Return Nothing
                                                       End Try
                                                   End Function)
                If res Is Nothing Then
                    lbInfo.Content = "Server Error."
                ElseIf res = True Then
                    Dim auths = Await Async(Of List(Of SynContract.Authorizee))(Function()
                                                                                    Try
                                                                                        Return sData.GetAuthorizees(u.ID, u.Password)
                                                                                    Catch ex As Exception
                                                                                        Return Nothing
                                                                                    End Try
                                                                                End Function)
                    dgAuth.ItemsSource = auths
                    lbInfo.Content = "Success!"
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

    Private Async Sub Refresh()
        '从服务器获取被授权人列表
        Try
            Dim u As SynContract.Customer = LoginManagement.Customer

            lbInfo.Content = "Connecting to Server ..."
            Dim sData = Await Async(Of SynContract.ISynData)(Function() As SynContract.ISynData
                                                                 Try
                                                                     Return WellKnownExtension.GetService(Of SynContract.ISynData)(ServerAddress, ServerPort, ServiceName)
                                                                 Catch ex As Exception
                                                                     Return Nothing
                                                                 End Try
                                                             End Function)
            If TypeOf sData Is SynContract.ISynData Then
                lbInfo.Content = "Updating..."
                Dim auths = Await Async(Of List(Of SynContract.Authorizee))(Function()
                                                                                Try
                                                                                    Return sData.GetAuthorizees(u.ID, u.Password)
                                                                                Catch ex As Exception
                                                                                    Return Nothing
                                                                                End Try
                                                                            End Function)
                dgAuth.ItemsSource = auths
                lbInfo.Content = "List Updated!"
            Else
                lbInfo.Content = "Server Error."
            End If
        Catch ex As Exception
            lbInfo.Content = "Server Error."
        End Try
    End Sub
 
    Private Sub Authorization_Loaded(sender As Object, e As Windows.RoutedEventArgs) Handles Me.Loaded
        Refresh()
    End Sub
End Class
