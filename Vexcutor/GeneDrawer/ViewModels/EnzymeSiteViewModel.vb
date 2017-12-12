Imports System.Windows, System.Windows.Media
Public Class EnzymeSiteViewModel
    Inherits GeneEnzymeViewModel
    Public Sub New()
    End Sub
    Public Sub New(_Radius As Double, _ArrowWidth As Double, _SweepStart As Double, _SweepEnd As Double, _SweepDirection As Boolean)
        Dim _HalfWidth As Double = _ArrowWidth / 2.0#
        Dim _SweepCenter As Double
        _SweepCenter = (_SweepStart + _SweepEnd) * 0.5#

        Dim _StartOuter = New Point(_Radius * (_SweepStart / 180.0# - 1.0#), _HalfWidth)
        Dim _StartInner = New Point(_Radius * (_SweepStart / 180.0# - 1.0#), -_HalfWidth)

        Dim _EndOuter = New Point(_Radius * (_SweepEnd / 180.0# - 1.0#), _HalfWidth)
        Dim _EndInner = New Point(_Radius * (_SweepEnd / 180.0# - 1.0#), -_HalfWidth)

        Dim _Center = New Point(_Radius * (_SweepStart / 360.0# + _SweepEnd / 360.0# - 1.0#), 0.0#)

        Dim _Geometry As New StreamGeometry

        Using context = _Geometry.Open()
            context.BeginFigure(_StartOuter, True, True)
            context.LineTo(_EndOuter, True, False)
            context.LineTo(_EndInner, True, False)
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
