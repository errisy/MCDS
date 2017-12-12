Public Class SCFHeader
    Public Property magic As String
    Public Property samples As UInt32
    Public Property samples_offset As UInt32
    Public Property bases As UInt32
    Public Property bases_left_clip As UInt32
    Public Property bases_right_clip As UInt32
    Public Property bases_offset As UInt32
    Public Property comment_size As UInt32
    Public Property comment_offset As UInt32
    Public Property version As String
    Public Property sample_size As UInt32
    Public Property code_set As UInt32
    Public Property private_size As UInt32
    Public Property private_offset As UInt32

    Public Sub Initialize(_Stream As System.IO.Stream, Position As Integer)
        Dim Bytes As Byte()
        _Stream.Position = Position
        Bytes = New Byte(3) {}
        _Stream.Read(Bytes, 0, Bytes.Length)
        magic = System.Text.Encoding.ASCII.GetString(Bytes)

        Bytes = New Byte(3) {}
        _Stream.Read(Bytes, 0, Bytes.Length)
        samples = BitConverter.ToUInt32(Bytes.Endian(False), 0)

        Bytes = New Byte(3) {}
        _Stream.Read(Bytes, 0, Bytes.Length)
        samples_offset = BitConverter.ToUInt32(Bytes.Endian(False), 0)

        Bytes = New Byte(3) {}
        _Stream.Read(Bytes, 0, Bytes.Length)
        bases = BitConverter.ToUInt32(Bytes.Endian(False), 0)

        Bytes = New Byte(3) {}
        _Stream.Read(Bytes, 0, Bytes.Length)
        bases_left_clip = BitConverter.ToUInt32(Bytes.Endian(True), 0)

        Bytes = New Byte(3) {}
        _Stream.Read(Bytes, 0, Bytes.Length)
        bases_right_clip = BitConverter.ToUInt32(Bytes.Endian(False), 0)

        Bytes = New Byte(3) {}
        _Stream.Read(Bytes, 0, Bytes.Length)
        bases_offset = BitConverter.ToUInt32(Bytes.Endian(False), 0)

        Bytes = New Byte(3) {}
        _Stream.Read(Bytes, 0, Bytes.Length)
        comment_size = BitConverter.ToUInt32(Bytes.Endian(False), 0)

        Bytes = New Byte(3) {}
        _Stream.Read(Bytes, 0, Bytes.Length)
        comment_offset = BitConverter.ToUInt32(Bytes.Endian(False), 0)

        Bytes = New Byte(3) {}
        _Stream.Read(Bytes, 0, Bytes.Length)
        version = System.Text.Encoding.ASCII.GetString(Bytes)

        Bytes = New Byte(3) {}
        _Stream.Read(Bytes, 0, Bytes.Length)
        sample_size = BitConverter.ToUInt32(Bytes.Endian(False), 0)

        Bytes = New Byte(3) {}
        _Stream.Read(Bytes, 0, Bytes.Length)
        code_set = BitConverter.ToUInt32(Bytes.Endian(False), 0)

        Bytes = New Byte(3) {}
        _Stream.Read(Bytes, 0, Bytes.Length)
        private_size = BitConverter.ToUInt32(Bytes.Endian(False), 0)

        Bytes = New Byte(3) {}
        _Stream.Read(Bytes, 0, Bytes.Length)
        private_offset = BitConverter.ToUInt32(Bytes.Endian(False), 0)



    End Sub
End Class


Public Class SequenceDataBase
    Public Property ATrace As Double()
    Public Property CTrace As Double()
    Public Property GTrace As Double()
    Public Property TTrace As Double()
    Public Property Indices As Integer()
    Public Property Sequence As String
    'Public Property AProb As Byte()
    'Public Property CProb As Byte()
    'Public Property GProb As Byte()
    'Public Property TProb As Byte()
    Public Function GetRawData() As RawSequencingData.RawData
        Dim rd = New RawSequencingData.RawData With {.A = ATrace, .C = CTrace, .G = GTrace, .T = TTrace, .Locations = Indices, .Sequence = Sequence}
        rd.Check()
        Return rd
    End Function
End Class

