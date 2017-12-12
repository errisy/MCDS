Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes
Public Class RoundEndRectangle
    Inherits Shape

    Protected Overrides ReadOnly Property DefiningGeometry() As Geometry
        Get
            ' Create a StreamGeometry for describing the shape
            Dim geometry As New StreamGeometry()
            geometry.FillRule = FillRule.EvenOdd

            Using context As StreamGeometryContext = geometry.Open()
                InternalDrawArrowGeometry(context)
            End Using

            ' Freeze the geometry for performance benefits
            geometry.Freeze()

            Return geometry
        End Get
    End Property
    Public Property ActualSize As System.Windows.Vector
        Get
            Dim w As Double
            Dim h As Double
            w = Width
            If w = 0D Then w = ActualWidth
            h = Height
            If h = 0D Then h = ActualHeight
            Return V(w, h)
        End Get
        Set(value As System.Windows.Vector)
            Width = value.X
            Height = value.Y
        End Set
    End Property
    ''' <summary>
    ''' Draws an Arrow
    ''' </summary>
    Private Sub InternalDrawArrowGeometry(context As StreamGeometryContext)
        Dim az As System.Windows.Vector = ActualSize
        Dim w As Double = az.X
        Dim h As Double = az.Y
        Dim r As Double = h
        If w < h Then
            r = w
        End If
        'frmAnimation.Text = String.Format("r:{0} h:{1} w:{2}", r, h, w)
        context.BeginFigure(P(r / 2 + 0.5D, 0), True, False)
        context.LineTo(P(w - r / 2 - 1D, 0), True, True)
        'context.ArcTo(P(w, h / 2), New Size(r * 2, h), 0D, False, System.Windows.Media.SweepDirection.Clockwise, True, False)
        context.ArcTo(P(w - r / 2 - 1D, h), New Size(r / 2, h / 2), 0D, False, System.Windows.Media.SweepDirection.Clockwise, True, False)
        context.LineTo(P(r / 2 + 0.5D, h), True, True)
        context.ArcTo(P(r / 2 + 0.5D, 0), New Size(r / 2, h / 2), 0D, False, System.Windows.Media.SweepDirection.Clockwise, True, True)
        context.Close()
    End Sub
End Class

Public Class RightRoundEndRectangle
    Inherits Shape

    Protected Overrides ReadOnly Property DefiningGeometry() As Geometry
        Get
            ' Create a StreamGeometry for describing the shape
            Dim geometry As New StreamGeometry()
            geometry.FillRule = FillRule.EvenOdd

            Using context As StreamGeometryContext = geometry.Open()
                InternalDrawArrowGeometry(context)
            End Using

            ' Freeze the geometry for performance benefits
            geometry.Freeze()

            Return geometry
        End Get
    End Property
    Public Property ActualSize As System.Windows.Vector
        Get
            Dim w As Double
            Dim h As Double
            w = Width
            If w = 0D Then w = ActualWidth
            h = Height
            If h = 0D Then h = ActualHeight
            Return V(w, h)
        End Get
        Set(value As System.Windows.Vector)
            Width = value.X
            Height = value.Y
        End Set
    End Property
    ''' <summary>
    ''' Draws an Arrow
    ''' </summary>
    Private Sub InternalDrawArrowGeometry(context As StreamGeometryContext)
        Dim az As System.Windows.Vector = ActualSize
        Dim w As Double = az.X
        Dim h As Double = az.Y
        Dim r As Double = h
        If w < h Then
            r = w
        End If
        'frmAnimation.Text = String.Format("r:{0} h:{1} w:{2}", r, h, w)
        context.BeginFigure(P(0.5D, 0), True, False)
        context.LineTo(P(w - r / 2 - 1D, 0), True, True)
        'context.ArcTo(P(w, h / 2), New Size(r * 2, h), 0D, False, System.Windows.Media.SweepDirection.Clockwise, True, False)
        context.ArcTo(P(w - r / 2 - 1D, h), New Size(r / 2, h / 2), 0D, False, System.Windows.Media.SweepDirection.Clockwise, True, False)
        context.LineTo(P(0.5D, h), True, True)
        context.LineTo(P(0.5D, 0), True, True)
        context.Close()
    End Sub
