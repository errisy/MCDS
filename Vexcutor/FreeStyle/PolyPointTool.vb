Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes
<Shallow()>
Public Class PolyPointTool
    Inherits PolyPointShape
    Implements IActor


    Public Sub New()
        Me.Path.StrokeThickness = 1D
        Me.Path.Stroke = Brushes.Black
        Begin.TargetPoint = V(0D, 0D)
    End Sub
    Protected TemperatorySender As Object
    'Protected LockedLocation As System.Windows.Vector
    Protected Locked As Boolean = False

    <Act("O")> Public Property Location() As System.Windows.Point
        Get
            Return Begin.TargetPoint
        End Get
        Set(value As System.Windows.Point)
            'Dim ct As System.Windows.Vector = value + (ActualSize / 2).RotateByDegree(Rotation)
            Begin.TargetPoint = value
            Update()
            PassMovement()
        End Set
    End Property
    <Act("D")> Public Property Positions() As ShallowDictionary(Of String, System.Windows.Point)
        Get
            Dim dl As New ShallowDictionary(Of String, System.Windows.Point)
            Dim i As Integer = 0
            For Each d As Draw In Draws
                d.Index = i
                d.ReadPoints(dl)
                i += 1
            Next
            Return dl
        End Get
        Set(value As ShallowDictionary(Of String, System.Windows.Point))
            Dim l As Integer
            Dim i As Integer
            Dim x As String
            For Each Key As String In value.Keys
                l = Key.Length
                i = CInt(Key.Substring(0, l - 1))
                x = Key.Substring(l - 1)
                Draws(i).Point(x) = value(Key)
            Next
            Update()
            PassMovement()
        End Set
    End Property
    'Public Event DependencyMoved(sender As Object, e As System.EventArgs)

    Public Property AppearancePoint(Optional sender As IActor = Nothing) As System.Windows.Point
        Get
            Return Location + V(0, 20)
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

    Public Function BindPoint(cp As ControlPoint) As Boolean Implements IActor.BindPoint
        If _Adorners.ContainsValue(cp) Then Return False
        Return False
    End Function
    Public Sub ReleasePoint(cp As ControlPoint) Implements IActor.ReleasePoint
        Dim keys = BindingPoints.Where(Function(kvp) kvp.Key.Point Is cp).Select(Function(kvp) kvp.Key).ToArray
        For Each Key In keys
            If BindingPoints.ContainsKey(Key) Then BindingPoints.Remove(Key)
        Next
    End Sub
    Public Overridable Sub ProcessBindingPoints()

    End Sub
    Private _Adorners As New Dictionary(Of String, ControlPoint) From {
{"O", New ControlPoint("O", Me) With {.Fill = New SolidColorBrush(Color.FromArgb(64, 255, 255, 0))}},
 {"C", New ControlPoint("C", Me, AddressOf OnMenu) With {.Fill = Brushes.Pink}}
}
    Public Overridable Sub AddAdorner(position As System.Windows.Point, done As Boolean) Implements IActor.AddAdorner
        If done Then
            Dim i As Integer = Draws.Count
            Dim l As New Line() With {.TargetPoint = position, .Index = i}
            Draws.Add(l)
            Dim cp As New ControlPoint(i.ToString + "T", Me)
            _Adorners.Add(cp.ID, cp)
            cp.AddToStage()
            Update()
        End If
    End Sub

    Public Overridable ReadOnly Property Adorners As System.Collections.Generic.Dictionary(Of String, ControlPoint) Implements IActor.Adorners
        Get
            Return _Adorners
        End Get
    End Property
    Public Overridable Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector Implements IActor.AdornerLocation
        Get
            Select Case aID
                Case "O"
                    Return Location
                Case "C"
                    Return AppearancePoint
                Case Else
                    Dim l As Integer
                    Dim i As Integer
                    Dim x As String
                    l = aID.Length
                    i = CInt(aID.Substring(0, l - 1))
                    x = aID.Substring(l - 1)
                    Return Draws(i).Point(x)
            End Select
        End Get
        Set(value As System.Windows.Vector)
            TemperatorySender = sender
            Select Case aID
                Case "O"
                    Location = value

                Case "C"
                    _Adorners(aID).Move()
                Case Else

                    Dim l As Integer
                    Dim i As Integer
                    Dim x As String
                    l = aID.Length
                    i = CInt(aID.Substring(0, l - 1))
                    x = aID.Substring(l - 1)
                    Draws(i).Point(x) = value
                    'Form1.Text = Me.ActualSize.ToString
                    Update()
            End Select
            If sender IsNot _Adorners(aID) Then _Adorners(aID).Move()
            TemperatorySender = Nothing
        End Set
    End Property
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
    <LateLoad()> Public RelatedActor As Actor
    Public Sub AddTo(vHost As FreeStage) Implements IActor.AddTo
        Host = vHost
        Dim vT As Type = Me.GetType
        If Not Host.Menus.ContainsKey(vT) Then
            Host.Menus.Add(vT, New AppearanceMenu(vT))
        End If
        IsReadOnly = Not Host.DesignMode
        'AddHandler Me.RequireMenu, AddressOf Host.OnMenuRequiring
        Host.ElementCollection.Add(Me.Path)
        Host.Actors.Add(Me)
        For Each cp As ControlPoint In Adorners.Values
            cp.AddToStage()
        Next
        RelatedActor = New Actor With {.RelatedActor = Me}
        Host.RelatedDirector.Crew.Items.Add(RelatedActor)

        AddHandler RelatedActor.OnActivate, AddressOf Activate
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

    Public Overridable Property IsReadOnly As Boolean Implements IActor.IsReadOnly
        Get

        End Get
        Set(value As Boolean)
            If value Then
                For Each cp As ControlPoint In Adorners.Values
                    cp.Visibility = Windows.Visibility.Hidden
                Next
            Else
                For Each cp As ControlPoint In Adorners.Values
                    cp.Visibility = Windows.Visibility.Visible
                Next
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
        'RemoveHandler Me.RequireMenu, AddressOf Host.OnMenuRequiring
        Host.ElementCollection.Remove(Me.Path)
        Host.Actors.Remove(Me)
        Host = Nothing
    End Sub

    <Menu(), Act()> Public Property Visible As Double Implements IActor.Visible
        Get
            Return IIf(Me.Path.Visibility = Windows.Visibility.Visible, 1D, 0D)
        End Get
        Set(value As Double)
            If value > 0 Then
                Me.Path.Visibility = Windows.Visibility.Visible
                If TypeOf _Host Is FreeStage AndAlso _Host.DesignMode Then
                    For Each cp As ControlPoint In Adorners.Values
                        cp.Visibility = Windows.Visibility.Visible
                    Next
                End If
            Else
                Me.Path.Visibility = Windows.Visibility.Hidden
                For Each cp As ControlPoint In Adorners.Values
                    cp.Visibility = Windows.Visibility.Hidden
                Next
            End If
        End Set
    End Property

    Public Property Location1 As System.Windows.Vector Implements IActor.Location
        Get

        End Get
        Set(value As System.Windows.Vector)

        End Set
    End Property

    Public Property Size As System.Windows.Vector Implements IActor.Size
        Get

        End Get
        Set(value As System.Windows.Vector)

        End Set
    End Property

    Public Overridable Function AddConnector(position As System.Windows.Point, vActor As System.Tuple(Of IActor, ControlPoint, ControlPoint), done As Boolean) As Tuple(Of IActor, ControlPoint, ControlPoint) Implements IActor.AddConnector

    End Function


