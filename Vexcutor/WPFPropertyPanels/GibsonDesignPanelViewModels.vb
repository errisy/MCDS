
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Windows

<Serializable>
Public Class GibsonDesignViewModel
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
        For Each item In _DNAInfo.GibsonNodeDesignInfos.ToArray
            If _DNAInfo.Source.Contains(item.SourceNode) Then
                existingList.Add(item.SourceNode)
                _Source.Add(New NodeViewModel(item.SourceNode, Me, item))
            Else
                _DNAInfo.GibsonNodeDesignInfos.Remove(item)
            End If
        Next
        For Each dInfo In existingList
            newList.Remove(dInfo)
        Next
        For Each scr In _DNAInfo.Source
            If Not existingList.Contains(scr) Then
                Dim _NewNode As New NodeDesignInfo With {.SourceNode = scr}
                _Source.Add(New NodeViewModel(scr, Me, _NewNode)) '_Source已经被监视 所以不要用额外的代码来管理
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
    Public Property MinTmTemperature As Single
        Get
            Return _DNAInfo.GibsonMinAnnealingTm
        End Get
        Set(value As Single)
            _DNAInfo.GibsonMinAnnealingTm = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("MinTmTemperature"))
        End Set
    End Property
    Public Property MinLength As Integer
        Get
            Return _DNAInfo.GibsonMinAnnealingLength
        End Get
        Set(value As Integer)
            _DNAInfo.GibsonMinAnnealingLength = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("MinLength"))
        End Set
    End Property
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
            MsgBox(stbError.ToString, MsgBoxStyle.OkOnly, "Gibson Optimization Error")
        Else
            Dim ThisNode As NodeViewModel
            Dim NextNode As NodeViewModel
            Dim cnt As Integer = _Source.Count
            For i As Integer = 0 To cnt - 1
                ThisNode = _Source(i)
                NextNode = _Source((i + 1) Mod cnt)
                NodeViewModel.FindAnnealing(ThisNode, NextNode)
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

        Dim ThisNode As NodeViewModel
        Dim NextNode As NodeViewModel
        Dim cnt As Integer = _Source.Count
        'For i As Integer = 0 To cnt - 1

        '    ThisNode.ClearTailAttach()
        '    NextNode.ClearHeadAttach()
        '    ThisNode.CalculateTailTm()
        '    NextNode.CalculateHeadTm()
        'Next
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
            Dim xy = Nuctions.FacingSequenceAnnealingSearch(ThisNode.Tail, NextNode.Head, 12, 250)
            ThisNode.SetTail(xy.Key)
            NextNode.SetHead(xy.Value)
        Next
        _DNAInfo.LoadResultDNAList(New List(Of Nuctions.GeneFile))
        _DNAInfo.GetParetntChartItem.Reload(_DNAInfo, _View.EnzymeCol)
        If PropertyControlView IsNot Nothing Then PropertyControlView.RefreshDNAView()
        DrawOperationView()
    End Sub
    Private WithEvents _Source As New ObjectModel.ObservableCollection(Of NodeViewModel)
    Public ReadOnly Property Source As ObjectModel.ObservableCollection(Of NodeViewModel)
        Get
            Return _Source
        End Get
    End Property
    Private Sub _Source_CollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs) Handles _Source.CollectionChanged
        _DNAInfo.GibsonNodeDesignInfos.Clear()
        For Each it In _Source
            _DNAInfo.GibsonNodeDesignInfos.Add(it.DesignInfo)
        Next
    End Sub

    <NonSerialized> Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class

<System.Windows.Markup.ContentProperty("DataTemplates")>
Public Class GibsonTemplateSelector
    Inherits System.Windows.Controls.DataTemplateSelector
    Public ReadOnly Property DataTemplates As New ObjectModel.ObservableCollection(Of DataTemplate)
    Public Overrides Function SelectTemplate(item As Object, container As DependencyObject) As DataTemplate
        If Not (TypeOf item Is NodeViewModel) Then Return Nothing
        If _DataTemplates.Count < 2 Then Return Nothing
        Dim nvm As NodeViewModel = item
        If nvm.IsPCR Then
            Return _DataTemplates(0)
        Else
            Return _DataTemplates(1)
        End If
    End Function
