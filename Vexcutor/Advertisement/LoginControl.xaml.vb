Imports System.Windows.Controls, System.Windows, System.Windows.Media
Public Class LoginControl
    Private UserList As New UserList

    Private Sub LoginControl_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Dim AppPath As String = System.Windows.Forms.Application.StartupPath
        Dim UserSettingPath As String = AppPath + "\Users.xml"
        If IO.File.Exists(UserSettingPath) Then
            Try
                Dim ul As UserList = Xaml.XamlServices.Load(UserSettingPath)
                Dim ulist As New List(Of String)
                For Each u In ul.Users
                    ulist.Add(u.UserName)
                Next
                'acpUserName.Values = ulist
                UserList = ul
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub LoginKeyDown(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        If e.Key = Input.Key.Enter Then
            Login(sender, e)
            e.Handled = True
        End If
    End Sub

    Private Async Sub Login(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim u As New LoginAndAdvertisement With {.UserName = tbUserName.Text, .Password = LoginManagement.EncryptPassword(pbPassword.Password)}
        If u.UserName.Length > 0 Then

            elInfo.Content = "Connecting to Synthenome ..."

            Dim Connect As Boolean = False
            Try
                Dim sData = Await Async(Of SynContract.ISynData)(Function() As SynContract.ISynData
                                                                     Try
                                                                         Return WellKnownExtension.GetService(Of SynContract.ISynData)(ServerAddress, ServerPort, ServiceName)
                                                                     Catch ex As Exception
                                                                         Return Nothing
                                                                     End Try
                                                                 End Function)
                If TypeOf sData Is SynContract.ISynData Then
                    elInfo.Content = "Getting Computer Key ..."
                    Dim computerKey As String = Await LoginManagement.GetComputerKey
                    elInfo.Content = "Checking User Info ..."
                    Dim vCust = Await Async(Of SynContract.Customer)(Function()
                                                                         Try
                                                                             Return sData.CustomerLogin(u.UserName, u.Password, computerKey)
                                                                         Catch ex As Exception
                                                                             Return Nothing
                                                                         End Try
                                                                     End Function)
                    If vCust IsNot Nothing Then
                        'LoginManagement.AddUserToList(u.UserName)
                        Connect = True
                        vCust.Password = u.Password
                        LoginManagement.Customer = vCust
                        LoginManagement.GetAdertisements()
                        LoginManagement.GetPrice()
                        LoginManagement.GerVersion()
                        WPFContainer.GetWinForm(Me).DialogResult = DialogResult.OK
                    Else
                        Connect = False
                    End If
                Else
                    elInfo.Content = "Unable to Connect Synthenome ..."
                End If
            Catch ex As Exception
                elInfo.Content = "Unable to Connect Synthenome ..."

            End Try
            If Not Connect Then
                elInfo.Content = "Local Login..."
                'tbUserName.IsEnabled = False
                Dim t As New System.Threading.Tasks.Task(Sub()
                                                             System.Threading.Thread.Sleep(1000)
                                                         End Sub)
                t.Start()
                Await t
                Dim NameData As Byte() = IO.File.ReadAllBytes(System.Windows.Forms.Application.StartupPath + "\license.vfa")
                Dim signature As String = System.Text.ASCIIEncoding.ASCII.GetString(PassWordSign.VFAFileDecryption.TransformFinalBlock(NameData, 0, NameData.Length))
                Dim signaturelines As String() = signature.Split(New Char() {vbCr, vbLf, vbCrLf}, System.StringSplitOptions.RemoveEmptyEntries)
                Dim UsernameCheck As String = signaturelines(1)
                Dim PasswordCheck As String = signaturelines(2)

                Dim EncryptedPassword As String = pbPassword.Password

                If PassWordSign.PasswordRSAEncrypt.VerifyData(UsernameCheck, u.UserName) And PassWordSign.PasswordRSAEncrypt.VerifyData(PasswordCheck, EncryptedPassword) Then
                    WPFContainer.GetWinForm(Me).DialogResult = DialogResult.OK
                Else
                    elInfo.Content = "Wrong Password!"
                End If
            End If
        Else
            elInfo.Content = "Username must not be empty!"
        End If
    End Sub

    Private Sub tbUserName_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles tbUserName.TextChanged
        If tbUserName.Text.Length = 0 Then
            tbUserName.BorderBrush = Brushes.Red
        Else
            tbUserName.BorderBrush = Brushes.Blue
        End If
    End Sub

    Public Property Host As frmLogin
        Get
            Return GetValue(HostProperty)
        End Get

        Set(ByVal value As frmLogin)
            SetValue(HostProperty, value)
        End Set
    End Property

    Public Shared ReadOnly HostProperty As DependencyProperty = _
                           DependencyProperty.Register("Host", _
                           GetType(frmLogin), GetType(LoginControl), _
                           New FrameworkPropertyMetadata(Nothing))

    Private Sub Register(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
        Process.Start("http:\\www.synthenome.com")
    End Sub
End Class
