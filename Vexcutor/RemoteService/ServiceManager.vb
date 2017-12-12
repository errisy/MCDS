Imports System.ComponentModel
Imports System.Windows.Forms


Public Class ServiceManager

    Protected WithEvents LocalServer As New RemoteServices

    '自动启动所有的服务
    Private SettingFile As String = "ServiceSetting.rdx"

    '本控件会自动创建一个基本服务
    Private mBaseService As BaseService

    Private ConnectionCode As String '作为服务器使用时 这个是服务器生成的用于用户连接到服务器的连接代码 包含了IP信息等 单击按钮后可以导出
    Private ServerCode As String '作为客户端使用时 这个是客户端的连接码

    Private _Port As Integer = 0
    <System.ComponentModel.Category("服务")> Public Property Port As Integer
        Set(ByVal value As Integer)
            If value < 0 Then value = 0
            _Port = value
        End Set
        Get
            Return _Port
        End Get
    End Property
    Private _Username As String = "Client User"
    <System.ComponentModel.Category("服务"), Description("设置该客户端可以接受的用户名。")> Public Property Username As String
        Set(ByVal value As String)
            _Username = value
        End Set
        Get
            Return _Username
        End Get
    End Property

    <System.ComponentModel.Category("服务"), Description("是作为服务器还是作为客户端。")> Public Property ServiceType As ServiceType

    <System.ComponentModel.Category("服务"), Description("是否检查该客户端允许的登录名。")> Public Property CheckUserName As Boolean = False

    Private SM As New SecureMessage
    Public Property SelfDefinedBaseService As BaseService

    Public Property UserType As StaffGroups
    Public Property UserPermissions As New ICol

    Friend Sub LoadUI()
        ServiceManager_Load(Me, New EventArgs)
    End Sub

    Private Sub ServiceManager_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If DesignMode Then Exit Sub
        '自动加载配置文件

        If ServiceType = ServiceType.Client Then
            Visible = False
            '只要客户端才需要连接到服务器
            'Dim Setting As RemoteSetting
            'If IO.File.Exists(SettingFile) Then
            '    Setting = BinarySerializer.FromFile(SettingFile)

            '    If Setting Is Nothing Then
            '        Setting = New RemoteSetting

            '        If ofdServerInfo.ShowDialog = DialogResult.OK Then
            '            Setting.ServerAddress = IO.File.ReadAllText(ofdServerInfo.FileName)
            '        End If
            '        BinarySerializer.ToFile(Setting, SettingFile)
            '    End If

            '    ServerCode = Setting.ServerAddress

            'Else
            '    'Dim XOS As New XmlObjectSerializer
            '    Setting = New RemoteSetting

            '    If ofdServerInfo.ShowDialog = DialogResult.OK Then
            '        Setting.ServerAddress = IO.File.ReadAllText(ofdServerInfo.FileName)
            '    End If
            '    BinarySerializer.ToFile(Setting, SettingFile)
            '    ServerCode = Setting.ServerAddress
            'End If
        End If
        LocalServer.ServerPort = _Port.ToString
        LocalServer.HostName = _Username
        LocalServer.ServiceType = Me.ServiceType
        '创建并加载基本服务程序
        If mIsBasicServiceEnabled Then
            If (SelfDefinedBaseService Is Nothing) Then
                mBaseService = New BaseService
            Else
                mBaseService = SelfDefinedBaseService
            End If
            mBaseService.ServiceType = Me.ServiceType
            BaseService.SharedInstance = mBaseService
            AddHandler mBaseService.OnGotData, AddressOf OnBasicServiceSynchronized
            AddHandler mBaseService.UpdateConnections, AddressOf UpdateConnections
            mServiceInstances.Add("BasicService", mBaseService)
            If mBaseService.ServiceType = ServiceType.Server Then
                '绑定服务器事件
                AddHandler mBaseService.MonitorInfo, AddressOf UpdateOperatorStatus
                'AddHandler mBaseService.OnLogin, AddressOf ClientLogin
                'AddHandler mBaseService.OnQuery, AddressOf ClientQuery
                'AddHandler mBaseService.OnCustomer, AddressOf ClientCustomer
                'AddHandler mBaseService.OnQuit, AddressOf ClientQuit
                'AddHandler mBaseService.OnInventory, AddressOf ClientInventory
                'AddHandler mBaseService.OnTask, AddressOf ClientTask
            End If
        End If

        LocalServer.Start(mServiceName)
        For Each key As String In mServiceInstances.Keys
            LocalServer.AddService(mServiceInstances(key), key)
        Next

        '客户端用户登录服务器
        Dim LS As New LoginServer

        Dim Success As Boolean = False

        If LoginRequired Then
            While LS.ShowDialog = DialogResult.OK
                If (Not CheckUserName) OrElse SM.DoubleHasher(LS.UsernameTextBox.Text) = Username Then
                    If ServiceType = ServiceType.Client Then
                        '连接到服务器
                        fConnecting.Show()
                        If Connect() Then
                            fConnecting.Text = "Connected to Server. Login processing..."
                            If Login(LS.UsernameTextBox.Text, SM.DoubleHasher(LS.PasswordTextBox.Text)) Then
                                Success = True
                                fConnecting.Hide()

                                Exit While
                            Else
                                fConnecting.Hide()
                                MsgBox("Wrong Password!")
                            End If
                        Else
                            fConnecting.Hide()
                            MsgBox("Can not connect to server.")
                        End If
                    ElseIf ServiceType = ServiceType.Server Then
                        If SM.DoubleHasher(LS.PasswordTextBox.Text) = "ZN4ukDDQLRZ8rguiNJSVQQ" Then
                            Success = True
                            Exit While
                        Else
                            MsgBox("Wrong Password!")
                        End If
                    End If
                Else
                    MsgBox("Wrong user name or invalid user.")
                End If
            End While
        Else
            Success = True
        End If
        If Not Success Then Application.Exit()
    End Sub
    Public Property ServiceConnected As Boolean

    '用来维护远程访问的具体凭据
    'Private LoginName As String
    'Private LoginPass As String
    Public LoginCredential As Login

    <System.ComponentModel.Category("服务"), System.ComponentModel.Browsable(True), System.ComponentModel.DefaultValue(False)>
    Public Property LoginRequired As Boolean = False

    Dim fConnecting As New frmConnecting '登录时显示的窗体

    Public ServerInstance As BaseService '远程服务器的引用

    Private ServerConnected As Boolean = False '说明服务器是否已经连接 如果已经连接则不再连接 具体看connect函数

    Private Function Login(ByVal Username As String, ByVal Password As String) As Boolean
        Dim LI As New Login
        LI.UserName = Username
        LI.Password = Password
        'LI.IP = GetGlobalIP()
        'LI = ServerInstance.Login(LI)
        If LI.Result = True Then
            LoginCredential = LI
            UserType = LI.UserGroup
            UserPermissions.Clear()
            For Each pm As UserPermission In LI.Permissions
                UserPermissions.Add(CInt(pm))
            Next
        End If
        Return LI.Result
    End Function
    Public Sub SendData(ByVal Destination As String, ByVal Value As Docklands)
        Dim BB As New Bunbury
        BB.Destinations = New String() {Destination}
        Select Case Value.GetType
        End Select
        'SynchronizeServiceObject(BB, SM.WaveHasher(BB.Data))
    End Sub

    Private mServiceName As String = "iWonder"

    Private mIsBasicServiceEnabled As Boolean = True
    <Description("在设计时指示是否加载基本服务。必须在设计窗体时或者加载之前启用才能生效。"), Category("服务"), Browsable(True)> Public Property IsBasicServiceEnabled() As Boolean
        Get
            Return mIsBasicServiceEnabled
        End Get
        Set(ByVal value As Boolean)
            mIsBasicServiceEnabled = value
        End Set
    End Property
    <Description("加载的服务的名称。仅在设计时设置该值才能生效。"), Category("服务"), Browsable(True)> Public Property ServiceName() As String
        Get
            Return mServiceName
        End Get
        Set(ByVal value As String)
            mServiceName = value
        End Set
    End Property

    Private mServiceInstances As New Dictionary(Of String, MarshalByRefObject)
    <Description("请在设计时把需要加载的服务实例在主窗体的构造函数中添加到这个字典。此控件加载时会自动启动这些服务。字典的Key是服务的名称。"), Browsable(False)> Public ReadOnly Property ServiceInstances() As Dictionary(Of String, MarshalByRefObject)
        Get
            Return mServiceInstances
        End Get
    End Property

    <Description("通过基本服务程序用来同步数据。此方法将创建一个线程来调用基本服务来发送信息。所以不会引发GUI界面等待问题。")> Public Sub SynchronizeServiceObject(ByVal Obj As Bunbury, ByVal vSignature As String)
        '如果基本服务不可使用 那么就不能运行
        If mBaseService Is Nothing Then Exit Sub
        Dim TH As New ThreadHeader(Of Bunbury, String)(AddressOf mBaseService.SendData, Obj, vSignature)
    End Sub

    <Description("当外部数据传入到基础服务时引发此事件。请根据vSignature判断对象的种类和响应方法。")> Public Event BasicServiceSynchronized(ByVal Obj As Bunbury, ByVal vSignature As String)

    <Description("当外部数据传入到基础服务时经由此方法引发事件。此方法已经包含Invoke方法，所有不会引发外源线程调用GUI问题。")> Private Sub OnBasicServiceSynchronized(ByVal vData As Bunbury, ByVal vSignature As String)
        '简化的Invoke方法
        If ThreadHeader(Of Bunbury, String).ControlInvoke(Me, AddressOf OnBasicServiceSynchronized, vData, vSignature) Then Exit Sub
        RaiseEvent BasicServiceSynchronized(vData, vSignature)
    End Sub

    <Description("用来在运行时返回服务器的名称。"), Browsable(False)> Public ReadOnly Property HostName() As String
        Get
            Return LocalServer.HostName
        End Get
    End Property

    <Description("用来在运行时返回服务器的完整名称。"), Browsable(False)> Public ReadOnly Property FullHostName() As String
        Get
            Return String.Format("{0}<{1}>", LocalServer.HostName, ReduceID(LocalServer.UUPI))
        End Get
    End Property

    Private Sub LocalServer_UpdateConnectionCode(ByVal sender As Object, ByVal e As RemoteInfoEventArgs) Handles LocalServer.UpdateConnectionCode
        If ThreadHeader(Of Object, RemoteInfoEventArgs).ControlInvoke(tvLocal, AddressOf LocalServer_UpdateConnectionCode, sender, e) Then Exit Sub
        ConnectionCode = e.Code
        llExportConnectionFile.Enabled = True
        UpdateLocalView()
    End Sub

    Private Sub UpdateLocalView()

        '获取根节点
        Dim rtNode As TreeNode = tvLocal.Nodes(0)
        rtNode.Text = LocalServer.HostName + "<" + ReduceID(LocalServer.UUPI) + ">"

        '获取所有的网址
        Dim lastNodeIndex As Integer = 0
        '更新
        For Each key As String In LocalServer.LocalIPAddress.Keys
            If rtNode.Nodes.ContainsKey("IP" + key) Then
                rtNode.Nodes("IP" + key).Text = LocalServer.LocalIPAddress(key) + "<" + key + ">"
            Else
                rtNode.Nodes.Insert(lastNodeIndex, "IP" + key, LocalServer.LocalIPAddress(key) + "<" + key + ">", ImageID.网址, ImageID.网址)
                lastNodeIndex += 1
            End If
            'rtNode.Nodes.Add("IP" + key, LocalServer.LocalIPAddress(key) + "<" + key + ">", ImageID.网址)
        Next

        '添加所有的服务
        For Each key As String In ServiceInstances.Keys
            If rtNode.Nodes.ContainsKey("SV" + key) Then
                rtNode.Nodes("SV" + key).Text = key
            Else
                rtNode.Nodes.Insert(lastNodeIndex, "SV" + key, key, ImageID.停止的本地服务, ImageID.停止的本地服务)
                lastNodeIndex += 1
            End If
        Next
        '从服务器列表当中刷新服务的状态
        For Each record As RemoteServiceRecord In LocalServer.ServiceDict.Values
            If rtNode.Nodes.ContainsKey("SV" + record.ServiceName) Then
                rtNode.Nodes("SV" + record.ServiceName).ImageIndex = ImageID.启动的本地服务
                rtNode.Nodes("SV" + record.ServiceName).SelectedImageIndex = ImageID.启动的本地服务
            Else
                rtNode.Nodes("SV" + record.ServiceName).ImageIndex = ImageID.停止的本地服务
                rtNode.Nodes("SV" + record.ServiceName).SelectedImageIndex = ImageID.停止的本地服务
            End If
        Next
        tvLocal.ExpandAll()
    End Sub

    Public Enum ImageID As Integer
        被动连接
        本地主机
        断开的服务
        连接的服务
        启动的本地服务
        停止的本地服务
        远程主机
        主动连接
        网址
    End Enum

    Private Function Connect() As Boolean
        Dim SI As ServiceInfo = ServiceInfo.Decode(ServerCode)
        Return ConnectTo(SI, SI.Name) '非异步呼叫 直接等到动作完成
    End Function

    Private Function ReduceID(ByVal vUUPI As String) As String
        Dim l As Integer = vUUPI.Length
        Dim stb As New System.Text.StringBuilder
        For i As Integer = 0 To l Step l \ 7
            stb.Append(vUUPI(i))
        Next
        Return stb.ToString
    End Function

    Private Sub tvRemote_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs)
        '双击时连接服务
        If e.Node.ImageIndex <> ImageID.断开的服务 Then Exit Sub
        Dim SI As ServiceInfo = e.Node.Tag
        Dim TH As New ThreadHeader(Of ServiceInfo, String)(AddressOf ConnectTo, SI, e.Node.Text)
    End Sub

    Friend Function ConnectTo(ByVal IP As String, ByVal ServiceName As String, ByRef rService As BaseService) As Boolean
        'If ServiceConnected Then Return ServiceConnected
        Try
            'mBaseService.ConnectToRemote(LocalServer.GetRemoteService(Of BaseService)("tcp://" + IP + "/" + ServiceName))
            'ServerConnected = True
            rService = LocalServer.GetRemoteService(Of BaseService)("tcp://" + IP + "/" + ServiceName)
            Return True
        Catch ex As Exception

        End Try
    End Function

    Private Function ConnectTo(ByVal SI As ServiceInfo, ByVal ServiceName As String) As Boolean
        '如果已经连接到服务器 则不需要再次连接
        If ServerConnected Then Return ServerConnected

        For Each key As String In SI.IPEntries.Keys
            Try
                mBaseService.ConnectToRemote(LocalServer.GetRemoteService(Of BaseService)("tcp://" + SI.IPEntries(key) + "/" + ServiceName))
                ServerConnected = True
                ServerInstance = mBaseService.ServerInstance
                Exit For
            Catch ex As Exception

            End Try
        Next
        Return ServerConnected
    End Function

    Private ServerKey As String
    Private ClientKeys As New Dictionary(Of String, String) ' Usernames -> Keys

    Private Sub UpdateConnections(ByVal vKey As String)
        If ThreadHeader(Of String).ControlInvoke(tvLocal, AddressOf UpdateConnections, vKey) Then Exit Sub

        If ServiceType - ServiceType.Server Then
            '收到连接信息

        ElseIf ServiceType = ServiceType.Client Then
            '发送登录信息
            ServerKey = vKey
            fConnecting.DialogResult = DialogResult.OK
        End If
    End Sub

    'Private Sub rtbLocal_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
    '    rtbLocal.SelectAll()
    'End Sub

    Private Sub llExportConnectionFile_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llExportConnectionFile.LinkClicked
        If sfdServerInfo.ShowDialog = DialogResult.OK Then
            IO.File.WriteAllText(sfdServerInfo.FileName, ConnectionCode)
        End If
    End Sub

    Private Sub llStatus_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llStatus.LinkClicked
        dgvLog.Visible = False
        lvStatus.Visible = True
    End Sub

    Private Sub llLog_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llLog.LinkClicked
        lvStatus.Visible = False
        dgvLog.Visible = True
    End Sub

