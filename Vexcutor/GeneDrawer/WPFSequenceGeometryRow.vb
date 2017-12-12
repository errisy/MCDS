
Imports System.Windows
Imports System.Windows.Media
Imports System.Windows.Input
Imports System.Collections.ObjectModel
Public Class WPFSequenceGeometryRow
    Inherits Errisy.GeometryContainer

    Private _ForwardSequence As New ObservableCollection(Of NucleotideViewModel)
    Private _ComplementSequence As New ObservableCollection(Of NucleotideViewModel)
    Private _ForwardPrimer As New ObservableCollection(Of NucleotideViewModel)
    Private _ReversePrimer As New ObservableCollection(Of NucleotideViewModel)
    Private _Enzymes As New ObservableCollection(Of EnzymeSequenceViewModel)
    Private _Features As New ObservableCollection(Of FeatureSequenceViewModel)
    Private _Index As New Errisy.FormatedTextViewModel

    Public Sub New()
        AddHandler _ForwardSequence.CollectionChanged, AddressOf CollectionChanged
        AddHandler _ComplementSequence.CollectionChanged, AddressOf CollectionChanged
        AddHandler _ForwardPrimer.CollectionChanged, AddressOf CollectionChanged
        AddHandler _ReversePrimer.CollectionChanged, AddressOf CollectionChanged
        AddHandler _Enzymes.CollectionChanged, AddressOf CollectionChanged
        AddHandler _Features.CollectionChanged, AddressOf CollectionChanged

        SetValue(ForwardSequencePropertyKey, _ForwardSequence)
        SetValue(ComplementSequencePropertyKey, _ComplementSequence)
        SetValue(ForwardPrimerPropertyKey, _ForwardPrimer)
        SetValue(ReversePrimerPropertyKey, _ReversePrimer)
        SetValue(EnzymesPropertyKey, _Enzymes)
        SetValue(FeaturesPropertyKey, _Features)
        SetValue(IndexPropertyKey, _Index)

        Dim _Models = GeometryModels
        If _Models Is Nothing Then Return
        _Models.Add(_Index)
    End Sub
    Public Sub Initialize(StartIndex As Integer, Indent As Double, GroupSpacing As Double, FontSize As Double, NucleotidesPerRow As Integer, NucleotidesPerGroup As Integer)
        SetValue(StartNucleotideIndexPropertyKey, StartIndex)
        Dim _RowLeft As Double = FontSize * WPFSequenceView.SequenceFontFactor * 10.0# + Indent
        Dim _DesiredSize = New Size(_RowLeft + NucleotidesPerRow * FontSize * WPFSequenceView.SequenceFontFactor + ((NucleotidesPerRow \ NucleotidesPerGroup) + 2.0#) * GroupSpacing, FontSize * 5.0#)
        SetValue(DesiredSizePropertyKey, _DesiredSize)
    End Sub
    'WPFSequenceRow -> StartNucleotideIndex As Integer Default: 0
    Public ReadOnly Property StartNucleotideIndex As Integer
        Get
            Return GetValue(WPFSequenceGeometryRow.StartNucleotideIndexProperty)
        End Get
    End Property
    Private Shared ReadOnly StartNucleotideIndexPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("StartNucleotideIndex", _
                              GetType(Integer), GetType(WPFSequenceGeometryRow), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly StartNucleotideIndexProperty As DependencyProperty = _
                             StartNucleotideIndexPropertyKey.DependencyProperty
    'WPFSequenceRow -> IsLoaded As Boolean Default: False
    Public ReadOnly Property IsLoaded As Boolean
        Get
            Return GetValue(WPFSequenceGeometryRow.IsLoadedProperty)
        End Get
    End Property
    Private Shared ReadOnly IsLoadedPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("IsLoaded", _
                              GetType(Boolean), GetType(WPFSequenceGeometryRow), _
                              New PropertyMetadata(False))
    Public Shared ReadOnly IsLoadedProperty As DependencyProperty = _
                             IsLoadedPropertyKey.DependencyProperty

    Friend Sub LoadContent(Indent As Double, GroupSpacing As Double, FontSize As Double, NucleotidesPerRow As Integer, NucleotidesPerGroup As Integer,
                     EnzymeCuts As List(Of Nuctions.EnzymeAnalysis), Annotations As List(Of Nuctions.GeneAnnotation), GeneFile As Nuctions.GeneFile)

        _ForwardSequence.Clear()
        _ComplementSequence.Clear()
        _ForwardPrimer.Clear()
        _ReversePrimer.Clear()
        _Enzymes.Clear()
        _Features.Clear()

        Dim StartIndex As Integer = StartNucleotideIndex
        'Index
        _Index.FontSize = FontSize
        _Index.Text = StartIndex
        Dim _IndexSize = _Index.Bounds.Size
        _Index.Location = New Point(FontSize * WPFSequenceView.SequenceFontFactor * 10.0# - _IndexSize.Width, -FontSize)
        Dim Sequence As String = GeneFile.Sequence
        Dim _FChar As Char
        Dim _RChar As Char
        Dim Length As Integer = GeneFile.Length

        'Nucleotides
        Dim NucleotideAllocator As New Errisy.GeometryAllocator

        Dim _RowLeft As Double = FontSize * WPFSequenceView.SequenceFontFactor * 10.0# + Indent
        Dim Nucleotide As NucleotideViewModel

        NucleotideAllocator.Add(New RectangleGeometry(New Rect(New Point(_RowLeft, -2.0# * FontSize),
                                                                 New Size(_RowLeft + NucleotidesPerRow * FontSize * WPFSequenceView.SequenceFontFactor + (NucleotidesPerRow \ NucleotidesPerGroup) * GroupSpacing, 4.0# * FontSize))))

        For i As Integer = 0 To NucleotidesPerRow - 1
            If i + StartIndex <= Length Then
                _FChar = Sequence(i + StartIndex - 1)
                _RChar = Nuctions.ComplementChar(_FChar)
            Else
                _FChar = " "c
                _RChar = " "c
            End If
            Nucleotide = New NucleotideViewModel With {.Text = _FChar, .FontSize = FontSize, .Location = New Point(_RowLeft + i * FontSize * WPFSequenceView.SequenceFontFactor + (i \ NucleotidesPerGroup) * GroupSpacing, -FontSize)}
            _ForwardSequence.Add(Nucleotide)

            Nucleotide = New NucleotideViewModel With {.Text = _RChar, .FontSize = FontSize, .Location = New Point(_RowLeft + i * FontSize * WPFSequenceView.SequenceFontFactor + (i \ NucleotidesPerGroup) * GroupSpacing, 0.0#)}
            _ComplementSequence.Add(Nucleotide)

            Nucleotide = New NucleotideViewModel With {.FontSize = FontSize, .Location = New Point(_RowLeft + i * FontSize * WPFSequenceView.SequenceFontFactor + (i \ NucleotidesPerGroup) * GroupSpacing, -2.0# * FontSize), .Fill = Brushes.Red}
            _ForwardPrimer.Add(Nucleotide)

            Nucleotide = New NucleotideViewModel With {.FontSize = FontSize, .Location = New Point(_RowLeft + i * FontSize * WPFSequenceView.SequenceFontFactor + (i \ NucleotidesPerGroup) * GroupSpacing, FontSize), .Fill = Brushes.Red}
            _ReversePrimer.Add(Nucleotide)
        Next


        'Allocators
        Dim EnzymeAllocator As New Errisy.GeometryAllocator
        Dim FeatureAllocator As New Errisy.GeometryAllocator
        Dim IsCircular As Boolean = GeneFile.Iscircular

        Dim RegionList As List(Of IntegerRegion)

        Dim EnzymeStartHeight As Double = -3.0# * FontSize

        For Each cut In EnzymeCuts
            RegionList = IntegerRegion.FindRegions(IsCircular, False, cut.StartRec, cut.EndRec, StartIndex, NucleotidesPerRow, Length)
            For Each iRegion In RegionList
                Dim _Height As Double = EnzymeStartHeight
                Dim _EnzymeModel As New EnzymeSequenceViewModel(StartIndex, iRegion.Start, iRegion.End, _Height, cut,
                                                                GeneFile, NucleotidesPerRow, NucleotidesPerGroup, _RowLeft, FontSize, GroupSpacing)
                While EnzymeAllocator.IntersectsWith(_EnzymeModel)
                    _Height -= FontSize
                    _EnzymeModel = New EnzymeSequenceViewModel(StartIndex, iRegion.Start, iRegion.End, _Height, cut,
                                                                GeneFile, NucleotidesPerRow, NucleotidesPerGroup, _RowLeft, FontSize, GroupSpacing)
                End While
                EnzymeAllocator.Add(_EnzymeModel)
                _Enzymes.Add(_EnzymeModel)
            Next
        Next

        Dim FeatureStartHeight As Double
        If _Enzymes.Any Then
            FeatureStartHeight = EnzymeAllocator.Offset.Y - FontSize
        Else
            FeatureStartHeight = EnzymeStartHeight
        End If


        For Each feature In GeneFile.Features
            RegionList = IntegerRegion.FindRegions(IsCircular, feature.Complement, feature.StartPosition, feature.EndPosition, StartIndex, NucleotidesPerRow, Length)

            For Each iRegion In RegionList
                Dim _Height As Double = FeatureStartHeight
                Dim _FeatureModel As New FeatureSequenceViewModel(StartIndex, iRegion.Start, iRegion.End, _Height, feature,
                                                                GeneFile, NucleotidesPerRow, NucleotidesPerGroup, _RowLeft, FontSize, GroupSpacing)
                While FeatureAllocator.IntersectsWith(_FeatureModel)
                    _Height -= FontSize
                    _FeatureModel = New FeatureSequenceViewModel(StartIndex, iRegion.Start, iRegion.End, _Height, feature,
                                                                GeneFile, NucleotidesPerRow, NucleotidesPerGroup, _RowLeft, FontSize, GroupSpacing)
                End While
                FeatureAllocator.Add(_FeatureModel)
                _Features.Add(_FeatureModel)
            Next
        Next
        Dim _Offset As System.Windows.Vector

        Dim Allocator = NucleotideAllocator Or EnzymeAllocator Or FeatureAllocator
        'If _Features.Any Then
        _Offset = New System.Windows.Vector(0.0#, Allocator.Offset.Y)
        'Else
        '_Offset = New System.Windows.Vector(0.0#, -FeatureStartHeight)
        'End If

        ApplyOffset(_Offset)
        Dim _DesiredSize = New Size(_RowLeft + NucleotidesPerRow * FontSize * WPFSequenceView.SequenceFontFactor + ((NucleotidesPerRow \ NucleotidesPerGroup) + 2.0#) * GroupSpacing, _Offset.Y + FontSize * 3.0#)
        SetValue(DesiredSizePropertyKey, _DesiredSize)
        SetValue(IsLoadedPropertyKey, True)
    End Sub

    Private Sub CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs)
        Dim _Models = GeometryModels
        If _Models Is Nothing Then Return
        If TypeOf e.OldItems Is IEnumerable Then
            For Each item In e.OldItems
                _Models.Remove(item)
            Next
        End If
        If TypeOf e.NewItems Is IEnumerable Then
            For Each item In e.NewItems
                _Models.Add(item)
            Next
        End If
    End Sub
    'WPFSequenceRow -> ForwardSequence As ObservableCollection(Of NucleotideViewModel) Default: Nothing
    Public ReadOnly Property ForwardSequence As ObservableCollection(Of NucleotideViewModel)
        Get
            Return GetValue(WPFSequenceGeometryRow.ForwardSequenceProperty)
        End Get
    End Property
    Private Shared ReadOnly ForwardSequencePropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("ForwardSequence", _
                              GetType(ObservableCollection(Of NucleotideViewModel)), GetType(WPFSequenceGeometryRow), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly ForwardSequenceProperty As DependencyProperty = _
                             ForwardSequencePropertyKey.DependencyProperty
    'WPFSequenceRow -> ComplementSequence As ObservableCollection(Of NucleotideViewModel) Default: Nothing
    Public ReadOnly Property ComplementSequence As ObservableCollection(Of NucleotideViewModel)
        Get
            Return GetValue(WPFSequenceGeometryRow.ComplementSequenceProperty)
        End Get
    End Property
    Private Shared ReadOnly ComplementSequencePropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("ComplementSequence", _
                              GetType(System.Collections.ObjectModel.ObservableCollection(Of NucleotideViewModel)), GetType(WPFSequenceGeometryRow), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly ComplementSequenceProperty As DependencyProperty = _
                             ComplementSequencePropertyKey.DependencyProperty

    'WPFSequenceRow -> ForwardPrimer As ObservableCollection(Of NucleotideViewModel) Default: Nothing
    Public ReadOnly Property ForwardPrimer As ObservableCollection(Of NucleotideViewModel)
        Get
            Return GetValue(WPFSequenceGeometryRow.ForwardPrimerProperty)
        End Get
    End Property
    Private Shared ReadOnly ForwardPrimerPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("ForwardPrimer", _
                              GetType(ObservableCollection(Of NucleotideViewModel)), GetType(WPFSequenceGeometryRow), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly ForwardPrimerProperty As DependencyProperty = _
                             ForwardPrimerPropertyKey.DependencyProperty

    'WPFSequenceRow -> ReversePrimer As ObservableCollection(Of NucleotideViewModel) Default: Nothing
    Public ReadOnly Property ReversePrimer As ObservableCollection(Of NucleotideViewModel)
        Get
            Return GetValue(WPFSequenceGeometryRow.ReversePrimerProperty)
        End Get
    End Property
    Private Shared ReadOnly ReversePrimerPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("ReversePrimer", _
                              GetType(ObservableCollection(Of NucleotideViewModel)), GetType(WPFSequenceGeometryRow), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly ReversePrimerProperty As DependencyProperty = _
                             ReversePrimerPropertyKey.DependencyProperty

    'WPFSequenceRow -> Enzymes As ObservableCollection(of EnzymeSequenceViewModel) Default: Nothing
    Public ReadOnly Property Enzymes As ObservableCollection(Of EnzymeSequenceViewModel)
        Get
            Return GetValue(WPFSequenceGeometryRow.EnzymesProperty)
        End Get
    End Property
    Private Shared ReadOnly EnzymesPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Enzymes", _
                              GetType(ObservableCollection(Of EnzymeSequenceViewModel)), GetType(WPFSequenceGeometryRow), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly EnzymesProperty As DependencyProperty = _
                             EnzymesPropertyKey.DependencyProperty
    'WPFSequenceRow -> Features As ObservableCollection(Of FeatureSequenceViewModel) Default: Nothing
    Public ReadOnly Property Features As ObservableCollection(Of FeatureSequenceViewModel)
        Get
            Return GetValue(WPFSequenceGeometryRow.FeaturesProperty)
        End Get
    End Property
    Private Shared ReadOnly FeaturesPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Features", _
                              GetType(ObservableCollection(Of FeatureSequenceViewModel)), GetType(WPFSequenceGeometryRow), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly FeaturesProperty As DependencyProperty = _
                             FeaturesPropertyKey.DependencyProperty

    'WPFSequenceRow -> Index As Errisy.FormatedTextViewModel Default: Nothing
    Public ReadOnly Property Index As Errisy.FormatedTextViewModel
        Get
            Return GetValue(WPFSequenceGeometryRow.IndexProperty)
        End Get
    End Property
    Private Shared ReadOnly IndexPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Index", _
                              GetType(Errisy.FormatedTextViewModel), GetType(WPFSequenceGeometryRow), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly IndexProperty As DependencyProperty = _
                             IndexPropertyKey.DependencyProperty




End Class
