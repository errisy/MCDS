
Imports System.Windows
Imports System.Windows.Media
Imports System.Windows.Input
Imports System.Collections.ObjectModel
Public Class WPFSequenceObjectRow
    Inherits Errisy.ObjectContainer

    Private _ForwardSequence As New ObservableCollection(Of NucleotideObjectModel)
    Private _ComplementSequence As New ObservableCollection(Of NucleotideObjectModel)
    Private _ForwardPrimer As New ObservableCollection(Of NucleotideObjectModel)
    Private _ReversePrimer As New ObservableCollection(Of NucleotideObjectModel)
    Private _Enzymes As New ObservableCollection(Of EnzymeSequenceObjectModel)
    Private _Features As New ObservableCollection(Of FeatureSequenceObjectModel)
    Private _Index As New Errisy.FormatedTextObjectModel
    'Private _Sequence As New Errisy.FormatedMultipleTextObjectModel(Of NucleotideTextModel)

    Public Sub New()
        AddHandler _ForwardSequence.CollectionChanged, AddressOf CollectionChanged
        AddHandler _ComplementSequence.CollectionChanged, AddressOf CollectionChanged
        AddHandler _ForwardPrimer.CollectionChanged, AddressOf CollectionChanged
        AddHandler _ReversePrimer.CollectionChanged, AddressOf CollectionChanged
        AddHandler _Enzymes.CollectionChanged, AddressOf CollectionChanged
        AddHandler _Features.CollectionChanged, AddressOf CollectionChanged

        Dim _Models = ObjectModels
        If _Models Is Nothing Then Return
        _Models.Add(_Index)
        '_Models.Add(_Sequence)
    End Sub
    Public Sub Initialize(StartIndex As Integer, Indent As Double, GroupSpacing As Double, FontSize As Double, NucleotidesPerRow As Integer, NucleotidesPerGroup As Integer)
        StartNucleotideIndex = StartIndex
        Dim _RowLeft As Double = FontSize * WPFSequenceView.SequenceFontFactor * 10.0# + Indent
        Dim _DesiredSize = New Size(_RowLeft + NucleotidesPerRow * FontSize * WPFSequenceView.SequenceFontFactor + ((NucleotidesPerRow \ NucleotidesPerGroup) + 2.0#) * GroupSpacing, FontSize * 5.0#)
        DesiredSize = _DesiredSize
    End Sub
    'WPFSequenceRow -> StartNucleotideIndex As Integer Default: 0
    Public Property StartNucleotideIndex As Integer

    'WPFSequenceRow -> IsLoaded As Boolean Default: False
    Public Property IsLoaded As Boolean

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
        Dim _IndexSize = _Index.Bounds.Value.Size
        _Index.Location = New Point(FontSize * WPFSequenceView.SequenceFontFactor * 10.0# - _IndexSize.Width, -FontSize)
        Dim Sequence As String = GeneFile.Sequence
        Dim _FChar As Char
        Dim _RChar As Char
        Dim Length As Integer = GeneFile.Length

        'Nucleotides
        'Dim NucleotideAllocator As New Errisy.ObjectAllocator

        Dim _RowLeft As Double = FontSize * WPFSequenceView.SequenceFontFactor * 10.0# + Indent
        Dim Nucleotide As NucleotideObjectModel

        'NucleotideAllocator.Add(New RectangleGeometry(New Rect(New Point(_RowLeft, -2.0# * FontSize),
        '                                                         New Size(_RowLeft + NucleotidesPerRow * FontSize * WPFSequenceView.SequenceFontFactor + (NucleotidesPerRow \ NucleotidesPerGroup) * GroupSpacing, 4.0# * FontSize))))

        For i As Integer = 0 To NucleotidesPerRow - 1
            If i + StartIndex <= Length Then
                _FChar = Sequence(i + StartIndex - 1)
                _RChar = Nuctions.ComplementChar(_FChar)
            Else
                _FChar = " "c
                _RChar = " "c
            End If
            Nucleotide = New NucleotideObjectModel With {.Text = _FChar, .FontSize = FontSize, .Location = New Point(_RowLeft + i * FontSize * WPFSequenceView.SequenceFontFactor + (i \ NucleotidesPerGroup) * GroupSpacing, -FontSize)}
            _ForwardSequence.Add(Nucleotide)

            Nucleotide = New NucleotideObjectModel With {.Text = _RChar, .FontSize = FontSize, .Location = New Point(_RowLeft + i * FontSize * WPFSequenceView.SequenceFontFactor + (i \ NucleotidesPerGroup) * GroupSpacing, 0.0#)}
            _ComplementSequence.Add(Nucleotide)

            Nucleotide = New NucleotideObjectModel With {.FontSize = FontSize, .Location = New Point(_RowLeft + i * FontSize * WPFSequenceView.SequenceFontFactor + (i \ NucleotidesPerGroup) * GroupSpacing, -2.0# * FontSize), .Fill = Brushes.Red}
            _ForwardPrimer.Add(Nucleotide)

            Nucleotide = New NucleotideObjectModel With {.FontSize = FontSize, .Location = New Point(_RowLeft + i * FontSize * WPFSequenceView.SequenceFontFactor + (i \ NucleotidesPerGroup) * GroupSpacing, FontSize), .Fill = Brushes.Red}
            _ReversePrimer.Add(Nucleotide)
        Next


        'Allocators
        Dim EnzymeAllocator As New RowSets
        Dim FeatureAllocator As New RowSets
        Dim IsCircular As Boolean = GeneFile.Iscircular

        Dim RegionList As List(Of IntegerRegion)

        Dim EnzymeStartHeight As Double = -3.0# * FontSize

        For Each cut In EnzymeCuts
            RegionList = IntegerRegion.FindRegions(IsCircular, False, cut.StartRec, cut.EndRec, StartIndex, NucleotidesPerRow, Length)
            For Each iRegion In RegionList
                Dim _Level As Integer = EnzymeAllocator.Fit(New IntegerRegion With {.Start = iRegion.Start, .End = iRegion.End})
                Dim _EnzymeModel As New EnzymeSequenceObjectModel(StartIndex, iRegion.Start, iRegion.End, EnzymeStartHeight - _Level * FontSize, cut,
                                                                GeneFile, NucleotidesPerRow, NucleotidesPerGroup, _RowLeft, FontSize, GroupSpacing)
                'While EnzymeAllocator.IntersectsWith(_EnzymeModel)
                '    _Height -= FontSize
                '    _EnzymeModel = New EnzymeSequenceObjectModel(StartIndex, iRegion.Start, iRegion.End, _Height, cut,
                '                                                GeneFile, NucleotidesPerRow, NucleotidesPerGroup, _RowLeft, FontSize, GroupSpacing)
                'End While
                'EnzymeAllocator.Add(_EnzymeModel)
                _Enzymes.Add(_EnzymeModel)
            Next
        Next

        Dim FeatureStartHeight As Double = EnzymeStartHeight - FontSize * FeatureAllocator.Count

        For Each feature In GeneFile.Features
            RegionList = IntegerRegion.FindRegions(IsCircular, feature.Complement, feature.StartPosition, feature.EndPosition, StartIndex, NucleotidesPerRow, Length)

            For Each iRegion In RegionList
                Dim _Level As Integer = FeatureAllocator.Fit(New IntegerRegion With {.Start = iRegion.Start, .End = iRegion.End})

                Dim _FeatureModel As New FeatureSequenceObjectModel(StartIndex, iRegion.Start, iRegion.End, FeatureStartHeight - _Level * FontSize, feature,
                                                                GeneFile, NucleotidesPerRow, NucleotidesPerGroup, _RowLeft, FontSize, GroupSpacing)
                'While FeatureAllocator.IntersectsWith(_FeatureModel)
                '    _Height -= FontSize
                '    _FeatureModel = New FeatureSequenceObjectModel(StartIndex, iRegion.Start, iRegion.End, _Height, feature,
                '                                                GeneFile, NucleotidesPerRow, NucleotidesPerGroup, _RowLeft, FontSize, GroupSpacing)
                'End While
                'FeatureAllocator.Add(_FeatureModel)
                _Features.Add(_FeatureModel)
            Next
        Next
        Dim _Offset As System.Windows.Vector

        'Dim Allocator = NucleotideAllocator Or EnzymeAllocator Or FeatureAllocator
        'If _Features.Any Then
        _Offset = New System.Windows.Vector(0.0#, -FeatureStartHeight + FeatureAllocator.Count * FontSize)
        'Else
        '_Offset = New System.Windows.Vector(0.0#, -FeatureStartHeight)
        'End If

        ApplyOffset(_Offset)
        Dim _DesiredSize = New Size(_RowLeft + NucleotidesPerRow * FontSize * WPFSequenceView.SequenceFontFactor + ((NucleotidesPerRow \ NucleotidesPerGroup) + 2.0#) * GroupSpacing, _Offset.Y + FontSize * 3.0#)
        DesiredSize = _DesiredSize
        IsLoaded = True
    End Sub

    Private Sub CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs)
        Dim _Models = ObjectModels
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
    Public ReadOnly Property ForwardSequence As ObservableCollection(Of NucleotideObjectModel)
        Get
            Return _ForwardSequence
        End Get
    End Property

    'WPFSequenceRow -> ComplementSequence As ObservableCollection(Of NucleotideViewModel) Default: Nothing
    Public ReadOnly Property ComplementSequence As ObservableCollection(Of NucleotideObjectModel)
        Get
            Return _ComplementSequence
        End Get
    End Property


    'WPFSequenceRow -> ForwardPrimer As ObservableCollection(Of NucleotideViewModel) Default: Nothing
    Public ReadOnly Property ForwardPrimer As ObservableCollection(Of NucleotideObjectModel)
        Get
            Return _ForwardPrimer
        End Get
    End Property


    'WPFSequenceRow -> ReversePrimer As ObservableCollection(Of NucleotideViewModel) Default: Nothing
    Public ReadOnly Property ReversePrimer As ObservableCollection(Of NucleotideObjectModel)
        Get
            Return _ReversePrimer
        End Get
    End Property


    'WPFSequenceRow -> Enzymes As ObservableCollection(of EnzymeSequenceViewModel) Default: Nothing
    Public ReadOnly Property Enzymes As ObservableCollection(Of EnzymeSequenceObjectModel)
        Get
            Return _Enzymes
        End Get
    End Property

    'WPFSequenceRow -> Features As ObservableCollection(Of FeatureSequenceViewModel) Default: Nothing
    Public ReadOnly Property Features As ObservableCollection(Of FeatureSequenceObjectModel)
        Get
            Return _Features
        End Get
    End Property


    'WPFSequenceRow -> Index As Errisy.FormatedTextViewModel Default: Nothing
    Public ReadOnly Property Index As Errisy.FormatedTextObjectModel
        Get
            Return _Index
        End Get
    End Property

End Class

