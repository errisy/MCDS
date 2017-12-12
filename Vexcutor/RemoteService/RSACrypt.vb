Imports System
Imports System.Security.Cryptography
Imports System.Text

Friend Class RSACrypt
    Public PublicKey As String
    Public PrivateKey As String
    Public Sub New()
        Dim RSA As New RSACryptoServiceProvider()
        PublicKey = RSA.ToXmlString(False)
        PrivateKey = RSA.ToXmlString(True)
    End Sub
    Public Sub New(ByVal key As String)
        PublicKey = key
        PrivateKey = key
    End Sub
    Function SignData(ByVal value As String) As String
        'decrypt a string from base64 code
        Dim ByteConverter As New ASCIIEncoding

        Dim encryptedData As Byte() = ByteConverter.GetBytes(value)
      
        Dim decryptedData() As Byte
        Dim RSA As New RSACryptoServiceProvider()
        RSA.FromXmlString(PrivateKey)
        decryptedData = RSA.SignData(encryptedData, New SHA1CryptoServiceProvider)
        'decryptedData = RSADecrypt(encryptedData, RSA.ExportParameters(True), False)
        Return Convert.ToBase64String(decryptedData)
    End Function
    Function VerifyData(ByVal value As String, ByVal originalData As String) As Boolean
        Dim key As String = Nothing
        If Not (PublicKey Is Nothing) AndAlso PublicKey.Length > 0 Then
            key = PublicKey
        ElseIf Not (PrivateKey Is Nothing) AndAlso PrivateKey.Length > 0 Then
            key = PrivateKey
        End If
        If key Is Nothing Then Throw New Exception("Wrong Key")
        Dim ByteConverter As New ASCIIEncoding()
        Dim dataToEncrypt As Byte() = Convert.FromBase64String(value)
        Dim encryptedData As Byte() = ByteConverter.GetBytes(originalData)
        Dim RSASP As New RSACryptoServiceProvider()
        RSASP.FromXmlString(key)
        Return RSASP.VerifyData(encryptedData, New SHA1CryptoServiceProvider, dataToEncrypt)
    End Function
    Public Shared Function GenerateKey() As RSACrypt
        Dim RSA As New RSACryptoServiceProvider()
        Dim crp As New RSACrypt
        crp.PublicKey = RSA.ToXmlString(False)
        crp.PrivateKey = RSA.ToXmlString(True)
        Return crp
    End Function

    Public Shared Function EncryptString(ByVal value As String, ByVal key As String) As String
        'encrypt a string into base64 code
        Dim ByteConverter As New UnicodeEncoding()
        'Create byte arrays to hold original, encrypted, and decrypted data.
        Dim dataToEncrypt As Byte() = ByteConverter.GetBytes(value)
        Dim encryptedData() As Byte
        'Dim decryptedData() As Byte

        'Create a new instance of RSACryptoServiceProvider to generate
        'public and private key data.
        Dim RSA As New RSACryptoServiceProvider()
        RSA.FromXmlString(key)
        'Pass the data to ENCRYPT, the public key information 
        '(using RSACryptoServiceProvider.ExportParameters(false),
        'and a boolean flag specifying no OAEP padding.
        encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(False), False)

        'Pass the data to DECRYPT, the private key information 
        '(using RSACryptoServiceProvider.ExportParameters(true),
        'and a boolean flag specifying no OAEP padding.
        'decryptedData = RSADecrypt(encryptedData, RSA.ExportParameters(True), False)

        'Display the decrypted plaintext to the console. 
        'Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData))
        Return Convert.ToBase64String(encryptedData)
    End Function
    Public Shared Function DecryptString(ByVal value As String, ByVal key As String) As String
        'decrypt a string from base64 code
        Dim ByteConverter As New UnicodeEncoding()
        'Create byte arrays to hold original, encrypted, and decrypted data.
        'Dim dataToEncrypt As Byte

        Dim encryptedData() As Byte = Convert.FromBase64String(value)
        Dim decryptedData() As Byte

        'Create a new instance of RSACryptoServiceProvider to generate
        'public and private key data.
        Dim RSA As New RSACryptoServiceProvider()
        RSA.FromXmlString(key)
        'Pass the data to ENCRYPT, the public key information 
        '(using RSACryptoServiceProvider.ExportParameters(false),
        'and a boolean flag specifying no OAEP padding.
        'encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(False), False)

        ''Pass the data to DECRYPT, the private key information 
        ''(using RSACryptoServiceProvider.ExportParameters(true),
        ''and a boolean flag specifying no OAEP padding.
        decryptedData = RSADecrypt(encryptedData, RSA.ExportParameters(True), False)

        ''Display the decrypted plaintext to the console. 
        'Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData))
        Return ByteConverter.GetString(decryptedData)
    End Function


    Public Shared Function RSAEncrypt(ByVal DataToEncrypt() As Byte, ByVal RSAKeyInfo As RSAParameters, ByVal DoOAEPPadding As Boolean) As Byte()
        Try
            'Create a new instance of RSACryptoServiceProvider.
            Dim RSA As New RSACryptoServiceProvider()

            'Import the RSA Key information. This only needs
            'toinclude the public key information.
            RSA.ImportParameters(RSAKeyInfo)

            'Encrypt the passed byte array and specify OAEP padding.  
            'OAEP padding is only available on Microsoft Windows XP or
            'later.  
            Return RSA.Encrypt(DataToEncrypt, DoOAEPPadding)
            'Catch and display a CryptographicException  
            'to the console.
        Catch e As CryptographicException
            Console.WriteLine(e.Message)

            Return Nothing
        End Try
    End Function


    Public Shared Function RSADecrypt(ByVal DataToDecrypt() As Byte, ByVal RSAKeyInfo As RSAParameters, ByVal DoOAEPPadding As Boolean) As Byte()
        Try
            'Create a new instance of RSACryptoServiceProvider.
            Dim RSA As New RSACryptoServiceProvider()

            'Import the RSA Key information. This needs
            'to include the private key information.
            RSA.ImportParameters(RSAKeyInfo)

            'Decrypt the passed byte array and specify OAEP padding.  
            'OAEP padding is only available on Microsoft Windows XP or
            'later.  
            Return(RSA.Decrypt(DataToDecrypt, DoOAEPPadding))
            'Catch and display a CryptographicException  
            'to the console.
        Catch e As CryptographicException
            Console.WriteLine(e.ToString())

            Return Nothing
        End Try
    End Function

End Class
