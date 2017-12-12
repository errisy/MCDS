Public Class AdvertisementModule
    Public Shared Advertisements As New SynContract.Suppliers
    Public Shared AdvertisementReady As Boolean = False
    Public Shared DataService As SynContract.ISynData
    Public Shared BeganGet As Boolean = False
    Public Shared AlreadyGot As Boolean = False
    Public Function SearchAdvertisement() As SynContract.Suppliers
        If AdvertisementReady Then
            Return Advertisements
        Else
            BeginGetAdvertisement("local")
            Return Nothing
        End If
    End Function
    Public Sub BeginGetAdvertisement(myLocation As String)
        If Not System.Threading.Thread.VolatileRead(AlreadyGot) Then
            GetAdvertisement(myLocation)
        End If
    End Sub
    Public Sub GetAdvertisement(myLocation As String)
        If Not System.Threading.Thread.VolatileRead(BeganGet) Then Exit Sub
        Dim t As New System.Threading.Tasks.Task(
            Sub()
                System.Threading.Thread.VolatileWrite(BeganGet, True)
                Try
                    DataService = WellKnownExtension.GetService(Of SynContract.ISynData)(ServerAddress, ServerPort, ServiceName)
                    Advertisements = DataService.GetSuppliers(LoginManagement.User.UserName, LoginManagement.User.Password)
                    If Advertisements IsNot Nothing Then System.Threading.Thread.VolatileWrite(AlreadyGot, True)
                Catch ex As Exception

                End Try
                System.Threading.Thread.VolatileWrite(BeganGet, False)
            End Sub)
        t.Start()
    End Sub
End Class