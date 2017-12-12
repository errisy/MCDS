Public Interface IRPCIdentity
    Inherits IComparable(Of IRPCIdentity)
    Property Name As String
    Property Password As String
End Interface
Friend Module RPCIdentity
    <ThreadStatic> Friend Identity As Object
    <ThreadStatic> Friend Mutex As System.Threading.Mutex
End Module

''' <summary>
''' 这个接口为远程调用一个类型提供了Identity支持，通过Identity类型可以判断远程用户的用户名密码等信息。
''' 实现这个接口的类型可以选择不调用它 那么就可能不验证身份而进行简单访问
''' </summary>
''' <remarks></remarks>
Public Interface IRPCIndentityInvokable
    ''' <summary>
    ''' 启用这个功能的类型需要在这个函数当中返回一个IDisposable的Mutex防止其他线程干扰当前线程运行
    ''' </summary>
    ''' <param name="Instance"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function LockIdentity(Instance As IRPCIdentity) As MutexDisposer
End Interface

''' <summary>
''' 这是一个简单的例子，解释如何实现IRPCIndentityInvokable。
''' </summary>
''' <remarks></remarks>
Public Class RPCIndentityInvokableExample
    Inherits MarshalByRefObject
    Implements IRPCIndentityInvokable
    Public Function LockIdentity(Instance As IRPCIdentity) As MutexDisposer Implements IRPCIndentityInvokable.LockIdentity
        If RPCIdentity.Mutex Is Nothing Then RPCIdentity.Mutex = New System.Threading.Mutex
        Dim mDisposer As New MutexDisposer(RPCIdentity.Mutex)
        RPCIdentity.Identity = Instance
        Return mDisposer
    End Function

End Class

''' <summary>
''' 在调用LockIdentity方法时返回这个类型，用于解除对Mutex的锁定
''' Using x = RPCIndentityInvokable.LockIndentity
'''     '调用RPCIndentityInvokable的函数
''' End Using
''' </summary>
''' <remarks></remarks>
Public Class MutexDisposer
    Implements IDisposable
    Private _LockedMutex As System.Threading.Mutex
    Public Sub New(LockedMutex As System.Threading.Mutex)
        _LockedMutex = LockedMutex
        LockedMutex.WaitOne()
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 检测冗余的调用

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)。
            End If

            ' TODO: 释放非托管资源(非托管对象)并重写下面的 Finalize()。
            ' TODO: 将大型字段设置为 null。
            _LockedMutex.ReleaseMutex()
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