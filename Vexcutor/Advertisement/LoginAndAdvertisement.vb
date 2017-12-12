Imports System.Windows.Markup, System.Windows, System.Windows.Data, System.Windows.Media, System.Windows.Controls, System.Windows.Shapes
Public Class LoginAndAdvertisement
    Inherits DependencyObject
    Public Property UserName As String
    Public Property Password As String
End Class
<ContentProperty("Users")>
Public Class UserList
    Inherits DependencyObject
    Public Sub New()
        MyBase.New()
        SetValue(UsersProperty, New System.Collections.ObjectModel.ObservableCollection(Of LoginAndAdvertisement))
    End Sub
    Public Property Users As System.Collections.ObjectModel.ObservableCollection(Of LoginAndAdvertisement)
        Get
            Return GetValue(UsersProperty)
        End Get
        Set(ByVal value As System.Collections.ObjectModel.ObservableCollection(Of LoginAndAdvertisement))
            SetValue(UsersProperty, value)
        End Set
    End Property
    Public Shared ReadOnly UsersProperty As DependencyProperty = _
                           DependencyProperty.Register("Users", _
                           GetType(System.Collections.ObjectModel.ObservableCollection(Of LoginAndAdvertisement)), GetType(UserList), _
                           New FrameworkPropertyMetadata(Nothing))
End Class

