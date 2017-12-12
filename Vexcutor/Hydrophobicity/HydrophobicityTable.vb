Public Class HydrophobicityTable
    Public Shared Function GetHydrophobicity(AminoAcid As String) As Double
        Select Case AminoAcid.ToLower
            Case "*", "stop"
                Return 0.0#
            Case "^", "start"
                Return 1.9#
            Case "ala", "a"
                Return 1.8#
            Case "arg", "r"
                Return -4.5#
            Case "asn", "n"
                Return -3.5#
            Case "asp", "d"
                Return -3.5#
            Case "cys", "c"
                Return 2.5#
            Case "glu", "e"
                Return -3.5#
            Case "gln", "q"
                Return -3.5#
            Case "gly", "g"
                Return -0.4#
            Case "his", "h"
                Return -3.2#
            Case "ile", "i"
                Return 4.5#
            Case "leu", "l"
                Return 3.8#
            Case "lys", "k"
                Return -3.9#
            Case "met", "m"
                Return 1.9#
            Case "phe", "f"
                Return 2.8#
            Case "pro", "p"
                Return -1.6#
            Case "ser", "s"
                Return -0.8#
            Case "thr", "t"
                Return -0.7#
            Case "trp", "w"
                Return -0.9#
            Case "tyr", "y"
                Return -1.3#
            Case "val", "v"
                Return 4.2#
            Case Else
                Return 0.0#
        End Select
    End Function
    Public Shared Function GetCharge(AminoAcid As String) As String
        Select Case AminoAcid.ToLower
            Case "arg", "r", "his", "h", "lys", "k"
                Return "+"
            Case "asp", "d", "glu", "e"
                Return "-"
            Case Else
                Return ""
        End Select
    End Function
    Public Shared Function CalculateProteinCharge(Peptide As String) As Integer
        Dim ChargeCount As Integer = 0
        For Each aa As Char In Peptide.ToLower.ToCharArray
            Select Case aa
                Case "r"c, "h"c, "k"c
                    ChargeCount += 1
                Case "d"c, "e"c
                    ChargeCount += 1
            End Select
        Next
        Return ChargeCount
    End Function
End Class
