Imports System.Collections.Specialized
''' <summary>
''' 默认情况下用ClosableTabItem作为包装单位的TabControl
''' </summary>
''' <remarks></remarks>
Public Class ClosableTabControl
    Inherits TabControl
    'ClosableTabControl->ItemContainerTemplate As DataTemplate Default: Nothing
    Public Property ItemContainerTemplate As DataTemplate
        Get
            Return GetValue(ItemContainerTemplateProperty)
        End Get
        Set(ByVal value As DataTemplate)
            SetValue(ItemContainerTemplateProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ItemContainerTemplateProperty As DependencyProperty =
                           DependencyProperty.Register("ItemContainerTemplate",
                           GetType(DataTemplate), GetType(ClosableTabControl),
                           New PropertyMetadata(Nothing))
    Protected Overrides Function GetContainerForItemOverride() As DependencyObject
        If ItemContainerTemplate IsNot Nothing Then
            Return ItemContainerTemplate.LoadContent
        Else
            Return New ClosableTabItem
        End If
    End Function
    'Public Custom Event TabClose As RoutedEventHandler
    '    AddHandler(ByVal value As RoutedEventHandler)
    '        Me.AddHandler(TabCloseEvent, value)
    '    End AddHandler
    '    RemoveHandler(ByVal value As RoutedEventHandler)
    '        Me.RemoveHandler(TabCloseEvent, value)
    '    End RemoveHandler
    '    RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
    '        Me.RaiseEvent(e)
    '    End RaiseEvent
    'End Event
    'Public Shared ReadOnly TabCloseEvent As RoutedEvent = _
    '                  EventManager.RegisterRoutedEvent("TabClose", _
    '                  RoutingStrategy.Bubble, _
    '                  GetType(RoutedEventHandler), GetType(ClosableTabControl))

    ''' <summary>
    ''' This enables the TabControl to automatically select and present the newly added tabitem.
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnItemsChanged(e As NotifyCollectionChangedEventArgs)
        SelectedIndex = e.NewStartingIndex
        MyBase.OnItemsChanged(e)
    End Sub
End Class
