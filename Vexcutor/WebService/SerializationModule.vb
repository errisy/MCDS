Module SerializationExtension
    Public Function SerializeToFile(filename As String, obj As Object) As Boolean
        'Dim data As Byte()
        'Using ms As New IO.MemoryStream
        '    Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        '    bf.Serialize(ms, obj)
        '    data = ms.ToArray
        'End Using
        'IO.File.WriteAllBytes(filename, data)
        Using fs As New System.IO.FileStream(filename, IO.FileMode.OpenOrCreate)
            Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            bf.Serialize(fs, obj)
        End Using
        Return True
    End Function
    Public Function DeserializeFile(filename As String) As Object
        If Not IO.File.Exists(filename) Then Return Nothing

        'Dim data As Byte() = IO.File.ReadAllBytes(filename)
        'Using ms As New IO.MemoryStream(data)
        '    Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        '    Return bf.Deserialize(ms)
        'End Using 

        Using fs As New IO.FileStream(filename, IO.FileMode.Open, IO.FileAccess.Read)
            Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            Return bf.Deserialize(fs)
        End Using
    End Function

    Public Function GetLocalDirectory(Name As String) As String
        If Not IO.Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\" + Name) Then IO.Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + "\" + Name)
        Return System.AppDomain.CurrentDomain.BaseDirectory + "\" + Name + "\"
    End Function
End Module
