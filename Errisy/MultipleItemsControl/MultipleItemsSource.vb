<System.Windows.Markup.ContentProperty("ItemsSourceProviders")>
Public Class MultipleItemsSource
    Inherits FrameworkElement
    Implements IList(Of ItemsSourceProvider), System.Collections.Specialized.INotifyCollectionChanged
    Public Sub New()
        SetValue(ItemsSourceProvidersPropertyKey, _ItemsSourceProviders)
        SetValue(ItemsSourcesPropertyKey, _ItemsSources)
    End Sub

#Region "Dependency Properties"


    Private WithEvents _ItemsSourceProviders As New System.Collections.ObjectModel.ObservableCollection(Of ItemsSourceProvider)
    'MultipleItemsSource -> ItemsSourceProviders As System.Collections.ObjectModel.ObservableCollection(Of ItemsSourceProvider) Default: Nothing
    Public ReadOnly Property ItemsSourceProviders As System.Collections.ObjectModel.ObservableCollection(Of ItemsSourceProvider)
        Get
            Return GetValue(MultipleItemsSource.ItemsSourceProvidersProperty)
        End Get
    End Property
    Private Shared ReadOnly ItemsSourceProvidersPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("ItemsSourceProviders", _
                              GetType(System.Collections.ObjectModel.ObservableCollection(Of ItemsSourceProvider)), GetType(MultipleItemsSource), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly ItemsSourceProvidersProperty As DependencyProperty = _
                             ItemsSourceProvidersPropertyKey.DependencyProperty
    Private _ItemsSources As New System.Collections.ObjectModel.ObservableCollection(Of IEnumerable)
    'MultipleItemsSource -> ItemsSources As System.Collections.ObjectModel.ObservableCollection(Of IEnumerable) Default: Nothing
    Public ReadOnly Property ItemsSources As System.Collections.ObjectModel.ObservableCollection(Of IEnumerable)
        Get
            Return GetValue(MultipleItemsSource.ItemsSourcesProperty)
        End Get
    End Property
    Private Shared ReadOnly ItemsSourcesPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("ItemsSources", _
                              GetType(System.Collections.ObjectModel.ObservableCollection(Of IEnumerable)), GetType(MultipleItemsSource), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly ItemsSourcesProperty As DependencyProperty = _
                             ItemsSourcesPropertyKey.DependencyProperty

    Private Sub _ItemsSourceProviders_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs) Handles _ItemsSourceProviders.CollectionChanged

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
        RaiseEvent CollectionChanged(Me, e)
    End Sub
#End Region
#Region "Multiple IList"

#End Region

    Public Sub Add(item As ItemsSourceProvider) Implements ICollection(Of ItemsSourceProvider).Add
        DirectCast(_ItemsSourceProviders, ICollection(Of ItemsSourceProvider)).Add(item)
    End Sub

    Public Sub Clear() Implements ICollection(Of ItemsSourceProvider).Clear
        DirectCast(_ItemsSourceProviders, ICollection(Of ItemsSourceProvider)).Clear()
    End Sub

    Public Function Contains(item As ItemsSourceProvider) As Boolean Implements ICollection(Of ItemsSourceProvider).Contains
        Return DirectCast(_ItemsSourceProviders, ICollection(Of ItemsSourceProvider)).Contains(item)
    End Function

    Public Sub CopyTo(array() As ItemsSourceProvider, arrayIndex As Integer) Implements ICollection(Of ItemsSourceProvider).CopyTo
        DirectCast(_ItemsSourceProviders, ICollection(Of ItemsSourceProvider)).CopyTo(array, arrayIndex)
    End Sub

    Public ReadOnly Property Count As Integer Implements ICollection(Of ItemsSourceProvider).Count
        Get
            Return _ItemsSourceProviders.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of ItemsSourceProvider).IsReadOnly
        Get
            Return DirectCast(_ItemsSourceProviders, ICollection(Of ItemsSourceProvider)).IsReadOnly
        End Get
    End Property

    Public Function Remove(item As ItemsSourceProvider) As Boolean Implements ICollection(Of ItemsSourceProvider).Remove
        Return DirectCast(_ItemsSourceProviders, ICollection(Of ItemsSourceProvider)).Remove(item)
    End Function

    Public Function GetEnumerator() As IEnumerator(Of ItemsSourceProvider) Implements IEnumerable(Of ItemsSourceProvider).GetEnumerator
        Return DirectCast(_ItemsSourceProviders, IEnumerable(Of ItemsSourceProvider)).GetEnumerator
    End Function

    Public Function IndexOf(item As ItemsSourceProvider) As Integer Implements IList(Of ItemsSourceProvider).IndexOf
        Return DirectCast(_ItemsSourceProviders, IList(Of ItemsSourceProvider)).IndexOf(item)
    End Function

    Public Sub Insert(index As Integer, item As ItemsSourceProvider) Implements IList(Of ItemsSourceProvider).Insert
        DirectCast(_ItemsSourceProviders, IList(Of ItemsSourceProvider)).Insert(index, item)
    End Sub

    Default Public Property Item(index As Integer) As ItemsSourceProvider Implements IList(Of ItemsSourceProvider).Item
        Get
            Return DirectCast(_ItemsSourceProviders, IList(Of ItemsSourceProvider)).Item(index)
        End Get
        Set(value As ItemsSourceProvider)
            DirectCast(_ItemsSourceProviders, IList(Of ItemsSourceProvider)).Item(index) = value
        End Set
    End Property

    Public Sub RemoveAt(index As Integer) Implements IList(Of ItemsSourceProvider).RemoveAt
        DirectCast(_ItemsSourceProviders, IList(Of ItemsSourceProvider)).RemoveAt(index)
    End Sub

    Private Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
        Return DirectCast(_ItemsSourceProviders, IEnumerable).GetEnumerator()
    End Function

    Public Event CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs) Implements Specialized.INotifyCollectionChanged.CollectionChanged
End Class
