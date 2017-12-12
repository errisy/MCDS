Public Class LineObjectModel
    Inherits AllocationObjectModel

    'Public Property Stroke As Brush
    Private _Stroke As Brush
    Public Property Stroke As Brush
        Get
            Return _Stroke
        End Get
        Set(value As Brush)
            _Stroke = value
            Change()
        End Set
    End Property
    'Public Property StrokeThickness As Double
    Private _StrokeThickness As Double
    Public Property StrokeThickness As Double
        Get
            Return _StrokeThickness
        End Get
        Set(value As Double)
            _StrokeThickness = value
            Change()
        End Set
    End Property
    'Public Property StartPoint As Point
    Private _StartPoint As Point
    Public Property StartPoint As Point
        Get
            Return _StartPoint
        End Get
        Set(value As Point)
            _StartPoint = value
            Change()
        End Set
    End Property
    'Public Property EndPoint As Point
    Private _EndPoint As Point
    Public Property EndPoint As Point
        Get
            Return _EndPoint
        End Get
        Set(value As Point)
            _EndPoint = value
            Change()
        End Set
    End Property
    Private _Line As StreamGeometry
    Public ReadOnly Property Geometry As Geometry
        Get
            If _Line Is Nothing Then
                _Line = New StreamGeometry
                Using c = _Line.Open
                    c.BeginFigure(_StartPoint, False, False)
                    c.LineTo(_EndPoint, True, False)
                End Using

            End If
            Return _Line
        End Get
    End Property

    Public Overrides Sub ApplyOffset(Offset As Vector)
        StartPoint += Offset
        EndPoint += Offset
        Change()
    End Sub

    Public Overrides ReadOnly Property GetSpaceGeometry As Geometry
        Get
            Return PathGeometry.Empty
        End Get
    End Property
    Public Overrides Sub Change()
        _Line = Nothing
        MyBase.Change()
    End Sub
End Class
