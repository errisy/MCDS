Public Class PCRViewModel
    Implements System.ComponentModel.INotifyPropertyChanged
    'Public Property Primers As System.Collections.ObjectModel.ObservableCollection(Of PrimerViewModel)
    Private _Primers As System.Collections.ObjectModel.ObservableCollection(Of PrimerViewModel)
    Public Property Primers As System.Collections.ObjectModel.ObservableCollection(Of PrimerViewModel)
        Get
            Return _Primers
        End Get
        Set(value As System.Collections.ObjectModel.ObservableCollection(Of PrimerViewModel))
            _Primers = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Primers"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

Public Class PrimerViewModel
    Implements System.ComponentModel.INotifyPropertyChanged
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
    'Public Property Sequence As String
    Private _Sequence As String
    Public Property Sequence As String
        Get
            Return _Sequence
        End Get
        Set(value As String)
            _Sequence = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Sequence"))
        End Set
    End Property
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

