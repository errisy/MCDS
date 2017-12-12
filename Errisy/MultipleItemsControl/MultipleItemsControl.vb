<Markup.ContentProperty("ItemsSourceProviders")>
Public Class MultipleItemsControl
    Inherits Control
    Implements IMouseInputProvider
    Public Sub New()
        SetValue(ItemsSourceProvidersPropertyKey, _ItemsSourceProviders)
        SetValue(ItemsSourcesProperty, _ItemSources)
    End Sub

#Region "RoutedEvents"

#End Region
#Region "Dependency Properties"


    'MultipleItemsControl->ItemTemplate As DataTemplate Default: Nothing
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
                           GetType(DataTemplate), GetType(MultipleItemsControl), _
                           New FrameworkPropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedItemTemplateChanged)))
    Private Shared Sub SharedItemTemplateChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, MultipleItemsControl).ItemTemplateChanged(d, e)
    End Sub
    Private Sub ItemTemplateChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Obliterate()
        BuildAllViews()
    End Sub

    'MultipleItemsControl->ItemTemplateSelector As DirectDataTemplateSelector Default: Nothing
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
                           GetType(DirectDataTemplateSelector), GetType(MultipleItemsControl), _
                           New FrameworkPropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedItemTemplateSelectorChanged)))
    Private Shared Sub SharedItemTemplateSelectorChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, MultipleItemsControl).ItemTemplateSelectorChanged(d, e)
    End Sub
    Private Sub ItemTemplateSelectorChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Obliterate()
        BuildAllViews()
    End Sub

    'MultipleItemsControl->ItemsPanel As DataTemplate Default: Nothing
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
                           GetType(DataTemplate), GetType(MultipleItemsControl), _
                           New FrameworkPropertyMetadata(New DataTemplate(GetType(StackPanel)), New PropertyChangedCallback(AddressOf SharedItemsPanelChanged)))
    Private Shared Sub SharedItemsPanelChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, MultipleItemsControl).ItemsPanelChanged(d, e)
    End Sub
    Private Sub ItemsPanelChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        BuildPanel()
    End Sub


    'MultipleItemsControl -> Panel As Panel Default: Nothing
    Public ReadOnly Property Panel As Panel
        Get
            Return GetValue(MultipleItemsControl.PanelProperty)
        End Get
    End Property
    Private Shared ReadOnly PanelPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Panel", _
                              GetType(Panel), GetType(MultipleItemsControl), _
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
    'Private WithEvents observableItemsSource As System.Collections.Specialized.INotifyCollectionChanged
    Protected Overrides Sub OnTemplateChanged(oldTemplate As ControlTemplate, newTemplate As ControlTemplate)
        MyBase.OnTemplateChanged(oldTemplate, newTemplate)
    End Sub
    Public Overrides Sub OnApplyTemplate()
        loadedItemsPresenter = Me.Child(Of DirectItemsPresenter)()
        BuildPanel()
        MyBase.OnApplyTemplate()
    End Sub

    Private ItemGenerators As New Dictionary(Of IEnumerable, ItemGenerator)
    Private Sub BuildPanel()
        If loadedItemsPresenter Is Nothing Then
            ItemGenerators.Clear()
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
        '_ItemGenerator = New ItemGenerator(loadedPanel)
        BuildAllViews()
    End Sub
    'Private Sub MultipleItemsControl_DataContextChanged(sender As Object, e As DependencyPropertyChangedEventArgs) Handles Me.DataContextChanged
    '    BuildAllViews()
    'End Sub
    Private Sub BuildAllViews()
        If TypeOf ItemsSources Is System.Collections.ObjectModel.ObservableCollection(Of IEnumerable) Then
            For Each Items In ItemsSources
                Present(Items)
            Next
        End If
    End Sub
    Private Sub BuildViews(Items As IEnumerable)
        Dim _ItemGenerator As ItemGenerator = ItemGenerators(Items)
        'Panel.Children.Clear()
        If _ItemGenerator IsNot Nothing AndAlso TypeOf Items Is IEnumerable Then
            _ItemGenerator.Clear()
            Dim loadedItemTemplate As DataTemplate
            Dim loadedItemTemplateSelector As DirectDataTemplateSelector = ItemTemplateSelector
            Dim view As FrameworkElement
            For Each item In Items
                If loadedItemTemplateSelector IsNot Nothing Then
                    loadedItemTemplate = loadedItemTemplateSelector.SelectTemplate(item)
                Else
                    loadedItemTemplate = ItemTemplate
                End If
                If loadedItemTemplate Is Nothing Then
                    loadedItemTemplate = New DataTemplate(GetType(ContentPresenter))
                    loadedItemTemplate.Seal()
                End If
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
    End Sub
