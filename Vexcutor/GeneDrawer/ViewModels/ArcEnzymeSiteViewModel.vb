Imports System.Windows, System.Windows.Media
Public Class ArcEnzymeSiteViewModel
    Inherits GeneEnzymeViewModel
    Public Sub New()
    End Sub
    Public Sub New(_Radius As Double, _ArrowWidth As Double, _SweepStart As Double, _SweepEnd As Double, _SweepDirection As Boolean)
        Dim _HalfWidth As Double = _ArrowWidth / 2.0#
        Dim _OuterRadius = _Radius + _HalfWidth
        Dim _InnerRadius = _Radius - _HalfWidth
        Dim _SweepCenter As Double
        Dim IsLargeArc As Boolean
        If _SweepDirection Then
            If _SweepEnd <= _SweepStart Then _SweepEnd += 360.0#
            IsLargeArc = _SweepEnd - _SweepStart >= 180.0#
            _SweepCenter = (_SweepStart + _SweepEnd) * 0.5#
        Else
            If _SweepStart <= _SweepEnd Then _SweepStart += 360.0#
            IsLargeArc = _SweepStart - _SweepEnd >= 180.0#
            _SweepCenter = (_SweepStart + _SweepEnd) * 0.5#
        End If

        Dim _StartOuter = GeometryMethods.RadiusDegreeToPoint(_OuterRadius, _SweepStart)
        Dim _StartInner = GeometryMethods.RadiusDegreeToPoint(_InnerRadius, _SweepStart)

        Dim _EndOuter = GeometryMethods.RadiusDegreeToPoint(_OuterRadius, _SweepEnd)
        Dim _EndInner = GeometryMethods.RadiusDegreeToPoint(_InnerRadius, _SweepEnd)

        Dim _Center = GeometryMethods.RadiusDegreeToPoint(_Radius, (_SweepStart + _SweepEnd) / 2.0#)

        Dim _OuterSize As New Size(_OuterRadius, _OuterRadius)
        Dim _InnerSize As New Size(_InnerRadius, _InnerRadius)

        Dim _Geometry As New StreamGeometry

        Using context = _Geometry.Open()
            context.BeginFigure(_StartOuter, True, True)
            context.ArcTo(_EndOuter, _OuterSize, _SweepEnd - _SweepStart, IsLargeArc, IIf(_SweepDirection, SweepDirection.Clockwise, SweepDirection.Counterclockwise), True, False)
            context.LineTo(_EndInner, True, False)
            context.ArcTo(_StartInner, _InnerSize, _SweepStart - _SweepEnd, IsLargeArc, IIf(_SweepDirection, SweepDirection.Counterclockwise, SweepDirection.Clockwise), True, False)
            context.LineTo(_StartOuter, True, False)
        End Using
        SetValue(CenterProperty, _Center)
        SetValue(CenterAngleProperty, _SweepCenter)
        SetValue(GeometryProperty, _Geometry)
    End Sub
    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New ArcEnzymeSiteViewModel
    End Function
    Protected Overrides Function FreezeCore(isChecking As Boolean) As Boolean
        Geometry.Freeze()
        Return MyBase.FreezeCore(isChecking)
    End Function
End Class