End Class

Public MustInherit Class PathShapeBase
    Public Sub New()
        Path.Data = InnerPathGeometry
    End Sub
    Public Path As New Path
    Public InnerPathGeometry As New PathGeometry
    Public Sub Update()
        DefineSegment(InnerPathGeometry)
    End Sub
    Protected MustOverride Sub DefineSegment(context As PathGeometry)
End Class


<Shallow()>
Public Class PolyPointShape
    Inherits PathShapeBase
    <Save()> Public Property Begin As New Begin
    <Save()> Public Property Draws As New ShallowList(Of Draw)
    Protected Overrides Sub DefineSegment(context As PathGeometry)
        Dim x = MyBase.Path.Dispatcher.DisableProcessing
        context.Figures.Clear()
        Dim fig As New PathFigure
        Begin.Draw(fig)
        For Each d As Draw In Draws
            d.Draw(fig)
        Next
        context.Figures.Add(fig)
        x.Dispose()
    End Sub
End Class
<Serializable()>
Public MustInherit Class Draw
    Protected _TargetPoint As System.Windows.Point = P(50, 50)
    Public Index As Integer
    Public Property TargetPoint As System.Windows.Point
        Get
            Return _TargetPoint
        End Get
        Set(value As System.Windows.Point)
            _TargetPoint = value
        End Set
    End Property
    Public MustOverride Sub Draw(contextFigure As System.Windows.Media.PathFigure)
    Public MustOverride Sub ReadPoints(dict As ShallowDictionary(Of String, System.Windows.Point))
    Public MustOverride Property Point(Key As String) As System.Windows.Point
End Class
<Serializable()>
Public Class Begin
    Inherits Draw

    Private _Close As Boolean = True
    Public Property Closed As System.Windows.Point
        Get
            Return _TargetPoint.AsVector + V(IIf(_Close, 40D, -40D))
        End Get
        Set(value As System.Windows.Point)
            _Close = (value - _TargetPoint).X > 0D
        End Set
    End Property
    Public Overrides Sub Draw(contextFigure As System.Windows.Media.PathFigure)
        contextFigure.StartPoint = _TargetPoint
        contextFigure.IsClosed = _Close
    End Sub

    Public Overrides Sub ReadPoints(dict As ShallowDictionary(Of String, System.Windows.Point))
        dict.Add(String.Format("D{0}T", Index), _TargetPoint)
        dict.Add(String.Format("D{0}C", Index), Closed)
    End Sub
    Public Overrides Property Point(Key As String) As System.Windows.Point
        Get
            Select Case Key
                Case "T"
                    Return _TargetPoint
                Case "C"
                    Return Closed
            End Select
        End Get
        Set(value As System.Windows.Point)
            Select Case Key
                Case "T"
                    _TargetPoint = value
                Case "C"
                    Closed = value
            End Select
        End Set
    End Property
