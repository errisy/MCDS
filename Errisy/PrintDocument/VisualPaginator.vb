Public Class VisualPaginator
    Inherits DocumentPaginator
    Private Pages As New List(Of Visual)
    Public Sub AddPageVisual(page As Visual)
        Pages.Add(page)
    End Sub

    Public Overrides Function GetPage(pageNumber As Integer) As DocumentPage
        Return New DocumentPage(Pages(pageNumber))
    End Function

    Public Overrides ReadOnly Property IsPageCountValid As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property PageCount As Integer
        Get
            Return Pages.Count
        End Get
    End Property

    Public Overrides Property PageSize As Size

    Public Overrides ReadOnly Property Source As IDocumentPaginatorSource
        Get
            Return New VisualPaginatorSource(Me)
        End Get
    End Property
End Class

Public Class VisualPaginatorSource
    Implements IDocumentPaginatorSource
    Private _Paginator As DocumentPaginator
    Public Sub New(vPaginator As DocumentPaginator)
        _Paginator = vPaginator
    End Sub
    Public ReadOnly Property DocumentPaginator As DocumentPaginator Implements IDocumentPaginatorSource.DocumentPaginator
        Get
            Return _Paginator
        End Get
    End Property
End Class

Public Class VisualRowAlignment
    Implements System.ComponentModel.INotifyPropertyChanged

    'Public Property RowType As RowTypeEnum
    Private _RowType As RowTypeEnum
    Public Property RowType As RowTypeEnum
        Get
            Return _RowType
        End Get
        Set(value As RowTypeEnum)
            _RowType = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("RowType"))
        End Set
    End Property
    'Public Property Height As Double
    Private _Height As Double
    Public Property Height As Double
        Get
            Return _Height
        End Get
        Set(value As Double)
            _Height = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Height"))
        End Set
    End Property

    'Public Property PrefixTemplate As DataTemplate
    Private _PrefixTemplate As DataTemplate
    Public Property PrefixTemplate As DataTemplate
        Get
            Return _PrefixTemplate
        End Get
        Set(value As DataTemplate)
            _PrefixTemplate = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("PrefixTemplate"))
        End Set
    End Property
    'Public Property SuffixTemplate As DataTemplate
    Private _SuffixTemplate As DataTemplate
    Public Property SuffixTemplate As DataTemplate
        Get
            Return _SuffixTemplate
        End Get
        Set(value As DataTemplate)
            _SuffixTemplate = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("SuffixTemplate"))
        End Set
    End Property

    'Public Property Elements As List(Of FrameworkElement)
    Private _Elements As New List(Of FrameworkElement)
    Public ReadOnly Property Elements As List(Of FrameworkElement)
        Get
            Return _Elements
        End Get
    End Property
    Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Enum RowTypeEnum
    Fixed
    Auto
    FullPage
    Flow
End Enum
Public Class VisualPageBase
    Inherits UserControl
    'VisualPageBase->PageMargin As Thickness Default: New Thickness(8#)
    Public Property PageMargin As Thickness
        Get
            Return GetValue(PageMarginProperty)
        End Get
        Set(ByVal value As Thickness)
            SetValue(PageMarginProperty, value)
        End Set
    End Property
    Public Shared ReadOnly PageMarginProperty As DependencyProperty = _
                           DependencyProperty.Register("PageMargin", _
                           GetType(Thickness), GetType(VisualPageBase), _
                           New PropertyMetadata(New Thickness(8.0#), New PropertyChangedCallback(AddressOf SharedPageMarginChanged)))
    Private Shared Sub SharedPageMarginChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, VisualPageBase).PageMarginChanged(d, e)
    End Sub
    Private Sub PageMarginChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim t As Thickness = e.NewValue
        SetValue(LeftMarginPropertyKey, New GridLength(t.Left))
        SetValue(RightMarginPropertyKey, New GridLength(t.Right))
        SetValue(TopMarginPropertyKey, New GridLength(t.Top))
        SetValue(BottomMarginPropertyKey, New GridLength(t.Bottom))
    End Sub
    'VisualPageBase -> LeftMargin As GridLength Default: 8#
    Public ReadOnly Property LeftMargin As GridLength
        Get
            Return GetValue(VisualPageBase.LeftMarginProperty)
        End Get
    End Property
    Private Shared ReadOnly LeftMarginPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("LeftMargin", _
                              GetType(GridLength), GetType(VisualPageBase), _
                              New PropertyMetadata(New GridLength(8.0#)))
    Public Shared ReadOnly LeftMarginProperty As DependencyProperty = _
                             LeftMarginPropertyKey.DependencyProperty
    'VisualPageBase -> RightMargin As GridLength Default: 0#
    Public ReadOnly Property RightMargin As GridLength
        Get
            Return GetValue(VisualPageBase.RightMarginProperty)
        End Get
    End Property
    Private Shared ReadOnly RightMarginPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("RightMargin", _
                              GetType(GridLength), GetType(VisualPageBase), _
                              New PropertyMetadata(New GridLength(8.0#)))
    Public Shared ReadOnly RightMarginProperty As DependencyProperty = _
                             RightMarginPropertyKey.DependencyProperty
    'VisualPageBase -> TopMargin As GridLength Default: 8#
    Public ReadOnly Property TopMargin As GridLength
        Get
            Return GetValue(VisualPageBase.TopMarginProperty)
        End Get
    End Property
    Private Shared ReadOnly TopMarginPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("TopMargin", _
                              GetType(GridLength), GetType(VisualPageBase), _
                              New PropertyMetadata(New GridLength(8.0#)))
    Public Shared ReadOnly TopMarginProperty As DependencyProperty = _
                             TopMarginPropertyKey.DependencyProperty
    'VisualPageBase -> BottomMargin As GridLength Default: 8#
    Public ReadOnly Property BottomMargin As GridLength
        Get
            Return GetValue(VisualPageBase.BottomMarginProperty)
        End Get
    End Property
    Private Shared ReadOnly BottomMarginPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("BottomMargin", _
                              GetType(GridLength), GetType(VisualPageBase), _
                              New PropertyMetadata(New GridLength(8.0#)))
    Public Shared ReadOnly BottomMarginProperty As DependencyProperty = _
                             BottomMarginPropertyKey.DependencyProperty
End Class

Friend Class PageStackPanel
    Inherits StackPanel
End Class
Friend Class PageGrid
    Inherits Grid
End Class