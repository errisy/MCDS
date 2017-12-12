Public Class FeatureManageView
    Public Sub LoadCurrentFeatures(ByVal ftList As List(Of Nuctions.Feature))
        dgvFeat.Rows.Clear()
        LoadFeaturesToView(ftList)
    End Sub

    Public Sub LoadFeaturesToView(ByVal ftList As List(Of Nuctions.Feature), Optional ByVal vSource As String = "")
        loading = True
        If ftList Is Nothing Then Exit Sub
        Dim dgvC As System.Windows.Forms.DataGridViewComboBoxColumn = dgvFeat.Columns(1)
        For Each ft As Nuctions.Feature In ftList
            Dim i As Integer = dgvFeat.Rows.Add
            Dim row As DataGridViewRow = dgvFeat.Rows(i)
            row.Cells(0).Value = ft.Label
            If Not (dgvC.Items.Contains(ft.Type)) Then
                dgvC.Items.Add(ft.Type)
            End If
            row.Cells(1).Value = ft.Type
            row.Cells(2).Value = ft.Note
            row.Cells(3).Value = IIf(vSource.Length > 0, vSource, ft.Useful)
            row.Cells(4).Value = ft.Sequence.Length
            row.Cells(5).Value = ft.Sequence
            row.Cells(6).Value = Nuctions.ExpressFeatureFunctions(ft.BioFunctions)
        Next
        loading = False
    End Sub
    Public Sub DeleteFeature()
        For Each row As DataGridViewRow In GetDGVRows(dgvFeat)
            dgvFeat.Rows.Remove(row)
        Next
        'Dim delRow As New List(Of DataGridViewRow)

        'For Each row As DataGridViewRow In dgvFeat.Rows
        '    If row.Cells(3).Value = "Delete" Then
        '        delRow.Add(row)
        '    End If
        'Next

        'For Each row As DataGridViewRow In delRow
        '    If row.Cells(3).Value = "Delete" Then
        '        dgvFeat.Rows.Remove(row)
        '    End If
        'Next
    End Sub

    Public Sub DeleteNativeFeature()
        Dim delRow As New List(Of DataGridViewRow)
        For Each row As DataGridViewRow In dgvFeat.Rows
            If row.Cells(3).Value = "Native" Then
                delRow.Add(row)
            End If
        Next
        For Each row As DataGridViewRow In delRow
            If row.Cells(3).Value = "Native" Then
                dgvFeat.Rows.Remove(row)
            End If
        Next
    End Sub
    Public Sub SaveStandardFeatures()
        Dim delRow As New List(Of DataGridViewRow)
        For Each row As DataGridViewRow In dgvFeat.Rows
            If row.Cells(3).Value = "Standard" Then
                delRow.Add(row)
            End If
        Next
        Dim seq As String
        Dim exists As Boolean
        For Each row As DataGridViewRow In delRow
            seq = Nuctions.TAGCFilter(row.Cells(5).Value)
            exists = False
            For Each ft As Nuctions.Feature In SettingEntry.StdFeatures
                If ft.Sequence = seq Or ft.RCSequence = seq Then
                    exists = True
                    Exit For
                End If
            Next
            If exists Then
                Exit For
            Else
                Dim vt As New Nuctions.Feature(row.Cells(0).Value, row.Cells(5).Value, row.Cells(0).Value, row.Cells(1).Value, row.Cells(2).Value)
            End If
        Next
    End Sub
    Public Sub LoadStandardFeatures()
        LoadFeaturesToView(SettingEntry.StdFeatures, "Standard")
    End Sub
    Public Sub RemoveDuplicateFeatures()
        RemoveDuplicateFeatures()
        Dim ftDict As New Dictionary(Of String, Nuctions.Feature)
        Dim seq As String
        Dim rc As String

        Dim delRow As New List(Of Nuctions.Feature)
        For Each row As DataGridViewRow In dgvFeat.Rows

            seq = Nuctions.TAGCFilter(row.Cells(5).Value)
            rc = Nuctions.ReverseComplement(seq)

            Dim vt As New Nuctions.Feature(row.Cells(0).Value, row.Cells(5).Value, row.Cells(0).Value, row.Cells(1).Value, row.Cells(2).Value, row.Cells(3).Value)
            Dim rt As Nuctions.Feature

            If ftDict.ContainsKey(seq) Or ftDict.ContainsKey(rc) Then
                rt = ftDict(seq)
                If (rt.Useful = "Native" Or rt.Useful = "Delete") And Not (vt.Useful = "Native" Or vt.Useful = "Delete") Then
                    ftDict(seq) = rt
                End If
            Else
                ftDict.Add(seq, vt)
                ftDict.Add(rc, vt)
            End If
        Next
        dgvFeat.Rows.Clear()
        For Each ft As Nuctions.Feature In ftDict.Values
            ft.Useful = "Local"
            delRow.Add(ft)
        Next
        LoadFeaturesToView(delRow, "Local")
    End Sub
    Private Sub tsb_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_Delete.Click
        DeleteFeature()
    End Sub
    Public Event UpdateFeature(ByVal sender As Object, ByVal e As FeatureEventArgs)
    Public Event CloseTab(ByVal sender As Object, ByVal e As EventArgs)

    Public Function GetCurrentFeatures() As List(Of Nuctions.Feature)
        Dim delRow As New List(Of Nuctions.Feature)
        For Each row As DataGridViewRow In dgvFeat.Rows
            If row.Cells(0).Value Is Nothing OrElse row.Cells(0).Value = "" Then Continue For 
            If row.Cells(5).Value Is Nothing OrElse row.Cells(5).Value = "" Then Continue For
            Dim vt As New Nuctions.Feature(row.Cells(0).Value, row.Cells(5).Value, row.Cells(0).Value, row.Cells(1).Value, row.Cells(2).Value, row.Cells(3).Value) With {.BioFunctions = Nuctions.AnalyzedFeatureCode(row.Cells(6).Value)}
            delRow.Add(vt)
        Next
        Return delRow
    End Function

    Private Sub tsb_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_OK.Click
        
        RaiseEvent UpdateFeature(Me, New FeatureEventArgs(GetCurrentFeatures()))
        RaiseEvent CloseTab(Me, New EventArgs)
    End Sub

    Private Sub tsbCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbCancel.Click
        RaiseEvent CloseTab(Me, New EventArgs)
    End Sub

    Private Sub tsb_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_Add.Click
        Dim i As Integer = dgvFeat.Rows.Add()
        Dim row As DataGridViewRow = dgvFeat.Rows(i)
        row.Cells(1).Value = "misc_feature"
        row.Cells(3).Value = "Local"
    End Sub

    Private Sub tsbSaveStandard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSaveStandard.Click
        SaveStandardFeatures()
    End Sub

    Private Sub tsbIncludeStandard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbIncludeStandard.Click
        LoadStandardFeatures()
    End Sub

    Private Sub tsbLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbLoad.Click
        If ofdFeature.ShowDialog = DialogResult.OK Then
            Dim vParameters As New List(Of String) From {"Features"}
            Dim dict As Dictionary(Of String, Object) = SettingEntry.LoadFromZXML(vParameters, ofdFeature.FileName)
            Dim fts As List(Of Nuctions.Feature)
            fts = dict("Features")
            LoadFeaturesToView(fts)
        End If
    End Sub

    Private Sub tsbSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSave.Click
        If sfdFeature.ShowDialog = DialogResult.OK Then
            Dim dict As New Dictionary(Of String, Object) From {{"Features", GetCurrentFeatures()}}
            SettingEntry.SaveToZXML(dict, sfdFeature.FileName)
        End If
    End Sub

    Dim loading As Boolean = False

    Private Sub dgvFeat_CellEndEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvFeat.CellEndEdit
        If e.RowIndex > -1 Then
            Select Case e.ColumnIndex
                Case 5
                    dgvFeat.Rows(e.RowIndex).Cells(5).Value = Nuctions.TAGCFilter(dgvFeat.Rows(e.RowIndex).Cells(5).Value)
                Case 6
                    dgvFeat.Rows(e.RowIndex).Cells(6).Value = Nuctions.ExpressFeatureFunctions(Nuctions.AnalyzedFeatureCode(dgvFeat.Rows(e.RowIndex).Cells(6).Value))
            End Select
        End If
    End Sub

    Private Sub dgvFeat_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvFeat.CellValueChanged
        If loading Then Exit Sub
        If e.RowIndex > -1 And (e.ColumnIndex = 0 Or e.ColumnIndex = 1 Or e.ColumnIndex = 2 Or e.ColumnIndex = 5) Then
            If dgvFeat.Rows(e.RowIndex).Cells(3).Value = "Native" Then dgvFeat.Rows(e.RowIndex).Cells(3).Value = "Local"
        End If
    End Sub

    Private Sub tsbRemoveNative_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRemoveNative.Click
        DeleteNativeFeature()
    End Sub

    Private Sub tsbExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbExport.Click
        If sfdFASTA.ShowDialog = DialogResult.OK Then
            Dim f As List(Of Nuctions.Feature) = GetCurrentFeatures()
            Dim stb As New System.Text.StringBuilder
            For Each ft As Nuctions.Feature In f
                stb.Append(">")
                stb.Append(ft.Label)
                stb.Append(" - ")
                stb.Append(ft.Note)
                stb.AppendLine()
                stb.AppendLine(ft.Sequence.ToUpper)
                stb.AppendLine()
            Next
            System.IO.File.WriteAllText(sfdFASTA.FileName, stb.ToString)
        End If
    End Sub

    Private Sub tsbCopyFeature_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbCopyFeature.Click
        Dim stb As New System.Text.StringBuilder
        '<Feature Label="" Type="" Note="" Useful="">
        Dim ft As New Nuctions.Feature

        For Each row As DataGridViewRow In dgvFeat.SelectedRows
            stb.AppendLine("<Feature ")
            stb.AppendLine("Label=")
            stb.AppendLine(IIf(row.Cells(0).Value Is Nothing, "", row.Cells(0).Value))
            stb.AppendLine("Type=")
            stb.AppendLine(IIf(row.Cells(1).Value Is Nothing, "", row.Cells(1).Value))
            stb.AppendLine("Note=")
            stb.AppendLine(IIf(row.Cells(2).Value Is Nothing, "", row.Cells(2).Value))
            stb.AppendLine("Sequence=")
            stb.AppendLine(IIf(row.Cells(5).Value Is Nothing, "", row.Cells(5).Value))
            stb.AppendLine("Function=")
            stb.AppendLine(IIf(row.Cells(6).Value Is Nothing, "", row.Cells(6).Value))
            stb.AppendLine("/>")
        Next
        Try
            Clipboard.SetDataObject(stb.ToString, True, 5, 20)
        Catch ex As Exception
            MsgBox("Clipboard Error.")
        End Try
    End Sub

    Private Sub tsbPasteFeature_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPasteFeature.Click
        Dim fts As String = Clipboard.GetText
        Dim lines As String() = fts.Split(New String() {Chr(13) & Chr(10)}, System.StringSplitOptions.None)
        Dim index As Integer = -1
        Dim row As DataGridViewRow = Nothing
        If Not (lines Is Nothing) AndAlso lines.Length > 0 AndAlso lines(0) = "<Feature " Then

            For Each l As String In lines
                Select Case l
                    Case "Label="
                        index = 0
                    Case "Type="
                        index = 1
                    Case "Note="
                        index = 2
                    Case "Sequence="
                        index = 5
                    Case "Function="
                        index = 6
                    Case "<Feature "
                        row = dgvFeat.Rows(dgvFeat.Rows.Add)
                        index = -1
                    Case "/>"
                        index = -1
                    Case Else
                        If index > -1 And Not (row Is Nothing) Then
                            Select Case index
                                Case 0, 1, 2, 6
                                    row.Cells(index).Value = l
                                Case 5
                                    row.Cells(5).Value = l
                                    row.Cells(4).Value = l.Length
                                    row.Cells(3).Value = "Local"
                            End Select

                            If index = 5 Then

                            End If
                            If index = 6 Then

                            End If
                        End If
                End Select
            Next
        End If
    End Sub

    Private Sub tsbSetLocal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSetLocal.Click
        For Each row As DataGridViewRow In dgvFeat.SelectedRows
            row.Cells(3).Value = "Local"
        Next
    End Sub
    Private Sub tsbSetDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSetDelete.Click
        For Each row As DataGridViewRow In dgvFeat.SelectedRows
            row.Cells(3).Value = "Delete"
        Next
    End Sub

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        If ReaderMode Then
            tspMain.Visible = False
            dgvFeat.ReadOnly = True
        End If
    End Sub

    'Private Sub tsbKEGGFeature_Click(sender As System.Object, e As System.EventArgs) Handles tsbKEGGFeature.Click

    'End Sub

    'Private Sub dgvFeat_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvFeat.CellContentClick

    'End Sub
End Class

Public Class FeatureEventArgs
    Inherits EventArgs
    Public Features As List(Of Nuctions.Feature)
    Public Sub New()

    End Sub
    Public Sub New(ByVal vFeatures As List(Of Nuctions.Feature))
        Features = vFeatures
    End Sub
End Class