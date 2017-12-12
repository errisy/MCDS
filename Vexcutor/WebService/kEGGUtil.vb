Imports System.Text.RegularExpressions, System.Collections.ObjectModel

Public Class KEGGUtil
    Public Shared Function FindGene(keyword As String) As List(Of KEGG.Gene)
        Dim dc As New System.Net.WebClient
        Dim value = dc.DownloadString(System.Net.WebUtility.HtmlEncode(String.Format("http://rest.genome.jp/find/genes/{0}", keyword)))

        Dim lines = SectionDivider.Divide(value, "(^|\n)", Text.RegularExpressions.RegexOptions.IgnoreCase)

        Dim genes As New List(Of KEGG.Gene)
        For Each L In lines
            If L.Length = 0 Then Continue For
            Dim values = L.Split(New Char() {vbTab}, StringSplitOptions.RemoveEmptyEntries)
            genes.Add(New KEGG.Gene With {.ID = values(0), .Definition = values(1)})
        Next
        Return genes
    End Function
    Public Shared Function FindGeneName(keyword As String) As List(Of String)
        Dim dc As New System.Net.WebClient
        Dim value = dc.DownloadString(System.Net.WebUtility.HtmlEncode(String.Format("http://rest.genome.jp/find/genes/{0}", keyword)))

        Dim lines = SectionDivider.Divide(value, "(^|\n)", Text.RegularExpressions.RegexOptions.IgnoreCase)

        Dim genes As New List(Of String)
        For Each L In lines
            If L.Length = 0 Then Continue For
            Dim values = L.Split(New Char() {vbTab}, StringSplitOptions.RemoveEmptyEntries)
            genes.Add(values(0))
        Next
        Return genes
    End Function

    Public Shared Function RemoveCrLf(value As String) As String
        Return value.Replace(vbCr, "").Replace(vbLf, "")
    End Function
    Public Shared Function Organisms() As List(Of KEGG.Organism)
        Dim orgs As New List(Of KEGG.Organism)


        Dim dir As String = GetDirectory("KEGGOrganism")
        Dim files = IO.Directory.GetFiles(dir, "*.org")
        If files.Length = 0 Then
            Dim dc As New System.Net.WebClient
            Dim value = dc.DownloadString("http://rest.genome.jp/list/organism")
            Dim lines = SectionDivider.Divide(value, "(^|\n)", Text.RegularExpressions.RegexOptions.IgnoreCase)
            For Each L In lines
                If L.Length = 0 Then Continue For
                Dim values = L.Split(New Char() {vbTab}, StringSplitOptions.RemoveEmptyEntries)
                Dim org = New KEGG.Organism With {.ID = values(0), .Code = values(1), .Name = values(2), .Taxonomy = values(3)}
                orgs.Add(org)
                SaveOrganism(org.Code, org)
            Next
        Else
            For Each f In files
                Dim org = DeserializeFile(f)
                orgs.Add(org)
            Next
        End If

        Return orgs
    End Function
    Public Shared Sub UpdateOrganisms()
        _OrganismList = Nothing
        Dim dc As New System.Net.WebClient
        Dim value = dc.DownloadString("http://rest.genome.jp/list/organism")
        Dim lines = SectionDivider.Divide(value, "(^|\n)", Text.RegularExpressions.RegexOptions.IgnoreCase)
        For Each L In lines
            If L.Length = 0 Then Continue For
            Dim values = L.Split(New Char() {vbTab}, StringSplitOptions.RemoveEmptyEntries)
            Dim org = New KEGG.Organism With {.ID = values(0), .Code = values(1), .Name = values(2), .Taxonomy = values(3)}
            SaveOrganism(org.Code, org)
        Next
        SaveOrganism("pg", New KEGG.Organism With {.ID = "", .Code = "pg", .Name = "Plasmid", .Taxonomy = "Plasmid"})
        SaveOrganism("vg", New KEGG.Organism With {.ID = "", .Code = "vg", .Name = "Viruses", .Taxonomy = "Viruses"})
    End Sub
    Private Shared Sub SaveOrganism(CID As String, cpd As KEGG.Organism)
        Dim filename = GetDirectory("KEGGOrganism") + CID + ".org"
        SerializeToFile(filename, cpd)
    End Sub
    Private Shared Function LoadOrganism(CID As String) As KEGG.Organism
        Dim filename = GetDirectory("KEGGOrganism") + CID + ".org"
        Return DeserializeFile(filename)
    End Function

    Private Shared _OrganismList As List(Of KEGG.Organism)
    Public Shared ReadOnly Property OrganismList As List(Of KEGG.Organism)
        Get
            If _OrganismList Is Nothing Then
                _OrganismList = Organisms()
            End If
            Return _OrganismList
        End Get
    End Property

    Public Shared Function ListGenesForOrganism(code As String) As List(Of KEGG.Gene)
        Dim dc As New System.Net.WebClient
        Dim value = dc.DownloadString(System.Net.WebUtility.HtmlEncode(String.Format("http://rest.genome.jp/list/{0}", code)))

        Dim lines = SectionDivider.Divide(value, "(^|\n)", Text.RegularExpressions.RegexOptions.IgnoreCase)

        Dim genes As New List(Of KEGG.Gene)
        For Each L In lines
            If L.Length = 0 Then Continue For
            Dim values = L.Split(New Char() {vbTab}, StringSplitOptions.RemoveEmptyEntries)
            genes.Add(New KEGG.Gene With {.ID = values(0), .Definition = values(1)})
        Next
        Return genes
    End Function

    Public Shared Function ListGeneIDsForOrganism(code As String) As List(Of String)
        Dim dc As New System.Net.WebClient
        Dim value = dc.DownloadString(System.Net.WebUtility.HtmlEncode(String.Format("http://rest.genome.jp/list/{0}", code)))

        Dim lines = SectionDivider.Divide(value, "(^|\n)", Text.RegularExpressions.RegexOptions.IgnoreCase)

        Dim genes As New List(Of String)
        For Each L In lines
            If L.Length = 0 Then Continue For
            Dim values = L.Split(New Char() {vbTab}, StringSplitOptions.RemoveEmptyEntries)
            genes.Add(values(0))
        Next
        Return genes
    End Function

    Public Shared Function GetGeneDetail(GeneID As String) As KEGG.GeneDetail
        If GeneExists(GeneID) Then Return LoadGene(GeneID)
        Dim dc As New System.Net.WebClient

        Dim value As String = Nothing

        While value Is Nothing OrElse value.IndexOf("///") < 0
            Try
                value = dc.DownloadString(System.Net.WebUtility.HtmlEncode(String.Format("http://rest.genome.jp/get/{0}", GeneID)))
            Catch ex As Exception

            End Try
        End While

        Return AnalyzeGeneDetail(value)
    End Function
    Public Shared Sub CheckGeneOrganism(GID As String, gd As KEGG.GeneDetail)
        If gd.Organism.Taxonomy Is Nothing Then
            Dim res = OrganismList.Where(Function(org) org.Code = gd.Organism.Code)
            If res.Any Then
                gd.Organism = OrganismList.Where(Function(org) org.Code = gd.Organism.Code).First
            Else
                UpdateOrganisms()
                res = OrganismList.Where(Function(org) org.Code = gd.Organism.Code)
                gd.Organism = OrganismList.Where(Function(org) org.Code = gd.Organism.Code).First
            End If
        End If
        Dim filename = GetDirectory("KEGGGene") + GID.Replace(":", "#") + ".gn"
        SerializeToFile(filename, gd)
    End Sub
    Private Shared Sub SaveGene(GID As String, gd As KEGG.GeneDetail)
        Dim filename = GetDirectory("KEGGGene") + GID.Replace(":", "#") + ".gn"
        SerializeToFile(filename, gd)
    End Sub
    Private Shared Function LoadGene(GID As String) As KEGG.GeneDetail
        Dim filename = GetDirectory("KEGGGene") + GID.Replace(":", "#") + ".gn"
        Return DeserializeFile(filename)
    End Function
    Private Shared Function GeneExists(GID As String) As Boolean
        Return IO.File.Exists(GetDirectory("KEGGGene") + GID.Replace(":", "#") + ".gn")
    End Function
    Public Shared Function GetCodonUsage(code As String) As KEGG.Translation
        Dim trans As KEGG.Translation
        If Not IO.Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\KEGGCodonUsage") Then IO.Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + "\KEGGCodonUsage")
        Dim filename As String = System.AppDomain.CurrentDomain.BaseDirectory + "\KEGGCodonUsage\" + code + ".tsl"
        If IO.File.Exists(filename) Then
            Using fs = IO.File.OpenRead(filename)
                Dim bf = New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter()
                trans = bf.Deserialize(fs)
            End Using
            Return trans
        Else
            MsgBox("Please make translation file first!")
            Return Nothing
        End If
    End Function
    Private Shared Function AnalyzeGeneDetail(value As String) As KEGG.GeneDetail
        Dim sections = SectionDivider.Divide(value, "(^|\n)\w+", Text.RegularExpressions.RegexOptions.IgnoreCase)

        Dim gd As New KEGG.GeneDetail

        Dim orgs = OrganismList

        'Organisms
        Dim Organisms = SectionDivider.SelectSection(sections, "^ORGANISM", Text.RegularExpressions.RegexOptions.IgnoreCase)
        If Organisms.Count > 0 AndAlso Organisms(0).Length > 0 Then
            Dim m = Regex.Match(Organisms(0), "^ORGANISM\s+(\w+)\s+([\w\W]+)")
            gd.Organism = New KEGG.Organism With {.Code = m.Groups(1).Value, .Name = m.Groups(2).Value}
            Dim orgres = orgs.Where(Function(org) org.Code = gd.Organism.Code)
            If orgres.Any Then
                Dim orgmatch = orgres.First
                gd.Organism.ID = orgmatch.ID
                gd.Organism.Taxonomy = orgmatch.Taxonomy
                gd.Organism.Name = orgmatch.Name
            End If

        End If

        'Entry
        Dim Entries = SectionDivider.SelectSection(sections, "^ENTRY", Text.RegularExpressions.RegexOptions.IgnoreCase)
        If Entries.Count > 0 AndAlso Entries(0).Length > 0 AndAlso gd.Organism IsNot Nothing Then
            Dim m = Regex.Match(Entries(0), "^ENTRY\s+([\w\.\-]+)\s+(\w+)\s+(\w+)")
            gd.ID = gd.Organism.Code + ":" + m.Groups(1).Value
            gd.GeneType = m.Groups(2).Value
        End If

        'Names
        Dim Names = SectionDivider.SelectSection(sections, "^NAME", Text.RegularExpressions.RegexOptions.IgnoreCase)
        If Names.Count > 0 AndAlso Names(0).Length > 0 Then
            Dim m = Regex.Match(Names(0), "^NAME\s+([\w\W]+)")
            gd.Name = m.Groups(1).Value
        End If

        'Definitions
        Dim Definitions = SectionDivider.SelectSection(sections, "^DEFINITION", Text.RegularExpressions.RegexOptions.IgnoreCase)
        If Definitions.Count > 0 AndAlso Definitions(0).Length > 0 Then
            Dim m = Regex.Match(Definitions(0), "^DEFINITION\s+([\w\W]+)")
            gd.Definition = m.Groups(1).Value
        End If

        'Orthologies
        Dim Orthologies = SectionDivider.SelectSection(sections, "^ORTHOLOGY", Text.RegularExpressions.RegexOptions.IgnoreCase)
        If Orthologies.Count > 0 AndAlso Orthologies(0).Length > 0 Then
            Dim m = Regex.Match(Definitions(0), "^ORTHOLOGY\s+(\w+)\s+([\w\W]+)")
            gd.Orthology = New KEGG.Orthology With {.ID = m.Groups(1).Value, .Name = m.Groups(2).Value}
        End If

        'Positions
        Dim Positions = SectionDivider.SelectSection(sections, "^POSITION", Text.RegularExpressions.RegexOptions.IgnoreCase)
        If Positions.Count > 0 AndAlso Positions(0).Length > 0 Then
            Dim m = Regex.Match(Positions(0), "^POSITION\s+([\w\W]+)")
            gd.Position = m.Groups(1).Value
        End If

        'Pathways
        Dim Pathways = SectionDivider.SelectSection(sections, "^PATHWAY", Text.RegularExpressions.RegexOptions.IgnoreCase)
        If Pathways.Count > 0 AndAlso Pathways(0).Length > 0 Then
            Dim pw As String = Pathways(0).Substring(8)
            For Each m As System.Text.RegularExpressions.Match In Regex.Matches(pw, "(^|\n)\s+(\w+)\s+([\w\W^\n]+)")
                Dim path As New KEGG.Map With {.ID = m.Groups(1).Value, .Name = m.Groups(2).Value}
                gd.Pathways.Add(path)
            Next
        End If


        'AASEQs
        Dim AASEQs = SectionDivider.SelectSection(sections, "^AASEQ", Text.RegularExpressions.RegexOptions.IgnoreCase)
        If AASEQs.Count > 0 AndAlso AASEQs(0).Length > 0 Then
            Dim lines = AASEQs(0).Split(New Char() {vbCr, vbLf, vbCrLf}, StringSplitOptions.RemoveEmptyEntries)
            'Dim m = Regex.Match(lines(0), "^AASEQ\s+(\d+)")
            Dim stb As New System.Text.StringBuilder
            For i = 1 To lines.Length - 1
                stb.Append(Nuctions.AminoAcidFilter(lines(i)))
            Next
            gd.AminoAcidSequence = stb.ToString
        End If

        'NTSEQs
        Dim NTSEQs = SectionDivider.SelectSection(sections, "^NTSEQ", Text.RegularExpressions.RegexOptions.IgnoreCase)
        If NTSEQs.Count > 0 AndAlso NTSEQs(0).Length > 0 Then
            Dim lines = NTSEQs(0).Split(New Char() {vbCr, vbLf, vbCrLf}, StringSplitOptions.RemoveEmptyEntries)
            'Dim m = Regex.Match(lines(0), "^AASEQ\s+(\d+)")
            Dim stb As New System.Text.StringBuilder
            For i = 1 To lines.Length - 1
                stb.Append(Nuctions.TAGCFilter(lines(i)))
            Next
            gd.NucleotideSequence = stb.ToString
            gd.Length = gd.NucleotideSequence.Length
        End If
        gd.CalculateHydrophobicity()
        SaveGene(gd.ID, gd)
        Return gd
    End Function

    Public Shared Function GetMultipleGeneDetail(GeneIDs As IEnumerable(Of String)) As List(Of KEGG.GeneDetail)
        Dim dc As New System.Net.WebClient

        Dim values As String = Nothing


        Dim IDs As New List(Of String)
        Dim gds As New List(Of KEGG.GeneDetail)

        For Each gid In GeneIDs
            If GeneExists(gid) Then
                Dim lgd As KEGG.GeneDetail
                Try
                    lgd = LoadGene(gid)
                Catch ex As Exception

                End Try
                If lgd Is Nothing Then
                    gds.Add(GetGeneDetail(gid))
                Else
                    gds.Add(lgd)
                End If

            Else
                IDs.Add(gid)
            End If
        Next

        Dim count As Integer = IDs.Count

        If count > 0 Then
            While values Is Nothing OrElse Regex.Matches(values, "///").Count <> count
                Try
                    values = dc.DownloadString(System.Net.WebUtility.HtmlEncode(String.Format("http://rest.genome.jp/get/{0}", String.Join("+", IDs))))
                Catch ex As Exception

                End Try
                'If Regex.Matches(values, "///").Count <> count Then Stop
            End While

            If values IsNot Nothing Then
                Dim secs = SectionDivider.Divide(values, "(^|\n)ENTRY", RegexOptions.IgnoreCase)

                For Each sec In secs
                    gds.Add(AnalyzeGeneDetail(sec))
                Next
            End If

        End If

        Return gds
    End Function