End Class
Public Class LeftRoundEndRectangle
    Inherits Shape

    Protected Overrides ReadOnly Property DefiningGeometry() As Geometry
        Get
            ' Create a StreamGeometry for describing the shape
            Dim geometry As New StreamGeometry()
            geometry.FillRule = FillRule.EvenOdd

            Using context As StreamGeometryContext = geometry.Open()
                InternalDrawArrowGeometry(context)
            End Using

            ' Freeze the geometry for performance benefits
            geometry.Freeze()

            Return geometry
        End Get
    End Property
    Public Property ActualSize As System.Windows.Vector
        Get
            Dim w As Double
            Dim h As Double
            w = Width
            If w = 0D Then w = ActualWidth
            h = Height
            If h = 0D Then h = ActualHeight
            Return V(w, h)
        End Get
        Set(value As System.Windows.Vector)
            Width = value.X
            Height = value.Y
        End Set
    End Property
    ''' <summary>
    ''' Draws an Arrow
    ''' </summary>
    Private Sub InternalDrawArrowGeometry(context As StreamGeometryContext)
        Dim az As System.Windows.Vector = ActualSize
        Dim w As Double = az.X
        Dim h As Double = az.Y
        Dim r As Double = h
        If w < h Then
            r = w
        End If
        '      .Text = String.Format("r:{0} h:{1} w:{2}", r, h, w)
        context.BeginFigure(P(r / 2 + 0.5D, 0), True, False)
        context.LineTo(P(w - 1D, 0), True, True)
        context.LineTo(P(w - 1D, h), True, True)
        'context.ArcTo(P(w, h / 2), New Size(r * 2, h), 0D, False, System.Windows.Media.SweepDirection.Clockwise, True, False)
        context.LineTo(P(r / 2 + 0.5D, h), True, True)
        context.ArcTo(P(r / 2 + 0.5D, 0), New Size(r / 2, h / 2), 0D, False, System.Windows.Media.SweepDirection.Clockwise, True, True)
        context.Close()
    End Sub
End Class

Public Class RoundCornorRectangle
    Inherits Shape
    Protected Overrides ReadOnly Property DefiningGeometry() As Geometry
        Get
            ' Create a StreamGeometry for describing the shape
            Dim geometry As New StreamGeometry()
            geometry.FillRule = FillRule.EvenOdd

            Using context As StreamGeometryContext = geometry.Open()
                InternalDrawArrowGeometry(context)
            End Using

            ' Freeze the geometry for performance benefits
            geometry.Freeze()

            Return geometry
        End Get
    End Property
    Public Property ActualSize As System.Windows.Vector
        Get
            Dim w As Double
            Dim h As Double
            w = Width
            If w = 0D Then w = ActualWidth
            h = Height
            If h = 0D Then h = ActualHeight
            Return V(w, h)
        End Get
        Set(value As System.Windows.Vector)
            Width = value.X
            Height = value.Y
        End Set
    End Property
    Private _Depth As Double = 4
    Public Property Depth As Double
        Get
            Return _Depth
        End Get
        Set(value As Double)
            _Depth = value
            Dim u = Stroke
            Stroke = Nothing
            Stroke = u
        End Set
    End Property
    ''' <summary>
    ''' Draws an Arrow
    ''' </summary>
    Private Sub InternalDrawArrowGeometry(context As StreamGeometryContext)
        Dim az As System.Windows.Vector = ActualSize
        Dim w As Double = az.X
        Dim h As Double = az.Y
        Dim r As Double = _Depth
        If r < 0D Then r = 0D
        Dim hf As Double = Math.Min(w, h) / 2
        If r > hf Then r = hf
        context.BeginFigure(P(r + 1D, 0), True, False)
        context.LineTo(P(w - r - 1D, 0), True, True)
        context.ArcTo(P(w - 1D, r), New Size(r, r), 0D, False, System.Windows.Media.SweepDirection.Clockwise, True, False)
        context.LineTo(P(w - 1D, h - r), True, True)
        context.ArcTo(P(w - r - 1D, h), New Size(r, r), 0D, False, System.Windows.Media.SweepDirection.Clockwise, True, False)
        context.LineTo(P(r + 1D, h), True, True)
        context.ArcTo(P(1D, h - r), New Size(r, r), 0D, False, System.Windows.Media.SweepDirection.Clockwise, True, True)
        context.LineTo(P(1D, r), True, True)
        context.ArcTo(P(r + 1D, 0), New Size(r, r), 0D, False, System.Windows.Media.SweepDirection.Clockwise, True, True)
        context.Close()
    End Sub
End Class

Public MustInherit Class ShapeBase
    Inherits Shape
    Protected Overrides ReadOnly Property DefiningGeometry() As Geometry
        Get
            ' Create a StreamGeometry for describing the shape
            Dim geometry As New StreamGeometry()

            'frmAnimation.Text = geometry.Bounds.ToString
            Using context As StreamGeometryContext = geometry.Open()
                InternalDrawArrowGeometry(context)
            End Using

            ' Freeze the geometry for performance benefits
            geometry.Freeze()

            Return geometry
        End Get
    End Property
    Public Overrides ReadOnly Property RenderedGeometry As System.Windows.Media.Geometry
        Get
            Return DefiningGeometry
            Return MyBase.RenderedGeometry
        End Get
    End Property
    Public Property ActualSize As System.Windows.Vector
        Get
            Dim w As Double
            Dim h As Double
            w = Width
            If w = 0D Then w = ActualWidth
            h = Height
            If h = 0D Then h = ActualHeight
            Return V(w, h)
        End Get
        Set(value As System.Windows.Vector)
            Width = value.X
            Height = value.Y
        End Set
    End Property
    Protected MustOverride Sub InternalDrawArrowGeometry(context As StreamGeometryContext)
End Class

