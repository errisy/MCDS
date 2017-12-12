Imports System.ComponentModel
Friend Class LigationViewModel
    Implements INotifyPropertyChanged
    Private _DNAInfo As DNAInfo
    Public Sub New(_info As DNAInfo)
        _DNAInfo = _info
    End Sub
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
