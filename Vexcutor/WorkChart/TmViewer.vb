Public Class TmViewer

    'Implements TabContentControl

    Private vGeneFile As Nuctions.GeneFile

    Public Property GeneFile() As Nuctions.GeneFile
        Get
            Return vGeneFile
        End Get
        Set(ByVal value As Nuctions.GeneFile)
            vGeneFile = value
            GenerateInfo(value, nudBlock.Value)
        End Set
    End Property

    Private mInfo As CodeInfo

    Public Class CodeInfo

        Public Sequence As String
        Public Tm As New List(Of Single)
        Public Length As Integer
    End Class

    Public Sub GenerateInfo(ByVal vGeneFile As Nuctions.GeneFile, ByVal oligoLength As Integer)
        Dim CI As New CodeInfo
        If (Not (vGeneFile Is Nothing)) AndAlso vGeneFile.Sequence.Length > 19 Then
            Dim stb As New System.Text.StringBuilder
            stb.Append(vGeneFile.Sequence)
            stb.Append(vGeneFile.Sequence.Substring(0, oligoLength))
            CI.Sequence = stb.ToString
            Dim vSeq As String
            For i As Integer = 0 To vGeneFile.Sequence.Length - 1
                vSeq = CI.Sequence.Substring(i, oligoLength)
                CI.Tm.Add(Nuctions.CalculateTm(vSeq, nudNa.Value * 0.001, nudC.Value * 0.000000001).Tm)
            Next
            CI.Length = vGeneFile.Sequence.Length
        End If
        mInfo = CI
        hsbCode.Maximum = mInfo.Length
        Draw(hsbCode.Value)
    End Sub

    Private ValueFont As New Font("Arial", 8)
    Private IndexFont As New Font("Arial", 6)
    Private CodeFont As New Font("Arial", 12, FontStyle.Bold)

    Public Sub Draw(ByVal Index As Integer)
        Dim g As Graphics = bpbView.BufferedGraphics
        g.Clear(Color.White)
        Dim w As Integer = 0

        Dim h As Integer = bpbView.Height

        'range from 40 to 100
        Dim vLow As Integer = nudLow.Value
        Dim vHigh As Integer = nudHigh.Value

        If vLow = vHigh Then Exit Sub


        Dim dy As Single = (h - 30) / (vHigh - vLow)

        Dim cY As Single = nudY.Value
        Dim cR As Single = nudR.Value

        If mInfo.Length > 0 Then
            Dim i As Integer = hsbCode.Value
            Dim v As Single

            While w < bpbView.Width And i < mInfo.Length
                v = mInfo.Tm(i)
                If v >= cR Then
                    g.FillPolygon(Brushes.MistyRose, New PointF() {New PointF(w + 1, h - 30), New PointF(w + 1, h - 30 - (mInfo.Tm(i) - vLow) * dy), New PointF(w + 19, h - 30 - (mInfo.Tm(i) - vLow) * dy), New PointF(w + 19, h - 30)})

                ElseIf v >= cY Then
                    g.FillPolygon(Brushes.LemonChiffon, New PointF() {New PointF(w + 1, h - 30), New PointF(w + 1, h - 30 - (mInfo.Tm(i) - vLow) * dy), New PointF(w + 19, h - 30 - (mInfo.Tm(i) - vLow) * dy), New PointF(w + 19, h - 30)})

                Else
                    g.FillPolygon(Brushes.Honeydew, New PointF() {New PointF(w + 1, h - 30), New PointF(w + 1, h - 30 - (mInfo.Tm(i) - vLow) * dy), New PointF(w + 19, h - 30 - (mInfo.Tm(i) - vLow) * dy), New PointF(w + 19, h - 30)})

                End If
                g.DrawString(mInfo.Sequence(i), CodeFont, Brushes.Black, 1 + w, h - 16)
                g.DrawString(mInfo.Tm(i).ToString("0"), ValueFont, Brushes.Blue, w, h - 28)
                g.DrawString((i \ 100).ToString, IndexFont, Brushes.Black, 1 + w, 0)
                g.DrawString((i Mod 100).ToString, IndexFont, Brushes.Black, 1 + w, 6)
                w += 20
                i += 1
            End While
        End If
        'draw the tm line

        g.DrawLine(Pens.Yellow, 0, h - 30 - (cY - vLow) * dy, bpbView.Width, h - 30 - (cY - vLow) * dy)
        g.DrawLine(Pens.Red, 0, h - 30 - (cR - vLow) * dy, bpbView.Width, h - 30 - (cR - vLow) * dy)

        bpbView.Refresh()
    End Sub

 
    Private Sub nudBlock_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudBlock.ValueChanged, nudNa.ValueChanged, nudC.ValueChanged
        GenerateInfo(vGeneFile, nudBlock.Value)
        Draw(hsbCode.Value)
    End Sub

    Private Sub nudHigh_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudHigh.ValueChanged, nudLow.ValueChanged, nudR.ValueChanged, nudY.ValueChanged
        Draw(hsbCode.Value)
    End Sub
    Private Sub hsbCode_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles hsbCode.Scroll
        Draw(hsbCode.Value)
    End Sub

    Private Sub llClose_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llClose.LinkClicked
        Dim tp As TabPage = Me.Parent

        SettingEntry.TryCloseTab(tp)
    End Sub

    Private Sub bpbView_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bpbView.MouseClick

    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        dgvPrimers.Rows.Clear()
        'this method is for designing verification primers
        Dim vGI As New List(Of GapInfo)
        Dim cY As Single = nudY.Value
        Dim cR As Single = nudR.Value

        Dim v As Single
        Dim v2 As Single
        Dim GI As GapInfo = Nothing
        Dim thr As Single = nudTHR.Value


        For i As Integer = 0 To mInfo.Tm.Count - 1
            v = mInfo.Tm(i)
            If v < cR Then
                If GI Is Nothing Then
                    GI = New GapInfo
                    GI.Threshold = thr
                    GI.Start = i
                    GI.Max = 0
                    GI.Score = 0
                    GI.Max = Math.Max(GI.Max, v)
                End If
                '如果超过黄色标准 将记录Score并且减少Threshold
                If v > cY Then
                    v2 = v - cY
                    v2 = v2 * v2
                    If GI.Threshold < v2 Then
                        GI.End = i - 1
                        GI.Average /= (GI.End - GI.Start + 1)
                        vGI.Add(GI)
                        GI = Nothing
                    Else
                        GI.Max = Math.Max(GI.Max, v)
                        GI.Average += v
                        GI.Threshold -= v2
                        GI.Score += v2
                    End If
                Else
                    GI.Max = Math.Max(GI.Max, v)
                    GI.Average += v
                End If

            Else
                If Not (GI Is Nothing) Then
                    GI.End = i - 1
                    GI.Average /= (GI.End - GI.Start + 1)
                    vGI.Add(GI)
                    GI = Nothing
                End If
            End If
        Next
        If Not (GI Is Nothing) Then
            GI.End = mInfo.Tm.Count - 1
            GI.Average /= (GI.End - GI.Start + 1)
            vGI.Add(GI)
        End If
        Dim row As DataGridViewRow
        Dim j As Integer = 0
        vGI.Sort()
        For Each GI In vGI
            row = dgvPrimers.Rows(dgvPrimers.Rows.Add)
            row.Cells(0).Value = j
            j += 1
            row.Cells(1).Value = GI.Start
            row.Cells(2).Value = GI.End
            row.Cells(3).Value = GI.Max
            row.Cells(4).Value = GI.Average
            row.Cells(5).Value = GI.Score
            row.Cells(6).Value = GI.End - GI.Start + 1
        Next
    End Sub

    Public Class GapInfo
        Implements IComparable(Of GapInfo)

        Public Start As Integer
        Public [End] As Integer
        Public Max As Single ' Max Tm Value
        Public Score As Single
        Public Threshold As Single
        Public Average As Single

        Public Function CompareTo(ByVal other As GapInfo) As Integer Implements System.IComparable(Of GapInfo).CompareTo
            Return Math.Sign((other.End - other.Start) - ([End] - Start))
        End Function
    End Class

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
    End Sub

    Private Sub TmViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (GeneFile Is Nothing) Then Draw(hsbCode.Value)
    End Sub
End Class
