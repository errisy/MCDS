Namespace KEGG
    <Serializable>
    Public Class AminoTable
        Inherits Dictionary(Of String, AminoAcid)
        Implements System.ComponentModel.INotifyPropertyChanged

        Public Sub New()
        End Sub
        Public Sub New(info As Runtime.Serialization.SerializationInfo, context As Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
        Public Sub AddAminoAcid(ByVal c As AminoAcid)
            MyBase.Add(c.ShortName, c)
        End Sub

        Public ReadOnly Property CodonCollection() As System.Collections.ICollection
            Get
                Return MyBase.Values
            End Get
        End Property

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
    End Class
    Public Class AminoAcids
        Public Shared ReadOnly Property A As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Ala", .ShortName = "A"}
            End Get
        End Property
        Public Shared ReadOnly Property Ala As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Ala", .ShortName = "A"}
            End Get
        End Property
        Public Shared ReadOnly Property R As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Arg", .ShortName = "R"}
            End Get
        End Property
        Public Shared ReadOnly Property Arg As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Arg", .ShortName = "R"}
            End Get
        End Property
        Public Shared ReadOnly Property N As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Asn", .ShortName = "N"}
            End Get
        End Property
        Public Shared ReadOnly Property Asn As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Asn", .ShortName = "N"}
            End Get
        End Property
        Public Shared ReadOnly Property D As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Asp", .ShortName = "D"}
            End Get
        End Property
        Public Shared ReadOnly Property Asp As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Asp", .ShortName = "D"}
            End Get
        End Property
        Public Shared ReadOnly Property C As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Cys", .ShortName = "C"}
            End Get
        End Property
        Public Shared ReadOnly Property Cys As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Cys", .ShortName = "C"}
            End Get
        End Property
        Public Shared ReadOnly Property E As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Glu", .ShortName = "E"}
            End Get
        End Property
        Public Shared ReadOnly Property Glu As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Glu", .ShortName = "E"}
            End Get
        End Property
        Public Shared ReadOnly Property Q As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Gln", .ShortName = "Q"}
            End Get
        End Property
        Public Shared ReadOnly Property Gln As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Gln", .ShortName = "Q"}
            End Get
        End Property
        Public Shared ReadOnly Property G As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Gly", .ShortName = "G"}
            End Get
        End Property
        Public Shared ReadOnly Property Gly As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Gly", .ShortName = "G"}
            End Get
        End Property
        Public Shared ReadOnly Property H As AminoAcid
            Get
                Return New AminoAcid With {.Name = "His", .ShortName = "H"}
            End Get
        End Property
        Public Shared ReadOnly Property His As AminoAcid
            Get
                Return New AminoAcid With {.Name = "His", .ShortName = "H"}
            End Get
        End Property
        Public Shared ReadOnly Property I As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Ile", .ShortName = "I"}
            End Get
        End Property
        Public Shared ReadOnly Property Ile As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Ile", .ShortName = "I"}
            End Get
        End Property
        Public Shared ReadOnly Property L As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Leu", .ShortName = "L"}
            End Get
        End Property
        Public Shared ReadOnly Property Leu As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Leu", .ShortName = "L"}
            End Get
        End Property
        Public Shared ReadOnly Property K As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Lys", .ShortName = "K"}
            End Get
        End Property
        Public Shared ReadOnly Property Lys As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Lys", .ShortName = "K"}
            End Get
        End Property
        Public Shared ReadOnly Property M As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Met", .ShortName = "M"}
            End Get
        End Property
        Public Shared ReadOnly Property Met As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Met", .ShortName = "M"}
            End Get
        End Property
        Public Shared ReadOnly Property F As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Phe", .ShortName = "F"}
            End Get
        End Property
        Public Shared ReadOnly Property Phe As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Phe", .ShortName = "F"}
            End Get
        End Property
        Public Shared ReadOnly Property P As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Pro", .ShortName = "P"}
            End Get
        End Property
        Public Shared ReadOnly Property Pro As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Pro", .ShortName = "P"}
            End Get
        End Property
        Public Shared ReadOnly Property S As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Ser", .ShortName = "S"}
            End Get
        End Property
        Public Shared ReadOnly Property Ser As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Ser", .ShortName = "S"}
            End Get
        End Property
        Public Shared ReadOnly Property T As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Thr", .ShortName = "T"}
            End Get
        End Property
        Public Shared ReadOnly Property Thr As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Thr", .ShortName = "T"}
            End Get
        End Property
        Public Shared ReadOnly Property W As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Trp", .ShortName = "W"}
            End Get
        End Property
        Public Shared ReadOnly Property Trp As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Trp", .ShortName = "W"}
            End Get
        End Property
        Public Shared ReadOnly Property Y As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Tyr", .ShortName = "Y"}
            End Get
        End Property
        Public Shared ReadOnly Property Tyr As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Tyr", .ShortName = "Y"}
            End Get
        End Property
        Public Shared ReadOnly Property V As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Val", .ShortName = "V"}
            End Get
        End Property
        Public Shared ReadOnly Property Val As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Val", .ShortName = "V"}
            End Get
        End Property
        Public Shared ReadOnly Property [Stop] As AminoAcid
            Get
                Return New AminoAcid With {.Name = "End", .ShortName = "*"}
            End Get
        End Property
        Public Shared ReadOnly Property [Start] As AminoAcid
            Get
                Return New AminoAcid With {.Name = "Ini", .ShortName = "^"}
            End Get
        End Property
        Public Shared Function ParseCharge(Name As String) As String
            Dim Key As String = Name.ToUpper
            Select Case Key
                Case "ARG", "R"
                    Return "+"
                Case "ASP", "D"
                    Return "-"
                Case "GLU", "E"
                    Return "-"
                Case "HIS", "H"
                    Return "+"
                Case "LYS", "K"
                    Return "+"
                Case Else
                    Return ""
            End Select
        End Function
        Public Shared Function ParseTripletName(TripletName As String) As String
            Dim Key As String = TripletName.ToUpper
            Select Case Key
                Case "*", "Stop"
                    Return "*"
                Case "^", "Start"
                    Return "^"
                Case "ALA", "A"
                    Return "A"
                Case "ARG", "R"
                    Return "R"
                Case "ASN", "N"
                    Return "N"
                Case "ASP", "D"
                    Return "D"
                Case "CYS", "C"
                    Return "C"
                Case "GLU", "E"
                    Return "E"
                Case "GLN", "Q"
                    Return "Q"
                Case "GLY", "G"
                    Return "G"
                Case "HIS", "H"
                    Return "H"
                Case "ILE", "I"
                    Return "I"
                Case "LEU", "L"
                    Return "L"
                Case "LYS", "K"
                    Return "K"
                Case "MET", "M"
                    Return "M"
                Case "PHE", "F"
                    Return "F"
                Case "PRO", "P"
                    Return "P"
                Case "SER", "S"
                    Return "S"
                Case "THR", "T"
                    Return "T"
                Case "TRP", "W"
                    Return "W"
                Case "TYR", "Y"
                    Return "Y"
                Case "VAL", "V"
                    Return "V"
                Case Else
                    Return ""
            End Select
        End Function
        Public Shared Function IsAminoAcidCode(TripletName As String) As Boolean
            Dim Key As String = TripletName.ToLower
            Select Case Key
                Case "*", "stop", "^", "start", "ala", "a", "arg", "r", "asn", "n", "asp", "d", "cys", "c",
                    "glu", "e", "gln", "q", "gly", "g", "his", "h", "ile", "i", "leu", "l", "lys", "k", "met", "m",
                    "phe", "f", "pro", "p", "ser", "s", "thr", "t", "trp", "w", "tyr", "y", "val", "v"
                    Return True
                Case Else
                    Return False
            End Select
        End Function
        Public Shared ReadOnly Property AminoAcid(Key As String) As AminoAcid
            Get
                If Key.Length = 1 Then
                    Key = Key.ToUpper
                Else
                    Key = Key.Substring(0, 1).ToUpper & Key.Substring(1).ToLower
                End If
                Select Case Key
                    Case "*", "Stop"
                        Return [Stop]
                    Case "^", "Start"
                        Return [Start]
                    Case "Ala", "A"
                        Return A
                    Case "Arg", "R"
                        Return R
                    Case "Asn", "N"
                        Return N
                    Case "Asp", "D"
                        Return D
                    Case "Cys", "C"
                        Return C
                    Case "Glu", "E"
                        Return E
                    Case "Gln", "Q"
                        Return Q
                    Case "Gly", "G"
                        Return G
                    Case "His", "H"
                        Return H
                    Case "Ile", "I"
                        Return I
                    Case "Leu", "L"
                        Return L
                    Case "Lys", "K"
                        Return K
                    Case "Met", "M"
                        Return M
                    Case "Phe", "F"
                        Return F
                    Case "Pro", "P"
                        Return P
                    Case "Ser", "S"
                        Return S
                    Case "Thr", "T"
                        Return T
                    Case "Trp", "W"
                        Return W
                    Case "Tyr", "Y"
                        Return Y
                    Case "Val", "V"
                        Return V
                    Case Else
                        Return [Stop]
                End Select
            End Get
        End Property

        Public Shared ReadOnly Property AminoAcids As List(Of AminoAcid)
            Get
                Return New List(Of AminoAcid) From {Start, A, R, N, D, C, E, Q, G, H, I, L, K, M, F, P, S, T, W, Y, V, [Stop]}
            End Get
        End Property

        Public Shared ReadOnly Property AminoAcidShortNames As List(Of String)
            Get
                Return New List(Of String) From {"^", "A", "R", "N", "D", "C", "E", "Q", "G", "H", "I", "L", "K", "M", "F", "P", "S", "T", "W", "Y", "V", "*"}
            End Get
        End Property
    End Class
End Namespace