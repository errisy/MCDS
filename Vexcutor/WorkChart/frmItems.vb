Public Class frmItems
    Private SelCol As List(Of DNAInfo)
    Private ItemCol As List(Of DNAInfo)
    Private Result As System.Windows.Forms.DialogResult
    Public Sub LoadItems(ByVal AllItems As List(Of DNAInfo), ByVal SelectedItems As List(Of DNAInfo))
        Dim ci As DNAInfo
        Dim ui As ListViewItem
        ItemCol = AllItems
        SelCol = SelectedItems
        'load all items
        For Each ci In AllItems
            'If Not ci.Editing Then
            ui = SView.Items.Add(ci.Name, ci.MolecularOperation)
            ui.Tag = ci
            If SelectedItems.Contains(ci) Then ui.Checked = True
            'End If
        Next
    End Sub

    Private Sub tsb_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_OK.Click
        Me.Result = Windows.Forms.DialogResult.OK
        Me.SelCol.Clear()
        Dim ui As ListViewItem
        For Each ui In Me.SView.Items
            If ui.Checked Then Me.SelCol.Add(ui.Tag)
        Next
        Me.SView.Items.Clear()
        Me.Close()
    End Sub

    Private Sub frmItems_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.DialogResult = Me.Result
    End Sub

    Private Sub frmItems_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                Me.Close()
        End Select
    End Sub

    Private Sub frmItems_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Stop 'Load Item Views
        'SView.SmallImageList = frmMain.SmallIconList
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