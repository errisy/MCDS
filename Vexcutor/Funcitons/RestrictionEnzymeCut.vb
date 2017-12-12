 
Public Class RestrictionEnzymeCut
    Implements System.IComparable(Of RestrictionEnzymeCut)
    Friend EnzymeAnalysis As Nuctions.EnzymeAnalysis
    Public SingleCut As Boolean
    Public Degree As Single
    Public Function CompareTo(ByVal other As RestrictionEnzymeCut) As Integer Implements System.IComparable(Of RestrictionEnzymeCut).CompareTo
        Dim d1 As Single = Degree
        Dim d2 As Single = other.Degree
        If d1 > 180 Then d1 = d1 - 180
        If d2 > 180 Then d2 = d2 - 180
        d1 = Math.Abs(d1 - 90)
        d2 = Math.Abs(d2 - 90)
        Return Math.Sign(d2 - d1)
    End Function
End Class