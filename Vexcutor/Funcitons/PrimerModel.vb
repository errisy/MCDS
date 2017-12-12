Imports System.ComponentModel

Public Class PrimerModel
    Implements System.ComponentModel.INotifyPropertyChanged
    Private _PrimerName As String
    Public Property PrimerName As String
        Get
            Return _PrimerName
        End Get
        Set(value As String)
            _PrimerName = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("PrimerName"))
        End Set
    End Property
    Public ReadOnly Property Primers As New ObjectModel.ObservableCollection(Of PrimerEntry)
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
Public Class PrimerEntry
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
    Private _Hairpin As Single
    Public Property Hairpin As Single
        Get
            Return _Hairpin
        End Get
        Set(value As Single)
            _Hairpin = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Hairpin"))
        End Set
    End Property
    Private _Dimer As Single
    Public Property Dimer As Single
        Get
            Return _Dimer
        End Get
        Set(value As Single)
            _Dimer = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Dimer"))
        End Set
    End Property
    Private _Distance As Integer
    Public Property Distance As Integer
        Get
            Return _Distance
        End Get
        Set(value As Integer)
            _Distance = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Distance"))
        End Set
    End Property
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class