﻿Public Module FontParameterMapper

    Public Function Style2Double(vStyle As System.Windows.FontStyle) As Double
        Select Case vStyle
            Case System.Windows.FontStyles.Normal
                Return 0D
            Case System.Windows.FontStyles.Italic
                Return 1D
            Case System.Windows.FontStyles.Oblique
                Return 2D
            Case Else
                Return 0D
        End Select
    End Function
    Public Function Double2Style(vStyle As Double) As System.Windows.FontStyle
        Select Case Math.Round(vStyle)
            Case 0
                Return System.Windows.FontStyles.Normal
            Case 1
                Return System.Windows.FontStyles.Italic
            Case 2
                Return System.Windows.FontStyles.Oblique
            Case Else
                Return System.Windows.FontStyles.Normal
        End Select
    End Function

    Public Function Weight2Double(vWeight As System.Windows.FontWeight) As Double
        Select Case vWeight
            Case System.Windows.FontWeights.Thin
            Case System.Windows.FontWeights.ExtraLight
            Case System.Windows.FontWeights.Normal
            Case System.Windows.FontWeights.Medium
            Case System.Windows.FontWeights.SemiBold
            Case System.Windows.FontWeights.Bold
            Case System.Windows.FontWeights.ExtraBold
            Case System.Windows.FontWeights.Black
            Case System.Windows.FontWeights.ExtraBlack
        End Select
    End Function
    Public Function Double2Weight(vWeight As Double) As System.Windows.FontWeight
        Select Case Math.Ceiling(vWeight)
            Case 0 To 100
            Case 100 To 200
            Case 200 To 300
            Case 300 To 400
            Case 400 To 500
            Case 500 To 600
            Case 600 To 700
            Case 700 To 800
            Case 800 To 900
            Case 900 To 1000
        End Select
    End Function
End Module