#End Region
    Private Sub ObservableItemsSource_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs)
        Dim _ItemGenerator As ItemGenerator = ItemGenerators(sender)
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
                        If loadedItemTemplate Is Nothing Then
                            loadedItemTemplate = New DataTemplate(GetType(ContentPresenter))
                            loadedItemTemplate.Seal()
                        End If
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
                        If loadedItemTemplate Is Nothing Then
                            loadedItemTemplate = New DataTemplate(GetType(ContentPresenter))
                            loadedItemTemplate.Seal()
                        End If
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
                BuildViews(sender)
        End Select
    End Sub
#Region "ItemsSourceProviders"
    Private WithEvents _ItemsSourceProviders As New System.Collections.ObjectModel.ObservableCollection(Of ItemsSourceProvider)
    'MultipleItemsControl -> MultipleItemsSource As MultipleItemsSource Default: Nothing
    Public ReadOnly Property ItemsSourceProviders As System.Collections.ObjectModel.ObservableCollection(Of ItemsSourceProvider)
        Get
            Return GetValue(MultipleItemsControl.ItemsSourceProvidersProperty)
        End Get
    End Property
    Private Shared ReadOnly ItemsSourceProvidersPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("ItemsSourceProviders", _
                              GetType(System.Collections.ObjectModel.ObservableCollection(Of ItemsSourceProvider)), GetType(MultipleItemsControl), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly ItemsSourceProvidersProperty As DependencyProperty = _
                             ItemsSourceProvidersPropertyKey.DependencyProperty

    Private Sub _ItemsSourceProviders_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs) Handles _ItemsSourceProviders.CollectionChanged
        If System.ComponentModel.DesignerProperties.GetIsInDesignMode(Me) Then Return
        Select Case e.Action
            Case Specialized.NotifyCollectionChangedAction.Add
                If TypeOf e.NewItems Is IEnumerable Then
                    For Each item As ItemsSourceProvider In e.NewItems
                        item.SetBinding(FrameworkElement.DataContextProperty, New Binding With {.Source = Me, .Path = New PropertyPath(FrameworkElement.DataContextProperty)})
                        AddLogicalChild(item)
                        If item.ItemsSource IsNot Nothing Then _ItemsSources.Add(item.ItemsSource)
                    Next
                End If
            Case Specialized.NotifyCollectionChangedAction.Remove
                If TypeOf e.OldItems Is IEnumerable Then
                    For Each item As ItemsSourceProvider In e.OldItems
                        If item.ItemsSource IsNot Nothing Then _ItemsSources.Remove(item.ItemsSource)
                        RemoveLogicalChild(item)
                        BindingOperations.ClearBinding(item, FrameworkElement.DataContextProperty)
                        _ItemsSources.Remove(item.ItemsSource)
                    Next
                End If
            Case Specialized.NotifyCollectionChangedAction.Move

            Case Specialized.NotifyCollectionChangedAction.Replace
                If TypeOf e.OldItems Is IEnumerable Then
                    For Each item As ItemsSourceProvider In e.OldItems
                        If item.ItemsSource IsNot Nothing Then _ItemsSources.Remove(item.ItemsSource)
                        RemoveLogicalChild(item)
                        BindingOperations.ClearBinding(item, FrameworkElement.DataContextProperty)
                        _ItemsSources.Remove(item.ItemsSource)
                    Next
                End If
                If TypeOf e.NewItems Is IEnumerable Then
                    For Each item As ItemsSourceProvider In e.NewItems
                        item.SetBinding(FrameworkElement.DataContextProperty, New Binding With {.Source = Me, .Path = New PropertyPath(FrameworkElement.DataContextProperty)})
                        AddLogicalChild(item)
                        If item.ItemsSource IsNot Nothing Then _ItemsSources.Add(item.ItemsSource)
                    Next
                End If
            Case Specialized.NotifyCollectionChangedAction.Reset
                _ItemsSources.Clear()
                LogicalChildren.Reset()
                While LogicalChildren.MoveNext
                    RemoveLogicalChild(LogicalChildren.Current)
                End While
                For Each item As ItemsSourceProvider In _ItemsSourceProviders
                    item.SetBinding(FrameworkElement.DataContextProperty, New Binding With {.Source = Me, .Path = New PropertyPath(FrameworkElement.DataContextProperty)})
                    AddLogicalChild(item)
                    If item.ItemsSource IsNot Nothing Then _ItemsSources.Add(item.ItemsSource)
                Next
        End Select
    End Sub
#End Region

