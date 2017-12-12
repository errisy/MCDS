Public Class BinarySerializer
    Public Shared Sub ToFile(ByVal obj As Object, ByVal f As String)
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim fs As New IO.FileStream(f, IO.FileMode.OpenOrCreate)
        bf.Serialize(fs, obj)
        fs.Close()
    End Sub
    Public Shared Function FromFile(ByVal f As String) As Object
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim fs As New IO.FileStream(f, IO.FileMode.OpenOrCreate)
        Dim obj As Object = Nothing
        Try
            obj = bf.Deserialize(fs)
        Catch ex As Exception

        End Try
        fs.Close()
        Return obj
    End Function
    Public Shared Function ToBytes(ByVal obj As Object) As Byte()
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim fs As New IO.MemoryStream()
        bf.Serialize(fs, obj)
        Dim bytes As Byte() = New Byte(fs.Length - 1) {}
        fs.Position = 0
        fs.Read(bytes, 0, fs.Length)
        fs.Close()
        Return bytes
    End Function
    Public Shared Function FromBytes(ByVal bytes As Byte()) As Object
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim fs As New IO.MemoryStream(bytes)
        Dim obj As Object = bf.Deserialize(fs)
        fs.Close()
        Return obj
    End Function
    Public Shared Shadows Function ToString(ByVal obj As Object) As String
        Return Convert.ToBase64String(ToBytes(obj))
    End Function
    Public Shared Function FromString(ByVal str As String) As Object
        Return FromBytes(Convert.FromBase64String(str))
    End Function

End Class
