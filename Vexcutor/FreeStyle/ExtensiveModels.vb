Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes
<Shallow()>
Public Class TextRectangleBase
    Inherits TextShape
    Protected WithEvents ShapeBase As Shape
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New Rectangle
            'Children.Add(New Rectangle With {.Stroke = Brushes.Black, .HorizontalAlignment = Windows.HorizontalAlignment.Stretch, .VerticalAlignment = Windows.VerticalAlignment.Center})
            Return ShapeBase
        End Get
    End Property
    Private Sub TextArrow_SizeChanged(sender As Object, e As System.Windows.SizeChangedEventArgs) Handles Me.SizeChanged
        Dim w As Double
        Dim h As Double
        w = Width
        If w = 0D Then w = ActualWidth
        h = Height
        If h = 0D Then h = ActualHeight
        Dim vw = V(w, 0)
        Dim vh = V(0, h)
        Dim vd = V(h, 0)
        'Dim pc As New PointCollection From {vh / 3, vh / 3 + vw - vd / 2, vw - vd / 2, vw + vh / 2, vw - vd / 2 + vh, vh * (2 / 3) + vw - vd / 2, vh * (2 / 3)}
        ShapeBase.Width = w
        ShapeBase.Height = h
    End Sub
    <Menu(), Act("R")> Public Property RotationAngle As Double
        Get
            Return MyBase.Rotation
        End Get
        Set(value As Double)
            Dim az As System.Windows.Vector = ActualSize / 2
            Dim ct As System.Windows.Vector = CanvasLocation + az.RotateByDegree(Rotation)
            CanvasLocation = ct - az.RotateByDegree(value)
            Rotation = value
            RotateText(value)
            PassMovement()
        End Set
    End Property
    Protected Overrides ReadOnly Property RotationCenter As System.Windows.Vector
        Get
            Return V(0D, 0D)
        End Get
    End Property
    Public Property RotationPoint() As System.Windows.Point
        Get
            Dim ag As Double = Rotation
            Dim az As System.Windows.Vector = ActualSize / 2
            Dim ct As System.Windows.Vector = CanvasLocation + az.RotateByDegree(ag)
            Dim vx = V(0, -az.Y)
            Return ct + vx.RotateByDegree(ag)
        End Get
        Set(value As System.Windows.Point)
            Dim az As System.Windows.Vector = ActualSize / 2
            Dim ct As System.Windows.Vector = CanvasLocation + az.RotateByDegree(Rotation)
            'Dim vx As System.Windows.Vector = V(0, az.Y / 2)
            Dim ag As Double = (value.AsVector - ct).AngleByDegree + 90D
            CanvasLocation = ct - az.RotateByDegree(ag)
            Rotation = ag
            RotateText(ag)
            PassMovement()
        End Set
    End Property
    <Act("S")> Public Property RealSize() As System.Windows.Point
        Get
            Return ActualSize
        End Get
        Set(value As System.Windows.Point)
            Dim ct As System.Windows.Vector = Location
            ActualSize = value
            CanvasLocation = ct - (value.AsVector / 2).RotateByDegree(Rotation)
            PassMovement()
        End Set
    End Property

    <Act("O")> Public Property OLocation() As System.Windows.Point
        Get
            Return CanvasLocation + (ActualSize / 2).RotateByDegree(Rotation) '- (ActualSize / 2).RotateByDegree(Rotation)
        End Get
        Set(value As System.Windows.Point)
            'Dim ct As System.Windows.Vector = value + (ActualSize / 2).RotateByDegree(Rotation)
            CanvasLocation = value - (ActualSize / 2).RotateByDegree(Rotation)
            RotateText(Rotation)
            PassMovement()
        End Set
    End Property
    Public Property RSize() As System.Windows.Point
        Get
            Dim az As System.Windows.Vector = ActualSize
            Return CanvasLocation + az.RotateByDegree(Rotation)
        End Get
        Set(value As System.Windows.Point)
            Dim az As System.Windows.Vector = ActualSize / 2
            Dim ct As System.Windows.Vector = OLocation
            az = (value.AsVector - ct).RotateByDegree(-Rotation)
            If az.X < 6D Then az.X = 6D
            If az.Y < 6D Then az.Y = 6D
            ActualSize = az * 2
            OLocation = ct
            Rotation = Rotation
            RotateText(Rotation)
            PassMovement()
        End Set
    End Property
    Public Event DependencyMoved(sender As Object, e As System.EventArgs)
    Public Sub Straight()
        Dim u As Integer = Math.Round(Rotation / 15)
        Dim ct As System.Windows.Vector = OLocation
        Rotation = u * 15D
        OLocation = ct
        RotateText(Rotation)
        PassMovement()
    End Sub
    Public Property AppearancePoint(Optional sender As IActor = Nothing) As System.Windows.Point
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return CanvasLocation + az.RotateByDegree(Rotation) + V(az.X, -az.Y).RotateByDegree(Rotation)
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property
    <LateLoad()> Public Property Bindings As ShallowDictionary(Of ControlPointMapping, Integer)
        Get
            Return BindingPoints
        End Get
        Set(value As ShallowDictionary(Of ControlPointMapping, Integer))
            BindingPoints = value
        End Set
    End Property
    Private BindingPoints As New ShallowDictionary(Of ControlPointMapping, Integer)
    Private ReadOnly Property LeftPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return Location + V(-az.X, 0).RotateByDegree(Rotation)
        End Get
    End Property
    Private ReadOnly Property RightPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return Location + V(az.X, 0).RotateByDegree(Rotation)
        End Get
    End Property
    Private ReadOnly Property TopPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return Location + V(0, -az.Y).RotateByDegree(Rotation)
        End Get
    End Property
    Private ReadOnly Property BottomPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return Location + V(0, az.Y).RotateByDegree(Rotation)
        End Get
    End Property
    Public Overrides Function BindPoint(cp As ControlPoint) As Boolean
        If _Adorners.ContainsValue(cp) Then Return False
        Dim p As System.Windows.Vector = cp.Position
        If (p - LeftPoint).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), 1)
            cp.PositionMove(New List(Of Object) From {Me}, LeftPoint, True)
            Return True
        End If
        If (p - RightPoint).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), 2)
            cp.PositionMove(New List(Of Object) From {Me}, RightPoint, True)
            Return True
        End If
        If (p - TopPoint).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), 3)
            cp.PositionMove(New List(Of Object) From {Me}, TopPoint, True)
            Return True
        End If
        If (p - BottomPoint).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), 4)
            cp.PositionMove(New List(Of Object) From {Me}, BottomPoint, True)
            Return True
        End If
        Return False
    End Function
    Public Overrides Sub ReleasePoint(cp As ControlPoint)
        Dim keys = BindingPoints.Where(Function(kvp) kvp.Key.Point Is cp).Select(Function(kvp) kvp.Key).ToArray
        For Each Key In keys
            If BindingPoints.ContainsKey(Key) Then BindingPoints.Remove(Key)
        Next
    End Sub
    Public Overrides Sub ProcessBindingPoints()
        Dim lp As System.Windows.Vector = LeftPoint
        Dim rp As System.Windows.Vector = RightPoint
        Dim tp As System.Windows.Vector = TopPoint
        Dim bp As System.Windows.Vector = BottomPoint
        For Each cp In BindingPoints.Keys
            If TemperatorySender IsNot cp.Target Then
                Select Case BindingPoints(cp)
                    Case 1
                        cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = lp
                    Case 2
                        cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = rp
                    Case 3
                        cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = tp
                    Case 4
                        cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = bp
                End Select
            End If
        Next
    End Sub
    Private _Adorners As New Dictionary(Of String, ControlPoint) From {
{"O", New ControlPoint("O", Me)},
 {"S", New ControlPoint("S", Me)},
 {"R", New ControlPoint("R", Me, Nothing, AddressOf Straight) With {.Fill = Brushes.Green}},
 {"C", New ControlPoint("C", Me, AddressOf OnMenu) With {.Fill = Brushes.Pink}}
}
    Public Sub Square()
        Dim az As System.Windows.Vector = ActualSize
        Dim min As Double = Math.Min(az.X, az.Y)
        Dim ct As System.Windows.Vector = Location
        ActualSize = az * 2
        Location = ct
        Rotation = Rotation
        RotateText(Rotation)
        PassMovement()
    End Sub
    Public Overrides ReadOnly Property Adorners As System.Collections.Generic.Dictionary(Of String, ControlPoint)
        Get
            Return _Adorners
        End Get
    End Property
    Public Overrides Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
        Get
            Select Case aID
                Case "O"
                    Return OLocation
                Case "S"
                    Return RSize
                Case "R"
                    Return RotationPoint
                Case "C"
                    Return AppearancePoint
            End Select
        End Get
        Set(value As System.Windows.Vector)
            TemperatorySender = sender
            Select Case aID
                Case "O"
                    OLocation = value
                Case "S"
                    RSize = value
                Case "R"
                    RotationPoint = value
                Case "C"
                    _Adorners(aID).Move()
            End Select
            If sender IsNot _Adorners(aID) Then _Adorners(aID).Move()
            TemperatorySender = Nothing
        End Set
    End Property
    Public Overrides Sub PassMovement()
        For Each cp As ControlPoint In _Adorners.Values
            If TemperatorySender IsNot cp Then cp.Move()
        Next
        If TemperatorySender Is Nothing Then TemperatorySender = Me
        ProcessBindingPoints()
        TemperatorySender = Nothing
    End Sub
