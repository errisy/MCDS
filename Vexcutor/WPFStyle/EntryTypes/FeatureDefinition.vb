Imports System.ComponentModel

Public Class FeatureDefinitions
    Inherits System.Collections.ObjectModel.ObservableCollection(Of FeatureDefinition)
End Class

Public Class FeatureDefinition
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
    Private _Fill As System.Windows.Media.Color
    Public Property Fill As System.Windows.Media.Color
        Get
            Return _Fill
        End Get
        Set(value As System.Windows.Media.Color)
            _Fill = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Fill"))
        End Set
    End Property
    Private _Stroke As System.Windows.Media.Color
    Public Property Stroke As System.Windows.Media.Color
        Get
            Return _Stroke
        End Get
        Set(value As System.Windows.Media.Color)
            _Stroke = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Stroke"))
        End Set
    End Property
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