Namespace RawSequencingData
    <Serializable>
    Public Class RawData
        Public A As Double()
        Public C As Double()
        Public G As Double()
        Public T As Double()
        Public Sequence As String
        Public Locations As Integer()
        ''' <summary>
        ''' Check the consistency of the data and remove errors.
        ''' </summary>
        Public Sub Check()
            Dim minLength As Integer = Integer.MaxValue
            Dim min = Sub(ByRef x As Double())
                          Dim l = x.Length
                          If l < minLength Then minLength = l
                      End Sub
            min(A)
            min(C)
            min(G)
            min(T)
            Dim truncate = Sub(ByRef x As Double())
                               If x.Length > minLength Then x = x.Take(minLength).ToArray
                           End Sub
            truncate(A)
            truncate(C)
            truncate(G)
            truncate(T)

            Dim lastloc As Integer = -1
            Dim iList As New List(Of Integer)
            For Each i In Locations
                If lastloc > i Then
                    iList.Add(lastloc)
                Else
                    iList.Add(i)
                    lastloc = i
                End If
            Next
            If Sequence.Length < iList.Count Then Sequence = Sequence.PadRight(iList.Count, "N"c)
            Locations = iList.ToArray
        End Sub
    End Class
End Namespace



Public Class SCFData
    Inherits SequenceDataBase
    Private Header As New SCFHeader
    Public Sub Read(vStream As IO.Stream)
        Header.Initialize(vStream, 0)
        Select Case Header.version
            Case "3.00"
                Dim bytes As Byte()

                bytes = New Byte(1) {}
                vStream.Position = Header.samples_offset
                Dim c As Integer
                Dim y As Int16

                c = Header.samples
                Dim aList As New List(Of Int16)
                For i = 1 To c
                    vStream.Read(bytes, 0, bytes.Length)
                    y = BitConverter.ToInt16(bytes.Reverse.ToArray, 0)
                    aList.Add(y)
                    'APoints.Add(New Point(i, y))
                Next
                ATrace = aList.Select(Of Double)(Function(s) -s / 256.0#)

                Dim cList As New List(Of Int16)
                For i = 1 To c
                    vStream.Read(bytes, 0, bytes.Length)
                    y = BitConverter.ToInt16(bytes.Reverse.ToArray, 0)
                    cList.Add(y)
                    'CPoints.Add(New Point(i, y))
                Next
                CTrace = cList.Select(Of Double)(Function(s) -s / 256.0#)

                Dim gList As New List(Of Int16)
                For i = 1 To c
                    vStream.Read(bytes, 0, bytes.Length)
                    y = BitConverter.ToInt16(bytes.Reverse.ToArray, 0)
                    gList.Add(y)
                    'GPoints.Add(New Point(i, y))
                Next
                GTrace = gList.Select(Of Double)(Function(s) -s / 256.0#)

                Dim tList As New List(Of Int16)
                For i = 1 To c
                    vStream.Read(bytes, 0, bytes.Length)
                    y = BitConverter.ToInt16(bytes.Reverse.ToArray, 0)
                    tList.Add(y)
                    'TPoints.Add(New Point(i, y))
                Next
                TTrace = tList.Select(Of Double)(Function(s) -s / 256.0#)

                vStream.Position = Header.bases_offset
                bytes = New Byte(3) {}

                c = Header.bases
                Dim iList As New List(Of UInt32)
                While c > 0
                    vStream.Read(bytes, 0, bytes.Length)
                    iList.Add(BitConverter.ToUInt32(bytes.Reverse.ToArray, 0))
                    c -= 1
                End While
                Indices = iList.Select(Of Integer)(Function(s) s).ToArray

                bytes = New Byte(0) {}

                c = Header.bases
                Dim apList As New List(Of Byte)
                While c > 0
                    vStream.Read(bytes, 0, bytes.Length)
                    apList.Add(bytes(0))
                    c -= 1
                End While


                c = Header.bases
                Dim cpList As New List(Of Byte)
                While c > 0
                    vStream.Read(bytes, 0, bytes.Length)
                    cpList.Add(bytes(0))
                    c -= 1
                End While


                c = Header.bases
                Dim gpList As New List(Of Byte)
                While c > 0
                    vStream.Read(bytes, 0, bytes.Length)
                    gpList.Add(bytes(0))
                    c -= 1
                End While


                c = Header.bases
                Dim tpList As New List(Of Byte)
                While c > 0
                    vStream.Read(bytes, 0, bytes.Length)
                    tpList.Add(bytes(0))
                    c -= 1
                End While


                Dim stb As New System.Text.StringBuilder
                For i As Integer = 0 To Math.Min(Math.Min(apList.Count, cpList.Count), Math.Min(gpList.Count, tpList.Count)) - 1
                    Dim aa = apList(i)
                    Dim cc = cpList(i)
                    Dim gg = gpList(i)
                    Dim tt = tpList(i)
                    Dim max = Math.Max(Math.Max(aa, cc), Math.Max(gg, tt))
                    If aa = max Then stb.Append("A"c) : Continue For
                    If cc = max Then stb.Append("C"c) : Continue For
                    If gg = max Then stb.Append("G"c) : Continue For
                    If tt = max Then stb.Append("T"c) : Continue For
                Next
                Sequence = stb.ToString
        End Select

    End Sub

    'Public Property APoints As New List(Of Point)
    'Public Property CPoints As New List(Of Point)
    'Public Property GPoints As New List(Of Point)
    'Public Property TPoints As New List(Of Point)
End Class

Public Module HexFunction
    <System.Runtime.CompilerServices.Extension> Public Function Endian(bytes As Byte(), IsLittleEndian As Boolean) As Byte()
        If IsLittleEndian Then
            Return bytes
        Else
            Return bytes.Reverse.ToArray
        End If
    End Function
End Module