
Public Class ChartItem
    'Private vVectorMap As Bitmap
    Private Shared vFont As Font = New Font("Arial", 10)
    Private Shared tFont As Font = New Font("Calibri", 16, FontStyle.Bold)
    Private title As String
    Private descr As String = ""
    Private vVector As VectorMap
    Private sList As New List(Of String)
    Private Screening As New List(Of String)
    Private vIndex As Integer

    Public Sub New()
        AddHandler MolecularInfo.RequireParentChartItem, AddressOf GetParent
    End Sub

    '<XmlDescribe(True)> 
    <System.ComponentModel.Bindable(True)> Public WithEvents MolecularInfo As New DNAInfo

    Public Event RequireParent(ByVal sender As Object, ByVal e As RequireParentEventArgs)

    Private Sub MolecularInfo_RequireParentChartItem(ByVal sender As Object, ByVal e As GetChartItemEventArgs) Handles MolecularInfo.RequireParentChartItem
        e.ParentChartItem = Me
    End Sub

    Private ReadOnly Property MapStringLength() As Integer
        Get
            If Parent Is Nothing Then
                Return 1
            Else
                Return Parent.Items.Count.ToString.Length
            End If
        End Get
    End Property

    Public ReadOnly Property Parent() As OperationView
        Get
            Dim rpe As New RequireParentEventArgs
            RaiseEvent RequireParent(Me, rpe)
            Return rpe.Parent
        End Get
    End Property

    Public ReadOnly Property AllMolecularInfo() As List(Of DNAInfo)
        Get
            Dim lv As OperationView = Me.Parent
            Dim mis As New List(Of DNAInfo)
            If Not (lv Is Nothing) Then
                For Each ci As ChartItem In lv.Items
                    mis.Add(ci.MolecularInfo)
                Next
            End If
            Return mis
        End Get
    End Property

    Public ReadOnly Property AllGeneFiles() As List(Of Nuctions.GeneFile)
        Get
            Dim gfList As New List(Of Nuctions.GeneFile)
            For Each gf In MolecularInfo.DNAs
                gfList.Add(gf)
            Next
            For Each cell In MolecularInfo.Cells
                For Each gf In cell.DNAs
                    gfList.Add(gf)
                Next
            Next
            Return gfList
        End Get
    End Property

    Public Sub ResetEnzyme(ByVal Enzymes As List(Of String))
        If MolecularInfo.NotDrawMap Then vVector = Nothing : Exit Sub
        Select Case MolecularInfo.MolecularOperation
            Case Nuctions.MolecularOperationEnum.SequencingResult
                vVector = New VectorMap
                vVector.Width = GetVectorWidth(MolecularInfo, MolecularInfo.SequencingTheorica)
                vVector.RestrictionSite = GetRestrictionEnzyme(MolecularInfo, Enzymes)
                vVector.GeneFile = MolecularInfo.SequencingTheorica
            Case Nuctions.MolecularOperationEnum.Compare
                If MolecularInfo.DNAs.Count > 0 Then
                    vVector = New VectorMap
                    vVector.Width = GetVectorWidth(MolecularInfo, MolecularInfo.DNAs(1))
                    vVector.RestrictionSite = GetRestrictionEnzyme(MolecularInfo, Enzymes)
                    vVector.GeneFile = MolecularInfo.DNAs(1)
                End If

            Case Nuctions.MolecularOperationEnum.Host, Nuctions.MolecularOperationEnum.Incubation
                If MolecularInfo.Cells.Count = 1 AndAlso MolecularInfo.Cells(0).DNAs.Count = 1 Then
                    vVector = New VectorMap
                    vVector.Width = GetVectorWidth(MolecularInfo, MolecularInfo.Cells(0).DNAs(0))
                    vVector.RestrictionSite = GetRestrictionEnzyme(MolecularInfo, Enzymes)
                    vVector.GeneFile = MolecularInfo.Cells(0).DNAs(0)
                End If
            Case Else
                If MolecularInfo.Cells.Count = 1 AndAlso MolecularInfo.Cells(0).DNAs.Count = 1 Then
                    vVector = New VectorMap
                    vVector.Width = GetVectorWidth(MolecularInfo, MolecularInfo.Cells(0).DNAs(0))
                    vVector.RestrictionSite = GetRestrictionEnzyme(MolecularInfo, Enzymes)
                    vVector.GeneFile = MolecularInfo.Cells(0).DNAs(0)
                ElseIf MolecularInfo.DNAs.Count = 1 Then
                    vVector = New VectorMap
                    vVector.Width = GetVectorWidth(MolecularInfo, MolecularInfo.DNAs(1))
                    vVector.RestrictionSite = GetRestrictionEnzyme(MolecularInfo, Enzymes)
                    vVector.GeneFile = MolecularInfo.DNAs(1)
                Else
                    vVector = Nothing
                End If
        End Select
    End Sub

    Private Function GetVectorWidth(ByVal di As DNAInfo, gf As Nuctions.GeneFile) As Integer
        If di.PixelPerKBP > 0 Then
            'Dim gf As Nuctions.GeneFile = di.DNAs(1)
            If gf.Iscircular Then
                Return gf.Length / Math.PI / 1000 * di.PixelPerKBP
            Else
                Return gf.Length / 1000 * di.PixelPerKBP
            End If
        ElseIf di.RealSize Then
            'Dim gf As Nuctions.GeneFile = di.DNAs(1)
            If gf.Iscircular Then
                If gf.Length > 1800 * Math.PI Then
                    Return gf.Length / Math.PI / 10 + 120
                End If
            Else
                If gf.Length > 1800 Then
                    Return gf.Length / 10 + 120
                End If
            End If
        Else
            Return 300
        End If
    End Function
    Private Function GetRestrictionEnzyme(ByVal di As DNAInfo, ByVal Enzymes As List(Of String)) As List(Of String)
        Dim enzCol As New List(Of String)
        enzCol.AddRange(Enzymes)
        If di.MolecularOperation = Nuctions.MolecularOperationEnum.FreeDesign Then
            For Each ez As String In di.DesignedEnzymeSite
                If Not enzCol.Contains(ez) Then enzCol.Add(ez)
            Next
        ElseIf di.MolecularOperation = Nuctions.MolecularOperationEnum.EnzymeAnalysis Then
            For Each ez As String In di.FetchedEnzymes
                If Not enzCol.Contains(ez) Then enzCol.Add(ez)
            Next
        End If
        Return enzCol
    End Function

    Public ReadOnly Property Index() As Integer
        Get
            Return vIndex
        End Get
    End Property
    Public Sub SetIndex(ByVal vID As Integer)
        vIndex = vID
    End Sub

    Public Sub GetParent(ByVal sender As Object, ByVal e As GetChartItemEventArgs)
        e.ParentChartItem = Me
    End Sub

    Private _Enzymes As New List(Of String)
    Public ReadOnly Property DisplayedEnzymes As List(Of String)
        Get
            Return _Enzymes
        End Get
    End Property
    Public Sub Reload(ByVal di As DNAInfo, ByVal Enzymes As IEnumerable(Of String))
