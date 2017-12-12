Imports System.ComponentModel, System.Threading.Tasks

Public Class OnlineDatabaseImporterModel
    Implements System.ComponentModel.INotifyPropertyChanged

    Public Sub New()
        _KEGG = New KEGGGeneDatabaseModel With {.Output = Entries}
        _NCBI = New NCBIGeneDatabaseModel With {.Output = Entries}
    End Sub
    Public ReadOnly Property Entries As New ObjectModel.ObservableCollection(Of EntryModel)
    Public ReadOnly Property KEGG As KEGGGeneDatabaseModel
    Public ReadOnly Property NCBI As NCBIGeneDatabaseModel

    Public ReadOnly Property Obtain As New ViewModelCommand(AddressOf cmdObtain)
    Private Async Sub cmdObtain(value As Object)
        Dim KEGGList As New List(Of String)
        Dim NCBIList As New List(Of String)

        For Each entry In Entries
            If entry.Selected Then
                Select Case entry.Database
                    Case "KEGG"
                        KEGGList.Add(entry.ID)
                    Case "NCBI"
                        NCBIList.Add(entry.ID)
                End Select
            End If
        Next
        If KEGGList.Count > 0 Then
            _DownloadStatus = "Downloading from KEGG..."
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DownloadStatus"))
            Dim keggDownload As New KEGG.GeneDownloader
            _Downloader = keggDownload
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Downloader"))
            Dim gdList = Await keggDownload.StartDownloadTask(KEGGList)
            For Each gd In gdList
                Dim gf As New Nuctions.GeneFile
                gf.Sequence = gd.NucleotideSequence
                gf.Name = KEGGUtil.RemoveCrLf(gd.Name)
                gf.End_F = "*B"
                gf.End_R = "*B"
                gf.ModificationDate = Now
                Dim f = New Nuctions.Feature(gf.Name, gd.NucleotideSequence, gf.Name, "gene", KEGGUtil.RemoveCrLf(gd.Definition))
                Nuctions.AddFeatures(New List(Of Nuctions.GeneFile) From {gf}, New List(Of Nuctions.Feature) From {f})
                _ObtainedGenes.Add(gf)
            Next
        End If
        If NCBIList.Count > 0 Then
            _DownloadStatus = "Downloading from NCBI..."
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DownloadStatus"))
            Dim ncbiDownload As New NCBI.GeneDownloader
            _Downloader = ncbiDownload
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Downloader"))
            Dim gfList = Await ncbiDownload.StartDownloadTask(NCBIList)
            For Each gf In gfList
                _ObtainedGenes.Add(gf)
            Next
        End If
        _DownloadStatus = "Ready."
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DownloadStatus"))
        _Downloader = Nothing
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Downloader"))
    End Sub
    Public ReadOnly Property DownloadStatus As String = "Ready."
    Public ReadOnly Property Downloader As Object

    Public ReadOnly Property ObtainedGenes As New List(Of Nuctions.GeneFile)
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
Public Class DatabaseModel
    Implements System.ComponentModel.INotifyPropertyChanged
    Public Property Output As ObjectModel.ObservableCollection(Of EntryModel)

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(e As ComponentModel.PropertyChangedEventArgs)
        RaiseEvent PropertyChanged(Me, e)
    End Sub
End Class
Public Class NCBIGeneDatabaseModel
    Inherits DatabaseModel
    Private _Keywords As String = ""
    Public Property Keywords As String
        Get
            Return _Keywords
        End Get
        Set(value As String)
            _Keywords = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("Keywords"))
        End Set
    End Property
    Public ReadOnly Property Search As New ViewModelCommand(AddressOf cmdSearch)
    Protected Async Sub cmdSearch(value As Object)
        If Output Is Nothing Then Return
        If Not EventCommand.IsKeyEventArgsAndKey(value, System.Windows.Input.Key.Enter) Then Return
        Dim t As New Task(Of String)(Function() NCBIUtil.ESEARCH(NCBIDBEnum.nuccore, Keywords, RETMODEEnum.xml, RETMODEEnum.xml, 50))
            t.Start()
            Dim Results = Await t
            Dim XmlDoc As New System.Xml.XmlDocument()
            XmlDoc.LoadXml(Results)
            Dim CountNode As Xml.XmlElement = XmlDoc.GetElementsByTagName("Count")(0)
            Dim count As Integer
            Dim idList As New List(Of String)
            If Integer.TryParse(CountNode.InnerText, count) AndAlso count > 0 Then
                Dim IdListNode As Xml.XmlElement = XmlDoc.GetElementsByTagName("IdList")(0)
                For Each id As Xml.XmlElement In IdListNode.GetElementsByTagName("Id")
                    idList.Add(id.InnerText)
                Next
            End If
            If idList.Count = 0 Then Return
            Dim tSum As New Task(Of String)(Function() NCBIUtil.ESUMMARY(NCBIDBEnum.nuccore, idList))
            tSum.Start()
            Dim Summaries = Await tSum
            Dim XmlDocSum As New System.Xml.XmlDocument
            XmlDocSum.LoadXml(Summaries)
            Dim DocSums = XmlDocSum.GetElementsByTagName("DocSum")
            For Each DocSum As Xml.XmlElement In DocSums
                Dim IdNode As Xml.XmlElement = DocSum.GetElementsByTagName("Id")(0)
                'Dim Items = DocSum.GetElementsByTagName("Item ")

                Dim em As New EntryModel With {.Database = "NCBI", .ID = IdNode.InnerText}
                For Each it As Xml.XmlElement In DocSum.ChildNodes
                    If it.HasAttribute("Name") Then
                        Select Case it.GetAttribute("Name")
                            Case "Title"
                                em.Description = it.InnerText
                            Case "Length"
                                Dim intLength As Integer
                                If Integer.TryParse(it.InnerText, intLength) Then em.Length = intLength
                        End Select
                    End If
                Next
                Output.Add(em)
            Next

    End Sub
