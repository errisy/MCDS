Public Class PrimerManageView



    Private vSetting As Boolean = False

    Private vKeys As New Dictionary(Of String, List(Of DataGridViewRow))

    Private refPrimer As List(Of PrimerInfo)

    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property Primers As List(Of PrimerInfo)
        Get
            Dim vPrimers As New List(Of PrimerInfo)
            For Each row As DataGridViewRow In dgvPrimers.Rows
                If TypeOf row.Tag Is PrimerInfo Then
                    Dim pi As PrimerInfo = row.Tag
                    vPrimers.Add(pi)
                End If
            Next
            Return vPrimers
        End Get
        Set(value As List(Of PrimerInfo))
            Dim row As DataGridViewRow
            Dim seq As String
            vSetting = True
            dgvPrimers.Rows.Clear()
            If value Is Nothing Then Exit Property
            For Each pi As PrimerInfo In value
                row = dgvPrimers.Rows(dgvPrimers.Rows.Add())
                row.Cells(0).Value = pi.Name

                seq = pi.Sequence

                row.Cells(1).Value = seq

                row.Cells(2).Value = pi.Length.ToString
                row.Cells(3).Value = (pi.GCRatio * 100).ToString("0.00") + "%"
                row.Cells(4).Value = pi.TmBind.ToString("0.00")
                row.Cells(5).Value = pi.TmFull.ToString("0.00")

                row.Cells(6).Value = IIf(pi.NeedSynthesis, 1, 0)

                row.Tag = pi
                If Not pi.UserCreated Then row.ReadOnly = True
                If pi.UserCreated Then RenderRowText(row, Color.Blue)
            Next
            vSetting = False
            refPrimer = value
            SummarizeKeys()
        End Set
    End Property

    Private Sub SummarizeKeys()
        vKeys.Clear()
        For Each row As DataGridViewRow In dgvPrimers.Rows
            AddToKeys(row.Cells(0).Value, row)
        Next
        LabelRepeat()
    End Sub


    Private Sub AddToKeys(key As String, row As DataGridViewRow)
        Dim x As String
        If key Is Nothing Then
            x = ""
        Else
            x = key.ToLower
        End If
        If vKeys.ContainsKey(x) Then
            If Not vKeys(x).Contains(row) Then
                vKeys(x).Add(row)
            End If
        Else
            vKeys.Add(x, New List(Of DataGridViewRow) From {row})
        End If
    End Sub

    Private Sub LabelRepeat()
        Dim count As Integer = 0
        For Each x As String In vKeys.Keys
            If vKeys(x).Count > 1 Then count += 1
        Next
        Dim i As Integer = 0
        Dim c As Integer = 0
        For Each x As String In vKeys.Keys
            If vKeys(x).Count > 1 Then
                c = (255 / count) * i
                For Each v As DataGridViewRow In vKeys(x)
                    RenderRow(v, Color.FromArgb(255 - c, c, 0))
                Next
                i += 1
            Else
                For Each v As DataGridViewRow In vKeys(x)
                    RenderRow(v, Color.White)
                Next
            End If
        Next
    End Sub

    Private Sub RenderRow(row As DataGridViewRow, vColor As Color)
        For Each c As DataGridViewCell In row.Cells
            c.Style.BackColor = vColor
        Next
    End Sub
    Private Sub RenderRowText(row As DataGridViewRow, vColor As Color)
        For Each c As DataGridViewCell In row.Cells
            c.Style.ForeColor = vColor
        Next
    End Sub

    Private Sub dgvPrimers_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPrimers.CellValueChanged
        If vSetting Then Exit Sub
        If e.RowIndex < 0 Then Exit Sub
        Select Case e.ColumnIndex
            Case 0
                Dim row As DataGridViewRow = dgvPrimers.Rows(e.RowIndex)
                Dim pi As PrimerInfo
                If TypeOf row.Tag Is PrimerInfo Then
                    pi = row.Tag
                Else
                    pi = New PrimerInfo
                    row.Tag = pi
                End If
                pi.Name = row.Cells(0).Value
                SummarizeKeys()
            Case 1
                Dim row As DataGridViewRow = dgvPrimers.Rows(e.RowIndex)
                Dim seq As String = row.Cells(1).Value

                Dim pi As PrimerInfo
                If TypeOf row.Tag Is PrimerInfo Then
                    pi = row.Tag
                Else
                    pi = New PrimerInfo
                    row.Tag = pi
                End If
                pi.Sequence = seq
                pi.Calculate()

                vSetting = True
                row.Cells(1).Value = pi.Sequence
                row.Cells(2).Value = pi.Length.ToString
                row.Cells(3).Value = (pi.GCRatio * 100).ToString("0.00") + "%"
                row.Cells(4).Value = pi.TmBind.ToString("0.00")
                row.Cells(5).Value = pi.TmFull.ToString("0.00")
                SummarizeKeys()
                vSetting = False
            Case (6)
                Dim row As DataGridViewRow = dgvPrimers.Rows(e.RowIndex)
                Dim pi As PrimerInfo
                If TypeOf row.Tag Is PrimerInfo Then
                    pi = row.Tag
                Else
                    pi = New PrimerInfo
                    row.Tag = pi
                End If
                pi.NeedSynthesis = (row.Cells(6).Value = 1)
        End Select
    End Sub
    Private Function GetLength(vSequence As String) As Integer
        If vSequence Is Nothing OrElse vSequence.Length = 0 Then Return 0
        Return Nuctions.TAGCFilter(vSequence).Length
    End Function
    Private Function GetGCRatio(vSequence As String) As Single
        If vSequence Is Nothing OrElse vSequence.Length = 0 Then Return "N/A"
        Dim GC As Integer = 0
        For i As Integer = 0 To vSequence.Length - 1
            Select Case vSequence(i)
                Case "G", "g", "C", "c"
                    GC += 1
            End Select
        Next
        Return CSng(GC) / CSng(vSequence.Length)
    End Function
    Private Function GetTmBind(vSequence As String) As Single
        If vSequence Is Nothing OrElse vSequence.Length = 0 Then Return 0.0F
        Dim vSeq As String = ""
        If vSequence.IndexOf(">") < 0 Then
            vSeq = Nuctions.TAGCFilter(vSequence)
        Else
            vSeq = Nuctions.TAGCFilter(vSequence.Substring(vSequence.IndexOf(">")))
        End If
        If vSeq.Length = 0 Then
            Return 0.0F
        Else
            Return Nuctions.CalculateTm(vSeq, 80 * 0.001, 625 * 0.000000001).Tm
        End If
    End Function
    Private Function GetTmFull(vSequence As String) As Single
        If vSequence Is Nothing OrElse vSequence.Length = 0 Then Return 0.0F
        Dim vSeq As String = ""
        vSeq = Nuctions.TAGCFilter(vSequence)
        If vSeq.Length = 0 Then
            Return 0.0F
        Else
            Return Nuctions.CalculateTm(vSeq, 80 * 0.001, 625 * 0.000000001).Tm
        End If
    End Function

    Private Function GetSelectedRows() As List(Of DataGridViewRow)
        Dim selRows As New List(Of DataGridViewRow)
        For Each cell As DataGridViewCell In dgvPrimers.SelectedCells
            If cell.RowIndex > -1 AndAlso Not (selRows.Contains(cell.OwningRow)) Then selRows.Add(cell.OwningRow)
        Next
        For Each row As DataGridViewRow In dgvPrimers.SelectedRows
            If row.Index > -1 AndAlso Not (selRows.Contains(row)) Then selRows.Add(row)
        Next
        Return selRows
    End Function

    Private Sub llAdd_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llAdd.LinkClicked
        Dim row As DataGridViewRow = dgvPrimers.Rows(dgvPrimers.Rows.Add())
        Dim pi As PrimerInfo
        pi = New PrimerInfo
        pi.UserCreated = True
 
        row.Tag = pi
        row.Cells(6).Value = 1
        If pi.UserCreated Then RenderRowText(row, Color.Blue)
    End Sub

    Private Sub llRemove_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llRemove.LinkClicked
        Dim delRows As List(Of DataGridViewRow) = GetSelectedRows()
        Dim velRows As New List(Of DataGridViewRow)
        Dim pi As PrimerInfo
        For Each row As DataGridViewRow In delRows
            If TypeOf row.Tag Is PrimerInfo Then
                pi = row.Tag
                If Not pi.UserCreated Then velRows.Add(row)
            End If
            dgvPrimers.Rows.Remove(row)
        Next
        For Each row As DataGridViewRow In velRows
            dgvPrimers.Rows.Remove(row)
        Next
    End Sub

    Private Sub llPaste_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llPaste.LinkClicked
        Dim list As String = Clipboard.GetText()
        If list IsNot Nothing Then
            Dim lines As String() = list.Split(New Char() {ControlChars.NewLine, ControlChars.CrLf, ControlChars.Cr, ControlChars.Lf}, System.StringSplitOptions.RemoveEmptyEntries)
            Dim rgx As New System.Text.RegularExpressions.Regex("([^\:]+)\:([^\:]+)")
            Dim m As System.Text.RegularExpressions.Match
            Dim row As DataGridViewRow
            Dim pi As PrimerInfo
            For Each s As String In lines
                If rgx.IsMatch(s) Then
                    m = rgx.Match(s)
                    row = dgvPrimers.Rows(dgvPrimers.Rows.Add())
                    pi = New PrimerInfo
                    row.Tag = pi
                    pi.UserCreated = True
                    pi.Name = m.Groups(1).Value
                    pi.Sequence = m.Groups(2).Value
                    pi.Calculate()
                    vSetting = True

                    row.Cells(0).Value = pi.Name
                    row.Cells(1).Value = pi.Sequence
                    row.Cells(2).Value = pi.Length.ToString
                    row.Cells(3).Value = (pi.GCRatio * 100).ToString("0.00") + "%"
                    row.Cells(4).Value = pi.TmBind.ToString("0.00")
                    row.Cells(5).Value = pi.TmFull.ToString("0.00")
                    row.Cells(6).Value = 1
                    If pi.UserCreated Then RenderRowText(row, Color.Blue)
                    vSetting = False
                End If
            Next
        End If

    End Sub

    Private Sub llCopy_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llCopy.LinkClicked
        Dim selRows As List(Of DataGridViewRow) = GetSelectedRows()
        Dim stb As New System.Text.StringBuilder
        For Each row As DataGridViewRow In selRows
            stb.AppendFormat("{0}:{1}", row.Cells(0).Value, row.Cells(1).Value)
            stb.AppendLine()
        Next
        Clipboard.SetText(stb.ToString)
    End Sub

    Private Sub llApply_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llApply.LinkClicked
        'check if all lines are valid.
        refPrimer.Clear()
        For Each row As DataGridViewRow In dgvPrimers.Rows
            If TypeOf row.Tag Is PrimerInfo Then
                Dim pi As PrimerInfo = row.Tag
                refPrimer.Add(pi)
            End If
        Next
        If TypeOf Me.Parent Is TabPage Then
            Dim tp As TabPage = Parent
            If TypeOf tp.Parent Is TabContainer Then
                Dim tc As TabContainer = tp.Parent
                tc.TabPages.Remove(tp)
            End If
        End If
    End Sub

    Private Sub llCancel_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llCancel.LinkClicked
        If TypeOf Me.Parent Is TabPage Then
            Dim tp As TabPage = Parent
            If TypeOf tp.Parent Is TabContainer Then
                Dim tc As TabContainer = tp.Parent
                tc.TabPages.Remove(tp)
            End If
        End If
    End Sub
End Class