End Class

Namespace KEGG
    <Serializable>
    Public Class Gene
        Implements System.ComponentModel.INotifyPropertyChanged
        'Public Property ID As String
        Private _ID As String
        Public Property ID As String
            Get
                Return _ID
            End Get
            Set(value As String)
                _ID = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ID"))
            End Set
        End Property
        'Public Property Description As String
        Private _Definition As String
        Public Property Definition As String
            Get
                Return _Definition
            End Get
            Set(value As String)
                _Definition = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Description"))
            End Set
        End Property
#Region "Functions"
        Public Function GetDetail() As GeneDetail
            Return KEGGUtil.GetGeneDetail(ID)
        End Function
#End Region
        <NonSerialized> Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class
    <Serializable>
    Public Class GeneDetail
        Implements System.ComponentModel.INotifyPropertyChanged
        'Public Property ID As String
        Private _ID As String
        Public Property ID As String
            Get
                Return _ID
            End Get
            Set(value As String)
                _ID = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ID"))
            End Set
        End Property
        'Public Property Name As String
        Private _Name As String
        Public Property Name As String
            Get
                Return _Name
            End Get
            Set(value As String)
                _Name = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Name"))
            End Set
        End Property
        'Public Property Definition As String
        Private _Definition As String
        Public Property Definition As String
            Get
                Return _Definition
            End Get
            Set(value As String)
                _Definition = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Definition"))
            End Set
        End Property
        'Public Property GeneType As String
        Private _GeneType As String
        Public Property GeneType As String
            Get
                Return _GeneType
            End Get
            Set(value As String)
                _GeneType = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("GeneType"))
            End Set
        End Property

        'Public Property Orthology As Orthology
        Private _Orthology As Orthology
        Public Property Orthology As Orthology
            Get
                Return _Orthology
            End Get
            Set(value As Orthology)
                _Orthology = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Orthology"))
            End Set
        End Property
        'Public Property Organism As Organism
        Private _Organism As Organism
        Public Property Organism As Organism
            Get
                Return _Organism
            End Get
            Set(value As Organism)
                _Organism = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Organism"))
            End Set
        End Property

        'Public Property Position As String
        Private _Position As String
        Public Property Position As String
            Get
                Return _Position
            End Get
            Set(value As String)
                _Position = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Position"))
            End Set
        End Property
        'Public Property Pathways As ObservableCollection(Of Pathway)
        Private _Pathways As New ObservableCollection(Of Map)
        Public ReadOnly Property Pathways As ObservableCollection(Of Map)
            Get
                Return _Pathways
            End Get
        End Property
        'Public Property Length As Integer
        Private _Length As Integer
        Public Property Length As Integer
            Get
                Return _Length
            End Get
            Set(value As Integer)
                _Length = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Length"))
            End Set
        End Property

        'Public Property AminoAcidSequence As String
        Private _AminoAcidSequence As String
        Public Property AminoAcidSequence As String
            Get
                Return _AminoAcidSequence
            End Get
            Set(value As String)
                _AminoAcidSequence = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("AminoAcidSequence"))
            End Set
        End Property
        'Public Property NucleotideSequence As String
        Private _NucleotideSequence As String
        Public Property NucleotideSequence As String
            Get
                Return _NucleotideSequence
            End Get
            Set(value As String)
                _NucleotideSequence = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("NucleotideSequence"))
            End Set
        End Property
        'Public Property AverageCodonUsage As Double
        Private _AverageCodonUsage As Double
        Public Property AverageCodonUsage As Double
            Get
                Return _AverageCodonUsage
            End Get
            Set(value As Double)
                _AverageCodonUsage = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("AverageCodonUsage"))
            End Set
        End Property
        'Public Property CodonUsageStandardDeviation As Double
        Private _CodonUsageStandardDeviation As Double
        Public Property CodonUsageStandardDeviation As Double
            Get
                Return _CodonUsageStandardDeviation
            End Get
            Set(value As Double)
                _CodonUsageStandardDeviation = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("CodonUsageStandardDeviation"))
            End Set
        End Property
        'Public Property AverageHydrophobicity As Double
        Private _AverageHydrophobicity As Double
        Public Property AverageHydrophobicity As Double
            Get
                Return _AverageHydrophobicity
            End Get
            Set(value As Double)
                _AverageHydrophobicity = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("AverageHydrophobicity"))
            End Set
        End Property
        'Public Property HydrophobicityStandardDeviation As Double
        Private _HydrophobicityStandardDeviation As Double
        Public Property HydrophobicityStandardDeviation As Double
            Get
                Return _HydrophobicityStandardDeviation
            End Get
            Set(value As Double)
                _HydrophobicityStandardDeviation = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("HydrophobicityStandardDeviation"))
            End Set
        End Property