End Class


Public Class KEGGGeneDatabaseModel
    Inherits DatabaseModel
    Private _Keywords As String = ""
    Public Property Keywords As String
        Get
            Return _Keywords
        End Get
        Set(value As String)
            _Keywords = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("Keywords"))
        End Set
    End Property
    Public ReadOnly Property Search As New ViewModelCommand(AddressOf cmdSearch)
    Private Shared newlinechars = New Char() {vbCr, vbLf, vbCrLf}
    Private Shared tabchars = New Char() {vbTab}
    Protected Async Sub cmdSearch(value As Object)
        If Output Is Nothing Then Return
        If Not EventCommand.IsKeyEventArgsAndKey(value, System.Windows.Input.Key.Enter) Then Return
        Dim t As New Task(Of String)(Function() As String
                                         Dim wc As New System.Net.WebClient
                                         Dim result As String = ""
                                         Try
                                             result = wc.DownloadString(String.Format("http://rest.kegg.jp/find/{0}/{1}", OrganismKey, Keywords))
                                         Catch ex As Exception

                                         End Try
                                         Return result
                                     End Function)
        t.Start()
        Dim res = Await t
        Dim entries As String() = res.Split(newlinechars, StringSplitOptions.RemoveEmptyEntries)
        For Each ent In entries
            Dim items As String() = ent.Split(tabchars, StringSplitOptions.RemoveEmptyEntries)
            Dim entry As New EntryModel With {.ID = items(0), .Description = items(1), .Database = "KEGG"}
            Output.Add(entry)
        Next
    End Sub
    Private _OrganismKey As String = "genes"
    Public Property OrganismKey As String
        Get
            Return _OrganismKey
        End Get
        Set(value As String)
            _OrganismKey = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("OrganismKey"))
        End Set
    End Property
    Public ReadOnly Property Organisms As New ObjectModel.ObservableCollection(Of KEGGOrganismModel)
    Public ReadOnly Property OrganismIDs As New ObjectModel.ObservableCollection(Of KEGGOrganismModel)
    Private _NameKeywords As String
    Public Property NameKeywords As String
        Get
            Return _NameKeywords
        End Get
        Set(value As String)
            _NameKeywords = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("NameKeywords"))
        End Set
    End Property
    Public ReadOnly Property SearchName As New ViewModelCommand(AddressOf cmdSearchName)
    Private Sub cmdSearchName(value As Object)
        If NameKeywords Is Nothing Then Return
        If Not EventCommand.IsKeyEventArgsAndKey(value, System.Windows.Input.Key.Enter) Then Return
        _OrganismIDs.Clear()
        Dim keys = NameKeywords.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        For Each org In _Organisms
            If org.Name.ContainsAnyOf(keys) Then
                _OrganismIDs.Add(org)
            End If
        Next
    End Sub
    Public Sub New()
        _Organisms.Add(New KEGGOrganismModel With {.ID = "genes", .Name = "All Organisms", .SelectKey = AddressOf SelectKey})
        For Each org In KEGGUtil.OrganismList
            _Organisms.Add(New KEGGOrganismModel(org) With {.SelectKey = AddressOf SelectKey})
        Next
        For Each org In _Organisms
            _OrganismIDs.Add(org)
        Next
    End Sub
    Public Sub SelectKey(id As String)
        OrganismKey = id
    End Sub
End Class

Public Class KEGGOrganismModel
    Implements System.ComponentModel.INotifyPropertyChanged
    Public Sub New()

    End Sub
    Public Sub New(org As KEGG.Organism)
        _Name = org.Name
        _ID = org.Code
    End Sub
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
    Public ReadOnly Property SelectID As New ViewModelCommand(AddressOf cmdSelectID)
    Private Sub cmdSelectID(value As Object)
        SelectKey(ID)
    End Sub
    Public SelectKey As Action(Of String)

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
Public Class EntryModel
    Implements System.ComponentModel.INotifyPropertyChanged
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
    Private _Database As String
    Public Property Database As String
        Get
            Return _Database
        End Get
        Set(value As String)
            _Database = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Database"))
        End Set
    End Property
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
    Private _Description As String
    Public Property Description As String
        Get
            Return _Description
        End Get
        Set(value As String)
            _Description = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Description"))
        End Set
    End Property
    Private _Selected As Boolean = False
    Public Property Selected As Boolean
        Get
            Return _Selected
        End Get
        Set(value As Boolean)
            _Selected = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Selected"))
        End Set
    End Property
    Public ReadOnly Property AccessOnline As New ViewModelCommand(AddressOf cmdAccessOnline)
    Private Sub cmdAccessOnline(value As Object)
        Select Case _Database
            Case "NCBI"
                Process.Start(String.Format("http://www.ncbi.nlm.nih.gov/nuccore/{0}", _ID))
            Case "KEGG"
                Process.Start(String.Format("http://www.genome.jp/dbget-bin/www_bget?{0}", _ID))
        End Select
    End Sub
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class