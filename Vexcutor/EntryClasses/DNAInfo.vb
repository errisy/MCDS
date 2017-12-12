<System.ComponentModel.Bindable(True), Serializable>
Public Class DNAInfo
    'common titles
    Private vName As String
    <System.ComponentModel.Bindable(False)> Public Property Name() As String
        Get
            Return vName
        End Get
        Set(ByVal value As String)
            vName = value
        End Set
    End Property
    Public ReadOnly Property IsAnalytical As Boolean
        Get
            Return IsVerificationStep Or (vMolecularOperation = Nuctions.MolecularOperationEnum.EnzymeAnalysis) Or (vMolecularOperation = Nuctions.MolecularOperationEnum.SequencingResult)
        End Get
    End Property
    Public OperationDescription As String
    Public IsVerificationStep As Boolean = False
    Public IsConstructionNode As Boolean = False
    Public NotDrawMap As Boolean = False
    Public Host As Nuctions.Host = New Nuctions.Host With {.Name = "in vitro"}
    Private vMolecularOperation As Nuctions.MolecularOperationEnum
    <System.ComponentModel.Bindable(False)> Public Event UpdateImage(ByVal sender As Object, ByVal e As EventArgs)
    <System.ComponentModel.Bindable(False)> Public Property MolecularOperation() As Nuctions.MolecularOperationEnum
        Get
            Return vMolecularOperation
        End Get
        Set(ByVal value As Nuctions.MolecularOperationEnum)
            vMolecularOperation = value
            RaiseEvent UpdateImage(Me, New EventArgs)
        End Set
    End Property
    Public DNAs As New Collection
    Public Calculated As Boolean

    Public File_Filename As String
    Public Enzyme_Enzymes As New List(Of String)
    Public Enzyme_Buffers As New List(Of BufferCondition)
    Public Enzyme_Condition As New List(Of String)
    Public DephosphorylateWhenDigestion As Boolean = False
    Public PCR_ForwardPrimer As String
    Public PCR_FPrimerName As String
    Public PCR_ReversePrimer As String
    Public PCR_RPrimerName As String
    Public PCR_Overlap As Boolean = False
    Public Modify_Method As Nuctions.ModificationMethodEnum = Nuctions.ModificationMethodEnum.CIAP
    'T4DNAP Klewnow CIAP PNK
    Public Screen_Features As New List(Of FeatureScreenInfo)
    Public Screen_OnlyCircular As Boolean
    Public Source As New List(Of DNAInfo)
    Public Editing As Boolean = False
    Public Creating As Boolean = False

    Public Ligation_TriFragment As LigationMethod = MCDS.LigationMethod.Normal2Fragment

    Public LigationMethod As LigationMethod

    'Public FeatureCol As New List(Of Nuctions.Feature)
    Public Gel_Minimum As Integer = 1000
    Public Gel_Maximun As Integer = 1500
    Public SolutionExtration As Boolean = False
    Public SetPosition As Point
    'ScreenMode
    Public Screen_PCRMax As Integer = 1500
    Public Screen_PCRMin As Integer = 1000
    Public Screen_FPrimer As String = ""
    Public Screen_FName As String = "F"
    Public Screen_RPrimer As String = ""
    Public Screen_RName As String = "R"
    Public Screen_Mode As Nuctions.ScreenModeEnum = Nuctions.ScreenModeEnum.Features

    'Recombination
    Public RecombinationMethod As RecombinationMethod


    'EnzymeAnalysis
    Public EnzymeAnalysisParamters As New List(Of EnzymeAnalysisItem)
    Public FetchedEnzymes As New List(Of String)

    'Merge
    Public OnlySignificant As Boolean = True
    Public OnlyExtend As Boolean = True

    'Gel Extraction
    Public AutomaticChoose As Boolean = True

    'Sequencing
    Public SequencingTheorica As Nuctions.GeneFile
    Public SequencingPrimerName As String = ""
    Public SequencingPrimer As String = ""
    Public SequencingSequence As String = ""
    Public TheorySequences As New List(Of String)
    Public SequencingResultComment As SequenceResultOptions

    'Compare
    Public CompareSelectedGeneFile As Nuctions.GeneFile
    Public CompareResultComment As SequenceResultOptions

    <System.ComponentModel.Bindable(False)> Public Event RequireParentChartItem(ByVal sender As Object, ByVal e As GetChartItemEventArgs)

    <System.ComponentModel.Bindable(False)> Public Function GetParetntChartItem() As ChartItem
        Dim pnt As ChartItem = Nothing
        Dim e As New GetChartItemEventArgs
        RaiseEvent RequireParentChartItem(Me, e)
        Return e.ParentChartItem
    End Function

    'HashPicker
    Public PickedDNAs As New List(Of String)

    Public DescribeType As DescribeEnum
    'Host
    'Public HostCell As New Nuctions.Host
    'Transformation 
    Public TransformationMethod As Nuctions.TransformationMethod = Nuctions.TransformationMethod.ChemicalTransformation
    Public TransformationMode As Nuctions.TransformationMode = Nuctions.TransformationMode.AllToOneCell
    '培养
    Public Incubation As New List(Of IncubationStep)

    Public Cells As New List(Of Nuctions.Cell) '主要是在transformation之后用到的 用来按照细胞的组合进行筛选
    '提质粒
    Public IncludeVerification As Boolean = False
    Public SequencingVerify As Boolean = False
    '绘图信息

    Public DX As Single
    Public DY As Single
    Public RealSize As Boolean

    Public Finished As Boolean = False
    Public Progress As ProgressEnum

    Public PixelPerKBP As Integer = 0

    Public IsKeyName As Boolean = False

    Public PCRDesignerData As Byte()
    Public SequenceDesignerData As Byte()
    Public UseFreeDesigner As Boolean = False

    Public SCFFiles As New System.Collections.ObjectModel.ObservableCollection(Of SequenceItem)
    Public GelFiles As New System.Collections.ObjectModel.ObservableCollection(Of BitImage)


    Public GelFirstFigureNumber As Integer = 0
    Public GelSecondFigureNumber As Integer = 0
    Public ColonyFirstSequencingNumber As Integer = 0
    Public ColonySecondSequencingNumber As Integer = 0

    Public IsExhaustiveAssembly As Boolean = False
    Public TimesForAssembly As Integer = 1
    Public RecombinationMethodString As String
    Public CRISPRgRNA As New HashSet(Of DNAInfo)

    'Gibson Assembly Design
    Public GibsonMinAnnealingTm As Single = 46.0F
    Public GibsonMinAnnealingLength As Integer = 14
    Public GibsonNodeDesignInfos As New List(Of NodeDesignInfo)
    Public GoldenGateDesignInfos As New List(Of GoldenGateDesignInfo)
    Private Sub TryReload()
        Dim pc = GetParetntChartItem()
        If pc.Exists Then
            GetParetntChartItem.Reload(Me, pc.DisplayedEnzymes)
        End If
    End Sub
    Public Property FirstFigureNumber As Integer
        Get
            Return GelFirstFigureNumber
        End Get
        Set(value As Integer)
            Dim c = GelFirstFigureNumber <> value
            GelFirstFigureNumber = value
            If c Then TryReload()
        End Set
    End Property
    Public Property SecondFigureNumber As Integer
        Get
            Return GelSecondFigureNumber
        End Get
        Set(value As Integer)
            Dim c = GelFirstFigureNumber <> value
            GelSecondFigureNumber = value
            If c Then TryReload()
        End Set
    End Property
    Public Property FirstSequencingNumber As Integer
        Get
            Return ColonyFirstSequencingNumber
        End Get
        Set(value As Integer)
            Dim c = GelFirstFigureNumber <> value
            ColonyFirstSequencingNumber = value
            If c Then TryReload()
        End Set
    End Property
    Public Property SecondSequencingNumber As Integer
        Get
            Return ColonySecondSequencingNumber
        End Get
        Set(value As Integer)
            Dim c = GelFirstFigureNumber <> value
            ColonySecondSequencingNumber = value
            If c Then TryReload()
        End Set
    End Property
    Public Function Clone() As DNAInfo
        Dim dnai As New DNAInfo
        Dim t As Type = GetType(DNAInfo)
        For Each fi As System.Reflection.FieldInfo In t.GetFields(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
            fi.SetValue(dnai, fi.GetValue(Me))
        Next
        'need to deep clone some fields
        dnai.Host = Host.Clone
        dnai.Cells = New List(Of Nuctions.Cell)
        For Each cell As Nuctions.Cell In Cells
            dnai.Cells.Add(cell.Clone)
        Next
        dnai.Incubation = New List(Of IncubationStep)
        For Each ib As IncubationStep In Incubation
            dnai.Incubation.Add(ib.Clone)
        Next
        dnai.vName = vName
        dnai.vMolecularOperation = vMolecularOperation
        Return dnai
    End Function

    Public Function Backup() As DNAInfo
        Dim dnai As New DNAInfo
        Dim t As Type = GetType(DNAInfo)
        For Each fi As System.Reflection.FieldInfo In t.GetFields(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
            fi.SetValue(dnai, fi.GetValue(Me))
        Next
        dnai.Source = New List(Of DNAInfo)
        dnai.Source.AddRange(Source)
        dnai.DNAs = New Collection
        For Each gf As Nuctions.GeneFile In DNAs
            dnai.DNAs.Add(gf)
        Next
        dnai.vName = vName
        dnai.vMolecularOperation = vMolecularOperation
        Return dnai
    End Function

    Public Sub Recover(ByVal DNAi As DNAInfo)
        Dim t As Type = GetType(DNAInfo)
        For Each fi As System.Reflection.FieldInfo In t.GetFields(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
            fi.SetValue(Me, fi.GetValue(DNAi))
        Next
    End Sub
    Public ReadOnly Property AllDNAinSource() As List(Of Nuctions.GeneFile)
        Get
            Dim gfCol As New List(Of Nuctions.GeneFile)
            If Source.OK Then
                For Each mi As DNAInfo In Source
                    If mi.Cells.Count > 0 Then
                        For Each c As Nuctions.Cell In mi.Cells
                            gfCol.AddRange(c.DNAs)
                        Next
                    End If
                    For Each gf In mi.DNAs
                        gfCol.Add(gf)
                    Next
                Next
            End If
            Return gfCol
        End Get
    End Property

    Public Sub Calculate()
#If ReaderMode = 0 Then
        Select Case Me.MolecularOperation
            Case Nuctions.MolecularOperationEnum.Enzyme
                'get the vectors from source and analyze enzyme cutting.
                Dim gf As Nuctions.GeneFile
                Dim EARC As Nuctions.EnzymeAnalysis.EnzymeAnalysisResult
                Dim earcCol As New Collection
                Dim info As String
                Dim infoQ As New Queue(Of String)
                Dim dnaList As List(Of Nuctions.GeneFile)
                Dim DNA As Nuctions.GeneFile
                Dim SourceDNA As New Collection
                For Each mi As DNAInfo In Source
                    For Each gf In mi.DNAs
                        SourceDNA.Add(gf)
                    Next
                Next
                For Each gf In SourceDNA
                    EARC = New Nuctions.EnzymeAnalysis.EnzymeAnalysisResult(Enzyme_Enzymes, gf)
                    For Each info In EARC.Confliction
                        infoQ.Enqueue(info)
                    Next
                    earcCol.Add(EARC)
                Next
                If infoQ.Count = 0 Then
                    If DNAs.Count > 0 Then DNAs.Clear()
                    Dim dList As New List(Of Nuctions.GeneFile)
                    For Each EARC In earcCol
                        dnaList = EARC.CutDNA()
                        For Each DNA In dnaList
                            dList.Add(DNA)
                        Next
                    Next
                    If DephosphorylateWhenDigestion Then dList = Nuctions.ModifyDNA(dList, Nuctions.ModificationMethodEnum.CIAP)
                    LoadResultDNAList(dList)
                    Dim lv As New List(Of String)
                    For Each ez As String In Enzyme_Enzymes
                        lv.Add(ez)
                    Next
                    If Enzyme_Condition Is Nothing Then Enzyme_Condition = New List(Of String)
                    Enzyme_Condition.Clear()
                    Dim conditions = Nuctions.FindDigestionInformation(lv)
                    For Each value In conditions.Values
                        Enzyme_Condition.Add(value)
                    Next
                    'refresh the DNA view:
                Else
                    infoQ.Enqueue("Failed to Cut the Provided DNAs.")
                    Dim fER As New WPFErrorWindow
                    fER.WindowStartupLocation = Windows.WindowStartupLocation.CenterScreen
                    Dim wih As New System.Windows.Interop.WindowInteropHelper(fER)
                    wih.Owner = SettingEntry.MainUIWindow.Handle
                    fER.ShowQueue(infoQ)
                    fER.ShowDialog()
                    'SettingEntry.MainUIWindow.ShowDialog(fER)
                End If
            Case Nuctions.MolecularOperationEnum.Gel
                Dim gf As Nuctions.GeneFile
                Dim gfCol As New Collection
                'get all the DNAs
                For Each mi As DNAInfo In Source
                    For Each gf In mi.DNAs
                        gfCol.Add(gf)
                    Next
                Next
                Dim gelCol As New Collection
                Dim gfList As New List(Of Nuctions.GeneFile)
                For Each g As Nuctions.GeneFile In gfCol
                    If g.Length >= 50 Then gelCol.Add(g)
                Next
                If AutomaticChoose Then
                    If gelCol.Count > 1 Then
                        gfList = Nuctions.ScreenLength(gelCol, Gel_Minimum, Gel_Maximun)
                    ElseIf gelCol.Count = 0 Then

                    Else
                        Gel_Minimum = gelCol(1).Length
                        Gel_Maximun = gelCol(1).Length
                        gfList = Nuctions.ScreenLength(gelCol, Gel_Minimum, Gel_Maximun)
                    End If
                Else
                    gfList = Nuctions.ScreenLength(gelCol, Gel_Minimum, Gel_Maximun)
                End If
                LoadResultDNAList(gfList)
            Case Nuctions.MolecularOperationEnum.Ligation

                Dim lgCol As List(Of Nuctions.GeneFile) = Nothing

                'Select Case Ligation_TriFragment
                '    Case LigationMethod.Normal2Fragment
                '        lgCol = Nuctions.LigateDNA(GetSourceDNAList, False)
                '    Case LigationMethod.Consider3Fragment
                '        lgCol = Nuctions.LigateDNA(GetSourceDNAList, True)
                '    Case LigationMethod.MultipleFragmentUnique
                '        lgCol = Nuctions.MultipleLinearLigate(GetSourceDNAList)
                'End Select

                lgCol = Nuctions.MultipleLigate(GetSourceDNAList, IsExhaustiveAssembly, TimesForAssembly)

                LoadResultDNAList(lgCol)
            'Nuctions.AddFeatures(DNAs, GetParetntChartItem.Feature)
            Case Nuctions.MolecularOperationEnum.Modify
                LoadResultDNAList(Nuctions.ModifyDNA(GetSourceDNAList, Modify_Method))
            'Nuctions.AddFeatures(DNAs, GetParetntChartItem.Feature)
            Case Nuctions.MolecularOperationEnum.PCR

                Dim gf As Nuctions.GeneFile
                Dim gfCol As New List(Of Nuctions.GeneFile)

                'get all the DNAs
                For Each mi As DNAInfo In Source
                    If mi.Cells.Count > 0 Then
                        For Each c As Nuctions.Cell In mi.Cells
                            gfCol.AddRange(c.DNAs)
                        Next
                    End If
                    For Each gf In mi.DNAs
                        gfCol.Add(gf)
                    Next
                Next
                Dim pmrList As New List(Of String)
                If PrimerDesignerMode Then
                    pmrList.AddRange(DesignedPrimers.Values)
                Else
                    pmrList.Add(Nuctions.TAGCFilter(PCR_ForwardPrimer))
                    pmrList.Add(Nuctions.TAGCFilter(PCR_ReversePrimer))
                End If
                Dim gfList As List(Of Nuctions.GeneFile) = Nuctions.PCR(gfCol, pmrList)
                LoadResultDNAList(gfList)
                Dim pDict As New Dictionary(Of String, String) From {{PCR_FPrimerName, PCR_ForwardPrimer},
                    {PCR_RPrimerName, PCR_ReversePrimer}}
                GetParetntChartItem.Parent.SummarziePirmers(GetParetntChartItem, pDict)
            'Nuctions.AddFeatures(DNAs, GetParetntChartItem.Feature)

            Case Nuctions.MolecularOperationEnum.Screen
                Dim cList As List(Of Nuctions.Cell) = GetSourceCellList()
                Dim newcell As Nuctions.Cell
                If cList.Count > 0 Then
                    Dim rList As New List(Of Nuctions.Cell)
                    For Each c As Nuctions.Cell In cList
                        If Screen_Mode = Nuctions.ScreenModeEnum.Features Then
                            Dim gfList As List(Of Nuctions.GeneFile) = Nuctions.ScreenFeature(c.DNAs, Screen_Features, Screen_OnlyCircular)
                            If gfList.Count > 0 Then
                                newcell = c.Clone
                                rList.Add(newcell)
                            End If
                            Nuctions.AddFeatures(DNAs, GetParetntChartItem.Feature, Me.GetParetntChartItem.Parent.Primers)
                        Else
                            Dim errQ As New Queue(Of String)
                            Dim scrErr As Boolean = False
                            Dim gfList As List(Of Nuctions.GeneFile) = Nuctions.ScreenPCR(c.DNAs, Screen_FPrimer, Screen_RPrimer, Screen_PCRMax, Screen_PCRMin, Screen_OnlyCircular)
                            If gfList.Count > 0 Then
                                newcell = c.Clone
                                rList.Add(newcell)
                            End If
                        End If
                    Next
                    LoadResultCellList(rList)
                Else
                    Dim gfCol As List(Of Nuctions.GeneFile) = GetSourceDNAList()
                    'get all the DNAs
                    If Screen_Mode = Nuctions.ScreenModeEnum.Features Then
                        Dim gfList As List(Of Nuctions.GeneFile) = Nuctions.ScreenFeature(gfCol, Screen_Features, Screen_OnlyCircular)
                        LoadResultDNAList(gfList)
                        'Nuctions.AddFeatures(DNAs, GetParetntChartItem.Feature)
                    Else
                        Dim errQ As New Queue(Of String)
                        Dim scrErr As Boolean = False
                        Dim gfList As List(Of Nuctions.GeneFile) = Nuctions.ScreenPCR(gfCol, Screen_FPrimer, Screen_RPrimer, Screen_PCRMax, Screen_PCRMin, Screen_OnlyCircular)
                        LoadResultDNAList(gfList)
                        'Nuctions.AddFeatures(DNAs, GetParetntChartItem.Feature)
                    End If
                End If
            Case Nuctions.MolecularOperationEnum.Vector
                Nuctions.AddFeatures(DNAs, Features, Me.GetParetntChartItem.Parent.Primers)
                Calculated = True
            Case Nuctions.MolecularOperationEnum.Recombination
                Dim cList As New List(Of Nuctions.Cell)
                Dim gList As New List(Of Nuctions.GeneFile)
                For Each scr As DNAInfo In Source
                    Select Case scr.vMolecularOperation
                        Case Nuctions.MolecularOperationEnum.Transformation, Nuctions.MolecularOperationEnum.Incubation, Nuctions.MolecularOperationEnum.Host
                            For Each c As Nuctions.Cell In scr.Cells
                                cList.Add(c.Clone)
                            Next
                        Case Nuctions.MolecularOperationEnum.Recombination, Nuctions.MolecularOperationEnum.Enzyme
                            For Each c As Nuctions.Cell In scr.Cells
                                If c.DNAs.Count > 0 Then cList.Add(c.Clone)
                            Next
                            If cList.Count = 0 Then
                                For Each gf As Nuctions.GeneFile In scr.DNAs
                                    gList.Add(gf)
                                Next
                            End If
                        Case Nuctions.MolecularOperationEnum.EnzymeAnalysis

                        Case Else
                            For Each gf As Nuctions.GeneFile In scr.DNAs
                                gList.Add(gf)
                            Next
                    End Select
                Next
                Dim rList As List(Of Nuctions.GeneFile)
                rList = Nuctions.Recombination(gList, RecombinationMethodString, IsExhaustiveAssembly, TimesForAssembly)
                DNAs.Clear()
                For Each gf As Nuctions.GeneFile In rList
                    DNAs.Add(gf)
                Next
                Nuctions.AddFeatures(DNAs, GetParetntChartItem.Feature, Me.GetParetntChartItem.Parent.Primers)
                Cells.Clear()
                For Each c As Nuctions.Cell In cList
                    c.PrepareRecombine()
                    c.DNAs = Nuctions.Recombination(c.DNAs, RecombinationMethodString, IsExhaustiveAssembly, TimesForAssembly)
                    For Each cx As Nuctions.Cell In c.FixRecombine()
                        Nuctions.AddFeatures(cx.DNAs, GetParetntChartItem.Feature, Me.GetParetntChartItem.Parent.Primers)
                        Cells.Add(cx)
                    Next
                Next
            Case Nuctions.MolecularOperationEnum.EnzymeAnalysis
                FetchedEnzymes = Nuctions.FindEnzymes(EnzymeAnalysisParamters)
                Dim gList As List(Of Nuctions.GeneFile) = GetSourceDNAList()
                'Nuctions.AddFeatures(gList, Features)
                LoadResultDNAList(gList)
                Dim stb As New System.Text.StringBuilder
                For Each enzm As String In FetchedEnzymes
                    stb.Append(enzm)
                    stb.Append(" ")
                Next
                Dim EnzCol As New List(Of String)
                EnzCol.AddRange(Me.GetParetntChartItem.Parent.EnzymeCol)
                For Each ez As String In Me.GetParetntChartItem.MolecularInfo.FetchedEnzymes
                    If Not (EnzCol.Contains(ez)) Then EnzCol.Add(ez)
                Next
                Me.GetParetntChartItem.Reload(Me.GetParetntChartItem.MolecularInfo, EnzCol)
            'OperationDescription = stb.ToString
            Case Nuctions.MolecularOperationEnum.FreeDesign
                If UseFreeDesigner Then
                    'nothing to do because generate at real time
                Else
                    Dim EnzList As New List(Of String)
                    Nuctions.ParseCode(FreeDesignName, FreeDesignCode, GetParetntChartItem.Parent.Features, DNAs, EnzList)
                    Dim EnzCol As New List(Of String)
                    EnzCol.AddRange(GetParetntChartItem.Parent.EnzymeCol)
                    DesignedEnzymeSite.Clear()
                    DesignedEnzymeSite.AddRange(EnzList)
                    For Each ez As String In EnzList
                        If Not (EnzCol.Contains(ez)) Then EnzCol.Add(ez)
                    Next
                End If
                Calculated = True
            Case Nuctions.MolecularOperationEnum.HashPicker
                Dim results As List(Of Nuctions.GeneFile) = Nuctions.ScreenByHash(GetSourceDNAList(), PickedDNAs)
                LoadResultDNAList(results)
                'Nuctions.AddFeatures(results, GetParetntChartItem.Parent.Features)
                Calculated = True
            Case Nuctions.MolecularOperationEnum.SequencingResult
                Dim results As New List(Of Nuctions.GeneFile)
                If SequencingSequence.Length > 25 Then
                    Dim RealResult As New Nuctions.GeneFile
                    RealResult.Name = SequencingPrimerName + " Seq"
                    RealResult.Sequence = SequencingSequence
                    results.Add(RealResult)
                    Nuctions.AddFeatures(results, GetParetntChartItem.Parent.Features, Me.GetParetntChartItem.Parent.Primers)

                    Dim res As List(Of String) = Nuctions.CalculateTheoreticalSequencing(GetSourceDNAList, SequencingPrimer, SequencingSequence.Length)
                    TheorySequences = res
                    If TheorySequences.Count = 1 Then
                        SequencingTheorica = New Nuctions.GeneFile
                        SequencingTheorica.Name = SequencingPrimerName + " Cal"
                        SequencingTheorica.Sequence = TheorySequences(0)
                        SequencingTheorica.BLAST(New List(Of Nuctions.GeneFile) From {RealResult})
                    End If
                    LoadResultDNAList(results, False)
                End If
                Calculated = True
            Case Nuctions.MolecularOperationEnum.Compare
                If Not (CompareSelectedGeneFile Is Nothing) Then
                    Dim results As New List(Of Nuctions.GeneFile)
                    Dim vList As New List(Of Nuctions.GeneFile) From {CompareSelectedGeneFile.CloneWithoutFeatures}
                    Nuctions.AddFeatures(vList, Features, Me.GetParetntChartItem.Parent.Primers)
                    'Dim gList As New List(Of Nuctions.GeneFile)
                    Dim oriList As New List(Of Nuctions.GeneFile)
                    oriList.Add(CompareSelectedGeneFile.CloneWithoutFeatures)
                    For Each gf In GetOriginalDNAList()
                        If gf Is CompareSelectedGeneFile Then Continue For
                        oriList.Add(gf.CloneWithoutFeatures)
                    Next
                    For Each gf As Nuctions.GeneFile In oriList
                        Dim targetList As New List(Of Nuctions.GeneFile)
                        targetList.AddRange(oriList)
                        targetList.Remove(gf)
                        'If gf Is CompareSelectedGeneFile Then Continue For
                        gf.BLAST(targetList)
                    Next
                    'vList(0).BLAST(gList)
                    Nuctions.AddFeatures(oriList, Features, Me.GetParetntChartItem.Parent.Primers)
                    'results.Add(vList(0))
                    results.AddRange(oriList)
                    LoadResultDNAList(results, False)
                Else
                    Dim results As List(Of Nuctions.GeneFile) = GetSourceDNAList()
                    Nuctions.AddFeatures(results, Features, Me.GetParetntChartItem.Parent.Primers)
                    LoadResultDNAList(results, False)
                End If
                Calculated = True
            Case Nuctions.MolecularOperationEnum.Host
                'nothing to do
                For Each c As Nuctions.Cell In Cells
                    Nuctions.AddFeatures(c.DNAs, Features, Me.GetParetntChartItem.Parent.Primers)
                Next
                Calculated = True
            Case Nuctions.MolecularOperationEnum.Transformation
                'find the host and put the rest non-free-ended DNA into it.

                'generate a list of host

                Dim nList As New List(Of Nuctions.Cell)
                For Each s As DNAInfo In Source
                    Select Case s.MolecularOperation
                        Case Nuctions.MolecularOperationEnum.Host
                            'there can be only one host.
                            nList.AddRange(s.Cells)
                        Case Nuctions.MolecularOperationEnum.Transformation
                            'there may be many different hosts.
                            nList.AddRange(s.Cells)
                        Case Nuctions.MolecularOperationEnum.Incubation
                            'there may be many different host.
                            nList.AddRange(s.Cells)
                    End Select
                Next

                Dim cList As New List(Of Nuctions.Cell)

                For Each c As Nuctions.Cell In nList
                    cList.Add(c.Clone)
                Next

                'generate a list of DNA
                Dim gList As New List(Of Nuctions.GeneFile)
                For Each s As DNAInfo In Source
                    If s.Host.Name = "in vitro" Then
                        For Each g As Nuctions.GeneFile In s.DNAs
                            gList.Add(g.CloneWithoutFeatures)
                        Next
                    End If
                Next
                Nuctions.ReduceDNA(gList)
                'see the type of transformation
                Select Case TransformationMethod
                    Case Nuctions.TransformationMethod.ChemicalTransformation, Nuctions.TransformationMethod.Electroporation
                        Select Case TransformationMode
                            Case Nuctions.TransformationMode.AllToOneCell
                                For Each c As Nuctions.Cell In cList
                                    c.AddRange(gList)
                                Next
                                Cells.Clear()
                                Cells.AddRange(cList)
                            Case Nuctions.TransformationMode.EachPerCell
                                Dim xList As New List(Of Nuctions.Cell)
                                Dim cc As Nuctions.Cell
                                For Each c As Nuctions.Cell In cList
                                    For Each g As Nuctions.GeneFile In gList
                                        cc = c.Clone
                                        cc.Add(g)
                                        xList.Add(cc)
                                    Next
                                Next
                                Cells.Clear()
                                Cells.AddRange(xList)
                            Case Nuctions.TransformationMode.Combinational
                                Cells.Clear()
                                Dim cc As Nuctions.Cell
                                For Each gl As List(Of Nuctions.GeneFile) In Combination(gList)
                                    For Each c As Nuctions.Cell In cList
                                        cc = c.Clone()
                                        cc.AddRange(gl)
                                        Cells.Add(cc)
                                    Next
                                Next
                        End Select
                    Case Nuctions.TransformationMethod.Conjugation
                        Dim dList As New List(Of Nuctions.Cell)
                        Dim tList As New List(Of Nuctions.Cell)
                        Dim oriTs As List(Of String)
                        For Each c As Nuctions.Cell In cList
                            Nuctions.AddFeatures(c, Features, Me.GetParetntChartItem.Parent.Primers)
                            oriTs = c.GetConjugationType(True)
                            If oriTs.Count > 0 Then
                                dList.Add(c.Clone)
                            End If
                            tList.Add(c.Clone)
                        Next
                        For Each d As Nuctions.Cell In dList
                            Nuctions.AddFeatures(d, Features, Me.GetParetntChartItem.Parent.Primers)
                            oriTs = d.GetConjugationType(True)
                            For Each g As Nuctions.GeneFile In d.DNAs
                                If g.CanConjugate(oriTs) Then
                                    Dim gc As Nuctions.GeneFile = g.Clone
                                    If gc.Chromosomal Then gc.Chromosomal = False
                                    For Each t As Nuctions.Cell In tList
                                        t.Add(gc)
                                    Next
                                End If
                            Next
                        Next
                        Cells.Clear()
                        Cells.AddRange(tList)
                End Select
                Nuctions.AddFeatures(DNAs, Features, Me.GetParetntChartItem.Parent.Primers)
                For Each cl In Cells
                    Nuctions.AddFeatures(cl.DNAs, Features, Me.GetParetntChartItem.Parent.Primers)
                Next
                Calculated = True
            Case Nuctions.MolecularOperationEnum.Incubation
                Dim cList As New List(Of Nuctions.Cell)
                Dim dList As New List(Of DNAInfo)
                For Each s As DNAInfo In Source
                    Select Case s.MolecularOperation
                        Case Nuctions.MolecularOperationEnum.Host, Nuctions.MolecularOperationEnum.Incubation, Nuctions.MolecularOperationEnum.Transformation
                            cList.AddRange(s.Cells)
                        Case Nuctions.MolecularOperationEnum.Recombination, Nuctions.MolecularOperationEnum.Screen, Nuctions.MolecularOperationEnum.Enzyme, Nuctions.MolecularOperationEnum.HashPicker
                            If s.Cells.Count > 0 Then
                                cList.AddRange(s.Cells)
                            Else
                                dList.Add(s)
                            End If
                        Case Else
                            dList.Add(s)
                    End Select
                Next
                'remove the invalid source
                For Each s As DNAInfo In dList
                    Source.Remove(s)
                Next
                Dim inc As IncubationStep
                'Dim oriRs As List(Of String)
                Dim abe As List(Of Nuctions.Antibiotics)
                'Dim abs As New List(Of String)
                Dim tmp As Single
                Dim rList As New List(Of Nuctions.GeneFile)

                Dim nList As New List(Of Nuctions.Cell)
                Dim nCell As Nuctions.Cell

                For i As Integer = 0 To Incubation.Count - 1
                    nList.Clear()
                    inc = Incubation(i)
                    If Not Single.TryParse(Incubation(i).Temperature, tmp) Then tmp = 37
                    abe = Incubation(i).Antibiotics
                    For Each c As Nuctions.Cell In cList
                        If c IsNot Nothing AndAlso c.DNAs.Count > 0 Then
                            nCell = c.Incubate(abe, tmp, Features, inc.Time)
                            If nCell IsNot Nothing Then nList.Add(nCell)
                        End If
                    Next
                    cList.Clear()
                    cList.AddRange(Nuctions.ReduceDuplicateCells(nList))
                Next
                Cells.Clear()
                Dim fList As New List(Of Nuctions.GeneFile)
                For Each c As Nuctions.Cell In cList
                    fList.AddRange(c.DNAs)
                Next
                Nuctions.AddFeatures(fList, Features, Me.GetParetntChartItem.Parent.Primers)
                Cells.AddRange(cList)
                Calculated = True
            Case Nuctions.MolecularOperationEnum.Extraction
                Dim cList As New List(Of Nuctions.Cell)
                Dim dList As New List(Of DNAInfo)
                For Each s As DNAInfo In Source
                    Select Case s.MolecularOperation
                        Case Nuctions.MolecularOperationEnum.Host, Nuctions.MolecularOperationEnum.Incubation, Nuctions.MolecularOperationEnum.Transformation
                            cList.AddRange(s.Cells)
                        Case Nuctions.MolecularOperationEnum.Enzyme, Nuctions.MolecularOperationEnum.Recombination
                            If Cells.Count > 0 Then
                                cList.AddRange(s.Cells)
                            Else
                                dList.Add(s)
                            End If
                        Case Else
                            dList.Add(s)
                    End Select
                Next
                For Each s As DNAInfo In dList
                    Source.Remove(s)
                Next
                Dim results As New List(Of Nuctions.GeneFile)
                For Each c As Nuctions.Cell In cList
                    For Each gf As Nuctions.GeneFile In c.DNAs
                        If Not gf.Chromosomal Then results.Add(gf.CloneWithoutFeatures)
                    Next
                Next
                LoadResultDNAList(results)
                Calculated = True
            Case Nuctions.MolecularOperationEnum.GibsonDesign
                Dim Gibson = New GibsonDesignViewModel(Me)
                Gibson.cmdOptimize()
                Calculated = True
            Case Nuctions.MolecularOperationEnum.CRISPRCut
                CRISPRCut()
                Calculated = True
        End Select
#End If
    End Sub

    Public Sub DetermineHost(Hosts As IEnumerable(Of Nuctions.Host))
        Select Case vMolecularOperation
            Case Nuctions.MolecularOperationEnum.Host
                For Each h As Nuctions.Host In Hosts
                    If h.Name = "in vivo" Then Host = h
                Next
            Case Nuctions.MolecularOperationEnum.Recombination

            Case Nuctions.MolecularOperationEnum.Transformation
                For Each h As Nuctions.Host In Hosts
                    If h.Name = "in vivo" Then Host = h
                Next
            Case Nuctions.MolecularOperationEnum.Incubation
                For Each h As Nuctions.Host In Hosts
                    If h.Name = "in vivo" Then Host = h
                Next
        End Select
    End Sub
    ''' <summary>
    ''' 指示这个操作的Source当中是否有Cell
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsCellNode As Boolean
        Get
            For Each scr As DNAInfo In Source
                Select Case scr.vMolecularOperation
                    Case Nuctions.MolecularOperationEnum.Transformation, Nuctions.MolecularOperationEnum.Incubation, Nuctions.MolecularOperationEnum.Host, Nuctions.MolecularOperationEnum.Screen, Nuctions.MolecularOperationEnum.HashPicker, Nuctions.MolecularOperationEnum.Recombination
                        If scr.Cells.Count > 0 Then Return True
                End Select
            Next
            Return False
        End Get
    End Property
    Public Function GetSourceCellDNAList() As List(Of Nuctions.GeneFile)
        Dim cList As New List(Of Nuctions.GeneFile)
        For Each scr As DNAInfo In Source
            Select Case scr.vMolecularOperation
                Case Nuctions.MolecularOperationEnum.Transformation, Nuctions.MolecularOperationEnum.Incubation, Nuctions.MolecularOperationEnum.Host, Nuctions.MolecularOperationEnum.Screen, Nuctions.MolecularOperationEnum.HashPicker, Nuctions.MolecularOperationEnum.Recombination
                    For Each c As Nuctions.Cell In scr.Cells
                        For Each g As Nuctions.GeneFile In c.DNAs
                            cList.Add(g.Clone)
                        Next
                    Next
            End Select
        Next
        Return cList
    End Function
    Public Function GetSourceCellList() As List(Of Nuctions.Cell)
        Dim cList As New List(Of Nuctions.Cell)
        For Each scr As DNAInfo In Source
            Select Case scr.vMolecularOperation
                Case Nuctions.MolecularOperationEnum.Transformation, Nuctions.MolecularOperationEnum.Incubation, Nuctions.MolecularOperationEnum.Host, Nuctions.MolecularOperationEnum.Screen, Nuctions.MolecularOperationEnum.HashPicker, Nuctions.MolecularOperationEnum.Recombination
                    For Each c As Nuctions.Cell In scr.Cells
                        cList.Add(c.Clone)
                    Next
            End Select
        Next
        Return cList
    End Function
    Public Function GetSourceDNAList() As List(Of Nuctions.GeneFile)
        Dim scr As New List(Of Nuctions.GeneFile)
        For Each di As DNAInfo In Me.Source
            For Each gf As Nuctions.GeneFile In di.DNAs
                scr.Add(gf.CloneWithoutFeatures)
            Next
        Next
        Return scr
    End Function
    Public Function GetOriginalDNAList() As List(Of Nuctions.GeneFile)
        Dim scr As New List(Of Nuctions.GeneFile)
        For Each di As DNAInfo In Me.Source
            For Each gf As Nuctions.GeneFile In di.DNAs
                scr.Add(gf)
            Next
        Next
        Return scr
    End Function
    Public Sub LoadResultCellList(ByVal results As List(Of Nuctions.Cell))
        Cells.Clear()
        For Each c As Nuctions.Cell In results
            Nuctions.AddFeatures(c.DNAs, Features, Me.GetParetntChartItem.Parent.Primers)
            Cells.Add(c)
        Next
    End Sub
    Public Sub LoadResultDNAList(ByVal results As List(Of Nuctions.GeneFile), Optional AddFeature As Boolean = True)
        DNAs.Clear()
        If AddFeature Then Nuctions.AddFeatures(results, Features, Me.GetParetntChartItem.Parent.Primers)
        For Each gf As Nuctions.GeneFile In results
            DNAs.Add(gf)
        Next
    End Sub
    Public ReadOnly Property Features() As List(Of Nuctions.Feature)
        Get
            Return GetParetntChartItem.Feature
        End Get
    End Property
    Public FreeDesignCode As String = ""
    Public FreeDesignName As String = ""
    Public DesignedEnzymeSite As New List(Of String)
    Public PrimerDesignerMode As Boolean = False
    Public DesignedPrimers As New Dictionary(Of String, String)

    Public ReadOnly Property KeyName As String
        Get
            If IsKeyName Then
                Return Name
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public ReadOnly Property HasKeySource As Boolean
        Get
            For Each di As DNAInfo In Source
                If di.KeyName Is Nothing Then Return False
            Next
            Return True
        End Get
    End Property
    Public Shared Function DescribeSourceList(Source As List(Of DNAInfo)) As List(Of String)
        Dim producttypes As New List(Of Nuctions.MolecularOperationEnum)
        Dim splist As New List(Of String)
        Dim sourcedes As New List(Of String)
        For Each di As DNAInfo In Source
            If di.IsKeyName Then
                Select Case di.vMolecularOperation
                    Case Nuctions.MolecularOperationEnum.Host
                        splist.Add(di.Cells(0).Host.Name)
                    Case Else
                        splist.Add(di.KeyName)
                End Select
            Else
                Select Case di.MolecularOperation
                    Case Nuctions.MolecularOperationEnum.Host
                        splist.Add(di.Cells(0).Host.Name)
                    Case Nuctions.MolecularOperationEnum.Vector
                        splist.Add(di.Name)
                    Case Nuctions.MolecularOperationEnum.FreeDesign
                        splist.Add(di.FreeDesignName)
                    Case Nuctions.MolecularOperationEnum.Enzyme
                        If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Enzyme) Then
                            sourcedes.Add("the digestion product")
                            producttypes.Add(Nuctions.MolecularOperationEnum.Enzyme)
                        ElseIf sourcedes.Contains("the digestion product") Then
                            sourcedes.Remove("the digestion product")
                            sourcedes.Add("the digestion products")
                        End If
                    Case Nuctions.MolecularOperationEnum.Ligation
                        If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Ligation) Then
                            sourcedes.Add("the ligation product")
                            producttypes.Add(Nuctions.MolecularOperationEnum.Ligation)
                        ElseIf sourcedes.Contains("the ligation product") Then
                            sourcedes.Remove("the ligation product")
                            sourcedes.Add("the ligation products")
                        End If
                    Case Nuctions.MolecularOperationEnum.PCR
                        If Not producttypes.Contains(Nuctions.MolecularOperationEnum.PCR) Then
                            sourcedes.Add("the PCR product")
                            producttypes.Add(Nuctions.MolecularOperationEnum.PCR)
                        ElseIf sourcedes.Contains("the PCR product") Then
                            sourcedes.Remove("the PCR product")
                            sourcedes.Add("the PCR products")
                        End If
                    Case Nuctions.MolecularOperationEnum.Gel
                        If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Gel) Then
                            sourcedes.Add("the purification product")
                            producttypes.Add(Nuctions.MolecularOperationEnum.Gel)
                        ElseIf sourcedes.Contains("the purification product") Then
                            sourcedes.Remove("the purification product")
                            sourcedes.Add("the purification products")
                        End If
                    Case Nuctions.MolecularOperationEnum.Modify
                        If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Modify) Then
                            sourcedes.Add("the modification product")
                            producttypes.Add(Nuctions.MolecularOperationEnum.Modify)
                        ElseIf sourcedes.Contains("the modification product") Then
                            sourcedes.Remove("the modification product")
                            sourcedes.Add("the modification products")
                        End If
                    Case Nuctions.MolecularOperationEnum.Screen
                        If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Modify) Then
                            sourcedes.Add("the screening products")
                            producttypes.Add(Nuctions.MolecularOperationEnum.Modify)
                        ElseIf sourcedes.Contains("the screening product") Then
                            sourcedes.Remove("the screening product")
                            sourcedes.Add("the screening products")
                        End If
                    Case Nuctions.MolecularOperationEnum.Recombination
                        If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Modify) Then
                            sourcedes.Add("the recombination products")
                            producttypes.Add(Nuctions.MolecularOperationEnum.Modify)
                        ElseIf sourcedes.Contains("the recombination product") Then
                            sourcedes.Remove("the recombination product")
                            sourcedes.Add("the recombination products")
                        End If
                    Case Nuctions.MolecularOperationEnum.Incubation
                        If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Incubation) Then
                            If di.Incubation.Last.IsPlate Then
                                sourcedes.Add("the colonies")
                            Else
                                sourcedes.Add("the culture")
                            End If
                            producttypes.Add(Nuctions.MolecularOperationEnum.Incubation)
                        End If
                    Case Nuctions.MolecularOperationEnum.Transformation
                        If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Transformation) Then
                            sourcedes.Add("the transformation product")
                            producttypes.Add(Nuctions.MolecularOperationEnum.Transformation)
                        ElseIf sourcedes.Contains("the transformation product") Then
                            sourcedes.Remove("the transformation product")
                            sourcedes.Add("the transformation products")
                        End If
                    Case Nuctions.MolecularOperationEnum.Extraction
                        If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Extraction) Then
                            sourcedes.Add("the plasmid")
                            producttypes.Add(Nuctions.MolecularOperationEnum.Extraction)
                        ElseIf sourcedes.Contains("the plasmid") Then
                            sourcedes.Remove("the plasmid")
                            sourcedes.Add("the plasmids")
                        End If
                End Select
            End If
        Next
        Dim vlist As New List(Of String)
        Dim c As Integer = splist.Count + sourcedes.Count
        Dim i As Integer = 0
        For Each s As String In splist
            i += 1
            If i > 1 And i = c Then vlist.Add("and")
            vlist.Add("")
            vlist.Add(s)
        Next
        For Each s As String In sourcedes
            i += 1
            If i > 1 And i = c Then vlist.Add("and")
            vlist.Add(s)
        Next
        Return vlist
    End Function
    Public Shared Function DescribeSource(Source As List(Of DNAInfo)) As String
        Dim stb As New System.Text.StringBuilder
        Select Case RegionalLanguage
            Case Language.English
                Dim producttypes As New List(Of Nuctions.MolecularOperationEnum)
                Dim sourcedes As New List(Of String)
                For Each di As DNAInfo In Source
                    If di.IsKeyName Then
                        sourcedes.Add(di.KeyName)
                    Else
                        Select Case di.MolecularOperation
                            Case Nuctions.MolecularOperationEnum.Host
                                sourcedes.Add(di.Cells(0).Host.Name)
                            Case Nuctions.MolecularOperationEnum.Vector
                                sourcedes.Add(di.Name)
                            Case Nuctions.MolecularOperationEnum.FreeDesign
                                sourcedes.Add(di.FreeDesignName)
                            Case Nuctions.MolecularOperationEnum.Enzyme
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Enzyme) Then
                                    sourcedes.Add("the digestion product")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Enzyme)
                                ElseIf sourcedes.Contains("the digestion product") Then
                                    sourcedes.Remove("the digestion product")
                                    sourcedes.Add("the digestion products")
                                End If
                            Case Nuctions.MolecularOperationEnum.Ligation
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Ligation) Then
                                    sourcedes.Add("the ligation product")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Ligation)
                                ElseIf sourcedes.Contains("the ligation product") Then
                                    sourcedes.Remove("the ligation product")
                                    sourcedes.Add("the ligation products")
                                End If
                            Case Nuctions.MolecularOperationEnum.PCR
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.PCR) Then
                                    sourcedes.Add("the PCR product")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.PCR)
                                ElseIf sourcedes.Contains("the PCR product") Then
                                    sourcedes.Remove("the PCR product")
                                    sourcedes.Add("the PCR products")
                                End If
                            Case Nuctions.MolecularOperationEnum.Gel
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Gel) Then
                                    sourcedes.Add("the separation product")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Gel)
                                ElseIf sourcedes.Contains("the separation product") Then
                                    sourcedes.Remove("the separation product")
                                    sourcedes.Add("the separation products")
                                End If
                            Case Nuctions.MolecularOperationEnum.Modify
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Modify) Then
                                    sourcedes.Add("the modification product")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Modify)
                                ElseIf sourcedes.Contains("the modification product") Then
                                    sourcedes.Remove("the modification product")
                                    sourcedes.Add("the modification products")
                                End If
                            Case Nuctions.MolecularOperationEnum.Screen
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Modify) Then
                                    sourcedes.Add("the screening products")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Modify)
                                ElseIf sourcedes.Contains("the screening product") Then
                                    sourcedes.Remove("the screening product")
                                    sourcedes.Add("the screening products")
                                End If
                            Case Nuctions.MolecularOperationEnum.Recombination
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Modify) Then
                                    sourcedes.Add("the recombination products")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Modify)
                                ElseIf sourcedes.Contains("the recombination product") Then
                                    sourcedes.Remove("the recombination product")
                                    sourcedes.Add("the recombination products")
                                End If
                            Case Nuctions.MolecularOperationEnum.Incubation
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Incubation) Then
                                    If di.Incubation.Last.IsPlate Then
                                        sourcedes.Add("the colonies")
                                    Else
                                        sourcedes.Add("the culture")
                                    End If
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Incubation)
                                End If
                            Case Nuctions.MolecularOperationEnum.Transformation
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Transformation) Then
                                    sourcedes.Add("the transformation product")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Transformation)
                                ElseIf sourcedes.Contains("the transformation product") Then
                                    sourcedes.Remove("the transformation product")
                                    sourcedes.Add("the transformation products")
                                End If
                            Case Nuctions.MolecularOperationEnum.Extraction
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Extraction) Then
                                    sourcedes.Add("the plasmid")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Extraction)
                                ElseIf sourcedes.Contains("the plasmid") Then
                                    sourcedes.Remove("the plasmid")
                                    sourcedes.Add("the plasmids")
                                End If
                        End Select
                    End If
                Next
                Dim i As Integer = 0
                stb.Append(FacilityFunctions.DescribeStringList(sourcedes, Language.Chinese))
            Case Language.Chinese

                Dim producttypes As New List(Of Nuctions.MolecularOperationEnum)
                Dim sourcedes As New List(Of String)
                For Each di As DNAInfo In Source
                    If di.IsKeyName Then
                        sourcedes.Add(di.KeyName)
                    Else
                        Select Case di.MolecularOperation
                            Case Nuctions.MolecularOperationEnum.Host
                                sourcedes.Add(di.Cells(0).Host.Name)
                            Case Nuctions.MolecularOperationEnum.Vector
                                sourcedes.Add(di.Name)
                            Case Nuctions.MolecularOperationEnum.FreeDesign
                                sourcedes.Add(di.FreeDesignName)
                            Case Nuctions.MolecularOperationEnum.Enzyme
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Enzyme) Then
                                    sourcedes.Add("酶切产物")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Enzyme)
                                End If
                            Case Nuctions.MolecularOperationEnum.Ligation
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Ligation) Then
                                    sourcedes.Add("连接产物")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Ligation)
                                End If
                            Case Nuctions.MolecularOperationEnum.PCR
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.PCR) Then
                                    sourcedes.Add("PCR产物")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.PCR)
                                End If
                            Case Nuctions.MolecularOperationEnum.Gel
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Gel) Then
                                    sourcedes.Add("分离产物")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Gel)
                                End If
                            Case Nuctions.MolecularOperationEnum.Modify
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Modify) Then
                                    sourcedes.Add("处理产物")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Modify)
                                End If
                            Case Nuctions.MolecularOperationEnum.Screen
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Modify) Then
                                    sourcedes.Add("筛选产物")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Modify)
                                End If
                            Case Nuctions.MolecularOperationEnum.Recombination
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Modify) Then
                                    sourcedes.Add("重组产物")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Modify)
                                End If
                            Case Nuctions.MolecularOperationEnum.Incubation
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Incubation) Then
                                    If di.Incubation.Last.IsPlate Then
                                        sourcedes.Add("长出的克隆")
                                    Else
                                        sourcedes.Add("菌液")
                                    End If
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Incubation)
                                End If
                            Case Nuctions.MolecularOperationEnum.Transformation
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Transformation) Then
                                    sourcedes.Add("转化产物")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Transformation)
                                End If
                            Case Nuctions.MolecularOperationEnum.Extraction
                                If Not producttypes.Contains(Nuctions.MolecularOperationEnum.Extraction) Then
                                    sourcedes.Add("质粒")
                                    producttypes.Add(Nuctions.MolecularOperationEnum.Extraction)
                                End If
                        End Select
                    End If
                Next
                Dim i As Integer = 0
                stb.Append(FacilityFunctions.DescribeStringList(sourcedes, Language.Chinese))
        End Select
        Return stb.ToString
    End Function
    Public ReadOnly Property SourceKeyDescription As String
        Get
            Return DescribeSource(Source)
        End Get
    End Property
    Public ReadOnly Property LabelName As String
        Get
            Select Case RegionalLanguage
                Case Language.English
                    If MolecularOperation = Nuctions.MolecularOperationEnum.Compare Then
                        Return String.Format("Alignment {0}", Me.GetParetntChartItem.Index.ToString)
                    Else
                        Select Case DescribeType
                            Case DescribeEnum.Vecotor
                                Return IIf(DNAs.Count > 1, "DNA Mixture ", "DNA Molecule ") + Me.GetParetntChartItem.Index.ToString
                            Case DescribeEnum.Chromosome
                                Return IIf(DNAs.Count > 1, "Colony Mixture ", "Clone ") + Me.GetParetntChartItem.Index.ToString
                            Case Else
                                Return IIf(DNAs.Count > 1, "DNA Mixture ", "DNA Molecule ") + Me.GetParetntChartItem.Index.ToString
                        End Select
                    End If
                Case Language.Chinese
                    If MolecularOperation = Nuctions.MolecularOperationEnum.Compare Then
                        Return String.Format("序列比对结果{0}", Me.GetParetntChartItem.Index.ToString)
                    Else
                        Select Case DescribeType
                            Case DescribeEnum.Vecotor
                                Return IIf(DNAs.Count > 1, "DNA混合物", "DNA分子") + Me.GetParetntChartItem.Index.ToString
                            Case DescribeEnum.Chromosome
                                Return IIf(DNAs.Count > 1, "混合克隆", "克隆") + Me.GetParetntChartItem.Index.ToString
                            Case Else
                                Return IIf(DNAs.Count > 1, "DNA混合物", "DNA分子") + Me.GetParetntChartItem.Index.ToString
                        End Select
                    End If
            End Select
        End Get
    End Property
    Public ReadOnly Property SourceDescribeType As DescribeEnum
        Get
            Dim dt As Integer = 0
            For Each di As DNAInfo In Source
                dt = IIf(dt > di.DescribeType, dt, di.DescribeType)
            Next
            Return dt
        End Get
    End Property
    Public ReadOnly Property DNADescription() As String
        Get
            Dim stb As New System.Text.StringBuilder
            Select Case RegionalLanguage
                Case Language.English
                    Dim i As Integer = 0
                    For Each gf As Nuctions.GeneFile In DNAs
                        i += 1
                        stb.AppendFormat("""{0}""", DNAs(i).Name)
                        If i < DNAs.Count - 1 Then
                            stb.Append(", ")
                        ElseIf i = DNAs.Count - 1 Then
                            stb.Append(" and ")
                        ElseIf i = DNAs.Count Then
                            'stb.Append("，")
                        End If
                        stb.AppendFormat(" with the length of {0}bp", CType(DNAs(i), Nuctions.GeneFile).Length.ToString)
                    Next
                Case Language.Chinese
                    Dim i As Integer = 0
                    For Each gf As Nuctions.GeneFile In DNAs
                        i += 1
                        stb.AppendFormat("长度为{0}bp的", CType(DNAs(i), Nuctions.GeneFile).Length.ToString)
                        stb.Append(DNAs(i).Name)
                        If i < DNAs.Count - 1 Then
                            stb.Append("、")
                        ElseIf i = DNAs.Count - 1 Then
                            stb.Append("和")
                        ElseIf i = DNAs.Count Then
                            'stb.Append("，")
                        End If
                    Next
            End Select
            Return stb.ToString
        End Get
    End Property
    Public ReadOnly Property SourceDescription As String
        Get
            Dim stb As New System.Text.StringBuilder
            Select Case RegionalLanguage
                Case Language.English
                    Dim i As Integer = 0
                    For Each di As DNAInfo In Source
                        i += 1
                        stb.Append(di.LabelName)
                        If i < Source.Count - 1 Then
                            stb.Append(", ")
                        ElseIf i = Source.Count - 1 Then
                            stb.Append(" and ")
                        ElseIf i = Source.Count Then
                            'stb.Append("，")
                        End If
                    Next
                Case Language.Chinese
                    Dim i As Integer = 0
                    For Each di As DNAInfo In Source
                        i += 1
                        stb.Append(di.LabelName)
                        If i < Source.Count - 1 Then
                            stb.Append("、")
                        ElseIf i = Source.Count - 1 Then
                            stb.Append("和")
                        ElseIf i = Source.Count Then
                            'stb.Append("，")
                        End If
                    Next
            End Select
            Return stb.ToString
        End Get
    End Property
    Public ReadOnly Property Description(SynthesisList As List(Of DNAInfo)) As String
        Get
            Select Case RegionalLanguage
                Case Language.English
                    Return GetShortEnglishDescription(SynthesisList)
                Case Language.Chinese
                    Return GetShortChineseDescription(SynthesisList)
                Case Else
                    Return ""
            End Select
        End Get
    End Property
    Private Function GetShortEnglishDescription(SynthesisList As List(Of DNAInfo)) As String
        Dim stb As New EnglishSentenceConstructor
        Select Case vMolecularOperation
            Case Nuctions.MolecularOperationEnum.Vector
            Case Nuctions.MolecularOperationEnum.Modify
                stb.Append(DescribeSourceList(Source))
                stb.PastBe(Source)
                Select Case Modify_Method
                    Case Nuctions.ModificationMethodEnum.CIAP
                        stb.Append("dephosphorylated", "by", "CIAP")
                    Case Nuctions.ModificationMethodEnum.T4DNAP
                        stb.Append("blunted", "by", "T4 DNA polymerase")
                    Case Nuctions.ModificationMethodEnum.Klewnow
                        stb.Append("blunted", "by", "Klewnow fragment")
                    Case Nuctions.ModificationMethodEnum.PNK
                        stb.Append("phosphorylated", "by", "PNK")
                End Select
                If IsKeyName Then
                    stb.Comma()
                    stb.Append("and labeled as")
                    stb.Append(KeyName)
                    stb.Period()
                Else
                    stb.Period()
                End If
            Case Nuctions.MolecularOperationEnum.Enzyme
                stb.Append(DescribeSourceList(Source))
                stb.PastBe(Source)
                stb.Append("digested", "by")
                Dim i As Integer = 0
                For Each ez As String In Enzyme_Enzymes
                    i += 1
                    stb.Append(ez)
                    If i < Enzyme_Enzymes.Count - 1 Then
                        stb.Comma()
                    ElseIf i = Enzyme_Enzymes.Count - 1 Then
                        stb.Append("and")
                    ElseIf i = Enzyme_Enzymes.Count Then
                    End If
                Next
                If IsConstructionNode Then
                    stb.Period()
                    stb.Append("Digestion")
                    stb.Plural("product", DNAs.Count)
                    stb.Multiple(DescribeMultipleDNASize(DNAs))
                    stb.Be(DNAs)
                    stb.Append("observed in gel electrophoresis")
                End If
                If IsKeyName Then
                    If DephosphorylateWhenDigestion Then stb.Comma() : stb.Append("dephospherylated by CIAP")
                    stb.Comma()
                    stb.Append("and labeled as")
                    stb.Append(KeyName)
                    stb.Period()
                Else
                    If DephosphorylateWhenDigestion Then stb.Append("and dephospherylated by CIAP")
                    stb.Period()
                End If
            Case Nuctions.MolecularOperationEnum.PCR
                SynthesisList.Add(Me)
                stb.Append("A PCR was performed with")
                stb.Append(DescribeSourceList(Source))
                stb.Append("as the template")
                stb.Append("using", PCR_FPrimerName, "and", PCR_RPrimerName, "as the primers")
                If IsConstructionNode Then
                    stb.Period()
                    stb.Append("PCR")
                    stb.Plural("product", DNAs.Count)
                    stb.Multiple(DescribeMultipleDNASize(DNAs))
                    stb.Be(DNAs)
                    stb.Append("observed in gel electrophoresis")
                End If
                If IsKeyName Then
                    stb.Period()
                    stb.Append("The PCR product was labeled as")
                    stb.Append(KeyName)
                    stb.Period()
                Else
                    stb.Period()
                End If
            Case Nuctions.MolecularOperationEnum.Ligation
                stb.Append(DescribeSourceList(Source))
                stb.PastBe(Source)
                stb.Append("ligated by T4 DNA ligase")
                If IsKeyName Then
                    stb.Comma()
                    stb.Append("and labeled as")
                    stb.Append(KeyName)
                    stb.Period()
                Else
                    stb.Period()
                End If
            Case Nuctions.MolecularOperationEnum.Gel
                If SolutionExtration Then
                    stb.Append(DescribeSourceList(Source))
                    stb.PastBe(Source)
                    stb.Append("purified by solution purification")
                Else
                    If Gel_Minimum = Gel_Maximun Then
                        stb.Append("fragments", "of", Gel_Minimum.ToString, "bp", "were", "purified", "by", "argarose", "gel", "electrophoresis", "from")
                    Else
                        stb.Append("fragments", "between", Gel_Minimum.ToString, "bp", "to", Gel_Maximun.ToString, "bp", "were", "purified", "by", "argarose", "gel", "electrophoresis", "from")
                    End If
                    stb.Append(SourceKeyDescription)
                End If
                If IsKeyName Then
                    stb.Comma()
                    stb.Append("and labeled as")
                    stb.Append(KeyName)
                    stb.Period()
                Else
                    stb.Period()
                End If
            Case Nuctions.MolecularOperationEnum.Screen
                If IsCellNode Then
                    Select Case Screen_Mode
                        Case Nuctions.ScreenModeEnum.PCR
                            SynthesisList.Add(Me)
                            If Screen_PCRMin = Screen_PCRMax Then
                                stb.Append("colonies that produce", Screen_PCRMin.ToString, "bp fragment were selected from")
                                stb.Append(DescribeSourceList(Source))
                                stb.Append("by PCR with the primers of", Screen_FName, "and", Screen_RName)
                            Else
                                stb.Append("colonies that produced one or more fragments between", Screen_PCRMax.ToString, "bp", "and", Screen_PCRMin.ToString, "were selected from")
                                stb.Append(DescribeSourceList(Source))
                                stb.Append("by PCR with the primers of", Screen_FName, "and", Screen_RName)
                            End If
                        Case Nuctions.ScreenModeEnum.Features

                    End Select
                Else
                    Select Case Screen_Mode
                        Case Nuctions.ScreenModeEnum.PCR
                            SynthesisList.Add(Me)
                            If Screen_PCRMin = Screen_PCRMax Then
                                stb.Append("products that produce", Screen_PCRMin.ToString, "bp fragment were selected from")
                                stb.Append(DescribeSourceList(Source))
                                stb.Append("by PCR with the primers of", Screen_FName, "and", Screen_RName)
                            Else
                                stb.Append("products that produced one or more fragments between", Screen_PCRMax.ToString, "bp", "and", Screen_PCRMin.ToString, "were selected from")
                                stb.Append(DescribeSourceList(Source))
                                stb.Append("by PCR with the primers of", Screen_FName, "and", Screen_RName)
                            End If
                        Case Nuctions.ScreenModeEnum.Features

                    End Select
                End If
                If IsKeyName Then
                    stb.Comma()
                    stb.Append("and labeled as")
                    stb.Append(KeyName)
                    stb.Period()
                Else
                    stb.Period()
                End If
            Case Nuctions.MolecularOperationEnum.HashPicker
                stb.Append("one clone was selected from")
                stb.Append(DescribeSourceList(Source))
                stb.Append("randomly")
                If IsKeyName Then
                    stb.Comma()
                    stb.Append("and labeled as")
                    stb.Append(KeyName)
                    stb.Period()
                Else
                    stb.Period()
                End If
            Case Nuctions.MolecularOperationEnum.FreeDesign
                SynthesisList.Add(Me)
            Case Nuctions.MolecularOperationEnum.Recombination
                Dim inCell As Boolean = Cells.Count > 0
                If inCell Then
                    Select Case RecombinationMethod
                        Case MCDS.RecombinationMethod.LambdaRecombination
                            stb.Append(DescribeSourceList(Source))
                            stb.PastBe(Source)
                            stb.Append("induced to perform λ Red recombination")
                        Case MCDS.RecombinationMethod.LambdaAttBP
                            stb.Append(DescribeSourceList(Source))
                            stb.PastBe(Source)
                            stb.Append("induced to perform λ BP recombination")
                        Case MCDS.RecombinationMethod.LambdaAttLR
                            stb.Append(DescribeSourceList(Source))
                            stb.PastBe(Source)
                            stb.Append("induced to perform λ LR recombination")
                        Case MCDS.RecombinationMethod.HK022AttBP
                            stb.Append(DescribeSourceList(Source))
                            stb.PastBe(Source)
                            stb.Append("induced to perform HK022 BP recombination")
                        Case MCDS.RecombinationMethod.HK022AttLR
                            stb.Append(DescribeSourceList(Source))
                            stb.PastBe(Source)
                            stb.Append("induced to perform HK022 LR recombination")
                        Case MCDS.RecombinationMethod.Phi80AttBP
                            stb.Append(DescribeSourceList(Source))
                            stb.PastBe(Source)
                            stb.Append("induced to perform φ80 BP recombination")
                        Case MCDS.RecombinationMethod.Phi80AttLR
                            stb.Append(DescribeSourceList(Source))
                            stb.PastBe(Source)
                            stb.Append("induced to perform φ80 LR recombination")
                        Case MCDS.RecombinationMethod.FRT
                            stb.Append(DescribeSourceList(Source))
                            stb.PastBe(Source)
                            stb.Append("induced to perform frt recombination")
                        Case MCDS.RecombinationMethod.LoxP
                            stb.Append(DescribeSourceList(Source))
                            stb.PastBe(Source)
                            stb.Append("induced to perform loxP recombination")
                        Case MCDS.RecombinationMethod.telRLSplit
                            stb.Append(DescribeSourceList(Source))
                            stb.PastBe(Source)
                            stb.Append("induced to form telomeres")
                    End Select
                    If IsKeyName Then
                        stb.Comma()
                        stb.Append("and the recombination product labeled as")
                        stb.Append(KeyName)
                        stb.Period()
                    Else
                        stb.Period()
                    End If
                Else
                    Select Case RecombinationMethod
                        Case MCDS.RecombinationMethod.LambdaRecombination
                            stb.Append("λ Red recombinase was used to catalyze the λ Red recombination in")
                            stb.Append(DescribeSourceList(Source))
                        Case MCDS.RecombinationMethod.LambdaAttBP
                            stb.Append("λ Int recombinase and IFH were used to catalyze the λ BP recombination in")
                            stb.Append(DescribeSourceList(Source))
                        Case MCDS.RecombinationMethod.LambdaAttLR
                            stb.Append("λ Int, Xis recombinase and IFH were used to catalyze the λ LR recombination in")
                            stb.Append(DescribeSourceList(Source))
                        Case MCDS.RecombinationMethod.HK022AttBP
                            stb.Append("HK022 Int recombinase and IFH were used to catalyze the HK022 BP recombination in")
                            stb.Append(DescribeSourceList(Source))
                        Case MCDS.RecombinationMethod.HK022AttLR
                            stb.Append("HK022 Int, Xis recombinase and IFH were used to catalyze the HK022 LR recombination in")
                            stb.Append(DescribeSourceList(Source))
                        Case MCDS.RecombinationMethod.Phi80AttBP
                            stb.Append("Phi80 Int recombinase and IFH were used to catalyze the Phi80 BP recombination in")
                            stb.Append(DescribeSourceList(Source))
                        Case MCDS.RecombinationMethod.Phi80AttLR
                            stb.Append("Phi80 Int, Xis recombinase and IFH were used to catalyze the Phi80 LR recombination in")
                            stb.Append(DescribeSourceList(Source))
                        Case MCDS.RecombinationMethod.FRT
                            stb.Append("FLP recombinase were used to catalyze the frt recombination in")
                            stb.Append(DescribeSourceList(Source))
                        Case MCDS.RecombinationMethod.LoxP
                            stb.Append("Cre recombinase were used to catalyze the loxP recombination in")
                            stb.Append(DescribeSourceList(Source))
                        Case MCDS.RecombinationMethod.telRLSplit
                            stb.Append("TelN telomerase was used to catalyze the formation of telomere in")
                            stb.Append(DescribeSourceList(Source))
                    End Select
                    If IsKeyName Then
                        stb.Comma()
                        stb.Append("and the recombination product was labeled as")
                        stb.Append(KeyName)
                        stb.Period()
                    Else
                        stb.Period()
                    End If
                End If
            Case Nuctions.MolecularOperationEnum.SequencingResult
                SynthesisList.Add(Me)
            Case Nuctions.MolecularOperationEnum.Host
            Case Nuctions.MolecularOperationEnum.Transformation
                Dim DNASource As New List(Of DNAInfo)
                Dim CellSource As New List(Of DNAInfo)
                For Each sc As DNAInfo In Source
                    Select Case sc.vMolecularOperation
                        Case Nuctions.MolecularOperationEnum.Host, Nuctions.MolecularOperationEnum.Incubation, Nuctions.MolecularOperationEnum.Transformation
                            CellSource.Add(sc)
                        Case Else
                            DNASource.Add(sc)
                    End Select
                Next
                Select Case TransformationMethod
                    Case Nuctions.TransformationMethod.Electroporation
                        stb.Append(DescribeSourceList(DNASource))
                        stb.PastBe(DNASource)
                        stb.Append("transformed into")
                        stb.Append(DescribeSourceList(CellSource))
                        stb.Append("cells")
                        stb.Append("by electroporation")
                    Case Nuctions.TransformationMethod.ChemicalTransformation
                        stb.Append(DescribeSourceList(DNASource))
                        stb.PastBe(DNASource)
                        stb.Append("transformed into")
                        stb.Append(DescribeSourceList(CellSource))
                        stb.Append("cells")
                        stb.Append("by chemical transformation")
                    Case Nuctions.TransformationMethod.Conjugation
                        stb.Append("Conjugation was performed in")
                        stb.Append(DescribeSourceList(CellSource))
                End Select
                If IsKeyName Then
                    stb.Comma()
                    stb.Append("and the transformants were labeled as")
                    stb.Append(KeyName)
                    stb.Period()
                Else
                    stb.Period()
                End If
            Case Nuctions.MolecularOperationEnum.Incubation
                Dim lasticb As IncubationStep = Nothing
                Dim icb As IncubationStep
                For j As Integer = 0 To Incubation.Count - 1
                    icb = Incubation(j)
                    If lasticb Is Nothing Then
                        stb.Append(DescribeSourceList(Source))
                        stb.PastBe(Source)
                    ElseIf icb.Medium = "-" Then
                        If j < Incubation.Count - 1 AndAlso Incubation(j + 1).Medium = "-" Then
                            stb.Append("then")
                        ElseIf j = Incubation.Count - 1 Then
                            stb.Append("and then")
                        ElseIf j < Incubation.Count - 1 AndAlso Incubation(j + 1).Medium <> "-" Then
                            stb.Append("and finally")
                        End If
                    ElseIf lasticb.IsPlate Then
                        stb.Append("the colonies from the plates were")
                    Else
                        stb.Append("the cell culture was")
                    End If
                    If lasticb Is Nothing AndAlso Source.Count = 1 AndAlso Source(0).vMolecularOperation = Nuctions.MolecularOperationEnum.Transformation AndAlso icb.Antibiotics.Count = 0 AndAlso icb.Time <> TimeSpan.Zero Then
                        '针对于转化
                        stb.Append("inoculated")
                        stb.Append("into")
                        stb.Append(icb.Medium)
                        stb.Append("medium")
                        stb.Comma()
                        stb.Append("and incubated at")
                        stb.Append(icb.Temperature)
                        stb.Append("°C")
                        If icb.Time < onehour Then
                            stb.Append("for")
                            stb.Append(icb.Time.Minutes.ToString)
                            stb.Plural("minute", icb.Time.Minutes)
                        Else
                            If icb.Time.Minutes = 0 Then
                                stb.Append("for")
                                stb.Append(icb.Time.Hours.ToString)
                                stb.Plural("hour", icb.Time.Hours)
                            Else
                                stb.Append("for")
                                stb.Append(icb.Time.Hours.ToString)
                                stb.Plural("hour", icb.Time.Hours)
                                stb.Append(icb.Time.Minutes.ToString)
                                stb.Plural("minute", icb.Time.Minutes)
                            End If
                        End If
                        stb.Period()
                    ElseIf lasticb IsNot Nothing AndAlso icb.Medium = "-" Then
                        '这个设定是更换培养温度
                        stb.Append("incubated at")
                        stb.Append(icb.Temperature)
                        stb.Append("°C")
                        If icb.Time = TimeSpan.Zero Then
                            If icb.IsPlate Then
                                stb.Append("until individual colonies became visible")
                            Else
                                stb.Append("until the culture medium became turbid")
                            End If
                        Else
                            If icb.Time < onehour Then
                                stb.Append("for")
                                stb.Append(icb.Time.Minutes.ToString)
                                stb.Plural("minute", icb.Time.Minutes)
                            Else
                                If icb.Time.Minutes = 0 Then
                                    stb.Append("for")
                                    stb.Append(icb.Time.Hours.ToString)
                                    stb.Plural("hour", icb.Time.Hours)
                                Else
                                    stb.Append("for")
                                    stb.Append(icb.Time.Hours.ToString)
                                    stb.Plural("hour", icb.Time.Hours)
                                    stb.Append(icb.Time.Minutes.ToString)
                                    stb.Plural("minute", icb.Time.Minutes)
                                End If
                            End If
                        End If
                        If j < Incubation.Count - 1 AndAlso Incubation(j + 1).Medium = "-" Then
                            stb.Comma()
                        Else
                            stb.Period()
                        End If
                    Else
                        If icb.IsPlate Then
                            stb.Append("sprayed")
                            stb.Append("onto")
                        Else
                            stb.Append("inoculated")
                            stb.Append("into")
                        End If
                        stb.Append(icb.Medium)
                        If icb.IsPlate Then
                            stb.Append("agar medium")
                        Else
                            stb.Append("medium")
                        End If
                        If icb.Antibiotics.Count > 0 Then
                            stb.Append("with")
                            stb.Append(Nuctions.DescribeAntibiotics(icb.Antibiotics, Language.English).ToLower)
                        End If
                        If icb.Inducer IsNot Nothing And icb.Inducer.Length > 0 Then
                            stb.Comma()
                            stb.Append("induced")
                            stb.Append("by")
                            stb.Append(icb.Inducer)
                        End If
                        stb.Append("and incubated at")
                        stb.Append(icb.Temperature)
                        stb.Append("°C")
                        If icb.Time = TimeSpan.Zero Then
                            If icb.IsPlate Then
                                stb.Append("until individual colonies became visible")
                            Else
                                stb.Append("until the culture medium became turbid")
                            End If
                        Else
                            If icb.Time < onehour Then
                                stb.Append("for")
                                stb.Append(icb.Time.Minutes.ToString)
                                stb.Plural("minute", icb.Time.Minutes)
                            Else
                                If icb.Time.Minutes = 0 Then
                                    stb.Append("for")
                                    stb.Append(icb.Time.Hours.ToString)
                                    stb.Plural("hour", icb.Time.Hours)
                                Else
                                    stb.Append("for")
                                    stb.Append(icb.Time.Hours.ToString)
                                    stb.Plural("hour", icb.Time.Hours)
                                    stb.Append(icb.Time.Minutes.ToString)
                                    stb.Plural("minute", icb.Time.Minutes)
                                End If
                            End If
                        End If
                        stb.Period()
                    End If
                    lasticb = icb
                Next
                If IsKeyName Then
                    If lasticb.IsPlate Then
                        stb.Append("the colonies were labeled as")
                    Else
                        stb.Append("the culture was labeled as")
                    End If
                    stb.Append(KeyName)
                    stb.Period()
                End If
            Case Nuctions.MolecularOperationEnum.Extraction
                stb.Append("plasmids were extracted from")
                stb.Append(DescribeSourceList(Source))
                If IncludeVerification Then
                    stb.Period()
                    Dim IsFirstVer As Boolean = True
                    For Each ci As ChartItem In Me.GetParetntChartItem.Parent.Items
                        If ci.MolecularInfo.Source.Contains(Me) And ci.MolecularInfo.IsVerificationStep Then
                            Dim cdi As DNAInfo = ci.MolecularInfo
                            Select Case cdi.vMolecularOperation
                                Case Nuctions.MolecularOperationEnum.PCR
                                    SynthesisList.Add(cdi)
                                    stb.Append("a PCR screening was performed with the primers of", cdi.PCR_FPrimerName, "and", cdi.PCR_RPrimerName)
                                    stb.Period()
                                    stb.Append("the plasmids that produced")
                                    If DNAs.Count > 0 Then
                                        Dim shortest As Integer = Integer.MaxValue
                                        For Each gf As Nuctions.GeneFile In cdi.DNAs
                                            If shortest > gf.Length Then shortest = gf.Length
                                        Next
                                        stb.Append("fragments of")
                                        stb.Append(shortest.ToString)
                                        stb.Append("bp")
                                    Else
                                        stb.Append("no fragment")
                                    End If
                                    stb.Append("in PCR", "were selected from the")
                                    If IsFirstVer Then
                                        stb.Append("purified")
                                        IsFirstVer = False
                                    Else
                                        stb.Append("previously selected")
                                    End If
                                    stb.Append("plasmids")
                                    stb.Period()
                                Case Nuctions.MolecularOperationEnum.Enzyme
                                    stb.Append("a restriction enzyme digestion screening was performed with")
                                    stb.Append(FacilityFunctions.DescribeStringList(cdi.Enzyme_Enzymes, Language.English))
                                    stb.Period()
                                    stb.Append("the plasmids that produced")
                                    Dim products As New List(Of String)
                                    For Each gf As Nuctions.GeneFile In cdi.DNAs
                                        products.Add(gf.Length.ToString + "bp")
                                    Next
                                    stb.Append(FacilityFunctions.DescribeStringList(products, Language.English))
                                    stb.Plural("fragment", products)
                                    stb.Append("were selected from the")
                                    If IsFirstVer Then
                                        stb.Append("purified")
                                        IsFirstVer = False
                                    Else
                                        stb.Append("previously selected")
                                    End If
                                    stb.Append("plasmids")
                                    stb.Period()
                            End Select
                        End If
                    Next
                    If SequencingVerify Then
                        stb.Append("The selected plasmids were also verified by sequencing")
                    End If
                Else
                    If SequencingVerify Then
                        stb.Append("and verified by sequencing")
                    End If
                End If

                If IsKeyName Then
                    If SequencingVerify Or IncludeVerification Then
                        stb.Period()
                        stb.Append("the correct plasmids were")
                    Else
                        stb.Comma()
                        stb.Append("and")
                    End If
                    stb.Append("labeled as")
                    stb.Append(KeyName)
                    stb.Period()
                Else
                    stb.Period()
                End If
        End Select
        Return stb.ToString()
    End Function
    Private Function GetShortChineseDescription(SynthesisList As List(Of DNAInfo)) As String
        Dim stb As New System.Text.StringBuilder
        Select Case vMolecularOperation
            Case Nuctions.MolecularOperationEnum.Vector
            Case Nuctions.MolecularOperationEnum.Modify
                stb.Append("将")
                stb.Append(SourceKeyDescription)
                Select Case Modify_Method
                    Case Nuctions.ModificationMethodEnum.CIAP
                        stb.Append("用碱式磷酸酶处理")
                    Case Nuctions.ModificationMethodEnum.T4DNAP
                        stb.Append("用T4 DNA聚合酶处理")
                    Case Nuctions.ModificationMethodEnum.Klewnow
                        stb.Append("用Klewnow聚合酶处理")
                    Case Nuctions.ModificationMethodEnum.PNK
                        stb.Append("用多磷酸激酶处理")
                End Select
                stb.Append("，")
                If IsKeyName Then
                    stb.Append("产物标记为")
                    stb.Append(KeyName)
                    stb.Append("。")
                End If
            Case Nuctions.MolecularOperationEnum.Enzyme
                stb.Append("用限制内切酶")
                Dim i As Integer = 0
                For Each ez As String In Enzyme_Enzymes
                    i += 1
                    stb.Append(ez)
                    If i < Enzyme_Enzymes.Count - 1 Then
                        stb.Append("、")
                    ElseIf i = Enzyme_Enzymes.Count - 1 Then
                        stb.Append("和")
                    ElseIf i = Enzyme_Enzymes.Count Then
                        'stb.Append("，")
                    End If
                Next
                stb.Append("对")
                stb.Append(SourceKeyDescription)
                stb.Append("作酶切，")
                If DephosphorylateWhenDigestion Then stb.Append("并加入CIAP去磷酸化处理，")
                If IsKeyName Then
                    stb.Append("产物标记为")
                    stb.Append(KeyName)
                    stb.Append("。")
                End If
            Case Nuctions.MolecularOperationEnum.PCR
                SynthesisList.Add(Me)
                stb.Append("以")
                stb.Append(SourceKeyDescription)
                stb.Append("为模板")
                stb.AppendFormat("用引物{0}和{1}进行PCR，",
                                 PCR_FPrimerName, PCR_RPrimerName)
                If IsKeyName Then
                    stb.Append("产物标记为")
                    stb.Append(KeyName)
                    stb.Append("。")
                End If
            Case Nuctions.MolecularOperationEnum.Ligation
                stb.Append("用T4 DNA连接酶对")
                stb.Append(SourceKeyDescription)
                stb.Append("进行连接，")
                If IsKeyName Then
                    stb.Append("产物标记为")
                    stb.Append(KeyName)
                    stb.Append("。")
                End If
            Case Nuctions.MolecularOperationEnum.Gel
                If SolutionExtration Then
                    stb.Append("用溶液法回收")
                    stb.Append(SourceKeyDescription)
                    stb.Append("，")
                Else
                    stb.Append("用琼脂糖凝胶电泳分离")
                    stb.Append(SourceKeyDescription)
                    stb.Append("中")
                    If Gel_Minimum = Gel_Maximun Then
                        stb.AppendFormat("{0}bp的片段，", Gel_Minimum.ToString)
                    Else
                        stb.AppendFormat("介于{0}bp和{1}bp之间的DNA片段，", Gel_Minimum.ToString, Gel_Maximun.ToString)
                    End If
                End If
                If IsKeyName Then
                    stb.Append("标记为")
                    stb.Append(KeyName)
                    stb.Append("。")
                End If
            Case Nuctions.MolecularOperationEnum.Screen
                If IsCellNode Then
                    Select Case Screen_Mode
                        Case Nuctions.ScreenModeEnum.PCR
                            SynthesisList.Add(Me)
                            If Screen_PCRMin = Screen_PCRMax Then
                                stb.AppendFormat("用引物{0}和引物{1}对{2}进行菌落PCR，筛选其中产生{3}bp产物的克隆，", Screen_FName, Screen_RName, SourceKeyDescription, Screen_PCRMin.ToString)
                            Else
                                stb.AppendFormat("用引物{0}和引物{1}对{2}进行菌落PCR，筛选其中产生介于{3}bp和{4}bp产物的克隆，", Screen_FName, Screen_RName, SourceKeyDescription, Screen_PCRMin.ToString, Screen_PCRMax.ToString)
                            End If
                        Case Nuctions.ScreenModeEnum.Features

                    End Select
                Else
                    Select Case Screen_Mode
                        Case Nuctions.ScreenModeEnum.PCR
                            SynthesisList.Add(Me)
                            If Screen_PCRMin = Screen_PCRMax Then
                                stb.AppendFormat("用引物{0}和引物{1}对{2}进行PCR，筛选其中产生{3}bp产物的，", Screen_FName, Screen_RName, SourceKeyDescription, Screen_PCRMin.ToString)
                            Else
                                stb.AppendFormat("用引物{0}和引物{1}对{2}进行PCR，筛选其中产生介于{3}bp和{4}bp产物的，", Screen_FName, Screen_RName, SourceKeyDescription, Screen_PCRMin.ToString, Screen_PCRMax.ToString)
                            End If
                        Case Nuctions.ScreenModeEnum.Features

                    End Select
                End If
                If IsKeyName Then
                    stb.Append("标记为")
                    stb.Append(KeyName)
                    stb.Append("。")
                End If
            Case Nuctions.MolecularOperationEnum.HashPicker
                stb.Append("从")
                stb.Append(SourceKeyDescription)
                stb.Append("中随机筛选一个克隆，")
                If IsKeyName Then
                    stb.Append("标记为")
                    stb.Append(KeyName)
                    stb.Append("。")
                End If
            Case Nuctions.MolecularOperationEnum.FreeDesign
                SynthesisList.Add(Me)
            Case Nuctions.MolecularOperationEnum.Recombination
                Dim inCell As Boolean = Cells.Count > 0
                If inCell Then
                    Select Case RecombinationMethod
                        Case MCDS.RecombinationMethod.LambdaRecombination
                            stb.Append("诱导")
                            stb.Append(SourceKeyDescription)
                            stb.Append("进行λ Red重组，")
                        Case MCDS.RecombinationMethod.LambdaAttBP
                            stb.Append("诱导")
                            stb.Append(SourceKeyDescription)
                            stb.Append("进行λ BP重组，")
                        Case MCDS.RecombinationMethod.LambdaAttLR
                            stb.Append("诱导")
                            stb.Append(SourceKeyDescription)
                            stb.Append("进行λ LR重组，")
                        Case MCDS.RecombinationMethod.HK022AttBP
                            stb.Append("诱导")
                            stb.Append(SourceKeyDescription)
                            stb.Append("进行HK022 BP重组，")
                        Case MCDS.RecombinationMethod.HK022AttLR
                            stb.Append("诱导")
                            stb.Append(SourceKeyDescription)
                            stb.Append("进行HK022 LR重组，")
                        Case MCDS.RecombinationMethod.Phi80AttBP
                            stb.Append("诱导")
                            stb.Append(SourceKeyDescription)
                            stb.Append("进行φ80 BP重组，")
                        Case MCDS.RecombinationMethod.Phi80AttLR
                            stb.Append("诱导")
                            stb.Append(SourceKeyDescription)
                            stb.Append("进行φ80 LR重组，")
                        Case MCDS.RecombinationMethod.FRT
                            stb.Append("诱导")
                            stb.Append(SourceKeyDescription)
                            stb.Append("进行frt重组，")
                        Case MCDS.RecombinationMethod.LoxP
                            stb.Append("诱导")
                            stb.Append(SourceKeyDescription)
                            stb.Append("进行loxP重组，")
                        Case MCDS.RecombinationMethod.telRLSplit
                            stb.Append("诱导")
                            stb.Append(SourceKeyDescription)
                            stb.Append("形成端粒，")
                    End Select
                Else
                    Select Case RecombinationMethod
                        Case MCDS.RecombinationMethod.LambdaRecombination
                            stb.Append("利用Lambda Red重组酶催化")
                            stb.Append(SourceDescription)
                            stb.Append("重组，")
                        Case MCDS.RecombinationMethod.LambdaAttBP
                            stb.Append("利用Lambda Int重组酶和IFH催化")
                            stb.Append(SourceDescription)
                            stb.Append("重组，")
                        Case MCDS.RecombinationMethod.LambdaAttLR
                            stb.Append("利用Lambda Int、Xis重组酶和IFH催化")
                            stb.Append(SourceDescription)
                            stb.Append("重组，")
                        Case MCDS.RecombinationMethod.HK022AttBP
                            stb.Append("利用HK022 Int重组酶和IFH催化")
                            stb.Append(SourceDescription)
                            stb.Append("重组，")
                        Case MCDS.RecombinationMethod.HK022AttLR
                            stb.Append("利用HK022 Int、Xis重组酶和IFH催化")
                            stb.Append(SourceDescription)
                            stb.Append("重组，")
                        Case MCDS.RecombinationMethod.Phi80AttBP
                            stb.Append("利用φ80 Int重组酶和IFH催化")
                            stb.Append(SourceDescription)
                            stb.Append("重组，")
                        Case MCDS.RecombinationMethod.Phi80AttLR
                            stb.Append("利用φ80 Int、Xis重组酶和IFH催化")
                            stb.Append(SourceDescription)
                            stb.Append("重组，")
                        Case MCDS.RecombinationMethod.FRT
                            stb.Append("利用FLP重组酶催化")
                            stb.Append(SourceDescription)
                            stb.Append("重组，")
                        Case MCDS.RecombinationMethod.LoxP
                            stb.Append("利用Cre重组酶催化")
                            stb.Append(SourceDescription)
                            stb.Append("重组，")
                        Case MCDS.RecombinationMethod.telRLSplit
                            stb.Append("利用TelN使")
                            stb.Append(SourceDescription)
                            stb.Append("线性化，")
                    End Select
                End If
                If IsKeyName Then
                    stb.Append("标记为")
                    stb.Append(KeyName)
                    stb.Append("。")
                End If
            Case Nuctions.MolecularOperationEnum.SequencingResult
                SynthesisList.Add(Me)
            Case Nuctions.MolecularOperationEnum.Host
            Case Nuctions.MolecularOperationEnum.Transformation
                Dim DNASource As New List(Of DNAInfo)
                Dim CellSource As New List(Of DNAInfo)
                For Each sc As DNAInfo In Source
                    Select Case sc.vMolecularOperation
                        Case Nuctions.MolecularOperationEnum.Host, Nuctions.MolecularOperationEnum.Incubation, Nuctions.MolecularOperationEnum.Transformation
                            CellSource.Add(sc)
                        Case Else
                            DNASource.Add(sc)
                    End Select
                Next
                Select Case TransformationMethod
                    Case Nuctions.TransformationMethod.Electroporation
                        stb.Append("将")
                        stb.Append(DescribeSource(DNASource))
                        stb.Append("电转化到")
                        stb.Append(DescribeSource(CellSource))
                        stb.Append("制成的感受态中，")
                    Case Nuctions.TransformationMethod.ChemicalTransformation
                        stb.Append("将")
                        stb.Append(DescribeSource(DNASource))
                        stb.Append("转化到")
                        stb.Append(DescribeSource(CellSource))
                        stb.Append("制成的化转感受态中，")
                    Case Nuctions.TransformationMethod.Conjugation
                        stb.Append("对")
                        stb.Append(DescribeSource(CellSource))
                        stb.Append("做接合转化，")
                End Select
                If IsKeyName Then
                    stb.Append("标记为")
                    stb.Append(KeyName)
                    stb.Append("。")
                End If
            Case Nuctions.MolecularOperationEnum.Incubation
                Dim lasticb As IncubationStep = Nothing
                For Each icb As IncubationStep In Incubation
                    If lasticb Is Nothing Then

                        stb.Append("将")
                        stb.Append(SourceKeyDescription)
                    ElseIf icb.Medium = "-" Then

                    ElseIf lasticb.IsPlate Then
                        stb.Append("挑取平板上的克隆")
                    Else
                        stb.Append("取菌液")
                    End If
                    If lasticb Is Nothing AndAlso Source.Count = 1 AndAlso Source(0).vMolecularOperation = Nuctions.MolecularOperationEnum.Transformation AndAlso icb.Antibiotics.Count = 0 AndAlso icb.Time <> TimeSpan.Zero Then
                        '针对于转化
                        stb.Append("在")
                        stb.Append(icb.Medium)
                        stb.Append("培养基中重悬，")
                        stb.Append("在")
                        stb.Append(icb.Temperature)
                        stb.Append("℃")
                        stb.Append("培养")
                        If icb.Time < onehour Then
                            stb.Append(icb.Time.Minutes.ToString)
                            stb.Append("分钟, ")
                        Else
                            If icb.Time.Minutes = 0 Then
                                stb.Append(icb.Time.Hours.ToString)
                                stb.Append("小时，")
                            Else
                                stb.Append(icb.Time.Hours.ToString)
                                stb.Append("小时")
                                stb.Append(icb.Time.Minutes.ToString)
                                stb.Append("分钟, ")
                            End If
                        End If
                    ElseIf lasticb IsNot Nothing AndAlso icb.Medium = "-" Then
                        '这个设定是更换培养温度
                        stb.Append("在")
                        stb.Append(icb.Temperature)
                        stb.Append("℃")
                        stb.Append("培养")
                        If icb.Time = TimeSpan.Zero Then
                            If icb.IsPlate Then
                                stb.Append("至可见单克隆，")
                            Else
                                stb.Append("至菌液浑浊，")
                            End If
                        Else
                            If icb.Time < onehour Then
                                stb.Append(icb.Time.Minutes.ToString)
                                stb.Append("分钟, ")
                            Else
                                If icb.Time.Minutes = 0 Then
                                    stb.Append(icb.Time.Hours.ToString)
                                    stb.Append("小时，")
                                Else
                                    stb.Append(icb.Time.Hours.ToString)
                                    stb.Append("小时")
                                    stb.Append(icb.Time.Minutes.ToString)
                                    stb.Append("分钟, ")
                                End If
                            End If
                        End If
                    Else
                        If icb.IsPlate Then
                            stb.Append("涂布")
                        Else
                            stb.Append("接种")
                        End If
                        stb.Append("于")
                        If icb.Antibiotics.Count > 0 Then
                            stb.Append("含有")
                            stb.Append(Nuctions.DescribeAntibiotics(icb.Antibiotics, Language.Chinese))
                            stb.Append("的")
                        End If
                        stb.Append(icb.Medium)
                        If icb.IsPlate Then
                            stb.Append("琼脂平板上，")
                        Else
                            stb.Append("液体培养基中，")
                        End If
                        If icb.Inducer IsNot Nothing And icb.Inducer.Length > 0 Then
                            stb.Append("并加入")
                            stb.Append(icb.Inducer)
                            stb.Append("诱导，")
                        End If
                        stb.Append("在")
                        stb.Append(icb.Temperature)
                        stb.Append("℃")
                        stb.Append("培养")
                        If icb.Time = TimeSpan.Zero Then
                            If icb.IsPlate Then
                                stb.Append("至可见单克隆，")
                            Else
                                stb.Append("至菌液浑浊，")
                            End If
                        Else
                            If icb.Time < onehour Then
                                stb.Append(icb.Time.Minutes.ToString)
                                stb.Append("分钟, ")
                            Else
                                If icb.Time.Minutes = 0 Then
                                    stb.Append(icb.Time.Hours.ToString)
                                    stb.Append("小时，")
                                Else
                                    stb.Append(icb.Time.Hours.ToString)
                                    stb.Append("小时")
                                    stb.Append(icb.Time.Minutes.ToString)
                                    stb.Append("分钟, ")
                                End If
                            End If
                        End If
                    End If
                    lasticb = icb
                Next

                If IsKeyName Then
                    stb.Append("标记为")
                    stb.Append(KeyName)
                    stb.Append("。")
                End If
            Case Nuctions.MolecularOperationEnum.Extraction
                stb.Append("从")
                stb.Append(SourceKeyDescription)
                stb.Append("中提取质粒")
                If IncludeVerification Then
                    For Each ci As ChartItem In Me.GetParetntChartItem.Parent.Items
                        If ci.MolecularInfo.Source.Contains(Me) And ci.MolecularInfo.IsVerificationStep Then
                            Dim cdi As DNAInfo = ci.MolecularInfo
                            Select Case cdi.vMolecularOperation
                                Case Nuctions.MolecularOperationEnum.PCR
                                    SynthesisList.Add(cdi)
                                    stb.AppendFormat("用引物{0}和{1}进行PCR验证", cdi.PCR_FPrimerName, cdi.PCR_RPrimerName)
                                    If DNAs.Count > 0 Then
                                        Dim shortest As Integer = Integer.MaxValue
                                        For Each gf As Nuctions.GeneFile In cdi.DNAs
                                            If shortest > gf.Length Then shortest = gf.Length
                                        Next
                                        stb.AppendFormat("筛选其中能够产生{0}bp片段的，", shortest.ToString)
                                    Else
                                        stb.Append("筛选其中不能产生PCR产物的，")
                                    End If
                                Case Nuctions.MolecularOperationEnum.Enzyme
                                    stb.Append("用")
                                    stb.Append(FacilityFunctions.DescribeStringList(cdi.Enzyme_Enzymes, Language.Chinese))
                                    stb.Append("酶切验证筛选其中能够产生")
                                    Dim products As New List(Of String)
                                    For Each gf As Nuctions.GeneFile In cdi.DNAs
                                        products.Add(gf.Length.ToString + "bp")
                                    Next
                                    stb.Append(FacilityFunctions.DescribeStringList(products, Language.Chinese))
                                    stb.Append("片段的，")
                            End Select
                        End If
                    Next
                End If
                If SequencingVerify Then
                    stb.Append("并测序验证")
                End If
                stb.Append("，")
                If IsKeyName Then
                    If SequencingVerify Or IncludeVerification Then
                        stb.Append("验证正确的")
                    End If
                    stb.Append("标记为")
                    stb.Append(KeyName)
                    stb.Append("。")
                End If
        End Select
        Return stb.ToString()
    End Function
    Private Function GetChineseDescription() As String
        Dim stb As New System.Text.StringBuilder
        Select Case vMolecularOperation
            Case Nuctions.MolecularOperationEnum.Vector
                Select Case DescribeType
                    Case DescribeEnum.Vecotor
                        stb.Append("初始载体为")
                        stb.Append(Name)
                        stb.Append("，")
                        stb.Append("标记为")
                        stb.Append(LabelName)
                        stb.Append("。")
                    Case DescribeEnum.Chromosome
                        stb.Append("初始宿主为")
                        stb.Append(Name)
                        stb.Append("，")
                        stb.Append("标记为")
                        stb.Append(LabelName)
                        stb.Append("。")
                    Case Else
                        stb.Append("初始载体为")
                        stb.Append(Name)
                        stb.Append("，")
                        stb.Append("标记为")
                        stb.Append(LabelName)
                        stb.Append("。")
                End Select
            Case Nuctions.MolecularOperationEnum.Modify
                Select Case Modify_Method
                    Case Nuctions.ModificationMethodEnum.CIAP
                        stb.Append("用碱式磷酸酶处理")
                    Case Nuctions.ModificationMethodEnum.T4DNAP
                        stb.Append("用T4 DNA聚合酶处理")
                    Case Nuctions.ModificationMethodEnum.Klewnow
                        stb.Append("用Klewnow大片段处理")
                    Case Nuctions.ModificationMethodEnum.PNK
                        stb.Append("用多磷酸激酶处理")
                End Select
                stb.Append(SourceDescription)
                stb.Append("，")
                stb.Append("得到的产物标记为")
                stb.Append(LabelName)
                stb.Append("。")
            Case Nuctions.MolecularOperationEnum.Enzyme
                stb.Append("用限制内切酶")
                Dim i As Integer = 0
                For Each ez As String In Enzyme_Enzymes
                    i += 1
                    stb.Append(ez)
                    If i < Enzyme_Enzymes.Count - 1 Then
                        stb.Append("、")
                    ElseIf i = Enzyme_Enzymes.Count - 1 Then
                        stb.Append("和")
                    ElseIf i = Enzyme_Enzymes.Count Then
                        'stb.Append("，")
                    End If
                Next
                stb.Append("对")
                stb.Append(SourceDescription)
                stb.Append("作酶切，")
                stb.Append("得到的产物标记为")
                stb.Append(LabelName)
                stb.Append("。")
            Case Nuctions.MolecularOperationEnum.PCR
                stb.Append("将")
                stb.Append(SourceDescription)
                stb.Append("作为PCR模板，")
                stb.AppendFormat("用引物{0}:{1}和引物{2}:{3}对所得克隆进行菌落PCR，",
                                 PCR_FPrimerName, Nuctions.TAGCFilter(PCR_ForwardPrimer), PCR_RPrimerName, Nuctions.TAGCFilter(PCR_ReversePrimer))
                stb.Append("得到的产物标记为")
                stb.Append(LabelName)
                stb.Append("。")
            Case Nuctions.MolecularOperationEnum.Ligation
                stb.Append("利用T4 DNA连接酶对")
                stb.Append(SourceDescription)
                stb.Append("进行连接，")
                stb.Append("得到的产物标记为")
                stb.Append(LabelName)
                stb.Append("。")
            Case Nuctions.MolecularOperationEnum.Gel
                stb.Append("从")
                stb.Append(SourceDescription)
                stb.Append("中利用琼脂糖电泳分离获得")
                stb.AppendFormat("介于{0}bp和{1}bp之间的DNA片段，", Gel_Minimum.ToString, Gel_Maximun.ToString)
                stb.Append("得到")
                stb.Append(DNADescription)
                stb.Append("，")
                stb.Append("标记为")
                stb.Append(LabelName)
                stb.Append("。")
            Case Nuctions.MolecularOperationEnum.Screen
                Select Case SourceDescribeType
                    Case DescribeEnum.Vecotor
                        stb.Append("将")
                        stb.Append(SourceDescription)
                        stb.Append("转化，培养至培养皿中可见单克隆菌落。")
                    Case DescribeEnum.Chromosome
                        stb.Append("将")
                        stb.Append(SourceDescription)
                        stb.Append("涂平板培养至培养皿中可见单克隆菌落。")
                    Case Else
                        stb.Append("将")
                        stb.Append(SourceDescription)
                        stb.Append("转化，培养至培养皿中可见单克隆菌落。")
                End Select
                Select Case Screen_Mode
                    Case Nuctions.ScreenModeEnum.PCR
                        stb.AppendFormat("用引物{0}:{1}和引物{2}:{3}对所得克隆进行菌落PCR，从",
                                         Screen_FName, Nuctions.TAGCFilter(Screen_FPrimer), Screen_RName, Nuctions.TAGCFilter(Screen_RPrimer))
                        stb.Append(SourceDescription)
                        stb.Append("中筛选")
                        stb.AppendFormat("PCR产物介于{0}bp和{1}bp之间{2}的克隆，", Screen_PCRMin.ToString, Screen_PCRMax.ToString, IIf(Screen_OnlyCircular, "且为环形质粒", ""))
                    Case Nuctions.ScreenModeEnum.Features
                        stb.Append("从所得克隆当中通过提取质粒酶切验证筛选其中满足条件")
                        Dim i As Integer = 0
                        If Screen_Features.Count > 0 Then
                            stb.Append("满足条件")
                            For Each fsi As FeatureScreenInfo In Screen_Features
                                If fsi.ScreenMethod <> FeatureScreenEnum.NotEngaged Then
                                    i += 1
                                    Select Case fsi.ScreenMethod
                                        Case FeatureScreenEnum.None
                                            stb.Append("不含有")
                                        Case FeatureScreenEnum.Once
                                            stb.Append("仅含有一个")
                                        Case FeatureScreenEnum.OnceOrMore
                                            stb.Append("含有至少一个")
                                        Case FeatureScreenEnum.Maximum
                                            stb.Append("含有最多个")
                                    End Select
                                    stb.Append(fsi.Feature.Label)
                                    stb.Append("")
                                    If i < Screen_Features.Count Then
                                        stb.Append("、且")
                                    Else
                                        'stb.Append("，")
                                    End If
                                End If
                            Next
                        Else
                            stb.Append("验证成功")
                        End If
                        stb.Append("的")
                        stb.Append(IIf(Screen_OnlyCircular, "环形质粒", ""))
                        stb.Append("克隆，")
                End Select
                Select Case DescribeType
                    Case DescribeEnum.Vecotor
                        stb.Append("得到")
                        stb.Append(DNADescription)
                        stb.Append("，")
                    Case DescribeEnum.Chromosome
                        stb.Append("得到具有符合条件的克隆，")
                    Case Else
                        stb.Append("得到")
                        stb.Append(DNADescription)
                        stb.Append("，")
                End Select
                stb.Append("标记为")
                stb.Append(LabelName)
                stb.Append("。")
            Case Nuctions.MolecularOperationEnum.HashPicker
                stb.Append("用PCR对所得克隆进行筛选，从")
                stb.Append(SourceDescription)
                stb.Append("中随机筛选")
                stb.Append(IIf(Screen_OnlyCircular, "环形", ""))
                stb.Append("DNA，")
                stb.Append("得到")
                stb.Append(DNADescription)
                stb.Append("，")
                stb.Append("标记为")
                stb.Append(LabelName)
                stb.Append("。")
            Case Nuctions.MolecularOperationEnum.FreeDesign
                stb.Append("利用基因合成得到")
                stb.Append(Name)
                stb.Append("，")
                If DNAs.Count = 1 Then
                    stb.Append("序列为：")
                    stb.Append(CType(DNAs(1), Nuctions.GeneFile).Sequence)
                    stb.Append("，")
                End If
                stb.Append("标记为")
                stb.Append(LabelName)
                stb.Append("。")
            Case Nuctions.MolecularOperationEnum.Recombination
                Select Case RecombinationMethod
                    Case MCDS.RecombinationMethod.LambdaRecombination
                        stb.Append("利用Lambda Red重组酶催化")
                        stb.Append(SourceDescription)
                        stb.Append("重组，")
                    Case MCDS.RecombinationMethod.LambdaAttBP
                        stb.Append("利用Lambda Int重组酶和IFH催化")
                        stb.Append(SourceDescription)
                        stb.Append("重组，")
                    Case MCDS.RecombinationMethod.LambdaAttLR
                        stb.Append("利用Lambda Int、Xis重组酶和IFH催化")
                        stb.Append(SourceDescription)
                        stb.Append("重组，")
                    Case MCDS.RecombinationMethod.HK022AttBP
                        stb.Append("利用HK022 Int重组酶和IFH催化")
                        stb.Append(SourceDescription)
                        stb.Append("重组，")
                    Case MCDS.RecombinationMethod.HK022AttLR
                        stb.Append("利用HK022 Int、Xis重组酶和IFH催化")
                        stb.Append(SourceDescription)
                        stb.Append("重组，")
                    Case MCDS.RecombinationMethod.Phi80AttBP
                        stb.Append("利用φ80 Int重组酶和IFH催化")
                        stb.Append(SourceDescription)
                        stb.Append("重组，")
                    Case MCDS.RecombinationMethod.Phi80AttLR
                        stb.Append("利用φ80 Int、Xis重组酶和IFH催化")
                        stb.Append(SourceDescription)
                        stb.Append("重组，")
                    Case MCDS.RecombinationMethod.FRT
                        stb.Append("利用FLP重组酶催化")
                        stb.Append(SourceDescription)
                        stb.Append("重组，")
                    Case MCDS.RecombinationMethod.LoxP
                        stb.Append("利用Cre重组酶催化")
                        stb.Append(SourceDescription)
                        stb.Append("重组，")
                    Case MCDS.RecombinationMethod.telRLSplit
                        stb.Append("利用TelN使")
                        stb.Append(SourceDescription)
                        stb.Append("线性化，")
                End Select
                Select Case DescribeType
                    Case DescribeEnum.Vecotor
                        stb.Append("得到")
                        stb.Append(DNADescription)
                        stb.Append("，")
                    Case DescribeEnum.Chromosome
                        If DNAs.Count > 1 Then
                            stb.Append("得到重组产物克隆，")
                        Else
                            stb.Append("得到含有重组产物和副产物的不同克隆，")
                        End If
                    Case Else
                        stb.Append("得到")
                        stb.Append(DNADescription)
                        stb.Append("，")
                End Select
                stb.Append("标记为")
                stb.Append(LabelName)
                stb.Append("。")
        End Select
        Return stb.ToString()
    End Function
    Private Function GetEnglishDescription() As String

        Dim stb As New System.Text.StringBuilder
        Dim be As String = "was"
        If Source.Count > 1 Then
            be = "were"
        End If
        Select Case vMolecularOperation
            Case Nuctions.MolecularOperationEnum.Vector
                Select Case DescribeType
                    Case DescribeEnum.Vecotor
                        stb.Append("The starting vector was ")
                        stb.Append(Name)
                        stb.Append(", ")
                        stb.Append("which was labeled as ")
                        stb.Append(LabelName)
                        stb.Append(".")
                    Case DescribeEnum.Chromosome
                        stb.Append("The starting host bacteria is ")
                        stb.Append(Name)
                        stb.Append(", ")
                        stb.Append("which was labeled as ")
                        stb.Append(LabelName)
                        stb.Append(".")
                    Case Else
                        stb.Append("The starting vector was ")
                        stb.Append(Name)
                        stb.Append(", ")
                        stb.Append("which is labeled as ")
                        stb.Append(LabelName)
                        stb.Append(".")
                End Select
            Case Nuctions.MolecularOperationEnum.Modify
                stb.Append(SourceDescription)
                stb.AppendFormat(" {0} ", be)
                Select Case Modify_Method
                    Case Nuctions.ModificationMethodEnum.CIAP
                        stb.Append("dephosphorated by Calf Intestinal Alkaline Phosphatase. ")
                    Case Nuctions.ModificationMethodEnum.T4DNAP
                        stb.Append("blunted by T4 DNA Polymerase. ")
                    Case Nuctions.ModificationMethodEnum.Klewnow
                        stb.Append("blunted by Klewnow fragment. ")
                    Case Nuctions.ModificationMethodEnum.PNK
                        stb.Append("phosphorated by Polynucleotide Kinase. ")
                End Select
                stb.Append("The product was labeled as ")
                stb.Append(LabelName)
                stb.Append(".")
            Case Nuctions.MolecularOperationEnum.Enzyme
                stb.Append(SourceDescription)
                stb.AppendFormat(" {0} digested by ", be)
                Dim i As Integer = 0
                For Each ez As String In Enzyme_Enzymes
                    i += 1
                    stb.Append(ez)
                    If i < Enzyme_Enzymes.Count - 1 Then
                        stb.Append(", ")
                    ElseIf i = Enzyme_Enzymes.Count - 1 Then
                        stb.Append(" and ")
                    ElseIf i = Enzyme_Enzymes.Count Then
                        'stb.Append("，")
                    End If
                Next
                stb.Append(". ")
                stb.Append("The product was labeled as ")
                stb.Append(LabelName)
                stb.Append(".")
            Case Nuctions.MolecularOperationEnum.PCR
                stb.Append("PCR was performed by using ")
                stb.Append(SourceDescription)
                stb.Append(" as PCR template, ")

                stb.Append("where the primers were ")
                stb.AppendFormat("{0}:{1} and {2}:{3}. ",
                                 PCR_FPrimerName, Nuctions.TAGCFilter(PCR_ForwardPrimer), PCR_RPrimerName, Nuctions.TAGCFilter(PCR_ReversePrimer))
                stb.Append("The PCR product was labeled as ")
                stb.Append(LabelName)
                stb.Append(".")
            Case Nuctions.MolecularOperationEnum.Ligation
                stb.Append(SourceDescription)
                stb.AppendFormat(" {0} ligated by T4 DNA ligase. ", be)
                stb.Append("The ligation product was labeled as ")
                stb.Append(LabelName)
                stb.Append(".")
            Case Nuctions.MolecularOperationEnum.Gel
                stb.Append("Gel extracion was performed to isolate ")
                stb.AppendFormat("DNA fragments with the length between {0}bp and {1}bp from ", Gel_Minimum.ToString, Gel_Maximun.ToString)
                stb.Append(SourceDescription)
                stb.Append(". ")
                If DNAs.Count > 1 Then
                    stb.Append("The extracted product was the mixture of ")
                    stb.Append(DNADescription)
                    stb.Append(", ")
                ElseIf DNAs.Count = 0 Then
                    stb.Append("The extracted product does not contains any DNA fragments, ")
                ElseIf DNAs.Count = 1 Then
                    stb.Append("The extracted product was ")
                    stb.Append(DNADescription)
                    stb.Append(", ")
                End If
                stb.Append("which was labeled as ")
                stb.Append(LabelName)
                stb.Append(".")
            Case Nuctions.MolecularOperationEnum.Screen
                Select Case SourceDescribeType
                    Case DescribeEnum.Vecotor
                        stb.Append(SourceDescription)
                        stb.AppendFormat(" {0} ", be)
                        stb.Append("transformed to competent cells, which was then sprayed onto agar plates and incubated until colonies become visible. ")
                    Case DescribeEnum.Chromosome
                        stb.Append(SourceDescription)
                        stb.AppendFormat(" {0} ", be)
                        stb.Append("sprayed onto agar plates and incubated until colonies become visible. ")
                    Case Else
                        stb.Append(SourceDescription)
                        stb.AppendFormat(" {0} ", be)
                        stb.Append("transformed to competent cells, which was then sprayed onto agar plates and incubated until colonies become visible. ")
                End Select
                Select Case Screen_Mode
                    Case Nuctions.ScreenModeEnum.PCR
                        stb.AppendFormat("Primer {0}:{1} and Primer {2}:{3} were used to perform colony PCR to screen the colonies that could produce PCR products with the length between {4}bp and {5}bp from ",
                                         Screen_FName, Nuctions.TAGCFilter(Screen_FPrimer), Screen_RName, Nuctions.TAGCFilter(Screen_RPrimer), Screen_PCRMin.ToString, Screen_PCRMax.ToString)
                        stb.Append(SourceDescription)
                        stb.Append(". ")
                        If Screen_OnlyCircular Then
                            stb.AppendFormat("PCR screened colonies were further confirmed to contains circular plasmid by plasmid extraction. ")
                        End If
                    'stb.AppendFormat("PCR产物介于{0}bp和{1}bp之间{2}的克隆，", Screen_PCRMin.ToString, Screen_PCRMax.ToString, IIf(Screen_OnlyCircular, "且为环形质粒", ""))
                    Case Nuctions.ScreenModeEnum.Features
                        Dim i As Integer = 0
                        If Screen_Features.Count > 0 Then
                            stb.Append("The colonies were innoculated and then incubated respectively. ")
                            Select Case SourceDescribeType
                                Case DescribeEnum.Vecotor
                                    stb.Append("Plasmids were extracted and used for PCR and restriction enzyme digestion to screen the plasmids that ")
                                Case DescribeEnum.Chromosome
                                    stb.Append("PCR screen was performed to screen the colonies that ")
                                Case Else
                                    stb.Append("Plasmids were extracted and used for PCR and restriction enzyme digestion to screen the plasmids that ")
                            End Select
                            For Each fsi As FeatureScreenInfo In Screen_Features
                                If fsi.ScreenMethod <> FeatureScreenEnum.NotEngaged Then
                                    i += 1
                                    Select Case fsi.ScreenMethod
                                        Case FeatureScreenEnum.None
                                            stb.Append("do not contain ")
                                        Case FeatureScreenEnum.Once
                                            stb.Append("contain only one copy of ")
                                        Case FeatureScreenEnum.OnceOrMore
                                            stb.Append("contain at least one copy of ")
                                        Case FeatureScreenEnum.Maximum
                                            stb.Append("contain the most copies of ")
                                    End Select
                                    stb.Append(fsi.Feature.Label)
                                    If i < Screen_Features.Count Then
                                        stb.Append(", and ")
                                    Else
                                        'stb.Append("，")
                                    End If
                                End If
                            Next
                            If Screen_OnlyCircular Then
                                stb.AppendFormat("The screened colonies were further confirmed to contains circular plasmid. ")
                            End If
                        Else
                            If Screen_OnlyCircular Then
                                stb.AppendFormat("The colonies were then confirmed to contains circular plasmid. ")
                            End If
                        End If
                End Select
                Select Case DescribeType
                    Case DescribeEnum.Vecotor
                        If DNAs.Count > 1 Then
                            stb.Append("The obtained vectors were ")
                            stb.Append(DNADescription)
                            stb.Append(", which were labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 0 Then
                            stb.Append("No vector was obtained. ")
                            stb.Append("This empty product was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 1 Then
                            stb.Append("The obtained vector was ")
                            stb.Append(DNADescription)
                            stb.Append(", which was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        End If
                    Case DescribeEnum.Chromosome
                        If DNAs.Count > 1 Then
                            stb.Append("The obtained colonies were ")
                            stb.Append(DNADescription)
                            stb.Append(", which were labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 0 Then
                            stb.Append("No colony was obtained. ")
                            stb.Append("This empty product was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 1 Then
                            stb.Append("The obtained colony was ")
                            stb.Append(DNADescription)
                            stb.Append(", which was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        End If
                    Case Else
                        If DNAs.Count > 1 Then
                            stb.Append("The obtained vectors were ")
                            stb.Append(DNADescription)
                            stb.Append(", which were labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 0 Then
                            stb.Append("No vector was obtained. ")
                            stb.Append("This empty product was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 1 Then
                            stb.Append("The obtained vector was ")
                            stb.Append(DNADescription)
                            stb.Append(", which was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        End If
                End Select
            Case Nuctions.MolecularOperationEnum.HashPicker
                stb.AppendFormat("PCR screen was performed to select {0} ", DNAs.Count.ToString)
                If DNAs.Count > 0 Then
                    Select Case DescribeType
                        Case DescribeEnum.Vecotor
                            stb.Append("plasmids ")
                        Case DescribeEnum.Chromosome
                            stb.Append("colonies ")
                    End Select
                Else
                    Select Case DescribeType
                        Case DescribeEnum.Vecotor
                            stb.Append("plasmid ")
                        Case DescribeEnum.Chromosome
                            stb.Append("colony ")
                    End Select
                End If
                stb.Append("from ")
                stb.Append(SourceDescription)
                stb.Append(", ")
                Select Case DescribeType
                    Case DescribeEnum.Vecotor
                        If DNAs.Count > 1 Then
                            stb.Append("The obtained vectors were ")
                            stb.Append(DNADescription)
                            stb.Append(", which were labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 0 Then
                            stb.Append("No vector was obtained. ")
                            stb.Append("This empty product was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 1 Then
                            stb.Append("The obtained vector was ")
                            stb.Append(DNADescription)
                            stb.Append(", which was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        End If
                    Case DescribeEnum.Chromosome
                        If DNAs.Count > 1 Then
                            stb.Append("The obtained colonies were ")
                            stb.Append(DNADescription)
                            stb.Append(", which were labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 0 Then
                            stb.Append("No colony was obtained. ")
                            stb.Append("This empty product was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 1 Then
                            stb.Append("The obtained colony was ")
                            stb.Append(DNADescription)
                            stb.Append(", which was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        End If
                    Case Else
                        If DNAs.Count > 1 Then
                            stb.Append("The obtained vectors were ")
                            stb.Append(DNADescription)
                            stb.Append(", which were labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 0 Then
                            stb.Append("No vector was obtained. ")
                            stb.Append("This empty product was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 1 Then
                            stb.Append("The obtained vector was ")
                            stb.Append(DNADescription)
                            stb.Append(", which was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        End If
                End Select
            Case Nuctions.MolecularOperationEnum.FreeDesign
                If DNAs.Count = 1 Then
                    stb.AppendFormat("The following double strand DNA sequence, namely {0}, was synthesized by gene synthesis: ", Name)
                    stb.Append(CType(DNAs(1), Nuctions.GeneFile).Sequence)
                    stb.Append(". ")
                End If
                stb.Append("The synthesized DNA was labeled as ")
                stb.Append(LabelName)
                stb.Append(".")
            Case Nuctions.MolecularOperationEnum.Recombination
                Dim cmode As String = "used"
                Select Case DescribeType
                    Case DescribeEnum.Vecotor
                        cmode = "used"
                    Case DescribeEnum.Chromosome
                        cmode = "expressed"
                End Select
                Select Case RecombinationMethod
                    Case MCDS.RecombinationMethod.LambdaRecombination
                        stb.AppendFormat("Lambda Red recombinases were {0} to catalyzed the recombination of ", cmode)
                        stb.Append(SourceDescription)
                        stb.Append(". ")
                    Case MCDS.RecombinationMethod.LambdaAttBP
                        stb.AppendFormat("Lambda Int recombinase and IFH recombinases were {0} to catalyzed the recombination of ", cmode)
                        stb.Append(SourceDescription)
                        stb.Append(". ")
                    Case MCDS.RecombinationMethod.LambdaAttLR
                        stb.AppendFormat("Lambda Int, Xis recombinase and IFH recombinases were {0} to catalyzed the recombination of ", cmode)
                        stb.Append(SourceDescription)
                        stb.Append(". ")
                    Case MCDS.RecombinationMethod.HK022AttBP
                        stb.AppendFormat("HK022 Int recombinase and IFH recombinases were {0} to catalyzed the recombination of ", cmode)
                        stb.Append(SourceDescription)
                        stb.Append(". ")
                    Case MCDS.RecombinationMethod.HK022AttLR
                        stb.AppendFormat("HK022 Int, Xis recombinase and IFH recombinases were {0} to catalyzed the recombination of ", cmode)
                        stb.Append(SourceDescription)
                        stb.Append(". ")
                    Case MCDS.RecombinationMethod.Phi80AttBP
                        stb.AppendFormat("φ80 Int recombinase and IFH recombinases were {0} to catalyzed the recombination of ", cmode)
                        stb.Append(SourceDescription)
                        stb.Append(". ")
                    Case MCDS.RecombinationMethod.Phi80AttLR
                        stb.AppendFormat("φ80 Int, Xis recombinase and IFH recombinases were {0} to catalyzed the recombination of ", cmode)
                        stb.Append(SourceDescription)
                        stb.Append(". ")
                    Case MCDS.RecombinationMethod.FRT
                        stb.AppendFormat("FLP was {0} to catalyzed the recombination of ", cmode)
                        stb.Append(SourceDescription)
                        stb.Append(". ")
                    Case MCDS.RecombinationMethod.LoxP
                        stb.AppendFormat("Cre was {0} to catalyzed the recombination of ", cmode)
                        stb.Append(SourceDescription)
                        stb.Append(". ")
                    Case MCDS.RecombinationMethod.telRLSplit
                        stb.AppendFormat("TelN was {0} to linearize ", cmode)
                        stb.Append(SourceDescription)
                        stb.Append(". ")
                End Select
                Select Case DescribeType
                    Case DescribeEnum.Vecotor
                        If DNAs.Count > 1 Then
                            stb.Append("The obtained vectors were supposed to contain ")
                            stb.Append(DNADescription)
                            stb.Append(", which were labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 0 Then
                            stb.Append("No vector was obtained.")
                        ElseIf DNAs.Count = 1 Then
                            stb.Append("The obtained vector was ")
                            stb.Append(DNADescription)
                            stb.Append(", which was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        End If
                    Case DescribeEnum.Chromosome
                        If DNAs.Count > 1 Then
                            stb.Append("The obtained colonies were supposed to contain ")
                            stb.Append(DNADescription)
                            stb.Append(", which were labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 0 Then
                            stb.Append("No colony was obtained. ")
                            stb.Append("This empty product was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 1 Then
                            stb.Append("The obtained colony was ")
                            stb.Append(DNADescription)
                            stb.Append(", which was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        End If
                    Case Else
                        If DNAs.Count > 1 Then
                            stb.Append("The obtained vectors were ")
                            stb.Append(DNADescription)
                            stb.Append(", which were labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 0 Then
                            stb.Append("No vector was obtained. ")
                            stb.Append("This empty product was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        ElseIf DNAs.Count = 1 Then
                            stb.Append("The obtained vector was ")
                            stb.Append(DNADescription)
                            stb.Append(", which was labeled as ")
                            stb.Append(LabelName)
                            stb.Append(".")
                        End If
                End Select
            Case Nuctions.MolecularOperationEnum.EnzymeAnalysis
                Dim u As Integer = 0
                For Each eap As EnzymeAnalysisItem In EnzymeAnalysisParamters
                    If eap.Use Then u += 1
                Next
                If u > 0 Then
                    stb.Append("Restriction sites that ")
                    For Each eap As EnzymeAnalysisItem In EnzymeAnalysisParamters
                        If eap.Use Then
                            stb.Append("cut the ")
                            stb.Append(eap.Region.ToLower)
                            stb.Append(" region of ")
                            stb.Append(eap.GeneFile.Name)
                            Select Case eap.Method
                                Case EnzymeAnalysisEnum.Equal

                                Case EnzymeAnalysisEnum.Greater
                                    stb.Append("more than ")
                                Case EnzymeAnalysisEnum.Less
                                    stb.Append("less than ")
                            End Select
                            Select Case eap.Value
                                Case 0
                                    stb.Append(" zero time, ")
                                Case 1
                                    stb.Append(" once, ")
                                Case Else
                                    stb.Append(" ")
                                    stb.Append(eap.Value.ToString)
                                    stb.Append(" times, ")
                            End Select
                        End If
                    Next
                    If FetchedEnzymes.Count > 0 Then
                        stb.Append("were found.")
                        stb.Append(" They were ")
                        Dim i As Integer = 0
                        For i = 0 To FetchedEnzymes.Count - 1
                            If i = 0 Then
                                stb.Append(FetchedEnzymes(i))
                            ElseIf i > 0 AndAlso i = FetchedEnzymes.Count - 1 Then
                                stb.Append(" and ")
                                stb.Append(FetchedEnzymes(i))
                            Else
                                stb.Append(", ")
                                stb.Append(FetchedEnzymes(i))
                            End If
                        Next
                        stb.Append(".")
                    Else
                        stb.Append(" were not found.")
                    End If
                Else
                    stb.Append("Restriction sites that met no conditions ")
                    If FetchedEnzymes.Count > 0 Then
                        stb.Append(" were found.")
                        stb.Append(" They were ")
                        Dim i As Integer = 0
                        For i = 0 To FetchedEnzymes.Count - 1
                            If i = 0 Then
                                stb.Append(FetchedEnzymes(i))
                            ElseIf i > 0 AndAlso i = FetchedEnzymes.Count - 1 Then
                                stb.Append(" and ")
                                stb.Append(FetchedEnzymes(i))
                            Else
                                stb.Append(", ")
                                stb.Append(FetchedEnzymes(i))
                            End If
                        Next
                        stb.Append(".")
                    Else
                        stb.Append(" were not found.")
                    End If
                End If
            Case Nuctions.MolecularOperationEnum.SequencingResult
                stb.AppendFormat("Sequencing was performed with the Primer {0}:{1}. ", SequencingPrimerName, SequencingPrimer)
                Select Case SequencingResultComment
                    Case SequenceResultOptions.Unchecked
                        stb.Append("Result was not checked.")
                    Case SequenceResultOptions.Correct
                        stb.Append("Result was correct.")
                    Case SequenceResultOptions.PointMutation
                        stb.Append("Result showed the appearance of point mutation.")
                    Case SequenceResultOptions.FragmentInsertion
                        stb.Append("Result showed the fragment insertion mutation.")
                    Case SequenceResultOptions.FragmentLoss
                        stb.Append("Result showed the fragment loss mutation.")
                    Case SequenceResultOptions.NoneMatch
                        stb.Append("Result showed no match with the subject.")
                End Select
            Case Nuctions.MolecularOperationEnum.Merge
                stb.Append(SourceDescription)
                stb.Append("was merged. ")
                stb.Append("The product was labeled as ")
                stb.Append(LabelName)
                stb.Append(".")
            Case Nuctions.MolecularOperationEnum.Compare
                stb.Append(SourceDescription)
                stb.Append(" was compared. ")
                Select Case CompareResultComment
                    Case SequenceResultOptions.Unchecked
                        stb.Append("Result was not checked.")
                    Case SequenceResultOptions.Correct
                        stb.Append("Result was correct.")
                    Case SequenceResultOptions.PointMutation
                        stb.Append("Result showed the appearance of point mutation.")
                    Case SequenceResultOptions.FragmentInsertion
                        stb.Append("Result showed the fragment insertion mutation.")
                    Case SequenceResultOptions.FragmentLoss
                        stb.Append("Result showed the fragment loss mutation.")
                    Case SequenceResultOptions.NoneMatch
                        stb.Append("Result showed no match with the subject.")
                End Select
                'stb.Append(" The result was labeled as ")
                'stb.Append(LabelName)
                'stb.Append(".")
        End Select
        Return stb.ToString
    End Function
    Public Function GetConstructionDescription(ByVal vList As List(Of DNAInfo), Optional Summary As Boolean = False) ', ByVal vRoots As List(Of DNAInfo), ByVal vTreeStack As Stack(Of DNAInfo)) As String
        Dim CStack As New Stack(Of DNAInfo)
        Dim i As Integer = vList.Count
        Dim zList As New List(Of DNAInfo)
        '构建一个拓扑逻辑关系堆栈，出堆栈顺序就是正确的构建顺序。
        PushSourceStack(CStack, vList)
        Dim stb As New System.Text.StringBuilder
        Select Case RegionalLanguage
            Case Language.English
                Select Case vMolecularOperation
                    Case Nuctions.MolecularOperationEnum.EnzymeAnalysis
                        stb.AppendFormat("The restriction analysis of {0}:", SourceDescription)
                    Case Nuctions.MolecularOperationEnum.SequencingResult
                        stb.AppendFormat("The sequencing of {0}:", SourceDescription)
                    Case Nuctions.MolecularOperationEnum.Merge
                        stb.AppendFormat("The merged sequence of {0}:", SourceDescription)
                    Case Nuctions.MolecularOperationEnum.Compare
                        stb.AppendFormat("The alignment of {0}:", SourceDescription)
                    Case Else
                        stb.AppendFormat("The construction of {0}:", Name)
                End Select
                stb.AppendLine()
                While CStack.Count > 0
                    i += 1
                    stb.AppendFormat("({0}) ", i.ToString)
                    stb.Append(CStack.Pop.Description(zList))
                    stb.AppendLine()
                End While
                Select Case vMolecularOperation
                    Case Nuctions.MolecularOperationEnum.EnzymeAnalysis

                    Case Nuctions.MolecularOperationEnum.SequencingResult

                    Case Nuctions.MolecularOperationEnum.Merge

                    Case Nuctions.MolecularOperationEnum.Compare

                    Case Else
                        stb.AppendFormat("The obtained product labeled as {0} was {1}.", LabelName, Name)
                        stb.AppendLine()
                End Select
                stb.AppendLine()
            Case Language.Chinese
                Select Case Me.vMolecularOperation
                    Case Nuctions.MolecularOperationEnum.Extraction, Nuctions.MolecularOperationEnum.Screen, Nuctions.MolecularOperationEnum.Incubation
                        stb.Append("构建")
                    Case Nuctions.MolecularOperationEnum.Gel, Nuctions.MolecularOperationEnum.Enzyme, Nuctions.MolecularOperationEnum.Ligation
                        stb.Append("准备")
                    Case Else
                        stb.Append("构建")
                End Select
                stb.Append(Name)
                stb.Append("：")
                'stb.AppendLine()
                While CStack.Count > 0
                    i += 1
                    'stb.AppendFormat("({0}) ", i.ToString)
                    stb.Append(CStack.Pop.Description(zList))
                    'stb.AppendLine()
                End While
                'stb.AppendFormat("本步骤所得标记为{0}的产物即为{1}", LabelName, Name)
                'stb.AppendLine()
                stb.AppendLine()
                If Not Summary Then
                    '在Summary模式下 已经在前面生成了引物和合成列表。
                    Dim dict As Dictionary(Of String, String)
                    dict = PrimerSummary(zList)
                    If dict.Count > 0 Then stb.AppendLine("合成引物:")
                    For Each key As String In dict.Keys
                        stb.AppendFormat("{0}:{1}", key, dict(key))
                        stb.AppendLine()
                    Next
                    dict = SynthesisSummary(zList)
                    If dict.Count > 0 Then stb.AppendLine("合成基因:")
                    For Each key As String In dict.Keys
                        stb.AppendFormat("{0}:{1}", key, dict(key))
                        stb.AppendLine()
                    Next
                End If
            Case Language.Japanese

        End Select
        Return stb.ToString
    End Function
    Public Function ConstructionProcess(Optional Summary As Boolean = False) ', ByVal vRoots As List(Of DNAInfo), ByVal vTreeStack As Stack(Of DNAInfo)) As String
        Dim CStack As New Stack(Of DNAInfo)
        Dim zList As New List(Of DNAInfo)
        '构建一个拓扑逻辑关系堆栈，出堆栈顺序就是正确的构建顺序。
        TraceSourceStack(CStack)
        Dim stb As New System.Text.StringBuilder
        Select Case RegionalLanguage
            Case Language.English
                Select Case Me.vMolecularOperation
                    Case Nuctions.MolecularOperationEnum.Extraction, Nuctions.MolecularOperationEnum.Screen, Nuctions.MolecularOperationEnum.Incubation
                        stb.Append("Construct ")
                    Case Nuctions.MolecularOperationEnum.Gel, Nuctions.MolecularOperationEnum.Enzyme, Nuctions.MolecularOperationEnum.Ligation
                        stb.Append("Prepare ")
                    Case Else
                        stb.Append("Construct ")
                End Select
                stb.Append(Name)
                stb.Append(":")
                While CStack.Count > 0
                    stb.Append(CStack.Pop.Description(zList))
                End While
                stb.AppendLine()
                If Not Summary Then
                    '在Summary模式下 已经在前面生成了引物和合成列表。
                    Dim dict As Dictionary(Of String, String)
                    dict = PrimerSummary(zList)
                    If dict.Count > 0 Then stb.AppendLine("Synthesized Primers:")
                    For Each key As String In dict.Keys
                        stb.AppendFormat("{0}:{1}", key, dict(key))
                        stb.AppendLine()
                    Next
                    dict = SynthesisSummary(zList)
                    If dict.Count > 0 Then stb.AppendLine("Synthesized Genes:")
                    For Each key As String In dict.Keys
                        stb.AppendFormat("{0}:{1}", key, dict(key))
                        stb.AppendLine()
                    Next
                End If
            Case Language.Chinese
                Select Case Me.vMolecularOperation
                    Case Nuctions.MolecularOperationEnum.Extraction, Nuctions.MolecularOperationEnum.Screen, Nuctions.MolecularOperationEnum.Incubation
                        stb.Append("构建")
                    Case Nuctions.MolecularOperationEnum.Gel, Nuctions.MolecularOperationEnum.Enzyme, Nuctions.MolecularOperationEnum.Ligation
                        stb.Append("准备")
                    Case Else
                        stb.Append("构建")
                End Select
                stb.Append(Name)
                stb.Append("：")
                While CStack.Count > 0
                    stb.Append(CStack.Pop.Description(zList))
                End While
                stb.AppendLine()
                If Not Summary Then
                    '在Summary模式下 已经在前面生成了引物和合成列表。
                    Dim dict As Dictionary(Of String, String)
                    dict = PrimerSummary(zList)
                    If dict.Count > 0 Then stb.AppendLine("合成引物:")
                    For Each key As String In dict.Keys
                        stb.AppendFormat("{0}:{1}", key, dict(key))
                        stb.AppendLine()
                    Next
                    dict = SynthesisSummary(zList)
                    If dict.Count > 0 Then stb.AppendLine("合成基因:")
                    For Each key As String In dict.Keys
                        stb.AppendFormat("{0}:{1}", key, dict(key))
                        stb.AppendLine()
                    Next
                End If
            Case Language.Japanese

        End Select
        Return stb.ToString
    End Function
    Public Shared Function PrimerSummary(vList As List(Of DNAInfo)) As Dictionary(Of String, String)
        Dim vDict As New Dictionary(Of String, String)
        For Each di As DNAInfo In vList
            Select Case di.vMolecularOperation
                Case Nuctions.MolecularOperationEnum.PCR
                    If Not vDict.ContainsKey(di.PCR_FPrimerName) Then vDict.Add(di.PCR_FPrimerName, Nuctions.TAGCFilter(di.PCR_ForwardPrimer))
                    If Not vDict.ContainsKey(di.PCR_RPrimerName) Then vDict.Add(di.PCR_RPrimerName, Nuctions.TAGCFilter(di.PCR_ReversePrimer))
                Case Nuctions.MolecularOperationEnum.SequencingResult
                    If Not vDict.ContainsKey(di.SequencingPrimerName) Then vDict.Add(di.SequencingPrimerName, Nuctions.TAGCFilter(di.SequencingPrimer))
                Case Nuctions.MolecularOperationEnum.Screen
                    If di.Screen_Mode = Nuctions.ScreenModeEnum.PCR Then
                        If Not vDict.ContainsKey(di.Screen_FName) Then vDict.Add(di.Screen_FName, Nuctions.TAGCFilter(di.Screen_FPrimer))
                        If Not vDict.ContainsKey(di.Screen_RName) Then vDict.Add(di.Screen_RName, Nuctions.TAGCFilter(di.Screen_RPrimer))
                    End If
            End Select
        Next
        Return vDict
    End Function
    Public Shared Function SynthesisSummary(vList As List(Of DNAInfo)) As Dictionary(Of String, String)
        Dim vDict As New Dictionary(Of String, String)
        For Each di As DNAInfo In vList
            Select Case di.vMolecularOperation
                Case Nuctions.MolecularOperationEnum.FreeDesign
                    If Not vDict.ContainsKey(di.FreeDesignName) Then
                        If di.DNAs.Count > 0 Then
                            Dim gf As Nuctions.GeneFile = di.DNAs(1)
                            vDict.Add(di.FreeDesignName, gf.Sequence)
                        End If
                    End If
            End Select
        Next
        Return vDict
    End Function
    ''' <summary>
    ''' 如果Me不是ConstructionNode，若vStack当中没有Me，则将Me添加到vStack当中。并向Source递归查找。
    ''' 用来按照拓扑关系构建一个vStack，vStack当中出堆栈顺序就是从头开始构建的逻辑顺序。
    ''' 这个函数是调用DNAInfo实例当中ConstructionProcess时调用的。它会在遇到ConstructionNode时停止追踪。
    ''' </summary>
    ''' <param name="vStack">ConstructionProcess所需要的构建拓扑关系堆栈。</param>
    ''' <remarks></remarks>
    Friend Sub TraceSourceStack(ByVal vStack As Stack(Of DNAInfo))
        If Me.Source.Contains(Me) Then Me.Source.Remove(Me)
        If Not vStack.Contains(Me) Then
            vStack.Push(Me)
            For Each sc As DNAInfo In Source
                If Not sc.IsConstructionNode Then sc.TraceSourceStack(vStack)
            Next
        End If
    End Sub
    ''' <summary>
    ''' 如果vList当中有Me但是vStack当中没有Me，则将Me添加到vStack当中。并向Source递归查找。
    ''' 用来把vList当中的元素按照拓扑关系排列成为一个vStack，vStack当中出堆栈顺序就是从头开始构建的逻辑顺序。
    ''' </summary>
    ''' <param name="vStack"></param>
    ''' <param name="vList"></param>
    ''' <remarks></remarks>
    Friend Sub TraceDependencyStack(ByVal vStack As Stack(Of DNAInfo), ByVal vList As List(Of DNAInfo), vVisited As List(Of DNAInfo))
        If Not vVisited.Contains(Me) Then
            If Me.Source.Contains(Me) Then Me.Source.Remove(Me)
            If vList.Contains(Me) Then
                If vStack.Contains(Me) Then
                    '已经访问过的 不用处理
                Else
                    '尚未访问过
                    vList.Remove(Me)
                    vStack.Push(Me)
                    vVisited.Add(Me)
                    For Each sc As DNAInfo In Source
                        sc.TraceDependencyStack(vStack, vList, vVisited)
                    Next
                End If
            Else
                If vStack.Contains(Me) Then
                    '已经访问过的 不用处理
                Else
                    '尚未访问过
                    vVisited.Add(Me)
                    For Each sc As DNAInfo In Source
                        sc.TraceDependencyStack(vStack, vList, vVisited)
                    Next
                End If
            End If

        End If
    End Sub
    ''' <summary>
    ''' 如果vList当中有Me但是vStack当中没有Me，则将Me添加到vStack当中。并向Source递归查找。
    ''' 用来按照拓扑关系构建一个vStack，vStack当中出堆栈顺序就是从头开始构建的逻辑顺序。
    ''' 这个函数是调用DNAInfo实例当中GetConstructionDescription时调用的。它会在遇到ConstructionNode时停止。
    ''' </summary>
    ''' <param name="vStack"></param>
    ''' <param name="vList"></param>
    ''' <remarks></remarks>
    Friend Sub PushSourceStack(ByVal vStack As Stack(Of DNAInfo), ByVal vList As List(Of DNAInfo))
        If Not IsConstructionNode Then
            If (Not vList.Contains(Me)) AndAlso (Not vStack.Contains(Me)) Then
                vList.Add(Me)
                vStack.Push(Me)
                For Each sc As DNAInfo In Source
                    sc.PushSourceStack(vStack, vList)
                Next
            End If
        End If
    End Sub
    ''' <summary>
    ''' 如果vList当中有Me但是vStack当中没有Me，则将Me添加到vTreeStack当中，并从vRoots当中去除Me。并向Source递归查找。
    ''' </summary>
    ''' <param name="vStack"></param>
    ''' <param name="vList"></param>
    ''' <param name="vRoots"></param>
    ''' <param name="vTreeStack"></param>
    ''' <remarks></remarks>
    Friend Sub TraceSourceStack(ByVal vStack As Stack(Of DNAInfo), ByVal vList As List(Of DNAInfo), ByVal vRoots As List(Of DNAInfo), ByVal vTreeStack As Stack(Of DNAInfo))
        If (Not vList.Contains(Me)) AndAlso (Not vStack.Contains(Me)) Then
            If vRoots.Contains(Me) Then
                vTreeStack.Push(Me)
                vRoots.Remove(Me)
            End If
            vList.Add(Me)
            vStack.Push(Me)
            For Each sc As DNAInfo In Source
                sc.TraceSourceStack(vStack, vList, vRoots, vTreeStack)
            Next
        End If
    End Sub

End Class