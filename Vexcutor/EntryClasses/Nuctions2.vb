

Partial Public Class Nuctions

    Public Class Energy
        Public dS As Single
        Public dH As Single
        Public dG As Single
        Public Sub New(ByVal vH As Single, ByVal vS As Single, ByVal vG As Single)
            dS = vS
            dH = vH
            dG = vG
        End Sub
    End Class

    Public Shared EnergyDict As New Dictionary(Of String, Energy)

    Public Structure OligoInfo
        Public Tm As Single
        Public dG As Single
        Public Sub New(ByVal vTm As Single, ByVal vdG As Single)
            Tm = vTm
            dG = vdG
        End Sub
    End Structure

    Friend Shared Function CalculateTm(ByVal Oligo As String, ByVal Na As Single, ByVal C As Single) As OligoInfo
        Static R As Single = 1.987
        Static S0 As Single = -10.8
        Static regGC As New System.Text.RegularExpressions.Regex("[GC][GC]")
        Static regAT As New System.Text.RegularExpressions.Regex("[AT][AT]")
        If EnergyDict.Count = 0 Then
            Dim ed As Energy
            '一种16种相邻序列
            'AA/TT -9.1 -24.0 -1.9
            ed = New Energy(-9.1, -24.0, -1.9)
            EnergyDict.Add("AA", ed)
            EnergyDict.Add("TT", ed)
            'AT/TA -8.6 -23.9 -1.5
            ed = New Energy(-8.6, -23.9, -1.5)
            EnergyDict.Add("AT", ed)
            'TA/AT -6.0 -16.9 -1.0
            ed = New Energy(-6.0, -16.9, -1.0)
            EnergyDict.Add("TA", ed)
            'CA/GT -5.8 -12.9 -2.0
            ed = New Energy(-5.8, -12.9, -2.0)
            EnergyDict.Add("CA", ed)
            EnergyDict.Add("TG", ed)
            'GT/CA -6.5 -17.3 -1.3
            ed = New Energy(-6.5, -17.3, -1.3)
            EnergyDict.Add("GT", ed)
            EnergyDict.Add("AC", ed)
            'CT/GA -7.8 -20.8 -1.6
            ed = New Energy(-7.8, -20.8, -1.6)
            EnergyDict.Add("CT", ed)
            EnergyDict.Add("AG", ed)
            'GA/CT -5.6 -13.5 -1.6
            ed = New Energy(-5.6, -13.5, -1.6)
            EnergyDict.Add("GA", ed)
            EnergyDict.Add("TC", ed)
            'CG/GC -11.9 -27.8 -3.6
            ed = New Energy(-11.9, -27.8, -3.6)
            EnergyDict.Add("CG", ed)
            'GC/CG -11.1 -26.7 -3.1
            ed = New Energy(-11.1, -26.7, -3.1)
            EnergyDict.Add("GC", ed)
            'GG/CC -11.0 -26.6 -3.1
            ed = New Energy(-11.0, -26.6, -3.1)
            EnergyDict.Add("GG", ed)
            EnergyDict.Add("CC", ed)
        End If

        Oligo = TAGCFilter(Oligo)
        Dim key As String
        Dim H As Single
        Dim S As Single
        Dim G As Single

        For i As Integer = 0 To Oligo.Length - 2
            key = Oligo.Substring(i, 2)
            H += EnergyDict(key).dH
            G += EnergyDict(key).dG
            S += EnergyDict(key).dS
        Next
        S += S0
        H = H * 1000

        If Oligo.Length > 2 Then
            If Not regAT.IsMatch(Oligo) Then
                G += 6
            ElseIf regGC.IsMatch(Oligo) Then
                G += 5
            Else

            End If
        End If

        'A + B = C

        'https://sg.idtdna.com/Analyzer/Applications/Instructions/Default.aspx?AnalyzerDefinitions=true
        'Tm = ΔH/(ΔS°+ R * ln[oligo]) - 273.15



        'Therm. Tm = dH°- 273.15 + 16.6(log[Na+]) dS° + dSo° + R(ln(c/4))

        Return New OligoInfo(H / (S + S0 + R * Math.Log(C / 4)) - 273.15 + 16.6 * Math.Log10(Na), G)
    End Function

    Friend Class PrimerAnalysisResult
        Public Primers As New Dictionary(Of String, String)
        Public Hairpins As New Dictionary(Of String, List(Of StructInfo))
        Public Dimers As New Dictionary(Of String, List(Of StructInfo))
        Public CrossDimers As New List(Of StructInfo)
        Public Primings As New Dictionary(Of String, List(Of PrimeInfo))
        Public Products As List(Of GeneFile)

        Public Function GetScore() As Single
            Return MaxHairpinEntropyScore() * 2 + MaxDimerEntropyScore() + MaxCrossDimerEntropyScore()
        End Function
        Public Function MaxHairpinEntropyScore() As Single
            Dim m = MaxHairpinEntropy()
            If Single.IsInfinity(m) Then
                Return 8.0F
            Else
                Return m
            End If
        End Function
        Public Function MaxDimerEntropyScore() As Single
            Dim m = MaxDimerEntropy()
            If Single.IsInfinity(m) Then
                Return 8.0F
            Else
                Return m
            End If
        End Function
        Public Function MaxCrossDimerEntropyScore() As Single
            Dim m = MaxCrossDimerEntropy()
            If Single.IsInfinity(m) Then
                Return 8.0F
            Else
                Return m
            End If
        End Function
        Private _MaxHE As Single = Single.PositiveInfinity
        Private _MaxHC As Boolean = False
        Public Function MaxHairpinEntropy() As Single
            If _MaxHC Then
                Return _MaxHE
            Else
                For Each sl In Hairpins.Values
                    Dim tp As Single = Single.MaxValue
                    If sl.Any Then
                        If _MaxHE > sl(0).AG Then _MaxHE = sl(0).AG
                    End If
                Next
                _MaxHC = True
                Return _MaxHE
            End If
        End Function
        Private _MaxDE As Single = Single.PositiveInfinity
        Private _MaxDC As Boolean = False
        Public Function MaxDimerEntropy() As Single
            If _MaxDC Then
                Return _MaxDE
            Else
                For Each sl In Dimers.Values
                    Dim tp As Single = Single.MaxValue
                    If sl.Any Then
                        If _MaxDE > sl(0).AG Then _MaxDE = sl(0).AG
                    End If
                Next
                _MaxDC = True
                Return _MaxDE
            End If
        End Function
        Private _MaxCE As Single = Single.PositiveInfinity
        Private _MaxCC As Boolean = False
        Public Function MaxCrossDimerEntropy() As Single
            If _MaxCC Then
                Return _MaxCE
            Else
                If CrossDimers.Any Then
                    If _MaxCE > CrossDimers(0).AG Then _MaxCE = CrossDimers(0).AG
                End If
                _MaxCC = True
                Return _MaxCE
            End If
        End Function
    End Class

    Public Class StructInfo
        Implements System.IComparable(Of StructInfo)
        Public K1 As String
        Public K2 As String
        Public S1 As Integer
        Public S2 As Integer
        Public E1 As Integer
        Public E2 As Integer
        Public AG As Single
        Public Function CompareTo(ByVal other As StructInfo) As Integer Implements System.IComparable(Of StructInfo).CompareTo
            Return Math.Sign(AG - other.AG)
        End Function
    End Class

    Public Class PrimeInfo
        Implements System.IComparable(Of PrimeInfo)
        Public K1 As String
        Public K2 As GeneFile
        Public IsRC As Boolean
        Public S1 As Integer
        Public S2 As Integer
        Public E1 As Integer
        Public E2 As Integer
        Public AG As Single
        Public Function CompareTo(ByVal other As PrimeInfo) As Integer Implements System.IComparable(Of PrimeInfo).CompareTo
            Return Math.Sign(AG - other.AG)
        End Function
    End Class

    Friend Class PrimerAnalyzer
        Public Event ReturnResult(ByVal PAR As PrimerAnalysisResult)
        Public Function AsyncAnalyze(ByVal Primers As Dictionary(Of String, String), ByVal temps As List(Of GeneFile), ByVal Na As Single, ByVal C As Single) As System.Threading.Tasks.Task(Of PrimerAnalysisResult)
            Dim t As New System.Threading.Tasks.Task(Of PrimerAnalysisResult)(Function() As PrimerAnalysisResult
                                                                                  Return Analyze(Primers, temps, Na, C)
                                                                              End Function)
            't.Start()
            Return t
        End Function
        Public Function Analyze(ByVal Primers As Dictionary(Of String, String), ByVal temps As List(Of GeneFile), ByVal Na As Single, ByVal C As Single) As PrimerAnalysisResult
            Dim reg As System.Text.RegularExpressions.Regex
            Dim info As StructInfo
            Dim pmr As String

            Dim PAR As New PrimerAnalysisResult

            For Each key As String In Primers.Keys
                pmr = Primers(key)
                If pmr Is Nothing OrElse pmr.Length < 5 Then Continue For
                PAR.Primers.Add(key, pmr)
            Next

            If PAR.Primers.Count = 0 Then
                PAR.Products = New List(Of GeneFile)
                RaiseEvent ReturnResult(PAR)
            End If

            '分析发卡
            info = New StructInfo
            'Try
            For Each key As String In Primers.Keys
                pmr = Primers(key)


                '分析发卡
                PAR.Hairpins.Add(key, New List(Of StructInfo))
                For l As Integer = 3 To pmr.Length
                    For i As Integer = 0 To pmr.Length - l
                        reg = New System.Text.RegularExpressions.Regex(ReverseComplement(pmr.Substring(i, l)))
                        For Each mt As System.Text.RegularExpressions.Match In reg.Matches(pmr)
                            If mt.Index + mt.Length < i - 3 Or i + l + 3 < mt.Index Then
                                Dim AG As Single = CalculateTm(mt.Captures(0).Value, Na, C).dG
                                info = New StructInfo
                                info.S1 = i
                                info.S2 = mt.Index
                                info.E1 = i + l
                                info.E2 = mt.Index + l
                                info.AG = AG
                                PAR.Hairpins(key).Add(info)
                            End If
                        Next
                    Next
                Next
                PAR.Hairpins(key).Sort()


                '二聚体
                PAR.Dimers.Add(key, New List(Of StructInfo))
                For l As Integer = 3 To pmr.Length
                    For i As Integer = 0 To pmr.Length - l
                        reg = New System.Text.RegularExpressions.Regex(ReverseComplement(pmr.Substring(i, l)))
                        For Each mt As System.Text.RegularExpressions.Match In reg.Matches(pmr)
                            Dim AG As Single = CalculateTm(mt.Captures(0).Value, Na, C).dG
                            info = New StructInfo
                            info.S1 = i
                            info.S2 = mt.Index
                            info.E1 = i + l
                            info.E2 = mt.Index + l
                            info.AG = AG
                            PAR.Dimers(key).Add(info)
                        Next
                    Next
                Next
                PAR.Dimers(key).Sort()
                '交叉二聚体
                For l As Integer = 3 To pmr.Length
                    For i As Integer = 0 To pmr.Length - l
                        reg = New System.Text.RegularExpressions.Regex(ReverseComplement(pmr.Substring(i, l)))
                        Dim started As Boolean = False
                        For Each rKey As String In Primers.Keys
                            If rKey <> key And Not started Then

                            ElseIf rKey = key Then
                                started = True
                            ElseIf started Then
                                Dim pmr2 As String = Primers(rKey)
                                For Each mt As System.Text.RegularExpressions.Match In reg.Matches(pmr2)
                                    Dim AG As Single = CalculateTm(mt.Captures(0).Value, Na, C).dG

                                    info = New StructInfo
                                    info.K1 = key
                                    info.K2 = rKey
                                    info.S1 = i
                                    info.S2 = mt.Index
                                    info.E1 = i + l
                                    info.E2 = mt.Index + l
                                    info.AG = AG
                                    PAR.CrossDimers.Add(info)

                                Next
                            End If
                        Next

                    Next
                Next

                PAR.Primings.Add(key, New List(Of PrimeInfo))
            Next

            'RaiseEvent ReturnResult(PAR)
            'Catch ex As Exception
            '    Throw ex
            'End Try
            'Try
            Dim tmpF As String
            Dim tmpR As String
            Dim pmrl As Integer

            For Each temp As GeneFile In temps
                If temp.Iscircular Then
                    tmpF = temp.Sequence + temp.Sequence
                    tmpR = temp.RCSequence + temp.RCSequence
                Else
                    tmpF = temp.Sequence
                    tmpR = temp.RCSequence
                End If
                For Each key As String In PAR.Primers.Keys

                    pmr = PAR.Primers(key)
                    pmrl = pmr.Length
                    If temp.Length < pmrl Then Continue For
                    For si As Integer = 0 To temp.Length - 1
                        PAR.Primings(key).AddRange(FindMatches(pmr, tmpF, si, pmrl, key, temp, False, Na, C))
                    Next
                    For si As Integer = 0 To temp.Length - 1
                        PAR.Primings(key).AddRange(FindMatches(pmr, tmpR, si, pmrl, key, temp, True, Na, C))
                    Next
                Next
            Next

            For Each key As String In PAR.Primers.Keys
                PAR.Primings(key).Sort()
            Next

            Dim col As New List(Of GeneFile)
            For Each temp As GeneFile In temps
                col.Add(temp)
            Next
            Dim plist As New List(Of String)
            For Each pmr In Primers.Values
                plist.Add(pmr)
            Next
            PAR.Products = PCR(col, plist)
            PAR.CrossDimers.Sort()
            RaiseEvent ReturnResult(PAR)
            Return PAR
            'Catch ex As Exception

            'End Try
        End Function

        Friend Shared Function AnalyzeSinglePrimer(ByVal vPrimer As String, ByVal Na As Single, ByVal C As Single,
                                                   ByRef vMaxHairpinF As String, ByRef vMaxHairpinR As String,
                                                   ByRef vMaxDimerF As String, ByRef vMaxDimerR As String) As Single
            Dim reg As System.Text.RegularExpressions.Regex
            Dim info As StructInfo
            Dim pmr As String

            Dim PAR As New PrimerAnalysisResult

            Dim key As String = "F"

            PAR.Primers.Add(key, vPrimer)


            '分析发卡
            info = New StructInfo
            Try

                pmr = vPrimer


                '分析发卡
                PAR.Hairpins.Add(key, New List(Of StructInfo))
                For l As Integer = 3 To pmr.Length
                    For i As Integer = 0 To pmr.Length - l
                        reg = New System.Text.RegularExpressions.Regex(ReverseComplement(pmr.Substring(i, l)))
                        For Each mt As System.Text.RegularExpressions.Match In reg.Matches(pmr)
                            If mt.Index + mt.Length < i - 3 Or i + l + 3 < mt.Index Then
                                Dim AG As Single = CalculateTm(mt.Captures(0).Value, Na, C).dG
                                info = New StructInfo
                                info.S1 = i
                                info.S2 = mt.Index
                                info.E1 = i + l
                                info.E2 = mt.Index + l
                                info.AG = AG
                                PAR.Hairpins(key).Add(info)
                            End If
                        Next
                    Next
                Next
                PAR.Hairpins(key).Sort()
                '二聚体
                PAR.Dimers.Add(key, New List(Of StructInfo))
                For l As Integer = 3 To pmr.Length
                    For i As Integer = 0 To pmr.Length - l
                        reg = New System.Text.RegularExpressions.Regex(ReverseComplement(pmr.Substring(i, l)))
                        For Each mt As System.Text.RegularExpressions.Match In reg.Matches(pmr)
                            Dim AG As Single = CalculateTm(mt.Captures(0).Value, Na, C).dG
                            info = New StructInfo
                            info.S1 = i
                            info.S2 = mt.Index
                            info.E1 = i + l
                            info.E2 = mt.Index + l
                            info.AG = AG
                            PAR.Dimers(key).Add(info)
                        Next
                    Next
                Next
                PAR.Dimers(key).Sort()
                Dim HP As Single = 0
                If PAR.Hairpins(key).Count > 0 Then
                    With PAR.Hairpins(key)(0)
                        HP = .AG
                        vMaxHairpinF = vPrimer.Substring(.S1, .E1 - .S1)
                        vMaxHairpinR = vPrimer.Substring(.S2, .E2 - .S2)
                    End With
                Else
                    HP = 10
                    vMaxHairpinF = ""
                    vMaxHairpinR = ""
                End If
                Dim DM As Single = 0
                If PAR.Dimers(key).Count > 0 Then
                    With PAR.Dimers(key)(0)
                        DM = .AG
                        vMaxDimerF = vPrimer.Substring(.S1, .E1 - .S1)
                        vMaxDimerR = vPrimer.Substring(.S2, .E2 - .S2)
                    End With
                Else
                    DM = 10
                    vMaxDimerF = ""
                    vMaxDimerR = ""
                End If
                Dim gc As String = Nuctions.GCFilter(vPrimer)
                Dim gcr = 1.0# - Math.Abs((gc.Length / pmr.Length) - 0.64#)
                Dim gc2 = gcr * gcr
                Return HP * 2 + DM + gc2 * gc2 * 10.0#

            Catch ex As Exception

            End Try
            Return 0.0F
        End Function
        Friend Shared Function AnalyzeCrossDimer(ByVal vFPrimer As String, ByVal vRPrimer As String, ByVal Na As Single, ByVal C As Single) As Single
            'Dim reg As System.Text.RegularExpressions.Regex
            'Dim info As StructInfo
            Dim pmr As String = Nuctions.ReverseComplement(vFPrimer)

            ''Dim PAR As New PrimerAnalysisResult
            ''PAR.Primers.Add("F", vFPrimer)
            ''PAR.Primers.Add("R", vRPrimer)
            '分析发卡
            'info = New StructInfo
            Dim CrossDimers As New List(Of StructInfo)
            Dim pmrl As Integer = vFPrimer.Length
            Dim offset As Integer = pmrl - 3
            Dim PX As String = "".PadRight(vFPrimer.Length - 3, "-")
            Dim PT As String = PX + vRPrimer + PX

            For i As Integer = 0 To vFPrimer.Length + vRPrimer.Length - 6
                CrossDimers.AddRange(FindMatches2(pmr, PT, i, pmrl, "", "", offset, Na, C))
            Next
            'Try
            '    For Each key As String In PAR.Primers.Keys
            '        pmr = PAR.Primers(key)
            '        '交叉二聚体
            '        For l As Integer = 3 To pmr.Length
            '            For i As Integer = 0 To pmr.Length - l
            '                reg = New System.Text.RegularExpressions.Regex(ReverseComplement(pmr.Substring(i, l)))
            '                Dim started As Boolean = False
            '                For Each rKey As String In PAR.Primers.Keys
            '                    If rKey <> key And Not started Then

            '                    ElseIf rKey = key Then
            '                        started = True
            '                    ElseIf started Then
            '                        Dim pmr2 As String = PAR.Primers(rKey)
            '                        For Each mt As System.Text.RegularExpressions.Match In reg.Matches(pmr2)
            '                            Dim AG As Single = CalculateTm(mt.Captures(0).Value, Na, C).dG

            '                            info = New StructInfo
            '                            info.K1 = key
            '                            info.K2 = rKey
            '                            info.S1 = i
            '                            info.S2 = mt.Index
            '                            info.E1 = i + l
            '                            info.E2 = mt.Index + l
            '                            info.AG = AG
            '                            PAR.CrossDimers.Add(info)
            '                        Next
            '                    End If
            '                Next

            '            Next
            '        Next
            '    Next
            CrossDimers.Sort()
            If CrossDimers.Count > 0 Then
                Return CrossDimers(0).AG
            Else
                Return 10
            End If
        End Function
    End Class

    Friend Shared Function AnalyzePrimer(ByVal Primers As Dictionary(Of String, String), ByVal temps As List(Of GeneFile), ByVal Na As Single, ByVal C As Single) As PrimerAnalysisResult
        Dim reg As System.Text.RegularExpressions.Regex
        Dim info As StructInfo
        Dim pmr As String

        Dim PAR As New PrimerAnalysisResult

        For Each key As String In Primers.Keys
            pmr = Primers(key)
            If pmr Is Nothing OrElse pmr.Length < 5 Then Continue For
            PAR.Primers.Add(key, pmr)
        Next

        If PAR.Primers.Count = 0 Then
            PAR.Products = New List(Of GeneFile)
            Return PAR
        End If

        '分析发卡
        info = New StructInfo
        For Each key As String In Primers.Keys
            pmr = Primers(key)


            '分析发卡
            PAR.Hairpins.Add(key, New List(Of StructInfo))
            For l As Integer = 3 To pmr.Length
                For i As Integer = 0 To pmr.Length - l
                    reg = New System.Text.RegularExpressions.Regex(ReverseComplement(pmr.Substring(i, l)))
                    For Each mt As System.Text.RegularExpressions.Match In reg.Matches(pmr)
                        If mt.Index + mt.Length < i - 3 Or i + l + 3 < mt.Index Then
                            Dim AG As Single = CalculateTm(mt.Captures(0).Value, Na, C).dG
                            info = New StructInfo
                            info.S1 = i
                            info.S2 = mt.Index
                            info.E1 = i + l
                            info.E2 = mt.Index + l
                            info.AG = AG
                            PAR.Hairpins(key).Add(info)
                        End If
                    Next
                Next
            Next
            PAR.Hairpins(key).Sort()
            '二聚体
            PAR.Dimers.Add(key, New List(Of StructInfo))
            For l As Integer = 3 To pmr.Length
                For i As Integer = 0 To pmr.Length - l
                    reg = New System.Text.RegularExpressions.Regex(ReverseComplement(pmr.Substring(i, l)))
                    For Each mt As System.Text.RegularExpressions.Match In reg.Matches(pmr)
                        Dim AG As Single = CalculateTm(mt.Captures(0).Value, Na, C).dG
                        info = New StructInfo
                        info.S1 = i
                        info.S2 = mt.Index
                        info.E1 = i + l
                        info.E2 = mt.Index + l
                        info.AG = AG
                        PAR.Dimers(key).Add(info)
                    Next
                Next
            Next
            PAR.Dimers(key).Sort()
            '交叉二聚体
            For l As Integer = 3 To pmr.Length
                For i As Integer = 0 To pmr.Length - l
                    reg = New System.Text.RegularExpressions.Regex(ReverseComplement(pmr.Substring(i, l)))
                    Dim started As Boolean = False
                    For Each rKey As String In Primers.Keys
                        If rKey <> key And Not started Then

                        ElseIf rKey = key Then
                            started = True
                        ElseIf started Then
                            Dim pmr2 As String = Primers(rKey)
                            For Each mt As System.Text.RegularExpressions.Match In reg.Matches(pmr2)
                                Dim AG As Single = CalculateTm(mt.Captures(0).Value, Na, C).dG

                                info = New StructInfo
                                info.K1 = key
                                info.K2 = rKey
                                info.S1 = i
                                info.S2 = mt.Index
                                info.E1 = i + l
                                info.E2 = mt.Index + l
                                info.AG = AG
                                PAR.CrossDimers.Add(info)

                            Next
                        End If
                    Next

                Next
            Next

            PAR.Primings.Add(key, New List(Of PrimeInfo))
        Next



        Dim tmpF As String
        Dim tmpR As String
        Dim pmrl As Integer

        For Each temp As GeneFile In temps
            For Each key As String In PAR.Primers.Keys
                pmr = PAR.Primers(key)
                pmrl = pmr.Length
                If temp.Iscircular Then
                    tmpF = temp.Sequence + temp.Sequence.Substring(0, pmrl)
                    tmpR = temp.RCSequence + temp.RCSequence.Substring(0, pmrl)
                Else
                    tmpF = temp.Sequence
                    tmpR = temp.RCSequence
                End If
                For si As Integer = 0 To tmpF.Length - pmrl
                    PAR.Primings(key).AddRange(FindMatches(pmr, tmpF, si, pmrl, key, temp, False, Na, C))
                Next
                For si As Integer = 0 To tmpR.Length - pmrl
                    PAR.Primings(key).AddRange(FindMatches(pmr, tmpR, si, pmrl, key, temp, True, Na, C))
                Next
            Next
        Next

        For Each key As String In PAR.Primers.Keys
            PAR.Primings(key).Sort()
        Next

        Dim col As New List(Of GeneFile)
        For Each temp As GeneFile In temps
            col.Add(temp)
        Next
        Dim plist As New List(Of String)
        For Each pmr In Primers.Values
            plist.Add(pmr)
        Next
        PAR.Products = PCR(col, plist)
        PAR.CrossDimers.Sort()
        Return PAR
    End Function

    Friend Shared Function FindMatches(ByVal Pmr As String, ByVal Temp As String, ByVal start As Integer, ByVal length As Integer, ByVal key As String, ByVal source As GeneFile, ByVal IsRC As Boolean, ByVal Na As Single, ByVal C As Single) As List(Of PrimeInfo)

        Dim mList As New List(Of PrimeInfo)
        Dim acc As Integer
        Dim pi As PrimeInfo

        acc = 0
        Dim i As Integer

        For i = 0 To length - 1
            If i + start >= Temp.Length Then Exit For
            If Pmr.Chars(i) = Temp.Chars(i + start) Then
                acc += 1
            ElseIf acc > 3 Then
                pi = New PrimeInfo
                pi.K1 = key
                pi.K2 = source
                pi.IsRC = IsRC
                pi.S1 = i - acc
                pi.E1 = i
                pi.S2 = start + i - acc
                pi.E2 = start + i
                pi.AG = CalculateTm(Pmr.Substring(i - acc, acc), Na, C).dG
                mList.Add(pi)
                acc = 0
            Else
                acc = 0
            End If
        Next
        If acc > 3 Then
            pi = New PrimeInfo
            pi.K1 = key
            pi.K2 = source
            pi.IsRC = IsRC
            pi.S1 = i - acc
            pi.E1 = i
            pi.S2 = start + i - acc
            pi.E2 = start + i
            pi.AG = CalculateTm(Pmr.Substring(i - acc, acc), Na, C).dG
            mList.Add(pi)
        End If
        Return mList
    End Function
    Friend Shared Function FindMatches2(ByVal Pmr As String, ByVal Temp As String, ByVal start As Integer, ByVal Primerlength As Integer, ByVal key As String, ByVal source As String, ByVal offset As Integer, ByVal Na As Single, ByVal C As Single) As List(Of StructInfo)

        'Dim Offsets As New List(Of Integer)
        Dim i As Integer
        Dim pi As StructInfo

        'Dim subpmr As String
        'Dim rgx As System.Text.RegularExpressions.Regex


        Dim mList As New List(Of StructInfo)
        Dim acc As Integer

        acc = 0


        For i = 0 To Primerlength - 1
            If i + start >= Temp.Length Then Exit For
            If Pmr.Chars(i) = Temp.Chars(i + start) Then
                acc += 1
            ElseIf acc > 2 Then
                pi = New StructInfo
                pi.K1 = key
                pi.K2 = source
                pi.S1 = i - acc
                pi.E1 = i
                pi.S2 = start + i - acc - offset
                pi.E2 = start + i - offset
                pi.AG = CalculateTm(Pmr.Substring(i - acc, acc), Na, C).dG
                mList.Add(pi)
                acc = 0
            Else
                acc = 0
            End If
        Next
        If acc > 2 Then
            pi = New StructInfo
            pi.K1 = key
            pi.K2 = source
            pi.S1 = i - acc
            pi.E1 = i
            pi.S2 = start + i - acc - offset
            pi.E2 = start + i - offset
            pi.AG = CalculateTm(Pmr.Substring(i - acc, acc), Na, C).dG
            mList.Add(pi)
        End If
        Return mList
    End Function

    Private Shared Sub ExtendMatch(ByVal Pmr As String, ByRef pStart As Integer, ByRef pEnd As Integer, ByVal temp As String, ByRef tStart As Integer, ByRef tEnd As Integer)
        While pStart > 0 And tStart > 0
            If Pmr.Chars(pStart - 1) = temp.Chars(tStart - 1) Then
                pStart -= 1
                tStart -= 1
            Else
                Exit While
            End If
        End While
        Dim pl As Integer = Pmr.Length - 1
        Dim tl As Integer = temp.Length - 1
        While pEnd < pl And tEnd < tl
            If Pmr.Chars(pEnd + 1) = temp.Chars(tEnd + 1) Then
                pEnd += 1
                tEnd += 1
            Else
                Exit While
            End If
        End While
    End Sub

    Friend Shared Function BLASTHomologousRegion(ByVal gList As List(Of GeneFile), ByVal boxlength As Integer) As List(Of String)
        Dim i As Integer, j As Integer
        Dim vList As New List(Of Nuctions.GeneFile)
        Dim stl As New List(Of String)
        For i = 0 To gList.Count - 1
            vList.Clear()
            For j = i + 1 To gList.Count - 1
                vList.Add(gList(j))
            Next
            stl.AddRange(gList(i).BLAST2(vList, boxlength))
        Next
        Return stl
    End Function

    Friend Shared Function SearchMaximumHomologousRegion(ByVal gList As List(Of GeneFile), ByVal boxlength As Integer) As List(Of String)
        Dim wtList As New List(Of GeneFile)
        wtList.AddRange(gList)
        Dim mc As System.Text.RegularExpressions.MatchCollection
        Dim boxF As String
        Dim boxR As String
        Dim cF As Integer
        Dim cR As Integer
        Dim sList As New List(Of String)
        Dim pattern As String

        For Each g As GeneFile In gList
            If g.Sequence.Length < boxlength Then Continue For

            'get a box of 300bp length to search the sequence

            Dim be As Integer = g.BoxEnd(boxlength)
            For i As Integer = 0 To be
                boxF = g.SubSequence(i, boxlength + i - 1)
                boxR = g.SubRCSequence(i, boxlength + i - 1)

                For Each gv As GeneFile In wtList
                    mc = gv.Matches(boxF)
                    For Each m As System.Text.RegularExpressions.Match In mc
                        If gv Is g And i = m.Index Then Continue For
                        cF = GeneFile.ContinueMatch(g, i + boxlength, True, gv, m.Index + boxlength, True)
                        pattern = g.SubSequence(i, boxlength + cF + i - 1)
                        i += cF
                        If Not sList.Contains(pattern) Then sList.Add(pattern)
                    Next
                    mc = gv.Matches(boxR)
                    For Each m As System.Text.RegularExpressions.Match In mc
                        If gv Is g And i = m.Index Then Continue For
                        cR = GeneFile.ContinueMatch(g, i + boxlength, False, gv, m.Index + boxlength, True)
                        pattern = g.SubRCSequence(i, boxlength + cR + i - 1)
                        i += cR
                        If Not sList.Contains(pattern) Then sList.Add(pattern)
                    Next
                Next
            Next
            wtList.Remove(g)
        Next
        Return sList
    End Function

    Friend Shared Function MergeSequence(ByVal gList As List(Of GeneFile), ByVal OnlySignificant As Boolean, ByVal OnlyExtend As Boolean) As List(Of GeneFile)
        '至少12个碱基完全相同
        Dim boxlength As Integer = 12
        'find a homologous arm first
        Dim xList As List(Of String)
        xList = BLASTHomologousRegion(gList, boxlength)


        Dim sList As New List(Of String)
        If OnlySignificant Then
            Dim xLength As Integer = 0
            For Each s As String In xList
                xLength = Math.Max(s.Length, xLength)
            Next
            For Each s As String In xList
                If s.Length > 0.6 * xLength Then sList.Add(s)
            Next
        Else
            sList.AddRange(xList)
        End If

        'Next

        Dim LDict As New Dictionary(Of String, RestrictionEnzyme)

        Dim rec As String
        Dim cut As String
        Dim idx As Integer
        Dim ptn As String
        Dim kList As New List(Of String)

        For i As Integer = 0 To sList.Count - 1
            ptn = sList(i)
            rec = ptn
            cut = rec
            idx = 0
            LDict.Add("Homo" + i.ToString, New Nuctions.RestrictionEnzyme("Homo" + i.ToString, rec, idx + cut.Length, idx, "&"))
            kList.Add("Homo" + i.ToString)
        Next

        '确保只发生单交换 任何情况下，只有一种方法会发挥作用

        Dim zList As New List(Of String)

        '被单交换切割的列表
        Dim c0List As New List(Of GeneFile)
        Dim rsList As New List(Of GeneFile)
        Dim slrList As List(Of GeneFile)


        '不添加原始的质粒
        'rsList.AddRange(gList)
        For Each ptn In kList
            '每次清空列表 然后仅加入一种序列 确保单交换
            zList.Clear()
            zList.Add(ptn)
            c0List.Clear()
            For Each gf As GeneFile In gList
                Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(zList, gf, LDict)
                c0List.AddRange(re.CutDNA())
            Next
            slrList = SingleLigateRecombination(c0List)
            For Each gf As GeneFile In slrList
                gf.Name = "Merge(" + LDict(ptn).Sequence.Length.ToString + ")-" + gf.Length.ToString
            Next
            rsList.AddRange(slrList)
        Next

        Dim reList As New List(Of GeneFile)

        If OnlyExtend Then
            Dim gLength As Integer = 0
            For Each gf As GeneFile In gList
                gLength = IIf(gf.Length > gLength, gf.Length, gLength)
            Next
            For Each gf As GeneFile In rsList
                If gf.Length > gLength Then
                    reList.Add(gf)
                End If
            Next
        Else
            reList.AddRange(rsList)
        End If

        Dim sVList As New List(Of GeneFile)
        Dim contains As Boolean
        For Each gf As GeneFile In reList
            '不可能有小于1000bp的环形质粒
            If gf.Length <= 1024 And gf.Iscircular Then Continue For
            contains = False
            For Each gfx As GeneFile In gList
                contains = contains Or (gfx = gf)
                If contains Then Exit For
            Next
            If Not contains Then
                For Each gfx As GeneFile In sVList
                    contains = contains Or (gfx = gf)
                    If contains Then Exit For
                Next
            End If
            If Not contains Then sVList.Add(gf)
        Next
        Return sVList
    End Function

    Friend Shared Sub SequenceCompare(ByVal Source As GeneFile, ByVal gList As List(Of GeneFile), ByVal pList As List(Of String), Optional ByVal MiniBoxLength As Integer = 10, Optional ByVal MaxBoxLength As Integer = 50)
        'this method will add special features to the source genefile.

        'these speciall features will be shown in the viewer.

        'pList is the list of primers. Primers define the beginning of the sequencing.
        Dim fs As String
        Dim rs As String
        If Source.Iscircular Then
            fs = Source.Sequence + Source.Sequence
            rs = Source.RCSequence + Source.RCSequence
        Else

        End If

    End Sub
    Public Class MatchDictionary
        Inherits Dictionary(Of Integer, MatchInfo)
        Public Function Contains(ByVal vOffset As Integer, ByVal l As Integer, ByVal r As Integer) As Boolean
            Return ContainsKey(vOffset) AndAlso Me(vOffset).Intersect(l, r)
        End Function
        Public Shadows Function Add(ByVal vOffset As Integer, ByVal vCompareStart As Integer, ByVal vCompareEnd As Integer, ByVal vMatchStart As Integer, ByVal vMatchEnd As Integer) As Boolean
            'add a match and put it in the correct position.
            If Contains(vOffset, vCompareStart, vCompareEnd) Then
                Return False
            Else
                If Not ContainsKey(vOffset) Then MyBase.Add(vOffset, New MatchInfo)
                Me(vOffset).Add(vCompareStart, vCompareEnd, vMatchStart, vMatchEnd)
            End If
        End Function
        Public Sub MergeMatch(ByVal vBoxLength As Integer)
            Dim CanMerge As Boolean = True
            Dim i As Integer
            Dim Merged As MatchItem
            For Each mi As MatchInfo In Values
                mi.Sort()
                i = 0
                If mi.Count = 1 Then
                    If mi(0).MatchEnd - mi(0).MatchStart + 1 >= vBoxLength Then mi(0).MainMatch = True
                End If
                While i < mi.Count - 1
                    Merged = mi(i) & mi(i + 1)
                    If Not (Merged Is Nothing) Then
                        mi(i) = Merged
                        mi.RemoveAt(i + 1)
                        If Merged.MatchEnd - Merged.MatchStart + 1 >= vBoxLength Then Merged.MainMatch = True
                    Else
                        If mi(i).MatchEnd - mi(i).MatchStart + 1 >= vBoxLength Then mi(i).MainMatch = True
                        If mi(i + 1).MatchEnd - mi(i + 1).MatchStart + 1 >= vBoxLength Then mi(i + 1).MainMatch = True
                        i += 1
                    End If
                End While
            Next
        End Sub
        Public Sub ClearUnattachedItems()
            Dim i As Integer
            For Each mi As MatchInfo In Values
                i = 0
                For Each mt As MatchItem In mi
                    If Not mt.MainMatch AndAlso mt.LastMain Is Nothing AndAlso mt.NextMain Is Nothing Then mt.Valid = False
                Next
            Next
        End Sub
        Public ReadOnly Property MatchCount As Integer
            Get
                Dim i As Integer = 0
                For Each mi As MatchInfo In Values
                    i += mi.Count
                Next
                Return i
            End Get
        End Property
        Public ReadOnly Property ByMatchStart(ByVal vStart As Integer) As List(Of MatchItem)
            Get
                Dim res As New List(Of MatchItem)
                For Each mi As MatchInfo In Values
                    For Each mt As MatchItem In mi
                        If mt.MatchStart = vStart Then res.Add(mt)
                    Next
                Next
                Return res
            End Get
        End Property
    End Class

    Public Class MatchInfo
        Inherits List(Of MatchItem)
        'Public Matches As New List(Of MatchItem)
        Public Sub New()

        End Sub
        Public Shadows Sub Add(ByVal vCompareStart As Integer, ByVal vCompareEnd As Integer, ByVal vMatchStart As Integer, ByVal vMatchEnd As Integer)
            MyBase.Add(New MatchItem(vCompareStart, vCompareEnd, vMatchStart, vMatchEnd))
        End Sub
        Public Function Intersect(ByVal l As Integer, ByVal r As Integer) As Boolean
            For Each mi As MatchItem In Me
                If Not (l > mi.CompareEnd Or r < mi.CompareStart) Then Return True
            Next
            Return False
        End Function
        Public Shared Operator And(ByVal X As Integer, ByVal Y As MatchInfo) As Boolean

            For Each mi As MatchItem In Y
                If X <= mi.CompareEnd And X <= mi.CompareEnd Then Return True
            Next
            Return False
        End Operator
        Public Shared Operator And(ByVal Y As MatchInfo, ByVal X As Integer) As Boolean
            For Each mi As MatchItem In Y
                If X >= mi.CompareStart And X <= mi.CompareEnd Then Return True
            Next
            Return False
        End Operator
        Public Shadows Sub Sort()
            MyBase.Sort(New MatchItemSorter)
        End Sub
    End Class
    Public Class MatchItemSorter
        Implements IComparer(Of MatchItem)
        Public Function Compare(ByVal x As MatchItem, ByVal y As MatchItem) As Integer Implements System.Collections.Generic.IComparer(Of MatchItem).Compare
            Return Math.Sign(y.CompareStart - x.CompareStart)
        End Function
    End Class
    Public Class MatchItem
        Public CompareStart, CompareEnd, MatchStart, MatchEnd As Integer
        Public MainMatch As Boolean = False
        Public LastMain As MatchItem
        Public NextMain As MatchItem
        Public Valid As Boolean = True
        Public Source As GeneFile
        Public Sub New()

        End Sub
        Public Sub New(ByVal vCS As Integer, ByVal vCE As Integer, ByVal vMS As Integer, ByVal vME As Integer)
            CompareStart = vCS
            CompareEnd = vCE
            MatchStart = vMS
            MatchEnd = vME
        End Sub
        Public Shared Operator And(ByVal MI1 As MatchItem, ByVal MI2 As MatchItem) As Boolean
            Return (MI1.MatchStart - MI1.CompareStart = MI2.MatchStart - MI2.CompareStart) AndAlso (Not (MI2.CompareStart > MI1.CompareEnd Or MI2.CompareEnd < MI1.CompareStart))
        End Operator
        Public Shared Operator &(ByVal MI1 As MatchItem, ByVal MI2 As MatchItem) As MatchItem
            If (MI1.MatchStart - MI1.CompareStart <> MI2.MatchStart - MI2.CompareStart) OrElse (MI2.CompareStart > MI1.CompareEnd - 1 Or MI2.CompareEnd - 1 < MI1.CompareStart) Then
                Return Nothing
            Else
                Return New MatchItem(Math.Min(MI1.CompareStart, MI2.CompareStart), Math.Max(MI1.CompareEnd, MI2.CompareEnd), Math.Min(MI1.MatchStart, MI2.MatchStart), Math.Max(MI1.MatchEnd, MI2.MatchEnd))
            End If
        End Operator
        Public Last As MatchItem = Nothing
        Public [Next] As MatchItem = Nothing
        Public LastDist As Integer = Integer.MaxValue
        Public NextDist As Integer = Integer.MaxValue
        Public RemLastDist As Integer
        Public RemNextDist As Integer
    End Class

    Private Shared FirstJunction As Single = 1.6
    Private Shared SecondJunction As Single = 2.0
    Friend Shared Sub FindJunctionItem(ByVal mh As MatchItem, ByVal mk As MatchItem, ByVal boxLength As Integer)
        Static nd As Integer
        If mk.CompareStart < mh.CompareEnd And mk.CompareStart > mh.CompareStart Then '只考虑mk.CompareStart小于mh.CompareEnd的情况 否则会导致同样的比较计算运行2次
            If mk.CompareEnd > mh.CompareEnd Then
                '错开的情况
                '-----
                '  -----
                '这种情况仅允许两个MainMatch发生，一旦确认 将清除附近所有其他的非主流匹配
                If mk.MainMatch And mh.MainMatch Then
                    If mk.MatchEnd - mk.MatchStart > boxLength * 2 And mh.MatchEnd - mh.MatchStart > boxLength * 2 Then
                        nd = -boxLength + Math.Abs(mh.CompareEnd - mk.CompareStart)
                        If mh.NextDist > nd Then
                            '将mk设置为mh的最近匹配
                            If Not (mh.Next Is Nothing) Then
                                mh.Next.Last = Nothing
                                mh.Next.LastDist = Integer.MaxValue
                                mh.Next.LastMain = Nothing
                            End If
                            mh.NextDist = nd
                            mh.Next = mk
                            mh.NextMain = mk
                            If Not (mk.Last Is Nothing) Then
                                mk.Last.Next = Nothing
                                mk.Last.NextDist = Integer.MaxValue
                                mk.Last.NextMain = Nothing
                            End If
                            mk.LastDist = nd
                            mk.Last = mh
                            mk.LastMain = mh
                        Else
                            '已经存在一个匹配比mk更好 什么都不做
                        End If
                    Else
                        '这个匹配的效果并不理想 什么都不做
                    End If
                Else
                    '不都是MainMatch 什么都不做
                End If
            Else
                '包含子 不去管它
            End If
        ElseIf mk.CompareStart >= mh.CompareEnd And mk.CompareStart <= mh.CompareEnd + boxLength Then
            'mk.CompareStart >= mh.CompareEnd
            '断开的情况
            '----
            '      -----
            '这种情况需要统计分数来延伸
            If mh.MainMatch Then
                'mh是主要匹配的情况
                If mk.MainMatch Then
                    nd = -boxLength + mk.CompareStart - mh.CompareEnd
                    If mh.NextDist > nd And mk.LastDist > nd Then
                        If Not (mh.Next Is Nothing) Then
                            mh.Next.Last = Nothing
                            mh.Next.LastDist = Integer.MaxValue
                            mh.Next.LastMain = Nothing
                        End If
                        mh.NextDist = nd
                        mh.Next = mk
                        mh.NextMain = mk
                        If Not (mk.Last Is Nothing) Then
                            mk.Last.Next = Nothing
                            mk.Last.NextDist = Integer.MaxValue
                            mk.Last.NextMain = Nothing
                        End If
                        mk.LastDist = nd
                        mk.Last = mh
                        mk.LastMain = mh
                    End If
                Else
                    nd = boxLength + (mk.CompareStart - mh.CompareEnd) * 2 + (mk.CompareStart - mk.CompareEnd)
                    If nd < boxLength * 1.6 Then
                        If mh.NextDist > nd And mk.LastDist > nd Then
                            mh.NextDist = nd
                            mh.Next = mk
                            If Not (mk.Last Is Nothing) Then
                                mk.Last.Next = Nothing
                                mk.Last.NextDist = Integer.MaxValue
                                mk.Last.NextMain = Nothing
                            End If
                            mk.LastMain = mh
                            mk.LastDist = nd
                            mk.Last = mh
                            '找到非主要连接时有分析后续数据
                            Dim IT As MatchItem = GetNextChain(mh, mk, boxLength, nd)
                            While Not (IT Is Nothing)
                                IT = GetNextChain(mh, mk, boxLength, nd)
                            End While
                        End If
                    End If
                End If
            Else
                'mh不是主要匹配
                nd = boxLength + (mk.CompareStart - mh.CompareEnd) * 2 + (mk.CompareStart - mk.CompareEnd)
                If nd < boxLength * FirstJunction Then '一级连接不能超过1.6 超过这个距离之后 就无效了 这是BLAST算法的基本道理
                    If mk.MainMatch Then
                        If mk.LastDist > nd And mh.NextDist > nd Then
                            If Not (mk.Last Is Nothing) Then
                                mk.Last.Next = Nothing
                                mk.Last.NextDist = Integer.MaxValue
                                mk.Last.NextMain = Nothing
                            End If
                            mk.LastDist = nd
                            mk.Last = mh
                            mh.NextDist = nd
                            mh.Next = mk
                            mh.NextMain = mk
                            '从前方找到主要连接之后 要向后方搜索
                            Dim IT As MatchItem = GetLastChain(mk, mh, boxLength, nd)
                            While Not (IT Is Nothing)
                                IT = GetLastChain(mk, mh, boxLength, nd)
                            End While
                        End If
                    Else
                        If mk.NextMain Is Nothing AndAlso (Not (mh.LastMain Is Nothing)) Then
                            '仅从mh上游有链接
                            If mk.NextDist < boxLength * SecondJunction - nd Then '二级连接不能超过2 超过这个距离之后 就无效了 这是BLAST算法的基本道理
                                If mk.LastDist > nd And mh.NextDist > nd Then
                                    mk.LastDist = nd
                                    mk.Last = mh
                                    If Not (mh.Next Is Nothing) Then
                                        mh.Next.Last = Nothing
                                        mh.Next.LastDist = Integer.MaxValue
                                        mh.Next.LastMain = Nothing
                                    End If
                                    mh.NextDist = nd
                                    mh.Next = mk
                                End If
                            End If
                        ElseIf mh.LastMain Is Nothing AndAlso (Not (mk.NextMain Is Nothing)) Then
                            '仅从mk下游有链接
                            If mk.NextDist < boxLength * SecondJunction - nd Then '二级连接不能超过2 超过这个距离之后 就无效了 这是BLAST算法的基本道理
                                If mk.LastDist > nd And mh.NextDist > nd Then
                                    If Not (mk.Last Is Nothing) Then
                                        mk.Last.Next = Nothing
                                        mk.Last.NextDist = Integer.MaxValue
                                        mk.Last.NextMain = Nothing
                                    End If
                                    mk.LastDist = nd
                                    mk.Last = mh
                                    mh.NextDist = nd
                                    mh.Next = mk
                                End If
                            End If
                        ElseIf (Not (mh.LastMain Is Nothing)) AndAlso (Not (mk.NextMain Is Nothing)) Then
                            '双侧连接
                            If mk.NextDist < boxLength * SecondJunction - nd Or mh.LastDist < boxLength * SecondJunction - nd Then '二级连接不能超过2 超过这个距离之后 就无效了 这是BLAST算法的基本道理
                                If mk.LastDist > nd And mh.NextDist > nd Then
                                    If Not (mk.Last Is Nothing) Then
                                        mk.Last.Next = Nothing
                                        mk.Last.NextDist = Integer.MaxValue
                                        mk.Last.NextMain = Nothing
                                    End If
                                    mk.LastDist = nd
                                    mk.Last = mh
                                    mk.LastMain = mh.LastMain
                                    If Not (mh.Next Is Nothing) Then
                                        mh.Next.Last = Nothing
                                        mh.Next.LastDist = Integer.MaxValue
                                        mh.Next.LastMain = Nothing
                                    End If
                                    mh.NextDist = nd
                                    mh.Next = mk
                                    mh.NextMain = mk.NextMain
                                End If
                            End If
                        Else
                            '无有效连接 仅仅记录数据 以备后面找到主要连接时再分析
                            If mk.LastDist > nd And mh.NextDist > nd Then
                                mk.LastDist = nd
                                mk.Last = mh
                                mh.NextDist = nd
                                mh.Next = mk
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Friend Shared Function GetGapChar(ByVal CurrentCompareEnd As Integer, ByVal NextCompareStart As Integer, ByVal CurrentMatchEnd As Integer, ByVal NextMatchStart As Integer, ByVal gf As GeneFile,
                                      ByRef vStart As Integer, ByRef vEnd As Integer) As String
        Dim GapSequence As String
        Dim GapLength As Integer 'length 
        Dim SpaceLength As Integer

        If CurrentCompareEnd <= NextCompareStart Then
            '第一个匹配在第二个匹配之前
            If CurrentMatchEnd <= NextMatchStart Then
                '末端无重叠
                GapSequence = gf.SubSequence(CurrentMatchEnd - 1, NextMatchStart - 1)
                GapLength = Math.Max(GapSequence.Length, NextCompareStart - CurrentCompareEnd + 1)
                vStart = CurrentCompareEnd + (NextCompareStart - CurrentCompareEnd) \ 2 - GapLength \ 2 - (GapLength Mod 2) + 1
                vEnd = CurrentCompareEnd + (NextCompareStart - CurrentCompareEnd) \ 2 + GapLength \ 2 + 1
                SpaceLength = GapLength - GapSequence.Length
                GapSequence = "".PadRight(SpaceLength \ 2, " ") + GapSequence + "".PadRight(SpaceLength \ 2 + SpaceLength Mod 2, " ")
            Else 'If ar > bl Then
                GapSequence = "".PadRight(CurrentMatchEnd - NextMatchStart, ">") + "".PadRight(NextCompareStart - CurrentCompareEnd, " ") + "".PadRight(CurrentMatchEnd - NextMatchStart, "<")
                GapLength = NextCompareStart - CurrentCompareEnd + (CurrentMatchEnd - NextMatchStart) * 2
                vStart = CurrentCompareEnd - (CurrentMatchEnd - NextMatchStart) + 1
                vEnd = NextCompareStart + (CurrentMatchEnd - NextMatchStart) + 1
            End If

        Else 'If ae > bs Then
            '第一个匹配在第二个匹配之后
            If CurrentMatchEnd <= NextMatchStart Then
                GapSequence = gf.SubSequence(CurrentMatchEnd + 1, NextMatchStart)
                GapLength = Math.Max(GapSequence.Length, CurrentCompareEnd - NextCompareStart)
                If GapLength > GapSequence.Length Then
                    vStart = NextCompareStart
                    vEnd = CurrentCompareEnd
                Else
                    vStart = NextCompareStart + (CurrentCompareEnd - NextCompareStart) \ 2 - GapLength \ 2 - (GapLength Mod 2)
                    vEnd = NextCompareStart + (CurrentCompareEnd - NextCompareStart) \ 2 + GapLength \ 2
                End If
                SpaceLength = GapLength - GapSequence.Length
                GapSequence = "".PadRight(SpaceLength \ 2, " ") + GapSequence + "".PadRight(SpaceLength \ 2 + SpaceLength Mod 2, " ")
            Else 'If ar > bl Then
                If CurrentMatchEnd - NextMatchStart >= CurrentCompareEnd - NextCompareStart Then
                    SpaceLength = CurrentCompareEnd - NextCompareStart
                    GapSequence = "".PadRight(CurrentMatchEnd - NextMatchStart - SpaceLength, ">") + "".PadRight(SpaceLength, "=") + "".PadRight(CurrentMatchEnd - NextMatchStart - SpaceLength, "<")
                    vStart = CurrentCompareEnd - (CurrentMatchEnd - NextMatchStart)
                    vEnd = NextCompareStart + (CurrentMatchEnd - NextMatchStart)
                ElseIf (CurrentMatchEnd - NextMatchStart) * 2 >= (CurrentCompareEnd - NextCompareStart) Then
                    SpaceLength = (CurrentMatchEnd - NextMatchStart) * 2 - (CurrentCompareEnd - NextCompareStart)
                    GapSequence = "".PadRight(CurrentMatchEnd - NextMatchStart - SpaceLength, "<") + "".PadRight(SpaceLength, "=") + "".PadRight(CurrentMatchEnd - NextMatchStart - SpaceLength, ">")
                    vStart = NextCompareStart
                    vEnd = CurrentCompareEnd
                Else
                    SpaceLength = (CurrentCompareEnd - NextCompareStart) - (CurrentMatchEnd - NextMatchStart) * 2
                    GapSequence = "".PadRight(CurrentCompareEnd - NextCompareStart - SpaceLength, "<") + "".PadRight(SpaceLength, " ") + "".PadRight(CurrentCompareEnd - NextCompareStart - SpaceLength, ">")
                    vStart = NextCompareStart
                    vEnd = CurrentCompareEnd
                End If
            End If
        End If
        vStart += 1
        Return GapSequence
    End Function
    Friend Shared Function GetNextChain(ByVal vMain As MatchItem, ByVal vNext As MatchItem, ByVal boxLength As Integer, ByRef currentScore As Integer) As MatchItem
        If vNext.NextMain Is Nothing AndAlso (Not (vNext.Next Is Nothing)) Then
            If vNext.NextDist < boxLength * SecondJunction - currentScore Then
                currentScore += vNext.NextDist
                vNext.Next.LastMain = vMain
                Return vNext.Next
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function
    Friend Shared Function GetLastChain(ByVal vMain As MatchItem, ByVal vLast As MatchItem, ByVal boxLength As Integer, ByRef currentScore As Integer) As MatchItem
        If vLast.LastMain Is Nothing AndAlso (Not (vLast.Last Is Nothing)) Then
            If vLast.NextDist < boxLength * SecondJunction - currentScore Then
                currentScore += vLast.LastDist
                vLast.Last.NextMain = vMain
                Return vLast.Last
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function
    'Friend Shared Function ExpandMatch(ByVal Source As String, ByVal Start As Integer, ByVal [End] As Integer, vGeneFile As GeneFile,  ) As Integer

    'End Function
    Friend Shared Function Recombination(gList As List(Of GeneFile), method As String, IsExhuastive As Boolean, Times As Integer) As List(Of GeneFile)
        Dim vList As New List(Of GeneFile)
        Select Case method
            Case "Homologous Recombination"
                '至少300个碱基
                Dim boxlength As Integer = 300
                'find a homologous arm first
                Dim sList As List(Of String)
                sList = SearchMaximumHomologousRegion(gList, boxlength)

                Dim LDict As New Dictionary(Of String, RestrictionEnzyme)

                Dim rec As String
                Dim cut As String
                Dim idx As Integer
                Dim ptn As String
                Dim kList As New List(Of String)

                For i As Integer = 0 To sList.Count - 1
                    ptn = sList(i)
                    rec = ptn
                    cut = rec
                    idx = 0
                    LDict.Add("Homo" + i.ToString, New Nuctions.RestrictionEnzyme("Homo" + i.ToString, rec, idx + cut.Length, idx, "&"))
                    kList.Add("Homo" + i.ToString)
                Next

                '确保只发生单交换 任何情况下，只有一种方法会发挥作用

                Dim zList As New List(Of String)

                '被单交换切割的列表
                Dim c0List As New List(Of GeneFile)
                Dim rsList As New List(Of GeneFile)
                rsList.AddRange(gList)

                For Each ptn In kList
                    '每次清空列表 然后仅加入一种序列 确保单交换
                    zList.Clear()
                    zList.Add(ptn)
                    c0List.Clear()
                    For Each gf As GeneFile In gList
                        Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(zList, gf, LDict)
                        c0List.AddRange(re.CutDNA())

                    Next
                    rsList.AddRange(LigateRecombination(c0List, IsExhuastive, Times))
                Next

                Dim sVList As New List(Of GeneFile)
                Dim contains As Boolean
                For Each gf As GeneFile In rsList
                    contains = False
                    For Each gfx As GeneFile In sVList
                        contains = contains Or (gfx = gf)
                    Next
                    If Not contains Then sVList.Add(gf)
                Next

                Return sVList
            Case "Lambda Red Recombination"
                Dim LDict As New Dictionary(Of String, RestrictionEnzyme)

                Dim rvList As New List(Of GeneFile)
                Dim intermediateList As New List(Of GeneFile)
                Dim resultList As New List(Of GeneFile)
                Dim cutsiteList As New List(Of String)

                'Dim i As Integer = 0
                For Each gf As GeneFile In gList
                    'clear the sites
                    cutsiteList.Clear()
                    LDict.Clear()
                    If gf.Length > 70 And gf.Length < 10000 And (rgxFreeEnd.IsMatch(gf.End_F) And rgxFreeEnd.IsMatch(gf.End_R)) Then
                        Dim rec As String
                        Dim cut As String
                        Dim idx As Integer
                        rec = gf.Sequence.Substring(0, 35)
                        If Not LDict.ContainsKey(rec) Then
                            cut = rec
                            idx = 0
                            LDict.Add(rec, New Nuctions.RestrictionEnzyme(rec, rec, idx + cut.Length, idx, "&"))
                            cutsiteList.Add(rec)
                        End If
                        rec = gf.RCSequence.Substring(0, 35)
                        If Not LDict.ContainsKey(rec) Then
                            cut = rec
                            idx = 0
                            LDict.Add(rec, New Nuctions.RestrictionEnzyme(rec, rec, idx + cut.Length, idx, "&"))
                            cutsiteList.Add(rec)
                        End If
                    End If
                    rvList.Add(gf)
                    If cutsiteList.Count > 0 Then
                        For Each substrate In gList
                            Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(cutsiteList, substrate, LDict)
                            intermediateList.AddRange(re.CutDNA)
                        Next
                        resultList.AddRange(LigateRecombination(intermediateList, IsExhuastive, Times))
                    End If
                    ReduceDNA(resultList)
                Next
                Return resultList
            Case "in vitro Annealing"
                Return MultipleLinearEndAnneal(gList, IsExhuastive, Times)
            Case "in vivo Annealing"
                Return DegenerateMultipleLinearEndAnneal(gList, IsExhuastive, Times)
            Case Else
                If method Is Nothing OrElse method.Length = 0 Then
                    For Each gf In gList
                        vList.Add(gf.CloneWithoutFeatures)
                    Next
                Else
                    Dim c0List As New List(Of GeneFile)
                    Dim ReList As New List(Of EnzymeAnalysis.EnzymeAnalysisResult)
                    For Each gf As GeneFile In gList
                        Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(SettingEntry.RecombinationSiteGroups(method), gf, SettingEntry.RecombinationSiteDict)
                        ReList.Add(re)
                    Next
                    ClearNoRecombinationCuts(ReList)
                    For Each re In ReList
                        c0List.AddRange(re.CutDNA)
                    Next
                    Return LigateRecombination(c0List, IsExhuastive, Times)
                End If
        End Select
        Return vList
    End Function

    Friend Shared Sub ClearNoRecombinationCuts(ReList As List(Of EnzymeAnalysis.EnzymeAnalysisResult))
        Dim overhangs As New HashSet(Of String)
        For Each re In ReList
            For Each cut In re.CutList
                overhangs.Add(cut.AOverhang)
                overhangs.Add(cut.SOverhang)
            Next
        Next
        Dim IsRecombining As New HashSet(Of String)
        For i As Integer = 0 To overhangs.Count - 1
            Dim this As String = overhangs(i)
            For j As Integer = i + 1 To overhangs.Count - 1
                Dim that As String = overhangs(j)
                If CanRecombine(this, that) Then
                    IsRecombining.Add(this)
                    IsRecombining.Add(that)
                End If
            Next
        Next
        For Each re In ReList
            For Each cut In re.CutList.ToArray
                If (Not IsRecombining.Contains(cut.AOverhang)) OrElse (Not IsRecombining.Contains(cut.SOverhang)) Then
                    re.CutList.Remove(cut)
                End If
            Next
        Next
    End Sub
    Friend Shared Function Recombination(ByVal gList As List(Of GeneFile), ByVal method As RecombinationMethod, IsExhuastive As Boolean, Times As Integer) As List(Of GeneFile)
