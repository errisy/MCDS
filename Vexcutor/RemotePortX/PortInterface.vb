Public Interface IPort
    Sub Send(obj As Object)
    Sub SendTo(obj As Object, ParamArray Targets As String())
    Sub SendExcept(obj As Object, ParamArray excepts As String())
    Event ObjectReceived As EventHandler(Of PackageReceivedEventArgs)
    Property IsRunning As Boolean
    ReadOnly Property Connections As IList(Of IConnection)
End Interface
Public Interface IConnection
    Sub Send(obj As Object)
    Event ObjectReceived As EventHandler(Of PackageReceivedEventArgs)
    Property IsConnected As Boolean
    ReadOnly Property URI As String
    Property Identification As Object
End Interface
Public Interface IRemotingConnection
    Inherits IConnection
    Property UpdateInterval As Integer
    Property ResponseLatency As Integer
End Interface
Public Enum PackageFlag As Byte
    Normal = 0
    Idenfication = 1
    Distributive = 2
    CallBack = 3
    RemoteProcessCall = 4
    RemoteProcessCallBack = 5
    Test = 254
    Disconnect = 255
End Enum
''' <summary>
''' 基本数据包 由长度+MD5验证构成
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class Package
    <NonSerialized()> Private Shared MD5 As System.Security.Cryptography.MD5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create
    <NonSerialized()> Private Shared ASCII As System.Text.ASCIIEncoding = System.Text.Encoding.ASCII
    Public Sub New()
    End Sub
    Public Sub New(pFlag As PackageFlag)
        _Flag = pFlag
        _Data = ShallowSerializer.Serialize(Nothing)
        _MD5 = MD5.ComputeHash(_Data)
        _Length = _Data.Length
    End Sub
    Public Sub New(data As Object)
        _Data = ShallowSerializer.Serialize(data)
        _MD5 = MD5.ComputeHash(_Data)
        _Length = _Data.Length
    End Sub
    Public Sub New(data As Object, ParamArray Wrappers As ISerializationWrapper())
        _Data = ShallowSerializer.Serialize(data, Wrappers)
        _MD5 = MD5.ComputeHash(_Data)
        _Length = _Data.Length
    End Sub
    Public Sub New(data As Object, pFlag As PackageFlag)
        _Flag = pFlag
        _Data = ShallowSerializer.Serialize(data)
        _MD5 = MD5.ComputeHash(_Data)
        _Length = _Data.Length
    End Sub
    Public Sub New(data As Object, pFlag As PackageFlag, ParamArray Wrappers As ISerializationWrapper())
        _Flag = pFlag
        _Data = ShallowSerializer.Serialize(data, Wrappers)
        _MD5 = MD5.ComputeHash(_Data)
        _Length = _Data.Length
    End Sub
    Public ReadOnly Property Flag As PackageFlag
        Get
            Return _Flag
        End Get
    End Property
    Public ReadOnly Property Length As Integer
        Get
            Return _Length
        End Get
    End Property
    Private _Length As Int32
    Private _Flag As Byte
    Private _Data As Byte()
    Private _MD5 As Byte()
    ''' <summary>
    ''' 获取数据代表的对象
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>因为有些对象对线程敏感 所以要在调用的线程当中反序列化 因为Package不保留对象的引用 而是重新从字节数据获得对象</remarks>
    Public Function Child(ParamArray Wrappers As ISerializationWrapper()) As Object
        Return ShallowSerializer.Deserialize(_Data, Wrappers)
    End Function
    Public Function Child() As Object
        Return ShallowSerializer.Deserialize(_Data)
    End Function
    Public Function Child(Of T)(ParamArray Wrappers As ISerializationWrapper()) As T
        Return ShallowSerializer.Deserialize(_Data, Wrappers)
    End Function
    Public Function Child(Of T)() As T
        Return ShallowSerializer.Deserialize(_Data)
    End Function
    Public Function ToBytes() As Byte()
        Dim bList As New List(Of Byte)
        bList.Add(_Flag)
        bList.AddRange(BitConverter.GetBytes(_Length))
        bList.AddRange(_Data)
        bList.AddRange(_MD5)
        Return bList.ToArray
    End Function
    Public Shared Function FromBytes(pFlag As Byte, bytes As Byte(), hash As Byte()) As Package
        If ASCII.GetString(MD5.ComputeHash(bytes)) = ASCII.GetString(hash) Then
            Return New Package With {._Flag = pFlag, ._Data = bytes, ._MD5 = hash, ._Length = bytes.Length}
        Else
            Return Nothing
        End If
    End Function
    Public Shared Function ParseBytes(bytes As Byte()) As Package
        Dim Flag As New List(Of Byte)
        Dim Header As New List(Of Byte)
        Dim Data As New List(Of Byte)
        Dim Hash As New List(Of Byte)
        Dim length As Integer = bytes.Length
        Dim offset As Integer = 0
        If Length > 0 AndAlso Flag.Count < 1 Then
            Dim t As Integer = 1
            Flag.AddRange(bytes.Skip(offset).Take(t))
            offset += t
            Length -= t
        End If
        If Length > 0 AndAlso Flag.Count = 1 AndAlso Header.Count < 4 Then
            If length < 4 - Hash.Count Then
                Header.AddRange(bytes.Skip(offset).Take(length))
                offset += length
                length -= length
            Else
                Dim t As Integer = 4 - Hash.Count
                Header.AddRange(bytes.Skip(offset).Take(t))
                offset += t
                length -= t
            End If
        End If
        If Length > 0 AndAlso Flag.Count = 1 AndAlso Header.Count = 4 AndAlso Data.Count < BitConverter.ToInt32(Header.ToArray, 0) Then
            Dim r As Integer = BitConverter.ToInt32(Header.ToArray, 0)
            If Length < r - Data.Count Then
                Data.AddRange(bytes.Skip(offset).Take(Length))
                offset += Length
                Length -= Length
            Else
                Dim t As Integer = r - Data.Count
                Data.AddRange(bytes.Skip(offset).Take(t))
                offset += t
                Length -= t
            End If
        End If
        If Length > 0 AndAlso Flag.Count = 1 AndAlso Header.Count = 4 AndAlso Data.Count = BitConverter.ToInt32(Header.ToArray, 0) AndAlso Hash.Count < 16 Then
            If Length < 16 - Hash.Count Then
                Hash.AddRange(bytes.Skip(offset).Take(Length))
                offset += Length
                Length -= Length
            Else
                Dim t As Integer = 16 - Hash.Count
                Hash.AddRange(bytes.Skip(offset).Take(t))
                offset += t
                Length -= t
            End If
        End If
        If Header.Count = 4 AndAlso Flag.Count = 1 AndAlso Data.Count = BitConverter.ToInt32(Header.ToArray, 0) AndAlso Hash.Count = 16 Then
            If ASCII.GetString(MD5.ComputeHash(Data.ToArray)) = ASCII.GetString(Hash.ToArray) Then
                Return New Package With {._Data = Data.ToArray, ._Flag = Flag(0), ._Length = BitConverter.ToInt32(Header.ToArray, 0), ._MD5 = Hash.ToArray}
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function
End Class
'<Serializable()>
'Public Class Disconnection
'End Class
<Serializable()>
Public Class TestInfo
    Public Value As String
End Class
Public Class PackageReceivedEventArgs
    Inherits EventArgs
    Private _obj As Package
    Public Sub New(pack As Package)
        _obj = pack
    End Sub
    Public ReadOnly Property Value As Package
        Get
            Return _obj
        End Get
    End Property
    Public Property Source As String
    Public Property Handled As Boolean = False
End Class