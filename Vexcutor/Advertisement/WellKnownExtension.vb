Imports System.Runtime.Remoting, System.Runtime.Remoting.Channels, System.Runtime.Remoting.Channels.Tcp

Public Module WellKnownExtension
    <System.Runtime.CompilerServices.Extension()> Public Sub Register(Of T As MarshalByRefObject)(WellKnownObject As T, mode As WellKnownObjectMode, Optional Name As String = Nothing)
        Dim vT As Type = WellKnownObject.GetType
        If Name Is Nothing Then Name = vT.Name
        RemotingConfiguration.RegisterWellKnownServiceType(WellKnownObject.GetType, Name, mode)
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Function GetService(Of T As MarshalByRefObject)(WellKnownObject As T, IPAddress As String, Port As Integer, Optional Name As String = Nothing) As T
        If Name Is Nothing Then Name = GetType(T).Name
        Return Activator.GetObject(GetType(T), String.Format("tcp://{0}:{1}/{2}", IPAddress, Port, Name))
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function GetService(Of T)(IPAddress As String, Port As Integer, Optional Name As String = Nothing) As T

        'Dim synService As New SynContract.SynServerProxy("http://localhost:2681/data/Index")
        Dim synService As New SynContract.HTTPWebServiceProxy(Of SynContract.ISynData)("http://synthenome.org/Data/Index")
        Return synService.GetTransparentProxy

        'If Name Is Nothing Then Name = GetType(T).Name
        'Dim obj As T = Activator.GetObject(GetType(T), String.Format("tcp://{0}:{1}/{2}", IPAddress, Port, Name))
        'Return obj
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function GetService(Of T As MarshalByRefObject)(WellKnownObject As T, tcpURI As String, Optional Name As String = Nothing) As T
        If Name Is Nothing Then Name = GetType(T).Name
        Dim obj As T = Activator.GetObject(GetType(T), String.Format("{0}/{1}", tcpURI, Name))
        Return obj
    End Function
    Private CurrentDomainServer As TcpChannel
    <System.Runtime.CompilerServices.Extension()> Public Function StartServer(App As AppDomain, PortNumber As Integer) As TcpChannel
        If TypeOf CurrentDomainServer Is TcpChannel Then
            App.StopServer()
        End If
        Dim bsprovider As New BinaryServerFormatterSinkProvider()
        Dim bcprovider As New BinaryClientFormatterSinkProvider()
        bsprovider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full
        Dim props As IDictionary = New Hashtable()
        '注册服务通道
        props("name") = System.Reflection.Assembly.GetExecutingAssembly.GetName.Name
        props("port") = PortNumber
        props("typeFilterLevel") = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full
        Dim channel As TcpChannel = New TcpChannel(props, bcprovider, bsprovider)
        ChannelServices.RegisterChannel(channel, False)
        CurrentDomainServer = channel
        Return channel
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Sub StopServer(App As AppDomain)
        If TypeOf CurrentDomainServer Is TcpChannel Then
            Try
                ChannelServices.UnregisterChannel(CurrentDomainServer)
            Catch ex As Exception

            End Try
            CurrentDomainServer = Nothing
        End If
    End Sub
    Private regexPort As New System.Text.RegularExpressions.Regex("\:([\d]+)")
    <System.Runtime.CompilerServices.Extension()> Public Function Port(ref As ObjRef) As Integer
        Dim s As String = ref.ChannelInfo.ChannelData.GetValue(1).ChannelUris(0)
        Return CInt(regexPort.Match(s).Groups(1).Value)
    End Function
End Module

Public Module AsyncTask
    Public Function [Async](Of T)(f As Func(Of T)) As System.Threading.Tasks.Task(Of T)
        Dim _Task As New System.Threading.Tasks.Task(Of T)(f)
        _Task.Start()
        Return _Task
    End Function
End Module