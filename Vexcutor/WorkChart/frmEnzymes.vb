Public Class frmEnzymes
    Private SelCol As List(Of String)
    Private ItemCol As Nuctions.RestrictionEnzymes
    Private Result As System.Windows.Forms.DialogResult

    Public Sub LoadEnzymeItems(ByVal AllItems As Nuctions.RestrictionEnzymes, ByVal SelectedItems As List(Of String))
        Dim ei As Nuctions.RestrictionEnzyme
        Dim ui As ListViewItem
        ItemCol = AllItems
        SelCol = SelectedItems
        'load all items
        For Each ei In AllItems.RECollection
            ui = SView.Items.Add(ei.Name)
            ui.Name = ei.Name
            If SelectedItems.Contains(ei.Name) Then ui.Checked = True
        Next
        Me.SView.Sort()
    End Sub

    Private Sub tsb_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_OK.Click
        Me.Result = Windows.Forms.DialogResult.OK
        Me.SelCol.Clear()
        Dim ui As ListViewItem
        For Each ui In Me.SView.Items
            If ui.Checked Then Me.SelCol.Add(ui.Name)
        Next
        Me.SView.Items.Clear()
        Me.Close()
    End Sub

    Private Sub frmItems_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.DialogResult = Me.Result
    End Sub

    Private Sub frmEnzymes_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                Me.Close()
        End Select
    End Sub

    Private Sub frmItems_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Result = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub tsb_All_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_All.Click
        Dim ui As ListViewItem
        For Each ui In SView.Items
            ui.Checked = True
        Next
    End Sub

    Private Sub tsb_None_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_None.Click
        Dim ui As ListViewItem
        For Each ui In SView.Items
            ui.Checked = False
        Next
    End Sub

    Private Sub SView_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles SView.ColumnClick
        SView.Sorting = 2 - SView.Sorting
        SView.Sort()
    End Sub
End Class