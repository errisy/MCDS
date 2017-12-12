Public Class HTTPProxy(Of T As {IRemoteProcessCall, MarshalByRefObject})
    Inherits System.Runtime.Remoting.Proxies.RealProxy
    Implements IRemoteProcessCallProxy, ISerializationWrapper
    Private objref As T
    Public Sub New(obj As T)
        MyBase.New(GetType(T))
        objref = obj
    End Sub
    Public Overrides Function Invoke(msg As System.Runtime.Remoting.Messaging.IMessage) As System.Runtime.Remoting.Messaging.IMessage
        Dim imc As System.Runtime.Remoting.Messaging.IMethodCallMessage = msg
        Select Case imc.MethodName
            Case "set_Target"
                objref.Target = imc.Args(0)
                Dim rmsg As New ReturnMessage(msg, Nothing)
                Return rmsg
            Case "get_Target"
                Dim rmsg As New ReturnMessage(msg, objref.Target)
                Return rmsg
            Case "set_Name"
                objref.Name = imc.Args(0)
                Dim rmsg As New ReturnMessage(msg, Nothing)
                Return rmsg
            Case "get_Name"
                Dim rmsg As New ReturnMessage(msg, objref.Name)
                Return rmsg
            Case "set_Identity"
                objref.Identity = imc.Args(0)
                Dim rmsg As New ReturnMessage(msg, Nothing)
                Return rmsg
            Case "get_Identity"
                Dim rmsg As New ReturnMessage(msg, objref.Identity)
                Return rmsg
            Case "Local"
                Dim rmsg As New ReturnMessage(msg, objref)
                Return rmsg
            Case Else
                Dim rpco As New RPCObject
                rpco.Parameters.AddRange(imc.Args)
                rpco.MethodBase = imc.MethodBase
                rpco.Identity = objref.Identity
                rpco.Host = objref.Name
                Dim post As New PostPackage
                post.AddData("httpcall", ShallowSerializer.Serialize(rpco, Me).ToBase64)
                Dim resTask = post.PostData(objref.Target)
                resTask.Wait()
                Dim res = resTask.Result
                Dim bytesTask = res.GetResponseStream.ToBytes
                Dim bytes = bytesTask.Result
                Dim returnobj As RPCObject = ShallowSerializer.Deserialize(bytes, Me)
                Dim rmsg As New ReturnMessage(msg, returnobj.ReturnValue)
                Return rmsg
        End Select
    End Function
    Public Property Identity As Object Implements IRemoteProcessCall.Identity
        Get
            Return objref.Identity
        End Get
        Set(value As Object)
            objref.Identity = value
        End Set
    End Property
    Public Event InvokeRemote(sender As Object, e As RPCEventArgs) Implements IRemoteProcessCallProxy.InvokeRemote
    Public Property Name As String Implements IRemoteProcessCall.Name
        Get
            Return objref.Name
        End Get
        Set(value As String)
            objref.Name = value
        End Set
    End Property
    Public Function Process(value As RPCObject) As RPCObject Implements IRemoteProcessCallProxy.Process
        SourceIdentity = value.Identity
        value.ReturnValue = value.MethodBase.Invoke(objref, value.Parameters.ToArray)
        Return value
    End Function

    Public Function Proxy() As Object Implements IRemoteProcessCallProxy.Proxy
        Return GetTransparentProxy()
    End Function

    Public Property SourceIdentity As Object Implements IRemoteProcessCall.SourceIdentity
        Get
            Return objref.SourceIdentity
        End Get
        Set(value As Object)
            objref.SourceIdentity = value
        End Set
    End Property

    Public Property Target As Object Implements IRemoteProcessCall.Target
        Get
            Return objref.Target
        End Get
        Set(value As Object)
            objref.Target = value
        End Set
    End Property

    Public Function Local() As IRemoteProcessCall Implements IRemoteProcessCall.Local
        Return objref
    End Function
#Region "Wrapper"
    Private _WrapperKey2Obj As New Hashtable
    Private _WrapperObj2Key As New Hashtable
    Public Function RegisterWrapper(key As String, value As Object) As Boolean
        If _WrapperKey2Obj.ContainsKey(key) Then
            Dim o = _WrapperKey2Obj(key)
            _WrapperKey2Obj.Remove(key)
            _WrapperObj2Key.Remove(o)
            _WrapperKey2Obj.Add(key, value)
            _WrapperObj2Key.Add(value, key)
            Return False
        Else
            _WrapperKey2Obj.Add(key, value)
            _WrapperObj2Key.Add(value, key)
            Return True
        End If
    End Function
    Public Function UnRegisterWrapper(key As String) As Boolean
        If _WrapperKey2Obj.ContainsKey(key) Then
            Dim o = _WrapperKey2Obj(key)
            _WrapperKey2Obj.Remove(key)
            _WrapperObj2Key.Remove(o)
            Return False
        Else
            Return True
        End If
    End Function
    Public Function Unwrap(obj As Object) As Object Implements ISerializationWrapper.Unwrap
        If TypeOf obj Is RemoteProcessCallReference Then
            Return _WrapperKey2Obj(DirectCast(obj, RemoteProcessCallReference).Name)
        End If
        Return obj
    End Function
    Public Function Wrap(obj As Object) As Object Implements ISerializationWrapper.Wrap
        If _WrapperObj2Key.ContainsKey(obj) Then
            Return New RemoteProcessCallReference With {.Name = _WrapperObj2Key(obj)}
        End If
        Return obj
    End Function
