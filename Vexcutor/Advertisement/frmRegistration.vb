Public Class frmRegistration

    Private Sub frmRegistration_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Not Me.DialogResult = Windows.Forms.DialogResult.OK Then Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Public Sub Accept()
        ElementHost1.Child = Me.ClientRegistration
    End Sub
    Public Sub Deny()
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
End Class