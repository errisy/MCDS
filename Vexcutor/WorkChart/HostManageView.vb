Public Class HostManageView
    Private mHosts As New List(Of Nuctions.Host)

    Public Sub LoadHosts(vHosts As List(Of Nuctions.Host))
        dgvHost.Rows.Clear()
        mHosts = vHosts
        Dim r As DataGridViewRow
        For Each h As Nuctions.Host In vHosts
            r = dgvHost.Rows(dgvHost.Rows.Add)
            r.Cells(0).Value = h.Name
            r.Cells(1).Value = Nuctions.ExpressFeatureFunctions(h.BioFunctions)
            r.Cells(2).Value = h.Description
        Next
        If dgvHost.Rows.Count > 0 Then dgvHost.Rows(0).ReadOnly = True
        If dgvHost.Rows.Count > 1 Then dgvHost.Rows(1).ReadOnly = True
    End Sub
    Public Function GetHosts() As List(Of Nuctions.Host)
        Dim vh As New List(Of Nuctions.Host)
        For Each r As DataGridViewRow In dgvHost.Rows
            vh.Add(New Nuctions.Host With {.Name = CastStr(r.Cells(0).Value), .BioFunctions = Nuctions.AnalyzedFeatureCode(r.Cells(1).Value), .Description = CastStr(r.Cells(2).Value)})
        Next
        Return vh
    End Function
    Private Sub llApply_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llApply.LinkClicked
        If TypeOf Me.Parent Is TabPage Then
            Dim tp As TabPage = Parent
            If TypeOf tp.Parent Is TabContainer Then
                Dim tc As TabContainer = tp.Parent
                tc.TabPages.Remove(tp)
            End If
        End If
        mHosts.Clear()
        mHosts.AddRange(GetHosts())
    End Sub
    Private Sub llAdd_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llAdd.LinkClicked
        Dim r As DataGridViewRow
        r = dgvHost.Rows(dgvHost.Rows.Add)
        r.Cells(0).Value = ""
        r.Cells(1).Value = ""
        r.Cells(2).Value = ""
    End Sub
    Private Sub llDelete_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llDelete.LinkClicked
        Dim rows As List(Of DataGridViewRow) = GetDGVRows(dgvHost)
        Dim delrows As New List(Of DataGridViewRow)
        For Each r As DataGridViewRow In rows
            If Not r.ReadOnly Then delrows.Add(r)
        Next
        For Each r As DataGridViewRow In delrows
            dgvHost.Rows.Remove(r)
        Next
    End Sub
    Private Sub llCopy_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llCopy.LinkClicked

    End Sub
    Private Sub llPaste_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llPaste.LinkClicked

    End Sub

    Private Sub llCommonStrains_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llCommonStrains.LinkClicked
        Dim r As DataGridViewRow
        r = dgvHost.Rows(dgvHost.Rows.Add)
        r.Cells(0).Value = "E. coli DH5α"
        r.Cells(1).Value = "Primase[p15a]Primase[pUC]"
        r.Cells(2).Value = ""
        r = dgvHost.Rows(dgvHost.Rows.Add)
        r.Cells(0).Value = "E. coli TOP10"
        r.Cells(1).Value = "Primase[p15a]Primase[pUC]"
        r.Cells(2).Value = ""
        r = dgvHost.Rows(dgvHost.Rows.Add)
        r.Cells(0).Value = "E. coli BL21"
        r.Cells(1).Value = "Primase[p15a]Primase[pUC]"
        r.Cells(2).Value = ""
        r = dgvHost.Rows(dgvHost.Rows.Add)
        r.Cells(0).Value = "E. coli MG1655"
        r.Cells(1).Value = "Primase[p15a]Primase[pUC]"
        r.Cells(2).Value = ""
        r = dgvHost.Rows(dgvHost.Rows.Add)
        r.Cells(0).Value = "E. coli DH10B"
        r.Cells(1).Value = "Primase[p15a]Primase[pUC]"
        r.Cells(2).Value = ""
        r = dgvHost.Rows(dgvHost.Rows.Add)
        r.Cells(0).Value = "E. coli JM109"
        r.Cells(1).Value = "Primase[p15a]Primase[pUC]"
        r.Cells(2).Value = ""
        r = dgvHost.Rows(dgvHost.Rows.Add)
        r.Cells(0).Value = "E. coli S17-1"
        r.Cells(1).Value = "Primase[p15a]Primase[pUC]"
        r.Cells(2).Value = ""
        r = dgvHost.Rows(dgvHost.Rows.Add)
        r.Cells(0).Value = "E. coli S17-1 Pir"
        r.Cells(1).Value = "Primase[R6Kγ]Primase[p15a]Primase[pUC]"
        r.Cells(2).Value = ""
    End Sub
End Class

Public Class HostEventArgs
    Inherits EventArgs
    Public Hosts As New List(Of Nuctions.Host)
End Class