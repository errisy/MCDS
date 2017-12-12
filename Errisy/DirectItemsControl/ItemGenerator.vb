Public Class ItemGenerator
    Public AddingViewForItem As Action(Of Object, FrameworkElement)
    Private ItemToViewHashTable As New Hashtable
    Private ViewToItemHashTable As New Hashtable
    Private _Panel As Panel
    Public Sub New(panel As Panel)
        _Panel = panel
    End Sub
    Friend Sub Clear()
        For Each value In ItemToViewHashTable.Values
            If _Panel.Children.Contains(value) Then _Panel.Children.Remove(value)
        Next
        ItemToViewHashTable.Clear()
        ViewToItemHashTable.Clear()
        '_Panel.Children.Clear()
    End Sub
    Friend Function Add(item As Object, view As FrameworkElement) As Boolean
        Dim hashItem = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(item)
        Dim hashView = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(view)
        If Not ItemToViewHashTable.ContainsKey(hashItem) AndAlso Not ViewToItemHashTable.ContainsKey(hashView) Then
            If AddingViewForItem IsNot Nothing Then AddingViewForItem.Invoke(item, view)
            _Panel.Children.Add(view)
            ItemToViewHashTable.Add(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(item), view)
            ViewToItemHashTable.Add(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(view), item)
            Return True
        Else
            Return False
        End If
    End Function
    Friend Function Remove(item As Object) As Boolean
        Dim hashItem = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(item)
        If ItemToViewHashTable.ContainsKey(hashItem) Then
            Dim view = ItemToViewHashTable(hashItem)
            _Panel.Children.Remove(view)
            Dim hashView = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(view)
            ItemToViewHashTable.Remove(hashItem)
            ItemToViewHashTable.Remove(hashView)
            Return True
        Else
            Return False
        End If
    End Function
    Public Function GetItemForView(ByRef view As FrameworkElement) As Object
        Dim hash = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(view)
        If ViewToItemHashTable.ContainsKey(hash) Then Return DirectCast(ViewToItemHashTable(hash), WeakReference).Target
        Return Nothing
    End Function
    ''' <summary>
    ''' 由于Virtualization，不保证View一定会存在。
    ''' </summary>
    ''' <param name="item"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetViewForItem(ByRef item As Object) As FrameworkElement
        Dim hash = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(item)
        If ItemToViewHashTable.ContainsKey(hash) Then Return ItemToViewHashTable(hash)
        Return Nothing
    End Function
End Class

Public Class ItemViewEventArgs
    Inherits RoutedEventArgs
    Public Sub New(addingItem As Object, addingView As FrameworkElement)
        _Item = addingItem
        _View = addingView
    End Sub
    Private _Item As Object
    Public ReadOnly Property Item As Object
        Get
            Return _Item
        End Get
    End Property
    Private _View As FrameworkElement
    Public ReadOnly Property View As FrameworkElement
        Get
            Return _View
        End Get
    End Property
End Class

Public Delegate Sub ItemViewEventHandler(sender As Object, e As ItemViewEventArgs)