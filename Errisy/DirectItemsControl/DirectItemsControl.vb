Public Class DirectItemsControl
    Inherits Control
    Implements IMouseInputProvider


#Region "RoutedEvents"
    'DirectItemsControl -> AddingViewForItem As ItemViewEventHandler
    Public Custom Event AddingViewForItem As ItemViewEventHandler
        AddHandler(ByVal value As ItemViewEventHandler)
            Me.AddHandler(AddingViewForItemEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As ItemViewEventHandler)
            Me.RemoveHandler(AddingViewForItemEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As Object, ByVal e As ItemViewEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event
    Public Shared ReadOnly AddingViewForItemEvent As RoutedEvent = _
                  EventManager.RegisterRoutedEvent("AddingViewForItem", _
                  RoutingStrategy.Bubble, _
                  GetType(ItemViewEventHandler), GetType(DirectItemsControl))

#End Region
#Region "Dependency Properties"


    'DirectItemsControl->ItemTemplate As DataTemplate Default: Nothing
    Public Property ItemTemplate As DataTemplate
        Get
            Return GetValue(ItemTemplateProperty)
        End Get
        Set(ByVal value As DataTemplate)
            SetValue(ItemTemplateProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ItemTemplateProperty As DependencyProperty = _
                           DependencyProperty.Register("ItemTemplate", _
                           GetType(DataTemplate), GetType(DirectItemsControl), _
                           New FrameworkPropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedItemTemplateChanged)))
    Private Shared Sub SharedItemTemplateChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, DirectItemsControl).ItemTemplateChanged(d, e)
    End Sub
    Private Sub ItemTemplateChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        BuildViews()
    End Sub

    'DirectItemsControl->ItemTemplateSelector As DirectDataTemplateSelector Default: Nothing
    Public Property ItemTemplateSelector As DirectDataTemplateSelector
        Get
            Return GetValue(ItemTemplateSelectorProperty)
        End Get
        Set(ByVal value As DirectDataTemplateSelector)
            SetValue(ItemTemplateSelectorProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ItemTemplateSelectorProperty As DependencyProperty = _
                           DependencyProperty.Register("ItemTemplateSelector", _
                           GetType(DirectDataTemplateSelector), GetType(DirectItemsControl), _
                           New FrameworkPropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedItemTemplateSelectorChanged)))
    Private Shared Sub SharedItemTemplateSelectorChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, DirectItemsControl).ItemTemplateSelectorChanged(d, e)
    End Sub
    Private Sub ItemTemplateSelectorChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        BuildViews()
    End Sub

    'DirectItemsControl->ItemsPanel As DataTemplate Default: Nothing
    Public Property ItemsPanel As DataTemplate
        Get
            Return GetValue(ItemsPanelProperty)
        End Get
        Set(ByVal value As DataTemplate)
            SetValue(ItemsPanelProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ItemsPanelProperty As DependencyProperty = _
                           DependencyProperty.Register("ItemsPanel", _
                           GetType(DataTemplate), GetType(DirectItemsControl), _
                           New FrameworkPropertyMetadata(New DataTemplate(GetType(StackPanel)), New PropertyChangedCallback(AddressOf SharedItemsPanelChanged)))
    Private Shared Sub SharedItemsPanelChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, DirectItemsControl).ItemsPanelChanged(d, e)
    End Sub
    Private Sub ItemsPanelChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        BuildPanel()
    End Sub

    'DirectItemsControl->ItemsSource As IEnumerable Default: Nothing
    Public Property ItemsSource As IEnumerable
        Get
            Return GetValue(ItemsSourceProperty)
        End Get
        Set(ByVal value As IEnumerable)
            SetValue(ItemsSourceProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ItemsSourceProperty As DependencyProperty = _
                           DependencyProperty.Register("ItemsSource", _
                           GetType(IEnumerable), GetType(DirectItemsControl), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedItemsSourceChanged)))
    Private Shared Sub SharedItemsSourceChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, DirectItemsControl).ItemsSourceChanged(d, e)
    End Sub
    Private Sub ItemsSourceChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If TypeOf e.NewValue Is System.Collections.Specialized.INotifyCollectionChanged Then
            observableItemsSource = e.NewValue
        Else
            observableItemsSource = Nothing
        End If
        BuildViews()
    End Sub

    'DirectItemsControl -> Panel As Panel Default: Nothing
    Public ReadOnly Property Panel As Panel
        Get
            Return GetValue(DirectItemsControl.PanelProperty)
        End Get
    End Property
    Private Shared ReadOnly PanelPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Panel", _
                              GetType(Panel), GetType(DirectItemsControl), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly PanelProperty As DependencyProperty = _
                             PanelPropertyKey.DependencyProperty