Public Class LoginManagement
    Public Shared User As LoginAndAdvertisement
    Public Shared Customer As SynContract.Customer
    Public Shared Function Login() As Boolean
        If Not (TypeOf Customer Is SynContract.Customer) Then
            Dim fLogin As New frmLogin
            Select Case fLogin.ShowDialog()
                Case DialogResult.OK
                    Return True
                Case DialogResult.Ignore
                    Dim fRegist As New frmRegistration
                    Select Case fRegist.ShowDialog()
                        Case DialogResult.OK
                            Return True
                        Case Else
                            Return False
                    End Select
                Case DialogResult.Cancel
                    Return False
            End Select
        End If
    End Function
    Private Shared loginKey As String = "<RSAKeyValue><Modulus>pDOOAeL64F6O3Kx5MG3yiKUVw3ootW128PMtjad/2BfADhhu0CjzGq5BXSQYoQHpJGl2o//mREgCdgoXExSuwT63LLDTytDiwnPIU3iEhNeHglB23am1elD07vznnd/7UApTYXfrD5Udc5luOTMrlJeq1r4eCi+WSZKg/t/vR8M=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
    'Private Shared loginPrivateKey As String = "<RSAKeyValue><Modulus>pDOOAeL64F6O3Kx5MG3yiKUVw3ootW128PMtjad/2BfADhhu0CjzGq5BXSQYoQHpJGl2o//mREgCdgoXExSuwT63LLDTytDiwnPIU3iEhNeHglB23am1elD07vznnd/7UApTYXfrD5Udc5luOTMrlJeq1r4eCi+WSZKg/t/vR8M=</Modulus><Exponent>AQAB</Exponent><P>3ohAAAMdCQmmo05rvtj1CAllHDpjPYByZzazYRwI0t28rT8f0Ark7GLae/KWLsBgVU3syfcsT05B1pJqdvK/gQ==</P><Q>vOV/1yn+dRMXIyqHjRqr+kQXafTlG2LM8NouGA9bccg0BkgLQQnUeNCTwuoGLRvqjdOaLk27k625gvk2+3upQw==</Q><DP>RZZ53QccN01LTNojG2UhCshVVAR2MC9QlzIl2gI4SCiK7epFentNpxYqmIP9rtT9yu85Utb2hj5EnGLg5B15AQ==</DP><DQ>dkt7HfIMiqDj8n/l17YGUXpm91IIUHg1Q/g+uY6Ug9MO0Yg4lAhl6Ssl/gC00XcUErGbcrf3amp3LNCJYEB3JQ==</DQ><InverseQ>WEa/h9KayxpLsechIwOrqYd39HeA7lI7i54LMROlbuJ9D1LAmqae5XdDptP+SfROJpgGElAjc6StjRAthcHq9g==</InverseQ><D>g+ficUOMs8diToXDCROZ3Ql5FUJCB5T7Eo7Xjk77VXWm0+vuwQvUVx+a5J7Fpjnpt1V9eDtvGHUcfQSYCrz3sH0U0/t4eqks577t392NO2kkJlib7qgxaixczzp/b2uIRoFspVsxXaU6ArkB9pPEXvpHwCy4xGPzJVmX/yijeQE=</D></RSAKeyValue>"

    Private Shared ReadOnly Property Crypto As System.Security.Cryptography.RSACryptoServiceProvider
        Get
            Dim rsa = New System.Security.Cryptography.RSACryptoServiceProvider
            rsa.FromXmlString(loginKey)
            Return rsa
        End Get
    End Property
    Public Shared Function EncryptPassword(value As String) As String
        Dim inbytes = System.Text.Encoding.UTF8.GetBytes(Now.ToString("yyyyMMddHHmmss") + value)
        Dim outbytes As Byte() = Crypto.Encrypt(inbytes, True)
        Return Convert.ToBase64String(outbytes)
    End Function
    Public Shared Function AddUserToList(value As String) As Boolean
        Dim AppPath As String = System.Windows.Forms.Application.StartupPath
        Dim UserSettingPath As String = AppPath + "\Users.xml"
        If IO.File.Exists(UserSettingPath) Then
            Try
                Dim ul As UserList = Xaml.XamlServices.Load(UserSettingPath)
                If Not ul.Users.Select(Function(li) As Boolean
                                           li.UserName = value
                                       End Function).Any() Then
                    ul.Users.Add(New LoginAndAdvertisement With {.UserName = value})
                    Xaml.XamlServices.Save(UserSettingPath, ul)
                End If
            Catch ex As Exception

            End Try
        End If
    End Function

    Public Shared Async Function GetComputerKey() As System.Threading.Tasks.Task(Of String)
        Return Await Async(Of String)(Function()
                                          Dim stb As New System.Text.StringBuilder
                                          '另一种代码形式的
                                          Dim objMOS As System.Management.ManagementObjectSearcher
                                          Dim objMOC As Management.ManagementObjectCollection
                                          Dim objMO As Management.ManagementObject = Nothing
                                          'Now, execute the query to get the results

                                          objMOS = New System.Management.ManagementObjectSearcher("Select * From Win32_Processor")
                                          objMOC = objMOS.Get
                                          'Finally, get the CPU's id.
                                          For Each objMO In objMOC
                                              stb.AppendLine(objMO("ProcessorID"))
                                          Next
                                          'Dispose object variables.
                                          objMOS.Dispose()
                                          objMOS = Nothing
                                          objMO.Dispose()
                                          objMO = Nothing
                                          Return ConvertToHexString(System.Security.Cryptography.MD5.Create.ComputeHash(System.Text.Encoding.UTF8.GetBytes(stb.ToString)))
                                      End Function)
    End Function
    Private Shared _Advertisements As SynContract.Suppliers
    Private Shared IsGettingAdvertisements As Boolean
    Private Shared LastGetAdvertisement As Date = Date.MinValue
    Public Shared ReadOnly Property Advertisements As SynContract.Suppliers
        Get
            If _Advertisements Is Nothing Then
                GetAdertisements()
                Return Nothing
            Else
                '每过20分钟刷新一次广告
                If Date.Now - LastGetAdvertisement > New TimeSpan(0, 20, 0) Then
                    GetAdertisements()
                End If
                Return _Advertisements
            End If
        End Get
    End Property
    Public Shared Async Sub GetAdertisements()
        If System.Threading.Thread.VolatileRead(IsGettingAdvertisements) Then Exit Sub
        System.Threading.Thread.VolatileWrite(IsGettingAdvertisements, True)
        Try

            Dim iData = Await Async(Of SynContract.ISynData)(Function()
                                                                 Try
                                                                     Return WellKnownExtension.GetService(Of SynContract.ISynData)(ServerAddress, ServerPort, ServiceName)
                                                                 Catch ex As Exception
                                                                     Return Nothing
                                                                 End Try
                                                             End Function)
            If Not (TypeOf iData Is SynContract.ISynData) Then Return
            Dim _Suppliers = Await Async(Of SynContract.Suppliers)(Function()
                                                                       Try
                                                                           Return iData.GetSuppliers(Customer.ID, Customer.Password)
                                                                       Catch ex As Exception
                                                                           Return New SynContract.Suppliers
                                                                       End Try
                                                                   End Function)
            If TypeOf _Suppliers Is SynContract.Suppliers Then _Advertisements = _Suppliers
        Catch ex As Exception
            _Advertisements = New SynContract.Suppliers
        End Try
        LastGetAdvertisement = Now
        System.Threading.Thread.VolatileWrite(IsGettingAdvertisements, False)
    End Sub
    Private Shared CurrentVersion As Integer = 1000
    Public Shared Async Sub GerVersion()
        Try
            Dim iData = Await Async(Of SynContract.ISynData)(Function()
                                                                 Try
                                                                     Return WellKnownExtension.GetService(Of SynContract.ISynData)(ServerAddress, ServerPort, ServiceName)
                                                                 Catch ex As Exception
                                                                     Return Nothing
                                                                 End Try
                                                             End Function)
            If Not (TypeOf iData Is SynContract.ISynData) Then Return
            Dim _Version = Await Async(Of SynContract.VersionInfo)(Function()
                                                                       Try
                                                                           Return iData.CheckNewVersion(CurrentVersion)
                                                                       Catch ex As Exception
                                                                           Return Nothing
                                                                       End Try
                                                                   End Function)
            If TypeOf _Version Is SynContract.VersionInfo Then _NewVersion = _Version
            If _Version.Version > 1000 Then
                Dim frmVC As New frmVersionCheck
                DirectCast(frmVC.InteropHost1.Child, VersionCheck).DataContext = _Version
                frmVC.ShowDialog()
            End If
        Catch ex As Exception
            _NewVersion = Nothing
        End Try
    End Sub
    Private Shared _NewVersion As SynContract.VersionInfo = Nothing
    Public Shared ReadOnly Property NewVersion As SynContract.VersionInfo
        Get
            Return _NewVersion
        End Get
    End Property
    Public Shared Async Sub GetPrice()
        Try
            Dim iData = Await Async(Of SynContract.ISynData)(Function()
                                                                 Try
                                                                     Return WellKnownExtension.GetService(Of SynContract.ISynData)(ServerAddress, ServerPort, ServiceName)
                                                                 Catch ex As Exception
                                                                     Return Nothing
                                                                 End Try
                                                             End Function)
            If Not (TypeOf iData Is SynContract.ISynData) Then Return
            Dim _p = Await Async(Of SynContract.SynthenomePrice)(Function()
                                                                     Try
                                                                         Return iData.GetPrice(Customer.ID, Customer.Password)
                                                                     Catch ex As Exception
                                                                         Return New SynContract.SynthenomePrice
                                                                     End Try
                                                                 End Function)
            If TypeOf _p Is SynContract.SynthenomePrice Then _Price = _p
        Catch ex As Exception
            _Price = New SynContract.SynthenomePrice
        End Try
    End Sub
    Private Shared _Price As New SynContract.SynthenomePrice
    Public Shared ReadOnly Property Price As SynContract.SynthenomePrice
        Get
            Return _Price
        End Get
    End Property
    Public Shared ReadOnly Property ProjectPrice As SynthenomeConstructionPrice
        Get
            Dim scp As New SynthenomeConstructionPrice
            scp.ConstructionPrice = Price.ConstructionPrice
            Return scp
        End Get
    End Property
    Public Shared ReadOnly Property SynthesisPrice As SynthenomeSynthesisPrice
        Get
            Dim scp As New SynthenomeSynthesisPrice
            scp.WorldWideSynthesisPrice = Price.WorldWidePrice
            scp.ChinaSynthesisPrice = Price.ChinaPrice
            scp.MinChargeLength = Price.MinimiumChargeLength
            scp.MinChargePrice = Price.MinimiumSynthesisPrice
            Return scp
        End Get
    End Property
    Public Shared ReadOnly Property PrimerPrice As SynthenomePrimerPrice
        Get
            Dim scp As New SynthenomePrimerPrice
            scp.PrimerPrice = Price.PrimerPrice
            Return scp
        End Get
    End Property
    Public Shared Async Sub SendEmailtoCompanies(Companies As IEnumerable(Of SynContract.Company), EmailContent As String)
        Dim iData = Await Async(Of SynContract.ISynData)(Function()
                                                             Try
                                                                 Return WellKnownExtension.GetService(Of SynContract.ISynData)(ServerAddress, ServerPort, ServiceName)
                                                             Catch ex As Exception
                                                                 Return Nothing
                                                             End Try
                                                         End Function)
        If Not (TypeOf iData Is SynContract.ISynData) Then Exit Sub
        Dim _Suppliers = Await Async(Of Boolean)(Function()
                                                     Try
                                                         Return iData.SendEmails(User.UserName, User.Password, Companies.ToList, EmailContent)
                                                     Catch ex As Exception
                                                         Return False
                                                     End Try
                                                 End Function)
    End Sub
End Class

Public Module ServiceInformation
#If Online = 1 Then
    Public Property ServerAddress As String = "data.synthenome.com"
#Else
    Public Property ServerAddress As String = "127.0.0.1"
#End If
    Public Property ServerPort As Integer = 1888
    Public Property ServiceName As String = "SynDataService"
End Module

Public Module HexString

    Public Function ConvertToHexString(ByVal bytes As Byte()) As String
        Dim stb As New System.Text.StringBuilder
        For Each b As Byte In bytes
            stb.Append(Hex(b).PadLeft(2, "0"))
        Next
        Return stb.ToString
    End Function
End Module
Public Module EmailModule
    Public RFC2822 As New System.Text.RegularExpressions.Regex("(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])")

End Module