Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp
Imports System.ComponentModel
Imports System.Runtime.Remoting.Messaging
Imports System.Security.Principal
Imports System.Runtime.Serialization


'desktop main thread only for draw, the background thread for doing calculation.

'该类型的代码来自于http://wayfarer.cnblogs.com/archive/2004/07/30/28723.html
'
'System.Runtime.Remoting提供了一系列的方法用于访问远程结构
Public Enum ServiceType As Integer
    Server
    Client
End Enum
<Serializable()> Public Class RemoteSetting
    Public ServerAddress As String
End Class

Public Class RemoteServices
    Public ServerChannel As TcpChannel
    Private ClientChannel As TcpChannel
    Public ServiceDict As New Dictionary(Of String, RemoteServiceRecord)
    Public ServerPort As String = ""
    Public LocalIPAddress As New Dictionary(Of String, String)
    Public Event UpdateConnectionCode(ByVal sender As Object, ByVal e As RemoteInfoEventArgs)
    'Private WithEvents UPnP As UPnPEngine
    Public DNAAddress As String

    Public UUPI As String 'MAC 全局唯一进程ID 由MAC+时间戳+用户名+随机数组形成
    Public HostName As String
    Public ServerIPAddress As String
    Public ServiceType As ServiceType

    'Moonee's Pond Lord is the defualt value for server, client should have user name.
    Public Sub Start(ByVal ServiceName As String)
        System.Runtime.Remoting.RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off
        Dim bsprovider As New BinaryServerFormatterSinkProvider()
        Dim bcprovider As New BinaryClientFormatterSinkProvider()
        bsprovider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full
        Dim props As IDictionary = New Hashtable()

        If ServerPort.Length > 0 Then
            '按照指定的端口注册服务
        Else
            '如果没有指定端口 获得一个可用的端口号
            Dim ipad As String = ""
            If True Then
                Dim ip As System.Net.IPAddress = System.Net.IPAddress.Parse("127.0.0.1")
                Dim tl As New System.Net.Sockets.TcpListener(ip, 0)
                tl.Start()
                ipad = tl.LocalEndpoint.ToString
                tl.Stop()
            End If

            ServerPort = ipad.Substring(ipad.IndexOf(":") + 1)
        End If

        '注册服务通道
        props("name") = "Synthenome Vexcutor"
        props("port") = CInt(ServerPort)
        props("typeFilterLevel") = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full
        Dim channel As TcpChannel = New TcpChannel(props, bcprovider, bsprovider)
        ChannelServices.RegisterChannel(channel, False)
        RemotingConfiguration.ApplicationName = ServiceName
        ServerChannel = channel

        '注册客户通道
        RegistClientChannel()
        '获得本机网卡的IP地址列表
        Dim UUPIMacSeed As String = Now.ToString

        'Dim wmi As New System.Management.ManagementObjectSearcher("select * from win32_networkadapterconfiguration")
        'For Each wmiobj As System.Management.ManagementObject In wmi.Get
        '    If CBool(wmiobj("ipenabled")) Then
        '        '用MAC地址作为KEY
        '        LocalIPAddress.Add(wmiobj("MACAddress"), wmiobj("ipaddress")(0) + ":" + ServerPort)
        '        UUPIMacSeed += wmiobj("MACAddress")
        '    End If
        'Next

        '生成UUPI
        Dim SM As New SecureMessage

        'UUPI当中包含了用户名部分 所以一定是唯一的
        UUPI = SM.Sign(UUPIMacSeed).Replace("=", "") + IIf(HostName.Length > 0, SM.Sign(HostName).Replace("=", ""), "")

        '部署UPnP
        'UPnP = New UPnPEngine
        Dim PortNum As Integer = CInt(ServerPort)
        'UPnP.StartUp(PortNum, PortNum)
    End Sub

    Public Function AddService(ByVal ServiceObject As BaseService, ByVal RemoteTypeName As String) As String()
        '传递主服务通道的信息
        ServiceObject.Name = HostName
        ServiceObject.UUPI = UUPI
        ServiceObject.Key = RemoteTypeName

        RemotingConfiguration.RegisterWellKnownServiceType(ServiceObject.GetType, RemoteTypeName, WellKnownObjectMode.Singleton)
        Dim rmsr As New RemoteServiceRecord
        rmsr.ServiceName = RemoteTypeName
        rmsr.ServiceRef = RemotingServices.Marshal(ServiceObject, RemoteTypeName)
        rmsr.Service = ServiceObject

        For Each ip As String In LocalIPAddress.Values
            rmsr.ServiceURLs.Add(String.Format("tcp://{0}/{1}", ip, RemoteTypeName))
        Next
        ServiceDict.Add(RemoteTypeName, rmsr)

        '引发连接事件
        If ServiceType = ServiceType.Server Then
            Dim SI As New ServiceInfo
            SI.UUPI = UUPI
            SI.Name = RemoteTypeName
            '获得本机网卡的IP地址列表
            Dim UIP As String = ""

            'Try
            '    UIP = GetGlobalIP()
            '    'Dim NC As New System.Net.WebClient
            '    'Dim regexIP As New System.Text.RegularExpressions.Regex("\d+.\d+.\d+.\d+")
            '    'UIP = NC.DownloadString("http://archive.apnic.net/templates/ipv6man/")
            '    'UIP = regexIP.Match(UIP).Captures(0).Value
            '    'Dim ip As System.Net.IPAddress = System.Net.IPAddress.Parse(UIP)
            'Catch ex As Exception
            '    UIP = ServerIPAddress
            'End Try

            'If Not (UIP Is Nothing) Then SI.IPEntries.Add(UIP, UIP & ":" & ServerPort)

            'Dim wmi As New System.Management.ManagementObjectSearcher("select * from win32_networkadapterconfiguration")
            'For Each wmiobj As System.Management.ManagementObject In wmi.Get
            '    If CBool(wmiobj("ipenabled")) Then
            '        If (Not SI.IPEntries.ContainsKey(wmiobj("ipaddress")(0))) Then SI.IPEntries.Add(wmiobj("ipaddress")(0), wmiobj("ipaddress")(0) & ":" & ServerPort)
            '    End If
            'Next
            'Dim UCIAC As System.Net.NetworkInformation.UnicastIPAddressInformationCollection = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties.GetUnicastAddresses
            'For Each UCIA As System.Net.NetworkInformation.UnicastIPAddressInformation In UCIAC
            '    If UCIA.Address.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork AndAlso (Not SI.IPEntries.ContainsKey(UCIA.Address.ToString)) Then SI.IPEntries.Add(UCIA.Address.ToString, UCIA.Address.ToString & ":" & ServerPort)
            'Next

            'Dim UUPIMacSeed As String = ""
            'DNAAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList(0).ToString
            'SI.DNSAddress = DNAAddress
            'For Each key As String In LocalIPAddress.Keys
            '    SI.IPEntries.Add(key, LocalIPAddress(key))
            'Next
            'For Each RSR As RemoteServiceRecord In ServiceDict.Values
            '    SI.Services.Add(RSR.ServiceName)
            'Next

            'RaiseEvent UpdateConnectionCode(Me, New RemoteInfoEventArgs(SI.Encode, ServerPort))
        End If
        Return rmsr.ServiceURLs.ToArray
    End Function

    Public Sub RegistClientChannel()
        'System.Runtime.Remoting.RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off
        Dim bsprovider As New BinaryServerFormatterSinkProvider()
        Dim bcprovider As New BinaryClientFormatterSinkProvider()
        bsprovider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full
        Dim props As IDictionary = New Hashtable()
        props("name") = "BloodHimeClient"
        props("typeFilterLevel") = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full
        Dim channel As TcpChannel = New TcpChannel(props, bcprovider, bsprovider)
        ChannelServices.RegisterChannel(channel, False)
        ClientChannel = channel
    End Sub

    Public Function GetRemoteService(Of WellKnownType)(ByVal URI As String) As MarshalByRefObject

        '用来从远程地址当中获得服务对象
        Return Activator.GetObject(GetType(WellKnownType), URI)
    End Function