#Region "管理用户列表状态"
    Private UserMonitor As New Dictionary(Of String, ListViewItem)
    Private IdentityGroup As New Dictionary(Of String, ListViewGroup)

    Public Delegate Sub UpdateOperatorStatusDelegate(ByVal User As String, ByVal ID As String, ByVal Operation As String, ByVal Detail As String, ByVal Time As DateTime, ByVal Identity As String)

    Public Sub UpdateOperatorStatus(ByVal User As String, ByVal ID As String, ByVal Operation As String, ByVal Detail As String, ByVal Time As DateTime, ByVal Identity As String)
        If InvokeRequired Then
            Me.Invoke(New UpdateOperatorStatusDelegate(AddressOf UpdateOperatorStatus), New Object() {User, ID, Operation, Detail, Time, Identity})
        Else
            dgvLog.Rows.Insert(0)
            Dim row As DataGridViewRow = dgvLog.Rows(0)
            row.Cells(0).Value = User
            row.Cells(1).Value = Operation
            row.Cells(2).Value = Detail
            row.Cells(3).Value = Time.ToString
            If Not IdentityGroup.ContainsKey(Identity) Then
                Dim lvGroup As New ListViewGroup
                lvGroup.Name = Identity
                lvGroup.Header = Identity
                IdentityGroup.Add(Identity, lvGroup)
                lvStatus.Groups.Add(lvGroup)
            End If
            If Not UserMonitor.ContainsKey(ID) Then
                Dim lvItem As New ListViewItem
                lvItem.Name = ID
                lvItem.Text = User
                UserMonitor.Add(ID, lvItem)
                lvItem.SubItems.Add(Operation)
                lvItem.SubItems.Add(Time.ToString)
                lvItem.SubItems.Add(Time.ToString)
                IdentityGroup(Identity).Items.Add(lvItem)
                lvStatus.Items.Add(lvItem)
            Else
                With UserMonitor(ID)
                    .SubItems(1).Text = Operation
                    .SubItems(2).Text = Time.ToString
                    If Operation = "Login" Then
                        .SubItems(3).Text = Time.ToString
                    End If
                End With
            End If
        End If

    End Sub


