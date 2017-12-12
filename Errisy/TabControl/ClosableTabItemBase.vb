Public MustInherit Class ClosableTabItemBase
    Inherits TabItem
    Protected Overridable Sub RaiseTabClose()
        Dim e As New RoutedEventArgs(TabCloseEvent, Me)
        OnTabClose(e)
        Me.RaiseEvent(e)
        If Not e.Handled Then
            If TypeOf Parent Is TabControl Then
                If AskBeforeClose AndAlso MsgBox("Do you want to close this tab?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    DirectCast(Parent, TabControl).Items.Remove(Me)
                Else
                    DirectCast(Parent, TabControl).Items.Remove(Me)
                End If
            End If
        End If
        If CloseCommand IsNot Nothing AndAlso CloseCommand.CanExecute(DataContext) Then
            CloseCommand.Execute(DataContext)
        End If
    End Sub
    Protected Overridable Sub OnTabClose(e As RoutedEventArgs)
    End Sub
    Public Custom Event TabClose As RoutedEventHandler
        AddHandler(ByVal value As RoutedEventHandler)
            Me.AddHandler(TabCloseEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As RoutedEventHandler)
            Me.RemoveHandler(TabCloseEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event
    Public Shared ReadOnly TabCloseEvent As RoutedEvent = _
                      EventManager.RegisterRoutedEvent("TabClose", _
                      RoutingStrategy.Bubble, _
                      GetType(RoutedEventHandler), GetType(ClosableTabItemBase))
    'ClosableTabItemBase->AskBeforeClose As Boolean Default: False
    Public Property AskBeforeClose As Boolean
        Get
            Return GetValue(AskBeforeCloseProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(AskBeforeCloseProperty, value)
        End Set
    End Property
    Public Shared ReadOnly AskBeforeCloseProperty As DependencyProperty = _
                           DependencyProperty.Register("AskBeforeClose", _
                           GetType(Boolean), GetType(ClosableTabItemBase), _
                           New PropertyMetadata(False))
    'ClosableTabItemBase->IsDefaultClose As Boolean Default: False
    Public Property IsDefaultClose As Boolean
        Get
            Return GetValue(IsDefaultCloseProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsDefaultCloseProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsDefaultCloseProperty As DependencyProperty =
                           DependencyProperty.Register("IsDefaultClose",
                           GetType(Boolean), GetType(ClosableTabItemBase),
                           New PropertyMetadata(False))
    Public Property CloseCommand As ICommand
        Get
            Return GetValue(CloseCommandProperty)
        End Get
        Set(ByVal value As ICommand)
            SetValue(CloseCommandProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CloseCommandProperty As DependencyProperty =
                             DependencyProperty.Register("CloseCommand",
                             GetType(ICommand), GetType(ClosableTabItemBase),
                             New PropertyMetadata(Nothing))
End Class