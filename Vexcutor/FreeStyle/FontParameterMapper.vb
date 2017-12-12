Public Module FontParameterMapper

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
            Case System.Windows.FontWeights.Thin                Return 100D
            Case System.Windows.FontWeights.ExtraLight                Return 200D            Case System.Windows.FontWeights.Light                Return 300D
            Case System.Windows.FontWeights.Normal                Return 400D
            Case System.Windows.FontWeights.Medium                Return 500D
            Case System.Windows.FontWeights.SemiBold                Return 600D
            Case System.Windows.FontWeights.Bold                Return 700D
            Case System.Windows.FontWeights.ExtraBold                Return 800D
            Case System.Windows.FontWeights.Black                Return 900D
            Case System.Windows.FontWeights.ExtraBlack                Return 950D
        End Select
    End Function
    Public Function Double2Weight(vWeight As Double) As System.Windows.FontWeight
        Select Case Math.Ceiling(vWeight)
            Case 0 To 100                Return System.Windows.FontWeights.Thin
            Case 100 To 200                Return System.Windows.FontWeights.ExtraLight
            Case 200 To 300                Return System.Windows.FontWeights.Light
            Case 300 To 400                Return System.Windows.FontWeights.Normal
            Case 400 To 500                Return System.Windows.FontWeights.Medium
            Case 500 To 600                Return System.Windows.FontWeights.SemiBold
            Case 600 To 700                Return System.Windows.FontWeights.Bold
            Case 700 To 800                Return System.Windows.FontWeights.ExtraBold
            Case 800 To 900                Return System.Windows.FontWeights.Black
            Case 900 To 1000                Return System.Windows.FontWeights.ExtraBlack
        End Select
    End Function
End Module