#If ReaderMode = 0 Then
        Dim vList As New List(Of GeneFile)
        Select Case method
            Case RecombinationMethod.Homologous
                '至少300个碱基
                Dim boxlength As Integer = 300
                'find a homologous arm first
                Dim sList As List(Of String)
                sList = SearchMaximumHomologousRegion(gList, boxlength)

                Dim LDict As New Dictionary(Of String, RestrictionEnzyme)

                Dim rec As String
                Dim cut As String
                Dim idx As Integer
                Dim ptn As String
                Dim kList As New List(Of String)

                For i As Integer = 0 To sList.Count - 1
                    ptn = sList(i)
                    rec = ptn
                    cut = rec
                    idx = 0
                    LDict.Add("Homo" + i.ToString, New Nuctions.RestrictionEnzyme("Homo" + i.ToString, rec, idx + cut.Length, idx, "&"))
                    kList.Add("Homo" + i.ToString)
                Next

                '确保只发生单交换 任何情况下，只有一种方法会发挥作用

                Dim zList As New List(Of String)

                '被单交换切割的列表
                Dim c0List As New List(Of GeneFile)
                Dim rsList As New List(Of GeneFile)
                rsList.AddRange(gList)

                For Each ptn In kList
                    '每次清空列表 然后仅加入一种序列 确保单交换
                    zList.Clear()
                    zList.Add(ptn)
                    c0List.Clear()
                    For Each gf As GeneFile In gList
                        Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(zList, gf, LDict)
                        c0List.AddRange(re.CutDNA())

                    Next
                    rsList.AddRange(LigateRecombination(c0List, IsExhuastive, Times))
                Next

                Dim sVList As New List(Of GeneFile)
                Dim contains As Boolean
                For Each gf As GeneFile In rsList
                    contains = False
                    For Each gfx As GeneFile In sVList
                        contains = contains Or (gfx = gf)
                    Next
                    If Not contains Then sVList.Add(gf)
                Next

                Return sVList

            Case RecombinationMethod.LambdaRecombination

                Dim LDict As New Dictionary(Of String, RestrictionEnzyme)

                Dim rvList As New List(Of GeneFile)
                Dim rsList As New List(Of GeneFile)
                Dim c0List As New List(Of GeneFile)
                Dim sList As New List(Of String)

                Dim i As Integer = 0
                For Each gf As GeneFile In gList
                    If gf.Length > 70 And gf.Length < 10000 And (rgxFreeEnd.IsMatch(gf.End_F) And rgxFreeEnd.IsMatch(gf.End_R)) Then
                        Dim gfc As GeneFile = gf.CloneWithoutFeatures
                        Dim rec As String
                        Dim cut As String
                        Dim idx As Integer
                        'LDict.Clear()
                        rec = gf.Sequence.Substring(0, 35)
                        If Not LDict.ContainsKey(rec) Then
                            cut = rec
                            idx = 0
                            LDict.Add(rec, New Nuctions.RestrictionEnzyme(rec, rec, idx + cut.Length, idx, "&"))
                            gfc.End_F = "&3" + Nuctions.ReverseComplement(rec)
                            sList.Add(rec)
                        End If


                        rec = gf.RCSequence.Substring(0, 35)
                        If Not LDict.ContainsKey(rec) Then
                            cut = rec
                            idx = 0
                            LDict.Add(rec, New Nuctions.RestrictionEnzyme(rec, rec, idx + cut.Length, idx, "&"))
                            gfc.End_R = "&3" + rec
                            sList.Add(rec)
                        End If
                        'c0List.Add(gfc)
                        i += 1
                        'Else

                    End If
                    rvList.Add(gf)
                Next
                If sList.Count > 0 Then
                    For Each gf In rvList
                        Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(sList, gf, LDict)
                        c0List.AddRange(re.CutDNA)
                    Next
                    rsList = LigateRecombination(c0List, IsExhuastive, Times)
                End If

                Return rsList
            Case RecombinationMethod.FRT
                Dim rlist As New List(Of String)
                rlist.Add("frt")
                Dim c0List As New List(Of GeneFile)

                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return LigateRecombination(c0List, IsExhuastive, Times)
            Case RecombinationMethod.LoxP
                Dim rlist As New List(Of String)
                rlist.Add("loxP")
                Dim c0List As New List(Of GeneFile)

                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return LigateRecombination(c0List, IsExhuastive, Times)
            Case RecombinationMethod.LambdaAttBP

                '成对出现原罪
                '必须成对出现才能切割 否则会造成错误
                Dim rlist As New List(Of String)
                '

                If ContainsRecombineGroup(New String() {"LattB", "LattP"}, gList) Then
                    rlist.Add("LattB")
                    rlist.Add("LattP")
                End If
                If ContainsRecombineGroup(New String() {"LattB1", "LattP1"}, gList) Then
                    rlist.Add("LattB1")
                    rlist.Add("LattP1")
                End If
                If ContainsRecombineGroup(New String() {"LattB2", "LattP2"}, gList) Then
                    rlist.Add("LattB2")
                    rlist.Add("LattP2")
                End If
                If ContainsRecombineGroup(New String() {"LattB3", "LattP3"}, gList) Then
                    rlist.Add("LattB3")
                    rlist.Add("LattP3")
                End If
                If ContainsRecombineGroup(New String() {"LattB4", "LattP4"}, gList) Then
                    rlist.Add("LattB4")
                    rlist.Add("LattP4")
                End If
                Dim c0List As New List(Of GeneFile)

                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return LigateRecombination(c0List, IsExhuastive, Times)
            Case RecombinationMethod.LambdaAttLR
                Dim rlist As New List(Of String)
                If ContainsRecombineGroup(New String() {"LattL", "LattR"}, gList) Then
                    rlist.Add("LattL")
                    rlist.Add("LattR")
                End If
                If ContainsRecombineGroup(New String() {"LattL1", "LattR1"}, gList) Then
                    rlist.Add("LattL1")
                    rlist.Add("LattR1")
                End If
                If ContainsRecombineGroup(New String() {"LattL2", "LattR2"}, gList) Then
                    rlist.Add("LattL2")
                    rlist.Add("LattR2")
                End If
                If ContainsRecombineGroup(New String() {"LattL3", "LattR3"}, gList) Then
                    rlist.Add("LattL3")
                    rlist.Add("LattR3")
                End If
                If ContainsRecombineGroup(New String() {"LattL4", "LattR4"}, gList) Then
                    rlist.Add("LattL4")
                    rlist.Add("LattR4")
                End If
                Dim c0List As New List(Of GeneFile)

                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return LigateRecombination(c0List, IsExhuastive, Times)
            Case RecombinationMethod.HK022AttBP
                Dim rlist As New List(Of String)
                If ContainsRecombineGroup(New String() {"HattB", "HattP"}, gList) Then
                    rlist.Add("HattB")
                    rlist.Add("HattP")
                End If
                Dim c0List As New List(Of GeneFile)

                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return LigateRecombination(c0List, IsExhuastive, Times)
            Case RecombinationMethod.HK022AttLR
                Dim rlist As New List(Of String)
                If ContainsRecombineGroup(New String() {"HattL", "HattR"}, gList) Then
                    rlist.Add("HattL")
                    rlist.Add("HattR")
                End If

                Dim c0List As New List(Of GeneFile)

                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return LigateRecombination(c0List, IsExhuastive, Times)
            Case RecombinationMethod.P21AttBP
                Dim rlist As New List(Of String)
                If ContainsRecombineGroup(New String() {"P21attB", "P21attP"}, gList) Then
                    rlist.Add("P21attB")
                    rlist.Add("P21attP")
                End If
                Dim c0List As New List(Of GeneFile)

                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return LigateRecombination(c0List, IsExhuastive, Times)
            Case RecombinationMethod.P21AttLR
                Dim rlist As New List(Of String)
                If ContainsRecombineGroup(New String() {"P21attL", "P21attR"}, gList) Then
                    rlist.Add("P21attL")
                    rlist.Add("P21attR")
                End If
                Dim c0List As New List(Of GeneFile)

                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return LigateRecombination(c0List, IsExhuastive, Times)
            Case RecombinationMethod.P22AttBP
                Dim rlist As New List(Of String)
                If ContainsRecombineGroup(New String() {"P22attB", "P22attP"}, gList) Then
                    rlist.Add("P22attB")
                    rlist.Add("P22attP")
                End If

                Dim c0List As New List(Of GeneFile)

                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return LigateRecombination(c0List, IsExhuastive, Times)
            Case RecombinationMethod.P22AttLR
                Dim rlist As New List(Of String)
                If ContainsRecombineGroup(New String() {"P22attL", "P22attR"}, gList) Then
                    rlist.Add("P22attL")
                    rlist.Add("P22attR")

                End If
                Dim c0List As New List(Of GeneFile)

                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return LigateRecombination(c0List, IsExhuastive, Times)
            Case RecombinationMethod.Phi80AttBP
                Dim rlist As New List(Of String)
                If ContainsRecombineGroup(New String() {"PattB", "PattP"}, gList) Then
                    rlist.Add("PattB")
                    rlist.Add("PattP")

                End If
                Dim c0List As New List(Of GeneFile)

                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return LigateRecombination(c0List, IsExhuastive, Times)
            Case RecombinationMethod.Phi80AttLR
                Dim rlist As New List(Of String)
                If ContainsRecombineGroup(New String() {"PattL", "PattR"}, gList) Then
                    rlist.Add("PattL")
                    rlist.Add("PattR")

                End If
                Dim c0List As New List(Of GeneFile)

                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return LigateRecombination(c0List, IsExhuastive, Times)
            Case RecombinationMethod.invitroAnnealing
                '增加的新重组方法 利用T4 DNA聚合酶或者lambda exo在体外重组

                Return MultipleLinearEndAnneal(gList, IsExhuastive, Times)
            Case RecombinationMethod.invivoAnnealing
                Return DegenerateMultipleLinearEndAnneal(gList, IsExhuastive, Times)

            Case RecombinationMethod.telRLSplit
                Dim rlist As New List(Of String) From {"TelRL", "TelRR", "TelLL"}
                Dim c0List As New List(Of GeneFile)
                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return c0List
            Case RecombinationMethod.HairpinReplication
                Dim c0List As New List(Of GeneFile)
                Dim ngf As GeneFile
                For Each gf As GeneFile In gList
                    If gf.End_F = "0B" And gf.End_R = "0B" Then
                        ngf = New GeneFile
                        ngf.End_F = "::"
                        ngf.End_R = "::"
                        ngf.Sequence = gf.Sequence + gf.RCSequence
                        ngf.Name = "HR-" + ngf.Length.ToString
                        c0List.Add(ngf)
                    End If
                Next
                Return c0List
            Case RecombinationMethod.phiC31attBP
                Dim rlist As New List(Of String)
                If ContainsRecombineGroup(New String() {"phiC31attBCT", "phiC31attPCT"}, gList) Then
                    rlist.Add("phiC31attBCT")
                    rlist.Add("phiC31attPCT")
                End If
                If ContainsRecombineGroup(New String() {"phiC31attBGT", "phiC31attPGT"}, gList) Then
                    rlist.Add("phiC31attBGT")
                    rlist.Add("phiC31attPGT")
                End If
                If ContainsRecombineGroup(New String() {"phiC31attBCC", "phiC31attPCC"}, gList) Then
                    rlist.Add("phiC31attBCC")
                    rlist.Add("phiC31attPCC")
                End If
                Dim c0List As New List(Of GeneFile)

                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return LigateRecombination(c0List, IsExhuastive, Times)
            Case RecombinationMethod.phiC31attLR
                Dim rlist As New List(Of String)
                If ContainsRecombineGroup(New String() {"phiC31attLCT", "phiC31attRCT"}, gList) Then
                    rlist.Add("phiC31attLCT")
                    rlist.Add("phiC31attRCT")
                End If
                If ContainsRecombineGroup(New String() {"phiC31attLGT", "phiC31attRGT"}, gList) Then
                    rlist.Add("phiC31attLGT")
                    rlist.Add("phiC31attRGT")
                End If
                If ContainsRecombineGroup(New String() {"phiC31attLCC", "phiC31attRCC"}, gList) Then
                    rlist.Add("phiC31attLCC")
                    rlist.Add("phiC31attRCC")
                End If
                Dim c0List As New List(Of GeneFile)

                For Each gf As GeneFile In gList
                    Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(rlist, gf, SettingEntry.RecombinationSiteDict)
                    c0List.AddRange(re.CutDNA)
                Next
                Return LigateRecombination(c0List, IsExhuastive, Times)
            Case Else
                Return New List(Of GeneFile)
        End Select