#Region "Functions"
        Public Function GetCodonUsage() As Translation
            Dim trans As Translation
            If Not IO.Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\KEGGCodonUsage") Then IO.Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + "\KEGGCodonUsage")
            Dim filename As String = System.AppDomain.CurrentDomain.BaseDirectory + "\KEGGCodonUsage\" + Organism.Code + ".tsl"
            If IO.File.Exists(filename) Then
                Using fs = IO.File.OpenRead(filename)
                    Dim bf = New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter()
                    trans = bf.Deserialize(fs)
                End Using
                Return trans
            Else
                MsgBox("Please make translation file first!")
                Return Nothing
            End If
        End Function
        Public Sub CalculateCodonUsage()
            Dim total As Double = 0.0#, avg As Double = 0.0#
            Dim std2 As Double = 0.0#, std As Double = 0.0#
            Dim cnt As Integer = 0
            Dim cus As New List(Of Double)

            Dim trans As Translation = GetCodonUsage()
            If trans Is Nothing Then Return

            Dim startvalue As Double = trans.CodonTable(NucleotideSequence.Substring(0, 3)).StartRatio
            cus.Add(startvalue)
            total += startvalue
            cnt += 1

            For i As Integer = 3 To NucleotideSequence.Count - 6
                Dim value As Double = trans.CodonTable(NucleotideSequence.Substring(i, 3)).Ratio
                cus.Add(value)
                total += value
                cnt += 1
            Next

            avg = total / cnt
            For Each value In cus
                Dim dif = value - avg
                std2 += dif * dif
            Next
            std = Math.Sqrt(std2 / (cnt - 1))
            AverageCodonUsage = avg
            CodonUsageStandardDeviation = std
        End Sub
        Public Sub CalculateCAI()
            Dim total As Double = 0.0#, avg As Double = 0.0#
            Dim std2 As Double = 0.0#, std As Double = 0.0#
            Dim cnt As Integer = 0
            Dim cus As New List(Of Double)

            Dim trans As Translation = GetCodonUsage()

            For Each aa In trans.AminoTable.Values
                aa.Sort()
            Next

            If trans Is Nothing Then Return

            Dim startvalue As Double = trans.CodonTable(NucleotideSequence.Substring(0, 3)).StartRatio
            cus.Add(startvalue)
            total += startvalue
            cnt += 1

            For i As Integer = 3 To NucleotideSequence.Count - 6
                Dim value As Double = trans.CodonTable(NucleotideSequence.Substring(i, 3)).Ratio
                cus.Add(value)
                total += value
                cnt += 1
            Next

            avg = total / cnt
            For Each value In cus
                Dim dif = value - avg
                std2 += dif * dif
            Next
            std = Math.Sqrt(std2 / (cnt - 1))
            AverageCodonUsage = avg
            CodonUsageStandardDeviation = std
        End Sub
        Public Sub CalculateHydrophobicity()
            If AminoAcidSequence Is Nothing OrElse AminoAcidSequence.Length = 0 Then Return
            Dim total As Double = 0.0#, avg As Double = 0.0#
            Dim std2 As Double = 0.0#, std As Double = 0.0#
            Dim cnt As Integer = 0
            Dim hdb As New List(Of Double)
            For Each aa In AminoAcidSequence.ToCharArray
                Dim value As Double = HydrophobicityTable.GetHydrophobicity(aa)
                hdb.Add(value)
                total += value
                cnt += 1
            Next
            avg = total / cnt
            For Each value In hdb
                Dim dif = value - avg
                std2 += dif * dif
            Next
            std = Math.Sqrt(std2 / (cnt - 1))
            AverageHydrophobicity = avg
            HydrophobicityStandardDeviation = std
        End Sub
