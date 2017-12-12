Public Class WebProxy(Of T As MarshalByRefObject, TSerializer As {New, ISerializer})
    Inherits System.Runtime.Remoting.Proxies.RealProxy
    Private Processor As WebCallProcessor
    Private ProxyName As String
    Private WebURL As String
    Public Property Identity As Object
    Private DataSerializer As New TSerializer
    Public Sub New(URL As String, Optional Name As String = Nothing)
        MyBase.New(GetType(T))
        Dim vT As Type = GetType(T)
        If Name Is Nothing Then Name = vT.Name
        ProxyName = Name
        WebURL = URL
    End Sub
    Public Overrides Function Invoke(msg As Runtime.Remoting.Messaging.IMessage) As Runtime.Remoting.Messaging.IMessage
        Dim imc As System.Runtime.Remoting.Messaging.IMethodCallMessage = msg
        Dim rpco As New RPCObject
        rpco.Identity = Identity
        rpco.Host = ProxyName
        rpco.Parameters.AddRange(imc.Args)
        rpco.MethodBase = imc.MethodBase
        Dim wc As New System.Net.WebClient
        Dim data As New System.Collections.Specialized.NameValueCollection
        data.Add("Service", ProxyName)
        data.Add("Method", "Invoke")
        data.Add("CallContext", Convert.ToBase64String(DataSerializer.Serialize(rpco)))
        Dim res = wc.UploadValues(WebURL, data)
        rpco = DataSerializer.Deserialize(res)
        Dim rmsg As New ReturnMessage(msg, rpco.ReturnValue)
        Return rmsg
    End Function
End Class


Public MustInherit Class WebCallProcessor
    Inherits MarshalByRefObject
    Friend Shared ServerEntries As New Hashtable
    Friend Shared ProxyList As New List(Of WebCallProcessor)
    Public Shared Sub Register(Of T As MarshalByRefObject, TSerializer As {New, ISerializer})(Server As T, Optional Name As String = Nothing)
        Dim vT As Type = GetType(T)
        If Name Is Nothing Then Name = vT.Name
        If ServerEntries.ContainsKey(Name) Then Return
        ServerEntries.Add(Name, Server)
        Dim po As New WebCallProcessor(Of TSerializer)
        ProxyList.Add(po)
    End Sub
    ''' <summary>
    ''' 这个函数需要在HTTP系统当中调用 关键问题是如何进行身份验证
    ''' 目前将这个函数声明为overridable以便于在继承时加入身份验证机制
    ''' </summary>
    ''' <param name="Name"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function Process(Name As String, value As Byte()) As Byte()
End Class

Public Class WebCallProcessor(Of TSerializer As {New, ISerializer})
    Inherits WebCallProcessor
    Private DataSerializer As New TSerializer
    Public Overloads Shared Sub Register(Of T As MarshalByRefObject)(Server As T, Optional Name As String = Nothing)
        WebCallProcessor.Register(Of T, TSerializer)(Server, Name)
    End Sub
    Public Overrides Function Process(Name As String, value As Byte()) As Byte()
        Dim rpco As RPCObject = DataSerializer.Deserialize(value)
        If TypeOf ServerEntries(Name) Is IRPCIndentityInvokable Then
            Using DirectCast(ServerEntries(Name), IRPCIndentityInvokable).LockIdentity(rpco.Identity)
                rpco.ReturnValue = rpco.MethodBase.Invoke(ServerEntries(Name), rpco.Parameters.ToArray)
            End Using
        Else
            rpco.ReturnValue = rpco.MethodBase.Invoke(ServerEntries(Name), rpco.Parameters.ToArray)
        End If
        Return DataSerializer.Serialize(rpco)
    End Function
End Class