End Class

<Shallow()>
Public MustInherit Class TextLineBase
    Inherits TextShape
    Protected WithEvents ShapeBase As Shape
    Private Sub TextArrow_SizeChanged(sender As Object, e As System.Windows.SizeChangedEventArgs) Handles Me.SizeChanged
        Dim w As Double
        Dim h As Double
        w = Width
        If w = 0D Then w = ActualWidth
        h = Height
        If h = 0D Then h = ActualHeight
        Dim vw = V(w, 0)
        Dim vh = V(0, h)
        Dim vd = V(h, 0)
        'Dim pc As New PointCollection From {vh / 3, vh / 3 + vw - vd / 2, vw - vd / 2, vw + vh / 2, vw - vd / 2 + vh, vh * (2 / 3) + vw - vd / 2, vh * (2 / 3)}
        'ShapeBase.Points = pc
    End Sub
    <Menu(), Act("H")> Public Property ArrowHeight As Double
        Get
            Return Me.ActualSize.Y
        End Get
        Set(value As Double)
            Dim X As System.Windows.Vector = LinearX
            Dim Y As System.Windows.Vector = LinearY
            Dim ct = (X + Y) / 2
            Dim ag As Double = (Y - X).AngleByDegree
            Dim az As System.Windows.Vector = ActualSize
            az.Y = value
            ActualSize = az
            CanvasLocation = ct - az / 2
            Rotation = ag
            RotateText(ag)
            PassMovement()
        End Set
    End Property
    Public Property LinearHeight() As System.Windows.Point
        Get
            Dim ag As Double = Rotation
            Dim az As System.Windows.Vector = ActualSize
            Dim ct As System.Windows.Vector = CanvasLocation + az / 2
            Dim vx = V(0, az.Y / 2)
            Return ct + vx.RotateByDegree(ag)
        End Get
        Set(value As System.Windows.Point)
            Dim X As System.Windows.Vector = LinearX
            Dim Y As System.Windows.Vector = LinearY
            Dim ct = (X + Y) / 2
            Dim ag As Double = (Y - X).AngleByDegree
            Dim az As System.Windows.Vector = ActualSize
            az.Y = CType((value - ct), System.Windows.Vector).Length * 2
            ActualSize = az
            CanvasLocation = ct - az / 2
            Rotation = ag
            RotateText(ag)
            PassMovement()
        End Set
    End Property

    <Act("X")> Public Property LinearX() As System.Windows.Point
        Get
            Dim ag As Double = Rotation
            Dim az As System.Windows.Vector = ActualSize
            Dim ct As System.Windows.Vector = CanvasLocation + az / 2
            Dim vx = V(-az.X / 2, 0)
            Return ct + vx.RotateByDegree(ag)
        End Get
        Set(value As System.Windows.Point)
            Dim X As System.Windows.Vector = value
            Dim Y As System.Windows.Vector = LinearY
            Dim ct = (X + Y) / 2
            Dim ag As Double = (Y - X).AngleByDegree
            Dim az As System.Windows.Vector = ActualSize
            az.X = (Y - X).Length
            ActualSize = az
            CanvasLocation = ct - az / 2
            Rotation = ag
            RotateText(ag)
            PassMovement()
        End Set
    End Property
    <Act("Y")> Public Property LinearY() As System.Windows.Point
        Get
            Dim ag As Double = Rotation
            Dim az As System.Windows.Vector = ActualSize
            Dim ct As System.Windows.Vector = CanvasLocation + az / 2
            Dim vx = V(az.X / 2, 0)
            Return ct + vx.RotateByDegree(ag)
        End Get
        Set(value As System.Windows.Point)
            'If sender Is Me Then Exit Property
            Dim X As System.Windows.Vector = LinearX
            Dim Y As System.Windows.Vector = value
            Dim ct = (X + Y) / 2
            Dim ag As Double = (Y - X).AngleByDegree
            Dim az As System.Windows.Vector = ActualSize
            az.X = (Y - X).Length
            ActualSize = az
            CanvasLocation = ct - az / 2
            Rotation = ag
            RotateText(ag)
            PassMovement()
        End Set
    End Property
    Public Event DependencyMoved(sender As Object, e As System.EventArgs)
    Public Sub AutoHeight()
        Dim X As System.Windows.Vector = LinearX
        Dim Y As System.Windows.Vector = LinearY
        Dim ct = (X + Y) / 2
        Dim ag As Double = (Y - X).AngleByDegree
        Dim az As System.Windows.Vector = ActualSize
        az.Y = 72
        ActualSize = az
        CanvasLocation = ct - az / 2
        Rotation = ag
        RotateText(ag)
        PassMovement()
        'RaiseEvent DependencyMoved(Me, New EventArgs)
    End Sub
    '<Act()> Public Property X1 As Double
    '<Act()> Public Property X2 As Double
    '<Act()> Public Property Y1 As Double
    '<Act()> Public Property Y2 As Double


    Public Property AppearancePoint(Optional sender As IActor = Nothing) As System.Windows.Point
        Get
            Dim ag As Double = Rotation
            Dim az As System.Windows.Vector = ActualSize
            Dim vx = V(0, az.Y / 2)
            Dim ct As System.Windows.Vector = LinearX + LinearY
            Return ct / 2 - vx.RotateByDegree(ag)
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property
    <LateLoad()> Public Property Bindings As ShallowDictionary(Of ControlPointMapping, Integer)
        Get
            Return BindingPoints
        End Get
        Set(value As ShallowDictionary(Of ControlPointMapping, Integer))
            BindingPoints = value
        End Set
    End Property
    Private BindingPoints As New ShallowDictionary(Of ControlPointMapping, Integer)
    '<LateLoad()> Private BindingPoints As New ShallowDictionary(Of ControlPointMapping, Integer)
    Public Overrides Function BindPoint(cp As ControlPoint) As Boolean
        If _Adorners.ContainsValue(cp) Then Return False
        Dim p As System.Windows.Vector = cp.Position
        If (p - LinearX.AsVector).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), 1)
            cp.PositionMove(New List(Of Object) From {Me}, LinearX, True)
            Return True
        End If
        If (p - LinearY.AsVector).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), -1)
            cp.PositionMove(New List(Of Object) From {Me}, LinearY, True)
            Return True
        End If
        Return False
    End Function
    Public Overrides Sub ReleasePoint(cp As ControlPoint)
        Dim keys = BindingPoints.Where(Function(kvp) kvp.Key.Point Is cp).Select(Function(kvp) kvp.Key).ToArray
        For Each Key In keys
            If BindingPoints.ContainsKey(Key) Then BindingPoints.Remove(Key)
        Next
    End Sub
    Public Overrides Sub ProcessBindingPoints()
        Dim lx As System.Windows.Vector = LinearX
        Dim ly As System.Windows.Vector = LinearY
        For Each cp In BindingPoints.Keys
            If TemperatorySender IsNot cp.Target Then
                If BindingPoints(cp) = 1 Then
                    cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = lx
                ElseIf BindingPoints(cp) = -1 Then
                    cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = ly
                End If
            End If
        Next
    End Sub
    Private _Adorners As New Dictionary(Of String, ControlPoint) From {
{"X", New ControlPoint("X", Me)},
 {"Y", New ControlPoint("Y", Me)},
 {"H", New ControlPoint("H", Me, Nothing, AddressOf AutoHeight) With {.Fill = Brushes.Green}},
 {"C", New ControlPoint("C", Me, AddressOf OnMenu) With {.Fill = Brushes.Pink}}
}
    Public Overrides ReadOnly Property Adorners As System.Collections.Generic.Dictionary(Of String, ControlPoint)
        Get
            Return _Adorners
        End Get
    End Property
    Public Overrides Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
        Get
            Select Case aID
                Case "X"
                    Return LinearX
                Case "Y"
                    Return LinearY
                Case "H"
                    Return LinearHeight
                Case "C"
                    Return AppearancePoint
            End Select
        End Get
        Set(value As System.Windows.Vector)
            TemperatorySender = sender
            Select Case aID
                Case "X"
                    LinearX = value
                Case "Y"
                    LinearY = value
                Case "H"
                    LinearHeight = value
                Case "C"
                    _Adorners(aID).Move()
            End Select
            If sender IsNot _Adorners(aID) Then _Adorners(aID).Move()
            TemperatorySender = Nothing
        End Set
    End Property
    Public Overrides Sub PassMovement()
        For Each cp As ControlPoint In _Adorners.Values
            If TemperatorySender IsNot cp Then cp.Move()
        Next
        If TemperatorySender Is Nothing Then TemperatorySender = Me
        ProcessBindingPoints()
        TemperatorySender = Nothing
    End Sub

