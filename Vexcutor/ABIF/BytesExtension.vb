
Public Module BytesExtension
    <System.Runtime.CompilerServices.Extension> Public Function Search(bytes As Byte(), pattern As Byte(), Optional start As Integer = 0) As Integer
        If pattern Is Nothing Then Return -1

        Dim l As Integer = pattern.Length

        If l = 0 Then Return -1

        For i As Integer = start To bytes.Length - l
            Dim match As Boolean = True
            For j As Integer = 0 To l - 1
                If bytes(j + i) <> pattern(j) Then match = False : Exit For
            Next
            If match Then Return i
        Next

        Return -1
    End Function

    <System.Runtime.CompilerServices.Extension> Public Function ToASCIIString(bytes As Byte()) As String
        Return System.Text.Encoding.ASCII.GetString(bytes)
    End Function

    <System.Runtime.CompilerServices.Extension> Public Function ToASCIIBytes(value As String) As Byte()
        Return System.Text.Encoding.ASCII.GetBytes(value)
    End Function


    <System.Runtime.CompilerServices.Extension> Public Iterator Function Search(bytes As Byte(), value As String) As IEnumerable(Of Integer)

        'Dim res As New List(Of Integer)

        Dim pattern As Byte() = value.ToASCIIBytes

        Dim l As Integer = pattern.Length

        For i As Integer = 0 To bytes.Length - l
            Dim match As Boolean = True
            For j As Integer = 0 To l - 1
                If bytes(j + i) <> pattern(j) Then match = False : Exit For
            Next
            If match Then Yield i
        Next

    End Function

    <System.Runtime.CompilerServices.Extension> Public Iterator Function Search(bytes As Byte(), value As Int16) As IEnumerable(Of Integer)

        'Dim res As New List(Of Integer)

        Dim pattern As Byte() = BitConverter.GetBytes(value)

        Dim l As Integer = pattern.Length

        For i As Integer = 0 To bytes.Length - l
            Dim match As Boolean = True
            For j As Integer = 0 To l - 1
                If bytes(j + i) <> pattern(j) Then match = False : Exit For
            Next
            If match Then Yield i
        Next

    End Function
    <System.Runtime.CompilerServices.Extension> Public Iterator Function Search(bytes As Byte(), value As Int32) As IEnumerable(Of Integer)

        'Dim res As New List(Of Integer)

        Dim pattern As Byte() = BitConverter.GetBytes(value)

        Dim l As Integer = pattern.Length

        For i As Integer = 0 To bytes.Length - l
            Dim match As Boolean = True
            For j As Integer = 0 To l - 1
                If bytes(j + i) <> pattern(j) Then match = False : Exit For
            Next
            If match Then Yield i
        Next
    End Function

    <System.Runtime.CompilerServices.Extension> Public Iterator Function SearchInteger(bytes As Byte(), [From] As Int32, [To] As Int32) As IEnumerable(Of Integer)
        For i As Integer = 0 To bytes.Length - 4
            Dim value = BitConverter.ToInt32(bytes, i)
            If value >= [From] And value <= [To] Then Yield i
        Next
    End Function

    <System.Runtime.CompilerServices.Extension> Public Iterator Function Search(bytes As Byte(), value As Int64) As IEnumerable(Of Integer)

        'Dim res As New List(Of Integer)

        Dim pattern As Byte() = BitConverter.GetBytes(value)

        Dim l As Integer = pattern.Length

        For i As Integer = 0 To bytes.Length - l
            Dim match As Boolean = True
            For j As Integer = 0 To l - 1
                If bytes(j + i) <> pattern(j) Then match = False : Exit For
            Next
            If match Then Yield i
        Next
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function ReadInt32LE(bytes As Byte(), offset As Integer) As Int32
        Return BitConverter.ToInt32(bytes, offset)
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function ReadInt32BE(bytes As Byte(), offset As Integer) As Int32
        Return BitConverter.ToInt32(New Byte() {bytes(offset + 3), bytes(offset + 2), bytes(offset + 1), bytes(offset)}, 0)
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function TakeInt32LE(bytes As Byte(), ByRef offset As Integer) As Int32
        Dim value = BitConverter.ToInt32(bytes, offset)
        offset += 4
        Return value
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function TakeInt32BE(bytes As Byte(), ByRef offset As Integer) As Int32
        Dim value = BitConverter.ToInt32(New Byte() {bytes(offset + 3), bytes(offset + 2), bytes(offset + 1), bytes(offset)}, 0)
        offset += 4
        Return value
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function ReadInt16LE(bytes As Byte(), offset As Integer) As Int16
        Return BitConverter.ToInt16(bytes, offset)
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function ReadInt16BE(bytes As Byte(), offset As Integer) As Int16
        Return BitConverter.ToInt16(New Byte() {bytes(offset + 1), bytes(offset)}, 0)
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function TakeInt16LE(bytes As Byte(), ByRef offset As Integer) As Int16
        Dim value = BitConverter.ToInt16(bytes, offset)
        offset += 2
        Return value
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function TakeInt16BE(bytes As Byte(), ByRef offset As Integer) As Int16
        Dim value = BitConverter.ToInt16(New Byte() {bytes(offset + 1), bytes(offset)}, 0)
        offset += 2
        Return value
    End Function

    <System.Runtime.CompilerServices.Extension> Public Function ReadASCIIString(bytes As Byte(), offset As Integer, length As Integer) As String
        Return bytes.Skip(offset).Take(length).ToArray.ToASCIIString
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function TakeASCIIString(bytes As Byte(), ByRef offset As Integer, length As Integer) As String
        Dim value = bytes.Skip(offset).Take(length).ToArray.ToASCIIString
        offset += length
        Return value
    End Function

End Module
