Imports Errisy
Imports System.Windows, System.Windows.Media, System.Windows.Input, System.Windows.Data

Public Class FeatureSequenceViewModel
    Inherits GeneSequenceViewModel
    Public Sub New()
    End Sub
    Public Sub New(_StartIndex As Integer, _RegionStart As Integer, _RegionEnd As Integer, _Top As Double, _Feature As Nuctions.GeneAnnotation, _File As Nuctions.GeneFile,
                   _NucleotidesPerRow As Integer, _NucleotidesPerGroup As Integer, _RowLeft As Double, _FontSize As Double, _GroupSpacing As Double)
        'Draw the Geometry
        Dim _StreamGeometry As New StreamGeometry
        Dim _From As Integer = _RegionStart - _StartIndex
        Dim _To As Integer = _RegionEnd - _StartIndex
        Dim _Location As New Point(_RowLeft + _From * _FontSize * WPFSequenceView.SequenceFontFactor + (_From \ _NucleotidesPerGroup) * _GroupSpacing, _Top)
        Using _Context = _StreamGeometry.Open
            _Context.BeginFigure(_Location, True, True)
            _Context.LineTo(New Point(_RowLeft + (_To + 1) * _FontSize * WPFSequenceView.SequenceFontFactor + (_To \ _NucleotidesPerGroup) * _GroupSpacing, _Top), True, False)
            _Context.LineTo(New Point(_RowLeft + (_To + 1) * _FontSize * WPFSequenceView.SequenceFontFactor + (_To \ _NucleotidesPerGroup) * _GroupSpacing, _Top + _FontSize), True, False)
            _Context.LineTo(New Point(_RowLeft + _From * _FontSize * WPFSequenceView.SequenceFontFactor + (_From \ _NucleotidesPerGroup) * _GroupSpacing, _Top + _FontSize), True, False)
            _Context.LineTo(_Location, True, False)
        End Using
        'We will draw the cut site as well.
        SetValue(FeaturePropertyKey, _Feature)
        SetValue(LocationPropertyKey, _Location)
        SetValue(RegionStartPropertyKey, _RegionStart)
        SetValue(RegionEndPropertyKey, _RegionEnd)
        SetValue(FilePropertyKey, _File)
        SetValue(GeometryProperty, _StreamGeometry)
    End Sub
    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New FeatureSequenceViewModel
    End Function
    'FeatureSequenceViewModel -> Feature As Nuctions.GeneAnnotation Default: Nothing
    Public ReadOnly Property Feature As Nuctions.GeneAnnotation
        Get
            Return GetValue(FeatureSequenceViewModel.FeatureProperty)
        End Get
    End Property
    Private Shared ReadOnly FeaturePropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Feature", _
                              GetType(Nuctions.GeneAnnotation), GetType(FeatureSequenceViewModel), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly FeatureProperty As DependencyProperty = _
                             FeaturePropertyKey.DependencyProperty

End Class
