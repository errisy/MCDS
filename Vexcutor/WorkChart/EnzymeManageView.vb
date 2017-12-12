Public Class EnzymeManageView
    Private Sub llAdd_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llAdd.LinkClicked
        dgvManage.Rows.Add()
    End Sub
    Private Sub llRemove_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llRemove.LinkClicked
        For Each row As DataGridViewRow In FacilityFunctions.GetDGVRows(dgvManage)
            dgvManage.Rows.Remove(row)
        Next
    End Sub
End Class
