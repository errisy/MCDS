Public Class AB1Data
    Inherits SequenceDataBase
    Public Sub Read(bytes As Byte())

        Dim res = bytes.Search("AEPt")

        Dim AB1Directories As New Dictionary(Of Integer, AB1Directory)

        Dim r = res.ToArray

        Dim length As Integer = bytes.Length

        For Each index In r
            Dim offset = index

            Dim ok As Boolean = True

            While ok And offset < length - 28
                Dim current As Integer = offset
                If Not AB1Directories.ContainsKey(current) Then
                    Dim d = ReadDirecotry(bytes, offset)
                    If d Is Nothing Then
                        ok = False
                    Else
                        AB1Directories.Add(current, d)
                    End If
                Else
                    ok = False
                End If
            End While
        Next

        Dim texts = AB1Directories.Values.Where(Function(d) d.Value IsNot Nothing AndAlso d.Value.Length > 300).ToList
        '"PBAS"
        Dim PBASs = AB1Directories.Values.Where(Function(d) d.Name = "PBAS").ToList

        Dim pl = PBASs(0).Value.Length

        Dim DATAs = AB1Directories.Values.Where(Function(d) d.Name = "DATA").ToList

        'Dim trim = AB1Directories.Values.Where(Function(d) d.Shorts.Count = pl Or d.Integers.Count = pl Or d.Floats.Count = pl Or d.Doubles.Count = pl).ToList

        Dim PLOCs = AB1Directories.Values.Where(Function(d) d.Name = "PLOC").ToList

        '5 + (h - 10) * (1 + u / 256)
        Dim factor As Double = 0.000390625#
        MyBase.GTrace = DATAs(8).Shorts.Select(Of Double)(Function(s As Short) s * factor).ToArray
        MyBase.ATrace = DATAs(9).Shorts.Select(Of Double)(Function(s As Short) s * factor).ToArray
        MyBase.TTrace = DATAs(10).Shorts.Select(Of Double)(Function(s As Short) s * factor).ToArray
        MyBase.CTrace = DATAs(11).Shorts.Select(Of Double)(Function(s As Short) s * factor).ToArray
        MyBase.Indices = PLOCs(0).Shorts.Select(Of Integer)(Function(s As Short) CUInt(s)).ToArray
        MyBase.Sequence = PBASs(0).Value
    End Sub
    Public Function ReadDirecotry(bytes As Byte(), ByRef offset As Integer) As AB1Directory
        Dim d As New AB1Directory
        d.Name = bytes.TakeASCIIString(offset, 4)
        d.Tag = bytes.TakeInt32BE(offset)
        d.ElementType = bytes.TakeInt16BE(offset)
        d.ElementSize = bytes.TakeInt16BE(offset)
        d.NumberOfElements = bytes.TakeInt32BE(offset)
        d.ItemSize = bytes.TakeInt32BE(offset)
        d.ItemOffset = bytes.TakeInt32BE(offset)
        d.DataHandle = bytes.TakeInt32BE(offset)
        If d.DataHandle <> 0 Then Return Nothing
        d.Read(bytes)
        Return d
    End Function



    Public Extension As String
    Public Version As String
    Public BaseDirEntry As New AB1DirEntry
    Public AB1Directories As New List(Of AB1Directory)

End Class

Public Class AB1DirEntry
    Public Name As String
    Public Number As Int32
    Public ElementType As Int16
    Public ElementSize As Int16
    Public NumElement As Int32
    Public DataSize As Int32
    Public DataOffset As Int32
    Public DataHandle As Int32
End Class