End Class

<Serializable()> Public Class ServiceInfo
    Public DNSAddress As String = "" '其他用户通过DNS地址来判断两台电脑是否在同一个局域网内 如果DNS地址不可用 则认为是没有外网可用的LAN
    Public IPEntries As New Dictionary(Of String, String) '列出本机所能提供的所有IP地址
    Public Services As New List(Of String) '列出所有的服务
    Public Name As String '用户名
    Public UUPI As String 'UUPI
    Public Function Encode() As String
        '把本地的信息包装起来
        'Dim RSAKey As String = "<RSAKeyValue><Modulus>tE+IyI+scgKAaLmHe3USizA5PUQ+pjUu3vsx/CtauAMoxQmCmPjj8o+soZj7j4OAxavCvJI5ofS1XThXrKdCTziZqi+vtfRWjbPSg949oaU3aqcxphEkcrszfvHzVooKTvPrMZ1dsuqSQAdjXNkbl3N7eQ0vK8zpjQydV8gOGAk=</Modulus><Exponent>AQAB</Exponent><P>9CsTHJQP/0B29F8LoKJWHI2/WjNxbj8GvuI6O3KMvv4g4c9VVut+Lt94V2nC5KhsIYoIpbT1JbCVzr1haRQpBw==</P><Q>vQxNOUG4bjH5Jv8vC13oG87tL9p+KiyAoH9VzTqiBKWxDPKTLUS2a/T6a57aP1MmtHiYqb8Se6+xSliZw7/Cbw==</Q><DP>iRXiDgqHDL1py/vM8GwUQsXGqGL3jXkvVV54aUYABX9ygLVCaaGf37sxuoozlOOGijIQdtCpTnbdNQyYs0FRYw==</DP><DQ>gAQDiyE43c4TVNf6qGzXz3TpYr1HMBHgrE5t8MKikhkNcqIVDdN44FQM+7GfQsBw9kavwkq0HnOTz699uHNzGw==</DQ><InverseQ>oWFxCWvKHDfwRQ8nJjUhXGLC10yqcLDdc30HxRhgMqYK7UKlgtCpzX6HqswXR4EizrZ7CJPzg63adgtlg2exoQ==</InverseQ><D>pWsB9d25Oq8I1wq+PIjfBkqreIWPZDQOWArjhJDRnVdoo7th0K0lknY0zdvnjXX7QV2ePhl4Phid+nOiG4cm0LGPfmkaAUtrI6ulozEwGHLZCuj4xoJiQ6ntIt1nuz3M8I03nxQ2JFdLrXE0QAQO560AHAy4DyrRuRb96/uMlzE=</D></RSAKeyValue>"
        Dim AC As New SecureMessage
        Return AC.EncrytAndSign(BinarySerializer.ToString(Me))
    End Function
    Public Shared Function Decode(ByVal Code As String) As ServiceInfo
        '用来分析远程发送过来的信息
        'Dim RSAKey As String = "<RSAKeyValue><Modulus>tE+IyI+scgKAaLmHe3USizA5PUQ+pjUu3vsx/CtauAMoxQmCmPjj8o+soZj7j4OAxavCvJI5ofS1XThXrKdCTziZqi+vtfRWjbPSg949oaU3aqcxphEkcrszfvHzVooKTvPrMZ1dsuqSQAdjXNkbl3N7eQ0vK8zpjQydV8gOGAk=</Modulus><Exponent>AQAB</Exponent><P>9CsTHJQP/0B29F8LoKJWHI2/WjNxbj8GvuI6O3KMvv4g4c9VVut+Lt94V2nC5KhsIYoIpbT1JbCVzr1haRQpBw==</P><Q>vQxNOUG4bjH5Jv8vC13oG87tL9p+KiyAoH9VzTqiBKWxDPKTLUS2a/T6a57aP1MmtHiYqb8Se6+xSliZw7/Cbw==</Q><DP>iRXiDgqHDL1py/vM8GwUQsXGqGL3jXkvVV54aUYABX9ygLVCaaGf37sxuoozlOOGijIQdtCpTnbdNQyYs0FRYw==</DP><DQ>gAQDiyE43c4TVNf6qGzXz3TpYr1HMBHgrE5t8MKikhkNcqIVDdN44FQM+7GfQsBw9kavwkq0HnOTz699uHNzGw==</DQ><InverseQ>oWFxCWvKHDfwRQ8nJjUhXGLC10yqcLDdc30HxRhgMqYK7UKlgtCpzX6HqswXR4EizrZ7CJPzg63adgtlg2exoQ==</InverseQ><D>pWsB9d25Oq8I1wq+PIjfBkqreIWPZDQOWArjhJDRnVdoo7th0K0lknY0zdvnjXX7QV2ePhl4Phid+nOiG4cm0LGPfmkaAUtrI6ulozEwGHLZCuj4xoJiQ6ntIt1nuz3M8I03nxQ2JFdLrXE0QAQO560AHAy4DyrRuRb96/uMlzE=</D></RSAKeyValue>"
        Dim AC As New SecureMessage
        Return BinarySerializer.FromString(AC.Decode(Code))
    End Function
