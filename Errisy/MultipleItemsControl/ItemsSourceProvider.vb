Public Class ItemsSourceProvider
    Inherits FrameworkElement
    'ItemsSourceProvider->ItemsSource As IEnumerable with Event Default: Nothing
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
                           GetType(IEnumerable), GetType(ItemsSourceProvider), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedItemsSourceChanged)))
    Private _HierarchicalItemsSourceBinding As Binding = Nothing
    'Window1->HierarchicalItemsSourceBinding As Binding Default: Nothing
    Public Property HierarchicalItemsSourceBinding As Binding
        Get
            Return _HierarchicalItemsSourceBinding
        End Get
        Set(ByVal value As Binding)
            _HierarchicalItemsSourceBinding = value
        End Set
    End Property
    Private Shared Sub SharedItemsSourceChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, ItemsSourceProvider).ItemsSourceChanged(d, e)
    End Sub
    Private Sub ItemsSourceChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim mic As MultipleItemsControl = Parent
        If TypeOf e.OldValue Is IEnumerable Then
            mic.ItemsSources.Remove(e.OldValue)
            If _HierarchicalItemsSourceBinding IsNot Nothing Then
                For Each i In e.OldValue
                    Dim be As New BindingEvaluator
                    be.SetBinding(BindingEvaluator.ValueProperty, _HierarchicalItemsSourceBinding)
                    be.DataContext = i
                    Dim result = be.Value
                    If TypeOf result Is IEnumerable AndAlso mic.ItemsSources.Contains(result) Then mic.ItemsSources.Remove(result)
                Next
                _ItemsSource = Nothing
            End If
        End If
        If TypeOf e.NewValue Is IEnumerable Then
            mic.ItemsSources.Add(e.NewValue)
            If _HierarchicalItemsSourceBinding IsNot Nothing Then
                For Each i In e.NewValue
                    Dim be As New BindingEvaluator
                    be.SetBinding(BindingEvaluator.ValueProperty, _HierarchicalItemsSourceBinding)
                    be.DataContext = i
                    Dim result = be.Value
                    If TypeOf result Is IEnumerable AndAlso Not mic.ItemsSources.Contains(result) Then mic.ItemsSources.Add(result)
                Next
                If TypeOf e.NewValue Is System.Collections.Specialized.INotifyCollectionChanged Then _ItemsSource = e.NewValue
            End If
        End If
    End Sub
    Private WithEvents _ItemsSource As System.Collections.Specialized.INotifyCollectionChanged
    Private Sub _ItemsSource_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs) Handles _ItemsSource.CollectionChanged
        Dim mic As MultipleItemsControl = Parent
        If e.OldItems IsNot Nothing Then
            For Each i In e.OldItems
                Dim be As New BindingEvaluator
                be.SetBinding(BindingEvaluator.ValueProperty, _HierarchicalItemsSourceBinding)
                be.DataContext = i
                Dim result = be.Value
                If TypeOf result Is IEnumerable AndAlso mic.ItemsSources.Contains(result) Then mic.ItemsSources.Remove(result)
            Next
        End If
        If e.NewItems IsNot Nothing Then
            For Each i In e.NewItems
                Dim be As New BindingEvaluator
                be.SetBinding(BindingEvaluator.ValueProperty, _HierarchicalItemsSourceBinding)
                be.DataContext = i
                Dim result = be.Value
                If TypeOf result Is IEnumerable AndAlso Not mic.ItemsSources.Contains(result) Then mic.ItemsSources.Add(result)
            Next
        End If
    End Sub
End Class

Public Class BindingEvaluator
    Inherits FrameworkElement
    'BindingEvaluator->Value As Object Default: Nothing
    Public Property Value As Object
        Get
            Return GetValue(ValueProperty)
        End Get
        Set(ByVal value As Object)
            SetValue(ValueProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ValueProperty As DependencyProperty = _
                           DependencyProperty.Register("Value", _
                           GetType(Object), GetType(BindingEvaluator), _
                           New PropertyMetadata(Nothing))

End Class