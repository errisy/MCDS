Partial Public Class Nuctions
    Friend Shared Function ReduceDuplicateCells(cList As List(Of Cell)) As List(Of Cell)
        Dim rList As New List(Of Cell)
        Dim notexists As Boolean = True
        For Each cl In cList
            notexists = True
            For Each tl In rList
                If tl = cl Then notexists = False : Exit For
            Next
            If notexists Then rList.Add(cl)
        Next
        Return rList
    End Function
    Friend Shared Function ReduceDuplicateGroups(cList As List(Of List(Of Nuctions.GeneFile))) As List(Of List(Of Nuctions.GeneFile))
        Dim rList As New List(Of List(Of Nuctions.GeneFile))
        Dim notexists As Boolean = True
        For Each cl In cList
            notexists = True
            For Each tl In rList
                If IsListGeneFileSame(tl, cl) Then notexists = False : Exit For
            Next
            If notexists Then rList.Add(cl)
        Next
        Return rList
    End Function
    Friend Shared Function IsListGeneFileSame(list1 As List(Of Nuctions.GeneFile), list2 As List(Of Nuctions.GeneFile)) As Boolean
        If list1.Count <> list2.Count Then Return False
        Dim gtList As New List(Of Nuctions.GeneFile)
        gtList.AddRange(list2)
        For Each gf In list1
            Dim found As Integer = -1
            For i As Integer = 0 To gtList.Count - 1
                If gtList(i) = gf Then
                    found = i : Exit For
                End If
            Next
            If found > -1 Then gtList.RemoveAt(found)
        Next
        If gtList.Count > 0 Then Return False
        Return True
    End Function
    <Serializable>
    Public Class Host
        Public Name As String = ""
        Public BioFunctions As New List(Of FeatureFunction)
        Public Description As String = ""
        Public Function Clone() As Host
            Dim h As New Host
            h.Name = New String(Name)
            'h.BioFunctions.AddRange(BioFunctions)
            For Each bf As FeatureFunction In BioFunctions
                h.BioFunctions.Add(bf.Clone)
            Next
            h.Description = New String(Description)
            Return h
        End Function
        Public Shared Operator =(H1 As Host, H2 As Host) As Boolean
            Dim eq As Boolean = True
            If H1.Name <> H2.Name Then Return False
            If H1.Description <> H2.Description Then Return False
            If H1.BioFunctions.Count <> H2.BioFunctions.Count Then Return False
            If IsListDifferent(H1.BioFunctions, H2.BioFunctions) Then Return False
            Return True
        End Operator
        Public ReadOnly Property HostFunctions As Dictionary(Of FeatureFunctionEnum, List(Of String))
            Get
                Dim bfd As New Dictionary(Of FeatureFunctionEnum, List(Of String))
                For Each ffe As FeatureFunctionEnum In [Enum].GetValues(GetType(FeatureFunctionEnum))
                    bfd.Add(ffe, New List(Of String))
                Next
                For Each bf As FeatureFunction In BioFunctions
                    bfd(bf.BioFunction).Add(bf.Parameters)
                Next
                Return bfd
            End Get
        End Property
        Public Shared Operator <>(H1 As Host, H2 As Host) As Boolean
            Return Not (H1 = H2)
        End Operator
    End Class
    'feature function: replicate[R6Kgamma]
    'feature function: resistance[Ampicilin]
    'feature function: color[Red]

    Friend Shared Function AnalyzedFeatureCode(vCode As String) As List(Of FeatureFunction)
        Static repRX As New System.Text.RegularExpressions.Regex("Primase\[([^\[^\]^<^>]+)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Static repRXT As New System.Text.RegularExpressions.Regex("Primase\[([^\[^\]^<^>]+[<>][\d\.]+)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Static repRXTH As New System.Text.RegularExpressions.Regex("Primase\[([^\[^\]^<^>]+[<>][\d\.]+@[\w\.\s]+)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Static repRXH As New System.Text.RegularExpressions.Regex("Primase\[(\[^\[^\]^<^>^@]+@[\w\.\s]+)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Static antiRX As New System.Text.RegularExpressions.Regex("AntibioticsResistance\[(\w*)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Static clrRX As New System.Text.RegularExpressions.Regex("Color\[(\w*)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Static flrRX As New System.Text.RegularExpressions.Regex("Fluorescence\[(\w*)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Static ilmRX As New System.Text.RegularExpressions.Regex("Illumination\[(\w*)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Static cjgRX As New System.Text.RegularExpressions.Regex("Conjugation\[(\w*)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        Dim vFF As New List(Of FeatureFunction)
        If vCode Is Nothing Then Return vFF

        For Each m As System.Text.RegularExpressions.Match In repRX.Matches(vCode)
            vFF.Add(New FeatureFunction With {.BioFunction = FeatureFunctionEnum.Primase, .Parameters = m.Groups(1).Value})
        Next
        For Each m As System.Text.RegularExpressions.Match In repRXT.Matches(vCode)
            vFF.Add(New FeatureFunction With {.BioFunction = FeatureFunctionEnum.Primase, .Parameters = m.Groups(1).Value})
        Next
        For Each m As System.Text.RegularExpressions.Match In antiRX.Matches(vCode)
            vFF.Add(New FeatureFunction With {.BioFunction = FeatureFunctionEnum.AntibioticsResistance, .Parameters = m.Groups(1).Value})
        Next
        For Each m As System.Text.RegularExpressions.Match In clrRX.Matches(vCode)
            vFF.Add(New FeatureFunction With {.BioFunction = FeatureFunctionEnum.Color, .Parameters = m.Groups(1).Value})
        Next
        For Each m As System.Text.RegularExpressions.Match In flrRX.Matches(vCode)
            vFF.Add(New FeatureFunction With {.BioFunction = FeatureFunctionEnum.Fluorescence, .Parameters = m.Groups(1).Value})
        Next
        For Each m As System.Text.RegularExpressions.Match In ilmRX.Matches(vCode)
            vFF.Add(New FeatureFunction With {.BioFunction = FeatureFunctionEnum.Illumination, .Parameters = m.Groups(1).Value})
        Next
        For Each m As System.Text.RegularExpressions.Match In cjgRX.Matches(vCode)
            vFF.Add(New FeatureFunction With {.BioFunction = FeatureFunctionEnum.Conjugation, .Parameters = m.Groups(1).Value})
        Next
        Return vFF
    End Function
    Friend Shared Function ExpressFeatureFunctions(vList As List(Of FeatureFunction)) As String
        Dim stb As New System.Text.StringBuilder
        For Each ff As FeatureFunction In vList
            stb.Append(ff.ToString)
        Next
        Return stb.ToString
    End Function
    <Serializable()>
    Public Class FeatureFunction
        Public BioFunction As FeatureFunctionEnum
        Public Parameters As String
        Public Function Clone() As FeatureFunction
            Dim ff As New FeatureFunction
            ff.BioFunction = BioFunction
            ff.Parameters = New String(Parameters)
            Return ff
        End Function
        Public Overrides Function ToString() As String
            Return String.Format("{0}[{1}] ", [Enum].GetName(GetType(FeatureFunctionEnum), BioFunction), Parameters)
        End Function
        Public Shared Operator =(FF1 As FeatureFunction, FF2 As FeatureFunction) As Boolean
            If FF1.BioFunction <> FF2.BioFunction Then Return False
            If FF1.Parameters <> FF1.Parameters Then Return False
            Return True
        End Operator
        Public Shared Operator <>(FF1 As FeatureFunction, FF2 As FeatureFunction) As Boolean
            Return Not (FF1 = FF2)
        End Operator

    End Class
    <Serializable()>
    Public Enum FeatureFunctionEnum As Integer
        Primase
        Color
        Fluorescence
        Illumination
        AntibioticsResistance
        Metabolic
        Conjugation
    End Enum
    Private Shared rgxCellRec As New System.Text.RegularExpressions.Regex("^=(\d+)\-(\d+)")
    Private Shared rgxFreeEnd As New System.Text.RegularExpressions.Regex("^[\*\^]")
    Public Class FragmentGroup
        Public Count As Integer
        Public Genes As New List(Of GeneFile)
        Public Rest As New List(Of GeneFile)
        Public Numbers As New List(Of Integer)
        Public Sub New()

        End Sub
        Public Sub New(gList As List(Of GeneFile))
            Rest.AddRange(gList)
            Dim m As System.Text.RegularExpressions.Match
            Dim id As Integer
            If gList.Count > 0 Then
                Dim gf As GeneFile = gList(0)
                If rgxCellRec.IsMatch(gf.End_F) Then
                    m = rgxCellRec.Match(gf.End_F)
                    If Integer.TryParse(m.Groups(2).Value, id) Then
                        Count = id
                    End If
                End If
                If rgxCellRec.IsMatch(gf.End_R) Then
                    m = rgxCellRec.Match(gf.End_R)
                    If Integer.TryParse(m.Groups(2).Value, id) Then
                        Count = id
                    End If
                End If
            End If
        End Sub

        Public Function Clone() As FragmentGroup
            Dim fg As New FragmentGroup With {.Count = Count}
            fg.Genes.AddRange(Genes)
            fg.Rest.AddRange(Rest)
            fg.Numbers.AddRange(Numbers)
            Return fg
        End Function
        Public Function LigateOne() As List(Of FragmentGroup)
            Dim nfg As New List(Of FragmentGroup)
            Dim nextgroup As New List(Of GeneFile)

            Dim m As System.Text.RegularExpressions.Match
            Dim mo As System.Text.RegularExpressions.Match
            Dim nextid As Integer
            For i As Integer = 1 To Count
                If Not Numbers.Contains(i) Then nextid = i : Exit For
            Next
            Dim id As Integer
            Dim other As Integer
            For Each gf As GeneFile In Rest
                If rgxCellRec.IsMatch(gf.End_F) Then
                    m = rgxCellRec.Match(gf.End_F)
                    If Integer.TryParse(m.Groups(1).Value, id) Then
                        If id = nextid Then
                            If rgxCellRec.IsMatch(gf.End_R) Then
                                mo = rgxCellRec.Match(gf.End_R)
                                If Integer.TryParse(m.Groups(1).Value, other) Then
                                    If Not Numbers.Contains(other) Then
                                        nextgroup.Add(gf)
                                    End If
                                End If
                            Else
                                nextgroup.Add(gf)
                            End If
                        End If

                    End If
                End If
                If rgxCellRec.IsMatch(gf.End_R) Then
                    m = rgxCellRec.Match(gf.End_R)
                    If Integer.TryParse(m.Groups(1).Value, id) Then
                        If id = nextid Then
                            If rgxCellRec.IsMatch(gf.End_F) Then
                                mo = rgxCellRec.Match(gf.End_F)
                                If Integer.TryParse(m.Groups(1).Value, other) Then
                                    If Not Numbers.Contains(other) Then
                                        nextgroup.Add(gf)
                                    End If
                                End If
                            Else
                                nextgroup.Add(gf)
                            End If
                        End If
                    End If
                End If
            Next
            Dim fg As FragmentGroup
            For Each gf In nextgroup
                fg = Me.Clone
                fg.Ligate(gf)
                nfg.Add(fg)
            Next
            Return nfg
        End Function
        Public ReadOnly Property Full As Boolean
            Get
                For i As Integer = 1 To Count
                    If Not Numbers.Contains(i) Then Return False
                Next
                Return True
            End Get
        End Property
        Public Sub Ligate(gf As GeneFile)
            Dim m As System.Text.RegularExpressions.Match
            Dim id As Integer
            Genes.Add(gf)
            Rest.Remove(gf)
            If rgxCellRec.IsMatch(gf.End_R) Then
                m = rgxCellRec.Match(gf.End_R)
                If Integer.TryParse(m.Groups(1).Value, id) Then
                    Numbers.Add(id)
                End If
            End If
            If rgxCellRec.IsMatch(gf.End_F) Then
                m = rgxCellRec.Match(gf.End_F)
                If Integer.TryParse(m.Groups(1).Value, id) Then
                    Numbers.Add(id)
                End If
            End If
        End Sub
    End Class
    <Serializable>
    Public Class Cell

        Public Shared Operator =(c1 As Cell, c2 As Cell)
            If c1.Host <> c2.Host Then Return False
            If c1.DNAs.Count <> c2.DNAs.Count Then Return False
            Dim gtList As New List(Of Nuctions.GeneFile)
            gtList.AddRange(c2.DNAs)
            For Each gf In c1.DNAs
                Dim found As Integer = -1
                For i As Integer = 0 To gtList.Count - 1
                    If gtList(i) = gf Then
                        found = i : Exit For
                    End If
                Next
                If found > -1 Then gtList.RemoveAt(found)
            Next
            If gtList.Count > 0 Then Return False
            Return True
        End Operator
        Public Shared Operator <>(c1 As Cell, c2 As Cell)
            Return Not (c1 = c2)
        End Operator
        Public Sub AddRange(vList As IEnumerable(Of GeneFile))
            DNAs.AddRange(vList)
        End Sub
        Public Sub Add(gf As GeneFile)
            DNAs.Add(gf)
        End Sub
        Public Sub PrepareRecombine()
            Dim count As Integer = 0
            For Each gf In DNAs
                If gf.End_F.StartsWith("=") Then
                    count += 1
                End If
                If gf.End_R.StartsWith("=") Then
                    count += 1
                End If
            Next
            Dim i As Integer = 1
            For Each gf In DNAs
                If gf.End_F.StartsWith("=") Then
                    gf.End_F = String.Format("={0}-{1}", i, count)
                    i += 1
                End If
                If gf.End_R.StartsWith("=") Then
                    gf.End_R = String.Format("={0}-{1}", i, count)
                    i += 1
                End If
            Next
        End Sub
        Public Function FixRecombine() As List(Of Cell)
            '先分离含有=的序列
            Dim gn As New List(Of GeneFile)
            Dim mx As New List(Of GeneFile)


            Dim ggfgr As New GenomicGeneFileGrouper(DNAs)

            Dim cList As New List(Of Cell)
            Dim c As Cell
            Dim gt As Nuctions.GeneFile
            For Each vList In ggfgr.GetFullDividedGroup()
                c = New Cell
                c.Host = Host.Clone
                For Each gf As GeneFile In vList
                    gt = gf.Clone
                    If rgxCellRec.IsMatch(gt.End_F) Then gt.End_F = "=B"
                    If rgxCellRec.IsMatch(gt.End_R) Then gt.End_R = "=B"
                    c.DNAs.Add(gt)
                Next
                cList.Add(c)
            Next


            'how to deal with the other methods?
            'ss()

            'For Each gf As GeneFile In DNAs
            '    gf.GenomeIndex()

            'Next


            'For Each gf As GeneFile In DNAs
            '    If gf.End_F.StartsWith("=") Or gf.End_R.StartsWith("=") Then
            '        gn.Add(gf)
            '    Else
            '        mx.Add(gf)
            '    End If
            'Next



            'For Each vList As List(Of GeneFile) In FindFullFragmentGroup(gn)
            '    c = New Cell
            '    c.Host = Host.Clone
            '    For Each gf As GeneFile In mx
            '        c.DNAs.Add(gf.Clone)
            '    Next
            '    For Each gf As GeneFile In vList
            '        gt = gf.Clone
            '        If rgxCellRec.IsMatch(gt.End_F) Then gt.End_F = "=B"
            '        If rgxCellRec.IsMatch(gt.End_R) Then gt.End_R = "=B"
            '        c.DNAs.Add(gt)
            '    Next
            '    cList.Add(c)
            'Next
            Return cList
        End Function
        Friend Shared Function FindFullFragmentGroup(gList As List(Of GeneFile)) As List(Of List(Of GeneFile))
            Dim fgList As New List(Of FragmentGroup)
            Dim ntList As List(Of FragmentGroup)
            Dim nxList As New List(Of FragmentGroup)
            Dim fg As New FragmentGroup(gList)
            Dim res As New List(Of List(Of GeneFile))
            fgList.Add(fg)
            While fgList.Count > 0
                nxList.Clear()
                For Each fx As FragmentGroup In fgList
                    If fx.Full Then
                        res.Add(fx.Genes)
                    Else
                        ntList = fx.LigateOne
                        nxList.AddRange(ntList)
                    End If
                Next
                fgList.Clear()
                fgList.AddRange(nxList)
            End While
            Dim dups As New List(Of List(Of GeneFile))
            For Each l1 As List(Of GeneFile) In res
                If Not dups.Contains(l1) Then
                    For Each l2 As List(Of GeneFile) In res
                        If Not dups.Contains(l2) Then
                            If l1 IsNot l2 Then
                                If IsRefListSame(l1, l2) Then dups.Add(l2)
                            End If
                        End If
                    Next
                End If
            Next
            For Each l As List(Of GeneFile) In dups
                res.Remove(l)
            Next
            Return res
        End Function
        Public DNAs As New List(Of GeneFile)
        Public Host As New Host
        '实际上是一个DNA的集合体
        '名称是为了表示宿主的名称
        Public Function Clone() As Cell
            Dim c As New Cell
            For Each gf As GeneFile In DNAs
                c.Add(gf.CloneWithoutFeatures)
            Next
            c.Host = Me.Host.Clone
            Return c
        End Function
        Dim bufferedReplication As New List(Of String)
        Public Function GetReplicationType(NotBufferd As Boolean, temperature As Single) As List(Of String)
            If NotBufferd Then
                bufferedReplication.Clear()
                Dim bfList As New List(Of FeatureFunction)
                bfList.AddRange(Host.BioFunctions)
                Dim f As Feature
                For Each gf As GeneFile In DNAs
                    For Each ga As GeneAnnotation In gf.Features
                        If ga.Feature IsNot Nothing Then
                            f = ga.Feature
                            bfList.AddRange(f.BioFunctions)
                        End If
                    Next
                Next
                For Each bf As FeatureFunction In bfList
                    If bf.BioFunction = FeatureFunctionEnum.Primase Then
                        If bf.Parameters.Contains(">") Then
                            Dim temp As Single = 0.0F
                            Dim idx As Integer = bf.Parameters.LastIndexOf(">")
                            If Single.TryParse(bf.Parameters.Substring(idx + 1), temp) Then
                                If temp < temperature Then
                                    bufferedReplication.Add(bf.Parameters.Substring(0, idx))
                                End If
                            End If
                        ElseIf bf.Parameters.Contains("<") Then
                            Dim temp As Single = 0.0F
                            Dim idx As Integer = bf.Parameters.LastIndexOf(">")
                            If Single.TryParse(bf.Parameters.Substring(idx + 1), temp) Then
                                If temp > temperature Then
                                    bufferedReplication.Add(bf.Parameters.Substring(0, idx))
                                End If
                            End If
                        Else
                            bufferedReplication.Add(bf.Parameters)
                        End If
                    End If
                Next
                Return bufferedReplication
            Else
                Return bufferedReplication
            End If
        End Function
        Dim bufferedConjugation As New List(Of String)
        Public Function GetConjugationType(NotBufferd As Boolean) As List(Of String)
            If NotBufferd Then
                bufferedConjugation.Clear()
                Dim bfList As New List(Of FeatureFunction)
                bfList.AddRange(Host.BioFunctions)
                Dim f As Feature
                For Each gf As GeneFile In DNAs
                    For Each ga As GeneAnnotation In gf.Features
                        If ga.Feature IsNot Nothing Then
                            f = ga.Feature
                            bfList.AddRange(f.BioFunctions)
                        End If
                    Next
                Next
                For Each bf As FeatureFunction In bfList
                    If bf.BioFunction = FeatureFunctionEnum.Conjugation Then
                        bufferedConjugation.Add(bf.Parameters)
                    End If
                Next
                Return bufferedConjugation
            Else
                Return bufferedConjugation
            End If
        End Function


        ''' <summary>
        ''' 模拟单个细胞培养时的质粒变化.
        ''' </summary>
        ''' <param name="antibioticsinmedium">a list of antibiotics.</param>
        ''' <param name="temperature">the temperature at which the cell is incubated.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Incubate(antibioticsinmedium As List(Of Antibiotics), temperature As Single, Features As List(Of Feature), time As TimeSpan) As Cell
            If time = TimeSpan.Zero Then
                '在无限长时间里，考虑质粒的丢失。
                Dim antibiotics As New List(Of Antibiotics)
                antibiotics.AddRange(antibioticsinmedium)
                'remove that are not circular and not in genome.
                'read biofunctions
                AddFeatures(DNAs, Features)
                Dim vGenome As New Dictionary(Of GeneFile, Dictionary(Of FeatureFunctionEnum, List(Of String)))
                Dim vMids As New Dictionary(Of GeneFile, Dictionary(Of FeatureFunctionEnum, List(Of String)))
                Dim hFs As Dictionary(Of FeatureFunctionEnum, List(Of String)) = Host.HostFunctions
                For Each gf As GeneFile In DNAs
                    If gf.IsClosedVector Then
                        If Not vMids.ContainsKey(gf) Then vMids.Add(gf, gf.BioFunctions)
                    ElseIf gf.Chromosomal Then
                        vGenome.Add(gf, gf.BioFunctions)
                    End If
                Next

                '再去除不能复制的环形DNA
                Dim oris As New List(Of String)
                For Each gf As GeneFile In vGenome.Keys
                    oris.AddRange(vGenome(gf)(FeatureFunctionEnum.Primase))
                Next
                For Each gf As GeneFile In vMids.Keys
                    oris.AddRange(vMids(gf)(FeatureFunctionEnum.Primase))
                Next
                oris.AddRange(hFs(FeatureFunctionEnum.Primase))

                Dim primases As New List(Of String)

                Dim vit As Integer
                Dim val As Single
                For Each s As String In oris
                    If s.Contains(">") Then
                        vit = s.IndexOf(">")
                        If Single.TryParse(s.Substring(vit + 1), val) Then
                            If temperature > val AndAlso Not primases.Contains(s.Substring(0, vit)) Then primases.Add(s.Substring(0, vit))
                        End If
                    ElseIf s.Contains("<") Then
                        vit = s.IndexOf("<")
                        If Single.TryParse(s.Substring(vit + 1), val) Then
                            If temperature < val AndAlso Not primases.Contains(s.Substring(0, vit)) Then primases.Add(s.Substring(0, vit))
                        End If
                    Else
                        If Not primases.Contains(s) Then primases.Add(s)
                    End If
                Next

                Dim reps As New List(Of GeneFile)
                Dim notreps As New List(Of GeneFile)
                For Each gf As GeneFile In vMids.Keys
                    If gf.CanReplicate(primases) Then
                        reps.Add(gf)
                    Else
                        notreps.Add(gf)
                    End If
                Next
                For Each gf As GeneFile In notreps
                    vMids.Remove(gf)
                Next

                'screen with antibiotics first
                Dim ants As New List(Of String)

                '去除基因组当中已经有的抗生素抗性
                For Each gf As GeneFile In vGenome.Keys
                    ants.AddRange(vGenome(gf)(FeatureFunctionEnum.AntibioticsResistance))
                Next
                ants.AddRange(hFs(FeatureFunctionEnum.AntibioticsResistance))

                Dim eab As Antibiotics
                For Each abs As String In ants
                    If [Enum].TryParse(abs, eab) Then
                        If antibiotics.Contains(eab) Then antibiotics.Remove(eab)
                    End If
                Next

                Dim ABG As New Dictionary(Of Antibiotics, List(Of GeneFile))
                For Each ab As Antibiotics In antibiotics
                    ABG.Add(ab, New List(Of GeneFile))
                Next
                Dim atbsmid As List(Of Antibiotics)
                For Each gf As GeneFile In vMids.Keys
                    atbsmid = ParseAntibiotics(vMids(gf)(FeatureFunctionEnum.AntibioticsResistance))
                    For Each ab As Antibiotics In atbsmid
                        If ABG.ContainsKey(ab) Then ABG(ab).Add(gf)
                    Next
                Next

                Dim res As New Cell
                res.Host = Me.Host.Clone
                For Each vPeri As GeneFile In vGenome.Keys
                    res.DNAs.Add(vPeri.CloneWithoutFeatures)
                Next

                'Dim vhasanti As Boolean

                Dim remains As New List(Of GeneFile)


                If antibiotics.Count > 0 Then

                    Dim survival As Boolean = True
                    Dim smallestGF As GeneFile
                    For Each ab As Antibiotics In ABG.Keys
                        If ABG(ab).Count = 0 Then
                            survival = False
                        Else
                            smallestGF = Nothing
                            For Each gf As GeneFile In ABG(ab)
                                If smallestGF Is Nothing OrElse smallestGF.Length > gf.Length Then smallestGF = gf
                            Next
                            ABG(ab).Clear()
                            If smallestGF IsNot Nothing AndAlso Not res.DNAs.Contains(smallestGF) Then res.Add(smallestGF)
                        End If
                    Next
                    If survival Then
                        Return res
                    Else
                        Return Nothing
                    End If
                Else
                    Return res
                End If
            Else
                Return Clone()
            End If
        End Function
    End Class
    Const TypicalNa As Single = 80 * 0.001
    Const TypicalC As Single = 625 * 0.000000001
    Public Structure CharValue
        Public Sub New(vItem1 As Char, vItem2 As Single)
            Item1 = vItem1
            Item2 = vItem2
        End Sub
        Public Item1 As Char
        Public Item2 As Single
    End Structure
    Public Class PrimerExtender
        Public OtherPrimers As New List(Of String)
        Public MainPrimer As String = ""
        Public Templates As New List(Of GeneFile)
        Public Function Extend(Value As Integer) As String
            Dim vList As New List(Of Char)
            For i As Integer = 1 To Value
                ExtendOne(MainPrimer, vList)
            Next
            Dim stb As New System.Text.StringBuilder
            For Each _char As Char In vList
                stb.Insert(0, _char)
            Next
            'stb.Append(MainPrimer)
            Return stb.ToString
        End Function
        'Private Shared Nucleotides As New List(Of Char) From {"A"c, "T"c, "G"c, "C"c}
        Public Sub ExtendOne(vPrimer As String, vList As List(Of Char))
            Dim stb As New System.Text.StringBuilder
            For Each _char As Char In vList
                stb.Insert(0, _char)
            Next
            stb.Append(vPrimer)
            Dim vp As String = stb.ToString
            Dim vDict As New List(Of CharValue)
            vDict.Add(New CharValue("A"c, 40.0F))
            vDict.Add(New CharValue("T"c, 40.0F))
            vDict.Add(New CharValue("G"c, 40.0F))
            vDict.Add(New CharValue("C"c, 40.0F))
            System.Threading.Tasks.Parallel.ForEach(vDict, Sub(kvp As CharValue)
                                                               Dim pa As New PrimerAnalyzer
                                                               Dim dList As New Dictionary(Of String, String)
                                                               Dim j As Integer = 0
                                                               dList.Add(j.ToString, kvp.Item1 + vp)
                                                               For Each s As String In OtherPrimers
                                                                   j += 1
                                                                   dList.Add(j.ToString, s)
                                                               Next
                                                               Dim par = AnalyzePrimer(dList, Templates, TypicalNa, TypicalC)
                                                               kvp.Item2 = par.GetScore
                                                           End Sub)
            Dim nV As Single = Single.MaxValue
            Dim rList As New List(Of Char)
            For Each kvx In vDict
                If nV > kvx.Item2 Then
                    nV = kvx.Item2
                End If
            Next
            For Each kvx In vDict
                If kvx.Item2 = nV Then rList.Add(kvx.Item1)
            Next
            vList.Add(rList(Math.Floor(Rnd(Now.ToOADate) * rList.Count)))
        End Sub
        'Public Function Analyze(vPrimer As String) As System.Threading.Tasks.Task(Of Single)
        '    Dim t As New System.Threading.Tasks.Task(Of Single)(Function() As Single
        '                                                            Dim pa As New PrimerAnalyzer
        '                                                            Dim dList As New Dictionary(Of String, String)
        '                                                            Dim j As Integer = 0
        '                                                            dList.Add(j.ToString, vPrimer)
        '                                                            For Each s As String In OtherPrimers
        '                                                                j += 1
        '                                                                dList.Add(j.ToString, s)
        '                                                            Next
        '                                                            Dim par = AnalyzePrimer(dList, Templates, TypicalNa, TypicalC)
        '                                                            Return par.GetScore
        '                                                        End Function)
        '    t.Start()
        '    Return t
        'End Function
    End Class

End Class

Public Module GenomeFileIndex
    <System.Runtime.CompilerServices.Extension> Public Function GenomeIndex(gf As Nuctions.GeneFile) As Tuple(Of Integer, Integer)
        Static idx As New System.Text.RegularExpressions.Regex("^\=(\d+)\-\d+")
        Dim idf As Integer = 0
        Dim mf = idx.Match(gf.End_F)
        If mf.Success Then
            idf = CInt(mf.Groups(1).Value)
        End If
        Dim idr As Integer = 0
        Dim mr = idx.Match(gf.End_R)
        If mf.Success Then
            idr = CInt(mr.Groups(1).Value)
        End If
        Return New Tuple(Of Integer, Integer)(idf, idr)
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function GenomeFIndex(gf As Nuctions.GeneFile) As Integer
        Static idx As New System.Text.RegularExpressions.Regex("^\=(\d+)\-\d+")
        Dim id As Integer = 0
        Dim m = idx.Match(gf.End_F)
        If m.Success Then
            id = CInt(m.Groups(1).Value)
        End If
        Return id
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function GenomeRIndex(gf As Nuctions.GeneFile) As Integer
        Static idx As New System.Text.RegularExpressions.Regex("^\=(\d+)\-\d+")
        Dim id As Integer = 0
        Dim m = idx.Match(gf.End_R)
        If m.Success Then
            id = CInt(m.Groups(1).Value)
        End If
        Return id
    End Function
End Module

Public Class GenomicGeneFileGrouper

    Private IndexMap As New List(Of Integer)
    Private Unused As New List(Of Nuctions.GeneFile)
    Public Sub New(pool As List(Of Nuctions.GeneFile))
        Dim c0List As New List(Of Nuctions.GeneFile)
        For Each g In pool
            Dim f As Integer = g.GenomeFIndex
            Dim r As Integer = g.GenomeRIndex
            If f > 0 Or r > 0 Then
                If f <> r Then
                    If f > 0 AndAlso Not IndexMap.Contains(f) Then IndexMap.Add(f)
                    If r > 0 AndAlso Not IndexMap.Contains(r) Then IndexMap.Add(r)
                    c0List.Add(g)
                End If
            Else
                Unused.Add(g)
            End If
        Next
        Groups.Add(New GenomicGeneFileGroup(IndexMap, c0List))
    End Sub
    Private Groups As New List(Of GenomicGeneFileGroup)
    Private Function Reduce() As Boolean
        Dim reduced As Boolean = False
        Dim nextlevel As New List(Of GenomicGeneFileGroup)
        For Each ggfg In Groups
            If ggfg.CanReduce Then
                Dim thisreduced As Boolean = False
                For Each gn In ggfg.Reduce(thisreduced)
                    nextlevel.Add(gn)
                Next
                reduced = reduced Or thisreduced
            Else
                nextlevel.Add(ggfg)
            End If
        Next
        Groups = nextlevel
        Return reduced
    End Function

    Public Function DividedGroups() As List(Of List(Of Nuctions.GeneFile))
        While Reduce()
        End While
        Dim divList As New List(Of List(Of Nuctions.GeneFile))
        For Each gp In Groups
            divList.Add(New List(Of Nuctions.GeneFile)(gp))
        Next
        divList = Nuctions.ReduceDuplicateGroups(divList)
        Return divList
    End Function
    Public ReadOnly Property UnusedGroups As List(Of List(Of Nuctions.GeneFile))
        Get
            Dim divList As New List(Of List(Of Nuctions.GeneFile))
            Dim limit As Integer = 1
            For j As Integer = 1 To Unused.Count
                limit *= 2
            Next
            limit -= 1

            For i As Integer = 0 To limit
                Dim gList As New List(Of Nuctions.GeneFile)
                Dim k As Integer = i
                For j As Integer = 0 To Unused.Count - 1
                    If k Mod 2 = 1 Then
                        gList.Add(Unused(j))
                        k -= 1
                    End If
                    k = k / 2
                Next
                divList.Add(gList)
            Next
            Return divList
        End Get
    End Property

    Public Function GetFullDividedGroup() As List(Of List(Of Nuctions.GeneFile))
        Dim divList As New List(Of List(Of Nuctions.GeneFile))
        Dim usList = UnusedGroups
        For Each gp In DividedGroups()
            For Each up In usList
                Dim gList As New List(Of Nuctions.GeneFile)
                gList.AddRange(gp)
                gList.AddRange(up)
                divList.Add(gList)
            Next
        Next
        Return divList
    End Function
End Class

Public Class GenomicGeneFileGroup
    Inherits List(Of Nuctions.GeneFile)
    Public Sub New()
    End Sub
    Public Sub New(genefiles As IEnumerable(Of Nuctions.GeneFile))
        For Each gf In genefiles
            Add(gf)
        Next
    End Sub
    Public Sub New(indices As IEnumerable(Of Integer), restList As IEnumerable(Of Nuctions.GeneFile))
        iMap.AddRange(indices)
        Rest.AddRange(restList)
    End Sub
    Dim iMap As New List(Of Integer)
    Dim Rest As New List(Of Nuctions.GeneFile)
    Private Function TryAdd(gf As Nuctions.GeneFile) As GenomicGeneFileGroup
        Dim f As Integer = gf.GenomeFIndex
        Dim r As Integer = gf.GenomeRIndex
        If iMap.Contains(f) Or iMap.Contains(r) Then

            Dim ggfg = Clone()
            ggfg.Add(gf)
            If ggfg.iMap.Contains(f) Then ggfg.iMap.Remove(f)
            If ggfg.iMap.Contains(r) Then ggfg.iMap.Remove(r)
            ggfg.Rest.Remove(gf)
            Return ggfg
        End If
        Return Nothing
    End Function
    Public Function Reduce(ByRef reduced As Boolean) As List(Of GenomicGeneFileGroup)
        If Rest.Count = 0 Then reduced = False : Return New List(Of GenomicGeneFileGroup) From {Me}
        If iMap.Count = 0 Then reduced = False : Return New List(Of GenomicGeneFileGroup) From {Me}
        Dim nextlevel As New List(Of GenomicGeneFileGroup)
        For Each rg In Rest
            Dim gn = TryAdd(rg)
            If gn IsNot Nothing Then nextlevel.Add(gn)
        Next
        If nextlevel.Count = 0 Then
            reduced = False
            Return New List(Of GenomicGeneFileGroup) From {Me}
        Else
            reduced = True
            Return nextlevel
        End If
    End Function
    Public Function Clone() As GenomicGeneFileGroup
        Return New GenomicGeneFileGroup(Me) With {.iMap = New List(Of Integer)(iMap), .Rest = New List(Of Nuctions.GeneFile)(Rest)}
    End Function
    Public ReadOnly Property CanReduce As Boolean
        Get
            Return Rest.Count > 0 AndAlso iMap.Count > 0
        End Get
    End Property
End Class