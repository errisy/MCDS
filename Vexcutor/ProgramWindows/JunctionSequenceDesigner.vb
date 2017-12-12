Public Class JunctionSequenceDesigner
 
    Private GenCon As GeneratorContext = Nothing
    Private FileLocation As String = ""
    Public Sub LoadListToData(ByVal vList As List(Of MerPair))
        Dim row As DataGridViewRow
        For Each mp As MerPair In vList
            row = dgvMain.Rows(dgvMain.Rows.Add)
            row.Cells(0).Value = mp.ID
            row.Cells(1).Value = mp.Sequence
            row.Cells(2).Value = mp.F
            row.Cells(3).Value = mp.R
            row.Cells(4).Value = mp.FH
            row.Cells(5).Value = mp.RH
            row.Cells(6).Value = mp.FC
            row.Cells(7).Value = mp.RC
            row.Cells(8).Value = mp.FCIS
            row.Cells(9).Value = mp.RCIS
        Next
    End Sub
    Public Function Start(ByVal Count As Integer) As List(Of MerPair)
        If GenCon Is Nothing Then
            Dim ofd As New OpenFileDialog
            ofd.Title = "Load Juction Sequence Designer File"
            ofd.Filter = "Juction Sequence Designer File|*.jsd"
            If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
                FileLocation = ofd.FileName
                LoadFile()
                LoadListToData(GenCon.MerPairList)
            Else
                GenCon = New GeneratorContext
            End If
            Used12Mer = GenCon.Used12Mer
            Forbid12Mer = GenCon.Forbid12Mer
            UnitA = GenCon.UnitA
            UnitB = GenCon.UnitB
            If Not GenCon.Started Then
                GenerateCode()
                Optimize()
                Collect()
                GenCon.Started = True
            Else

            End If
        End If

        Dim mp As MerPair
        Dim mpl As New List(Of MerPair)
        If Count = 0 Then Return mpl
        While Count > 0
            mp = GeneratePair(Seed, 80, 50, GenCon.MerPairList.Count)
            GenCon.MerPairList.Add(mp)
            mpl.Add(mp)
            Count -= 1
        End While
        LoadListToData(mpl)
        SaveFile()
        Return mpl
    End Function
    Public Sub LoadFile()
        Dim BD As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        GenCon = BD.Deserialize(New IO.FileStream(FileLocation, IO.FileMode.Open))
    End Sub
    Public Sub SaveFile()
        If FileLocation Is Nothing OrElse FileLocation.Length = 0 OrElse (Not IO.File.Exists(FileLocation)) Then
            Dim sfd As New SaveFileDialog
            sfd.Title = "Save Juction Sequence Designer File"
            sfd.Filter = "Juction Sequence Designer File|*.jsd"
            If sfd.ShowDialog = Windows.Forms.DialogResult.OK Then
                FileLocation = sfd.FileName
            End If
        End If
        If FileLocation Is Nothing OrElse FileLocation.Length = 0 OrElse (Not IO.File.Exists(FileLocation)) Then
        Else

            Dim BD As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            BD.Serialize(New IO.FileStream(FileLocation, IO.FileMode.OpenOrCreate), GenCon)
        End If

    End Sub

    <Serializable()> Public Class GeneratorContext
        Public Started As Boolean = False

        Public Used12Mer As New List(Of String)
        Public Forbid12Mer As New List(Of String)

        Public UnitA As New Dictionary(Of String, Coder)
        Public UnitB As New Dictionary(Of String, Coder)

        Public MerPairList As New List(Of MerPair)
    End Class
