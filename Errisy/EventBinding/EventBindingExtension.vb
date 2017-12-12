Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Windows
Imports System.Windows.Markup

Public Class EventBindingExtension
    Inherits MarkupExtension
    Private _eventInfo As EventInfo

    Public Sub New()
    End Sub

    Public Sub New(eventHandlerName As String)
        Me.EventHandlerName = eventHandlerName
    End Sub

    <ConstructorArgument("eventHandlerName")> _
    Public Property EventHandlerName() As String
        Get
            Return m_EventHandlerName
        End Get
        Set(value As String)
            m_EventHandlerName = Value
        End Set
    End Property
    Private m_EventHandlerName As String

    Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
        If String.IsNullOrEmpty(EventHandlerName) Then
            Throw New ArgumentException("The EventHandlerName property is not set", "EventHandlerName")
        End If

        Dim target = DirectCast(serviceProvider.GetService(GetType(IProvideValueTarget)), IProvideValueTarget)

        Dim targetObj = TryCast(target.TargetObject, DependencyObject)
        If targetObj Is Nothing Then
            Throw New InvalidOperationException("The target object must be a DependencyObject")
        End If

        _eventInfo = TryCast(target.TargetProperty, EventInfo)

        If _eventInfo Is Nothing Then
            Throw New InvalidOperationException("The target property must be an event")
        End If

        Dim dataContext As Object = GetDataContext(targetObj)
        If dataContext Is Nothing Then
            SubscribeToDataContextChanged(targetObj)
            Return GetDummyHandler(_eventInfo.EventHandlerType)
        End If

        Dim handler = GetHandler(dataContext, _eventInfo, EventHandlerName)
        If handler Is Nothing Then
            Trace.TraceError("EventBinding: no suitable method named '{0}' found in type '{1}' to handle event '{2'}", EventHandlerName, dataContext.[GetType](), _eventInfo)
            Return GetDummyHandler(_eventInfo.EventHandlerType)
        End If

        Return handler

    End Function

#Region "Helper methods"

    Private Shared Function GetHandler(dataContext As Object, eventInfo As EventInfo, eventHandlerName As String) As [Delegate]
        Dim dcType As Type = dataContext.[GetType]()

        Dim method = dcType.GetMethod(eventHandlerName, GetParameterTypes(eventInfo.EventHandlerType))
        If method IsNot Nothing Then
            If method.IsStatic Then
                Return [Delegate].CreateDelegate(eventInfo.EventHandlerType, method)
            Else
                Return [Delegate].CreateDelegate(eventInfo.EventHandlerType, dataContext, method)
            End If
        End If

        Return Nothing
    End Function

    Private Shared Function GetParameterTypes(delegateType As Type) As Type()
        Dim invokeMethod = delegateType.GetMethod("Invoke")
        Return invokeMethod.GetParameters().[Select](Function(p) p.ParameterType).ToArray()
    End Function

    Private Shared Function GetDataContext(target As DependencyObject) As Object
        Return If(target.GetValue(FrameworkElement.DataContextProperty), target.GetValue(FrameworkContentElement.DataContextProperty))
    End Function

    Shared ReadOnly _dummyHandlers As New Dictionary(Of Type, [Delegate])()

    Private Shared Function GetDummyHandler(eventHandlerType As Type) As [Delegate]
        Dim handler As [Delegate]
        If Not _dummyHandlers.TryGetValue(eventHandlerType, handler) Then
            handler = CreateDummyHandler(eventHandlerType)
            _dummyHandlers(eventHandlerType) = handler
        End If
        Return handler
    End Function

    Private Shared Function CreateDummyHandler(eventHandlerType As Type) As [Delegate]
        Dim parameterTypes = GetParameterTypes(eventHandlerType)
        Dim returnType = eventHandlerType.GetMethod("Invoke").ReturnType
        Dim dm = New DynamicMethod("DummyHandler", returnType, parameterTypes)
        Dim il = dm.GetILGenerator()
        If returnType IsNot GetType(System.Void) Then
            If returnType.IsValueType Then
                Dim local = il.DeclareLocal(returnType)
                il.Emit(OpCodes.Ldloca_S, local)
                il.Emit(OpCodes.Initobj, returnType)
                il.Emit(OpCodes.Ldloc_0)
            Else
                il.Emit(OpCodes.Ldnull)
            End If
        End If
        il.Emit(OpCodes.Ret)
        Return dm.CreateDelegate(eventHandlerType)
    End Function

    Private Sub SubscribeToDataContextChanged(targetObj As DependencyObject)
        DependencyPropertyDescriptor.FromProperty(FrameworkElement.DataContextProperty, targetObj.[GetType]()).AddValueChanged(targetObj, AddressOf TargetObject_DataContextChanged)
    End Sub

    Private Sub UnsubscribeFromDataContextChanged(targetObj As DependencyObject)
        DependencyPropertyDescriptor.FromProperty(FrameworkElement.DataContextProperty, targetObj.[GetType]()).RemoveValueChanged(targetObj, AddressOf TargetObject_DataContextChanged)
    End Sub

    Private Sub TargetObject_DataContextChanged(sender As Object, e As EventArgs)
        Dim targetObj As DependencyObject = TryCast(sender, DependencyObject)
        If targetObj Is Nothing Then
            Return
        End If

        Dim dataContext As Object = GetDataContext(targetObj)
        If dataContext Is Nothing Then
            Return
        End If

        Dim handler = GetHandler(dataContext, _eventInfo, EventHandlerName)
        If handler IsNot Nothing Then
            _eventInfo.AddEventHandler(targetObj, handler)
        End If
        UnsubscribeFromDataContextChanged(targetObj)
    End Sub

#End Region
End Class