#Region "MultipleItemsSource"
    Private _ItemSources As New System.Collections.ObjectModel.ObservableCollection(Of IEnumerable)
    'MultipleItemsControl->ItemsSources As System.Collections.ObjectModel.ObservableCollection(Of IEnumerable) with Event Default: Nothing
    Public Property ItemsSources As System.Collections.ObjectModel.ObservableCollection(Of IEnumerable)
        Get
            Return GetValue(ItemsSourcesProperty)
        End Get
        Set(ByVal value As System.Collections.ObjectModel.ObservableCollection(Of IEnumerable))
            SetValue(ItemsSourcesProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ItemsSourcesProperty As DependencyProperty = _
                           DependencyProperty.Register("ItemsSources", _
                           GetType(System.Collections.ObjectModel.ObservableCollection(Of IEnumerable)), GetType(MultipleItemsControl), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedItemsSourcesChanged)))
    Private Shared Sub SharedItemsSourcesChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, MultipleItemsControl).ItemsSourcesChanged(d, e)
    End Sub
    Private Sub ItemsSourcesChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If TypeOf e.OldValue Is System.Collections.ObjectModel.ObservableCollection(Of IEnumerable) Then
            For Each Items In e.OldValue
                Efface(Items)
            Next
        End If
        _ItemsSources = e.NewValue
        Obliterate()
        BuildAllViews()
    End Sub
    Private WithEvents _ItemsSources As System.Collections.ObjectModel.ObservableCollection(Of IEnumerable)
    Private Sub _ItemsSources_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs) Handles _ItemsSources.CollectionChanged
        Select Case e.Action
            Case Specialized.NotifyCollectionChangedAction.Add
                If TypeOf e.NewItems Is IEnumerable Then
                    For Each item In e.NewItems
                        Present(item)
                    Next
                End If
            Case Specialized.NotifyCollectionChangedAction.Remove
                If TypeOf e.OldItems Is IEnumerable Then
                    For Each item In e.OldItems
                        Efface(item)
                    Next
                End If
            Case Specialized.NotifyCollectionChangedAction.Move

            Case Specialized.NotifyCollectionChangedAction.Replace
                If TypeOf e.OldItems Is IEnumerable Then
                    For Each item In e.OldItems
                        Efface(item)
                    Next
                End If
                If TypeOf e.NewItems Is IEnumerable Then
                    For Each item In e.NewItems
                        Present(item)
                    Next
                End If
            Case Specialized.NotifyCollectionChangedAction.Reset
                Obliterate()
                BuildAllViews()
        End Select
    End Sub
    Public Sub Obliterate()
        If Panel Is Nothing Then Return
        Panel.Children.Clear()
        ItemGenerators.Clear()
    End Sub
    Public Sub Efface(Items As IEnumerable)
        If TypeOf Items Is System.Collections.Specialized.INotifyCollectionChanged Then
            Dim NotifyCollectionChanged As System.Collections.Specialized.INotifyCollectionChanged = Items
            RemoveHandler NotifyCollectionChanged.CollectionChanged, AddressOf observableItemsSource_CollectionChanged
            ItemGenerators(Items).Clear()
            ItemGenerators.Remove(Items)
        End If
    End Sub
    Public Sub Present(Items As IEnumerable)
        If Panel IsNot Nothing Then
            ItemGenerators.Add(Items, New ItemGenerator(Panel))
            If TypeOf Items Is System.Collections.Specialized.INotifyCollectionChanged Then
                Dim NotifyCollectionChanged As System.Collections.Specialized.INotifyCollectionChanged = Items
                AddHandler NotifyCollectionChanged.CollectionChanged, AddressOf observableItemsSource_CollectionChanged
            End If
            BuildViews(Items)
        End If
    End Sub
    'Object->DataContext As Object with Event Default: Nothing
    Public Function GetItemGenerator(Source As IEnumerable) As ItemGenerator
        If Not ItemGenerators.ContainsKey(Source) Then Return Nothing
        Return ItemGenerators(Source)
    End Function
#End Region

#Region "IMouseInputProvider"

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
    'MultipleItemsControl -> MouseWheelZoom As MouseEventHandler
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
                  GetType(MouseWheelEventHandler), GetType(MultipleItemsControl))
    'MultipleItemsControl -> ProvideMouseDown As MouseButtonEventHandler
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
                  GetType(MouseButtonEventHandler), GetType(MultipleItemsControl))
    'MultipleItemsControl -> ProvideMouseMove As MouseEventHandler
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
                  GetType(MouseEventHandler), GetType(MultipleItemsControl))
    'MultipleItemsControl -> ProvideMouseUp As MouseButtonEventHandler
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
                  GetType(MouseButtonEventHandler), GetType(MultipleItemsControl))
    'MultipleItemsControl -> ProvideMouseDoubleDown As MouseButtonEventHandler
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
                  GetType(MouseButtonEventHandler), GetType(MultipleItemsControl))

#End Region
    Shared Sub New()
        Control.BackgroundProperty.OverrideMetadata(GetType(MultipleItemsControl), New FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender))
    End Sub



End Class
