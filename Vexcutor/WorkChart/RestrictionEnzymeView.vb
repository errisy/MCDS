Public Class RestrictionEnzymeView
    Private SelCol As List(Of String)
    Private ItemCol As Nuctions.RestrictionEnzymes
    Private Result As System.Windows.Forms.DialogResult


    'Public Owner As PropertyControl
    Public Class RESiteEventArgs
        Inherits EventArgs
        Public RESites As New List(Of String)
        Public Sub New()
        End Sub
        Public Sub New(ByVal vSite As List(Of String))
            RESites = vSite
        End Sub
    End Class
    Public Event SetRestrictSite(ByVal sender As Object, ByVal e As RESiteEventArgs)

    Public Event CloseTab(ByVal sender As Object, ByVal e As EventArgs)

    Public Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        AddHandler tbEnzymes.TextChanged, AddressOf tbEnzymes_TextChanged
        AddHandler SView.ItemChecked, AddressOf SView_ItemChecked
    End Sub
    Public Sub Close()
        RaiseEvent CloseTab(Me, New EventArgs)
    End Sub
    Public Sub LoadEnzymeItems(ByVal AllItems As Nuctions.RestrictionEnzymes, ByVal SelectedItems As List(Of String))

        RemoveHandler tbEnzymes.TextChanged, AddressOf tbEnzymes_TextChanged
        RemoveHandler SView.ItemChecked, AddressOf SView_ItemChecked

        Dim ei As Nuctions.RestrictionEnzyme
        Dim ui As ListViewItem
        ItemCol = AllItems
        SelCol = SelectedItems
        'load all items
        SView.Items.Clear()
        For Each ei In AllItems.RECollection
            ui = SView.Items.Add(ei.Name)
            ui.SubItems.Add(ei.Sequence)
            ui.SubItems.Add("S-" + ei.SCut.ToString + "  C-" + ei.ACut.ToString)
            ui.SubItems.Add(IIf(ei.Palindromic, "Yes", "No"))
            ui.Name = ei.Name

            If SelectedItems.Contains(ei.Name) Then ui.Checked = True
        Next
        Me.SView.Sort()

        Dim stb As New System.Text.StringBuilder

        For Each str As String In SelectedItems
            stb.Append(str)
            stb.Append(" ")
        Next
        tbEnzymes.Text = stb.ToString

        AddHandler tbEnzymes.TextChanged, AddressOf tbEnzymes_TextChanged
        AddHandler SView.ItemChecked, AddressOf SView_ItemChecked
    End Sub

    Public Sub Accept()
        Dim enzList As New List(Of String)


        For Each ci As ListViewItem In SView.Items
            If ci.Checked Then enzList.Add(ci.Name)
        Next
        RaiseEvent SetRestrictSite(Me, New RESiteEventArgs(enzList))
        RaiseEvent CloseTab(Me, New EventArgs)
    End Sub

    Private Sub tsb_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles llApply.Click

        Accept()

        'Owner.SetEnzymes(enzList)
    End Sub

    Private Sub frmEnzymes_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                Me.Close()
        End Select
    End Sub

    Private Sub frmItems_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub tsb_All_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ui As ListViewItem
        For Each ui In SView.Items
            ui.Checked = True
        Next
    End Sub

    Private Sub tsb_None_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ui As ListViewItem
        For Each ui In SView.Items
            ui.Checked = False
        Next
    End Sub

    Private Sub SView_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs)
        AddHandler tbEnzymes.TextChanged, AddressOf tbEnzymes_TextChanged
        Dim stb As New System.Text.StringBuilder

        For Each ci As ListViewItem In SView.Items
            If ci Is Nothing Then Continue For
            If ci.Checked Then
                stb.Append(ci.Name)
                stb.Append(" ")
            End If
        Next
        tbEnzymes.Text = stb.ToString
        RemoveHandler tbEnzymes.TextChanged, AddressOf tbEnzymes_TextChanged
    End Sub

    'Private Sub SView_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles SView.ColumnClick, SView.SelectedIndexChanged
    '    SView.Sorting = 2 - SView.Sorting
    '    SView.Sort()
    'End Sub

    Private Sub tbEnzymes_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RemoveHandler SView.ItemChecked, AddressOf SView_ItemChecked
        Dim enzs As String() = tbEnzymes.Text.ToLower.Split(New Char() {" "}, StringSplitOptions.RemoveEmptyEntries)

        For Each ci As ListViewItem In SView.Items
            If ci Is Nothing Then Continue For
            If Array.IndexOf(enzs, ci.Name.ToLower) > -1 Then
                ci.Checked = True
            Else
                ci.Checked = False
            End If
        Next
        AddHandler SView.ItemChecked, AddressOf SView_ItemChecked
    End Sub

    Private Sub llCancel_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llCancel.LinkClicked
        RaiseEvent CloseTab(Me, New EventArgs)
    End Sub

    Private Sub tbEnzymes_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbEnzymes.KeyDown
        If e.KeyCode = Keys.Enter Then
            Accept()
        End If
    End Sub

 
End Class