#End If
        Return New List(Of GeneFile)
    End Function

    Private Shared Function ContainsRecombineGroup(ByVal Keys As String(), ByVal gList As List(Of GeneFile)) As Boolean

        Dim vList As New List(Of String)
        Dim contains As Boolean = True
        Dim found As Boolean

        For Each key As String In Keys
            vList.Clear()
            vList.Add(key)
            found = False
            For Each gf As GeneFile In gList
                Dim re As New EnzymeAnalysis.EnzymeAnalysisResult(vList, gf, SettingEntry.RecombinationSiteDict)
                If re.Count > 0 Then found = True : Exit For
            Next
            contains = contains And found
        Next
        Return contains
    End Function

    Private Shared Function EndLigate(ByVal G1 As GeneFile, ByVal G2 As GeneFile) As GeneFile
        Return G1 + G2
    End Function
    Private Shared Function SelfLigate(ByVal G1 As GeneFile) As GeneFile
        Return +G1
    End Function

    Private Shared Function EndAnneal(ByVal G1 As GeneFile, ByVal G2 As GeneFile) As GeneFile
        '必须完全匹配才可以连接
        Dim rs As GeneFile = Nothing
        If G1.Length >= 50 And G2.Length >= 50 Then
            Dim E1 As String = G1.Sequence.Substring(G1.Length - 50, 50)
            Dim E2 As String = G2.Sequence.Substring(0, 50)
            Dim rec As Boolean = False
            Dim i As Integer
            For i = 25 To 50
                If E1.Substring(50 - i, i) = E2.Substring(0, i) Then
                    rec = True
                    Exit For
                End If
            Next
            If rec Then
                rs = New GeneFile
                rs.Name = "In-Vitro Anneal"
                rs.End_F = G1.End_F
                rs.End_R = G2.End_R
                Dim stb As New System.Text.StringBuilder
                stb.Append(G1.Sequence)
                stb.Append(G2.Sequence, i, G2.Sequence.Length - i)
                rs.Sequence = stb.ToString
            End If
        End If
        Return rs
    End Function
    Private Shared Function DegenerateEndAnneal(ByVal G1 As GeneFile, ByVal G2 As GeneFile) As GeneFile
        '必须完全匹配才可以连接
        'DegenerateEndAnneal How to do this and the rest?

        Dim rs As GeneFile = Nothing
        'key length is 20 
        Dim MinLength As Integer = 12
        Dim MaxSearchRegion As Integer = 250
        Dim MinSpacing As Integer = 25

        If G1.Length < MinLength * 2 + MinSpacing Then Return Nothing
        If G2.Length < MinLength * 2 + MinSpacing Then Return Nothing

        Dim key As String = Nothing
        Dim fit As Integer = -1

        For j As Integer = 0 To Math.Min(G2.Length - MinLength, MaxSearchRegion - MinLength)
            key = G2.Sequence.Substring(j, MinLength)
            fit = G1.Sequence.IndexOf(key, Math.Max(G1.Length - MaxSearchRegion, MinLength + MinSpacing))
            If fit > -1 Then
                rs = New GeneFile
                rs.Name = "In-Vivo Anneal"
                rs.End_F = G1.End_F
                rs.End_R = G2.End_R
                Dim stb As New System.Text.StringBuilder
                stb.Append(G1.Sequence.Substring(0, fit))
                stb.Append(G2.Sequence.Substring(j))
                rs.Sequence = stb.ToString
                Exit For
            End If
        Next


        'If G1.Length >= 50 And G2.Length >= 50 Then
        '    For j As Integer = 0 To 30
        '        key = G2.Sequence.Substring(j, 20)
        '        fit = G1.Sequence.Substring(G1.Length - 50, 50).IndexOf(key)
        '        If fit > -1 Then
        '            rs = New GeneFile
        '            rs.Name = "In-Vivo Anneal"
        '            rs.End_F = G1.End_F
        '            rs.End_R = G2.End_R
        '            Dim stb As New System.Text.StringBuilder
        '            stb.Append(G1.Sequence.Substring(0, G1.Length - 50 + fit))
        '            stb.Append(G2.Sequence.Substring(j))
        '            rs.Sequence = stb.ToString
        '            Exit For
        '        End If
        '    Next
        'End If
        'If rs Is Nothing Then
        '    If G1.Length >= 500 And G2.Length >= 500 Then
        '        For j As Integer = 0 To 450
        '            key = G2.Sequence.Substring(j, 50)
        '            fit = G1.Sequence.Substring(G1.Length - 500, 500).IndexOf(key)
        '            If fit > -1 Then
        '                rs = New GeneFile
        '                rs.Name = "In-Vivo Anneal"
        '                rs.End_F = G1.End_F
        '                rs.End_R = G2.End_R
        '                Dim stb As New System.Text.StringBuilder
        '                stb.Append(G1.Sequence.Substring(0, G1.Length - 500 + fit))
        '                stb.Append(G2.Sequence.Substring(j))
        '                rs.Sequence = stb.ToString
        '                Exit For
        '            End If
        '        Next
        '    End If
        'End If
        Return rs
    End Function
    Private Shared Function DegenerateSelfEndAnneal(ByVal G1 As GeneFile) As GeneFile
        '必须完全匹配才可以连接
        'DegenerateEndAnneal How to do this and the rest?

        Dim rs As GeneFile = Nothing
        'key length is 20 
        Dim MinLength As Integer = 12
        Dim MaxSearchRegion As Integer = 250
        Dim MinSpacing As Integer = 25

        If G1.Length < MinLength * 2 + MinSpacing Then Return Nothing

        Dim key As String = Nothing
        Dim fit As Integer = -1

        For j As Integer = 0 To Math.Min(250, G1.Length - MinLength)
            key = G1.Sequence.Substring(j, MinLength)
            fit = G1.Sequence.IndexOf(key, Math.Max(G1.Length - MaxSearchRegion, j + MinLength + MinSpacing))
            If fit > -1 Then
                rs = New GeneFile
                rs.Name = "In-Vivo Anneal"
                rs.End_F = "::"
                rs.End_R = "::"
                rs.Sequence = G1.Sequence.Substring(j, fit - j)
                Exit For
            End If
        Next

        'If G1.Length >= 50 Then
        '    For j As Integer = 0 To 30
        '        key = G1.Sequence.Substring(j, MinLength)
        '        fit = G1.Sequence.Substring(G1.Length - 50, 50).IndexOf(key)
        '        If fit > -1 Then
        '            rs = New GeneFile
        '            rs.Name = "In-Vivo Anneal"
        '            rs.End_F = "::"
        '            rs.End_R = "::"
        '            rs.Sequence = G1.Sequence.Substring(j, G1.Length - 50 + fit - j)
        '            Exit For
        '        End If
        '    Next
        'End If
        'If rs Is Nothing Then
        '    If G1.Length >= 1000 Then
        '        For j As Integer = 0 To 500 - MinLength
        '            key = G1.Sequence.Substring(j, MinLength)
        '            fit = G1.Sequence.Substring(G1.Length - 500, 500).IndexOf(key)
        '            If fit > -1 Then
        '                rs = New GeneFile
        '                rs.Name = "In-Vivo Anneal"
        '                rs.End_F = "::"
        '                rs.End_R = "::"
        '                rs.Sequence = G1.Sequence.Substring(j, G1.Length - 500 + fit - j)
        '                Exit For
        '            End If
        '        Next
        '    End If
        'End If
        Return rs
    End Function
    Private Shared Function SelfEndAnneal(ByVal G1 As GeneFile) As GeneFile
        '必须完全匹配才可以连接
        Dim rs As GeneFile = Nothing
        If G1.Length >= 50 Then
            Dim E1 As String = G1.Sequence.Substring(G1.Length - 50, 50)
            Dim E2 As String = G1.Sequence.Substring(0, 50)
            Dim rec As Boolean = False
            Dim i As Integer
            For i = 25 To 50
                If E1.Substring(50 - i, i) = E2.Substring(0, i) Then
                    rec = True
                    Exit For
                End If
            Next
            If rec Then
                rs = New GeneFile
                rs.Name = "In-Vitro Anneal"
                rs.End_F = "::"
                rs.End_R = "::"
                Dim stb As New System.Text.StringBuilder
                stb.Append(G1.Sequence, 0, G1.Length - i)
                rs.Sequence = stb.ToString
            End If
        End If
        Return rs
    End Function

    Private Shared Function PartialEndAnneal(ByVal G1 As GeneFile, ByVal G2 As GeneFile) As GeneFile
        '允许一些末端10个碱基不匹配 属于Landing Pad方法
        Dim rs As GeneFile = Nothing


        Return rs
    End Function
    Private Shared Function SelfPartialEndAnneal(ByVal G1 As GeneFile, ByVal G2 As GeneFile) As GeneFile
        '允许一些末端10个碱基不匹配 属于Landing Pad方法
        Dim rs As GeneFile = Nothing


        Return rs
    End Function
    Private Shared Function LigateRecombination(ByVal c0List As List(Of GeneFile), IsExhaustive As Boolean, Times As Integer) As List(Of GeneFile)
        Dim vResults As New List(Of GeneFile)
        If IsExhaustive Then
            Dim MR As New MultipleVirtualRecombinator(c0List, New PseudoConnector(AddressOf PseudoGeneFile.EndRecombine), New PseudoSelfConnector(AddressOf PseudoGeneFile.SelfRecombine), True, Times)
            MR.Connect()
            For Each gf In MR.GetProducts()
                Dim gr = gf
                If gr.IsNotRecombinating Then vResults.Add(gr)
            Next
        Else
            Dim MR As New MultipleVirtualReactor(c0List, New PseudoConnector(AddressOf PseudoGeneFile.EndRecombine), New PseudoSelfConnector(AddressOf PseudoGeneFile.SelfRecombine), True, Times)
            MR.Connect()
            For Each gf In MR.GetProducts()
                Dim gr = gf
                If gr.IsNotRecombinating Then vResults.Add(gr)
            Next
        End If

        Dim rmList As New List(Of GeneFile)
        If vResults.Count > 20 Then
            Dim _Cancel As New CancelRunViewModel() With {.Operation = "Simplifying Products"}
            Dim ConnectingTask As New System.Threading.Tasks.Task(Sub(token As System.Threading.CancellationToken)
                                                                      Try
                                                                          For Each gf1 In vResults
                                                                              If token.IsCancellationRequested Then Exit For
                                                                              If rmList.Contains(gf1) Then Continue For
                                                                              For Each gf2 In vResults
                                                                                  If token.IsCancellationRequested Then Exit For
                                                                                  If gf2 Is gf1 Then Continue For
                                                                                  If rmList.Contains(gf2) Then Continue For
                                                                                  If gf1 = gf2 Then rmList.Add(gf2)
                                                                              Next
                                                                          Next
                                                                          For Each gf In rmList
                                                                              If token.IsCancellationRequested Then Exit For
                                                                              vResults.Remove(gf)
                                                                          Next
                                                                          If token.IsCancellationRequested Then vResults.Clear()
                                                                      Catch ex As Exception

                                                                      End Try
                                                                      _Cancel.Close()
                                                                  End Sub, _Cancel.Token)
            ConnectingTask.Start()
            CancelRunViewModel.ShowCancelRunWindow(_Cancel)
        Else
            For Each gf1 In vResults
                If rmList.Contains(gf1) Then Continue For
                For Each gf2 In vResults
                    If gf2 Is gf1 Then Continue For
                    If rmList.Contains(gf2) Then Continue For
                    If gf1 = gf2 Then rmList.Add(gf2)
                Next
            Next
            For Each gf In rmList
                vResults.Remove(gf)
            Next
        End If

        Return vResults
    End Function

    Private Shared Function SingleLigateRecombination(ByVal c0List As List(Of GeneFile)) As List(Of GeneFile)
        Dim c1List As New List(Of GeneFile)

        For Each gf1 As GeneFile In c0List
            For Each gf2 As GeneFile In c0List
                Dim gf As GeneFile
                gf = gf1 - gf2
                If Not (gf Is Nothing) Then c1List.Add(gf)
                gf = gf1 - gf2.RC
                If Not (gf Is Nothing) Then c1List.Add(gf)
                gf = gf1.RC - gf2
                If Not (gf Is Nothing) Then c1List.Add(gf)
                gf = gf1.RC - gf2.RC
                If Not (gf Is Nothing) Then c1List.Add(gf)
            Next
        Next

        'Dim c2List As New List(Of GeneFile)

        'For Each gf1 As GeneFile In c1List
        '    For Each gf2 As GeneFile In c0List
        '        Dim gf As GeneFile
        '        gf = gf1 - gf2
        '        If Not (gf Is Nothing) Then c2List.Add(gf)
        '        gf = gf1 - gf2.RC
        '        If Not (gf Is Nothing) Then c2List.Add(gf)
        '        gf = gf1.RC - gf2
        '        If Not (gf Is Nothing) Then c2List.Add(gf)
        '        gf = gf1.RC - gf2.RC
        '        If Not (gf Is Nothing) Then c2List.Add(gf)
        '    Next
        'Next

        Dim cSList As New List(Of GeneFile)
        Dim gfu As GeneFile
        For Each gf As GeneFile In c0List
            gfu = -gf
            If Not (gfu Is Nothing) And gf.Length > 0 Then cSList.Add(gfu)
        Next
        For Each gf As GeneFile In c1List
            gfu = -gf
            If Not (gfu Is Nothing) And gf.Length > 0 Then cSList.Add(gfu)
        Next
        'For Each gf As GeneFile In c2List
        '    gfu = -gf
        '    If Not (gfu Is Nothing) Then cSList.Add(gfu)
        'Next
        'cSList.AddRange(c0List)
        cSList.AddRange(c1List)
        'cSList.AddRange(c2List)

        Dim sRList As New List(Of GeneFile)
        For Each gf As GeneFile In cSList
            If gf.IsNotRecombinating Then sRList.Add(gf)
        Next

        Dim sVList As New List(Of GeneFile)
        Dim contains As Boolean
        For Each gf As GeneFile In sRList
            contains = False
            For Each gfx As GeneFile In sVList
                contains = contains Or (gfx = gf)
            Next
            If Not contains Then sVList.Add(gf)
        Next

        For Each gf As GeneFile In sVList
            gf.Name = "Rec " + gf.Length.ToString
        Next
        Return sVList
    End Function

    Friend Shared Function FindEnzymes(ByVal Conditions As List(Of EnzymeAnalysisItem)) As List(Of String)
        Dim sList As New List(Of String)
