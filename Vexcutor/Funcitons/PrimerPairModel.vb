Imports System.ComponentModel

Public Class PrimerPairListModel
    Implements System.ComponentModel.INotifyPropertyChanged
    Public ReadOnly Property PrimerPairs As New ObjectModel.ObservableCollection(Of PrimerPairModel)
    Public ReadOnly Property Search As New ViewModelCommand(AddressOf cmdSearch)
    Private Sub cmdSearch(value As Object)
        SearchPrimers(Me, value)
    End Sub
    Public SearchPrimers As EventHandler
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class

Public Class PrimerPairModel
    Implements System.ComponentModel.INotifyPropertyChanged
    Private _FSequence As String
    Public Property FSequence As String
        Get
            Return _FSequence
        End Get
        Set(value As String)
            _FSequence = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("FSequence"))
        End Set
    End Property
    Private _RSequence As String
    Public Property RSequence As String
        Get
            Return _RSequence
        End Get
        Set(value As String)
            _RSequence = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("RSequence"))
        End Set
    End Property
    Private _FLength As Integer
    Public Property FLength As Integer
        Get
            Return _FLength
        End Get
        Set(value As Integer)
            _FLength = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("FLength"))
        End Set
    End Property
    Private _RLength As Integer
    Public Property RLength As Integer
        Get
            Return _RLength
        End Get
        Set(value As Integer)
            _RLength = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("RLength"))
        End Set
    End Property
    Private _FTm As Single
    Public Property FTm As Single
        Get
            Return _FTm
        End Get
        Set(value As Single)
            _FTm = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("FTm"))
        End Set
    End Property
    Private _RTm As Single
    Public Property RTm As Single
        Get
            Return _RTm
        End Get
        Set(value As Single)
            _RTm = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("RTm"))
        End Set
    End Property
    Private _FHairpin As Single
    Public Property FHairpin As Single
        Get
            Return _FHairpin
        End Get
        Set(value As Single)
            _FHairpin = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("FHairpin"))
        End Set
    End Property
    Private _RHairpin As Single
    Public Property RHairpin As Single
        Get
            Return _RHairpin
        End Get
        Set(value As Single)
            _RHairpin = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("RHairpin"))
        End Set
    End Property
    Private _FDimer As Single
    Public Property FDimer As Single
        Get
            Return _FDimer
        End Get
        Set(value As Single)
            _FDimer = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("FDimer"))
        End Set
    End Property
    Private _RDimer As Single
    Public Property RDimer As Single
        Get
            Return _RDimer
        End Get
        Set(value As Single)
            _RDimer = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("RDimer"))
        End Set
    End Property
    Private _CrossDimer As Single
    Public Property CrossDimer As Single
        Get
            Return _CrossDimer
        End Get
        Set(value As Single)
            _CrossDimer = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("CrossDimer"))
        End Set
    End Property
    Private _FDistance As Integer
    Public Property FDistance As Integer
        Get
            Return _FDistance
        End Get
        Set(value As Integer)
            _FDistance = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("FDistance"))
        End Set
    End Property
    Private _RDistance As Integer
    Public Property RDistance As Integer
        Get
            Return _RDistance
        End Get
        Set(value As Integer)
            _RDistance = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("RDistance"))
        End Set
    End Property
    'Private _PrimerPair As PrimerPair
    'Public Property PrimerPair As PrimerPair
    '    Get
    '        Return _PrimerPair
    '    End Get
    '    Set(value As PrimerPair)
    '        _PrimerPair = value
    '        RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("PrimerPair"))
    '    End Set
    'End Property
    Public ReadOnly Property UseFPrimer As New ViewModelCommand(AddressOf cmdUseFPrimer)
    Private Sub cmdUseFPrimer(value As Object)
        UseF(Me)
    End Sub
    Public ReadOnly Property UseRPrimer As New ViewModelCommand(AddressOf cmdUseRPrimer)
    Private Sub cmdUseRPrimer(value As Object)
        UseR(Me)
    End Sub
    Public ReadOnly Property UseBothPrimers As New ViewModelCommand(AddressOf cmdUseBothPrimers)
    Private Sub cmdUseBothPrimers(value As Object)
        UsePair(Me)
    End Sub
    Public UseF As Action(Of PrimerPairModel)
    Public UseR As Action(Of PrimerPairModel)
    Public UsePair As Action(Of PrimerPairModel)

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
