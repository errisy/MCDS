<Serializable>
Public Class ItemsSourceProvider
    Private WithEvents _INotifyCollectionChanged As System.Collections.Specialized.INotifyCollectionChanged
    Private _IList As IList
    Public Property List As IList
        Get
            Return _IList
        End Get
        Set(value As IList)
            _IList = value
            If TypeOf value Is System.Collections.Specialized.INotifyCollectionChanged Then _INotifyCollectionChanged = value
        End Set
    End Property
    <NonSerialized> Public Event ItemsAdded As EventHandler(Of ItemsEventArgs)
    <NonSerialized> Public Event ItemsDeleted As EventHandler(Of ItemsEventArgs)
    Private Sub _INotifyCollectionChanged_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs) Handles _INotifyCollectionChanged.CollectionChanged
        If e.OldItems IsNot Nothing Then
            RaiseEvent ItemsDeleted(Me, New ItemsEventArgs(e.OldItems, e.OldStartingIndex))
        End If
        If e.NewItems IsNot Nothing Then
            RaiseEvent ItemsAdded(Me, New ItemsEventArgs(e.NewItems, e.NewStartingIndex))
        End If
    End Sub
End Class
Public Class ItemsEventArgs
    Inherits EventArgs
    Private _Items As IList
    Private _Index As Integer
    Public Sub New(vItems As IList, vIndex As Integer)
        _Items = vItems
        _Index = vIndex
    End Sub
    Public ReadOnly Property Items As IList
        Get
            Return _Items
        End Get
    End Property
    Public ReadOnly Property StaringIndex As Integer
        Get
            Return _Index
        End Get
    End Property
End Class