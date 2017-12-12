Imports System.ComponentModel
Imports System.Dynamic

Public Class DynamicItem
    Inherits System.Dynamic.DynamicObject
    Implements System.ComponentModel.INotifyPropertyChanged
    Public Overrides Function GetDynamicMemberNames() As IEnumerable(Of String)
        Return _MemberDict.Keys
    End Function
    Public Overrides Function TryGetMember(binder As GetMemberBinder, ByRef result As Object) As Boolean
        If _MemberDict.ContainsKey(binder.Name) Then
            result = _MemberDict(binder.Name)
            Return True
        End If
        Return False
    End Function
    Private _MemberDict As New Dictionary(Of String, Object)
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Property EncodedMember(key As String) As Object
        Get
            Dim EncodedKey = PathStringEncode(key)
            If _MemberDict.ContainsKey(EncodedKey) Then Return _MemberDict(EncodedKey)
            Return Nothing
        End Get
        Set(value As Object)
            Dim EncodedKey = PathStringEncode(key)
            If _MemberDict.ContainsKey(EncodedKey) Then
                _MemberDict(EncodedKey) = value
            Else
                _MemberDict.Add(EncodedKey, value)
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(EncodedKey))
        End Set
    End Property
    Public Property Member(key As String) As Object
        Get
            If _MemberDict.ContainsKey(key) Then Return _MemberDict(key)
            Return Nothing
        End Get
        Set(value As Object)
            If _MemberDict.ContainsKey(key) Then
                _MemberDict(key) = value
            Else
                _MemberDict.Add(key, value)
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(key))
        End Set
    End Property
    Public Shared Function PathStringEncode(value As String) As String
        Dim utf8 = System.Text.Encoding.UTF8
        Dim stb As New System.Text.StringBuilder
        For Each b In utf8.GetBytes(value)
            stb.Append(Chr(65 + b Mod 16))
            stb.Append(Chr(65 + b \ 16))
        Next
        Return stb.ToString
    End Function
End Class