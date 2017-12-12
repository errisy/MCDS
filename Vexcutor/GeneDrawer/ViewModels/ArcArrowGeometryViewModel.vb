Imports System.Windows, System.Windows.Media
Public Class ArcArrowGeometryViewModel
    Inherits GeneFeatureViewModel
    Public Sub New()
    End Sub
    Public Sub New(_Radius As Double, _ArrowWidth As Double, _SweepStart As Double, _SweepEnd As Double, _SweepDirection As Boolean)
        Dim _QuarterWidth As Double = _ArrowWidth / 4.0#
        Dim _OuterRadius = _Radius + _QuarterWidth
        Dim _InnerRadius = _Radius - _QuarterWidth
        Dim _SweepNeck As Double
        Dim _SweepCenter As Double
        Dim IsLargeArc As Boolean
        If _SweepDirection Then
            If _SweepEnd <= _SweepStart Then _SweepEnd += 360.0#
            Dim NeckDegree = GeometryMethods.SpringRadiusToDegree(_QuarterWidth * 2.0#, _Radius)
            If _SweepEnd - 2 * NeckDegree <= _SweepStart Then
                _SweepNeck = (_SweepEnd + _SweepStart) / 2.0#
            Else
                _SweepNeck = _SweepEnd - NeckDegree
            End If
            IsLargeArc = _SweepNeck - _SweepStart >= 180.0#
            _SweepCenter = (_SweepStart + _SweepEnd) * 0.5#
        Else
            If _SweepStart <= _SweepEnd Then _SweepStart += 360.0#
            Dim NeckDegree = GeometryMethods.SpringRadiusToDegree(_QuarterWidth * 2.0#, _Radius)
            If _SweepEnd + 2 * NeckDegree >= _SweepStart Then
                _SweepNeck = (_SweepEnd + _SweepStart) / 2.0#
            Else
                _SweepNeck = _SweepEnd + NeckDegree
            End If
            IsLargeArc = _SweepStart - _SweepNeck >= 180.0#
            _SweepCenter = (_SweepStart + _SweepEnd) * 0.5#
        End If

        Dim _StartOuter = GeometryMethods.RadiusDegreeToPoint(_OuterRadius, _SweepStart)
        Dim _StartInner = GeometryMethods.RadiusDegreeToPoint(_InnerRadius, _SweepStart)

        Dim _NeckOuter = GeometryMethods.RadiusDegreeToPoint(_OuterRadius, _SweepNeck)
        Dim _NeckInner = GeometryMethods.RadiusDegreeToPoint(_InnerRadius, _SweepNeck)

        Dim _CapOuter = GeometryMethods.RadiusDegreeToPoint(_OuterRadius + _QuarterWidth, _SweepNeck)
        Dim _CapInner = GeometryMethods.RadiusDegreeToPoint(_InnerRadius - _QuarterWidth, _SweepNeck)

        Dim _EndTip = GeometryMethods.RadiusDegreeToPoint(_Radius, _SweepEnd)

        Dim _Center = GeometryMethods.RadiusDegreeToPoint(_Radius, (_SweepStart + _SweepEnd) / 2.0#)

        Dim _OuterSize As New Size(_OuterRadius, _OuterRadius)
        Dim _InnerSize As New Size(_InnerRadius, _InnerRadius)

        Dim _Geometry As New StreamGeometry



        Using context = _Geometry.Open()
            context.BeginFigure(_StartOuter, True, True)
            context.ArcTo(_NeckOuter, _OuterSize, _SweepNeck - _SweepStart, IsLargeArc, IIf(_SweepDirection, SweepDirection.Clockwise, SweepDirection.Counterclockwise), True, False)
            context.LineTo(_CapOuter, True, False)
            context.LineTo(_EndTip, True, False)
            context.LineTo(_CapInner, True, False)
            context.LineTo(_NeckInner, True, False)
            context.ArcTo(_StartInner, _InnerSize, _SweepStart - _SweepNeck, IsLargeArc, IIf(_SweepDirection, SweepDirection.Counterclockwise, SweepDirection.Clockwise), True, False)
            context.LineTo(_StartOuter, True, False)
        End Using
        SetValue(CenterProperty, _Center)
        SetValue(CenterAngleProperty, _SweepCenter)
        SetValue(GeometryProperty, _Geometry)
    End Sub
    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New ArcArrowGeometryViewModel
    End Function
    Protected Overrides Function FreezeCore(isChecking As Boolean) As Boolean
        Geometry.Freeze()
        Return MyBase.FreezeCore(isChecking)
    End Function
End Class