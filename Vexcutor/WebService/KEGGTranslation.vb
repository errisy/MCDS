Namespace KEGG
    'Translation 需要在系统当用单独一个数据库去存储
    <Serializable>
    Public Class Translation
        Implements System.ComponentModel.INotifyPropertyChanged

        Public Sub New()

        End Sub
        Public Sub New(Optional LoadAnimoAcids As Boolean = False)
            For Each aa In AminoAcids.AminoAcids
                _AminoTable.Add(aa.ShortName, aa)
            Next
        End Sub

        'Public Property Organism As String
        Private _Organism As String
        Public Property Organism As String
            Get
                Return _Organism
            End Get
            Set(value As String)
                _Organism = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Organism"))
            End Set
        End Property

        'Public Property AnimoTable As CodonTable
        Private _AminoTable As New KEGG.AminoTable
        Public ReadOnly Property AminoTable As AminoTable
            Get
                Return _AminoTable
            End Get
        End Property

        'Public Property CodeTable As Dictionary(Of String, Codon)
        Private _CodonTable As New CodonTable
        Public Property CodonTable As CodonTable
            Get
                Return _CodonTable
            End Get
            Set(value As CodonTable)
                _CodonTable = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("CodeTable"))
            End Set
        End Property

        Public Shared ReadOnly Property GetDefault As Translation
            Get
                'Return frmMain.CodonTraslation
                Return Nothing
            End Get
        End Property
        <NonSerialized> Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged

        Public Sub GenerateCodeTableFromAminoTable()
            For Each aa In _AminoTable.Values
                If aa.ShortName = "^" Then

                Else
                    For Each cd In aa.Codons
                        _CodonTable.Add(cd.Codon.Name, cd)
                    Next
                End If
            Next
        End Sub

        Public Function CalculateCodonAdaptionIndex(DNASequence As String) As CodonAdaptionIndexInfo
            Dim Triplets As New List(Of String)
            For i As Integer = 0 To DNASequence.Length - 1 Step 3
                Triplets.Add(DNASequence.Substring(i, 3))
            Next
            Dim product As Double = 0.0#
            For Each aa In _AminoTable.Values
                aa.Sort()
            Next
            Dim CodonCount As Integer = 0
            If _AminoTable IsNot Nothing AndAlso _CodonTable IsNot Nothing Then
                For Each triplet In Triplets
                    If CodonCount = 0 Then
                        Dim Codon = _CodonTable(triplet)
                        If Codon.AminoAcid = "*" Then Exit For
                        Dim AminoCode = Codon.AminoAcid
                        Dim AminoAcid = _AminoTable(AminoCode)
                        If AminoAcid.Codons(0).StartRatio = 0.0# Then
                            product += 0.0#
                        Else
                            product += Math.Log(Codon.StartRatio / AminoAcid.Codons.Last.StartRatio)
                        End If
                    Else
                        Dim Codon = _CodonTable(triplet)
                        If Codon.AminoAcid = "*" Then Exit For
                        Dim AminoCode = Codon.AminoAcid
                        Dim AminoAcid = _AminoTable(AminoCode)
                        product += Math.Log(Codon.Ratio / AminoAcid.Codons.Last.Ratio)
                    End If
                    CodonCount += 1
                Next
            End If
            If CodonCount = 0 Then
                Return New CodonAdaptionIndexInfo With {.CAI = Double.NaN, .Length = 0}
            Else
                Return New CodonAdaptionIndexInfo With {.CAI = Math.Exp(product / CodonCount), .Length = CodonCount}
            End If
        End Function
    End Class

    <Serializable>
    Public Class CodonTable
        Inherits Dictionary(Of String, CodonUsage)
        Implements System.ComponentModel.INotifyPropertyChanged
        Public Sub New()
        End Sub
        Public Sub New(info As Runtime.Serialization.SerializationInfo, context As Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
        Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class

    <Serializable>
    Public Class CodonAdaptionIndexInfo
        Implements System.ComponentModel.INotifyPropertyChanged
        'Public Property Length As Integer
        Private _Length As Integer
        Public Property Length As Integer
            Get
                Return _Length
            End Get
            Set(value As Integer)
                _Length = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Length"))
            End Set
        End Property
        'Public Property CAI As Double
        Private _CAI As Double
        Public Property CAI As Double
            Get
                Return _CAI
            End Get
            Set(value As Double)
                _CAI = value
                RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("CAI"))
            End Set
        End Property
        <NonSerialized> Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace