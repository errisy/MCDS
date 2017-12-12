Public Class FeatureScreenView
    Public Sub SetFeaturesInfo(ByVal FeatureInfos As List(Of FeatureScreenInfo), ByVal Features As List(Of Nuctions.Feature))
        Dim i As Integer
        dgvFeatures.Rows.Clear()
        Dim row As DataGridViewRow
        For Each f As Nuctions.Feature In Features
            i = dgvFeatures.Rows.Add()
            row = dgvFeatures.Rows(i)
            row.Cells(0).Value = f.Name + "/" + f.Label
            row.Cells(1).Value = f.Note
            row.Cells(2).Value = f.Sequence.Length.ToString
            For Each fsi As FeatureScreenInfo In FeatureInfos
                If fsi.Feature Is f Then
                    Select Case fsi.ScreenMethod
                        Case FeatureScreenEnum.NotEngaged
                            row.Cells(3).Value = "NotEngaged"
                        Case FeatureScreenEnum.None
                            row.Cells(3).Value = "None"
                        Case FeatureScreenEnum.Once
                            row.Cells(3).Value = "Once"
                        Case FeatureScreenEnum.OnceOrMore
                            row.Cells(3).Value = "OnceOrMore"
                        Case FeatureScreenEnum.Maximum
                            row.Cells(3).Value = "Maximum"
                    End Select
                End If
            Next
            row.Tag = f
        Next

        dgvFeatures.Sort(dgvFeatures.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
    End Sub

    Public Event CloseTab(ByVal sender As Object, ByVal e As EventArgs)
    Public Event UpdateFeatures(ByVal sender As Object, ByVal e As FeatureScreenEventArgs)

    Public Sub OnCloseTab()
        RaiseEvent CloseTab(Me, New EventArgs)
    End Sub
    Public Sub OnUpdateFeatures()

        Dim fList As New List(Of FeatureScreenInfo)

        For Each row As DataGridViewRow In dgvFeatures.Rows
            Select Case row.Cells(3).Value
                Case "None"
                    Dim fsi As New FeatureScreenInfo
                    fsi.Feature = row.Tag
                    fsi.ScreenMethod = FeatureScreenEnum.None
                    fList.Add(fsi)
                Case "Once"
                    Dim fsi As New FeatureScreenInfo
                    fsi.Feature = row.Tag
                    fsi.ScreenMethod = FeatureScreenEnum.Once
                    fList.Add(fsi)
                Case "OnceOrMore"
                    Dim fsi As New FeatureScreenInfo
                    fsi.Feature = row.Tag
                    fsi.ScreenMethod = FeatureScreenEnum.OnceOrMore
                    fList.Add(fsi)
                Case "Maximum"
                    Dim fsi As New FeatureScreenInfo
                    fsi.Feature = row.Tag
                    fsi.ScreenMethod = FeatureScreenEnum.Maximum
                    fList.Add(fsi)
                Case Else

            End Select
        Next
        RaiseEvent UpdateFeatures(Me, New FeatureScreenEventArgs(fList))
    End Sub
    Private Sub llApply_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llApply.LinkClicked
        OnUpdateFeatures()
        OnCloseTab()
    End Sub

    Private Sub llCancel_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llCancel.LinkClicked
        OnCloseTab()
    End Sub
End Class

Public Class FeatureScreenEventArgs
    Inherits EventArgs
    Public FeatureScreenInfos As List(Of FeatureScreenInfo)
    Public Sub New()
    End Sub
    Public Sub New(ByVal vFSIs As List(Of FeatureScreenInfo))
        FeatureScreenInfos = vFSIs
    End Sub
End Class

Public Class FeatureScreenInfo
    Implements IComparable(Of FeatureScreenInfo)

    Public Feature As Nuctions.Feature
    Public ScreenMethod As FeatureScreenEnum

    Public Function CompareTo(ByVal other As FeatureScreenInfo) As Integer Implements System.IComparable(Of FeatureScreenInfo).CompareTo
        Return Math.Sign(other.ScreenMethod - Me.ScreenMethod)
    End Function
End Class

Public Enum FeatureScreenEnum As Integer
    NotEngaged = 0
    None = 1
    Once = 2
    OnceOrMore = 3
    Maximum = 4
End Enum