Imports System.Windows
Public Class RPCClient
    Inherits DependencyObject
    Implements ISerializationWrapper
    Protected WithEvents _Connection As IConnection
    Public Sub New()
        MyBase.New()
    End Sub
    'DataPort -> Connection As IConnection Default: Nothing
    Public Property Connection As IConnection
        Get
            Return GetValue(ConnectionProperty)
        End Get
        Set(ByVal value As IConnection)
            SetValue(ConnectionProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ConnectionProperty As DependencyProperty = _
                            DependencyProperty.Register("Connection", _
                            GetType(IConnection), GetType(RPCClient), _
                            New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedConnectionChanged)))
    Public Shared ReadOnly ConnectionPropertySerializer As Serializer = Serializer.Save(ConnectionProperty, GetType(RPCClient))
    Private Shared Sub SharedConnectionChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, RPCClient).ConnectionChanged(d, e)
    End Sub
    Private Sub ConnectionChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        _Connection = e.NewValue
    End Sub
#Region "Util"
    Private _Entries As New Dictionary(Of String, IRemoteProcessCallProxy)
    Public Function Register(Of T As {IRemoteProcessCall, MarshalByRefObject})(RPCInstance As T) As T
        If _Entries.ContainsKey(RPCInstance.Name) Then
            RemoveHandler _Entries(RPCInstance.Name).InvokeRemote, AddressOf InvokeCall
            _Entries(RPCInstance.Name) = New Proxy(Of T)(RPCInstance)
            AddHandler _Entries(RPCInstance.Name).InvokeRemote, AddressOf InvokeCall
        Else
            _Entries.Add(RPCInstance.Name, New Proxy(Of T)(RPCInstance))
            AddHandler _Entries(RPCInstance.Name).InvokeRemote, AddressOf InvokeCall
        End If
        Return _Entries(RPCInstance.Name).Proxy
    End Function
    Public Function Unregister(Name As String) As IRemoteProcessCall
        If _Entries.ContainsKey(Name) Then
            Dim proxy = _Entries(Name)
            RemoveHandler proxy.InvokeRemote, AddressOf InvokeCall
            _Entries.Remove(Name)
            Return proxy
        Else
            Return Nothing
        End If
    End Function
    Private Sub InvokeCall(sender As Object, e As RPCEventArgs)
        Dim hash As Integer = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(e)
        CallContext.Add(hash, e)
        e.Value.ID = hash
        _Connection.Send(New Package(e.Value, PackageFlag.RemoteProcessCall, Me))
    End Sub
    Private CallContext As New Dictionary(Of Integer, RPCEventArgs)
    Private Sub _Connection_ObjectReceived(sender As Object, e As PackageReceivedEventArgs) Handles _Connection.ObjectReceived
        Select Case e.Value.Flag
            Case PackageFlag.RemoteProcessCall
                Dim rpco As RPCObject = e.Value.Child(Me)
                rpco.Source = e.Source
                rpco = DispatchCall(rpco)
                _Connection.Send(New Package(rpco, PackageFlag.RemoteProcessCallBack, Me))
            Case PackageFlag.RemoteProcessCallBack
                Dim rpco As RPCObject = e.Value.Child(Me)
                EndCall(rpco)
        End Select
    End Sub
    Private Sub EndCall(rpco As RPCObject)
        If CallContext.ContainsKey(rpco.ID) Then
            Dim rpce = CallContext(rpco.ID)
            CallContext.Remove(rpco.ID)
            rpce.Value.ReturnValue = rpco.ReturnValue
            rpce.EndCall(RPCCallbackEnum.Success)
        End If
    End Sub
    Private Function DispatchCall(rpcObj As RPCObject) As RPCObject
        If _Entries.ContainsKey(rpcObj.Host) Then
            Return _Entries(rpcObj.Host).Process(rpcObj)
        End If
        Return rpcObj
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
#End Region
End Class
Public Class RPCServer
    Inherits DependencyObject
    Implements ISerializationWrapper
    Private WithEvents _Port As IPort
    'ObjectServer -> Port As IPort Default: Nothing
    Public Property Port As IPort
        Get
            Return GetValue(PortProperty)
        End Get
        Set(ByVal value As IPort)
            SetValue(PortProperty, value)
        End Set
    End Property
    Public Shared ReadOnly PortProperty As DependencyProperty = _
                            DependencyProperty.Register("Port", _
                            GetType(IPort), GetType(RPCServer), _
                            New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedPortChanged)))
    Public Shared ReadOnly PortPropertySerializer As Serializer = Serializer.Save(PortProperty, GetType(RPCServer))
    Private Shared Sub SharedPortChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, RPCServer).PortChanged(d, e)
    End Sub
    Private Sub PortChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        _Port = e.NewValue
    End Sub
