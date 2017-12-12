Imports System.Windows
Public Class NCBIUtil
    Public Shared Function EFTECH(db As NCBIDBEnum, id As String, rettype As RETTYPEEnum, retmode As RETMODEEnum) As String
        Dim wc As New System.Net.WebClient
        Return wc.DownloadString(String.Format("http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db={0}&id={1}&rettype={2}&retmode={3}", db, id, rettype, retmode))
    End Function
    Public Shared Function ESEARCH(db As NCBIDBEnum, term As String, rettype As RETTYPEEnum, retmode As RETMODEEnum, take As Integer) As String
        Dim wc As New System.Net.WebClient
        Return wc.DownloadString(String.Format("http://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi?db={0}&term={1}&rettype={2}&retmode={3}&retmax={4}", db, term, rettype, retmode, take))
    End Function
    Public Shared Function ESUMMARY(db As NCBIDBEnum, ids As IEnumerable(Of String)) As String
        Dim wc As New System.Net.WebClient
        Return wc.DownloadString(String.Format("http://eutils.ncbi.nlm.nih.gov/entrez/eutils/esummary.fcgi?db={0}&id={1}", db, String.Join(",", ids)))
    End Function
    Public Shared Function ESEARCHTask(db As NCBIDBEnum, term As String, rettype As RETTYPEEnum, retmode As RETMODEEnum, take As Integer) As System.Threading.Tasks.Task(Of String)
        Dim t As New System.Threading.Tasks.Task(Of String)(Function()
                                                                Return ESEARCH(db, term, rettype, retmode, take)
                                                            End Function)
        t.Start()
        Return t
    End Function


    'Public Shared Function GetProtein(ID As String) As ProteinFile
    '    If ProteinExists(ID) Then Return LoadProtein(ID)
    '    Dim wc As New System.Net.WebClient
    '    Dim value As String = wc.DownloadString(String.Format("http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=protein&id={0}&rettype=gb", ID))

    '    Dim sections = SectionDivider.Divide(value, "(^|\n)\w+", Text.RegularExpressions.RegexOptions.IgnoreCase)
    '    Dim sequences = SectionDivider.SelectSection(sections, "^ORIGIN", Text.RegularExpressions.RegexOptions.IgnoreCase)

    '    Dim pf As New ProteinFile With {.ID = ID}
    '    If sequences.Count = 1 Then
    '        Dim sequence = sequences(0).Substring(6)
    '        Dim AminoAcids = Nuctions.AminoAcidFilter(sequence)
    '        pf.Sequence = AminoAcids
    '    End If

    '    Dim sources = SectionDivider.SelectSection(sections, "^SOURCE\s+", Text.RegularExpressions.RegexOptions.IgnoreCase)
    '    If sources.Count > 0 Then
    '        Dim srcsections = SectionDivider.Divide(sources(0), "(^|\n)  \w+", Text.RegularExpressions.RegexOptions.IgnoreCase)
    '        Dim organisms = SectionDivider.SelectSection(srcsections, "^  ORGANISM\s+", Text.RegularExpressions.RegexOptions.IgnoreCase)
    '        If organisms.Count > 0 Then
    '            Dim linesections = SectionDivider.Divide(sources(0), "(^|\n)\s+", Text.RegularExpressions.RegexOptions.IgnoreCase)
    '            Dim lines = SectionDivider.SelectSection(linesections, "^\s+", Text.RegularExpressions.RegexOptions.IgnoreCase)
    '            Dim stb As New System.Text.StringBuilder

    '            For i = 1 To lines.Count - 1
    '                stb.Append(lines(i).Substring(12))
    '            Next
    '            stb.Append(lines(0).Substring(12))
    '            pf.Source = stb.ToString.Replace(vbCr, "").Replace(vbLf, "")
    '        Else
    '            pf.Source = sources(0).ToString.Replace(vbCr, "").Replace(vbLf, "")
    '        End If
    '    End If

    '    Dim definitions = SectionDivider.SelectSection(sections, "^DEFINITION\s+", Text.RegularExpressions.RegexOptions.IgnoreCase)
    '    If definitions.Count > 0 Then
    '        pf.Definition = definitions(0).Substring(12).ToString.Replace(vbCr, "").Replace(vbLf, "")
    '    End If

    '    SaveProtein(ID, pf)
    '    Return pf
    'End Function

    'Private Shared Sub SaveProtein(PID As String, gd As ProteinFile)
    '    Dim filename = GetDirectory("NCBIProtein") + PID + ".pt"
    '    SerializeToFile(filename, gd)
    'End Sub
    'Private Shared Function LoadProtein(PID As String) As ProteinFile
    '    Dim filename = GetDirectory("NCBIProtein") + PID + ".pt"
    '    Return DeserializeFile(filename)
    'End Function
    'Private Shared Function ProteinExists(PID As String) As Boolean
    '    Return IO.File.Exists(GetDirectory("NCBIProtein") + PID + ".pt")
    'End Function

    Private Shared Function GetDirectory(Name As String) As String
        If Not IO.Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\" + Name) Then IO.Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + "\" + Name)
        Return System.AppDomain.CurrentDomain.BaseDirectory + "\" + Name + "\"
    End Function
