Public Class gRNADefinitions
    Inherits System.Collections.ObjectModel.ObservableCollection(Of gRNADefinition)
End Class
Public Class gRNADefinition
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
    Private _TargetPattern As String
    Public Property TargetPattern As String
        Get
            Return _TargetPattern
        End Get
        Set(value As String)
            _TargetPattern = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("TargetPattern"))
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Sequence"))
        End Set
    End Property
    Private _gRNAScaffold As String
    Public Property gRNAScaffold As String
        Get
            Return _gRNAScaffold
        End Get
        Set(value As String)
            _gRNAScaffold = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("gRNAScaffold"))
        End Set
    End Property
    Private _PAM As String
    Public Property PAM As String
        Get
            Return _PAM
        End Get
        Set(value As String)
            _PAM = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("PAM"))
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Sequence"))
        End Set
    End Property
    Public ReadOnly Property Sequence As String Implements ICutSite.Sequence
        Get
            Return _TargetPattern + _PAM
        End Get
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

    Public Function IdentifygRNA(gList As IList(Of Nuctions.GeneFile)) As List(Of Nuctions.RestrictionEnzyme)
        Dim gRNAExtraLength As Integer = _TargetPattern.Length + _gRNAScaffold.Length - 1
        Dim gRNAPattern As New System.Text.RegularExpressions.Regex(Nuctions.WildCardNucleotides2RegexPattern(String.Format("({0}){1}", _TargetPattern, _gRNAScaffold)), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim enzList As New List(Of Nuctions.RestrictionEnzyme)
        Dim idx As Integer = 0
        For Each gf In gList
            Dim fs As String = gf.Sequence + gf.Sequence.Substring(0, gRNAExtraLength)
            Dim rs As String = gf.RCSequence + gf.RCSequence.Substring(0, gRNAExtraLength)
            For Each m As System.Text.RegularExpressions.Match In gRNAPattern.Matches(fs)
                enzList.Add(New Nuctions.RestrictionEnzyme(_Name + idx.ToString, m.Groups(1).Value + _PAM, _SCut, _ACut))
                idx += 1
            Next
            For Each m As System.Text.RegularExpressions.Match In gRNAPattern.Matches(rs)
                enzList.Add(New Nuctions.RestrictionEnzyme(_Name + idx.ToString, m.Groups(1).Value + _PAM, _SCut, _ACut))
                idx += 1
            Next
        Next
        Return enzList
    End Function
End Class
