
<Serializable>
    Public Class CodonPreferenceAnalyzer
        Implements System.ComponentModel.INotifyPropertyChanged
        'Private AminoAcids = New String() {"^", "A", "R", "N", "D", "C", "E", "Q", "G", "H", "I", "L", "K", "M", "F", "P", "S", "T", "W", "Y", "V", "*"}
        Public Sub New()
        For Each aa In KEGG.AminoAcids.AminoAcids
            Dim aacpa As New AminoAcidCodonPreferenceAnalyzer With {.Name = aa.Name, .ShortName = aa.ShortName}
            CodonAnalyzerDictionary.Add(aa.ShortName, aacpa)
            _CodonAnalyzers.Add(aacpa)
        Next
    End Sub
        'Public Property ID As Integer
        Private _ID As Integer
        Public Property ID As Integer
            Get
                Return _ID
            End Get
            Set(value As Integer)
                _ID = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ID"))
            End Set
        End Property
        'Public Property CDSCount As Integer
        Private _CDSCount As Integer = 0
        Public Property CDSCount As Integer
            Get
                Return _CDSCount
            End Get
            Set(value As Integer)
                _CDSCount = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("CDSCount"))
            End Set
        End Property

        'Public Property IsCompleted As Boolean
        Private _IsCompleted As Boolean = False
        Public Property IsCompleted As Boolean
            Get
                Return _IsCompleted
            End Get
            Set(value As Boolean)
                _IsCompleted = value
                If _IsCompleted Then
                    For Each aa In _CodonAnalyzers
                        For Each cd In aa.Codons
                            _CodonTable.Add(cd.Codon.Name, aa.ShortName)
                        Next
                    Next
                End If
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("IsCompleted"))
            End Set
        End Property
        Private _CodonTable As New Dictionary(Of String, String)(New IgnoreCaseStringComparer)
        Public Function Translate(Codon As String) As String
            If _CodonTable.ContainsKey(Codon) Then
                Return _CodonTable(Codon)
            Else
                Return ""
            End If
        End Function
        Public Sub Normalize()
            For Each aa In _CodonAnalyzers
                aa.Normalize()
            Next
        End Sub
        Public Sub AssignCodon(Codon As String, AminoAcidName As String)
            If _CodonTable.ContainsKey(Codon) Then
                If _CodonTable(Codon) = AminoAcidName Then
                    CodonAnalyzerDictionary(AminoAcidName).RecordCodon(Codon, 0)
                Else
                    Throw New Exception
                End If
            Else
                CodonAnalyzerDictionary(AminoAcidName).RecordCodon(Codon, 0)
            End If
        End Sub
        Public Sub RecordCodon(Codon As String, AminoAcidName As String)
            If AminoAcidName = "^" Then
                CodonAnalyzerDictionary(AminoAcidName).RecordCodon(Codon, 1)
            ElseIf _CodonTable.ContainsKey(Codon) Then
                If _CodonTable(Codon) = AminoAcidName Then
                    CodonAnalyzerDictionary(AminoAcidName).RecordCodon(Codon, 1)
                Else
                    Throw New Exception
                End If
            Else
                _CodonTable.Add(Codon, AminoAcidName)
                CodonAnalyzerDictionary(AminoAcidName).RecordCodon(Codon, 1)
            End If
        End Sub

        Public ReadOnly Property CodonAnalyzer(AminoAcid As String) As AminoAcidCodonPreferenceAnalyzer
            Get
                If CodonAnalyzerDictionary.ContainsKey(AminoAcid) Then Return CodonAnalyzerDictionary(AminoAcid)
                Return Nothing
            End Get
        End Property
        Private CodonAnalyzerDictionary As New Dictionary(Of String, AminoAcidCodonPreferenceAnalyzer)(New IgnoreCaseStringComparer)

        'Public Property CodonAnalyzers As System.Collections.ObjectModel.ObservableCollection(Of AminoAcidCodonPreferenceAnalyzer)
        Private _CodonAnalyzers As New System.Collections.ObjectModel.ObservableCollection(Of AminoAcidCodonPreferenceAnalyzer)
        Public ReadOnly Property CodonAnalyzers As System.Collections.ObjectModel.ObservableCollection(Of AminoAcidCodonPreferenceAnalyzer)
            Get
                Return _CodonAnalyzers
            End Get
        End Property
        <NonSerialized> Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged

        Public Function CreateTranslation()
        Dim _Trans As New KEGG.Translation(True)
        For Each aa In _CodonAnalyzers
                For Each cd In aa.Codons
                    _Trans.AminoTable(aa.ShortName).Codons.Add(New CodonUsage With {.Codon = New Codon With {.Name = cd.Codon.Name}, .AminoAcid = aa.ShortName,
                                                                                    .Ratio = cd.Ratio / aa.Count, .IsStartCodon = (aa.ShortName = "^"), .StartRatio = cd.NormalizedRatio})
                Next
            Next
            Return _Trans
        End Function
    End Class

    Public Class IgnoreCaseStringComparer
        Implements IEqualityComparer(Of String)
        Public Function Equals2(x As String, y As String) As Boolean Implements IEqualityComparer(Of String).Equals
            Return String.Compare(x, y, True) = 0
        End Function
        Public Function GetHashCode2(obj As String) As Integer Implements IEqualityComparer(Of String).GetHashCode
            Return obj.ToLower.GetHashCode()
        End Function
    End Class

    <Serializable>
    Public Class AminoAcidCodonPreferenceAnalyzer
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
        'Public Property ShortName As String
        Private _ShortName As String
        Public Property ShortName As String
            Get
                Return _ShortName
            End Get
            Set(value As String)
                _ShortName = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ShortName"))
            End Set
        End Property

        'Public Property Count As Integer
        Private _Count As Integer = 0
        Public Property Count As Integer
            Get
                Return _Count
            End Get
            Set(value As Integer)
                _Count = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Count"))
            End Set
        End Property
        Public Sub RecordCodon(Code As String, Optional CodonCount As Integer = 1)
            _Count += CodonCount
            If _CodonDictionary.ContainsKey(Code) Then
                _CodonDictionary(Code).Ratio += CodonCount
            Else
                Code = Code.ToUpper
                Dim cd As New CodonUsageAnalyzer With {.Codon = New Codon With {.Name = Code}, .Ratio = CodonCount}
                _CodonDictionary.Add(Code, cd)
                _Codons.Add(cd)
            End If
        End Sub
        Public Sub Normalize()
            For Each cd In _Codons
                cd.NormalizedRatio = cd.Ratio / _Count
            Next
        End Sub
        Private _CodonDictionary As New Dictionary(Of String, CodonUsageAnalyzer)(New IgnoreCaseStringComparer)
        'Public Property Codons As System.Collections.ObjectModel.ObservableCollection(Of CodonUsageAnalyzer)
        Private _Codons As New System.Collections.ObjectModel.ObservableCollection(Of CodonUsageAnalyzer)
        Public ReadOnly Property Codons As System.Collections.ObjectModel.ObservableCollection(Of CodonUsageAnalyzer)
            Get
                Return _Codons
            End Get
        End Property
        Public Function ContainsCodon(code As String) As Boolean
            code = code.ToUpper
            For Each cd In _Codons
                If cd.Codon.Name = code Then Return True
            Next
            Return False
        End Function
        <NonSerialized> Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
        Public Overrides Function ToString() As String
            Return _ShortName
        End Function


    End Class

    <Serializable>
    Public Class AminoAcid
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
        'Public Property ShortName As String
        Private _ShortName As String
        Public Property ShortName As String
            Get
                Return _ShortName
            End Get
            Set(value As String)
                _ShortName = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ShortName"))
            End Set
        End Property

        Public Sub Sort()
            Dim vList As New List(Of CodonUsage)(_Codons)
            vList.Sort()
            _Codons.Clear()
            For Each aa In vList
                _Codons.Add(aa)
            Next
        End Sub

        'Public Property Codons As System.Collections.ObjectModel.ObservableCollection(Of CodonUsage)
        Private _Codons As New System.Collections.ObjectModel.ObservableCollection(Of CodonUsage)
        Public ReadOnly Property Codons As System.Collections.ObjectModel.ObservableCollection(Of CodonUsage)
            Get
                Return _Codons
            End Get
        End Property
        <NonSerialized> Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class



    <Serializable>
    Public Class Codon
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
        <NonSerialized> Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
        Public Shared Operator =(c As Codon, value As String) As Boolean
            Return String.Compare(c.Name, value, True) = 0
        End Operator
        Public Shared Operator <>(c As Codon, value As String) As Boolean
            Return String.Compare(c.Name, value, True) <> 0
        End Operator
    End Class

    <Serializable>
    Public Class CodonUsage
        Implements System.ComponentModel.INotifyPropertyChanged, IComparable(Of CodonUsage), ICloneable

        'Public Property Codon As Codon
        Private _Codon As Codon
        Public Property Codon As Codon
            Get
                Return _Codon
            End Get
            Set(value As Codon)
                _Codon = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Codon"))
            End Set
        End Property
        'Public Property Ratio As Double
        Private _Ratio As Double
        Public Property Ratio As Double
            Get
                Return _Ratio
            End Get
            Set(value As Double)
                _Ratio = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Ratio"))
            End Set
        End Property
        'Public Property AminoAcid As String
        Private _AminoAcid As String
        Public Property AminoAcid As String
            Get
                Return _AminoAcid
            End Get
            Set(value As String)
                _AminoAcid = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("AminoAcid"))
            End Set
        End Property
        'Public Property IsStartCodon As Boolean
        Private _IsStartCodon As Boolean = False
        Public Property IsStartCodon As Boolean
            Get
                Return _IsStartCodon
            End Get
            Set(value As Boolean)
                _IsStartCodon = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("IsStartCodon"))
            End Set
        End Property
        'Public Property StartRatio As Double
        Private _StartRatio As Double
        Public Property StartRatio As Double
            Get
                Return _StartRatio
            End Get
            Set(value As Double)
                _StartRatio = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("StartRatio"))
            End Set
        End Property

        <NonSerialized> Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged

        Public Function CompareTo(other As CodonUsage) As Integer Implements IComparable(Of CodonUsage).CompareTo
            Return Math.Sign(_Ratio - other._Ratio)
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Return New CodonUsage With {._AminoAcid = _AminoAcid, ._Codon = _Codon, ._IsStartCodon = _IsStartCodon, ._Ratio = _Ratio, ._StartRatio = _StartRatio}
        End Function
    End Class

    <Serializable>
    Public Class CodonUsageAnalyzer
        Implements System.ComponentModel.INotifyPropertyChanged
        'Public Property Codon As Codon
        Private _Codon As Codon
        Public Property Codon As Codon
            Get
                Return _Codon
            End Get
            Set(value As Codon)
                _Codon = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Codon"))
            End Set
        End Property
        'Public Property Ratio As Integer
        Private _Ratio As Integer
        Public Property Ratio As Integer
            Get
                Return _Ratio
            End Get
            Set(value As Integer)
                _Ratio = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Ratio"))
            End Set
        End Property

        'Public Property NormalizedRatio As Double
        Private _NormalizedRatio As Double
        Public Property NormalizedRatio As Double
            Get
                Return _NormalizedRatio
            End Get
            Set(value As Double)
                _NormalizedRatio = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("NormalizedRatio"))
            End Set
        End Property


        <NonSerialized> Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class