#If ReaderMode = 0 Then


        For Each ez As RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
            sList.Add(ez.Name)
        Next

        Dim rmList As New List(Of String)
        Dim kList As New List(Of String)

        For Each eai As EnzymeAnalysisItem In Conditions
            If eai.Use Then
                Select Case eai.Method
                    Case EnzymeAnalysisEnum.Equal
                        rmList.Clear()
                        For Each key As String In sList
                            kList.Clear()
                            kList.Add(key)
                            Dim ear As New Nuctions.EnzymeAnalysis.EnzymeAnalysisResult(kList, eai.GeneFile)
                            If ear.Count <> eai.Value Then
                                rmList.Add(key)
                            End If
                        Next
                        For Each key As String In rmList
                            sList.Remove(key)
                        Next
                    Case EnzymeAnalysisEnum.Greater
                        rmList.Clear()
                        For Each key As String In sList
                            kList.Clear()
                            kList.Add(key)
                            Dim ear As New Nuctions.EnzymeAnalysis.EnzymeAnalysisResult(kList, eai.GeneFile)
                            If ear.Count <= eai.Value Then
                                rmList.Add(key)
                            End If
                        Next
                        For Each key As String In rmList
                            sList.Remove(key)
                        Next
                    Case EnzymeAnalysisEnum.Less
                        rmList.Clear()
                        For Each key As String In sList
                            kList.Clear()
                            kList.Add(key)
                            Dim ear As New Nuctions.EnzymeAnalysis.EnzymeAnalysisResult(kList, eai.GeneFile)
                            If ear.Count >= eai.Value Then
                                rmList.Add(key)
                            End If
                        Next
                        For Each key As String In rmList
                            sList.Remove(key)
                        Next
                End Select
            End If
        Next
        sList.Sort()
