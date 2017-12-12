Public Class frmORF
    Private ORF As New ORFSearchDialog With {.Form = Me}
    Private _ORFOptions As New ORFSearchOptions
    Public ReadOnly Property ORFOptions As ORFSearchOptions
        Get
            Return _ORFOptions
        End Get
    End Property
    Private Sub frmORF_Load(sender As Object, e As EventArgs) Handles Me.Load
        ehORF.Child = ORF
        ORF.DataContext = _ORFOptions
    End Sub
End Class