End Class

Public Structure AnnealingInformation
    Public Offset As Integer
    Public Length As Integer
End Structure

<Serializable>
Public Class NodeDesignInfo
    Public SourceNode As DNAInfo
    Public IsForwardAssemblyDirection As Boolean
    Public IsForwardFixed As Boolean
    Public IsReverseFixed As Boolean
    Public ForwardLeadingTailLength As Integer
    Public ReverseLeadingTailLength As Integer
    Public ForwardLeadingTail As String = ""
    Public ReverseLeadingTail As String = ""
    Public ForwardAnnealingTail As String = ""
    Public ReverseAnnealingTail As String = ""
    Public ForwardAnnealingTm As Single
    Public ReverseAnnealingTm As Single
    Public ForwardFollowingTail As String = ""
    Public ReverseFollowingTail As String = ""
    Public ForwardAttached As String = ""
    Public ReverseAttached As String = ""
End Class

<Serializable>
Public Class NodeViewModel
    Implements INotifyPropertyChanged
    Private _OperationInfo As GibsonDesignViewModel
    Private _NodeDesignInfo As NodeDesignInfo
    Public Sub New(_Source As DNAInfo, _Operation As GibsonDesignViewModel, _DesignInfo As NodeDesignInfo)
        _Node = _Source
        _OperationInfo = _Operation
        _NodeDesignInfo = _DesignInfo
        _NodeType = Nuctions.GetMolecularOperationDescription(_Source.MolecularOperation)
        _NodeID = _Source.GetParetntChartItem.Index
        _IsPCR = (Node.MolecularOperation = Nuctions.MolecularOperationEnum.PCR)
        LoadPrimerInformation()
    End Sub
    Public ReadOnly Property DesignInfo As NodeDesignInfo
        Get
            Return _NodeDesignInfo
        End Get
    End Property
    Public ReadOnly Property SourceDNAInfo As DNAInfo
        Get
            Return _NodeDesignInfo.SourceNode
        End Get
    End Property
    Public ReadOnly Property SourceGeneFile As Nuctions.GeneFile
        Get
            If _NodeDesignInfo.SourceNode.DNAs.Count > 0 Then
                Return _NodeDesignInfo.SourceNode.DNAs(1)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public Sub LoadPrimerInformation()
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
            _ForwardPrimerName = _NodeDesignInfo.SourceNode.PCR_FPrimerName
            OnPropertyChanged("ForwardPrimerName")
            _ReversePrimerName = _NodeDesignInfo.SourceNode.PCR_RPrimerName
            OnPropertyChanged("ReversePrimerName")
            '_ForwardPrimerLength = _NodeDesignInfo.SourceNode.PCR_ForwardPrimer.Length
            'OnPropertyChanged("ForwardPrimerLength")
            '_ReversePrimerLength = _NodeDesignInfo.SourceNode.PCR_ReversePrimer.Length
            'OnPropertyChanged("ReversePrimerLength")

            CalculateForwardTm()
            CalculateReverseTm()
            AnalyzePrimerPair()
        Else
            If _DNALength - _NodeDesignInfo.ForwardLeadingTailLength - _NodeDesignInfo.ReverseLeadingTailLength <= 0 Then
                _Warning += "No sequence will be assembled due to leading tail length. "
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
        _ForwardPrimerLength = _NodeDesignInfo.SourceNode.PCR_ForwardPrimer.Length
        _ForwardPrimerAttached = _NodeDesignInfo.ForwardAttached
        If _ForwardPrimerAttached Is Nothing Then _ForwardPrimerAttached = ""
        _ForwardPrimerNonAnnealing = _NodeDesignInfo.SourceNode.PCR_ForwardPrimer.Substring(_ForwardPrimerAttached.Length)
        _ForwardTm = Nuctions.CalculateTm(_NodeDesignInfo.SourceNode.PCR_ForwardPrimer, Na, Concentration).Tm
        OnPropertyChanged("ForwardPrimerAttached")
        OnPropertyChanged("ForwardPrimerNonAnnealing")
        OnPropertyChanged("ForwardPrimerLength")
        OnPropertyChanged("ForwardTm")
    End Sub

    Public Sub CalculateReverseTm()
        _ReversePrimerLength = _NodeDesignInfo.SourceNode.PCR_ReversePrimer.Length
        _ReversePrimerAttached = _NodeDesignInfo.ReverseAttached
        If _ReversePrimerAttached Is Nothing Then _ReversePrimerAttached = ""
        _ReversePrimerNonAnnealing = _NodeDesignInfo.SourceNode.PCR_ReversePrimer.Substring(_ReversePrimerAttached.Length)
        _ReverseTm = Nuctions.CalculateTm(_NodeDesignInfo.SourceNode.PCR_ReversePrimer, Na, Concentration).Tm
        OnPropertyChanged("ReversePrimerAttached")
        OnPropertyChanged("ReversePrimerNonAnnealing")
        OnPropertyChanged("ReversePrimerLength")
        OnPropertyChanged("ReverseTm")
    End Sub
    Public Sub AnalyzePrimerPair()
        If Not _IsPCR Then Return
        Static _F = "F"
        Static _R = "R"
        Dim res = Nuctions.AnalyzePrimer(New Dictionary(Of String, String) From {{_F, _NodeDesignInfo.SourceNode.PCR_ForwardPrimer}, {_R, _NodeDesignInfo.SourceNode.PCR_ReversePrimer}}, New List(Of Nuctions.GeneFile), Na, Concentration)

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
    Public ReadOnly Property Warning As String
    Public ReadOnly Property NodeType As String
    Public ReadOnly Property NodeID As String

    '这个值是一个强制性的设置 要求优化时必须避开这个长度
    Public Property ForwardLeadingTailLength As Integer
        Get
            Return _NodeDesignInfo.ForwardLeadingTailLength
        End Get
        Set(value As Integer)
            _NodeDesignInfo.ForwardLeadingTailLength = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ForwardLeadingTailLength"))
        End Set
    End Property
    '这个值是一个强制性的设置 要求优化时必须避开这个长度
    Public Property ReverseLeadingTailLength As Integer
        Get
            Return _NodeDesignInfo.ReverseLeadingTailLength
        End Get
        Set(value As Integer)
            _NodeDesignInfo.ReverseLeadingTailLength = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ReverseLeadingTailLength"))
        End Set
    End Property
    Public ReadOnly Property ForwardLeadingTail As String
        Get
            Return _NodeDesignInfo.ForwardLeadingTail
        End Get
    End Property
    Public ReadOnly Property ReverseLeadingTail As String
        Get
            Return _NodeDesignInfo.ReverseLeadingTail
        End Get
    End Property
    Public ReadOnly Property ForwardAnnealingTail As String
        Get
            Return _NodeDesignInfo.ForwardAnnealingTail
        End Get
    End Property
    Public ReadOnly Property ReverseAnnealingTail As String
        Get
            Return _NodeDesignInfo.ReverseAnnealingTail
        End Get
    End Property
    Public ReadOnly Property ForwardFollowingTail As String
        Get
            Return _NodeDesignInfo.ForwardFollowingTail
        End Get
    End Property
    Public ReadOnly Property ReverseFollowingTail As String
        Get
            Return _NodeDesignInfo.ReverseFollowingTail
        End Get
    End Property
    Public Property IsForwardFixed As Boolean
        Get
            Return _NodeDesignInfo.IsForwardFixed
        End Get
        Set(value As Boolean)
            _NodeDesignInfo.IsForwardFixed = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("IsForwardFixed"))
        End Set
    End Property
    Public Property IsReverseFixed As Boolean
        Get
            Return _NodeDesignInfo.IsReverseFixed
        End Get
        Set(value As Boolean)
            _NodeDesignInfo.IsReverseFixed = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("IsReverseFixed"))
        End Set
    End Property
    'Attached>Annealing>NonAnnealing
    'Attached>Annealing>NonAnnealing
    Public ReadOnly Property ForwardPrimerName As String
    Public ReadOnly Property ReversePrimerName As String
    Public ReadOnly Property ForwardPrimerAttached As String
    Public ReadOnly Property ForwardPrimerAnnealing As String
    Public ReadOnly Property ForwardPrimerNonAnnealing As String
    Public ReadOnly Property ReversePrimerAttached As String
    Public ReadOnly Property ReversePrimerAnnealing As String
    Public ReadOnly Property ReversePrimerNonAnnealing As String
    Public ReadOnly Property ForwardPrimerLength As Integer
    Public ReadOnly Property ReversePrimerLength As Integer
    Public ReadOnly Property ForwardTm As Single
    Public ReadOnly Property ReverseTm As Single
    Public ReadOnly Property ForwardAnnealingTm As Single
        Get
            Return _NodeDesignInfo.ForwardAnnealingTm
        End Get
    End Property
    Public ReadOnly Property ReverseAnnealingTm As Single
        Get
            Return _NodeDesignInfo.ReverseAnnealingTm
        End Get
    End Property
    Public ReadOnly Property MaxForwardHairpin As String
    Public ReadOnly Property MaxReverseHairpin As String
    Public ReadOnly Property MaxForwardDimer As String
    Public ReadOnly Property MaxReverseDimer As String
    Public ReadOnly Property MaxCrossDimer As String
    Public Property IsForwardAssemblyDirection As Boolean
        Get
            Return _NodeDesignInfo.IsForwardAssemblyDirection
        End Get
        Set(value As Boolean)
            _NodeDesignInfo.IsForwardAssemblyDirection = value
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
    Public Shared Sub FindAnnealing(x As NodeViewModel, y As NodeViewModel)
        Dim xTail As String = x.Tail
        Dim yHead As String = y.Head
        Dim xLength = x.SourceGeneFile.Length
        Dim yLength = y.SourceGeneFile.Length
        Dim xy = Nuctions.FacingSequenceAnnealingSearch(xTail, yHead, 12, Math.Min(xLength, yLength) / 2)

        Dim MinOptimizedLength As Integer = x._OperationInfo.MinLength
        Dim MinOptimizedTm As Single = x._OperationInfo.MinTmTemperature

        If xy IsNot Nothing Then
            Dim NeedMoreOptimization As Boolean = False
            If xy.Key.Length < MinOptimizedLength Then NeedMoreOptimization = True
            If Nuctions.CalculateTm(xTail.Substring(xy.Key.Offset, xy.Key.Length), Na, Concentration).Tm < MinOptimizedTm Then NeedMoreOptimization = True
            If NeedMoreOptimization Then Optimize(x, y, xTail, yHead, MinOptimizedLength, MinOptimizedTm)
        Else
            Optimize(x, y, xTail, yHead, MinOptimizedLength, MinOptimizedTm)
        End If
        xy = Nuctions.FacingSequenceAnnealingSearch(x.Tail, y.Head, 12, 250)
        If xy IsNot Nothing Then
            x.SetTail(xy.Key)
            y.SetHead(xy.Value)
        End If
    End Sub
    Private Shared Sub Optimize(x As NodeViewModel, y As NodeViewModel, xTail As String, yHead As String, MinOptimizedLength As Integer, MinOptimizedTm As Single)
        If (x.IsPCR And Not x.TailPCRFixed) AndAlso (y.IsPCR And Not y.HeadPCRFixed) Then
            '可以双向优化
            '先找到max facing match 以便最大化利用已有的匹配

            Dim existingmatch As Integer = Nuctions.FindFacingTailMatch(xTail, yHead, 12)
            Dim ly = SearchAnnealing(yHead, y.HeadLeadingLength, MinOptimizedTm)
            ly = Math.Max(ly, MinOptimizedLength)
            Dim lx = SearchAnnealing(xTail, x.TailLeadingLength, MinOptimizedTm)
            lx = Math.Max(lx, MinOptimizedLength)
            '如何搜索最好的搭配？先生成一个从左到右的搭配
            'screen from x to y
            Dim XtoY As String = Nuctions.ReverseComplement(x.Tail.Substring(0, lx)) + y.Head.Substring(existingmatch, ly - existingmatch)
            Dim YtoX As String = Nuctions.ReverseComplement(XtoY)

            Dim MaxScore As Single = Single.MinValue
            Dim MinIndex As Integer
            Dim EmptyList As New List(Of Nuctions.GeneFile)
            Dim xTailPrimer As String = ""
            Dim yHeadPrimer As String = ""
            Dim xPPs As New List(Of PrimerPair)
            Dim yPPs As New List(Of PrimerPair)
            For i As Integer = 0 To lx - existingmatch
                Dim TotalAnnealingLength = SearchAnnealing(XtoY, i, MinOptimizedTm)
                TotalAnnealingLength = Math.Max(TotalAnnealingLength, MinOptimizedLength)
                If TotalAnnealingLength + i < lx Then Continue For
                If lx - existingmatch < i Then Continue For
                Dim xprmDict As New Dictionary(Of String, String)
                xprmDict.Add("XT", Nuctions.ReverseComplement(XtoY.Substring(lx, TotalAnnealingLength + i - lx)) + x.TailPrimer)
                xprmDict.Add("XO", x.TailOther)
                Dim xPNR = Nuctions.AnalyzePrimer(xprmDict, EmptyList, Na, Concentration)
                Dim yprmDict As New Dictionary(Of String, String)
                yprmDict.Add("YH", XtoY.Substring(i, lx - existingmatch - i) + y.HeadPrimer)
                yprmDict.Add("YO", y.HeadOther)
                Dim yPNR = Nuctions.AnalyzePrimer(yprmDict, EmptyList, Na, Concentration)
                Dim Score = (xPNR.MaxDimerEntropyScore + yPNR.MaxDimerEntropyScore + xPNR.MaxCrossDimerEntropyScore + yPNR.MaxCrossDimerEntropyScore) +
                    (xPNR.MaxHairpinEntropyScore + yPNR.MaxHairpinEntropyScore) * 2.0F
                If MaxScore < Score Then
                    MaxScore = Score
                    MinIndex = i
                    xTailPrimer = xprmDict("XT")
                    yHeadPrimer = yprmDict("YH")
                End If
            Next
            x.SetTailAttach(xTailPrimer.Substring(0, xTailPrimer.Length - x.TailPrimer.Length))
            y.SetHeadAttach(yHeadPrimer.Substring(0, yHeadPrimer.Length - y.HeadPrimer.Length))
            x.TailPrimer = xTailPrimer
            y.HeadPrimer = yHeadPrimer
            x.SourceDNAInfo.Calculate()
            x._OperationInfo.ReloadDNAInfo(x.SourceDNAInfo)
            y.SourceDNAInfo.Calculate()
            y._OperationInfo.ReloadDNAInfo(y.SourceDNAInfo)
            x.CalculateTailTm()
            y.CalculateHeadTm()
        ElseIf x.IsPCR And Not x.TailPCRFixed Then
            'x.ClearTailAttach()
            '优化一头
            '先找到max facing match 以便最大化利用已有的匹配
            Dim existingmatch As Integer = Nuctions.FindFacingTailMatch(xTail, yHead, 12)
            Dim l = SearchAnnealing(yHead, y.HeadLeadingLength, MinOptimizedTm)
            l = Math.Max(l, MinOptimizedLength)
            Dim attached = Nuctions.ReverseComplement(y.Head.Substring(existingmatch, l - existingmatch))
            x.SetTailAttach(attached)
            x.TailPrimer = attached + x.TailPrimer
            x.SourceDNAInfo.Calculate()
            x._OperationInfo.ReloadDNAInfo(x.SourceDNAInfo)
            x.CalculateTailTm()
        ElseIf y.IsPCR And Not y.HeadPCRFixed Then
            'y.ClearHeadAttach()
            '优化一头
            '先找到max facing match 以便最大化利用已有的匹配
            Dim existingmatch As Integer = Nuctions.FindFacingTailMatch(xTail, yHead, 12)
            Dim l = SearchAnnealing(xTail, x.TailLeadingLength, MinOptimizedTm)
            l = Math.Max(l, MinOptimizedLength)
            Dim attached = Nuctions.ReverseComplement(x.Tail.Substring(existingmatch, l - existingmatch))
            y.SetHeadAttach(attached)
            y.HeadPrimer = attached + y.HeadPrimer
            y.SourceDNAInfo.Calculate()
            y._OperationInfo.ReloadDNAInfo(y.SourceDNAInfo)
            y.CalculateHeadTm()
        End If
    End Sub

    Public Sub SetTailAttach(value As String)
        If IsForwardAssemblyDirection Then
            _NodeDesignInfo.ReverseAttached = value
        Else
            _NodeDesignInfo.ForwardAttached = value
        End If
    End Sub
    Public Sub ClearTailAttach()
        If IsForwardAssemblyDirection Then
            If _NodeDesignInfo.ReverseAttached IsNot Nothing AndAlso _NodeDesignInfo.ReverseAttached.Length > 0 Then
                If _NodeDesignInfo.SourceNode.PCR_ReversePrimer IsNot Nothing And _NodeDesignInfo.SourceNode.PCR_ReversePrimer.StartsWith(_NodeDesignInfo.ReverseAttached) Then
                    _NodeDesignInfo.SourceNode.PCR_ReversePrimer = _NodeDesignInfo.SourceNode.PCR_ReversePrimer.Substring(_NodeDesignInfo.ReverseAttached.Length)
                End If
                _NodeDesignInfo.ReverseAttached = ""
            End If
        Else
            If _NodeDesignInfo.ForwardAttached IsNot Nothing AndAlso _NodeDesignInfo.ForwardAttached.Length > 0 Then
                If _NodeDesignInfo.SourceNode.PCR_ForwardPrimer IsNot Nothing And _NodeDesignInfo.SourceNode.PCR_ForwardPrimer.StartsWith(_NodeDesignInfo.ForwardAttached) Then
                    _NodeDesignInfo.SourceNode.PCR_ForwardPrimer = _NodeDesignInfo.SourceNode.PCR_ForwardPrimer.Substring(_NodeDesignInfo.ForwardAttached.Length)
                End If
                _NodeDesignInfo.ForwardAttached = ""
            End If
        End If
    End Sub
    Public Sub SetHeadAttach(value As String)
        If IsForwardAssemblyDirection Then
            _NodeDesignInfo.ForwardAttached = value
        Else
            _NodeDesignInfo.ReverseAttached = value
        End If
    End Sub
    Public Sub ClearHeadAttach()
        If IsForwardAssemblyDirection Then
            If _NodeDesignInfo.ForwardAttached IsNot Nothing AndAlso _NodeDesignInfo.ForwardAttached.Length > 0 Then
                If _NodeDesignInfo.SourceNode.PCR_ForwardPrimer IsNot Nothing And _NodeDesignInfo.SourceNode.PCR_ForwardPrimer.StartsWith(_NodeDesignInfo.ForwardAttached) Then
                    _NodeDesignInfo.SourceNode.PCR_ForwardPrimer = _NodeDesignInfo.SourceNode.PCR_ForwardPrimer.Substring(_NodeDesignInfo.ForwardAttached.Length)
                End If
                _NodeDesignInfo.ForwardAttached = ""
            End If
        Else
            If _NodeDesignInfo.ReverseAttached IsNot Nothing AndAlso _NodeDesignInfo.ReverseAttached.Length > 0 Then
                If _NodeDesignInfo.SourceNode.PCR_ReversePrimer IsNot Nothing And _NodeDesignInfo.SourceNode.PCR_ReversePrimer.StartsWith(_NodeDesignInfo.ReverseAttached) Then
                    _NodeDesignInfo.SourceNode.PCR_ReversePrimer = _NodeDesignInfo.SourceNode.PCR_ReversePrimer.Substring(_NodeDesignInfo.ReverseAttached.Length)
                End If
                _NodeDesignInfo.ReverseAttached = ""
            End If
        End If

    End Sub
    Private Property TailPCRFixed As Boolean
        Get
            If IsForwardAssemblyDirection Then
                Return _NodeDesignInfo.IsReverseFixed
            Else
                Return _NodeDesignInfo.IsForwardFixed
            End If
        End Get
        Set(value As Boolean)
            If IsForwardAssemblyDirection Then
                _NodeDesignInfo.IsReverseFixed = value
            Else
                _NodeDesignInfo.IsForwardFixed = value
            End If
        End Set
    End Property
    Private Property HeadPCRFixed As Boolean
        Get
            If IsForwardAssemblyDirection Then
                Return _NodeDesignInfo.IsForwardFixed
            Else
                Return _NodeDesignInfo.IsReverseFixed
            End If
        End Get
        Set(value As Boolean)
            If IsForwardAssemblyDirection Then
                _NodeDesignInfo.IsForwardFixed = value
            Else
                _NodeDesignInfo.IsReverseFixed = value
            End If
        End Set
    End Property
    Private Property TailPrimer As String
        Get
            If IsForwardAssemblyDirection Then
                Return _NodeDesignInfo.SourceNode.PCR_ReversePrimer
            Else
                Return _NodeDesignInfo.SourceNode.PCR_ForwardPrimer
            End If
        End Get
        Set(value As String)
            If IsForwardAssemblyDirection Then
                _NodeDesignInfo.SourceNode.PCR_ReversePrimer = value
            Else
                _NodeDesignInfo.SourceNode.PCR_ForwardPrimer = value
            End If
        End Set
    End Property
    Private Property HeadPrimer As String
        Get
            If IsForwardAssemblyDirection Then
                Return _NodeDesignInfo.SourceNode.PCR_ForwardPrimer
            Else
                Return _NodeDesignInfo.SourceNode.PCR_ReversePrimer
            End If
        End Get
        Set(value As String)
            If IsForwardAssemblyDirection Then
                _NodeDesignInfo.SourceNode.PCR_ForwardPrimer = value
            Else
                _NodeDesignInfo.SourceNode.PCR_ReversePrimer = value
            End If
        End Set
    End Property
    Private Property TailOther As String
        Get
            If IsForwardAssemblyDirection Then
                Return _NodeDesignInfo.SourceNode.PCR_ForwardPrimer
            Else
                Return _NodeDesignInfo.SourceNode.PCR_ReversePrimer
            End If
        End Get
        Set(value As String)
            If IsForwardAssemblyDirection Then
                _NodeDesignInfo.SourceNode.PCR_ForwardPrimer = value
            Else
                _NodeDesignInfo.SourceNode.PCR_ReversePrimer = value
            End If
        End Set
    End Property
    Private Property HeadOther As String
        Get
            If IsForwardAssemblyDirection Then
                Return _NodeDesignInfo.SourceNode.PCR_ReversePrimer
            Else
                Return _NodeDesignInfo.SourceNode.PCR_ForwardPrimer
            End If
        End Get
        Set(value As String)
            If IsForwardAssemblyDirection Then
                _NodeDesignInfo.SourceNode.PCR_ReversePrimer = value
            Else
                _NodeDesignInfo.SourceNode.PCR_ForwardPrimer = value
            End If
        End Set
    End Property
    Public ReadOnly Property TailLeadingLength As Integer
        Get
            If IsForwardAssemblyDirection Then
                Return _NodeDesignInfo.ReverseLeadingTailLength
            Else
                Return _NodeDesignInfo.ForwardLeadingTailLength
            End If
        End Get
    End Property
    Public ReadOnly Property HeadLeadingLength As Integer
        Get
            If IsForwardAssemblyDirection Then
                Return _NodeDesignInfo.ForwardLeadingTailLength
            Else
                Return _NodeDesignInfo.ReverseLeadingTailLength
            End If
        End Get
    End Property
    Friend Sub SetTail(AI As AnnealingInformation)
        Dim _seq As String = Tail()
        If IsForwardAssemblyDirection Then
            _NodeDesignInfo.ReverseLeadingTail = _seq.Substring(0, AI.Offset)
            OnPropertyChanged("ReverseLeadingTail")
            _NodeDesignInfo.ReverseAnnealingTail = _seq.Substring(AI.Offset, AI.Length)
            OnPropertyChanged("ReverseAnnealingTail")
            _NodeDesignInfo.ReverseAnnealingTm = Nuctions.CalculateTm(_NodeDesignInfo.ReverseAnnealingTail, Na, Concentration).Tm
            OnPropertyChanged("ReverseAnnealingTm")
            _NodeDesignInfo.ReverseFollowingTail = _seq.Substring(AI.Offset + AI.Length, 8) + "..."
            OnPropertyChanged("ReverseFollowingTail")
        Else
            _NodeDesignInfo.ForwardLeadingTail = _seq.Substring(0, AI.Offset)
            OnPropertyChanged("ForwardLeadingTail")
            _NodeDesignInfo.ForwardAnnealingTail = _seq.Substring(AI.Offset, AI.Length)
            OnPropertyChanged("ForwardAnnealingTail")
            _NodeDesignInfo.ForwardAnnealingTm = Nuctions.CalculateTm(_NodeDesignInfo.ForwardAnnealingTail, Na, Concentration).Tm
            OnPropertyChanged("ForwardAnnealingTm")
            _NodeDesignInfo.ForwardFollowingTail = _seq.Substring(AI.Offset + AI.Length, 8) + "..."
            OnPropertyChanged("ForwardFollowingTail")
        End If
    End Sub
    Friend Sub SetHead(AI As AnnealingInformation)
        Dim _seq As String = Head()
        If IsForwardAssemblyDirection Then
            _NodeDesignInfo.ForwardLeadingTail = _seq.Substring(0, AI.Offset)
            OnPropertyChanged("ForwardLeadingTail")
            _NodeDesignInfo.ForwardAnnealingTail = _seq.Substring(AI.Offset, AI.Length)
            OnPropertyChanged("ForwardAnnealingTail")
            _NodeDesignInfo.ForwardAnnealingTm = Nuctions.CalculateTm(_NodeDesignInfo.ForwardAnnealingTail, Na, Concentration).Tm
            OnPropertyChanged("ForwardAnnealingTm")
            _NodeDesignInfo.ForwardFollowingTail = _seq.Substring(AI.Offset + AI.Length, 8) + "..."
            OnPropertyChanged("ForwardFollowingTail")
        Else
            _NodeDesignInfo.ReverseLeadingTail = _seq.Substring(0, AI.Offset)
            OnPropertyChanged("ReverseLeadingTail")
            _NodeDesignInfo.ReverseAnnealingTail = _seq.Substring(AI.Offset, AI.Length)
            OnPropertyChanged("ReverseAnnealingTail")
            _NodeDesignInfo.ReverseAnnealingTm = Nuctions.CalculateTm(_NodeDesignInfo.ReverseAnnealingTail, Na, Concentration).Tm
            OnPropertyChanged("ReverseAnnealingTm")
            _NodeDesignInfo.ReverseFollowingTail = _seq.Substring(AI.Offset + AI.Length, 8) + "..."
            OnPropertyChanged("ReverseFollowingTail")
        End If
    End Sub
    Friend Function Tail() As String
        If IsForwardAssemblyDirection Then
            If SourceGeneFile.End_R.Length >= 2 AndAlso SourceGeneFile.End_R(1) = "5"c Then Return SourceGeneFile.RCSequence.Substring(SourceGeneFile.End_R.Length - 2)
            Return SourceGeneFile.RCSequence
        Else
            If SourceGeneFile.End_F.Length >= 2 AndAlso SourceGeneFile.End_F(1) = "5"c Then Return SourceGeneFile.Sequence.Substring(SourceGeneFile.End_F.Length - 2)
            Return SourceGeneFile.Sequence
        End If
    End Function
    Friend Function Head() As String
        If IsForwardAssemblyDirection Then
            If SourceGeneFile.End_F.Length >= 2 AndAlso SourceGeneFile.End_F(1) = "5"c Then Return SourceGeneFile.Sequence.Substring(SourceGeneFile.End_F.Length - 2)
            Return SourceGeneFile.Sequence.Substring(ReverseLeadingTailLength)
        Else
            If SourceGeneFile.End_R.Length >= 2 AndAlso SourceGeneFile.End_R(1) = "5"c Then Return SourceGeneFile.RCSequence.Substring(SourceGeneFile.End_R.Length - 2)
            Return SourceGeneFile.RCSequence
        End If
    End Function
#End Region
End Class

