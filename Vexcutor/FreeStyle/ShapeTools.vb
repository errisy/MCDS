Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes
<Shallow()>
Public MustInherit Class ShapeToolBase
    Implements IActor
    Public MustOverride ReadOnly Property LayoutShape As LayoutShapeBase
    Protected TemperatorySender As Object
    'Protected LockedLocation As System.Windows.Vector
    Protected Locked As Boolean = False
    Public Property Location() As System.Windows.Vector Implements IActor.Location
        Get
            Return LayoutShape.O
        End Get
        Set(value As System.Windows.Vector)
            Dim delta As System.Windows.Vector = value - LayoutShape.O.AsVector
            LayoutShape.O = value
            MovementOffset(delta)
            PassMovement()
        End Set
    End Property
    Public Overridable Sub MovementOffset(delta As System.Windows.Vector)

    End Sub

    <Act("O")> Public Property O() As System.Windows.Point
        Get
            Return LayoutShape.O
        End Get
        Set(value As System.Windows.Point)
            LayoutShape.O = value
            PassMovement()
        End Set
    End Property
    Private _RunTimeVisible As Boolean = True
    <Menu(), Act()> Public Property RunTimeVisible As Double
        Get
            Return IIf(_RunTimeVisible, 1D, -1D)
        End Get
        Set(value As Double)
            _RunTimeVisible = value > 0D
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

    Public Overridable Function BindPoint(cp As ControlPoint) As Boolean Implements IActor.BindPoint
        If _Adorners.ContainsValue(cp) Then Return False
        Return False
    End Function
    Public Overridable Sub ReleasePoint(cp As ControlPoint) Implements IActor.ReleasePoint
        Dim keys = BindingPoints.Where(Function(kvp) kvp.Key.Point Is cp).Select(Function(kvp) kvp.Key).ToArray
        For Each Key In keys
            If BindingPoints.ContainsKey(Key) Then BindingPoints.Remove(Key)
        Next
    End Sub
    Public Overridable Sub ProcessBindingPoints()

    End Sub
    Protected _Adorners As New Dictionary(Of String, ControlPoint) From {
{"O", New ControlPoint("O", Me) With {.Fill = New SolidColorBrush(Color.FromArgb(64, 255, 255, 0))}}
}
    Public MustOverride Sub AddAdorner(position As System.Windows.Point, done As Boolean) Implements IActor.AddAdorner

    Public Overridable ReadOnly Property Adorners As System.Collections.Generic.Dictionary(Of String, ControlPoint) Implements IActor.Adorners
        Get
            Return _Adorners
        End Get
    End Property
    Public MustOverride Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector Implements IActor.AdornerLocation

    Public Overridable Sub PassMovement() Implements IActor.PassMovement
        For Each cp As ControlPoint In _Adorners.Values
            If TemperatorySender IsNot cp Then cp.Move()
        Next
        If TemperatorySender Is Nothing Then TemperatorySender = Me
        ProcessBindingPoints()
        TemperatorySender = Nothing
    End Sub
    Public Sub Activate(sender As Object, e As System.Windows.RoutedEventArgs) Implements IActor.Activate
        If sender Is Me Then Exit Sub
        If _Host IsNot Nothing Then _Host.IActorActivated(Me, Nothing)
        Activate()
        If RelatedActor IsNot Nothing AndAlso sender IsNot RelatedActor Then
            RelatedActor.Activate(sender, e)
        End If
        'RaiseEvent OnActivate(sender, Nothing)
    End Sub
    <Save()> Public RelatedActor As Actor
    Public Sub AddTo(vHost As FreeStage) Implements IActor.AddTo
        Host = vHost
        Dim vT As Type = Me.GetType
        If Not Host.Menus.ContainsKey(vT) Then
            Host.Menus.Add(vT, New AppearanceMenu(vT))
        End If
        IsReadOnly = Not Host.DesignMode
        Host.ElementCollection.Add(Me.LayoutShape.Path)
        Host.Actors.Add(Me)
        For Each cp As ControlPoint In Adorners.Values
            cp.AddToStage()
        Next
        'AddHandler Me.OnActivate, AddressOf Host.IActorActivated
        If RelatedActor Is Nothing Then
            RelatedActor = New Actor With {.RelatedActor = Me}
            Host.RelatedDirector.Crew.Items.Add(RelatedActor)
            AddHandler RelatedActor.OnActivate, AddressOf Activate
        End If
    End Sub
    Public Event AdornerChanged(sender As Object, e As System.EventArgs) Implements IActor.AdornerChanged
    Public Sub Activate()
        For Each cp As ControlPoint In Adorners.Values
            cp.Dragger.Stroke = Brushes.Red
        Next
    End Sub
    Public Sub Deactivate() Implements IActor.Deactivate
        If RelatedActor IsNot Nothing Then RelatedActor.Deactivate()
        For Each cp As ControlPoint In Adorners.Values
            cp.Dragger.Stroke = Brushes.Black
        Next
    End Sub
    <EarlyBind("Stage")> Public Property Host As FreeStage Implements IActor.Host

    Protected _ID As String
    <LateLoad()> Public Property ID As String Implements IActor.ID
        Get
            If _ID Is Nothing Then
                _ID = GetHashCode.ToString
            End If
            Return _ID
        End Get
        Set(value As String)
            If value Is Nothing Then
                _ID = GetHashCode.ToString
            Else
                _ID = value
            End If
            If RelatedActor IsNot Nothing Then RelatedActor.txtName.Text = _ID
        End Set
    End Property

    Public Property IsReadOnly As Boolean Implements IActor.IsReadOnly
        Get

        End Get
        Set(value As Boolean)
            If LayoutShape IsNot Nothing AndAlso LayoutShape.Path IsNot Nothing Then
                If _RunTimeVisible Then
                    LayoutShape.Path.Visibility = Visibility.Visible
                Else
                    If value Then
                        LayoutShape.Path.Visibility = Visibility.Hidden
                    Else
                        LayoutShape.Path.Visibility = Visibility.Visible
                    End If
                End If
                If value Then
                    For Each cp As ControlPoint In Adorners.Values
                        cp.Visibility = Windows.Visibility.Hidden
                    Next
                Else
                    For Each cp As ControlPoint In Adorners.Values
                        cp.Visibility = Windows.Visibility.Visible
                    Next
                End If
            End If
        End Set
    End Property

    Public Sub OnMenu() Implements IActor.OnMenu
        If TypeOf Host Is FreeStage Then
            Dim vT As Type = Me.GetType
            If Not Host.Menus.ContainsKey(vT) Then
                Host.Menus.Add(vT, New AppearanceMenu(vT))
            End If
            Host.Menus(vT).Bind(Me)
            Host.Menus(vT).IsOpen = True
        End If
        'RaiseEvent RequireMenu(Me, Nothing)
    End Sub

    Public Sub Remove() Implements IActor.Remove
        For Each cp As ControlPoint In Adorners.Values
            cp.RemoveFromStage()
        Next
        Host.ElementCollection.Remove(Me.LayoutShape.Path)
        Host.Actors.Remove(Me)
        Host = Nothing
    End Sub

    <Menu(), Act()> Public Property Visible As Double Implements IActor.Visible
        Get
            Return IIf(Me.LayoutShape.Path.Visibility = Windows.Visibility.Visible, 1D, 0D)
        End Get
        Set(value As Double)
            If value > 0 Then
                Me.LayoutShape.Path.Visibility = Windows.Visibility.Visible
                If TypeOf _Host Is FreeStage AndAlso _Host.DesignMode Then
                    For Each cp As ControlPoint In Adorners.Values
                        cp.Visibility = Windows.Visibility.Visible
                    Next
                End If
            Else
                Me.LayoutShape.Path.Visibility = Windows.Visibility.Hidden
                For Each cp As ControlPoint In Adorners.Values
                    cp.Visibility = Windows.Visibility.Hidden
                Next
            End If
        End Set
    End Property
    Public MustOverride Property Size As System.Windows.Vector Implements IActor.Size

    Public Overridable Function AddConnector(position As System.Windows.Point, vActor As System.Tuple(Of IActor, ControlPoint, ControlPoint), done As Boolean) As Tuple(Of IActor, ControlPoint, ControlPoint) Implements IActor.AddConnector

    End Function

