Partial Class Nuctions
    Public Class CodonTables
        Inherits List(Of CodonTable)
        Public Sub New()
            Add(Table1)
            Add(Table2)
            Add(Table3)
            Add(Table4)
            Add(Table5)
            Add(Table6)
            Add(Table9)
            Add(Table10)
            Add(Table11)
            Add(Table12)
            Add(Table13)
            Add(Table14)
            Add(Table15)
            Add(Table16)
            Add(Table21)
            Add(Table22)
            Add(Table23)
            Add(Table24)
        End Sub
        Private Shared _Table1 As CodonTable
        ''' <summary>
        ''' 1. The Standard Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table1 As CodonTable
            Get
                If _Table1 Is Nothing Then
                    Dim LA = "FFLLSSSSYY**CC*WLLLLPPPPHHQQRRRRIIIMTTTTNNKKSSRRVVVVAAAADDEEGGGG"
                    Dim LS = "---M---------------M---------------M----------------------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "1. The Standard Code"
                    _Table1 = tb
                End If
                Return Table1
            End Get
        End Property

        Private Shared _Table2 As CodonTable
        ''' <summary>
        ''' 2. The Vertebrate Mitochondrial Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table2 As CodonTable
            Get
                If _Table2 Is Nothing Then
                    Dim LA = "FFLLSSSSYY**CC*WLLLLPPPPHHQQRRRRIIIMTTTTNNKKSSRRVVVVAAAADDEEGGGG"
                    Dim LS = "---M---------------M---------------M----------------------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "2. The Vertebrate Mitochondrial Code"
                    _Table2 = tb
                End If
                Return Table2
            End Get
        End Property

        Private Shared _Table3 As CodonTable
        ''' <summary>
        ''' 3. The Yeast Mitochondrial Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table3 As CodonTable
            Get
                If _Table3 Is Nothing Then
                    Dim LA = "FFLLSSSSYY**CCWWTTTTPPPPHHQQRRRRIIMMTTTTNNKKSSRRVVVVAAAADDEEGGGG"
                    Dim LS = "----------------------------------MM----------------------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "3. The Yeast Mitochondrial Code"
                    _Table3 = tb
                End If
                Return Table3
            End Get
        End Property

        Private Shared _Table4 As CodonTable
        ''' <summary>
        ''' 4. The Mold, Protozoan, and Coelenterate Mitochondrial Code and the Mycoplasma/Spiroplasma Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table4 As CodonTable
            Get
                If _Table4 Is Nothing Then
                    Dim LA = "FFLLSSSSYY**CCWWLLLLPPPPHHQQRRRRIIIMTTTTNNKKSSRRVVVVAAAADDEEGGGG"
                    Dim LS = "--MM---------------M------------MMMM---------------M------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "4. The Mold, Protozoan, and Coelenterate Mitochondrial Code and the Mycoplasma/Spiroplasma Code"
                    _Table4 = tb
                End If
                Return Table4
            End Get
        End Property

        Private Shared _Table5 As CodonTable
        ''' <summary>
        ''' 5. The Invertebrate Mitochondrial Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table5 As CodonTable
            Get
                If _Table5 Is Nothing Then
                    Dim LA = "FFLLSSSSYY**CCWWLLLLPPPPHHQQRRRRIIMMTTTTNNKKSSSSVVVVAAAADDEEGGGG"
                    Dim LS = "---M----------------------------MMMM---------------M------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "5. The Invertebrate Mitochondrial Code"
                    _Table5 = tb
                End If
                Return Table5
            End Get
        End Property

        Private Shared _Table6 As CodonTable
        ''' <summary>
        ''' 6. The Ciliate, Dasycladacean and Hexamita Nuclear Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table6 As CodonTable
            Get
                If _Table5 Is Nothing Then
                    Dim LA = "FFLLSSSSYYQQCC*WLLLLPPPPHHQQRRRRIIIMTTTTNNKKSSRRVVVVAAAADDEEGGGG"
                    Dim LS = "-----------------------------------M----------------------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "6. The Ciliate, Dasycladacean and Hexamita Nuclear Code"
                    _Table6 = tb
                End If
                Return Table6
            End Get
        End Property

        Private Shared _Table9 As CodonTable
        ''' <summary>
        ''' 9. The Echinoderm and Flatworm Mitochondrial Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table9 As CodonTable
            Get
                If _Table9 Is Nothing Then
                    Dim LA = "FFLLSSSSYY**CCWWLLLLPPPPHHQQRRRRIIIMTTTTNNNKSSSSVVVVAAAADDEEGGGG"
                    Dim LS = "-----------------------------------M---------------M------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "9. The Echinoderm and Flatworm Mitochondrial Code"
                    _Table9 = tb
                End If
                Return Table9
            End Get
        End Property
        Private Shared _Table10 As CodonTable
        ''' <summary>
        ''' 10. The Euplotid Nuclear Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table10 As CodonTable
            Get
                If _Table10 Is Nothing Then
                    Dim LA = "FFLLSSSSYY**CCCWLLLLPPPPHHQQRRRRIIIMTTTTNNKKSSRRVVVVAAAADDEEGGGG"
                    Dim LS = "-----------------------------------M----------------------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "10. The Euplotid Nuclear Code"
                    _Table10 = tb
                End If
                Return Table10
            End Get
        End Property
        Private Shared _Table11 As CodonTable
        ''' <summary>
        ''' 11. The Bacterial, Archaeal and Plant Plastid Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table11 As CodonTable
            Get
                If _Table11 Is Nothing Then
                    Dim LA = "FFLLSSSSYY**CC*WLLLLPPPPHHQQRRRRIIIMTTTTNNKKSSRRVVVVAAAADDEEGGGG"
                    Dim LS = "---M---------------M------------MMMM---------------M------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = ""
                    _Table11 = tb
                End If
                Return Table11
            End Get
        End Property
        Private Shared _Table12 As CodonTable
        ''' <summary>
        ''' 12. The Alternative Yeast Nuclear Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table12 As CodonTable
            Get
                If _Table12 Is Nothing Then
                    Dim LA = "FFLLSSSSYY**CC*WLLLSPPPPHHQQRRRRIIIMTTTTNNKKSSRRVVVVAAAADDEEGGGG"
                    Dim LS = "-------------------M---------------M----------------------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "12. The Alternative Yeast Nuclear Code"
                    _Table12 = tb
                End If
                Return Table12
            End Get
        End Property
        Private Shared _Table13 As CodonTable
        ''' <summary>
        ''' 13. The Ascidian Mitochondrial Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table13 As CodonTable
            Get
                If _Table13 Is Nothing Then
                    Dim LA = "FFLLSSSSYY**CCWWLLLLPPPPHHQQRRRRIIMMTTTTNNKKSSGGVVVVAAAADDEEGGGG"
                    Dim LS = "---M------------------------------MM---------------M------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = ""
                    _Table13 = tb
                End If
                Return Table13
            End Get
        End Property
        Private Shared _Table14 As CodonTable
        ''' <summary>
        ''' 14. The Alternative Flatworm Mitochondrial Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table14 As CodonTable
            Get
                If _Table14 Is Nothing Then
                    Dim LA = "FFLLSSSSYYY*CCWWLLLLPPPPHHQQRRRRIIIMTTTTNNNKSSSSVVVVAAAADDEEGGGG"
                    Dim LS = "-----------------------------------M----------------------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "14. The Alternative Flatworm Mitochondrial Code"
                    _Table14 = tb
                End If
                Return Table14
            End Get
        End Property
        Private Shared _Table15 As CodonTable
        ''' <summary>
        ''' 15. Blepharisma Nuclear Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table15 As CodonTable
            Get
                If _Table15 Is Nothing Then
                    Dim LA = "FFLLSSSSYY*QCC*WLLLLPPPPHHQQRRRRIIIMTTTTNNKKSSRRVVVVAAAADDEEGGGG"
                    Dim LS = "-----------------------------------M----------------------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "15. Blepharisma Nuclear Code"
                    _Table15 = tb
                End If
                Return Table15
            End Get
        End Property
        Private Shared _Table16 As CodonTable
        ''' <summary>
        ''' 16. Chlorophycean Mitochondrial Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table16 As CodonTable
            Get
                If _Table16 Is Nothing Then
                    Dim LA = "FFLLSSSSYY*LCC*WLLLLPPPPHHQQRRRRIIIMTTTTNNKKSSRRVVVVAAAADDEEGGGG"
                    Dim LS = "-----------------------------------M----------------------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "16. Chlorophycean Mitochondrial Code"
                    _Table16 = tb
                End If
                Return Table16
            End Get
        End Property
        Private Shared _Table21 As CodonTable
        ''' <summary>
        ''' 21. Trematode Mitochondrial Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table21 As CodonTable
            Get
                If _Table21 Is Nothing Then
                    Dim LA = "FFLLSSSSYY**CCWWLLLLPPPPHHQQRRRRIIMMTTTTNNNKSSSSVVVVAAAADDEEGGGG"
                    Dim LS = "-----------------------------------M---------------M------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "21. Trematode Mitochondrial Code"
                    _Table21 = tb
                End If
                Return Table21
            End Get
        End Property
        Private Shared _Table22 As CodonTable
        ''' <summary>
        ''' 22. Scenedesmus obliquus mitochondrial Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table22 As CodonTable
            Get
                If _Table22 Is Nothing Then
                    Dim LA = "FFLLSS*SYY*LCC*WLLLLPPPPHHQQRRRRIIIMTTTTNNKKSSRRVVVVAAAADDEEGGGG"
                    Dim LS = "-----------------------------------M----------------------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "22. Scenedesmus obliquus mitochondrial Code"
                    _Table22 = tb
                End If
                Return Table22
            End Get
        End Property
        Private Shared _Table23 As CodonTable
        ''' <summary>
        ''' 23. Thraustochytrium Mitochondrial Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table23 As CodonTable
            Get
                If _Table23 Is Nothing Then
                    Dim LA = "FF*LSSSSYY**CC*WLLLLPPPPHHQQRRRRIIIMTTTTNNKKSSRRVVVVAAAADDEEGGGG"
                    Dim LS = "--------------------------------M--M---------------M------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "23. Thraustochytrium Mitochondrial Code"
                    _Table23 = tb
                End If
                Return Table23
            End Get
        End Property
        Private Shared _Table24 As CodonTable
        ''' <summary>
        ''' 24. Pterobranchia mitochondrial code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Table24 As CodonTable
            Get
                If _Table24 Is Nothing Then
                    Dim LA = "FFLLSSSSYY**CCWWLLLLPPPPHHQQRRRRIIIMTTTTNNKKSSSKVVVVAAAADDEEGGGG"
                    Dim LS = "---M---------------M---------------M---------------M------------"
                    Dim L1 = "TTTTTTTTTTTTTTTTCCCCCCCCCCCCCCCCAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGG"
                    Dim L2 = "TTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGGTTTTCCCCAAAAGGGG"
                    Dim L3 = "TCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAGTCAG"
                    Dim tb As New CodonTable
                    For i As Integer = 0 To 63
                        Dim c As Codon = AnminoAcidParse(LA(i))
                        If Not tb.ContainsKey(c.ShortName) Then tb.AddCodon(c)
                        Dim gc = New Nuctions.GeneticCode
                        gc.Name = L1(i) + L2(i) + L3(i)
                        gc.ratio = 0.0F
                        gc.CanStart = LS(i) = "M"
                        tb(c.ShortName).CodeList.Add(gc)
                    Next
                    tb.Name = "24. Pterobranchia mitochondrial code"
                    _Table24 = tb
                End If
                Return Table24
            End Get
        End Property

    End Class
End Class

