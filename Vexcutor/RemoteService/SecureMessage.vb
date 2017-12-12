Imports System.Security.Cryptography
Imports System.Text

Public Class SecureMessage
    Private ByteConverter As New UnicodeEncoding()
    Private MD5Hasher As MD5 = MD5.Create()
    Public Function MD5Hash(ByVal msg As String) As String
        Dim bytes As Byte() = ByteConverter.GetBytes(msg)
        Dim signature As Byte() = MD5Hasher.ComputeHash(bytes)
        Return Convert.ToBase64String(signature)
    End Function
    Public Function DoubleHasher(ByVal msg As String) As String
        Return MD5Hash(MD5Hash(msg).Replace("=", "")).Replace("=", "")
    End Function
    Public Function Sign(ByVal msg As String) As String
        Dim bytes As Byte() = ByteConverter.GetBytes(msg)
        Dim signature As Byte() = MD5Hasher.ComputeHash(bytes)
        Return Convert.ToBase64String(signature)
    End Function
    Public Function Base64AndSign(ByVal msg As String) As String
        '返回Base64编码的信息和签名
        Dim stb As New StringBuilder
        Dim bytes As Byte() = ByteConverter.GetBytes(msg)
        stb.Append(Convert.ToBase64String(bytes))
        stb.Append(">")
        Dim signature As Byte() = MD5Hasher.ComputeHash(bytes)
        stb.Append(Convert.ToBase64String(signature))
        Return stb.ToString
    End Function
    Private AESALG As Aes
    Private DEAES As ICryptoTransform
    Private ENAES As ICryptoTransform
    Public Sub New()
        AESALG = Aes.Create
        AESALG.Key = Convert.FromBase64String("aPL8akp+rZATfx5Z8xmFguOdQgfvr/7lS6gqcW3yx4c=")
        AESALG.IV = Convert.FromBase64String("e8O6kxGt+NOTNKHgIUXpTw==")
        'DEAES = AESALG.CreateDecryptor
        'ENAES = AESALG.CreateEncryptor
    End Sub
    Public Function EncrytAndSign(ByVal msg As String) As String
        Dim stb As New StringBuilder
        stb.Append(">")
        Dim bytes As Byte() = ByteConverter.GetBytes(msg)
        ENAES = AESALG.CreateEncryptor
        Dim AESCodes As Byte() = ENAES.TransformFinalBlock(bytes, 0, bytes.Length)
        stb.Append(Convert.ToBase64String(AESCodes))
        stb.Append(">")
        Dim signature As Byte() = MD5Hasher.ComputeHash(bytes)
        stb.Append(Convert.ToBase64String(signature))
        Return stb.ToString
        Return stb.ToString
    End Function
    Public Function Decode(ByVal msg As String) As String
        If msg Is Nothing Then Return Nothing
        If msg.StartsWith(">") Then
            '这个是加密的数据
            Dim codes As String() = msg.Split(New Char() {">"}, StringSplitOptions.RemoveEmptyEntries)
            Dim data As String = codes(0)
            Dim signature As String = codes(1)

            Dim stb As String
            Dim AESCodes As Byte() = Convert.FromBase64String(data)
            DEAES = AESALG.CreateDecryptor
            Dim bytes As Byte() = DEAES.TransformFinalBlock(AESCodes, 0, AESCodes.Length)
            stb = ByteConverter.GetString(bytes)
            If signature = Convert.ToBase64String(MD5Hasher.ComputeHash(bytes)) Then
                Return stb
            Else
                Return Nothing
            End If
        Else
            '这个是未加密的数据
            Dim codes As String() = msg.Split(New Char() {">"}, StringSplitOptions.RemoveEmptyEntries)
            Dim data As String = codes(0)
            Dim signature As String = codes(1)

            Dim stb As String
            Dim bytes As Byte() = Convert.FromBase64String(data)
            stb = ByteConverter.GetString(bytes)
            If signature = Convert.ToBase64String(MD5Hasher.ComputeHash(bytes)) Then
                Return stb
            Else
                Return Nothing
            End If
        End If
    End Function
    Public Function SSLEncode(ByVal msg As String) As Byte()
        Dim bytes As Byte() = ByteConverter.GetBytes(msg)
        ENAES = AESALG.CreateEncryptor
        Return ENAES.TransformFinalBlock(bytes, 0, bytes.Length)
    End Function
    Public Function SSLDecode(ByVal bytes As Byte()) As String
        DEAES = AESALG.CreateDecryptor
        Return ByteConverter.GetString(DEAES.TransformFinalBlock(bytes, 0, bytes.Length))
    End Function
    Public Function UTF8Encode(ByVal msg As String) As Byte()
        Return ByteConverter.GetBytes(msg)
    End Function
    Public Function UTF8Decode(ByVal bytes As Byte()) As String
        Return ByteConverter.GetString(bytes)
    End Function

    Public Function SSLEN(ByVal data As Byte()) As Byte()
        ENAES = AESALG.CreateEncryptor
        Return ENAES.TransformFinalBlock(data, 0, data.Length)
    End Function
    Public Function SSLDE(ByVal data As Byte()) As Byte()
        DEAES = AESALG.CreateDecryptor
        Return DEAES.TransformFinalBlock(data, 0, data.Length)
    End Function
    Public Function ZIP(ByVal bytes As Byte()) As Byte()
        Dim ms As New IO.MemoryStream
        Dim gz As New IO.Compression.GZipStream(ms, IO.Compression.CompressionMode.Compress)
        gz.Write(bytes, 0, bytes.Length)
        Return ms.GetBuffer
    End Function
    Public Function UNZIP(ByVal bytes As Byte()) As Byte()
        Dim ms As New IO.MemoryStream(bytes)
        Dim gz As New IO.Compression.GZipStream(ms, IO.Compression.CompressionMode.Decompress)
        Dim buffer As Byte() = New Byte(4095) {}
        Dim lenth As Integer
        Dim output As New IO.MemoryStream
        While True
            lenth = gz.Read(buffer, 0, buffer.Length)
            If lenth = 0 Then Exit While
            output.Write(buffer, 0, lenth)
        End While
        Return output.GetBuffer
    End Function
End Class