End Class


Public MustInherit Class RectangleShapeToolBase
    Inherits ShapeToolBase
    Public Sub New()
        _Adorners.Add("D", New ControlPoint("D", Me) With {.Fill = Brushes.Green})
        _Adorners.Add("H", New ControlPoint("H", Me) With {.Fill = Brushes.Pink})
        _Adorners.Add("C", New ControlPoint("C", Me, AddressOf OnMenu) With {.Fill = Brushes.Red})
    End Sub
    Public MustOverride ReadOnly Property RectShape As RectangleShapeBase
    Public Overrides Sub MovementOffset(delta As System.Windows.Vector)
        RectShape.D = RectShape.D + delta
    End Sub
    <Menu(), Act()> Public Property StrokeThickness As Double
        Get
            Return RectShape.Path.StrokeThickness
        End Get
        Set(value As Double)
            RectShape.Path.StrokeThickness = value
        End Set
    End Property
    <Menu(), Act()> Public Property Effect As Effects.Effect
        Get
            Return RectShape.Path.Effect
        End Get
        Set(value As Effects.Effect)
            RectShape.Path.Effect = value
        End Set
    End Property
    <Menu(), Act()> Public Property Fill As Brush
        Get
            Return RectShape.Path.Fill
        End Get
        Set(value As Brush)
            RectShape.Path.Fill = value
        End Set
    End Property
    <Menu(), Act()> Public Property Stroke As Brush
        Get
            Return RectShape.Path.Stroke
        End Get
        Set(value As Brush)
            RectShape.Path.Stroke = value
        End Set
    End Property
    <Act("D")> Public Property D As System.Windows.Point
        Get
            Return RectShape.D
        End Get
        Set(value As System.Windows.Point)
            RectShape.D = value
            PassMovement()
        End Set
    End Property
    <Act()> Public Property RectHeight As Double
        Get
            Return RectShape.Height
        End Get
        Set(value As Double)
            RectShape.Height = value
            PassMovement()
        End Set
    End Property
    <Act("H")> Public Property H As System.Windows.Point
        Get
            Return RectShape.H
        End Get
        Set(value As System.Windows.Point)
            RectShape.H = value
            PassMovement()
        End Set
    End Property
    Public Property AppearancePoint As System.Windows.Point
        Get
            Dim ss As ScaleSystem = (RectShape.D - RectShape.O).ClockwiseSystem
            ss.O = RectShape.O
            Return ss(0, RectShape.Height)
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property
    Public Overrides Sub AddAdorner(position As System.Windows.Point, done As Boolean)

    End Sub
    Public Overrides Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
        Get
            Select Case aID
                Case "O"
                    Return O
                Case "D"
                    Return D
                Case "H"
                    Return H
                Case "C"
                    Return AppearancePoint
            End Select
        End Get
        Set(value As System.Windows.Vector)
            TemperatorySender = sender
            Select Case aID
                Case "O"
                    O = value
                Case "D"
                    D = value
                Case "H"
                    H = value
            End Select
            TemperatorySender = Nothing
        End Set
    End Property
    Public Overrides ReadOnly Property LayoutShape As LayoutShapeBase
        Get
            Return RectShape
        End Get
    End Property
    Public Overrides Property Size As System.Windows.Vector
        Get

        End Get
        Set(value As System.Windows.Vector)
            RectShape.D = RectShape.O + V(value.X, 0D)
            RectShape.Height = value.Y
            PassMovement()
        End Set
    End Property
