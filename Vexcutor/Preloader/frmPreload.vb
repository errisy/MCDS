Imports System.Management
Imports System.Security.Cryptography

Public Class frmPreload

    Private Function TryCStr(ByVal Value As Object) As String
        If Value Is Nothing Then
            Return ""
        Else
            Try
                Return CStr(Value)
            Catch ex As Exception
                Return ""
            End Try
        End If
    End Function
    Private Sub llSendInfo_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llSendInfo.LinkClicked

        If tbEmail.Text.Length > 0 AndAlso tbEmail.Text.Contains("@") Then

            Dim IDSTB As New System.Text.StringBuilder

            If rbProfessionalEdition.Checked Then
                IDSTB.AppendLine("Professional")
            Else
                IDSTB.AppendLine("Server")
            End If

            IDSTB.AppendLine(tbComputerID.Text)
            IDSTB.AppendLine(tbEmail.Text)
            IDSTB.AppendLine(tbOrganization.Text)


            '另一种代码形式的
            Dim objMOS As ManagementObjectSearcher

            Dim objMOC As Management.ManagementObjectCollection

            Dim objMO As Management.ManagementObject = Nothing

            'Now, execute the query to get the results

            objMOS = New ManagementObjectSearcher("Select * From Win32_Processor")

            objMOC = objMOS.Get

            'Finally, get the CPU's id.

            For Each objMO In objMOC
                IDSTB.AppendLine(objMO("ProcessorID"))
            Next

            'Dispose object variables.

            objMOS.Dispose()

            objMOS = Nothing

            objMO.Dispose()

            objMO = Nothing

            '/////////////////////
            Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(IDSTB.ToString())

            Dim des As Aes = Aes.Create

            Dim tsf = des.CreateEncryptor(KI, VI)

            Dim cypt As Byte() = tsf.TransformFinalBlock(bytes, 0, bytes.Length)

            Dim email As String = tbEmail.Text
            Dim b64 = ConvertToHexString(cypt)

            Dim thr As New Threading.Thread(Sub()
                                                Dim value As String = Post(furl, "Email", email, "Regist", b64)
                                                ThreadHeader(Of String).ControlInvoke(Me, AddressOf OnReturn, value)
                                            End Sub)
            thr.Start()

            System.Diagnostics.Process.Start("http://www.synthenome.com")

        End If
    End Sub
    Private VI As Byte()
    Private KI As Byte()

    Private Sub OnReturn(ByVal Value As String)
        If Value.Contains("Thank you very much!") Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub frmPreload_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub frmPreload_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        KI = New Byte() {13, 129, 228, 153,
                          233, 121, 23, 52,
                          9, 1, 76, 38,
                          45, 248, 87, 3,
                          4, 52, 125, 63,
                          255, 241, 23, 1,
                          0, 29, 91, 2,
                          47, 67, 3, 172}
        VI = New Byte() {0, 221, 12, 34,
                      2, 155, 184, 92,
                      92, 12, 38, 38,
                      145, 28, 7, 236}
    End Sub

    Private furl As String = "http://www.synthenome.com/register.php"

    Private Function Post(ByVal URL As String, ByVal Name1 As String, ByVal Value1 As String, ByVal Name2 As String, ByVal Value2 As String) As String
        Dim enc As System.Text.Encoding = System.Text.Encoding.UTF8
        'POST送信する文字列を作成
        Dim stb As New System.Text.StringBuilder
        'stb.Append(URL)
        'stb.Append("?")
        stb.Append(Name1)
        stb.Append("=")
        stb.Append(Value1)
        stb.Append("&")
        stb.Append(Name2)
        stb.Append("=")
        stb.Append(Value2)

        Dim postData As String = stb.ToString
        Dim postDataBytes As Byte() = enc.GetBytes(postData)
        Dim req As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create(URL)

        req.Method = "POST"
        req.ContentType = "application/x-www-form-urlencoded"
        req.ContentLength = postDataBytes.Length
        Dim reqStream As System.IO.Stream = req.GetRequestStream()
        reqStream.Write(postDataBytes, 0, postDataBytes.Length)
        reqStream.Close()
        Dim res As System.Net.HttpWebResponse = req.GetResponse
        Dim stm As System.IO.Stream = res.GetResponseStream
        Dim str As New System.IO.StreamReader(stm)
        Return str.ReadToEnd
    End Function

    Private Function ConvertToHexString(ByVal bytes As Byte()) As String
        Dim stb As New System.Text.StringBuilder
        For Each b As Byte In bytes
            stb.Append(Hex(b).PadLeft(2, "0"))
        Next
        Return stb.ToString
    End Function
 
End Class
