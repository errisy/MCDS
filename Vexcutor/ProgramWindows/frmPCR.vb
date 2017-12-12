Public Class frmPCR

    Private Sub btnCalculateProduct_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalculateProduct.Click
        'get information
        Dim pcr As New PCRv1_0
        pcr.pcr_Forward = Nuctions.TAGCFilter(Me.tbForwardPrimer.Text)
        pcr.pcr_Reverse = Nuctions.TAGCFilter(Me.tbReversePrimer.Text)
        pcr.pcr_Template = Nuctions.TAGCFilter(Me.rtbTemplate.Text)
        Me.rtbProduct.Text = pcr.Run()
    End Sub

    Private Sub btnCopyProduct_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyProduct.Click
        Clipboard.SetText(Me.rtbProduct.Text)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class