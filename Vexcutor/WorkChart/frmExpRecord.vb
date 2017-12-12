Public Class frmExpRecord
    Default Public ReadOnly Property Records(ByVal ID As Integer) As Nuctions.ExperimentRecord
        Get
            Dim er As New Nuctions.ExperimentRecord
            If ID < dgvRecord.Rows.Count And ID > -1 Then
                Dim row As DataGridViewRow = dgvRecord.Rows(ID)
                er.ID = CInt(row.Cells(0).Value)
                er.ExpDate = CDate(row.Cells(1).Value)
                er.InheritID = row.Cells(2).Value
                er.ExpIndex = row.Cells(3).Value
                er.Success = row.Cells(4).Value
                er.Record = row.Cells(5).Value
                er.NextIndex = row.Cells(6).Value
                er.Visible = row.Visible
            End If
            Return er
        End Get
    End Property
    Public Sub AddRecord(ByVal ID As Integer, ByVal OpDate As Date, ByVal inheritID As String, ByVal OpIndex As String, ByVal Success As Boolean, ByVal Record As String, ByVal OpNext As String, Optional ByVal Visibility As Boolean = True)
        Dim row As DataGridViewRow = dgvRecord.Rows(dgvRecord.Rows.Add())
        Dim clm As DataGridViewComboBoxColumn = dgvRecord.Columns(2)
        If Not clm.Items.Contains(inheritID.ToString) Then clm.Items.Add(inheritID.ToString)
        clm.Items.Add((row.Index + 1).ToString)
        row.Cells(0).Value = ID.ToString
        row.Cells(1).Value = OpDate.ToString
        row.Cells(2).Value = inheritID
        row.Cells(3).Value = OpIndex
        row.Cells(4).Value = Success
        row.Cells(5).Value = Record
        row.Cells(6).Value = OpNext
        row.Visible = Visibility
    End Sub
    Public Sub AddIndex(ByVal Title As String)
        Dim clm As DataGridViewComboBoxColumn
        clm = dgvRecord.Columns(3)
        clm.Items.Add(Title)
        clm = dgvRecord.Columns(6)
        clm.Items.Add(Title)
    End Sub
    Public Sub RemoveIndex(ByVal Title As String)
        Dim clm As DataGridViewComboBoxColumn
        clm = dgvRecord.Columns(3)
        If clm.Items.Contains(Title) Then clm.Items.Remove(Title)
        clm = dgvRecord.Columns(6)
        If clm.Items.Contains(Title) Then clm.Items.Remove(Title)
    End Sub

    Private Sub dgvRecord_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs)
        e.Cancel = True
        e.Row.Visible = False
    End Sub

    Private Sub tsb_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_Add.Click
        Dim row As DataGridViewRow = dgvRecord.Rows(dgvRecord.Rows.Add())
        row.Cells(0).Value = row.Index + 1
        row.Cells(1).Value = Date.Now.ToString
        row.Cells(2).Value = "-"
        row.Cells(3).Value = "Not Shown"
        row.Cells(4).Value = True
        row.Cells(5).Value = ""
        row.Cells(6).Value = "Not Shown"
        Dim clm As DataGridViewComboBoxColumn = dgvRecord.Columns(2)
        clm.Items.Add((row.Index + 1).ToString)
    End Sub

    Private AllowClose As Boolean = False

    Public ReadOnly Property Count() As Integer
        Get
            Return dgvRecord.Rows.Count
        End Get
    End Property

    Private Sub frmExpRecord_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.F4
 
        End Select
    End Sub

    Private Sub tsb_Remove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_Remove.Click
        If dgvRecord.SelectedCells.Count > 0 Then
            If MsgBox("Are you sure to delete this row?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Dim rowid As Integer = dgvRecord.SelectedCells(0).RowIndex
                Dim row As DataGridViewRow = dgvRecord.Rows(rowid)
                row.Visible = False
            End If
        End If
    End Sub

End Class