End Class

Public Enum NCBIDBEnum
    nuccore
End Enum

Public Enum RETTYPEEnum
    null
    gbwithparts
    fasta_cds_na
    uilist
End Enum
Public Enum RETMODEEnum
    text
    xml
End Enum

Namespace NCBI
    Public Class NucleotideDownloader
        Inherits DependencyObject
        'NucleotideDownloader->Total As Integer Default: 0
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
                               GetType(Integer), GetType(NucleotideDownloader),
                               New PropertyMetadata(0))
        'NucleotideDownloader->Count As Integer Default: 0
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
                               GetType(Integer), GetType(NucleotideDownloader),
                               New PropertyMetadata(0))
        'NucleotideDownloader->Threads As Integer Default: 5
        Public Property Threads As Integer
            Get
                Return GetValue(ThreadsProperty)
            End Get
            Set(ByVal value As Integer)
                SetValue(ThreadsProperty, value)
            End Set
        End Property
        Public Shared ReadOnly ThreadsProperty As DependencyProperty =
                               DependencyProperty.Register("Threads",
                               GetType(Integer), GetType(NucleotideDownloader),
                               New PropertyMetadata(5))



        Public Async Function StartDownloadTask(sList As IEnumerable(Of String)) As System.Threading.Tasks.Task(Of List(Of Nuctions.GeneFile))
            Total = sList.Count
            Dim cQueue As New System.Collections.Concurrent.ConcurrentQueue(Of String)
            For Each id In sList
                cQueue.Enqueue(id)
            Next
            Dim rList As New System.Collections.Concurrent.ConcurrentBag(Of Nuctions.GeneFile)

            Dim pTasks As New List(Of System.Threading.Tasks.Task)
            For i As Integer = 1 To Math.Min(Threads, cQueue.Count)
                Dim t As New System.Threading.Tasks.Task(Sub()
                                                             Dim vID As String = Nothing
                                                             While vID Is Nothing AndAlso cQueue.Count > 0
                                                                 If cQueue.TryDequeue(vID) Then
                                                                     Dim gbString = NCBIUtil.EFTECH(NCBIDBEnum.nuccore, vID, RETTYPEEnum.gbwithparts, RETMODEEnum.text)
                                                                     rList.Add(Nuctions.GeneFile.LoadFromGeneBankFormatString(gbString))
                                                                     vID = Nothing
                                                                 Else
                                                                     System.Threading.Thread.Sleep(10)
                                                                 End If
                                                                 Dispatcher.Invoke(Sub()
                                                                                       Count = rList.Count
                                                                                   End Sub)
                                                             End While
                                                         End Sub)
                t.Start()
                pTasks.Add(t)
            Next


            'Task.WaitAll(pTasks.ToArray)
            Await System.Threading.Tasks.Task.WhenAll(pTasks.ToArray)
            Dim gList As New List(Of Nuctions.GeneFile)
            For Each g In rList
                gList.Add(g)
            Next
            Return gList
        End Function

    End Class
End Namespace