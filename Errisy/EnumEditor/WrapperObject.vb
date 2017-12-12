Public MustInherit Class DynamicWrapper
    Inherits System.Dynamic.DynamicObject
    Implements System.ComponentModel.INotifyPropertyChanged
    Protected _WrappedObject As Object
    Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
    Protected Overridable Sub OnPropertyChanged(e As ComponentModel.PropertyChangedEventArgs)
        RaiseEvent PropertyChanged(Me, e)
    End Sub
    Public Function GetInnerType() As Type
        Return _WrappedObject.GetType
    End Function
End Class



Public Class SelectionDynamicWrapper
    Inherits DynamicWrapper
    Private WithEvents _INotifyPropertyChanged As System.ComponentModel.INotifyPropertyChanged
    Public Sub New(innerObject As Object)
        _WrappedObject = innerObject
        If TypeOf _WrappedObject Is System.ComponentModel.INotifyPropertyChanged Then
            _INotifyPropertyChanged = _WrappedObject
        End If
    End Sub

    Public Property WrappedObject As Object
        Get
            Return _WrappedObject
        End Get
        Set(value As Object)
            _WrappedObject = value
        End Set
    End Property
    Public Overrides Function TryGetMember(binder As Dynamic.GetMemberBinder, ByRef result As Object) As Boolean
        If binder.Name = "_IsSelected" Then
            result = _IsSelected
            Return True

        Else
            If _WrappedObject Is Nothing Then
                result = Nothing
                Return False
            Else
                Try
                    result = CallByName(_WrappedObject, binder.Name, CallType.Get)
                    Return True
                Catch ex As Exception
                    result = Nothing
                    Return False
                End Try
            End If
        End If
        Return True
    End Function
    Public Overrides Function TrySetMember(binder As Dynamic.SetMemberBinder, value As Object) As Boolean
        If binder.Name = "_IsSelected" Then
            If TypeOf value Is String Then
                _IsSelected = value
                Return True
            ElseIf value IsNot Nothing Then
                _IsSelected = value.ToString
                Return True
            Else
                _IsSelected = ""
                Return True
            End If
        Else
            If _WrappedObject Is Nothing Then
                Return False
            Else
                Try
                    CallByName(_WrappedObject, binder.Name, CallType.Set, New Object() {value})
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            End If
        End If
        Return True
    End Function
    Protected __IsSelected As Boolean = False
    Public Property _IsSelected As Boolean
        Get
            Return __IsSelected
        End Get
        Set(value As Boolean)
            __IsSelected = value
            RaiseEvent _DynamicHeaderChanged(Me, New EventArgs)
            OnPropertyChanged(New ComponentModel.PropertyChangedEventArgs("_IsSelected"))
        End Set
    End Property

    Public Event _DynamicHeaderChanged As EventHandler
    Public Shared Function CreateTypedSelectableDynamicObject(obj As Object) As Object
        Dim gType As Type = GetType(SelectionDynamicWrapper(Of ))
        Dim tPara = New Type() {obj.GetType}
        Dim vType = gType.MakeGenericType(tPara)
        Return vType.GetConstructor(tPara).Invoke(New Object() {obj})
    End Function

    Private Sub WrappedPropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles _INotifyPropertyChanged.PropertyChanged
        OnPropertyChanged(e)
    End Sub
End Class

Public Class SelectionDynamicWrapper(Of T)
    Inherits SelectionDynamicWrapper
    Public Sub New(innerObject As T)
        MyBase.New(innerObject)
    End Sub
    Public Shared Narrowing Operator CType(obj As SelectionDynamicWrapper(Of T)) As T
        Return obj._WrappedObject
    End Operator
    Public Shared Widening Operator CType(obj As T) As SelectionDynamicWrapper(Of T)
        Return New SelectionDynamicWrapper(Of T)(obj)
    End Operator
End Class