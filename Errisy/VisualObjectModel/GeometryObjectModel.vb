Public Class GeometryObjectModel
    Inherits AllocationObjectModel
    'Public Property Fill As Brush
    Private _Fill As Brush = Brushes.White
    Public Property Fill As Brush
        Get
            Return _Fill
        End Get
        Set(value As Brush)
            _Fill = value
            Change()
        End Set
    End Property
    'Public Property Stroke As Brush
    Private _Stroke As Brush = Brushes.Black
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
    Private _StrokeThickness As Double = 1.0#
    Public Property StrokeThickness As Double
        Get
            Return _StrokeThickness
        End Get
        Set(value As Double)
            _StrokeThickness = value
            Change()
        End Set
    End Property
    'Public Property Geometry As Geometry
    Private _Geometry As Geometry
    Public Property Geometry As Geometry
        Get
            Return _Geometry
        End Get
        Set(value As Geometry)
            _Geometry = value
            Change()
        End Set
    End Property
    Public Overrides ReadOnly Property GetSpaceGeometry As Geometry
        Get
            If _Geometry Is Nothing Then Return PathGeometry.Empty
            Return Geometry
        End Get
    End Property
    Public Overrides Sub ApplyOffset(Offset As Vector)
        Geometry.Transform = New TranslateTransform(Offset.X, Offset.Y)
    End Sub
    'Public Property ToolTip As Object
    Private _ToolTip As Object
    Public Property ToolTip As Object
        Get
            Return _ToolTip
        End Get
        Set(value As Object)
            _ToolTip = value
            Change()
        End Set
    End Property
End Class