#End Region

        <NonSerialized> Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class

    <Serializable>
    Public Class Organism
        Implements System.ComponentModel.INotifyPropertyChanged
        'Public Property ID As String
        Private _ID As String
        Public Property ID As String
            Get
                Return _ID
            End Get
            Set(value As String)
                _ID = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ID"))
            End Set
        End Property
        'Public Property Code As String
        Private _Code As String
        Public Property Code As String
            Get
                Return _Code
            End Get
            Set(value As String)
                _Code = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Code"))
            End Set
        End Property
        'Public Property Name As String
        Private _Name As String
        Public Property Name As String
            Get
                Return _Name
            End Get
            Set(value As String)
                _Name = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Name"))
            End Set
        End Property
        'Public Property Taxonomy  As String
        Private _Taxonomy As String
        Public Property Taxonomy As String
            Get
                Return _Taxonomy
            End Get
            Set(value As String)
                _Taxonomy = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Taxonomy "))
            End Set
        End Property
#Region "Functions"
        'Public Function CalculateCodonPreference() As TranslationTables

        'End Function
        Public Shared Operator =(org1 As Organism, org2 As Organism)
            Return org1.Code.ToLower = org2.Code.ToLower
        End Operator
        Public Shared Operator <>(org1 As Organism, org2 As Organism)
            Return org1.Code.ToLower <> org2.Code.ToLower
        End Operator
