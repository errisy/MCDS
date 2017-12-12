Public Class frmAddFeature

    Dim vFeature As Nuctions.GeneAnnotation

    Private Sub ll_OK_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ll_OK.LinkClicked

        'add a new feature
        vFeature.Label = tbLabel.Text
        vFeature.Note = tbNote.Text
        vFeature.Type = cbType.Text
        vFeature.StartPosition = nudStart.Value
        vFeature.EndPosition = nudEnd.Value
        vFeature.Complement = cbDirection.Checked
        Me.DialogResult = Windows.Forms.DialogResult.OK

    End Sub
    Public Shadows Function ShowDialog(ByVal sFeature As Nuctions.GeneAnnotation, ByVal gf As Nuctions.GeneFile, ByVal vParent As System.Windows.Forms.IWin32Window) As Nuctions.GeneAnnotation
        vFeature = sFeature
        tbLabel.Text = vFeature.Label
        tbNote.Text = vFeature.Note
        If Not cbType.Items.Contains(vFeature.Type) Then
            cbType.Items.Add(vFeature.Type)
        End If
        cbType.Text = vFeature.Type
        nudStart.Minimum = 0
        nudStart.Maximum = gf.Length
        nudEnd.Minimum = 0
        nudEnd.Maximum = gf.Length
        nudStart.Value = vFeature.StartPosition
        nudEnd.Value = vFeature.EndPosition
        cbDirection.Checked = vFeature.Complement
        If MyBase.ShowDialog(vParent) = Windows.Forms.DialogResult.OK Then
            Return vFeature
        Else
            Return Nothing
        End If
    End Function

    Private Sub frmAddFeature_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    Private Sub frmAddFeature_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                Me.Close()
        End Select
    End Sub

    Public Property Feature() As Nuctions.GeneAnnotation
        Get
            Return vFeature
        End Get
        Set(ByVal value As Nuctions.GeneAnnotation)
            vFeature = value
        End Set
    End Property

    Private Sub llCancel_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llCancel.LinkClicked
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
End Class