End Class

Public Class RemoteServiceRecord
    Public ServiceRef As ObjRef
    Public ServiceName As String
    Public ServiceURLs As New List(Of String)
    Public Service As BaseService
End Class

Public Class RemoteInfoEventArgs
    Inherits EventArgs
    Public Code As String
    Public Port As String
    Public Sub New(ByVal vCode As String, ByVal vPort As String)
        Code = vCode
        Port = vPort
    End Sub
End Class

<Serializable()> Public Class BaseService
    Inherits MarshalByRefObject
    '其他服务请继承自此类型
    Public Overrides Function InitializeLifetimeService() As Object
        Return Nothing
    End Function

    Protected Sub OnMonitorInfo(ByVal User As String, ByVal ID As String, ByVal Operation As UserPermission, ByVal Detail As String, ByVal Time As DateTime, ByVal Identity As StaffGroups)
        RaiseEvent MonitorInfo(User, ID, [Enum].GetName(GetType(UserPermission), Operation), Detail, Time, [Enum].GetName(GetType(StaffGroups), Identity))
    End Sub

    Public Event MonitorInfo(ByVal User As String, ByVal ID As String, ByVal Operation As String, ByVal Detail As String, ByVal Time As DateTime, ByVal Identity As String)

    Public Shared SharedInstance As BaseService
    Public Name As String
    Public UUPI As String
    Public Key As String
    <NonSerialized()> Public Connections As New Dictionary(Of String, ServiceConnection)
    Public ServiceType As ServiceType
    <NonSerialized()> Public ServerInstance As BaseService
    Public Function GetSharedInstance() As BaseService
        If SharedInstance Is Nothing Then
            SharedInstance = New BaseService
        End If
        Return SharedInstance
    End Function

    Public Function Ping(ByVal InitTime As Date) As Date
        '用来判断此服务是否仍然可用
        Return InitTime
    End Function



    Public Sub GotMessage(ByVal tag As RouterTable, ByVal msg As String)
        RaiseEvent ReceivedMessage(tag, msg)
    End Sub

    Public Sub SendMessage(ByVal msg As String)
        Dim RT As New RouterTable
        RT.Init(UUPI)
        RaiseEvent BroadCastMessage(RT, msg)
    End Sub

    Public Sub GotData(ByVal vData As Bunbury, ByVal vSignature As String)
        RaiseEvent OnGotData(vData, vSignature)
    End Sub

    Public Sub SendData(ByVal vData As Bunbury, ByVal vSignature As String)
        '向指定的所有客户发送相同的信息和路由表
        Dim BrokenConnections As New List(Of String)
        Dim SC As ServiceConnection
        For Each ConKey As String In vData.Destinations
            '如果抛出异常 说明已经断开连接
            Try
                SC = Connections(ConKey)
                SC.SendData(vData, vSignature)
            Catch ex As Exception
                BrokenConnections.Add(ConKey)
            End Try
        Next

        For Each ConKey As String In BrokenConnections
            '如果抛出异常 说明已经断开连接
            Connections.Remove(ConKey)
        Next

        If BrokenConnections.Count > 0 Then
            RaiseEvent UpdateConnections(Key)
        End If
    End Sub

    Public Sub ConnectToRemote(ByVal remoteBaseService As BaseService)
        'AddHandler Me.BroadCastMessage, AddressOf remoteBaseService.GotMessage
        If ServiceType = ServiceType.Client Then ServerInstance = remoteBaseService
        Dim SC As New ServiceConnection
        SC.Name = remoteBaseService.Name
        SC.UUPI = remoteBaseService.UUPI
        SC.Connector = remoteBaseService
        SC.Method = ConnectionMethod.Subscribe
        If Connections.ContainsKey(SC.UUPI) Then
            Connections(SC.UUPI) = SC
        Else
            Connections.Add(SC.UUPI, SC)
        End If

        Dim remoteConnector As ServiceConnection = remoteBaseService.AddSubscriber(SC)
        'AddHandler SC.OnSendData, AddressOf remoteConnector.GotData

        '当收到消息时把信息发给自己来处理 决定是否继续传播
        'AddHandler SC.OnGotData, AddressOf Me.GotData
        RaiseEvent UpdateConnections(Key)
    End Sub

    Public Function AddSubscriber(ByVal remoteConnector As ServiceConnection) As ServiceConnection
        'Dim objref As ObjRef = remoteBaseService.CreateObjRef(remoteBaseService.GetType)
        'Dim s As String = ObjRef.URI
        'AddHandler Me.BroadCastMessage, AddressOf remoteBaseService.GotMessage

        Dim SC As New ServiceConnection
        'SC.Name = remoteBaseService.Name
        'SC.UUPI = remoteBaseService.UUPI
        'SC.Connector = remoteBaseService
        'SC.Method = ConnectionMethod.Broadcast
        'If Connections.ContainsKey(SC.UUPI) Then
        '    Connections(SC.UUPI) = SC
        'Else
        '    Connections.Add(SC.UUPI, SC)
        'End If


        'RaiseEvent UpdateConnections(Me.Key)

        'Dim TH As New ThreadHeader(Of String)(AddressOf OnUpdateConnections, Key)

        'AddHandler SC.OnSendData, AddressOf remoteConnector.GotData

        '当收到消息时把信息发给自己来处理 决定是否继续传播
        'AddHandler SC.OnGotData, AddressOf Me.GotData
        Return SC
    End Function

    Public Event OnGotData(ByVal vData As Bunbury, ByVal vSignature As String)

    Public Event BroadCastMessage(ByVal tag As RouterTable, ByVal msg As String)
    Public Event ReceivedMessage(ByVal tag As RouterTable, ByVal msg As String)

    Public Event UpdateConnections(ByVal Key As String)

