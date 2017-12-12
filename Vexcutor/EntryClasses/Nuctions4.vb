Partial Public Class Nuctions
    Friend Shared Function GenerateDesignForDNAs(cList As List(Of Nuctions.GeneFile))

    End Function

    Friend Shared Function CRISPRDigest(Substrates As IList(Of GeneFile), gRNASources As IList(Of GeneFile)) As List(Of GeneFile)
        Dim CRISPRs As New List(Of Nuctions.RestrictionEnzyme)
        'Obtain gRNAs from gRNASources
        For Each gRNA In SettingEntry.gRNAs
            CRISPRs.AddRange(gRNA.IdentifygRNA(gRNASources))
        Next
        Dim Results As New List(Of GeneFile)
        'Digest Substrates and put them into results
        For Each gf In Substrates
            Dim EARC = New Nuctions.EnzymeAnalysis.EnzymeAnalysisResult(CRISPRs, gf)
            Results.AddRange(EARC.CutDNA)
        Next
        Return Results
    End Function

    Friend Shared Function FacingSequenceAnnealingSearch(x As String, y As String, Optional minLength As Integer = 12, Optional MaxSearchRegion As Integer = 250) As Pair(Of AnnealingInformation, AnnealingInformation)
        Dim xAI As AnnealingInformation
        Dim yAI As AnnealingInformation
        Dim box As String
        Dim found As Integer
        Dim extension As Integer

        Dim extend = Function() As Integer
                         Dim yIndex As Integer
                         Dim xIndex As Integer
                         Dim Accumulation As Integer = 1
                         xIndex = xAI.Offset + minLength + Accumulation - 1
                         yIndex = found - Accumulation
                         While (Accumulation <= found And Accumulation + xAI.Offset + minLength <= x.Length) AndAlso IsComplementary(x(xIndex), y(yIndex))
                             Accumulation += 1
                             xIndex = xAI.Offset + minLength + Accumulation - 1
                             yIndex = found - Accumulation
                         End While
                         Return Accumulation - 1
                     End Function
        xAI.Offset = 0
        Dim limit As Integer = Math.Min(x.Length - minLength, MaxSearchRegion)
        While xAI.Offset < limit
            extension = 1
            box = Nuctions.ReverseComplement(x.Substring(xAI.Offset, minLength))
            found = y.IndexOf(box)
            If found > -1 Then
                extension = extend()
                xAI.Length = minLength + extension
                yAI.Offset = found - extension
                yAI.Length = xAI.Length
                Return New Pair(Of AnnealingInformation, AnnealingInformation) With {.Key = xAI, .Value = yAI}
            End If
            xAI.Offset += extension
        End While
        Return New Pair(Of AnnealingInformation, AnnealingInformation) With {.Key = New AnnealingInformation, .Value = New AnnealingInformation}
    End Function
    Friend Shared Function FindFacingTailMatch(x As String, y As String, Optional MaxLength As Integer = 12) As Integer
        Dim lastmatch As Integer = 0
        For i As Integer = 1 To MaxLength
            If x.Substring(0, i) = Nuctions.ReverseComplement(y.Substring(0, i)) Then lastmatch = i
        Next
        Return lastmatch
    End Function
    Friend Shared Function IsComplementary(X As Char, Y As Char) As Boolean
        Select Case X
            Case "A"c
                Return Y = "T"c
            Case "T"c
                Return Y = "A"c
            Case "G"c
                Return Y = "C"c
            Case "C"c
                Return Y = "G"c
            Case Else
                Return False
        End Select
    End Function

    Friend Shared Function FindDigestionInformation(ezList As List(Of String)) As Dictionary(Of String, String)

        Dim dict As New Dictionary(Of String, String)
        For Each sys In SettingEntry.DigestionBuffer.DigestionBufferSystems
            Dim stb As New System.Text.StringBuilder
            stb.AppendFormat("{0}: ", sys.SupplierName)
            Dim infoList As New Dictionary(Of String, List(Of DigestionBufferInfo))
            'Generate the dict for enzymes
            For Each ez In ezList
                infoList.Add(ez, New List(Of DigestionBufferInfo))
            Next
            Dim BufferTypes As New HashSet(Of String)
            'find infos for enzymes
            For Each info In sys.DigestionBufferInfos
                If info.Activity >= 50.0F AndAlso ezList.Contains(info.EnzymeName) Then
                    infoList(info.EnzymeName).Add(info)
                    BufferTypes.Add(info.BufferName)
                End If
            Next
            'try to work out proper conditions.
            Dim BufferConditions As New List(Of DigestionBufferCondition) From {New DigestionBufferCondition}
            For Each ez In ezList
                Dim infoSet = infoList(ez)
                infoSet.Sort(DigestionBufferInfoComparer.Default)
                Dim CurrentConditions = BufferConditions.ToArray
                BufferConditions.Clear()
                For Each condition In CurrentConditions
                    For Each info In infoSet
                        Dim combined As DigestionBufferCondition = condition.ApplyDigestionInfo(info)
                        If combined IsNot Nothing Then BufferConditions.Add(combined)
                    Next
                Next
            Next
            If BufferConditions.Count > 0 Then
                BufferConditions.Sort(DigestionBufferConditionComparer.Default)
                Dim CBS As New CombinedBufferSorter
                For Each bc In BufferConditions
                    CBS.Push(bc)
                Next
                Dim CBCs = CBS.GetCombined
                For i As Integer = 0 To Math.Min(2, CBCs.Count - 1)
                    stb.Append(CBCs(i).ToString)
                Next
            Else
                Dim digStep As Integer = 1
                For Each ez In ezList
                    Dim infoSet = infoList(ez)
                    Dim CDS As New CombinedDigestionSorter
                    For Each info In infoSet
                        CDS.Push(info)
                    Next
                    Dim maxSet = CDS.GetCombined()

                    'Dim maxSet = infoSet.Where(Function(info) info.Activity = 100.0F).ToList
                    If maxSet.Count > 0 Then
                        stb.AppendFormat("{0}) ", digStep)
                        'For i As Integer = 0 To Math.Min(3, maxSet.Count - 1)
                        stb.Append(maxSet(0).ToString)
                        'Next
                    Else
                        stb.AppendFormat("{0}) Not Found for {1} ", digStep, ez)
                    End If
                    digStep += 1
                Next
            End If
            dict.Add(sys.SupplierName, stb.ToString)
        Next
        Return dict
    End Function

    ''' <summary>
    ''' This function will use the codon definition to search for ORFs.
    ''' </summary>
    ''' <param name="DNAs">the DNAs that you want to search for ORFs</param>
    ''' <param name="MinimumLength">The minimal length of the ORF</param>
    Public Shared Sub SearchORFs(DNAs As List(Of GeneFile), StartCodons As List(Of String), MinimumLength As Integer, Features As List(Of Feature))
        'ATG NNN 
        Dim beginCodons As New List(Of String)
        Dim Codons As New List(Of String)
        Dim endCodons As New List(Of String)

        For Each cdn In StartCodons
            beginCodons.Add(cdn)
        Next

        For Each key In SettingEntry.CodonCol.AnimoTable.Keys
            Select Case key
                Case "-"
                    For Each code In SettingEntry.CodonCol.AnimoTable(key).CodeList
                        endCodons.Add(code.Name)
                    Next
                Case Else
                    For Each code In SettingEntry.CodonCol.AnimoTable(key).CodeList
                        Codons.Add(code.Name)
                    Next
            End Select
        Next

        Dim _Start = String.Join("|", beginCodons)
        Dim _Middle = String.Join("|", Codons)
        Dim _End = String.Join("|", endCodons)

        Dim pattern = String.Format("({0})({1}){{{2},}}({3})", _Start, _Middle, MinimumLength.ToString, _End)

        Dim regex As New System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        For Each dna In DNAs

            'remove all the annotations that were detected before;
            Dim rmList As New List(Of GeneAnnotation)
            For Each ga In dna.Features
                If Not (Features.Contains(ga.Feature)) Then rmList.Add(ga)
            Next
            For Each ga In rmList
                dna.Features.Remove(ga)
            Next
            Dim length As Integer = dna.Length
            If dna.Iscircular Then
                Dim forward As String = dna.SubSequence(0, length * 2)
                Dim reverse As String = dna.SubRCSequence(0, length * 2)
                For Each m As System.Text.RegularExpressions.Match In regex.Matches(forward)
                    If m.Index < length AndAlso m.Length < length Then
                        Dim ant As New GeneAnnotation()
                        'ant.Index = m.Index
                        ant.StartPosition = m.Index + 1
                        ant.EndPosition = m.Index + m.Length
                        ant.Complement = False
                        ant.Vector = dna
                        ant.Feature = New Feature("Detected CDS", ant.GetSuqence, String.Format("Detected CDS {0}aa", (m.Value.Length \ 3) - 1), "CDS", String.Format("Detected CDS {0}aa", (m.Value.Length / 3 - 1)), True)
                        ant.Label = ant.Feature.Label
                        ant.Note = ant.Feature.Note
                        ant.Type = ant.Feature.Type
                        dna.Features.Add(ant)
                        'Dim seq As String = ant.GetSuqence
                        'Dim test = seq
                    End If
                Next
                For Each m As System.Text.RegularExpressions.Match In regex.Matches(reverse)
                    If m.Index < length And m.Length < length Then
                        Dim ant As New GeneAnnotation()
                        'ant.Index = length - m.Index
                        ant.StartPosition = length - (m.Index + m.Length) + 1
                        ant.EndPosition = length - m.Index
                        While ant.EndPosition < 0
                            ant.EndPosition += length
                        End While
                        ant.Complement = True
                        ant.Vector = dna
                        ant.Feature = New Feature("Detected CDS", ant.GetSuqence, String.Format("Detected CDS {0}aa", (m.Value.Length \ 3) - 1), "CDS", String.Format("Detected CDS {0}aa", (m.Value.Length / 3 - 1)), True)
                        ant.Label = ant.Feature.Label
                        ant.Note = ant.Feature.Note
                        ant.Type = ant.Feature.Type
                        dna.Features.Add(ant)
                        'Dim seq As String = ant.GetSuqence
                        'Dim test = seq
                    End If
                Next
            Else
                Dim forward As String = dna.Sequence
                Dim reverse As String = dna.RCSequence
                For Each m As System.Text.RegularExpressions.Match In regex.Matches(forward)
                    Dim ant As New GeneAnnotation()
                    'ant.Index = m.Index
                    ant.StartPosition = m.Index + 1
                    ant.EndPosition = m.Index + m.Length
                    ant.Complement = False
                    ant.Vector = dna
                    ant.Feature = New Feature("Detected CDS", ant.GetSuqence, String.Format("Detected CDS {0}aa", (m.Value.Length \ 3) - 1), "CDS", String.Format("Detected CDS {0}aa", (m.Value.Length / 3 - 1)), True)
                    ant.Label = ant.Feature.Label
                    ant.Note = ant.Feature.Note
                    ant.Type = ant.Feature.Type
                    dna.Features.Add(ant)
                    'Dim seq As String = ant.GetSuqence
                    'Debug.WriteLine(seq)
                Next
                For Each m As System.Text.RegularExpressions.Match In regex.Matches(reverse)
                    Dim ant As New GeneAnnotation()
                    'ant.Index = length - m.Index
                    ant.StartPosition = length - (m.Index + m.Length) + 1
                    ant.EndPosition = length - m.Index
                    While ant.EndPosition < 0
                        ant.EndPosition += length
                    End While
                    ant.Complement = True
                    ant.Vector = dna
                    ant.Feature = New Feature("Detected CDS", ant.GetSuqence, String.Format("Detected CDS {0}aa", (m.Value.Length \ 3) - 1), "CDS", String.Format("Detected CDS {0}aa", (m.Value.Length / 3 - 1)), True)
                    ant.Label = ant.Feature.Label
                    ant.Note = ant.Feature.Note
                    ant.Type = ant.Feature.Type
                    dna.Features.Add(ant)
                    'Dim seq As String = ant.GetSuqence
                    'Debug.WriteLine(seq)
                Next
            End If
        Next

    End Sub
End Class


Public Class MultipleSetIterator
    Public Sub Iterate()
        Dim sets As New List(Of List(Of DigestionBufferInfo))


    End Sub
End Class