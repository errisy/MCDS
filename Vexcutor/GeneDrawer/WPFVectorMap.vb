Imports System.Windows
Imports System.Windows.Media
Imports System.Windows.Input

Public Class WPFVectorMap
    Inherits Errisy.GeometryContainer

    'WPFVectorMap->IsAnalyzing As Boolean Default: False
    Public Property IsAnalyzing As Boolean
        Get
            Return GetValue(IsAnalyzingProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsAnalyzingProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsAnalyzingProperty As DependencyProperty = _
                           DependencyProperty.Register("IsAnalyzing", _
                           GetType(Boolean), GetType(WPFVectorMap), _
                           New PropertyMetadata(False))
    'WPFVectorMap->AnalysisProgress As Double Default: 0#
    Public Property AnalysisProgress As Double
        Get
            Return GetValue(AnalysisProgressProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(AnalysisProgressProperty, value)
        End Set
    End Property
    Public Shared ReadOnly AnalysisProgressProperty As DependencyProperty = _
                           DependencyProperty.Register("AnalysisProgress", _
                           GetType(Double), GetType(WPFVectorMap), _
                           New PropertyMetadata(0.0#))
    'WPFVectorMap->AnalysisStatus As String Default: ""
    Public Property AnalysisStatus As String
        Get
            Return GetValue(AnalysisStatusProperty)
        End Get
        Set(ByVal value As String)
            SetValue(AnalysisStatusProperty, value)
        End Set
    End Property
    Public Shared ReadOnly AnalysisStatusProperty As DependencyProperty = _
                           DependencyProperty.Register("AnalysisStatus", _
                           GetType(String), GetType(WPFVectorMap), _
                           New PropertyMetadata(""))
    'WPFVectorMap->PixelsPerKbp As Double with Event Default: 100.0#
    Public Property PixelsPerKbp As Double
        Get
            Return GetValue(PixelsPerKbpProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(PixelsPerKbpProperty, value)
        End Set
    End Property
    Public Shared ReadOnly PixelsPerKbpProperty As DependencyProperty = _
                           DependencyProperty.Register("PixelsPerKbp", _
                           GetType(Double), GetType(WPFVectorMap), _
                           New PropertyMetadata(100.0#, New PropertyChangedCallback(AddressOf SharedAnalyzeGeneFile)))
    'WPFVectorMap->SingleCutBrush As Brush Default: Brushes.Brown
    Public Property SingleCutBrush As Brush
        Get
            Return GetValue(SingleCutBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(SingleCutBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SingleCutBrushProperty As DependencyProperty = _
                           DependencyProperty.Register("SingleCutBrush", _
                           GetType(Brush), GetType(WPFVectorMap), _
                           New PropertyMetadata(Brushes.Brown, New PropertyChangedCallback(AddressOf SharedAnalyzeGeneFile)))
    'WPFVectorMap->MultipleCutBrush As Brush Default: Brushes.Purple
    Public Property MultipleCutBrush As Brush
        Get
            Return GetValue(MultipleCutBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(MultipleCutBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly MultipleCutBrushProperty As DependencyProperty = _
                           DependencyProperty.Register("MultipleCutBrush", _
                           GetType(Brush), GetType(WPFVectorMap), _
                           New PropertyMetadata(Brushes.Purple, New PropertyChangedCallback(AddressOf SharedAnalyzeGeneFile)))

    'WPFVectorMap->FontSize As Double Default: 12#
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
                           GetType(Double), GetType(WPFVectorMap), _
                           New PropertyMetadata(12.0#, New PropertyChangedCallback(AddressOf SharedAnalyzeGeneFile)))
    'WPFVectorMap->ArrowWidth As Double with Event Default: 20#
    Public Property ArrowWidth As Double
        Get
            Return GetValue(ArrowWidthProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(ArrowWidthProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ArrowWidthProperty As DependencyProperty = _
                           DependencyProperty.Register("ArrowWidth", _
                           GetType(Double), GetType(WPFVectorMap), _
                           New PropertyMetadata(20.0#, New PropertyChangedCallback(AddressOf SharedAnalyzeGeneFile)))
    'WPFVectorMap->GeneFile As Nuctions.GeneFile with Event Default: Nothing
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
                           GetType(Nuctions.GeneFile), GetType(WPFVectorMap), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedAnalyzeGeneFile)))

    'WPFVectorMap->RestrictionEnzymes As System.Collections.ObjectModel.ObservableCollection(Of Nuctions.RestrictionEnzyme) with Event Default: Nothing
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
                           GetType(List(Of String)), GetType(WPFVectorMap), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedAnalyzeGeneFile)))
    Private Shared Sub SharedAnalyzeGeneFile(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, WPFVectorMap).AnalyzeGeneFile()
    End Sub
    Private Sub AnalyzeGeneFile()

        Dim _GeneFile As Nuctions.GeneFile = GeneFile

        Dim FeatureAllocator As New Errisy.GeometryAllocator

        Dim _BackboneModel As Errisy.GeometryViewModel

        Dim _PixelsPerKbp As Double = PixelsPerKbp
        Dim _ArrowWidth As Double = ArrowWidth
        Dim _QuarterArrowWidth As Double = _ArrowWidth / 4.0#
        Dim MaxLevel As Double = 0.0#

        _AllocationViewModels = New Errisy.AllocationViewModelCollection
        _FeatureViewModels = New System.Collections.ObjectModel.ObservableCollection(Of GeneFeatureViewModel)

        Dim _SequenceLength As Double = _GeneFile.Length

        Dim _BackboneRadius As Double = _GeneFile.Length / 500.0# * _PixelsPerKbp / Math.PI

        'Generate Arrows
        If _GeneFile.Iscircular Then
            _BackboneModel = New CircleBackboneGeometry(_BackboneRadius, _ArrowWidth) With {.ToolTip =
                                                  New WPFToolTipView With
                                                  {.Label = _GeneFile.Name,
                                                   .Type = "DNA",
                                                   .Note = String.Format("{0}bp", _GeneFile.Length)},
                                                                                            .File = _GeneFile}
            _Backbone = _BackboneModel
            _AllocationViewModels.Add(_BackboneModel)
            LinkEvents(_BackboneModel)

            For Each _Feature In _GeneFile.Features
                Dim _SweepStart, _SweepEnd As Double
                Dim _SweepDirection As Boolean
                If _Feature.Complement Then
                    _SweepDirection = False
                    _SweepStart = _Feature.EndPosition / _SequenceLength * 360.0#
                    _SweepEnd = (_Feature.StartPosition - 1) / _SequenceLength * 360.0#
                Else
                    _SweepDirection = True
                    _SweepStart = (_Feature.StartPosition - 1) / _SequenceLength * 360.0#
                    _SweepEnd = _Feature.EndPosition / _SequenceLength * 360.0#
                End If
                Dim Level As Double = 0.0#
                Dim _FeatureModel = New ArcArrowGeometryViewModel(_BackboneRadius + _ArrowWidth * Level, _ArrowWidth, _SweepStart, _SweepEnd, _SweepDirection)
                While FeatureAllocator.IntersectsWith(_FeatureModel)
                    Level += 1.0#
                    _FeatureModel = New ArcArrowGeometryViewModel(_BackboneRadius + _ArrowWidth * Level, _ArrowWidth, _SweepStart, _SweepEnd, _SweepDirection)
                End While
                MaxLevel = Math.Max(Level, MaxLevel)
                _FeatureModel.ToolTip = New WPFToolTipView With
                                                  {.Label = _Feature.Label,
                                                   .Type = _Feature.Type,
                                                   .Note = _Feature.Note}
                _FeatureModel.File = _GeneFile
                _FeatureModel.Feature = _Feature
                FeatureAllocator.Add(_FeatureModel)
                _FeatureViewModels.Add(_FeatureModel)
                _AllocationViewModels.Add(_FeatureModel)
                LinkEvents(_FeatureModel)
                _FeatureModel.Stroke = Brushes.Black
                _FeatureModel.Fill = Brushes.Red
                _FeatureModel.StrokeThickness = 1.0#
            Next
        Else

            _BackboneModel = New BackboneGeometry(_BackboneRadius, _ArrowWidth) With {.ToolTip =
                                                  New WPFToolTipView With
                                                  {.Label = _GeneFile.Name,
                                                   .Type = "DNA",
                                                   .Note = String.Format("{0}bp", _GeneFile.Length)},
                                                                                            .File = _GeneFile}
            _Backbone = _BackboneModel
            _AllocationViewModels.Add(_BackboneModel)
            LinkEvents(_BackboneModel)

            For Each _Feature In _GeneFile.Features
                Dim _SweepStart, _SweepEnd As Double
                Dim _SweepDirection As Boolean
                If _Feature.Complement Then
                    _SweepDirection = False
                    _SweepStart = _Feature.EndPosition / _SequenceLength * 360.0#
                    _SweepEnd = (_Feature.StartPosition - 1) / _SequenceLength * 360.0#
                Else
                    _SweepDirection = True
                    _SweepStart = (_Feature.StartPosition - 1) / _SequenceLength * 360.0#
                    _SweepEnd = _Feature.EndPosition / _SequenceLength * 360.0#
                End If
                Dim Level As Double = 0.0#
                Dim _FeatureModel = New ArrowGeometryViewModel(_BackboneRadius, -_ArrowWidth * Level, _ArrowWidth, _SweepStart, _SweepEnd, _SweepDirection)
                While FeatureAllocator.IntersectsWith(_FeatureModel)
                    Level += 1.0#
                    _FeatureModel = New ArrowGeometryViewModel(_BackboneRadius, -_ArrowWidth * Level, _ArrowWidth, _SweepStart, _SweepEnd, _SweepDirection)
                End While
                MaxLevel = Math.Max(Level, MaxLevel)
                _FeatureModel.ToolTip = New WPFToolTipView With
                                                  {.Label = _Feature.Label,
                                                   .Type = _Feature.Type,
                                                   .Note = _Feature.Note}
                _FeatureModel.File = _GeneFile
                _FeatureModel.Feature = _Feature
                FeatureAllocator.Add(_FeatureModel)
                _FeatureViewModels.Add(_FeatureModel)
                _AllocationViewModels.Add(_FeatureModel)
                LinkEvents(_FeatureModel)
                _FeatureModel.Stroke = Brushes.Black
                _FeatureModel.Fill = Brushes.Red
                _FeatureModel.StrokeThickness = 1.0#
            Next
        End If

        'Add Restriction Enzymes
        Dim _RestrictionEnzymes = RestrictionEnzymes
        _EnzymeViewModels = New System.Collections.ObjectModel.ObservableCollection(Of GeneEnzymeViewModel)
        If _RestrictionEnzymes IsNot Nothing Then
            Dim _EnzymeList As New List(Of String)
            Dim ear As Nuctions.EnzymeAnalysis.EnzymeAnalysisResult

            Dim _EnzymeCutList As New List(Of RestrictionEnzymeCut)

            For Each _RestrictionEnzyme In RestrictionEnzymes
                _EnzymeList.Clear()
                _EnzymeList.Add(_RestrictionEnzyme)
                ear = New Nuctions.EnzymeAnalysis.EnzymeAnalysisResult(_EnzymeList, _GeneFile)
                For Each _EnzymeAnalysis As Nuctions.EnzymeAnalysis In ear
                    Dim _EnzymeCut = New RestrictionEnzymeCut
                    _EnzymeCut.SingleCut = (ear.Count = 1)
                    _EnzymeCut.EnzymeAnalysis = _EnzymeAnalysis
                    _EnzymeCut.Degree = _EnzymeAnalysis.StartRec / _SequenceLength * 360.0#
                    _EnzymeCutList.Add(_EnzymeCut)
                Next
            Next
            _EnzymeCutList.Sort()
            For Each _EnzymeCutResult In _EnzymeCutList
                If _GeneFile.Iscircular Then
                    Dim _SweepStart = _EnzymeCutResult.EnzymeAnalysis.StartRec / _SequenceLength * 360.0#
                    Dim _SweepEnd = _EnzymeCutResult.EnzymeAnalysis.EndRec / _SequenceLength * 360.0#
                    Dim _RestrictionEnzymeModel = New ArcEnzymeSiteViewModel(
                                                  _BackboneRadius, _ArrowWidth, _SweepStart, _SweepEnd, _SweepEnd > _SweepStart) With
                                              {.ToolTip =
                                                  New WPFToolTipView With
                                                  {.Label = _EnzymeCutResult.EnzymeAnalysis.nEnzyme.Name,
                                                   .Type = "Restriction Enzyme",
                                                   .Note = String.Format("Cut at {0}/{1}", _EnzymeCutResult.EnzymeAnalysis.SCut, _EnzymeCutResult.EnzymeAnalysis.ACut)},
                                               .EnzymeAnalysis = _EnzymeCutResult.EnzymeAnalysis,
                                               .IsSingleCut = _EnzymeCutResult.SingleCut}
                    _EnzymeViewModels.Add(_RestrictionEnzymeModel)
                    _AllocationViewModels.Add(_RestrictionEnzymeModel)
                Else
                    Dim _SweepStart = _EnzymeCutResult.EnzymeAnalysis.StartRec / _SequenceLength * 360.0#
                    Dim _SweepEnd = _EnzymeCutResult.EnzymeAnalysis.EndRec / _SequenceLength * 360.0#
                    Dim _RestrictionEnzymeModel = New EnzymeSiteViewModel(
                                                  _BackboneRadius, _ArrowWidth, _SweepStart, _SweepEnd, _SweepEnd > _SweepStart) With
                                              {.ToolTip =
                                                  New WPFToolTipView With
                                                  {.Label = _EnzymeCutResult.EnzymeAnalysis.nEnzyme.Name,
                                                   .Type = "Restriction Enzyme",
                                                   .Note = String.Format("Cut at {0}/{1}", _EnzymeCutResult.EnzymeAnalysis.SCut, _EnzymeCutResult.EnzymeAnalysis.ACut)},
                                               .EnzymeAnalysis = _EnzymeCutResult.EnzymeAnalysis,
                                               .IsSingleCut = _EnzymeCutResult.SingleCut}
                    _EnzymeViewModels.Add(_RestrictionEnzymeModel)
                    _AllocationViewModels.Add(_RestrictionEnzymeModel)
                End If
            Next
        End If

        'Add Labels
        Dim _FontSize As Double = FontSize
        '>>> File Name
        _BackboneLabel = New BackboneLabelViewModel With {.FontWeight = FontWeights.Bold, .FontSize = _FontSize + 4.0#, .Text = _GeneFile.Name}
        Dim _BackboneLabelSize = _BackboneLabel.HighlightGeometry.Bounds.Size
        If _GeneFile.Iscircular Then
            _BackboneLabel.Location = New Point(-_BackboneLabelSize.Width / 2.0#, -_BackboneLabelSize.Height / 2.0#)
        Else
            _BackboneLabel.Location = New Point(-_BackboneLabelSize.Width / 2.0#, _ArrowWidth)
        End If
        _AllocationViewModels.Add(_BackboneLabel)

        '>>> Feature Labels
        Dim _LeftTopLabelList As New List(Of GeneLabelViewModel)
        Dim _LeftBottomLabelList As New List(Of GeneLabelViewModel)
        Dim _RightTopLabelList As New List(Of GeneLabelViewModel)
        Dim _RightBottomLabelList As New List(Of GeneLabelViewModel)

        For Each _Feature In _FeatureViewModels
            Dim _FeatureLabel As New FeatureLabelViewModel With
                {.Text = _Feature.Feature.Label,
                 .File = _GeneFile,
                 .Feature = _Feature.Feature,
                 .FontSize = _FontSize,
                 .Angle = _Feature.CenterAngle,
                 .StartPoint = _Feature.Center}

            Dim _Cycle = _FeatureLabel.Angle / 360.0#
            _FeatureLabel.Angle = 360.0# * (_Cycle - Math.Floor(_Cycle))
            If _FeatureLabel.Angle < 90.0# Then
                _RightTopLabelList.Add(_FeatureLabel)
            ElseIf _FeatureLabel.Angle < 180.0# Then
                _RightBottomLabelList.Add(_FeatureLabel)
            ElseIf _FeatureLabel.Angle < 270.0# Then
                _LeftBottomLabelList.Add(_FeatureLabel)
            Else
                _LeftTopLabelList.Add(_FeatureLabel)
            End If
        Next
        '>>> Enzyme Labels
        Dim _SingleCutBrush = SingleCutBrush
        Dim _MultipleCutBrush = MultipleCutBrush
        For Each _Enzyme As GeneEnzymeViewModel In _EnzymeViewModels
            Dim _EnzymeLabel As New EnzymeLabelViewModel With {
                .Text = _Enzyme.EnzymeAnalysis.Enzyme.Name,
                .EnzymeAnalysis = _Enzyme.EnzymeAnalysis,
                .File = _Enzyme.File,
                .Angle = _Enzyme.CenterAngle,
                .StartPoint = _Enzyme.Center,
                .FontSize = _FontSize,
                .Fill = IIf(_Enzyme.IsSingleCut, _SingleCutBrush, _MultipleCutBrush),
                .IsSingleCut = _Enzyme.IsSingleCut}
            Dim _Cycle = _EnzymeLabel.Angle / 360.0#
            _EnzymeLabel.Angle = 360.0# * (_Cycle - Math.Floor(_Cycle))
            If _EnzymeLabel.Angle < 90.0# Then
                _RightTopLabelList.Add(_EnzymeLabel)
            ElseIf _EnzymeLabel.Angle < 180.0# Then
                _RightBottomLabelList.Add(_EnzymeLabel)
            ElseIf _EnzymeLabel.Angle < 270.0# Then
                _LeftBottomLabelList.Add(_EnzymeLabel)
            Else
                _LeftTopLabelList.Add(_EnzymeLabel)
            End If
        Next
        _LeftTopLabelList.Sort(New GeneLabelComparer(False))
        _LeftBottomLabelList.Sort(New GeneLabelComparer(True))
        _RightTopLabelList.Sort(New GeneLabelComparer(True))
        _RightBottomLabelList.Sort(New GeneLabelComparer(False))

        '>>> Allocate Labels
        Dim LabelAllocator As New Errisy.GeometryAllocator
        Dim _LabelRadius As Double = _BackboneRadius + (MaxLevel + 5.0#) * _ArrowWidth
        Dim _LabelHeight As Double = -(MaxLevel + 5.0#) * _ArrowWidth
        For Each _GeneLabelViewModel In _LeftTopLabelList
            Dim _Location As Point
            Dim _Size As Size = _GeneLabelViewModel.Bounds.Size
            If _GeneFile.Iscircular Then
                _Location = GeometryMethods.RadiusDegreeToPoint(_LabelRadius, _GeneLabelViewModel.Angle)
                _Location = New Point(_Location.X - _Size.Width / 2.0#, _Location.Y - _Size.Height)
                _GeneLabelViewModel.Location = _Location
                While LabelAllocator.IntersectsWith(_GeneLabelViewModel)
                    _Location = New Point(_Location.X, _Location.Y - _FontSize)
                    _GeneLabelViewModel.Location = _Location
                End While
                _GeneLabelViewModel.EndPoint = New Point(_Location.X + _Size.Width / 2.0#, _Location.Y + _Size.Height)
            Else
                _Location = New Point(_BackboneRadius * (_GeneLabelViewModel.Angle / 180.0# - 1.0#) - _Size.Width / 2.0#, _LabelHeight - _Size.Height)
                _GeneLabelViewModel.Location = _Location
                While LabelAllocator.IntersectsWith(_GeneLabelViewModel)
                    _Location = New Point(_Location.X, _Location.Y - _FontSize)
                    _GeneLabelViewModel.Location = _Location
                End While
                _GeneLabelViewModel.EndPoint = New Point(_Location.X + _Size.Width / 2.0#, _Location.Y + _Size.Height)
            End If
            LabelAllocator.Add(_GeneLabelViewModel)
            _AllocationViewModels.Add(_GeneLabelViewModel)
            _AllocationViewModels.Add(New Errisy.LineViewModel With {.StartPoint = _GeneLabelViewModel.StartPoint, .EndPoint = _GeneLabelViewModel.EndPoint})
        Next
        For Each _GeneLabelViewModel In _LeftBottomLabelList
            Dim _Location As Point
            Dim _Size As Size = _GeneLabelViewModel.Bounds.Size
            If _GeneFile.Iscircular Then
                _Location = GeometryMethods.RadiusDegreeToPoint(_LabelRadius, _GeneLabelViewModel.Angle)
                _Location = New Point(_Location.X - _Size.Width / 2.0#, _Location.Y)
                _GeneLabelViewModel.Location = _Location
                While LabelAllocator.IntersectsWith(_GeneLabelViewModel)
                    _Location = New Point(_Location.X, _Location.Y + _FontSize)
                    _GeneLabelViewModel.Location = _Location
                End While
                _GeneLabelViewModel.EndPoint = New Point(_Location.X + _Size.Width / 2.0#, _Location.Y)
            Else
                _Location = New Point(_BackboneRadius * (_GeneLabelViewModel.Angle / 180.0# - 1.0#) - _Size.Width / 2.0#, _LabelHeight - _Size.Height)
                _GeneLabelViewModel.Location = _Location
                While LabelAllocator.IntersectsWith(_GeneLabelViewModel)
                    _Location = New Point(_Location.X, _Location.Y - _FontSize)
                    _GeneLabelViewModel.Location = _Location
                End While
                _GeneLabelViewModel.EndPoint = New Point(_Location.X + _Size.Width / 2.0#, _Location.Y + _Size.Height)
            End If
            LabelAllocator.Add(_GeneLabelViewModel)
            _AllocationViewModels.Add(_GeneLabelViewModel)
            _AllocationViewModels.Add(New Errisy.LineViewModel With {.StartPoint = _GeneLabelViewModel.StartPoint, .EndPoint = _GeneLabelViewModel.EndPoint})
        Next
        For Each _GeneLabelViewModel In _RightTopLabelList
            Dim _Location As Point
            Dim _Size As Size = _GeneLabelViewModel.Bounds.Size
            If _GeneFile.Iscircular Then
                _Location = GeometryMethods.RadiusDegreeToPoint(_LabelRadius, _GeneLabelViewModel.Angle)
                _Location = New Point(_Location.X - _Size.Width / 2.0#, _Location.Y - _Size.Height)
                _GeneLabelViewModel.Location = _Location
                While LabelAllocator.IntersectsWith(_GeneLabelViewModel)
                    _Location = New Point(_Location.X, _Location.Y - _FontSize)
                    _GeneLabelViewModel.Location = _Location
                End While
                _GeneLabelViewModel.EndPoint = New Point(_Location.X + _Size.Width / 2.0#, _Location.Y + _Size.Height)
            Else
                _Location = New Point(_BackboneRadius * (_GeneLabelViewModel.Angle / 180.0# - 1.0#) - _Size.Width / 2.0#, _LabelHeight - _Size.Height)
                _GeneLabelViewModel.Location = _Location
                While LabelAllocator.IntersectsWith(_GeneLabelViewModel)
                    _Location = New Point(_Location.X, _Location.Y - _FontSize)
                    _GeneLabelViewModel.Location = _Location
                End While
                _GeneLabelViewModel.EndPoint = New Point(_Location.X + _Size.Width / 2.0#, _Location.Y + _Size.Height)
            End If
            LabelAllocator.Add(_GeneLabelViewModel)
            _AllocationViewModels.Add(_GeneLabelViewModel)
            _AllocationViewModels.Add(New Errisy.LineViewModel With {.StartPoint = _GeneLabelViewModel.StartPoint, .EndPoint = _GeneLabelViewModel.EndPoint})
        Next
        For Each _GeneLabelViewModel In _RightBottomLabelList
            Dim _Location As Point
            Dim _Size As Size = _GeneLabelViewModel.Bounds.Size
            If _GeneFile.Iscircular Then
                _Location = GeometryMethods.RadiusDegreeToPoint(_LabelRadius, _GeneLabelViewModel.Angle)
                _Location = New Point(_Location.X - _Size.Width / 2.0#, _Location.Y)
                _GeneLabelViewModel.Location = _Location
                While LabelAllocator.IntersectsWith(_GeneLabelViewModel)
                    _Location = New Point(_Location.X, _Location.Y + _FontSize)
                    _GeneLabelViewModel.Location = _Location
                End While
                _GeneLabelViewModel.EndPoint = New Point(_Location.X + _Size.Width / 2.0#, _Location.Y)
            Else
                _Location = New Point(_BackboneRadius * (_GeneLabelViewModel.Angle / 180.0# - 1.0#) - _Size.Width / 2.0#, _LabelHeight - _Size.Height)
                _GeneLabelViewModel.Location = _Location
                While LabelAllocator.IntersectsWith(_GeneLabelViewModel)
                    _Location = New Point(_Location.X, _Location.Y - _FontSize)
                    _GeneLabelViewModel.Location = _Location
                End While
                _GeneLabelViewModel.EndPoint = New Point(_Location.X + _Size.Width / 2.0#, _Location.Y + _Size.Height)
            End If
            LabelAllocator.Add(_GeneLabelViewModel)
            _AllocationViewModels.Add(_GeneLabelViewModel)
            _AllocationViewModels.Add(New Errisy.LineViewModel With {.StartPoint = _GeneLabelViewModel.StartPoint, .EndPoint = _GeneLabelViewModel.EndPoint})
        Next
        'Add Connection Lines

        Dim CombinedAllocator = FeatureAllocator Or LabelAllocator

        _AllocationViewModels.ApplyOffset(CombinedAllocator.Offset)
        SetValue(DesiredSizePropertyKey, CombinedAllocator.Size)
        SetValue(GeometryModelsPropertyKey, _AllocationViewModels)
    End Sub

    Private _AllocationViewModels As Errisy.AllocationViewModelCollection
    Private _Backbone As GeneViewModel
    Private _FeatureViewModels As System.Collections.ObjectModel.ObservableCollection(Of GeneFeatureViewModel)
    Private _EnzymeViewModels As System.Collections.ObjectModel.ObservableCollection(Of GeneEnzymeViewModel)
    Private _AlignmentViewModels As Errisy.AllocationViewModelCollection
    Dim _BackboneLabel As BackboneLabelViewModel
    Private _LabelViewModels As System.Collections.ObjectModel.ObservableCollection(Of GeneLabelViewModel)
    Private _LineViewModels As Errisy.AllocationViewModelCollection
    Private Sub LinkEvents(model As GeneViewModel)
        AddHandler model.MouseDown, AddressOf FeatureMouseDown
        AddHandler model.MouseUp, AddressOf FeatureMouseUp
        AddHandler model.MouseMove, AddressOf FeatureMouseMove
        AddHandler model.MouseWheel, AddressOf FeatureMouseWheel
    End Sub
    Private Sub FeatureMouseDown(sender As GeneViewModel, e As MouseButtonEventArgs)
        For Each vm In GeometryModels
            If TypeOf vm Is GeneViewModel Then
                DirectCast(vm, GeneViewModel).IsSelected = vm Is sender
            End If
        Next
    End Sub
    Private Sub FeatureMouseUp(sender As GeneViewModel, e As MouseButtonEventArgs)

    End Sub
    Private Sub FeatureMouseMove(sender As GeneViewModel, e As MouseEventArgs)

    End Sub
    Private Sub FeatureMouseWheel(sender As GeneViewModel, e As MouseWheelEventArgs)

    End Sub
End Class