End Class

Public Class TextEllipse
    Inherits TextRectangleBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New Ellipse
            Return ShapeBase
        End Get
    End Property
End Class
Public Class TextDoubleRoundRectangle
    Inherits TextRectangleBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New RoundEndRectangle
            Return ShapeBase
        End Get
    End Property
End Class
Public Class TextRightRoundRectangle
    Inherits TextRectangleBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New RightRoundEndRectangle
            Return ShapeBase
        End Get
    End Property
End Class
Public Class TextLeftRoundRectangle
    Inherits TextRectangleBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New LeftRoundEndRectangle
            Return ShapeBase
        End Get
    End Property
End Class
Public Class TextSharpRectangle
    Inherits TextRectangleBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New SharpRectangle
            Return ShapeBase
        End Get
    End Property
End Class
Public Class TextRightSharpRectangle
    Inherits TextRectangleBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New RightSharpRectangle
            Return ShapeBase
        End Get
    End Property
End Class
Public Class TextLeftSharpRectangle
    Inherits TextRectangleBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New LeftSharpRectangle
            Return ShapeBase
        End Get
    End Property
End Class


Public Class TextRectangleLine
    Inherits TextLineBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New Rectangle
            Return ShapeBase
        End Get
    End Property
End Class
Public Class TextEllipseLine
    Inherits TextLineBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New Ellipse
            Return ShapeBase
        End Get
    End Property
