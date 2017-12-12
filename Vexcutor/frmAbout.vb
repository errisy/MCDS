
Imports System.Management
Imports System.Security.Cryptography
Friend Class frmAbout


    Private Sub llSynthenome_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llSynthenome.LinkClicked
        Process.Start("https://mcds.codeplex.com/")
    End Sub

    Private Sub llOK_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llOK.LinkClicked
        DialogResult = Windows.Forms.DialogResult.OK
    End Sub
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

    Private Function ConvertFromHexString(ByVal code As String) As Byte()
        Dim blist As New List(Of Byte)
        Dim l As Integer = code.Length
        '0 1 2 3 (l=4) 
        For i As Integer = 0 To l - 1 Step 2
            If i + 1 >= l Then Exit For
            blist.Add(Char2Int(code.Chars(i)) * 16 + Char2Int(code.Chars(i + 1)))
        Next
        Return blist.ToArray
    End Function
    Private Function Char2Int(ByVal c As Char) As Integer
        Dim i As Integer = Asc(c)
        If i < 58 Then
            Return i - 48
        Else
            Return i - 55
        End If
    End Function

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub frmAbout_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblIC.Text = String.Format("{0}.{1}", VersionControl.Major, VersionControl.Minor.ToString.PadLeft(3, "0"))
    End Sub
End Class