End Class
<Serializable()>
Public Class Line
    Inherits Draw
    Public Overrides Sub Draw(contextFigure As System.Windows.Media.PathFigure)
        contextFigure.Segments.Add(New LineSegment(_TargetPoint, True))
    End Sub

    Public Overrides Sub ReadPoints(dict As ShallowDictionary(Of String, System.Windows.Point))
        dict.Add(String.Format("D{0}T", Index), _TargetPoint)
    End Sub

    Public Overrides Property Point(Key As String) As System.Windows.Point
        Get
            Select Case Key
                Case "T"
                    Return _TargetPoint

            End Select
        End Get
        Set(value As System.Windows.Point)
            Select Case Key
                Case "T"
                    _TargetPoint = value

            End Select
        End Set
    End Property
End Class
<Serializable()>
Public Class QuadraticBezier
    Inherits Draw

    Public Property Middle As System.Windows.Point
    Public Overrides Sub Draw(contextFigure As System.Windows.Media.PathFigure)
        contextFigure.Segments.Add(New QuadraticBezierSegment(_Middle, _TargetPoint, True))
    End Sub

    Public Overrides Sub ReadPoints(dict As ShallowDictionary(Of String, System.Windows.Point))
        dict.Add(String.Format("D{0}T", Index), _TargetPoint)
        dict.Add(String.Format("D{0}M", Index), _Middle)
    End Sub

    Public Overrides Property Point(Key As String) As System.Windows.Point
        Get
            Select Case Key
                Case "T"
                    Return _TargetPoint
                Case "M"
                    Return _Middle
            End Select
        End Get
        Set(value As System.Windows.Point)
            Select Case Key
                Case "T"
                    _TargetPoint = value
                Case "M"
                    _Middle = value
            End Select
        End Set
    End Property
End Class
<Serializable()>
Public Class Bezier
    Inherits Draw

    Public Property Left As System.Windows.Point
    Public Property Right As System.Windows.Point
    Public Overrides Sub Draw(contextFigure As System.Windows.Media.PathFigure)
        contextFigure.Segments.Add(New BezierSegment(_Left, _Right, _TargetPoint, True))
    End Sub

    Public Overrides Sub ReadPoints(dict As ShallowDictionary(Of String, System.Windows.Point))
        dict.Add(String.Format("D{0}T", Index), _TargetPoint)
        dict.Add(String.Format("D{0}L", Index), _Left)
        dict.Add(String.Format("D{0}R", Index), _Right)
    End Sub
    Public Overrides Property Point(Key As String) As System.Windows.Point
        Get
            Select Case Key
                Case "T"
                    Return _TargetPoint
                Case "L"
                    Return _Left
                Case "R"
                    Return _Right
            End Select
        End Get
        Set(value As System.Windows.Point)
            Select Case Key
                Case "T"
                    _TargetPoint = value
                Case "L"
                    _Left = value
                Case "R"
                    _Right = value
            End Select
        End Set
    End Property
End Class
<Serializable()>
Public Class Arc
    Inherits Draw
    Private _Radius As Size
    Public Property Radius As System.Windows.Point
        Get
            Return _TargetPoint + Radius.AsVector
        End Get
        Set(value As System.Windows.Point)
            _Radius = (value - _TargetPoint).AsSize
        End Set
    End Property
    Private _Angle As Double
    Private Shared V0 As New System.Windows.Vector(1D, 0D)
    Public Property Angle As System.Windows.Point
        Get
            Return _TargetPoint.AsVector + V(40, 0).RotateByDegree(_Angle)
        End Get
        Set(value As System.Windows.Point)
            _Angle = (value.AsVector - _TargetPoint.AsVector).AngleByDegree
        End Set
    End Property
    Public Overrides Sub Draw(contextFigure As System.Windows.Media.PathFigure)
        contextFigure.Segments.Add(New ArcSegment(_TargetPoint, _Radius, _Angle, _Angle < 0D, SweepDirection.Clockwise, True))
    End Sub
    Public Overrides Sub ReadPoints(dict As ShallowDictionary(Of String, System.Windows.Point))
        dict.Add(String.Format("D{0}T", Index), _TargetPoint)
        dict.Add(String.Format("D{0}E", Index), Radius)
        dict.Add(String.Format("D{0}A", Index), Angle)
    End Sub
    Public Overrides Property Point(Key As String) As System.Windows.Point
        Get
            Select Case Key
                Case "T"
                    Return _TargetPoint
                Case "E"
                    Return Radius
                Case "A"
                    Return Angle
            End Select
        End Get
        Set(value As System.Windows.Point)
            Select Case Key
                Case "T"
                    _TargetPoint = value
                Case "E"
                    Radius = value
                Case "R"
                    Angle = value
            End Select
        End Set
    End Property
End Class