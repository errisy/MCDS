Public Class MultipleItemView
    Public Sub SetSelectedItems(ByVal value As List(Of ChartItem))
        Dim ci As ListViewItem
        DNAView.Items.Clear()
        CIList.Clear()
        CIList.AddRange(value)

        Dim Unstarted As Integer = 0
        Dim Inprogress As Integer = 0
        Dim Finished As Integer = 0
        Dim Obsolete As Integer = 0

        Dim IsChromosome As Boolean = True

        For Each di As ChartItem In value
            Select Case di.MolecularInfo.Progress
                Case ProgressEnum.Unstarted
                    Unstarted += 1
                Case ProgressEnum.Inprogress
                    Inprogress += 1
                Case ProgressEnum.Finished
                    Finished += 1
                Case ProgressEnum.Obsolete
                    Obsolete += 1
            End Select

            IsChromosome = IsChromosome And (di.MolecularInfo.DescribeType = DescribeEnum.Chromosome)

            For Each dna As Nuctions.GeneFile In di.MolecularInfo.DNAs
                ci = New ListViewItem
                ci.Text = dna.Name
                ci.SubItems.Add(dna.Sequence.Length.ToString)
                ci.SubItems.Add(IIf(dna.Iscircular, "Circular", "Linear"))
                ci.SubItems.Add(dna.End_F)
                ci.SubItems.Add(dna.End_R)
                ci.ImageIndex = di.MolecularInfo.MolecularOperation
                DNAView.Items.Add(ci)
            Next
        Next
        desSetting = True
        cbDescribe.Checked = IsChromosome
        desSetting = False
        llDNA.Text = DNAView.Items.Count.ToString + " DNA" + IIf(DNAView.Items.Count > 1, "s", "")
        llOperation.Text = value.Count.ToString + " Item" + IIf(value.Count > 1, "s", "")
        Setting_Finished = True
        rbUnstarted.Checked = (Unstarted = value.Count)
        rbInprogress.Checked = (Inprogress = value.Count)
        rbFinished.Checked = (Finished = value.Count)
        rbObsolete.Checked = (Obsolete = value.Count)
        Setting_Finished = False
    End Sub

    Private CIList As New List(Of ChartItem)

    Private Sub MultipleItemView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Load ItemView
        DNAView.SmallImageList = SettingEntry.SmallIconList
    End Sub
    Private Setting_Finished As Boolean = False
    Private Sub cbFinished_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Setting_Finished Then Exit Sub
        For Each ci As ChartItem In CIList
            ci.MolecularInfo.Finished = True
            ci.Reload(ci.MolecularInfo, ci.Parent.EnzymeCol)
            ci.Parent.Draw()
        Next
    End Sub

    Private Sub rbProgress_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUnstarted.CheckedChanged,
        rbInprogress.CheckedChanged, rbFinished.CheckedChanged, rbObsolete.CheckedChanged
        If Setting_Finished Then Exit Sub
        Dim sdr As RadioButton = sender
        For Each ci As ChartItem In CIList
            ci.MolecularInfo.Progress = sdr.Tag
            ci.Reload(ci.MolecularInfo, ci.Parent.EnzymeCol)
        Next
        If CIList.Count > 0 Then CIList(0).Parent.Draw()
    End Sub

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
    End Sub
    Private desSetting As Boolean = False
    Private Sub cbDescribe_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbDescribe.CheckedChanged
        If desSetting Then Exit Sub
        For Each ci As ChartItem In CIList
            If cbDescribe.Checked Then
                ci.MolecularInfo.DescribeType = DescribeEnum.Chromosome
            Else
                ci.MolecularInfo.DescribeType = DescribeEnum.Vecotor
            End If
        Next
    End Sub

    Private Sub llCalculate_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llCalculate.LinkClicked
        For Each ci In CIList
            ci.MolecularInfo.Calculate()
            ci.Reload(ci.MolecularInfo, ci.Parent.EnzymeCol)
        Next
    End Sub
End Class
