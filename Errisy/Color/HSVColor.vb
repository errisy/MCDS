Public Structure HSVColor
    Public Sub New(RGBColor As System.Windows.Media.Color)
        Dim hsv = RGBtoHSV(RGBColor.A, RGBColor.R, RGBColor.G, RGBColor.B)
        _Alpha = hsv._Alpha
        _Hue = hsv._Hue
        _Saturation = hsv._Saturation
        _Value = hsv._Value
    End Sub
    Public Sub New(H As Double, S As Double, V As Double)
        _Alpha = 100.0#
        _Hue = H
        _Saturation = S
        _Value = V
    End Sub
    Public Sub New(A As Double, H As Double, S As Double, V As Double)
        _Alpha = A
        _Hue = H
        _Saturation = S
        _Value = V
    End Sub
    Public Property Alpha As Double
    Public Property Hue As Double
    Public Property Saturation As Double
    Public Property Value As Double

    Public Shared Function HSVtoRGB(alpha As Double, hue As Double, saturation As Double, value As Double) As System.Windows.Media.Color
        Dim hi As Integer = Convert.ToInt32(Math.Floor(hue / 60.0#)) Mod 6
        Dim f As Double = hue / 60.0# - Math.Floor(hue / 60.0#)
        value = value * 2.55#
        Dim v As Integer = Convert.ToInt32(value)
        Dim p As Integer = Convert.ToInt32(value * (1.0# - saturation / 100.0#))
        Dim q As Integer = Convert.ToInt32(value * (1.0# - f * saturation / 100.0#))
        Dim t As Integer = Convert.ToInt32(value * (1.0# - (1.0# - f) * saturation / 100.0#))
        Dim a As Integer = alpha * 2.55#
        If a < 0 Then a = 0
        If a > 255 Then a = 255

        If hi = 0 Then
            Return System.Windows.Media.Color.FromArgb(a, v, t, p)
        ElseIf hi = 1 Then
            Return System.Windows.Media.Color.FromArgb(a, q, v, p)
        ElseIf hi = 2 Then
            Return System.Windows.Media.Color.FromArgb(a, p, v, t)
        ElseIf hi = 3 Then
            Return System.Windows.Media.Color.FromArgb(a, p, q, v)
        ElseIf hi = 4 Then
            Return System.Windows.Media.Color.FromArgb(a, t, p, v)
        Else
            Return System.Windows.Media.Color.FromArgb(a, v, p, q)
        End If
    End Function
    Public Shared Function RGBtoHSV(A As Integer, R As Integer, G As Integer, B As Integer) As HSVColor
        Dim red As Double = R / 255.0#
        Dim green As Double = G / 255.0#
        Dim blue As Double = B / 255.0#

        Dim minValue As Double = Math.Min(red, Math.Min(green, blue))
        Dim maxValue As Double = Math.Max(red, Math.Max(green, blue))
        Dim delta As Double = maxValue - minValue

        Dim h As Double
        Dim s As Double
        Dim v As Double = maxValue

        ''# Calculate the hue (in degrees of a circle, between 0 and 360)
        Select Case maxValue
            Case red
                If green >= blue Then
                    If delta = 0 Then
                        h = 0
                    Else
                        h = 60 * (green - blue) / delta
                    End If
                ElseIf green < blue Then
                    h = 60 * (green - blue) / delta + 360
                End If
            Case green
                h = 60 * (blue - red) / delta + 120
            Case blue
                h = 60 * (red - green) / delta + 240
        End Select

        ''# Calculate the saturation (between 0 and 1)
        If maxValue = 0 Then
            s = 0
        Else
            s = 1D - (minValue / maxValue)
        End If

        ''# Scale the saturation and value to a percentage between 0 and 100
        s *= 100
        v *= 100

        ''# Return a color in the new color space
        Return New HSVColor(A / 2.55#, h, s, v)
    End Function
    Public Shared Widening Operator CType(Color As System.Windows.Media.Color) As HSVColor
        Return RGBtoHSV(Color.A, Color.R, Color.G, Color.B)
    End Operator
    Public Shared Narrowing Operator CType(Color As HSVColor) As System.Windows.Media.Color
        Return HSVtoRGB(Color.Alpha, Color.Hue, Color.Saturation, Color.Value)
    End Operator

    Public Shared Operator *(hsv As HSVColor, Number As Double) As HSVColor
        Return New HSVColor(hsv._Alpha * Number, hsv._Hue * Number, hsv._Saturation * Number, hsv._Value * Number)
    End Operator
    Public Shared Operator +(hsv1 As HSVColor, hsv2 As HSVColor) As HSVColor
        Return New HSVColor(hsv1._Alpha + hsv2._Alpha, hsv1._Hue + hsv2._Hue,
                            hsv1._Saturation + hsv2._Saturation, hsv1._Value + hsv2._Value)
    End Operator
    Public Shared Function HueInterpolate(rgb1 As System.Windows.Media.Color, rgb2 As System.Windows.Media.Color, Value As Double, Optional Positive As Boolean = True) As System.Windows.Media.Color
        Dim hsv1 As HSVColor = rgb1
        Dim hsv2 As HSVColor = rgb2
        Dim hsv = hsv1 * (1.0# - Value) + hsv2 * Value
        Return hsv
    End Function

End Structure


