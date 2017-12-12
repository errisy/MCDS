Imports System.Windows, System.Windows.Media, System.Windows.Input
Public Class CutViewModel
    Inherits Errisy.GeometryViewModel
    Public Sub New()
    End Sub
    Public Sub New(_Location As Point, _IsSenseSequence As Boolean, _SiteIndex As Integer)
        Dim _Geometry As New StreamGeometry
        Using _Context = _Geometry.Open
            _Context.BeginFigure(_Location, True, True)
            If _IsSenseSequence Then
                _Context.LineTo(New Point(_Location.X - 6.0#, _Location.Y - 10.0#), True, False)
                _Context.LineTo(New Point(_Location.X + 6.0#, _Location.Y - 10.0#), True, False)
            Else
                _Context.LineTo(New Point(_Location.X - 6.0#, _Location.Y + 10.0#), True, False)
                _Context.LineTo(New Point(_Location.X + 6.0#, _Location.Y + 10.0#), True, False)
            End If
            _Context.LineTo(_Location, True, False)
        End Using
        SetValue(IsSenseSequenceProperty, _IsSenseSequence)
        SetValue(SiteIndexProperty, _SiteIndex)
        SetValue(GeometryProperty, _Geometry)
    End Sub
    'CutViewModel->IsSelected As Boolean with Event Default: False
    Public Property IsSelected As Boolean
        Get
            Return GetValue(IsSelectedProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsSelectedProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsSelectedProperty As DependencyProperty = _
                           DependencyProperty.Register("IsSelected", _
                           GetType(Boolean), GetType(CutViewModel), _
                           New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedIsSelectedChanged)))
    Private Shared Sub SharedIsSelectedChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, CutViewModel).IsSelectedChanged(d, e)
    End Sub
    Private Sub IsSelectedChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.NewValue Then
            Fill = Brushes.Red
        Else
            Fill = Brushes.LightGray
        End If
    End Sub

    'CutViewModel->SiteIndex As Integer Default: 0
    Public Property SiteIndex As Integer
        Get
            Return GetValue(SiteIndexProperty)
        End Get
        Set(ByVal value As Integer)
            SetValue(SiteIndexProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SiteIndexProperty As DependencyProperty = _
                           DependencyProperty.Register("SiteIndex", _
                           GetType(Integer), GetType(CutViewModel), _
                           New PropertyMetadata(0))
    'CutViewModel->IsSenseSequence As Boolean Default: True
    Public Property IsSenseSequence As Boolean
        Get
            Return GetValue(IsSenseSequenceProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsSenseSequenceProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsSenseSequenceProperty As DependencyProperty = _
                           DependencyProperty.Register("IsSenseSequence", _
                           GetType(Boolean), GetType(CutViewModel), _
                           New PropertyMetadata(True))

End Class