#Region "Util"
    Private _Entries As New Dictionary(Of String, IRemoteProcessCallProxy)
    Public Function Register(Of T As {IRemoteProcessCall, MarshalByRefObject})(RPCInstance As T) As T
        If _Entries.ContainsKey(RPCInstance.Name) Then
            RemoveHandler _Entries(RPCInstance.Name).InvokeRemote, AddressOf InvokeCall
            _Entries(RPCInstance.Name) = New Proxy(Of T)(RPCInstance)
            AddHandler _Entries(RPCInstance.Name).InvokeRemote, AddressOf InvokeCall
        Else
            _Entries.Add(RPCInstance.Name, New Proxy(Of T)(RPCInstance))
            AddHandler _Entries(RPCInstance.Name).InvokeRemote, AddressOf InvokeCall
        End If
        Return _Entries(RPCInstance.Name).Proxy
    End Function
    Public Function Unregister(Name As String) As IRemoteProcessCall
        If _Entries.ContainsKey(Name) Then
            Dim proxy = _Entries(Name)
            RemoveHandler proxy.InvokeRemote, AddressOf InvokeCall
            _Entries.Remove(Name)
            Return proxy
        Else
            Return Nothing
        End If
    End Function
    Private Sub InvokeCall(sender As Object, e As RPCEventArgs)
        Dim hash As Integer = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(e)
        CallContext.Add(hash, e)
        e.Value.ID = hash
        'If e.Target IsNot Nothing Then
        '    For Each conn In _Port.Connections
        '        If conn.Identification.Equals(e.Target) Then
        '            _Port.SendTo(New Package(e.Value, PackageFlag.RemoteProcessCall, Me), conn.URI)
        '        End If
        '    Next
        'End If
        _Port.SendTo(New Package(e.Value, PackageFlag.RemoteProcessCall, Me), e.Target)
    End Sub
    Private CallContext As New Dictionary(Of Integer, RPCEventArgs)
    Private Sub _Connection_ObjectReceived(sender As Object, e As PackageReceivedEventArgs) Handles _Port.ObjectReceived
        Select Case e.Value.Flag
            Case PackageFlag.RemoteProcessCall
                Dim rpco As RPCObject = e.Value.Child(Me)
                rpco.Source = e.Source
                rpco = DispatchCall(rpco)
                _Port.SendTo(New Package(rpco, PackageFlag.RemoteProcessCallBack, Me), rpco.Source)
            Case PackageFlag.RemoteProcessCallBack
                Dim rpco As RPCObject = e.Value.Child(Me)
                EndCall(rpco)
        End Select
    End Sub
    Private Sub EndCall(rpco As RPCObject)
        If CallContext.ContainsKey(rpco.ID) Then
            Dim rpce = CallContext(rpco.ID)
            CallContext.Remove(rpco.ID)
            rpce.Value.ReturnValue = rpco.ReturnValue
            rpce.EndCall(RPCCallbackEnum.Success)
        End If
    End Sub
    Private Function DispatchCall(rpcObj As RPCObject) As RPCObject
        If _Entries.ContainsKey(rpcObj.Host) Then
            Return _Entries(rpcObj.Host).Process(rpcObj)
        End If
        Return rpcObj
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
#End Region
End Class