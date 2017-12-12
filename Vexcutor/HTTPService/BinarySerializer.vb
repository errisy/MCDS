Public Class BinarySerializer
    Implements ISerializer
    Public Function Deserialize(Bytes() As Byte) As Object Implements ISerializer.Deserialize
        Dim BinaryFormatter As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim obj As Object = Nothing
        Using ms As New System.IO.MemoryStream(Bytes)
            obj = BinaryFormatter.Deserialize(ms)
        End Using
        Return obj
    End Function
    Public Function Deserialize1(Of T)(Bytes() As Byte) As T Implements ISerializer.Deserialize
        Dim BinaryFormatter As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim obj As Object = Nothing
        Using ms As New System.IO.MemoryStream(Bytes)
            obj = BinaryFormatter.Deserialize(ms)
        End Using
        Return obj
    End Function
    Public Function Serialize(Value As Object) As Byte() Implements ISerializer.Serialize
        Dim BinaryFormatter As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim obj As Object = Nothing
        Dim bytes As Byte()
        Using ms As New System.IO.MemoryStream()
            BinaryFormatter.Serialize(ms, Value)
            bytes = ms.ToArray
        End Using
        Return bytes
    End Function
End Class

Public Class ZipBinarySerializer
    Implements ISerializer
    Public Function Deserialize(Bytes() As Byte) As Object Implements ISerializer.Deserialize
        Dim BinaryFormatter As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim obj As Object = Nothing
        Using ms As New System.IO.MemoryStream(Bytes)
            Using gz As New System.IO.Compression.GZipStream(ms, IO.Compression.CompressionMode.Decompress)
                obj = BinaryFormatter.Deserialize(gz)
            End Using
        End Using
        Return obj
    End Function
    Public Function Deserialize1(Of T)(Bytes() As Byte) As T Implements ISerializer.Deserialize
        Dim BinaryFormatter As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim obj As Object = Nothing
        Using ms As New System.IO.MemoryStream(Bytes)
            Using gz As New System.IO.Compression.GZipStream(ms, IO.Compression.CompressionMode.Decompress)
                obj = BinaryFormatter.Deserialize(gz)
            End Using
        End Using
        Return obj
    End Function
    Public Function Serialize(Value As Object) As Byte() Implements ISerializer.Serialize
        Dim BinaryFormatter As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim obj As Object = Nothing
        Dim bytes As Byte()
        Using ms As New System.IO.MemoryStream()
            Using gz As New System.IO.Compression.GZipStream(ms, IO.Compression.CompressionMode.Compress)
                BinaryFormatter.Serialize(gz, Value)
                bytes = ms.ToArray
            End Using
        End Using
        Return bytes
    End Function
End Class