Public Class AB1Directory
    Public Name As String
    Public Tag As Int32
    Public ElementType As Int16
    Public ElementSize As Int16
    Public NumberOfElements As Int32
    Public ItemSize As Int32
    Public ItemOffset As Int32
    Public DataHandle As Int32

    Public Longs As New List(Of Int64)
    Public Integers As New List(Of Int32)
    Public Doubles As New List(Of Double)
    Public Floats As New List(Of Single)
    Public Shorts As New List(Of Int16)
    Public Bytes As New List(Of Byte)
    Public Value As String

    Public Sub Read(data As Byte())
        Select Case ElementType
            Case 1
                If NumberOfElements > 4 Then
                    For i As Integer = ItemOffset To ItemOffset + NumberOfElements - 1 Step 1
                        Bytes.Add(data(i))
                    Next
                Else
                    Bytes.AddRange(BitConverter.GetBytes(ItemOffset).Reverse.Take(NumberOfElements))
                End If
            Case 2
                If NumberOfElements > 4 Then
                    Value = data.Skip(ItemOffset).Take(NumberOfElements).ToArray.ToASCIIString
                Else
                    Value = BitConverter.GetBytes(ItemOffset).Reverse.Take(NumberOfElements).ToArray.ToASCIIString
                End If
            Case 3
            Case 4
                If NumberOfElements > 2 Then
                    For i As Integer = ItemOffset To ItemOffset + NumberOfElements * 2 - 2 Step 2
                        Shorts.Add(data.ReadInt16BE(i))
                    Next
                Else

                End If
            Case 5
                If NumberOfElements > 1 Then
                    For i As Integer = ItemOffset To ItemOffset + NumberOfElements * 4 - 4 Step 4
                        Integers.Add(data.ReadInt32BE(i))
                    Next
                End If
            Case 18
                If NumberOfElements > 4 Then
                    Value = data.ReadASCIIString(ItemOffset, NumberOfElements)
                End If
            Case 19
                If NumberOfElements > 4 Then
                    Value = data.ReadASCIIString(ItemOffset, NumberOfElements)
                End If
        End Select
    End Sub

    Public ReadOnly Property Description As String
        Get
            Select Case Name
                Case "APFN"
                    Return "Sequencing Analysis parameteres file name"
                Case "APXV"
                    Return "Analysis Protocol XML schema version"
                Case "APrN"
                    Return "Analysis Protocol settings name"
                Case "APrV"
                    Return "Analysis Protocol settings version"
                Case "APrX"
                    Return "Analysis Protocol XML string"
                Case "CMNT"
                    Return "Comment about sample (optional)"
                Case "CTID"
                    Return "Contianer identifier, aka. plate barcode"
                Case "CTNM"
                    Return "Contianer name (usually identical to CTID)"
                Case "CTTL"
                    Return "Comment title"
                Case "CpEP"
                    Return "Is Capillary Machine?"
                Case "DATA"
                    Return "Channel raw data"
                Case "DSam"
                    Return "Downsampling factor"
                Case "DySN"
                    Return "Dye set name"
                Case "Dye#"
                    Return "Number of dyes"
                Case "DyeN"
                    Return "Dye name"
                Case "DyeW"
                    Return "Dye wavelength"
                Case "EPVt"
                    Return "Electrophoresis voltage setting (volts)"
                Case "EPNT"
                    Return "Start Run event"
                Case "FWO_"
                    Return "Base order"
                Case "GTyp"
                    Return "Gel type description"
                Case "InSc"
                    Return "Injection time (seconds)"
                Case "InVt"
                    Return "Injection voltage (volts)"
                Case "LANE"
                    Return "Lane/Capillary"
                Case "LIMS"
                    Return "Sample tracking ID"
                Case "LNTD"
                    Return "Length to detector"
                Case "LsrP"
                    Return "Laser Power Setting (micro Watts)"
                Case "MCHN"
                    Return "Instrument name and serial number"
                Case "MODF"
                    Return "Data collection module file"
                Case "MODL"
                    Return "Model number"
                Case "NAVG"
                    Return "Pixels averaged per lane"
                Case "NLNE"
                    Return "Number of capillaries"
                Case "OfSc"
                    Return "List of scans that are marked off scale in Collection. (optional)"
                Case "Ovrl"
                    Return "One value for each dye. List of scan number indices for scans with color data values > 32767. Values cannot be greater than 32000. (optional)"
                Case "OvrV"
                    Return "One value for each dye. List of color data values for the locations listed in the Ovrl tag. Number of OvrV tags must be equal to the number of Ovrl tags. (optional)"
                Case "PDMF"
                    Return "Mobility file (orig)"
                Case "PXLB"
                    Return "Pixel bin size"
                Case "RGCm"
                    Return "Results group comment (optional)"
                Case "RGNm"
                    Return "Results group name"
                Case "RMXV"
                    Return "Run Module XML schema version"
                Case "RMdN"
                    Return "Run Module name (same as MODF)"
                Case "RMdV"
                    Return "Run Module version"
                Case "RMdX"
                    Return "Run Module XML string"
                Case "PBAS"
                    Return "Sequence Result"
            End Select
        End Get
    End Property

    Public ReadOnly Property TypeName As String
        Get
            Select Case ElementType
                Case 1
                    Return "byte"
                Case 2
                    Return "char"
                Case 3
                    Return "word"
                Case 4
                    Return "short"
                Case 5
                    Return "long"
                Case 7
                    Return "float"
                Case 8
                    Return "double"
                Case 10
                    Return "date"
                Case 11
                    Return "time"
                Case 18
                    Return "pString"
                Case 19
                    Return "cString"
                Case 12
                    Return "thumb"
                Case 13
                    Return "bool"
                Case 6
                    Return "rational"
                Case 9
                    Return "BCD"
                Case 14
                    Return "point"
                Case 15
                    Return "rect"
                Case 16
                    Return "vPoint"
                Case 17
                    Return "vRect"
                Case 20
                    Return "Tag"
                Case 128
                    Return "deltaComp"
                Case 256
                    Return "LZWComp"
                Case 384
                    Return "deltaLZW"
                Case 42
                    Return "FooS"
            End Select
        End Get
    End Property

End Class

Public Module BinaryReaderExtension
    <System.Runtime.CompilerServices.Extension> Public Function ReadInt16BigEndian(reader As IO.BinaryReader) As Int16
        Return BitConverter.ToInt16(reader.ReadBytes(2).Reverse.ToArray, 0)
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function ReadInt32BigEndian(reader As IO.BinaryReader) As Int16
        Return BitConverter.ToInt16(reader.ReadBytes(4).Reverse.ToArray, 0)
    End Function

End Module