#End Region

#Region "用户的操作"
    Public Sub Logout()
        'Dim th As New ThreadHeader(Of Login)(AddressOf ServerInstance.Quit, LoginCredential)
    End Sub
#End Region

#Region "加密"


    Private rsakey As String = "<RSAKeyValue><Modulus>qW7kxBP6F8hPqPWi+Q88zZRiPlgcRnHnKQ458bFsmh+WEJ1mUFn6GQGuhUFx1PyzBtcoDURKoV/b2e9LScMjqQE94A2YV0gnD7wHa0XzKV35yBlxpbankZDvpM0/jf5K0UdjUoVLwL6P+4gUOHlZ0Z0d18Z1wf8ls1L1fkN19Uc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
    Public Sub New()
        'If Not DesignMode Then
        '    Dim di As New System.IO.DirectoryInfo(Application.StartupPath)
        '    Dim hs As List(Of String) = Equipment.GenerateMachineHash
        '    Dim ky As List(Of String) = Nothing

        '    For Each fi As System.IO.FileInfo In di.GetFiles("*.epm")
        '        Try
        '            Dim bytes As Byte() = System.IO.File.ReadAllBytes(fi.FullName)
        '            ky = Equipment.CheckKey(bytes)
        '        Catch ex As Exception

        '        End Try
        '    Next
        '    If ky Is Nothing Then Application.Exit()
        '    Dim RSA As New RSACrypt(rsakey)
        '    For i As Integer = 0 To ky.Count - 1
        '        If Not RSA.VerifyData(ky(i), hs(i)) Then Application.Exit()
        '    Next
        'End If
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub
#End Region
End Class
