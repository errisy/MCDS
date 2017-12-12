Imports System.ComponentModel

<Serializable>
Public Class StandardFeatures
    Implements System.ComponentModel.INotifyPropertyChanged
    Public Property Features As New System.Collections.ObjectModel.ObservableCollection(Of Feature)
    <NonSerialized> Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
#Region "Management"
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property Items As New ObjectModel.ObservableCollection(Of Feature)
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property AddFeature As New ViewModelCommand(AddressOf cmdAddFeature)
    Private Sub cmdAddFeature(value As Object)
        If _Items.Count < _Features.Count Then _Items = _Features
        _Features.Add(New Feature)
    End Sub
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property RemoveFeature As New ViewModelCommand(AddressOf cmdRemoveFeature)
    Private Sub cmdRemoveFeature(value As Object)
        If TypeOf value Is IList Then
            Dim rmList As New List(Of Feature)
            For Each it In DirectCast(value, IList)
                If TypeOf it Is Feature Then
                    rmList.Add(it)
                End If
            Next
            For Each it In rmList
                If _Items.Contains(it) Then _Items.Remove(it)
                If (_Items IsNot _Features) AndAlso _Features.Contains(it) Then _Features.Remove(it)
            Next
        End If
    End Sub
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property Save As New ViewModelCommand(AddressOf cmdSave)
    Private Sub cmdSave(value As Object)
        IO.File.WriteAllText(SettingEntry.StandardFeaturesPath, System.Xaml.XamlServices.Save(Me))
        SettingEntry.LoadStandardFeatures()
    End Sub
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property Search As New ViewModelCommand(AddressOf cmdSearch)
    Private Sub cmdSearch(value As Object)
        _Items = New ObjectModel.ObservableCollection(Of Feature)
        Dim condition As Func(Of Feature, Boolean) = Function(f) True
        Dim keywords As String() = SearchKeyWords.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        Select Case SearchType
            Case "Label"
                condition = Function(f) f.Label.ContainsAllOf(keywords)
            Case "Type"
                condition = Function(f) f.Type.ContainsAllOf(keywords)
            Case "Note"
                condition = Function(f) f.Note.ContainsAllOf(keywords)
            Case "Sequence"
                condition = Function(f) f.Note.ContainsAllOf(keywords)
        End Select
        For Each it In _Features.Where(condition)
            _Items.Add(it)
        Next
    End Sub
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property SearchTypes As New List(Of String) From {"Label", "Type", "Note", "Sequence"}
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property SearchType As String = "Label"
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property SearchKeyWords As String
#End Region
End Class
<Serializable>
Public Class Feature
    Public Property Label As String = ""
    Public Property Note As String = ""
    Public Property Name As String = ""
    Public Property Type As String = ""
    Public Property Sequence As String = ""
    Public Property BioFunction As New System.Collections.ObjectModel.ObservableCollection(Of FeatureFunction)

End Class

<Serializable>
Public Class FeatureFunction
    Public Property BioFunction As Nuctions.FeatureFunctionEnum
    Public Property Parameters As String
End Class