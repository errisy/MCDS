Imports System.Windows, System.Windows.Media, System.Windows.Input
Imports Errisy
Public Class CutSiteViewModel
    Inherits Errisy.GeometryContainer
    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New CutSiteViewModel
    End Function
    'CutSiteViewModel->Name As String Default: ""
    Public Property Name As String
        Get
            Return GetValue(NameProperty)
        End Get
        Set(ByVal value As String)
            SetValue(NameProperty, value)
        End Set
    End Property
    Public Shared ReadOnly NameProperty As DependencyProperty = _
                           DependencyProperty.Register("Name", _
                           GetType(String), GetType(CutSiteViewModel), _
                           New PropertyMetadata(""))
    'CutSiteViewModel->Sequence As String with Event Default: ""
    Public Property Sequence As String
        Get
            Return GetValue(SequenceProperty)
        End Get
        Set(ByVal value As String)
            SetValue(SequenceProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SequenceProperty As DependencyProperty = _
                           DependencyProperty.Register("Sequence", _
                           GetType(String), GetType(CutSiteViewModel), _
                           New PropertyMetadata("", New PropertyChangedCallback(AddressOf SharedModelChanged)))
    'CutSiteViewModel->SCut As Integer Default: 0
    Public Property SCut As Integer
        Get
            Return GetValue(SCutProperty)
        End Get
        Set(ByVal value As Integer)
            SetValue(SCutProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SCutProperty As DependencyProperty = _
                           DependencyProperty.Register("SCut", _
                           GetType(Integer), GetType(CutSiteViewModel), _
                           New PropertyMetadata(0, New PropertyChangedCallback(AddressOf SharedModelChanged)))
    'CutSiteViewModel->ACut As Integer Default: 0
    Public Property ACut As Integer
        Get
            Return GetValue(ACutProperty)
        End Get
        Set(ByVal value As Integer)
            SetValue(ACutProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ACutProperty As DependencyProperty = _
                           DependencyProperty.Register("ACut", _
                           GetType(Integer), GetType(CutSiteViewModel), _
                           New PropertyMetadata(0, New PropertyChangedCallback(AddressOf SharedModelChanged)))

    Private Shared Sub SharedModelChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, CutSiteViewModel).ModelChanged(d, e)
    End Sub
    Private Sub ModelChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim _Models As New System.Collections.ObjectModel.ObservableCollection(Of Errisy.AllocationViewModel)
        Dim _Sequence As String = Sequence
        If _Sequence IsNot Nothing Then
            Dim _Length As Integer = _Sequence.Length
            Dim _FontFamily As New FontFamily("Arial")
            Dim _OffsetX As Double = 12.0#
            Dim _OffsetY As Double = 16.0#
            Dim _FontWidth As Double = 16.0#
            Dim _FontHeight As Double = 20.0#
            For i As Integer = 0 To _Length - 1
                _Models.Add(New FormatedTextViewModel With {.Fill = Brushes.Blue, .Text = _Sequence(i), .FontFamily = _FontFamily, .FontWeight = FontWeights.Bold, .FontSize = 18.0#, .Location = New Point(_OffsetX + _FontWidth * i, _OffsetY)})
                _Models.Add(New FormatedTextViewModel With {.Fill = Brushes.Blue, .Text = Nuctions.ComplementChar(_Sequence(i)), .FontFamily = _FontFamily, .FontWeight = FontWeights.Bold, .FontSize = 18.0#, .Location = New Point(_OffsetX + _FontWidth * i, _OffsetY + _FontHeight)})
            Next
            For i As Integer = 0 To _Length
                Dim _S As New CutViewModel(New Point(_OffsetX - 1.0# + _FontWidth * i, _OffsetY), True, i) With {.Stroke = Brushes.Black, .Fill = Brushes.LightGray, .IsSelected = i = SCut}
                Dim _A As New CutViewModel(New Point(_OffsetX - 1.0# + _FontWidth * i, _OffsetY + _FontHeight * 2.0#), False, i) With {.Stroke = Brushes.Black, .Fill = Brushes.LightGray, .IsSelected = i = ACut}
                AddHandler _S.MouseDown, AddressOf SelectCut
                AddHandler _A.MouseDown, AddressOf SelectCut
                _Models.Add(_S)
                _Models.Add(_A)
            Next
        End If
        SetValue(GeometryModelsPropertyKey, _Models)
    End Sub

    Private Sub SelectCut(sender As GeometryViewModel, e As MouseButtonEventArgs)
        If Not (TypeOf sender Is CutViewModel) Then Return
        Dim _CutModel As CutViewModel = sender
        If GeometryModels Is Nothing Then Return
        If _CutModel.IsSenseSequence Then
            SetValue(SCutProperty, _CutModel.SiteIndex)
        Else
            SetValue(ACutProperty, _CutModel.SiteIndex)
        End If
    End Sub

End Class
