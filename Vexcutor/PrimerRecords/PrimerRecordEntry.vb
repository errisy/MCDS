Imports System.ComponentModel, System.Text.RegularExpressions

Public Class PrimerRecordEntry
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
    Private _Project As String
    Public Property Project As String
        Get
            Return _Project
        End Get
        Set(value As String)
            _Project = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Project"))
        End Set
    End Property
    Private _OrderDate As Date
    Public Property OrderDate As Date
        Get
            Return _OrderDate
        End Get
        Set(value As Date)
            _OrderDate = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("OrderDate"))
        End Set
    End Property
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Overrides Function ToString() As String
        Return String.Format("{1}{0}{2}{0}{3}{0}{4}", vbTab, Name, Sequence, Project, OrderDate.ToString())
    End Function
End Class
Friend Class PrimerRecordManager
    Private Shared ReadOnly RecordPath As String = AppDomain.CurrentDomain.BaseDirectory + "PrimerRecords.txt"
    Public Shared Function SearchPrimers(pList As IList(Of PrimerSearchEntry)) As List(Of PrimerSearchEntry)

        If Not IO.File.Exists(RecordPath) Then IO.File.Create(RecordPath)
        Dim value As String = IO.File.ReadAllText(RecordPath)
        Dim found As New List(Of PrimerSearchEntry)
        For Each pm In pList
            For Each m As Match In Regex.Matches(value, String.Format("([^\t^\n^\r]+)\t({0})\t([^\t^\n^\r]*)\t([^\t^\n^\r]+)", pm.Sequence))
                pm.MatchedEntries.Add(New PrimerRecordEntry With
                                      {.Name = m.Groups(1).Value,
                                      .Sequence = m.Groups(2).Value,
                                      .Project = m.Groups(3).Value,
                                      .OrderDate = Date.Parse(m.Groups(4).Value)})
            Next
            If pm.MatchedEntries.Count > 0 Then pm.IsSynthesized = True : pm.Synthesis = False : found.Add(pm)
        Next
        Return found
    End Function
    Public Shared Sub Save(pList As List(Of PrimerSearchEntry), ProjectName As String)
        If Not IO.File.Exists(RecordPath) Then IO.File.Create(RecordPath)
        Dim lines As New List(Of String)

        Dim value As String = IO.File.ReadAllText(RecordPath)

        Dim NotSaved As New List(Of PrimerSearchEntry)
        For Each pm In pList
            If Regex.IsMatch(value, String.Format("([^\t^\n^\r]+)\t({0})\t({1})\t([^\t^\n^\r]+)", pm.Sequence, ProjectName)) Then
                pm.IsSynthesized = True
                pm.Synthesis = False
            Else
                NotSaved.Add(pm)
            End If
        Next

        For Each pm In NotSaved
            lines.Add(pm.GenerateRecord(ProjectName))
            pm.IsSynthesized = True
            pm.Synthesis = False
        Next
        IO.File.AppendAllLines(RecordPath, lines)
    End Sub
End Class

Friend Class PrimerSearchEntry
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
    Private _Sequence As String
    Public Property Sequence As String
        Get
            Return _Sequence
        End Get
        Set(value As String)
            _Sequence = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Sequence"))
            _Length = _Sequence.Length
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Length"))
        End Set
    End Property
    Public ReadOnly Property Length As Integer

    Private _IsSynthesized As Boolean
    Public Property IsSynthesized As Boolean
        Get
            Return _IsSynthesized
        End Get
        Set(value As Boolean)
            _IsSynthesized = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("IsSynthesized"))
        End Set
    End Property
    Private _Synthesis As Boolean
    Public Property Synthesis As Boolean
        Get
            Return _Synthesis
        End Get
        Set(value As Boolean)
            _Synthesis = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Synthesis"))
        End Set
    End Property
    Public Function GenerateRecord(Project As String) As String
        Return String.Format("{1}{0}{2}{0}{3}{0}{4}", vbTab, Name, Sequence, Project, Now.ToString("yyyy-MM-dd HH:mm:ss"))
    End Function
    Public ReadOnly Property MatchedEntries As New ObjectModel.ObservableCollection(Of PrimerRecordEntry)
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
