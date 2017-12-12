Imports System.Windows, System.Windows.Media
Public Class BackboneGeometry
    Inherits GeneViewModel
    Public Sub New()
        Fill = Brushes.Gray
    End Sub
    Public Sub New(_Radius As Double, _ArrowWidth As Double)
        Fill = Brushes.Gray
        Dim _QuarterWidth As Double = _ArrowWidth / 4.0#
        Dim _StartOuter = New Point(-_Radius, _QuarterWidth)
        Dim _StartInner = New Point(-_Radius, -_QuarterWidth)
        Dim _EndOuter = New Point(_Radius, _QuarterWidth)
        Dim _EndInner = New Point(_Radius, -_QuarterWidth)
        Dim _Center = New Point(0.0#, 0.0#)
        Dim _Geometry As New StreamGeometry
        Using context = _Geometry.Open()
            context.BeginFigure(_StartOuter, True, True)
            context.LineTo(_EndOuter, True, False)
            context.LineTo(_EndInner, True, False)
            context.LineTo(_StartInner, True, False)
            context.LineTo(_StartOuter, True, False)
        End Using
        SetValue(GeometryProperty, _Geometry)
        SetValue(CenterProperty, _Center)
    End Sub
    Protected Overrides Function CreateInstanceCore() As Windows.Freezable
        Return New BackboneGeometry
    End Function
    Protected Overrides Function FreezeCore(isChecking As Boolean) As Boolean
        Geometry.Freeze()
        Return MyBase.FreezeCore(isChecking)
    End Function
End Class
