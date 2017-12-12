Public Class SettingEntry
    Public Shared MainUIWindow As frmMain
    Public Shared ReadOnly Property MainUIDispatcher As System.Windows.Threading.Dispatcher
        Get
            Return System.Windows.Threading.Dispatcher.CurrentDispatcher
        End Get
    End Property
    'Public Shared Sub Main()
    '    MainUIWindow.ShowDialog()

    'End Sub

#Region "Shared Resources"
    'Restriction Enzyme List
    Public Shared EnzymeCol As Nuctions.RestrictionEnzymes
    Public Shared gRNAs As gRNADefinitions
    'default translation data used in the translation function
    Public Shared CodonCol As Nuctions.Translation

    'a dictionary to host all the translation data
    Public Shared CodonDatabase As New Dictionary(Of String, Nuctions.Translation)

    'Standard features managed by the software
    Public Shared StdFeatures As New List(Of Nuctions.Feature)

    'Recombination sits managed by the software
    Public Shared RecombinationSiteDict As New Dictionary(Of String, Nuctions.RestrictionEnzyme)
    'This entry records the recombination site groups, such attB and attP are in the same group.
    Public Shared RecombinationSiteGroups As New Dictionary(Of String, List(Of String))

    Public Shared enzCollection As New Collection

    'Public Shared DigestionBuffers As New List(Of Digestion)

    Public Shared GroupCopy As List(Of DNAInfo)

    Friend Shared GroupHost As WorkControl

    Public Shared DigestionBuffer As DigestionBufferData

    'Recent Files
    Public Shared RecentFiles As New RecentFileList
    Public Shared SmallIconList As ImageList
    'Standard Features
    Public Shared StandardFeatureList As New List(Of Nuctions.Feature)
    Public Shared ReadOnly Property CodonTraslation() As Nuctions.Translation
        Get
            Return CodonCol
        End Get
    End Property

    Public Shared ReadOnly RestrictionEnzymeFilePath As String = AppDomain.CurrentDomain.BaseDirectory + "RestrictionEnzymes.xml"
    Public Shared ReadOnly DigestionBufferFilePath As String = AppDomain.CurrentDomain.BaseDirectory + "DigestionBuffers.xml"
    Public Shared ReadOnly RecombinationSiteFilePath As String = AppDomain.CurrentDomain.BaseDirectory + "RecombinationSites.xml"
    Public Shared ReadOnly TranslationTableFile As String = AppDomain.CurrentDomain.BaseDirectory + "Codons.xml"
    Public Shared ReadOnly gRNAFilePath As String = AppDomain.CurrentDomain.BaseDirectory + "gRNAs.xml"
    Public Shared ReadOnly RecentFilesPath As String = AppDomain.CurrentDomain.BaseDirectory + "RecentFiles.xml"
    Public Shared ReadOnly StandardFeaturesPath As String = AppDomain.CurrentDomain.BaseDirectory + "StandardFeatures.xml"
    Public Shared ReadOnly OligosPath As String = AppDomain.CurrentDomain.BaseDirectory + "Oligos.txt"
    Shared Sub New()
        LoadRestrictionEnzymes()
        LoadgRNAs()
        LoadDigestionBufferData()
        LoadRecombinationSites()
        LoadTranslationTable()
    End Sub
