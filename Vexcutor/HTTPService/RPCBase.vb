Imports System.Runtime.Remoting.Messaging
<Serializable>
Public Class RPCObject
    'RPCObject -> Host As String
    Public Host As String
    Private Shared Sub HostFieldSetter(obj As Object, value As Object)
        DirectCast(obj, Object).Host = value
    End Sub
    Private Shared Function HostFieldGetter(obj As Object) As Object
        Return DirectCast(obj, Object).Host
    End Function
    Public Shared ReadOnly HostFieldSerializer As Serializer = Serializer.Save("Host", GetType(RPCObject), AddressOf HostFieldSetter, AddressOf HostFieldGetter)
    'RPCObject -> BeginTime As Date
    Public BeginTime As Date = Now
    Private Shared Sub BeginTimeFieldSetter(obj As Object, value As Object)
        DirectCast(obj, Object).BeginTime = value
    End Sub
    Private Shared Function BeginTimeFieldGetter(obj As Object) As Object
        Return DirectCast(obj, Object).BeginTime
    End Function
    Public Shared ReadOnly BeginTimeFieldSerializer As Serializer = Serializer.Save("BeginTime", GetType(RPCObject), AddressOf BeginTimeFieldSetter, AddressOf BeginTimeFieldGetter)
    'RPCObject -> Method As String
    'Public Method As String
    'Private Shared Sub MethodFieldSetter(obj As Object, value As Object)
    '    DirectCast(obj, RPCObject).Method = value
    'End Sub
    'Private Shared Function MethodFieldGetter(obj As Object) As Object
    '    Return DirectCast(obj, RPCObject).Method
    'End Function
    'Public Shared ReadOnly MethodFieldSerializer As Serializer = Serializer.Save("Method", GetType(RPCObject), AddressOf MethodFieldSetter, AddressOf MethodFieldGetter)
    'RPCObject -> Parameters As ShallowDictionary(Of String, Object)
    Public Parameters As New ShallowList(Of Object)
    Private Shared Sub ParametersFieldSetter(obj As Object, value As Object)
        DirectCast(obj, RPCObject).Parameters = value
    End Sub
    Private Shared Function ParametersFieldGetter(obj As Object) As Object
        Return DirectCast(obj, RPCObject).Parameters
    End Function
    Public Shared ReadOnly ParametersFieldSerializer As Serializer = Serializer.Save("Parameters", GetType(RPCObject), AddressOf ParametersFieldSetter, AddressOf ParametersFieldGetter)
    'RPCObject -> ReturnValues As Object 
    Public ReturnValue As Object
    Private Shared Sub ReturnValuesFieldSetter(obj As Object, value As Object)
        DirectCast(obj, RPCObject).ReturnValue = value
    End Sub
    Private Shared Function ReturnValuesFieldGetter(obj As Object) As Object
        Return DirectCast(obj, RPCObject).ReturnValue
    End Function
    Public Shared ReadOnly ReturnValuesFieldSerializer As Serializer = Serializer.Save("ReturnValue", GetType(RPCObject), AddressOf ReturnValuesFieldSetter, AddressOf ReturnValuesFieldGetter)

    'RPCObject -> ID As Integer
    Public ID As Integer
    Private Shared Sub IDFieldSetter(obj As Object, value As Object)
        DirectCast(obj, RPCObject).ID = value
    End Sub
    Private Shared Function IDFieldGetter(obj As Object) As Object
        Return DirectCast(obj, RPCObject).ID
    End Function
    Public Shared ReadOnly IDFieldSerializer As Serializer = Serializer.Save("ID", GetType(RPCObject), AddressOf IDFieldSetter, AddressOf IDFieldGetter)

    'RPCObject -> Source As String
    Public Source As String
    Private Shared Sub SourceFieldSetter(obj As Object, value As Object)
        DirectCast(obj, RPCObject).Source = value
    End Sub
    Private Shared Function SourceFieldGetter(obj As Object) As Object
        Return DirectCast(obj, RPCObject).Source
    End Function
    Public Shared ReadOnly SourceFieldSerializer As Serializer = Serializer.Save("Source", GetType(RPCObject), AddressOf SourceFieldSetter, AddressOf SourceFieldGetter)

    'RPCObject -> Identity As Object
    Public Identity As Object
    Private Shared Sub IdentityFieldSetter(obj As Object, value As Object)
        DirectCast(obj, RPCObject).Identity = value
    End Sub
    Private Shared Function IdentityFieldGetter(obj As Object) As Object
        Return DirectCast(obj, RPCObject).Identity
    End Function
    Public Shared ReadOnly IdentityFieldSerializer As Serializer = Serializer.Save("Identity", GetType(RPCObject), AddressOf IdentityFieldSetter, AddressOf IdentityFieldGetter)

    'RPCObject -> ErrorMessage As String
    Public ErrorMessage As String
    Private Shared Sub ErrorMessageFieldSetter(obj As Object, value As Object)
        DirectCast(obj, RPCObject).ErrorMessage = value
    End Sub
    Private Shared Function ErrorMessageFieldGetter(obj As Object) As Object
        Return DirectCast(obj, RPCObject).ErrorMessage
    End Function
    Public Shared ReadOnly ErrorMessageFieldSerializer As Serializer = Serializer.Save("ErrorMessage", GetType(RPCObject), AddressOf ErrorMessageFieldSetter, AddressOf ErrorMessageFieldGetter)
    'RPCObject -> CallBackType As RPCCallbackEnum
    Public CallBackType As RPCCallbackEnum
    Private Shared Sub CallBackTypeFieldSetter(obj As Object, value As Object)
        DirectCast(obj, RPCObject).CallBackType = value
    End Sub
    Private Shared Function CallBackTypeFieldGetter(obj As Object) As Object
        Return DirectCast(obj, RPCObject).CallBackType
    End Function
    Public Shared ReadOnly CallBackTypeFieldSerializer As Serializer = Serializer.Save("CallBackType", GetType(RPCObject), AddressOf CallBackTypeFieldSetter, AddressOf CallBackTypeFieldGetter)
    'RPCObject -> MethodBase As System.Reflection.MethodBase
    Public MethodBase As System.Reflection.MethodBase
    Private Shared Sub MethodBaseFieldSetter(obj As Object, value As Object)
        DirectCast(obj, RPCObject).MethodBase = value
    End Sub
    Private Shared Function MethodBaseFieldGetter(obj As Object) As Object
        Return DirectCast(obj, RPCObject).MethodBase
    End Function
    Public Shared ReadOnly MethodBaseFieldSerializer As Serializer = Serializer.Save("MethodBase", GetType(RPCObject), AddressOf MethodBaseFieldSetter, AddressOf MethodBaseFieldGetter)