End Class

<Shallow()>
Public Class RectangleShapeBase
    Inherits LayoutShapeBase
    Protected _D As System.Windows.Point
    Protected _H As Double
    Public Property Height As Double
        Get
            Return _H
        End Get
        Set(value As Double)
            _H = value
            Update()
        End Set
    End Property
    Public Property Width As Double
        Get
            Return (_D - _O).Length
        End Get
        Set(value As Double)

        End Set
    End Property
    Public Property D As System.Windows.Point
        Get
            Return _D
        End Get
        Set(value As System.Windows.Point)
            _D = value
            Update()
        End Set
    End Property
    Public Property H As System.Windows.Point
        Get
            Dim ss As ScaleSystem = (_D - _O).ClockwiseSystem
            ss.O = _D
            Return ss(0, _H)
        End Get
        Set(value As System.Windows.Point)
            Dim ss As ScaleSystem = (_D - _O).ClockwiseSystem
            ss.O = _D
            _H = ss.Measure(value).Y
            Update()
        End Set
    End Property
    Protected Overrides Sub DefineSegment(context As System.Windows.Media.PathGeometry)
        Dim x = MyBase.Path.Dispatcher.DisableProcessing
        context.Figures.Clear()
        Dim fig As New PathFigure
        fig.IsClosed = True

        Dim ss As ScaleSystem = (_D - _O).ClockwiseSystem
        Dim w As Double = (_D - _O).Length / 2
        Dim h As Double = _H / 2
        ss.O = (_O + _D).AsVector / 2 + ss(0, h)
        'Dim t1 = (_O + _D).AsVector / 2
        Dim ag As Double = (_D - _O).AngleByDegree
        'Dim cO As System.Windows.Vector = t1 + V(_H / 2, 0D).RotateByDegree(ag + 90D)
        'Dim t2 = H.AsVector + (_O - _D) / 2
        Dim sz As New Size(w, h)
        fig.StartPoint = ss(0D, -h)
        fig.Segments.Add(New ArcSegment(ss(w, 0D), sz, ag, False, SweepDirection.Clockwise, True))
        fig.Segments.Add(New ArcSegment(ss(0D, h), sz, ag, False, SweepDirection.Clockwise, True))
        fig.Segments.Add(New ArcSegment(ss(-w, 0D), sz, ag, False, SweepDirection.Clockwise, True))
        fig.Segments.Add(New ArcSegment(ss(0D, -h), sz, ag, False, SweepDirection.Clockwise, True))
        context.Figures.Add(fig)
        x.Dispose()
    End Sub
