Public Class frmErrorReport

    Private Sub frmErrorReport_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Public Shadows Function ShowDialog(ByVal errQ As Queue(Of String), ByVal parent As System.Windows.Forms.IWin32Window) As Windows.Forms.DialogResult
        While errQ.Count > 0
            lvError.Items.Add(errQ.Dequeue)
        End While
        Return MyBase.ShowDialog(parent)
    End Function

    Private Sub frmErrorReport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                Me.Close()
        End Select
    End Sub
End Class