#End Region
End Class

Public Module HTTPExtension
    <System.Runtime.CompilerServices.Extension()> Public Function ToBase64(value As Byte()) As String
        Return Convert.ToBase64String(value)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function FromBase64(value As String) As Byte()
        Return Convert.FromBase64String(value)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function ToBytes(stream As System.IO.Stream) As System.Threading.Tasks.Task(Of Byte())
        Dim t As New System.Threading.Tasks.Task(Of Byte())(Function() As Byte()
                                                                Return ReadStreamBytes(stream)
                                                            End Function)
        t.Start()
        Return t
    End Function
    Private Function ReadStreamBytes(stream As System.IO.Stream) As Byte()
        Dim buffer As Byte() = New Byte(4095) {}
        Dim read As Integer = 0
        Dim bytes As New List(Of Byte)
        Do
            read = stream.Read(buffer, 0, 4096)
            If read > 0 Then bytes.AddRange(buffer.Take(read))
        Loop While read > 0
        Return bytes.ToArray
    End Function

End Module

'Public Class HTTPCallController
'    Inherits System.Web.Mvc.Controller
'    Implements ISerializationWrapper
'    '<System.Web.Mvc.HttpPost()>
'    'Public Function Index(httpcall As String) As System.Web.Mvc.ActionResult
'    '    Dim bytes = httpcall.FromBase64
'    '    Dim rpco = ShallowSerializer.Deserialize(bytes, Me)
'    '    rpco = DispatchCall(rpco)
'    '    Return New System.Web.Mvc.FileContentResult(ShallowSerializer.Serialize(rpco, Me), "image/jpeg")
'    'End Function
'    Overridable Function ProcessCall(value As String) As System.Web.Mvc.ActionResult
'        Dim bytes = value.FromBase64
'        Dim rpco = ShallowSerializer.Deserialize(bytes, Me)
'        rpco = DispatchCall(rpco)
'        Return New System.Web.Mvc.FileContentResult(ShallowSerializer.Serialize(rpco, Me), "image/jpeg")
'    End Function
'    Private _Entries As New Dictionary(Of String, IRemoteProcessCallProxy)
'    Public Function Register(Of T As {IRemoteProcessCall, MarshalByRefObject})(RPCInstance As T) As T
'        If _Entries.ContainsKey(RPCInstance.Name) Then
'            _Entries(RPCInstance.Name) = New Proxy(Of T)(RPCInstance)
'        Else
'            _Entries.Add(RPCInstance.Name, New Proxy(Of T)(RPCInstance))
'        End If
'        Return _Entries(RPCInstance.Name).Proxy
'    End Function
'    Public Function Unregister(Name As String) As IRemoteProcessCall
'        If _Entries.ContainsKey(Name) Then
'            Dim proxy = _Entries(Name)
'            _Entries.Remove(Name)
'            Return proxy
'        Else
'            Return Nothing
'        End If
'    End Function
'    Private Function DispatchCall(rpcObj As RPCObject) As RPCObject
'        If _Entries.ContainsKey(rpcObj.Host) Then
'            Return _Entries(rpcObj.Host).Process(rpcObj)
'        End If
'        Return rpcObj
'    End Function
'#Region "Wrapper"
'    Private Shared _WrapperKey2Obj As New Hashtable
'    Private Shared _WrapperObj2Key As New Hashtable
'    Public Shared Function RegisterWrapper(key As String, value As Object) As Boolean
'        If _WrapperKey2Obj.ContainsKey(key) Then
'            Dim o = _WrapperKey2Obj(key)
'            _WrapperKey2Obj.Remove(key)
'            _WrapperObj2Key.Remove(o)
'            _WrapperKey2Obj.Add(key, value)
'            _WrapperObj2Key.Add(value, key)
'            Return False
'        Else
'            _WrapperKey2Obj.Add(key, value)
'            _WrapperObj2Key.Add(value, key)
'            Return True
'        End If
'    End Function
'    Public Shared Function UnRegisterWrapper(key As String) As Boolean
'        If _WrapperKey2Obj.ContainsKey(key) Then
'            Dim o = _WrapperKey2Obj(key)
'            _WrapperKey2Obj.Remove(key)
'            _WrapperObj2Key.Remove(o)
'            Return False
'        Else
'            Return True
'        End If
'    End Function
'    Public Function Unwrap(obj As Object) As Object Implements ISerializationWrapper.Unwrap
'        If TypeOf obj Is RemoteProcessCallReference Then
'            Return _WrapperKey2Obj(DirectCast(obj, RemoteProcessCallReference).Name)
'        End If
'        Return obj
'    End Function
'    Public Function Wrap(obj As Object) As Object Implements ISerializationWrapper.Wrap
'        If _WrapperObj2Key.ContainsKey(obj) Then
'            Return New RemoteProcessCallReference With {.Name = _WrapperObj2Key(obj)}
'        End If
'        Return obj
'    End Function
'#End Region
'End Class