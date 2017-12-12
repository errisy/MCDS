Public Class RestrictionEnzymeDefinitions
    Inherits System.Collections.ObjectModel.ObservableCollection(Of RestrictionEnzymeDefinition)
End Class
Public Class RestrictionEnzymeDefinition
    Implements ICutSite
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
    Public Property Sequence As String Implements ICutSite.Sequence
        Get
            Return _Sequence
        End Get
        Set(value As String)
            _Sequence = Nuctions.NFilter(value)
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Sequence"))
        End Set
    End Property
    'Public Property SCut As Integer
    Private _SCut As Integer
    Public Property SCut As Integer Implements ICutSite.SCut
        Get
            Return _SCut
        End Get
        Set(value As Integer)
            _SCut = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("SCut"))
        End Set
    End Property
    'Public Property ACut As Integer
    Private _ACut As Integer
    Public Property ACut As Integer Implements ICutSite.ACut
        Get
            Return _ACut
        End Get
        Set(value As Integer)
            _ACut = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ACut"))
        End Set
    End Property
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class