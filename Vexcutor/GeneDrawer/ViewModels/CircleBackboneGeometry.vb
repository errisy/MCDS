Imports System.Windows, System.Windows.Media
Public Class CircleBackboneGeometry
    Inherits GeneViewModel
    Public Sub New()
        Fill = Brushes.Gray
    End Sub
    Public Sub New(_Radius As Double, _ArrowWidth As Double)
        Fill = Brushes.Gray
        Dim _QuarterWidth As Double = _ArrowWidth / 4.0#
        Dim _StartOuter = New Point(_Radius + _QuarterWidth, 0.0#)
        Dim _StartInner = New Point(_Radius - _QuarterWidth, 0.0#)
        Dim _MiddleOuter = New Point(-_Radius - _QuarterWidth, 0.0#)
        Dim _MiddleInner = New Point(-_Radius + _QuarterWidth, 0.0#)
        Dim _SizeOuter As New Size(_Radius + _QuarterWidth, _Radius + _QuarterWidth)
        Dim _SizeInner As New Size(_Radius - _QuarterWidth, _Radius - _QuarterWidth)
        Dim _Center = New Point(0.0#, 0.0#)
        Dim _Geometry As New StreamGeometry
        Using context = _Geometry.Open()
            context.BeginFigure(_StartOuter, True, True)
            context.ArcTo(_MiddleOuter, _SizeOuter, 180.0#, True, SweepDirection.Clockwise, True, False)
            context.ArcTo(_StartOuter, _SizeOuter, 180.0#, True, SweepDirection.Clockwise, True, False)
            context.LineTo(_StartInner, False, False)
            context.ArcTo(_MiddleInner, _SizeInner, 180.0#, True, SweepDirection.Counterclockwise, True, False)
            context.ArcTo(_StartInner, _SizeInner, 180.0#, True, SweepDirection.Counterclockwise, True, False)
            context.LineTo(_StartOuter, False, False)
        End Using
        SetValue(GeometryProperty, _Geometry)
        SetValue(CenterProperty, _Center)
    End Sub
    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New CircleBackboneGeometry
    End Function
    Protected Overrides Function FreezeCore(isChecking As Boolean) As Boolean
        Geometry.Freeze()
        Return MyBase.FreezeCore(isChecking)
    End Function
End Class
