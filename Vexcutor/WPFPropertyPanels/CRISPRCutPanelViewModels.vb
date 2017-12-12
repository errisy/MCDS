Imports System.ComponentModel
Public Class CRISPRCutDNAInfoViewModel
    Implements INotifyPropertyChanged
    Private _DNAInfo As DNAInfo
    Public Sub New(_info As DNAInfo)
        _DNAInfo = _info
        LoadInfo()
    End Sub
    Public Sub LoadInfo()
        _Suggestion = ""
        If _DNAInfo.Source.Count = 0 Then
            _Suggestion = "No Source Nodes Applied! Please select source nodes."
        Else
            _Suggestion = "Please Select the gRNA Source:"
            For Each scr In _DNAInfo.Source
                Dim item As New GeneFileSelectionViewModel(scr)
                AddHandler item.PropertyChanged, AddressOf IsgRNAChanged
                item.IssgRNA = _DNAInfo.CRISPRgRNA.Contains(scr)
                _Source.Add(item)
            Next
        End If
    End Sub
    Private Sub IsgRNAChanged(sender As Object, e As PropertyChangedEventArgs)
        If e.PropertyName = "IssgRNA" Then
            Dim model As GeneFileSelectionViewModel = sender
            If model.IssgRNA Then
                If Not _DNAInfo.CRISPRgRNA.Contains(model.Node) Then
                    _DNAInfo.CRISPRgRNA.Add(model.Node)
                End If
            Else
                If _DNAInfo.CRISPRgRNA.Contains(model.Node) Then
                    _DNAInfo.CRISPRgRNA.Remove(model.Node)
                End If
            End If
        End If
    End Sub
    Public ReadOnly Property Suggestion As String
    Public ReadOnly Property Source As New ObjectModel.ObservableCollection(Of GeneFileSelectionViewModel)

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class


Public Class GeneFileSelectionViewModel
    Implements INotifyPropertyChanged
    Public Sub New(_SourceInfo As DNAInfo)
        _Node = _SourceInfo
        LoadInfo()
    End Sub
    Public Sub LoadInfo()
        _Warning = ""
        Select Case _Node.DNAs.Count
            Case 0
                _Warning = "No DNA Source!"
            Case 1
                _GeneFile = _Node.DNAs(1)
                _Name = _GeneFile.Name
                _Length = _GeneFile.Length
            Case Else
                _Warning = "More than 1 DNA Source!"
        End Select
    End Sub
    Public ReadOnly Property Node As DNAInfo
    Public ReadOnly Property GeneFile As Nuctions.GeneFile
    Public ReadOnly Property Name As String
    Public ReadOnly Property Length As Integer
    Public ReadOnly Property Warning As String
    Private _IssgRNA As Boolean
    Public Property IssgRNA As Boolean 'utilize PropertyChanged, map to "Model.IsSgRNASource" 
        Get
            Return _IssgRNA
        End Get
        Set(value As Boolean)
            _IssgRNA = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("IssgRNA"))
        End Set
    End Property
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class