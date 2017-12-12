Imports System.ComponentModel

Public Class ORFSearchOptions
    Implements System.ComponentModel.INotifyPropertyChanged
    Private _MinimalLength As Integer = 50
    Public Property MinimalLength As Integer
        Get
            Return _MinimalLength
        End Get
        Set(value As Integer)
            _MinimalLength = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("MinimalLength"))
        End Set
    End Property
    Private _UseATG As Boolean = True
    Public Property UseATG As Boolean
        Get
            Return _UseATG
        End Get
        Set(value As Boolean)
            _UseATG = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("UseATG"))
        End Set
    End Property
    Private _UseCTG As Boolean
    Public Property UseCTG As Boolean
        Get
            Return _UseCTG
        End Get
        Set(value As Boolean)
            _UseCTG = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("UseCTG"))
        End Set
    End Property
    Private _UseGTG As Boolean
    Public Property UseGTG As Boolean
        Get
            Return _UseGTG
        End Get
        Set(value As Boolean)
            _UseGTG = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("UseGTG"))
        End Set
    End Property
    Private _UseTTG As Boolean
    Public Property UseTTG As Boolean
        Get
            Return _UseTTG
        End Get
        Set(value As Boolean)
            _UseTTG = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("UseTTG"))
        End Set
    End Property
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class