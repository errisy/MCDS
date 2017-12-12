Public Class RPCProxy(Of T As MarshalByRefObject, TAgent As {New, RPCCallAgent}, TSerializer As {New, ISerializer})
    Inherits System.Runtime.Remoting.Proxies.RealProxy
    Implements IDisposable
    Private Agent As TAgent
    Private ProxyName As String
    Private InactiveMethods As New HashSet(Of System.Reflection.MethodBase)
    Private LocalMethods As New HashSet(Of System.Reflection.MethodBase)
    Private Local As T
    Public Property Identity As Object
    Private _CanTimeOut As Boolean = False
    Private _TimeOutCount As Integer = -1
    Private _DefaultReturnValue As Object = Nothing
    Private DataSerializer As New TSerializer
 
    Public Sub New(ServiceAgent As TAgent, Optional LocalSource As T = Nothing, Optional TimeOut As Integer = -1, Optional DefaultReturnValue As Object = Nothing, Optional vIdentity As IRPCIdentity = Nothing, Optional Name As String = Nothing)
        MyBase.New(GetType(T))
        Local = LocalSource
        Identity = vIdentity
        Dim vT As Type = GetType(T)
        If Name Is Nothing Then Name = vT.Name
        ProxyName = Name
        For Each mi In vT.GetMethods(Reflection.BindingFlags.Instance And (Reflection.BindingFlags.Public Or Reflection.BindingFlags.NonPublic))
            If mi.GetCustomAttributes(True).Where(Function(obj) TypeOf obj Is ReverseAttribute).Any() Then InactiveMethods.Add(mi)
            If mi.GetCustomAttributes(True).Where(Function(obj) TypeOf obj Is CentralAttribute).Any() Then InactiveMethods.Add(mi)
            If mi.GetCustomAttributes(True).Where(Function(obj) TypeOf obj Is LocalAttribute).Any() Then LocalMethods.Add(mi)
        Next

        _DefaultReturnValue = DefaultReturnValue

        If TimeOut >= 0 Then
            _CanTimeOut = True
            _TimeOutCount = TimeOut
        Else
            _CanTimeOut = False
        End If
        Dim tokenSource As New System.Threading.CancellationTokenSource
        Dim t As New System.Threading.Tasks.Task(Of Boolean)(Function(token As System.Threading.CancellationToken) As Boolean
                                                                 While Not TryReconnect()
                                                                     If token.IsCancellationRequested Then Return False
                                                                     System.Threading.Thread.Sleep(1000)
                                                                     If token.IsCancellationRequested Then Return False
                                                                 End While
                                                                 Return True
                                                             End Function, tokenSource.Token)
        t.Start()
        If _CanTimeOut Then
            t.Wait(_TimeOutCount)
            tokenSource.Cancel(True)
        Else
            t.Wait()
        End If
        Dim GetEventTask As New System.Threading.Tasks.Task(AddressOf GetEvents)
        GetEventTask.Start()
    End Sub
    Private Sub GetEvents()
        While Running
            Try
                Dim rpcos As ShallowList(Of RPCObject) = DataSerializer.Deserialize(Agent.GetEvents(ProxyName, Identity))
                If rpcos Is Nothing Then
                    Debug.WriteLine("Error: Get Nothing")
                Else
                    Debug.WriteLine(String.Format("Get {0} Calls", rpcos.Count))
                End If
                For Each rpco In rpcos
                    Try
                        rpco.MethodBase.Invoke(Local, rpco.Parameters.ToArray)
                    Catch ex As Exception
                        Dim tokenSource As New System.Threading.CancellationTokenSource
                        Dim t As New System.Threading.Tasks.Task(Of Boolean)(Function(token As System.Threading.CancellationToken) As Boolean
                                                                                 While Not TryReconnect()
                                                                                     If token.IsCancellationRequested Then Return False
                                                                                     System.Threading.Thread.Sleep(1000)
                                                                                     If token.IsCancellationRequested Then Return False
                                                                                 End While
                                                                                 Return True
                                                                             End Function, tokenSource.Token)
                        t.Start()
                        If _CanTimeOut Then
                            t.Wait(_TimeOutCount)
                            tokenSource.Cancel(True)
                        Else
                            t.Wait()
                        End If
                        If t.Result Then
                            rpco.MethodBase.Invoke(Local, rpco.Parameters.ToArray)
                        End If
                    End Try
                Next
            Catch ex As Exception
                Debug.WriteLine("Error: " + ex.ToString)
            End Try
        End While
    End Sub
    Public ReadOnly Property IsConnected As Boolean
        Get
            Try
                Agent.Ping()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Get
    End Property
    Public Function TryReconnect() As Boolean
        Return Agent.CanConnect
    End Function
    Public Overrides Function Invoke(msg As Runtime.Remoting.Messaging.IMessage) As Runtime.Remoting.Messaging.IMessage
        Dim imc As System.Runtime.Remoting.Messaging.IMethodCallMessage = msg
        Dim rmsg As ReturnMessage
        If InactiveMethods.Contains(imc.MethodBase) Then
            rmsg = New ReturnMessage(msg, Nothing)
            Return rmsg
        ElseIf LocalMethods.Contains(imc.MethodBase) Then
            rmsg = New ReturnMessage(msg, imc.MethodBase.Invoke(Local, imc.Args))
            Return rmsg
        Else
            Dim rpco As New RPCObject
            rpco.Identity = Identity
            rpco.Host = ProxyName
            rpco.Parameters.AddRange(imc.Args)
            rpco.MethodBase = imc.MethodBase
            Dim res As Byte()

            Try
                res = Agent.Process(ProxyName, DataSerializer.Serialize(rpco))
            Catch ex As Exception
                Dim tokenSource As New System.Threading.CancellationTokenSource
                Dim t As New System.Threading.Tasks.Task(Of Boolean)(Function(token As System.Threading.CancellationToken) As Boolean
                                                                         While Not TryReconnect()
                                                                             If token.IsCancellationRequested Then Return False
                                                                             System.Threading.Thread.Sleep(1000)
                                                                             If token.IsCancellationRequested Then Return False
                                                                         End While
                                                                         Return True
                                                                     End Function, tokenSource.Token)
                t.Start()
                If _CanTimeOut Then
                    'wait for the timeout
                    t.Wait(_TimeOutCount)
                    tokenSource.Cancel(True)
                Else
                    'wait for ever
                    t.Wait()
                End If
                If t.Result Then
                    res = Agent.Process(ProxyName, DataSerializer.Serialize(rpco))
                End If
            End Try
            If res Is Nothing Then
                rpco = _DefaultReturnValue
            Else
                rpco = DataSerializer.Deserialize(res)
            End If
            rmsg = New ReturnMessage(msg, rpco.ReturnValue)
            Return rmsg
        End If
    End Function
    Public ReadOnly Property LocalObject
        Get
            Return Local
        End Get
    End Property
    Private Running As Boolean = True
    Protected Overrides Sub Finalize()
        Running = False
        MyBase.Finalize()
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 检测冗余的调用

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)。
                Running = False
            End If
            ' TODO: 释放非托管资源(非托管对象)并重写下面的 Finalize()。
            ' TODO: 将大型字段设置为 null。
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: 仅当上面的 Dispose(ByVal disposing As Boolean)具有释放非托管资源的代码时重写 Finalize()。
    'Protected Overrides Sub Finalize()
    '    ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Visual Basic 添加此代码是为了正确实现可处置模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入上面的 Dispose (disposing As Boolean)中。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class

