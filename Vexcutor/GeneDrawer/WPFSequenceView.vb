Imports System.Windows
Imports System.Windows.Media
Imports System.Windows.Input
Imports System.Collections.ObjectModel

Public Class WPFSequenceView
    Inherits System.Windows.Freezable
    Private _SequenceRows As New ObservableCollection(Of WPFSequenceObjectRow)
    Public Sub New()
        SetValue(SequenceRowsPropertyKey, _SequenceRows)
    End Sub
    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New WPFSequenceView
    End Function

    'WPFSequenceView->NucleotidesPerRow As Integer Default: 50
    Public Property NucleotidesPerRow As Integer
        Get
            Return GetValue(NucleotidesPerRowProperty)
        End Get
        Set(ByVal value As Integer)
            SetValue(NucleotidesPerRowProperty, value)
        End Set
    End Property
    Public Shared ReadOnly NucleotidesPerRowProperty As DependencyProperty = _
                           DependencyProperty.Register("NucleotidesPerRow", _
                           GetType(Integer), GetType(WPFSequenceView), _
                           New PropertyMetadata(50, New PropertyChangedCallback(AddressOf SharedViewChanged)))
    'WPFSequenceView->NucleotidesPerGroup As Integer Default: 10
    Public Property NucleotidesPerGroup As Integer
        Get
            Return GetValue(NucleotidesPerGroupProperty)
        End Get
        Set(ByVal value As Integer)
            SetValue(NucleotidesPerGroupProperty, value)
        End Set
    End Property
    Public Shared ReadOnly NucleotidesPerGroupProperty As DependencyProperty = _
                           DependencyProperty.Register("NucleotidesPerGroup", _
                           GetType(Integer), GetType(WPFSequenceView), _
                           New PropertyMetadata(10, New PropertyChangedCallback(AddressOf SharedViewChanged)))
    'WPFSequenceView->GeneFile As Nuctions.GeneFile Default: Nothing
    Public Property GeneFile As Nuctions.GeneFile
        Get
            Return GetValue(GeneFileProperty)
        End Get
        Set(ByVal value As Nuctions.GeneFile)
            SetValue(GeneFileProperty, value)
        End Set
    End Property
    Public Shared ReadOnly GeneFileProperty As DependencyProperty = _
                           DependencyProperty.Register("GeneFile", _
                           GetType(Nuctions.GeneFile), GetType(WPFSequenceView), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedViewChanged)))
    'WPFSequenceView->Indent As Double Default: 20#
    Public Property Indent As Double
        Get
            Return GetValue(IndentProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(IndentProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IndentProperty As DependencyProperty = _
                           DependencyProperty.Register("Indent", _
                           GetType(Double), GetType(WPFSequenceView), _
                           New PropertyMetadata(20.0#))
    'WPFSequenceView->GroupSpacing As Double Default: 10#
    Public Property GroupSpacing As Double
        Get
            Return GetValue(GroupSpacingProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(GroupSpacingProperty, value)
        End Set
    End Property
    Public Shared ReadOnly GroupSpacingProperty As DependencyProperty = _
                           DependencyProperty.Register("GroupSpacing", _
                           GetType(Double), GetType(WPFSequenceView), _
                           New PropertyMetadata(10.0#))

    'WPFSequenceView->RestrictionEnzymes As List(Of String) Default: Nothing
    Public Property RestrictionEnzymes As List(Of String)
        Get
            Return GetValue(RestrictionEnzymesProperty)
        End Get
        Set(ByVal value As List(Of String))
            SetValue(RestrictionEnzymesProperty, value)
        End Set
    End Property
    Public Shared ReadOnly RestrictionEnzymesProperty As DependencyProperty = _
                           DependencyProperty.Register("RestrictionEnzymes", _
                           GetType(List(Of String)), GetType(WPFSequenceView), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedViewChanged)))
    'WPFSequenceView->FontSize As Double Default: 12#
    Public Property FontSize As Double
        Get
            Return GetValue(FontSizeProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(FontSizeProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FontSizeProperty As DependencyProperty = _
                           DependencyProperty.Register("FontSize", _
                           GetType(Double), GetType(WPFSequenceView), _
                           New PropertyMetadata(12.0#, New PropertyChangedCallback(AddressOf SharedViewChanged)))
    'WPFSequenceView -> SequenceRows As ObservableCollection(Of WPFSequenceRow) Default: Nothing
    Public ReadOnly Property SequenceRows As ObservableCollection(Of WPFSequenceObjectRow)
        Get
            Return GetValue(WPFSequenceView.SequenceRowsProperty)
        End Get
    End Property
    Private Shared ReadOnly SequenceRowsPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("SequenceRows", _
                              GetType(ObservableCollection(Of WPFSequenceObjectRow)), GetType(WPFSequenceView), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly SequenceRowsProperty As DependencyProperty = _
                             SequenceRowsPropertyKey.DependencyProperty
    'WPFSequenceView->EnzymeCuts As List(Of Nuctions.EnzymeAnalysis) Default: Nothing
    Friend Property EnzymeCuts As List(Of Nuctions.EnzymeAnalysis)
        Get
            Return GetValue(EnzymeCutsProperty)
        End Get
        Set(ByVal value As List(Of Nuctions.EnzymeAnalysis))
            SetValue(EnzymeCutsProperty, value)
        End Set
    End Property
    Public Shared ReadOnly EnzymeCutsProperty As DependencyProperty = _
                           DependencyProperty.Register("EnzymeCuts", _
                           GetType(List(Of Nuctions.EnzymeAnalysis)), GetType(WPFSequenceView), _
                           New PropertyMetadata(Nothing))

    Private Shared Sub SharedViewChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        'DirectCast(d, WPFSequenceView).ViewChanged(d, e)
    End Sub
    Public Sub Load()
        ViewChanged()
    End Sub
    Private Sub ViewChanged() 'd As DependencyObject, e As DependencyPropertyChangedEventArgs)
        _SequenceRows.Clear()
        Dim _GeneFile As Nuctions.GeneFile = GeneFile
        If _GeneFile Is Nothing Then Return
        Dim _NucleotidesPerRow As Integer = NucleotidesPerRow
        Dim _NucleotidesPerGroup As Integer = NucleotidesPerGroup
        If _NucleotidesPerRow < 1 Then Return
        If _NucleotidesPerGroup < 1 Then Return
        Dim _FontSize As Double = FontSize
        If _FontSize < 4.0# Then Return
        Dim _Indent As Double = Indent
        Dim _GroupSpacing As Double = GroupSpacing
        Dim _Length As Integer = _GeneFile.Length
        'Generate Rows
        For i As Integer = 1 To Math.Ceiling(_Length / _NucleotidesPerRow)
            _SequenceRows.Add(New WPFSequenceObjectRow)
        Next

        'Analyze Enzymes
        Dim _RestrictionEnzymes = RestrictionEnzymes
        Dim _EnzymeAnalysisList As New List(Of Nuctions.EnzymeAnalysis)

        If _RestrictionEnzymes IsNot Nothing Then
            Dim _EnzymeList As New List(Of String)
            Dim ear As Nuctions.EnzymeAnalysis.EnzymeAnalysisResult
            For Each _RestrictionEnzyme In RestrictionEnzymes
                _EnzymeList.Clear()
                _EnzymeList.Add(_RestrictionEnzyme)
                ear = New Nuctions.EnzymeAnalysis.EnzymeAnalysisResult(_EnzymeList, _GeneFile)
                _EnzymeAnalysisList.AddRange(ear.CutList)
            Next
        End If
        SetValue(EnzymeCutsProperty, _EnzymeAnalysisList)

        'Set up Enzymes and Features for each row
        For i As Integer = 0 To _SequenceRows.Count - 1
            _SequenceRows(i).Initialize(i * _NucleotidesPerRow + 1, _Indent, _GroupSpacing, _FontSize, _NucleotidesPerRow, _NucleotidesPerGroup)
            '_SequenceRows(i).Setup(i * _NucleotidesPerRow + 1, _Indent, _GroupSpacing, _FontSize, _NucleotidesPerRow, _NucleotidesPerGroup, _EnzymeAnalysisList, _GeneFile.Features, _GeneFile)
        Next
    End Sub

    Protected Overrides Function FreezeCore(isChecking As Boolean) As Boolean
        'For Each _row In _SequenceRows
        '    _row.Freeze()
        'Next
        Return MyBase.FreezeCore(isChecking)
    End Function
    Public Shared SequenceFontFactor As Double = 0.707#

End Class