End Class
Public Class TextDoubleRoundLine
    Inherits TextLineBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New RoundEndRectangle
            Return ShapeBase
        End Get
    End Property
End Class
Public Class TextRightRoundLine
    Inherits TextLineBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New RightRoundEndRectangle
            Return ShapeBase
        End Get
    End Property
End Class
Public Class TextLeftRoundLine
    Inherits TextLineBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New LeftRoundEndRectangle
            Return ShapeBase
        End Get
    End Property
End Class

Public Class TextSharpLine
    Inherits TextLineBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New SharpRectangle
            Return ShapeBase
        End Get
    End Property
End Class
Public Class TextRightSharpLine
    Inherits TextLineBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New RightSharpRectangle
            Return ShapeBase
        End Get
    End Property
End Class
Public Class TextLeftSharpLine
    Inherits TextLineBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New LeftSharpRectangle
            Return ShapeBase
        End Get
    End Property
End Class

Public Class TextRightArrow
    Inherits TextRectangleBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New RightArrow
            Return ShapeBase
        End Get
    End Property
End Class
Public Class TextLeftArrow
    Inherits TextRectangleBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New LeftArrow
            Return ShapeBase
        End Get
    End Property
End Class

Public Class TextRightArrowLine
    Inherits TextLineBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New RightArrow
            Return ShapeBase
        End Get
    End Property