End Class

<Serializable()> Public Class ServiceConnection
    Inherits MarshalByRefObject '这个必须是MarshalByRef 远程调用的
    '每一个连接都会自动生成两个匹配的ServiceConnection配对，适用于路由消息的传播。
    Public Overrides Function InitializeLifetimeService() As Object
        Return Nothing
    End Function
    Public Method As ConnectionMethod
    Public Name As String
    Public UUPI As String
    Public Connector As BaseService
    Public Sub GotData(ByVal vData As Bunbury, ByVal vSignature As String)
        '每收到一个消息之后 都会验证这个消息的收件人是不是自己
        '如果消息收件人不是自己 那么就把消息转发出去
        RaiseEvent OnGotData(vData, vSignature)
    End Sub
    Public Sub SendData(ByVal vData As Bunbury, ByVal vSignature As String)
        RaiseEvent OnSendData(vData, vSignature)
    End Sub
    Public Event OnSendData(ByVal vData As Bunbury, ByVal vSignature As String)
    Public Event OnGotData(ByVal vData As Bunbury, ByVal vSignature As String)
End Class

<Serializable()> Public Enum ConnectionMethod As Integer
    Subscribe
    Broadcast
End Enum

<Serializable()> Public Class RouterTable
    Inherits Dictionary(Of String, InfoRouterState) '如果一个类型不是MarshalByRef，那么可能是把整个类型的数据序列化之后发送过去 在远程创建一个新的实例
    Implements System.Runtime.Serialization.ISerializable
    '这个类型专门用来用来进行路由
    Public Sub New()

    End Sub
    Public Sub New(ByVal UUPI As String)

    End Sub
    Public Sub New(ByVal si As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(si, context)
    End Sub
    Public Overrides Sub GetObjectData(ByVal si As SerializationInfo, ByVal context As StreamingContext)
        MyBase.GetObjectData(si, context)
    End Sub
    Public Sub Init(ByVal UUPI As String)
        Me.Add(UUPI, InfoRouterState.GotInfo)
    End Sub
    Public Function CheckAndForward(ByVal UUPI As String) As Boolean
        If ContainsKey(UUPI) Then
            Return False
        Else
            Me.Add(UUPI, InfoRouterState.GotInfo)
            Return True
        End If
    End Function
    Public Function TryForward(ByVal UUPI As String) As Boolean
        If Not (ContainsKey(UUPI)) OrElse Me(UUPI) = 0 Then
            Add(UUPI, InfoRouterState.Forwarded)
            Return True
        Else
            Return False
        End If
    End Function
End Class

<Serializable()> Public Enum InfoRouterState As Integer
    NotEngaged = 0
    GotInfo = 1
    Forwarded = 2
End Enum