#End If
        Return sList
    End Function

    Friend Shared Function ParseInnerPrimer(ByVal pmr As String) As String
        If pmr Is Nothing Then Return ""

        Dim ip As Integer = pmr.LastIndexOf(">")
        ip = IIf(ip < 0, 0, ip)
        Return pmr.Substring(ip)
    End Function

    Friend Shared Function GenerateAssemblyLibrary() As String
        '需要生成一系列DNA序列，互相之间没有重复，并且本身没有Hairpin和Dimer
        Dim Uni As String = ""
        Dim A As String = "A"
        Dim T As String = "T"
        Dim G As String = "G"
        Dim C As String = "C"



        Return Uni.ToString
    End Function
    Friend Shared Function GenerateUniqueCodons() As List(Of String)
        Dim Nts As New List(Of String) From {"A", "T", "G", "C"}
        Dim codons As New List(Of String)
        For i1 As Integer = 0 To 3
            For i2 As Integer = 0 To 3
                For i3 As Integer = 0 To 3
                    codons.Add(Nts(i1) + Nts(i2) + Nts(i3))
                Next
            Next
        Next
        Return codons
    End Function
    Friend Shared Function ScreenByHash(ByVal items As List(Of GeneFile), ByVal conditions As List(Of String)) As List(Of GeneFile)
        Dim Results As New List(Of GeneFile)
        For Each gf As GeneFile In items
            If conditions.Contains(gf.GetHash) Then
                Results.Add(gf)
            End If
        Next
        Return Results
    End Function

    Friend Shared Function CalculateTheoreticalSequencing(ByVal gList As List(Of GeneFile), ByVal Primer As String, ByVal vLength As Integer) As List(Of String)
        Dim FS As String
        Dim RS As String
        Dim res As New List(Of String)
        Dim rgx As New System.Text.RegularExpressions.Regex(Primer)
        Dim m As System.Text.RegularExpressions.Match
        Dim L As Integer
        For Each gf As GeneFile In gList
            If gf.Iscircular Then
                FS = gf.Sequence + gf.SubSequence(0, Primer.Length - 2)
                RS = gf.RCSequence + gf.SubRCSequence(0, Primer.Length - 2)
                For Each m In rgx.Matches(FS)
                    res.Add(gf.SubSequence(m.Index, m.Index + m.Length + vLength + 499))
                Next
                For Each m In rgx.Matches(RS)
                    res.Add(gf.SubRCSequence(m.Index, m.Index + m.Length + vLength + 499))
                Next
            Else
                FS = gf.Sequence
                RS = gf.RCSequence
                For Each m In rgx.Matches(FS)
                    L = m.Index + vLength + m.Length + 500
                    If m.Index + L > gf.Length Then L = gf.Length - 1
                    res.Add(gf.SubSequence(m.Index, L - 1))
                Next
                For Each m In rgx.Matches(RS)
                    L = m.Index + vLength + m.Length + 500
                    If m.Index + L > gf.Length Then L = gf.Length - 1
                    res.Add(gf.SubRCSequence(m.Index, L - 1))
                Next
            End If
        Next
        Return res
    End Function
    Public Class AminoCodeUnit
        Public AA As Codon
        Public Index As Integer
        Public CurrentOption As Integer
        Public PossibleOptions As New List(Of Integer)
        Public MaxChawBack As Integer
        Public IsRandom As Boolean = False
        Public Function Roll() As Boolean
            Dim vPO As New List(Of Integer)
            vPO.AddRange(PossibleOptions)
            Dim vBan As List(Of Integer) = FindBanned.Invoke(Index)
            For Each vKey As Integer In vBan
                vPO.Remove(vKey)
            Next
            If vPO.Count > 0 Then
                If IsRandom Then
                    Dim k As Single = Rnd(Now.ToOADate) * Rnd(Now.ToOADate)
                    CurrentOption = vPO(Math.Floor(vPO.Count * k))
                Else
                    CurrentOption = vPO(0)
                End If
                Return True
            Else
                Return False
            End If
        End Function
        Public Function GetCode() As String
            Return AA.CodeList(CurrentOption).Name
        End Function
        Public FindBanned As Func(Of Integer, List(Of Integer))
        Public Sub GenerateOptions()
            Dim i As Integer = 0
            For Each vvc In AA.CodeList
                PossibleOptions.Add(i)
                i += 1
            Next
        End Sub
    End Class
    Public Class OptimizingSequence
        Private ACUS As New List(Of AminoCodeUnit)
        Private Avoids As New List(Of RestrictionEnzyme)
        Public Sub New(AAs As String, ByVal trans As Translation, ByVal RE As RestrictionEnzymes, ByVal IsRandom As Boolean, ByVal Avoiding As List(Of String))
            AAs = Nuctions.AminoAcidFilter(AAs)

            Dim ez As RestrictionEnzyme
            For Each ezkey As String In Avoiding
                ez = RE(ezkey)
                MaxChawBack = Math.Max(MaxChawBack, Math.Floor((ez.Sequence.Length + 1) / 3) + 1)
                '1:1 2:2 3:2 4:2 5:3 
                Avoids.Add(ez)
            Next

            Dim _Last As AminoCodeUnit = Nothing
            Dim acu As AminoCodeUnit
            For i As Integer = 0 To AAs.Length - 1
                acu = New AminoCodeUnit With {.AA = SettingEntry.CodonCol.AnimoTable(AAs.Chars(i)), .MaxChawBack = MaxChawBack, .Index = i, .IsRandom = IsRandom, .FindBanned = AddressOf FindBanned}
                ACUS.Add(acu)
                acu.GenerateOptions()
                _Last = acu
            Next
        End Sub
        Public Function FindBanned(vIndex As Integer) As List(Of Integer)
            Dim klist As New List(Of Integer)
            For i As Integer = CurrentOrigin To vIndex - 1
                klist.Add(ACUS(i).Index)
            Next
            Return CurrentNode.GetBanned(klist, 0)
        End Function
        Private CurrentOrigin As Integer
        Private CurrentEnd As Integer
        Private CurrentNode As New BanNode
        Private MaxChawBack As Integer = 0
        Public Function Optimize() As List(Of Integer)
            CurrentOrigin = 0
            CurrentEnd = 0
            Dim CurrentOffset As Integer = 0
            Dim aaLength As Integer = ACUS.Count
            Dim vCode As String
            While CurrentEnd < aaLength
                CurrentOffset = 0
                While Not ACUS(CurrentEnd - CurrentOffset).Roll()
                    While Not ACUS(CurrentEnd - CurrentOffset).Roll()
                        CurrentOffset += 1
                        If CurrentOffset > MaxChawBack Then
                            Return Nothing
                        End If
                    End While
                    CurrentOffset = 0
                End While
                vCode = ReadCode()
                If CheckCode(vCode) Then
                    CurrentEnd += 1
                    CurrentOrigin = Math.Max(CurrentEnd - MaxChawBack, 0)
                    CurrentNode = New BanNode
                Else
                    CurrentNode.Ban(ReadTree, 0)
                End If
            End While
            Dim kList As New List(Of Integer)
            For Each ac As AminoCodeUnit In ACUS
                kList.Add(ac.CurrentOption)
            Next
            Return kList
        End Function
        Public Function ReadCode() As String
            Dim stb As New System.Text.StringBuilder
            For i As Integer = CurrentOrigin To CurrentEnd
                stb.Append(ACUS(i).GetCode)
            Next
            Return stb.ToString
        End Function
        Public Function CheckCode(value As String) As Boolean
            For Each rse As RestrictionEnzyme In Avoids
                If rse.Reg.IsMatch(value) Then Return False
                If Not rse.Palindromic Then
                    If rse.Reg.IsMatch(ReverseComplement(value)) Then Return False
                End If
            Next
            Return True
        End Function
        Public Function ReadTree() As List(Of Integer)
            Dim kList As New List(Of Integer)
            For i As Integer = CurrentOrigin To CurrentEnd
                kList.Add(ACUS(i).Index)
            Next
            Return kList
        End Function
    End Class
    Public Class BanNode
        'Public Key As Integer
        Public Children As New Dictionary(Of Integer, BanNode)
        Private Shared Empty As New List(Of Integer)
        Public Sub Ban(values As IEnumerable(Of Integer), index As Integer)
            Dim idx As Integer = values(index)
            Dim inner As BanNode
            If Children.ContainsKey(idx) Then
                If index < values.Count - 1 Then
                    Children(idx).Ban(values, index + 1)
                End If
            Else
                inner = New BanNode ' With {.Key = }
                Children.Add(idx, inner)
                inner.Ban(values, index + 1)
            End If
        End Sub
        Public Function GetBanned(values As IEnumerable(Of Integer), index As Integer) As IEnumerable(Of Integer)
            Dim idx As Integer = values(index)
            If Children.ContainsKey(idx) Then
                If index = values.Count - 1 Then
                    Return Me.Children.Keys
                Else
                    Return Children(idx).GetBanned(values, index + 1)
                End If
            Else
                Return Empty
            End If
        End Function
        Public Function IsBanned(values As IEnumerable(Of Integer), index As Integer) As Boolean
            Dim idx As Integer = values(index)
            If Children.ContainsKey(idx) Then
                If index = values.Count - 1 Then
                    Return True
                Else
                    Return Children(idx).IsBanned(values, index + 1)
                End If
            Else
                Return False
            End If
        End Function
    End Class
    Friend Shared Function SequenceOptimize(ByVal AAs As String, ByVal trans As Translation, ByVal RE As RestrictionEnzymes, ByVal IsRandom As Boolean, ByVal Avoiding As List(Of String)) As String

    End Function

    Friend Shared Function CodonOptimize(ByVal AAs As String, ByVal trans As Translation, ByVal RE As RestrictionEnzymes, ByVal IsRandom As Boolean, ByVal Avoiding As List(Of String)) As String
        Dim AC As String = AminoAcidFilter(AAs)

        Dim i As Integer = 0

        Dim l As Integer = AC.Length

        Dim C As String

        Dim Alist As New BanList
        Dim Clist As New List(Of Codon)

        Dim reList As List(Of RestrictionEnzyme) = GetEnzymes(RE, Avoiding)

        Dim backlength As Integer
        Dim maxlength As Integer = 0
        For Each r As RestrictionEnzyme In reList
            maxlength = Math.Max(maxlength, r.Sequence.Length)
        Next
        backlength = (maxlength + 4) \ 3

        Dim local As String

        While i < l
            C = AC.Chars(i)
            'If Not ADict.ContainsKey(i) Then ADict.Add(i, New List(Of GeneticCode))
            Clist.Add(trans.AnimoTable(C))
            ChooseCodon(trans.AnimoTable(C).CodeList, Alist, IsRandom)
            i += 1
        End While

        local = ReadLocalCode(Alist, Clist)


        Return local
        'There is another method and how to deal with it. Are there many reason for this kind of replication.
    End Function

    Private Class BanList
        Inherits List(Of Integer)
        Public Start As Integer
        Public RelatedEnzyme As RestrictionEnzyme
        Public RCRe As Boolean = False
        Public Property Code(ByVal Index As Integer) As Integer
            Get
                Return Me(Index - Start)
            End Get
            Set(ByVal value As Integer)
                Me(Index - Start) = value
            End Set
        End Property
        Public Function SubList(ByVal Index As Integer, ByVal Length As Integer) As BanList
            Dim bl As New BanList With {.Start = Index}
            For i As Integer = Index To Index + Length - 1
                bl.Add(Me(i))
            Next
            Return bl
        End Function
        Public Function Allows(ByVal TryCode As Integer, ByVal Index As Integer, ByVal OriginalList As BanList) As Boolean
            If Index < Start Or Index > Start + Count - 1 Then
                Return True
            Else
                Dim fit As Boolean = (TryCode = OriginalList(Index))
                For i As Integer = 0 To Count - 1
                    If i + Start = Index Then
                        fit = fit And (OriginalList(Start + i) = TryCode)
                    Else
                        fit = fit And (OriginalList(Start + i) = Me(i))
                    End If
                Next
                Return Not fit
            End If
        End Function
        Public Sub Shuffle(ByVal Alist As BanList, ByVal Clist As List(Of Codon), ByVal BanDict As List(Of BanList))
            Dim v As New Dictionary(Of Integer, Integer)
            Dim cx As New Dictionary(Of Integer, Integer)
            For i As Integer = Start To Start + Count - 1
                cx(i) = IIf(Code(i) > -1, Clist(i).CodeList.Count - 1, -1)
                v(i) = Alist(i)
            Next

            Dim r As Integer = Start
            While r < Start + Count
                For k As Integer = 0 To cx(r)

                Next
            End While
        End Sub
    End Class

    Private Function FindBanList(ByVal DNA As String, ByVal Enzymes As List(Of RestrictionEnzyme), ByVal Alist As BanList) As List(Of BanList)
        Dim bList As New List(Of BanList)
        Dim RCDNA As String = ReverseComplement(DNA)
        Dim bIndex As Integer
        Dim DL As Integer = DNA.Length
        For Each re As RestrictionEnzyme In Enzymes
            For Each m As System.Text.RegularExpressions.Match In re.Reg.Matches(DNA)
                Dim b As New BanList
                bIndex = m.Index \ 3
                b.Start = bIndex
                For i As Integer = m.Index To m.Index + m.Length - 1
                    If i \ 3 > bIndex Then
                        b.Add(-1)
                    End If
                    If re.Sequence.Chars(i - m.Index) <> "N" Then
                        b.Code(bIndex) = Alist.Code(i \ 3)
                    End If
                Next
                bList.Add(b)
            Next
            If Not re.Palindromic Then
                Dim RCRE As String
                For Each m As System.Text.RegularExpressions.Match In re.Reg.Matches(RCDNA)
                    RCRE = ReverseComplement(re.Sequence)
                    Dim b As New BanList
                    bIndex = (DL - (m.Index + m.Length)) \ 3
                    b.Start = bIndex
                    b.RelatedEnzyme = re
                    b.RCRe = True
                    For i As Integer = DL - (m.Index + m.Length) To DL - m.Index - 1
                        If i \ 3 > bIndex Then
                            b.Add(-1)
                        End If
                        If RCRE.Chars(DL - i) <> "N" Then
                            b.Code(bIndex) = Alist.Code((DL - i) \ 3)
                        End If
                    Next
                    bList.Add(b)
                Next
            End If
        Next
        Return bList
    End Function

    'Private Shared Function GenerateANCodes(ByVal trans As Translation) As Dictionary(Of String, GeneticCode)
    '    Dim i As Integer
    '    Dim GAN As New Dictionary(Of String, GeneticCode)
    '    For Each cx As Codon In trans.AnimoTable.Values
    '        i = 0
    '        For Each gx As GeneticCode In cx.CodeList
    '            GAN.Add(cx.ShortName + i.ToString, gx)
    '            i += 1
    '        Next
    '    Next
    '    Return GAN
    'End Function

    'Private Shared Function GenerateTransDict(ByVal GAN As Dictionary(Of String, GeneticCode)) As Dictionary(Of String, List(Of String))
    '    Static N As New List(Of String) From {"A", "T", "G", "C", "N"}
    '    '"A" "T" "G" "C" "H" "I" "V" "N"

    'End Function

    'Friend Class RestrictionChainCollection
    '    Inherits List(Of RestrictChain)
    '    Private GAN As Dictionary(Of String, GeneticCode)

    '    Public Sub New(ByVal trans As Translation, ByVal RE As RestrictionEnzymes, ByVal vList As List(Of String))

    '    End Sub
    '    Public Function GetStricts(ByVal A As String) As List(Of GeneticCode)

    '    End Function

    'End Class

    'Friend Class RestrictChain
    '    Inherits List(Of List(Of String))
    '    Public Sub New(ByVal Sequence As String, ByVal trans As Translation)

    '    End Sub
    'End Class

    Private Shared Function GetEnzymes(ByVal RE As RestrictionEnzymes, ByVal vList As List(Of String)) As List(Of RestrictionEnzyme)
        Dim reList As New List(Of RestrictionEnzyme)
        Dim lList As New List(Of String)
        For Each s As String In vList
            lList.Add(s.ToLower)
        Next
        For Each r As RestrictionEnzyme In RE.RECollection
            If lList.Contains(r.Name.ToLower()) Then reList.Add(r)
        Next
        Return reList
    End Function

    Private Shared Function ChooseCodon(ByVal pool As List(Of GeneticCode), ByVal used As BanList, ByVal IsRandom As Boolean) As Boolean
        Dim cd As Integer
        Dim i As Integer = 0
        If IsRandom Then
            Dim r As Single = Rnd(Now.ToOADate) * 0.9F
            For i = 0 To pool.Count - 1
                'If used.Contains(cd) Then Continue For
                r -= pool(i).ratio
                If r < 0.0F Then
                    cd = i
                    Exit For
                Else
                    cd = 0
                End If
            Next
        Else
            cd = 0
        End If
        used.Add(cd)
        Return True
    End Function

    Private Shared Function ReadLocalCode(ByVal Alist As BanList, ByVal Clist As List(Of Codon)) As String
        Dim stb As New System.Text.StringBuilder
        For i As Integer = 0 To Alist.Count - 1
            stb.Append(Clist(i + Alist.Start).CodeList(Alist(i)))
        Next
        Return stb.ToString
    End Function

    Private Shared Function SumRatio(ByVal used As List(Of GeneticCode)) As Single
        Dim v As Single = 0.0F
        For Each cd As GeneticCode In used
            v += cd.ratio
        Next
        Return v
    End Function

    Friend Shared Function AminoAcidFilter(ByVal AAs As String) As String
        Dim AC As String = AAs.ToUpper

        Dim stb As New System.Text.StringBuilder

        For Each C As Char In AC.ToCharArray
            Select Case C
                Case "G", "S", "A", "T", "V",
                    "N", "I", "Q", "L", "Y",
                    "F", "H", "P", "D", "M",
                    "E", "W", "K", "C", "R"
                    stb.Append(C)
                Case "-"
                    stb.Append(C)
                    'Exit For
            End Select
        Next
        Return stb.ToString
    End Function

    Friend Shared Sub ParseCode(ByVal FreeDesignName As String, ByRef FreeDesignCode As String, ByVal Features As List(Of Feature), ByVal DNAs As Collection, ByVal EnzList As List(Of String))
        Dim gf As New Nuctions.GeneFile
        gf.Name = FreeDesignName
        '[BamHI]
        '<Partname>
        '{TAG::}tagc[bamhi]<partname>tcga{CGA::}
        'TAGC[bamhi]<partname>ttagc::
        '{TAG::}TAGC[BamHI]<partname>tcga{CGA::}
        '
        Dim regE As New System.Text.RegularExpressions.Regex("\[(\w+)]", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regENC As New System.Text.RegularExpressions.Regex("\[(\w+)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regEVNC As New System.Text.RegularExpressions.Regex("\[(\w+)\:([ATGCatgc]+)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regERC As New System.Text.RegularExpressions.Regex("<(\w+)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regEVRC As New System.Text.RegularExpressions.Regex("<(\w+)\:([ATGCatgc]+)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regP As New System.Text.RegularExpressions.Regex("{([\w\d\s\-]+)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regPRC As New System.Text.RegularExpressions.Regex("<([\w\d\s\-]+)}", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regPrg As New System.Text.RegularExpressions.Regex("{([\w\d\s\-]+)\:(\d+)\-(\d+)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regPRCrg As New System.Text.RegularExpressions.Regex("<([\w\d\s\-]+):(\d+)\-(\d+)}", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regN As New System.Text.RegularExpressions.Regex("\w+", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regS As New System.Text.RegularExpressions.Regex("\s+", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        Dim regEN As New System.Text.RegularExpressions.Regex("[Nn]+", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regA As New System.Text.RegularExpressions.Regex("{([\w\s\-]+)}", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regAF As New System.Text.RegularExpressions.Regex("{([\w\s\-]+)/}", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regAR As New System.Text.RegularExpressions.Regex("{/([\w\s\-]+)}", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        Dim regFEnd As New System.Text.RegularExpressions.Regex("\s*\:([\^\*0]?)(B?)([35]?[ATGCatgc]*)\:", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regREnd As New System.Text.RegularExpressions.Regex("\:([\^\*0]?)(B?)([35]?[ATGCatgc]*)\:\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        Dim regCommentF As New System.Text.RegularExpressions.Regex("\\\*", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim regCommentR As New System.Text.RegularExpressions.Regex("\*\\", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        'Dim EnzList As New List(Of String)

        Dim FormalCode As New System.Text.StringBuilder

        Dim FindFEnd As Boolean = False
        Dim FindREnd As Boolean = False

        Dim i As Integer = 0
        Dim code As String = FreeDesignCode
        Dim m As System.Text.RegularExpressions.Match
        Dim stb As New System.Text.StringBuilder

        'If code.StartsWith("::") Then FormalCode.Append("::")
        'If code.StartsWith("0B") Then FormalCode.Append("0B")
        'If code.StartsWith("*B") Then FormalCode.Append("*B")
        'If code.StartsWith("^B") Then FormalCode.Append("^B")
        While i < code.Length
            If regN.IsMatch(code, i) AndAlso regN.Match(code, i).Index = i Then
                m = regN.Match(code, i)
                stb.Append(Nuctions.TAGCFilter(m.Captures(0).Value))
                FormalCode.Append(Nuctions.TAGCFilter(m.Captures(0).Value))
                i += m.Captures(0).Length
            ElseIf regE.IsMatch(code, i) AndAlso regE.Match(code, i).Index = i Then
                'only for panlindromic enzymes
                m = regE.Match(code, i)
                Dim EnzName As String = m.Groups(1).Value
                Dim EnzCode As String = Nuctions.TAGCFilter(m.Groups(2).Value)
                Dim EnzSeq As String = ""
                For Each key As Nuctions.RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
                    If EnzName.ToLower = key.Name.ToLower Then
                        EnzSeq = key.Sequence
                        Dim n As Integer
                        If regEN.IsMatch(EnzSeq) Then
                            n = regEN.Match(EnzSeq).Length
                            If EnzCode.Length < n Then EnzCode = EnzCode.PadRight(n, "A")
                            EnzSeq = regEN.Replace(EnzSeq, EnzCode.Substring(0, n))
                            FormalCode.Append(String.Format("[{0}:{1}>", key.Name, EnzCode.Substring(0, n)))
                        ElseIf EnzSeq = Nuctions.ReverseComplement(EnzSeq) Then
                            FormalCode.Append(String.Format("[{0}]", key.Name))
                        Else
                            FormalCode.Append(String.Format("[{0}>", key.Name))
                        End If
                        If Not (EnzList.Contains(key.Name)) Then EnzList.Add(key.Name)

                        Exit For
                    End If
                Next
                stb.Append(EnzSeq)

                i += m.Captures(0).Length
            ElseIf regEVNC.IsMatch(code, i) AndAlso regEVNC.Match(code, i).Index = i Then
                m = regEVNC.Match(code, i)
                Dim EnzName As String = m.Groups(1).Value
                Dim EnzCode As String = Nuctions.TAGCFilter(m.Groups(2).Value)
                Dim EnzSeq As String = ""
                For Each key As Nuctions.RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
                    If EnzName.ToLower = key.Name.ToLower Then
                        EnzSeq = key.Sequence
                        Dim n As Integer
                        If regEN.IsMatch(EnzSeq) Then
                            n = regEN.Match(EnzSeq).Length
                            If EnzCode.Length < n Then EnzCode = EnzCode.PadRight(n, "A")
                            EnzSeq = regEN.Replace(EnzSeq, EnzCode.Substring(0, n))
                            FormalCode.Append(String.Format("[{0}:{1}>", key.Name, EnzCode.Substring(0, n)))
                        Else
                            FormalCode.Append(String.Format("[{0}>", key.Name))
                        End If
                        If Not (EnzList.Contains(key.Name)) Then EnzList.Add(key.Name)

                        Exit For
                    End If
                Next
                stb.Append(EnzSeq)

                i += m.Captures(0).Length
            ElseIf regENC.IsMatch(code, i) AndAlso regENC.Match(code, i).Index = i Then
                m = regENC.Match(code, i)
                Dim EnzName As String = m.Groups(1).Value
                Dim EnzSeq As String = ""
                For Each key As Nuctions.RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
                    If EnzName.ToLower = key.Name.ToLower Then
                        EnzSeq = key.Sequence
                        If regEN.IsMatch(EnzSeq) Then
                            Dim n As Integer = regEN.Match(EnzSeq).Length
                            EnzSeq = regEN.Replace(EnzSeq, "".PadRight(n, "A"))
                            FormalCode.Append(String.Format("[{0}:{1}>", key.Name, "".PadRight(n, "A")))
                        Else
                            FormalCode.Append(String.Format("[{0}>", key.Name))
                        End If
                        If Not (EnzList.Contains(key.Name)) Then EnzList.Add(key.Name)
                        Exit For
                    End If
                Next
                stb.Append(EnzSeq)
                i += m.Captures(0).Length
            ElseIf regEVRC.IsMatch(code, i) AndAlso regEVRC.Match(code, i).Index = i Then
                m = regEVRC.Match(code, i)
                Dim EnzName As String = m.Groups(1).Value
                Dim EnzCode As String = Nuctions.TAGCFilter(m.Groups(2).Value)
                Dim EnzSeq As String = ""
                For Each key As Nuctions.RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
                    If EnzName.ToLower = key.Name.ToLower Then
                        EnzSeq = key.Sequence
                        Dim n As Integer
                        If regEN.IsMatch(EnzSeq) Then
                            n = regEN.Match(EnzSeq).Length
                            If EnzCode.Length < n Then EnzCode = EnzCode.PadRight(n, "A")
                            EnzSeq = regEN.Replace(EnzSeq, EnzCode.Substring(0, n))
                            FormalCode.Append(String.Format("<{0}:{1}]", key.Name, EnzCode.Substring(0, n)))
                        Else
                            FormalCode.Append(String.Format("<{0}]", key.Name))
                        End If
                        If Not (EnzList.Contains(key.Name)) Then EnzList.Add(key.Name)

                        Exit For
                    End If
                Next
                stb.Append(Nuctions.ReverseComplement(EnzSeq))
                i += m.Captures(0).Length
            ElseIf regERC.IsMatch(code, i) AndAlso regERC.Match(code, i).Index = i Then
                m = regERC.Match(code, i)
                Dim EnzName As String = m.Groups(1).Value
                Dim EnzSeq As String = ""
                For Each key As Nuctions.RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
                    If EnzName.ToLower = key.Name.ToLower Then
                        EnzSeq = key.Sequence
                        If regEN.IsMatch(EnzSeq) Then
                            Dim n As Integer = regEN.Match(EnzSeq).Length
                            EnzSeq = regEN.Replace(EnzSeq, "".PadRight(n, "A"))
                            FormalCode.Append(String.Format("<{0}:{1}]", key.Name, "".PadRight(n, "A")))
                        Else
                            FormalCode.Append(String.Format("<{0}]", key.Name))
                        End If
                        If Not (EnzList.Contains(key.Name)) Then EnzList.Add(key.Name)
                        Exit For
                    End If
                Next
                stb.Append(Nuctions.ReverseComplement(EnzSeq))
                i += m.Captures(0).Length
            ElseIf regP.IsMatch(code, i) AndAlso regP.Match(code, i).Index = i Then
                m = regP.Match(code, i)
                Dim featureName As String = m.Groups(1).Value
                For Each f As Nuctions.Feature In Features
                    If f.Label.ToLower = featureName.ToLower Then
                        stb.Append(f.Sequence.ToUpper)
                        FormalCode.Append("{" + f.Label + ">")
                        Exit For
                    End If
                Next
                i += m.Captures(0).Length
            ElseIf regPRC.IsMatch(code, i) AndAlso regPRC.Match(code, i).Index = i Then
                m = regPRC.Match(code, i)
                Dim featureName As String = m.Groups(1).Value
                For Each f As Nuctions.Feature In Features
                    If f.Label.ToLower = featureName.ToLower Then
                        stb.Append(f.RCSequence.ToUpper)
                        FormalCode.Append("<" + f.Label + "}")
                        Exit For
                    End If
                Next
                i += m.Captures(0).Length
            ElseIf regPrg.IsMatch(code, i) AndAlso regPrg.Match(code, i).Index = i Then
                m = regPrg.Match(code, i)
                Dim d1 As Integer = CInt(m.Groups(2).Value)
                Dim d2 As Integer = CInt(m.Groups(3).Value)
                Dim featureName As String = m.Groups(1).Value
                For Each f As Nuctions.Feature In Features
                    If f.Label.ToLower = featureName.ToLower Then
                        If d1 < 1 Then d1 = 1
                        If d1 > f.Sequence.Length Then d1 = f.Sequence.Length
                        If d2 >= d1 Then
                            If d2 > f.Sequence.Length Then d2 = f.Sequence.Length
                            stb.Append(f.Sequence.Substring(d1 - 1, d2 - d1 + 1).ToUpper)
                            FormalCode.Append("{" + f.Label + ":" + d1.ToString + "-" + d2.ToString + ">")
                        End If
                        Exit For
                    End If
                Next
                i += m.Captures(0).Length
            ElseIf regPRCrg.IsMatch(code, i) AndAlso regPRCrg.Match(code, i).Index = i Then
                m = regPRCrg.Match(code, i)
                Dim d1 As Integer = CInt(m.Groups(2).Value)
                Dim d2 As Integer = CInt(m.Groups(3).Value)
                Dim featureName As String = m.Groups(1).Value
                For Each f As Nuctions.Feature In Features
                    If f.Label.ToLower = featureName.ToLower Then
                        If d1 < 1 Then d1 = 1
                        If d1 > f.Sequence.Length Then d1 = f.Sequence.Length
                        If d2 >= d1 Then
                            If d2 > f.Sequence.Length Then d2 = f.Sequence.Length
                            stb.Append(Nuctions.ReverseComplement(f.Sequence.Substring(d1 - 1, d2 - d1 + 1)))
                            FormalCode.Append("<" + f.Label + ":" + d1.ToString + "-" + d2.ToString + "}")
                        End If
                        Exit For
                    End If
                Next
                i += m.Captures(0).Length

            ElseIf regA.IsMatch(code, i) AndAlso regA.Match(code, i).Index = i Then
                m = regA.Match(code, i)
                Dim aCode As String = m.Groups(1).Value
                Dim gc As Nuctions.GeneticCode = Nothing
                Dim AA As Nuctions.Codon
                FormalCode.Append("{")
                'Dim ac As String = AminoAcidFilter(aCode.ToUpper)
                'FormalCode.Append(ac)
                'stb.Append(CodonOptimize(ac, frmMain.CodonTraslation, frmMain.EnzymeCol, True, New List(Of String)))

                For j As Integer = 0 To aCode.Length - 1
                    If SettingEntry.CodonTraslation.AnimoTable.ContainsKey(aCode.Substring(j, 1).ToUpper) Then
                        AA = SettingEntry.CodonTraslation.AnimoTable(aCode.Substring(j, 1).ToUpper)
                        gc = AA.GetMaxRatioCode
                        stb.Append(gc.Name)
                        FormalCode.Append(AA.ShortName)
                    End If
                Next
                FormalCode.Append("/}")
                i += m.Captures(0).Length
            ElseIf regAF.IsMatch(code, i) AndAlso regAF.Match(code, i).Index = i Then
                m = regAF.Match(code, i)
                Dim aCode As String = m.Groups(1).Value
                Dim gc As Nuctions.GeneticCode = Nothing
                Dim AA As Nuctions.Codon
                FormalCode.Append("{")
                For j As Integer = 0 To aCode.Length - 1
                    If SettingEntry.CodonTraslation.AnimoTable.ContainsKey(aCode.Substring(j, 1).ToUpper) Then
                        AA = SettingEntry.CodonTraslation.AnimoTable(aCode.Substring(j, 1).ToUpper)
                        gc = AA.GetMaxRatioCode
                        stb.Append(gc.Name)
                        FormalCode.Append(AA.ShortName)
                    End If
                Next
                FormalCode.Append("/}")
                i += m.Captures(0).Length
            ElseIf regAR.IsMatch(code, i) AndAlso regAR.Match(code, i).Index = i Then
                m = regAR.Match(code, i)
                Dim aCode As String = m.Groups(1).Value
                Dim gc As Nuctions.GeneticCode = Nothing
                Dim AA As Nuctions.Codon
                FormalCode.Append("{/")
                For j As Integer = 0 To aCode.Length - 1
                    If SettingEntry.CodonTraslation.AnimoTable.ContainsKey(aCode.Substring(j, 1).ToUpper) Then
                        AA = SettingEntry.CodonTraslation.AnimoTable(aCode.Substring(j, 1).ToUpper)
                        FormalCode.Append(AA.ShortName)
                    End If
                Next
                For j As Integer = aCode.Length - 1 To 0 Step -1
                    If SettingEntry.CodonTraslation.AnimoTable.ContainsKey(aCode.Substring(j, 1).ToUpper) Then
                        AA = SettingEntry.CodonTraslation.AnimoTable(aCode.Substring(j, 1).ToUpper)
                        gc = AA.GetMaxRatioCode
                        stb.Append(Nuctions.ReverseComplement(gc.Name))
                        'FormalCode.Append(Nuctions.ReverseComplement(gc.Name))
                    End If
                Next
                FormalCode.Append("}")
                i += m.Captures(0).Length
            ElseIf regS.IsMatch(code, i) AndAlso regS.Match(code, i).Index = i Then
                m = regS.Match(code, i)
                FormalCode.Append(m.Value)
                i += m.Captures(0).Length
            ElseIf Not FindFEnd AndAlso regFEnd.IsMatch(code, i) AndAlso regFEnd.Match(code, i).Index = i Then
                m = regFEnd.Match(code, i)
                If m.Groups(1).Length > 0 Then
                    If m.Groups(2).Length > 0 Then
                        gf.End_F = m.Groups(1).Value + m.Groups(2).Value
                        FormalCode.Append(":")
                        FormalCode.Append(gf.End_F)
                        FormalCode.Append(":")
                        'i += m.Captures(0).Length
                    ElseIf m.Groups(3).Length > 0 Then
                        If m.Groups(3).Value.StartsWith("5") Then
                            stb.Append(Nuctions.TAGCFilter(m.Groups(3).Value))
                            gf.End_F = m.Groups(1).Value + "5" + Nuctions.TAGCFilter(m.Groups(3).Value)
                        ElseIf m.Groups(3).Value.StartsWith("3") Then
                            stb.Append(Nuctions.ReverseComplementFilter(m.Groups(3).Value))
                            gf.End_F = m.Groups(1).Value + "3" + Nuctions.TAGCFilter(m.Groups(3).Value)
                        Else
                            stb.Append(Nuctions.ReverseComplementFilter(m.Groups(3).Value))
                            gf.End_F = m.Groups(1).Value + "3" + Nuctions.TAGCFilter(m.Groups(3).Value)
                        End If
                        FormalCode.Append(":")
                        FormalCode.Append(gf.End_F)
                        FormalCode.Append(":")
                        'i += m.Captures(0).Length
                    Else
                        gf.End_F = m.Groups(1).Value + "B"
                        FormalCode.Append(":")
                        FormalCode.Append(gf.End_F)
                        FormalCode.Append(":")
                        'i += m.Captures(0).Length
                    End If
                Else
                    gf.End_F = "::"
                    FormalCode.Append("::")
                    'i += m.Captures(0).Length
                End If
                i += m.Captures(0).Length
                FindFEnd = True
            ElseIf Not FindREnd AndAlso regREnd.IsMatch(code, i) AndAlso regREnd.Match(code, i).Index = i Then
                m = regREnd.Match(code, i)
                If m.Groups(1).Length > 0 Then
                    If m.Groups(2).Length > 0 Then
                        gf.End_R = m.Groups(1).Value + m.Groups(2).Value
                        FormalCode.Append(":")
                        FormalCode.Append(gf.End_R)
                        FormalCode.Append(":")
                    ElseIf m.Groups(3).Length > 0 Then
                        If m.Groups(3).Value.StartsWith("3") Then
                            stb.Append(Nuctions.TAGCFilter(m.Groups(3).Value))
                            gf.End_R = m.Groups(1).Value + "3" + Nuctions.TAGCFilter(m.Groups(3).Value)
                        ElseIf m.Groups(3).Value.StartsWith("5") Then
                            stb.Append(Nuctions.ReverseComplementFilter(m.Groups(3).Value))
                            gf.End_R = m.Groups(1).Value + "5" + Nuctions.TAGCFilter(m.Groups(3).Value)
                        Else
                            stb.Append(Nuctions.TAGCFilter(m.Groups(3).Value))
                            gf.End_R = m.Groups(1).Value + "3" + Nuctions.TAGCFilter(m.Groups(3).Value)
                        End If
                        FormalCode.Append(":")
                        FormalCode.Append(gf.End_R)
                        FormalCode.Append(":")
                    Else
                        gf.End_R = m.Groups(1).Value + "B"
                        FormalCode.Append(":")
                        FormalCode.Append(gf.End_R)
                        FormalCode.Append(":")
                    End If
                Else
                    gf.End_R = "::"
                    FormalCode.Append("::")
                End If
                FindREnd = True
                i += m.Captures(0).Length
            ElseIf regCommentF.IsMatch(code, i) AndAlso regCommentF.Match(code, i).Index = i Then
                m = regCommentF.Match(code, i)
                Dim start As Integer = m.Index
                If regCommentR.IsMatch(code, i) Then
                    m = regCommentR.Match(code, i)
                    Dim count As Integer = m.Index + m.Length - start
                    FormalCode.Append(code, start, count)
                    i += count
                Else
                    i += m.Length
                End If
            Else
                i += 1
            End If
        End While
        'If code.EndsWith("::") Then FormalCode.Append("::")
        'If code.EndsWith("0B") Then FormalCode.Append("0B")
        'If code.EndsWith("*B") Then FormalCode.Append("*B")
        'If code.EndsWith("^B") Then FormalCode.Append("^B")
        If gf.End_F = "" Then
            gf.End_F = "*B"
        End If
        If gf.End_R = "" Then
            gf.End_R = "*B"
        End If
        gf.Sequence = stb.ToString
        If gf.End_F = "::" And gf.End_R <> "::" Then gf.End_F = "*B"
        If gf.End_F <> "::" And gf.End_R = "::" Then gf.End_R = "*B"
        'If code.StartsWith("::") AndAlso code.EndsWith("::") Then gf.End_F = "::" : gf.End_R = "::"

        DNAs.Clear()
        If gf.Length > 0 Then DNAs.Add(gf)
        Nuctions.AddFeatures(DNAs, Features)
        FreeDesignCode = FormalCode.ToString
        'Dim EnzCol As New List(Of String)
        'EnzCol.AddRange(GetParetntChartItem.Parent.EnzymeCol)
        'DesignedEnzymeSite.Clear()
        'DesignedEnzymeSite.AddRange(EnzList)
        'For Each ez As String In EnzList
        '    If Not (EnzCol.Contains(ez)) Then EnzCol.Add(ez)
        'Next
    End Sub

End Class