#End Region

#Region "Functions"
    ''' <summary>
    ''' 加载的ControlTemplate
    ''' </summary>
    ''' <remarks></remarks>
    Private loadedTemplate As FrameworkElement
    ''' <summary>
    ''' 用于加载Panel
    ''' </summary>
    ''' <remarks></remarks>
    Private loadedItemsPresenter As DirectItemsPresenter
    Private WithEvents observableItemsSource As System.Collections.Specialized.INotifyCollectionChanged
    Protected Overrides Sub OnTemplateChanged(oldTemplate As ControlTemplate, newTemplate As ControlTemplate)
        MyBase.OnTemplateChanged(oldTemplate, newTemplate)
    End Sub
    Public Overrides Sub OnApplyTemplate()
        loadedItemsPresenter = Me.Child(Of DirectItemsPresenter)()
        BuildPanel()
        MyBase.OnApplyTemplate()
    End Sub

    Private _ItemGenerator As ItemGenerator
    Private Sub BuildPanel()
        If loadedItemsPresenter Is Nothing Then
            _ItemGenerator = Nothing
            SetValue(PanelPropertyKey, Nothing)
            Return
        End If

        Dim loadedPanel As Panel
        If ItemsPanel Is Nothing Then
            loadedPanel = New StackPanel
        Else
            loadedPanel = ItemsPanel.LoadContent
        End If
        loadedItemsPresenter.Content = loadedPanel
        SetValue(PanelPropertyKey, loadedPanel)
        _ItemGenerator = New ItemGenerator(loadedPanel) With {.AddingViewForItem = AddressOf OnAddingViewForItem}
        BuildViews()
    End Sub
    Private Sub OnAddingViewForItem(item As Object, view As FrameworkElement)
        Dim ivea As New ItemViewEventArgs(item, view)
        ivea.RoutedEvent = AddingViewForItemEvent
        ivea.Source = Me
        Me.RaiseEvent(ivea)
    End Sub

    Private Shared DefaultTemplate As DataTemplate = Markup.XamlReader.Parse(
         <DataTemplate
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:e="clr-namespace:Errisy;assembly=Errisy">
             <ContentPresenter/>
         </DataTemplate>.ToString)
    Private Sub BuildViews()
        If _ItemGenerator IsNot Nothing AndAlso TypeOf ItemsSource Is IEnumerable Then
            _ItemGenerator.Clear()
            Dim loadedItemTemplate As DataTemplate
            Dim loadedItemTemplateSelector As DirectDataTemplateSelector = ItemTemplateSelector
            Dim view As FrameworkElement
            For Each item In ItemsSource
                If loadedItemTemplateSelector IsNot Nothing Then
                    loadedItemTemplate = loadedItemTemplateSelector.SelectTemplate(item)
                Else
                    loadedItemTemplate = ItemTemplate
                End If
                If loadedItemTemplate Is Nothing Then
                    loadedItemTemplate = DefaultTemplate
                End If
                view = loadedItemTemplate.LoadContent
                view.DataContext = item
                _ItemGenerator.Add(item, view)
            Next
        End If
    End Sub

    Public Function GetItemGenerator() As ItemGenerator
        Return _ItemGenerator
    End Function
