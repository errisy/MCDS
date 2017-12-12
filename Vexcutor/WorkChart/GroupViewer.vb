Public Class GroupViewer
    Public Sub ShowGeneFiles(ByVal gList As List(Of Nuctions.GeneFile), ByVal vRE As List(Of String))
        tcViewers.TabPages.Clear()
        For Each g As Nuctions.GeneFile In gList
            Dim tp As New CustomTabPage
            Dim sv As New SequenceViewer
            sv.RestrictionSites = vRE
            tp.Controls.Add(sv)
            tp.Text = g.Name
            tcViewers.TabPages.Add(tp)
            sv.Dock = DockStyle.Fill
            sv.ViewMode()
            sv.GeneFile = g
        Next
    End Sub
    Public Sub ShowCells(ByVal cList As List(Of Nuctions.Cell), vRE As List(Of String))
        tcViewers.TabPages.Clear()
        'tcViewers.SuspendLayout()
        For Each cell In cList
            Dim cTab As New CustomTabPage
            tcViewers.TabPages.Add(cTab)
            tcViewers.Update()
            cTab.Text = cell.Host.Name
            Dim innerViewer As New SubGroupViewer
            cTab.Controls.Add(innerViewer)
            innerViewer.Dock = DockStyle.Fill
            For Each g As Nuctions.GeneFile In cell.DNAs
                Dim tp As New CustomTabPage
                Dim sv As New SequenceViewer
                sv.RestrictionSites = vRE
                tp.Controls.Add(sv)
                innerViewer.TabPages.Add(tp)
                sv.ViewMode()
                tp.Text = g.Name
                'sv.Location = New Point(0, 0)
                'sv.Size = New Size(innerViewer.Width, innerViewer.Height - 80)
                'sv.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
                sv.Dock = DockStyle.Fill
                sv.GeneFile = g

            Next
        Next
        'tcViewers.ResumeLayout()
    End Sub
    Public Sub ShowPCR(ByVal gList As List(Of Nuctions.GeneFile), ByVal vRE As List(Of String), ByVal Primers As Dictionary(Of String, String))
        tcViewers.TabPages.Clear()
        For Each g As Nuctions.GeneFile In gList
            Dim tp As New CustomTabPage
            Dim sv As New SequenceViewer With {.GetSequenceTargets = RequireSequences}
            sv.RestrictionSites = vRE
            tp.Controls.Add(sv)
            tp.Text = g.Name
            tcViewers.TabPages.Add(tp)
            sv.Dock = DockStyle.Fill
            sv.PCREvent = AddressOf PassPCR
            sv.GeneFile = g
            sv.PCRMode(Primers)
        Next
    End Sub
    Public Sub ShowSelect(ByVal gList As List(Of Nuctions.GeneFile), ByVal vRE As List(Of String))
        tcViewers.TabPages.Clear()
        For Each g As Nuctions.GeneFile In gList
            Dim tp As New CustomTabPage
            Dim sv As New SequenceViewer
            sv.RestrictionSites = vRE
            tp.Controls.Add(sv)
            tp.Text = g.Name
            tcViewers.TabPages.Add(tp)
            sv.Dock = DockStyle.Fill
            sv.SelectMode()
            sv.SelectEvent = AddressOf PassSelectSequence
            sv.GeneFile = g
        Next
    End Sub

    Public Event SelectSequence(ByVal sender As Object, ByVal e As SelectEventArgs)
    Public Event PCR(ByVal sender As Object, ByVal e As PCREventArgs)

    Public Sub PassSelectSequence(ByVal e As SelectEventArgs)
        RaiseEvent SelectSequence(Me, e)
    End Sub
    Public Sub PassPCR(ByVal e As PCREventArgs)
        RaiseEvent PCR(Me, e)
    End Sub
    Public Function CopySequence() As String
        Dim tp As TabPage = tcViewers.SelectedTab
        If tp Is Nothing Then
            Return ""
        ElseIf TypeOf tp.Controls(0) Is SequenceViewer Then
            Dim sv As SequenceViewer = tp.Controls(0)
            Return sv.CopySelectedSequence()
        ElseIf TypeOf tp.Controls(0) Is SubGroupViewer Then
            Dim sgv As SubGroupViewer = tp.Controls(0)
            Dim stp As TabPage = sgv.tcViewers.SelectedTab
            If stp Is Nothing Then
                Return ""
            Else
                Dim sv As SequenceViewer = stp.Controls(0)
                Return sv.CopySelectedSequence
            End If
        End If
    End Function
    Public RequireSequences As System.Func(Of List(Of DNASequence))
End Class

Public Class DNAViewEventArgs
    Inherits EventArgs
    Public Sub New()

    End Sub
    Public DNAs As New List(Of Nuctions.GeneFile)
    Public Enzymes As New List(Of String)
    Public Sub New(ByVal nDNAs As List(Of Nuctions.GeneFile), ByVal vEnzymes As List(Of String))
        DNAs = nDNAs
        Enzymes.AddRange(vEnzymes)
    End Sub
End Class
Public Class CellViewEventArgs
    Inherits EventArgs
    Public Sub New()

    End Sub
    Public Cells As New List(Of Nuctions.Cell)
    Public Enzymes As New List(Of String)
    Public Sub New(ByVal nCells As List(Of Nuctions.Cell), ByVal vEnzymes As List(Of String))
        Cells = nCells
        Enzymes.AddRange(vEnzymes)
    End Sub
End Class

Public Class PCRViewEventArgs
    Inherits EventArgs
    Public Sub New()

    End Sub
    Public DNAs As New List(Of Nuctions.GeneFile)
    Public Sub New(ByVal nDNAs As List(Of Nuctions.GeneFile), ByVal vPrimers As Dictionary(Of String, String))
        DNAs = nDNAs
        Primers = vPrimers
    End Sub
    Public Primers As New Dictionary(Of String, String)
End Class

Public Delegate Sub PCREvent(ByVal e As PCREventArgs)
Public Delegate Sub SelectEvent(ByVal e As SelectEventArgs)

Public Class PCREventArgs
    Inherits EventArgs
    Public Key As String
    Public Primer As String
    Public Target As String
    Public Sub New()
    End Sub
    Public Sub New(ByVal vKey As String, ByVal vPrimer As String, ByVal vTarget As String)
        Key = vKey
        Primer = vPrimer
        Target = vTarget
    End Sub
End Class

Public Class SelectEventArgs
    Inherits EventArgs
    Public GeneFile As Nuctions.GeneFile
    Public Region As String
    Public Sub New()
    End Sub
    Public Sub New(ByVal vGeneFile As Nuctions.GeneFile, ByVal vRegion As String)
        GeneFile = vGeneFile
        Region = vRegion
    End Sub
End Class
