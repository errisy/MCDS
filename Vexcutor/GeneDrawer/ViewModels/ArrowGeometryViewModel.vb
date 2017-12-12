Imports System.Windows, System.Windows.Media
Public Class ArrowGeometryViewModel
    Inherits GeneFeatureViewModel

    Public Sub New()

    End Sub
    Public Sub New(_Radius As Double, _Height As Double, _ArrowWidth As Double, _SweepStart As Double, _SweepEnd As Double, _SweepDirection As Boolean)
        Dim _QuarterWidth As Double = _ArrowWidth / 4.0#
        Dim _StartOuter = New Point(_Radius * (_SweepStart / 180.0# - 1.0#), _Height + _QuarterWidth)
        Dim _StartInner = New Point(_Radius * (_SweepStart / 180.0# - 1.0#), _Height - _QuarterWidth)

        Dim _SweepNeck As Double
        Dim _SweepCenter As Double

        If _SweepDirection Then

            If _SweepEnd - 720.0# * _QuarterWidth / _Radius <= _SweepStart Then
                _SweepNeck = (_SweepStart + _SweepEnd) / 2.0#
            Else
                _SweepNeck = _SweepEnd - 360.0# * _QuarterWidth / _Radius
            End If

        Else
            If _SweepEnd + 720.0# * _QuarterWidth / _Radius >= _SweepStart Then
                _SweepNeck = (_SweepStart + _SweepEnd) / 2.0#
            Else
                _SweepNeck = _SweepEnd + 360.0# * _QuarterWidth / _Radius
            End If
        End If
        _SweepCenter = (_SweepStart + _SweepEnd) * 0.5#

        Dim _NeckOuter = New Point(_Radius * (_SweepNeck / 180.0# - 1.0#), _Height + _QuarterWidth)
        Dim _NeckInner = New Point(_Radius * (_SweepNeck / 180.0# - 1.0#), _Height - _QuarterWidth)

        Dim _CapOuter = New Point(_Radius * (_SweepNeck / 180.0# - 1.0#), _Height + 2.0# * _QuarterWidth)
        Dim _CapInner = New Point(_Radius * (_SweepNeck / 180.0# - 1.0#), _Height - 2.0# * _QuarterWidth)

        Dim _EndTip = New Point(_Radius * (_SweepEnd / 180.0# - 1.0#), _Height)

        Dim _Center = New Point(_Radius * (_SweepStart / 360.0# + _SweepEnd / 360.0# - 1.0#), _Height)

        Dim _Geometry As New StreamGeometry
        Using context = _Geometry.Open()
            context.BeginFigure(_StartOuter, True, True)
            context.LineTo(_NeckOuter, True, False)
            context.LineTo(_CapOuter, True, False)
            context.LineTo(_EndTip, True, False)
            context.LineTo(_CapInner, True, False)
            context.LineTo(_NeckInner, True, False)
            context.LineTo(_StartInner, True, False)
            context.LineTo(_StartOuter, True, False)
        End Using
        SetValue(GeometryProperty, _Geometry)
        SetValue(CenterAngleProperty, _SweepCenter)
        SetValue(CenterProperty, _Center)
    End Sub
    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New ArrowGeometryViewModel
    End Function
    Protected Overrides Function FreezeCore(isChecking As Boolean) As Boolean
        Geometry.Freeze()
        Return MyBase.FreezeCore(isChecking)
    End Function
 
End Class