#End Region
    Private Sub observableItemsSource_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs) Handles observableItemsSource.CollectionChanged
        Dim loadedItemTemplate As DataTemplate
        Dim loadedItemTemplateSelector As DirectDataTemplateSelector = ItemTemplateSelector
        Dim view As FrameworkElement
        Select Case e.Action
            Case Specialized.NotifyCollectionChangedAction.Add
                If TypeOf e.NewItems Is IEnumerable Then
                    For Each item In e.NewItems
                        If loadedItemTemplateSelector IsNot Nothing Then
                            loadedItemTemplate = loadedItemTemplateSelector.SelectTemplate(item)
                        Else
                            loadedItemTemplate = ItemTemplate
                        End If
                        If loadedItemTemplate Is Nothing Then loadedItemTemplate = New DataTemplate(GetType(ContentPresenter))
                        view = loadedItemTemplate.LoadContent
                        'Dim viewStyle As Style
                        'If view.Style Is Nothing Then
                        '    viewStyle = New Style With {.TargetType = view.GetType}
                        'Else
                        '    viewStyle = view.Style
                        'End If
                        'For Each t In loadedItemTemplate.Triggers
                        '    viewStyle.Triggers.Add(t)
                        'Next
                        'view.Style = viewStyle
                        view.DataContext = item
                        _ItemGenerator.Add(item, view)
                    Next
                End If
            Case Specialized.NotifyCollectionChangedAction.Remove
                If TypeOf e.OldItems Is IEnumerable Then
                    For Each item In e.OldItems
                        _ItemGenerator.Remove(item)
                    Next
                End If
            Case Specialized.NotifyCollectionChangedAction.Move, Specialized.NotifyCollectionChangedAction.Replace
                If TypeOf e.OldItems Is IEnumerable Then
                    For Each item In e.OldItems
                        _ItemGenerator.Remove(item)
                    Next
                End If
                If TypeOf e.NewItems Is IEnumerable Then
                    For Each item In e.NewItems
                        If loadedItemTemplateSelector IsNot Nothing Then
                            loadedItemTemplate = loadedItemTemplateSelector.SelectTemplate(item)
                        Else
                            loadedItemTemplate = ItemTemplate
                        End If
                        If loadedItemTemplate Is Nothing Then loadedItemTemplate = New DataTemplate(GetType(ContentPresenter))
                        view = loadedItemTemplate.LoadContent
                        'Dim viewStyle As Style
                        'If view.Style Is Nothing Then
                        '    viewStyle = New Style With {.TargetType = view.GetType}
                        'Else
                        '    viewStyle = view.Style
                        'End If
                        'For Each t In loadedItemTemplate.Triggers
                        '    viewStyle.Triggers.Add(t)
                        'Next
                        view.DataContext = item
                        _ItemGenerator.Add(item, view)
                    Next
                End If
            Case Specialized.NotifyCollectionChangedAction.Reset
                BuildViews()
        End Select
    End Sub
    Protected Overrides Sub OnPreviewMouseWheel(e As MouseWheelEventArgs)
        PerformMouseWheel(e)
        MyBase.OnPreviewMouseWheel(e)
    End Sub
    Protected Overrides Sub OnPreviewMouseDown(e As MouseButtonEventArgs)
        PerformMouseDown(e)
        If e.ClickCount > 1 Then PerformMouseDoubleDown(e)
        MyBase.OnPreviewMouseDown(e)
    End Sub
    Protected Overrides Sub OnPreviewMouseMove(e As MouseEventArgs)
        PerformMouseMove(e)
        MyBase.OnPreviewMouseMove(e)
    End Sub
    Protected Overrides Sub OnPreviewMouseUp(e As MouseButtonEventArgs)
        PerformMouseUp(e)
        MyBase.OnPreviewMouseUp(e)
    End Sub
    Protected Sub PerformMouseWheel(e As MouseWheelEventArgs)
        Dim mwe As New MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
        mwe.RoutedEvent = ProvideMouseWheelEvent
        mwe.Source = Me
        [RaiseEvent](mwe)
    End Sub
    Protected Sub PerformMouseDown(e As MouseButtonEventArgs)
        Dim mbe As New MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton, e.StylusDevice)
        mbe.RoutedEvent = ProvideMouseDownEvent
        mbe.Source = Me
        [RaiseEvent](mbe)
    End Sub
    Protected Sub PerformMouseMove(e As MouseEventArgs)
        Dim mwe As New MouseEventArgs(e.MouseDevice, e.Timestamp, e.StylusDevice)
        mwe.RoutedEvent = ProvideMouseMoveEvent
        mwe.Source = Me
        [RaiseEvent](mwe)
    End Sub
    Protected Sub PerformMouseUp(e As MouseButtonEventArgs)
        Dim mbe As New MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton, e.StylusDevice)
        mbe.RoutedEvent = ProvideMouseUpEvent
        mbe.Source = Me
        [RaiseEvent](mbe)
    End Sub
    Protected Sub PerformMouseDoubleDown(e As MouseButtonEventArgs)
        Dim mbe As New MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton, e.StylusDevice)
        mbe.RoutedEvent = ProvideMouseDoubleDownEvent
        mbe.Source = Me
        [RaiseEvent](mbe)
    End Sub
    'DirectItemsControl -> MouseWheelZoom As MouseEventHandler
    Public Custom Event ProvideMouseWheel As MouseWheelEventHandler Implements IMouseInputProvider.MouseWheel
        AddHandler(ByVal value As MouseWheelEventHandler)
            Me.AddHandler(ProvideMouseWheelEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As MouseWheelEventHandler)
            Me.RemoveHandler(ProvideMouseWheelEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event
    Public Shared ReadOnly ProvideMouseWheelEvent As RoutedEvent = _
                  EventManager.RegisterRoutedEvent("ProvideMouseWheel", _
                  RoutingStrategy.Direct, _
                  GetType(MouseWheelEventHandler), GetType(DirectItemsControl))
    'DirectItemsControl -> ProvideMouseDown As MouseButtonEventHandler
    Public Custom Event ProvideMouseDown As MouseButtonEventHandler Implements IMouseInputProvider.MouseDown
        AddHandler(ByVal value As MouseButtonEventHandler)
            Me.AddHandler(ProvideMouseDownEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As MouseButtonEventHandler)
            Me.RemoveHandler(ProvideMouseDownEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event
    Public Shared ReadOnly ProvideMouseDownEvent As RoutedEvent = _
                  EventManager.RegisterRoutedEvent("ProvideMouseDown", _
                  RoutingStrategy.Direct, _
                  GetType(MouseButtonEventHandler), GetType(DirectItemsControl))
    'DirectItemsControl -> ProvideMouseMove As MouseEventHandler
    Public Custom Event ProvideMouseMove As MouseEventHandler Implements IMouseInputProvider.MouseMove
        AddHandler(ByVal value As MouseEventHandler)
            Me.AddHandler(ProvideMouseMoveEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As MouseEventHandler)
            Me.RemoveHandler(ProvideMouseMoveEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event
    Public Shared ReadOnly ProvideMouseMoveEvent As RoutedEvent = _
                  EventManager.RegisterRoutedEvent("ProvideMouseMove", _
                  RoutingStrategy.Direct, _
                  GetType(MouseEventHandler), GetType(DirectItemsControl))
    'DirectItemsControl -> ProvideMouseUp As MouseButtonEventHandler
    Public Custom Event ProvideMouseUp As MouseButtonEventHandler Implements IMouseInputProvider.MouseUp
        AddHandler(ByVal value As MouseButtonEventHandler)
            Me.AddHandler(ProvideMouseUpEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As MouseButtonEventHandler)
            Me.RemoveHandler(ProvideMouseUpEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event
    Public Shared ReadOnly ProvideMouseUpEvent As RoutedEvent = _
                  EventManager.RegisterRoutedEvent("ProvideMouseUp", _
                  RoutingStrategy.Direct, _
                  GetType(MouseButtonEventHandler), GetType(DirectItemsControl))
    'DirectItemsControl -> ProvideMouseDoubleDown As MouseButtonEventHandler
    Public Custom Event ProvideMouseDoubleDown As MouseButtonEventHandler Implements IMouseInputProvider.MouseDoubleDown
        AddHandler(ByVal value As MouseButtonEventHandler)
            Me.AddHandler(ProvideMouseDoubleDownEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As MouseButtonEventHandler)
            Me.RemoveHandler(ProvideMouseDoubleDownEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event
    Public Shared ReadOnly ProvideMouseDoubleDownEvent As RoutedEvent = _
                  EventManager.RegisterRoutedEvent("ProvideMouseDoubleDown", _
                  RoutingStrategy.Bubble, _
                  GetType(MouseButtonEventHandler), GetType(DirectItemsControl))

    Shared Sub New()
        Control.BackgroundProperty.OverrideMetadata(GetType(DirectItemsControl), New FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender))
    End Sub
 
End Class
