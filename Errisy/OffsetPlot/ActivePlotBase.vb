Public Class ActivePlotBase
    Inherits DependencyObject
    'ActivePlotBase->EndDirection As Double Default: 0#
    Public Property EndDirection As Double
        Get
            Return GetValue(EndDirectionProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(EndDirectionProperty, value)
        End Set
    End Property
    Public Shared ReadOnly EndDirectionProperty As DependencyProperty = _
                           DependencyProperty.Register("EndDirection", _
                           GetType(Double), GetType(ActivePlotBase), _
                           New PropertyMetadata(0.0#))
    'ActivePlotBase->EndPoint As Point Default: New Point(0#,0#)
    Public Property EndPoint As Point
        Get
            Return GetValue(EndPointProperty)
        End Get
        Set(ByVal value As Point)
            SetValue(EndPointProperty, value)
        End Set
    End Property
    Public Shared ReadOnly EndPointProperty As DependencyProperty = _
                           DependencyProperty.Register("EndPoint", _
                           GetType(Point), GetType(ActivePlotBase), _
                           New PropertyMetadata(New Point(0.0#, 0.0#)))
End Class

Public Enum PlotType
    OriginAbsolute 'OA
    OriginRelative 'OR
    LineAbsoluteEndPoint 'LAE
    LineRelativeEndPoint 'LRE
    LineRelativeAngleLength 'LRAL
    QuadraticBezierAbsoluteControlPointEndPoint 'QBACE
    QuadraticBezierRelativeControlPointEndPoint 'QBRCE
    QuadraticBezierRelativeEndPointSmoothTurnAngle 'QBREST
    QuadraticBezierRelativeAngleLengthSmoothTurnAngle 'QBRALST
End Enum
