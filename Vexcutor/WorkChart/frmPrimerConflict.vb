Public Class frmPrimerConflict
    Public Sub AddPrimer(ByVal ID As Integer, ByVal Name As String, ByVal Primer As String)
        Dim i As Integer = dgvPrimers.Rows.Add
        Dim row As DataGridViewRow = dgvPrimers.Rows(i)
        row.Cells(0).Value = ID
        row.Cells(1).Value = Name
        row.Cells(2).Value = Primer
    End Sub

    Public Sub HighlightPrimers(ByVal Names As List(Of String))
        For i As Integer = 0 To Names.Count - 1
            For Each row As DataGridViewRow In dgvPrimers.Rows
                If row.Cells(1).Value = Names(i) Then
                    For Each cell As DataGridViewCell In row.Cells
                        cell.Style.BackColor = GetColorFromIndex(i, Names.Count)
                    Next
                End If
            Next
        Next
    End Sub
    Public Function GetColorFromIndex(ByVal Index As Integer, ByVal Count As Integer) As Color
        'Index from 0 to count - 1
        Dim v As Double = Index / Count * Math.PI * 1.5
        Dim hp As Double = Math.PI / 2
        If v > Math.PI Then
            v -= Math.PI
            'b-r
            Return Color.FromArgb(255 * Math.Sin(v), 255, 255 * Math.Cos(v))
        ElseIf v > hp Then
            v -= hp
            'g-b
            Return Color.FromArgb(255, 255 * Math.Cos(v), 255 * Math.Sin(v))
        Else
            'r-g
            Return Color.FromArgb(255 * Math.Cos(v), 255 * Math.Sin(v), 255)
        End If
    End Function
    Public Shadows Sub Show()
        Me.dgvPrimers.ResumeLayout()
        MyBase.Show()
    End Sub

    Private Sub dgvPrimers_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPrimers.CellContentClick

    End Sub

End Class