#If Remote = 1 Then
        MsgBox("Before Reload", MsgBoxStyle.OkOnly)
#End If
        If Not MolecularInfo Is Nothing Then
            RemoveHandler MolecularInfo.RequireParentChartItem, AddressOf GetParent
        End If
        MolecularInfo = di

        _Enzymes = Enzymes
        ResetEnzyme(Enzymes)
        'Select Case di.MolecularOperation
        '    Case Nuctions.MolecularOperationEnum.SequencingResult
        '        vVector = New VectorMap
        '        vVector.Width = GetVectorWidth(di, di.SequencingTheorica)
        '        vVector.RestrictionSite = GetRestrictionEnzyme(di, Enzymes)
        '        vVector.GeneFile = di.SequencingTheorica
        '    Case Nuctions.MolecularOperationEnum.Compare
        '        If di.DNAs.Count >= 1 Then
        '            vVector = New VectorMap
        '            vVector.Width = GetVectorWidth(di, di.DNAs(1))
        '            vVector.RestrictionSite = GetRestrictionEnzyme(di, Enzymes)
        '            vVector.GeneFile = di.DNAs(1)
        '        End If
        '    Case Nuctions.MolecularOperationEnum.Host, Nuctions.MolecularOperationEnum.Incubation
        '        If di.Cells.Count = 1 AndAlso di.Cells(0).DNAs.Count = 1 Then
        '            vVector = New VectorMap
        '            vVector.Width = GetVectorWidth(di, di.Cells(0).DNAs(0))
        '            vVector.RestrictionSite = GetRestrictionEnzyme(di, Enzymes)
        '            vVector.GeneFile = di.Cells(0).DNAs(0)
        '        End If
        '    Case Else
        '        If di.Cells.Count = 1 AndAlso di.Cells(0).DNAs.Count = 1 Then
        '            vVector = New VectorMap
        '            vVector.Width = GetVectorWidth(di, di.Cells(0).DNAs(0))
        '            vVector.RestrictionSite = GetRestrictionEnzyme(di, Enzymes)
        '            vVector.GeneFile = di.Cells(0).DNAs(0)
        '        ElseIf di.DNAs.Count = 1 Then
        '            vVector = New VectorMap
        '            vVector.Width = GetVectorWidth(di, di.DNAs(1))
        '            vVector.RestrictionSite = GetRestrictionEnzyme(di, Enzymes)
        '            vVector.GeneFile = di.DNAs(1)
        '        Else
        '            vVector = Nothing
        '        End If
        'End Select

