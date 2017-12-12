Public Class DigestionManageView
    Private Shared rgxNames As New System.Text.RegularExpressions.Regex("[\w\.]+")
    Private Data As New List(Of Digestion)
    Private Digestion As Digestion
    Private IndexList As New List(Of String)

    Sub LoadDigestion(ByVal Digestions As List(Of Digestion))
        Data = Digestions
        IndexList.Clear()
        For Each dg As Digestion In Digestions
            IndexList.Add(dg.Title)
        Next
        blvList.DataSource = IndexList
        If IndexList.Count > 0 Then
            blvList.Index = 0
        Else
            blvList.Index = -1
        End If
    End Sub


    Private Sub llApplyBuffers_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llApplyBuffers.LinkClicked
        Dim clnames As New List(Of String)
        For Each m As System.Text.RegularExpressions.Match In rgxNames.Matches(tbBuffers.Text)
            clnames.Add(m.Groups(0).Value)
        Next

        Dim ColumnValues As New Dictionary(Of String, List(Of String))
        Dim rList As List(Of String)

        Dim rmList As New List(Of DataGridViewColumn)
        For Each cl As DataGridViewColumn In dgvEnzymeInfo.Columns
            If cl.Index > 2 Then rmList.Add(cl)
            If Not ColumnValues.ContainsKey(cl.HeaderText) Then
                rList = New List(Of String)
                ColumnValues.Add(cl.HeaderText, rList)
                For i As Integer = 0 To dgvEnzymeInfo.Rows.Count - 1
                    rList.Add(dgvEnzymeInfo.Rows(i).Cells(cl.Name).Value)
                Next
            End If
        Next

        'delete columns
        dgvEnzymeInfo.SuspendLayout()
        For Each cl As DataGridViewColumn In rmList
            dgvEnzymeInfo.Columns.Remove(cl)
        Next

        're add these columns
        For Each Buffer As String In clnames
            dgvEnzymeInfo.Columns(dgvEnzymeInfo.Columns.Add(Buffer, Buffer)).Width = 60
            If ColumnValues.ContainsKey(Buffer) Then
                rList = ColumnValues(Buffer)
                For i As Integer = 0 To dgvEnzymeInfo.Rows.Count - 1
                    dgvEnzymeInfo.Rows(i).Cells(Buffer).Value = rList(i)
                Next
            End If
        Next
        dgvEnzymeInfo.ResumeLayout()

        Digestion.Title = tbName.Text
    End Sub
    Private Function FindColumn(ByVal Header As String, ByVal Columns As List(Of DataGridViewColumn)) As DataGridViewColumn
        For Each cl As DataGridViewColumn In Columns
            If cl.HeaderText = Header Then Return cl
        Next
        Return Nothing
    End Function

    'Private SettingName As Boolean = False

    Friend Sub LoadDigestion(ByVal vDigestion As Digestion)
        Dim clnames As New List(Of String)
        For Each di As DigestionInfo In vDigestion
            For Each key As String In di.BufferActivities.Keys
                If Not clnames.Contains(key) Then clnames.Add(key)
            Next
        Next
        Dim stb As New System.Text.StringBuilder

        For i As Integer = 3 To dgvEnzymeInfo.Columns.Count - 1
            dgvEnzymeInfo.Columns.Remove(dgvEnzymeInfo.Columns(3))
        Next


        For Each key As String In clnames
            stb.Append(key)
            stb.Append(" ")
            dgvEnzymeInfo.Columns(dgvEnzymeInfo.Columns.Add(key, key)).Width = 60
        Next
        tbBuffers.Text = stb.ToString

        dgvEnzymeInfo.Rows.Clear()

        Dim row As DataGridViewRow
        dgvEnzymeInfo.SuspendLayout()

        For Each di As DigestionInfo In vDigestion
            row = dgvEnzymeInfo.Rows(dgvEnzymeInfo.Rows.Add)
            row.Cells(0).Value = ListToString(di.Names)
            row.Cells(1).Value = ListToString(di.Temp)
            row.Cells(2).Value = ListToString(di.Additives)
            For Each key As String In di.BufferActivities.Keys
                row.Cells(key).Value = di.BufferActivities(key)
            Next
        Next

        dgvEnzymeInfo.ResumeLayout()
        'SettingName = True
        tbName.Text = vDigestion.Title
        'SettingName = False
        Digestion = vDigestion
    End Sub



    Private Sub llHelpDigestionBuffer_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llHelpDigestionBuffer.LinkClicked
        Process.Start("http://synthenome.com/help/digestionbuffermanage.htm")
    End Sub

    Private Sub llCancel_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llCancel.LinkClicked
        LoadDigestion(Digestion)
    End Sub

    Private Sub llAdd_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim dg As New Digestion
        dg.Title = "Digestion Buffer Table"
        Data.Add(dg)
        IndexList.Add(dg.Title)
        blvList.Index = IndexList.Count - 1
    End Sub

    Private Sub blvList_LineSelectByClickOrDataSource(ByVal sender As Object, ByVal e As System.EventArgs) Handles blvList.SelectIndexChanged
        If Data Is Nothing Then Exit Sub
        If blvList.Index < 0 Then Exit Sub
        Dim vKey As String = IndexList(blvList.Index)
        Dim vSelection As Digestion = Nothing
        For Each dg As Digestion In Data
            If dg.Title = vKey Then
                vSelection = dg
            End If
        Next
        If Not (vSelection Is Nothing) Then
            LoadDigestion(vSelection)
        End If
    End Sub

    Private Sub llOK_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llOK.LinkClicked
        ChangeDigestionInfo()
    End Sub

    Private rgxActivity As New System.Text.RegularExpressions.Regex("[0123]\*?")
    Private Sub ChangeDigestionInfo()
        Dim DI As DigestionInfo
        Digestion.Clear() 'Collect information from the data
        Dim m As System.Text.RegularExpressions.Match
        For Each row As DataGridViewRow In dgvEnzymeInfo.Rows
            DI = New DigestionInfo
            DI.Names = StringToList(row.Cells(0).Value)
            DI.Temp = StringToList(row.Cells(1).Value)
            DI.Additives = StringToList(row.Cells(2).Value)
            For i As Integer = 3 To row.Cells.Count - 1
                If TypeOf row.Cells(i).Value Is String Then
                    Dim s As String = row.Cells(i).Value
                    If rgxActivity.IsMatch(s) Then
                        m = rgxActivity.Match(s)
                        DI.BufferActivities.Add(row.Cells(i).OwningColumn.HeaderText, m.Groups(0).Value)
                    Else
                        DI.BufferActivities.Add(row.Cells(i).OwningColumn.HeaderText, "0")
                    End If
                Else
                    DI.BufferActivities.Add(row.Cells(i).OwningColumn.HeaderText, "0")
                End If
            Next
            Digestion.Add(DI)
        Next
        Digestion.Title = tbName.Text
        Digestion.Save()
    End Sub

    Private Sub llAddEnzyme_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llAddEnzyme.LinkClicked
        dgvEnzymeInfo.Rows.Add()
    End Sub

    Private Sub llRemoveEnzymeInfo_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llRemoveEnzymeInfo.LinkClicked
        Dim dels As New List(Of DataGridViewRow)
        If dgvEnzymeInfo.SelectedRows.Count > 0 Then
            For Each row As DataGridViewRow In dgvEnzymeInfo.SelectedRows
                If row.Index > -1 Then
                    dels.Add(row)
                End If
            Next
        ElseIf dgvEnzymeInfo.SelectedCells.Count > 0 Then
            For Each c As DataGridViewCell In dgvEnzymeInfo.SelectedCells
                If c.RowIndex > -1 Then
                    If Not dels.Contains(c.OwningRow) Then dels.Add(c.OwningRow)
                End If
            Next
        End If
        For Each row As DataGridViewRow In dels
            dgvEnzymeInfo.Rows.Remove(row)
        Next
    End Sub

    Private Sub llRemove_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llRemove.LinkClicked
        Dim i As Integer = blvList.Index
        If i > -1 And i < Data.Count Then
            Dim dg As Digestion = Data(i)
            Data.Remove(dg)
            blvList.DataSource.RemoveAt(i)
        End If
        i -= 1
        If i < 0 Then i = 0
        blvList.Index = i
    End Sub
End Class