End Class

<Shallow()>
Public Class EllipseShape
    Inherits RectangleShapeToolBase

    Private _Ellipse As New EllipseShapeX
    Public Overrides ReadOnly Property RectShape As RectangleShapeBase
        Get
            If _Ellipse Is Nothing Then _Ellipse = New EllipseShapeX
            Return _Ellipse
        End Get
    End Property
End Class
<Shallow()>
Public Class EllipseShapeX
    Inherits RectangleShapeBase
    Protected Overrides Sub DefineSegment(context As System.Windows.Media.PathGeometry)
        Dim x = MyBase.Path.Dispatcher.DisableProcessing
        context.Figures.Clear()
        Dim fig As New PathFigure
        fig.IsClosed = True
        Dim ss As ScaleSystem = (_D - _O).ClockwiseSystem
        Dim w As Double = (_D - _O).Length / 2
        Dim h As Double = _H / 2
        ss.O = (_O + _D).AsVector / 2 + ss(0, h)
        'Dim t1 = (_O + _D).AsVector / 2
        Dim ag As Double = (_D - _O).AngleByDegree
        'Dim cO As System.Windows.Vector = t1 + V(_H / 2, 0D).RotateByDegree(ag + 90D)
        'Dim t2 = H.AsVector + (_O - _D) / 2
        Dim sz As New Size(w, h)
        fig.StartPoint = ss(0D, -h)
        fig.Segments.Add(New ArcSegment(ss(w, 0D), sz, ag, False, SweepDirection.Clockwise, True))
        fig.Segments.Add(New ArcSegment(ss(0D, h), sz, ag, False, SweepDirection.Clockwise, True))
        fig.Segments.Add(New ArcSegment(ss(-w, 0D), sz, ag, False, SweepDirection.Clockwise, True))
        fig.Segments.Add(New ArcSegment(ss(0D, -h), sz, ag, False, SweepDirection.Clockwise, True))
        context.Figures.Add(fig)
        x.Dispose()
    End Sub
End Class

<Shallow()>
Public Class RectangleShape
    Inherits RectangleShapeToolBase

    Private _Ellipse As New RectangleShapeX
    Public Overrides ReadOnly Property RectShape As RectangleShapeBase
        Get
            If _Ellipse Is Nothing Then _Ellipse = New RectangleShapeX
            Return _Ellipse
        End Get
    End Property
End Class
<Shallow()>
Public Class RectangleShapeX
    Inherits RectangleShapeBase
    Protected Overrides Sub DefineSegment(context As System.Windows.Media.PathGeometry)
        Dim x = MyBase.Path.Dispatcher.DisableProcessing
        context.Figures.Clear()
        Dim fig As New PathFigure
        fig.IsClosed = True
        Dim ss As ScaleSystem = (_D - _O).ClockwiseSystem
        Dim w As Double = (_D - _O).Length / 2
        Dim h As Double = _H / 2
        ss.O = (_O + _D).AsVector / 2 + ss(0, h)
        fig.StartPoint = ss(-w, -h)
        fig.Segments.Add(New LineSegment(ss(w, -h), True))
        fig.Segments.Add(New LineSegment(ss(w, h), True))
        fig.Segments.Add(New LineSegment(ss(-w, h), True))
        fig.Segments.Add(New LineSegment(ss(-w, -h), True))
        context.Figures.Add(fig)
        x.Dispose()
    End Sub
