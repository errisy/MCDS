Friend Module PassWordSign
    Private PublicKey As String = "<RSAKeyValue><Modulus>xUt85zma0hdRuuUJA0R+ZaTabQpbskoLOdEFJOVWLyobAero8ciBj3EbiXCi8KUnvE+2nkrB3WH9guL9sU7TaxQmyTY+NgQZ9PW9Ep5APAVd1Sr/BoRjzvPr0oFrUHoLoOi6cL2nbTKmidkoD9EmbA3Z19DoibsF3vLJ5WyqiIk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
    Public ReadOnly Property PasswordRSAEncrypt As RSACrypt
        Get
            Dim rsa As New RSACrypt(PublicKey)
            Return rsa
        End Get
    End Property
    Private vfaKey As Byte() = {144, 250, 213, 107, 66, 153, 66, 162, 116, 68, 33, 97, 183, 27, 119, 175, 230, 170, 22, 207, 124, 95, 72, 130, 90, 38, 159, 165, 79, 77, 145, 237}
    Private vfaIV As Byte() = {206, 245, 254, 210, 2, 70, 108, 221, 103, 198, 186, 32, 28, 49, 121, 23}
    Public ReadOnly Property VFAFileEncryption As System.Security.Cryptography.ICryptoTransform
        Get
            Dim aes As New System.Security.Cryptography.AesCryptoServiceProvider
            aes.IV = vfaIV
            aes.Key = vfaKey
            Return aes.CreateEncryptor
        End Get
    End Property
    Public ReadOnly Property VFAFileDecryption As System.Security.Cryptography.ICryptoTransform
        Get
            Dim aes As New System.Security.Cryptography.AesCryptoServiceProvider
            aes.IV = vfaIV
            aes.Key = vfaKey
            Return aes.CreateDecryptor
        End Get
    End Property
End Module