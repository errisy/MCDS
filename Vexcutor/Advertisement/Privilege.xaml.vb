Imports System.Windows.Input
Public Class Privilege
    Private Sub Privilege_Loaded(sender As Object, e As Windows.RoutedEventArgs) Handles Me.Loaded
        dgAccepted.ItemsSource = AcceptedList
        dgRequest.ItemsSource = OrderList
        dgUnaccepted.ItemsSource = UnacceptedList
        Refresh()
    End Sub
    Private Async Sub Refresh()
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
                lbInfo.Content = "Obataining information from Server. Please wait..."
                Dim auths = Await Async(Of List(Of SynContract.Authorizee))(Function()
                                                                                Try
                                                                                    Return sData.GetAuthorization(u.ID, u.Password)
                                                                                Catch ex As Exception
                                                                                    Return Nothing
                                                                                End Try
                                                                            End Function)
                UnacceptedList.Clear()
                AcceptedList.Clear()
                For Each au In auths
                    If au.Paid Then
                        AcceptedList.Add(au)
                    Else
                        UnacceptedList.Add(au)
                    End If
                Next
                lbInfo.Content = "List Updated!"
            Else
                lbInfo.Content = "Server Error."
            End If
        Catch ex As Exception
            lbInfo.Content = "Server Error."
        End Try
    End Sub
    Private UnacceptedList As New System.Collections.ObjectModel.ObservableCollection(Of SynContract.Authorizee)
    Private AcceptedList As New System.Collections.ObjectModel.ObservableCollection(Of SynContract.Authorizee)
    Private OrderList As New System.Collections.ObjectModel.ObservableCollection(Of SynContract.Authorizee)
    Private Sub AddSelected(sender As Object, e As MouseButtonEventArgs)
        For Each au In dgUnaccepted.SelectedItems
            If Not OrderList.Contains(au) Then OrderList.Add(au)
        Next
    End Sub
    Private Sub RemoveSelected(sender As Object, e As MouseButtonEventArgs)
        Dim vList As New List(Of Object)
        For Each au In dgRequest.SelectedItems
            If Not vList.Contains(au) Then vList.Add(au)
        Next
        For Each obj In vList
            If OrderList.Contains(obj) Then OrderList.Remove(obj)
        Next
    End Sub
    Private Async Sub OrderAcception(sender As Object, e As MouseButtonEventArgs)
        Try
            Dim u As SynContract.Customer = LoginManagement.Customer
            Dim uList As New List(Of String)
            For Each au In OrderList
                uList.Add(au.Username)
            Next
            lbInfo.Content = "Connecting to Server ..."
            Dim sData = Await Async(Of SynContract.ISynData)(Function() As SynContract.ISynData
                                                                 Try
                                                                     Return WellKnownExtension.GetService(Of SynContract.ISynData)(ServerAddress, ServerPort, ServiceName)
                                                                 Catch ex As Exception
                                                                     Return Nothing
                                                                 End Try
                                                             End Function)
            If TypeOf sData Is SynContract.ISynData Then

                lbInfo.Content = "Ordering..."

                Dim res = Await Async(Of Boolean?)(Function()
                                                       Try
                                                           Return sData.AcceptAuthorization(u.ID, u.Password, uList)
                                                       Catch ex As Exception
                                                           Return Nothing
                                                       End Try
                                                   End Function)
                If res Is Nothing Then
                    lbInfo.Content = "Server Error."
                ElseIf res = True Then
                    lbInfo.Content = "Authorization Accepted. Synthenome will send an email to you regarding this."
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
    Private Sub RefreshAuhorization(sender As Object, e As MouseButtonEventArgs)
        Refresh()
    End Sub

End Class
