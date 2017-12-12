Public Class LoginServer

    ' TODO: 插入代码，以使用提供的用户名和密码执行自定义的身份验证
    ' (请参见 http://go.microsoft.com/fwlink/?LinkId=35339)。 
    ' 随后自定义主体可附加到当前线程的主体，如下所示: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' 其中 CustomPrincipal 是用于执行身份验证的 IPrincipal 实现。 
    ' 随后，My.User 将返回 CustomPrincipal 对象中封装的标识信息
    ' 如用户名、显示名等

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private BlockNextChar As Boolean = False
    Private Sub TextBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles UsernameTextBox.KeyDown, PasswordTextBox.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Back AndAlso e.Modifiers = System.Windows.Forms.Keys.Control Then
            Dim tb As System.Windows.Forms.TextBox = sender
            e.Handled = True
            BlockNextChar = True
            With tb
                Dim sp As Integer = .Text.LastIndexOf(" ", .SelectionStart)
                If sp >= 0 Then
                    .Text = .Text.Substring(0, sp) + .Text.Substring(.SelectionStart)
                    .SelectionStart = sp
                Else
                    .Text = ""
                End If
            End With
        End If
    End Sub

    Private Sub TextBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles UsernameTextBox.KeyPress, PasswordTextBox.KeyPress ', UsernameTextBox.KeyDown, PasswordTextBox.KeyDown
        If BlockNextChar Then e.Handled = True : BlockNextChar = False
    End Sub
End Class
