Public Class PrimerPair
    Implements IComparable(Of PrimerPair)
    Public F As Primer
    Public R As Primer
    Public CrossDimer As Single
    Public Score As Single
    Public Function CompareTo(ByVal other As PrimerPair) As Integer Implements System.IComparable(Of PrimerPair).CompareTo
        Dim dif As Single = other.Score - Score
        If Single.IsNaN(dif) Or Single.IsInfinity(dif) Then Return 0
        Return Math.Sign(other.Score - Score)
    End Function
    Public Sub New()
    End Sub
    Public Sub New(ByVal P1 As Primer, ByVal P2 As Primer, ByVal Na As Single, ByVal C As Single)
        With Me
            .F = P1
            .R = P2
            .CrossDimer = Nuctions.PrimerAnalyzer.AnalyzeCrossDimer(P1.Seq, P2.Seq, Na, C)
            .Score = P1.Score +
                P2.Score +
                .CrossDimer -
                Math.Abs(.F.Seq.Length - .R.Seq.Length)   '-
            'Math.Abs(.F.Pos - .R.Pos) / 100
        End With
    End Sub
End Class