End Class

<Shallow()>
Public Class RoundRectangleShape
    Inherits RectangleShapeToolBase

    Private _Ellipse As New RoundRectangleShapeX
    Public Sub New()
        MyBase.New()
        _Adorners.Add("R", New ControlPoint("R", Me) With {.Fill = Brushes.Bisque})
    End Sub

    Public Overrides ReadOnly Property RectShape As RectangleShapeBase
        Get
            If _Ellipse Is Nothing Then _Ellipse = New RoundRectangleShapeX
            Return _Ellipse
        End Get
    End Property
    <Act()> Public Property Radius As Double
        Get
            Return _Ellipse.Radius
        End Get
        Set(value As Double)
            _Ellipse.Radius = value
            PassMovement()
        End Set
    End Property
    <Act("R")> Public Property RadiusPoint As System.Windows.Point
        Get
            Dim ss As ScaleSystem = (_Ellipse.D - _Ellipse.O).ClockwiseSystem
            ss.O = _Ellipse.O
            Return ss(0D, _Ellipse.Radius)
        End Get
        Set(value As System.Windows.Point)
            Dim ss As ScaleSystem = (_Ellipse.D - _Ellipse.O).ClockwiseSystem
            ss.O = _Ellipse.O
            _Ellipse.Radius = ss.Measure(value).Y
            PassMovement()
        End Set
    End Property
    Public Overrides Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
        Get
            Select Case aID
                Case "R"
                    Return RadiusPoint
                Case Else
                    Return MyBase.AdornerLocation(sender, aID)
            End Select
        End Get
        Set(value As System.Windows.Vector)
            TemperatorySender = sender
            Select Case aID
                Case "R"
                    RadiusPoint = value
                Case Else
                    MyBase.AdornerLocation(sender, aID) = value
            End Select
            TemperatorySender = Nothing
        End Set
    End Property
End Class
<Shallow()>
Public Class RoundRectangleShapeX
    Inherits RectangleShapeBase
    Private _Radius As Double = 6D
    Public Property Radius As Double
        Get
            Return _Radius
        End Get
        Set(value As Double)
            _Radius = value
            Update()
        End Set
    End Property
    Protected Overrides Sub DefineSegment(context As System.Windows.Media.PathGeometry)
        Dim x = MyBase.Path.Dispatcher.DisableProcessing
        context.Figures.Clear()
        Dim fig As New PathFigure
        fig.IsClosed = True
        Dim ss As ScaleSystem = (_D - _O).ClockwiseSystem
        Dim w As Double = (_D - _O).Length / 2
        Dim h As Double = _H / 2
        Dim r As Double = Math.Max(0D, Math.Min(_Radius, Math.Min(w, h)))
        ss.O = (_O + _D).AsVector / 2 + ss(0, h)
        Dim ag As Double = (_D - _O).AngleByDegree
        Dim sz As New Size(r, r)
        fig.StartPoint = ss(-w + r, -h)
        fig.Segments.Add(New LineSegment(ss(w - r, -h), True))
        fig.Segments.Add(New ArcSegment(ss(w, -h + r), sz, ag, False, SweepDirection.Clockwise, True))
        fig.Segments.Add(New LineSegment(ss(w, h - r), True))
        fig.Segments.Add(New ArcSegment(ss(w - r, h), sz, ag, False, SweepDirection.Clockwise, True))
        fig.Segments.Add(New LineSegment(ss(-w + r, h), True))
        fig.Segments.Add(New ArcSegment(ss(-w, h - r), sz, ag, False, SweepDirection.Clockwise, True))
        fig.Segments.Add(New LineSegment(ss(-w, -h + r), True))
        fig.Segments.Add(New ArcSegment(ss(-w + r, -h), sz, ag, False, SweepDirection.Clockwise, True))
        context.Figures.Add(fig)
        x.Dispose()
    End Sub
End Class