#Region "Resources"
    Public Shared Sub LoadRestrictionEnzymes()
        Dim _RestrictionEnzymeDefinitions As RestrictionEnzymeDefinitions
        If IO.File.Exists(RestrictionEnzymeFilePath) Then
            Try
                Dim obj = System.Xaml.XamlServices.Parse(IO.File.ReadAllText(RestrictionEnzymeFilePath))
                If TypeOf obj Is RestrictionEnzymeDefinitions Then _RestrictionEnzymeDefinitions = obj
            Catch ex As Exception
                _RestrictionEnzymeDefinitions = New RestrictionEnzymeDefinitions
            End Try
        Else
            _RestrictionEnzymeDefinitions = New RestrictionEnzymeDefinitions
        End If
        If _RestrictionEnzymeDefinitions Is Nothing Then _RestrictionEnzymeDefinitions = New RestrictionEnzymeDefinitions

        EnzymeCol = New Nuctions.RestrictionEnzymes
        For Each _RestrictionEnzymeDefinition In _RestrictionEnzymeDefinitions
            EnzymeCol.Add(_RestrictionEnzymeDefinition.Name,
                          New Nuctions.RestrictionEnzyme(_RestrictionEnzymeDefinition.Name,
                                                         _RestrictionEnzymeDefinition.Sequence,
                                                         _RestrictionEnzymeDefinition.SCut,
                                                         _RestrictionEnzymeDefinition.ACut))
        Next
    End Sub

    Public Shared Sub LoadgRNAs()
        If IO.File.Exists(SettingEntry.gRNAFilePath) Then
            Try
                Dim obj = System.Xaml.XamlServices.Parse(IO.File.ReadAllText(SettingEntry.gRNAFilePath))
                If TypeOf obj Is gRNADefinitions Then gRNAs = obj
            Catch ex As Exception
                gRNAs = New gRNADefinitions
            End Try
        Else
            gRNAs = New gRNADefinitions
        End If
        If gRNAs Is Nothing Then gRNAs = New gRNADefinitions
    End Sub
    Public Shared Sub LoadRecombinationSites()
        Dim _RecombinationSetDefinitions As RecombinationSetDefinitions
        If IO.File.Exists(SettingEntry.RecombinationSiteFilePath) Then
            Try
                Dim obj = System.Xaml.XamlServices.Parse(IO.File.ReadAllText(SettingEntry.RecombinationSiteFilePath))
                If TypeOf obj Is RecombinationSetDefinitions Then _RecombinationSetDefinitions = obj
            Catch ex As Exception
                _RecombinationSetDefinitions = New RecombinationSetDefinitions
            End Try
        Else
            _RecombinationSetDefinitions = New RecombinationSetDefinitions
        End If
        If _RecombinationSetDefinitions Is Nothing Then _RecombinationSetDefinitions = New RecombinationSetDefinitions

        RecombinationSiteDict = New Dictionary(Of String, Nuctions.RestrictionEnzyme)
        RecombinationSiteGroups = New Dictionary(Of String, List(Of String))
        For Each RecSetDef In _RecombinationSetDefinitions
            Dim _SiteGroup As New List(Of String)
            RecombinationSiteGroups.Add(RecSetDef.Name, _SiteGroup)
            For Each RecSite In RecSetDef
                _SiteGroup.Add(RecSite.Name)
                RecombinationSiteDict.Add(RecSite.Name, New Nuctions.RestrictionEnzyme(RecSite.Name, RecSite.Sequence, RecSite.SCut, RecSite.ACut, RecSite.RecombinationType))
            Next
        Next
    End Sub
    Public Shared Sub LoadDigestionBufferData()
        Dim _DigestionBufferData As DigestionBufferData
        If IO.File.Exists(DigestionBufferFilePath) Then
            Try
                Dim obj = System.Xaml.XamlServices.Parse(IO.File.ReadAllText(DigestionBufferFilePath))
                If TypeOf obj Is DigestionBufferData Then _DigestionBufferData = obj
            Catch ex As Exception
                _DigestionBufferData = New DigestionBufferData
            End Try
        Else
            _DigestionBufferData = New DigestionBufferData
        End If
        If _DigestionBufferData Is Nothing Then _DigestionBufferData = New DigestionBufferData
        DigestionBuffer = _DigestionBufferData
    End Sub

    Public Shared Sub LoadTranslationTable()
        Dim _CodonTable As KEGG.Translation
        If IO.File.Exists(TranslationTableFile) Then
            Try
                Dim obj = System.Xaml.XamlServices.Parse(IO.File.ReadAllText(TranslationTableFile))
                If TypeOf obj Is KEGG.Translation Then _CodonTable = obj
            Catch ex As Exception
                _CodonTable = New KEGG.Translation(True)
            End Try
        Else
            _CodonTable = New KEGG.Translation(True)
        End If
        If _CodonTable Is Nothing Then _CodonTable = New KEGG.Translation(True)
        'translate translation to CodonCol
        CodonCol = New Nuctions.Translation With {.Organism = _CodonTable.Organism}

        'CodonCol.AnimoTable.Add("", New Nuctions.Codon)


        For Each cdn In _CodonTable.CodonTable.Values
            Dim gCodon = New Nuctions.GeneticCode With {.Name = cdn.Codon.Name, .CanStart = cdn.IsStartCodon, .StartRatio = cdn.StartRatio, .ratio = cdn.Ratio}
            Dim c = Nuctions.AnminoAcidParse(cdn.AminoAcid)
            If Not CodonCol.AnimoTable.ContainsKey(c.ShortName) Then CodonCol.AnimoTable.Add(c.ShortName, New Nuctions.Codon With {.FullName = c.FullName, .ShortName = c.ShortName})
            CodonCol.AnimoTable(c.ShortName).CodeList.Add(gCodon)
            CodonCol.CodeTable.Add(gCodon.Name, CodonCol.AnimoTable(c.ShortName))
        Next
    End Sub
    Public Shared Sub LoadStandardFeatures()
        Dim _StandardFeatures As StandardFeatures
        If IO.File.Exists(TranslationTableFile) Then
            Try
                Dim obj = System.Xaml.XamlServices.Parse(IO.File.ReadAllText(StandardFeaturesPath))
                If TypeOf obj Is StandardFeatures Then _StandardFeatures = obj
            Catch ex As Exception
                _StandardFeatures = New StandardFeatures
            End Try
        Else
            _StandardFeatures = New StandardFeatures
        End If
        If _StandardFeatures Is Nothing Then _StandardFeatures = New StandardFeatures
        'translate _StandardFeatures to Features
        StandardFeatureList = New List(Of Nuctions.Feature)
        For Each feature In _StandardFeatures.Features
            Dim f As New Nuctions.Feature(feature.Name, feature.Sequence, feature.Label, feature.Type, feature.Note)
            For Each biof In feature.BioFunction
                f.BioFunctions.Add(New Nuctions.FeatureFunction With {.BioFunction = biof.BioFunction, .Parameters = biof.Parameters})
            Next
            StandardFeatureList.Add(f)
        Next
    End Sub
    Public Shared Sub LoadRecentFiles()
        Dim _RecentFileList As RecentFileList
        If IO.File.Exists(TranslationTableFile) Then
            Try
                Dim obj = System.Xaml.XamlServices.Parse(IO.File.ReadAllText(RecentFilesPath))
                If TypeOf obj Is RecentFileList Then _RecentFileList = obj
            Catch ex As Exception
                _RecentFileList = New RecentFileList
            End Try
        Else
            _RecentFileList = New RecentFileList
        End If
        If _RecentFileList Is Nothing Then _RecentFileList = New RecentFileList
        RecentFiles = _RecentFileList
    End Sub

#End Region
#End Region

#Region "Shared Util Functions"
    Public Shared Function ParseEnzymeString(value As String) As List(Of String)
        Dim str As String() = value.ToLower.Split(" ")
        Dim stList As New List(Of String)
        Dim rList As New List(Of String)

        stList.AddRange(str)

        Dim stb As New System.Text.StringBuilder

        For Each key As Nuctions.RestrictionEnzyme In SettingEntry.EnzymeCol.RECollection
            If stList.IndexOf(key.Name.ToLower) > -1 Then
                stb.Append(key.Name)
                stb.Append(" ")
                rList.Add(key.Name)
            End If
        Next
        Return rList
    End Function
#End Region

End Class
