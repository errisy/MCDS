Public Class TickBoxBase
    Inherits Button
    Public Property IsChecked As Boolean
        Get
            Return GetValue(IsCheckedProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsCheckedProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsCheckedProperty As DependencyProperty =
                             DependencyProperty.Register("IsChecked",
                             GetType(Boolean), GetType(TickBoxBase),
                             New FrameworkPropertyMetadata(False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault))
    Protected Overrides Sub OnClick()
        SetValue(IsCheckedProperty, Not IsChecked)
        Dim IsCheckedBinding = GetBindingExpression(IsCheckedProperty)
        IsCheckedBinding.UpdateSource()
        MyBase.RaiseEvent(New RoutedEventArgs(TickChangedEvent, Me))
    End Sub
    Public ReadOnly Property TickWidth As GridLength
        Get
            Return GetValue(TickBoxBase.TickWidthProperty)
        End Get
    End Property
    Private Shared ReadOnly TickWidthPropertyKey As DependencyPropertyKey =
                                  DependencyProperty.RegisterReadOnly("TickWidth",
                                  GetType(GridLength), GetType(TickBoxBase),
                                  New PropertyMetadata(New GridLength(20.0#, GridUnitType.Pixel)))
    Public Shared ReadOnly TickWidthProperty As DependencyProperty =
                                 TickWidthPropertyKey.DependencyProperty
    Protected Overrides Sub OnRenderSizeChanged(sizeInfo As SizeChangedInfo)
        Dim aw = ActualHeight
        If Double.IsNaN(aw) Then aw = 0#
        SetValue(TickWidthPropertyKey, New GridLength(aw, GridUnitType.Pixel))
        MyBase.OnRenderSizeChanged(sizeInfo)
    End Sub

    Public Custom Event TickChanged As RoutedEventHandler
        AddHandler(ByVal value As RoutedEventHandler)
            Me.AddHandler(TickChangedEvent, value)
        End AddHandler

        RemoveHandler(ByVal value As RoutedEventHandler)
            Me.RemoveHandler(TickChangedEvent, value)
        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event

    Public Shared ReadOnly TickChangedEvent As RoutedEvent =
                      EventManager.RegisterRoutedEvent("TickChanged",
                      RoutingStrategy.Bubble,
                      GetType(RoutedEventHandler), GetType(TickBoxBase))

End Class
