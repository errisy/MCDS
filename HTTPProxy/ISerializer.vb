Public Interface ISerializer
    Function Serialize(Value As Object) As Byte()
    Function Deserialize(Bytes As Byte()) As Object
    Function Deserialize(Of T)(Bytes As Byte()) As T
End Interface

Public Class BinarySerializer
    Implements ISerializer
    Public Function Deserialize(Bytes() As Byte) As Object Implements ISerializer.Deserialize
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Using mo As New System.IO.MemoryStream(Bytes)
            Return bf.Deserialize(mo)
        End Using
    End Function
    Public Function DeserializeType(Of T)(Bytes() As Byte) As T Implements ISerializer.Deserialize
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Using mo As New System.IO.MemoryStream(Bytes)
            Return bf.Deserialize(mo)
        End Using
    End Function
    Public Function Serialize(Value As Object) As Byte() Implements ISerializer.Serialize
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Using mo As New System.IO.MemoryStream()
            bf.Serialize(mo, Value)
            Return mo.ToArray
        End Using
    End Function
End Class

Public Class GZipBinarySerializer
    Implements ISerializer
    Public Function Deserialize(Bytes() As Byte) As Object Implements ISerializer.Deserialize
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Using mo As New System.IO.MemoryStream(Bytes)
            Using gz As New System.IO.Compression.GZipStream(mo, IO.Compression.CompressionMode.Decompress)
                Return bf.Deserialize(gz)
            End Using
        End Using
    End Function
    Public Function DeserializeType(Of T)(Bytes() As Byte) As T Implements ISerializer.Deserialize
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Using mo As New System.IO.MemoryStream(Bytes)
            Using gz As New System.IO.Compression.GZipStream(mo, IO.Compression.CompressionMode.Decompress)
                Return bf.Deserialize(gz)
            End Using
        End Using
    End Function
    Public Function Serialize(Value As Object) As Byte() Implements ISerializer.Serialize
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Using mo As New System.IO.MemoryStream()
            Using gz As New System.IO.Compression.GZipStream(mo, IO.Compression.CompressionMode.Compress)
                bf.Serialize(gz, Value)
            End Using
            Return mo.ToArray
        End Using
    End Function
End Class