End Class

Public Class RPCEventArgs
    Inherits EventArgs
    Public Value As RPCObject
    Private AutoReset As New System.Threading.AutoResetEvent(False)
    Private CallBack As RPCCallbackEnum = RPCCallbackEnum.Timeout
    'Private RemoteCallTask As New System.Threading.Tasks.Task(Of RPCCallbackEnum)(Function()
    '                                                                                  AutoReset.WaitOne()
    '                                                                                  Return System.Threading.Thread.VolatileRead(CallBack)
    '                                                                              End Function)
    Public Sub EndCall(vCallBack As RPCCallbackEnum)
        System.Threading.Thread.VolatileWrite(CallBack, vCallBack)
        AutoReset.Set()

    End Sub
    Public Sub WaitCallBack()
        AutoReset.WaitOne(10000)
        If System.Threading.Thread.VolatileRead(CallBack) = RPCCallbackEnum.Timeout Then
            Value.ReturnValue = Nothing
        End If

    End Sub
    'Public Function WaitCallBack() As System.Threading.Tasks.Task(Of RPCCallbackEnum)
    '    RemoteCallTask.Start()
    '    Return RemoteCallTask
    'End Function
    Private _StartTime As Date = Now
    Public ReadOnly Property StartTime As Date
        Get
            Return _StartTime
        End Get
    End Property
    Public Target As Object
End Class
Public Enum RPCCallbackEnum
    Success
    Fail
    Timeout
