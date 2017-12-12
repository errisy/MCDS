Imports System.Windows, System.Windows.Controls, System.Windows.Data, System.Windows.Media, System.Windows.Media.Imaging
<ValueConversion(GetType(Byte()), GetType(BitmapImage))>
Public Class BitmapConverter
    Implements IValueConverter
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If TypeOf value Is Byte() Then
            Dim bytes As Byte() = value
            Dim bi As New System.Windows.Media.Imaging.BitmapImage()
            bi.BeginInit()
            bi.StreamSource = New System.IO.MemoryStream(bytes)



            bi.EndInit()
            Return bi
        Else
            Return Nothing
        End If
    End Function
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack

    End Function
End Class

Public Class Bytes2BitmapExtension
    Inherits System.Windows.Markup.MarkupExtension
    Public Sub New()
    End Sub
    Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
        Return New BitmapConverter
    End Function
End Class

<ValueConversion(GetType(Byte()), GetType(SCFData))>
Public Class SCFConverter
    Implements IValueConverter
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If TypeOf value Is Byte() Then
            Dim bytes As Byte() = value

            Dim sd As New SCFData
            Using ms = New System.IO.MemoryStream(bytes)
                sd.Read(ms)
            End Using

            Return sd

             
        Else
            Return Nothing
        End If
    End Function
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack

    End Function
End Class

Public Class Bytes2SCFExtension
    Inherits System.Windows.Markup.MarkupExtension
    Public Sub New()
    End Sub
    Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
        Return New SCFConverter
    End Function
End Class
<ValueConversion(GetType(Byte()), GetType(AB1Data))>
Public Class AB1Converter
    Implements IValueConverter
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If TypeOf value Is Byte() Then
            Dim bytes As Byte() = value

            Dim ab As New AB1Data

            ab.Read(bytes)

            Return ab
        Else
            Return Nothing
        End If
    End Function
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack

    End Function
End Class
Public Class Bytes2AB1Extension
    Inherits System.Windows.Markup.MarkupExtension
    Public Sub New()
    End Sub
    Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
        Return New AB1Converter
    End Function
End Class

<Serializable>
Public Class BitImage
    Public Property Name As String
    Public Property Data As Byte()
End Class

<Serializable>
Public Class SequenceItem
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
    'Public Property FileType As SequencingFileTypeEnum
    Private _FileType As SequencingFileTypeEnum
    Public Property FileType As SequencingFileTypeEnum
        Get
            Return _FileType
        End Get
        Set(value As SequencingFileTypeEnum)
            _FileType = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("FileType"))
        End Set
    End Property
    'Public Property Data As Byte()
    Private _Data As Byte()
    Public Property Data As Byte()
        Get
            Return _Data
        End Get
        Set(value As Byte())
            _Data = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Data"))
        End Set
    End Property
    Private _RawData As RawSequencingData.RawData
    Public Property RawData As RawSequencingData.RawData
        Get
            Return _RawData
        End Get
        Set(value As RawSequencingData.RawData)
            _RawData = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("RawData"))
        End Set
    End Property
    <NonSerialized> Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
<Serializable> Public Enum SequencingFileTypeEnum
    SCF
    AB1
End Enum

<Serializable>
Public Class SequenceItemGroup
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
    'Public Property Sequences As system.Collections.ObjectModel.ObservableCollection(Of SequenceItem)
    Private _Sequences As New System.Collections.ObjectModel.ObservableCollection(Of SequenceItem)
    Public ReadOnly Property Sequences As System.Collections.ObjectModel.ObservableCollection(Of SequenceItem)
        Get
            Return _Sequences
        End Get
    End Property
    <NonSerialized> Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class