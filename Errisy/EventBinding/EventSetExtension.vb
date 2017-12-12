Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Windows
Imports System.Windows.Markup
Public Class EventSetExtension
    Inherits MarkupExtension
 
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

        Dim handler As Object

        Dim ProvideValueTarget = DirectCast(serviceProvider.GetService(GetType(IProvideValueTarget)), IProvideValueTarget)
        Dim RootObjectProvider = DirectCast(serviceProvider.GetService(GetType(Xaml.IRootObjectProvider)), Xaml.IRootObjectProvider)
        Dim Root As FrameworkElement = RootObjectProvider.RootObject

        Dim RootType As Type = Root.GetType
        
        Dim TargetObj = TryCast(ProvideValueTarget.TargetObject, DependencyObject)

        If TargetObj Is Nothing Then
            Throw New InvalidOperationException("The target object must be a DependencyObject")
        End If

        If TypeOf ProvideValueTarget.TargetProperty Is MethodInfo Then
            Dim TargetMethod As MethodInfo = ProvideValueTarget.TargetProperty
            Dim paras = TargetMethod.GetParameters()
            If paras(0).ParameterType = GetType(DependencyObject) Then
                Select Case paras(1).ParameterType
                    Case GetType(KeyEventHandler)
                        Dim _handler = RootType.GetMethod(EventHandlerName, BindingFlags.Instance Or BindingFlags.NonPublic, Nothing,
                                                          New Type() {GetType(Object), GetType(KeyEventArgs)},
                                                          Nothing)
                        If _handler IsNot Nothing Then
                            handler = [Delegate].CreateDelegate(GetType(KeyEventHandler), Root, _handler)
                            'TargetMethod.Invoke(Nothing, New Object() {Root, handler})
                        End If
                    Case GetType(MouseEventHandler)
                        Dim _handler = RootType.GetMethod(EventHandlerName, BindingFlags.Instance Or BindingFlags.NonPublic, Nothing,
              New Type() {GetType(Object), GetType(MouseEventArgs)},
              Nothing)
                        If _handler IsNot Nothing Then
                            handler = [Delegate].CreateDelegate(GetType(MouseEventHandler), Root, _handler)
                            'TargetMethod.Invoke(Nothing, New Object() {Root, handler})
                        End If
                    Case GetType(MouseButtonEventHandler)
                        Dim _handler = RootType.GetMethod(EventHandlerName, BindingFlags.Instance Or BindingFlags.NonPublic, Nothing,
              New Type() {GetType(Object), GetType(MouseButtonEventArgs)},
              Nothing)
                        If _handler IsNot Nothing Then
                            handler = [Delegate].CreateDelegate(GetType(MouseButtonEventHandler), Root, _handler)
                            'TargetMethod.Invoke(Nothing, New Object() {Root, handler})
                        End If
                    Case GetType(MouseWheelEventArgs)
                        Dim _handler = RootType.GetMethod(EventHandlerName, BindingFlags.Instance Or BindingFlags.NonPublic, Nothing,
              New Type() {GetType(Object), GetType(MouseWheelEventArgs)},
              Nothing)
                        If _handler IsNot Nothing Then
                            handler = [Delegate].CreateDelegate(GetType(MouseWheelEventHandler), Root, _handler)
                            'TargetMethod.Invoke(Nothing, New Object() {Root, handler})
                        End If
                End Select

            End If
        ElseIf TypeOf ProvideValueTarget.TargetProperty Is EventInfo Then
            Dim TargetEvent As EventInfo = ProvideValueTarget.TargetProperty
            Dim TargetEventHandler = TargetEvent.EventHandlerType

            Dim AddMethod As MethodInfo = TargetEvent.GetAddMethod
            Dim paras = AddMethod.GetParameters
            Select Case paras(0).ParameterType
                Case GetType(MouseEventHandler)
                    Dim _handler = RootType.GetMethod(EventHandlerName, BindingFlags.Instance Or BindingFlags.NonPublic, Nothing,
                                  New Type() {GetType(Object), GetType(MouseEventArgs)},
                                  Nothing)
                    If _handler IsNot Nothing Then
                        handler = [Delegate].CreateDelegate(GetType(MouseEventHandler), Root, _handler)
                        'AddMethod.Invoke(TargetObj, New Object() {handler})
                    End If
                Case GetType(MouseButtonEventHandler)
                    Dim _handler = RootType.GetMethod(EventHandlerName, BindingFlags.Instance Or BindingFlags.NonPublic, Nothing,
              New Type() {GetType(Object), GetType(MouseButtonEventArgs)},
              Nothing)
                    If _handler IsNot Nothing Then
                        handler = [Delegate].CreateDelegate(GetType(MouseButtonEventHandler), Root, _handler)
                        'AddMethod.Invoke(TargetObj, New Object() {handler})
                    End If
                Case GetType(MouseWheelEventArgs)
                    Dim _handler = RootType.GetMethod(EventHandlerName, BindingFlags.Instance Or BindingFlags.NonPublic, Nothing,
              New Type() {GetType(Object), GetType(MouseWheelEventArgs)},
              Nothing)
                    If _handler IsNot Nothing Then
                        handler = [Delegate].CreateDelegate(GetType(MouseWheelEventHandler), Root, _handler)
                        'AddMethod.Invoke(TargetObj, New Object() {handler})
                    End If
            End Select
        End If
 

        Return handler

    End Function
  
End Class
