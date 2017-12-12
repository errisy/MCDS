Public Class frmFeatureManage
    Dim nFeatureCol As Collection
    Public Shadows Function ShowDialog(ByVal FeatureCol As Collection, ByVal vParent As System.Windows.Forms.IWin32Window)
        'load features
        Dim ft As Nuctions.Feature
        Dim idx As Integer
        Dim col As DataGridViewComboBoxColumn
        Dim row As DataGridViewRow
        nFeatureCol = FeatureCol
        For Each ft In FeatureCol
            idx = dgvFeat.Rows.Add()
            row = dgvFeat.Rows(idx)
            'save the feature in the row
            row.Tag = ft
            row.Cells(0).Value = ft.Label
            row.Cells(1).Value = ft.Useful
            col = dgvFeat.Columns(2)
            If col.Items.Contains(ft.Type) Then
                row.Cells(2).Value = ft.Type
            Else
                col.Items.Add(ft.Type)
                row.Cells(2).Value = ft.Type
            End If
            row.Cells(3).Value = ft.Note
            row.Cells(4).Value = ft.Sequence.Length.ToString
            row.Cells(5).Value = ft.Name
            row.Cells(6).Value = ft.Sequence
        Next
        Return MyBase.ShowDialog(vParent)
    End Function

    Private Sub tsb_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_OK.Click
        'save the features
        Dim row As DataGridViewRow
        nFeatureCol.Clear()
        For Each row In dgvFeat.Rows
            nFeatureCol.Add(New Nuctions.Feature(row.Cells(5).Value, row.Cells(6).Value, _
            row.Cells(0).Value, row.Cells(2).Value, row.Cells(3).Value, row.Cells(1).Value), row.Cells(5).Value)
        Next
        Me.Close()
    End Sub

    Private Sub tsb_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_Add.Click
        Dim ft As Nuctions.Feature = New frmAddFeature().ShowDialog(nFeatureCol, Me)
        Dim col As DataGridViewComboBoxColumn
        If Not ft Is Nothing Then
            Dim idx As Integer
            Dim row As DataGridViewRow
            idx = dgvFeat.Rows.Add()
            row = dgvFeat.Rows(idx)
            'save the feature in the row
            row.Tag = ft
            row.Cells(0).Value = ft.Label
            row.Cells(1).Value = ft.Useful
            col = dgvFeat.Columns(2)
            If col.Items.Contains(ft.Type) Then
                row.Cells(2).Value = ft.Type
            Else
                col.Items.Add(ft.Type)
                row.Cells(2).Value = ft.Type
            End If
            row.Cells(3).Value = ft.Note
            row.Cells(4).Value = ft.Sequence.Length.ToString
            row.Cells(5).Value = ft.Name
            row.Cells(6).Value = ft.Sequence
        End If
    End Sub

    Private Sub tsb_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_Delete.Click
        If MsgBox("Do You Really Want To Delete All Selected Rows?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
        Dim SelCol As New Collection
        Dim row As DataGridViewRow
        For Each row In dgvFeat.SelectedRows
            SelCol.Add(row)
        Next
        For Each row In SelCol
            dgvFeat.Rows.Remove(row)
        Next
    End Sub

    Private Sub dgvFeat_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvFeat.DataError

    End Sub

    Private Sub frmFeatureManage_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                Me.Close()
        End Select
    End Sub

    Private Sub dgvFeat_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvFeat.CellEndEdit

        If e.ColumnIndex = 6 Then
            dgvFeat.Rows(e.RowIndex).Cells(6).Value = Nuctions.TAGCFilter(dgvFeat.Rows(e.RowIndex).Cells(6).Value)
            Dim row As DataGridViewRow = dgvFeat.Rows(e.RowIndex).Cells(4).Value = dgvFeat.Rows(e.RowIndex).Cells(6).Value.ToString.Length
        End If
    End Sub
End Class