End Class
Public Class TextLeftArrowLine
    Inherits TextLineBase
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New LeftArrow
            Return ShapeBase
        End Get
    End Property
End Class

Public Class TextRoundCornerRectangle
    Inherits TextShape
    Protected WithEvents ShapeBase As RoundCornorRectangle
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New RoundCornorRectangle
            'Children.Add(New Rectangle With {.Stroke = Brushes.Black, .HorizontalAlignment = Windows.HorizontalAlignment.Stretch, .VerticalAlignment = Windows.VerticalAlignment.Center})
            Return ShapeBase
        End Get
    End Property
    Private Sub TextArrow_SizeChanged(sender As Object, e As System.Windows.SizeChangedEventArgs) Handles Me.SizeChanged
        Dim w As Double
        Dim h As Double
        w = Width
        If w = 0D Then w = ActualWidth
        h = Height
        If h = 0D Then h = ActualHeight
        Dim vw = V(w, 0)
        Dim vh = V(0, h)
        Dim vd = V(h, 0)
        'Dim pc As New PointCollection From {vh / 3, vh / 3 + vw - vd / 2, vw - vd / 2, vw + vh / 2, vw - vd / 2 + vh, vh * (2 / 3) + vw - vd / 2, vh * (2 / 3)}
        ShapeBase.Width = w
        ShapeBase.Height = h
    End Sub
    <Menu(), Act("R")> Public Property RotationAngle As Double
        Get
            Return MyBase.Rotation
        End Get
        Set(value As Double)
            Dim az As System.Windows.Vector = ActualSize / 2
            Dim ct As System.Windows.Vector = CanvasLocation + az.RotateByDegree(Rotation)
            CanvasLocation = ct - az.RotateByDegree(value)
            Rotation = value
            RotateText(value)
            PassMovement()
        End Set
    End Property
    Protected Overrides ReadOnly Property RotationCenter As System.Windows.Vector
        Get
            Return V(0D, 0D)
        End Get
    End Property
    Public Property RotationPoint() As System.Windows.Point
        Get
            Dim ag As Double = Rotation
            Dim az As System.Windows.Vector = ActualSize / 2
            Dim ct As System.Windows.Vector = CanvasLocation + az.RotateByDegree(ag)
            Dim vx = V(0, -az.Y)
            Return ct + vx.RotateByDegree(ag)
        End Get
        Set(value As System.Windows.Point)
            Dim az As System.Windows.Vector = ActualSize / 2
            Dim ct As System.Windows.Vector = CanvasLocation + az.RotateByDegree(Rotation)
            'Dim vx As System.Windows.Vector = V(0, az.Y / 2)
            Dim ag As Double = (value.AsVector - ct).AngleByDegree + 90D
            CanvasLocation = ct - az.RotateByDegree(ag)
            Rotation = ag
            RotateText(ag)
            PassMovement()
        End Set
    End Property
    <Act("S")> Public Property RealSize() As System.Windows.Point
        Get
            Return ActualSize
        End Get
        Set(value As System.Windows.Point)
            Dim ct As System.Windows.Vector = Location
            ActualSize = value
            CanvasLocation = ct - (value.AsVector / 2).RotateByDegree(Rotation)
            PassMovement()
        End Set
    End Property

    <Act("O")> Public Property OLocation() As System.Windows.Point
        Get
            Return CanvasLocation + (ActualSize / 2).RotateByDegree(Rotation) '- (ActualSize / 2).RotateByDegree(Rotation)
        End Get
        Set(value As System.Windows.Point)
            'Dim ct As System.Windows.Vector = value + (ActualSize / 2).RotateByDegree(Rotation)
            CanvasLocation = value - (ActualSize / 2).RotateByDegree(Rotation)
            RotateText(Rotation)
            PassMovement()
        End Set
    End Property
    Public Property RSize() As System.Windows.Point
        Get
            Dim az As System.Windows.Vector = ActualSize
            Return CanvasLocation + az.RotateByDegree(Rotation)
        End Get
        Set(value As System.Windows.Point)
            Dim az As System.Windows.Vector = ActualSize / 2
            Dim ct As System.Windows.Vector = OLocation
            az = (value.AsVector - ct).RotateByDegree(-Rotation)
            If az.X < 6D Then az.X = 6D
            If az.Y < 6D Then az.Y = 6D
            ActualSize = az * 2
            OLocation = ct
            Rotation = Rotation
            RotateText(Rotation)
            PassMovement()
        End Set
    End Property
    Public Event DependencyMoved(sender As Object, e As System.EventArgs)
    Public Sub Straight()
        Dim u As Integer = Math.Round(Rotation / 15)
        Rotation = u * 15D
        RotateText(Rotation)
        PassMovement()
    End Sub
    Public Property AppearancePoint(Optional sender As IActor = Nothing) As System.Windows.Point
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return CanvasLocation + az.RotateByDegree(Rotation) + V(az.X, -az.Y).RotateByDegree(Rotation)
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property
    <LateLoad()> Public Property Bindings As ShallowDictionary(Of ControlPointMapping, Integer)
        Get
            Return BindingPoints
        End Get
        Set(value As ShallowDictionary(Of ControlPointMapping, Integer))
            BindingPoints = value
        End Set
    End Property
    Private BindingPoints As New ShallowDictionary(Of ControlPointMapping, Integer)
    Private ReadOnly Property LeftPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return Location + V(-az.X, 0).RotateByDegree(Rotation)
        End Get
    End Property
    Private ReadOnly Property RightPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return Location + V(az.X, 0).RotateByDegree(Rotation)
        End Get
    End Property
    Private ReadOnly Property TopPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return Location + V(0, -az.Y).RotateByDegree(Rotation)
        End Get
    End Property
    Private ReadOnly Property BottomPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return Location + V(0, az.Y).RotateByDegree(Rotation)
        End Get
    End Property
    Public Overrides Function BindPoint(cp As ControlPoint) As Boolean
        If _Adorners.ContainsValue(cp) Then Return False
        Dim p As System.Windows.Vector = cp.Position
        If (p - LeftPoint).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), 1)
            cp.PositionMove(New List(Of Object) From {Me}, LeftPoint, True)
            Return True
        End If
        If (p - RightPoint).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), 2)
            cp.PositionMove(New List(Of Object) From {Me}, RightPoint, True)
            Return True
        End If
        If (p - TopPoint).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), 3)
            cp.PositionMove(New List(Of Object) From {Me}, TopPoint, True)
            Return True
        End If
        If (p - BottomPoint).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), 4)
            cp.PositionMove(New List(Of Object) From {Me}, BottomPoint, True)
            Return True
        End If
        Return False
    End Function
    Public Overrides Sub ReleasePoint(cp As ControlPoint)
        Dim keys = BindingPoints.Where(Function(kvp) kvp.Key.Point Is cp).Select(Function(kvp) kvp.Key).ToArray
        For Each Key In keys
            If BindingPoints.ContainsKey(Key) Then BindingPoints.Remove(Key)
        Next
    End Sub
    Public Overrides Sub ProcessBindingPoints()
        Dim lp As System.Windows.Vector = LeftPoint
        Dim rp As System.Windows.Vector = RightPoint
        Dim tp As System.Windows.Vector = TopPoint
        Dim bp As System.Windows.Vector = BottomPoint
        For Each cp In BindingPoints.Keys
            If TemperatorySender IsNot cp.Target Then
                Select Case BindingPoints(cp)
                    Case 1
                        cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = lp
                    Case 2
                        cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = rp
                    Case 3
                        cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = tp
                    Case 4
                        cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = bp
                End Select
            End If
        Next
    End Sub
    Private _Adorners As New Dictionary(Of String, ControlPoint) From {
{"O", New ControlPoint("O", Me)},
 {"S", New ControlPoint("S", Me)},
 {"R", New ControlPoint("R", Me, Nothing, AddressOf Straight) With {.Fill = Brushes.Green}},
 {"D", New ControlPoint("D", Me) With {.Fill = Brushes.Cyan}},
 {"C", New ControlPoint("C", Me, AddressOf OnMenu) With {.Fill = Brushes.Pink}}
}
    <Act("D")> Public Property Cornor As System.Windows.Point
        Get
            Dim ct As System.Windows.Vector = OLocation
            Dim az As System.Windows.Vector = ActualSize / 2
            Return ct + V(ShapeBase.Depth, az.Y).RotateByDegree(Rotation)
        End Get
        Set(value As System.Windows.Point)
            Dim ct As System.Windows.Vector = OLocation
            Dim az As System.Windows.Vector = ActualSize / 2
            Dim u As System.Windows.Vector = ct + V(0, az.Y).RotateByDegree(Rotation)
            Dim k As System.Windows.Vector = (value.AsVector - u).RotateByDegree(-Rotation)
            ShapeBase.Depth = k.X
        End Set
    End Property
    Public Sub Square()
        Dim az As System.Windows.Vector = ActualSize
        Dim min As Double = Math.Min(az.X, az.Y)
        Dim ct As System.Windows.Vector = Location
        ActualSize = az * 2
        Location = ct
        Rotation = Rotation
        RotateText(Rotation)
        PassMovement()
    End Sub
    Public Overrides ReadOnly Property Adorners As System.Collections.Generic.Dictionary(Of String, ControlPoint)
        Get
            Return _Adorners
        End Get
    End Property
    Public Overrides Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
        Get
            Select Case aID
                Case "O"
                    Return OLocation
                Case "S"
                    Return RSize
                Case "D"
                    Return Cornor
                Case "R"
                    Return RotationPoint
                Case "C"
                    Return AppearancePoint
            End Select
        End Get
        Set(value As System.Windows.Vector)
            TemperatorySender = sender
            Select Case aID
                Case "O"
                    OLocation = value
                Case "S"
                    RSize = value
                Case "D"
                    Cornor = value
                Case "R"
                    RotationPoint = value
                Case "C"
                    _Adorners(aID).Move()
            End Select
            If sender IsNot _Adorners(aID) Then _Adorners(aID).Move()
            TemperatorySender = Nothing
        End Set
    End Property
    Public Overrides Sub PassMovement()
        For Each cp As ControlPoint In _Adorners.Values
            If TemperatorySender IsNot cp Then cp.Move()
        Next
        If TemperatorySender Is Nothing Then TemperatorySender = Me
        ProcessBindingPoints()
        TemperatorySender = Nothing
    End Sub
End Class