#If Remote = 1 Then
        MsgBox("DNA Map Created", MsgBoxStyle.OkOnly)
#End If
        X = di.DX
        Y = di.DY

        sList.Clear()
        Screening.Clear()

        AddHandler MolecularInfo.RequireParentChartItem, AddressOf GetParent

        'set up the DNA compare for the view.
        'If di.MolecularOperation = Nuctions.MolecularOperationEnum.Compare Then di.Calculate()


        Dim stb As System.Text.StringBuilder

        Select Case MolecularInfo.MolecularOperation
            Case Nuctions.MolecularOperationEnum.Vector
                title = MolecularInfo.Name
            Case Nuctions.MolecularOperationEnum.Enzyme

                If MolecularInfo.DephosphorylateWhenDigestion Then
                    title = "Digestion & Dephosphorylation"
                Else
                    title = "Digestion"
                End If
                stb = New System.Text.StringBuilder
                For Each s As String In MolecularInfo.Enzyme_Enzymes
                    stb.Append(s)
                    stb.Append(" ")
                Next
                If MolecularInfo.DephosphorylateWhenDigestion Then stb.Append("CIAP ")
                'stb.AppendLine()
                sList.Add(stb.ToString)
                sList.Add("Possible Buffers: ")
                'Dim bi As Integer = 0
                'For Each bc As BufferCondition In MolecularInfo.Enzyme_Buffers
                '    stb.Append(String.Format("{0}{1} {2}; ", bc.Name, bc.Addtional, IIf(bc.B50_100 = 0, " =100%", " >50%")))
                '    bi += 1
                '    If bi > 3 Then Exit For
                'Next
                For Each cd As String In MolecularInfo.Enzyme_Condition
                    sList.Add(cd)
                Next
            'sList.Add(stb.ToString)
            Case Nuctions.MolecularOperationEnum.Gel
                If MolecularInfo.SolutionExtration Then
                    title = "Solution Extraction"
                Else
                    title = "Gel Extraction"
                    stb = New System.Text.StringBuilder
                    stb.Append("From ")
                    stb.Append(MolecularInfo.Gel_Minimum.ToString)
                    stb.Append(" to ")
                    stb.Append(MolecularInfo.Gel_Maximun.ToString)
                    sList.Add(stb.ToString)
                End If
            Case Nuctions.MolecularOperationEnum.PCR
                title = "PCR"

                If MolecularInfo.PrimerDesignerMode Then
                    For Each pr In MolecularInfo.DesignedPrimers
                        stb = New System.Text.StringBuilder
                        stb.Append(pr.Key)
                        stb.Append(": ")
                        stb.Append(pr.Value)
                        stb.Append(" Tm:")
                        stb.Append(Nuctions.CalculateTm(pr.Value, 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                        stb.Append("/")
                        stb.Append(Nuctions.CalculateTm(Nuctions.ParseInnerPrimer(pr.Value), 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                        sList.Add(stb.ToString)
                    Next
                Else
                    stb = New System.Text.StringBuilder
                    stb.Append(MolecularInfo.PCR_FPrimerName)
                    stb.Append(": ")
                    stb.Append(MolecularInfo.PCR_ForwardPrimer)
                    stb.Append(" Tm:")
                    stb.Append(Nuctions.CalculateTm(MolecularInfo.PCR_ForwardPrimer, 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                    stb.Append("/")
                    stb.Append(Nuctions.CalculateTm(Nuctions.ParseInnerPrimer(MolecularInfo.PCR_ForwardPrimer), 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                    sList.Add(stb.ToString)
                    stb = New System.Text.StringBuilder
                    stb.Append(MolecularInfo.PCR_RPrimerName)
                    stb.Append(": ")
                    stb.Append(MolecularInfo.PCR_ReversePrimer)
                    stb.Append(" Tm:")
                    stb.Append(Nuctions.CalculateTm(MolecularInfo.PCR_ReversePrimer, 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                    stb.Append("/")
                    stb.Append(Nuctions.CalculateTm(Nuctions.ParseInnerPrimer(MolecularInfo.PCR_ReversePrimer), 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                    sList.Add(stb.ToString)
                End If
            Case Nuctions.MolecularOperationEnum.Recombination
                title = "Recombination"
                stb = New System.Text.StringBuilder
                stb.Append(MolecularInfo.RecombinationMethodString)
                sList.Add(stb.ToString)
            Case Nuctions.MolecularOperationEnum.Ligation
                title = "Ligation"
                stb = New System.Text.StringBuilder
                stb.Append(MolecularInfo.LigationMethod.ToString)
                sList.Add(stb.ToString)
            Case Nuctions.MolecularOperationEnum.Screen
                Select Case MolecularInfo.Screen_Mode
                    Case Nuctions.ScreenModeEnum.PCR
                        title = "PCR Screening"
                        stb = New System.Text.StringBuilder
                        stb.Append(MolecularInfo.Screen_FName)
                        stb.Append(": ")
                        stb.Append(MolecularInfo.Screen_FPrimer)
                        stb.Append(" Tm:")
                        stb.Append(Nuctions.CalculateTm(MolecularInfo.Screen_FPrimer, 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                        stb.Append("°„C/")
                        stb.Append(Nuctions.CalculateTm(Nuctions.ParseInnerPrimer(MolecularInfo.Screen_FPrimer), 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                        stb.Append("°„C")
                        sList.Add(stb.ToString)

                        stb = New System.Text.StringBuilder
                        stb.Append(MolecularInfo.Screen_RName)
                        stb.Append(": ")
                        stb.Append(MolecularInfo.Screen_RPrimer)
                        stb.Append(" Tm:")
                        stb.Append(Nuctions.CalculateTm(MolecularInfo.Screen_RPrimer, 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                        stb.Append("°„C/")
                        stb.Append(Nuctions.CalculateTm(Nuctions.ParseInnerPrimer(MolecularInfo.Screen_RPrimer), 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                        stb.Append("°„C")
                        sList.Add(stb.ToString)

                    Case Nuctions.ScreenModeEnum.Features
                        title = "Feature Screening"
                        stb = New System.Text.StringBuilder
                        If MolecularInfo.Screen_OnlyCircular Then stb.Append("Only Circular; ")
                        For Each s As FeatureScreenInfo In MolecularInfo.Screen_Features
                            stb.Append(s.Feature.Name)
                            stb.Append(" <")
                            stb.Append(s.ScreenMethod.ToString)
                            stb.Append(">; ")
                        Next
                        sList.Add(stb.ToString)
                End Select
            Case Nuctions.MolecularOperationEnum.EnzymeAnalysis
                title = "Enzyme Analysis"

                For Each eai As EnzymeAnalysisItem In di.EnzymeAnalysisParamters
                    stb = New System.Text.StringBuilder
                    If eai.Use Then
                        stb.Append(eai.GeneFile.Name)
                        stb.Append(" <")
                        stb.Append(eai.Region)
                        stb.Append(">  Appearace ")

                        Select Case eai.Method
                            Case EnzymeAnalysisEnum.Equal
                                stb.Append("= ")

                            Case EnzymeAnalysisEnum.Greater
                                stb.Append("> ")

                            Case EnzymeAnalysisEnum.Less
                                stb.Append("< ")

                        End Select
                        stb.Append(eai.Value.ToString)
                        sList.Add(stb.ToString)
                    End If
                Next
                Dim g As Graphics = Graphics.FromImage(New Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                Dim w As Integer
                If di.DNAs.Count > 0 Then w = GetVectorWidth(MolecularInfo, di.DNAs(1)) + 240
                Dim s As String = ""
                Dim enzs As New List(Of String)
                enzs.AddRange(MolecularInfo.FetchedEnzymes)
                Dim enzq As New Queue(Of String)(enzs)

                Dim enz As String = Nothing
                Dim sz As SizeF
                While enzq.Count > 0
                    enz = enzq.Dequeue
                    sz = g.MeasureString(s + enz + "; ", vFont)
                    If sz.Width > w Then
                        sList.Add(s)
                        s = enz + "; "
                    Else
                        s += enz + "; "
                    End If
                End While
                If s.Length > 0 Then sList.Add(s)
            'sList.Add("Enzyme Found: " + MolecularInfo.OperationDescription)
            Case Nuctions.MolecularOperationEnum.Modify
                Select Case di.Modify_Method
                    Case Nuctions.ModificationMethodEnum.CIAP
                        title = "CIAP Dephospheration"
                    Case Nuctions.ModificationMethodEnum.Klewnow
                        title = "Klewnow Fragment Blunting"
                    Case Nuctions.ModificationMethodEnum.PNK
                        title = "PNK Phospheration"
                    Case Nuctions.ModificationMethodEnum.T4DNAP
                        title = "T4DNAP Blunting"
                End Select
            Case Nuctions.MolecularOperationEnum.Merge
                title = "Merging Sequencing Results"
                sList.Add(IIf(di.OnlySignificant, "Only Consider Long Significant Matches", "Consider All Matches"))
                sList.Add(IIf(di.OnlyExtend, "Only Get Results Longer than Inputs", "Get All Results"))
            Case Nuctions.MolecularOperationEnum.FreeDesign
                title = "DNA Design & Synthesis"
            Case Nuctions.MolecularOperationEnum.HashPicker
                title = "Pick by Hash Code"
                For Each s As String In MolecularInfo.PickedDNAs
                    sList.Add(s + "; ")
                Next
            Case Nuctions.MolecularOperationEnum.SequencingResult
                title = "Sequencing"
                sList.Add(String.Format("{0}:{1}", MolecularInfo.SequencingPrimerName, MolecularInfo.SequencingPrimer))
            Case Nuctions.MolecularOperationEnum.Compare
                title = "Compare DNA"
            Case Nuctions.MolecularOperationEnum.Host
                If MolecularInfo.Cells.Count = 0 Then
                    MolecularInfo.Cells.Add(New Nuctions.Cell)
                End If
                title = MolecularInfo.Cells(0).Host.Name
            Case Nuctions.MolecularOperationEnum.Transformation
                title = "Transformation"
            Case Nuctions.MolecularOperationEnum.Incubation
                title = "Incubation"
            Case Nuctions.MolecularOperationEnum.Extraction
                title = "DNA Extraction"
            Case Nuctions.MolecularOperationEnum.GibsonDesign
                title = "Gibson Assembly Design"
            Case Nuctions.MolecularOperationEnum.CRISPRCut
                title = "CRISPR Guided Digestion"
        End Select
        If MolecularInfo.Cells.Count > 0 Then
            stb = New System.Text.StringBuilder
            For Each c As Nuctions.Cell In MolecularInfo.Cells
                For Each gf As Nuctions.GeneFile In c.DNAs
                    stb.Append(gf.Length.ToString)
                    stb.Append("bp ")
                    stb.Append(IIf(gf.Iscircular, "C; ", "L; "))
                Next
                sList.Add(c.Host.Name + ": " + stb.ToString)
                stb.Clear()
            Next
        Else
            stb = New System.Text.StringBuilder
            For Each s As Nuctions.GeneFile In MolecularInfo.DNAs
                stb.Append(s.Length.ToString)
                stb.Append("bp ")
                stb.Append(IIf(s.Iscircular, "C; ", "L; "))
            Next
            sList.Add(stb.ToString)
        End If
        If MolecularInfo.GelFirstFigureNumber > 0 Then
            Screening.Add(String.Format("Screen {0} Samples with Gel Electrophoresis.", MolecularInfo.GelFirstFigureNumber))
            If MolecularInfo.GelSecondFigureNumber > 0 Then
                Screening.Add(String.Format("Screen another {0} Samples with Gel Electrophoresis, if previous screen failed.", MolecularInfo.GelSecondFigureNumber))
            End If
        End If
        If MolecularInfo.ColonyFirstSequencingNumber > 0 Then
            Screening.Add(String.Format("Screen {0} Samples with Sequencing.", MolecularInfo.ColonyFirstSequencingNumber))
            If MolecularInfo.ColonySecondSequencingNumber > 0 Then
                Screening.Add(String.Format("Screen another {0} Samples with Sequencing, if previous screen failed.", MolecularInfo.ColonySecondSequencingNumber))
            End If
        End If
        If di.MolecularOperation <> Nuctions.MolecularOperationEnum.EnzymeAnalysis Then
            descr = di.OperationDescription
        End If
    End Sub

    Public Sub New(ByVal di As DNAInfo, ByVal Enzymes As List(Of String), ByVal vID As Integer)
        vIndex = vID
        Me.Reload(di, Enzymes)
    End Sub
    Dim R As Single
    Dim B As Single
    Dim X As Single
    Dim Y As Single

    Dim sX As Single
    Dim sY As Single
    Dim sR As Single
    Dim sB As Single

    Public Sub StartMove()
        sX = X
        sY = Y
        sR = R
        sB = B
    End Sub
    Public Sub MoveBy(ByVal dvX As Single, ByVal dvY As Single)
        X = sX + dvX
        R = sR + dvX
        Y = sY + dvY
        B = sB + dvY
    End Sub


    Public Sub SaveGeneFile(ByVal Path As String)
        If vVector Is Nothing Then Exit Sub

        Dim regex As New System.Text.RegularExpressions.Regex("[\\/\:\*\?""<>\|]")
        Dim vName As String = MolecularInfo.Name
        vName = regex.Replace(vName, " ")
        If Not Path.EndsWith("\") Then Path += "\"
        vVector.GeneFile.WriteToFile(Path + vIndex.ToString.PadLeft(Me.MapStringLength, "0") + " " + vName + ".gb")
    End Sub

    Public Property Selected() As Boolean
        Get
            Dim opv As OperationView = Parent
            If opv Is Nothing Then
                Return False
            Else
                Return opv.SelectedItems.Contains(Me)
            End If
        End Get
        Set(ByVal value As Boolean)
            Dim opv As OperationView = Parent
            If Not (opv Is Nothing) AndAlso Not opv.SelectedItems.Contains(Me) Then
                opv.SelectedItems.Add(Me)
                opv.Draw()
                opv.OnSelectedIndexChanged()
            End If
        End Set
    End Property

    Private Old_X As Single = 0
    Private Old_Width As Single = 0
    Private firstUse As Boolean = True
    Public Function MeasureSize(ByVal g As Graphics) As SizeF
        Dim vX As Single
        Dim vY As Single
        vX = 0
        vY = 0
        MolecularInfo.DX = 0
        MolecularInfo.DY = 0
        R = 0
        Dim vS As SizeF
        Dim vstr As String

        If Not (vVector Is Nothing) Then
            vY += vVector.Height
            R = Math.Max(R, vVector.Width)
        ElseIf MolecularInfo.IsKeyName Then
            vS = g.MeasureString(MolecularInfo.Name, tFont)
            vY += vS.Height
            R = Math.Max(R, vS.Width)
        End If
        vstr = IIf(Me.Parent Is Nothing, title, vIndex.ToString.PadLeft(Parent.Items.Count.ToString.Length, "0") + " " + title)
        vS = g.MeasureString(vstr, tFont)
        R = Math.Max(R, vX + vS.Width)
        vY += vS.Height
        For Each vstr In sList
            vS = g.MeasureString(vstr, vFont)
            R = Math.Max(R, vX + vS.Width)
            vY += vS.Height
        Next
        For Each vstr In Screening
            vS = g.MeasureString(vstr, vFont)
            R = Math.Max(R, vX + vS.Width)
            vY += vS.Height
        Next
        If Not (descr Is Nothing) AndAlso descr.Length > 0 Then
            vstr = descr
            vS = g.MeasureString(vstr, vFont)
            R = Math.Max(R, vX + vS.Width)
            vY += vS.Height
        End If
        B = vY
        Return New SizeF(R, B)
    End Function
    Public Function DrawMap(ByVal g As Graphics) As SizeF
        Dim vX As Single
        Dim vY As Single
        vX = 0
        vY = 0
        MolecularInfo.DX = 0
        MolecularInfo.DY = 0
        R = 0
        Dim vS As SizeF
        Dim vstr As String

        If Not (vVector Is Nothing) Then
            g.DrawString(Me.MolecularInfo.Name, tFont, Brushes.Red, 0, 0)
            'If redrawvector Then
            vVector.DrawImage(g, 0, 0)
            'Else
            '    g.DrawImage(vVectorMap, New PointF(X, Y))
            'End If
            vY += vVector.Height
            R += vVector.Width
        End If
        vstr = IIf(Me.Parent Is Nothing, title, vIndex.ToString.PadLeft(Parent.Items.Count.ToString.Length, "0") + " " + title)
        g.DrawString(vstr, tFont, Brushes.Black, vX, vY)
        vS = g.MeasureString(vstr, tFont)
        R = Math.Max(R, vX + vS.Width)
        vY += vS.Height
        For Each vstr In sList
            g.DrawString(vstr, vFont, Brushes.Black, vX, vY)
            vS = g.MeasureString(vstr, vFont)
            R = Math.Max(R, vX + vS.Width)
            vY += vS.Height
        Next
        For Each vstr In Screening
            g.DrawString(vstr, vFont, Brushes.Blue, vX, vY)
            vS = g.MeasureString(vstr, vFont)
            R = Math.Max(R, vX + vS.Width)
            vY += vS.Height
        Next

        If Not (descr Is Nothing) AndAlso descr.Length > 0 Then
            vstr = descr
            g.DrawString(vstr, vFont, Brushes.Red, vX, vY)
            vS = g.MeasureString(vstr, vFont)
            R = Math.Max(R, vX + vS.Width)
            vY += vS.Height
        End If
        B = vY
        g.DrawRectangle(Pens.Blue, 0, 0, R - 0, B - 0)
        Return New SizeF(R, B)
    End Function

    Public Sub Draw(ByVal g As Graphics, Optional ByVal redrawvector As Boolean = False, Optional BlueEdge As Boolean = False)
        Dim vX As Single
        Dim vY As Single
        Dim sz As SizeF
        'important code to hold position
        sz = MeasureSize(g)
        'end position code
        vX = X - sz.Width / 2
        vY = Y
        MolecularInfo.DX = X
        MolecularInfo.DY = Y
        R = vX
        Dim vS As SizeF
        Dim vstr As String

        If Not (vVector Is Nothing) Then
            g.DrawString(Me.MolecularInfo.Name, tFont, Brushes.Red, vX, vY)
            'If redrawvector Then
            vVector.DrawImage(g, vX, vY)
            'Else
            '    g.DrawImage(vVectorMap, New PointF(X, Y))
            'End If
            vY += vVector.Height
            R += vVector.Width
        ElseIf MolecularInfo.IsKeyName Then
            g.DrawString(MolecularInfo.Name, tFont, Brushes.Red, vX, vY)
            vS = g.MeasureString(MolecularInfo.Name, tFont)
            vY += vS.Height
        End If
        vstr = IIf(Me.Parent Is Nothing, title, vIndex.ToString.PadLeft(Parent.Items.Count.ToString.Length, "0") + " " + title)
        g.DrawString(vstr, tFont, Brushes.Black, vX, vY)
        vS = g.MeasureString(vstr, tFont)
        R = Math.Max(R, vX + vS.Width)
        vY += vS.Height
        For Each vstr In sList
            g.DrawString(vstr, vFont, Brushes.Black, vX, vY)
            vS = g.MeasureString(vstr, vFont)
            R = Math.Max(R, vX + vS.Width)
            vY += vS.Height
        Next
        For Each vstr In Screening
            g.DrawString(vstr, vFont, Brushes.Blue, vX, vY)
            vS = g.MeasureString(vstr, vFont)
            R = Math.Max(R, vX + vS.Width)
            vY += vS.Height
        Next
        If Not (descr Is Nothing) AndAlso descr.Length > 0 Then
            vstr = descr
            g.DrawString(vstr, vFont, Brushes.Red, vX, vY)
            vS = g.MeasureString(vstr, vFont)
            R = Math.Max(R, vX + vS.Width)
            vY += vS.Height
        End If
        B = vY
        g.DrawRectangle(IIf(Not BlueEdge And Selected, Pens.Red, IIf(MolecularInfo.IsConstructionNode, Pens.LightPink, IIf(MolecularInfo.IsKeyName, Pens.Orange, Pens.LightBlue))), vX, Y, sz.Width, B - Y)
        Select Case MolecularInfo.Progress
            Case ProgressEnum.Unstarted
            Case ProgressEnum.Inprogress
                g.FillRectangle(New SolidBrush(Color.FromArgb(32, 255, 0, 0)), vX, Y, sz.Width, B - Y)
            Case ProgressEnum.Finished
                g.FillRectangle(New SolidBrush(Color.FromArgb(32, 0, 255, 0)), vX, Y, sz.Width, B - Y)
            Case ProgressEnum.Obsolete
                g.FillRectangle(New SolidBrush(Color.FromArgb(32, 0, 0, 0)), vX, Y, sz.Width, B - Y)
        End Select
    End Sub

    Public Sub DrawArrow(ByVal g As Graphics)
        Dim pnts As PointF()
        Dim vx As Vector2
        Dim si As ChartItem

        For Each dnai As DNAInfo In MolecularInfo.Source
            si = dnai.GetParetntChartItem
            vx = Center - si.Center
            pnts = GetArrow(si.GetPosition(vx), GetPosition(-vx), 0, 0)
            g.FillPolygon(Brushes.LightYellow, pnts)
            g.DrawPolygon(Pens.Gray, pnts)
        Next
    End Sub
    Public Sub DrawArrow(ByVal g As Graphics, limitedSource As IList(Of DNAInfo))
        Dim pnts As PointF()
        Dim vx As Vector2
        Dim si As ChartItem
        For Each dnai As DNAInfo In MolecularInfo.Source
            If limitedSource.Contains(dnai) Then
                si = dnai.GetParetntChartItem
                vx = Center - si.Center
                pnts = GetArrow(si.GetPosition(vx), GetPosition(-vx), 0, 0)
                g.FillPolygon(Brushes.LightYellow, pnts)
                g.DrawPolygon(Pens.Gray, pnts)
            End If
        Next
    End Sub


    Public Function Hittest(ByVal vP As PointF) As Boolean
        Return vP.X > (X + X - R) And vP.X < R And vP.Y > Y And vP.Y < B
    End Function
    Private Shared Function GetArrow(ByVal V0 As Vector2, ByVal VE As Vector2, ByVal prefix As Single, ByVal suffix As Single) As PointF()
        Dim VX As Vector2 = VE - V0
        Dim VY As Vector2 = VX.GetBase.Turn(Math.PI / 2)
        Dim L As Single = VX.GetLength
        If L < (prefix + suffix + 24) Then
            prefix = (L - 24) / 2 + 8
            suffix = (L - 24) / 2 - 8
        End If
        VX = VX.GetBase
        Return New PointF() {V0 + VX * prefix, V0 + VX * prefix + VY * 6, VE - VX * (suffix + 12) + VY * 6,
                             VE - VX * (suffix + 12) + VY * 12, VE - VX * suffix,
                             VE - VX * (suffix + 12) - VY * 12, VE - VX * (suffix + 12) - VY * 6, V0 + VX * prefix - VY * 6}
    End Function
    Public Property Left() As Single
        Get
            Return X + X - R
        End Get
        Set(ByVal value As Single)
            Dim delta As Single = value - X

            X += delta
            R += delta
        End Set
    End Property
    Public ReadOnly Property Right() As Single
        Get
            Return R
        End Get
    End Property
    Public Property Top() As Single
        Get
            Return Y
        End Get
        Set(ByVal value As Single)
            Dim delta As Single = value - Y
            Y += delta
            B += delta
        End Set
    End Property
    Public ReadOnly Property Bottom() As Single
        Get
            Return B
        End Get
    End Property
    Public ReadOnly Property Center() As Vector2
        Get
            Return New Vector2(X, (Y + B) / 2)
        End Get
    End Property
    Public Function GetPosition(ByVal vt As Vector2) As Vector2
        Dim w As Single = (R - X)
        Dim h As Single = (B - Y) / 2

        If vt.X = 0 And vt.Y = 0 Then
            Return New Vector2(X, B)
        ElseIf vt.Y = 0 Then
            Return New Vector2(X + w * Math.Sign(vt.X), (Y + B) / 2)
        ElseIf vt.X = 0 Then
            Return New Vector2(X, (Y + B) / 2 + h * Math.Sign(vt.Y))
        Else
            If h * Math.Abs(vt.X) > w * Math.Abs(vt.Y) Then
                Return New Vector2(X + w * Math.Sign(vt.X), (Y + B) / 2)
            Else
                Return New Vector2(X, (Y + B) / 2 + h * Math.Sign(vt.Y))
            End If
        End If
    End Function

    Public Sub AutoFit()
        If MolecularInfo.Source.Count > 0 Then
            Dim X1 As Single = 0
            Dim Y1 As Single = Single.MinValue


            Dim si As ChartItem
            For Each DNAi As DNAInfo In MolecularInfo.Source
                si = DNAi.GetParetntChartItem
                X1 += si.Center.X
                Y1 = IIf(Y1 > si.Bottom, Y1, si.Bottom)
            Next

            X = X1 / MolecularInfo.Source.Count
            Top = Y1 + 40
        End If
    End Sub

    Public ReadOnly Property Feature() As List(Of Nuctions.Feature)
        Get
            If TypeOf Me.Parent.Parent.Parent.Parent Is WorkControl Then
                Dim wc As WorkControl = Me.Parent.Parent.Parent.Parent
                Return wc.FeatureCol
            Else
                Return Nothing
            End If
        End Get
    End Property
End Class




Public Enum DescribeEnum
    Vecotor
    Chromosome
End Enum
Public Enum LigationMethod
    Normal2Fragment
    Consider3Fragment
    MultipleFragmentUnique
End Enum

Public Enum ProjectServiceStatusEnum As Integer
    None
    Quotation
    RequestForModify
    Modified
    ContractorConfirmed
    CustomerConfirmed
    InProgress
    ContractorAbandoned
    CustomerAbaondoned
    Finished
    Shipped
End Enum

Public Class SourceEventArgs
    Inherits EventArgs
    Public Target As DNAInfo
End Class

Public Enum ProgressEnum As Integer
    Unstarted
    Inprogress
    Finished
    Obsolete
End Enum

Public Enum SequenceResultOptions As Integer
    Unchecked
    Correct
    PointMutation
    FragmentInsertion
    FragmentLoss
    NoneMatch
End Enum