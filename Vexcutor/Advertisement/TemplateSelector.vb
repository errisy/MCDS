Imports System.Windows, System.Windows.Controls, System.Windows.Data
<System.Windows.Markup.ContentProperty("Templates")>
Public Class TemplateSelector
    Inherits DataTemplateSelector
    Private _Templates As New System.Collections.ObjectModel.ObservableCollection(Of TypeTemplate)
    Public ReadOnly Property Templates As System.Collections.ObjectModel.ObservableCollection(Of TypeTemplate)
        Get
            Return _Templates
        End Get
    End Property
    Public Overrides Function SelectTemplate(item As Object, container As DependencyObject) As DataTemplate
        Dim t As Type
        If TypeOf item Is SelectableDynamicObject Then
            t = DirectCast(item, SelectableDynamicObject).GetInnerType
        Else
            t = item.GetType
        End If
        For Each tt In _Templates
            If tt.DataType.IsAssignableFrom(t) Then Return tt.DataTemplate
        Next
        Return MyBase.SelectTemplate(item, container)
    End Function
End Class

<System.Windows.Markup.ContentProperty("DataTemplate")>
Public Class TypeTemplate
    Public Property DataType As Type
    Public Property DataTemplate As DataTemplate
End Class

Public Class SelectableDynamicObject
    Inherits System.Dynamic.DynamicObject
    Public Sub New(innerObject As Object)
        _WrappedObject = innerObject
    End Sub
    Protected _WrappedObject As Object
    Public Property WrappedObject As Object
        Get
            Return _WrappedObject
        End Get
        Set(value As Object)
            _WrappedObject = value
        End Set
    End Property
    Public Overrides Function TryGetMember(binder As Dynamic.GetMemberBinder, ByRef result As Object) As Boolean
        If binder.Name = "_IsDynamicSelected" Then
            result = __IsDynamicSelected
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
        If binder.Name = "_IsDynamicSelected" Then
            If TypeOf value Is Boolean Then
                _IsDynamicSelected = value
                Return True
            ElseIf value IsNot Nothing Then
                _IsDynamicSelected = True
                Return True
            Else
                _IsDynamicSelected = False
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
    Protected __IsDynamicSelected As Boolean = False
    Public Property _IsDynamicSelected As Boolean
        Get
            Return __IsDynamicSelected
        End Get
        Set(value As Boolean)
            __IsDynamicSelected = value
            RaiseEvent _IsDynamicSelectedChanged(Me, New EventArgs)
        End Set
    End Property
    Public Function GetInnerType() As Type
        Return _WrappedObject.GetType
    End Function
    Public Event _IsDynamicSelectedChanged As EventHandler
    Public Shared Function CreateTypedSelectableDynamicObject(obj As Object) As Object
        Dim gType As Type = GetType(SelectableDynamicObject(Of ))
        Dim tPara = New Type() {obj.GetType}
        Dim vType = gType.MakeGenericType(tPara)
        Return vType.GetConstructor(tPara).Invoke(New Object() {obj})
    End Function
End Class

Public Class SelectableConverterExtension
    Inherits System.Windows.Markup.MarkupExtension
    Public Sub New()
    End Sub
    Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
        Return New SelectableEnumerableConverter
    End Function
End Class

<ValueConversion(GetType(IEnumerable(Of )), GetType(System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject))), System.Windows.Markup.ContentProperty("ItemTemplate")>
Public Class SelectableEnumerableConverter
    Implements IValueConverter
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim vType As Type = value.GetType
        If TypeOf value Is IEnumerable Then
            Dim result As New System.Collections.ObjectModel.ObservableCollection(Of SelectableDynamicObject)
            For Each obj In value
                result.Add(SelectableDynamicObject.CreateTypedSelectableDynamicObject(obj))
            Next
            Return result
        Else
            Return New SelectableDynamicObject(value)
        End If
    End Function
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack

    End Function
End Class

Public Class SelectableDynamicObject(Of T)
    Inherits SelectableDynamicObject
    Public Sub New(innerObject As T)
        MyBase.New(innerObject)
    End Sub
    Public Shared Narrowing Operator CType(obj As SelectableDynamicObject(Of T)) As T
        Return obj._WrappedObject
    End Operator
    Public Shared Widening Operator CType(obj As T) As SelectableDynamicObject(Of T)
        Return New SelectableDynamicObject(Of T)(obj)
    End Operator
End Class