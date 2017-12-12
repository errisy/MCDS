
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Windows

<Serializable>
Public Class GoldenGateDesignViewModel
    Implements INotifyPropertyChanged
    Private _DNAInfo As DNAInfo
    Private _View As OperationView
    Public PropertyControlView As PropertyControl
    Public Sub New(_info As DNAInfo)
        _DNAInfo = _info
        _View = _info.GetParetntChartItem.Parent
        Dim newList As New List(Of DNAInfo)
        newList.AddRange(_DNAInfo.Source)
        Dim existingList As New HashSet(Of DNAInfo)
        For Each item In _DNAInfo.GoldenGateDesignInfos.ToArray
            If _DNAInfo.Source.Contains(item.SourceNode) Then
                existingList.Add(item.SourceNode)
                _Source.Add(New GoldenGateViewModel(item.SourceNode, Me, item))
            Else
                _DNAInfo.GoldenGateDesignInfos.Remove(item)
            End If
        Next
        For Each dInfo In existingList
            newList.Remove(dInfo)
        Next
        For Each scr In _DNAInfo.Source
            If Not existingList.Contains(scr) Then
                Dim _NewNode As New GoldenGateDesignInfo With {.SourceNode = scr}
                _Source.Add(New GoldenGateViewModel(scr, Me, _NewNode)) '_Source已经被监视 所以不要用额外的代码来管理
            End If
        Next
        'need to find a way to search existing matches.
    End Sub
    Public Sub ReloadDNAInfo(dna As DNAInfo)
        _View.Reload(dna)
    End Sub
    Public Sub DrawOperationView()
        _View.Draw()
    End Sub
    Public ReadOnly Property Optimize As New ViewModelCommand(AddressOf cmdOptimize)
    Public Sub cmdOptimize()
        Dim stbError As New System.Text.StringBuilder
        Dim HasError As Boolean = False
        For Each node In _Source
            node.ClearHeadAttach()
            node.ClearTailAttach()
            node.SourceDNAInfo.Calculate()
            ReloadDNAInfo(node.SourceDNAInfo)
            If node.SourceDNAInfo.DNAs.Count = 0 Then stbError.AppendLine(String.Format("Source Node #{0} is Empty.", node.NodeID)) : HasError = True
            If node.IsPCR Then
                If node.SourceDNAInfo.PCR_ForwardPrimer Is Nothing OrElse node.SourceDNAInfo.PCR_ForwardPrimer.Length < 10 Then
                    stbError.AppendLine(String.Format("Source Node #{0} has Invalid Forward PCR Primer.", node.NodeID)) : HasError = True
                End If
                If node.SourceDNAInfo.PCR_ReversePrimer Is Nothing OrElse node.SourceDNAInfo.PCR_ForwardPrimer.Length < 10 Then
                    stbError.AppendLine(String.Format("Source Node #{0} has Invalid Reverse PCR Primer.", node.NodeID)) : HasError = True
                End If
            End If
        Next

        Dim rList As New List(Of Nuctions.GeneFile)
        If HasError Then
            MsgBox(stbError.ToString, MsgBoxStyle.OkOnly, "GoldenGate Optimization Error")
        Else
            Dim ThisNode As GoldenGateViewModel
            Dim NextNode As GoldenGateViewModel
            Dim cnt As Integer = _Source.Count
            For i As Integer = 0 To cnt - 1
                ThisNode = _Source(i)
                NextNode = _Source((i + 1) Mod cnt)
                GoldenGateViewModel.BuildCompatibleSite(ThisNode, NextNode)
            Next
            Dim gList As New List(Of Nuctions.GeneFile)
            For Each scr In _Source
                For Each dna In scr.SourceDNAInfo.DNAs
                    gList.Add(dna)
                Next
            Next
            For Each node In _Source
                node.AnalyzePrimerPair()
            Next
            rList = Nuctions.Recombination(gList, "in vivo Annealing", True, 1)
        End If
        '如何进行优化呢？

        _DNAInfo.LoadResultDNAList(rList, True)

        DrawOperationView()
    End Sub
    Public ReadOnly Property Reset As New ViewModelCommand(AddressOf cmdReset)
    Private Sub cmdReset(value As Object)
        Dim ThisNode As GoldenGateViewModel
        Dim NextNode As GoldenGateViewModel
        Dim cnt As Integer = _Source.Count
        For Each node In _Source
            If node.IsPCR Then
                node.ClearHeadAttach()
                node.ClearTailAttach()
                node.CalculateForwardTm()
                node.CalculateReverseTm()
                node.AnalyzePrimerPair()
                node.SourceDNAInfo.Calculate()
                ReloadDNAInfo(node.SourceDNAInfo)
            End If
        Next
        For i As Integer = 0 To cnt - 1
            ThisNode = _Source(i)
            NextNode = _Source((i + 1) Mod cnt)
            'Dim xy = Nuctions.FacingSequenceAnnealingSearch(ThisNode.Tail, NextNode.Head, 12, 250)
            'ThisNode.SetTail(xy.Key)
            'NextNode.SetHead(xy.Value)
        Next
        _DNAInfo.LoadResultDNAList(New List(Of Nuctions.GeneFile))
        _DNAInfo.GetParetntChartItem.Reload(_DNAInfo, _View.EnzymeCol)
        If PropertyControlView IsNot Nothing Then PropertyControlView.RefreshDNAView()
        DrawOperationView()
    End Sub
    Private WithEvents _Source As New ObjectModel.ObservableCollection(Of GoldenGateViewModel)
    Public ReadOnly Property Source As ObjectModel.ObservableCollection(Of GoldenGateViewModel)
        Get
            Return _Source
        End Get
    End Property
    Private Sub _Source_CollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs) Handles _Source.CollectionChanged
        _DNAInfo.GoldenGateDesignInfos.Clear()
        For Each it In _Source
            _DNAInfo.GoldenGateDesignInfos.Add(it.DesignInfo)
        Next
    End Sub
    Private _GoldenGateEnzymes As List(Of Nuctions.RestrictionEnzyme)
    Public Function GetGoldenGateEnzymes() As List(Of Nuctions.RestrictionEnzyme)
        If _GoldenGateEnzymes Is Nothing Then
            _GoldenGateEnzymes = New List(Of Nuctions.RestrictionEnzyme)
            For Each enzyme In SettingEntry.EnzymeCol.Values
                If enzyme.ACut - enzyme.SCut = 4 AndAlso enzyme.Sequence.Substring(enzyme.SCut, 4) = "NNNN" AndAlso
                (enzyme.Sequence.Substring(0, enzyme.SCut).Replace("N", "").Length = 0 OrElse enzyme.Sequence.Substring(enzyme.ACut).Replace("N", "").Length = 0) Then
                    _GoldenGateEnzymes.Add(enzyme)
                End If
            Next
        End If
        Return _GoldenGateEnzymes
    End Function
    <NonSerialized> Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class

