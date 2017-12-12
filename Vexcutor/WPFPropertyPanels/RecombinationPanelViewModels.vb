Imports System.ComponentModel

Public Class RecombinationDNAInfoViewModel
    Implements INotifyPropertyChanged
    Private _DNAInfo As DNAInfo
    Public Sub New(_info As DNAInfo)
        _DNAInfo = _info
        _RecombinationMethods.Add("Homologous Recombination")
        _RecombinationMethods.Add("Lambda Red Recombination")
        _RecombinationMethods.Add("in vitro Annealing")
        _RecombinationMethods.Add("in vivo Annealing")
        For Each _GroupName In SettingEntry.RecombinationSiteGroups.Keys
            _RecombinationMethods.Add(_GroupName)
        Next
    End Sub
    Public Property RecombinationMethod As String
        Get
            Return _DNAInfo.RecombinationMethodString
        End Get
        Set(value As String)
            _DNAInfo.RecombinationMethodString = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("RecombinationMethod"))
        End Set
    End Property
    Private _RecombinationMethods As New ObjectModel.ObservableCollection(Of String)
    Public ReadOnly Property RecombinationMethods As ObjectModel.ObservableCollection(Of String)
        Get
            Return _RecombinationMethods
        End Get
    End Property
    Public Property IsExhuastiveAssembly As Boolean
        Get
            Return _DNAInfo.IsExhaustiveAssembly
        End Get
        Set(value As Boolean)
            _DNAInfo.IsExhaustiveAssembly = value
            If _DNAInfo.IsExhaustiveAssembly Then
                _DNAInfo.TimesForAssembly = 1
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Times"))
            Else
                _DNAInfo.TimesForAssembly = 2
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Times"))
            End If
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("IsExhuastiveAssembly"))
        End Set
    End Property
    Public Property Times As Integer
        Get
            Return _DNAInfo.TimesForAssembly
        End Get
        Set(value As Integer)
            _DNAInfo.TimesForAssembly = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Times"))
        End Set
    End Property
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class