End Enum
Public Interface IRemoteProcessCall
    ''' <summary>
    ''' 注册名
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Name As String
    Property Identity As Object
    Property SourceIdentity As Object
    Property Target As Object
    Function Local() As IRemoteProcessCall
End Interface

Public Interface IRemoteProcessCallProxy
    Inherits IRemoteProcessCall
    Function Process(value As RPCObject) As RPCObject
    Event InvokeRemote(sender As Object, e As RPCEventArgs)
    Function Proxy() As Object
End Interface
<Serializable()>
Public NotInheritable Class RemoteProcessCallReference
    Public Name As String
End Class
'Public Class Server
'    Inherits System.Dynamic.DynamicObject
'    Public Sub UK()
'        Dim s As Server = New Server



'    End Sub
'    Public Overrides Function TryInvoke(binder As System.Dynamic.InvokeBinder, args() As Object, ByRef result As Object) As Boolean
'        Return MyBase.TryInvoke(binder, args, result)
'    End Function
'End Class

'Public Module IRemoteProcessCallExtension
'    <System.Runtime.CompilerServices.Extension()> Public Async Function InvokeCall(iRPC As IRemoteProcessCall, Method As String, ParamArray Parameters As RPCParam()) As System.Threading.Tasks.Task(Of RPCObject)
'        Dim rpco As New RPCObject With {.Host = iRPC.Name, .Identity = iRPC.Identity}
'        For Each pa In Parameters
'            If pa.Key IsNot Nothing AndAlso pa.Key.Length > 0 Then rpco.Parameters.Add(pa.Value)
'        Next
'        Dim rpce As New RPCEventArgs With {.Value = rpco}
'        iRPC.OnInvoke(rpce)
'        Dim callback = Await rpce.WaitCallBask
'        Return rpce.Value
'    End Function
'End Module
'Public Class RPCParam
'    Public Key As String
'    Public Value As Object
'    Public Shared Narrowing Operator CType(value As Object()) As RPCParam
'        If value IsNot Nothing Then
'            If value.Length >= 2 Then
'                Dim p As New RPCParam
'                p.Key = value(0).ToString
'                p.Value = value(1)
'                Return p
'            End If
'        End If
'        Return New RPCParam With {.Key = "", .Value = Nothing}
'    End Operator
'End Class
Public MustInherit Class RPCEntry
    Inherits MarshalByRefObject
    Implements IRemoteProcessCall

    <System.ThreadStatic()> Public Source As String
    <System.ThreadStatic()> Public _SourceIdentity As Object
    Public Overridable Property Identity As Object Implements IRemoteProcessCall.Identity
    Public Overridable Property Name As String Implements IRemoteProcessCall.Name
    Public Property SourceIdentity As Object Implements IRemoteProcessCall.SourceIdentity
        Get
            Return _SourceIdentity
        End Get
        Set(value As Object)
            _SourceIdentity = value
        End Set
    End Property
    Private _Target As Object
    Public Property Target As Object Implements IRemoteProcessCall.Target
        Get
            Return _Target
        End Get
        Set(value As Object)
            _Target = value
        End Set
    End Property
    Public Function Local() As IRemoteProcessCall Implements IRemoteProcessCall.Local
        Return Me
    End Function
End Class
Public Module TaskExtension
    Public Function AsyncCall(Of T)(expression As System.Linq.Expressions.Expression(Of Func(Of T))) As System.Threading.Tasks.Task(Of T)
        Dim f As Func(Of T) = expression.Compile
        Dim task As New System.Threading.Tasks.Task(Of T)(f)
        task.Start()
        Return task
    End Function
End Module
Public Class Proxy(Of T As {IRemoteProcessCall, MarshalByRefObject})
    Inherits System.Runtime.Remoting.Proxies.RealProxy
    Implements IRemoteProcessCallProxy

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
                Dim rpce As New RPCEventArgs With {.Value = rpco, .Target = objref.Target}
                RaiseEvent InvokeRemote(Me, rpce)
                rpce.WaitCallBack()
                Dim rmsg As New ReturnMessage(msg, rpce.Value.ReturnValue)
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
End Class