Public Class GoldenGateEnzyme
    Public Enzyme As Nuctions.RestrictionEnzyme
    Public PatternBetween As String
End Class

<System.Windows.Markup.ContentProperty("DataTemplates")>
Public Class GoldenGateTemplateSelector
    Inherits System.Windows.Controls.DataTemplateSelector
    Public ReadOnly Property DataTemplates As New ObjectModel.ObservableCollection(Of DataTemplate)
    Public Overrides Function SelectTemplate(item As Object, container As DependencyObject) As DataTemplate
        If Not (TypeOf item Is GoldenGateViewModel) Then Return Nothing
        If _DataTemplates.Count < 2 Then Return Nothing
        Dim nvm As GoldenGateViewModel = item
        If nvm.IsPCR Then
            Return _DataTemplates(0)
        Else
            Return _DataTemplates(1)
        End If
    End Function
End Class



<Serializable>
Public Class GoldenGateDesignInfo
    Public SourceNode As DNAInfo
    Public IsForwardAssemblyDirection As Boolean
    Public ForwardOverhang As String
    Public ReverseOverhang As String
    Public ForwardAttached As String = ""
    Public ReverseAttached As String = ""
End Class

<Serializable>
Public Class GoldenGateViewModel
    Implements INotifyPropertyChanged
    Private _OperationInfo As GoldenGateDesignViewModel
    Private _GoldenGateDesignInfo As GoldenGateDesignInfo
    Public Sub New(_Source As DNAInfo, _Operation As GoldenGateDesignViewModel, _DesignInfo As GoldenGateDesignInfo)
        _Node = _Source
        _OperationInfo = _Operation
        _GoldenGateDesignInfo = _DesignInfo
        _NodeType = Nuctions.GetMolecularOperationDescription(_Source.MolecularOperation)
        _NodeID = _Source.GetParetntChartItem.Index
        _IsPCR = (Node.MolecularOperation = Nuctions.MolecularOperationEnum.PCR)
        LoadDNAInformation()
    End Sub
    Public ReadOnly Property DesignInfo As GoldenGateDesignInfo
        Get
            Return _GoldenGateDesignInfo
        End Get
    End Property
    Public ReadOnly Property SourceDNAInfo As DNAInfo
        Get
            Return _GoldenGateDesignInfo.SourceNode
        End Get
    End Property
    Public ReadOnly Property SourceGeneFile As Nuctions.GeneFile
        Get
            If _GoldenGateDesignInfo.SourceNode.DNAs.Count > 0 Then
                Return _GoldenGateDesignInfo.SourceNode.DNAs(1)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public Sub LoadDNAInformation()
        If _Node.DNAs.Count <> 1 Then
            _Warning = String.Format("{0} DNA found. Please ensure one DNA per source node.", _Node.DNAs.Count)
            Return
        ElseIf DirectCast(_Node.DNAs(1), Nuctions.GeneFile).Iscircular Then
            _Warning = "Circular DNA can't be assembled. "
            Return
        ElseIf DirectCast(_Node.DNAs(1), Nuctions.GeneFile).Length < 50 Then
            _Warning = "The DNA for assembly may be too short. "
        Else
            _Warning = ""
        End If
        Dim _GeneFile As Nuctions.GeneFile = _Node.DNAs(1)
        Dim _DNALength As Integer = _GeneFile.Length
        If _IsPCR Then
            _ForwardPrimerName = _GoldenGateDesignInfo.SourceNode.PCR_FPrimerName
            OnPropertyChanged("ForwardPrimerName")
            _ReversePrimerName = _GoldenGateDesignInfo.SourceNode.PCR_RPrimerName
            OnPropertyChanged("ReversePrimerName")
            CalculateForwardTm()
            CalculateReverseTm()
            AnalyzePrimerPair()
        Else
            Dim endF As String = _GeneFile.End_F
            Dim endR As String = _GeneFile.End_R

            Dim ForwardCompatible As Boolean = False
            Dim ReverseCompatible As Boolean = False

            Select Case endF.Substring(0, 2)
                Case "*5"
                    ForwardCompatible = True
                Case "^5"
                    _Warning += "Forward end dephosphorylated. "
                    ForwardCompatible = True
                Case Else
                    _Warning += "Forward end not compatible! Ends must be 4nt of 5' overhang. "
                    ForwardCompatible = False
            End Select
            If ForwardCompatible Then
                If endF.Length = 6 Then
                    _ForwardOverhang = endF.Substring(2, 4)
                    OnPropertyChanged("ForwardOverhang")
                    _IsForwardPalindromic = _ReverseOverhang = Nuctions.ReverseComplement(_ReverseOverhang)
                    OnPropertyChanged("IsForwardPalindromic")
                    If _IsForwardPalindromic Then _Warning += "Forward end is palindromic, please try avoid palindromic overhangs! "
                    _IsForwardCompatible = True
                    OnPropertyChanged("IsForwardCompatible")
                Else
                    _IsForwardCompatible = False
                    OnPropertyChanged("IsForwardCompatible")
                    _Warning += "Ends must be 4nt of 5' overhang. "
                End If
            End If
            Select Case endR.Substring(0, 2)
                Case "*5"
                    ReverseCompatible = True
                Case "^5"
                    _Warning += "Reverse end dephosphorylated. "
                    ReverseCompatible = True
                Case Else
                    _Warning += "Reverse end not compatible! Ends must be 4nt of 5' overhang. "
                    ReverseCompatible = False
            End Select
            If ReverseCompatible Then
                If endR.Length = 6 Then
                    _ReverseOverhang = endR.Substring(2, 4)
                    OnPropertyChanged("ReverseOverhang")
                    _IsReversePalindromic = _ReverseOverhang = Nuctions.ReverseComplement(_ReverseOverhang)
                    OnPropertyChanged("IsReversePalindromic")
                    If _IsReversePalindromic Then _Warning += "Reverse end is palindromic, please try avoid palindromic overhangs! "
                    _IsReverseCompatible = True
                    OnPropertyChanged("IsReverseCompatible")
                Else
                    _IsReverseCompatible = False
                    OnPropertyChanged("IsReverseCompatible")
                    _Warning += "Ends must be 4nt of 5' overhang. "
                End If
            End If
        End If
    End Sub
    Public Sub CalculateHeadTm()
        If IsForwardAssemblyDirection Then
            CalculateForwardTm()
        Else
            CalculateReverseTm()
        End If
    End Sub
    Public Sub CalculateTailTm()
        If IsForwardAssemblyDirection Then
            CalculateReverseTm()
        Else
            CalculateForwardTm()
        End If
    End Sub
    Public Sub CalculateForwardTm()
        _ForwardPrimerLength = _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer.Length
        _ForwardPrimerAttached = _GoldenGateDesignInfo.ForwardAttached
        If _ForwardPrimerAttached Is Nothing Then _ForwardPrimerAttached = ""
        _ForwardTm = Nuctions.CalculateTm(_GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer, Na, Concentration).Tm
        OnPropertyChanged("ForwardPrimerAttached")
        OnPropertyChanged("ForwardPrimerLength")
        OnPropertyChanged("ForwardTm")
    End Sub

    Public Sub CalculateReverseTm()
        _ReversePrimerLength = _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer.Length
        _ReversePrimerAttached = _GoldenGateDesignInfo.ReverseAttached
        If _ReversePrimerAttached Is Nothing Then _ReversePrimerAttached = ""
        _ReverseTm = Nuctions.CalculateTm(_GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer, Na, Concentration).Tm
        OnPropertyChanged("ReversePrimerAttached")
        OnPropertyChanged("ReversePrimerLength")
        OnPropertyChanged("ReverseTm")
    End Sub
    Public Sub AnalyzePrimerPair()
        If Not _IsPCR Then Return
        Static _F = "F"
        Static _R = "R"
        Dim res = Nuctions.AnalyzePrimer(New Dictionary(Of String, String) From {{_F, _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer}, {_R, _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer}}, New List(Of Nuctions.GeneFile), Na, Concentration)

        Dim fhairpins = res.Hairpins(_F)
        If fhairpins.Count > 0 Then
            _MaxForwardHairpin = fhairpins(0).AG.ToString("0.0")
        Else
            _MaxForwardHairpin = "N/A"
        End If
        Dim rhairpins = res.Hairpins(_R)
        If rhairpins.Count > 0 Then
            _MaxReverseHairpin = rhairpins(0).AG.ToString("0.0")
        Else
            _MaxReverseHairpin = "N/A"
        End If
        Dim fdimers = res.Dimers(_F)
        If fdimers.Count > 0 Then
            _MaxForwardDimer = fdimers(0).AG.ToString("0.0")
        Else
            _MaxForwardDimer = "N/A"
        End If
        Dim rdimers = res.Dimers(_R)
        If rdimers.Count > 0 Then
            _MaxReverseDimer = rdimers(0).AG.ToString("0.0")
        Else
            _MaxReverseDimer = "N/A"
        End If
        Dim cdimers = res.CrossDimers
        If cdimers.Count > 0 Then
            _MaxCrossDimer = cdimers(0).AG.ToString("0.0")
        Else
            _MaxCrossDimer = "N/A"
        End If
        OnPropertyChanged("MaxForwardHairpin")
        OnPropertyChanged("MaxForwardDimer")
        OnPropertyChanged("MaxReverseHairpin")
        OnPropertyChanged("MaxReverseDimer")
        OnPropertyChanged("MaxCrossDimer")
    End Sub

    Private Shared Na As Single = 0.08F
    Private Shared Concentration As Single = 0.000000625F
    ''' <summary>
    ''' This will return the length of binding sequence.
    ''' </summary>
    ''' <param name="_Sequence"></param>
    ''' <param name="Offset"></param>
    ''' <param name="Tm"></param>
    ''' <returns></returns>
    Private Shared Function SearchAnnealing(_Sequence As String, Offset As Integer, Tm As Single) As Integer
        Dim oligoLength As Integer = Math.Min(15, _Sequence.Length - Offset)
        Dim LenTmDict As New Dictionary(Of Integer, Nuctions.OligoInfo)
        LenTmDict.Add(oligoLength - 1, Nuctions.CalculateTm(_Sequence.Substring(Offset, oligoLength - 1), Na, Concentration))
        LenTmDict.Add(oligoLength, Nuctions.CalculateTm(_Sequence.Substring(Offset, oligoLength), Na, Concentration))
        While Not (LenTmDict(oligoLength - 1).Tm < Tm And LenTmDict(oligoLength).Tm >= Tm)
            If LenTmDict(oligoLength).Tm < Tm Then
                oligoLength += 1
                LenTmDict.Add(oligoLength, Nuctions.CalculateTm(_Sequence.Substring(Offset, oligoLength), Na, Concentration))
            ElseIf LenTmDict(oligoLength - 1).Tm >= Tm Then
                oligoLength -= 1
                LenTmDict.Add(oligoLength - 1, Nuctions.CalculateTm(_Sequence.Substring(Offset, oligoLength - 1), Na, Concentration))
            End If
        End While
        Return oligoLength
    End Function
    Public ReadOnly Property Node As DNAInfo
    Public ReadOnly Property ForwardOverhang As String
    Public ReadOnly Property ReverseOverhang As String
    Public ReadOnly Property IsForwardCompatible As Boolean
    Public ReadOnly Property IsReverseCompatible As Boolean
    Public ReadOnly Property IsForwardPalindromic As Boolean
    Public ReadOnly Property IsReversePalindromic As Boolean

    Public ReadOnly Property Warning As String
    Public ReadOnly Property NodeType As String
    Public ReadOnly Property NodeID As String
    Public ReadOnly Property ForwardPrimerName As String
    Public ReadOnly Property ReversePrimerName As String
    Public ReadOnly Property ForwardPrimerAttached As String
    Public ReadOnly Property ForwardPrimerAnnealing As String
    Public ReadOnly Property ReversePrimerAttached As String
    Public ReadOnly Property ReversePrimerAnnealing As String
    Public ReadOnly Property ForwardPrimerLength As Integer
    Public ReadOnly Property ReversePrimerLength As Integer
    Public ReadOnly Property ForwardTm As Single
    Public ReadOnly Property ReverseTm As Single
    Public ReadOnly Property MaxForwardHairpin As String
    Public ReadOnly Property MaxReverseHairpin As String
    Public ReadOnly Property MaxForwardDimer As String
    Public ReadOnly Property MaxReverseDimer As String
    Public ReadOnly Property MaxCrossDimer As String
    Public Property IsForwardAssemblyDirection As Boolean
        Get
            Return _GoldenGateDesignInfo.IsForwardAssemblyDirection
        End Get
        Set(value As Boolean)
            _GoldenGateDesignInfo.IsForwardAssemblyDirection = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("IsForwardAssemblyDirection"))
        End Set
    End Property
    Protected Sub OnPropertyChanged(e As ComponentModel.PropertyChangedEventArgs)
        RaiseEvent PropertyChanged(Me, e)
    End Sub
    Protected Sub OnPropertyChanged(PropertyName As String)
        RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs(PropertyName))
    End Sub
    Public ReadOnly Property IsPCR As Boolean
    <NonSerialized> Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

#Region "Commands"
    Public ReadOnly Property MoveUp As New ViewModelCommand(AddressOf cmdMoveUp)
    Private Sub cmdMoveUp(value As Object)
        Dim i As Integer = _OperationInfo.Source.IndexOf(Me)
        If i > 0 Then
            _OperationInfo.Source.Remove(Me)
            _OperationInfo.Source.Insert(i - 1, Me)
        End If
    End Sub
    Public ReadOnly Property MoveDown As New ViewModelCommand(AddressOf cmdMoveDown)
    Private Sub cmdMoveDown(value As Object)
        Dim i As Integer = _OperationInfo.Source.IndexOf(Me)
        If i < _OperationInfo.Source.Count Then
            _OperationInfo.Source.Remove(Me)
            _OperationInfo.Source.Insert(i + 1, Me)
        End If
    End Sub
#End Region
#Region "Annealing"
    Public Shared Sub BuildCompatibleSite(x As GoldenGateViewModel, y As GoldenGateViewModel)
        Dim xTail As GoldenGateEndInformation = x.Tail
        Dim yHead As GoldenGateEndInformation = y.Head
        If GoldenGateEndInformation.CanLigate(xTail, yHead) Then
            If Optimize(x, y) Then
                'x.SetTail(xy.Key)
                'y.SetHead(xy.Value)
            End If
        Else

        End If
    End Sub

    Private Shared Function Optimize(x As GoldenGateViewModel, y As GoldenGateViewModel) As Boolean
        'If x.IsPCR AndAlso y.IsPCR Then
        '    '可以双向优化
        '    '先找到max facing match 以便最大化利用已有的匹配

        '    Dim existingmatch As Integer = Nuctions.FindFacingTailMatch(xTail, yHead, 12)
        '    Dim ly = SearchAnnealing(yHead, y.HeadLeadingLength, MinOptimizedTm)
        '    ly = Math.Max(ly, MinOptimizedLength)
        '    Dim lx = SearchAnnealing(xTail, x.TailLeadingLength, MinOptimizedTm)
        '    lx = Math.Max(lx, MinOptimizedLength)
        '    '如何搜索最好的搭配？先生成一个从左到右的搭配
        '    'screen from x to y
        '    Dim XtoY As String = Nuctions.ReverseComplement(x.Tail.Substring(0, lx)) + y.Head.Substring(existingmatch, ly - existingmatch)
        '    Dim YtoX As String = Nuctions.ReverseComplement(XtoY)

        '    Dim MaxScore As Single = Single.MinValue
        '    Dim MinIndex As Integer
        '    Dim EmptyList As New List(Of Nuctions.GeneFile)
        '    Dim xTailPrimer As String = ""
        '    Dim yHeadPrimer As String = ""
        '    Dim xPPs As New List(Of PrimerPair)
        '    Dim yPPs As New List(Of PrimerPair)
        '    For i As Integer = 0 To lx - existingmatch
        '        Dim TotalAnnealingLength = SearchAnnealing(XtoY, i, MinOptimizedTm)
        '        TotalAnnealingLength = Math.Max(TotalAnnealingLength, MinOptimizedLength)
        '        If TotalAnnealingLength + i < lx Then Continue For
        '        If lx - existingmatch < i Then Continue For
        '        Dim xprmDict As New Dictionary(Of String, String)
        '        xprmDict.Add("XT", Nuctions.ReverseComplement(XtoY.Substring(lx, TotalAnnealingLength + i - lx)) + x.TailPrimer)
        '        xprmDict.Add("XO", x.TailOther)
        '        Dim xPNR = Nuctions.AnalyzePrimer(xprmDict, EmptyList, Na, Concentration)
        '        Dim yprmDict As New Dictionary(Of String, String)
        '        yprmDict.Add("YH", XtoY.Substring(i, lx - existingmatch - i) + y.HeadPrimer)
        '        yprmDict.Add("YO", y.HeadOther)
        '        Dim yPNR = Nuctions.AnalyzePrimer(yprmDict, EmptyList, Na, Concentration)
        '        Dim Score = (xPNR.MaxDimerEntropyScore + yPNR.MaxDimerEntropyScore + xPNR.MaxCrossDimerEntropyScore + yPNR.MaxCrossDimerEntropyScore) +
        '            (xPNR.MaxHairpinEntropyScore + yPNR.MaxHairpinEntropyScore) * 2.0F
        '        If MaxScore < Score Then
        '            MaxScore = Score
        '            MinIndex = i
        '            xTailPrimer = xprmDict("XT")
        '            yHeadPrimer = yprmDict("YH")
        '        End If
        '    Next
        '    x.SetTailAttach(xTailPrimer.Substring(0, xTailPrimer.Length - x.TailPrimer.Length))
        '    y.SetHeadAttach(yHeadPrimer.Substring(0, yHeadPrimer.Length - y.HeadPrimer.Length))
        '    x.TailPrimer = xTailPrimer
        '    y.HeadPrimer = yHeadPrimer
        '    x.SourceDNAInfo.Calculate()
        '    x._OperationInfo.ReloadDNAInfo(x.SourceDNAInfo)
        '    y.SourceDNAInfo.Calculate()
        '    y._OperationInfo.ReloadDNAInfo(y.SourceDNAInfo)
        '    x.CalculateTailTm()
        '    y.CalculateHeadTm()
        'ElseIf x.IsPCR Then
        '    'x.ClearTailAttach()
        '    '优化一头
        '    '先找到max facing match 以便最大化利用已有的匹配
        '    y.Tail()

        '    Dim existingmatch As Integer = Nuctions.FindFacingTailMatch(xTail, yHead, 12)
        '    Dim l = SearchAnnealing(yHead, y.HeadLeadingLength, MinOptimizedTm)
        '    l = Math.Max(l, MinOptimizedLength)
        '    Dim attached = Nuctions.ReverseComplement(y.Head.Substring(existingmatch, l - existingmatch))
        '    x.SetTailAttach(attached)
        '    x.TailPrimer = attached + x.TailPrimer
        '    x.SourceDNAInfo.Calculate()
        '    x._OperationInfo.ReloadDNAInfo(x.SourceDNAInfo)
        '    x.CalculateTailTm()
        'ElseIf y.IsPCR Then
        '    'y.ClearHeadAttach()
        '    '优化一头
        '    '先找到max facing match 以便最大化利用已有的匹配
        '    Dim existingmatch As Integer = Nuctions.FindFacingTailMatch(xTail, yHead, 12)
        '    Dim l = SearchAnnealing(xTail, x.TailLeadingLength, MinOptimizedTm)
        '    l = Math.Max(l, MinOptimizedLength)
        '    Dim attached = Nuctions.ReverseComplement(x.Tail.Substring(existingmatch, l - existingmatch))
        '    y.SetHeadAttach(attached)
        '    y.HeadPrimer = attached + y.HeadPrimer
        '    y.SourceDNAInfo.Calculate()
        '    y._OperationInfo.ReloadDNAInfo(y.SourceDNAInfo)
        '    y.CalculateHeadTm()
        'End If
    End Function

    Public Sub SetTailAttach(value As String)
        If IsForwardAssemblyDirection Then
            _GoldenGateDesignInfo.ReverseAttached = value
        Else
            _GoldenGateDesignInfo.ForwardAttached = value
        End If
    End Sub
    Public Sub ClearTailAttach()
        If IsForwardAssemblyDirection Then
            If _GoldenGateDesignInfo.ReverseAttached IsNot Nothing AndAlso _GoldenGateDesignInfo.ReverseAttached.Length > 0 Then
                If _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer IsNot Nothing And _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer.StartsWith(_GoldenGateDesignInfo.ReverseAttached) Then
                    _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer = _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer.Substring(_GoldenGateDesignInfo.ReverseAttached.Length)
                End If
                _GoldenGateDesignInfo.ReverseAttached = ""
            End If
        Else
            If _GoldenGateDesignInfo.ForwardAttached IsNot Nothing AndAlso _GoldenGateDesignInfo.ForwardAttached.Length > 0 Then
                If _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer IsNot Nothing And _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer.StartsWith(_GoldenGateDesignInfo.ForwardAttached) Then
                    _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer = _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer.Substring(_GoldenGateDesignInfo.ForwardAttached.Length)
                End If
                _GoldenGateDesignInfo.ForwardAttached = ""
            End If
        End If
    End Sub
    Public Sub SetHeadAttach(value As String)
        If IsForwardAssemblyDirection Then
            _GoldenGateDesignInfo.ForwardAttached = value
        Else
            _GoldenGateDesignInfo.ReverseAttached = value
        End If
    End Sub
    Public Sub ClearHeadAttach()
        If IsForwardAssemblyDirection Then
            If _GoldenGateDesignInfo.ForwardAttached IsNot Nothing AndAlso _GoldenGateDesignInfo.ForwardAttached.Length > 0 Then
                If _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer IsNot Nothing And _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer.StartsWith(_GoldenGateDesignInfo.ForwardAttached) Then
                    _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer = _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer.Substring(_GoldenGateDesignInfo.ForwardAttached.Length)
                End If
                _GoldenGateDesignInfo.ForwardAttached = ""
            End If
        Else
            If _GoldenGateDesignInfo.ReverseAttached IsNot Nothing AndAlso _GoldenGateDesignInfo.ReverseAttached.Length > 0 Then
                If _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer IsNot Nothing And _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer.StartsWith(_GoldenGateDesignInfo.ReverseAttached) Then
                    _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer = _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer.Substring(_GoldenGateDesignInfo.ReverseAttached.Length)
                End If
                _GoldenGateDesignInfo.ReverseAttached = ""
            End If
        End If

    End Sub
    Private Property TailPrimer As String
        Get
            If IsForwardAssemblyDirection Then
                Return _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer
            Else
                Return _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer
            End If
        End Get
        Set(value As String)
            If IsForwardAssemblyDirection Then
                _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer = value
            Else
                _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer = value
            End If
        End Set
    End Property
    Private Property HeadPrimer As String
        Get
            If IsForwardAssemblyDirection Then
                Return _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer
            Else
                Return _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer
            End If
        End Get
        Set(value As String)
            If IsForwardAssemblyDirection Then
                _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer = value
            Else
                _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer = value
            End If
        End Set
    End Property
    Private Property TailOther As String
        Get
            If IsForwardAssemblyDirection Then
                Return _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer
            Else
                Return _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer
            End If
        End Get
        Set(value As String)
            If IsForwardAssemblyDirection Then
                _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer = value
            Else
                _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer = value
            End If
        End Set
    End Property
    Private Property HeadOther As String
        Get
            If IsForwardAssemblyDirection Then
                Return _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer
            Else
                Return _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer
            End If
        End Get
        Set(value As String)
            If IsForwardAssemblyDirection Then
                _GoldenGateDesignInfo.SourceNode.PCR_ReversePrimer = value
            Else
                _GoldenGateDesignInfo.SourceNode.PCR_ForwardPrimer = value
            End If
        End Set
    End Property
    Friend Sub SetTail(AI As AnnealingInformation)
        'Dim _seq As String = Tail()
        'If IsForwardAssemblyDirection Then
        '    _GoldenGateDesignInfo.ReverseLeadingTail = _seq.Substring(0, AI.Offset)
        '    OnPropertyChanged("ReverseLeadingTail")
        '    _GoldenGateDesignInfo.ReverseAnnealingTail = _seq.Substring(AI.Offset, AI.Length)
        '    OnPropertyChanged("ReverseAnnealingTail")
        '    _GoldenGateDesignInfo.ReverseAnnealingTm = Nuctions.CalculateTm(_GoldenGateDesignInfo.ReverseAnnealingTail, Na, Concentration).Tm
        '    OnPropertyChanged("ReverseAnnealingTm")
        '    _GoldenGateDesignInfo.ReverseFollowingTail = _seq.Substring(AI.Offset + AI.Length, 8) + "..."
        '    OnPropertyChanged("ReverseFollowingTail")
        'Else
        '    _GoldenGateDesignInfo.ForwardLeadingTail = _seq.Substring(0, AI.Offset)
        '    OnPropertyChanged("ForwardLeadingTail")
        '    _GoldenGateDesignInfo.ForwardAnnealingTail = _seq.Substring(AI.Offset, AI.Length)
        '    OnPropertyChanged("ForwardAnnealingTail")
        '    _GoldenGateDesignInfo.ForwardAnnealingTm = Nuctions.CalculateTm(_GoldenGateDesignInfo.ForwardAnnealingTail, Na, Concentration).Tm
        '    OnPropertyChanged("ForwardAnnealingTm")
        '    _GoldenGateDesignInfo.ForwardFollowingTail = _seq.Substring(AI.Offset + AI.Length, 8) + "..."
        '    OnPropertyChanged("ForwardFollowingTail")
        'End If
    End Sub
    Friend Sub SetHead(AI As AnnealingInformation)
        'Dim _seq As String = Head()
        'If IsForwardAssemblyDirection Then
        '    _GoldenGateDesignInfo.ForwardLeadingTail = _seq.Substring(0, AI.Offset)
        '    OnPropertyChanged("ForwardLeadingTail")
        '    _GoldenGateDesignInfo.ForwardAnnealingTail = _seq.Substring(AI.Offset, AI.Length)
        '    OnPropertyChanged("ForwardAnnealingTail")
        '    _GoldenGateDesignInfo.ForwardAnnealingTm = Nuctions.CalculateTm(_GoldenGateDesignInfo.ForwardAnnealingTail, Na, Concentration).Tm
        '    OnPropertyChanged("ForwardAnnealingTm")
        '    _GoldenGateDesignInfo.ForwardFollowingTail = _seq.Substring(AI.Offset + AI.Length, 8) + "..."
        '    OnPropertyChanged("ForwardFollowingTail")
        'Else
        '    _GoldenGateDesignInfo.ReverseLeadingTail = _seq.Substring(0, AI.Offset)
        '    OnPropertyChanged("ReverseLeadingTail")
        '    _GoldenGateDesignInfo.ReverseAnnealingTail = _seq.Substring(AI.Offset, AI.Length)
        '    OnPropertyChanged("ReverseAnnealingTail")
        '    _GoldenGateDesignInfo.ReverseAnnealingTm = Nuctions.CalculateTm(_GoldenGateDesignInfo.ReverseAnnealingTail, Na, Concentration).Tm
        '    OnPropertyChanged("ReverseAnnealingTm")
        '    _GoldenGateDesignInfo.ReverseFollowingTail = _seq.Substring(AI.Offset + AI.Length, 8) + "..."
        '    OnPropertyChanged("ReverseFollowingTail")
        'End If
    End Sub
    Friend Function FindAvailableEnzymes() As List(Of Nuctions.RestrictionEnzyme)
        Dim fAdditional As Integer = ForwardPrimerAttached.Length
        Dim rAdditional As Integer = ReversePrimerAttached.Length
        Dim FSeq As String = SourceGeneFile.Sequence.Substring(fAdditional, SourceGeneFile.Sequence.Length - fAdditional - rAdditional)
        Dim RSeq As String = SourceGeneFile.RCSequence.Substring(rAdditional, SourceGeneFile.Sequence.Length - fAdditional - rAdditional)
        Dim eList As New List(Of Nuctions.RestrictionEnzyme)
        For Each enzyme In _OperationInfo.GetGoldenGateEnzymes()
            If Not (enzyme.Reg.IsMatch(FSeq) Or enzyme.Reg.IsMatch(RSeq)) Then eList.Add(enzyme)
        Next
        Return eList
    End Function
    Friend Function Tail() As GoldenGateEndInformation
        If IsForwardAssemblyDirection Then
            Return New GoldenGateEndInformation(SourceGeneFile, False)
        Else
            Return New GoldenGateEndInformation(SourceGeneFile, True)
        End If
    End Function
    Friend Function Head() As GoldenGateEndInformation
        If IsForwardAssemblyDirection Then
            Return New GoldenGateEndInformation(SourceGeneFile, True)
        Else
            Return New GoldenGateEndInformation(SourceGeneFile, False)
        End If
    End Function
#End Region
End Class

Friend Structure GoldenGateEndInformation
    Public Sequence As String
    Public OverhangType As Char
    Public Sub New(vGeneFile As Nuctions.GeneFile, IsForward As Boolean)
        If IsForward Then
            Sequence = vGeneFile.Sequence.Substring(0, vGeneFile.End_F.Length - 2)
            OverhangType = vGeneFile.End_F(0)
        Else
            Sequence = vGeneFile.RCSequence.Substring(0, vGeneFile.End_R.Length - 2)
            OverhangType = vGeneFile.End_R(0)
        End If
    End Sub
    Public Shared Function CanLigate(x As GoldenGateEndInformation, y As GoldenGateEndInformation) As Boolean
        If Not x.OverhangType = "5"c Then Return False
        If Not y.OverhangType = "5"c Then Return False
        If x.Sequence Is Nothing OrElse y.Sequence Is Nothing Then Return False
        If Not (x.Sequence.Length = y.Sequence.Length) Then Return False
        If Not (x.Sequence = Nuctions.ReverseComplement(y.Sequence)) Then Return False
        Return True
    End Function
End Structure

