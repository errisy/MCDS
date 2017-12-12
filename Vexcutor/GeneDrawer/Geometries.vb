Imports System.Windows
Imports System.Windows.Media
Public Class GeometryMethods
    Public Shared Function RadiusDegreeToPoint(_Radius As Double, _Degree As Double) As Point
        Return New Point(_Radius * Math.Sin(_Degree / 180.0# * Math.PI), -_Radius * Math.Cos(_Degree / 180.0# * Math.PI))
    End Function
    Public Shared Function SpringRadiusToDegree(_Spring As Double, _Radius As Double) As Double
        Return Math.Atan(_Spring / 2.0# / _Radius) * 360.0# / Math.PI
    End Function
End Class