Public Class ReturnMessage
    Implements IMethodReturnMessage
    Private _ReturnValue As Object
    Public Sub New(msg As IMethodCallMessage, Value As Object)
        _HasVarArgs = msg.HasVarArgs
        _LogicalCallContext = msg.LogicalCallContext
        _MethodBase = msg.MethodBase
        _MethodName = msg.MethodName
        _MethodSignature = msg.MethodSignature
        _TypeName = msg.TypeName
        _Uri = msg.Uri
        _ReturnValue = Value
    End Sub
    Private _Properties As New Dictionary(Of String, Object)
    Public ReadOnly Property Properties As System.Collections.IDictionary Implements System.Runtime.Remoting.Messaging.IMessage.Properties
        Get
            Return _Properties '
        End Get
    End Property

    Public ReadOnly Property ArgCount As Integer Implements System.Runtime.Remoting.Messaging.IMethodMessage.ArgCount
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Args As Object() Implements System.Runtime.Remoting.Messaging.IMethodMessage.Args
        Get
            Return New Object() {} '
        End Get
    End Property

    Public Function GetArg(argNum As Integer) As Object Implements System.Runtime.Remoting.Messaging.IMethodMessage.GetArg
        Return Nothing
    End Function

    Public Function GetArgName(index As Integer) As String Implements System.Runtime.Remoting.Messaging.IMethodMessage.GetArgName
        Return Nothing
    End Function
    Private _HasVarArgs As Boolean
    Public ReadOnly Property HasVarArgs As Boolean Implements System.Runtime.Remoting.Messaging.IMethodMessage.HasVarArgs
        Get
            Return _HasVarArgs
        End Get
    End Property
    Private _LogicalCallContext As System.Runtime.Remoting.Messaging.LogicalCallContext
    Public ReadOnly Property LogicalCallContext As System.Runtime.Remoting.Messaging.LogicalCallContext Implements System.Runtime.Remoting.Messaging.IMethodMessage.LogicalCallContext
        Get
            Return _LogicalCallContext '
        End Get
    End Property

    Private _MethodBase As System.Reflection.MethodBase
    Public ReadOnly Property MethodBase As System.Reflection.MethodBase Implements System.Runtime.Remoting.Messaging.IMethodMessage.MethodBase
        Get
            Return _MethodBase
        End Get
    End Property
    Private _MethodName As String
    Public ReadOnly Property MethodName As String Implements System.Runtime.Remoting.Messaging.IMethodMessage.MethodName
        Get
            Return _MethodName
        End Get
    End Property
    Private _MethodSignature As Object
    Public ReadOnly Property MethodSignature As Object Implements System.Runtime.Remoting.Messaging.IMethodMessage.MethodSignature
        Get
            Return _MethodSignature
        End Get
    End Property
    Private _TypeName As String
    Public ReadOnly Property TypeName As String Implements System.Runtime.Remoting.Messaging.IMethodMessage.TypeName
        Get
            Return _TypeName
        End Get
    End Property
    Private _Uri As String
    Public ReadOnly Property Uri As String Implements System.Runtime.Remoting.Messaging.IMethodMessage.Uri
        Get
            Return _Uri
        End Get
    End Property

    Public ReadOnly Property Exception As System.Exception Implements System.Runtime.Remoting.Messaging.IMethodReturnMessage.Exception
        Get
            Return Nothing '
        End Get
    End Property

    Public Function GetOutArg(argNum As Integer) As Object Implements System.Runtime.Remoting.Messaging.IMethodReturnMessage.GetOutArg
        Return Nothing
    End Function

    Public Function GetOutArgName(index As Integer) As String Implements System.Runtime.Remoting.Messaging.IMethodReturnMessage.GetOutArgName
        Return Nothing
    End Function

    Public ReadOnly Property OutArgCount As Integer Implements System.Runtime.Remoting.Messaging.IMethodReturnMessage.OutArgCount
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property OutArgs As Object() Implements System.Runtime.Remoting.Messaging.IMethodReturnMessage.OutArgs
        Get
            Return New Object() {}
        End Get
    End Property

    Public ReadOnly Property ReturnValue As Object Implements System.Runtime.Remoting.Messaging.IMethodReturnMessage.ReturnValue
        Get
            Return _ReturnValue
        End Get
    End Property
End Class
