Public Class Primer
    Implements IComparable(Of Primer)
    Public Pos As Integer
    Public Distance As Integer
    Public Seq As String = ""
    Public Score As Single
    Public MaxHairpinF As String
    Public MaxHairpinR As String
    Public MaxDimerF As String
    Public MaxDimerR As String
    Public Function CompareTo(ByVal other As Primer) As Integer Implements System.IComparable(Of Primer).CompareTo
        Dim dif As Single = other.Score - Score
        If Single.IsNaN(dif) Or Single.IsInfinity(dif) Then Return 0
        Return Math.Sign(other.Score - Score)
    End Function
End Class