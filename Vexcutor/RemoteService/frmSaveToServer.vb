Public Class frmSaveToServer

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If tbFileName.Text Like "![\/:?""<|>]*" Then
            tbFileName.Focus()
            lbInfo.Text = "Filename contains invalid chars such as ![\/:?""<|>]* "
        Else
            Hide()
            Dim acc As Account
            If cbSaveLoginInfo.Checked Then
                acc = New Account With {.IP = cbIP.Text, .UserName = tbName.Text, .Password = mtbPassword.Text}
            Else
                acc = New Account With {.IP = cbIP.Text}
            End If
            If Not (acc Is Nothing) Then
                frmMain.ClientSetting.AddAccount(acc)
                BinarySerializer.ToFile(frmMain.ClientSetting, frmMain.ClientSettingAddress)
            End If
            DialogResult = Windows.Forms.DialogResult.OK
        End If


    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Hide()
        DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
 
    Private Sub frmSaveToServer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        For Each acc As Account In frmMain.ClientSetting.Accounts
            cbIP.Items.Add(acc)
        Next
    End Sub

    Private Sub cbIP_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbIP.SelectedIndexChanged
        If cbIP.SelectedIndex > -1 Then
            Try
                Dim acc As Account = cbIP.Items(cbIP.SelectedIndex)
                If Not (acc.UserName Is Nothing) Then tbName.Text = acc.UserName
                If Not (acc.Password Is Nothing) Then mtbPassword.Text = acc.Password
            Catch ex As Exception

            End Try
        End If
    End Sub
End Class