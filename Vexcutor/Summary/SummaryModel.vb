Imports System.ComponentModel

Friend Class SummaryModel
    Implements System.ComponentModel.INotifyPropertyChanged
    Public Sub New()
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
    Private _ProjectSummary As String
    Public Property ProjectSummary As String
        Get
            Return _ProjectSummary
        End Get
        Set(value As String)
            _ProjectSummary = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ProjectSummary"))
        End Set
    End Property
    Public ReadOnly Property Primers As New ObjectModel.ObservableCollection(Of PrimerSearchEntry)
    Public ReadOnly Property SyntheticDNAs As New ObjectModel.ObservableCollection(Of SyntheticDNAModel)
    Public ReadOnly Property RestrictionEnzymes As New ObjectModel.ObservableCollection(Of EnzymeModel)
    Public ReadOnly Property ModificationEnzymes As New ObjectModel.ObservableCollection(Of EnzymeModel)

    Public ReadOnly Property CopyAllPrimers As New ViewModelCommand(AddressOf cmdCopyAllPrimers)
    Private Sub cmdCopyAllPrimers(value As Object)
        Dim stb As New System.Text.StringBuilder
        For Each pm In Primers
            stb.AppendLine(String.Format("{0}{1}{2}", pm.Name, vbTab, pm.Sequence))
        Next
        Clipboard.SetText(stb.ToString)
    End Sub
    Public ReadOnly Property CopyNewPrimers As New ViewModelCommand(AddressOf cmdCopyNewPrimers)
    Private Sub cmdCopyNewPrimers(value As Object)
        Dim stb As New System.Text.StringBuilder
        For Each pm In Primers.Where(Function(pc) pc.Synthesis).ToList
            stb.AppendLine(String.Format("{0}{1}{2}", pm.Name, vbTab, pm.Sequence))
        Next
        Clipboard.SetText(stb.ToString)
    End Sub
    Public ReadOnly Property RecordNewPrimers As New ViewModelCommand(AddressOf cmdRecordNewPrimers)
    Private Sub cmdRecordNewPrimers(value As Object)
        'Need to Check if the primers were already saved.
        PrimerRecordManager.Save(Primers.Where(Function(pc) pc.Synthesis).ToList, _Name)
    End Sub
    Public ReadOnly Property CopyAllSyntheticDNAs As New ViewModelCommand(AddressOf cmdCopyAllSyntheticDNAs)
    Private Sub cmdCopyAllSyntheticDNAs(value As Object)
        Dim stb As New System.Text.StringBuilder
        For Each syn In SyntheticDNAs
            stb.AppendLine(String.Format(">{0}{1}{2}", syn.Name, vbCrLf, syn.Sequence))
        Next
        Clipboard.SetText(stb.ToString)
    End Sub
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class


Friend Class SyntheticDNAModel
    Implements System.ComponentModel.INotifyPropertyChanged
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
    Private _Sequence As String
    Public Property Sequence As String
        Get
            Return _Sequence
        End Get
        Set(value As String)
            _Sequence = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Sequence"))
            _Length = _Sequence.Length
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Length"))
        End Set
    End Property
    Public ReadOnly Property Length As Integer
    Public ReadOnly Property Copy As New ViewModelCommand(AddressOf cmdCopy)
    Private Sub cmdCopy(value As Object)
        Clipboard.SetText(String.Format(">{0}{1}{2}", _Name, vbCrLf, _Sequence))
    End Sub
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class

Friend Class EnzymeModel
    Implements System.ComponentModel.INotifyPropertyChanged
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
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class