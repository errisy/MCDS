Imports Errisy
Imports System.Windows, System.Windows.Media, System.Windows.Input, System.Windows.Data
Public Class EnzymeSequenceCutViewModel
    Inherits GeometryViewModel
    Public Sub New()
    End Sub
    Friend Sub New(_StartIndex As Integer, _RegionStart As Integer, _RegionEnd As Integer, _Top As Double, _Analysis As Nuctions.EnzymeAnalysis, _File As Nuctions.GeneFile,
                   _NucleotidesPerRow As Integer, _NucleotidesPerGroup As Integer, _RowLeft As Double, _FontSize As Double, _GroupSpacing As Double)
        'Draw the Geometry
        Dim _StreamGeometry As New StreamGeometry
        Dim _From As Integer = _RegionStart - _StartIndex
        Dim _To As Integer = _RegionEnd - _StartIndex + 1
        Dim _Location As New Point(_RowLeft + _From * _FontSize * WPFSequenceView.SequenceFontFactor + (_From \ _NucleotidesPerGroup) * _GroupSpacing, _Top)
        Dim _S As Integer = _Analysis.SCut - _StartIndex
        Dim _A As Integer = _Analysis.ACut - _StartIndex
        Using _Context = _StreamGeometry.Open
            _Context.BeginFigure(_Location, True, True)
            If _S >= _From And _S <= _To Then
                _Context.LineTo(New Point(_RowLeft + _S * _FontSize * WPFSequenceView.SequenceFontFactor + (_S \ _NucleotidesPerGroup) * _GroupSpacing, _Top), False, False)
                _Context.LineTo(New Point(_RowLeft + _S * _FontSize * WPFSequenceView.SequenceFontFactor + (_S \ _NucleotidesPerGroup) * _GroupSpacing, _Top + _FontSize * 0.5#), True, False)
            ElseIf _S < _From Then
                _S = _From
            ElseIf _S > _To Then
                _S = _To
            End If
            If _A >= _From And _A <= _To Then
                _Context.LineTo(New Point(_RowLeft + _A * _FontSize * WPFSequenceView.SequenceFontFactor + (_A \ _NucleotidesPerGroup) * _GroupSpacing, _Top + _FontSize), False, False)
                _Context.LineTo(New Point(_RowLeft + _A * _FontSize * WPFSequenceView.SequenceFontFactor + (_A \ _NucleotidesPerGroup) * _GroupSpacing, _Top + _FontSize * 0.5#), True, False)
            ElseIf _A < _From Then
                _A = _From
            ElseIf _A > _To Then
                _A = _To
            End If
            If _S <> _A Then
                _Context.LineTo(New Point(_RowLeft + _S * _FontSize * WPFSequenceView.SequenceFontFactor + (_S \ _NucleotidesPerGroup) * _GroupSpacing, _Top + _FontSize * 0.5#), False, False)
                _Context.LineTo(New Point(_RowLeft + _A * _FontSize * WPFSequenceView.SequenceFontFactor + (_A \ _NucleotidesPerGroup) * _GroupSpacing, _Top + _FontSize * 0.5#), True, False)
            End If
        End Using
        SetValue(GeometryProperty, _StreamGeometry)
    End Sub
    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New EnzymeSequenceCutViewModel
    End Function
End Class
