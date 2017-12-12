
Imports System.ComponentModel

Public Class Nuctions
    ''' <summary>
    ''' 把氨基酸的单字母或者三字母缩写转化成Codon类，但是Codon类内部的密码子列没有被填充。
    ''' 创建后请人工填充密码子列
    ''' </summary>
    ''' <param name="Value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function AnminoAcidParse(ByVal Value As String) As Codon
        'A:Ala>GCC#GCA#GCT#GCG#
        'C:Cys>TGC#TGT#
        'D:Asp>GAC#GAT#
        'E:Glu>GAG#GAA#
        'F:Phe>TTC#TTT#
        'G:Gly>GGC#GGA#GGT#GGG#
        'H:His>CAC#CAT#
        'I:Ile>ATC#ATA#ATT#
        'K:Lys>AAA#AAG#
        'L:Leu>CTA#CTC#CTG#CTT#TTA#TTG#
        'M:Met>ATG#
        'N:Asn>AAC#AAT#
        'P:Pro>CCA#CCC#CCT#CCG#
        'Q:Gln>CAG#CAA#
        'R:Arg>CGA#CGC#CGT#CGG#AGA#AGG#
        'S:Ser>TCA#TCC#TCG#TCT#AGC#AGT#
        'T:Thr>ACA#ACC#ACG#ACT#
        'V:Val>GTA#GTC#GTT#GTG#
        'W:Trp>TGG#
        'Y:Tyr>TAC#TAT#
        '-:STP>TAG#TAA#TGA#
        Dim key As String = Value.ToUpper
        Dim c As New Codon
        With c

            Select Case Value
                Case "A", "ALA"
                    c.FullName = "Ala"
                    c.ShortName = "A"
                Case "C", "CYS"
                    c.FullName = "Cys"
                    c.ShortName = "C"
                Case "D", "ASP"
                    c.FullName = "Asp"
                    c.ShortName = "D"
                Case "E", "GLU"
                    c.FullName = "Glu"
                    c.ShortName = "E"
                Case "F", "PHE"
                    c.FullName = "Phe"
                    c.ShortName = "F"
                Case "G", "GLY"
                    c.FullName = "Gly"
                    c.ShortName = "G"
                Case "H", "HIS"
                    c.FullName = "His"
                    c.ShortName = "H"
                Case "I", "ILE"
                    c.FullName = "Ile"
                    c.ShortName = "I"
                Case "K", "LYS"
                    c.FullName = "Lys"
                    c.ShortName = "K"
                Case "L", "LEU"
                    c.FullName = "Leu"
                    c.ShortName = "L"
                Case "M", "MET"
                    c.FullName = "Met"
                    c.ShortName = "M"
                Case "N", "ASN"
                    c.FullName = "Asn"
                    c.ShortName = "N"
                Case "P", "PRO"
                    c.FullName = "Pro"
                    c.ShortName = "P"
                Case "Q", "GLN"
                    c.FullName = "Gln"
                    c.ShortName = "Q"
                Case "R", "ARG"
                    c.FullName = "Arg"
                    c.ShortName = "R"
                Case "S", "SER"
                    c.FullName = "Ser"
                    c.ShortName = "S"
                Case "T", "THR"
                    c.FullName = "Thr"
                    c.ShortName = "T"
                Case "V", "VAL"
                    c.FullName = "Val"
                    c.ShortName = "V"
                Case "W", "TRP"
                    c.FullName = "Trp"
                    c.ShortName = "W"
                Case "Y", "TYR"
                    c.FullName = "Tyr"
                    c.ShortName = "Y"
                Case "*", "-", "STP"
                    c.FullName = "STP"
                    c.ShortName = "-"
            End Select

        End With
        Return c

    End Function
    ''' <summary>
    ''' 把数字转化成氨基酸单字母表示，0表示*，其他氨基酸按照字母顺序排列；小于0或者超过20返回"-"
    ''' </summary>
    ''' <param name="i"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function ParseNumberToAminoAcid(i As Integer) As String
        Select Case i
            Case 0 : Return "*"
            Case 1 : Return "A"
            Case 2 : Return "C"
            Case 3 : Return "D"
            Case 4 : Return "E"
            Case 5 : Return "F"
            Case 6 : Return "G"
            Case 7 : Return "H"
            Case 8 : Return "I"
            Case 9 : Return "K"
            Case 10 : Return "L"
            Case 11 : Return "M"
            Case 12 : Return "N"
            Case 13 : Return "P"
            Case 14 : Return "Q"
            Case 15 : Return "R"
            Case 16 : Return "S"
            Case 17 : Return "T"
            Case 18 : Return "V"
            Case 19 : Return "W"
            Case 20 : Return "Y"
            Case Else : Return "-"
        End Select
    End Function
    ''' <summary>
    ''' 将数字转化成Codon 0->T 1->C 2->A 3->G
    ''' </summary>
    ''' <param name="i">数字</param>
    ''' <param name="Length">形成的codon的长度</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function ParseNumberToCodon(ByVal i As Integer, ByVal Length As Integer) As String
        'T C A G
        Dim v As Integer = i
        Dim stb As New System.Text.StringBuilder
        Dim k As Integer
        Dim vL As Integer = Length
        While v >= 0 And vL > 0
            k = v Mod 4
            Select Case k
                Case 0
                    stb.Append("T")
                Case 1
                    stb.Append("C")
                Case 2
                    stb.Append("A")
                Case 3
                    stb.Append("G")
            End Select
            v = v \ 4
            vL -= 1
        End While
        Return stb.ToString
    End Function


    Friend Shared Function ParseGeneCharToByte(ByVal Seq As String) As Byte()
        Dim bytes As Byte() = New Byte(Seq.Length - 1) {}
        For i As Integer = 0 To Seq.Length - 1
            Select Case Seq.Chars(i)
                Case "T"
                    bytes(i) = 64
                Case "A"
                    bytes(i) = 65
                Case "G"
                    bytes(i) = 66
                Case "C"
                    bytes(i) = 68
            End Select
        Next
        Return bytes
    End Function
    ''' <summary>
    ''' 检测一个String序列是否全有ATGC构成 如果是返回True
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function CheckTAGC(ByVal text As String) As Boolean
        text = text.ToUpper
        Dim i As Integer
        For i = 0 To text.Length - 1
            If Not (text.Chars(i) = "T" Or text.Chars(i) = "A" Or text.Chars(i) = "G" Or text.Chars(i) = "C") Then Return False
        Next
        Return True
    End Function
    ''' <summary>
    ''' 去除一个序列当中非ATGC的字符 并且取反向互补序列
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function ReverseComplementFilter(ByVal text As String) As String
        text = text.ToUpper
        Dim a As New System.Text.StringBuilder

        Dim c As Char
        Dim i As Integer
        For i = text.Length - 1 To 0 Step -1
            c = text.Chars(i)
            a.Append(IIf(c = "T", "A", IIf(c = "A", "T", IIf(c = "G", "C", IIf(c = "C", "G", "")))))
        Next
        Return a.ToString
    End Function
    Friend Shared Function ReverseComplement(ByVal text As String) As String
        text = text.ToUpper
        Dim a As New System.Text.StringBuilder

        Dim c As Char
        Dim i As Integer
        For i = text.Length - 1 To 0 Step -1
            c = text.Chars(i)
            a.Append(IIf(c = "T", "A", IIf(c = "A", "T", IIf(c = "G", "C", IIf(c = "C", "G", "-")))))
        Next
        Return a.ToString
    End Function
    Private Const A As Char = "A"
    Private Const T As Char = "T"
    Private Const C As Char = "C"
    Private Const G As Char = "G"
    Private Const BT As Char = ">"
    Private Const ST As Char = "<"
    Private Const EQ As Char = "="
    Private Const MN As Char = "-"
    Private Const _A As String = "A"
    Private Const _T As String = "T"
    Private Const _C As String = "C"
    Private Const _G As String = "G"
    Private Const _BT As String = ">"
    Private Const _ST As String = "<"
    Private Const _EQ As String = "="
    Private Const _MN As String = "-"
    Friend Shared Function ReverseComplementF(ByVal V As Char) As String
        Select Case V
            Case A
                Return _T
            Case T
                Return _A
            Case C
                Return _G
            Case G
                Return _C
            Case BT
                Return _ST
            Case ST
                Return _BT
            Case EQ
                Return _EQ
            Case MN
                Return _MN
            Case Else
                Return V
        End Select
    End Function

    Friend Shared Function CanLigate(ByVal str1 As String, ByVal str2 As String) As Boolean
        If str1.Length <> str2.Length Then Return False
        Dim l As Integer = str1.Length
        Dim IsRC As Boolean = True
        '关于磷酸化和末端的定义
        '*3ATGC
        '* phosphate
        '^ no phosphate
        '& recombination end
        '+ 3' - 5'
        '
        IsRC = IsRC And (str1.Chars(0) = chrE Or str2.Chars(0) = chrE)
        IsRC = IsRC And (str1.Chars(1) = str2.Chars(1))
        For i As Integer = 2 To l - 1
            IsRC = IsRC And (str1.Chars(i) = GetCompatibleChar(str2.Chars(l - i + 1)))
        Next
        Return IsRC
    End Function

    Private Const chrL As Char = "L"c
    Private Const chrR As Char = "R"c
    Private Const chrB As Char = "B"c
    Private Const chrP As Char = "P"c
    Private Const chrF As Char = "F"c
    Private Const chrA As Char = "&"c
    Private Const chrE As Char = "*"c
    Private Const chrSpace As Char = " "c

    Friend Shared Function GetCompatibleRecombination(ByVal Char1 As Char) As Char
        '主要应对BP LR形式重组和Frt形式的重组
        Select Case Char1
            Case chrL
                Return chrR
            Case chrR
                Return chrL
            Case chrB
                Return chrP
            Case chrP
                Return chrB
            Case chrF
                Return chrF
            Case chrA
                Return chrA
            Case Else
                Return chrSpace
        End Select
    End Function

    Friend Shared Function CanRecombine(ByVal str1 As String, ByVal str2 As String) As Boolean
        If str1.Length <> str2.Length Then Return False
        Dim l As Integer = str1.Length
        Dim IsRC As Boolean = True
        '关于磷酸化和末端的定义
        '*3ATGC
        '* phosphate
        '^ no phosphate
        '& recombination end
        '+ 3' - 5'
        '
        IsRC = IsRC And (str1.Chars(0) = GetCompatibleRecombination(str2.Chars(0)))
        IsRC = IsRC And (str1.Chars(1) = str2.Chars(1))
        For i As Integer = 2 To l - 1
            IsRC = IsRC And (str1.Chars(i) = GetCompatibleChar(str2.Chars(l - i + 1)))
        Next
        Return IsRC
    End Function

    Private Const vchrA As Char = "A"
    Private Const vchrT As Char = "T"
    Private Const vchrG As Char = "G"
    Private Const vchrC As Char = "C"
    Friend Shared Function GetCompatibleChar(ByVal Value As Char) As Char
        Select Case Value
            Case vchrA
                Return vchrT
            Case vchrT
                Return vchrA
            Case vchrG
                Return vchrC
            Case vchrC
                Return vchrG
            Case "a"
                Return vchrT
            Case "t"
                Return vchrA
            Case "c"
                Return vchrC
            Case "g"
                Return vchrG
        End Select
    End Function

    Friend Shared Function ReverseComplementN(ByVal text As String) As String
        text = text.ToUpper
        Dim a As New System.Text.StringBuilder
        Dim c As Char
        Dim i As Integer
        For i = text.Length - 1 To 0 Step -1
            c = text.Chars(i)
            Select Case c
                Case "A"
                    a.Append("T")
                Case "T"
                    a.Append("A")
                Case "G"
                    a.Append("C")
                Case "C"
                    a.Append("G")
                Case "N"
                    a.Append("N")
                Case "Y" 'TC
                    a.Append("R")
                Case "R" 'GA
                    a.Append("Y")
                Case "M" 'AC
                    a.Append("K")
                Case "K" 'GT
                    a.Append("M")
                Case "W" 'TA
                    a.Append("W")
                Case "S" 'GC
                    a.Append("S")
                Case "D" 'AGT
                    a.Append("H")
                Case "H" 'ACT
                    a.Append("D")
            End Select
        Next
        Return a.ToString
    End Function

    Friend Shared Function Reverse(ByVal text As String) As String
        text = text.ToUpper
        Dim a As String = ""
        Dim c As Char
        Dim i As Integer
        For i = text.Length - 1 To 0 Step -1
            c = text.Chars(i)
            a &= c
        Next
        Return a
    End Function
    Friend Shared Function ComplementChar(ByVal Nucleotide As Char) As Char
        Select Case Nucleotide
            Case "a"c, "A"c
                Return "T"c
            Case "t"c, "T"c
                Return "A"c
            Case "g"c, "G"c
                Return "C"c
            Case "c"c, "C"c
                Return "G"c
            Case "n", "N"c
                Return "N"c
            Case "y"c, "Y"c 'TC
                Return "R"c
            Case "r"c, "R"c 'GA
                Return "Y"c
            Case "m"c, "M"c 'AC
                Return "K"c
            Case "k"c, "K"c 'GT
                Return "M"c
            Case "w"c, "W"c 'TA
                Return "W"c
            Case "s"c, "S"c 'GC
                Return "S"c
            Case "d"c, "D"c 'AGT
                Return "H"c
            Case "h"c, "H"c 'ACT
                Return "D"c
            Case Else
                Return " "c
        End Select
    End Function
    Friend Shared Function Complement(ByVal text As String) As String
        text = text.ToUpper
        Dim a As String = ""
        Dim c As Char
        Dim i As Integer
        For i = 0 To text.Length - 1
            c = text.Chars(i)
            a &= IIf(c = "T", "A", IIf(c = "A", "T", IIf(c = "G", "C", IIf(c = "C", "G", "-"))))
        Next
        Return a
    End Function

    Friend Shared Function ParseEnzymes(value As String, enzyme As RestrictionEnzymes) As List(Of String)
        Dim keys As New Dictionary(Of String, String)
        For Each ez As RestrictionEnzyme In enzyme.Values
            keys.Add(ez.Name.ToLower, ez.Name)
        Next
        Dim res As New List(Of String)
        For Each s As String In value.ToLower.Split(New Char() {" "c, ","c}, System.StringSplitOptions.RemoveEmptyEntries)
            If keys.ContainsKey(s) AndAlso Not res.Contains(s) Then res.Add(keys(s))
        Next
        res.Sort()
        Return res
    End Function

    Public Class CodonTable
        Inherits Dictionary(Of String, Codon)
        Public Sub AddCodon(ByVal c As Codon)
            MyBase.Add(c.ShortName, c)
        End Sub
        Public ReadOnly Property CodonCollection() As System.Collections.ICollection
            Get
                Return MyBase.Values
            End Get
        End Property
        Public Name As String
    End Class
    Public Class Codon
        Public CodeList As New List(Of GeneticCode)

        Public ShortName As String
        Public FullName As String
        Public Sub Add(ByVal GCode As GeneticCode)
            CodeList.Add(GCode)
        End Sub
        Public Sub AddCode(ByVal GCode As GeneticCode)
            CodeList.Add(GCode)
        End Sub
        Public Function GetRatio(ByVal vCode As String) As Single
            For Each c As GeneticCode In CodeList
                If c.Name = vCode Then Return c.ratio
            Next
        End Function
        Default Public Property Code(ByVal i As Integer) As GeneticCode
            Get
                Return CodeList(i)
            End Get
            Set(ByVal value As GeneticCode)
                CodeList(i) = value
            End Set
        End Property
        Public Function GetMaxRatioCode() As GeneticCode
            Dim maxRatio As Single = 0
            Dim maxCode As GeneticCode = Nothing
            For Each GC As GeneticCode In CodeList
                If GC.ratio > maxRatio Then
                    maxCode = GC
                    maxRatio = GC.ratio
                End If
            Next
            Return maxCode
        End Function
    End Class
    Public Class GeneticCode
        Implements IComparable(Of GeneticCode), ICloneable
        Public Function CompareTo(ByVal other As GeneticCode) As Integer Implements System.IComparable(Of GeneticCode).CompareTo
            Return Math.Sign(other.ratio - ratio)
        End Function
        Public Sub New()
        End Sub
        Public Sub New(ByVal vName As String, ByVal vratio As Single)
            Name = vName
            ratio = vratio
        End Sub
        Public Name As String
        Public ratio As Single
        Public CanStart As Boolean
        Public StartRatio As Single
        Public Overrides Function ToString() As String
            Return String.Format("{0} {1}%", Name, Math.Round(ratio * 100))
        End Function
        Public Function Clone() As Object Implements System.ICloneable.Clone
            Return New GeneticCode With {
                .CanStart = CanStart,
                .Name = Name,
                .ratio = ratio,
                .StartRatio = StartRatio}
        End Function
    End Class
    Friend Shared Function LoadCodon(ByVal Filename As String) As Translation
        Dim fi As New IO.FileInfo(Filename)
        If fi.Exists Then
            Try
                Dim sr As IO.StreamReader
                sr = New IO.StreamReader(fi.Open(System.IO.FileMode.Open))
                Dim vKey As String = sr.ReadLine()
                Dim vCodes As String = sr.ReadToEnd()
                Dim regCode As New System.Text.RegularExpressions.Regex("([TAGUC][TAGUC][TAGUC])\s+([a-z\*\-]+)\s+([0-9\.]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim mc As System.Text.RegularExpressions.MatchCollection = regCode.Matches(vCodes)
                Dim tb As New Nuctions.Translation
                tb.Organism = vKey
                Dim c As Nuctions.Codon
                Dim gc As Nuctions.GeneticCode
                For Each m As System.Text.RegularExpressions.Match In mc
                    c = Nuctions.AnminoAcidParse(m.Groups(2).Value)
                    If Not tb.AnimoTable.ContainsKey(c.ShortName) Then tb.AnimoTable.AddCodon(c)
                    gc = New Nuctions.GeneticCode
                    gc.Name = m.Groups(1).Value.ToUpper.Replace("U", "T")
                    gc.ratio = CSng(m.Groups(3).Value)
                    tb.AnimoTable(c.ShortName).CodeList.Add(gc)
                    tb.CodeTable.Add(gc.Name, tb.AnimoTable(c.ShortName))
                Next
                For Each cd As Nuctions.Codon In tb.AnimoTable.Values
                    cd.CodeList.Sort()
                Next
                sr.Close()
                Return tb
            Catch ex As Exception
                Return Nothing
            End Try
        Else
            Return Nothing
        End If
        'Dim TransTable As New Translation

        'TransTable.AnimoTable = New CodonTable
        'TransTable.CodeTable = New Dictionary(Of String, Codon)

        'Dim vfile As New System.IO.FileInfo(Filename)
        'Dim vreader As New System.IO.StreamReader(vfile.FullName)
        'Dim codes As String
        'Dim reg As New System.Text.RegularExpressions.Regex("([a-zA-Z\-])\:([a-zA-Z]+)\>([\w#]+)")
        'Dim mt As System.Text.RegularExpressions.Match
        'Dim amc As Codon

        'Dim regcode As New System.Text.RegularExpressions.Regex("([\w]+)#")
        'Dim mc As System.Text.RegularExpressions.MatchCollection
        'Dim mct As System.Text.RegularExpressions.Match

        'Dim gc As GeneticCode

        'While Not vreader.EndOfStream
        '    codes = vreader.ReadLine()
        '    mt = reg.Match(codes)
        '    amc = New Codon
        '    amc.ShortName = mt.Groups(1).Value.ToUpper
        '    amc.FullName = mt.Groups(2).Value
        '    mc = regcode.Matches(mt.Groups(3).Value)

        '    For Each mct In mc
        '        gc = New GeneticCode(mct.Groups(1).Value.ToUpper, 0)
        '        TransTable.CodeTable.Add(gc.Name, amc)
        '        amc.AddCode(gc)
        '    Next
        '    TransTable.AnimoTable.AddCodon(amc)
        'End While
        'Return TransTable
    End Function
    Friend Shared Function TAGCFilter(ByVal Value As String) As String
        If Value Is Nothing Then Return ""
        Dim data As String = Value.ToUpper
        Dim i As Integer
        Dim stb As New System.Text.StringBuilder
        For i = 0 To data.Length - 1
            stb.Append(IIf(data.Chars(i) = "A"c OrElse data.Chars(i) = "T"c OrElse data.Chars(i) = "G"c OrElse data.Chars(i) = "C"c, data.Chars(i), ""))
        Next
        Return stb.ToString
    End Function
    Friend Shared Function GCFilter(ByVal Value As String) As String
        If Value Is Nothing Then Return ""
        Dim data As String = Value.ToUpper
        Dim i As Integer
        Dim stb As New System.Text.StringBuilder
        For i = 0 To data.Length - 1
            stb.Append(IIf(data.Chars(i) = "G"c OrElse data.Chars(i) = "C"c, data.Chars(i), ""))
        Next
        Return stb.ToString
    End Function
    Friend Shared Function NFilter(value As String) As String
        '("N", "[ATGC]").Replace("Y", "[TC]").Replace("R", "[GA]").Replace("M", "[AC]").Replace("K", "[GT]").Replace("W", "[TA]").Replace("S", "[GC]").Replace("D", "[AGT]").Replace("H", "[ACT]")
        If value Is Nothing Then Return ""
        Dim data As String = value.ToUpper
        Dim stb As New System.Text.StringBuilder
        For Each _Char In data.ToCharArray
            Select Case _Char
                Case "A"c, "T"c, "G"c, "C"c, "N"c, "Y"c, "R"c, "M"c, "K"c, "W"c, "S"c, "D"c, "H"c
                    stb.Append(_Char)
            End Select
        Next
        Return stb.ToString
    End Function
    Friend Shared Function TAGCNFilter(ByVal Value As String) As String
        If Value Is Nothing Then Return ""
        Dim data As String = Value.ToUpper
        'Dim i As Integer
        Dim stb As New System.Text.StringBuilder
        For Each v As Char In data.ToCharArray
            Select Case v
                Case "A"c
                    stb.Append("A")
                Case "T"c
                    stb.Append("T")
                Case "G"c
                    stb.Append("G")
                Case "C"c
                    stb.Append("C")
                Case "N"c
                    stb.Append("N")
                Case "Y"c 'TC
                    stb.Append("Y")
                Case "R"c 'GA
                    stb.Append("R")
                Case "M"c 'AC
                    stb.Append("M")
                Case "K"c 'GT
                    stb.Append("K")
                Case "W"c 'TA
                    stb.Append("W")
                Case "S"c 'GC
                    stb.Append("S")
                Case "D"c 'AGT
                    stb.Append("D")
                Case "H"c 'ACT
                    stb.Append("H")
                Case "B"c 'CGT
                    stb.Append("B")
                Case "V"c 'ACG
                    stb.Append("V")
            End Select 'stb.Append(IIf(data.Chars(i) = "A" OrElse data.Chars(i) = "T" OrElse data.Chars(i) = "G" OrElse data.Chars(i) = "C" OrElse data.Chars(i) = "N", data.Chars(i), ""))
        Next
        Return stb.ToString
    End Function

    Public Class Translation
        Public Organism As String
        Public AnimoTable As New CodonTable
        Public CodeTable As New Dictionary(Of String, Codon)
        Public Shared ReadOnly Property GetDefault As Translation
            Get
                Return SettingEntry.CodonTraslation
            End Get
        End Property
    End Class
    Public Class RestrictionEnzymes
        Inherits Dictionary(Of String, RestrictionEnzyme)

        'Inherits Collections.DictionaryBase

        'Public Sub AddRE(ByVal Name As String, ByVal REnzyme As RestrictionEnzyme)
        '    Dictionary.Add(Name, REnzyme)
        'End Sub

        'Default Public ReadOnly Property RE(ByVal Name As String) As RestrictionEnzyme
        '    Get
        '        If Dictionary.Contains(Name) Then
        '            Return Dictionary.Item(Name)
        '        Else
        '            Return Nothing
        '        End If
        '    End Get
        'End Property
        'Public ReadOnly Property RECollection() As System.Collections.ICollection
        '    Get
        '        Return Dictionary.Values
        '    End Get
        'End Property
        Public Sub AddRE(ByVal Name As String, ByVal REnzyme As RestrictionEnzyme)
            MyBase.Add(Name, REnzyme)
        End Sub
        Default Public Shadows ReadOnly Property RE(ByVal Name As String) As RestrictionEnzyme
            Get
                If MyBase.ContainsKey(Name) Then
                    Return MyBase.Item(Name)
                Else
                    Return Nothing
                End If
            End Get
        End Property
        Public ReadOnly Property RECollection() As System.Collections.ICollection
            Get
                Return MyBase.Values
            End Get
        End Property

    End Class
    Public Class REBase
        Public Name As String
        Public Sequence As String
        Public SCut As Integer
        Public ACut As Integer
        Public Sub New()

        End Sub
        Public Sub New(ByVal RestrictionEnzymeObject As RestrictionEnzyme)
            Name = RestrictionEnzymeObject.Name
            Sequence = RestrictionEnzymeObject.Sequence
            SCut = RestrictionEnzymeObject.SCut
            ACut = RestrictionEnzymeObject.ACut
        End Sub
        Public Function GetRestrictionEnzyme() As RestrictionEnzyme
            Return New RestrictionEnzyme(Name, Sequence, SCut, ACut)
        End Function
    End Class

    Friend Shared Function WildCardNucleotides2RegexPattern(vSequence As String) As String
        Return vSequence.
            Replace("N", "[ATGC]").
            Replace("Y", "[TC]").
            Replace("R", "[GA]").
            Replace("M", "[AC]").
            Replace("K", "[GT]").
            Replace("W", "[TA]").
            Replace("S", "[GC]").
            Replace("D", "[AGT]").
            Replace("H", "[ACT]").
            Replace("B", "[CGT]").
            Replace("V", "[ACG]")
    End Function
    Public Class RestrictionEnzyme
        Implements IComparable(Of RestrictionEnzyme)

        Private nName, nSequence As String
        Private nSCut, nACut As Integer
        Private nReg As System.Text.RegularExpressions.Regex
        Private nOverhang As Integer
        Private nOverhangNT As String
        Private nTag As Object
        Private nPalindromic As Boolean = True
        Private nOverhangPrefix As String

        Public Sub New()
        End Sub
        Public Overrides Function ToString() As String
            Return nName
        End Function
        Public Sub New(ByVal vName As String, ByVal vSequence As String, ByVal vSCut As Integer, ByVal vACut As Integer, Optional ByVal vOverhangPrefix As String = "*")
            nName = vName
            nSequence = vSequence
            nSCut = vSCut
            nACut = vACut
            nOverhang = nSCut - nACut
            nOverhangNT = IIf(nOverhang > 0, "+", IIf(nOverhang < 0, "-", "")) + nSequence.Substring(Math.Min(nSCut, nACut), Math.Abs(nSCut - nACut))

            'add supports for the Ns
            Dim JSequence As String = WildCardNucleotides2RegexPattern(nSequence) 'nSequence.Replace("N", "[ATGC]").Replace("Y", "[TC]").Replace("R", "[GA]").Replace("M", "[AC]").Replace("K", "[GT]").Replace("W", "[TA]").Replace("S", "[GC]").Replace("D", "[AGT]").Replace("H", "[ACT]")

            '   Case "Y" 'TC

            '    Case "R" 'GA

            '    Case "M" 'AC

            '    Case "K" 'GT

            '    Case "W" 'TA

            '    Case "S" 'GC

            '    Case "D" 'AGT

            '    Case "H" 'ACT
            nOverhangPrefix = vOverhangPrefix
            Select Case Math.Sign(vSCut - vACut)
                Case -1
                    nOverhangPrefix += "5"
                Case 0
                    nOverhangPrefix += "B"
                Case 1
                    nOverhangPrefix += "3"
                Case Else
                    nOverhangPrefix += "B"
            End Select
            nPalindromic = nSequence.Equals(ReverseComplementN(nSequence))
            nReg = New System.Text.RegularExpressions.Regex(JSequence, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        End Sub
        Public ReadOnly Property Name() As String
            Get
                Return nName
            End Get
        End Property
        Public ReadOnly Property Sequence() As String
            Get
                Return nSequence
            End Get
        End Property
        Public ReadOnly Property Reg() As System.Text.RegularExpressions.Regex
            Get
                Return nReg
            End Get
        End Property
        Public ReadOnly Property SCut() As Integer
            Get
                Return nSCut
            End Get
        End Property
        Public ReadOnly Property ACut() As Integer
            Get
                Return nACut
            End Get
        End Property
        Public ReadOnly Property Overhang() As Integer
            Get
                Return nOverhang
            End Get
        End Property
        Public ReadOnly Property OverhangNT() As String
            Get
                Return nOverhangNT
            End Get
        End Property
        Public ReadOnly Property OverhangPrefix() As String
            Get
                Return nOverhangPrefix
            End Get
        End Property
        Public Property Tag() As Object
            Get
                Return nTag
            End Get
            Set(ByVal value As Object)
                nTag = value
            End Set
        End Property
        Public ReadOnly Property Palindromic() As Boolean
            Get
                Return nPalindromic
            End Get
        End Property
        Public Function WrapDesignSequence(vSequence As String) As String
            Dim stb As New System.Text.StringBuilder
            Dim j As Integer = 0
            For i As Integer = 0 To nSequence.Length - 1
                Dim vC As Char = nSequence.Chars(i)
                Select Case vC
                    Case "A"c, "T"c, "G"c, "C"c, "a"c, "t"c, "g"c, "c"c

                        stb.Append(vC)
                    Case Else
                        If vSequence.OK And j < vSequence.Length Then
                            Dim uChar As Char = vSequence.Chars(j)
                            If IsATGC(uChar) Then
                                stb.Append(uChar)
                            Else
                                stb.Append(RandomATGC)
                            End If
                        Else
                            stb.Append(RandomATGC)
                        End If
                        j += 1
                End Select
            Next
            Return stb.ToString
        End Function

        Public Function CompareTo(other As RestrictionEnzyme) As Integer Implements System.IComparable(Of RestrictionEnzyme).CompareTo
            Return String.Compare(Name, other.Name)
        End Function
    End Class
    Friend Shared Function IsATGC(vChar As Char) As Boolean
        Select Case vChar
            Case "A"c, "T"c, "G"c, "C"c, "a"c, "t"c, "g"c, "c"c
                Return True
            Case Else
                Return False
        End Select
    End Function
    Friend Shared Function RandomATGC() As String
        Dim i As Integer = Math.Floor(4 * Rnd(Now.ToOADate))
        Select Case i
            Case 0
                Return "A"
            Case 1
                Return "T"
            Case 2
                Return "C"
            Case Else
                Return "G"
        End Select
    End Function
    Friend Class EnzymeAnalysis

        Friend nStartRec As Integer
        Friend nEndRec As Integer
        Friend nACut As Integer
        Friend nSCut As Integer
        Friend nEnzyme As RestrictionEnzyme
        Friend nDNA As GeneFile
        Friend nSOverhang As String
        Friend nAOverhang As String

        Public ReadOnly Property StartRec() As Integer
            Get
                Return nStartRec
            End Get
        End Property
        Public ReadOnly Property EndRec() As Integer
            Get
                Return nEndRec
            End Get
        End Property
        Public ReadOnly Property ACut() As Integer
            Get
                Return nACut
            End Get
        End Property
        Public ReadOnly Property SCut() As Integer
            Get
                Return nSCut
            End Get
        End Property
        Public ReadOnly Property Enzyme() As RestrictionEnzyme
            Get
                Return nEnzyme
            End Get
        End Property
        Public ReadOnly Property DNA() As GeneFile
            Get
                Return nDNA
            End Get
        End Property
        Public ReadOnly Property SOverhang() As String
            Get
                Return nSOverhang
            End Get
        End Property

        Public ReadOnly Property AOverhang() As String
            Get
                Return nAOverhang
            End Get
        End Property

        Friend Class EnzymeAnalysisResult
            Implements Collections.ICollection

            Private stList As New System.Collections.Generic.List(Of EnzymeAnalysis)
            Private nEnzymeCol As New List(Of RestrictionEnzyme)
            Private nDNA As GeneFile
            Private nConfliction As New Queue(Of String)
            Private Shared Function GetOverHangPrefix(ByVal vSCut As Integer, ByVal vACut As Integer) As String
                Select Case Math.Sign(vSCut - vACut)
                    Case -1
                        Return "*3"
                    Case 0
                        Return "*B"
                    Case 1
                        Return "*5"
                    Case Else
                        Return " B"
                End Select
            End Function
            Private Sub Digest()
                For Each vEnzyme In nEnzymeCol
                    Dim circleSide As Integer = vEnzyme.Sequence.Length - 1
                    Dim cSeq As String = nDNA.GetDoubleStrandRegion(vEnzyme.Sequence.Length)

                    Dim mcEnz As System.Text.RegularExpressions.MatchCollection = vEnzyme.Reg.Matches(cSeq)
                    Dim mEnz As System.Text.RegularExpressions.Match
                    Dim eaR As EnzymeAnalysis
                    For Each mEnz In mcEnz
                        eaR = New EnzymeAnalysis
                        eaR.nStartRec = mEnz.Index
                        eaR.nEndRec = mEnz.Index + mEnz.Length
                        eaR.nSCut = eaR.StartRec + vEnzyme.SCut
                        eaR.nACut = eaR.StartRec + vEnzyme.ACut
                        Dim min As Integer = Math.Min(vEnzyme.SCut, vEnzyme.ACut)
                        Dim max As Integer = Math.Max(vEnzyme.SCut, vEnzyme.ACut)
                        Dim oh As Boolean = eaR.nSCut - eaR.nACut > 0
                        Dim ohp As String = vEnzyme.OverhangPrefix
                        Dim ohs As String = mEnz.Groups(0).Value.Substring(min, max - min)
                        'eaR.nSOverhang = ohp + IIf(oh, ohs, ohs)
                        'eaR.nAOverhang = ohp + Nuctions.ReverseComplement(IIf(oh, ohs, ohs))
                        eaR.nSOverhang = ohp + IIf(oh, ohs, Nuctions.ReverseComplement(ohs))
                        eaR.nAOverhang = ohp + IIf(oh, Nuctions.ReverseComplement(ohs), ohs)
                        eaR.nEnzyme = vEnzyme
                        eaR.nDNA = nDNA
                        Me.Add(eaR)
                    Next
                    If Not vEnzyme.Palindromic Then
                        cSeq = nDNA.GetRCDoubleStrandRegion(vEnzyme.Sequence.Length)
                        mcEnz = vEnzyme.Reg.Matches(cSeq)
                        For Each mEnz In mcEnz
                            eaR = New EnzymeAnalysis
                            eaR.nStartRec = nDNA.Sequence.Length - (mEnz.Index + mEnz.Length)
                            eaR.nEndRec = nDNA.Sequence.Length - (mEnz.Index)
                            eaR.nSCut = nDNA.Sequence.Length - (mEnz.Index + vEnzyme.ACut)
                            eaR.nACut = nDNA.Sequence.Length - (mEnz.Index + vEnzyme.SCut)
                            Dim min As Integer = Math.Min(vEnzyme.SCut, vEnzyme.ACut)
                            Dim max As Integer = Math.Max(vEnzyme.SCut, vEnzyme.ACut)
                            Dim oh As Boolean = eaR.nSCut - eaR.nACut > 0
                            Dim ohp As String = vEnzyme.OverhangPrefix
                            Dim ohs As String = mEnz.Groups(0).Value.Substring(min, max - min)
                            eaR.nSOverhang = ohp + IIf(oh, Nuctions.ReverseComplement(ohs), ohs)
                            eaR.nAOverhang = ohp + IIf(oh, ohs, Nuctions.ReverseComplement(ohs))
                            eaR.nEnzyme = vEnzyme
                            eaR.nDNA = nDNA
                            Me.Add(eaR)
                        Next
                    End If
                Next
                'check the confliction of the sites:
                Dim eaItem As EnzymeAnalysis
                Dim eaItrt As EnzymeAnalysis
                For Each eaItem In Me
                    For Each eaItrt In Me
                        If Not (eaItem Is eaItrt) And (eaItem.DNA Is eaItrt.DNA) Then
                            If eaItrt.StartRec < eaItrt.EndRec Then
                                If (eaItrt.StartRec < eaItem.SCut And eaItem.SCut < eaItrt.EndRec) And (eaItrt.StartRec < eaItem.ACut And eaItem.ACut < eaItrt.EndRec) Then _
                                nConfliction.Enqueue("A cut of " + eaItem.Enzyme.Name + " Occurs in " + eaItrt.Enzyme.Name + " At " + eaItrt.StartRec.ToString + " of " + eaItrt.DNA.Name)
                            Else
                                If (eaItrt.StartRec > IIf(eaItem.SCut < eaItem.Enzyme.Sequence.Length, eaItem.SCut + eaItem.DNA.Sequence.Length, eaItem.SCut) _
                                And IIf(eaItem.SCut < eaItem.Enzyme.Sequence.Length, eaItem.SCut + eaItem.DNA.Sequence.Length, eaItem.SCut) > eaItrt.EndRec + eaItrt.DNA.Sequence.Length) _
                                And (eaItrt.StartRec > IIf(eaItem.ACut < eaItem.Enzyme.Sequence.Length, eaItem.ACut + eaItem.DNA.Sequence.Length, eaItem.ACut) _
                                And IIf(eaItem.ACut < eaItem.Enzyme.Sequence.Length, eaItem.ACut + eaItem.DNA.Sequence.Length, eaItem.ACut) > eaItrt.EndRec + eaItrt.DNA.Sequence.Length) Then _
                                nConfliction.Enqueue("A cut of " + eaItem.Enzyme.Name + " Occurs in " + eaItrt.Enzyme.Name + " At " + eaItrt.StartRec.ToString + " of " + eaItrt.DNA.Name)
                            End If
                        End If
                    Next
                Next
            End Sub

            Public Sub New(ByVal vEnzymeCol As List(Of RestrictionEnzyme), ByVal vDNA As GeneFile)
                For Each ez In vEnzymeCol
                    nEnzymeCol.Add(ez)
                Next
                nDNA = vDNA
                Digest()
                'Dim vEnzyme As RestrictionEnzyme
                'For Each vEnzyme In nEnzymeCol


                '    Dim circleSide As Integer = vEnzyme.Sequence.Length - 1
                '    Dim cSeq As String = vDNA.GetDoubleStrandRegion(vEnzyme.Sequence.Length)

                '    Dim mcEnz As System.Text.RegularExpressions.MatchCollection = vEnzyme.Reg.Matches(cSeq)
                '    Dim mEnz As System.Text.RegularExpressions.Match
                '    Dim eaR As EnzymeAnalysis
                '    For Each mEnz In mcEnz
                '        eaR = New EnzymeAnalysis
                '        eaR.nStartRec = mEnz.Index
                '        eaR.nEndRec = mEnz.Index + mEnz.Length
                '        eaR.nSCut = eaR.StartRec + vEnzyme.SCut
                '        eaR.nACut = eaR.StartRec + vEnzyme.ACut
                '        Dim min As Integer = Math.Min(vEnzyme.SCut, vEnzyme.ACut)
                '        Dim max As Integer = Math.Max(vEnzyme.SCut, vEnzyme.ACut)
                '        Dim oh As Boolean = eaR.nSCut - eaR.nACut > 0
                '        Dim ohp As String = vEnzyme.OverhangPrefix
                '        Dim ohs As String = mEnz.Groups(0).Value.Substring(min, max - min)
                '        'eaR.nSOverhang = ohp + IIf(oh, ohs, ohs)
                '        'eaR.nAOverhang = ohp + Nuctions.ReverseComplement(IIf(oh, ohs, ohs))
                '        eaR.nSOverhang = ohp + IIf(oh, ohs, Nuctions.ReverseComplement(ohs))
                '        eaR.nAOverhang = ohp + IIf(oh, Nuctions.ReverseComplement(ohs), ohs)
                '        eaR.nEnzyme = vEnzyme
                '        eaR.nDNA = vDNA
                '        Me.Add(eaR)
                '    Next
                '    If Not vEnzyme.Palindromic Then
                '        cSeq = vDNA.GetRCDoubleStrandRegion(vEnzyme.Sequence.Length)
                '        mcEnz = vEnzyme.Reg.Matches(cSeq)
                '        For Each mEnz In mcEnz
                '            eaR = New EnzymeAnalysis
                '            eaR.nStartRec = vDNA.Sequence.Length - (mEnz.Index + mEnz.Length)
                '            eaR.nEndRec = vDNA.Sequence.Length - (mEnz.Index)
                '            eaR.nSCut = vDNA.Sequence.Length - (mEnz.Index + vEnzyme.ACut)
                '            eaR.nACut = vDNA.Sequence.Length - (mEnz.Index + vEnzyme.SCut)
                '            Dim min As Integer = Math.Min(vEnzyme.SCut, vEnzyme.ACut)
                '            Dim max As Integer = Math.Max(vEnzyme.SCut, vEnzyme.ACut)
                '            Dim oh As Boolean = eaR.nSCut - eaR.nACut > 0
                '            Dim ohp As String = vEnzyme.OverhangPrefix
                '            Dim ohs As String = mEnz.Groups(0).Value.Substring(min, max - min)
                '            eaR.nSOverhang = ohp + IIf(oh, Nuctions.ReverseComplement(ohs), ohs)
                '            eaR.nAOverhang = ohp + IIf(oh, ohs, Nuctions.ReverseComplement(ohs))
                '            eaR.nEnzyme = vEnzyme
                '            eaR.nDNA = vDNA
                '            Me.Add(eaR)
                '        Next
                '    End If
                'Next
                ''check the confliction of the sites:
                'Dim eaItem As EnzymeAnalysis
                'Dim eaItrt As EnzymeAnalysis
                'For Each eaItem In Me
                '    For Each eaItrt In Me
                '        If Not (eaItem Is eaItrt) And (eaItem.DNA Is eaItrt.DNA) Then
                '            If eaItrt.StartRec < eaItrt.EndRec Then
                '                If (eaItrt.StartRec < eaItem.SCut And eaItem.SCut < eaItrt.EndRec) And (eaItrt.StartRec < eaItem.ACut And eaItem.ACut < eaItrt.EndRec) Then _
                '                nConfliction.Enqueue("A cut of " + eaItem.Enzyme.Name + " Occurs in " + eaItrt.Enzyme.Name + " At " + eaItrt.StartRec.ToString + " of " + eaItrt.DNA.Name)
                '            Else
                '                If (eaItrt.StartRec > IIf(eaItem.SCut < eaItem.Enzyme.Sequence.Length, eaItem.SCut + eaItem.DNA.Sequence.Length, eaItem.SCut) _
                '                And IIf(eaItem.SCut < eaItem.Enzyme.Sequence.Length, eaItem.SCut + eaItem.DNA.Sequence.Length, eaItem.SCut) > eaItrt.EndRec + eaItrt.DNA.Sequence.Length) _
                '                And (eaItrt.StartRec > IIf(eaItem.ACut < eaItem.Enzyme.Sequence.Length, eaItem.ACut + eaItem.DNA.Sequence.Length, eaItem.ACut) _
                '                And IIf(eaItem.ACut < eaItem.Enzyme.Sequence.Length, eaItem.ACut + eaItem.DNA.Sequence.Length, eaItem.ACut) > eaItrt.EndRec + eaItrt.DNA.Sequence.Length) Then _
                '                nConfliction.Enqueue("A cut of " + eaItem.Enzyme.Name + " Occurs in " + eaItrt.Enzyme.Name + " At " + eaItrt.StartRec.ToString + " of " + eaItrt.DNA.Name)
                '            End If
                '        End If
                '    Next
                'Next

            End Sub
            Public Sub New(ByVal vEnzymeCol As List(Of String), ByVal vDNA As GeneFile)
                For Each ezs As String In vEnzymeCol
                    Dim re = SettingEntry.EnzymeCol.RE(ezs)
                    If re IsNot Nothing Then nEnzymeCol.Add(re)
                Next
                nDNA = vDNA
                Digest()
                'Dim vEnzyme As RestrictionEnzyme
                'For Each vEnzyme In nEnzymeCol


                '    Dim circleSide As Integer = vEnzyme.Sequence.Length - 1
                '    Dim cSeq As String = vDNA.GetDoubleStrandRegion(vEnzyme.Sequence.Length)

                '    Dim mcEnz As System.Text.RegularExpressions.MatchCollection = vEnzyme.Reg.Matches(cSeq)
                '    Dim mEnz As System.Text.RegularExpressions.Match
                '    Dim eaR As EnzymeAnalysis
                '    For Each mEnz In mcEnz
                '        eaR = New EnzymeAnalysis
                '        eaR.nStartRec = mEnz.Index
                '        eaR.nEndRec = mEnz.Index + mEnz.Length
                '        eaR.nSCut = eaR.StartRec + vEnzyme.SCut
                '        eaR.nACut = eaR.StartRec + vEnzyme.ACut
                '        Dim min As Integer = Math.Min(vEnzyme.SCut, vEnzyme.ACut)
                '        Dim max As Integer = Math.Max(vEnzyme.SCut, vEnzyme.ACut)
                '        Dim oh As Boolean = eaR.nSCut - eaR.nACut > 0
                '        Dim ohp As String = vEnzyme.OverhangPrefix
                '        Dim ohs As String = mEnz.Groups(0).Value.Substring(min, max - min)
                '        'eaR.nSOverhang = ohp + IIf(oh, ohs, ohs)
                '        'eaR.nAOverhang = ohp + Nuctions.ReverseComplement(IIf(oh, ohs, ohs))
                '        eaR.nSOverhang = ohp + IIf(oh, ohs, Nuctions.ReverseComplement(ohs))
                '        eaR.nAOverhang = ohp + IIf(oh, Nuctions.ReverseComplement(ohs), ohs)
                '        eaR.nEnzyme = vEnzyme
                '        eaR.nDNA = vDNA
                '        Me.Add(eaR)
                '    Next
                '    If Not vEnzyme.Palindromic Then
                '        cSeq = vDNA.GetRCDoubleStrandRegion(vEnzyme.Sequence.Length)
                '        mcEnz = vEnzyme.Reg.Matches(cSeq)
                '        For Each mEnz In mcEnz
                '            eaR = New EnzymeAnalysis
                '            eaR.nStartRec = vDNA.Sequence.Length - (mEnz.Index + mEnz.Length)
                '            eaR.nEndRec = vDNA.Sequence.Length - (mEnz.Index)
                '            eaR.nSCut = vDNA.Sequence.Length - (mEnz.Index + vEnzyme.ACut)
                '            eaR.nACut = vDNA.Sequence.Length - (mEnz.Index + vEnzyme.SCut)
                '            Dim min As Integer = Math.Min(vEnzyme.SCut, vEnzyme.ACut)
                '            Dim max As Integer = Math.Max(vEnzyme.SCut, vEnzyme.ACut)
                '            Dim oh As Boolean = eaR.nSCut - eaR.nACut > 0
                '            Dim ohp As String = vEnzyme.OverhangPrefix
                '            Dim ohs As String = mEnz.Groups(0).Value.Substring(min, max - min)
                '            eaR.nSOverhang = ohp + IIf(oh, Nuctions.ReverseComplement(ohs), ohs)
                '            eaR.nAOverhang = ohp + IIf(oh, ohs, Nuctions.ReverseComplement(ohs))
                '            eaR.nEnzyme = vEnzyme
                '            eaR.nDNA = vDNA
                '            Me.Add(eaR)
                '        Next
                '    End If
                'Next
                ''check the confliction of the sites:
                'Dim eaItem As EnzymeAnalysis
                'Dim eaItrt As EnzymeAnalysis
                'For Each eaItem In Me
                '    For Each eaItrt In Me
                '        If Not (eaItem Is eaItrt) And (eaItem.DNA Is eaItrt.DNA) Then
                '            If eaItrt.StartRec < eaItrt.EndRec Then
                '                If (eaItrt.StartRec < eaItem.SCut And eaItem.SCut < eaItrt.EndRec) And (eaItrt.StartRec < eaItem.ACut And eaItem.ACut < eaItrt.EndRec) Then _
                '                nConfliction.Enqueue("A cut of " + eaItem.Enzyme.Name + " Occurs in " + eaItrt.Enzyme.Name + " At " + eaItrt.StartRec.ToString + " of " + eaItrt.DNA.Name)
                '            Else
                '                If (eaItrt.StartRec > IIf(eaItem.SCut < eaItem.Enzyme.Sequence.Length, eaItem.SCut + eaItem.DNA.Sequence.Length, eaItem.SCut) _
                '                And IIf(eaItem.SCut < eaItem.Enzyme.Sequence.Length, eaItem.SCut + eaItem.DNA.Sequence.Length, eaItem.SCut) > eaItrt.EndRec + eaItrt.DNA.Sequence.Length) _
                '                And (eaItrt.StartRec > IIf(eaItem.ACut < eaItem.Enzyme.Sequence.Length, eaItem.ACut + eaItem.DNA.Sequence.Length, eaItem.ACut) _
                '                And IIf(eaItem.ACut < eaItem.Enzyme.Sequence.Length, eaItem.ACut + eaItem.DNA.Sequence.Length, eaItem.ACut) > eaItrt.EndRec + eaItrt.DNA.Sequence.Length) Then _
                '                nConfliction.Enqueue("A cut of " + eaItem.Enzyme.Name + " Occurs in " + eaItrt.Enzyme.Name + " At " + eaItrt.StartRec.ToString + " of " + eaItrt.DNA.Name)
                '            End If
                '        End If
                '    Next
                'Next

            End Sub

            Public Sub New(ByVal vEnzymeCol As List(Of String), ByVal vDNA As GeneFile, ByVal RecomDict As Dictionary(Of String, RestrictionEnzyme))
                For Each ezs As String In vEnzymeCol
                    nEnzymeCol.Add(RecomDict(ezs))
                Next
                nDNA = vDNA
                Digest()
                'Dim vEnzyme As RestrictionEnzyme
                'For Each vEnzyme In nEnzymeCol


                '    Dim circleSide As Integer = vEnzyme.Sequence.Length - 1
                '    Dim cSeq As String = vDNA.GetDoubleStrandRegion(vEnzyme.Sequence.Length)

                '    Dim mcEnz As System.Text.RegularExpressions.MatchCollection = vEnzyme.Reg.Matches(cSeq)
                '    Dim mEnz As System.Text.RegularExpressions.Match
                '    Dim eaR As EnzymeAnalysis
                '    For Each mEnz In mcEnz
                '        eaR = New EnzymeAnalysis
                '        eaR.nStartRec = mEnz.Index
                '        eaR.nEndRec = mEnz.Index + mEnz.Length
                '        eaR.nSCut = eaR.StartRec + vEnzyme.SCut
                '        eaR.nACut = eaR.StartRec + vEnzyme.ACut
                '        Dim min As Integer = Math.Min(vEnzyme.SCut, vEnzyme.ACut)
                '        Dim max As Integer = Math.Max(vEnzyme.SCut, vEnzyme.ACut)
                '        Dim oh As Boolean = eaR.nSCut - eaR.nACut > 0
                '        Dim ohp As String = vEnzyme.OverhangPrefix
                '        Dim ohs As String = mEnz.Groups(0).Value.Substring(min, max - min)
                '        'eaR.nSOverhang = ohp + IIf(oh, ohs, ohs)
                '        'eaR.nAOverhang = ohp + Nuctions.ReverseComplement(IIf(oh, ohs, ohs))
                '        eaR.nSOverhang = ohp + IIf(oh, ohs, Nuctions.ReverseComplement(ohs))
                '        eaR.nAOverhang = ohp + IIf(oh, Nuctions.ReverseComplement(ohs), ohs)
                '        eaR.nEnzyme = vEnzyme
                '        eaR.nDNA = vDNA
                '        Me.Add(eaR)
                '    Next
                '    If Not vEnzyme.Palindromic Then
                '        cSeq = vDNA.GetRCDoubleStrandRegion(vEnzyme.Sequence.Length)
                '        mcEnz = vEnzyme.Reg.Matches(cSeq)
                '        For Each mEnz In mcEnz
                '            eaR = New EnzymeAnalysis
                '            eaR.nStartRec = vDNA.Sequence.Length - (mEnz.Index + mEnz.Length)
                '            eaR.nEndRec = vDNA.Sequence.Length - (mEnz.Index)
                '            eaR.nSCut = vDNA.Sequence.Length - (mEnz.Index + vEnzyme.ACut)
                '            eaR.nACut = vDNA.Sequence.Length - (mEnz.Index + vEnzyme.SCut)
                '            Dim min As Integer = Math.Min(vEnzyme.SCut, vEnzyme.ACut)
                '            Dim max As Integer = Math.Max(vEnzyme.SCut, vEnzyme.ACut)
                '            Dim oh As Boolean = eaR.nSCut - eaR.nACut > 0
                '            Dim ohp As String = vEnzyme.OverhangPrefix
                '            Dim ohs As String = mEnz.Groups(0).Value.Substring(min, max - min)
                '            eaR.nSOverhang = ohp + IIf(oh, Nuctions.ReverseComplement(ohs), ohs)
                '            eaR.nAOverhang = ohp + IIf(oh, ohs, Nuctions.ReverseComplement(ohs))
                '            eaR.nEnzyme = vEnzyme
                '            eaR.nDNA = vDNA
                '            Me.Add(eaR)
                '        Next
                '    End If
                'Next
                ''check the confliction of the sites:
                'Dim eaItem As EnzymeAnalysis
                'Dim eaItrt As EnzymeAnalysis
                'For Each eaItem In Me
                '    For Each eaItrt In Me
                '        If Not (eaItem Is eaItrt) And (eaItem.DNA Is eaItrt.DNA) Then
                '            If eaItrt.StartRec < eaItrt.EndRec Then
                '                If (eaItrt.StartRec < eaItem.SCut And eaItem.SCut < eaItrt.EndRec) And (eaItrt.StartRec < eaItem.ACut And eaItem.ACut < eaItrt.EndRec) Then _
                '                nConfliction.Enqueue("A cut of " + eaItem.Enzyme.Name + " Occurs in " + eaItrt.Enzyme.Name + " At " + eaItrt.StartRec.ToString + " of " + eaItrt.DNA.Name)
                '            Else
                '                If (eaItrt.StartRec > IIf(eaItem.SCut < eaItem.Enzyme.Sequence.Length, eaItem.SCut + eaItem.DNA.Sequence.Length, eaItem.SCut) _
                '                And IIf(eaItem.SCut < eaItem.Enzyme.Sequence.Length, eaItem.SCut + eaItem.DNA.Sequence.Length, eaItem.SCut) > eaItrt.EndRec + eaItrt.DNA.Sequence.Length) _
                '                And (eaItrt.StartRec > IIf(eaItem.ACut < eaItem.Enzyme.Sequence.Length, eaItem.ACut + eaItem.DNA.Sequence.Length, eaItem.ACut) _
                '                And IIf(eaItem.ACut < eaItem.Enzyme.Sequence.Length, eaItem.ACut + eaItem.DNA.Sequence.Length, eaItem.ACut) > eaItrt.EndRec + eaItrt.DNA.Sequence.Length) Then _
                '                nConfliction.Enqueue("A cut of " + eaItem.Enzyme.Name + " Occurs in " + eaItrt.Enzyme.Name + " At " + eaItrt.StartRec.ToString + " of " + eaItrt.DNA.Name)
                '            End If
                '        End If
                '    Next
                'Next

            End Sub
            Public Function CutDNA() As List(Of GeneFile)
                'if this function returns nothing, it means that there is conflictions
                If nConfliction.Count > 0 Then Return Nothing
                'else there will be a list of cutted dna
                'we should first sort the sites
                Dim listGF As New List(Of GeneFile)
                Dim gf As GeneFile
                If stList.Count = 0 Then

                    listGF.Add(nDNA.CloneWithoutFeatures)
                Else


                    stList.Sort(New EnzymeAnalysisSorter)
                    'then start from 0
                    Dim i As Integer
                    Dim Start1 As Integer
                    Dim End1 As Integer
                    Dim Start2 As Integer
                    Dim End2 As Integer
                    Dim stb As System.Text.StringBuilder
                    Dim lastA As EnzymeAnalysis
                    Dim nextA As EnzymeAnalysis

                    If nDNA.Iscircular Then 'Check whether this is a circular DNA.
                        For i = 0 To stList.Count - 1
                            gf = New GeneFile
                            'gf.Iscircular = False
                            gf.ModificationDate = System.DateTime.Now
                            gf.Name = "EnzFrag-" + nDNA.Name + "-" + i.ToString
                            gf.Phos_F = True
                            gf.Phos_R = True
                            If i = stList.Count - 1 Then
                                'set sequence
                                lastA = stList(i)
                                nextA = stList(0)

                                stb = New System.Text.StringBuilder

                                Start1 = Math.Min(lastA.SCut, lastA.ACut)
                                End1 = Math.Max(lastA.SCut, lastA.ACut)

                                Start2 = Math.Min(nextA.SCut, nextA.ACut)
                                End2 = Math.Max(nextA.SCut, nextA.ACut)


                                If lastA.AOverhang.Length > 2 AndAlso Start1 <> End1 - 1 Then stb.Append(nDNA.SubSequence(Start1, End1 - 1))
                                stb.Append(nDNA.SubSequence(End1, Start2 - 1))
                                If nextA.SOverhang.Length > 2 AndAlso Start2 <> End2 - 1 Then stb.Append(nDNA.SubSequence(Start2, End2 - 1))

                                gf.Sequence = stb.ToString


                                gf.End_F = lastA.AOverhang
                                gf.End_R = nextA.SOverhang
                            Else
                                'set sequence
                                lastA = stList(i)
                                nextA = stList(i + 1)

                                stb = New System.Text.StringBuilder

                                Start1 = Math.Min(lastA.SCut, lastA.ACut)
                                End1 = Math.Max(lastA.SCut, lastA.ACut)

                                Start2 = Math.Min(nextA.SCut, nextA.ACut)
                                End2 = Math.Max(nextA.SCut, nextA.ACut)

                                If lastA.AOverhang.Length > 2 AndAlso Start1 <> End1 - 1 Then stb.Append(nDNA.SubSequence(Start1, End1 - 1))
                                stb.Append(nDNA.SubSequence(End1, Start2 - 1))
                                If nextA.SOverhang.Length > 2 AndAlso Start2 <> End2 - 1 Then stb.Append(nDNA.SubSequence(Start2, End2 - 1))

                                gf.Sequence = stb.ToString


                                gf.End_F = lastA.AOverhang
                                gf.End_R = nextA.SOverhang

                            End If
                            'find feathures
                            listGF.Add(gf)
                        Next
                    Else
                        'the first one
                        i = 0
                        nextA = stList(0)
                        End1 = 0
                        Start2 = Math.Min(nextA.SCut, nextA.ACut)
                        If Start2 > End1 Then
                            'set sequence
                            gf = New GeneFile
                            'gf.Iscircular = False
                            gf.ModificationDate = System.DateTime.Now
                            gf.Name = "EnzFrag-" + nDNA.Name + "-" + i.ToString
                            gf.Phos_F = nDNA.Phos_F
                            gf.Phos_R = True
                            gf.End_F = nDNA.End_F
                            'lastA = stList(i)


                            stb = New System.Text.StringBuilder

                            'Start1 = Math.Min(lastA.SCut, lastA.ACut)



                            End2 = Math.Max(nextA.SCut, nextA.ACut)

                            'stb.Append(nDNA.SubSequence(Start1, End1))
                            stb.Append(nDNA.SubSequence(End1, Start2 - 1))
                            If nextA.SOverhang.Length > 2 Then stb.Append(nDNA.SubSequence(Start2, End2 - 1))

                            gf.Sequence = stb.ToString
                            gf.End_R = nextA.SOverhang



                            listGF.Add(gf)
                        End If

                        'the inner ones
                        For i = 0 To stList.Count - 2
                            gf = New GeneFile
                            'gf.Iscircular = False
                            gf.ModificationDate = System.DateTime.Now
                            gf.Name = "EnzFrag-" + nDNA.Name + "-" + i.ToString
                            gf.Phos_F = True
                            gf.Phos_R = True
                            'set sequence
                            lastA = stList(i)
                            nextA = stList(i + 1)

                            stb = New System.Text.StringBuilder

                            Start1 = Math.Min(lastA.SCut, lastA.ACut)
                            End1 = Math.Max(lastA.SCut, lastA.ACut)

                            Start2 = Math.Min(nextA.SCut, nextA.ACut)
                            End2 = Math.Max(nextA.SCut, nextA.ACut)

                            If lastA.AOverhang.Length > 2 Then stb.Append(nDNA.SubSequence(Start1, End1 - 1))
                            stb.Append(nDNA.SubSequence(End1, Start2 - 1))
                            If nextA.SOverhang.Length > 2 Then stb.Append(nDNA.SubSequence(Start2, End2 - 1))

                            gf.Sequence = stb.ToString

                            gf.End_F = lastA.AOverhang
                            gf.End_R = nextA.SOverhang

                            listGF.Add(gf)
                        Next

                        'the last one
                        i = stList.Count - 1

                        lastA = stList(i)
                        Start1 = Math.Min(lastA.SCut, lastA.ACut)
                        End1 = Math.Max(lastA.SCut, lastA.ACut)
                        Start2 = nDNA.Length

                        If Start2 > End1 Then
                            gf = New GeneFile
                            'gf.Iscircular = False
                            gf.ModificationDate = System.DateTime.Now
                            gf.Name = "EnzFrag-" + nDNA.Name + "-" + i.ToString
                            gf.Phos_F = True
                            gf.Phos_R = nDNA.Phos_R
                            gf.End_R = nDNA.End_R


                            stb = New System.Text.StringBuilder




                            'End2 = Math.Max(nextA.SCut, nextA.ACut)

                            If lastA.AOverhang.Length > 2 Then stb.Append(nDNA.SubSequence(Start1, End1 - 1))
                            stb.Append(nDNA.SubSequence(End1, Start2 - 1))
                            'stb.Append(nDNA.SubSequence(Start2, End2))

                            gf.Sequence = stb.ToString


                            gf.End_F = lastA.AOverhang


                            'gf.End_R = nextA.AOverhang
                            'gf.End_R = nDNA.End_R

                            listGF.Add(gf)
                        End If


                    End If
                End If
                Return listGF
            End Function

            Public ReadOnly Property CutList() As List(Of EnzymeAnalysis)
                Get
                    Return stList
                End Get
            End Property

            Friend Class EnzymeAnalysisSorter
                Implements Collections.Generic.IComparer(Of EnzymeAnalysis)
                Public Function Compare(ByVal x As EnzymeAnalysis, ByVal y As EnzymeAnalysis) As Integer Implements System.Collections.Generic.IComparer(Of EnzymeAnalysis).Compare
                    Return x.SCut - y.SCut
                End Function
            End Class
            Private Sub Add(ByVal nValue As EnzymeAnalysis)
                stList.Add(nValue)
            End Sub
            Default Public ReadOnly Property ResultItems(ByVal nIndex As Integer) As EnzymeAnalysis
                Get
                    Return stList(nIndex)
                End Get
            End Property
            Public ReadOnly Property EnzymeCol() As List(Of RestrictionEnzyme)
                Get
                    Return nEnzymeCol
                End Get
            End Property
            Public ReadOnly Property DNA() As GeneFile
                Get
                    Return nDNA
                End Get
            End Property
            Public ReadOnly Property Confliction() As Queue(Of String)
                Get
                    Return nConfliction
                End Get
            End Property

            Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo

            End Sub

            Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
                Get
                    Return stList.Count
                End Get
            End Property

            Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
                Get
                    Return False
                End Get
            End Property

            Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
                Get
                    Return Nothing
                End Get
            End Property

            Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
                Return stList.GetEnumerator
            End Function
        End Class
    End Class
    Friend Shared Function LoadRestrictionEnzymes(ByVal FileName As String) As RestrictionEnzymes
        Dim res As New RestrictionEnzymes
        Dim vfile As New System.IO.FileInfo(FileName)
        Dim vreader As New System.IO.StreamReader(vfile.FullName)
        Dim codes As String
        Dim reg As New System.Text.RegularExpressions.Regex("([\w]+)#([\w]+)#([0-9]+)#([0-9]+)")
        Dim mt As System.Text.RegularExpressions.Match
        While Not vreader.EndOfStream
            codes = vreader.ReadLine
            mt = reg.Match(codes)
            Dim enz As New RestrictionEnzyme(mt.Groups(1).Value, mt.Groups(2).Value, Int(mt.Groups(3).Value), Int(mt.Groups(4).Value))
            res.AddRE(enz.Name, enz)
        End While
        Return res
    End Function

    Private Shared RecombiningRGX As New System.Text.RegularExpressions.Regex("^[FBPLR&]")
    Public Class GeneFileBuilder
        Public Name As String
        Public Sequences As New System.Text.StringBuilder
        Public End_F As String
        Public End_R As String
        Public Enzymes As New List(Of String)
        Public PrimerMode As Boolean = False
        Public Function ToGeneFile() As GeneFile
            Dim gf As New GeneFile
            gf.Name = Name
            If End_F.OK AndAlso End_F.Length >= 2 Then
                Select Case End_F.ToUpper.Substring(End_F.Length - 2, 2)
                    Case "=B"
                        gf.End_F = "=B"
                    Case "*B"
                        gf.End_F = "*B"
                    Case "^B"
                        gf.End_F = "^B"
                    Case "0B"
                        gf.End_F = "0B"
                    Case "::"
                        gf.End_F = "::"
                        End_R = "::"
                    Case "*3"
                        gf.End_F = TAGCFilter(End_F) & "*3"
                        Sequences.Insert(0, TAGCFilter(End_F))
                    Case "^3"
                        gf.End_F = TAGCFilter(End_F) & "^3"
                        Sequences.Insert(0, TAGCFilter(End_F))
                    Case "*5"
                        gf.End_F = TAGCFilter(End_F) & "*5"
                        Sequences.Insert(0, TAGCFilter(End_F))
                    Case "^5"
                        gf.End_F = TAGCFilter(End_F) & "^5"
                        Sequences.Insert(0, TAGCFilter(End_F))
                End Select
            Else
                gf.End_F = "*B"
            End If
            If End_R.OK AndAlso End_R.Length >= 2 Then
                Select Case End_R.ToUpper.Substring(End_R.Length - 2, 2)
                    Case "=B"
                        gf.End_R = "=B"
                    Case "*B"
                        gf.End_R = "*B"
                    Case "^B"
                        gf.End_R = "^B"
                    Case "0B"
                        gf.End_R = "0B"
                    Case "::"
                        gf.End_R = "::"
                    Case "*3"
                        gf.End_R = TAGCFilter(End_R) & "*3"
                        Sequences.Append(TAGCFilter(End_R))
                    Case "^3"
                        gf.End_R = TAGCFilter(End_R) & "^3"
                        Sequences.Append(TAGCFilter(End_R))
                    Case "*5"
                        gf.End_R = TAGCFilter(End_R) & "*5"
                        Sequences.Append(TAGCFilter(End_R))
                    Case "^5"
                        gf.End_R = TAGCFilter(End_R) & "^5"
                        Sequences.Append(TAGCFilter(End_R))
                End Select
            Else
                gf.End_R = "*B"
            End If
            gf.Sequence = Sequences.ToString
            Return gf
        End Function
    End Class
    <Serializable>
    Public Class GeneFile
        Friend Shared Function LoadFromGeneBankFormatString(Value As String) As GeneFile
            Dim Sections = SectionDivider.Divide(Value, "(^|\n)(//|\w+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            Dim gf As New GeneFile

            Dim LOCI = SectionDivider.SelectSection(Sections, "^locus", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            If LOCI.Count = 1 Then
                Dim LOCUS = LOCI(0)
                gf.Name = System.Text.RegularExpressions.Regex.Match(LOCUS, "LOCUS\s+([^\s]+)").Groups(1).Value

                Dim topo = System.Text.RegularExpressions.Regex.Match(LOCUS, "LOCUS\s+([^\s]+)\s+(\d+)\s+(\w+)\s+(\w+)\s+(\w+)\s+").Groups(5).Value
                Dim topoGroup = System.Text.RegularExpressions.Regex.Match(LOCUS, "LOCUS\s+([^\s]+)\s+(\d+)\s+(\w+)\s+(\w+)\s+(\w+)\s+").Groups(5)
                topo = topoGroup.Value
                gf.vDate = CDate(System.Text.RegularExpressions.Regex.Match(LOCUS.Substring(topoGroup.Index), "\w+-\w+-\w+").Groups(0).Value)
                If topo.ToLower = "linear" Then
                    gf.End_F = "*B"
                    gf.End_R = "*B"
                Else
                    gf.End_F = "::"
                    gf.End_R = "::"
                End If
            End If

            Dim Origins = SectionDivider.SelectSection(Sections, "^origin", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            Dim codes As String
            If Origins.Count = 1 Then
                Dim ORIGIN As String = Origins(0)
                codes = Nuctions.TAGCNFilter(ORIGIN.Substring(6))
                gf.vSequence = codes
                gf.vRCSequence = Nuctions.ReverseComplementN(codes)
            End If

            Dim Features = SectionDivider.SelectSection(Sections, "^features", System.Text.RegularExpressions.RegexOptions.IgnoreCase)


            Dim regFeature As New System.Text.RegularExpressions.Regex("\s+(\w+)\s+(\d+)\.\.(\d+)")
            Dim regFeatureComplement As New System.Text.RegularExpressions.Regex("\s+(\w+)\s+complement\((\d+)\.\.(\d+)\)")
            For Each f In Features
                Dim fSections = SectionDivider.Divide(f, "(^|\n)\s{1,9}(\w+)\s+\w+", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                For Each fSec In fSections
                    Dim ff = regFeature.Match(fSec)
                    Dim rf = regFeatureComplement.Match(fSec)
                    Dim ga As New GeneAnnotation With {.Vector = gf}

                    If ff.Success Then
                        ga.Complement = False
                        ga.Type = ff.Groups(1).Value
                        ga.StartPosition = CInt(ff.Groups(2).Value)
                        ga.EndPosition = CInt(ff.Groups(3).Value)
                    ElseIf rf.Success Then
                        ga.Complement = True
                        ga.Type = rf.Groups(1).Value
                        ga.StartPosition = CInt(rf.Groups(2).Value)
                        ga.EndPosition = CInt(rf.Groups(3).Value)
                    End If
                    Dim sfSections = SectionDivider.Divide(fSec, "(^|\n)\s+/\w+\s*=\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                    For i As Integer = 0 To sfSections.Count - 1
                        sfSections(i) = System.Text.RegularExpressions.Regex.Replace(sfSections(i), "(^|\n)\s+", "")
                    Next
                    Dim rMeta As New System.Text.RegularExpressions.Regex("^/(\w+)\s*=\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                    Dim protein_id As String
                    For Each meta In sfSections
                        Dim m = rMeta.Match(meta)
                        Dim field = m.Groups(1).Value.ToLower
                        Dim data = KEGGUtil.RemoveCrLf(SectionDivider.RemoveQuotation(meta.Substring(m.Length)))
                        Select Case field
                            Case "transl_table"

                                data = System.Text.RegularExpressions.Regex.Match(data, "\d+").Value
                            'ga.TranslationTable = CInt(data)
                            'ga.MetaData.Add(New KeyValuePair(Of String, String)(field, data))
                            Case "translation"
                                'Dim data = SectionDivider.RemoveQuotation(meta.Substring(m.Length))
                                'ga.Translation = Nuctions.AminoAcidFilter(data)
                                'ga.MetaData.Add(New KeyValuePair(Of String, String)(field, data))
                            Case "locus_tag"

                                'Dim data = SectionDivider.RemoveQuotation(meta.Substring(m.Length))

                                'If data = "b0002" Then Stop
                                ga.Label = data

                            'ga.MetaData.Add(New KeyValuePair(Of String, String)(field, data))
                            Case "product"
                                'Dim data = SectionDivider.RemoveQuotation(meta.Substring(m.Length))
                                ga.Note = data
                            'ga.MetaData.Add(New KeyValuePair(Of String, String)(field, data))
                            Case "codon_start"
                                'Dim data = SectionDivider.RemoveQuotation(meta.Substring(m.Length))
                                'ga.CodonStartPosition = CInt(data)
                                'ga.MetaData.Add(New KeyValuePair(Of String, String)(field, data))
                            Case "gene"
                                ga.Label = data
                            Case "label"
                                ga.Label = data
                            Case "note"
                                ga.Note = data
                            Case "protein_id"
                                protein_id = data
                            Case Else
                                'Dim data = data
                                'ga.MetaData.Add(New KeyValuePair(Of String, String)(field, data))
                        End Select
                    Next
                    If ga.Label Is Nothing OrElse ga.Label = "" Then
                        If ga.Note IsNot Nothing AndAlso ga.Note.Length > 0 Then
                            ga.Label = ga.Note
                        ElseIf protein_id IsNot Nothing AndAlso protein_id.Length > 0 Then
                            ga.Label = protein_id
                        End If
                    End If
                    gf.Features.Add(ga)
                Next
            Next
            Return gf

        End Function
        Friend Shared Function LoadFromGeneBankFile(ByVal FileName As String) As GeneFile
            Dim value As String = IO.File.ReadAllText(FileName)
            Return LoadFromGeneBankFormatString(value)
            'Dim gf As New GeneFile
            'Dim vfile As New System.IO.FileInfo(FileName)
            'Dim vreader As New System.IO.StreamReader(vfile.FullName)
            'Dim codes As String = ""
            ''codes to read gene sequences
            'Dim regGB As New System.Text.RegularExpressions.Regex("ORIGIN([\satgcATGC1234567890]+)\/\/", System.Text.RegularExpressions.RegexOptions.None)

            'Dim f As New System.IO.FileInfo(FileName)
            'FileName = FileName.ToUpper()
            'If System.IO.File.Exists(FileName) Then
            '    codes = System.IO.File.ReadAllText(FileName)
            '    gf.vSequence = TAGCNFilter(codes.Substring(codes.LastIndexOf("ORIGIN") + 6))
            '    gf.vRCSequence = Nuctions.ReverseComplementN(gf.vSequence)
            'End If

            ''codes to read annotations
            ''Dim regFN As New System.Text.RegularExpressions.Regex("LOCUS\s+([-\w\\\[\]\(\)\x72-\xFF]+)\s+(\d+)\s+bp\s+(\w+)\s+(\w+)[\sa-zA-Z]*(\d+-\w+-\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ''Dim regTest As New System.Text.RegularExpressions.Regex("\d+-\w+-\d+", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            'Dim regFN As New System.Text.RegularExpressions.Regex("LOCUS\s+(\S+)\s+(\d+)\s+bp\s+(\w+)\s+(\w+)[\sa-zA-Z]*(\d+-\w+-\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ''LOCUS       AY048736     2345 bp    DNA   circular  SYN       17-OCT-2001
            ''LOCUS       T7\1100BP    1120 bp    DNA                        7-DEC-2009
            'Dim fr As New System.IO.StreamReader(FileName)
            'Dim line As String = fr.ReadLine

            'Dim values As String() = line.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
            'gf.vName = values(1).Replace("\", " ")
            'Dim dix As Integer
            'dix = 5
            'While dix < values.Length
            '    If values(dix).ToLower = "circular" Then
            '        gf.vEnd_F = "::" : gf.vEnd_R = "::"
            '    End If

            '    Try
            '        gf.vDate = CDate(values(dix))
            '    Catch ex As Exception

            '    End Try
            '    dix += 1
            'End While
            ''


            'Dim minfo As System.Text.RegularExpressions.Match
            ''minfo = regFN.Match(line)
            ''If Not minfo.Success Then Return Nothing 'this is not a gb file
            '''minfo = regTest.Match(line)
            ''gf.vName = minfo.Groups(1).Value
            ''If minfo.Groups(3).Value.IndexOf("DNA") < 0 Then Return Nothing ' this should be a DNA file
            ''gf.vIscircular = (minfo.Groups(4).Value.IndexOf("circular") > -1)
            ''If gf.vIscircular Then gf.vEnd_F = "::" : gf.vEnd_R = "::"
            ''gf.vDate = CDate(minfo.Groups(5).Value)

            ''read features
            'Dim readfeature As Boolean = False
            'Dim regFT As New System.Text.RegularExpressions.Regex("FEATURES\s{13}Location/Qualifiers", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            'Dim regRG As New System.Text.RegularExpressions.Regex("\s+([a-zA-Z_]+)\s*(\d+)\.\.(\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            'Dim regRC As New System.Text.RegularExpressions.Regex("\s+([a-zA-Z_]+)\s*complement\((\d+)\.\.(\d+)\)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            'Dim regLabel As New System.Text.RegularExpressions.Regex("/label=""?([^""^=^\n]+)""?", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            'Dim regNote As New System.Text.RegularExpressions.Regex("/note=""?([^""^=^\n]+)""?", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            'Dim regGene As New System.Text.RegularExpressions.Regex("/gene=""?([^""^=^\n]+)""?", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            'Dim regProduct As New System.Text.RegularExpressions.Regex("/product=""?([^""^=^\n]+)""?", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            'Dim infeature As Boolean = False
            'Dim uFeature As GeneAnnotation

            ''gf.vFeatureCol = New Dictionary(Of Integer, GeneAnnotation)
            'If regFT.IsMatch(codes) Then
            '    While (Not readfeature)
            '        'untile we find the feature map
            '        line = fr.ReadLine
            '        readfeature = regFT.IsMatch(line)
            '    End While
            '    line = fr.ReadLine

            '    While (readfeature)
            '        If line Is Nothing Then readfeature = False : Exit While
            '        uFeature = New GeneAnnotation
            '        uFeature.Index = gf.vFeatureCol.Count
            '        uFeature.Label = ""
            '        uFeature.Note = ""
            '        If regRG.IsMatch(line) Then
            '            minfo = regRG.Match(line)
            '            uFeature.Type = minfo.Groups(1).Value
            '            uFeature.Complement = False
            '            uFeature.StartPosition = CInt(minfo.Groups(2).Value)
            '            uFeature.EndPosition = CInt(minfo.Groups(3).Value)
            '            infeature = True
            '        ElseIf regRC.IsMatch(line) Then
            '            minfo = regRC.Match(line)
            '            uFeature.Type = minfo.Groups(1).Value
            '            uFeature.Complement = True
            '            uFeature.StartPosition = CInt(minfo.Groups(2).Value)
            '            uFeature.EndPosition = CInt(minfo.Groups(3).Value)
            '            infeature = True
            '        Else
            '            readfeature = False
            '            infeature = False
            '        End If
            '        'Select Case rgxContent.Matches(line).Count = 1
            '        '    Case 0

            '        '    Case 1
            '        '        inContent = Not inContent
            '        '    Case 2

            '        'End Select)
            '        While (infeature)
            '            line = fr.ReadLine
            '            If line Is Nothing Then readfeature = False : Exit While
            '            If regLabel.IsMatch(line) Then
            '                minfo = regLabel.Match(line)
            '                uFeature.Label = minfo.Groups(1).Value.Replace("\", " ")
            '            End If
            '            If regNote.IsMatch(line) Then
            '                minfo = regNote.Match(line)
            '                uFeature.Note = minfo.Groups(1).Value.Replace("\", " ")
            '            End If
            '            If regGene.IsMatch(line) Then
            '                minfo = regGene.Match(line)
            '                uFeature.Label = minfo.Groups(1).Value.Replace("\", " ")
            '            End If
            '            If regProduct.IsMatch(line) Then
            '                minfo = regProduct.Match(line)
            '                uFeature.Note = minfo.Groups(1).Value.Replace("\", " ")
            '            End If

            '            If line.IndexOf("/") < 0 AndAlso (regRG.IsMatch(line) Or regRC.IsMatch(line)) Then infeature = False
            '        End While
            '        If uFeature.Type = "f_end" Then
            '            gf.End_F = uFeature.Label
            '        ElseIf uFeature.Type = "r_end" Then
            '            gf.End_R = uFeature.Label
            '        Else
            '            If readfeature Then
            '                If uFeature.Label.Length = 0 And uFeature.Note.Length > 0 Then uFeature.Label = uFeature.Note
            '                uFeature.Vector = gf : gf.vFeatureCol.Add(uFeature)
            '            End If
            '        End If
            '    End While
            'End If
            'Return gf
        End Function

        Private vName As String 'read from the genebank file
        Private vIscircular As Boolean 'read from the genebank file
        Private vDate As Date = Date.Now
        Private vSequence As String = ""
        Private vRCSequence As String = ""
        Private vFeatureCol As New List(Of GeneAnnotation)
        Private vEnd_F As String = "*B"
        Private vEnd_R As String = "*B"
        Private vPhos_F As Boolean = True
        Private vPhos_R As Boolean = True
        '仅用于切刻酶的项目
        Public FCleavages As New List(Of Integer)
        Public RCleavages As New List(Of Integer)
        Private Shared MD5 As New System.Security.Cryptography.MD5CryptoServiceProvider
        'Private vChromosomal As Boolean
        Public Property Chromosomal As Boolean
            Get
                Return (vEnd_F = "=B" And vEnd_R = "=B") OrElse (vEnd_F = "=B" And vEnd_R = "0B") OrElse (vEnd_F = "0B" And vEnd_R = "=B")
            End Get
            Set(value As Boolean)
                If value Then
                    If vEnd_F = "*B" Or vEnd_F = "^B" Then
                        vEnd_F = "=B"
                    End If
                    If vEnd_R = "*B" Or vEnd_R = "^B" Then
                        vEnd_R = "=B"
                    End If
                Else
                    If vEnd_F = "=B" Then
                        vEnd_F = "*B"
                    End If
                    If vEnd_R = "=B" Then
                        vEnd_R = "*B"
                    End If
                End If
            End Set
        End Property
        Public Function CanConjugate(oriT As List(Of String)) As Boolean
            If vEnd_F.Contains("*") OrElse vEnd_F.Contains("^") OrElse vEnd_R.Contains("*") OrElse vEnd_R.Contains("^") Then Return False
            For Each f As GeneAnnotation In Features
                If oriT.Contains(f.Label) AndAlso f.Type = "oriT" Then Return True
            Next
            Return False
        End Function
        Public Function CanReplicate(oriR As List(Of String)) As Boolean
            If Chromosomal Then Return True
            If vEnd_F.Contains("*") OrElse vEnd_F.Contains("^") OrElse vEnd_R.Contains("*") OrElse vEnd_R.Contains("^") Then Return False
            For Each f As GeneAnnotation In Features
                If oriR.Contains(f.Label) AndAlso f.Type = "rep_origin" Then Return True
            Next
            Return False
        End Function
        Public ReadOnly Property BioFunctions As Dictionary(Of FeatureFunctionEnum, List(Of String))
            Get
                Dim bfd As New Dictionary(Of FeatureFunctionEnum, List(Of String))
                For Each ffe As FeatureFunctionEnum In [Enum].GetValues(GetType(FeatureFunctionEnum))
                    bfd.Add(ffe, New List(Of String))
                Next
                Dim f As Feature
                For Each ga As GeneAnnotation In Features
                    f = ga.Feature
                    If f IsNot Nothing Then
                        For Each bf As FeatureFunction In f.BioFunctions
                            bfd(bf.BioFunction).Add(bf.Parameters)
                        Next
                    End If
                Next
                Return bfd
            End Get
        End Property
        Public Function IsResistant(Antibiotics As List(Of Antibiotics)) As Boolean
            Dim f As Feature
            Dim rest As New List(Of String)
            For Each ab As Antibiotics In Antibiotics
                rest.Add([Enum].GetName(GetType(Antibiotics), ab))
            Next
            For Each ga As GeneAnnotation In Features
                f = ga.Feature
                If f IsNot Nothing Then
                    For Each bf As FeatureFunction In f.BioFunctions
                        If bf.BioFunction = FeatureFunctionEnum.AntibioticsResistance Then
                            Dim ab As New List(Of String)
                            ab.AddRange(bf.Parameters.ToLower.Split({",", " "}, StringSplitOptions.RemoveEmptyEntries))
                            RemoveFromList(rest, ab)
                            If rest.Count = 0 Then Return True
                        End If
                    Next
                End If
            Next
            Return rest.Count = 0
        End Function

        Public Function GetHash() As String
            If vSequence Is Nothing Then Return ""
            Dim stb As New System.Text.StringBuilder
            stb.Append(vEnd_F)
            stb.Append(vSequence)
            stb.Append(vEnd_R)
            Return vSequence.Length.ToString + "+" + Bytes2Hex(MD5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(stb.ToString)))
        End Function
        Public Function GetShortHash(ByVal length As Integer) As String
            If vSequence Is Nothing Then Return ""
            Dim stb As New System.Text.StringBuilder
            stb.Append(vEnd_F)
            stb.Append(vSequence)
            stb.Append(vEnd_R)
            Dim hash As String = Bytes2Hex(MD5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(stb.ToString)))
            If hash.Length > length Then
                Return vSequence.Length.ToString + "+" + hash.Substring(0, length)
            Else
                Return vSequence.Length.ToString + "+" + hash
            End If
        End Function
        Public Function ToFreeDesignCode() As String 'mathod 
            Dim stb As New System.Text.StringBuilder
            'stb.AppendLine("{\rtf1\ansi\ansicpg936\deff0\deflang1033\deflangfe2052{\fonttbl{\f0\fnil NSimSun;}{\f1\fnil\fcharset134 \'cb\'ce\'cc\'e5;}} ")
            'stb.AppendLine("{\colortbl ;\red0\green0\blue255;\red0\green128\blue0;\red43\green145\blue175;\red163\green21\blue21;}")
            'stb.Append("\viewkind4\uc1\pard\lang2052\f0\fs19 ")

            Dim FIs As New Dictionary(Of Integer, List(Of String))
            Dim l As Integer
            For Each ga As GeneAnnotation In Features
                l = ga.StartPosition * 2 - 2
                If Not FIs.ContainsKey(l) Then FIs.Add(l, New List(Of String))
                FIs(l).Add(String.Format("\*Start: {0}*\", ga.Label))
                l = ga.EndPosition * 2 - 1
                If Not FIs.ContainsKey(l) Then FIs.Add(l, New List(Of String))
                FIs(l).Add(String.Format("\*End: {0}*\", ga.Label))
            Next
            Dim j As Integer = 0
            If Iscircular Then
                stb.Append("::")
                Dim endwithline As Boolean
                Dim iStart As Integer = 0
                Dim iEnd As Integer = vSequence.Length - 1
                For i As Integer = iStart To iEnd
                    If FIs.ContainsKey(i * 2) Then
                        For Each s As String In FIs(i * 2)
                            stb.Append(s)
                        Next
                    End If
                    stb.Append(vSequence, i, 1)
                    j += 1
                    If FIs.ContainsKey(i * 2 + 1) Then
                        For Each s As String In FIs(i * 2 + 1)
                            stb.Append(s)
                        Next
                    End If
                    j += 1
                    If (j Mod 100) = 0 Then
                        endwithline = True
                        stb.AppendFormat("  \*{0}*\", (i + 1).ToString)
                        stb.AppendLine()
                    ElseIf (j Mod 20) = 0 Then
                        stb.Append(" ")
                        endwithline = False
                    Else
                        endwithline = False
                    End If
                Next
                If Not endwithline Then stb.AppendLine()
                stb.AppendLine("::")
            Else
                stb.Append(":")
                stb.Append(vEnd_F)
                stb.AppendLine(":")
                'stb.Append(Sequence.Substring(vEnd_F.Length - 2, Length - vEnd_F.Length - vEnd_R.Length + 4))
                Dim endwithline As Boolean
                Dim iStart As Integer = vEnd_F.Length - 2
                Dim iEnd As Integer = vSequence.Length - (vEnd_R.Length - 2) - 1
                For i As Integer = iStart To iEnd
                    If FIs.ContainsKey(i * 2) Then
                        For Each s As String In FIs(i * 2)
                            stb.Append(s)
                        Next
                    End If
                    stb.Append(vSequence, i, 1)
                    j += 1
                    If FIs.ContainsKey(i * 2 + 1) Then
                        For Each s As String In FIs(i * 2 + 1)
                            stb.Append(s)
                        Next
                    End If
                    j += 1
                    If (j Mod 100) = 0 Then
                        endwithline = True
                        stb.AppendFormat("  \*{0}*\ ", (i + 1).ToString)
                        stb.AppendLine()
                    ElseIf (j Mod 20) = 0 Then
                        stb.Append(" ")
                        endwithline = False
                    Else
                        endwithline = False
                    End If
                Next
                If Not endwithline Then stb.AppendLine()
                stb.Append(":")
                stb.Append(vEnd_R)
                stb.AppendLine(":")
            End If
            'stb.AppendLine("\pard\f1\fs18\par")
            'stb.AppendLine("}")
            Return stb.ToString
        End Function
        Public Function CloneWithoutFeatures() As GeneFile
            Dim gf As New GeneFile
            gf.vDate = Me.vDate
            gf.vEnd_F = Me.vEnd_F
            gf.vEnd_R = Me.vEnd_R
            gf.vIscircular = Me.Iscircular
            gf.vName = Me.vName
            gf.vPhos_F = Me.vPhos_F
            gf.vPhos_R = Me.vPhos_R
            gf.vRCSequence = Me.vRCSequence
            gf.vSequence = Me.vSequence
            Return gf
        End Function
        Public ReadOnly Property Length() As Integer
            Get
                Return vSequence.Length
            End Get
        End Property
        Public ReadOnly Property BoxEnd(ByVal boxlength As Integer) As Integer
            Get
                Return IIf(vIscircular, vSequence.Length - 1, vSequence.Length - boxlength)
            End Get
        End Property
        Public Function Matches(ByVal pattern As String, Optional ByVal start As Integer = 0) As System.Text.RegularExpressions.MatchCollection
            If pattern.Length > vSequence.Length Or pattern.Length = 0 Then
                Dim rgx As New System.Text.RegularExpressions.Regex("NO")
                Return rgx.Matches("YES")
            End If
            If vIscircular Then
                Dim rgx As New System.Text.RegularExpressions.Regex(TAGCFilter(pattern))
                Dim vl As Integer = pattern.Length - 1
                '123451234
                Dim vbr As New System.Text.StringBuilder
                vbr.Append(vSequence)
                vbr.Append(vSequence.Substring(0, vl))
                Dim ser As String = vbr.ToString
                Return rgx.Matches(ser)
            Else
                Dim rgx As New System.Text.RegularExpressions.Regex(TAGCFilter(pattern))
                Return rgx.Matches(vSequence)
            End If
        End Function

        Public Function IsValidIndex(ByVal index As Integer) As Integer
            If index < 0 Then
                Return -1
            ElseIf index > -1 And index < vSequence.Length Then
                Return 0

            ElseIf index >= vSequence.Length Then
                Return 1
            End If
        End Function
        '获得可用于分析的有效双链DNA区间
        Public Function GetDoubleStrandRegion(ByVal length As Integer) As String
            If vEnd_F = "::" And vEnd_R = "::" Then
                Return vSequence + vSequence.Substring(0, length - 1)
            Else
                Dim rpad As Integer
                Dim fpad As Integer
                If vEnd_R.StartsWith("=") Then
                    rpad = 2
                Else
                    rpad = vEnd_R.Length - 2
                End If
                If vEnd_R.StartsWith("=") Then
                    fpad = 2
                Else
                    fpad = vEnd_F.Length - 2
                End If
                Return "".PadLeft(fpad, "-") + vSequence.Substring(fpad, vSequence.Length - fpad - rpad) + "".PadLeft(rpad, "-")
            End If
        End Function

        Public Function GetRCDoubleStrandRegion(ByVal length As Integer) As String
            If vEnd_F = "::" And vEnd_R = "::" Then
                Return vRCSequence + vRCSequence.Substring(0, length - 1)
            Else
                Dim rpad As Integer
                Dim fpad As Integer
                If vEnd_R.StartsWith("=") Then
                    rpad = 2
                Else
                    rpad = vEnd_R.Length - 2
                End If
                If vEnd_R.StartsWith("=") Then
                    fpad = 2
                Else
                    fpad = vEnd_F.Length - 2
                End If
                Return "".PadLeft(rpad, "-") + vRCSequence.Substring(rpad, vRCSequence.Length - rpad - fpad) + "".PadLeft(fpad, "-")
            End If
        End Function

        '通过shared operator来实现连接操作
        Public Shared Operator +(ByVal gf1 As GeneFile, ByVal gf2 As GeneFile) As GeneFile
            If CanLigate(gf1.vEnd_R, gf2.vEnd_F) Then
                Dim gf As New GeneFile
                Dim seq As System.Text.StringBuilder
                seq = New System.Text.StringBuilder
                seq.Append(gf1.vSequence)
                seq.Append(gf2.vSequence.Substring(gf2.vEnd_F.Length - 2))
                gf.vSequence = seq.ToString
                seq = New System.Text.StringBuilder
                seq.Append(gf2.vRCSequence)
                seq.Append(gf1.vRCSequence.Substring(gf1.vEnd_R.Length - 2))
                gf.vRCSequence = seq.ToString
                gf.vEnd_F = gf1.vEnd_F
                gf.vEnd_R = gf2.vEnd_R
                If Not gf.IsCompatible Then Stop
                Return gf
            Else
                Return Nothing
            End If
        End Operator

        Public Shared Operator +(ByVal gf1 As GeneFile) As GeneFile
            If CanLigate(gf1.vEnd_R, gf1.vEnd_F) Then
                Dim gf As New GeneFile
                gf.vSequence = gf1.vSequence.Substring(0, gf1.vSequence.Length - gf1.End_F.Length + 2)
                gf.vRCSequence = gf1.vRCSequence.Substring(gf1.End_F.Length - 2)
                '用::表示环形的
                gf.vIscircular = True
                gf.vEnd_F = "::"
                gf.vEnd_R = "::"
                If Not gf.IsCompatible Then Stop
                Return gf
            Else
                Return Nothing
            End If
        End Operator
        '用-来实现所有的重组效果
        Public Shared Operator -(ByVal gf1 As GeneFile, ByVal gf2 As GeneFile) As GeneFile
            If CanRecombine(gf1.vEnd_R, gf2.vEnd_F) Then
                Dim gf As New GeneFile
                Dim seq As System.Text.StringBuilder
                seq = New System.Text.StringBuilder
                seq.Append(gf1.vSequence)
                seq.Append(gf2.vSequence.Substring(gf2.vEnd_F.Length - 2))
                gf.vSequence = seq.ToString
                seq = New System.Text.StringBuilder
                seq.Append(gf2.vRCSequence)
                seq.Append(gf1.vRCSequence.Substring(gf1.vEnd_R.Length - 2))
                gf.vRCSequence = seq.ToString
                gf.vEnd_F = gf1.vEnd_F
                gf.vEnd_R = gf2.vEnd_R
                If Not gf.IsCompatible Then Stop
                Return gf
            Else
                Return Nothing
            End If
        End Operator

        Public Shared Operator -(ByVal gf1 As GeneFile) As GeneFile
            If CanRecombine(gf1.vEnd_R, gf1.vEnd_F) Then
                Dim gf As New GeneFile
                gf.vSequence = gf1.vSequence.Substring(0, gf1.vSequence.Length - gf1.End_F.Length + 2)
                gf.vRCSequence = gf1.vRCSequence.Substring(gf1.End_F.Length - 2)
                '用::表示环形的
                gf.vIscircular = True
                gf.vEnd_F = "::"
                gf.vEnd_R = "::"
                If Not gf.IsCompatible Then Stop
                Return gf
            Else
                Return Nothing
            End If
        End Operator
        Public ReadOnly Property Clone() As GeneFile
            Get
                Dim gf As New GeneFile
                gf.Name = Name.Clone
                gf.vSequence = vSequence.Clone
                gf.vRCSequence = vRCSequence.Clone
                gf.vEnd_F = vEnd_F.Clone
                gf.vEnd_R = vEnd_R.Clone
                gf.vPhos_F = vPhos_F
                gf.vPhos_R = vPhos_R
                gf.vFeatureCol = New List(Of GeneAnnotation)
                Dim fcol As New List(Of Feature)
                For Each ft As Nuctions.GeneAnnotation In vFeatureCol
                    If ft.Feature IsNot Nothing AndAlso (Not fcol.Contains(ft.Feature)) Then fcol.Add(ft.Feature)
                Next
                Nuctions.AddFeatures(New List(Of GeneFile) From {gf}, fcol)
                For Each ft As Nuctions.GeneAnnotation In vFeatureCol
                    If ft.Feature IsNot Nothing Then ft.Feature = ft.Feature.Clone
                Next
                gf.vIscircular = vIscircular
                gf.vDate = vDate
                Return gf
            End Get
        End Property
        '获得深度反相互补样本

        Public ReadOnly Property RCClone() As GeneFile
            Get
                Dim gf As New GeneFile
                gf.vSequence = vRCSequence.Clone
                gf.vRCSequence = vSequence.Clone
                gf.vEnd_F = vEnd_R.Clone
                gf.vEnd_R = vEnd_F.Clone
                gf.vPhos_F = vPhos_R
                gf.vPhos_R = vPhos_F
                gf.vFeatureCol = New List(Of GeneAnnotation)
                Dim fcol As New List(Of Feature)
                For Each ft As Nuctions.GeneAnnotation In vFeatureCol
                    If ft.Feature IsNot Nothing AndAlso (Not fcol.Contains(ft.Feature)) Then fcol.Add(ft.Feature)
                Next
                Nuctions.AddFeatures(New List(Of GeneFile) From {gf}, fcol)
                For Each ft As Nuctions.GeneAnnotation In vFeatureCol
                    If ft.Feature IsNot Nothing Then ft.Feature = ft.Feature.Clone
                Next
                gf.vIscircular = vIscircular
                gf.vDate = vDate
                Return gf
            End Get
        End Property
        '获得浅层反相互补样本
        Public ReadOnly Property RC() As GeneFile
            Get
                Dim gf As New GeneFile
                gf.vSequence = vRCSequence
                gf.vRCSequence = vSequence
                gf.vEnd_F = vEnd_R
                gf.vEnd_R = vEnd_F
                gf.vPhos_F = vPhos_R
                gf.vPhos_R = vPhos_F
                gf.vFeatureCol = vFeatureCol
                gf.vIscircular = vIscircular
                gf.vDate = vDate
                Return gf
            End Get
        End Property

        '获得比较函数
        Public Shared Operator =(ByVal gf1 As GeneFile, ByVal gf2 As GeneFile) As Boolean
            If gf1.vSequence.Length = gf2.vSequence.Length AndAlso ((gf1.vEnd_F = gf2.vEnd_F And gf1.vEnd_R = gf2.vEnd_R) Or (gf1.vEnd_F = gf2.vEnd_R And gf1.vEnd_R = gf2.vEnd_F)) Then
                Dim seq2 As String = gf1.vSequence + gf1.vSequence
                Return seq2.IndexOf(gf2.vSequence) > -1 Or seq2.IndexOf(gf2.vRCSequence) > -1
            Else
                Return False
            End If
        End Operator
        Public Shared Operator <>(ByVal gf1 As GeneFile, ByVal gf2 As GeneFile) As Boolean
            If gf1.vSequence.Length = gf2.vSequence.Length AndAlso ((gf1.vEnd_F = gf2.vEnd_F And gf1.vEnd_R = gf2.vEnd_R) Or (gf1.vEnd_F = gf2.vEnd_R And gf1.vEnd_R = gf2.vEnd_F)) Then
                Dim seq2 As String = gf1.vSequence + gf1.vSequence
                Return seq2.IndexOf(gf2.vSequence) = -1 And seq2.IndexOf(gf2.vRCSequence) = -1
            Else
                Return True
            End If
        End Operator

        '是否为重组中间产物
        Public ReadOnly Property IsNotRecombinating() As Boolean
            Get
                Return Not (RecombiningRGX.IsMatch(vEnd_F) Or RecombiningRGX.IsMatch(vEnd_R))
                'Return (vEnd_F.StartsWith("*") Or vEnd_F.StartsWith("^") Or vEnd_F.StartsWith(":") Or vEnd_F.StartsWith("0")) And (vEnd_R.StartsWith("*") Or vEnd_R.StartsWith("^") Or vEnd_R.StartsWith(":") Or vEnd_R.StartsWith("0"))
            End Get
        End Property

        Public ReadOnly Property IsCompatible() As Boolean
            Get
                If vSequence.Length = vRCSequence.Length Then
                    Dim ic As Boolean = True

                    For i As Integer = 0 To vSequence.Length - 1
                        ic = ic And (vSequence.Chars(i) = GetCompatibleChar(vRCSequence.Chars(vSequence.Length - 1 - i)))
                    Next
                    Return ic
                Else
                    Return False
                End If


            End Get
        End Property

        Friend Shared Function ContinueMatch(ByVal vGF1 As GeneFile, ByVal start1 As Integer, ByVal forward1 As Boolean,
                                     ByVal vGF2 As GeneFile, ByVal start2 As Integer, ByVal forward2 As Boolean) As Integer
            Dim i As Integer
            Dim max As Integer = Math.Max(vGF1.vSequence.Length, vGF2.vSequence.Length)
            While vGF1.Chars(start1 + i, forward1) = vGF2.Chars(start2 + i, forward2) And start1 + i < vGF1.Length And start2 + i < vGF2.Length
                i += 1
                If i > max Then Return 0
            End While
            Return i
        End Function

        Public Function FindPat(ByVal pLen As Integer, ByVal vSource As String, ByVal ValueOffset As Integer, ByVal vInit As Boolean, ByVal lLimit As Integer,
                                ByVal rLimit As Integer, ByVal lEnd As Integer, ByVal rEnd As Integer, ByVal MatchDic As MatchDictionary,
                                ByVal vTarget As GeneFile, ByVal tFrom As Integer, ByVal tTo As Integer, ByVal IsForward As Boolean, ByVal vBoxLength As Integer,
                                Optional ByVal Flaking As Boolean = False, Optional ByVal lFlak As Integer = 0, Optional ByVal rFlak As Integer = 0) As Integer
            'for the top level call, all the limits shall be 0.

            Dim FBox As String
            Dim mc As System.Text.RegularExpressions.MatchCollection
            Dim Offset As Integer
            'Dim mi As MatchInfo
            Dim lCompare As Integer
            Dim rCompare As Integer
            Dim lMatch As Integer
            Dim rMatch As Integer
            'Dim MatchLength As Integer
            'Dim CA As CompareAnnotation
            Dim vStep As Integer
            Dim MatchCount As Integer = 0


            Dim MatchStart As Integer

            If vInit Then
                tFrom = 0
                tTo = vTarget.Length - 1
            End If
            vStep = Math.Sign(tTo - tFrom) * (pLen \ 2)

            For i As Integer = tFrom To tTo Step vStep
                'forward 
                FBox = vTarget.SubSequence(i, i + pLen - 1)
                If FBox.Length < pLen Then Continue For
                mc = System.Text.RegularExpressions.Regex.Matches(vSource, FBox)
                MatchCount += mc.Count

                For Each m As System.Text.RegularExpressions.Match In mc
                    MatchStart = m.Index + ValueOffset
                    Offset = i - MatchStart
                    If Not MatchDic.Contains(Offset, MatchStart, MatchStart + m.Length) Then

                        lCompare = MatchStart
                        rCompare = MatchStart + m.Length
                        lMatch = i
                        rMatch = i + pLen

                        If vInit Then
                            If Iscircular Then
                                lLimit = rCompare - Length + 1
                                rLimit = lCompare + Length - 1
                            Else
                                lLimit = 0
                                rLimit = Length - 1
                            End If
                        End If
                        If vInit Then
                            If vTarget.Iscircular Then
                                lEnd = rMatch - vTarget.Length + 1
                                rEnd = lMatch + vTarget.Length - 1
                            Else
                                lEnd = 0
                                rEnd = vTarget.Length - 1
                            End If
                        End If
                        If lCompare = rCompare Then Stop
                        ExpandMat(pLen, vSource, lLimit, rLimit, lEnd, rEnd, MatchDic, vTarget, lCompare, rCompare, lMatch, rMatch, IsForward, Flaking, lFlak, rFlak)
                    End If
                Next
            Next
            For Each mInfo In MatchDic.Values
                For Each mItem In mInfo
                    mItem.Source = vTarget
                Next
            Next
            Return MatchCount
        End Function
        Public Sub ExpandMat(ByVal pLen As Integer, ByVal vSource As String, ByVal lLimit As Integer, ByVal rLimit As Integer, ByVal lEnd As Integer, ByVal rEnd As Integer, ByVal MatchDic As MatchDictionary,
                                ByVal vTarget As GeneFile, ByVal lCompare As Integer, ByVal rCompare As Integer, ByVal lMatch As Integer, ByVal rMatch As Integer, ByVal IsForward As Boolean, ByVal vBoxLength As Integer,
                                Optional ByVal Flaking As Boolean = False, Optional ByVal lFlak As Integer = 0, Optional ByVal rFlak As Integer = 0)
            'If Not IsForward Then
            '    If lEnd > 1295 And lEnd < 1300 Then Stop
            '    If rEnd > 1295 And rEnd < 1300 Then Stop
            'End If
            If lCompare = rCompare Then Stop
            '找出本次配对的边界
            'lCompare -= 1
            'lMatch -= 1
            'While lCompare >= lLimit And lMatch >= lEnd And TopoSourceChars(lCompare, IsForward) = vTarget.TopoTargetChars(lMatch, True)
            '    lCompare -= 1
            '    lMatch -= 1
            'End While
            While TopoSourceChars(lCompare, IsForward) = vTarget.TopoTargetChars(lMatch, True)
                lCompare -= 1
                lMatch -= 1
            End While

            'While rCompare <= rLimit And rMatch <= rEnd And TopoSourceChars(rCompare, IsForward) = vTarget.TopoTargetChars(rMatch, True)
            '    rCompare += 1
            '    rMatch += 1
            'End While
            While TopoSourceChars(rCompare, IsForward) = vTarget.TopoTargetChars(rMatch, True)
                rCompare += 1
                rMatch += 1
            End While

            '定义本次配对 添加到配对当中
            If MatchDic.Add(lMatch - lCompare, lCompare + 1, rCompare, lMatch + 1, rMatch) Then


                If rMatch - lMatch >= vBoxLength Then pLen = vBoxLength : Flaking = False


                Dim One As Integer
                'Dim Half As Integer
                Dim OneHalf As Integer

                'If vTarget.SubSequence(rMatch - 10, rMatch) = "TACTGTTGTA" Then Stop

                '继续向左拓展
                ''''先确认左侧是否有足够的空间用于拓展
                One = pLen
                OneHalf = pLen + pLen \ 2
                lCompare += 1
                lMatch += 1

                If Flaking Then
                    While Not (lCompare - lFlak >= OneHalf And lMatch - lEnd >= One)
                        One = One \ 2 + (One Mod 2)
                        OneHalf = One + One \ 2
                        If One = 1 Then Exit While
                    End While
                Else
                    While Not (lCompare - lLimit >= OneHalf And lMatch - lEnd >= One)
                        One = One \ 2 + (One Mod 2)
                        OneHalf = One + One \ 2
                        If One = 1 Then Exit While
                    End While
                    lFlak = lCompare - OneHalf
                    rFlak = lCompare
                End If
                If IsForward Then
                    While One > 1 AndAlso FindPat(One, SubSequence(lCompare - OneHalf, lCompare - 1), lCompare - OneHalf, False,
                                                  lFlak, rFlak, lEnd, lMatch, MatchDic, vTarget, lMatch, lMatch - OneHalf, IsForward,
                                                  True, lFlak, rFlak) = 0
                        'rEnd ==> lMatch
                        One = One \ 2 + (One Mod 2)
                        OneHalf = One + One \ 2
                    End While
                Else
                    While One > 1 AndAlso FindPat(One, SubRCSequence(lCompare - OneHalf, lCompare - 1), lCompare - OneHalf, False,
                                                  lFlak, rFlak, lEnd, lMatch, MatchDic, vTarget, lMatch, lMatch - OneHalf, IsForward,
                                                  True, lFlak, rFlak) = 0
                        'rEnd ==> lMatch
                        One = One \ 2 + (One Mod 2)
                        OneHalf = One + One \ 2
                    End While
                End If


                '继续向右拓展
                ''''先确认右侧是否有足够的空间进行拓展
                One = pLen
                OneHalf = pLen + pLen \ 2


                If Flaking Then
                    While Not (rFlak - rCompare + 1 >= OneHalf And rEnd - rMatch + 1 >= One)
                        One = One \ 2 + (One Mod 2)
                        OneHalf = One + One \ 2
                        If One = 1 Then Exit While
                    End While
                Else
                    While Not (rLimit - rCompare + 1 >= OneHalf And rEnd - rMatch + 1 >= One)
                        One = One \ 2 + (One Mod 2)
                        OneHalf = One + One \ 2
                        If One = 1 Then Exit While
                    End While
                    lFlak = rCompare
                    rFlak = rCompare + OneHalf
                End If
                If IsForward Then
                    While One > 1 AndAlso FindPat(One, SubSequence(rCompare, rCompare + OneHalf - 1), rCompare, False,
                                                  lFlak, rFlak, rMatch, rEnd, MatchDic, vTarget, rMatch, rMatch + OneHalf, IsForward, vBoxLength,
                                                  True, lFlak, rFlak) = 0
                        One = One \ 2 + (One Mod 2)
                        OneHalf = One + One \ 2
                    End While
                Else
                    While One > 1 AndAlso FindPat(One, SubRCSequence(rCompare, rCompare + OneHalf - 1), rCompare, False,
                                                  lFlak, rFlak, rMatch, rEnd, MatchDic, vTarget, rMatch, rMatch + OneHalf, IsForward, vBoxLength,
                                                  True, lFlak, rFlak) = 0
                        One = One \ 2 + (One Mod 2)
                        OneHalf = One + One \ 2
                    End While
                End If
            End If
        End Sub
        Public Property CompareAnnotations As New List(Of CompareAnnotation)
        Public Function ParsePosition(Value As Integer) As Integer
            Dim L As Integer = Sequence.Length
            If Value < 1 Then
                Return Value + (1 + Math.Abs(Value) \ L) * L
            Else
                Return Value - (Math.Abs(Value) \ L) * L
            End If
        End Function
        Public Sub BLAST(ByVal vGF As List(Of GeneFile), Optional ByVal vBoxLength As Integer = 12)
            'the result of this function is the source.

            Dim L As Integer = Length

            Dim FS As String = vSequence + SubSequence(0, vBoxLength - 2)
            Dim RS As String = vRCSequence + SubRCSequence(0, vBoxLength - 2)

            Dim boxlength As Integer = vBoxLength
            Dim minimummatch As Integer = boxlength * 2 + 1

            Dim CA As CompareAnnotation

            'Dim Offset As Integer
            Dim ForwardMatches As New MatchDictionary
            Dim ReverseMatches As New MatchDictionary
            'Dim mc As System.Text.RegularExpressions.MatchCollection
            'Dim mi As MatchInfo
            Dim i As Integer
            Dim j As Integer
            Dim idx As New List(Of Integer)

            Dim ml As MatchItem
            'Dim nd As Integer

            tmpCircular = Iscircular
            CompareAnnotations.Clear()
            For Each gf As GeneFile In vGF
                'prepare
                gf.tmpCircular = gf.Iscircular

                'forward 
                ForwardMatches.Clear()
                FindPat(vBoxLength, FS, 0, True, 0, 0, 0, 0, ForwardMatches, gf, 0, 0, True, vBoxLength)
                'Combine Matches
                ForwardMatches.MergeMatch(vBoxLength)

                'find gaps
                idx.Clear()
                For Each index As Integer In ForwardMatches.Keys
                    idx.Add(index)
                Next
                i = 0
                While i <= idx.Count - 1
                    'find all nearbys
                    j = 0
                    While j <= idx.Count - 1
                        If Math.Abs(idx(j) - idx(i)) <= boxlength Then
                            'find all possible gaps
                            For Each mh As MatchItem In ForwardMatches(idx(i))
                                For Each mk As MatchItem In ForwardMatches(idx(j))
                                    'new one
                                    Nuctions.FindJunctionItem(mh, mk, boxlength)
                                Next
                            Next
                        End If
                        j += 1
                    End While
                    i += 1
                End While
                ForwardMatches.ClearUnattachedItems()

                'Generate Features:
                For Each i In ForwardMatches.Keys
                    For Each mt As MatchItem In ForwardMatches(i)
                        If mt.Valid Then
                            CA = New CompareAnnotation
                            CA.Type = "match"
                            CA.StartPosition = mt.CompareStart + 1
                            CA.EndPosition = mt.CompareEnd
                            CA.Label = String.Format("{0}/{1}-{2} @ {3}", (mt.CompareEnd - mt.CompareStart - 1).ToString, (mt.CompareStart + 1).ToString, mt.CompareEnd.ToString, mt.Source.Name)
                            CA.Note = gf.SubSequence(mt.MatchStart, mt.MatchEnd - 1) ' + " - " + SubSequence(mt.CompareStart, mt.CompareEnd)
                            CA.MatchStart = mt.MatchStart + 1 'mt.MatchStart=1277
                            CA.MatchEnd = mt.MatchEnd 'mt.MatchEnd=1312
                            CA.Offset = (mt.MatchStart - mt.CompareStart) * 2
                            CompareAnnotations.Add(CA)
                            Features.Add(CA)
                            'If CA.MatchStart = 1314 Then Stop 'debug
                            If (Not (mt.Next Is Nothing)) AndAlso mt.Next.Valid Then
                                CA = New CompareAnnotation
                                CA.Type = "miss"
                                ml = mt.Next
                                '这个是gap的计算位置
                                CA.MatchStart = mt.MatchEnd + 1
                                CA.MatchEnd = ml.MatchStart
                                CA.Note = GetGapChar(mt.CompareEnd + 1, ml.CompareStart, mt.MatchEnd + 1, ml.MatchStart, gf, CA.StartPosition, CA.EndPosition)
                                CA.Label = String.Format("{0}/{1}-{2} @ {3}", (ml.CompareStart - mt.CompareEnd).ToString, (mt.CompareEnd + 1).ToString, ml.CompareStart.ToString, mt.Source.Name)
                                ' + " - " + SubSequence(mt.CompareEnd, ml.CompareStart)
                                CompareAnnotations.Add(CA)
                                Features.Add(CA)
                            End If
                        End If
                    Next
                Next

                'reverse
                ReverseMatches.Clear()
                FindPat(vBoxLength, RS, 0, True, 0, 0, 0, 0, ReverseMatches, gf, 0, 0, False, vBoxLength)
                ReverseMatches.MergeMatch(vBoxLength)

                'find gaps
                idx.Clear()
                For Each index As Integer In ReverseMatches.Keys
                    idx.Add(index)
                Next
                i = 0
                While i <= idx.Count - 1
                    'find all nearbys
                    j = 0
                    While j <= idx.Count - 1
                        If Math.Abs(idx(j) - idx(i)) <= boxlength Then
                            'find all possible gaps
                            For Each mh As MatchItem In ReverseMatches(idx(i))
                                For Each mk As MatchItem In ReverseMatches(idx(j))
                                    Nuctions.FindJunctionItem(mh, mk, boxlength)
                                Next
                            Next
                        End If
                        j += 1
                    End While
                    i += 1
                End While
                ReverseMatches.ClearUnattachedItems()
                'Generate Features:
                For Each i In ReverseMatches.Keys
                    For Each mt As MatchItem In ReverseMatches(i)
                        If mt.Valid Then
                            CA = New CompareAnnotation
                            CA.Type = "match"
                            CA.Complement = True
                            CA.StartPosition = L - mt.CompareEnd + 1
                            CA.EndPosition = L - mt.CompareStart
                            ''''' + 1
                            CA.Label = String.Format("{0}/{1}-{2} @ {3}", (mt.CompareEnd - mt.CompareStart).ToString, (L - mt.CompareStart).ToString, (L - mt.CompareEnd + 1).ToString, mt.Source.Name)
                            CA.Note = gf.SubSequence(mt.MatchStart, mt.MatchEnd - 1) ' + " - " + SubRCSequence(mt.CompareStart, mt.CompareEnd)
                            '这个东西是作为Feature保存在Genefile里面的 必须忠实反映在.gb文件当中的数值
                            CA.MatchEnd = mt.MatchEnd
                            CA.MatchStart = mt.MatchStart + 1
                            CA.Offset = (mt.MatchStart - mt.CompareStart) * 2
                            CompareAnnotations.Add(CA)
                            Features.Add(CA)
                            If (Not (mt.Next Is Nothing)) AndAlso mt.Next.Valid Then
                                CA = New CompareAnnotation
                                CA.Complement = True
                                CA.Type = "miss"
                                ml = mt.Next
                                '这个是gap的计算位置
                                CA.MatchStart = mt.MatchEnd + 1
                                CA.MatchEnd = ml.MatchStart
                                If CA.MatchStart = 680 And CA.MatchEnd = 680 Then Stop
                                CA.Note = GetGapChar(L - ml.CompareStart, L - (mt.CompareEnd + 1), mt.MatchEnd + 1, ml.MatchStart, gf, CA.StartPosition, CA.EndPosition)
                                CA.Label = String.Format("{0}/{1}-{2} @ {3}", (ml.CompareStart - mt.CompareEnd).ToString, (L - mt.CompareEnd).ToString, (L - ml.CompareStart + 1).ToString, mt.Source.Name)
                                ' + " - " + SubRCSequence(mt.CompareEnd, ml.CompareStart)
                                Features.Add(CA)
                                CompareAnnotations.Add(CA)
                            End If
                        End If
                    Next
                Next
            Next
        End Sub
        Public Function BLAST2(ByVal vGF As List(Of GeneFile), Optional ByVal vBoxLength As Integer = 12) As List(Of String)

            Dim L As Integer = Length

            Dim FS As String = vSequence + SubSequence(0, vBoxLength - 2)
            Dim RS As String = vRCSequence + SubRCSequence(0, vBoxLength - 2)

            Dim boxlength As Integer = vBoxLength
            Dim minimummatch As Integer = boxlength * 2 + 1

            'Dim Offset As Integer
            Dim ForwardMatches As New MatchDictionary
            Dim ReverseMatches As New MatchDictionary
            'Dim mc As System.Text.RegularExpressions.MatchCollection
            'Dim mi As MatchInfo

            Dim idx As New List(Of Integer)


            tmpCircular = Iscircular

            Dim res As New List(Of String)

            For Each gf As GeneFile In vGF
                'prepare
                gf.tmpCircular = gf.Iscircular
                'forward 
                ForwardMatches.Clear()
                FindPat(vBoxLength, FS, 0, True, 0, 0, 0, 0, ForwardMatches, gf, 0, 0, True, vBoxLength)
                'Combine Matches
                ForwardMatches.MergeMatch(vBoxLength)
                For Each mi As MatchInfo In ForwardMatches.Values
                    For Each mt As MatchItem In mi
                        res.Add(gf.SubSequence(mt.MatchStart, mt.MatchEnd - 1))
                    Next
                Next
                'reverse
                ReverseMatches.Clear()
                FindPat(vBoxLength, RS, 0, True, 0, 0, 0, 0, ReverseMatches, gf, 0, 0, False, vBoxLength)
                ReverseMatches.MergeMatch(vBoxLength)
                For Each mi As MatchInfo In ReverseMatches.Values
                    For Each mt As MatchItem In mi
                        res.Add(gf.SubSequence(mt.MatchStart, mt.MatchEnd - 1))
                    Next
                Next
            Next
            Return res
        End Function
        Public Function BLAST3(ByVal Primer As String, Optional ByVal vBoxLength As Integer = 12) As List(Of String)

            Dim L As Integer = Length

            Dim FS As String = vSequence + SubSequence(0, vBoxLength - 2)
            Dim RS As String = vRCSequence + SubRCSequence(0, vBoxLength - 2)

            Dim boxlength As Integer = vBoxLength
            Dim minimummatch As Integer = boxlength * 2 + 1

            'Dim Offset As Integer
            Dim ForwardMatches As New MatchDictionary
            Dim ReverseMatches As New MatchDictionary
            'Dim mc As System.Text.RegularExpressions.MatchCollection
            'Dim mi As MatchInfo

            Dim idx As New List(Of Integer)


            tmpCircular = Iscircular

            Dim res As New List(Of String)

            Dim gf As New GeneFile With {.Sequence = Primer, .End_F = "*B", .End_R = "*B"}
            'prepare
            gf.tmpCircular = gf.Iscircular
            'forward 
            ForwardMatches.Clear()
            FindPat(vBoxLength, FS, 0, True, 0, 0, 0, 0, ForwardMatches, gf, 0, 0, True, vBoxLength)
            'Combine Matches
            ForwardMatches.MergeMatch(vBoxLength)
            For Each mi As MatchInfo In ForwardMatches.Values
                For Each mt As MatchItem In mi
                    res.Add(gf.SubSequence(mt.MatchStart, mt.MatchEnd - 1))
                Next
            Next
            Return res
        End Function


        Public ReadOnly Property Chars(ByVal index As Integer, ByVal forward As Boolean) As Char
            Get
                index = index Mod vSequence.Length
                If index < 0 Then index += vSequence.Length
                If forward Then
                    Return vSequence.Chars(index)
                Else
                    Return vRCSequence.Chars(index)
                End If
            End Get
        End Property
        Private Shared SourceChar As Char = "-"
        Private tmpCircular As Boolean
        Public ReadOnly Property TopoSourceChars(ByVal index As Integer, ByVal forward As Boolean) As Char
            Get
                If tmpCircular Then
                    index = index Mod vSequence.Length
                    If index < 0 Then index += vSequence.Length
                    If forward Then
                        Return vSequence.Chars(index)
                    Else
                        Return vRCSequence.Chars(index)
                    End If
                Else
                    If index < 0 Or index >= vSequence.Length Then
                        Return SourceChar
                    Else
                        If forward Then
                            Return vSequence.Chars(index)
                        Else
                            Return vRCSequence.Chars(index)
                        End If
                    End If
                End If
            End Get
        End Property
        Private Shared TargetChar As Char = "~"
        Public ReadOnly Property TopoTargetChars(ByVal index As Integer, ByVal forward As Boolean) As Char
            Get
                If tmpCircular Then
                    index = index Mod vSequence.Length
                    If index < 0 Then index += vSequence.Length
                    If forward Then
                        Return vSequence.Chars(index)
                    Else
                        Return vRCSequence.Chars(index)
                    End If
                Else
                    If index < 0 Or index >= vSequence.Length Then
                        Return TargetChar
                    Else
                        If forward Then
                            Return vSequence.Chars(index)
                        Else
                            Return vRCSequence.Chars(index)
                        End If
                    End If
                End If
            End Get
        End Property

        Public Function SubSequence(ByVal start As Integer, ByVal [end] As Integer) As String
            [end] = [end] Mod vSequence.Length
            start = start Mod vSequence.Length
            If start < 0 Then start += vSequence.Length
            If [end] < 0 Then [end] += vSequence.Length
            If vEnd_F = "::" And vEnd_R = "::" Then
                If [end] > start Then
                    Return vSequence.Substring(start, [end] - start + 1)
                Else
                    Dim vbr As New System.Text.StringBuilder
                    vbr.Append(vSequence.Substring(start))
                    vbr.Append(vSequence.Substring(0, [end] + 1))
                    Return vbr.ToString
                End If
            Else
                If [end] >= start Then
                    Return vSequence.Substring(start, [end] - start + 1)
                Else
                    Return ""
                End If
            End If
        End Function

        Public Function SubRCSequence(ByVal start As Integer, ByVal [end] As Integer) As String
            [end] = [end] Mod vSequence.Length
            start = start Mod vSequence.Length
            If start < 0 Then start += vSequence.Length
            If [end] < 0 Then [end] += vSequence.Length
            If vEnd_F = "::" And vEnd_R = "::" Then
                If [end] > start Then
                    Return vRCSequence.Substring(start, [end] - start + 1)
                Else
                    Dim vbr As New System.Text.StringBuilder
                    vbr.Append(vRCSequence.Substring(start))
                    vbr.Append(vRCSequence.Substring(0, [end] + 1))
                    Return vbr.ToString
                End If
            Else
                If [end] > start Then
                    Return vRCSequence.Substring(start, [end] - start + 1)
                Else
                    Return ""
                End If
            End If
        End Function

        Public Function GetSubGeneFile(ByVal start As Integer, ByVal [end] As Integer) As GeneFile
            Dim gf As New GeneFile
            gf.vSequence = SubSequence(start, [end])
            gf.vRCSequence = SubRCSequence(vSequence.Length - [end], vSequence.Length - start)
            gf.vEnd_F = "*B"
            gf.vEnd_R = "*B"
            Return gf
        End Function

        Public Property Name() As String
            Get
                Return vName
            End Get
            Set(ByVal value As String)
                vName = value
            End Set
        End Property
        Public ReadOnly Property IsClosedVector() As Boolean
            Get
                Return (vEnd_F.StartsWith("::") And vEnd_R.StartsWith("::")) OrElse (vEnd_F.StartsWith("0B") And vEnd_R.StartsWith("0B"))
            End Get
        End Property

        Public ReadOnly Property Iscircular() As Boolean
            Get
                Return vEnd_F.StartsWith("::") And vEnd_R.StartsWith("::")
            End Get
            'Set(ByVal value As Boolean)
            '    vIscircular = value
            'End Set
        End Property
        Public Property ModificationDate() As Date
            Get
                Return vDate
            End Get
            Set(ByVal value As Date)
                vDate = value
            End Set
        End Property
        Public Property Sequence() As String
            Get
                Return vSequence
            End Get
            Set(ByVal value As String)
                vSequence = value
                vRCSequence = Nuctions.ReverseComplementN(value)
            End Set
        End Property
        Public ReadOnly Property RCSequence() As String
            Get
                Return vRCSequence
            End Get
        End Property
        Public Property Features() As List(Of GeneAnnotation)
            Get
                Return vFeatureCol
            End Get
            Set(ByVal value As List(Of GeneAnnotation))
                vFeatureCol = value
            End Set
        End Property
        Public Property End_F() As String
            Get
                Return vEnd_F
            End Get
            Set(ByVal value As String)
                vEnd_F = value
            End Set
        End Property
        Public Property End_R() As String
            Get
                Return vEnd_R
            End Get
            Set(ByVal value As String)
                vEnd_R = value
            End Set
        End Property
        Public Property Phos_F() As Boolean
            Get
                Return Me.vPhos_F
            End Get
            Set(ByVal value As Boolean)
                Me.vPhos_F = value
            End Set
        End Property
        Public Property Phos_R() As Boolean
            Get
                Return Me.vPhos_R
            End Get
            Set(ByVal value As Boolean)
                Me.vPhos_R = value
            End Set
        End Property
        'Define of the annotation [1>GGATCA<1]GGATGACCGAGTAT<2]GAGTACACGATAGACA[2>GAGTCAGTAC

        Public Function WriteToFile(ByVal filename As String) As Boolean
            Dim codes As String = ""
            'write header
            If Me.vName Is Nothing Then vName = "NA"
            codes &= "LOCUS       " + Me.vName.Replace(" ", "\").PadRight(16) + Me.vSequence.Length.ToString.PadLeft(12) + " bp    DNA     " + IIf(Me.Iscircular, "circular", "linear").ToString.PadRight(13) + Me.vDate.ToString("dd-MMM-yyyy") + ControlChars.NewLine
            codes &= "FEATURES             Location/Qualifiers" + ControlChars.NewLine
            Dim f As GeneAnnotation
            For Each f In Me.vFeatureCol
                codes &= "     " + f.Type.PadRight(16) + IIf(f.Complement, "complement(" + f.StartPosition.ToString + ".." + f.EndPosition.ToString + ")", f.StartPosition.ToString + ".." + f.EndPosition.ToString) + ControlChars.NewLine
                codes &= "                     /label=" + IIf(f.Label Is Nothing OrElse f.Label.Length = 0, f.Type, f.Label) + ControlChars.NewLine
                codes &= "                     /note=" + IIf(f.Note Is Nothing OrElse f.Note.Length = 0, f.Type, f.Note) + ControlChars.NewLine
            Next
            Dim sc As New BaseCount(Me.vSequence)
            codes &= "BASE COUNT" + sc.A_Count.ToString.PadLeft(9) + " a " + sc.C_Count.ToString.PadLeft(9) + " c " + sc.G_Count.ToString.PadLeft(9) + " g " + sc.T_Count.ToString.PadLeft(9) + " t " + ControlChars.NewLine
            codes &= "ORIGIN" + ControlChars.NewLine
            Dim i As Integer, ii As Integer
            For i = 0 To Me.vSequence.Length - 1 Step 60
                codes &= (i + 1).ToString.PadLeft(9) + " "
                For ii = 0 To 59 Step 10
                    If (ii + i + 10) > Me.vSequence.Length Then
                        codes &= Me.vSequence.Substring(i + ii, Me.vSequence.Length - i - ii) + " "
                        Exit For
                    Else
                        codes &= Me.vSequence.Substring(i + ii, 10) + " "
                    End If
                Next
                codes &= ControlChars.NewLine
            Next
            codes &= "//"
            If IO.File.Exists(filename) Then IO.File.Delete(filename)
            Dim gfile As New IO.FileStream(filename, IO.FileMode.OpenOrCreate)
            Dim gfilew As New IO.StreamWriter(gfile)
            gfilew.Write(codes)
            gfilew.Close()
            gfile.Close()
        End Function

        Public Function WriteIndexFile(ByVal filename As String) As Boolean
            Dim fi As New IO.FileStream(filename, IO.FileMode.OpenOrCreate)
            Dim pos As Integer = 0
            fi.Position = pos

            fi.Write(BitConverter.GetBytes(fi.Length), 0, 4)
            pos += 4
            fi.Position = pos

            Dim btCir As Byte() = New Byte() {IIf(Iscircular, 0, 1)}
            pos += 1
            fi.Position = pos

            fi.Write(btCir, 0, 1)
            Dim data As Byte()
            If Iscircular Then
                Dim stb As System.Text.StringBuilder
                stb = New System.Text.StringBuilder
                stb.Append(vSequence)
                stb.Append(vSequence)
                data = Nuctions.ParseGeneCharToByte(stb.ToString)
                fi.Write(data, 0, data.Length)
                pos += data.Length
                fi.Position = pos
                stb = New System.Text.StringBuilder
                stb.Append(vRCSequence)
                stb.Append(vRCSequence)
                data = Nuctions.ParseGeneCharToByte(stb.ToString)
                fi.Write(data, 0, data.Length)
                pos += data.Length
                fi.Position = pos
            Else
                Dim stb As System.Text.StringBuilder
                stb = New System.Text.StringBuilder
                stb.Append(vSequence)
                data = Nuctions.ParseGeneCharToByte(stb.ToString)
                fi.Write(data, 0, data.Length)
                stb = New System.Text.StringBuilder
                stb.Append(vRCSequence)
                data = Nuctions.ParseGeneCharToByte(stb.ToString)
                fi.Write(data, 0, data.Length)
            End If
            fi.Close()
        End Function
        Friend Shared Function SearchIndexFile(ByVal seqbytes As Byte(), ByVal filename As String) As Integer
            Dim fi As New IO.FileStream(filename, IO.FileMode.Open)
            Dim pos As Integer = 0
            fi.Position = pos
            Dim intBytes(3) As Byte

            fi.Read(intBytes, 0, 4)
            pos += 4
            fi.Position = pos
            Dim l As Integer = BitConverter.ToInt32(intBytes, 0)

            Dim btCir(0) As Byte
            fi.Read(btCir, 0, 1)
            pos += 1
            fi.Position = pos

            Dim lv As Integer
            If btCir(0) = 0 Then
                lv = l * 2
            Else
                lv = l
            End If

            Dim index As Integer
            Dim data As Byte() = New Byte(lv - 1) {}

            fi.Read(data, 0, lv)
            pos += lv
            fi.Position = pos

            index = Array.IndexOf(data, seqbytes)
            If index > -1 Then Return index + 1

            fi.Read(data, 0, lv)
            pos += lv
            fi.Position = pos

            index = Array.IndexOf(data, seqbytes)
            If index > -1 Then Return -index - 1

            fi.Close()
            Return 0
        End Function
    End Class
    Public Class BaseCount
        Dim a As Integer = 0, t As Integer = 0, g As Integer = 0, c As Integer = 0
        Public Sub New(ByVal sequence As String)
            sequence = sequence.ToUpper
            Dim i As Integer
            For i = 0 To sequence.Length - 1
                Select Case sequence(i)
                    Case "A"
                        a += 1
                    Case "C"
                        c += 1
                    Case "G"
                        g += 1
                    Case "T"
                        t += 1
                End Select
            Next
        End Sub
        Public ReadOnly Property A_Count() As Integer
            Get
                Return a
            End Get
        End Property
        Public ReadOnly Property T_Count() As Integer
            Get
                Return t
            End Get
        End Property
        Public ReadOnly Property C_Count() As Integer
            Get
                Return c
            End Get
        End Property
        Public ReadOnly Property G_Count() As Integer
            Get
                Return g
            End Get
        End Property
    End Class

    Public Class GeneAnnotation
        Public Sub New()

        End Sub
        Public Sub New(ByVal nIndex As Integer, ByVal oGeneAnnotation As Feature, ByVal nStartPosition As Integer, ByVal nEndPosition As Integer, ByVal nComplement As Boolean)
            Index = nIndex
            If oGeneAnnotation IsNot Nothing Then
                Type = oGeneAnnotation.Type
                Note = oGeneAnnotation.Note
                Label = oGeneAnnotation.Label
            End If
            StartPosition = nStartPosition
            EndPosition = nEndPosition
            Complement = nComplement
        End Sub
        Public Sub New(ByVal vIndex As Integer, ByVal vType As String, ByVal vNote As String, ByVal vLabel As String, ByVal vStartPosition As Integer, ByVal vEndPosition As Integer, ByVal vComplement As Boolean, ByVal vVector As GeneFile)
            Index = vIndex
            Type = vType
            Note = vNote
            Label = vLabel
            StartPosition = vStartPosition
            EndPosition = vEndPosition
            Complement = vComplement
            Vector = vVector
        End Sub
        Public Function GetSuqence() As String
            Dim vSeq As String
            If StartPosition > EndPosition Then
                vSeq = Vector.Sequence.Substring(StartPosition - 1, Vector.Sequence.Length - StartPosition + 1) & Vector.Sequence.Substring(0, EndPosition)
                Return IIf(Complement, Nuctions.ReverseComplement(vSeq), vSeq)
            ElseIf StartPosition < EndPosition Then
                vSeq = Vector.Sequence.Substring(StartPosition - 1, EndPosition - StartPosition + 1)
                Return IIf(Complement, Nuctions.ReverseComplement(vSeq), vSeq)
            Else
                Return ""
            End If
        End Function
        Public Index As Integer
        Public Type As String = ""
        Public Note As String = ""
        Public Label As String = ""
        Public StartPosition As Integer
        Public EndPosition As Integer
        Public Complement As Boolean
        Public Vector As GeneFile
        Public Feature As Feature
        Public Function Clone(ByVal nVector As GeneFile) As GeneAnnotation
            Dim ga As New GeneAnnotation(Index, Type, Note, Label, StartPosition, EndPosition, Complement, nVector)
            If Feature IsNot Nothing Then ga.Feature = Feature.Clone
            Return ga
        End Function
    End Class

    Public Class TranslationAnnotation
        Inherits GeneAnnotation
    End Class

    Public Class CompareAnnotation
        Inherits GeneAnnotation
        Public MatchStart As Integer
        Public MatchEnd As Integer
        Public Offset As Integer
    End Class



    Friend Shared Function ShowGBFile(ByVal vFilename As String) As Boolean
        'By default, the program is vector NTI. It depends what program you have installed.
        Dim open As Boolean = True
        Try
            Dim p As New System.Diagnostics.ProcessStartInfo()
            p.Verb = "open"
            p.FileName = vFilename
            p.WindowStyle = ProcessWindowStyle.Maximized
            p.UseShellExecute = True
            System.Diagnostics.Process.Start(p)
        Catch e As Exception
            open = False
        End Try
        Return open
    End Function

    Friend Shared Function GetMolecularOperationDescription(var As MolecularOperationEnum) As String
        Select Case var
            Case MolecularOperationEnum.Vector
                Return "Vector"
            Case MolecularOperationEnum.Enzyme
                Return "Enzyme"
            Case MolecularOperationEnum.PCR
                Return "PCR"
            Case MolecularOperationEnum.Modify
                Return "Modification"
            Case MolecularOperationEnum.Gel
                Return "Gel Electrophoresis"
            Case MolecularOperationEnum.Ligation
                Return "Ligation"
            Case MolecularOperationEnum.Screen
                Return "Screening"
            Case MolecularOperationEnum.Recombination
                Return "Recombination"
            Case MolecularOperationEnum.EnzymeAnalysis
                Return "Enzyme Analysis"
            Case MolecularOperationEnum.Merge
                Return "Merge"
            Case MolecularOperationEnum.FreeDesign
                Return "Sequence Design"
            Case MolecularOperationEnum.HashPicker
                Return "Hash Picker"
            Case MolecularOperationEnum.SequencingResult
                Return "Sequencing Result"
            Case MolecularOperationEnum.Compare
                Return "Compare"
            Case MolecularOperationEnum.Host
                Return "Host"
            Case MolecularOperationEnum.Transformation
                Return "Transformation"
            Case MolecularOperationEnum.Incubation
                Return "Incubation"
            Case MolecularOperationEnum.Extraction
                Return "Extraction"
            Case MolecularOperationEnum.Expression
                Return "Expression"
            Case MolecularOperationEnum.GibsonDesign
                Return "Gibson Design"
            Case MolecularOperationEnum.CRISPRCut
                Return "CRISPR Cut"
        End Select
    End Function
    Public Enum MolecularOperationEnum
        <Description("Vector")> Vector
        <Description("Enzyme")> Enzyme
        <Description("PCR")> PCR
        <Description("Modification")> Modify
        <Description("Gel Electrophoresis")> Gel
        <Description("Ligation")> Ligation
        <Description("Screening")> Screen
        <Description("Recombination")> Recombination
        <Description("Enzyme Analysis")> EnzymeAnalysis
        <Description("Merge")> Merge
        <Description("Sequence Design")> FreeDesign
        <Description("HashPicker")> HashPicker
        <Description("Sequencing Result")> SequencingResult 'SequencingResults are not .gb files. they need one more factor, the sequencing primer.
        <Description("Comparison")> Compare 'This is to compare sequencing results and its original vector. It could be anything.
        <Description("Host")> Host
        <Description("Transformation")> Transformation 'One In vitro into one host
        <Description("Incubation")> Incubation 'bacteria tATAGATGATAGACGAGATCAgTTGACCAGATCCAGATCAGATCAGATACAGATCAGATCGAATGACAGAGATCCGAAGTCAGCTATGCAGATCACAGTAAAATAGACAGTAAC
        <Description("Extraction")> Extraction '
        <Description("Expression")> Expression
        <Description("Gibson Design")> GibsonDesign
        <Description("CRISPR Cut")> CRISPRCut
        <Description("Oligo Annealing")> OligoAnnealing
    End Enum
    Public Enum Antibiotics As Integer
        amp
        kan
        chl
        tet
        gen
        neo
        spe
        str
        ery
        rif
    End Enum
    Public Enum TransformationMethod As Integer
        Electroporation
        Conjugation
        ChemicalTransformation
    End Enum
    Public Enum TransformationMode As Integer
        AllToOneCell
        EachPerCell
        Combinational
    End Enum
    Friend Shared Function ParseAntibiotics(ByVal vList As List(Of String)) As List(Of Antibiotics)
        Dim atbs As New List(Of Antibiotics)
        Dim atb As Antibiotics
        For Each value As String In vList
            Dim strings As String() = value.ToLower.Split({" "}, StringSplitOptions.RemoveEmptyEntries)
            For Each s As String In strings
                If [Enum].TryParse(s, atb) Then
                    If Not atbs.Contains(atb) Then atbs.Add(atb)
                Else

                End If
            Next
        Next
        Return atbs
    End Function
    Friend Shared Function ParseAntibiotics(ByVal Value As String) As List(Of Antibiotics)
        Dim strings As String() = Value.ToLower.Split({" "}, StringSplitOptions.RemoveEmptyEntries)
        Dim atbs As New List(Of Antibiotics)
        Dim atb As Antibiotics
        For Each s As String In strings
            If [Enum].TryParse(s, atb) Then
                If Not atbs.Contains(atb) Then atbs.Add(atb)
            Else

            End If
        Next
        Return atbs
    End Function
    Friend Shared Function DescribeAntibiotics(ByVal Value As List(Of Antibiotics), lang As Language) As String
        Dim stb As New System.Text.StringBuilder
        Dim t As Type = GetType(Antibiotics)
        Dim i As Integer
        Select Case lang
            Case Language.English
                For Each ab As Antibiotics In Value
                    i += 1
                    stb.Append(LangAntibiotics(ab, lang))
                    If i < Value.Count - 1 Then
                        stb.Append(", ")
                    ElseIf i = Value.Count - 1 Then
                        stb.Append(" and ")
                    ElseIf i = Value.Count Then
                        'stb.Append("，")
                    End If
                Next
            Case Language.Chinese
                For Each ab As Antibiotics In Value
                    i += 1
                    stb.Append(LangAntibiotics(ab, lang))
                    If i < Value.Count - 1 Then
                        stb.Append("、")
                    ElseIf i = Value.Count - 1 Then
                        stb.Append("和")
                    ElseIf i = Value.Count Then
                        'stb.Append("，")
                    End If
                Next
            Case Language.Japanese

        End Select

        Return stb.ToString
    End Function
    Friend Shared Function ExpressAntibiotics(ByVal Value As List(Of Antibiotics)) As String
        Dim stb As New System.Text.StringBuilder
        Dim t As Type = GetType(Antibiotics)
        For Each ab As Antibiotics In Value
            stb.Append([Enum].GetName(t, ab))
            stb.Append(" ")
        Next
        Return stb.ToString
    End Function
    Friend Shared Function LangAntibiotics(anti As Antibiotics, lang As Language) As String
        Static Translator As New List(Of List(Of String))
        If Translator.Count = 0 Then
            Translator.Add(New List(Of String) From {"Ampicilin", "氨苄青霉素", "アンピシリン"})
            Translator.Add(New List(Of String) From {"Kanamycin", "卡那霉素", "アンピシリン"})
            Translator.Add(New List(Of String) From {"Chloraphenicol", "氯霉素", "アンピシリン"})
            Translator.Add(New List(Of String) From {"Tetracycline", "四环素", "アンピシリン"})
            Translator.Add(New List(Of String) From {"Gentamycin", "庆大霉素", "アンピシリン"})
            Translator.Add(New List(Of String) From {"Neomycin", "新霉素", "アンピシリン"})
            Translator.Add(New List(Of String) From {"Spectinomycin", "壮观霉素", "アンピシリン"})
            Translator.Add(New List(Of String) From {"Streptomycin", "链霉素", "アンピシリン"})
            Translator.Add(New List(Of String) From {"Erythromycin", "红霉素", "アンピシリン"})
            Translator.Add(New List(Of String) From {"Rifampicin", "利福平", "リファンピシン"})
        End If
        Return Translator(anti)(lang)
    End Function

    'Public Shared Sub CollectionCombineIn(ByVal inCol As Collection, ByVal cbCol As ICollection)
    '    Dim obj As Object
    '    For Each obj In cbCol
    '        inCol.Add(obj)
    '    Next
    'End Sub
    <Serializable()>
    Public Class Feature
        Implements System.ComponentModel.INotifyPropertyChanged

        Private nSequence As String
        Private nRCSequence As String
        Private nName As String
        Private nLabel As String
        Private nNote As String
        Private nType As String
        Private nUseful As String
        Public BioFunctions As New List(Of FeatureFunction)
        <NonSerialized> Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Public Overrides Function ToString() As String
            Return nLabel
        End Function
        Public Sub New()
        End Sub
        Public Sub New(ByVal vName As String, ByVal vSequence As String, ByVal vLabel As String, ByVal vType As String, ByVal vNote As String, Optional ByVal vUseful As String = "Native")
            nName = vName
            nSequence = vSequence
            nRCSequence = Nuctions.ReverseComplement(vSequence)
            nLabel = vLabel
            nType = vType
            nNote = vNote
            nUseful = vUseful
        End Sub
        Public Function Clone() As Feature
            Dim ft As New Feature
            With ft
                .nName = nName
                .nSequence = nSequence
                .nRCSequence = nRCSequence
                .nLabel = nLabel
                .nType = nType
                .nNote = nNote
                .nUseful = nUseful
            End With
            For Each ff As FeatureFunction In BioFunctions
                ft.BioFunctions.Add(ff.Clone)
            Next
            Return ft
        End Function
        Public Shared Operator =(ByVal f1 As Feature, ByVal f2 As Feature) As Boolean
            If f1 Is f2 Then
                Return True
            Else
                Return ((f1.nSequence = f2.nSequence And f1.nRCSequence = f2.nRCSequence) Or (f1.nSequence = f2.nRCSequence And f1.nRCSequence = f2.nSequence))
            End If
        End Operator

        Public Shared Operator <>(ByVal f1 As Feature, ByVal f2 As Feature) As Boolean
            Return Not (f1 = f2)
        End Operator
        Public Property Useful() As String
            Get
                Return nUseful
            End Get
            Set(ByVal value As String)
                nUseful = value
            End Set
        End Property

        Public ReadOnly Property RCSequence() As String
            Get
                Return nRCSequence
            End Get
        End Property
        Public ReadOnly Property Sequence() As String
            Get
                Return nSequence
            End Get
        End Property
        Public ReadOnly Property Label() As String
            Get
                Return nLabel
            End Get
        End Property
        Public ReadOnly Property Name() As String
            Get
                Return nName
            End Get
        End Property
        Public ReadOnly Property Note() As String
            Get
                Return nNote
            End Get
        End Property
        Public ReadOnly Property Type() As String
            Get
                Return nType
            End Get
        End Property
    End Class
    Friend Shared Function Translate(ByVal DNA As String) As String
        Dim seq As String = DNA
        seq = Nuctions.TAGCFilter(Nuctions.TAGCFilter(seq))
        Dim tsl As New System.Text.StringBuilder
        Dim i As Integer
        Dim CodonCol As Translation = Translation.GetDefault
        If CodonCol Is Nothing Then Return ""
        For i = 3 To seq.Length Step 3
            tsl.Append(CodonCol.CodeTable(seq.Substring(i - 3, 3)).ShortName)
        Next
        Return tsl.ToString
    End Function
    Friend Shared Function GetAnimoAcidBasedMatch(ByVal DNA As String) As System.Text.RegularExpressions.Regex

        Dim pr As String = Translate(DNA)

        Dim stb As New System.Text.StringBuilder
        For i As Integer = 0 To pr.Length - 1
            stb.Append("(")
            For j As Integer = 0 To Translation.GetDefault.AnimoTable(pr.Substring(i, 1)).CodeList.Count - 1
                stb.Append(Translation.GetDefault.AnimoTable(pr.Substring(i, 1)).CodeList(j).Name)
                If j < Translation.GetDefault.AnimoTable(pr.Substring(i, 1)).CodeList.Count - 1 Then stb.Append("|")
            Next
            stb.Append(")")
        Next
        Return New System.Text.RegularExpressions.Regex(stb.ToString)
    End Function

    Friend Shared Function GetReverseAnimoAcidBasedMatch(ByVal DNA As String) As System.Text.RegularExpressions.Regex

        Dim pr As String = Translate(DNA)

        Dim stb As New System.Text.StringBuilder
        For i As Integer = pr.Length - 1 To 0 Step -1
            stb.Append("(")
            For j As Integer = 0 To Translation.GetDefault.AnimoTable(pr.Substring(i, 1)).CodeList.Count - 1
                stb.Append(Nuctions.ReverseComplement(Translation.GetDefault.AnimoTable(pr.Substring(i, 1)).CodeList(j).Name))
                If j < Translation.GetDefault.AnimoTable(pr.Substring(i, 1)).CodeList.Count - 1 Then stb.Append("|")
            Next
            stb.Append(")")
        Next
        Return New System.Text.RegularExpressions.Regex(stb.ToString)
    End Function

    'Public Class PrimerEntry
    '    Public Name As String
    '    Public Sequence As String
    'End Class
    Friend Shared Sub AddFeatures(ByVal DNAs As ICollection, ByVal Features As List(Of Nuctions.Feature), Optional Primers As List(Of MCDS.PrimerInfo) = Nothing)
        Dim ft As Feature
        'Dim ftSeq As String
        Dim regFN, regRC As System.Text.RegularExpressions.Regex
        Dim rgxF As System.Text.RegularExpressions.Regex
        Dim rgxR As System.Text.RegularExpressions.Regex
        Dim mcSN As System.Text.RegularExpressions.MatchCollection
        Dim mcAC As System.Text.RegularExpressions.MatchCollection
        Dim mFT As System.Text.RegularExpressions.Match
        Dim gf As GeneFile

        'regex buffer
        Static RegexFGDict As New Dictionary(Of Feature, System.Text.RegularExpressions.Regex)
        Static RegexRGDict As New Dictionary(Of Feature, System.Text.RegularExpressions.Regex)

        Dim removelist As New List(Of Feature)
        For Each key As Feature In RegexFGDict.Keys
            If Not Features.Contains(key) Then removelist.Add(key)
        Next
        For Each key As Feature In removelist
            RegexFGDict.Remove(key)
        Next
        For Each key As Feature In RegexRGDict.Keys
            If Not Features.Contains(key) Then removelist.Add(key)
        Next
        For Each key As Feature In removelist
            RegexRGDict.Remove(key)
        Next
        If Primers Is Nothing Then Primers = New List(Of MCDS.PrimerInfo)
        If DNAs.Count * (Features.Count + Primers.Count) > 500 Then
            'We need to run at Cancellation Mode.

            Dim _Cancel As New CancelRunViewModel() With {.Operation = "Generating DNAs from Pseudo Products"}
            Dim ConnectingTask As New System.Threading.Tasks.Task(Sub(token As System.Threading.CancellationToken)
                                                                      Try
                                                                          For Each gf In DNAs
                                                                              If token.IsCancellationRequested Then Exit For
                                                                              gf.Features.Clear()
                                                                              If gf.End_F.EndsWith("B") Then
                                                                                  gf.Features.Add(New GeneAnnotation(gf.Features.Count,
                                                                                  "f_end", "", gf.End_F,
                                                                                  1, 1, False, gf))
                                                                              ElseIf Not gf.End_F.StartsWith("::") Then
                                                                                  gf.Features.Add(New GeneAnnotation(gf.Features.Count,
                                                                                  "f_end", "", gf.End_F,
                                                                                  1, gf.End_F.Length - 2, False, gf))
                                                                              End If
                                                                              If gf.End_R.EndsWith("B") Then
                                                                                  gf.Features.Add(New GeneAnnotation(gf.Features.Count,
                                                                                  "r_end", "", gf.End_R,
                                                                                  gf.Sequence.Length, gf.Sequence.Length, False, gf))
                                                                              ElseIf Not gf.End_R.StartsWith("::") Then
                                                                                  gf.Features.Add(New GeneAnnotation(gf.Features.Count,
                                                                                  "r_end", "", gf.End_R,
                                                                                  gf.Sequence.Length + 2 - gf.End_R.Length, gf.Sequence.Length, False, gf))
                                                                              End If
                                                                              For Each ca In gf.CompareAnnotations
                                                                                  gf.Features.Add(ca)
                                                                              Next
                                                                          Next

                                                                          If token.IsCancellationRequested Then _Cancel.Close() : Return

                                                                          For Each ft In Features
                                                                              If token.IsCancellationRequested Then Exit For
                                                                              If ft.Sequence.Length > 8 Then
                                                                                  'we only analyze the features that are over 8 bps.
                                                                                  regFN = New System.Text.RegularExpressions.Regex(ft.Sequence, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                                                                                  regRC = New System.Text.RegularExpressions.Regex(ft.RCSequence, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                                                                                  If ft.Type.ToLower = "gene" Or ft.Type.ToLower = "cds" Then
                                                                                      If RegexFGDict.ContainsKey(ft) Then
                                                                                          rgxF = RegexFGDict(ft)
                                                                                      Else
                                                                                          rgxF = GetAnimoAcidBasedMatch(ft.Sequence)
                                                                                          RegexFGDict.Add(ft, rgxF)
                                                                                      End If
                                                                                      If RegexRGDict.ContainsKey(ft) Then
                                                                                          rgxR = RegexRGDict(ft)
                                                                                      Else
                                                                                          rgxR = GetReverseAnimoAcidBasedMatch(ft.Sequence)
                                                                                          RegexRGDict.Add(ft, rgxR)
                                                                                      End If
                                                                                  Else
                                                                                      rgxF = New System.Text.RegularExpressions.Regex("-")
                                                                                      rgxR = New System.Text.RegularExpressions.Regex("-")
                                                                                  End If
                                                                                  For Each gf In DNAs

                                                                                      If token.IsCancellationRequested Then Exit For

                                                                                      If gf.Iscircular Then
                                                                                          mcSN = regFN.Matches(gf.Sequence & gf.Sequence)
                                                                                          For Each mFT In mcSN
                                                                                              If mFT.Index <= gf.Sequence.Length - 1 Then
                                                                                                  gf.Features.Add(New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, IIf(mFT.Index + ft.Sequence.Length > gf.Sequence.Length, mFT.Index + ft.Sequence.Length - gf.Sequence.Length, mFT.Index + ft.Sequence.Length), False) With {.Vector = gf, .Feature = ft})
                                                                                              End If
                                                                                          Next
                                                                                          mcAC = regRC.Matches(gf.Sequence & gf.Sequence)
                                                                                          For Each mFT In mcAC
                                                                                              If mFT.Index <= gf.Sequence.Length - 1 Then
                                                                                                  gf.Features.Add(New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, IIf(mFT.Index + ft.Sequence.Length > gf.Sequence.Length, mFT.Index + ft.Sequence.Length - gf.Sequence.Length, mFT.Index + ft.Sequence.Length), True) With {.Vector = gf, .Feature = ft})
                                                                                              End If
                                                                                          Next

                                                                                          'search for cds and annotate

                                                                                          If ft.Type.ToLower = "gene" Or ft.Type.ToLower = "cds" Then
                                                                                              Dim l As Integer = ft.Sequence.Length
                                                                                              If l Mod 3 = 0 Then

                                                                                                  mcSN = rgxF.Matches(gf.Sequence & gf.Sequence)
                                                                                                  Dim ga As GeneAnnotation
                                                                                                  For Each mFT In mcSN
                                                                                                      If token.IsCancellationRequested Then Exit For
                                                                                                      If mFT.Index <= gf.Sequence.Length - 1 And mFT.Captures(0).Value <> ft.Sequence Then
                                                                                                          ga = New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, IIf(mFT.Index + ft.Sequence.Length > gf.Sequence.Length, mFT.Index + ft.Sequence.Length - gf.Sequence.Length, mFT.Index + ft.Sequence.Length), False) With {.Vector = gf, .Feature = ft}
                                                                                                          ga.Label += "(CDS)"
                                                                                                          gf.Features.Add(ga)
                                                                                                      End If
                                                                                                  Next
                                                                                                  mcAC = rgxR.Matches(gf.Sequence & gf.Sequence)
                                                                                                  For Each mFT In mcAC
                                                                                                      If token.IsCancellationRequested Then Exit For
                                                                                                      If mFT.Index <= gf.Sequence.Length - 1 And mFT.Captures(0).Value <> ft.RCSequence Then
                                                                                                          ga = New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, IIf(mFT.Index + ft.Sequence.Length > gf.Sequence.Length, mFT.Index + ft.Sequence.Length - gf.Sequence.Length, mFT.Index + ft.Sequence.Length), True) With {.Vector = gf, .Feature = ft}
                                                                                                          ga.Label += "(CDS)"
                                                                                                          gf.Features.Add(ga)
                                                                                                      End If
                                                                                                  Next
                                                                                              End If
                                                                                          End If

                                                                                      Else
                                                                                          mcSN = regFN.Matches(gf.Sequence)
                                                                                          For Each mFT In mcSN
                                                                                              If token.IsCancellationRequested Then Exit For
                                                                                              gf.Features.Add(New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, mFT.Index + ft.Sequence.Length, False) With {.Vector = gf, .Feature = ft})
                                                                                          Next
                                                                                          mcAC = regRC.Matches(gf.Sequence)
                                                                                          For Each mFT In mcAC
                                                                                              If token.IsCancellationRequested Then Exit For
                                                                                              gf.Features.Add(New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, mFT.Index + ft.Sequence.Length, True) With {.Vector = gf, .Feature = ft})
                                                                                          Next

                                                                                          'search for cds and annotate
                                                                                          If ft.Type.ToLower = "gene" Or ft.Type.ToLower = "cds" Then
                                                                                              Dim l As Integer = ft.Sequence.Length
                                                                                              If l Mod 3 = 0 Then
                                                                                                  mcSN = rgxF.Matches(gf.Sequence)
                                                                                                  Dim ga As GeneAnnotation
                                                                                                  For Each mFT In mcSN
                                                                                                      If token.IsCancellationRequested Then Exit For
                                                                                                      If mFT.Index <= gf.Sequence.Length - 1 And mFT.Captures(0).Value <> ft.Sequence Then
                                                                                                          ga = New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, mFT.Index + ft.Sequence.Length, False)
                                                                                                          ga.Vector = gf
                                                                                                          ga.Feature = ft
                                                                                                          ga.Label += "(CDS)"
                                                                                                          gf.Features.Add(ga)
                                                                                                      End If
                                                                                                  Next
                                                                                                  mcAC = rgxR.Matches(gf.Sequence)
                                                                                                  For Each mFT In mcAC
                                                                                                      If token.IsCancellationRequested Then Exit For
                                                                                                      If mFT.Index <= gf.Sequence.Length - 1 And mFT.Captures(0).Value <> ft.RCSequence Then
                                                                                                          ga = New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, mFT.Index + ft.Sequence.Length, True)
                                                                                                          ga.Vector = gf
                                                                                                          ga.Feature = ft
                                                                                                          ga.Label += "(CDS)"
                                                                                                          gf.Features.Add(ga)
                                                                                                      End If
                                                                                                  Next
                                                                                              End If
                                                                                          End If
                                                                                      End If
                                                                                      If token.IsCancellationRequested Then Exit For
                                                                                  Next
                                                                              End If
                                                                              If token.IsCancellationRequested Then Exit For
                                                                          Next

                                                                          If token.IsCancellationRequested Then _Cancel.Close() : Return

                                                                          If Primers IsNot Nothing Then
                                                                              For Each pm As MCDS.PrimerInfo In Primers
                                                                                  If token.IsCancellationRequested Then Exit For
                                                                                  'do not use unused features
                                                                                  'If Not ft.Useful Then Continue For
                                                                                  'ftSeq = ft.Sequence
                                                                                  If pm.Sequence Is Nothing Then Continue For
                                                                                  If pm.Sequence.Length > 10 Then
                                                                                      'we only analyze the features that are over 8 bps.
                                                                                      Dim BindingSequence As String = pm.Sequence
                                                                                      If pm.Sequence.Contains(">") Then BindingSequence = BindingSequence.Substring(BindingSequence.LastIndexOf(">") + 1)
                                                                                      regFN = New System.Text.RegularExpressions.Regex(BindingSequence, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                                                                                      regRC = New System.Text.RegularExpressions.Regex(Nuctions.ReverseComplement(BindingSequence), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                                                                                      For Each gf In DNAs
                                                                                          If token.IsCancellationRequested Then Exit For
                                                                                          If gf.Iscircular Then
                                                                                              mcSN = regFN.Matches(gf.Sequence & gf.Sequence)
                                                                                              For Each mFT In mcSN
                                                                                                  If token.IsCancellationRequested Then Exit For
                                                                                                  If mFT.Index <= gf.Sequence.Length - 1 Then
                                                                                                      gf.Features.Add(New GeneAnnotation(gf.Features.Count, Nothing, mFT.Index + 1, IIf(mFT.Index + BindingSequence.Length > gf.Sequence.Length, mFT.Index + BindingSequence.Length - gf.Sequence.Length, mFT.Index + BindingSequence.Length), False) With {.Vector = gf, .Type = "Primer", .Label = pm.Name})
                                                                                                  End If
                                                                                              Next
                                                                                              mcAC = regRC.Matches(gf.Sequence & gf.Sequence)
                                                                                              For Each mFT In mcAC
                                                                                                  If token.IsCancellationRequested Then Exit For
                                                                                                  If mFT.Index <= gf.Sequence.Length - 1 Then
                                                                                                      gf.Features.Add(New GeneAnnotation(gf.Features.Count, Nothing, mFT.Index + 1, IIf(mFT.Index + BindingSequence.Length > gf.Sequence.Length, mFT.Index + BindingSequence.Length - gf.Sequence.Length, mFT.Index + BindingSequence.Length), True) With {.Vector = gf, .Type = "Primer", .Label = pm.Name})
                                                                                                  End If
                                                                                              Next

                                                                                          Else
                                                                                              mcSN = regFN.Matches(gf.Sequence)
                                                                                              For Each mFT In mcSN
                                                                                                  If token.IsCancellationRequested Then Exit For
                                                                                                  gf.Features.Add(New GeneAnnotation(gf.Features.Count, Nothing, mFT.Index + 1, mFT.Index + BindingSequence.Length, False) With {.Vector = gf, .Type = "Primer", .Label = pm.Name})
                                                                                              Next
                                                                                              mcAC = regRC.Matches(gf.Sequence)
                                                                                              For Each mFT In mcAC
                                                                                                  If token.IsCancellationRequested Then Exit For
                                                                                                  gf.Features.Add(New GeneAnnotation(gf.Features.Count, Nothing, mFT.Index + 1, mFT.Index + BindingSequence.Length, True) With {.Vector = gf, .Type = "Primer", .Label = pm.Name})
                                                                                              Next

                                                                                          End If
                                                                                          If token.IsCancellationRequested Then Exit For
                                                                                      Next
                                                                                  End If
                                                                              Next
                                                                          End If

                                                                          _Cancel.Close()
                                                                      Catch ex As Exception

                                                                      End Try
                                                                      _Cancel.Close()
                                                                  End Sub, _Cancel.Token)
            ConnectingTask.Start()
            CancelRunViewModel.ShowCancelRunWindow(_Cancel)


        Else




            For Each gf In DNAs
                gf.Features.Clear()
                If gf.End_F.EndsWith("B") Then
                    gf.Features.Add(New GeneAnnotation(gf.Features.Count,
                    "f_end", "", gf.End_F,
                    1, 1, False, gf))
                ElseIf Not gf.End_F.StartsWith("::") Then
                    gf.Features.Add(New GeneAnnotation(gf.Features.Count,
                    "f_end", "", gf.End_F,
                    1, gf.End_F.Length - 2, False, gf))
                End If
                If gf.End_R.EndsWith("B") Then
                    gf.Features.Add(New GeneAnnotation(gf.Features.Count,
                    "r_end", "", gf.End_R,
                    gf.Sequence.Length, gf.Sequence.Length, False, gf))
                ElseIf Not gf.End_R.StartsWith("::") Then
                    gf.Features.Add(New GeneAnnotation(gf.Features.Count,
                    "r_end", "", gf.End_R,
                    gf.Sequence.Length + 2 - gf.End_R.Length, gf.Sequence.Length, False, gf))
                End If
                For Each ca In gf.CompareAnnotations
                    gf.Features.Add(ca)
                Next
            Next
            For Each ft In Features
                If ft.Sequence.Length > 8 Then
                    'we only analyze the features that are over 8 bps.
                    regFN = New System.Text.RegularExpressions.Regex(ft.Sequence, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                    regRC = New System.Text.RegularExpressions.Regex(ft.RCSequence, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                    If ft.Type.ToLower = "gene" Or ft.Type.ToLower = "cds" Then
                        If RegexFGDict.ContainsKey(ft) Then
                            rgxF = RegexFGDict(ft)
                        Else
                            rgxF = GetAnimoAcidBasedMatch(ft.Sequence)
                            RegexFGDict.Add(ft, rgxF)
                        End If
                        If RegexRGDict.ContainsKey(ft) Then
                            rgxR = RegexRGDict(ft)
                        Else
                            rgxR = GetReverseAnimoAcidBasedMatch(ft.Sequence)
                            RegexRGDict.Add(ft, rgxR)
                        End If
                    Else
                        rgxF = New System.Text.RegularExpressions.Regex("-")
                        rgxR = New System.Text.RegularExpressions.Regex("-")
                    End If
                    For Each gf In DNAs

                        If gf.Iscircular Then
                            mcSN = regFN.Matches(gf.Sequence & gf.Sequence)
                            For Each mFT In mcSN
                                If mFT.Index <= gf.Sequence.Length - 1 Then
                                    gf.Features.Add(New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, IIf(mFT.Index + ft.Sequence.Length > gf.Sequence.Length, mFT.Index + ft.Sequence.Length - gf.Sequence.Length, mFT.Index + ft.Sequence.Length), False) With {.Vector = gf, .Feature = ft})
                                End If
                            Next
                            mcAC = regRC.Matches(gf.Sequence & gf.Sequence)
                            For Each mFT In mcAC
                                If mFT.Index <= gf.Sequence.Length - 1 Then
                                    gf.Features.Add(New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, IIf(mFT.Index + ft.Sequence.Length > gf.Sequence.Length, mFT.Index + ft.Sequence.Length - gf.Sequence.Length, mFT.Index + ft.Sequence.Length), True) With {.Vector = gf, .Feature = ft})
                                End If
                            Next

                            'search for cds and annotate

                            If ft.Type.ToLower = "gene" Or ft.Type.ToLower = "cds" Then
                                Dim l As Integer = ft.Sequence.Length
                                If l Mod 3 = 0 Then

                                    mcSN = rgxF.Matches(gf.Sequence & gf.Sequence)
                                    Dim ga As GeneAnnotation
                                    For Each mFT In mcSN
                                        If mFT.Index <= gf.Sequence.Length - 1 And mFT.Captures(0).Value <> ft.Sequence Then
                                            ga = New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, IIf(mFT.Index + ft.Sequence.Length > gf.Sequence.Length, mFT.Index + ft.Sequence.Length - gf.Sequence.Length, mFT.Index + ft.Sequence.Length), False) With {.Vector = gf, .Feature = ft}
                                            ga.Label += "(CDS)"
                                            gf.Features.Add(ga)
                                        End If
                                    Next
                                    mcAC = rgxR.Matches(gf.Sequence & gf.Sequence)
                                    For Each mFT In mcAC
                                        If mFT.Index <= gf.Sequence.Length - 1 And mFT.Captures(0).Value <> ft.RCSequence Then
                                            ga = New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, IIf(mFT.Index + ft.Sequence.Length > gf.Sequence.Length, mFT.Index + ft.Sequence.Length - gf.Sequence.Length, mFT.Index + ft.Sequence.Length), True) With {.Vector = gf, .Feature = ft}
                                            ga.Label += "(CDS)"
                                            gf.Features.Add(ga)
                                        End If
                                    Next
                                End If
                            End If

                        Else
                            mcSN = regFN.Matches(gf.Sequence)
                            For Each mFT In mcSN
                                gf.Features.Add(New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, mFT.Index + ft.Sequence.Length, False) With {.Vector = gf, .Feature = ft})
                            Next
                            mcAC = regRC.Matches(gf.Sequence)
                            For Each mFT In mcAC
                                gf.Features.Add(New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, mFT.Index + ft.Sequence.Length, True) With {.Vector = gf, .Feature = ft})
                            Next

                            'search for cds and annotate
                            If ft.Type.ToLower = "gene" Or ft.Type.ToLower = "cds" Then
                                Dim l As Integer = ft.Sequence.Length
                                If l Mod 3 = 0 Then
                                    mcSN = rgxF.Matches(gf.Sequence)
                                    Dim ga As GeneAnnotation
                                    For Each mFT In mcSN
                                        If mFT.Index <= gf.Sequence.Length - 1 And mFT.Captures(0).Value <> ft.Sequence Then
                                            ga = New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, mFT.Index + ft.Sequence.Length, False)
                                            ga.Vector = gf
                                            ga.Feature = ft
                                            ga.Label += "(CDS)"
                                            gf.Features.Add(ga)
                                        End If
                                    Next
                                    mcAC = rgxR.Matches(gf.Sequence)
                                    For Each mFT In mcAC
                                        If mFT.Index <= gf.Sequence.Length - 1 And mFT.Captures(0).Value <> ft.RCSequence Then
                                            ga = New GeneAnnotation(gf.Features.Count, ft, mFT.Index + 1, mFT.Index + ft.Sequence.Length, True)
                                            ga.Vector = gf
                                            ga.Feature = ft
                                            ga.Label += "(CDS)"
                                            gf.Features.Add(ga)
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    Next
                End If
            Next
            If Primers IsNot Nothing Then
                For Each pm As MCDS.PrimerInfo In Primers
                    'do not use unused features
                    'If Not ft.Useful Then Continue For
                    'ftSeq = ft.Sequence
                    If pm.Sequence Is Nothing Then Continue For
                    If pm.Sequence.Length > 10 Then
                        'we only analyze the features that are over 8 bps.
                        Dim BindingSequence As String = pm.Sequence
                        If pm.Sequence.Contains(">") Then BindingSequence = BindingSequence.Substring(BindingSequence.LastIndexOf(">") + 1)
                        regFN = New System.Text.RegularExpressions.Regex(BindingSequence, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                        regRC = New System.Text.RegularExpressions.Regex(Nuctions.ReverseComplement(BindingSequence), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                        For Each gf In DNAs
                            If gf.Iscircular Then
                                mcSN = regFN.Matches(gf.Sequence & gf.Sequence)
                                For Each mFT In mcSN
                                    If mFT.Index <= gf.Sequence.Length - 1 Then
                                        gf.Features.Add(New GeneAnnotation(gf.Features.Count, Nothing, mFT.Index + 1, IIf(mFT.Index + BindingSequence.Length > gf.Sequence.Length, mFT.Index + BindingSequence.Length - gf.Sequence.Length, mFT.Index + BindingSequence.Length), False) With {.Vector = gf, .Type = "Primer", .Label = pm.Name})
                                    End If
                                Next
                                mcAC = regRC.Matches(gf.Sequence & gf.Sequence)
                                For Each mFT In mcAC
                                    If mFT.Index <= gf.Sequence.Length - 1 Then
                                        gf.Features.Add(New GeneAnnotation(gf.Features.Count, Nothing, mFT.Index + 1, IIf(mFT.Index + BindingSequence.Length > gf.Sequence.Length, mFT.Index + BindingSequence.Length - gf.Sequence.Length, mFT.Index + BindingSequence.Length), True) With {.Vector = gf, .Type = "Primer", .Label = pm.Name})
                                    End If
                                Next

                            Else
                                mcSN = regFN.Matches(gf.Sequence)
                                For Each mFT In mcSN
                                    gf.Features.Add(New GeneAnnotation(gf.Features.Count, Nothing, mFT.Index + 1, mFT.Index + BindingSequence.Length, False) With {.Vector = gf, .Type = "Primer", .Label = pm.Name})
                                Next
                                mcAC = regRC.Matches(gf.Sequence)
                                For Each mFT In mcAC
                                    gf.Features.Add(New GeneAnnotation(gf.Features.Count, Nothing, mFT.Index + 1, mFT.Index + BindingSequence.Length, True) With {.Vector = gf, .Type = "Primer", .Label = pm.Name})
                                Next

                            End If
                        Next
                    End If
                Next
            End If
        End If
    End Sub

    Friend Shared Function ScreenFeature(ByVal DNAs As List(Of GeneFile), ByVal Features As List(Of FeatureScreenInfo), Optional ByVal MustCircular As Boolean = False) As List(Of GeneFile)

        Dim gfList As New List(Of GeneFile)
#If ReaderMode = 0 Then
        'Dim ftSeq As String
        Dim rmlist As New List(Of GeneFile)
        For Each gf As GeneFile In DNAs
            If MustCircular Then
                If gf.Iscircular Then gfList.Add(gf.CloneWithoutFeatures)
            Else
                gfList.Add(gf.CloneWithoutFeatures)
            End If
        Next
        Dim fCon As Integer
        Dim mCon As Integer


        Features.Sort()
        For Each fsi As FeatureScreenInfo In Features
            Select Case fsi.ScreenMethod
                Case FeatureScreenEnum.None
                    rmlist.Clear()
                    For Each gf As GeneFile In gfList
                        fCon = gf.Matches(fsi.Feature.Sequence).Count + gf.Matches(fsi.Feature.RCSequence).Count
                        If fCon > 0 Then rmlist.Add(gf)
                    Next
                    For Each gf As GeneFile In rmlist
                        gfList.Remove(gf)
                    Next
                Case FeatureScreenEnum.Once
                    rmlist.Clear()
                    For Each gf As GeneFile In gfList
                        fCon = gf.Matches(fsi.Feature.Sequence).Count + gf.Matches(fsi.Feature.RCSequence).Count
                        If fCon <> 1 Then rmlist.Add(gf)
                    Next
                    For Each gf As GeneFile In rmlist
                        gfList.Remove(gf)
                    Next
                Case FeatureScreenEnum.OnceOrMore
                    rmlist.Clear()
                    For Each gf As GeneFile In gfList
                        fCon = gf.Matches(fsi.Feature.Sequence).Count + gf.Matches(fsi.Feature.RCSequence).Count
                        If fCon = 0 Then rmlist.Add(gf)
                    Next
                    For Each gf As GeneFile In rmlist
                        gfList.Remove(gf)
                    Next
                Case FeatureScreenEnum.Maximum
                    mCon = -1
                    For Each gf As GeneFile In gfList
                        fCon = gf.Matches(fsi.Feature.Sequence).Count + gf.Matches(fsi.Feature.RCSequence).Count
                        If fCon > mCon Then
                            mCon = fCon
                        End If
                    Next
                    rmlist.Clear()
                    For Each gf As GeneFile In gfList
                        fCon = gf.Matches(fsi.Feature.Sequence).Count + gf.Matches(fsi.Feature.RCSequence).Count
                        If fCon < mCon Then rmlist.Add(gf)
                    Next
                    For Each gf As GeneFile In rmlist
                        gfList.Remove(gf)
                    Next
            End Select
        Next
#End If
        Return gfList
    End Function
    Friend Shared Function ScreenLength(ByVal DNAs As Collection, ByVal StartLength As Integer, ByVal EndLength As Integer) As List(Of GeneFile)
        Dim gfList As New List(Of GeneFile)
#If ReaderMode = 0 Then
        Dim gf As GeneFile
        'add end features
        For Each gf In DNAs
            'ftSeq = ft.Sequence
            If StartLength <= gf.Sequence.Length And gf.Sequence.Length <= EndLength Then gfList.Add(gf.CloneWithoutFeatures)
        Next
#End If
        Return gfList
    End Function
    Friend Shared Function ModifyDNA(ByVal DNAs As List(Of GeneFile), ByVal Modify_Method As ModificationMethodEnum) As List(Of GeneFile)
        Dim gfList As New List(Of GeneFile)
#If ReaderMode = 0 Then
        Dim gf As GeneFile
        Dim gfr As GeneFile
        For Each gf In DNAs
            gfr = gf.CloneWithoutFeatures
            gfr.ModificationDate = Date.Now
            Select Case Modify_Method
                Case ModificationMethodEnum.CIAP
                    If Not gfr.Iscircular Then
                        gfr.End_F = gfr.End_F.Replace("*", "^")
                        gfr.End_R = gfr.End_R.Replace("*", "^")
                    End If
                Case ModificationMethodEnum.Klewnow
                    If Not gfr.Iscircular Then
                        If gfr.End_F.IndexOf("5") > -1 Then gfr.End_F = gfr.End_F.Substring(0, 1) + "B"
                        If gfr.End_R.IndexOf("5") > -1 Then gfr.End_R = gfr.End_R.Substring(0, 1) + "B"
                    End If
                Case ModificationMethodEnum.PNK
                    If Not gfr.Iscircular Then
                        gfr.End_F = gfr.End_F.Replace("^", "*")
                        gfr.End_R = gfr.End_R.Replace("^", "*")
                    End If
                Case ModificationMethodEnum.T4DNAP
                    If Not gfr.Iscircular Then
                        If gfr.End_F.IndexOf("5") > -1 Then gfr.End_F = gfr.End_F.Substring(0, 1) + "B"
                        If gfr.End_R.IndexOf("5") > -1 Then gfr.End_R = gfr.End_R.Substring(0, 1) + "B"
                        If gfr.End_F.IndexOf("3") > -1 Then gfr.Sequence = gfr.RCSequence.Substring(gfr.End_F.Length - 2, gfr.Sequence.Length - gfr.End_F.Length + 2) : gfr.End_F = gfr.End_F.Substring(0, 1) + "B"
                        If gfr.End_R.IndexOf("3") > -1 Then gfr.Sequence = gfr.RCSequence.Substring(0, gfr.Sequence.Length - gfr.End_R.Length + 2) : gfr.End_R = gfr.End_R.Substring(0, 1) + "B"
                    End If
            End Select
            gfList.Add(gfr)
        Next
#End If
        Return gfList
    End Function

    Public Enum ModificationMethodEnum
        'T4DNAP Klewnow CIAP PNK
        T4DNAP
        Klewnow
        CIAP
        PNK
    End Enum
    Protected Class PCRInfo
        Public Result As String
        Public Attach As String
        Public Index As Integer
        Public Name As String
        Public Sub New(ByVal vResult As String, ByVal vAttach As String, ByVal vName As String)
            Result = vResult
            Attach = vAttach
            Name = vName
        End Sub
    End Class
    <Serializable()> Protected Class PrimerInfo
        Public ShortPrimer As String
        Public PrimerAttach As String
        Public PrimerRegex As System.Text.RegularExpressions.Regex
        Public Valid As Boolean
        Public Sub New(ByVal vPrimer As String)
            vPrimer = TAGCFilter(vPrimer)
            Valid = vPrimer.Length > 12
            If Valid Then
                ShortPrimer = vPrimer.Substring(vPrimer.Length - 12, 12)
                PrimerAttach = vPrimer.Substring(0, vPrimer.Length - 12)
                PrimerRegex = New System.Text.RegularExpressions.Regex(ShortPrimer)
            End If
        End Sub
    End Class
    Friend Shared Function PCR(ByVal DNAs As List(Of GeneFile), ByVal Primers As List(Of String), Optional ByVal ConsiderOverlapExtension As Boolean = False) As List(Of GeneFile)
        Dim gfList As New List(Of GeneFile)
#If ReaderMode = 0 Then
        Dim SPrimerList As New List(Of PrimerInfo)
        Dim PI As PrimerInfo
        For Each pr As String In Primers
            PI = New PrimerInfo(pr)
            If PI.Valid Then SPrimerList.Add(PI)
        Next

        Dim mcPrime As System.Text.RegularExpressions.MatchCollection
        Dim mcU As System.Text.RegularExpressions.Match

        Dim gf As GeneFile
        Dim gfr As GeneFile
        'Dim fCol As New List(Of PCRInfo)
        'Dim rCol As New List(Of PCRInfo)

        Dim InfoCol As New List(Of PCRInfo)


        Dim ModelSeq As String
        Dim ModelBuilder As System.Text.StringBuilder
        Dim boxlength As Integer
        boxlength = IIf(ConsiderOverlapExtension, 12, 40)


        For Each gf In DNAs
            For Each PI In SPrimerList
                If gf.Iscircular Then
                    '正向序列
                    ModelBuilder = New System.Text.StringBuilder
                    'ModelBuilder.Append(gf.Sequence.Substring(gf.Sequence.Length - boxlength, boxlength))
                    ModelBuilder.Append(gf.Sequence)
                    ModelBuilder.Append(gf.Sequence)
                    'ModelBuilder.Append(gf.Sequence.Substring(0, boxlength))
                    ModelSeq = ModelBuilder.ToString

                    'ModelSeq = gf.Sequence.Substring(gf.Sequence.Length - 11, 11) + gf.Sequence + gf.Sequence.Substring(0, 11)
                    mcPrime = PI.PrimerRegex.Matches(ModelSeq)
                    For Each mcU In mcPrime
                        If mcU.Index < gf.Sequence.Length Then
                            InfoCol.Add(New PCRInfo(ModelSeq.Substring(mcU.Index, gf.Sequence.Length), PI.PrimerAttach, gf.Name))
                        End If
                    Next
                    '反向序列
                    ModelBuilder = New System.Text.StringBuilder
                    ModelBuilder.Append(gf.RCSequence)
                    ModelBuilder.Append(gf.RCSequence)
                    ModelSeq = ModelBuilder.ToString

                    mcPrime = PI.PrimerRegex.Matches(ModelSeq)
                    For Each mcU In mcPrime
                        If mcU.Index < gf.Sequence.Length Then
                            InfoCol.Add(New PCRInfo(ModelSeq.Substring(mcU.Index, gf.Sequence.Length), PI.PrimerAttach, gf.Name))
                        End If

                    Next
                Else
                    ModelSeq = gf.Sequence

                    'ModelSeq = gf.Sequence.Substring(gf.Sequence.Length - 11, 11) + gf.Sequence + gf.Sequence.Substring(0, 11)
                    mcPrime = PI.PrimerRegex.Matches(ModelSeq)
                    For Each mcU In mcPrime
                        InfoCol.Add(New PCRInfo(ModelSeq.Substring(mcU.Index), PI.PrimerAttach, gf.Name))
                    Next

                    '反向序列
                    ModelSeq = gf.RCSequence

                    mcPrime = PI.PrimerRegex.Matches(ModelSeq)
                    For Each mcU In mcPrime
                        InfoCol.Add(New PCRInfo(ModelSeq.Substring(mcU.Index), PI.PrimerAttach, gf.Name))
                    Next
                End If
            Next
        Next
        '搜索PCR产物 实现对OEPCR的支持 至少12碱基重叠

        Dim i As Integer
        Dim k As Integer
        Dim box As System.Text.RegularExpressions.Regex

        Dim WaitingList As New List(Of PCRInfo)
        Dim RemoveList As New List(Of PCRInfo)

        For i = 0 To InfoCol.Count - 1
            If InfoCol(i).Result.Length < boxlength Then Continue For
            WaitingList.Clear()
            RemoveList.Clear()
            For j As Integer = i To InfoCol.Count - 1
                WaitingList.Add(InfoCol(j))
            Next
            For k = 0 To InfoCol(i).Result.Length - boxlength
                '计算反相互补的模型
                box = New System.Text.RegularExpressions.Regex(ReverseComplement(InfoCol(i).Result.Substring(k, boxlength)))
                '从所有其他产物当中搜索反向互补的序列
                For Each pif As PCRInfo In WaitingList
                    '得到的是最短的产物
                    If box.IsMatch(pif.Result) Then
                        mcU = box.Match(pif.Result)
                        gfr = New GeneFile
                        gfr.End_F = "^B"
                        gfr.End_R = "^B"
                        'gfr.Iscircular = False
                        gfr.ModificationDate = Date.Now
                        If pif.Name = InfoCol(i).Name Then
                            gfr.Name = IIf(ConsiderOverlapExtension, "OEPCR-", "PCR-") + InfoCol(i).Name + " (" + gfList.Count.ToString + ")"
                        Else
                            gfr.Name = IIf(ConsiderOverlapExtension, "OEPCR-", "PCR-") + InfoCol(i).Name + pif.Name + " (" + gfList.Count.ToString + ")"
                        End If
                        gfr.Phos_F = False
                        gfr.Phos_R = False
                        ModelBuilder = New System.Text.StringBuilder
                        ModelBuilder.Append(InfoCol(i).Attach)
                        ModelBuilder.Append(InfoCol(i).Result.Substring(0, k + boxlength))
                        ModelBuilder.Append(ReverseComplement(pif.Result.Substring(0, mcU.Index)))
                        ModelBuilder.Append(ReverseComplement(pif.Attach))
                        gfr.Sequence = ModelBuilder.ToString
                        gfList.Add(gfr)
                        RemoveList.Add(pif)
                    End If
                Next
                For Each pif As PCRInfo In RemoveList
                    WaitingList.Remove(pif)
                Next
                RemoveList.Clear()
            Next
        Next
        ReduceDNA(gfList)
#End If
        Return gfList
    End Function
    Friend Shared Function ScreenPCR(ByVal DNAs As List(Of GeneFile), ByVal FPrimer As String, ByVal RPrimer As String, ByVal Max As Integer, ByVal Min As Integer, Optional ByVal OnlyCircular As Boolean = False) As List(Of GeneFile)
        'Dim gfList As ist(Of GeneFile) 'list that is used to hold product DNAs
        Dim scrList As New List(Of GeneFile) 'list that holds all the correct DNAs
        'Dim hit As Boolean
#If ReaderMode = 0 Then
        Dim col As List(Of GeneFile)
        Dim pmrs As New List(Of String)
        pmrs.Add(FPrimer)
        pmrs.Add(RPrimer)
        Dim resList As List(Of GeneFile)

        For Each DNA As GeneFile In DNAs
            If OnlyCircular And (Not DNA.Iscircular) Then Continue For
            col = New List(Of GeneFile)
            col.Add(DNA)
            '对每个文件进行PCR
            resList = PCR(col, pmrs)
            For Each gf As GeneFile In resList
                '只要找到一个符合条件的就跳出来
                If gf.Sequence.Length >= Min And gf.Sequence.Length <= Max Then scrList.Add(DNA.CloneWithoutFeatures) : Exit For
            Next
        Next
#End If
        Return scrList
    End Function
    '    Friend Shared Function LigateDNA(ByVal gList As List(Of GeneFile), ByVal ThirdLevel As Boolean) As List(Of GeneFile)
    '        Dim MR As New MultipleVirtualReactor(gList, New PseudoConnector(AddressOf PseudoGeneFile.EndLigate), New PseudoSelfConnector(AddressOf PseudoGeneFile.SelfLigate), True, IIf(ThirdLevel, 2, 1))
    '        MR.Connect()
    '        Dim vResults As New List(Of GeneFile)
    '        For Each gf In MR.GetProducts()
    '            vResults.Add(gf.GetResult)
    '        Next

    '        Dim rmList As New List(Of GeneFile)

    '        For Each gf1 In vResults
    '            If rmList.Contains(gf1) Then Continue For
    '            For Each gf2 In vResults
    '                If gf2 Is gf1 Then Continue For
    '                If rmList.Contains(gf2) Then Continue For
    '                If gf1 = gf2 Then rmList.Add(gf2)
    '            Next
    '        Next
    '        For Each gf In rmList
    '            vResults.Remove(gf)
    '        Next
    '        Return vResults


    '        Dim cSList As New List(Of GeneFile)
    '#If ReaderMode = 0 Then
    '        Dim c0List As New List(Of GeneFile)
    '        For Each gf As GeneFile In gList
    '            c0List.Add(gf.CloneWithoutFeatures)
    '        Next

    '        Dim c1List As New List(Of GeneFile)

    '        For Each gf1 As GeneFile In c0List
    '            For Each gf2 As GeneFile In c0List
    '                Dim gf As GeneFile
    '                gf = gf1 + gf2
    '                If Not (gf Is Nothing) Then c1List.Add(gf)
    '                gf = gf1 + gf2.RC
    '                If Not (gf Is Nothing) Then c1List.Add(gf)
    '                gf = gf1.RC + gf2
    '                If Not (gf Is Nothing) Then c1List.Add(gf)
    '                gf = gf1.RC + gf2.RC
    '                If Not (gf Is Nothing) Then c1List.Add(gf)
    '            Next
    '        Next

    '        Dim c2List As New List(Of GeneFile)

    '        If ThirdLevel Then
    '            For Each gf1 As GeneFile In c1List
    '                For Each gf2 As GeneFile In c0List
    '                    Dim gf As GeneFile
    '                    gf = gf1 + gf2
    '                    If Not (gf Is Nothing) Then c2List.Add(gf)
    '                    gf = gf1 + gf2.RC
    '                    If Not (gf Is Nothing) Then c2List.Add(gf)
    '                    gf = gf1.RC + gf2
    '                    If Not (gf Is Nothing) Then c2List.Add(gf)
    '                    gf = gf1.RC + gf2.RC
    '                    If Not (gf Is Nothing) Then c2List.Add(gf)
    '                Next
    '            Next
    '        End If

    '        Dim gfu As GeneFile
    '        For Each gf As GeneFile In c0List
    '            gfu = +gf
    '            If Not (gfu Is Nothing) Then cSList.Add(gfu)
    '        Next
    '        For Each gf As GeneFile In c1List
    '            gfu = +gf
    '            If Not (gfu Is Nothing) Then cSList.Add(gfu)
    '        Next
    '        For Each gf As GeneFile In c2List
    '            gfu = +gf
    '            If Not (gfu Is Nothing) Then cSList.Add(gfu)
    '        Next
    '        cSList.AddRange(c0List)
    '        cSList.AddRange(c1List)
    '        cSList.AddRange(c2List)
    '        ReduceDNA(cSList)
    '        For Each gf As GeneFile In cSList
    '            gf.Name = "Lig " + gf.Length.ToString
    '        Next
    '#End If
    '        Return cSList
    '    End Function
    '    Friend Shared Function LigateDNA(ByVal DNAs As Collection, ByVal ThirdLevel As Boolean) As List(Of GeneFile)
    '        Dim gfList As New List(Of GeneFile)
    '#If ReaderMode = 0 Then
    '        'we will not cover a ligation that include more than 3 DNAs, since the efficiency if quite low.
    '        Dim gf1 As GeneFile, gf2 As GeneFile
    '        Dim gfr As GeneFile
    '        '1 self ligation
    '        For Each gf1 In DNAs
    '            gfList.Add(gf1.CloneWithoutFeatures) 'contains all the substrate fragment
    '            If Not gf1.Iscircular And (gf1.End_F = gf1.End_R) And (gf1.Phos_F Or gf1.Phos_R) Then
    '                'check for compitable ends:
    '                gfr = New GeneFile
    '                If gf1.End_F.Equals("") Then
    '                    gfr.Sequence = gf1.Sequence
    '                Else
    '                    gfr.Sequence = gf1.Sequence.Substring(0, gf1.Sequence.Length - gf1.End_F.Length + 1)
    '                End If
    '                'gfr.Iscircular = True
    '                gfr.Name = "CircularLigation-" & gf1.Name
    '                gfr.ModificationDate = Date.Now
    '                gfr.Phos_F = True
    '                gfr.Phos_R = True
    '                gfr.End_F = ""
    '                gfr.End_R = ""
    '                gfList.Add(gfr)
    '            End If
    '        Next
    '        '2 dimer ligation
    '        Dim imList As New List(Of GeneFile)
    '        For Each gf1 In DNAs
    '            If gf1.Iscircular Then Continue For
    '            For Each gf2 In DNAs
    '                If gf2.Iscircular Then Continue For
    '                If gf1.End_F = gf2.End_F And (gf1.Phos_F Or gf2.Phos_F) Then
    '                    gfr = New GeneFile
    '                    If gf1.End_F.Equals("") Then
    '                        gfr.Sequence = gf2.RCSequence + gf1.Sequence
    '                    Else
    '                        gfr.Sequence = gf2.RCSequence.Substring(0, gf2.Sequence.Length - gf2.End_F.Length + 1) + gf1.Sequence
    '                    End If
    '                    'gfr.Sequence = IIf(gf1.End_F.Equals(""), gf2.RCSequence + gf1.Sequence, _
    '                    'gf2.RCSequence.Substring(0, gf2.Sequence.Length - gf2.End_F.Length + 1) + gf1.Sequence)
    '                    'gfr.Iscircular = False
    '                    gfr.Name = "Ligation-FF-" & gf1.Name & " & " & gf2.Name
    '                    gfr.ModificationDate = Date.Now
    '                    gfr.Phos_F = gf2.Phos_R
    '                    gfr.Phos_R = gf1.Phos_R
    '                    gfr.End_F = gf2.End_R
    '                    gfr.End_R = gf1.End_R
    '                    'gfList.Add(gfr)
    '                    imList.Add(gfr)
    '                End If
    '                If gf1.End_F = gf2.End_R And (gf1.Phos_F Or gf2.Phos_R) Then
    '                    gfr = New GeneFile
    '                    If gf1.End_F.Equals("") Then
    '                        gfr.Sequence = gf2.Sequence + gf1.Sequence
    '                    Else
    '                        gfr.Sequence = gf2.Sequence.Substring(0, gf2.Sequence.Length - gf2.End_R.Length + 1) + gf1.Sequence
    '                    End If
    '                    'gfr.Sequence = IIf(gf1.End_F.Equals(""), gf2.Sequence + gf1.Sequence, _
    '                    'gf2.Sequence.Substring(0, gf2.Sequence.Length - gf2.End_F.Length + 1) + gf1.Sequence)

    '                    'gfr.Iscircular = False
    '                    gfr.Name = "Ligation-FR-" & gf1.Name & " & " & gf2.Name
    '                    gfr.ModificationDate = Date.Now
    '                    gfr.Phos_F = gf2.Phos_F
    '                    gfr.Phos_R = gf1.Phos_R
    '                    gfr.End_F = gf2.End_F
    '                    gfr.End_R = gf1.End_R
    '                    'gfList.Add(gfr)
    '                    imList.Add(gfr)

    '                End If
    '                If gf1.End_R = gf2.End_F And (gf1.Phos_R Or gf2.Phos_F) Then
    '                    gfr = New GeneFile
    '                    If gf1.End_R.Equals("") Then
    '                        gfr.Sequence = gf1.Sequence + gf2.Sequence
    '                    Else
    '                        gfr.Sequence = gf1.Sequence.Substring(0, gf1.Sequence.Length - gf1.End_R.Length + 1) + gf2.Sequence
    '                    End If
    '                    'gfr.Sequence = IIf(gf1.End_F.Equals(""), gf1.Sequence + gf2.Sequence, _
    '                    'gf1.Sequence.Substring(0, gf1.Sequence.Length - gf1.End_R.Length + 1) + gf2.Sequence)

    '                    'gfr.Iscircular = False
    '                    gfr.Name = "Ligation-RF-" & gf1.Name & " & " & gf2.Name
    '                    gfr.ModificationDate = Date.Now
    '                    gfr.Phos_F = gf1.Phos_F
    '                    gfr.Phos_R = gf2.Phos_R
    '                    gfr.End_F = gf1.End_F
    '                    gfr.End_R = gf2.End_R
    '                    'gfList.Add(gfr)
    '                    imList.Add(gfr)

    '                End If
    '                If gf1.End_R = gf2.End_R And (gf1.Phos_R Or gf2.Phos_R) Then
    '                    gfr = New GeneFile
    '                    If gf1.End_R.Equals("") Then
    '                        gfr.Sequence = gf1.Sequence + gf2.RCSequence
    '                    Else
    '                        gfr.Sequence = gf1.Sequence + gf2.RCSequence.Substring(gf2.End_R.Length - 1, gf2.Sequence.Length - gf2.End_R.Length + 1)
    '                    End If
    '                    'gfr.Sequence = IIf(gf1.End_F.Equals(""), gf1.Sequence + gf2.RCSequence, _
    '                    'gf1.Sequence + gf2.RCSequence.Substring(gf2.End_R.Length - 1, gf2.Sequence.Length - gf2.End_R.Length + 1))

    '                    'gfr.Iscircular = False
    '                    gfr.Name = "Ligation-RR-" & gf1.Name & " & " & gf2.Name
    '                    gfr.ModificationDate = Date.Now
    '                    gfr.Phos_F = gf1.Phos_F
    '                    gfr.Phos_R = gf2.Phos_F
    '                    gfr.End_F = gf1.End_F
    '                    gfr.End_R = gf2.End_F
    '                    'gfList.Add(gfr)
    '                    imList.Add(gfr)

    '                End If
    '            Next
    '        Next
    '        '2 two part self ligation
    '        ReduceDNA(imList)
    '        For Each gf1 In imList
    '            gfList.Add(gf1)
    '        Next
    '        Dim scList As New List(Of GeneFile)
    '        For Each gf1 In imList
    '            If Not gf1.Iscircular And (gf1.End_F = gf1.End_R) And (gf1.Phos_F Or gf1.Phos_R) Then
    '                'check for compitable ends:
    '                gfr = New GeneFile
    '                If gf1.End_F.Equals("") Then
    '                    gfr.Sequence = gf1.Sequence
    '                Else
    '                    gfr.Sequence = gf1.Sequence.Substring(0, gf1.Sequence.Length - gf1.End_F.Length + 1)
    '                End If
    '                ' = IIf(gf1.End_F.Equals(""), gf1.Sequence, gf1.Sequence.Substring(0, gf1.Sequence.Length - gf1.End_F.Length + 1))
    '                'gfr.Iscircular = True
    '                gfr.Name = "CircularLigation-" & gf1.Name
    '                gfr.ModificationDate = Date.Now
    '                gfr.Phos_F = True
    '                gfr.Phos_R = True
    '                gfr.End_F = ""
    '                gfr.End_R = ""
    '                scList.Add(gfr)
    '            End If
    '        Next
    '        ReduceDNA(scList)
    '        For Each gf1 In scList
    '            gfList.Add(gf1)
    '        Next
    '        If ThirdLevel Then

    '            '3 trimer ligation
    '            Dim itList As New List(Of GeneFile)
    '            For Each gf1 In imList
    '                If gf1.Iscircular Then Continue For
    '                For Each gf2 In DNAs
    '                    If gf2.Iscircular Then Continue For
    '                    If gf1.End_F = gf2.End_F And (gf1.Phos_F Or gf2.Phos_F) Then
    '                        gfr = New GeneFile
    '                        If gf1.End_F.Equals("") Then
    '                            gfr.Sequence = gf2.RCSequence + gf1.Sequence
    '                        Else
    '                            gfr.Sequence = gf2.RCSequence.Substring(0, gf2.Sequence.Length - gf2.End_F.Length + 1) + gf1.Sequence
    '                        End If
    '                        'gfr.Sequence = IIf(gf1.End_F.Equals(""), gf2.RCSequence + gf1.Sequence, _
    '                        'gf2.RCSequence.Substring(0, gf2.Sequence.Length - gf2.End_F.Length + 1) + gf1.Sequence)
    '                        'gfr.Iscircular = False
    '                        gfr.Name = "Ligation-FF-" & gf1.Name & " & " & gf2.Name
    '                        gfr.ModificationDate = Date.Now
    '                        gfr.Phos_F = gf2.Phos_R
    '                        gfr.Phos_R = gf1.Phos_R
    '                        gfr.End_F = gf2.End_R
    '                        gfr.End_R = gf1.End_R
    '                        'gfList.Add(gfr)
    '                        imList.Add(gfr)
    '                    End If
    '                    If gf1.End_F = gf2.End_R And (gf1.Phos_F Or gf2.Phos_R) Then
    '                        gfr = New GeneFile
    '                        If gf1.End_F.Equals("") Then
    '                            gfr.Sequence = gf2.Sequence + gf1.Sequence
    '                        Else
    '                            gfr.Sequence = gf2.Sequence.Substring(0, gf2.Sequence.Length - gf2.End_R.Length + 1) + gf1.Sequence
    '                        End If
    '                        'gfr.Sequence = IIf(gf1.End_F.Equals(""), gf2.Sequence + gf1.Sequence, _
    '                        'gf2.Sequence.Substring(0, gf2.Sequence.Length - gf2.End_F.Length + 1) + gf1.Sequence)

    '                        'gfr.Iscircular = False
    '                        gfr.Name = "Ligation-FR-" & gf1.Name & " & " & gf2.Name
    '                        gfr.ModificationDate = Date.Now
    '                        gfr.Phos_F = gf2.Phos_F
    '                        gfr.Phos_R = gf1.Phos_R
    '                        gfr.End_F = gf2.End_F
    '                        gfr.End_R = gf1.End_R
    '                        'gfList.Add(gfr)
    '                        imList.Add(gfr)

    '                    End If
    '                    If gf1.End_R = gf2.End_F And (gf1.Phos_R Or gf2.Phos_F) Then
    '                        gfr = New GeneFile
    '                        If gf1.End_F.Equals("") Then
    '                            gfr.Sequence = gf1.Sequence + gf2.Sequence
    '                        Else
    '                            gfr.Sequence = gf1.Sequence.Substring(0, gf1.Sequence.Length - gf1.End_R.Length + 1) + gf2.Sequence
    '                        End If
    '                        'gfr.Sequence = IIf(gf1.End_F.Equals(""), gf1.Sequence + gf2.Sequence, _
    '                        'gf1.Sequence.Substring(0, gf1.Sequence.Length - gf1.End_R.Length + 1) + gf2.Sequence)

    '                        'gfr.Iscircular = False
    '                        gfr.Name = "Ligation-RF-" & gf1.Name & " & " & gf2.Name
    '                        gfr.ModificationDate = Date.Now
    '                        gfr.Phos_F = gf1.Phos_F
    '                        gfr.Phos_R = gf2.Phos_R
    '                        gfr.End_F = gf1.End_F
    '                        gfr.End_R = gf2.End_R
    '                        'gfList.Add(gfr)
    '                        imList.Add(gfr)

    '                    End If
    '                    If gf1.End_R = gf2.End_R And (gf1.Phos_R Or gf2.Phos_R) Then
    '                        gfr = New GeneFile
    '                        If gf1.End_R.Equals("") Then
    '                            gfr.Sequence = gf1.Sequence + gf2.RCSequence
    '                        Else
    '                            gfr.Sequence = gf1.Sequence + gf2.RCSequence.Substring(gf2.End_R.Length - 1, gf2.Sequence.Length - gf2.End_R.Length + 1)
    '                        End If
    '                        'gfr.Sequence = IIf(gf1.End_F.Equals(""), gf1.Sequence + gf2.RCSequence, _
    '                        'gf1.Sequence + gf2.RCSequence.Substring(gf2.End_R.Length - 1, gf2.Sequence.Length - gf2.End_R.Length + 1))

    '                        'gfr.Iscircular = False
    '                        gfr.Name = "Ligation-RR-" & gf1.Name & " & " & gf2.Name
    '                        gfr.ModificationDate = Date.Now
    '                        gfr.Phos_F = gf1.Phos_F
    '                        gfr.Phos_R = gf2.Phos_F
    '                        gfr.End_F = gf1.End_F
    '                        gfr.End_R = gf2.End_F
    '                        'gfList.Add(gfr)
    '                        imList.Add(gfr)

    '                    End If
    '                Next
    '            Next
    '            ReduceDNA(itList)
    '            Dim icList As New List(Of GeneFile)

    '            For Each gf1 In itList
    '                If Not gf1.Iscircular And (gf1.End_F = gf1.End_R) And (gf1.Phos_F Or gf1.Phos_R) Then
    '                    'check for compitable ends:
    '                    gfr = New GeneFile
    '                    If gf1.End_F.Equals("") Then
    '                        gfr.Sequence = gf1.Sequence
    '                    Else
    '                        gfr.Sequence = gf1.Sequence.Substring(0, gf1.Sequence.Length - gf1.End_F.Length + 1)
    '                    End If
    '                    'gfr.Iscircular = True
    '                    gfr.Name = "CircularLigation-" & gf1.Name
    '                    gfr.ModificationDate = Date.Now
    '                    gfr.Phos_F = True
    '                    gfr.Phos_R = True
    '                    gfr.End_F = ""
    '                    gfr.End_R = ""
    '                    icList.Add(gfr)
    '                End If
    '            Next
    '            ReduceDNA(icList)
    '            For Each gf1 In icList
    '                gfList.Add(gf1)
    '            Next
    '        End If
    '#End If
    '        Return gfList
    '    End Function

    Friend Shared Function MatchLigation(ByVal F As String, ByVal R As String) As Boolean

        If F.Length = R.Length Then
            If F.Length = 0 Then
                Return True
            Else
                Return F.Substring(0, 1) = R.Substring(0, 1) And F.Substring(1, F.Length - 1) = Nuctions.ReverseComplement(R.Substring(1, R.Length - 1))

            End If
        Else
            Return False
        End If

    End Function
    'Friend Shared Function MultipleLinearLigate(ByVal c0List As List(Of GeneFile)) As List(Of GeneFile)
    '    '找出所有可能的连接方法
    '    'Dim MR As New MultipleRecombinator(c0List, New Connector(AddressOf EndLigate), New SelfConnector(AddressOf SelfLigate), True)

    '    'Return MR.GetProducts()

    '    Dim MR As New MultipleVirtualRecombinator(c0List, New PseudoConnector(AddressOf PseudoGeneFile.EndLigate), New PseudoSelfConnector(AddressOf PseudoGeneFile.SelfLigate), True)

    '    Return MR.GetProducts()
    'End Function
    Friend Shared Function MultipleLigate(ByVal c0List As List(Of GeneFile), IsExhaustive As Boolean, Times As Integer) As List(Of GeneFile)
        Dim vResults As New List(Of GeneFile)
        If IsExhaustive Then
            Dim MR As New MultipleVirtualRecombinator(c0List, New PseudoConnector(AddressOf PseudoGeneFile.EndLigate), New PseudoSelfConnector(AddressOf PseudoGeneFile.SelfLigate), True, Times)
            MR.Connect()
            For Each gf In MR.GetProducts()
                Dim gr = gf
                If gr.IsNotRecombinating Then vResults.Add(gr)
            Next
        Else
            Dim MR As New MultipleVirtualReactor(c0List, New PseudoConnector(AddressOf PseudoGeneFile.EndLigate), New PseudoSelfConnector(AddressOf PseudoGeneFile.SelfLigate), True, Times)
            MR.Connect()
            For Each gf In MR.GetProducts()
                Dim gr = gf
                If gr.IsNotRecombinating Then vResults.Add(gr)
            Next
        End If
        Dim rmList As New List(Of GeneFile)

        If vResults.Count > 20 Then
            Dim _Cancel As New CancelRunViewModel() With {.Operation = "Simplifying Products"}
            Dim ConnectingTask As New System.Threading.Tasks.Task(Sub(token As System.Threading.CancellationToken)
                                                                      Try
                                                                          For Each gf1 In vResults
                                                                              If token.IsCancellationRequested Then Exit For
                                                                              If rmList.Contains(gf1) Then Continue For
                                                                              For Each gf2 In vResults
                                                                                  If token.IsCancellationRequested Then Exit For
                                                                                  If gf2 Is gf1 Then Continue For
                                                                                  If rmList.Contains(gf2) Then Continue For
                                                                                  If gf1 = gf2 Then rmList.Add(gf2)
                                                                              Next
                                                                          Next
                                                                          For Each gf In rmList
                                                                              If token.IsCancellationRequested Then Exit For
                                                                              vResults.Remove(gf)
                                                                          Next
                                                                          If token.IsCancellationRequested Then vResults.Clear()
                                                                      Catch ex As Exception

                                                                      End Try
                                                                      _Cancel.Close()
                                                                  End Sub, _Cancel.Token)
            ConnectingTask.Start()
            CancelRunViewModel.ShowCancelRunWindow(_Cancel)
        Else
            For Each gf1 In vResults
                If rmList.Contains(gf1) Then Continue For
                For Each gf2 In vResults
                    If gf2 Is gf1 Then Continue For
                    If rmList.Contains(gf2) Then Continue For
                    If gf1 = gf2 Then rmList.Add(gf2)
                Next
            Next
            For Each gf In rmList
                vResults.Remove(gf)
            Next
        End If
        Return vResults
    End Function
    '之前用于MultipleLinearLigate的代码
    'Exit Function
    ''

    'Dim gList As New List(Of GeneFile)

    'Dim cnt As Integer = c0List.Count

    'If cnt = 0 Then

    'ElseIf cnt = 1 Then
    '    gList.Add(c0List(0))
    'Else
    '    cnt -= 1

    '    Dim gf As GeneFile = c0List(0)
    '    Dim dict As New Dictionary(Of GeneFile, List(Of GeneFile))

    '    Dim nDict As New Dictionary(Of GeneFile, List(Of GeneFile))

    '    c0List.Remove(gf)

    '    dict.Add(gf, c0List)

    '    Dim nGF As GeneFile
    '    Dim nList As List(Of GeneFile)
    '    Dim level As Integer = 0
    '    Dim foundnext As Boolean
    '    Dim sList As New List(Of GeneFile)

    '    While cnt > 0
    '        nDict.Clear()
    '        For Each key As GeneFile In dict.Keys
    '            foundnext = False
    '            For Each gn As GeneFile In dict(key)
    '                nGF = key + gn
    '                If Not (nGF Is Nothing) Then
    '                    nList = New List(Of GeneFile)
    '                    nList.AddRange(dict(key))
    '                    nList.Remove(gn)
    '                    nDict.Add(nGF, nList)
    '                    foundnext = True
    '                End If
    '                nGF = key + gn.RC
    '                If Not (nGF Is Nothing) Then
    '                    nList = New List(Of GeneFile)
    '                    nList.AddRange(dict(key))
    '                    nList.Remove(gn)
    '                    nDict.Add(nGF, nList)
    '                    foundnext = True
    '                End If
    '            Next
    '            If (Not foundnext) And level > 0 Then
    '                sList.Add(key)
    '            End If
    '        Next

    '        dict.Clear()
    '        For Each key As GeneFile In nDict.Keys
    '            dict.Add(key, nDict(key))
    '        Next

    '        cnt -= 1
    '        level += 1
    '    End While
    '    For Each key As GeneFile In dict.Keys
    '        sList.Add(key)
    '    Next

    '    For Each key As GeneFile In sList
    '        nGF = +key
    '        If Not (nGF Is Nothing) Then
    '            nGF.Name = "MLig" + nGF.Length.ToString
    '            gList.Add(nGF)
    '        Else
    '            key.Name = "MLig" + key.Length.ToString
    '            gList.Add(key)
    '        End If
    '    Next

    '    Dim sRList As New List(Of GeneFile)
    '    For Each gfa As GeneFile In gList
    '        sRList.Add(gfa)
    '    Next
    '    gList.Clear()
    '    'Dim sVList As New List(Of GeneFile)
    '    Dim contains As Boolean
    '    For Each gfa As GeneFile In sRList
    '        contains = False
    '        For Each gfx As GeneFile In gList
    '            contains = contains Or (gfx = gfa)
    '        Next
    '        If Not contains Then gList.Add(gfa)
    '    Next
    '    ReduceDNA(gList)
    'End If

    'Return gList
    Friend Class MultipleReactor
        Private vGeneFiles As New List(Of GeneFile)
        Public Connector As Connector
        Public SelfConnector As SelfConnector
        Private _Rounds As Integer
        Public Sub New(ByVal gList As List(Of Nuctions.GeneFile), ByVal vConnector As Connector, ByVal vSelfConnector As SelfConnector, Optional ByVal vBothSide As Boolean = True, Optional Rounds As Integer = 1)
            If Not (gList Is Nothing) Then
                For Each gf As Nuctions.GeneFile In gList
                    vGeneFiles.Add(gf)
                Next
            End If
            'if any two fragments can only ligate two each other, they can be considered as one fragment.
            ReduceComplexity()
            Connector = vConnector
            SelfConnector = vSelfConnector
            _Rounds = Rounds
        End Sub
        Public Sub Connect()
            Dim _Cancel As New CancelRunViewModel() With {.Operation = "Joining DNA Fragments"}
            Dim ConnectingTask As New System.Threading.Tasks.Task(Sub(token As System.Threading.CancellationToken)
                                                                      Try
                                                                          For i As Integer = 1 To _Rounds
                                                                              If token.IsCancellationRequested Then Exit For
                                                                              TryConnectOne()
                                                                              If token.IsCancellationRequested Then Exit For
                                                                              ReduceComplexity()
                                                                              If token.IsCancellationRequested Then Exit For
                                                                              TrySelfConnect()
                                                                              If token.IsCancellationRequested Then Exit For
                                                                              ReduceComplexity()
                                                                              If token.IsCancellationRequested Then Exit For
                                                                          Next
                                                                      Catch ex As Exception

                                                                      End Try
                                                                      If token.IsCancellationRequested Then vGeneFiles.Clear()
                                                                      _Cancel.Close()
                                                                  End Sub, _Cancel.Token)
            ConnectingTask.Start()
            CancelRunViewModel.ShowCancelRunWindow(_Cancel)
        End Sub
        Public Sub ReduceComplexity()
            Dim rmList As New List(Of GeneFile)
            For Each pgP In vGeneFiles
                If rmList.Contains(pgP) Then Continue For
                For Each pgC In vGeneFiles
                    If pgC Is pgP Then Continue For
                    If rmList.Contains(pgC) Then Continue For
                    If pgP = pgC Then
                        rmList.Add(pgC)
                    End If
                Next
            Next
            For Each gf In rmList
                vGeneFiles.Remove(gf)
            Next
        End Sub
        Public Sub TryConnectOne()
            Dim pList As New List(Of GeneFile)
            Dim Result As GeneFile
            For Each pgP In vGeneFiles
                For Each pgC In vGeneFiles
                    Result = Connector.Invoke(pgP, pgC)
                    If Result IsNot Nothing Then pList.Add(Result)
                    Result = Connector.Invoke(pgP, pgC.RC)
                    If Result IsNot Nothing Then pList.Add(Result)
                    Result = Connector.Invoke(pgP.RC, pgC)
                    If Result IsNot Nothing Then pList.Add(Result)
                    Result = Connector.Invoke(pgP.RC, pgC.RC)
                    If Result IsNot Nothing Then pList.Add(Result)
                Next
            Next
            vGeneFiles.AddRange(pList)
        End Sub
        Public Sub TrySelfConnect()
            Dim pList As New List(Of GeneFile)
            For Each pgP In vGeneFiles
                Dim Result = SelfConnector.Invoke(pgP)
                If Result IsNot Nothing Then pList.Add(Result)
            Next
            vGeneFiles.AddRange(pList)
        End Sub
        Public Function GetProducts() As List(Of Nuctions.GeneFile)
            Return vGeneFiles
        End Function
    End Class

    Friend Class MultipleRecombinator
        Private vGeneFiles As New List(Of GeneFile)
        Private vBoxes As New List(Of MultipleRecmbineBox)
        Public Sub New(ByVal gList As List(Of GeneFile), ByVal vConnector As Connector, ByVal vSelfConnector As SelfConnector, Optional ByVal vBothSide As Boolean = True, Optional Rounds As Integer = 1)
            If Not (gList Is Nothing) Then
                For i As Integer = 1 To Rounds
                    For Each gf In gList
                        vGeneFiles.Add(gf)
                    Next
                Next
            End If
            For Each vG As GeneFile In vGeneFiles
                vBoxes.Add(New MultipleRecmbineBox(vG, vGeneFiles, vConnector, vSelfConnector, vBothSide))
            Next

        End Sub
        Public Sub Connect()
            Dim _Cancel As New CancelRunViewModel() With {.Operation = "Joining DNA Fragments"}
            Dim ConnectingTask As New System.Threading.Tasks.Task(Sub(token As System.Threading.CancellationToken)
                                                                      Try
                                                                          For Each box As MultipleRecmbineBox In vBoxes
                                                                              While box.TryConnectOne(token)
                                                                              End While
                                                                              box.TrySelfConnect(token)
                                                                          Next
                                                                      Catch ex As Exception

                                                                      End Try
                                                                      If token.IsCancellationRequested Then vGeneFiles.Clear()
                                                                      _Cancel.Close()
                                                                  End Sub, _Cancel.Token)
            ConnectingTask.Start()
            CancelRunViewModel.ShowCancelRunWindow(_Cancel)
        End Sub
        Public Function GetProducts() As List(Of GeneFile)
            Dim vProducts As New List(Of GeneFile)
            For Each box As MultipleRecmbineBox In vBoxes
                vProducts.AddRange(box.GetProducts)
            Next
            ReduceDNA(vProducts)
            Return vProducts
        End Function
    End Class
    Friend Delegate Function Connector(ByVal vGeneFile1 As GeneFile, ByVal vGeneFile2 As GeneFile) As GeneFile
    Friend Delegate Function SelfConnector(ByVal vGeneFile1 As GeneFile) As GeneFile
    Friend Class MultipleRecmbineBox
        Public Seed As GeneFile
        Public Connector As Connector
        Public SelfConnector As SelfConnector
        Public Products As New List(Of MultipleProductCombination)
        Public Intermediates As New List(Of MultipleProductCombination)
        Public BothSide As Boolean = True
        Public Sub New(ByVal vSeed As GeneFile, ByVal vPool As List(Of GeneFile), ByVal vConnector As Connector, ByVal vSelfConnector As SelfConnector, Optional ByVal vBothSide As Boolean = True)
            Seed = vSeed
            Connector = vConnector
            SelfConnector = vSelfConnector
            Products.Add(New MultipleProductCombination(Seed, Seed, vPool))
        End Sub
        Public Function TryConnectOne(token As System.Threading.CancellationToken) As Boolean
            Intermediates.AddRange(Products)
            Products.Clear()
            Dim vResult As GeneFile = Nothing
            Dim vTotalNewRecombineOccured As Boolean = False
            Dim vIntermediateNewRecombineOccured As Boolean = False
            For Each im As MultipleProductCombination In Intermediates
                If token.IsCancellationRequested Then Exit For
                If im.Working Then
                    im.Working = False
                    vIntermediateNewRecombineOccured = False
                    For Each gf As GeneFile In im.Pool
                        If token.IsCancellationRequested Then Exit For
                        vResult = Connector.Invoke(im.Product, gf)
                        If Not (vResult Is Nothing) Then
                            Products.Add(New MultipleProductCombination(vResult, gf, im.Pool))
                            vIntermediateNewRecombineOccured = True
                        End If
                        vResult = Connector.Invoke(im.Product, gf.RC)
                        If Not (vResult Is Nothing) Then
                            Products.Add(New MultipleProductCombination(vResult, gf, im.Pool))
                            vIntermediateNewRecombineOccured = True
                        End If
                        If BothSide Then
                            vResult = Connector.Invoke(im.Product.RC, gf)
                            If Not (vResult Is Nothing) Then
                                Products.Add(New MultipleProductCombination(vResult, gf, im.Pool))
                                vIntermediateNewRecombineOccured = True
                            End If
                            vResult = Connector.Invoke(im.Product.RC, gf.RC)
                            If Not (vResult Is Nothing) Then
                                Products.Add(New MultipleProductCombination(vResult, gf, im.Pool))
                                vIntermediateNewRecombineOccured = True
                            End If
                        End If
                        vTotalNewRecombineOccured = vTotalNewRecombineOccured Or vIntermediateNewRecombineOccured
                        If token.IsCancellationRequested Then Exit For
                    Next
                    im.Working = im.Working Or vIntermediateNewRecombineOccured
                    If Not im.Working Then Products.Add(im)
                Else
                    Products.Add(im)
                End If
                If token.IsCancellationRequested Then Exit For
            Next
            Intermediates.Clear()
            Return vTotalNewRecombineOccured
        End Function
        Public Function TrySelfConnect(token As System.Threading.CancellationToken) As Boolean
            Intermediates.AddRange(Products)
            Products.Clear()
            Dim vResult As GeneFile = Nothing
            Dim vTotalNewRecombineOccured As Boolean = False
            Dim vIntermediateNewRecombineOccured As Boolean = False
            For Each im As MultipleProductCombination In Intermediates
                If token.IsCancellationRequested Then Exit For
                vIntermediateNewRecombineOccured = False
                vResult = SelfConnector.Invoke(im.Product)
                If Not (vResult Is Nothing) Then
                    Products.Add(New MultipleProductCombination(vResult, im.Pool))
                    vIntermediateNewRecombineOccured = True
                Else
                    Products.Add(im)
                End If
                vTotalNewRecombineOccured = vTotalNewRecombineOccured And vIntermediateNewRecombineOccured
                'If Not vIntermediateNewRecombineOccured Then Products.Add(im)
                If token.IsCancellationRequested Then Exit For
            Next
            Intermediates.Clear()
            Return vTotalNewRecombineOccured
        End Function
        Public Function GetProducts() As List(Of GeneFile)
            Dim vGeneFile As New List(Of GeneFile)
            For Each MPC As MultipleProductCombination In Products
                vGeneFile.Add(MPC.Product)
            Next
            ReduceDNA(vGeneFile)
            Return vGeneFile
        End Function
    End Class
    Friend Class MultipleProductCombination
        Public Product As GeneFile
        Public Pool As New List(Of GeneFile)
        Public Working As Boolean = True
        Public Sub New(ByVal vProduct As GeneFile, ByVal vTarget As GeneFile, ByVal vPreviousPool As List(Of GeneFile))
            Product = vProduct
            Pool.AddRange(vPreviousPool)
            If Pool.Contains(vTarget) Then Pool.Remove(vTarget)
        End Sub
        Public Sub New(ByVal vProduct As GeneFile, ByVal vPreviousPool As List(Of GeneFile))
            Product = vProduct
            Pool.AddRange(vPreviousPool)
        End Sub
    End Class


    Friend Shared Function MultipleLinearEndAnneal(ByVal c0List As List(Of GeneFile), IsExhaustive As Boolean, Times As Integer) As List(Of GeneFile)
        '找出所有可能的重组方法 用于EndAnneal方法的多段连接
        Dim vResults As New List(Of GeneFile)
        If IsExhaustive Then
            Dim MR As New MultipleRecombinator(c0List, New Connector(AddressOf EndAnneal), New SelfConnector(AddressOf SelfEndAnneal), True, Times)
            MR.Connect()
            For Each gf In MR.GetProducts()
                vResults.Add(gf)
            Next
        Else
            Dim MR As New MultipleReactor(c0List, New Connector(AddressOf EndAnneal), New SelfConnector(AddressOf SelfEndAnneal), True, Times)
            MR.Connect()
            For Each gf In MR.GetProducts()
                vResults.Add(gf)
            Next
        End If

        Dim rmList As New List(Of GeneFile)
        If vResults.Count > 20 Then
            Dim _Cancel As New CancelRunViewModel() With {.Operation = "Simplifying Products"}
            Dim ConnectingTask As New System.Threading.Tasks.Task(Sub(token As System.Threading.CancellationToken)
                                                                      Try
                                                                          For Each gf1 In vResults
                                                                              If token.IsCancellationRequested Then Exit For
                                                                              If rmList.Contains(gf1) Then Continue For
                                                                              For Each gf2 In vResults
                                                                                  If token.IsCancellationRequested Then Exit For
                                                                                  If gf2 Is gf1 Then Continue For
                                                                                  If rmList.Contains(gf2) Then Continue For
                                                                                  If gf1 = gf2 Then rmList.Add(gf2)
                                                                              Next
                                                                          Next
                                                                          For Each gf In rmList
                                                                              If token.IsCancellationRequested Then Exit For
                                                                              vResults.Remove(gf)
                                                                          Next
                                                                          If token.IsCancellationRequested Then vResults.Clear()
                                                                      Catch ex As Exception

                                                                      End Try
                                                                      _Cancel.Close()
                                                                  End Sub, _Cancel.Token)
            ConnectingTask.Start()
            CancelRunViewModel.ShowCancelRunWindow(_Cancel)
        Else
            For Each gf1 In vResults
                If rmList.Contains(gf1) Then Continue For
                For Each gf2 In vResults
                    If gf2 Is gf1 Then Continue For
                    If rmList.Contains(gf2) Then Continue For
                    If gf1 = gf2 Then rmList.Add(gf2)
                Next
            Next
            For Each gf In rmList
                vResults.Remove(gf)
            Next
        End If

        Return vResults



        'Dim MR As New MultipleRecombinator(c0List, New Connector(AddressOf EndAnneal), New SelfConnector(AddressOf SelfEndAnneal), True)

        'Return MR.GetProducts()

        'Return New List(Of GeneFile)
    End Function
    Friend Shared Function DegenerateMultipleLinearEndAnneal(ByVal c0List As List(Of GeneFile), IsExhaustive As Boolean, Times As Integer) As List(Of GeneFile)
        '找出所有可能的重组方法 用于EndAnneal方法的多段连接
        Dim vResults As New List(Of GeneFile)
        If IsExhaustive Then
            Dim MR As New MultipleRecombinator(c0List, New Connector(AddressOf DegenerateEndAnneal), New SelfConnector(AddressOf DegenerateSelfEndAnneal), True, Times)
            MR.Connect()
            For Each gf In MR.GetProducts()
                vResults.Add(gf)
            Next
        Else
            Dim MR As New MultipleReactor(c0List, New Connector(AddressOf DegenerateEndAnneal), New SelfConnector(AddressOf DegenerateSelfEndAnneal), True, Times)
            MR.Connect()
            For Each gf In MR.GetProducts()
                vResults.Add(gf)
            Next
        End If

        Dim rmList As New List(Of GeneFile)
        If vResults.Count > 20 Then
            Dim _Cancel As New CancelRunViewModel() With {.Operation = "Simplifying Products"}
            Dim ConnectingTask As New System.Threading.Tasks.Task(Sub(token As System.Threading.CancellationToken)
                                                                      Try
                                                                          For Each gf1 In vResults
                                                                              If token.IsCancellationRequested Then Exit For
                                                                              If rmList.Contains(gf1) Then Continue For
                                                                              For Each gf2 In vResults
                                                                                  If token.IsCancellationRequested Then Exit For
                                                                                  If gf2 Is gf1 Then Continue For
                                                                                  If rmList.Contains(gf2) Then Continue For
                                                                                  If gf1 = gf2 Then rmList.Add(gf2)
                                                                              Next
                                                                          Next
                                                                          For Each gf In rmList
                                                                              If token.IsCancellationRequested Then Exit For
                                                                              vResults.Remove(gf)
                                                                          Next
                                                                          If token.IsCancellationRequested Then vResults.Clear()
                                                                      Catch ex As Exception

                                                                      End Try
                                                                      _Cancel.Close()
                                                                  End Sub, _Cancel.Token)
            ConnectingTask.Start()
            CancelRunViewModel.ShowCancelRunWindow(_Cancel)
        Else
            For Each gf1 In vResults
                If rmList.Contains(gf1) Then Continue For
                For Each gf2 In vResults
                    If gf2 Is gf1 Then Continue For
                    If rmList.Contains(gf2) Then Continue For
                    If gf1 = gf2 Then rmList.Add(gf2)
                Next
            Next
            For Each gf In rmList
                vResults.Remove(gf)
            Next
        End If

        Return vResults


        'Dim MR As New MultipleRecombinator(c0List, New Connector(AddressOf DegenerateEndAnneal), New SelfConnector(AddressOf DegenerateSelfEndAnneal), True)

        'Return MR.GetProducts()

        'Return New List(Of GeneFile)
    End Function

    Friend Shared Function MultipleLinearLigate(ByVal DNAs As Collection) As List(Of GeneFile)
        Dim gfList As New List(Of GeneFile)
#If ReaderMode = 0 Then
        Dim restList As New List(Of GeneFile)

        For Each gf As GeneFile In DNAs
            restList.Add(gf)
        Next

        If restList.Count = 0 Then Return gfList

        Dim currGF As GeneFile = restList(0)
        Dim tarGF As GeneFile = Nothing
        Dim ligationavailable As Boolean = True
        restList.Remove(currGF)

        While restList.Count > 0 And ligationavailable


            For Each gf As GeneFile In restList
                If MatchLigation(gf.End_F, currGF.End_F) And (gf.Phos_F And currGF.Phos_F) Then
                    Dim sb As New System.Text.StringBuilder
                    Dim ngf As New GeneFile
                    sb.Append(gf.RCSequence.Substring(0, gf.RCSequence.Length - gf.End_F.Length + 1))
                    sb.Append(currGF.Sequence)
                    ngf.Sequence = sb.ToString
                    'ngf.Iscircular = False
                    ngf.End_F = gf.End_R
                    ngf.End_R = currGF.End_R
                    currGF = ngf
                    tarGF = gf
                    ligationavailable = True
                    Exit For
                ElseIf MatchLigation(gf.End_R, currGF.End_F) And (gf.Phos_R And currGF.Phos_F) Then
                    Dim sb As New System.Text.StringBuilder
                    Dim ngf As New GeneFile
                    sb.Append(gf.Sequence.Substring(0, gf.Sequence.Length - gf.End_R.Length + 1))
                    sb.Append(currGF.Sequence)
                    ngf.Sequence = sb.ToString
                    'ngf.Iscircular = False
                    ngf.End_F = gf.End_F
                    ngf.End_R = currGF.End_R
                    currGF = ngf
                    tarGF = gf
                    ligationavailable = True
                    Exit For
                ElseIf MatchLigation(gf.End_F, currGF.End_R) And (gf.Phos_F And currGF.Phos_R) Then
                    Dim sb As New System.Text.StringBuilder
                    Dim ngf As New GeneFile
                    sb.Append(currGF.Sequence.Substring(0, currGF.Sequence.Length - currGF.End_R.Length + 1))
                    sb.Append(gf.Sequence)
                    ngf.Sequence = sb.ToString
                    'ngf.Iscircular = False
                    ngf.End_F = currGF.End_F
                    ngf.End_R = gf.End_R
                    currGF = ngf
                    tarGF = gf
                    ligationavailable = True
                    Exit For
                ElseIf MatchLigation(gf.End_R, currGF.End_R) And (gf.Phos_R And currGF.Phos_R) Then
                    Dim sb As New System.Text.StringBuilder
                    Dim ngf As New GeneFile
                    sb.Append(currGF.Sequence.Substring(0, currGF.Sequence.Length - currGF.End_R.Length + 1))
                    sb.Append(gf.RCSequence)
                    ngf.Sequence = sb.ToString
                    'ngf.Iscircular = False
                    ngf.End_F = currGF.End_F
                    ngf.End_R = gf.End_F
                    currGF = ngf
                    tarGF = gf
                    ligationavailable = True
                    Exit For
                Else
                    'no ligation available
                    'exit
                    ligationavailable = False
                End If
            Next
            If ligationavailable And Not (tarGF Is Nothing) Then
                restList.Remove(tarGF)
            End If
        End While
        currGF.Name = "Multiple"
        gfList.Add(currGF)
        Return gfList
#End If
    End Function

    Friend Shared Sub ReduceDNA(ByVal DNAs As List(Of GeneFile))
        Dim gf1 As GeneFile, gf2 As GeneFile
        Dim DuplicateList As New List(Of GeneFile)
        Dim UniqueList As New List(Of GeneFile)

        'Dim gfList As New List(Of GeneFile)
        Dim stb As System.Text.StringBuilder
        Dim dblg1 As String
        For Each gf1 In DNAs
            If DuplicateList.Contains(gf1) Then
                Continue For
            ElseIf UniqueList.Contains(gf1) Then
                Continue For
            Else
                UniqueList.Add(gf1)
                For Each gf2 In DNAs
                    If DuplicateList.Contains(gf2) Then Continue For
                    If gf2 Is gf1 Then Continue For
                    If gf2.Iscircular Xor gf1.Iscircular Then Continue For
                    If gf1.Iscircular Then

                        stb = New System.Text.StringBuilder
                        stb.Append(gf1.Sequence)
                        stb.Append(gf1.Sequence)
                        dblg1 = stb.ToString
                        'both are circular
                        'If gf2.Sequence.Length = gf1.Sequence.Length And ((gf2.Sequence + gf2.Sequence).IndexOf(gf1.Sequence) > -1 Or (gf2.Sequence + gf2.Sequence).IndexOf(gf1.RCSequence) > -1) Then gfList.Add(gf2)
                        If gf2.Sequence.Length = gf1.Sequence.Length AndAlso (dblg1.IndexOf(gf2.Sequence) > -1 Or dblg1.IndexOf(gf2.RCSequence) > -1) Then DuplicateList.Add(gf2)
                    Else
                        'both are not circular
                        If gf2.Sequence.Length = gf1.Sequence.Length Then
                            If gf2.End_F = gf1.End_F AndAlso gf2.End_R = gf1.End_R AndAlso gf2.Sequence = gf1.Sequence Then DuplicateList.Add(gf2)
                            If gf2.End_F = gf1.End_R AndAlso gf2.End_R = gf1.End_F AndAlso gf2.Sequence = gf1.RCSequence Then DuplicateList.Add(gf2)
                        End If
                    End If
                Next
            End If
        Next
        DNAs.Clear()
        DNAs.AddRange(UniqueList)
    End Sub
    Friend Shared Sub ReduceDNA(ByVal DNAs As List(Of GeneFile), Token As System.Threading.CancellationToken)
        Dim gf1 As GeneFile, gf2 As GeneFile
        Dim DuplicateList As New List(Of GeneFile)
        Dim UniqueList As New List(Of GeneFile)

        'Dim gfList As New List(Of GeneFile)
        Dim stb As System.Text.StringBuilder
        Dim dblg1 As String
        For Each gf1 In DNAs
            If Token.IsCancellationRequested Then Exit For
            If DuplicateList.Contains(gf1) Then
                Continue For
            ElseIf UniqueList.Contains(gf1) Then
                Continue For
            Else
                UniqueList.Add(gf1)
                For Each gf2 In DNAs
                    If Token.IsCancellationRequested Then Exit For
                    If DuplicateList.Contains(gf2) Then Continue For
                    If gf2 Is gf1 Then Continue For
                    If gf2.Iscircular Xor gf1.Iscircular Then Continue For
                    If gf1.Iscircular Then

                        stb = New System.Text.StringBuilder
                        stb.Append(gf1.Sequence)
                        stb.Append(gf1.Sequence)
                        dblg1 = stb.ToString
                        'both are circular
                        'If gf2.Sequence.Length = gf1.Sequence.Length And ((gf2.Sequence + gf2.Sequence).IndexOf(gf1.Sequence) > -1 Or (gf2.Sequence + gf2.Sequence).IndexOf(gf1.RCSequence) > -1) Then gfList.Add(gf2)
                        If gf2.Sequence.Length = gf1.Sequence.Length AndAlso (dblg1.IndexOf(gf2.Sequence) > -1 Or dblg1.IndexOf(gf2.RCSequence) > -1) Then DuplicateList.Add(gf2)
                    Else
                        'both are not circular
                        If gf2.Sequence.Length = gf1.Sequence.Length Then
                            If gf2.End_F = gf1.End_F AndAlso gf2.End_R = gf1.End_R AndAlso gf2.Sequence = gf1.Sequence Then DuplicateList.Add(gf2)
                            If gf2.End_F = gf1.End_R AndAlso gf2.End_R = gf1.End_F AndAlso gf2.Sequence = gf1.RCSequence Then DuplicateList.Add(gf2)
                        End If
                    End If
                    If Token.IsCancellationRequested Then Exit For
                Next
            End If
            If Token.IsCancellationRequested Then Exit For
        Next
        If Token.IsCancellationRequested Then Return
        DNAs.Clear()
        DNAs.AddRange(UniqueList)
    End Sub

    Public Structure ExperimentRecord
        Public ID As Integer
        Public ExpDate As Date
        Public InheritID As String
        Public ExpIndex As String
        Public Success As Boolean
        Public Record As String
        Public NextIndex As String
        Public Visible As Boolean
    End Structure
    Public Enum ScreenModeEnum
        Features
        PCR
    End Enum
End Class
