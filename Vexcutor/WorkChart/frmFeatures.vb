Public Class frmFeatures
    Private SelCol As Collection
    Private ItemCol As List(Of Nuctions.Feature)
    Private Result As System.Windows.Forms.DialogResult
    Public Sub LoadFeatureItems(ByVal AllItems As List(Of Nuctions.Feature), ByVal SelectedItems As Collection)
        Dim fi As Nuctions.Feature
        Dim ui As ListViewItem
        ItemCol = AllItems
        SelCol = SelectedItems
        'load all items
        For Each fi In AllItems
            If Not fi.Useful Then Continue For
            ui = SView.Items.Add(fi.Label)
            ui.Name = fi.Name
            ui.SubItems.Add(fi.Type)
            ui.SubItems.Add(fi.Sequence.Length)
            ui.SubItems.Add(fi.Note)
            If SelectedItems.Contains(ui.Name) Then ui.Checked = True
        Next
    End Sub

    Private Sub tsb_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_OK.Click
        Me.Result = Windows.Forms.DialogResult.OK
        Me.SelCol.Clear()
        Dim ui As ListViewItem
        For Each ui In Me.SView.Items
            If ui.Checked Then Me.SelCol.Add(ItemCol(ui.Name), ui.Name)
        Next
        Me.SView.Items.Clear()
        Me.Close()
    End Sub

    Private Sub frmItems_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.DialogResult = Me.Result
    End Sub

    Private Sub frmFeatures_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
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
End Class