#Region "GenerateSequences"
    Public Seed As String = "TAGGG"
    Public Shared SalI As String = "GTCGAC"
    Public Shared KpnI As String = "GGTACC"
    Public EcoRV As String = "GATATC"
    Public BsaIF As String = "GGTCTC"
    Public BsaIR As String = "GAGACC"
    Public Shared BsaIFPm As String = "CGCCTGAGACC"
    Public Shared BsaIRPm As String = "GGCGTGAGACC"
    Public Shared ISceI As String = "ATTACCCTGTTAT>CCCTA"
    Public DraIII As New System.Text.RegularExpressions.Regex("CAC[GTCA][GTCA][GTCA]GTG", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    <Serializable()> Public Class MerPair
        Public ID As Integer
        Public F As String = ""
        Public R As String = ""
        Public FCIS As String = ""
        Public RCIS As String = ""
        Public FC As String = ""
        Public RC As String = ""
        Public FH As String = ""
        Public RH As String = ""
        Public Sequence As String = ""
        Public Sub CreatePrimers(ByVal vBefore As Integer, ByVal vAfter As Integer)
            Sequence = ReverseComplement(F) + "GATATCACCGGGTG" + "ACAATGAGA" + "CACGCGGTGATATC" + R
            FCIS = "ATCG" + SalI + ISceI + ReverseComplement(F).Substring(vAfter + 5, 17)
            RCIS = "ATCG" + KpnI + ISceI + ReverseComplement(R).Substring(vAfter + 5, 17)
            FH = ">" + F.Substring(20, 22)
            RH = ">" + R.Substring(20, 22)
            FC = ReverseComplement(BsaIFPm) + ">" + ReverseComplement(F).Substring(0, 22)
            RC = ReverseComplement(BsaIRPm) + ">" + ReverseComplement(R).Substring(0, 22)
        End Sub
    End Class
    Public Function GeneratePair(ByVal vSeed As String, ByVal vBefore As Integer, ByVal vAfter As Integer, ByVal vID As Integer) As MerPair
        Dim UF As String
        Do
            UF = Seed.ToUpper
            ExtendAfter(UF, vAfter)
            ExtendBefore(UF, vBefore)
        Loop While UF.Length < vSeed.Length + vBefore + vAfter
        Dim UR As String
        Do
            UR = Seed.ToUpper
            ExtendAfter(UR, vAfter)
            ExtendBefore(UR, vBefore)
        Loop While UR.Length < vSeed.Length + vBefore + vAfter
        Dim MP As New MerPair With {.ID = vID, .F = UF, .R = UR}
        MP.CreatePrimers(vBefore, vAfter)
        Return MP
    End Function
    Public Function ExtendBefore(ByRef Mer As String, ByVal Countdown As Integer) As Boolean

        If Countdown = 0 Then Return True
        Dim success As Boolean = False

        Dim curMer As String = Mer.ToUpper
        Dim CanGrow As Boolean = True
        Dim curCode As Coder
        Dim oi As OligoInfo
        Dim Mer12 As String
        While CanGrow
            curCode = UnitA(First3(Mer))
            CanGrow = False
            For i As Integer = 0 To curCode.Before.Count - 1
                Mer = First1(curCode.Before(i)) + Mer

                If PassLocalCondition(Mer) Then 'avoid enzyme sites

                    If GCCount(First6(Mer)) < 6 Then 'avoid full GC sequence
                        If Mer.Length >= 12 Then 'Check long mers that should avoid
                            Mer12 = First12(Mer)

                            If Used12Mer.Contains(Mer12) Or Forbid12Mer.Contains(Mer12) Then
                                Mer = curMer.ToUpper
                            Else
                                oi = CalculateTm(Mer12, 0.08, 0.000000625)
                                If oi.Tm > 30 Then
                                    Forbid12Mer.Add(Mer12)
                                    Mer = curMer.ToUpper
                                Else
                                    Used12Mer.Add(Mer12)
                                    If ExtendBefore(Mer, Countdown - 1) Then
                                        CanGrow = False
                                        success = True
                                        Exit For
                                    Else
                                        Used12Mer.Remove(Mer12)
                                        Mer = curMer.ToUpper
                                    End If
                                End If
                            End If
                        Else
                            If ExtendBefore(Mer, Countdown - 1) Then
                                CanGrow = False
                                success = True
                                Exit For
                            Else
                                Mer = curMer.ToUpper
                            End If
                        End If
                    Else
                        Mer = curMer.ToUpper
                    End If
                Else
                    Mer = curMer.ToUpper
                End If
            Next
        End While
        Return success
    End Function
    Public Function ExtendAfter(ByRef Mer As String, ByVal Countdown As Integer) As Boolean

        If Countdown = 0 Then Return True
        Dim success As Boolean = False

        Dim curMer As String = Mer.ToUpper
        Dim CanGrow As Boolean = True
        Dim curCode As Coder
        Dim oi As OligoInfo
        Dim Mer12 As String
        While CanGrow
            curCode = UnitA(Last3(Mer))
            CanGrow = False
            For i As Integer = 0 To curCode.After.Count - 1
                Mer = Mer + Last1(curCode.After(i))

                If PassLocalCondition(Mer) Then 'avoid enzyme sites

                    If GCCount(Last6(Mer)) < 6 Then 'avoid full GC sequence
                        If Mer.Length >= 12 Then 'Check long mers that should avoid
                            Mer12 = Last12(Mer)

                            If Used12Mer.Contains(Mer12) Or Forbid12Mer.Contains(Mer12) Then
                                Mer = curMer.ToUpper
                            Else
                                oi = CalculateTm(Mer12, 0.08, 0.000000625)
                                If oi.Tm > 30 Then
                                    Forbid12Mer.Add(Mer12)
                                    Mer = curMer.ToUpper
                                Else
                                    Used12Mer.Add(Mer12)
                                    If ExtendAfter(Mer, Countdown - 1) Then
                                        CanGrow = False
                                        success = True
                                        Exit For
                                    Else
                                        Used12Mer.Remove(Mer12)
                                        Mer = curMer.ToUpper
                                    End If
                                End If
                            End If
                        Else
                            If ExtendAfter(Mer, Countdown - 1) Then
                                CanGrow = False
                                success = True
                                Exit For
                            Else
                                Mer = curMer.ToUpper
                            End If
                        End If
                    Else
                        Mer = curMer.ToUpper
                    End If
                Else
                    Mer = curMer.ToUpper
                End If
            Next
        End While
        Return success
    End Function
    Public Function PassLocalCondition(ByVal vMer As String) As Boolean
        Return Not (vMer.Contains(SalI) OrElse vMer.Contains(KpnI) OrElse vMer.Contains(EcoRV) OrElse vMer.Contains(BsaIF) OrElse vMer.Contains(BsaIR) OrElse DraIII.IsMatch(vMer)) 'vMer.Contains(SalI) OrElse vMer.Contains(KpnI) OrElse vMer.Contains(EcoRV) OrElse
    End Function
    Public Function GCCount(ByVal v6Mer As String) As Integer
        Dim v As Integer = 0
        For i As Integer = 0 To 5
            If v6Mer(i) = "G" Or v6Mer(i) = "C" Then v += 1
        Next
        Return v
    End Function
#End Region

#Region "CreateSubCodeGroup"


    <Serializable()> Public Class Coder
        Public Code As String
        Public After As New List(Of String)
        Public Before As New List(Of String)
        Public Score As Integer
        Public Sub New()

        End Sub
        Public Sub New(ByVal vCode)
            Code = vCode
        End Sub
    End Class

    Public ListA As New List(Of String)
    Public ListB As New List(Of String)

    Public GroupA As New List(Of Coder)
    Public GroupB As New List(Of Coder)

    Public UnitA As New Dictionary(Of String, Coder)
    Public UnitB As New Dictionary(Of String, Coder)


    Public Sub GenerateCode()
        Dim codes As New List(Of String) From {"G", "T", "C", "A"}
        Dim c As String = ""
        Dim rc As String = ""
        For i1 As Integer = 0 To 3
            For i2 As Integer = 0 To 3
                For i3 As Integer = 0 To 3
                    c = codes(i1) + codes(i2) + codes(i3)
                    rc = ReverseComplement(c)
                    If Not (ListA.Contains(c) OrElse ListA.Contains(rc) OrElse ListB.Contains(c) OrElse ListB.Contains(rc) OrElse Last2(c) = First2(rc)) Then
                        ListA.Add(c)
                        ListB.Add(rc)
                        GroupA.Add(New Coder(c))
                        GroupB.Add(New Coder(rc))
                    End If
                Next
            Next
        Next
    End Sub

    Public Function CalculateScore() As Integer
        Dim Overall As Integer = 0
        For Each c As Coder In GroupA
            c.Score = 0
            For Each c2 As Coder In GroupA
                If First2(c2.Code) = Last2(c.Code) Then c.Score += 1 : Overall += 1
            Next
        Next
        For Each c As Coder In GroupB
            c.Score = 0
            For Each c2 As Coder In GroupB
                If First2(c2.Code) = Last2(c.Code) Then c.Score += 1 : Overall += 1
            Next
        Next
        Return Overall
    End Function

    Public Sub Optimize()
        Dim curScore As Integer
        Dim newScore As Integer
        curScore = CalculateScore()
        Dim CanReduce As Boolean = True
        While CanReduce
            CanReduce = False
            For i As Integer = GroupA.Count - 1 To 0 Step -1
                newScore = Exchange(i)
                If newScore > curScore Then
                    curScore = newScore
                    CanReduce = True
                    Exit For
                Else
                    Exchange(i)
                    CanReduce = CanReduce Or False
                End If
            Next
        End While
    End Sub

    Public Function Exchange(ByVal i As Integer) As Integer
        Dim cA As New Coder
        Dim cB As New Coder
        cA.Code = GroupA(i).Code
        cB.Code = GroupB(i).Code
        GroupA(i).Code = cB.Code
        GroupB(i).Code = cA.Code
        Return CalculateScore()
    End Function

    Public Sub Collect()
        For Each c As Coder In GroupA
            UnitA.Add(c.Code, c)
            For Each c2 As Coder In GroupA
                If First2(c2.Code) = Last2(c.Code) Then
                    c.After.Add(c2.Code)
                End If
                If Last2(c2.Code) = First2(c.Code) Then
                    c.Before.Add(c2.Code)
                End If
            Next
        Next
        For Each c As Coder In GroupB
            UnitB.Add(c.Code, c)
            For Each c2 As Coder In GroupA
                If First2(c2.Code) = Last2(c.Code) Then
                    c.After.Add(c2.Code)
                End If
                If Last2(c2.Code) = First2(c.Code) Then
                    c.Before.Add(c2.Code)
                End If
            Next
        Next
    End Sub
#End Region

#Region "Functions"
    Public Function ListToString(ByVal vList As List(Of String)) As String
        Dim stb As New System.Text.StringBuilder
        stb.Append("{")
        For Each s As String In vList
            stb.Append(s)
            stb.Append(" ")
        Next
        stb.Append("}")
        Return stb.ToString
    End Function
    Public Used12Mer As New List(Of String)
    Public Forbid12Mer As New List(Of String)
    Public Function Last1(ByVal v As String) As String
        Return v.Substring(v.Length - 1, 1)
    End Function
    Public Function First1(ByVal v As String) As String
        Return v.Substring(0, 1)
    End Function
    Public Function Last2(ByVal v As String) As String
        Return v.Substring(v.Length - 2, 2)
    End Function
    Public Function First2(ByVal v As String) As String
        Return v.Substring(0, 2)
    End Function
    Public Function Last3(ByVal v As String) As String
        Return v.Substring(v.Length - 3, 3)
    End Function
    Public Function First3(ByVal v As String) As String
        Return v.Substring(0, 3)
    End Function
    Public Function Last6(ByVal v As String) As String
        Return v.Substring(v.Length - 6, 6)
    End Function
    Public Function First6(ByVal v As String) As String
        Return v.Substring(0, 6)
    End Function
    Public Function Last12(ByVal v As String) As String
        Return v.Substring(v.Length - 12, 12)
    End Function
    Public Function First12(ByVal v As String) As String
        Return v.Substring(0, 12)
    End Function

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

    Public Shared Function CalculateTm(ByVal Oligo As String, ByVal Na As Single, ByVal C As Single) As OligoInfo
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

        'Therm. Tm = dH°- 273.15 + 16.6(log[Na+]) dS° + dSo° + R(ln(c/4))
        Return New OligoInfo(H / (S + S0 + R * Math.Log(C / 4)) - 273.15 + 16.6 * Math.Log10(Na), G)
    End Function

    Public Shared Function TAGCFilter(ByVal Value As String) As String
        If Value Is Nothing Then Return ""
        Dim data As String = Value.ToUpper
        Dim seq As String = ""
        Dim i As Integer
        For i = 0 To data.Length - 1
            seq &= IIf(data.Chars(i) = "A" Or data.Chars(i) = "T" Or data.Chars(i) = "G" Or data.Chars(i) = "C", data.Chars(i), "")
        Next
        Return seq
    End Function

    Public Shared Function ReverseComplement(ByVal text As String) As String
        text = text.ToUpper
        Dim a As New System.Text.StringBuilder

        Dim c As Char
        Dim i As Integer
        For i = text.Length - 1 To 0 Step -1
            c = text.Chars(i)
            a.Append(IIf(c = "T", "A", IIf(c = "A", "T", IIf(c = "G", "C", IIf(c = "C", "G", "-")))))
        Next
        Return a.ToString
    End Function
#End Region

    Friend SWC As WorkControl

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        Dim mpl As List(Of MerPair) = Start(nudCount.Value)
        Dim Seq As ChartItem
        Dim SCE As ChartItem
        Dim FHE As ChartItem
        Dim RHE As ChartItem
        Dim ft As Nuctions.Feature
        For Each mp As MerPair In mpl
            ft = New Nuctions.Feature("F1" + mp.ID.ToString, mp.F.Substring(20, 60), "F1 Junction Sequence" + mp.ID.ToString, "recombination_site", "", "Local")
            SWC.FeatureCol.Add(ft)
            ft = New Nuctions.Feature("FVP1" + mp.ID.ToString, mp.F.Substring(0, 20), "F1 Vector Protector" + mp.ID.ToString, "loci", "", "Local")
            SWC.FeatureCol.Add(ft)
            ft = New Nuctions.Feature("FDSP1" + mp.ID.ToString, mp.F.Substring(80, 50), "F1 Double Strand Protector" + mp.ID.ToString, "loci", "", "Local")
            SWC.FeatureCol.Add(ft)
            ft = New Nuctions.Feature("R1" + mp.ID.ToString, mp.R.Substring(20, 60), "R1 Junction Sequence" + mp.ID.ToString, "recombination_site", "", "Local")
            SWC.FeatureCol.Add(ft)
            ft = New Nuctions.Feature("RVP1" + mp.ID.ToString, mp.R.Substring(0, 20), "R1 Vector Protector" + mp.ID.ToString, "loci", "", "Local")
            SWC.FeatureCol.Add(ft)
            ft = New Nuctions.Feature("RDSP1" + mp.ID.ToString, mp.R.Substring(80, 50), "R1 Double Strand Protector" + mp.ID.ToString, "loci", "", "Local")
            SWC.FeatureCol.Add(ft)
            Seq = SWC.AddNewOperation(Nuctions.MolecularOperationEnum.FreeDesign, True)
            Seq.MolecularInfo.FreeDesignName = "JSD" + mp.ID.ToString
            Seq.MolecularInfo.FreeDesignCode = mp.Sequence
            Seq.MolecularInfo.Calculate()
            Seq.Reload(Seq.MolecularInfo, SWC.EnzymeCol)
            Seq.Selected = True
            Application.DoEvents()
            SCE = SWC.AddNewOperation(Nuctions.MolecularOperationEnum.PCR, True)
            SCE.MolecularInfo.PCR_FPrimerName = "JSD" + mp.ID.ToString + "FIS"
            SCE.MolecularInfo.PCR_RPrimerName = "JSD" + mp.ID.ToString + "RIS"
            SCE.MolecularInfo.PCR_ForwardPrimer = mp.FCIS
            SCE.MolecularInfo.PCR_ReversePrimer = mp.RCIS
            SCE.MolecularInfo.Calculate()
            SCE.Reload(SCE.MolecularInfo, SWC.EnzymeCol)
            SCE.Selected = False
            SWC.lv_Chart.SelectedItems.Clear()
            Application.DoEvents()
            Seq.Selected = True
            Application.DoEvents()
            FHE = SWC.AddNewOperation(Nuctions.MolecularOperationEnum.PCR, True)
            FHE.MolecularInfo.PCR_FPrimerName = "JSD" + mp.ID.ToString + "FH"
            FHE.MolecularInfo.PCR_RPrimerName = "JSD" + mp.ID.ToString + "FC"
            FHE.MolecularInfo.PCR_ForwardPrimer = mp.FH
            FHE.MolecularInfo.PCR_ReversePrimer = mp.FC
            FHE.MolecularInfo.Calculate()
            FHE.Reload(FHE.MolecularInfo, SWC.EnzymeCol)
            FHE.Selected = False
            SWC.lv_Chart.SelectedItems.Clear()
            Application.DoEvents()
            Seq.Selected = True
            Application.DoEvents()
            RHE = SWC.AddNewOperation(Nuctions.MolecularOperationEnum.PCR, True)
            RHE.MolecularInfo.PCR_FPrimerName = "JSD" + mp.ID.ToString + "RH"
            RHE.MolecularInfo.PCR_RPrimerName = "JSD" + mp.ID.ToString + "RC"
            RHE.MolecularInfo.PCR_ForwardPrimer = mp.RH
            RHE.MolecularInfo.PCR_ReversePrimer = mp.RC
            RHE.MolecularInfo.Calculate()
            RHE.Reload(RHE.MolecularInfo, SWC.EnzymeCol)
            RHE.Selected = False
            SWC.lv_Chart.SelectedItems.Clear()
            Application.DoEvents()
        Next
        SWC.lv_Chart.AutoArragne()
    End Sub
    Friend WriteOnly Property RelatedWorkControl As WorkControl
        Set(ByVal value As WorkControl)
            SWC = value
        End Set
    End Property
End Class