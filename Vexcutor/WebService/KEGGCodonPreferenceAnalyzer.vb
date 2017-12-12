Imports System.Windows
Public Class KEGGCodonPreferenceAnalyzer
    Inherits DependencyObject
    'KEGGCodonPreferenceAnalyzer->Organsim As String Default: ""
    Public Property Organsim As String
        Get
            Return GetValue(OrgansimProperty)
        End Get
        Set(ByVal value As String)
            SetValue(OrgansimProperty, value)
        End Set
    End Property
    Public Shared ReadOnly OrgansimProperty As DependencyProperty =
                           DependencyProperty.Register("Organsim",
                           GetType(String), GetType(KEGGCodonPreferenceAnalyzer),
                           New PropertyMetadata(""))
    'KEGGCodonPreferenceAnalyzer->Count As Integer Default: 0
    Public Property Count As Integer
        Get
            Return GetValue(CountProperty)
        End Get
        Set(ByVal value As Integer)
            SetValue(CountProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CountProperty As DependencyProperty =
                           DependencyProperty.Register("Count",
                           GetType(Integer), GetType(KEGGCodonPreferenceAnalyzer),
                           New PropertyMetadata(0))
    'KEGGCodonPreferenceAnalyzer->Total As Integer Default: 0
    Public Property Total As Integer
        Get
            Return GetValue(TotalProperty)
        End Get
        Set(ByVal value As Integer)
            SetValue(TotalProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TotalProperty As DependencyProperty =
                           DependencyProperty.Register("Total",
                           GetType(Integer), GetType(KEGGCodonPreferenceAnalyzer),
                           New PropertyMetadata(0))
    'KEGGCodonPreferenceAnalyzer->Status As String Default: ""
    Public Property Status As String
        Get
            Return GetValue(StatusProperty)
        End Get
        Set(ByVal value As String)
            SetValue(StatusProperty, value)
        End Set
    End Property
    Public Shared ReadOnly StatusProperty As DependencyProperty =
                           DependencyProperty.Register("Status",
                           GetType(String), GetType(KEGGCodonPreferenceAnalyzer),
                           New PropertyMetadata(""))

    'KEGGCodonPreferenceAnalyzer->ErrorCount As Integer Default: 0
    Public Property ErrorCount As Integer
        Get
            Return GetValue(ErrorCountProperty)
        End Get
        Set(ByVal value As Integer)
            SetValue(ErrorCountProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ErrorCountProperty As DependencyProperty =
                           DependencyProperty.Register("ErrorCount",
                           GetType(Integer), GetType(KEGGCodonPreferenceAnalyzer),
                           New PropertyMetadata(0))
    'KEGGCodonPreferenceAnalyzer->NonCDS As Integer Default: 0
    Public Property NonCDS As Integer
        Get
            Return GetValue(NonCDSProperty)
        End Get
        Set(ByVal value As Integer)
            SetValue(NonCDSProperty, value)
        End Set
    End Property
    Public Shared ReadOnly NonCDSProperty As DependencyProperty =
                           DependencyProperty.Register("NonCDS",
                           GetType(Integer), GetType(KEGGCodonPreferenceAnalyzer),
                           New PropertyMetadata(0))


    Public Async Sub StartAnalyze()

        Status = "Downloading Gene List from KEGG..."
        Dim genes As List(Of KEGG.Gene) = Nothing
        Dim _org As String = Organsim
        Dim dt As New System.Threading.Tasks.Task(Sub()
                                                      genes = KEGGUtil.ListGenesForOrganism(_org)
                                                  End Sub)
        dt.Start()
        Await dt
        Dispatcher.Invoke(Sub()
                              Total = genes.Count
                          End Sub)
        Status = "Analyzing Genes..."

        Dim cua As New CodonPreferenceAnalyzer

        Dim t As New System.Threading.Tasks.Task(Sub()
                                                     'Dim cnt As Integer = 0
                                                     For gi As Integer = 0 To genes.Count - 1 Step 10
                                                         Dim sList As New List(Of String)
                                                         For j As Integer = gi To Math.Min(gi + 9, genes.Count - 1)
                                                             sList.Add(genes(j).ID)
                                                         Next
                                                         Dim gds = KEGGUtil.GetMultipleGeneDetail(sList)
                                                         For Each gd In gds
                                                             If gd.GeneType = "CDS" Then
                                                                 cua.CDSCount += 1
                                                                 If gd.AminoAcidSequence.Length * 3 + 3 = gd.NucleotideSequence.Length Then
                                                                     cua.RecordCodon(gd.NucleotideSequence.Substring(0, 3), "^")
                                                                     For i As Integer = 1 To gd.AminoAcidSequence.Length - 1
                                                                         cua.RecordCodon(gd.NucleotideSequence.Substring(i * 3, 3), gd.AminoAcidSequence(i))
                                                                     Next
                                                                     cua.RecordCodon(gd.NucleotideSequence.Substring(gd.NucleotideSequence.Length - 3, 3), "*")
                                                                 Else
                                                                     Dispatcher.Invoke(Sub()
                                                                                           ErrorCount += 1
                                                                                       End Sub)
                                                                 End If
                                                             Else
                                                                 Dispatcher.Invoke(Sub()
                                                                                       NonCDS += 1
                                                                                   End Sub)
                                                             End If
                                                             Dispatcher.Invoke(Sub()
                                                                                   Count += 1
                                                                               End Sub)
                                                         Next

                                                     Next
                                                 End Sub)
        t.Start()
        Await t
        Dim trans As New KEGG.Translation(True)
        For Each aa In cua.CodonAnalyzers
            aa.Normalize()
            For Each cd In aa.Codons
                trans.AminoTable(aa.ShortName).Codons.Add(New CodonUsage With {.Codon = New Codon With {.Name = cd.Codon.Name}, .Ratio = cd.NormalizedRatio})
            Next
        Next
        Translation = cua.CreateTranslation
        Translation.GenerateCodeTableFromAminoTable()
        Translation.Organism = Organsim
        Save()
        If CodonAnalyzedCallBack IsNot Nothing Then CodonAnalyzedCallBack.Invoke(Organsim)
    End Sub
    'KEGGCodonPreferenceAnalyzer->Translation As Translation Default: Nothing
    Public Property Translation As KEGG.Translation
        Get
            Return GetValue(TranslationProperty)
        End Get
        Set(ByVal value As KEGG.Translation)
            SetValue(TranslationProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TranslationProperty As DependencyProperty =
                           DependencyProperty.Register("Translation",
                           GetType(KEGG.Translation), GetType(KEGGCodonPreferenceAnalyzer),
                           New PropertyMetadata(Nothing))
    Public Function Save() As Boolean
        If Translation Is Nothing Then Return False
        Dim dir As New System.IO.DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory + "\KEGGCodonUsage")
        If Not dir.Exists Then dir.Create()
        Dim filename As String = System.AppDomain.CurrentDomain.BaseDirectory + "\KEGGCodonUsage\" + Organsim + ".tsl"
        Dim bytes As Byte()
        Using ms As New IO.MemoryStream
            Dim bf = New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter()
            bf.Serialize(ms, Translation)
            bytes = ms.ToArray
        End Using
        IO.File.WriteAllBytes(filename, bytes)
        Return True
    End Function

    Public CodonAnalyzedCallBack As Action(Of String)
End Class