Public Class SharpRectangle
    Inherits ShapeBase
    Protected Overrides Sub InternalDrawArrowGeometry(context As System.Windows.Media.StreamGeometryContext)
        Dim az As System.Windows.Vector = ActualSize
        Dim w As Double = az.X
        Dim h As Double = az.Y
        Dim r As Double = h / 2
        If w < h Then
            r = w / 2
        End If
        context.BeginFigure(P(0.5D + r, 0), True, False)
        context.LineTo(P(w - r - 1D, 0), True, False)
        context.LineTo(P(w - 1D, h / 2), True, False)
        context.LineTo(P(w - r - 1D, h), True, False)
        context.LineTo(P(0.5D + r, h), True, False)
        context.LineTo(P(0.5D, h / 2), True, False)
        context.LineTo(P(0.5D + r, 0), True, False)
        context.Close()
    End Sub
End Class

Public Class RightSharpRectangle
    Inherits ShapeBase
    Protected Overrides Sub InternalDrawArrowGeometry(context As System.Windows.Media.StreamGeometryContext)
        Dim az As System.Windows.Vector = ActualSize
        Dim w As Double = az.X
        Dim h As Double = az.Y
        Dim r As Double = h / 2
        If w < h Then
            r = w / 2
        End If
        context.BeginFigure(P(0.5D, 0), True, False)
        context.LineTo(P(w - r - 1D, 0), True, False)
        context.LineTo(P(w - 1D, h / 2), True, False)
        context.LineTo(P(w - r - 1D, h), True, False)
        context.LineTo(P(0.5D, h), True, False)
        context.LineTo(P(0.5D, 0), True, False)
        context.Close()
    End Sub
End Class

Public Class LeftSharpRectangle
    Inherits ShapeBase
    Protected Overrides Sub InternalDrawArrowGeometry(context As System.Windows.Media.StreamGeometryContext)
        Dim az As System.Windows.Vector = ActualSize
        Dim w As Double = az.X
        Dim h As Double = az.Y
        Dim r As Double = h / 2
        If w < h Then
            r = w / 2
        End If
        context.BeginFigure(P(0.5D + r, 0), True, False)
        context.LineTo(P(w - 1D, 0), True, False)
        context.LineTo(P(w - 1D, h), True, False)
        context.LineTo(P(0.5D + r, h), True, False)
        context.LineTo(P(0.5D, h / 2), True, False)
        context.LineTo(P(0.5D + r, 0), True, False)
        context.Close()
    End Sub
End Class

Public Class RightArrow
    Inherits ShapeBase
    Protected Overrides Sub InternalDrawArrowGeometry(context As System.Windows.Media.StreamGeometryContext)
        Dim az As System.Windows.Vector = ActualSize
        Dim w As Double = az.X
        Dim h As Double = az.Y
        Dim r As Double = h / 2
        Dim h1 As Double = h / 3
        Dim h2 As Double = h * 2 / 3
        If w < h Then
            r = w / 2
        End If
        context.BeginFigure(P(0.5D, h1), True, False)
        context.LineTo(P(w - r - 1D, h1), True, False)
        context.LineTo(P(w - r - 1D, 0), True, False)
        context.LineTo(P(w - 1D, h / 2), True, False)
        context.LineTo(P(w - r - 1D, h), True, False)
        context.LineTo(P(w - r - 1D, h2), True, False)
        context.LineTo(P(0.5D, h2), True, False)
        context.LineTo(P(0.5D, h1), True, False)
        context.Close()
    End Sub
End Class

Public Class LeftArrow
    Inherits ShapeBase
    Protected Overrides Sub InternalDrawArrowGeometry(context As System.Windows.Media.StreamGeometryContext)
        Dim az As System.Windows.Vector = ActualSize
        Dim w As Double = az.X
        Dim h As Double = az.Y
        Dim r As Double = h / 2
        Dim h1 As Double = h / 3
        Dim h2 As Double = h * 2 / 3
        If w < h Then
            r = w / 2
        End If
        With context
            .BeginFigure(0.5D, h / 2)
            .LineTo(0.5D + r, 0)
            .LineTo(0.5D + r, h1)
            .LineTo(w - 1D, h1)
            .LineTo(w - 1D, h2)
            .LineTo(0.5D + r, h2)
            .LineTo(0.5D + r, h)
            .LineTo(0.5D, h / 2)
            .CloseFigure()
        End With
    End Sub
End Class

Public Module ContextExtensions
    <System.Runtime.CompilerServices.Extension()> Sub BeginFigure(context As System.Windows.Media.StreamGeometryContext, x As Double, y As Double)
        context.BeginFigure(P(x, y), True, False)
    End Sub
    <System.Runtime.CompilerServices.Extension()> Sub LineTo(context As System.Windows.Media.StreamGeometryContext, x As Double, y As Double)
        context.LineTo(P(x, y), True, False)
    End Sub
    <System.Runtime.CompilerServices.Extension()> Sub CloseFigure(context As System.Windows.Media.StreamGeometryContext)
        context.Close()
    End Sub
    <System.Runtime.CompilerServices.Extension()> Sub ArcTo(context As System.Windows.Media.StreamGeometryContext, x As Double, y As Double, w As Double, h As Double)
        context.ArcTo(P(x, y), New Size(w, h), 0D, False, System.Windows.Media.SweepDirection.Clockwise, True, False)
    End Sub

End Module