#End Region
        Protected Sub OnPropertyChanged(e As ComponentModel.PropertyChangedEventArgs)
            RaiseEvent PropertyChanged(Me, e)
        End Sub
        <NonSerialized> Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class

    Public Class OrganismComparer
        Implements IEqualityComparer(Of Organism)
        Public Function Equals1(x As Organism, y As Organism) As Boolean Implements IEqualityComparer(Of Organism).Equals
            Return x.Code.ToLower = y.Code.ToLower
        End Function
        Public Function GetHashCode1(obj As Organism) As Integer Implements IEqualityComparer(Of Organism).GetHashCode
            Return obj.Code.ToLower.GetHashCode
        End Function
    End Class

    <Serializable>
    Public Class Map
        Implements System.ComponentModel.INotifyPropertyChanged
        'Public Property ID As String
        Private _ID As String
        Public Property ID As String
            Get
                Return _ID
            End Get
            Set(value As String)
                _ID = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ID"))
            End Set
        End Property
        'Public Property Name As String
        Private _Name As String
        Public Property Name As String
            Get
                Return _Name
            End Get
            Set(value As String)
                _Name = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Name"))
            End Set
        End Property

        <NonSerialized> Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class
    <Serializable>
    Public Class Orthology
        Implements System.ComponentModel.INotifyPropertyChanged
        'Public Property ID As String
        Private _ID As String
        Public Property ID As String
            Get
                Return _ID
            End Get
            Set(value As String)
                _ID = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ID"))
            End Set
        End Property
        'Public Property Name As String
        Private _Name As String
        Public Property Name As String
            Get
                Return _Name
            End Get
            Set(value As String)
                _Name = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Name"))
            End Set
        End Property
        <NonSerialized> Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class
    <Serializable>
    Public Class OrthologyDetail
        Implements System.ComponentModel.INotifyPropertyChanged
        'Public Property ID As String
        Private _ID As String
        Public Property ID As String
            Get
                Return _ID
            End Get
            Set(value As String)
                _ID = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ID"))
            End Set
        End Property
        'Public Property Name As String
        Private _Name As String
        Public Property Name As String
            Get
                Return _Name
            End Get
            Set(value As String)
                _Name = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Name"))
            End Set
        End Property
        'Public Property Definition As String
        Private _Definition As String
        Public Property Definition As String
            Get
                Return _Definition
            End Get
            Set(value As String)
                _Definition = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Definition"))
            End Set
        End Property

        ''Public Property OrthologyMaps As ObservableCollection(Of OrthologyMap)
        'Private _OrthologyMaps As New ObservableCollection(Of OrthologyMap)
        'Public ReadOnly Property OrthologyMaps As ObservableCollection(Of OrthologyMap)
        '    Get
        '        Return _OrthologyMaps
        '    End Get
        'End Property
        ''Public Property Diseases As ObservableCollection(Of Disease)
        'Private _Diseases As New ObservableCollection(Of Disease)
        'Public ReadOnly Property Diseases As ObservableCollection(Of Disease)
        '    Get
        '        Return _Diseases
        '    End Get
        'End Property
        'Public Property Organisms As ObservableCollection(Of Organism)
        Private _Organisms As New ObservableCollection(Of Organism)
        Public ReadOnly Property Organisms As ObservableCollection(Of Organism)
            Get
                Return _Organisms
            End Get
        End Property
        'Public Property Genes As ObservableCollection(Of Gene)
        Private _Genes As New ObservableCollection(Of Gene)
        Public ReadOnly Property Genes As ObservableCollection(Of Gene)
            Get
                Return _Genes
            End Get
        End Property
        <NonSerialized> Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace


