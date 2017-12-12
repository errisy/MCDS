Public Class ConstructionProject

    Public Property Project As WorkSpace
    Public Property ProjectName As String

    'Private Async Sub GetQuotation(sender As Object, e As System.Windows.RoutedEventArgs)
    '    If Project IsNot Nothing Then
    '        If Project.ChartItems.Count = 0 Then
    '            lbInfo.Content = "There is an empty project."
    '            Return
    '        End If
    '        Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", Project}}
    '        Dim data As Byte() = SettingEntry.SaveToZXMLBytes(DC)
    '        Dim cstOrder As New SynContract.ConstructionOrder With {.Name = ProjectName, .Project = data}
    '        Try
    '            Dim u As SynContract.Customer = LoginManagement.Customer
    '            lbInfo.Content = "Connecting Server ..."
    '            Dim sData = Await Async(Of SynContract.ISynData)(Function() As SynContract.ISynData
    '                                                                 Try
    '                                                                     Return WellKnownExtension.GetService(Of SynContract.ISynData)(ServerAddress, ServerPort, ServiceName)
    '                                                                 Catch ex As Exception
    '                                                                     Return Nothing
    '                                                                 End Try
    '                                                             End Function)
    '            If TypeOf sData Is SynContract.ISynData Then

    '                lbInfo.Content = "Uploading data to Server ..."

    '                Dim res = Await Async(Of Boolean?)(Function()
    '                                                       Try
    '                                                           Return sData.Construction(u.ID, u.Password, cstOrder)
    '                                                       Catch ex As Exception
    '                                                           Return Nothing
    '                                                       End Try
    '                                                   End Function)
    '                If res Is Nothing Then
    '                    lbInfo.Content = "Server Error."
    '                ElseIf res = True Then
    '                    lbInfo.Content = "Order Sent. We will contact you via email to confirm."
    '                Else
    '                    lbInfo.Content = "Username or Password Error."
    '                End If
    '            Else
    '                lbInfo.Content = "Server Error."
    '            End If
    '        Catch ex As Exception
    '            lbInfo.Content = "Server Error."
    '        End Try
    '    End If
    'End Sub


End Class