Public MustInherit Class RPCCallProcessor
    Inherits MarshalByRefObject
    Friend Shared ServerEntries As New Hashtable
    Friend Shared ServerProxies As New Hashtable
    Friend Shared ProxyList As New List(Of RemotingCallProcessor)
    Public Shared Function Register(Of T As MarshalByRefObject, TSerializer As {New, ISerializer})(Server As T, Optional Name As String = Nothing) As T
        Dim vT As Type = GetType(T)
        If Name Is Nothing Then Name = vT.Name
        If ServerEntries.ContainsKey(Name) Then Return DirectCast(ServerProxies(ServerEntries(Name)), ServerProxy).GetTransparentProxy
        ServerEntries.Add(Name, Server)
        Dim eProxy As New ServerProxy(Of T)(Server)
        ServerProxies.Add(Server, eProxy)
        Dim po As New RemotingCallProcessor(Of TSerializer)
        ProxyList.Add(po)
        WellKnownExtension.Register(po, Runtime.Remoting.WellKnownObjectMode.SingleCall, Name)
        Return eProxy.GetTransparentProxy
    End Function
    Public MustOverride Function CanConnect() As Boolean
    Public MustOverride Function Ping() As Boolean
    Public MustOverride Function Process(Name As String, value As Byte()) As Byte()
    Public MustOverride Function GetEvents(Name As String, Identity As IRPCIdentity) As Byte()
End Class

Public Class RPCCallProcessor(Of TSerializer As {New, ISerializer})
    Inherits RPCCallProcessor
    Private DataSerializer As New TSerializer
    Public Overrides Function CanConnect() As Boolean

    End Function
    Public Overrides Function GetEvents(Name As String, Identity As IRPCIdentity) As Byte()
        Dim rpcos As ShallowList(Of RPCObject) = DirectCast(RemotingCallProcessor.ServerProxies(RemotingCallProcessor.ServerEntries(Name)), ServerProxy).GetEvents(Identity)
        Return DataSerializer.Serialize(rpcos)
    End Function
    Public Overrides Function Ping() As Boolean

    End Function
    Public Overrides Function Process(Name As String, value() As Byte) As Byte()
        Dim rpco As RPCObject = DataSerializer.Deserialize(value)
        If TypeOf RPCCallProcessor.ServerEntries(Name) Is IRPCIndentityInvokable Then
            Using DirectCast(RPCCallProcessor.ServerEntries(Name), IRPCIndentityInvokable).LockIdentity(rpco.Identity)
                rpco.ReturnValue = rpco.MethodBase.Invoke(RPCCallProcessor.ServerEntries(Name), rpco.Parameters.ToArray)
            End Using
        Else
            rpco.ReturnValue = rpco.MethodBase.Invoke(RPCCallProcessor.ServerEntries(Name), rpco.Parameters.ToArray)
        End If
        Return DataSerializer.Serialize(rpco)
    End Function
End Class

Public MustInherit Class RPCCallAgent
    Public Sub New(ServiceAddress As String, Optional ServiceName As String = "", Optional ServicePort As Integer = 80)
        _Address = ServiceAddress
        _Name = ServiceName
        _Port = ServicePort
    End Sub
    Public Property Address As String
    Public Property Name As String
    Public Property Port As Integer
    Public MustOverride Function CanConnect() As Boolean
    Public MustOverride Function Ping() As Boolean
    Public MustOverride Function Process(Name As String, value As Byte()) As Byte()
    Public MustOverride Function GetEvents(Name As String, Identity As IRPCIdentity) As Byte()
End Class